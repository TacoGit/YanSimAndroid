using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class RichFunnel : RichPathPart
	{
		private readonly List<Vector3> left;

		private readonly List<Vector3> right;

		private List<TriangleMeshNode> nodes;

		public Vector3 exactStart;

		public Vector3 exactEnd;

		private NavmeshBase graph;

		private int currentNode;

		private Vector3 currentPosition;

		private int checkForDestroyedNodesCounter;

		private RichPath path;

		private int[] triBuffer = new int[3];

		public bool funnelSimplification = true;

		private static Queue<TriangleMeshNode> navmeshClampQueue = new Queue<TriangleMeshNode>();

		private static List<TriangleMeshNode> navmeshClampList = new List<TriangleMeshNode>();

		private static Dictionary<TriangleMeshNode, TriangleMeshNode> navmeshClampDict = new Dictionary<TriangleMeshNode, TriangleMeshNode>();

		public TriangleMeshNode CurrentNode
		{
			get
			{
				TriangleMeshNode triangleMeshNode = nodes[currentNode];
				if (!triangleMeshNode.Destroyed)
				{
					return triangleMeshNode;
				}
				return null;
			}
		}

		public float DistanceToEndOfPath
		{
			get
			{
				TriangleMeshNode triangleMeshNode = CurrentNode;
				Vector3 vector = ((triangleMeshNode == null) ? currentPosition : triangleMeshNode.ClosestPointOnNode(currentPosition));
				return (exactEnd - vector).magnitude;
			}
		}

		public RichFunnel()
		{
			left = ListPool<Vector3>.Claim();
			right = ListPool<Vector3>.Claim();
			nodes = new List<TriangleMeshNode>();
			graph = null;
		}

		public RichFunnel Initialize(RichPath path, NavmeshBase graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (this.graph != null)
			{
				throw new InvalidOperationException("Trying to initialize an already initialized object. " + graph);
			}
			this.graph = graph;
			this.path = path;
			return this;
		}

		public override void OnEnterPool()
		{
			left.Clear();
			right.Clear();
			nodes.Clear();
			graph = null;
			currentNode = 0;
			checkForDestroyedNodesCounter = 0;
		}

		public void BuildFunnelCorridor(List<GraphNode> nodes, int start, int end)
		{
			exactStart = (nodes[start] as MeshNode).ClosestPointOnNode(exactStart);
			exactEnd = (nodes[end] as MeshNode).ClosestPointOnNode(exactEnd);
			left.Clear();
			right.Clear();
			left.Add(exactStart);
			right.Add(exactStart);
			this.nodes.Clear();
			if (funnelSimplification)
			{
				List<GraphNode> list = ListPool<GraphNode>.Claim(end - start);
				SimplifyPath(graph, nodes, start, end, list, exactStart, exactEnd);
				if (this.nodes.Capacity < list.Count)
				{
					this.nodes.Capacity = list.Count;
				}
				for (int i = 0; i < list.Count; i++)
				{
					TriangleMeshNode triangleMeshNode = list[i] as TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						this.nodes.Add(triangleMeshNode);
					}
				}
				ListPool<GraphNode>.Release(ref list);
			}
			else
			{
				if (this.nodes.Capacity < end - start)
				{
					this.nodes.Capacity = end - start;
				}
				for (int j = start; j <= end; j++)
				{
					TriangleMeshNode triangleMeshNode2 = nodes[j] as TriangleMeshNode;
					if (triangleMeshNode2 != null)
					{
						this.nodes.Add(triangleMeshNode2);
					}
				}
			}
			for (int k = 0; k < this.nodes.Count - 1; k++)
			{
				this.nodes[k].GetPortal(this.nodes[k + 1], left, right, false);
			}
			left.Add(exactEnd);
			right.Add(exactEnd);
		}

		private void SimplifyPath(IRaycastableGraph graph, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (start > end)
			{
				throw new ArgumentException("start >= end");
			}
			int num = start;
			int num2 = 0;
			while (true)
			{
				if (num2++ > 1000)
				{
					Debug.LogError("Was the path really long or have we got cought in an infinite loop?");
					return;
				}
				if (start == end)
				{
					break;
				}
				int count = result.Count;
				int num3 = end + 1;
				int num4 = start + 1;
				bool flag = false;
				while (num3 > num4 + 1)
				{
					int num5 = (num3 + num4) / 2;
					Vector3 start2 = ((start != num) ? ((Vector3)nodes[start].position) : startPoint);
					Vector3 end2 = ((num5 != end) ? ((Vector3)nodes[num5].position) : endPoint);
					GraphHitInfo hit;
					if (graph.Linecast(start2, end2, nodes[start], out hit))
					{
						num3 = num5;
						continue;
					}
					flag = true;
					num4 = num5;
				}
				if (!flag)
				{
					result.Add(nodes[start]);
					start = num4;
					continue;
				}
				Vector3 start3 = ((start != num) ? ((Vector3)nodes[start].position) : startPoint);
				Vector3 end3 = ((num4 != end) ? ((Vector3)nodes[num4].position) : endPoint);
				GraphHitInfo hit2;
				graph.Linecast(start3, end3, nodes[start], out hit2, result);
				long num6 = 0L;
				long num7 = 0L;
				for (int i = start; i <= num4; i++)
				{
					num6 += nodes[i].Penalty + ((path.seeker != null) ? path.seeker.tagPenalties[nodes[i].Tag] : 0);
				}
				for (int j = count; j < result.Count; j++)
				{
					num7 += result[j].Penalty + ((path.seeker != null) ? path.seeker.tagPenalties[result[j].Tag] : 0);
				}
				if ((double)num6 * 1.4 * (double)(num4 - start + 1) < (double)(num7 * (result.Count - count)) || result[result.Count - 1] != nodes[num4])
				{
					result.RemoveRange(count, result.Count - count);
					result.Add(nodes[start]);
					start++;
				}
				else
				{
					result.RemoveAt(result.Count - 1);
					start = num4;
				}
			}
			result.Add(nodes[end]);
		}

		private void UpdateFunnelCorridor(int splitIndex, List<TriangleMeshNode> prefix)
		{
			nodes.RemoveRange(0, splitIndex);
			nodes.InsertRange(0, prefix);
			left.Clear();
			right.Clear();
			left.Add(exactStart);
			right.Add(exactStart);
			for (int i = 0; i < nodes.Count - 1; i++)
			{
				nodes[i].GetPortal(nodes[i + 1], left, right, false);
			}
			left.Add(exactEnd);
			right.Add(exactEnd);
		}

		private bool CheckForDestroyedNodes()
		{
			int i = 0;
			for (int count = nodes.Count; i < count; i++)
			{
				if (nodes[i].Destroyed)
				{
					return true;
				}
			}
			return false;
		}

		public Vector3 ClampToNavmesh(Vector3 position)
		{
			if (path.transform != null)
			{
				position = path.transform.InverseTransform(position);
			}
			ClampToNavmeshInternal(ref position);
			if (path.transform != null)
			{
				position = path.transform.Transform(position);
			}
			return position;
		}

		public Vector3 Update(Vector3 position, List<Vector3> buffer, int numCorners, out bool lastCorner, out bool requiresRepath)
		{
			if (path.transform != null)
			{
				position = path.transform.InverseTransform(position);
			}
			lastCorner = false;
			requiresRepath = false;
			if (checkForDestroyedNodesCounter >= 10)
			{
				checkForDestroyedNodesCounter = 0;
				requiresRepath |= CheckForDestroyedNodes();
			}
			else
			{
				checkForDestroyedNodesCounter++;
			}
			bool flag = ClampToNavmeshInternal(ref position);
			currentPosition = position;
			if (flag)
			{
				requiresRepath = true;
				lastCorner = false;
				buffer.Add(position);
			}
			else if (!FindNextCorners(position, currentNode, buffer, numCorners, out lastCorner))
			{
				Debug.LogError("Failed to find next corners in the path");
				buffer.Add(position);
			}
			if (path.transform != null)
			{
				for (int i = 0; i < buffer.Count; i++)
				{
					buffer[i] = path.transform.Transform(buffer[i]);
				}
				position = path.transform.Transform(position);
			}
			return position;
		}

		private bool ClampToNavmeshInternal(ref Vector3 position)
		{
			TriangleMeshNode triangleMeshNode = nodes[currentNode];
			if (triangleMeshNode.Destroyed)
			{
				return true;
			}
			if (triangleMeshNode.ContainsPoint(position))
			{
				return false;
			}
			Queue<TriangleMeshNode> queue = navmeshClampQueue;
			List<TriangleMeshNode> list = navmeshClampList;
			Dictionary<TriangleMeshNode, TriangleMeshNode> dictionary = navmeshClampDict;
			triangleMeshNode.TemporaryFlag1 = true;
			dictionary[triangleMeshNode] = null;
			queue.Enqueue(triangleMeshNode);
			list.Add(triangleMeshNode);
			float num = float.PositiveInfinity;
			Vector3 vector = position;
			TriangleMeshNode triangleMeshNode2 = null;
			while (queue.Count > 0)
			{
				TriangleMeshNode triangleMeshNode3 = queue.Dequeue();
				Vector3 vector2 = triangleMeshNode3.ClosestPointOnNodeXZ(position);
				float num2 = VectorMath.MagnitudeXZ(vector2 - position);
				if (!(num2 <= num * 1.05f + 0.001f))
				{
					continue;
				}
				if (num2 < num)
				{
					num = num2;
					vector = vector2;
					triangleMeshNode2 = triangleMeshNode3;
				}
				for (int i = 0; i < triangleMeshNode3.connections.Length; i++)
				{
					TriangleMeshNode triangleMeshNode4 = triangleMeshNode3.connections[i].node as TriangleMeshNode;
					if (triangleMeshNode4 != null && !triangleMeshNode4.TemporaryFlag1)
					{
						triangleMeshNode4.TemporaryFlag1 = true;
						dictionary[triangleMeshNode4] = triangleMeshNode3;
						queue.Enqueue(triangleMeshNode4);
						list.Add(triangleMeshNode4);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].TemporaryFlag1 = false;
			}
			list.ClearFast();
			int num3 = nodes.IndexOf(triangleMeshNode2);
			position.x = vector.x;
			position.z = vector.z;
			if (num3 == -1)
			{
				List<TriangleMeshNode> list2 = navmeshClampList;
				while (num3 == -1)
				{
					list2.Add(triangleMeshNode2);
					triangleMeshNode2 = dictionary[triangleMeshNode2];
					num3 = nodes.IndexOf(triangleMeshNode2);
				}
				exactStart = position;
				UpdateFunnelCorridor(num3, list2);
				list2.ClearFast();
				currentNode = 0;
			}
			else
			{
				currentNode = num3;
			}
			dictionary.Clear();
			return currentNode + 1 < nodes.Count && nodes[currentNode + 1].Destroyed;
		}

		public void FindWalls(List<Vector3> wallBuffer, float range)
		{
			FindWalls(currentNode, wallBuffer, currentPosition, range);
		}

		private void FindWalls(int nodeIndex, List<Vector3> wallBuffer, Vector3 position, float range)
		{
			if (range <= 0f)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			range *= range;
			position.y = 0f;
			int num = 0;
			while (!flag || !flag2)
			{
				if ((num >= 0 || !flag) && (num <= 0 || !flag2))
				{
					if (num < 0 && nodeIndex + num < 0)
					{
						flag = true;
					}
					else if (num > 0 && nodeIndex + num >= nodes.Count)
					{
						flag2 = true;
					}
					else
					{
						TriangleMeshNode triangleMeshNode = ((nodeIndex + num - 1 >= 0) ? nodes[nodeIndex + num - 1] : null);
						TriangleMeshNode triangleMeshNode2 = nodes[nodeIndex + num];
						TriangleMeshNode triangleMeshNode3 = ((nodeIndex + num + 1 < nodes.Count) ? nodes[nodeIndex + num + 1] : null);
						if (triangleMeshNode2.Destroyed)
						{
							break;
						}
						if ((triangleMeshNode2.ClosestPointOnNodeXZ(position) - position).sqrMagnitude > range)
						{
							if (num < 0)
							{
								flag = true;
							}
							else
							{
								flag2 = true;
							}
						}
						else
						{
							for (int i = 0; i < 3; i++)
							{
								triBuffer[i] = 0;
							}
							for (int j = 0; j < triangleMeshNode2.connections.Length; j++)
							{
								TriangleMeshNode triangleMeshNode4 = triangleMeshNode2.connections[j].node as TriangleMeshNode;
								if (triangleMeshNode4 == null)
								{
									continue;
								}
								int num2 = -1;
								for (int k = 0; k < 3; k++)
								{
									for (int l = 0; l < 3; l++)
									{
										if (triangleMeshNode2.GetVertex(k) == triangleMeshNode4.GetVertex((l + 1) % 3) && triangleMeshNode2.GetVertex((k + 1) % 3) == triangleMeshNode4.GetVertex(l))
										{
											num2 = k;
											k = 3;
											break;
										}
									}
								}
								if (num2 != -1)
								{
									triBuffer[num2] = ((triangleMeshNode4 != triangleMeshNode && triangleMeshNode4 != triangleMeshNode3) ? 1 : 2);
								}
							}
							for (int m = 0; m < 3; m++)
							{
								if (triBuffer[m] == 0)
								{
									wallBuffer.Add((Vector3)triangleMeshNode2.GetVertex(m));
									wallBuffer.Add((Vector3)triangleMeshNode2.GetVertex((m + 1) % 3));
								}
							}
						}
					}
				}
				num = ((num >= 0) ? (-num - 1) : (-num));
			}
			if (path.transform != null)
			{
				for (int n = 0; n < wallBuffer.Count; n++)
				{
					wallBuffer[n] = path.transform.Transform(wallBuffer[n]);
				}
			}
		}

		private bool FindNextCorners(Vector3 origin, int startIndex, List<Vector3> funnelPath, int numCorners, out bool lastCorner)
		{
			lastCorner = false;
			if (left == null)
			{
				throw new Exception("left list is null");
			}
			if (right == null)
			{
				throw new Exception("right list is null");
			}
			if (funnelPath == null)
			{
				throw new ArgumentNullException("funnelPath");
			}
			if (left.Count != right.Count)
			{
				throw new ArgumentException("left and right lists must have equal length");
			}
			int count = left.Count;
			if (count == 0)
			{
				throw new ArgumentException("no diagonals");
			}
			if (count - startIndex < 3)
			{
				funnelPath.Add(left[count - 1]);
				lastCorner = true;
				return true;
			}
			while (left[startIndex + 1] == left[startIndex + 2] && right[startIndex + 1] == right[startIndex + 2])
			{
				startIndex++;
				if (count - startIndex <= 3)
				{
					return false;
				}
			}
			Vector3 vector = left[startIndex + 2];
			if (vector == left[startIndex + 1])
			{
				vector = right[startIndex + 2];
			}
			while (VectorMath.IsColinearXZ(origin, left[startIndex + 1], right[startIndex + 1]) || VectorMath.RightOrColinearXZ(left[startIndex + 1], right[startIndex + 1], vector) == VectorMath.RightOrColinearXZ(left[startIndex + 1], right[startIndex + 1], origin))
			{
				startIndex++;
				if (count - startIndex < 3)
				{
					funnelPath.Add(left[count - 1]);
					lastCorner = true;
					return true;
				}
				vector = left[startIndex + 2];
				if (vector == left[startIndex + 1])
				{
					vector = right[startIndex + 2];
				}
			}
			Vector3 vector2 = origin;
			Vector3 vector3 = left[startIndex + 1];
			Vector3 vector4 = right[startIndex + 1];
			int num = startIndex;
			int num2 = startIndex + 1;
			int num3 = startIndex + 1;
			for (int i = startIndex + 2; i < count; i++)
			{
				if (funnelPath.Count >= numCorners)
				{
					return true;
				}
				if (funnelPath.Count > 2000)
				{
					Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
					break;
				}
				Vector3 vector5 = left[i];
				Vector3 vector6 = right[i];
				if (VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector6) >= 0f)
				{
					if (!(vector2 == vector4) && !(VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector6) <= 0f))
					{
						funnelPath.Add(vector3);
						vector2 = vector3;
						num = num3;
						vector3 = vector2;
						vector4 = vector2;
						num3 = num;
						num2 = num;
						i = num;
						continue;
					}
					vector4 = vector6;
					num2 = i;
				}
				if (VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector5) <= 0f)
				{
					if (vector2 == vector3 || VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector5) >= 0f)
					{
						vector3 = vector5;
						num3 = i;
						continue;
					}
					funnelPath.Add(vector4);
					vector2 = vector4;
					num = num2;
					vector3 = vector2;
					vector4 = vector2;
					num3 = num;
					num2 = num;
					i = num;
				}
			}
			lastCorner = true;
			funnelPath.Add(left[count - 1]);
			return true;
		}
	}
}
