using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public static class GraphUtilities
	{
		public static List<Vector3> GetContours(NavGraph graph)
		{
			List<Vector3> result = ListPool<Vector3>.Claim();
			if (graph is INavmesh)
			{
				GetContours(graph as INavmesh, delegate(List<Int3> vertices, bool cycle)
				{
					int index = (cycle ? (vertices.Count - 1) : 0);
					for (int j = 0; j < vertices.Count; j++)
					{
						result.Add((Vector3)vertices[index]);
						result.Add((Vector3)vertices[j]);
						index = j;
					}
				});
			}
			else if (graph is GridGraph)
			{
				GetContours(graph as GridGraph, delegate(Vector3[] vertices)
				{
					int num = vertices.Length - 1;
					for (int i = 0; i < vertices.Length; i++)
					{
						result.Add(vertices[num]);
						result.Add(vertices[i]);
						num = i;
					}
				}, 0f);
			}
			return result;
		}

		public static void GetContours(INavmesh navmesh, Action<List<Int3>, bool> results)
		{
			bool[] uses = new bool[3];
			Dictionary<int, int> outline = new Dictionary<int, int>();
			Dictionary<int, Int3> vertexPositions = new Dictionary<int, Int3>();
			HashSet<int> hasInEdge = new HashSet<int>();
			navmesh.GetNodes(delegate(GraphNode _node)
			{
				TriangleMeshNode triangleMeshNode = _node as TriangleMeshNode;
				bool[] array = uses;
				bool flag;
				uses[1] = (flag = (uses[2] = false));
				array[0] = flag;
				if (triangleMeshNode != null)
				{
					for (int j = 0; j < triangleMeshNode.connections.Length; j++)
					{
						TriangleMeshNode triangleMeshNode2 = triangleMeshNode.connections[j].node as TriangleMeshNode;
						if (triangleMeshNode2 != null)
						{
							int num = triangleMeshNode.SharedEdge(triangleMeshNode2);
							if (num != -1)
							{
								uses[num] = true;
							}
						}
					}
					for (int k = 0; k < 3; k++)
					{
						if (!uses[k])
						{
							int i2 = k;
							int i3 = (k + 1) % triangleMeshNode.GetVertexCount();
							outline[triangleMeshNode.GetVertexIndex(i2)] = triangleMeshNode.GetVertexIndex(i3);
							hasInEdge.Add(triangleMeshNode.GetVertexIndex(i3));
							vertexPositions[triangleMeshNode.GetVertexIndex(i2)] = triangleMeshNode.GetVertex(i2);
							vertexPositions[triangleMeshNode.GetVertexIndex(i3)] = triangleMeshNode.GetVertex(i3);
						}
					}
				}
			});
			Polygon.TraceContours(outline, hasInEdge, delegate(List<int> chain, bool cycle)
			{
				List<Int3> list = ListPool<Int3>.Claim();
				for (int i = 0; i < chain.Count; i++)
				{
					list.Add(vertexPositions[chain[i]]);
				}
				results(list, cycle);
			});
		}

		public static void GetContours(GridGraph grid, Action<Vector3[]> callback, float yMergeThreshold, GridNodeBase[] nodes = null)
		{
			HashSet<GridNodeBase> hashSet = ((nodes == null) ? null : new HashSet<GridNodeBase>(nodes));
			if (grid is LayerGridGraph)
			{
				nodes = nodes ?? (grid as LayerGridGraph).nodes;
			}
			nodes = nodes ?? grid.nodes;
			int[] neighbourXOffsets = grid.neighbourXOffsets;
			int[] neighbourZOffsets = grid.neighbourZOffsets;
			int[] array = ((grid.neighbours == NumNeighbours.Six) ? GridGraph.hexagonNeighbourIndices : new int[4] { 0, 1, 2, 3 });
			float num = ((grid.neighbours != NumNeighbours.Six) ? 0.5f : (1f / 3f));
			if (nodes == null)
			{
				return;
			}
			List<Vector3> list = ListPool<Vector3>.Claim();
			HashSet<int> hashSet2 = new HashSet<int>();
			foreach (GridNodeBase gridNodeBase in nodes)
			{
				if (gridNodeBase == null || !gridNodeBase.Walkable || (gridNodeBase.HasConnectionsToAllEightNeighbours && hashSet == null))
				{
					continue;
				}
				for (int j = 0; j < array.Length; j++)
				{
					int num2 = (gridNodeBase.NodeIndex << 4) | j;
					GridNodeBase neighbourAlongDirection = gridNodeBase.GetNeighbourAlongDirection(array[j]);
					if ((neighbourAlongDirection != null && (hashSet == null || hashSet.Contains(neighbourAlongDirection))) || hashSet2.Contains(num2))
					{
						continue;
					}
					list.ClearFast();
					int num3 = j;
					GridNodeBase gridNodeBase2 = gridNodeBase;
					while (true)
					{
						int num4 = (gridNodeBase2.NodeIndex << 4) | num3;
						if (num4 == num2 && list.Count > 0)
						{
							break;
						}
						hashSet2.Add(num4);
						GridNodeBase neighbourAlongDirection2 = gridNodeBase2.GetNeighbourAlongDirection(array[num3]);
						if (neighbourAlongDirection2 == null || (hashSet != null && !hashSet.Contains(neighbourAlongDirection2)))
						{
							int num5 = array[num3];
							num3 = (num3 + 1) % array.Length;
							int num6 = array[num3];
							Vector3 vector = new Vector3((float)gridNodeBase2.XCoordinateInGrid + 0.5f, 0f, (float)gridNodeBase2.ZCoordinateInGrid + 0.5f);
							vector.x += (float)(neighbourXOffsets[num5] + neighbourXOffsets[num6]) * num;
							vector.z += (float)(neighbourZOffsets[num5] + neighbourZOffsets[num6]) * num;
							vector.y = grid.transform.InverseTransform((Vector3)gridNodeBase2.position).y;
							if (list.Count >= 2)
							{
								Vector3 vector2 = list[list.Count - 2];
								Vector3 vector3 = list[list.Count - 1];
								Vector3 vector4 = vector3 - vector2;
								Vector3 vector5 = vector - vector2;
								if (((Mathf.Abs(vector4.x) > 0.01f || Mathf.Abs(vector5.x) > 0.01f) && (Mathf.Abs(vector4.z) > 0.01f || Mathf.Abs(vector5.z) > 0.01f)) || Mathf.Abs(vector4.y) > yMergeThreshold || Mathf.Abs(vector5.y) > yMergeThreshold)
								{
									list.Add(vector);
								}
								else
								{
									list[list.Count - 1] = vector;
								}
							}
							else
							{
								list.Add(vector);
							}
						}
						else
						{
							gridNodeBase2 = neighbourAlongDirection2;
							num3 = (num3 + array.Length / 2 + 1) % array.Length;
						}
					}
					Vector3[] array2 = list.ToArray();
					grid.transform.Transform(array2);
					callback(array2);
				}
			}
			ListPool<Vector3>.Release(ref list);
		}
	}
}
