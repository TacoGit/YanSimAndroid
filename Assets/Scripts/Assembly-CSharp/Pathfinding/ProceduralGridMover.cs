using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_procedural_grid_mover.php")]
	public class ProceduralGridMover : VersionedMonoBehaviour
	{
		public float updateDistance = 10f;

		public Transform target;

		public bool floodFill = true;

		private GridGraph graph;

		private GridNodeBase[] buffer;

		public bool updatingGraph { get; private set; }

		private void Start()
		{
			if (AstarPath.active == null)
			{
				throw new Exception("There is no AstarPath object in the scene");
			}
			graph = AstarPath.active.data.FindGraphWhichInheritsFrom(typeof(GridGraph)) as GridGraph;
			if (graph == null)
			{
				throw new Exception("The AstarPath object has no GridGraph or LayeredGridGraph");
			}
			UpdateGraph();
		}

		private void Update()
		{
			if (graph != null)
			{
				Vector3 a = PointToGraphSpace(graph.center);
				Vector3 b = PointToGraphSpace(target.position);
				if (VectorMath.SqrDistanceXZ(a, b) > updateDistance * updateDistance)
				{
					UpdateGraph();
				}
			}
		}

		private Vector3 PointToGraphSpace(Vector3 p)
		{
			return graph.transform.InverseTransform(p);
		}

		public void UpdateGraph()
		{
			if (updatingGraph)
			{
				return;
			}
			updatingGraph = true;
			IEnumerator ie = UpdateGraphCoroutine();
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext context, bool force)
			{
				if (floodFill)
				{
					context.QueueFloodFill();
				}
				if (force)
				{
					while (ie.MoveNext())
					{
					}
				}
				bool flag;
				try
				{
					flag = !ie.MoveNext();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, this);
					flag = true;
				}
				if (flag)
				{
					updatingGraph = false;
				}
				return flag;
			}));
		}

		private IEnumerator UpdateGraphCoroutine()
		{
			Vector3 dir = PointToGraphSpace(target.position) - PointToGraphSpace(graph.center);
			dir.x = Mathf.Round(dir.x);
			dir.z = Mathf.Round(dir.z);
			dir.y = 0f;
			if (dir == Vector3.zero)
			{
				yield break;
			}
			Int2 offset = new Int2(-Mathf.RoundToInt(dir.x), -Mathf.RoundToInt(dir.z));
			graph.center += graph.transform.TransformVector(dir);
			graph.UpdateTransform();
			int width = graph.width;
			int depth = graph.depth;
			int layers = graph.LayerCount;
			LayerGridGraph layeredGraph = graph as LayerGridGraph;
			GridNodeBase[] nodes = ((layeredGraph == null) ? ((GridNodeBase[])graph.nodes) : ((GridNodeBase[])layeredGraph.nodes));
			if (buffer == null || buffer.Length != width * depth)
			{
				buffer = new GridNodeBase[width * depth];
			}
			if (Mathf.Abs(offset.x) <= width && Mathf.Abs(offset.y) <= depth)
			{
				IntRect recalculateRect = new IntRect(0, 0, offset.x, offset.y);
				if (recalculateRect.xmin > recalculateRect.xmax)
				{
					int xmax3 = recalculateRect.xmax;
					recalculateRect.xmax = width + recalculateRect.xmin;
					recalculateRect.xmin = width + xmax3;
				}
				if (recalculateRect.ymin > recalculateRect.ymax)
				{
					int ymax = recalculateRect.ymax;
					recalculateRect.ymax = depth + recalculateRect.ymin;
					recalculateRect.ymin = depth + ymax;
				}
				IntRect connectionRect2 = recalculateRect.Expand(1);
				connectionRect2 = IntRect.Intersection(connectionRect2, new IntRect(0, 0, width, depth));
				for (int i = 0; i < layers; i++)
				{
					int layerOffset = i * width * depth;
					for (int j = 0; j < depth; j++)
					{
						int num = j * width;
						int num2 = (j + offset.y + depth) % depth * width;
						for (int k = 0; k < width; k++)
						{
							buffer[num2 + (k + offset.x + width) % width] = nodes[layerOffset + num + k];
						}
					}
					yield return null;
					for (int l = 0; l < depth; l++)
					{
						int num3 = l * width;
						for (int m = 0; m < width; m++)
						{
							int num4 = num3 + m;
							GridNodeBase gridNodeBase = buffer[num4];
							if (gridNodeBase != null)
							{
								gridNodeBase.NodeInGridIndex = num4;
							}
							nodes[layerOffset + num4] = gridNodeBase;
						}
						int num5;
						int num6;
						if (l >= recalculateRect.ymin && l < recalculateRect.ymax)
						{
							num5 = 0;
							num6 = depth;
						}
						else
						{
							num5 = recalculateRect.xmin;
							num6 = recalculateRect.xmax;
						}
						for (int n = num5; n < num6; n++)
						{
							GridNodeBase gridNodeBase2 = buffer[num3 + n];
							if (gridNodeBase2 != null)
							{
								gridNodeBase2.ClearConnections(false);
							}
						}
					}
					yield return null;
				}
				int yieldEvery3 = 1000;
				int approxNumNodesToUpdate = Mathf.Max(Mathf.Abs(offset.x), Mathf.Abs(offset.y)) * Mathf.Max(width, depth);
				yieldEvery3 = Mathf.Max(yieldEvery3, approxNumNodesToUpdate / 10);
				int counter2 = 0;
				for (int z4 = 0; z4 < depth; z4++)
				{
					int xmin;
					int xmax;
					if (z4 >= recalculateRect.ymin && z4 < recalculateRect.ymax)
					{
						xmin = 0;
						xmax = width;
					}
					else
					{
						xmin = recalculateRect.xmin;
						xmax = recalculateRect.xmax;
					}
					for (int num7 = xmin; num7 < xmax; num7++)
					{
						graph.RecalculateCell(num7, z4, false, false);
					}
					counter2 += xmax - xmin;
					if (counter2 > yieldEvery3)
					{
						counter2 = 0;
						yield return null;
					}
				}
				for (int z3 = 0; z3 < depth; z3++)
				{
					int xmin2;
					int xmax2;
					if (z3 >= connectionRect2.ymin && z3 < connectionRect2.ymax)
					{
						xmin2 = 0;
						xmax2 = width;
					}
					else
					{
						xmin2 = connectionRect2.xmin;
						xmax2 = connectionRect2.xmax;
					}
					for (int num8 = xmin2; num8 < xmax2; num8++)
					{
						graph.CalculateConnections(num8, z3);
					}
					counter2 += xmax2 - xmin2;
					if (counter2 > yieldEvery3)
					{
						counter2 = 0;
						yield return null;
					}
				}
				yield return null;
				for (int num9 = 0; num9 < depth; num9++)
				{
					for (int num10 = 0; num10 < width; num10++)
					{
						if (num10 == 0 || num9 == 0 || num10 == width - 1 || num9 == depth - 1)
						{
							graph.CalculateConnections(num10, num9);
						}
					}
				}
				if (!floodFill)
				{
					graph.GetNodes(delegate(GraphNode node)
					{
						node.Area = 1u;
					});
				}
				yield break;
			}
			int yieldEvery = Mathf.Max(depth * width / 20, 1000);
			int counter = 0;
			for (int z2 = 0; z2 < depth; z2++)
			{
				for (int num11 = 0; num11 < width; num11++)
				{
					graph.RecalculateCell(num11, z2);
				}
				counter += width;
				if (counter > yieldEvery)
				{
					counter = 0;
					yield return null;
				}
			}
			for (int z = 0; z < depth; z++)
			{
				for (int num12 = 0; num12 < width; num12++)
				{
					graph.CalculateConnections(num12, z);
				}
				counter += width;
				if (counter > yieldEvery)
				{
					counter = 0;
					yield return null;
				}
			}
		}
	}
}
