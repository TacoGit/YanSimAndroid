using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class MultiTargetPath : ABPath
	{
		public enum HeuristicMode
		{
			None = 0,
			Average = 1,
			MovingAverage = 2,
			Midpoint = 3,
			MovingMidpoint = 4,
			Sequential = 5
		}

		public OnPathDelegate[] callbacks;

		public GraphNode[] targetNodes;

		protected int targetNodeCount;

		public bool[] targetsFound;

		public Vector3[] targetPoints;

		public Vector3[] originalTargetPoints;

		public List<Vector3>[] vectorPaths;

		public List<GraphNode>[] nodePaths;

		public bool pathsForAll = true;

		public int chosenTarget = -1;

		private int sequentialTarget;

		public HeuristicMode heuristicMode = HeuristicMode.Sequential;

		public bool inverted { get; protected set; }

		public static MultiTargetPath Construct(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
		{
			MultiTargetPath multiTargetPath = Construct(target, startPoints, callbackDelegates, callback);
			multiTargetPath.inverted = true;
			return multiTargetPath;
		}

		public static MultiTargetPath Construct(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
		{
			MultiTargetPath multiTargetPath = PathPool.GetPath<MultiTargetPath>();
			multiTargetPath.Setup(start, targets, callbackDelegates, callback);
			return multiTargetPath;
		}

		protected void Setup(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
		{
			inverted = false;
			base.callback = callback;
			callbacks = callbackDelegates;
			if (callbacks != null && callbacks.Length != targets.Length)
			{
				throw new ArgumentException("The targets array must have the same length as the callbackDelegates array");
			}
			targetPoints = targets;
			originalStartPoint = start;
			startPoint = start;
			startIntPoint = (Int3)start;
			if (targets.Length == 0)
			{
				FailWithError("No targets were assigned to the MultiTargetPath");
				return;
			}
			endPoint = targets[0];
			originalTargetPoints = new Vector3[targetPoints.Length];
			for (int i = 0; i < targetPoints.Length; i++)
			{
				originalTargetPoints[i] = targetPoints[i];
			}
		}

		protected override void Reset()
		{
			base.Reset();
			pathsForAll = true;
			chosenTarget = -1;
			sequentialTarget = 0;
			inverted = true;
			heuristicMode = HeuristicMode.Sequential;
		}

		protected override void OnEnterPool()
		{
			if (vectorPaths != null)
			{
				for (int i = 0; i < vectorPaths.Length; i++)
				{
					if (vectorPaths[i] != null)
					{
						ListPool<Vector3>.Release(vectorPaths[i]);
					}
				}
			}
			vectorPaths = null;
			vectorPath = null;
			if (nodePaths != null)
			{
				for (int j = 0; j < nodePaths.Length; j++)
				{
					if (nodePaths[j] != null)
					{
						ListPool<GraphNode>.Release(nodePaths[j]);
					}
				}
			}
			nodePaths = null;
			path = null;
			callbacks = null;
			targetNodes = null;
			targetsFound = null;
			targetPoints = null;
			originalTargetPoints = null;
			base.OnEnterPool();
		}

		private void ChooseShortestPath()
		{
			chosenTarget = -1;
			if (nodePaths == null)
			{
				return;
			}
			uint num = 2147483647u;
			for (int i = 0; i < nodePaths.Length; i++)
			{
				List<GraphNode> list = nodePaths[i];
				if (list != null)
				{
					uint g = pathHandler.GetPathNode(list[(!inverted) ? (list.Count - 1) : 0]).G;
					if (chosenTarget == -1 || g < num)
					{
						chosenTarget = i;
						num = g;
					}
				}
			}
		}

		private void SetPathParametersForReturn(int target)
		{
			path = nodePaths[target];
			vectorPath = vectorPaths[target];
			if (inverted)
			{
				startNode = targetNodes[target];
				startPoint = targetPoints[target];
				originalStartPoint = originalTargetPoints[target];
			}
			else
			{
				endNode = targetNodes[target];
				endPoint = targetPoints[target];
				originalEndPoint = originalTargetPoints[target];
			}
		}

		protected override void ReturnPath()
		{
			if (base.error)
			{
				if (callbacks != null)
				{
					for (int i = 0; i < callbacks.Length; i++)
					{
						if (callbacks[i] != null)
						{
							callbacks[i](this);
						}
					}
				}
				if (callback != null)
				{
					callback(this);
				}
				return;
			}
			bool flag = false;
			if (inverted)
			{
				endPoint = startPoint;
				endNode = startNode;
				originalEndPoint = originalStartPoint;
			}
			for (int j = 0; j < nodePaths.Length; j++)
			{
				if (nodePaths[j] != null)
				{
					completeState = PathCompleteState.Complete;
					flag = true;
				}
				else
				{
					completeState = PathCompleteState.Error;
				}
				if (callbacks != null && callbacks[j] != null)
				{
					SetPathParametersForReturn(j);
					callbacks[j](this);
					vectorPaths[j] = vectorPath;
				}
			}
			if (flag)
			{
				completeState = PathCompleteState.Complete;
				SetPathParametersForReturn(chosenTarget);
			}
			else
			{
				completeState = PathCompleteState.Error;
			}
			if (callback != null)
			{
				callback(this);
			}
		}

		protected void FoundTarget(PathNode nodeR, int i)
		{
			nodeR.flag1 = false;
			Trace(nodeR);
			vectorPaths[i] = vectorPath;
			nodePaths[i] = path;
			vectorPath = ListPool<Vector3>.Claim();
			path = ListPool<GraphNode>.Claim();
			targetsFound[i] = true;
			targetNodeCount--;
			if (!pathsForAll)
			{
				base.CompleteState = PathCompleteState.Complete;
				targetNodeCount = 0;
			}
			else if (targetNodeCount <= 0)
			{
				base.CompleteState = PathCompleteState.Complete;
			}
			else
			{
				RecalculateHTarget(false);
			}
		}

		protected void RebuildOpenList()
		{
			BinaryHeap heap = pathHandler.heap;
			for (int i = 0; i < heap.numberOfItems; i++)
			{
				PathNode node = heap.GetNode(i);
				node.H = CalculateHScore(node.node);
				heap.SetF(i, node.F);
			}
			pathHandler.heap.Rebuild();
		}

		protected override void Prepare()
		{
			nnConstraint.tags = enabledTags;
			NNInfo nearest = AstarPath.active.GetNearest(startPoint, nnConstraint);
			startNode = nearest.node;
			if (startNode == null)
			{
				FailWithError("Could not find start node for multi target path");
				return;
			}
			if (!CanTraverse(startNode))
			{
				FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			PathNNConstraint pathNNConstraint = nnConstraint as PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			vectorPaths = new List<Vector3>[targetPoints.Length];
			nodePaths = new List<GraphNode>[targetPoints.Length];
			targetNodes = new GraphNode[targetPoints.Length];
			targetsFound = new bool[targetPoints.Length];
			targetNodeCount = targetPoints.Length;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < targetPoints.Length; i++)
			{
				NNInfo nearest2 = AstarPath.active.GetNearest(targetPoints[i], nnConstraint);
				targetNodes[i] = nearest2.node;
				targetPoints[i] = nearest2.position;
				if (targetNodes[i] != null)
				{
					flag3 = true;
					endNode = targetNodes[i];
				}
				bool flag4 = false;
				if (nearest2.node != null && CanTraverse(nearest2.node))
				{
					flag = true;
				}
				else
				{
					flag4 = true;
				}
				if (nearest2.node != null && nearest2.node.Area == startNode.Area)
				{
					flag2 = true;
				}
				else
				{
					flag4 = true;
				}
				if (flag4)
				{
					targetsFound[i] = true;
					targetNodeCount--;
				}
			}
			startPoint = nearest.position;
			startIntPoint = (Int3)startPoint;
			if (!flag3)
			{
				FailWithError("Couldn't find nodes close to the all of the end points");
			}
			else if (!flag)
			{
				FailWithError("No target nodes could be traversed");
			}
			else if (!flag2)
			{
				FailWithError("There are no valid paths to the targets");
			}
			else
			{
				RecalculateHTarget(true);
			}
		}

		private void RecalculateHTarget(bool firstTime)
		{
			if (!pathsForAll)
			{
				heuristic = Heuristic.None;
				heuristicScale = 0f;
				return;
			}
			switch (heuristicMode)
			{
			case HeuristicMode.None:
				heuristic = Heuristic.None;
				heuristicScale = 0f;
				break;
			case HeuristicMode.Average:
				if (!firstTime)
				{
					return;
				}
				goto case HeuristicMode.MovingAverage;
			case HeuristicMode.MovingAverage:
			{
				Vector3 zero = Vector3.zero;
				int num2 = 0;
				for (int j = 0; j < targetPoints.Length; j++)
				{
					if (!targetsFound[j])
					{
						zero += (Vector3)targetNodes[j].position;
						num2++;
					}
				}
				if (num2 == 0)
				{
					throw new Exception("Should not happen");
				}
				zero /= (float)num2;
				hTarget = (Int3)zero;
				break;
			}
			case HeuristicMode.Midpoint:
				if (!firstTime)
				{
					return;
				}
				goto case HeuristicMode.MovingMidpoint;
			case HeuristicMode.MovingMidpoint:
			{
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				bool flag = false;
				for (int k = 0; k < targetPoints.Length; k++)
				{
					if (!targetsFound[k])
					{
						if (!flag)
						{
							vector = (Vector3)targetNodes[k].position;
							vector2 = (Vector3)targetNodes[k].position;
							flag = true;
						}
						else
						{
							vector = Vector3.Min((Vector3)targetNodes[k].position, vector);
							vector2 = Vector3.Max((Vector3)targetNodes[k].position, vector2);
						}
					}
				}
				Int3 @int = (Int3)((vector + vector2) * 0.5f);
				hTarget = @int;
				break;
			}
			case HeuristicMode.Sequential:
			{
				if (!firstTime && !targetsFound[sequentialTarget])
				{
					return;
				}
				float num = 0f;
				for (int i = 0; i < targetPoints.Length; i++)
				{
					if (!targetsFound[i])
					{
						float sqrMagnitude = (targetNodes[i].position - startNode.position).sqrMagnitude;
						if (sqrMagnitude > num)
						{
							num = sqrMagnitude;
							hTarget = (Int3)targetPoints[i];
							sequentialTarget = i;
						}
					}
				}
				break;
			}
			}
			if (!firstTime)
			{
				RebuildOpenList();
			}
		}

		protected override void Initialize()
		{
			PathNode pathNode = pathHandler.GetPathNode(startNode);
			pathNode.node = startNode;
			pathNode.pathID = base.pathID;
			pathNode.parent = null;
			pathNode.cost = 0u;
			pathNode.G = GetTraversalCost(startNode);
			pathNode.H = CalculateHScore(startNode);
			for (int i = 0; i < targetNodes.Length; i++)
			{
				if (startNode == targetNodes[i])
				{
					FoundTarget(pathNode, i);
				}
				else if (targetNodes[i] != null)
				{
					pathHandler.GetPathNode(targetNodes[i]).flag1 = true;
				}
			}
			if (targetNodeCount <= 0)
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			startNode.Open(this, pathNode, pathHandler);
			searchedNodes++;
			if (pathHandler.heap.isEmpty)
			{
				FailWithError("No open points, the start node didn't open any nodes");
			}
			else
			{
				currentR = pathHandler.heap.Remove();
			}
		}

		protected override void Cleanup()
		{
			ChooseShortestPath();
			ResetFlags();
		}

		private void ResetFlags()
		{
			if (targetNodes == null)
			{
				return;
			}
			for (int i = 0; i < targetNodes.Length; i++)
			{
				if (targetNodes[i] != null)
				{
					pathHandler.GetPathNode(targetNodes[i]).flag1 = false;
				}
			}
		}

		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				searchedNodes++;
				if (currentR.flag1)
				{
					for (int i = 0; i < targetNodes.Length; i++)
					{
						if (!targetsFound[i] && currentR.node == targetNodes[i])
						{
							FoundTarget(currentR, i);
							if (base.CompleteState != 0)
							{
								break;
							}
						}
					}
					if (targetNodeCount <= 0)
					{
						base.CompleteState = PathCompleteState.Complete;
						break;
					}
				}
				currentR.node.Open(this, currentR, pathHandler);
				if (pathHandler.heap.isEmpty)
				{
					base.CompleteState = PathCompleteState.Complete;
					break;
				}
				currentR = pathHandler.heap.Remove();
				if (num > 500)
				{
					if (DateTime.UtcNow.Ticks >= targetTick)
					{
						break;
					}
					num = 0;
				}
				num++;
			}
		}

		protected override void Trace(PathNode node)
		{
			base.Trace(node);
			if (inverted)
			{
				int num = path.Count / 2;
				for (int i = 0; i < num; i++)
				{
					GraphNode value = path[i];
					path[i] = path[path.Count - i - 1];
					path[path.Count - i - 1] = value;
				}
				for (int j = 0; j < num; j++)
				{
					Vector3 value2 = vectorPath[j];
					vectorPath[j] = vectorPath[vectorPath.Count - j - 1];
					vectorPath[vectorPath.Count - j - 1] = value2;
				}
			}
		}

		internal override string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return string.Empty;
			}
			StringBuilder debugStringBuilder = pathHandler.DebugStringBuilder;
			debugStringBuilder.Length = 0;
			DebugStringPrefix(logMode, debugStringBuilder);
			if (!base.error)
			{
				debugStringBuilder.Append("\nShortest path was ");
				debugStringBuilder.Append((chosenTarget != -1) ? nodePaths[chosenTarget].Count.ToString() : "undefined");
				debugStringBuilder.Append(" nodes long");
				if (logMode == PathLog.Heavy)
				{
					debugStringBuilder.Append("\nPaths (").Append(targetsFound.Length).Append("):");
					for (int i = 0; i < targetsFound.Length; i++)
					{
						debugStringBuilder.Append("\n\n\tPath ").Append(i).Append(" Found: ")
							.Append(targetsFound[i]);
						if (nodePaths[i] == null)
						{
							continue;
						}
						debugStringBuilder.Append("\n\t\tLength: ");
						debugStringBuilder.Append(nodePaths[i].Count);
						GraphNode graphNode = nodePaths[i][nodePaths[i].Count - 1];
						if (graphNode != null)
						{
							PathNode pathNode = pathHandler.GetPathNode(endNode);
							if (pathNode != null)
							{
								debugStringBuilder.Append("\n\t\tEnd Node");
								debugStringBuilder.Append("\n\t\t\tG: ");
								debugStringBuilder.Append(pathNode.G);
								debugStringBuilder.Append("\n\t\t\tH: ");
								debugStringBuilder.Append(pathNode.H);
								debugStringBuilder.Append("\n\t\t\tF: ");
								debugStringBuilder.Append(pathNode.F);
								debugStringBuilder.Append("\n\t\t\tPoint: ");
								Vector3 vector = endPoint;
								debugStringBuilder.Append(vector.ToString());
								debugStringBuilder.Append("\n\t\t\tGraph: ");
								debugStringBuilder.Append(endNode.GraphIndex);
							}
							else
							{
								debugStringBuilder.Append("\n\t\tEnd Node: Null");
							}
						}
					}
					debugStringBuilder.Append("\nStart Node");
					debugStringBuilder.Append("\n\tPoint: ");
					Vector3 vector2 = endPoint;
					debugStringBuilder.Append(vector2.ToString());
					debugStringBuilder.Append("\n\tGraph: ");
					debugStringBuilder.Append(startNode.GraphIndex);
					debugStringBuilder.Append("\nBinary Heap size at completion: ");
					debugStringBuilder.AppendLine((pathHandler.heap != null) ? (pathHandler.heap.numberOfItems - 2).ToString() : "Null");
				}
			}
			DebugStringSuffix(logMode, debugStringBuilder);
			return debugStringBuilder.ToString();
		}
	}
}
