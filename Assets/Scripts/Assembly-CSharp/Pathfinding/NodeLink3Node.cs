using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class NodeLink3Node : PointNode
	{
		public NodeLink3 link;

		public Vector3 portalA;

		public Vector3 portalB;

		public NodeLink3Node(AstarPath active)
			: base(active)
		{
		}

		public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			if (connections.Length < 2)
			{
				return false;
			}
			if (connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + connections.Length);
			}
			if (left != null)
			{
				left.Add(portalA);
				right.Add(portalB);
			}
			return true;
		}

		public GraphNode GetOther(GraphNode a)
		{
			if (connections.Length < 2)
			{
				return null;
			}
			if (connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + connections.Length);
			}
			return (a != connections[0].node) ? (connections[0].node as NodeLink3Node).GetOtherInternal(this) : (connections[1].node as NodeLink3Node).GetOtherInternal(this);
		}

		private GraphNode GetOtherInternal(GraphNode a)
		{
			if (connections.Length < 2)
			{
				return null;
			}
			return (a != connections[0].node) ? connections[0].node : connections[1].node;
		}
	}
}
