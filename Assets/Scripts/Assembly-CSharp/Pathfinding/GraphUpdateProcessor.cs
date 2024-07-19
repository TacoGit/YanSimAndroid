using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	internal class GraphUpdateProcessor
	{
		private enum GraphUpdateOrder
		{
			GraphUpdate = 0,
			FloodFill = 1
		}

		private struct GUOSingle
		{
			public GraphUpdateOrder order;

			public IUpdatableGraph graph;

			public GraphUpdateObject obj;
		}

		private readonly AstarPath astar;

		private Thread graphUpdateThread;

		private bool anyGraphUpdateInProgress;

		private readonly Queue<GraphUpdateObject> graphUpdateQueue = new Queue<GraphUpdateObject>();

		private readonly Queue<GUOSingle> graphUpdateQueueAsync = new Queue<GUOSingle>();

		private readonly Queue<GUOSingle> graphUpdateQueuePost = new Queue<GUOSingle>();

		private readonly Queue<GUOSingle> graphUpdateQueueRegular = new Queue<GUOSingle>();

		private readonly ManualResetEvent asyncGraphUpdatesComplete = new ManualResetEvent(true);

		private readonly AutoResetEvent graphUpdateAsyncEvent = new AutoResetEvent(false);

		private readonly AutoResetEvent exitAsyncThread = new AutoResetEvent(false);

		private uint lastUniqueAreaIndex;

		public bool IsAnyGraphUpdateQueued
		{
			get
			{
				return graphUpdateQueue.Count > 0;
			}
		}

		public bool IsAnyGraphUpdateInProgress
		{
			get
			{
				return anyGraphUpdateInProgress;
			}
		}

		public event Action OnGraphsUpdated;

		public GraphUpdateProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		public AstarWorkItem GetWorkItem()
		{
			return new AstarWorkItem(QueueGraphUpdatesInternal, ProcessGraphUpdates);
		}

		public void EnableMultithreading()
		{
			if (graphUpdateThread == null || !graphUpdateThread.IsAlive)
			{
				graphUpdateThread = new Thread(ProcessGraphUpdatesAsync);
				graphUpdateThread.IsBackground = true;
				graphUpdateThread.Priority = System.Threading.ThreadPriority.Lowest;
				graphUpdateThread.Start();
			}
		}

		public void DisableMultithreading()
		{
			if (graphUpdateThread != null && graphUpdateThread.IsAlive)
			{
				exitAsyncThread.Set();
				if (!graphUpdateThread.Join(5000))
				{
					Debug.LogError("Graph update thread did not exit in 5 seconds");
				}
				graphUpdateThread = null;
			}
		}

		public void AddToQueue(GraphUpdateObject ob)
		{
			graphUpdateQueue.Enqueue(ob);
		}

		private void QueueGraphUpdatesInternal()
		{
			bool flag = false;
			while (graphUpdateQueue.Count > 0)
			{
				GraphUpdateObject graphUpdateObject = graphUpdateQueue.Dequeue();
				if (graphUpdateObject.requiresFloodFill)
				{
					flag = true;
				}
				foreach (IUpdatableGraph updateableGraph in astar.data.GetUpdateableGraphs())
				{
					NavGraph graph = updateableGraph as NavGraph;
					if (graphUpdateObject.nnConstraint == null || graphUpdateObject.nnConstraint.SuitableGraph(astar.data.GetGraphIndex(graph), graph))
					{
						GUOSingle item = default(GUOSingle);
						item.order = GraphUpdateOrder.GraphUpdate;
						item.obj = graphUpdateObject;
						item.graph = updateableGraph;
						graphUpdateQueueRegular.Enqueue(item);
					}
				}
			}
			if (flag)
			{
				GUOSingle item2 = default(GUOSingle);
				item2.order = GraphUpdateOrder.FloodFill;
				graphUpdateQueueRegular.Enqueue(item2);
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
			anyGraphUpdateInProgress = true;
		}

		private bool ProcessGraphUpdates(bool force)
		{
			if (force)
			{
				asyncGraphUpdatesComplete.WaitOne();
			}
			else if (!asyncGraphUpdatesComplete.WaitOne(0))
			{
				return false;
			}
			ProcessPostUpdates();
			if (!ProcessRegularUpdates(force))
			{
				return false;
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
			if (this.OnGraphsUpdated != null)
			{
				this.OnGraphsUpdated();
			}
			anyGraphUpdateInProgress = false;
			return true;
		}

		private bool ProcessRegularUpdates(bool force)
		{
			while (graphUpdateQueueRegular.Count > 0)
			{
				GUOSingle item = graphUpdateQueueRegular.Peek();
				GraphUpdateThreading graphUpdateThreading = ((item.order == GraphUpdateOrder.FloodFill) ? GraphUpdateThreading.SeparateThread : item.graph.CanUpdateAsync(item.obj));
				if (force || !Application.isPlaying || graphUpdateThread == null || !graphUpdateThread.IsAlive)
				{
					graphUpdateThreading &= (GraphUpdateThreading)(-2);
				}
				if ((graphUpdateThreading & GraphUpdateThreading.UnityInit) != 0)
				{
					if (StartAsyncUpdatesIfQueued())
					{
						return false;
					}
					item.graph.UpdateAreaInit(item.obj);
				}
				if ((graphUpdateThreading & GraphUpdateThreading.SeparateThread) != 0)
				{
					graphUpdateQueueRegular.Dequeue();
					graphUpdateQueueAsync.Enqueue(item);
					if ((graphUpdateThreading & GraphUpdateThreading.UnityPost) != 0 && StartAsyncUpdatesIfQueued())
					{
						return false;
					}
					continue;
				}
				if (StartAsyncUpdatesIfQueued())
				{
					return false;
				}
				graphUpdateQueueRegular.Dequeue();
				if (item.order == GraphUpdateOrder.FloodFill)
				{
					FloodFill();
				}
				else
				{
					try
					{
						item.graph.UpdateArea(item.obj);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error while updating graphs\n" + ex);
					}
				}
				if ((graphUpdateThreading & GraphUpdateThreading.UnityPost) != 0)
				{
					item.graph.UpdateAreaPost(item.obj);
				}
			}
			if (StartAsyncUpdatesIfQueued())
			{
				return false;
			}
			return true;
		}

		private bool StartAsyncUpdatesIfQueued()
		{
			if (graphUpdateQueueAsync.Count > 0)
			{
				asyncGraphUpdatesComplete.Reset();
				graphUpdateAsyncEvent.Set();
				return true;
			}
			return false;
		}

		private void ProcessPostUpdates()
		{
			while (graphUpdateQueuePost.Count > 0)
			{
				GUOSingle gUOSingle = graphUpdateQueuePost.Dequeue();
				GraphUpdateThreading graphUpdateThreading = gUOSingle.graph.CanUpdateAsync(gUOSingle.obj);
				if ((graphUpdateThreading & GraphUpdateThreading.UnityPost) != 0)
				{
					try
					{
						gUOSingle.graph.UpdateAreaPost(gUOSingle.obj);
					}
					catch (Exception ex)
					{
						Debug.LogError("Error while updating graphs (post step)\n" + ex);
					}
				}
			}
		}

		private void ProcessGraphUpdatesAsync()
		{
			AutoResetEvent[] waitHandles = new AutoResetEvent[2] { graphUpdateAsyncEvent, exitAsyncThread };
			while (true)
			{
				int num = WaitHandle.WaitAny(waitHandles);
				if (num == 1)
				{
					break;
				}
				while (graphUpdateQueueAsync.Count > 0)
				{
					GUOSingle item = graphUpdateQueueAsync.Dequeue();
					try
					{
						if (item.order == GraphUpdateOrder.GraphUpdate)
						{
							item.graph.UpdateArea(item.obj);
							graphUpdateQueuePost.Enqueue(item);
							continue;
						}
						if (item.order == GraphUpdateOrder.FloodFill)
						{
							FloodFill();
							continue;
						}
						throw new NotSupportedException(string.Empty + item.order);
					}
					catch (Exception ex)
					{
						Debug.LogError("Exception while updating graphs:\n" + ex);
					}
				}
				asyncGraphUpdatesComplete.Set();
			}
			graphUpdateQueueAsync.Clear();
			asyncGraphUpdatesComplete.Set();
		}

		public void FloodFill(GraphNode seed)
		{
			FloodFill(seed, lastUniqueAreaIndex + 1);
			lastUniqueAreaIndex++;
		}

		public void FloodFill(GraphNode seed, uint area)
		{
			if (area > 131071)
			{
				Debug.LogError("Too high area index - The maximum area index is " + 131071u);
				return;
			}
			if (area < 0)
			{
				Debug.LogError("Too low area index - The minimum area index is 0");
				return;
			}
			Stack<GraphNode> stack = StackPool<GraphNode>.Claim();
			stack.Push(seed);
			seed.Area = area;
			while (stack.Count > 0)
			{
				stack.Pop().FloodFill(stack, area);
			}
			StackPool<GraphNode>.Release(stack);
		}

		public void FloodFill()
		{
			NavGraph[] graphs = astar.graphs;
			if (graphs == null)
			{
				return;
			}
			foreach (NavGraph navGraph in graphs)
			{
				if (navGraph != null)
				{
					navGraph.GetNodes(delegate(GraphNode node)
					{
						node.Area = 0u;
					});
				}
			}
			lastUniqueAreaIndex = 0u;
			uint area = 0u;
			int forcedSmallAreas = 0;
			Stack<GraphNode> stack = StackPool<GraphNode>.Claim();
			foreach (NavGraph navGraph2 in graphs)
			{
				if (navGraph2 == null)
				{
					continue;
				}
				navGraph2.GetNodes(delegate(GraphNode node)
				{
					if (node.Walkable && node.Area == 0)
					{
						area++;
						uint num = area;
						if (area > 131071)
						{
							area--;
							num = area;
							if (forcedSmallAreas == 0)
							{
								forcedSmallAreas = 1;
							}
							forcedSmallAreas++;
						}
						stack.Clear();
						stack.Push(node);
						int num2 = 1;
						node.Area = num;
						while (stack.Count > 0)
						{
							num2++;
							stack.Pop().FloodFill(stack, num);
						}
					}
				});
			}
			lastUniqueAreaIndex = area;
			if (forcedSmallAreas > 0)
			{
				Debug.LogError(forcedSmallAreas + " areas had to share IDs. This usually doesn't affect pathfinding in any significant way (you might get 'Searched whole area but could not find target' as a reason for path failure) however some path requests may take longer to calculate (specifically those that fail with the 'Searched whole area' error).The maximum number of areas is " + 131071u + ".");
			}
			StackPool<GraphNode>.Release(stack);
		}
	}
}
