using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class Funnel
	{
		public struct FunnelPortals
		{
			public List<Vector3> left;

			public List<Vector3> right;
		}

		public struct PathPart
		{
			public int startIndex;

			public int endIndex;

			public Vector3 startPoint;

			public Vector3 endPoint;

			public bool isLink;
		}

		public static List<PathPart> SplitIntoParts(Path path)
		{
			List<GraphNode> path2 = path.path;
			List<PathPart> list = ListPool<PathPart>.Claim();
			if (path2 == null || path2.Count == 0)
			{
				return list;
			}
			int i;
			for (i = 0; i < path2.Count; i++)
			{
				if (path2[i] is TriangleMeshNode || path2[i] is GridNodeBase)
				{
					PathPart item = default(PathPart);
					item.startIndex = i;
					for (uint graphIndex = path2[i].GraphIndex; i < path2.Count && (path2[i].GraphIndex == graphIndex || path2[i] is NodeLink3Node); i++)
					{
					}
					i = (item.endIndex = i - 1);
					if (item.startIndex == 0)
					{
						item.startPoint = path.vectorPath[0];
					}
					else
					{
						item.startPoint = (Vector3)path2[item.startIndex - 1].position;
					}
					if (item.endIndex == path2.Count - 1)
					{
						item.endPoint = path.vectorPath[path.vectorPath.Count - 1];
					}
					else
					{
						item.endPoint = (Vector3)path2[item.endIndex + 1].position;
					}
					list.Add(item);
				}
				else
				{
					if (!(NodeLink2.GetNodeLink(path2[i]) != null))
					{
						throw new Exception("Unsupported node type or null node");
					}
					PathPart item2 = default(PathPart);
					item2.startIndex = i;
					uint graphIndex2 = path2[i].GraphIndex;
					for (i++; i < path2.Count && path2[i].GraphIndex == graphIndex2; i++)
					{
					}
					i--;
					if (i - item2.startIndex != 0)
					{
						if (i - item2.startIndex != 1)
						{
							throw new Exception("NodeLink2 link length greater than two (2) nodes. " + (i - item2.startIndex + 1));
						}
						item2.endIndex = i;
						item2.isLink = true;
						item2.startPoint = (Vector3)path2[item2.startIndex].position;
						item2.endPoint = (Vector3)path2[item2.endIndex].position;
						list.Add(item2);
					}
				}
			}
			return list;
		}

		public static FunnelPortals ConstructFunnelPortals(List<GraphNode> nodes, PathPart part)
		{
			if (nodes == null || nodes.Count == 0)
			{
				FunnelPortals result = default(FunnelPortals);
				result.left = ListPool<Vector3>.Claim(0);
				result.right = ListPool<Vector3>.Claim(0);
				return result;
			}
			if (part.endIndex < part.startIndex || part.startIndex < 0 || part.endIndex > nodes.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			List<Vector3> list = ListPool<Vector3>.Claim(nodes.Count + 1);
			List<Vector3> list2 = ListPool<Vector3>.Claim(nodes.Count + 1);
			list.Add(part.startPoint);
			list2.Add(part.startPoint);
			for (int i = part.startIndex; i < part.endIndex; i++)
			{
				if (!nodes[i].GetPortal(nodes[i + 1], list, list2, false))
				{
					list.Add((Vector3)nodes[i].position);
					list2.Add((Vector3)nodes[i].position);
					list.Add((Vector3)nodes[i + 1].position);
					list2.Add((Vector3)nodes[i + 1].position);
				}
			}
			list.Add(part.endPoint);
			list2.Add(part.endPoint);
			FunnelPortals result2 = default(FunnelPortals);
			result2.left = list;
			result2.right = list2;
			return result2;
		}

		public static void ShrinkPortals(FunnelPortals portals, float shrink)
		{
			if (shrink <= 1E-05f)
			{
				return;
			}
			for (int i = 0; i < portals.left.Count; i++)
			{
				Vector3 vector = portals.left[i];
				Vector3 vector2 = portals.right[i];
				float magnitude = (vector - vector2).magnitude;
				if (magnitude > 0f)
				{
					float num = Mathf.Min(shrink / magnitude, 0.4f);
					portals.left[i] = Vector3.Lerp(vector, vector2, num);
					portals.right[i] = Vector3.Lerp(vector, vector2, 1f - num);
				}
			}
		}

		private static bool UnwrapHelper(Vector3 portalStart, Vector3 portalEnd, Vector3 prevPoint, Vector3 nextPoint, ref Quaternion mRot, ref Vector3 mOffset)
		{
			if (VectorMath.IsColinear(portalStart, portalEnd, nextPoint))
			{
				return false;
			}
			Vector3 vector = portalEnd - portalStart;
			float sqrMagnitude = vector.sqrMagnitude;
			prevPoint -= Vector3.Dot(prevPoint - portalStart, vector) / sqrMagnitude * vector;
			nextPoint -= Vector3.Dot(nextPoint - portalStart, vector) / sqrMagnitude * vector;
			Quaternion quaternion = Quaternion.FromToRotation(nextPoint - portalStart, portalStart - prevPoint);
			mOffset += mRot * (portalStart - quaternion * portalStart);
			mRot *= quaternion;
			return true;
		}

		public static void Unwrap(FunnelPortals funnel, Vector2[] left, Vector2[] right)
		{
			int num = 1;
			Vector3 fromDirection = Vector3.Cross(funnel.right[1] - funnel.left[0], funnel.left[1] - funnel.left[0]);
			while (fromDirection.sqrMagnitude <= 1E-08f && num + 1 < funnel.left.Count)
			{
				num++;
				fromDirection = Vector3.Cross(funnel.right[num] - funnel.left[0], funnel.left[num] - funnel.left[0]);
			}
			left[0] = (right[0] = Vector2.zero);
			Vector3 vector = funnel.left[1];
			Vector3 vector2 = funnel.right[1];
			Vector3 prevPoint = funnel.left[0];
			Quaternion mRot = Quaternion.FromToRotation(fromDirection, Vector3.forward);
			Vector3 mOffset = mRot * -funnel.right[0];
			for (int i = 1; i < funnel.left.Count; i++)
			{
				if (UnwrapHelper(vector, vector2, prevPoint, funnel.left[i], ref mRot, ref mOffset))
				{
					prevPoint = vector;
					vector = funnel.left[i];
				}
				left[i] = mRot * funnel.left[i] + mOffset;
				if (UnwrapHelper(vector, vector2, prevPoint, funnel.right[i], ref mRot, ref mOffset))
				{
					prevPoint = vector2;
					vector2 = funnel.right[i];
				}
				right[i] = mRot * funnel.right[i] + mOffset;
			}
		}

		private static int FixFunnel(Vector2[] left, Vector2[] right, int numPortals)
		{
			if (numPortals > left.Length || numPortals > right.Length)
			{
				throw new ArgumentException("Arrays do not have as many elements as specified");
			}
			if (numPortals < 3)
			{
				return -1;
			}
			int num = 0;
			while (left[num + 1] == left[num + 2] && right[num + 1] == right[num + 2])
			{
				left[num + 1] = left[num];
				right[num + 1] = right[num];
				num++;
				if (numPortals - num < 3)
				{
					return -1;
				}
			}
			return num;
		}

		protected static Vector2 ToXZ(Vector3 p)
		{
			return new Vector2(p.x, p.z);
		}

		protected static Vector3 FromXZ(Vector2 p)
		{
			return new Vector3(p.x, 0f, p.y);
		}

		protected static bool RightOrColinear(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y <= 0f;
		}

		protected static bool LeftOrColinear(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y >= 0f;
		}

		public static List<Vector3> Calculate(FunnelPortals funnel, bool unwrap, bool splitAtEveryPortal)
		{
			if (funnel.left.Count != funnel.right.Count)
			{
				throw new ArgumentException("funnel.left.Count != funnel.right.Count");
			}
			Vector2[] array = ArrayPool<Vector2>.Claim(funnel.left.Count);
			Vector2[] array2 = ArrayPool<Vector2>.Claim(funnel.left.Count);
			if (unwrap)
			{
				Unwrap(funnel, array, array2);
			}
			else
			{
				for (int i = 0; i < funnel.left.Count; i++)
				{
					array[i] = ToXZ(funnel.left[i]);
					array2[i] = ToXZ(funnel.right[i]);
				}
			}
			int num = FixFunnel(array, array2, funnel.left.Count);
			List<int> list = ListPool<int>.Claim();
			if (num == -1)
			{
				list.Add(0);
				list.Add(funnel.left.Count - 1);
			}
			else
			{
				bool lastCorner;
				Calculate(array, array2, funnel.left.Count, num, list, int.MaxValue, out lastCorner);
			}
			List<Vector3> list2 = ListPool<Vector3>.Claim(list.Count);
			Vector2 p = array[0];
			int num2 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				int num3 = list[j];
				if (splitAtEveryPortal)
				{
					Vector2 vector = ((num3 < 0) ? array2[-num3] : array[num3]);
					for (int k = num2 + 1; k < Math.Abs(num3); k++)
					{
						float t = VectorMath.LineIntersectionFactorXZ(FromXZ(array[k]), FromXZ(array2[k]), FromXZ(p), FromXZ(vector));
						list2.Add(Vector3.Lerp(funnel.left[k], funnel.right[k], t));
					}
					num2 = Mathf.Abs(num3);
					p = vector;
				}
				if (num3 >= 0)
				{
					list2.Add(funnel.left[num3]);
				}
				else
				{
					list2.Add(funnel.right[-num3]);
				}
			}
			ListPool<int>.Release(ref list);
			ArrayPool<Vector2>.Release(ref array);
			ArrayPool<Vector2>.Release(ref array2);
			return list2;
		}

		private static void Calculate(Vector2[] left, Vector2[] right, int numPortals, int startIndex, List<int> funnelPath, int maxCorners, out bool lastCorner)
		{
			if (left.Length != right.Length)
			{
				throw new ArgumentException();
			}
			lastCorner = false;
			int num = startIndex;
			int num2 = startIndex + 1;
			int num3 = startIndex + 1;
			Vector2 vector = left[num];
			Vector2 vector2 = left[num3];
			Vector2 vector3 = right[num2];
			funnelPath.Add(num);
			for (int i = startIndex + 2; i < numPortals; i++)
			{
				if (funnelPath.Count >= maxCorners)
				{
					return;
				}
				if (funnelPath.Count > 2000)
				{
					Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
					break;
				}
				Vector2 vector4 = left[i];
				Vector2 vector5 = right[i];
				if (LeftOrColinear(vector3 - vector, vector5 - vector))
				{
					if (!(vector == vector3) && !RightOrColinear(vector2 - vector, vector5 - vector))
					{
						vector = (vector3 = vector2);
						i = (num = (num2 = num3));
						funnelPath.Add(num);
						continue;
					}
					vector3 = vector5;
					num2 = i;
				}
				if (RightOrColinear(vector2 - vector, vector4 - vector))
				{
					if (vector == vector2 || LeftOrColinear(vector3 - vector, vector4 - vector))
					{
						vector2 = vector4;
						num3 = i;
					}
					else
					{
						vector = (vector2 = vector3);
						i = (num = (num3 = num2));
						funnelPath.Add(-num);
					}
				}
			}
			lastCorner = true;
			funnelPath.Add(numPortals - 1);
		}
	}
}
