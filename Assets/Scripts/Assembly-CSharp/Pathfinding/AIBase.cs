using System;
using Pathfinding.RVO;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[RequireComponent(typeof(Seeker))]
	public abstract class AIBase : VersionedMonoBehaviour
	{
		public float repathRate = 0.5f;

		[FormerlySerializedAs("repeatedlySearchPaths")]
		public bool canSearch = true;

		public bool canMove = true;

		[FormerlySerializedAs("speed")]
		public float maxSpeed = 1f;

		public Vector3 gravity = new Vector3(float.NaN, float.NaN, float.NaN);

		public LayerMask groundMask = -1;

		public float centerOffset = 1f;

		public bool rotationIn2D;

		protected Vector3 simulatedPosition;

		protected Quaternion simulatedRotation;

		private Vector3 accumulatedMovementDelta = Vector3.zero;

		protected Vector2 velocity2D;

		protected float verticalVelocity;

		public Seeker seeker;

		public Transform tr;

		protected Rigidbody rigid;

		protected Rigidbody2D rigid2D;

		protected CharacterController controller;

		protected RVOController rvoController;

		public IMovementPlane movementPlane = GraphTransform.identityTransform;

		[NonSerialized]
		public bool updatePosition = true;

		[NonSerialized]
		public bool updateRotation = true;

		protected float lastDeltaTime;

		protected int prevFrame;

		protected Vector3 prevPosition1;

		protected Vector3 prevPosition2;

		protected Vector2 lastDeltaPosition;

		protected bool waitingForPathCalculation;

		protected float lastRepath = float.NegativeInfinity;

		[FormerlySerializedAs("target")]
		[SerializeField]
		[HideInInspector]
		private Transform targetCompatibility;

		private bool startHasRun;

		protected static readonly Color GizmoColorRaycast = new Color(0.4627451f, 0.80784315f, 0.4392157f);

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

		protected bool usingGravity { get; private set; }

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

		public Vector3 destination { get; set; }

		public Vector3 velocity
		{
			get
			{
				return (!(lastDeltaTime > 1E-06f)) ? Vector3.zero : ((prevPosition1 - prevPosition2) / lastDeltaTime);
			}
		}

		public Vector3 desiredVelocity
		{
			get
			{
				return (!(lastDeltaTime > 1E-05f)) ? Vector3.zero : movementPlane.ToWorld(lastDeltaPosition / lastDeltaTime, verticalVelocity);
			}
		}

		public bool isStopped { get; set; }

		public Action onSearchPath { get; set; }

		protected virtual bool shouldRecalculatePath
		{
			get
			{
				return Time.time - lastRepath >= repathRate && !waitingForPathCalculation && canSearch && !float.IsPositiveInfinity(destination.x);
			}
		}

		protected AIBase()
		{
			destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		public virtual void FindComponents()
		{
			tr = base.transform;
			seeker = GetComponent<Seeker>();
			rvoController = GetComponent<RVOController>();
			controller = GetComponent<CharacterController>();
			rigid = GetComponent<Rigidbody>();
			rigid2D = GetComponent<Rigidbody2D>();
		}

		protected virtual void OnEnable()
		{
			FindComponents();
			Seeker obj = seeker;
			obj.pathCallback = (OnPathDelegate)Delegate.Combine(obj.pathCallback, new OnPathDelegate(OnPathComplete));
			Init();
		}

		protected virtual void Start()
		{
			startHasRun = true;
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

		public virtual void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			if (clearPath)
			{
				CancelCurrentPathRequest();
			}
			prevPosition1 = (prevPosition2 = (simulatedPosition = newPosition));
			if (updatePosition)
			{
				tr.position = newPosition;
			}
			if (rvoController != null)
			{
				rvoController.Move(Vector3.zero);
			}
			if (clearPath)
			{
				SearchPath();
			}
		}

		protected void CancelCurrentPathRequest()
		{
			waitingForPathCalculation = false;
			if (seeker != null)
			{
				seeker.CancelCurrentPathRequest();
			}
		}

		protected virtual void OnDisable()
		{
			CancelCurrentPathRequest();
			Seeker obj = seeker;
			obj.pathCallback = (OnPathDelegate)Delegate.Remove(obj.pathCallback, new OnPathDelegate(OnPathComplete));
			velocity2D = Vector3.zero;
			accumulatedMovementDelta = Vector3.zero;
			verticalVelocity = 0f;
			lastDeltaTime = 0f;
		}

		protected virtual void Update()
		{
			if (shouldRecalculatePath)
			{
				SearchPath();
			}
			usingGravity = !(gravity == Vector3.zero) && (!updatePosition || ((rigid == null || rigid.isKinematic) && (rigid2D == null || rigid2D.isKinematic)));
			if (rigid == null && rigid2D == null && canMove)
			{
				Vector3 nextPosition;
				Quaternion nextRotation;
				MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
				FinalizeMovement(nextPosition, nextRotation);
			}
		}

		protected virtual void FixedUpdate()
		{
			if ((!(rigid == null) || !(rigid2D == null)) && canMove)
			{
				Vector3 nextPosition;
				Quaternion nextRotation;
				MovementUpdate(Time.fixedDeltaTime, out nextPosition, out nextRotation);
				FinalizeMovement(nextPosition, nextRotation);
			}
		}

		public void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			lastDeltaTime = deltaTime;
			MovementUpdateInternal(deltaTime, out nextPosition, out nextRotation);
		}

		protected abstract void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation);

		protected virtual void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			start = GetFeetPosition();
			end = destination;
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
				waitingForPathCalculation = true;
				seeker.CancelCurrentPathRequest();
				Vector3 start;
				Vector3 end;
				CalculatePathRequestEndpoints(out start, out end);
				seeker.StartPath(start, end);
			}
		}

		public virtual Vector3 GetFeetPosition()
		{
			if (rvoController != null && rvoController.enabled && rvoController.movementPlane == MovementPlane.XZ)
			{
				return position + rotation * Vector3.up * (rvoController.center - rvoController.height * 0.5f);
			}
			if (controller != null && controller.enabled && updatePosition)
			{
				return tr.TransformPoint(controller.center) - Vector3.up * controller.height * 0.5f;
			}
			return position;
		}

		protected abstract void OnPathComplete(Path newPath);

		public void SetPath(Path path)
		{
			if (path.PipelineState == PathState.Created)
			{
				lastRepath = Time.time;
				waitingForPathCalculation = true;
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

		protected void ApplyGravity(float deltaTime)
		{
			if (usingGravity)
			{
				float elevation;
				velocity2D += movementPlane.ToPlane(deltaTime * ((!float.IsNaN(gravity.x)) ? gravity : Physics.gravity), out elevation);
				verticalVelocity += elevation;
			}
			else
			{
				verticalVelocity = 0f;
			}
		}

		protected Vector2 CalculateDeltaToMoveThisFrame(Vector2 position, float distanceToEndOfPath, float deltaTime)
		{
			if (rvoController != null && rvoController.enabled)
			{
				return movementPlane.ToPlane(rvoController.CalculateMovementDelta(movementPlane.ToWorld(position), deltaTime));
			}
			return Vector2.ClampMagnitude(velocity2D * deltaTime, distanceToEndOfPath);
		}

		public Quaternion SimulateRotationTowards(Vector3 direction, float maxDegrees)
		{
			return SimulateRotationTowards(movementPlane.ToPlane(direction), maxDegrees);
		}

		protected Quaternion SimulateRotationTowards(Vector2 direction, float maxDegrees)
		{
			if (direction != Vector2.zero)
			{
				Quaternion to = Quaternion.LookRotation(movementPlane.ToWorld(direction), movementPlane.ToWorld(Vector2.zero, 1f));
				if (rotationIn2D)
				{
					to *= Quaternion.Euler(90f, 0f, 0f);
				}
				return Quaternion.RotateTowards(simulatedRotation, to, maxDegrees);
			}
			return simulatedRotation;
		}

		public virtual void Move(Vector3 deltaPosition)
		{
			accumulatedMovementDelta += deltaPosition;
		}

		public virtual void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
		{
			FinalizeRotation(nextRotation);
			FinalizePosition(nextPosition);
		}

		private void FinalizeRotation(Quaternion nextRotation)
		{
			simulatedRotation = nextRotation;
			if (updateRotation)
			{
				if (rigid != null)
				{
					rigid.MoveRotation(nextRotation);
				}
				else if (rigid2D != null)
				{
					rigid2D.MoveRotation(nextRotation.eulerAngles.z);
				}
				else
				{
					tr.rotation = nextRotation;
				}
			}
		}

		private void FinalizePosition(Vector3 nextPosition)
		{
			Vector3 vector = simulatedPosition;
			bool flag = false;
			if (controller != null && controller.enabled && updatePosition)
			{
				tr.position = vector;
				controller.Move(nextPosition - vector + accumulatedMovementDelta);
				vector = tr.position;
				if (controller.isGrounded)
				{
					verticalVelocity = 0f;
				}
			}
			else
			{
				float elevation;
				movementPlane.ToPlane(vector, out elevation);
				vector = nextPosition + accumulatedMovementDelta;
				if (usingGravity)
				{
					vector = RaycastPosition(vector, elevation);
				}
				flag = true;
			}
			bool positionChanged = false;
			vector = ClampToNavmesh(vector, out positionChanged);
			if ((flag || positionChanged) && updatePosition)
			{
				if (rigid != null)
				{
					rigid.MovePosition(vector);
				}
				else if (rigid2D != null)
				{
					rigid2D.MovePosition(vector);
				}
				else
				{
					tr.position = vector;
				}
			}
			accumulatedMovementDelta = Vector3.zero;
			simulatedPosition = vector;
			UpdateVelocity();
		}

		protected void UpdateVelocity()
		{
			int frameCount = Time.frameCount;
			if (frameCount != prevFrame)
			{
				prevPosition2 = prevPosition1;
			}
			prevPosition1 = position;
			prevFrame = frameCount;
		}

		protected virtual Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			positionChanged = false;
			return position;
		}

		protected Vector3 RaycastPosition(Vector3 position, float lastElevation)
		{
			float elevation;
			movementPlane.ToPlane(position, out elevation);
			float num = centerOffset + Mathf.Max(0f, lastElevation - elevation);
			Vector3 vector = movementPlane.ToWorld(Vector2.zero, num);
			RaycastHit hitInfo;
			if (Physics.Raycast(position + vector, -vector, out hitInfo, num, groundMask, QueryTriggerInteraction.Ignore))
			{
				verticalVelocity *= Math.Max(0f, 1f - 5f * lastDeltaTime);
				return hitInfo.point;
			}
			return position;
		}

		protected virtual void OnDrawGizmosSelected()
		{
			if (Application.isPlaying)
			{
				FindComponents();
			}
		}

		protected virtual void OnDrawGizmos()
		{
			if (!Application.isPlaying || !base.enabled)
			{
				FindComponents();
			}
			if (!(gravity == Vector3.zero) && (!updatePosition || ((rigid == null || rigid.isKinematic) && (rigid2D == null || rigid2D.isKinematic))) && (controller == null || !controller.enabled))
			{
				Gizmos.color = GizmoColorRaycast;
				Gizmos.DrawLine(position, position + base.transform.up * centerOffset);
				Gizmos.DrawLine(position - base.transform.right * 0.1f, position + base.transform.right * 0.1f);
				Gizmos.DrawLine(position - base.transform.forward * 0.1f, position + base.transform.forward * 0.1f);
			}
			if (!float.IsPositiveInfinity(destination.x) && Application.isPlaying)
			{
				Draw.Gizmos.CircleXZ(destination, 0.2f, Color.blue);
			}
		}

		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (unityThread && targetCompatibility != null)
			{
				target = targetCompatibility;
			}
			return 1;
		}
	}
}
