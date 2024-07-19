using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	public class StartEndModifier : PathModifier
	{
		public enum Exactness
		{
			SnapToNode = 0,
			Original = 1,
			Interpolate = 2,
			ClosestOnNode = 3,
			NodeConnection = 4
		}

		public bool addPoints;

		public Exactness exactStartPoint = Exactness.ClosestOnNode;

		public Exactness exactEndPoint = Exactness.ClosestOnNode;

		public Func<Vector3> adjustStartPoint;

		public bool useRaycasting;

		public LayerMask mask = -1;

		public bool useGraphRaycasting;

		private List<GraphNode> connectionBuffer;

		private Action<GraphNode> connectionBufferAddDelegate;

		public override int Order
		{
			get
			{
				return 0;
			}
		}

		public override void Apply(Path _p)
		{
			ABPath aBPath = _p as ABPath;
			if (aBPath != null && aBPath.vectorPath.Count != 0)
			{
				if (aBPath.vectorPath.Count == 1 && !addPoints)
				{
					aBPath.vectorPath.Add(aBPath.vectorPath[0]);
				}
				bool forceAddPoint;
				Vector3 vector = Snap(aBPath, exactStartPoint, true, out forceAddPoint);
				bool forceAddPoint2;
				Vector3 vector2 = Snap(aBPath, exactEndPoint, false, out forceAddPoint2);
				if ((forceAddPoint || addPoints) && exactStartPoint != 0)
				{
					aBPath.vectorPath.Insert(0, vector);
				}
				else
				{
					aBPath.vectorPath[0] = vector;
				}
				if ((forceAddPoint2 || addPoints) && exactEndPoint != 0)
				{
					aBPath.vectorPath.Add(vector2);
				}
				else
				{
					aBPath.vectorPath[aBPath.vectorPath.Count - 1] = vector2;
				}
			}
		}

		private Vector3 Snap(ABPath path, Exactness mode, bool start, out bool forceAddPoint)
		{
			int num = ((!start) ? (path.path.Count - 1) : 0);
			GraphNode graphNode = path.path[num];
			Vector3 vector = (Vector3)graphNode.position;
			forceAddPoint = false;
			switch (mode)
			{
			case Exactness.ClosestOnNode:
				return (!start) ? path.endPoint : path.startPoint;
			case Exactness.SnapToNode:
				return vector;
			case Exactness.Original:
			case Exactness.Interpolate:
			case Exactness.NodeConnection:
			{
				Vector3 vector2 = ((!start) ? path.originalEndPoint : ((adjustStartPoint == null) ? path.originalStartPoint : adjustStartPoint()));
				switch (mode)
				{
				case Exactness.Original:
					return GetClampedPoint(vector, vector2, graphNode);
				case Exactness.Interpolate:
				{
					GraphNode graphNode2 = path.path[Mathf.Clamp(num + (start ? 1 : (-1)), 0, path.path.Count - 1)];
					return VectorMath.ClosestPointOnSegment(vector, (Vector3)graphNode2.position, vector2);
				}
				case Exactness.NodeConnection:
				{
					connectionBuffer = connectionBuffer ?? new List<GraphNode>();
					connectionBufferAddDelegate = connectionBufferAddDelegate ?? new Action<GraphNode>(connectionBuffer.Add);
					GraphNode graphNode2 = path.path[Mathf.Clamp(num + (start ? 1 : (-1)), 0, path.path.Count - 1)];
					graphNode.GetConnections(connectionBufferAddDelegate);
					Vector3 result = vector;
					float num2 = float.PositiveInfinity;
					for (int num3 = connectionBuffer.Count - 1; num3 >= 0; num3--)
					{
						GraphNode graphNode3 = connectionBuffer[num3];
						Vector3 vector3 = VectorMath.ClosestPointOnSegment(vector, (Vector3)graphNode3.position, vector2);
						float sqrMagnitude = (vector3 - vector2).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							result = vector3;
							num2 = sqrMagnitude;
							forceAddPoint = graphNode3 != graphNode2;
						}
					}
					connectionBuffer.Clear();
					return result;
				}
				default:
					throw new ArgumentException("Cannot reach this point, but the compiler is not smart enough to realize that.");
				}
			}
			default:
				throw new ArgumentException("Invalid mode");
			}
		}

		protected Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
		{
			Vector3 vector = to;
			RaycastHit hitInfo;
			if (useRaycasting && Physics.Linecast(from, to, out hitInfo, mask))
			{
				vector = hitInfo.point;
			}
			if (useGraphRaycasting && hint != null)
			{
				IRaycastableGraph raycastableGraph = AstarData.GetGraph(hint) as IRaycastableGraph;
				GraphHitInfo hit;
				if (raycastableGraph != null && raycastableGraph.Linecast(from, vector, hint, out hit))
				{
					vector = hit.point;
				}
			}
			return vector;
		}
	}
}
