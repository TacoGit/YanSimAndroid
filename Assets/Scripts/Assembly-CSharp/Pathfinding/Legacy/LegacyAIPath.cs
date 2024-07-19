using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Legacy
{
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/Legacy/AI/Legacy AIPath (3D)")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_legacy_1_1_legacy_a_i_path.php")]
	public class LegacyAIPath : AIPath
	{
		public float forwardLook = 1f;

		public bool closestOnPathCheck = true;

		protected float minMoveScale = 0.05f;

		protected int currentWaypointIndex;

		protected Vector3 lastFoundWaypointPosition;

		protected float lastFoundWaypointTime = -9999f;

		protected new Vector3 targetDirection;

		protected override void Awake()
		{
			base.Awake();
			if (rvoController != null)
			{
				if (rvoController is LegacyRVOController)
				{
					(rvoController as LegacyRVOController).enableRotation = false;
				}
				else
				{
					Debug.LogError("The LegacyAIPath component only works with the legacy RVOController, not the latest one. Please upgrade this component", this);
				}
			}
		}

		protected override void OnPathComplete(Path _p)
		{
			ABPath aBPath = _p as ABPath;
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
			currentWaypointIndex = 0;
			base.reachedEndOfPath = false;
			if (closestOnPathCheck)
			{
				Vector3 vector = ((!(Time.time - lastFoundWaypointTime < 0.3f)) ? aBPath.originalStartPoint : lastFoundWaypointPosition);
				Vector3 feetPosition = GetFeetPosition();
				Vector3 vector2 = feetPosition - vector;
				float magnitude = vector2.magnitude;
				vector2 /= magnitude;
				int num = (int)(magnitude / pickNextWaypointDist);
				for (int i = 0; i <= num; i++)
				{
					CalculateVelocity(vector);
					vector += vector2;
				}
			}
		}

		protected override void Update()
		{
			if (canMove)
			{
				Vector3 vector = CalculateVelocity(GetFeetPosition());
				RotateTowards(targetDirection);
				if (rvoController != null)
				{
					rvoController.Move(vector);
				}
				else if (controller != null)
				{
					controller.SimpleMove(vector);
				}
				else if (rigid != null)
				{
					rigid.AddForce(vector);
				}
				else
				{
					tr.Translate(vector * Time.deltaTime, Space.World);
				}
			}
		}

		protected float XZSqrMagnitude(Vector3 a, Vector3 b)
		{
			float num = b.x - a.x;
			float num2 = b.z - a.z;
			return num * num + num2 * num2;
		}

		protected new Vector3 CalculateVelocity(Vector3 currentPosition)
		{
			if (path == null || path.vectorPath == null || path.vectorPath.Count == 0)
			{
				return Vector3.zero;
			}
			List<Vector3> vectorPath = path.vectorPath;
			if (vectorPath.Count == 1)
			{
				vectorPath.Insert(0, currentPosition);
			}
			if (currentWaypointIndex >= vectorPath.Count)
			{
				currentWaypointIndex = vectorPath.Count - 1;
			}
			if (currentWaypointIndex <= 1)
			{
				currentWaypointIndex = 1;
			}
			while (currentWaypointIndex < vectorPath.Count - 1)
			{
				float num = XZSqrMagnitude(vectorPath[currentWaypointIndex], currentPosition);
				if (num < pickNextWaypointDist * pickNextWaypointDist)
				{
					lastFoundWaypointPosition = currentPosition;
					lastFoundWaypointTime = Time.time;
					currentWaypointIndex++;
					continue;
				}
				break;
			}
			Vector3 vector = vectorPath[currentWaypointIndex] - vectorPath[currentWaypointIndex - 1];
			Vector3 vector2 = CalculateTargetPoint(currentPosition, vectorPath[currentWaypointIndex - 1], vectorPath[currentWaypointIndex]);
			vector = vector2 - currentPosition;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float num2 = Mathf.Clamp01(magnitude / slowdownDistance);
			targetDirection = vector;
			if (currentWaypointIndex == vectorPath.Count - 1 && magnitude <= endReachedDistance)
			{
				if (!base.reachedEndOfPath)
				{
					base.reachedEndOfPath = true;
					OnTargetReached();
				}
				return Vector3.zero;
			}
			Vector3 forward = tr.forward;
			float a = Vector3.Dot(vector.normalized, forward);
			float num3 = maxSpeed * Mathf.Max(a, minMoveScale) * num2;
			if (Time.deltaTime > 0f)
			{
				num3 = Mathf.Clamp(num3, 0f, magnitude / (Time.deltaTime * 2f));
			}
			return forward * num3;
		}

		protected void RotateTowards(Vector3 dir)
		{
			if (!(dir == Vector3.zero))
			{
				Quaternion a = tr.rotation;
				Quaternion b = Quaternion.LookRotation(dir);
				Vector3 eulerAngles = Quaternion.Slerp(a, b, base.turningSpeed * Time.deltaTime).eulerAngles;
				eulerAngles.z = 0f;
				eulerAngles.x = 0f;
				a = Quaternion.Euler(eulerAngles);
				tr.rotation = a;
			}
		}

		protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
		{
			a.y = p.y;
			b.y = p.y;
			float magnitude = (a - b).magnitude;
			if (magnitude == 0f)
			{
				return a;
			}
			float num = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(a, b, p));
			Vector3 vector = (b - a) * num + a;
			float magnitude2 = (vector - p).magnitude;
			float num2 = Mathf.Clamp(forwardLook - magnitude2, 0f, forwardLook);
			float num3 = num2 / magnitude;
			num3 = Mathf.Clamp(num3 + num, 0f, 1f);
			return (b - a) * num3 + a;
		}
	}
}
