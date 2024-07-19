using System;
using System.Text;

namespace Pathfinding
{
	public class PathHandler
	{
		private ushort pathID;

		public readonly int threadID;

		public readonly int totalThreadCount;

		public readonly BinaryHeap heap = new BinaryHeap(128);

		public PathNode[] nodes = new PathNode[0];

		public readonly StringBuilder DebugStringBuilder = new StringBuilder();

		public ushort PathID
		{
			get
			{
				return pathID;
			}
		}

		public PathHandler(int threadID, int totalThreadCount)
		{
			this.threadID = threadID;
			this.totalThreadCount = totalThreadCount;
		}

		public void InitializeForPath(Path p)
		{
			pathID = p.pathID;
			heap.Clear();
		}

		public void DestroyNode(GraphNode node)
		{
			PathNode pathNode = GetPathNode(node);
			pathNode.node = null;
			pathNode.parent = null;
			pathNode.pathID = 0;
			pathNode.G = 0u;
			pathNode.H = 0u;
		}

		public void InitializeNode(GraphNode node)
		{
			int nodeIndex = node.NodeIndex;
			if (nodeIndex >= nodes.Length)
			{
				PathNode[] array = new PathNode[Math.Max(128, nodes.Length * 2)];
				nodes.CopyTo(array, 0);
				for (int i = nodes.Length; i < array.Length; i++)
				{
					array[i] = new PathNode();
				}
				nodes = array;
			}
			nodes[nodeIndex].node = node;
		}

		public PathNode GetPathNode(int nodeIndex)
		{
			return nodes[nodeIndex];
		}

		public PathNode GetPathNode(GraphNode node)
		{
			return nodes[node.NodeIndex];
		}

		public void ClearPathIDs()
		{
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] != null)
				{
					nodes[i].pathID = 0;
				}
			}
		}
	}
}
