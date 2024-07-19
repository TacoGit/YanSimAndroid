using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Pathfinding
{
	public class PathProcessor
	{
		public struct GraphUpdateLock
		{
			private PathProcessor pathProcessor;

			private int id;

			public bool Held
			{
				get
				{
					return pathProcessor != null && pathProcessor.locks.Contains(id);
				}
			}

			public GraphUpdateLock(PathProcessor pathProcessor, bool block)
			{
				this.pathProcessor = pathProcessor;
				id = pathProcessor.Lock(block);
			}

			public void Release()
			{
				pathProcessor.Unlock(id);
			}
		}

		internal readonly ThreadControlQueue queue;

		private readonly AstarPath astar;

		private readonly PathReturnQueue returnQueue;

		private readonly PathHandler[] pathHandlers;

		private readonly Thread[] threads;

		private IEnumerator threadCoroutine;

		private int nextNodeIndex = 1;

		private readonly Stack<int> nodeIndexPool = new Stack<int>();

		private readonly List<int> locks = new List<int>();

		private int nextLockID;

		public int NumThreads
		{
			get
			{
				return pathHandlers.Length;
			}
		}

		public bool IsUsingMultithreading
		{
			get
			{
				return threads != null;
			}
		}

		public event Action<Path> OnPathPreSearch;

		public event Action<Path> OnPathPostSearch;

		public event Action OnQueueUnblocked;

		internal PathProcessor(AstarPath astar, PathReturnQueue returnQueue, int processors, bool multithreaded)
		{
			this.astar = astar;
			this.returnQueue = returnQueue;
			if (processors < 0)
			{
				throw new ArgumentOutOfRangeException("processors");
			}
			if (!multithreaded && processors != 1)
			{
				throw new Exception("Only a single non-multithreaded processor is allowed");
			}
			queue = new ThreadControlQueue(processors);
			pathHandlers = new PathHandler[processors];
			for (int i = 0; i < processors; i++)
			{
				pathHandlers[i] = new PathHandler(i, processors);
			}
			if (multithreaded)
			{
				threads = new Thread[processors];
				for (int j = 0; j < processors; j++)
				{
					PathHandler pathHandler = pathHandlers[j];
					threads[j] = new Thread((ThreadStart)delegate
					{
						CalculatePathsThreaded(pathHandler);
					});
					threads[j].Name = "Pathfinding Thread " + j;
					threads[j].IsBackground = true;
					threads[j].Start();
				}
			}
			else
			{
				threadCoroutine = CalculatePaths(pathHandlers[0]);
			}
		}

		private int Lock(bool block)
		{
			queue.Block();
			if (block)
			{
				while (!queue.AllReceiversBlocked)
				{
					if (IsUsingMultithreading)
					{
						Thread.Sleep(1);
					}
					else
					{
						TickNonMultithreaded();
					}
				}
			}
			nextLockID++;
			locks.Add(nextLockID);
			return nextLockID;
		}

		private void Unlock(int id)
		{
			if (!locks.Remove(id))
			{
				throw new ArgumentException("This lock has already been released");
			}
			if (locks.Count == 0)
			{
				if (this.OnQueueUnblocked != null)
				{
					this.OnQueueUnblocked();
				}
				queue.Unblock();
			}
		}

		public GraphUpdateLock PausePathfinding(bool block)
		{
			return new GraphUpdateLock(this, block);
		}

		public void TickNonMultithreaded()
		{
			if (threadCoroutine == null)
			{
				return;
			}
			try
			{
				threadCoroutine.MoveNext();
			}
			catch (Exception ex)
			{
				threadCoroutine = null;
				if (!(ex is ThreadControlQueue.QueueTerminationException))
				{
					Debug.LogException(ex);
					Debug.LogError("Unhandled exception during pathfinding. Terminating.");
					queue.TerminateReceivers();
					try
					{
						queue.PopNoBlock(false);
						return;
					}
					catch
					{
						return;
					}
				}
			}
		}

		public void JoinThreads()
		{
			if (threads == null)
			{
				return;
			}
			for (int i = 0; i < threads.Length; i++)
			{
				if (!threads[i].Join(50))
				{
					Debug.LogError("Could not terminate pathfinding thread[" + i + "] in 50ms, trying Thread.Abort");
					threads[i].Abort();
				}
			}
		}

		public void AbortThreads()
		{
			if (threads == null)
			{
				return;
			}
			for (int i = 0; i < threads.Length; i++)
			{
				if (threads[i] != null && threads[i].IsAlive)
				{
					threads[i].Abort();
				}
			}
		}

		public int GetNewNodeIndex()
		{
			return (nodeIndexPool.Count <= 0) ? nextNodeIndex++ : nodeIndexPool.Pop();
		}

		public void InitializeNode(GraphNode node)
		{
			if (!queue.AllReceiversBlocked)
			{
				throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.php#direct");
			}
			for (int i = 0; i < pathHandlers.Length; i++)
			{
				pathHandlers[i].InitializeNode(node);
			}
		}

		public void DestroyNode(GraphNode node)
		{
			if (node.NodeIndex != -1)
			{
				nodeIndexPool.Push(node.NodeIndex);
				for (int i = 0; i < pathHandlers.Length; i++)
				{
					pathHandlers[i].DestroyNode(node);
				}
			}
		}

		private void CalculatePathsThreaded(PathHandler pathHandler)
		{
			try
			{
				long num = 100000L;
				long targetTick = DateTime.UtcNow.Ticks + num;
				while (true)
				{
					Path path = queue.Pop();
					IPathInternals pathInternals = path;
					pathInternals.PrepareBase(pathHandler);
					pathInternals.AdvanceState(PathState.Processing);
					if (this.OnPathPreSearch != null)
					{
						this.OnPathPreSearch(path);
					}
					long ticks = DateTime.UtcNow.Ticks;
					pathInternals.Prepare();
					if (!path.IsDone())
					{
						astar.debugPathData = pathInternals.PathHandler;
						astar.debugPathID = path.pathID;
						pathInternals.Initialize();
						while (!path.IsDone())
						{
							pathInternals.CalculateStep(targetTick);
							targetTick = DateTime.UtcNow.Ticks + num;
							if (queue.IsTerminating)
							{
								path.FailWithError("AstarPath object destroyed");
							}
						}
						path.duration = (float)(DateTime.UtcNow.Ticks - ticks) * 0.0001f;
					}
					pathInternals.Cleanup();
					if (path.immediateCallback != null)
					{
						path.immediateCallback(path);
					}
					if (this.OnPathPostSearch != null)
					{
						this.OnPathPostSearch(path);
					}
					returnQueue.Enqueue(path);
					pathInternals.AdvanceState(PathState.ReturnQueue);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is ThreadControlQueue.QueueTerminationException)
				{
					if (astar.logPathResults == PathLog.Heavy)
					{
						Debug.LogWarning("Shutting down pathfinding thread #" + pathHandler.threadID);
					}
					return;
				}
				Debug.LogException(ex);
				Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				queue.TerminateReceivers();
			}
			Debug.LogError("Error : This part should never be reached.");
			queue.ReceiverTerminated();
		}

		private IEnumerator CalculatePaths(PathHandler pathHandler)
		{
			long maxTicks = (long)(astar.maxFrameTime * 10000f);
			long targetTick = DateTime.UtcNow.Ticks + maxTicks;
			while (true)
			{
				Path p = null;
				bool blockedBefore = false;
				while (p == null)
				{
					try
					{
						p = queue.PopNoBlock(blockedBefore);
						blockedBefore = blockedBefore || p == null;
					}
					catch (ThreadControlQueue.QueueTerminationException)
					{
						yield break;
					}
					if (p == null)
					{
						yield return null;
					}
				}
				IPathInternals ip = p;
				maxTicks = (long)(astar.maxFrameTime * 10000f);
				ip.PrepareBase(pathHandler);
				ip.AdvanceState(PathState.Processing);
				Action<Path> tmpOnPathPreSearch = this.OnPathPreSearch;
				if (tmpOnPathPreSearch != null)
				{
					tmpOnPathPreSearch(p);
				}
				long startTicks = DateTime.UtcNow.Ticks;
				long totalTicks2 = 0L;
				ip.Prepare();
				if (!p.IsDone())
				{
					astar.debugPathData = ip.PathHandler;
					astar.debugPathID = p.pathID;
					ip.Initialize();
					while (!p.IsDone())
					{
						ip.CalculateStep(targetTick);
						if (p.IsDone())
						{
							break;
						}
						totalTicks2 += DateTime.UtcNow.Ticks - startTicks;
						yield return null;
						startTicks = DateTime.UtcNow.Ticks;
						if (queue.IsTerminating)
						{
							p.FailWithError("AstarPath object destroyed");
						}
						targetTick = DateTime.UtcNow.Ticks + maxTicks;
					}
					totalTicks2 += DateTime.UtcNow.Ticks - startTicks;
					p.duration = (float)totalTicks2 * 0.0001f;
				}
				ip.Cleanup();
				OnPathDelegate tmpImmediateCallback = p.immediateCallback;
				if (tmpImmediateCallback != null)
				{
					tmpImmediateCallback(p);
				}
				Action<Path> tmpOnPathPostSearch = this.OnPathPostSearch;
				if (tmpOnPathPostSearch != null)
				{
					tmpOnPathPostSearch(p);
				}
				returnQueue.Enqueue(p);
				ip.AdvanceState(PathState.ReturnQueue);
				if (DateTime.UtcNow.Ticks > targetTick)
				{
					yield return null;
					targetTick = DateTime.UtcNow.Ticks + maxTicks;
				}
			}
		}
	}
}
