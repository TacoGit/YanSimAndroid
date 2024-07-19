using UnityEngine;

namespace Pathfinding
{
	public struct NNInfoInternal
	{
		public GraphNode node;

		public GraphNode constrainedNode;

		public Vector3 clampedPosition;

		public Vector3 constClampedPosition;

		public NNInfoInternal(GraphNode node)
		{
			this.node = node;
			constrainedNode = null;
			clampedPosition = Vector3.zero;
			constClampedPosition = Vector3.zero;
			UpdateInfo();
		}

		public void UpdateInfo()
		{
			clampedPosition = ((node == null) ? Vector3.zero : ((Vector3)node.position));
			constClampedPosition = ((constrainedNode == null) ? Vector3.zero : ((Vector3)constrainedNode.position));
		}
	}
}
