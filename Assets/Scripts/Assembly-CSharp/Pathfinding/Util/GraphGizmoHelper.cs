using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	public class GraphGizmoHelper : IAstarPooledObject, IDisposable
	{
		private RetainedGizmos gizmos;

		private PathHandler debugData;

		private ushort debugPathID;

		private GraphDebugMode debugMode;

		private bool showSearchTree;

		private float debugFloor;

		private float debugRoof;

		private Vector3 drawConnectionStart;

		private Color drawConnectionColor;

		private readonly Action<GraphNode> drawConnection;

		public RetainedGizmos.Hasher hasher { get; private set; }

		public RetainedGizmos.Builder builder { get; private set; }

		public GraphGizmoHelper()
		{
			drawConnection = DrawConnection;
		}

		public void Init(AstarPath active, RetainedGizmos.Hasher hasher, RetainedGizmos gizmos)
		{
			if (active != null)
			{
				debugData = active.debugPathData;
				debugPathID = active.debugPathID;
				debugMode = active.debugMode;
				debugFloor = active.debugFloor;
				debugRoof = active.debugRoof;
				showSearchTree = active.showSearchTree && debugData != null;
			}
			this.gizmos = gizmos;
			this.hasher = hasher;
			builder = ObjectPool<RetainedGizmos.Builder>.Claim();
		}

		public void OnEnterPool()
		{
			RetainedGizmos.Builder obj = builder;
			ObjectPool<RetainedGizmos.Builder>.Release(ref obj);
			builder = null;
			debugData = null;
		}

		public void DrawConnections(GraphNode node)
		{
			if (showSearchTree)
			{
				if (InSearchTree(node, debugData, debugPathID))
				{
					PathNode pathNode = debugData.GetPathNode(node);
					if (pathNode.parent != null)
					{
						builder.DrawLine((Vector3)node.position, (Vector3)debugData.GetPathNode(node).parent.node.position, NodeColor(node));
					}
				}
			}
			else
			{
				drawConnectionColor = NodeColor(node);
				drawConnectionStart = (Vector3)node.position;
				node.GetConnections(drawConnection);
			}
		}

		private void DrawConnection(GraphNode other)
		{
			builder.DrawLine(drawConnectionStart, Vector3.Lerp((Vector3)other.position, drawConnectionStart, 0.5f), drawConnectionColor);
		}

		public Color NodeColor(GraphNode node)
		{
			if (showSearchTree && !InSearchTree(node, debugData, debugPathID))
			{
				return Color.clear;
			}
			if (node.Walkable)
			{
				switch (debugMode)
				{
				case GraphDebugMode.Areas:
					return AstarColor.GetAreaColor(node.Area);
				case GraphDebugMode.Penalty:
					return Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, ((float)node.Penalty - debugFloor) / (debugRoof - debugFloor));
				case GraphDebugMode.Tags:
					return AstarColor.GetAreaColor(node.Tag);
				case GraphDebugMode.Connections:
					return AstarColor.NodeConnection;
				default:
					if (debugData != null)
					{
						PathNode pathNode = debugData.GetPathNode(node);
						return Color.Lerp(t: (((debugMode == GraphDebugMode.G) ? ((float)pathNode.G) : ((debugMode != GraphDebugMode.H) ? ((float)pathNode.F) : ((float)pathNode.H))) - debugFloor) / (debugRoof - debugFloor), a: AstarColor.ConnectionLowLerp, b: AstarColor.ConnectionHighLerp);
					}
					return AstarColor.NodeConnection;
				}
			}
			return AstarColor.UnwalkableNode;
		}

		public static bool InSearchTree(GraphNode node, PathHandler handler, ushort pathID)
		{
			return handler.GetPathNode(node).pathID == pathID;
		}

		public void DrawWireTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			builder.DrawLine(a, b, color);
			builder.DrawLine(b, c, color);
			builder.DrawLine(c, a, color);
		}

		public void DrawTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			List<int> list = ListPool<int>.Claim(numTriangles);
			for (int i = 0; i < numTriangles * 3; i++)
			{
				list.Add(i);
			}
			builder.DrawMesh(gizmos, vertices, list, colors);
			ListPool<int>.Release(ref list);
		}

		public void DrawWireTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			for (int i = 0; i < numTriangles; i++)
			{
				DrawWireTriangle(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2], colors[i * 3]);
			}
		}

		public void Submit()
		{
			builder.Submit(gizmos, hasher);
		}

		void IDisposable.Dispose()
		{
			GraphGizmoHelper obj = this;
			Submit();
			ObjectPool<GraphGizmoHelper>.Release(ref obj);
		}
	}
}
