using System;
using UnityEngine;

namespace Pathfinding.Legacy
{
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/Legacy/AI/Legacy RichAI (3D, for navmesh)")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_legacy_1_1_legacy_rich_a_i.php")]
	public class LegacyRichAI : RichAI
	{
		public bool preciseSlowdown = true;

		public bool raycastingForGroundPlacement;

		private new Vector3 velocity;

		private Vector3 lastTargetPoint;

		private Vector3 currentTargetDirection;

		private static float deltaTime;

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
					Debug.LogError("The LegacyRichAI component only works with the legacy RVOController, not the latest one. Please upgrade this component", this);
				}
			}
		}

		protected override void Update()
		{
			deltaTime = Mathf.Min(Time.smoothDeltaTime * 2f, Time.deltaTime);
			if (richPath != null)
			{
				RichPathPart currentPart = richPath.GetCurrentPart();
				RichFunnel richFunnel = currentPart as RichFunnel;
				if (richFunnel != null)
				{
					Vector3 vector = UpdateTarget(richFunnel);
					if (Time.frameCount % 5 == 0 && wallForce > 0f && wallDist > 0f)
					{
						wallBuffer.Clear();
						richFunnel.FindWalls(wallBuffer, wallDist);
					}
					int num = 0;
					Vector3 vector2 = nextCorners[num];
					Vector3 lhs = vector2 - vector;
					lhs.y = 0f;
					if (Vector3.Dot(lhs, currentTargetDirection) < 0f && nextCorners.Count - num > 1)
					{
						num++;
						vector2 = nextCorners[num];
					}
					if (vector2 != lastTargetPoint)
					{
						currentTargetDirection = vector2 - vector;
						currentTargetDirection.y = 0f;
						currentTargetDirection.Normalize();
						lastTargetPoint = vector2;
					}
					lhs = vector2 - vector;
					lhs.y = 0f;
					float num2 = (distanceToSteeringTarget = lhs.magnitude);
					lhs = ((num2 != 0f) ? (lhs / num2) : Vector3.zero);
					Vector3 lhs2 = lhs;
					Vector3 vector3 = Vector3.zero;
					if (wallForce > 0f && wallDist > 0f)
					{
						float num3 = 0f;
						float num4 = 0f;
						for (int i = 0; i < wallBuffer.Count; i += 2)
						{
							Vector3 vector4 = VectorMath.ClosestPointOnSegment(wallBuffer[i], wallBuffer[i + 1], tr.position);
							float sqrMagnitude = (vector4 - vector).sqrMagnitude;
							if (!(sqrMagnitude > wallDist * wallDist))
							{
								Vector3 normalized = (wallBuffer[i + 1] - wallBuffer[i]).normalized;
								float num5 = Vector3.Dot(lhs, normalized) * (1f - Math.Max(0f, 2f * (sqrMagnitude / (wallDist * wallDist)) - 1f));
								if (num5 > 0f)
								{
									num4 = Math.Max(num4, num5);
								}
								else
								{
									num3 = Math.Max(num3, 0f - num5);
								}
							}
						}
						Vector3 vector5 = Vector3.Cross(Vector3.up, lhs);
						vector3 = vector5 * (num4 - num3);
					}
					bool flag = lastCorner && nextCorners.Count - num == 1;
					if (flag)
					{
						if (slowdownTime < 0.001f)
						{
							slowdownTime = 0.001f;
						}
						Vector3 vector6 = vector2 - vector;
						vector6.y = 0f;
						lhs = ((!preciseSlowdown) ? (2f * (vector6 - slowdownTime * velocity) / (slowdownTime * slowdownTime)) : ((6f * vector6 - 4f * slowdownTime * velocity) / (slowdownTime * slowdownTime)));
						lhs = Vector3.ClampMagnitude(lhs, acceleration);
						vector3 *= Math.Min(num2 / 0.5f, 1f);
						if (num2 < endReachedDistance)
						{
							NextPart();
						}
					}
					else
					{
						lhs *= acceleration;
					}
					velocity += (lhs + vector3 * wallForce) * deltaTime;
					if (slowWhenNotFacingTarget)
					{
						float a = (Vector3.Dot(lhs2, tr.forward) + 0.5f) * (2f / 3f);
						float a2 = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
						float y = velocity.y;
						velocity.y = 0f;
						float num6 = Mathf.Min(a2, maxSpeed * Mathf.Max(a, 0.2f));
						velocity = Vector3.Lerp(tr.forward * num6, velocity.normalized * num6, Mathf.Clamp((!flag) ? 0f : (num2 * 2f), 0.5f, 1f));
						velocity.y = y;
					}
					else
					{
						float num7 = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
						num7 = maxSpeed / num7;
						if (num7 < 1f)
						{
							velocity.x *= num7;
							velocity.z *= num7;
						}
					}
					if (flag)
					{
						Vector3 trotdir = Vector3.Lerp(velocity, currentTargetDirection, Math.Max(1f - num2 * 2f, 0f));
						RotateTowards(trotdir);
					}
					else
					{
						RotateTowards(velocity);
					}
					velocity += deltaTime * gravity;
					if (rvoController != null && rvoController.enabled)
					{
						tr.position = vector;
						rvoController.Move(velocity);
					}
					else if (controller != null && controller.enabled)
					{
						tr.position = vector;
						controller.Move(velocity * deltaTime);
					}
					else
					{
						float y2 = vector.y;
						vector += velocity * deltaTime;
						vector = RaycastPosition(vector, y2);
						tr.position = vector;
					}
				}
				else if (rvoController != null && rvoController.enabled)
				{
					rvoController.Move(Vector3.zero);
				}
				if (currentPart is RichSpecial && !base.traversingOffMeshLink)
				{
					StartCoroutine(TraverseSpecial(currentPart as RichSpecial));
				}
			}
			else if (rvoController != null && rvoController.enabled)
			{
				rvoController.Move(Vector3.zero);
			}
			else if (!(controller != null) || !controller.enabled)
			{
				tr.position = RaycastPosition(tr.position, tr.position.y);
			}
			UpdateVelocity();
			lastDeltaTime = Time.deltaTime;
		}

		private new Vector3 RaycastPosition(Vector3 position, float lasty)
		{
			if (raycastingForGroundPlacement)
			{
				float num = Mathf.Max(centerOffset, lasty - position.y + centerOffset);
				RaycastHit hitInfo;
				if (Physics.Raycast(position + Vector3.up * num, Vector3.down, out hitInfo, num, groundMask) && hitInfo.distance < num)
				{
					position = hitInfo.point;
					velocity.y = 0f;
				}
			}
			return position;
		}

		private bool RotateTowards(Vector3 trotdir)
		{
			trotdir.y = 0f;
			if (trotdir != Vector3.zero)
			{
				Quaternion quaternion = tr.rotation;
				Vector3 eulerAngles = Quaternion.LookRotation(trotdir).eulerAngles;
				Vector3 eulerAngles2 = quaternion.eulerAngles;
				eulerAngles2.y = Mathf.MoveTowardsAngle(eulerAngles2.y, eulerAngles.y, rotationSpeed * deltaTime);
				tr.rotation = Quaternion.Euler(eulerAngles2);
				return Mathf.Abs(eulerAngles2.y - eulerAngles.y) < 5f;
			}
			return false;
		}
	}
}
