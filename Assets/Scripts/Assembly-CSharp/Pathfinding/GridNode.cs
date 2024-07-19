using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class GridNode : GridNodeBase
	{
		private static GridGraph[] _gridGraphs = new GridGraph[0];

		private const int GridFlagsConnectionOffset = 0;

		private const int GridFlagsConnectionBit0 = 1;

		private const int GridFlagsConnectionMask = 255;

		private const int GridFlagsEdgeNodeOffset = 10;

		private const int GridFlagsEdgeNodeMask = 1024;

		internal ushort InternalGridFlags
		{
			get
			{
				return gridFlags;
			}
			set
			{
				gridFlags = value;
			}
		}

		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				return (InternalGridFlags & 0xFF) == 255;
			}
		}

		public bool EdgeNode
		{
			get
			{
				return (gridFlags & 0x400) != 0;
			}
			set
			{
				gridFlags = (ushort)((gridFlags & 0xFFFFFBFFu) | (value ? 1024u : 0u));
			}
		}

		public GridNode(AstarPath astar)
			: base(astar)
		{
		}

		public static GridGraph GetGridGraph(uint graphIndex)
		{
			return _gridGraphs[graphIndex];
		}

		public static void SetGridGraph(int graphIndex, GridGraph graph)
		{
			if (_gridGraphs.Length <= graphIndex)
			{
				GridGraph[] array = new GridGraph[graphIndex + 1];
				for (int i = 0; i < _gridGraphs.Length; i++)
				{
					array[i] = _gridGraphs[i];
				}
				_gridGraphs = array;
			}
			_gridGraphs[graphIndex] = graph;
		}

		public bool HasConnectionInDirection(int dir)
		{
			return ((gridFlags >> dir) & 1) != 0;
		}

		[Obsolete("Use HasConnectionInDirection")]
		public bool GetConnectionInternal(int dir)
		{
			return HasConnectionInDirection(dir);
		}

		public void SetConnectionInternal(int dir, bool value)
		{
			gridFlags = (ushort)((gridFlags & ~(1 << dir)) | ((value ? 1 : 0) << 0 << dir));
		}

		public void SetAllConnectionInternal(int connections)
		{
			gridFlags = (ushort)((gridFlags & 0xFFFFFF00u) | (uint)(connections << 0));
		}

		public void ResetConnectionsInternal()
		{
			gridFlags = (ushort)(gridFlags & 0xFFFFFF00u);
		}

		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			if (HasConnectionInDirection(direction))
			{
				GridGraph gridGraph = GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction]];
			}
			return null;
		}

		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				for (int i = 0; i < 8; i++)
				{
					GridNode gridNode = GetNeighbourAlongDirection(i) as GridNode;
					if (gridNode != null)
					{
						gridNode.SetConnectionInternal((i >= 4) ? ((i - 2) % 4 + 4) : ((i + 2) % 4), false);
					}
				}
			}
			ResetConnectionsInternal();
		}

		public override void GetConnections(Action<GraphNode> action)
		{
			GridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (HasConnectionInDirection(i))
				{
					GridNode gridNode = nodes[base.NodeInGridIndex + neighbourOffsets[i]];
					if (gridNode != null)
					{
						action(gridNode);
					}
				}
			}
		}

		public Vector3 ClosestPointOnNode(Vector3 p)
		{
			GridGraph gridGraph = GetGridGraph(base.GraphIndex);
			p = gridGraph.transform.InverseTransform(p);
			int num = base.NodeInGridIndex % gridGraph.width;
			int num2 = base.NodeInGridIndex / gridGraph.width;
			float y = gridGraph.transform.InverseTransform((Vector3)position).y;
			Vector3 point = new Vector3(Mathf.Clamp(p.x, num, (float)num + 1f), y, Mathf.Clamp(p.z, num2, (float)num2 + 1f));
			return gridGraph.transform.Transform(point);
		}

		public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			if (backwards)
			{
				return true;
			}
			GridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNode[] nodes = gridGraph.nodes;
			for (int i = 0; i < 4; i++)
			{
				if (HasConnectionInDirection(i) && other == nodes[base.NodeInGridIndex + neighbourOffsets[i]])
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
			for (int j = 4; j < 8; j++)
			{
				if (!HasConnectionInDirection(j) || other != nodes[base.NodeInGridIndex + neighbourOffsets[j]])
				{
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				if (HasConnectionInDirection(j - 4))
				{
					GridNode gridNode = nodes[base.NodeInGridIndex + neighbourOffsets[j - 4]];
					if (gridNode.Walkable && gridNode.HasConnectionInDirection((j - 4 + 1) % 4))
					{
						flag = true;
					}
				}
				if (HasConnectionInDirection((j - 4 + 1) % 4))
				{
					GridNode gridNode2 = nodes[base.NodeInGridIndex + neighbourOffsets[(j - 4 + 1) % 4]];
					if (gridNode2.Walkable && gridNode2.HasConnectionInDirection(j - 4))
					{
						flag2 = true;
					}
				}
				Vector3 vector3 = (Vector3)(position + other.position) * 0.5f;
				Vector3 vector4 = Vector3.Cross(gridGraph.collision.up, (Vector3)(other.position - position));
				vector4.Normalize();
				vector4 *= gridGraph.nodeSize * 1.4142f;
				left.Add(vector3 - ((!flag2) ? Vector3.zero : vector4));
				right.Add(vector3 + ((!flag) ? Vector3.zero : vector4));
				return true;
			}
			return false;
		}

		public override void FloodFill(Stack<GraphNode> stack, uint region)
		{
			GridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNode[] nodes = gridGraph.nodes;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				if (HasConnectionInDirection(i))
				{
					GridNode gridNode = nodes[num + neighbourOffsets[i]];
					if (gridNode != null && gridNode.Area != region)
					{
						gridNode.Area = region;
						stack.Push(gridNode);
					}
				}
			}
		}

		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			GridGraph gridGraph = GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNode[] nodes = gridGraph.nodes;
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
			ushort pathID = handler.PathID;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				if (HasConnectionInDirection(i))
				{
					GridNode gridNode = nodes[num + neighbourOffsets[i]];
					PathNode pathNode2 = handler.GetPathNode(gridNode);
					if (pathNode2.parent == pathNode && pathNode2.pathID == pathID)
					{
						gridNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
		}

		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			GridGraph gridGraph = GetGridGraph(base.GraphIndex);
			ushort pathID = handler.PathID;
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			GridNode[] nodes = gridGraph.nodes;
			int num = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				if (!HasConnectionInDirection(i))
				{
					continue;
				}
				GridNode gridNode = nodes[num + neighbourOffsets[i]];
				if (path.CanTraverse(gridNode))
				{
					PathNode pathNode2 = handler.GetPathNode(gridNode);
					uint num2 = neighbourCosts[i];
					if (pathNode2.pathID != pathID)
					{
						pathNode2.parent = pathNode;
						pathNode2.pathID = pathID;
						pathNode2.cost = num2;
						pathNode2.H = path.CalculateHScore(gridNode);
						pathNode2.UpdateG(path);
						handler.heap.Add(pathNode2);
					}
					else if (pathNode.G + num2 + path.GetTraversalCost(gridNode) < pathNode2.G)
					{
						pathNode2.cost = num2;
						pathNode2.parent = pathNode;
						gridNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
		}

		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(position);
			ctx.writer.Write(gridFlags);
		}

		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			position = ctx.DeserializeInt3();
			gridFlags = ctx.reader.ReadUInt16();
		}
	}
}
