using System;
using Pathfinding.Util;

namespace Pathfinding
{
	public class NavmeshTile : INavmeshHolder, ITransformedGraph, INavmesh
	{
		public int[] tris;

		public Int3[] verts;

		public Int3[] vertsInGraphSpace;

		public int x;

		public int z;

		public int w;

		public int d;

		public TriangleMeshNode[] nodes;

		public BBTree bbTree;

		public bool flag;

		public NavmeshBase graph;

		public GraphTransform transform
		{
			get
			{
				return graph.transform;
			}
		}

		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			x = this.x;
			z = this.z;
		}

		public int GetVertexArrayIndex(int index)
		{
			return index & 0xFFF;
		}

		public Int3 GetVertex(int index)
		{
			int num = index & 0xFFF;
			return verts[num];
		}

		public Int3 GetVertexInGraphSpace(int index)
		{
			return vertsInGraphSpace[index & 0xFFF];
		}

		public void GetNodes(Action<GraphNode> action)
		{
			if (nodes != null)
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					action(nodes[i]);
				}
			}
		}
	}
}
