using UnityEngine;

namespace Pathfinding
{
	public class RichSpecial : RichPathPart
	{
		public NodeLink2 nodeLink;

		public Transform first;

		public Transform second;

		public bool reverse;

		public override void OnEnterPool()
		{
			nodeLink = null;
		}

		public RichSpecial Initialize(NodeLink2 nodeLink, GraphNode first)
		{
			this.nodeLink = nodeLink;
			if (first == nodeLink.startNode)
			{
				this.first = nodeLink.StartTransform;
				second = nodeLink.EndTransform;
				reverse = false;
			}
			else
			{
				this.first = nodeLink.EndTransform;
				second = nodeLink.StartTransform;
				reverse = true;
			}
			return this;
		}
	}
}
