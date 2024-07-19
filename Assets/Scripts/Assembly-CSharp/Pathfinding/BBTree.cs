using System;
using System.Diagnostics;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class BBTree : IAstarPooledObject
	{
		private struct BBTreeBox
		{
			public IntRect rect;

			public int nodeOffset;

			public int left;

			public int right;

			public bool IsLeaf
			{
				get
				{
					return nodeOffset >= 0;
				}
			}

			public BBTreeBox(IntRect rect)
			{
				nodeOffset = -1;
				this.rect = rect;
				left = (right = -1);
			}

			public BBTreeBox(int nodeOffset, IntRect rect)
			{
				this.nodeOffset = nodeOffset;
				this.rect = rect;
				left = (right = -1);
			}

			public bool Contains(Vector3 point)
			{
				Int3 @int = (Int3)point;
				return rect.Contains(@int.x, @int.z);
			}
		}

		private BBTreeBox[] tree;

		private TriangleMeshNode[] nodeLookup;

		private int count;

		private int leafNodes;

		private const int MaximumLeafSize = 4;

		public Rect Size
		{
			get
			{
				if (count == 0)
				{
					return new Rect(0f, 0f, 0f, 0f);
				}
				IntRect rect = tree[0].rect;
				return Rect.MinMaxRect((float)rect.xmin * 0.001f, (float)rect.ymin * 0.001f, (float)rect.xmax * 0.001f, (float)rect.ymax * 0.001f);
			}
		}

		public void Clear()
		{
			count = 0;
			leafNodes = 0;
			if (tree != null)
			{
				ArrayPool<BBTreeBox>.Release(ref tree);
			}
			if (nodeLookup != null)
			{
				for (int i = 0; i < nodeLookup.Length; i++)
				{
					nodeLookup[i] = null;
				}
				ArrayPool<TriangleMeshNode>.Release(ref nodeLookup);
			}
			tree = ArrayPool<BBTreeBox>.Claim(0);
			nodeLookup = ArrayPool<TriangleMeshNode>.Claim(0);
		}

		void IAstarPooledObject.OnEnterPool()
		{
			Clear();
		}

		private void EnsureCapacity(int c)
		{
			if (c > tree.Length)
			{
				BBTreeBox[] array = ArrayPool<BBTreeBox>.Claim(c);
				tree.CopyTo(array, 0);
				ArrayPool<BBTreeBox>.Release(ref tree);
				tree = array;
			}
		}

		private void EnsureNodeCapacity(int c)
		{
			if (c > nodeLookup.Length)
			{
				TriangleMeshNode[] array = ArrayPool<TriangleMeshNode>.Claim(c);
				nodeLookup.CopyTo(array, 0);
				ArrayPool<TriangleMeshNode>.Release(ref nodeLookup);
				nodeLookup = array;
			}
		}

		private int GetBox(IntRect rect)
		{
			if (count >= tree.Length)
			{
				EnsureCapacity(count + 1);
			}
			tree[count] = new BBTreeBox(rect);
			count++;
			return count - 1;
		}

		public void RebuildFrom(TriangleMeshNode[] nodes)
		{
			Clear();
			if (nodes.Length != 0)
			{
				EnsureCapacity(Mathf.CeilToInt((float)nodes.Length * 2.1f));
				EnsureNodeCapacity(Mathf.CeilToInt((float)nodes.Length * 1.1f));
				int[] array = ArrayPool<int>.Claim(nodes.Length);
				for (int i = 0; i < nodes.Length; i++)
				{
					array[i] = i;
				}
				IntRect[] array2 = ArrayPool<IntRect>.Claim(nodes.Length);
				for (int j = 0; j < nodes.Length; j++)
				{
					Int3 v;
					Int3 v2;
					Int3 v3;
					nodes[j].GetVertices(out v, out v2, out v3);
					IntRect intRect = new IntRect(v.x, v.z, v.x, v.z).ExpandToContain(v2.x, v2.z).ExpandToContain(v3.x, v3.z);
					array2[j] = intRect;
				}
				RebuildFromInternal(nodes, array, array2, 0, nodes.Length, false);
				ArrayPool<int>.Release(ref array);
				ArrayPool<IntRect>.Release(ref array2);
			}
		}

		private static int SplitByX(TriangleMeshNode[] nodes, int[] permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				if (nodes[permutation[i]].position.x > divider)
				{
					num--;
					int num2 = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = num2;
					i--;
				}
			}
			return num;
		}

		private static int SplitByZ(TriangleMeshNode[] nodes, int[] permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				if (nodes[permutation[i]].position.z > divider)
				{
					num--;
					int num2 = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = num2;
					i--;
				}
			}
			return num;
		}

		private int RebuildFromInternal(TriangleMeshNode[] nodes, int[] permutation, IntRect[] nodeBounds, int from, int to, bool odd)
		{
			IntRect rect = NodeBounds(permutation, nodeBounds, from, to);
			int box = GetBox(rect);
			if (to - from <= 4)
			{
				int num = (tree[box].nodeOffset = leafNodes * 4);
				EnsureNodeCapacity(num + 4);
				leafNodes++;
				for (int i = 0; i < 4; i++)
				{
					nodeLookup[num + i] = ((i >= to - from) ? null : nodes[permutation[from + i]]);
				}
				return box;
			}
			int num2;
			if (odd)
			{
				int divider = (rect.xmin + rect.xmax) / 2;
				num2 = SplitByX(nodes, permutation, from, to, divider);
			}
			else
			{
				int divider2 = (rect.ymin + rect.ymax) / 2;
				num2 = SplitByZ(nodes, permutation, from, to, divider2);
			}
			if (num2 == from || num2 == to)
			{
				if (!odd)
				{
					int divider3 = (rect.xmin + rect.xmax) / 2;
					num2 = SplitByX(nodes, permutation, from, to, divider3);
				}
				else
				{
					int divider4 = (rect.ymin + rect.ymax) / 2;
					num2 = SplitByZ(nodes, permutation, from, to, divider4);
				}
				if (num2 == from || num2 == to)
				{
					num2 = (from + to) / 2;
				}
			}
			tree[box].left = RebuildFromInternal(nodes, permutation, nodeBounds, from, num2, !odd);
			tree[box].right = RebuildFromInternal(nodes, permutation, nodeBounds, num2, to, !odd);
			return box;
		}

		private static IntRect NodeBounds(int[] permutation, IntRect[] nodeBounds, int from, int to)
		{
			IntRect result = nodeBounds[permutation[from]];
			for (int i = from + 1; i < to; i++)
			{
				IntRect intRect = nodeBounds[permutation[i]];
				result.xmin = Math.Min(result.xmin, intRect.xmin);
				result.ymin = Math.Min(result.ymin, intRect.ymin);
				result.xmax = Math.Max(result.xmax, intRect.xmax);
				result.ymax = Math.Max(result.ymax, intRect.ymax);
			}
			return result;
		}

		[Conditional("ASTARDEBUG")]
		private static void DrawDebugRect(IntRect rect)
		{
			UnityEngine.Debug.DrawLine(new Vector3(rect.xmin, 0f, rect.ymin), new Vector3(rect.xmax, 0f, rect.ymin), Color.white);
			UnityEngine.Debug.DrawLine(new Vector3(rect.xmin, 0f, rect.ymax), new Vector3(rect.xmax, 0f, rect.ymax), Color.white);
			UnityEngine.Debug.DrawLine(new Vector3(rect.xmin, 0f, rect.ymin), new Vector3(rect.xmin, 0f, rect.ymax), Color.white);
			UnityEngine.Debug.DrawLine(new Vector3(rect.xmax, 0f, rect.ymin), new Vector3(rect.xmax, 0f, rect.ymax), Color.white);
		}

		[Conditional("ASTARDEBUG")]
		private static void DrawDebugNode(TriangleMeshNode node, float yoffset, Color color)
		{
			UnityEngine.Debug.DrawLine((Vector3)node.GetVertex(1) + Vector3.up * yoffset, (Vector3)node.GetVertex(2) + Vector3.up * yoffset, color);
			UnityEngine.Debug.DrawLine((Vector3)node.GetVertex(0) + Vector3.up * yoffset, (Vector3)node.GetVertex(1) + Vector3.up * yoffset, color);
			UnityEngine.Debug.DrawLine((Vector3)node.GetVertex(2) + Vector3.up * yoffset, (Vector3)node.GetVertex(0) + Vector3.up * yoffset, color);
		}

		public NNInfoInternal QueryClosest(Vector3 p, NNConstraint constraint, out float distance)
		{
			distance = float.PositiveInfinity;
			return QueryClosest(p, constraint, ref distance, new NNInfoInternal(null));
		}

		public NNInfoInternal QueryClosestXZ(Vector3 p, NNConstraint constraint, ref float distance, NNInfoInternal previous)
		{
			float closestSqrDist = distance * distance;
			float num = closestSqrDist;
			if (count > 0 && SquaredRectPointDistance(tree[0].rect, p) < closestSqrDist)
			{
				SearchBoxClosestXZ(0, p, ref closestSqrDist, constraint, ref previous);
				if (closestSqrDist < num)
				{
					distance = Mathf.Sqrt(closestSqrDist);
				}
			}
			return previous;
		}

		private void SearchBoxClosestXZ(int boxi, Vector3 p, ref float closestSqrDist, NNConstraint constraint, ref NNInfoInternal nnInfo)
		{
			BBTreeBox bBTreeBox = tree[boxi];
			if (bBTreeBox.IsLeaf)
			{
				TriangleMeshNode[] array = nodeLookup;
				for (int i = 0; i < 4 && array[bBTreeBox.nodeOffset + i] != null; i++)
				{
					TriangleMeshNode triangleMeshNode = array[bBTreeBox.nodeOffset + i];
					if (constraint == null || constraint.Suitable(triangleMeshNode))
					{
						Vector3 constClampedPosition = triangleMeshNode.ClosestPointOnNodeXZ(p);
						float num = (constClampedPosition.x - p.x) * (constClampedPosition.x - p.x) + (constClampedPosition.z - p.z) * (constClampedPosition.z - p.z);
						if (nnInfo.constrainedNode == null || num < closestSqrDist - 1E-06f || (num <= closestSqrDist + 1E-06f && Mathf.Abs(constClampedPosition.y - p.y) < Mathf.Abs(nnInfo.constClampedPosition.y - p.y)))
						{
							nnInfo.constrainedNode = triangleMeshNode;
							nnInfo.constClampedPosition = constClampedPosition;
							closestSqrDist = num;
						}
					}
				}
			}
			else
			{
				int first = bBTreeBox.left;
				int second = bBTreeBox.right;
				float firstDist;
				float secondDist;
				GetOrderedChildren(ref first, ref second, out firstDist, out secondDist, p);
				if (firstDist <= closestSqrDist)
				{
					SearchBoxClosestXZ(first, p, ref closestSqrDist, constraint, ref nnInfo);
				}
				if (secondDist <= closestSqrDist)
				{
					SearchBoxClosestXZ(second, p, ref closestSqrDist, constraint, ref nnInfo);
				}
			}
		}

		public NNInfoInternal QueryClosest(Vector3 p, NNConstraint constraint, ref float distance, NNInfoInternal previous)
		{
			float closestSqrDist = distance * distance;
			float num = closestSqrDist;
			if (count > 0 && SquaredRectPointDistance(tree[0].rect, p) < closestSqrDist)
			{
				SearchBoxClosest(0, p, ref closestSqrDist, constraint, ref previous);
				if (closestSqrDist < num)
				{
					distance = Mathf.Sqrt(closestSqrDist);
				}
			}
			return previous;
		}

		private void SearchBoxClosest(int boxi, Vector3 p, ref float closestSqrDist, NNConstraint constraint, ref NNInfoInternal nnInfo)
		{
			BBTreeBox bBTreeBox = tree[boxi];
			if (bBTreeBox.IsLeaf)
			{
				TriangleMeshNode[] array = nodeLookup;
				for (int i = 0; i < 4 && array[bBTreeBox.nodeOffset + i] != null; i++)
				{
					TriangleMeshNode triangleMeshNode = array[bBTreeBox.nodeOffset + i];
					Vector3 vector = triangleMeshNode.ClosestPointOnNode(p);
					float sqrMagnitude = (vector - p).sqrMagnitude;
					if (sqrMagnitude < closestSqrDist && (constraint == null || constraint.Suitable(triangleMeshNode)))
					{
						nnInfo.constrainedNode = triangleMeshNode;
						nnInfo.constClampedPosition = vector;
						closestSqrDist = sqrMagnitude;
					}
				}
			}
			else
			{
				int first = bBTreeBox.left;
				int second = bBTreeBox.right;
				float firstDist;
				float secondDist;
				GetOrderedChildren(ref first, ref second, out firstDist, out secondDist, p);
				if (firstDist < closestSqrDist)
				{
					SearchBoxClosest(first, p, ref closestSqrDist, constraint, ref nnInfo);
				}
				if (secondDist < closestSqrDist)
				{
					SearchBoxClosest(second, p, ref closestSqrDist, constraint, ref nnInfo);
				}
			}
		}

		private void GetOrderedChildren(ref int first, ref int second, out float firstDist, out float secondDist, Vector3 p)
		{
			firstDist = SquaredRectPointDistance(tree[first].rect, p);
			secondDist = SquaredRectPointDistance(tree[second].rect, p);
			if (secondDist < firstDist)
			{
				int num = first;
				first = second;
				second = num;
				float num2 = firstDist;
				firstDist = secondDist;
				secondDist = num2;
			}
		}

		public TriangleMeshNode QueryInside(Vector3 p, NNConstraint constraint)
		{
			return (count == 0 || !tree[0].Contains(p)) ? null : SearchBoxInside(0, p, constraint);
		}

		private TriangleMeshNode SearchBoxInside(int boxi, Vector3 p, NNConstraint constraint)
		{
			BBTreeBox bBTreeBox = tree[boxi];
			if (bBTreeBox.IsLeaf)
			{
				TriangleMeshNode[] array = nodeLookup;
				for (int i = 0; i < 4 && array[bBTreeBox.nodeOffset + i] != null; i++)
				{
					TriangleMeshNode triangleMeshNode = array[bBTreeBox.nodeOffset + i];
					if (triangleMeshNode.ContainsPoint((Int3)p) && (constraint == null || constraint.Suitable(triangleMeshNode)))
					{
						return triangleMeshNode;
					}
				}
			}
			else
			{
				if (tree[bBTreeBox.left].Contains(p))
				{
					TriangleMeshNode triangleMeshNode2 = SearchBoxInside(bBTreeBox.left, p, constraint);
					if (triangleMeshNode2 != null)
					{
						return triangleMeshNode2;
					}
				}
				if (tree[bBTreeBox.right].Contains(p))
				{
					TriangleMeshNode triangleMeshNode3 = SearchBoxInside(bBTreeBox.right, p, constraint);
					if (triangleMeshNode3 != null)
					{
						return triangleMeshNode3;
					}
				}
			}
			return null;
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			if (count != 0)
			{
				OnDrawGizmos(0, 0);
			}
		}

		private void OnDrawGizmos(int boxi, int depth)
		{
			BBTreeBox bBTreeBox = tree[boxi];
			Vector3 vector = (Vector3)new Int3(bBTreeBox.rect.xmin, 0, bBTreeBox.rect.ymin);
			Vector3 vector2 = (Vector3)new Int3(bBTreeBox.rect.xmax, 0, bBTreeBox.rect.ymax);
			Vector3 vector3 = (vector + vector2) * 0.5f;
			Vector3 vector4 = (vector2 - vector3) * 2f;
			vector4 = new Vector3(vector4.x, 1f, vector4.z);
			vector3.y += depth * 2;
			Gizmos.color = AstarMath.IntToColor(depth, 1f);
			Gizmos.DrawCube(vector3, vector4);
			if (!bBTreeBox.IsLeaf)
			{
				OnDrawGizmos(bBTreeBox.left, depth + 1);
				OnDrawGizmos(bBTreeBox.right, depth + 1);
			}
		}

		private static bool NodeIntersectsCircle(TriangleMeshNode node, Vector3 p, float radius)
		{
			if (float.IsPositiveInfinity(radius))
			{
				return true;
			}
			return (p - node.ClosestPointOnNode(p)).sqrMagnitude < radius * radius;
		}

		private static bool RectIntersectsCircle(IntRect r, Vector3 p, float radius)
		{
			if (float.IsPositiveInfinity(radius))
			{
				return true;
			}
			Vector3 vector = p;
			p.x = Math.Max(p.x, (float)r.xmin * 0.001f);
			p.x = Math.Min(p.x, (float)r.xmax * 0.001f);
			p.z = Math.Max(p.z, (float)r.ymin * 0.001f);
			p.z = Math.Min(p.z, (float)r.ymax * 0.001f);
			return (p.x - vector.x) * (p.x - vector.x) + (p.z - vector.z) * (p.z - vector.z) < radius * radius;
		}

		private static float SquaredRectPointDistance(IntRect r, Vector3 p)
		{
			Vector3 vector = p;
			p.x = Math.Max(p.x, (float)r.xmin * 0.001f);
			p.x = Math.Min(p.x, (float)r.xmax * 0.001f);
			p.z = Math.Max(p.z, (float)r.ymin * 0.001f);
			p.z = Math.Min(p.z, (float)r.ymax * 0.001f);
			return (p.x - vector.x) * (p.x - vector.x) + (p.z - vector.z) * (p.z - vector.z);
		}
	}
}
