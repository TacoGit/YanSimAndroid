using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Link")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link.php")]
	public class NodeLink : GraphModifier
	{
		public Transform end;

		public float costFactor = 1f;

		public bool oneWay;

		public bool deleteConnection;

		public Transform Start
		{
			get
			{
				return base.transform;
			}
		}

		public Transform End
		{
			get
			{
				return end;
			}
		}

		public override void OnPostScan()
		{
			if (AstarPath.active.isScanning)
			{
				InternalOnPostScan();
				return;
			}
			AstarPath.active.AddWorkItem(new AstarWorkItem((Func<bool, bool>)delegate
			{
				InternalOnPostScan();
				return true;
			}));
		}

		public void InternalOnPostScan()
		{
			Apply();
		}

		public override void OnGraphsPostUpdate()
		{
			if (!AstarPath.active.isScanning)
			{
				AstarPath.active.AddWorkItem(new AstarWorkItem((Func<bool, bool>)delegate
				{
					InternalOnPostScan();
					return true;
				}));
			}
		}

		public virtual void Apply()
		{
			if (Start == null || End == null || AstarPath.active == null)
			{
				return;
			}
			GraphNode node = AstarPath.active.GetNearest(Start.position).node;
			GraphNode node2 = AstarPath.active.GetNearest(End.position).node;
			if (node == null || node2 == null)
			{
				return;
			}
			if (deleteConnection)
			{
				node.RemoveConnection(node2);
				if (!oneWay)
				{
					node2.RemoveConnection(node);
				}
				return;
			}
			uint cost = (uint)Math.Round((float)(node.position - node2.position).costMagnitude * costFactor);
			node.AddConnection(node2, cost);
			if (!oneWay)
			{
				node2.AddConnection(node, cost);
			}
		}

		public void OnDrawGizmos()
		{
			if (!(Start == null) && !(End == null))
			{
				Draw.Gizmos.Bezier(Start.position, End.position, (!deleteConnection) ? Color.green : Color.red);
			}
		}
	}
}
