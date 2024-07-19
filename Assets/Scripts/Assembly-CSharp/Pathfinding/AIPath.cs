using System;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/AI/AIPath (2D,3D)")]
	public class AIPath : AIBase, IAstarAI
	{
		public float maxAcceleration = -2.5f;

		[FormerlySerializedAs("turningSpeed")]
		public float rotationSpeed = 360f;

		public float slowdownDistance = 0.6f;

		public float pickNextWaypointDist = 2f;

		public float endReachedDistance = 0.2f;

		public bool alwaysDrawGizmos;

		public bool slowWhenNotFacingTarget = true;

		public CloseToDestinationMode whenCloseToDestination;

		public bool constrainInsideGraph;

		protected Path path;

		public PathInterpolator interpolator = new PathInterpolator();

		private static NNConstraint cachedNNConstraint = NNConstraint.Default;

		float IAstarAI.maxSpeed
		{
			get
			{
				return maxSpeed;
			}
			set
			{
				maxSpeed = value;
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

		public float remainingDistance
		{
			get
			{
				return (!interpolator.valid) ? float.PositiveInfinity : (interpolator.remainingDistance + movementPlane.ToPlane(interpolator.position - base.position).magnitude);
			}
		}

		public bool reachedEndOfPath { get; protected set; }

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
				return waitingForPathCalculation;
			}
		}

		public Vector3 steeringTarget
		{
			get
			{
				return (!interpolator.valid) ? base.position : interpolator.position;
			}
		}

		[Obsolete("When unifying the interfaces for different movement scripts, this property has been renamed to reachedEndOfPath.  [AstarUpgradable: 'TargetReached' -> 'reachedEndOfPath']")]
		public bool TargetReached
		{
			get
			{
				return reachedEndOfPath;
			}
		}

		[Obsolete("This field has been renamed to #rotationSpeed and is now in degrees per second instead of a damping factor")]
		public float turningSpeed
		{
			get
			{
				return rotationSpeed / 90f;
			}
			set
			{
				rotationSpeed = value * 90f;
			}
		}

		[Obsolete("This member has been deprecated. Use 'maxSpeed' instead. [AstarUpgradable: 'speed' -> 'maxSpeed']")]
		public float speed
		{
			get
			{
				return maxSpeed;
			}
			set
			{
				maxSpeed = value;
			}
		}

		[Obsolete("Only exists for compatibility reasons. Use desiredVelocity or steeringTarget instead.")]
		public Vector3 targetDirection
		{
			get
			{
				return (steeringTarget - tr.position).normalized;
			}
		}

		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			if (clearPath)
			{
				interpolator.SetPath(null);
			}
			reachedEndOfPath = false;
			base.Teleport(newPosition, clearPath);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (path != null)
			{
				path.Release(this);
			}
			path = null;
			interpolator.SetPath(null);
		}

		public virtual void OnTargetReached()
		{
		}

		protected override void OnPathComplete(Path newPath)
		{
			ABPath aBPath = newPath as ABPath;
			if (aBPath == null)
			{
				throw new Exception("This function only handles ABPaths, do not use special path types");
			}
			waitingForPathCalculation = false;
			aBPath.Claim(this);
			if (aBPath.error)
			{
				aBPath.Release(this);
				return;
			}
			if (path != null)
			{
				path.Release(this);
			}
			path = aBPath;
			if (path.vectorPath.Count == 1)
			{
				path.vectorPath.Add(path.vectorPath[0]);
			}
			interpolator.SetPath(path.vectorPath);
			ITransformedGraph transformedGraph = AstarData.GetGraph(path.path[0]) as ITransformedGraph;
			movementPlane = ((transformedGraph != null) ? transformedGraph.transform : ((!rotationIn2D) ? GraphTransform.identityTransform : new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 270f, 90f), Vector3.one))));
			reachedEndOfPath = false;
			interpolator.MoveToLocallyClosestPoint((GetFeetPosition() + aBPath.originalStartPoint) * 0.5f);
			interpolator.MoveToLocallyClosestPoint(GetFeetPosition());
			interpolator.MoveToCircleIntersection2D(base.position, pickNextWaypointDist, movementPlane);
			float num = remainingDistance;
			if (num <= endReachedDistance)
			{
				reachedEndOfPath = true;
				OnTargetReached();
			}
		}

		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			float num = maxAcceleration;
			if (num < 0f)
			{
				num *= 0f - maxSpeed;
			}
			if (updatePosition)
			{
				simulatedPosition = tr.position;
			}
			if (updateRotation)
			{
				simulatedRotation = tr.rotation;
			}
			Vector3 vector = simulatedPosition;
			interpolator.MoveToCircleIntersection2D(vector, pickNextWaypointDist, movementPlane);
			Vector2 deltaPosition = movementPlane.ToPlane(steeringTarget - vector);
			float num2 = deltaPosition.magnitude + Mathf.Max(0f, interpolator.remainingDistance);
			bool flag = reachedEndOfPath;
			reachedEndOfPath = num2 <= endReachedDistance && interpolator.valid;
			if (!flag && reachedEndOfPath)
			{
				OnTargetReached();
			}
			Vector2 vector2 = movementPlane.ToPlane(simulatedRotation * ((!rotationIn2D) ? Vector3.forward : Vector3.up));
			float num3;
			if (interpolator.valid && !base.isStopped)
			{
				num3 = ((!(num2 < slowdownDistance)) ? 1f : Mathf.Sqrt(num2 / slowdownDistance));
				if (reachedEndOfPath && whenCloseToDestination == CloseToDestinationMode.Stop)
				{
					velocity2D -= Vector2.ClampMagnitude(velocity2D, num * deltaTime);
				}
				else
				{
					velocity2D += MovementUtilities.CalculateAccelerationToReachPoint(deltaPosition, deltaPosition.normalized * maxSpeed, velocity2D, num, rotationSpeed, maxSpeed, vector2) * deltaTime;
				}
			}
			else
			{
				num3 = 1f;
				velocity2D -= Vector2.ClampMagnitude(velocity2D, num * deltaTime);
			}
			velocity2D = MovementUtilities.ClampVelocity(velocity2D, maxSpeed, num3, slowWhenNotFacingTarget, vector2);
			ApplyGravity(deltaTime);
			if (rvoController != null && rvoController.enabled)
			{
				Vector3 pos = vector + movementPlane.ToWorld(Vector2.ClampMagnitude(velocity2D, num2));
				rvoController.SetTarget(pos, velocity2D.magnitude, maxSpeed);
			}
			Vector2 p = (lastDeltaPosition = CalculateDeltaToMoveThisFrame(movementPlane.ToPlane(vector), num2, deltaTime));
			nextPosition = vector + movementPlane.ToWorld(p, verticalVelocity * lastDeltaTime);
			CalculateNextRotation(num3, out nextRotation);
		}

		protected virtual void CalculateNextRotation(float slowdown, out Quaternion nextRotation)
		{
			if (lastDeltaTime > 1E-05f)
			{
				Vector2 direction;
				if (rvoController != null && rvoController.enabled)
				{
					Vector2 b = lastDeltaPosition / lastDeltaTime;
					direction = Vector2.Lerp(velocity2D, b, 4f * b.magnitude / (maxSpeed + 0.0001f));
				}
				else
				{
					direction = velocity2D;
				}
				float num = rotationSpeed * Mathf.Max(0f, (slowdown - 0.3f) / 0.7f);
				nextRotation = SimulateRotationTowards(direction, num * lastDeltaTime);
			}
			else
			{
				nextRotation = base.rotation;
			}
		}

		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (constrainInsideGraph)
			{
				cachedNNConstraint.tags = seeker.traversableTags;
				cachedNNConstraint.graphMask = seeker.graphMask;
				cachedNNConstraint.distanceXZ = true;
				Vector3 vector = AstarPath.active.GetNearest(position, cachedNNConstraint).position;
				Vector2 vector2 = movementPlane.ToPlane(vector - position);
				float sqrMagnitude = vector2.sqrMagnitude;
				if (sqrMagnitude > 1.0000001E-06f)
				{
					velocity2D -= vector2 * Vector2.Dot(vector2, velocity2D) / sqrMagnitude;
					if (rvoController != null && rvoController.enabled)
					{
						rvoController.SetCollisionNormal(vector2);
					}
					positionChanged = true;
					return position + movementPlane.ToWorld(vector2);
				}
			}
			positionChanged = false;
			return position;
		}

		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			base.OnUpgradeSerializedData(version, unityThread);
			if (version < 1)
			{
				rotationSpeed *= 90f;
			}
			return 2;
		}

		[Obsolete("This method no longer calculates the velocity. Use the desiredVelocity property instead")]
		public Vector3 CalculateVelocity(Vector3 position)
		{
			return base.desiredVelocity;
		}
	}
}
