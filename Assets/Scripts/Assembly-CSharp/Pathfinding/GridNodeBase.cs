using System;
using UnityEngine;

namespace Pathfinding
{
	public abstract class GridNodeBase : GraphNode
	{
		private const int GridFlagsWalkableErosionOffset = 8;

		private const int GridFlagsWalkableErosionMask = 256;

		private const int GridFlagsWalkableTmpOffset = 9;

		private const int GridFlagsWalkableTmpMask = 512;

		protected const int NodeInGridIndexLayerOffset = 24;

		protected const int NodeInGridIndexMask = 16777215;

		protected int nodeInGridIndex;

		protected ushort gridFlags;

		public int NodeInGridIndex
		{
			get
			{
				return nodeInGridIndex & 0xFFFFFF;
			}
			set
			{
				nodeInGridIndex = (nodeInGridIndex & -16777216) | value;
			}
		}

		public int XCoordinateInGrid
		{
			get
			{
				return NodeInGridIndex % GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		public int ZCoordinateInGrid
		{
			get
			{
				return NodeInGridIndex / GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		public bool WalkableErosion
		{
			get
			{
				return (gridFlags & 0x100) != 0;
			}
			set
			{
				gridFlags = (ushort)((gridFlags & 0xFFFFFEFFu) | (value ? 256u : 0u));
			}
		}

		public bool TmpWalkable
		{
			get
			{
				return (gridFlags & 0x200) != 0;
			}
			set
			{
				gridFlags = (ushort)((gridFlags & 0xFFFFFDFFu) | (value ? 512u : 0u));
			}
		}

		public abstract bool HasConnectionsToAllEightNeighbours { get; }

		protected GridNodeBase(AstarPath astar)
			: base(astar)
		{
		}

		public override float SurfaceArea()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return gridGraph.nodeSize * gridGraph.nodeSize;
		}

		public override Vector3 RandomPointOnSurface()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 vector = gridGraph.transform.InverseTransform((Vector3)position);
			return gridGraph.transform.Transform(vector + new Vector3(UnityEngine.Random.value - 0.5f, 0f, UnityEngine.Random.value - 0.5f));
		}

		public override int GetGizmoHashCode()
		{
			int gizmoHashCode = base.GetGizmoHashCode();
			return gizmoHashCode ^ (109 * gridFlags);
		}

		public abstract GridNodeBase GetNeighbourAlongDirection(int direction);

		public override bool ContainsConnection(GraphNode node)
		{
			for (int i = 0; i < 8; i++)
			{
				if (node == GetNeighbourAlongDirection(i))
				{
					return true;
				}
			}
			return false;
		}

		public override void AddConnection(GraphNode node, uint cost)
		{
			throw new NotImplementedException("GridNodes do not have support for adding manual connections with your current settings.\nPlease disable ASTAR_GRID_NO_CUSTOM_CONNECTIONS in the Optimizations tab in the A* Inspector");
		}

		public override void RemoveConnection(GraphNode node)
		{
			throw new NotImplementedException("GridNodes do not have support for adding manual connections with your current settings.\nPlease disable ASTAR_GRID_NO_CUSTOM_CONNECTIONS in the Optimizations tab in the A* Inspector");
		}

		public void ClearCustomConnections(bool alsoReverse)
		{
		}
	}
}
