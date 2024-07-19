using System;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/AI/AILerp (2D,3D)")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_lerp.php")]
	public class AILerp : VersionedMonoBehaviour, IAstarAI
	{
		public float repathRate = 0.5f;

		public bool canSearch = true;

		public bool canMove = true;

		public float speed = 3f;

		public bool enableRotation = true;

		public bool rotationIn2D;

		public float rotationSpeed = 10f;

		public bool interpolatePathSwitches = true;

		public float switchPathInterpolationSpeed = 5f;

		[NonSerialized]
		public bool updatePosition = true;

		[NonSerialized]
		public bool updateRotation = true;

		protected Seeker seeker;

		protected Transform tr;

		protected float lastRepath = -9999f;

		protected ABPath path;

		protected bool canSearchAgain = true;

		protected Vector3 previousMovementOrigin;

		protected Vector3 previousMovementDirection;

		protected float pathSwitchInterpolationTime;

		protected PathInterpolator interpolator = new PathInterpolator();

		private bool startHasRun;

		private Vector3 previousPosition1;

		private Vector3 previousPosition2;

		private Vector3 simulatedPosition;

		private Quaternion simulatedRotation;

		[FormerlySerializedAs("target")]
		[SerializeField]
		[HideInInspector]
		private Transform targetCompatibility;

		float IAstarAI.maxSpeed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		bool IAstarAI.canSearch
		{
			get
			{
				return canSearch;
			}
			set
			{
				canSearch = value;
			}
		}

		bool IAstarAI.canMove
		{
			get
			{
				return canMove;
			}
			set
			{
				canMove = value;
			}
		}

		Vector3 IAstarAI.velocity
		{
			get
			{
				return (!(Time.deltaTime > 1E-05f)) ? Vector3.zero : ((previousPosition1 - previousPosition2) / Time.deltaTime);
			}
		}

		Vector3 IAstarAI.desiredVelocity
		{
			get
			{
				return ((IAstarAI)this).velocity;
			}
		}

		Vector3 IAstarAI.steeringTarget
		{
			get
			{
				return (!interpolator.valid) ? simulatedPosition : (interpolator.position + interpolator.tangent);
			}
		}

		public bool reachedEndOfPath { get; private set; }

		public Vector3 destination { get; set; }

		[Obsolete("Use the destination property or the AIDestinationSetter component instead")]
		public Transform target
		{
			get
			{
				AIDestinationSetter component = GetComponent<AIDestinationSetter>();
				return (!(component != null)) ? null : component.target;
			}
			set
			{
				targetCompatibility = null;
				AIDestinationSetter aIDestinationSetter = GetComponent<AIDestinationSetter>();
				if (aIDestinationSetter == null)
				{
					aIDestinationSetter = base.gameObject.AddComponent<AIDestinationSetter>();
				}
				aIDestinationSetter.target = value;
				destination = ((!(value != null)) ? new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity) : value.position);
			}
		}

		public Vector3 position
		{
			get
			{
				return (!updatePosition) ? simulatedPosition : tr.position;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return (!updateRotation) ? simulatedRotation : tr.rotation;
			}
		}

		public float remainingDistance
		{
			get
			{
				return Mathf.Max(interpolator.remainingDistance, 0f);
			}
			set
			{
				interpolator.remainingDistance = Mathf.Max(value, 0f);
			}
		}

		public bool hasPath
		{
			get
			{
				return interpolator.valid;
			}
		}

		public bool pathPending
		{
			get
			{
				return !canSearchAgain;
			}
		}

		public bool isStopped { get; set; }

		public Action onSearchPath { get; set; }

		protected virtual bool shouldRecalculatePath
		{
			get
			{
				return Time.time - lastRepath >= repathRate && canSearchAgain && canSearch && !float.IsPositiveInfinity(destination.x);
			}
		}

		protected AILerp()
		{
			destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		void IAstarAI.Move(Vector3 deltaPosition)
		{
		}

		protected override void Awake()
		{
			base.Awake();
			tr = base.transform;
			seeker = GetComponent<Seeker>();
			seeker.startEndModifier.adjustStartPoint = () => simulatedPosition;
		}

		protected virtual void Start()
		{
			startHasRun = true;
			Init();
		}

		protected virtual void OnEnable()
		{
			Seeker obj = seeker;
			obj.pathCallback = (OnPathDelegate)Delegate.Combine(obj.pathCallback, new OnPathDelegate(OnPathComplete));
			Init();
		}

		private void Init()
		{
			if (startHasRun)
			{
				Teleport(position, false);
				lastRepath = float.NegativeInfinity;
				if (shouldRecalculatePath)
				{
					SearchPath();
				}
			}
		}

		public void OnDisable()
		{
			if (seeker != null)
			{
				seeker.CancelCurrentPathRequest();
			}
			canSearchAgain = true;
			if (path != null)
			{
				path.Release(this);
			}
			path = null;
			interpolator.SetPath(null);
			Seeker obj = seeker;
			obj.pathCallback = (OnPathDelegate)Delegate.Remove(obj.pathCallback, new OnPathDelegate(OnPathComplete));
		}

		public void Teleport(Vector3 position, bool clearPath = true)
		{
			if (clearPath)
			{
				interpolator.SetPath(null);
			}
			simulatedPosition = (previousPosition1 = (previousPosition2 = position));
			if (updatePosition)
			{
				tr.position = position;
			}
			reachedEndOfPath = false;
			if (clearPath)
			{
				SearchPath();
			}
		}

		[Obsolete("Use SearchPath instead")]
		public virtual void ForceSearchPath()
		{
			SearchPath();
		}

		public virtual void SearchPath()
		{
			if (!float.IsPositiveInfinity(destination.x))
			{
				if (onSearchPath != null)
				{
					onSearchPath();
				}
				lastRepath = Time.time;
				Vector3 feetPosition = GetFeetPosition();
				canSearchAgain = false;
				seeker.StartPath(feetPosition, destination);
			}
		}

		public virtual void OnTargetReached()
		{
		}

		protected virtual void OnPathComplete(Path _p)
		{
			ABPath aBPath = _p as ABPath;
			if (aBPath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			canSearchAgain = true;
			aBPath.Claim(this);
			if (aBPath.error)
			{
				aBPath.Release(this);
				return;
			}
			if (interpolatePathSwitches)
			{
				ConfigurePathSwitchInterpolation();
			}
			ABPath aBPath2 = path;
			path = aBPath;
			reachedEndOfPath = false;
			if (path.vectorPath != null && path.vectorPath.Count == 1)
			{
				path.vectorPath.Insert(0, GetFeetPosition());
			}
			ConfigureNewPath();
			if (aBPath2 != null)
			{
				aBPath2.Release(this);
			}
			if (interpolator.remainingDistance < 0.0001f && !reachedEndOfPath)
			{
				reachedEndOfPath = true;
				OnTargetReached();
			}
		}

		public void SetPath(Path path)
		{
			if (path.PipelineState == PathState.Created)
			{
				lastRepath = Time.time;
				canSearchAgain = false;
				seeker.CancelCurrentPathRequest();
				seeker.StartPath(path);
				return;
			}
			if (path.PipelineState == PathState.Returned)
			{
				if (seeker.GetCurrentPath() != path)
				{
					seeker.CancelCurrentPathRequest();
					OnPathComplete(path);
					return;
				}
				throw new ArgumentException("If you calculate the path using seeker.StartPath then this script will pick up the calculated path anyway as it listens for all paths the Seeker finishes calculating. You should not call SetPath in that case.");
			}
			throw new ArgumentException("You must call the SetPath method with a path that either has been completely calculated or one whose path calculation has not been started at all. It looks like the path calculation for the path you tried to use has been started, but is not yet finished.");
		}

		protected virtual void ConfigurePathSwitchInterpolation()
		{
			bool flag = interpolator.valid && interpolator.remainingDistance < 0.0001f;
			if (interpolator.valid && !flag)
			{
				previousMovementOrigin = interpolator.position;
				previousMovementDirection = interpolator.tangent.normalized * interpolator.remainingDistance;
				pathSwitchInterpolationTime = 0f;
			}
			else
			{
				previousMovementOrigin = Vector3.zero;
				previousMovementDirection = Vector3.zero;
				pathSwitchInterpolationTime = float.PositiveInfinity;
			}
		}

		public virtual Vector3 GetFeetPosition()
		{
			return position;
		}

		protected virtual void ConfigureNewPath()
		{
			bool valid = interpolator.valid;
			Vector3 vector = ((!valid) ? Vector3.zero : interpolator.tangent);
			interpolator.SetPath(path.vectorPath);
			interpolator.MoveToClosestPoint(GetFeetPosition());
			if (interpolatePathSwitches && switchPathInterpolationSpeed > 0.01f && valid)
			{
				float num = Mathf.Max(0f - Vector3.Dot(vector.normalized, interpolator.tangent.normalized), 0f);
				interpolator.distance -= speed * num * (1f / switchPathInterpolationSpeed);
			}
		}

		protected virtual void Update()
		{
			if (shouldRecalculatePath)
			{
				SearchPath();
			}
			if (canMove)
			{
				Vector3 nextPosition;
				Quaternion nextRotation;
				MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
				FinalizeMovement(nextPosition, nextRotation);
			}
		}

		public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (updatePosition)
			{
				simulatedPosition = tr.position;
			}
			if (updateRotation)
			{
				simulatedRotation = tr.rotation;
			}
			Vector3 direction;
			nextPosition = CalculateNextPosition(out direction, (!isStopped) ? deltaTime : 0f);
			if (enableRotation)
			{
				nextRotation = SimulateRotationTowards(direction, deltaTime);
			}
			else
			{
				nextRotation = simulatedRotation;
			}
		}

		public void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			previousPosition2 = previousPosition1;
			previousPosition1 = (simulatedPosition = nextPosition);
			simulatedRotation = nextRotation;
			if (updatePosition)
			{
				tr.position = nextPosition;
			}
			if (updateRotation)
			{
				tr.rotation = nextRotation;
			}
		}

		private Quaternion SimulateRotationTowards(Vector3 direction, float deltaTime)
		{
			if (direction != Vector3.zero)
			{
				Quaternion b = Quaternion.LookRotation(direction, (!rotationIn2D) ? Vector3.up : Vector3.back);
				if (rotationIn2D)
				{
					b *= Quaternion.Euler(90f, 0f, 0f);
				}
				return Quaternion.Slerp(simulatedRotation, b, deltaTime * rotationSpeed);
			}
			return simulatedRotation;
		}

		protected virtual Vector3 CalculateNextPosition(out Vector3 direction, float deltaTime)
		{
			if (!interpolator.valid)
			{
				direction = Vector3.zero;
				return simulatedPosition;
			}
			interpolator.distance += deltaTime * speed;
			if (interpolator.remainingDistance < 0.0001f && !reachedEndOfPath)
			{
				reachedEndOfPath = true;
				OnTargetReached();
			}
			direction = interpolator.tangent;
			pathSwitchInterpolationTime += deltaTime;
			float num = switchPathInterpolationSpeed * pathSwitchInterpolationTime;
			if (interpolatePathSwitches && num < 1f)
			{
				Vector3 a = previousMovementOrigin + Vector3.ClampMagnitude(previousMovementDirection, speed * pathSwitchInterpolationTime);
				return Vector3.Lerp(a, interpolator.position, num);
			}
			return interpolator.position;
		}

		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (unityThread && targetCompatibility != null)
			{
				target = targetCompatibility;
			}
			return 2;
		}
	}
}
