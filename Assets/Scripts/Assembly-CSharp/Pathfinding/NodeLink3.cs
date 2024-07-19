using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Link3")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link3.php")]
	public class NodeLink3 : GraphModifier
	{
		protected static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();

		public Transform end;

		public float costFactor = 1f;

		public bool oneWay;

		private NodeLink3Node startNode;

		private NodeLink3Node endNode;

		private MeshNode connectedNode1;

		private MeshNode connectedNode2;

		private Vector3 clamped1;

		private Vector3 clamped2;

		private bool postScanCalled;

		private static readonly Color GizmosColor = new Color(0.80784315f, 8f / 15f, 16f / 85f, 0.5f);

		private static readonly Color GizmosColorSelected = new Color(47f / 51f, 41f / 85f, 0.1254902f, 1f);

		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		public Transform EndTransform
		{
			get
			{
				return end;
			}
		}

		public GraphNode StartNode
		{
			get
			{
				return startNode;
			}
		}

		public GraphNode EndNode
		{
			get
			{
				return endNode;
			}
		}

		public static NodeLink3 GetNodeLink(GraphNode node)
		{
			NodeLink3 value;
			reference.TryGetValue(node, out value);
			return value;
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
			if (AstarPath.active.data.pointGraph == null)
			{
				AstarPath.active.data.AddGraph(typeof(PointGraph));
			}
			startNode = AstarPath.active.data.pointGraph.AddNode(new NodeLink3Node(AstarPath.active), (Int3)StartTransform.position);
			startNode.link = this;
			endNode = AstarPath.active.data.pointGraph.AddNode(new NodeLink3Node(AstarPath.active), (Int3)EndTransform.position);
			endNode.link = this;
			connectedNode1 = null;
			connectedNode2 = null;
			if (startNode == null || endNode == null)
			{
				startNode = null;
				endNode = null;
				return;
			}
			postScanCalled = true;
			reference[startNode] = this;
			reference[endNode] = this;
			Apply(true);
		}

		public override void OnGraphsPostUpdate()
		{
			if (!AstarPath.active.isScanning)
			{
				if (connectedNode1 != null && connectedNode1.Destroyed)
				{
					connectedNode1 = null;
				}
				if (connectedNode2 != null && connectedNode2.Destroyed)
				{
					connectedNode2 = null;
				}
				if (!postScanCalled)
				{
					OnPostScan();
				}
				else
				{
					Apply(false);
				}
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && AstarPath.active != null && AstarPath.active.data != null && AstarPath.active.data.pointGraph != null)
			{
				OnGraphsPostUpdate();
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			postScanCalled = false;
			if (startNode != null)
			{
				reference.Remove(startNode);
			}
			if (endNode != null)
			{
				reference.Remove(endNode);
			}
			if (startNode != null && endNode != null)
			{
				startNode.RemoveConnection(endNode);
				endNode.RemoveConnection(startNode);
				if (connectedNode1 != null && connectedNode2 != null)
				{
					startNode.RemoveConnection(connectedNode1);
					connectedNode1.RemoveConnection(startNode);
					endNode.RemoveConnection(connectedNode2);
					connectedNode2.RemoveConnection(endNode);
				}
			}
		}

		private void RemoveConnections(GraphNode node)
		{
			node.ClearConnections(true);
		}

		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			if (Application.isPlaying)
			{
				Apply(true);
				if (AstarPath.active != null)
				{
					AstarPath.active.FloodFill();
				}
			}
		}

		public void Apply(bool forceNewCheck)
		{
			NNConstraint none = NNConstraint.None;
			none.distanceXZ = true;
			int graphIndex = (int)startNode.GraphIndex;
			none.graphMask = ~(1 << graphIndex);
			bool flag = true;
			NNInfo nearest = AstarPath.active.GetNearest(StartTransform.position, none);
			flag &= nearest.node == connectedNode1 && nearest.node != null;
			connectedNode1 = nearest.node as MeshNode;
			clamped1 = nearest.position;
			if (connectedNode1 != null)
			{
				Debug.DrawRay((Vector3)connectedNode1.position, Vector3.up * 5f, Color.red);
			}
			NNInfo nearest2 = AstarPath.active.GetNearest(EndTransform.position, none);
			flag &= nearest2.node == connectedNode2 && nearest2.node != null;
			connectedNode2 = nearest2.node as MeshNode;
			clamped2 = nearest2.position;
			if (connectedNode2 != null)
			{
				Debug.DrawRay((Vector3)connectedNode2.position, Vector3.up * 5f, Color.cyan);
			}
			if (connectedNode2 == null || connectedNode1 == null)
			{
				return;
			}
			startNode.SetPosition((Int3)StartTransform.position);
			endNode.SetPosition((Int3)EndTransform.position);
			if (flag && !forceNewCheck)
			{
				return;
			}
			RemoveConnections(startNode);
			RemoveConnections(endNode);
			uint cost = (uint)Mathf.RoundToInt((float)((Int3)(StartTransform.position - EndTransform.position)).costMagnitude * costFactor);
			startNode.AddConnection(endNode, cost);
			endNode.AddConnection(startNode, cost);
			Int3 rhs = connectedNode2.position - connectedNode1.position;
			for (int i = 0; i < connectedNode1.GetVertexCount(); i++)
			{
				Int3 vertex = connectedNode1.GetVertex(i);
				Int3 vertex2 = connectedNode1.GetVertex((i + 1) % connectedNode1.GetVertexCount());
				if (Int3.DotLong((vertex2 - vertex).Normal2D(), rhs) > 0)
				{
					continue;
				}
				for (int j = 0; j < connectedNode2.GetVertexCount(); j++)
				{
					Int3 vertex3 = connectedNode2.GetVertex(j);
					Int3 vertex4 = connectedNode2.GetVertex((j + 1) % connectedNode2.GetVertexCount());
					if (Int3.DotLong((vertex4 - vertex3).Normal2D(), rhs) >= 0 && (double)Int3.Angle(vertex4 - vertex3, vertex2 - vertex) > 2.967059810956319)
					{
						float val = 0f;
						float val2 = 1f;
						val2 = Math.Min(val2, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex3));
						val = Math.Max(val, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex4));
						if (!(val2 < val))
						{
							Vector3 vector = (Vector3)(vertex2 - vertex) * val + (Vector3)vertex;
							Vector3 vector2 = (Vector3)(vertex2 - vertex) * val2 + (Vector3)vertex;
							startNode.portalA = vector;
							startNode.portalB = vector2;
							endNode.portalA = vector2;
							endNode.portalB = vector;
							connectedNode1.AddConnection(startNode, (uint)Mathf.RoundToInt((float)((Int3)(clamped1 - StartTransform.position)).costMagnitude * costFactor));
							connectedNode2.AddConnection(endNode, (uint)Mathf.RoundToInt((float)((Int3)(clamped2 - EndTransform.position)).costMagnitude * costFactor));
							startNode.AddConnection(connectedNode1, (uint)Mathf.RoundToInt((float)((Int3)(clamped1 - StartTransform.position)).costMagnitude * costFactor));
							endNode.AddConnection(connectedNode2, (uint)Mathf.RoundToInt((float)((Int3)(clamped2 - EndTransform.position)).costMagnitude * costFactor));
							return;
						}
						Debug.LogError("Something went wrong! " + val + " " + val2 + " " + (string)vertex + " " + (string)vertex2 + " " + (string)vertex3 + " " + (string)vertex4 + "\nTODO, how can this happen?");
					}
				}
			}
		}

		public virtual void OnDrawGizmosSelected()
		{
			OnDrawGizmos(true);
		}

		public void OnDrawGizmos()
		{
			OnDrawGizmos(false);
		}

		public void OnDrawGizmos(bool selected)
		{
			Color color = ((!selected) ? GizmosColor : GizmosColorSelected);
			if (StartTransform != null)
			{
				Draw.Gizmos.CircleXZ(StartTransform.position, 0.4f, color);
			}
			if (EndTransform != null)
			{
				Draw.Gizmos.CircleXZ(EndTransform.position, 0.4f, color);
			}
			if (StartTransform != null && EndTransform != null)
			{
				Draw.Gizmos.Bezier(StartTransform.position, EndTransform.position, color);
				if (selected)
				{
					Vector3 normalized = Vector3.Cross(Vector3.up, EndTransform.position - StartTransform.position).normalized;
					Draw.Gizmos.Bezier(StartTransform.position + normalized * 0.1f, EndTransform.position + normalized * 0.1f, color);
					Draw.Gizmos.Bezier(StartTransform.position - normalized * 0.1f, EndTransform.position - normalized * 0.1f, color);
				}
			}
		}
	}
}
