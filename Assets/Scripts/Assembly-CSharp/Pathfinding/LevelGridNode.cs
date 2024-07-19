using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class LevelGridNode : GridNodeBase
	{
		private static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];

		public ulong gridConnections;

		protected static LayerGridGraph[] gridGraphs;

		public const int NoConnection = 255;

		public const int ConnectionMask = 255;

		private const int ConnectionStride = 8;

		public const int MaxLayerCount = 255;

		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				return false;
			}
		}

		public int LayerCoordinateInGrid
		{
			get
			{
				return nodeInGridIndex >> 24;
			}
			set
			{
				nodeInGridIndex = (nodeInGridIndex & 0xFFFFFF) | (value << 24);
			}
		}

		public LevelGridNode(AstarPath astar)
			: base(astar)
		{
		}

		public static LayerGridGraph GetGridGraph(uint graphIndex)
		{
			return _gridGraphs[graphIndex];
		}

		public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
		{
			GridNode.SetGridGraph(graphIndex, graph);
			if (_gridGraphs.Length <= graphIndex)
			{
				LayerGridGraph[] array = new LayerGridGraph[graphIndex + 1];
				for (int i = 0; i < _gridGraphs.Length; i++)
				{
					array[i] = _gridGraphs[i];
				}
				_gridGraphs = array;
			}
			_gridGraphs[graphIndex] = graph;
		}

		public void ResetAllGridConnections()
		{
			gridConnections = ulong.MaxValue;
		}

		public bool HasAnyGridConnections()
		{
			return gridConnections != ulong.MaxValue;
		}

		public void SetPosition(Int3 position)
		{
			base.position = position;
		}

		public override int GetGizmoHashCode()
		{
			return base.GetGizmoHashCode() ^ (int)(805306457 * gridConnections);
		}

		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			if (GetConnection(direction))
			{
				LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * GetConnectionValue(direction)];
			}
			return null;
		}

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
				int[] neighbourOffsets = gridGraph.neighbourOffsets;
				LevelGridNode[] nodes = gridGraph.nodes;
				for (int i = 0; i < 4; i++)
				{
					int connectionValue = GetConnectionValue(i);
					if (connectionValue != 255)
					{
						LevelGridNode levelGridNode = nodes[base.NodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
						if (levelGridNode != null)
						{
							levelGridNode.SetConnectionValue((i + 2) % 4, 255);
						}
					}
				}
			}
			ResetAllGridConnections();
		}

		public override void GetConnections(Action<GraphNode> action)
		{
			LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			LevelGridNode[] nodes = gridGraph.nodes;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = GetConnectionValue(i);
				if (connectionValue != 255)
				{
					LevelGridNode levelGridNode = nodes[num + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (levelGridNode != null)
					{
						action(levelGridNode);
					}
				}
			}
		}

		public override void FloodFill(Stack<GraphNode> stack, uint region)
		{
			int num = base.NodeInGridIndex;
			LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			LevelGridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = GetConnectionValue(i);
				if (connectionValue != 255)
				{
					LevelGridNode levelGridNode = nodes[num + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (levelGridNode != null && levelGridNode.Area != region)
					{
						levelGridNode.Area = region;
						stack.Push(levelGridNode);
					}
				}
			}
		}

		public bool GetConnection(int i)
		{
			return ((gridConnections >> i * 8) & 0xFF) != 255;
		}

		public void SetConnectionValue(int dir, int value)
		{
			gridConnections = (gridConnections & (ulong)(~(255L << dir * 8))) | (ulong)((long)value << dir * 8);
		}

		public int GetConnectionValue(int dir)
		{
			return (int)((gridConnections >> dir * 8) & 0xFF);
		}

		public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			if (backwards)
			{
				return true;
			}
			LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			LevelGridNode[] nodes = gridGraph.nodes;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = GetConnectionValue(i);
				if (connectionValue != 255 && other == nodes[num + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue])
				{
					Vector3 vector = (Vector3)(position + other.position) * 0.5f;
					Vector3 vector2 = Vector3.Cross(gridGraph.collision.up, (Vector3)(other.position - position));
					vector2.Normalize();
					vector2 *= gridGraph.nodeSize * 0.5f;
					left.Add(vector - vector2);
					right.Add(vector + vector2);
					return true;
				}
			}
			return false;
		}

		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			handler.heap.Add(pathNode);
			pathNode.UpdateG(path);
			LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			LevelGridNode[] nodes = gridGraph.nodes;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = GetConnectionValue(i);
				if (connectionValue != 255)
				{
					LevelGridNode levelGridNode = nodes[num + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					PathNode pathNode2 = handler.GetPathNode(levelGridNode);
					if (pathNode2 != null && pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
					{
						levelGridNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
		}

		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			LevelGridNode[] nodes = gridGraph.nodes;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = GetConnectionValue(i);
				if (connectionValue == 255)
				{
					continue;
				}
				GraphNode graphNode = nodes[num + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
				if (!path.CanTraverse(graphNode))
				{
					continue;
				}
				PathNode pathNode2 = handler.GetPathNode(graphNode);
				if (pathNode2.pathID != handler.PathID)
				{
					pathNode2.parent = pathNode;
					pathNode2.pathID = handler.PathID;
					pathNode2.cost = neighbourCosts[i];
					pathNode2.H = path.CalculateHScore(graphNode);
					pathNode2.UpdateG(path);
					handler.heap.Add(pathNode2);
				}
				else
				{
					uint num2 = neighbourCosts[i];
					if (pathNode.G + num2 + path.GetTraversalCost(graphNode) < pathNode2.G)
					{
						pathNode2.cost = num2;
						pathNode2.parent = pathNode;
						graphNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
		}

		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(position);
			ctx.writer.Write(gridFlags);
			ctx.writer.Write(gridConnections);
		}

		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			position = ctx.DeserializeInt3();
			gridFlags = ctx.reader.ReadUInt16();
			if (ctx.meta.version < AstarSerializer.V3_9_0)
			{
				gridConnections = ctx.reader.ReadUInt32() | 0xFFFFFFFF00000000uL;
			}
			else
			{
				gridConnections = ctx.reader.ReadUInt64();
			}
		}
	}
}
