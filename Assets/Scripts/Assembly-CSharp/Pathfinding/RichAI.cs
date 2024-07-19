using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)")]
	public class RichAI : AIBase, IAstarAI
	{
		public float acceleration = 5f;

		public float rotationSpeed = 360f;

		public float slowdownTime = 0.5f;

		public float endReachedDistance = 0.01f;

		public float wallForce = 3f;

		public float wallDist = 1f;

		public bool funnelSimplification;

		public bool slowWhenNotFacingTarget = true;

		public Func<RichSpecial, IEnumerator> onTraverseOffMeshLink;

		protected readonly RichPath richPath = new RichPath();

		protected bool delayUpdatePath;

		protected bool lastCorner;

		protected float distanceToSteeringTarget = float.PositiveInfinity;

		protected readonly List<Vector3> nextCorners = new List<Vector3>();

		protected readonly List<Vector3> wallBuffer = new List<Vector3>();

		protected static readonly Color GizmoColorPath = new Color(0.03137255f, 26f / 85f, 0.7607843f);

		[FormerlySerializedAs("anim")]
		[SerializeField]
		[HideInInspector]
		private Animation animCompatibility;

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

		Vector3 IAstarAI.position
		{
			get
			{
				return tr.position;
			}
		}

		public bool traversingOffMeshLink { get; protected set; }

		public float remainingDistance
		{
			get
			{
				return distanceToSteeringTarget + Vector3.Distance(steeringTarget, richPath.Endpoint);
			}
		}

		public bool reachedEndOfPath
		{
			get
			{
				return approachingPathEndpoint && distanceToSteeringTarget < endReachedDistance;
			}
		}

		public bool hasPath
		{
			get
			{
				return richPath.GetCurrentPart() != null;
			}
		}

		public bool pathPending
		{
			get
			{
				return waitingForPathCalculation || delayUpdatePath;
			}
		}

		public Vector3 steeringTarget { get; protected set; }

		public bool approachingPartEndpoint
		{
			get
			{
				return lastCorner && nextCorners.Count == 1;
			}
		}

		public bool approachingPathEndpoint
		{
			get
			{
				return approachingPartEndpoint && richPath.IsLastPart;
			}
		}

		protected override bool shouldRecalculatePath
		{
			get
			{
				return base.shouldRecalculatePath && !traversingOffMeshLink;
			}
		}

		[Obsolete("Use velocity instead (lowercase 'v'). [AstarUpgradable: 'Velocity' -> 'velocity']")]
		public Vector3 Velocity
		{
			get
			{
				return base.velocity;
			}
		}

		[Obsolete("Use steeringTarget instead. [AstarUpgradable: 'NextWaypoint' -> 'steeringTarget']")]
		public Vector3 NextWaypoint
		{
			get
			{
				return steeringTarget;
			}
		}

		[Obsolete("Use Vector3.Distance(transform.position, ai.steeringTarget) instead.")]
		public float DistanceToNextWaypoint
		{
			get
			{
				return distanceToSteeringTarget;
			}
		}

		[Obsolete("Use canSearch instead. [AstarUpgradable: 'repeatedlySearchPaths' -> 'canSearch']")]
		public bool repeatedlySearchPaths
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

		[Obsolete("When unifying the interfaces for different movement scripts, this property has been renamed to reachedEndOfPath (lowercase t).  [AstarUpgradable: 'TargetReached' -> 'reachedEndOfPath']")]
		public bool TargetReached
		{
			get
			{
				return reachedEndOfPath;
			}
		}

		[Obsolete("Use pathPending instead (lowercase 'p'). [AstarUpgradable: 'PathPending' -> 'pathPending']")]
		public bool PathPending
		{
			get
			{
				return pathPending;
			}
		}

		[Obsolete("Use approachingPartEndpoint (lowercase 'a') instead")]
		public bool ApproachingPartEndpoint
		{
			get
			{
				return approachingPartEndpoint;
			}
		}

		[Obsolete("Use approachingPathEndpoint (lowercase 'a') instead")]
		public bool ApproachingPathEndpoint
		{
			get
			{
				return approachingPathEndpoint;
			}
		}

		[Obsolete("This property has been renamed to 'traversingOffMeshLink'. [AstarUpgradable: 'TraversingSpecial' -> 'traversingOffMeshLink']")]
		public bool TraversingSpecial
		{
			get
			{
				return traversingOffMeshLink;
			}
		}

		[Obsolete("This property has been renamed to steeringTarget")]
		public Vector3 TargetPoint
		{
			get
			{
				return steeringTarget;
			}
		}

		[Obsolete("Use the onTraverseOffMeshLink event or the ... component instead. Setting this value will add a ... component")]
		public Animation anim
		{
			get
			{
				AnimationLinkTraverser component = GetComponent<AnimationLinkTraverser>();
				return (!(component != null)) ? null : component.anim;
			}
			set
			{
				animCompatibility = null;
				AnimationLinkTraverser animationLinkTraverser = GetComponent<AnimationLinkTraverser>();
				if (animationLinkTraverser == null)
				{
					animationLinkTraverser = base.gameObject.AddComponent<AnimationLinkTraverser>();
				}
				animationLinkTraverser.anim = value;
			}
		}

		public override void Teleport(Vector3 newPosition, bool clearPath = true)
		{
			NNInfo nNInfo = ((!(AstarPath.active != null)) ? default(NNInfo) : AstarPath.active.GetNearest(newPosition));
			float elevation;
			movementPlane.ToPlane(newPosition, out elevation);
			newPosition = movementPlane.ToWorld(movementPlane.ToPlane((nNInfo.node == null) ? newPosition : nNInfo.position), elevation);
			if (clearPath)
			{
				richPath.Clear();
			}
			base.Teleport(newPosition, clearPath);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			lastCorner = false;
			distanceToSteeringTarget = float.PositiveInfinity;
			traversingOffMeshLink = false;
			delayUpdatePath = false;
			StopAllCoroutines();
		}

		public override void SearchPath()
		{
			if (traversingOffMeshLink)
			{
				delayUpdatePath = true;
			}
			else
			{
				base.SearchPath();
			}
		}

		protected override void OnPathComplete(Path p)
		{
			waitingForPathCalculation = false;
			p.Claim(this);
			if (p.error)
			{
				p.Release(this);
				return;
			}
			if (traversingOffMeshLink)
			{
				delayUpdatePath = true;
			}
			else
			{
				richPath.Initialize(seeker, p, true, funnelSimplification);
				RichFunnel richFunnel = richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					if (updatePosition)
					{
						simulatedPosition = tr.position;
					}
					Vector2 vector = movementPlane.ToPlane(UpdateTarget(richFunnel));
					if (lastCorner && nextCorners.Count == 1)
					{
						steeringTarget = nextCorners[0];
						Vector2 vector2 = movementPlane.ToPlane(steeringTarget);
						distanceToSteeringTarget = (vector2 - vector).magnitude;
						if (distanceToSteeringTarget <= endReachedDistance)
						{
							NextPart();
						}
					}
				}
			}
			p.Release(this);
		}

		protected void NextPart()
		{
			if (!richPath.CompletedAllParts)
			{
				if (!richPath.IsLastPart)
				{
					lastCorner = false;
				}
				richPath.NextPart();
				if (richPath.CompletedAllParts)
				{
					OnTargetReached();
				}
			}
		}

		protected virtual void OnTargetReached()
		{
		}

		protected virtual Vector3 UpdateTarget(RichFunnel fn)
		{
			nextCorners.Clear();
			bool requiresRepath;
			Vector3 result = fn.Update(simulatedPosition, nextCorners, 2, out lastCorner, out requiresRepath);
			if (requiresRepath && !waitingForPathCalculation && canSearch)
			{
				SearchPath();
			}
			return result;
		}

		protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			if (updatePosition)
			{
				simulatedPosition = tr.position;
			}
			if (updateRotation)
			{
				simulatedRotation = tr.rotation;
			}
			RichPathPart currentPart = richPath.GetCurrentPart();
			if (currentPart is RichSpecial)
			{
				if (!traversingOffMeshLink)
				{
					StartCoroutine(TraverseSpecial(currentPart as RichSpecial));
				}
				Vector3 vector2 = (steeringTarget = simulatedPosition);
				nextPosition = vector2;
				nextRotation = base.rotation;
			}
			else
			{
				RichFunnel richFunnel = currentPart as RichFunnel;
				if (richFunnel != null && !base.isStopped)
				{
					TraverseFunnel(richFunnel, deltaTime, out nextPosition, out nextRotation);
					return;
				}
				velocity2D -= Vector2.ClampMagnitude(velocity2D, acceleration * deltaTime);
				FinalMovement(simulatedPosition, deltaTime, float.PositiveInfinity, 1f, out nextPosition, out nextRotation);
				steeringTarget = simulatedPosition;
			}
		}

		private void TraverseFunnel(RichFunnel fn, float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector3 vector = UpdateTarget(fn);
			float elevation;
			Vector2 vector2 = movementPlane.ToPlane(vector, out elevation);
			if (Time.frameCount % 5 == 0 && wallForce > 0f && wallDist > 0f)
			{
				wallBuffer.Clear();
				fn.FindWalls(wallBuffer, wallDist);
			}
			steeringTarget = nextCorners[0];
			Vector2 vector3 = movementPlane.ToPlane(steeringTarget);
			Vector2 vector4 = vector3 - vector2;
			Vector2 vector5 = VectorMath.Normalize(vector4, out distanceToSteeringTarget);
			Vector2 vector6 = CalculateWallForce(vector2, elevation, vector5);
			Vector2 targetVelocity;
			if (approachingPartEndpoint)
			{
				targetVelocity = ((!(slowdownTime > 0f)) ? (vector5 * maxSpeed) : Vector2.zero);
				vector6 *= Math.Min(distanceToSteeringTarget / 0.5f, 1f);
				if (distanceToSteeringTarget <= endReachedDistance)
				{
					NextPart();
				}
			}
			else
			{
				Vector2 vector7 = ((nextCorners.Count <= 1) ? (vector2 + 2f * vector4) : movementPlane.ToPlane(nextCorners[1]));
				targetVelocity = (vector7 - vector3).normalized * maxSpeed;
			}
			Vector2 forwardsVector = movementPlane.ToPlane(simulatedRotation * ((!rotationIn2D) ? Vector3.forward : Vector3.up));
			Vector2 vector8 = MovementUtilities.CalculateAccelerationToReachPoint(vector3 - vector2, targetVelocity, velocity2D, acceleration, rotationSpeed, maxSpeed, forwardsVector);
			velocity2D += (vector8 + vector6 * wallForce) * deltaTime;
			float num = distanceToSteeringTarget + Vector3.Distance(steeringTarget, fn.exactEnd);
			float slowdownFactor = ((!(num < maxSpeed * slowdownTime)) ? 1f : Mathf.Sqrt(num / (maxSpeed * slowdownTime)));
			FinalMovement(vector, deltaTime, num, slowdownFactor, out nextPosition, out nextRotation);
		}

		private void FinalMovement(Vector3 position3D, float deltaTime, float distanceToEndOfPath, float slowdownFactor, out Vector3 nextPosition, out Quaternion nextRotation)
		{
			Vector2 forward = movementPlane.ToPlane(simulatedRotation * ((!rotationIn2D) ? Vector3.forward : Vector3.up));
			velocity2D = MovementUtilities.ClampVelocity(velocity2D, maxSpeed, slowdownFactor, slowWhenNotFacingTarget, forward);
			ApplyGravity(deltaTime);
			if (rvoController != null && rvoController.enabled)
			{
				Vector3 pos = position3D + movementPlane.ToWorld(Vector2.ClampMagnitude(velocity2D, distanceToEndOfPath));
				rvoController.SetTarget(pos, velocity2D.magnitude, maxSpeed);
			}
			Vector2 vector = (lastDeltaPosition = CalculateDeltaToMoveThisFrame(movementPlane.ToPlane(position3D), distanceToEndOfPath, deltaTime));
			float num = ((!approachingPartEndpoint) ? 1f : Mathf.Clamp01(1.1f * slowdownFactor - 0.1f));
			nextRotation = SimulateRotationTowards(vector, rotationSpeed * num * deltaTime);
			nextPosition = position3D + movementPlane.ToWorld(vector, verticalVelocity * deltaTime);
		}

		protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
		{
			if (richPath != null)
			{
				RichFunnel richFunnel = richPath.GetCurrentPart() as RichFunnel;
				if (richFunnel != null)
				{
					Vector3 vector = richFunnel.ClampToNavmesh(position);
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
			}
			positionChanged = false;
			return position;
		}

		private Vector2 CalculateWallForce(Vector2 position, float elevation, Vector2 directionToTarget)
		{
			if (wallForce <= 0f || wallDist <= 0f)
			{
				return Vector2.zero;
			}
			float num = 0f;
			float num2 = 0f;
			Vector3 vector = movementPlane.ToWorld(position, elevation);
			for (int i = 0; i < wallBuffer.Count; i += 2)
			{
				Vector3 vector2 = VectorMath.ClosestPointOnSegment(wallBuffer[i], wallBuffer[i + 1], vector);
				float sqrMagnitude = (vector2 - vector).sqrMagnitude;
				if (!(sqrMagnitude > wallDist * wallDist))
				{
					Vector2 normalized = movementPlane.ToPlane(wallBuffer[i + 1] - wallBuffer[i]).normalized;
					float num3 = Vector2.Dot(directionToTarget, normalized);
					float num4 = 1f - Math.Max(0f, 2f * (sqrMagnitude / (wallDist * wallDist)) - 1f);
					if (num3 > 0f)
					{
						num2 = Math.Max(num2, num3 * num4);
					}
					else
					{
						num = Math.Max(num, (0f - num3) * num4);
					}
				}
			}
			Vector2 vector3 = new Vector2(directionToTarget.y, 0f - directionToTarget.x);
			return vector3 * (num2 - num);
		}

		protected virtual IEnumerator TraverseSpecial(RichSpecial link)
		{
			traversingOffMeshLink = true;
			velocity2D = Vector3.zero;
			IEnumerator offMeshLinkCoroutine = ((onTraverseOffMeshLink == null) ? TraverseOffMeshLinkFallback(link) : onTraverseOffMeshLink(link));
			yield return StartCoroutine(offMeshLinkCoroutine);
			traversingOffMeshLink = false;
			NextPart();
			if (delayUpdatePath)
			{
				delayUpdatePath = false;
				if (canSearch)
				{
					SearchPath();
				}
			}
		}

		protected IEnumerator TraverseOffMeshLinkFallback(RichSpecial link)
		{
			float duration = ((!(maxSpeed > 0f)) ? 1f : (Vector3.Distance(link.second.position, link.first.position) / maxSpeed));
			float startTime = Time.time;
			while (true)
			{
				Vector3 pos = Vector3.Lerp(link.first.position, link.second.position, Mathf.InverseLerp(startTime, startTime + duration, Time.time));
				if (updatePosition)
				{
					tr.position = pos;
				}
				else
				{
					simulatedPosition = pos;
				}
				if (Time.time >= startTime + duration)
				{
					break;
				}
				yield return null;
			}
		}

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (tr != null)
			{
				Gizmos.color = GizmoColorPath;
				Vector3 from = base.position;
				for (int i = 0; i < nextCorners.Count; i++)
				{
					Gizmos.DrawLine(from, nextCorners[i]);
					from = nextCorners[i];
				}
			}
		}

		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (unityThread && animCompatibility != null)
			{
				anim = animCompatibility;
			}
			return base.OnUpgradeSerializedData(version, unityThread);
		}

		[Obsolete("Use SearchPath instead. [AstarUpgradable: 'UpdatePath' -> 'SearchPath']")]
		public void UpdatePath()
		{
			SearchPath();
		}
	}
}
