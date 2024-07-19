using System;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
	public class ABPath : Path
	{
		public GraphNode startNode;

		public GraphNode endNode;

		public Vector3 originalStartPoint;

		public Vector3 originalEndPoint;

		public Vector3 startPoint;

		public Vector3 endPoint;

		public Int3 startIntPoint;

		public bool calculatePartial;

		protected PathNode partialBestTarget;

		protected int[] endNodeCosts;

		private GridNode gridSpecialCaseNode;

		protected virtual bool hasEndPoint
		{
			get
			{
				return true;
			}
		}

		public static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			ABPath aBPath = PathPool.GetPath<ABPath>();
			aBPath.Setup(start, end, callback);
			return aBPath;
		}

		protected void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
		{
			callback = callbackDelegate;
			UpdateStartEnd(start, end);
		}

		protected void UpdateStartEnd(Vector3 start, Vector3 end)
		{
			originalStartPoint = start;
			originalEndPoint = end;
			startPoint = start;
			endPoint = end;
			startIntPoint = (Int3)start;
			hTarget = (Int3)end;
		}

		internal override uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
		{
			if (startNode != null && endNode != null)
			{
				if (a == startNode)
				{
					return (uint)((double)(startIntPoint - ((b != endNode) ? b.position : hTarget)).costMagnitude * ((double)currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == startNode)
				{
					return (uint)((double)(startIntPoint - ((a != endNode) ? a.position : hTarget)).costMagnitude * ((double)currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (a == endNode)
				{
					return (uint)((double)(hTarget - b.position).costMagnitude * ((double)currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == endNode)
				{
					return (uint)((double)(hTarget - a.position).costMagnitude * ((double)currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
			}
			else
			{
				if (a == startNode)
				{
					return (uint)((double)(startIntPoint - b.position).costMagnitude * ((double)currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
				if (b == startNode)
				{
					return (uint)((double)(startIntPoint - a.position).costMagnitude * ((double)currentCost * 1.0 / (double)(a.position - b.position).costMagnitude));
				}
			}
			return currentCost;
		}

		protected override void Reset()
		{
			base.Reset();
			startNode = null;
			endNode = null;
			originalStartPoint = Vector3.zero;
			originalEndPoint = Vector3.zero;
			startPoint = Vector3.zero;
			endPoint = Vector3.zero;
			calculatePartial = false;
			partialBestTarget = null;
			startIntPoint = default(Int3);
			hTarget = default(Int3);
			endNodeCosts = null;
			gridSpecialCaseNode = null;
		}

		protected virtual bool EndPointGridGraphSpecialCase(GraphNode closestWalkableEndNode)
		{
			GridNode gridNode = closestWalkableEndNode as GridNode;
			if (gridNode != null)
			{
				GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
				GridNode gridNode2 = AstarPath.active.GetNearest(originalEndPoint, NNConstraint.None).node as GridNode;
				if (gridNode != gridNode2 && gridNode2 != null && gridNode.GraphIndex == gridNode2.GraphIndex)
				{
					int num = gridNode.NodeInGridIndex % gridGraph.width;
					int num2 = gridNode.NodeInGridIndex / gridGraph.width;
					int num3 = gridNode2.NodeInGridIndex % gridGraph.width;
					int num4 = gridNode2.NodeInGridIndex / gridGraph.width;
					bool flag = false;
					switch (gridGraph.neighbours)
					{
					case NumNeighbours.Four:
						if ((num == num3 && Math.Abs(num2 - num4) == 1) || (num2 == num4 && Math.Abs(num - num3) == 1))
						{
							flag = true;
						}
						break;
					case NumNeighbours.Eight:
						if (Math.Abs(num - num3) <= 1 && Math.Abs(num2 - num4) <= 1)
						{
							flag = true;
						}
						break;
					case NumNeighbours.Six:
					{
						for (int i = 0; i < 6; i++)
						{
							int num5 = num3 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
							int num6 = num4 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
							if (num == num5 && num2 == num6)
							{
								flag = true;
								break;
							}
						}
						break;
					}
					default:
						throw new Exception("Unhandled NumNeighbours");
					}
					if (flag)
					{
						SetFlagOnSurroundingGridNodes(gridNode2, 1, true);
						endPoint = (Vector3)gridNode2.position;
						hTarget = gridNode2.position;
						endNode = gridNode2;
						hTargetNode = endNode;
						gridSpecialCaseNode = gridNode2;
						return true;
					}
				}
			}
			return false;
		}

		private void SetFlagOnSurroundingGridNodes(GridNode gridNode, int flag, bool flagState)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
			int num = ((gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours != NumNeighbours.Eight) ? 6 : 8));
			int num2 = gridNode.NodeInGridIndex % gridGraph.width;
			int num3 = gridNode.NodeInGridIndex / gridGraph.width;
			if (flag != 1 && flag != 2)
			{
				throw new ArgumentOutOfRangeException("flag");
			}
			for (int i = 0; i < num; i++)
			{
				int num4;
				int num5;
				if (gridGraph.neighbours == NumNeighbours.Six)
				{
					num4 = num2 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
					num5 = num3 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
				}
				else
				{
					num4 = num2 + gridGraph.neighbourXOffsets[i];
					num5 = num3 + gridGraph.neighbourZOffsets[i];
				}
				if (num4 >= 0 && num5 >= 0 && num4 < gridGraph.width && num5 < gridGraph.depth)
				{
					GridNode node = gridGraph.nodes[num5 * gridGraph.width + num4];
					PathNode pathNode = pathHandler.GetPathNode(node);
					if (flag == 1)
					{
						pathNode.flag1 = flagState;
					}
					else
					{
						pathNode.flag2 = flagState;
					}
				}
			}
		}

		protected override void Prepare()
		{
			nnConstraint.tags = enabledTags;
			NNInfo nearest = AstarPath.active.GetNearest(startPoint, nnConstraint);
			PathNNConstraint pathNNConstraint = nnConstraint as PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			startPoint = nearest.position;
			startIntPoint = (Int3)startPoint;
			startNode = nearest.node;
			if (startNode == null)
			{
				FailWithError("Couldn't find a node close to the start point");
			}
			else if (!CanTraverse(startNode))
			{
				FailWithError("The node closest to the start point could not be traversed");
			}
			else if (hasEndPoint)
			{
				NNInfo nearest2 = AstarPath.active.GetNearest(endPoint, nnConstraint);
				endPoint = nearest2.position;
				endNode = nearest2.node;
				if (endNode == null)
				{
					FailWithError("Couldn't find a node close to the end point");
				}
				else if (!CanTraverse(endNode))
				{
					FailWithError("The node closest to the end point could not be traversed");
				}
				else if (startNode.Area != endNode.Area)
				{
					FailWithError("There is no valid path to the target");
				}
				else if (!EndPointGridGraphSpecialCase(nearest2.node))
				{
					hTarget = (Int3)endPoint;
					hTargetNode = endNode;
					pathHandler.GetPathNode(endNode).flag1 = true;
				}
			}
		}

		protected virtual void CompletePathIfStartIsValidTarget()
		{
			if (hasEndPoint && pathHandler.GetPathNode(startNode).flag1)
			{
				CompleteWith(startNode);
				Trace(pathHandler.GetPathNode(startNode));
			}
		}

		protected override void Initialize()
		{
			if (startNode != null)
			{
				pathHandler.GetPathNode(startNode).flag2 = true;
			}
			if (endNode != null)
			{
				pathHandler.GetPathNode(endNode).flag2 = true;
			}
			PathNode pathNode = pathHandler.GetPathNode(startNode);
			pathNode.node = startNode;
			pathNode.pathID = pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0u;
			pathNode.G = GetTraversalCost(startNode);
			pathNode.H = CalculateHScore(startNode);
			CompletePathIfStartIsValidTarget();
			if (base.CompleteState == PathCompleteState.Complete)
			{
				return;
			}
			startNode.Open(this, pathNode, pathHandler);
			searchedNodes++;
			partialBestTarget = pathNode;
			if (pathHandler.heap.isEmpty)
			{
				if (calculatePartial)
				{
					base.CompleteState = PathCompleteState.Partial;
					Trace(partialBestTarget);
				}
				else
				{
					FailWithError("No open points, the start node didn't open any nodes");
				}
			}
			else
			{
				currentR = pathHandler.heap.Remove();
			}
		}

		protected override void Cleanup()
		{
			if (startNode != null)
			{
				PathNode pathNode = pathHandler.GetPathNode(startNode);
				pathNode.flag1 = false;
				pathNode.flag2 = false;
			}
			if (endNode != null)
			{
				PathNode pathNode2 = pathHandler.GetPathNode(endNode);
				pathNode2.flag1 = false;
				pathNode2.flag2 = false;
			}
			if (gridSpecialCaseNode != null)
			{
				PathNode pathNode3 = pathHandler.GetPathNode(gridSpecialCaseNode);
				pathNode3.flag1 = false;
				pathNode3.flag2 = false;
				SetFlagOnSurroundingGridNodes(gridSpecialCaseNode, 1, false);
				SetFlagOnSurroundingGridNodes(gridSpecialCaseNode, 2, false);
			}
		}

		private void CompleteWith(GraphNode node)
		{
			if (endNode != node)
			{
				GridNode gridNode = node as GridNode;
				if (gridNode == null)
				{
					throw new Exception("Some path is not cleaning up the flag1 field. This is a bug.");
				}
				endPoint = gridNode.ClosestPointOnNode(originalEndPoint);
				endNode = node;
			}
			base.CompleteState = PathCompleteState.Complete;
		}

		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				searchedNodes++;
				if (currentR.flag1)
				{
					CompleteWith(currentR.node);
					break;
				}
				if (currentR.H < partialBestTarget.H)
				{
					partialBestTarget = currentR;
				}
				currentR.node.Open(this, currentR, pathHandler);
				if (pathHandler.heap.isEmpty)
				{
					if (calculatePartial && partialBestTarget != null)
					{
						base.CompleteState = PathCompleteState.Partial;
						Trace(partialBestTarget);
					}
					else
					{
						FailWithError("Searched whole area but could not find target");
					}
					return;
				}
				currentR = pathHandler.heap.Remove();
				if (num > 500)
				{
					if (DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
					if (searchedNodes > 1000000)
					{
						throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
			if (base.CompleteState == PathCompleteState.Complete)
			{
				Trace(currentR);
			}
			else if (calculatePartial && partialBestTarget != null)
			{
				base.CompleteState = PathCompleteState.Partial;
				Trace(partialBestTarget);
			}
		}

		internal override string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			DebugStringPrefix(logMode, stringBuilder);
			if (!base.error && logMode == PathLog.Heavy)
			{
				if (hasEndPoint && endNode != null)
				{
					PathNode pathNode = pathHandler.GetPathNode(endNode);
					stringBuilder.Append("\nEnd Node\n\tG: ");
					stringBuilder.Append(pathNode.G);
					stringBuilder.Append("\n\tH: ");
					stringBuilder.Append(pathNode.H);
					stringBuilder.Append("\n\tF: ");
					stringBuilder.Append(pathNode.F);
					stringBuilder.Append("\n\tPoint: ");
					Vector3 vector = endPoint;
					stringBuilder.Append(vector.ToString());
					stringBuilder.Append("\n\tGraph: ");
					stringBuilder.Append(endNode.GraphIndex);
				}
				stringBuilder.Append("\nStart Node");
				stringBuilder.Append("\n\tPoint: ");
				Vector3 vector2 = startPoint;
				stringBuilder.Append(vector2.ToString());
				stringBuilder.Append("\n\tGraph: ");
				if (startNode != null)
				{
					stringBuilder.Append(startNode.GraphIndex);
				}
				else
				{
					stringBuilder.Append("< null startNode >");
				}
			}
			DebugStringSuffix(logMode, stringBuilder);
			return stringBuilder.ToString();
		}

		[Obsolete]
		public Vector3 GetMovementVector(Vector3 point)
		{
			if (vectorPath == null || vectorPath.Count == 0)
			{
				return Vector3.zero;
			}
			if (vectorPath.Count == 1)
			{
				return vectorPath[0] - point;
			}
			float num = float.PositiveInfinity;
			int num2 = 0;
			for (int i = 0; i < vectorPath.Count - 1; i++)
			{
				Vector3 vector = VectorMath.ClosestPointOnSegment(vectorPath[i], vectorPath[i + 1], point);
				float sqrMagnitude = (vector - point).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					num2 = i;
				}
			}
			return vectorPath[num2 + 1] - point;
		}
	}
}
