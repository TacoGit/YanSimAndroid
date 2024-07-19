using System;
using System.Collections.Generic;

namespace Pathfinding
{
	internal class PathReturnQueue
	{
		private Queue<Path> pathReturnQueue = new Queue<Path>();

		private object pathsClaimedSilentlyBy;

		public PathReturnQueue(object pathsClaimedSilentlyBy)
		{
			this.pathsClaimedSilentlyBy = pathsClaimedSilentlyBy;
		}

		public void Enqueue(Path path)
		{
			lock (pathReturnQueue)
			{
				pathReturnQueue.Enqueue(path);
			}
		}

		public void ReturnPaths(bool timeSlice)
		{
			long num = ((!timeSlice) ? 0 : (DateTime.UtcNow.Ticks + 10000));
			int num2 = 0;
			while (true)
			{
				Path path;
				lock (pathReturnQueue)
				{
					if (pathReturnQueue.Count == 0)
					{
						break;
					}
					path = pathReturnQueue.Dequeue();
				}
				((IPathInternals)path).ReturnPath();
				((IPathInternals)path).AdvanceState(PathState.Returned);
				path.Release(pathsClaimedSilentlyBy, true);
				num2++;
				if (num2 > 5 && timeSlice)
				{
					num2 = 0;
					if (DateTime.UtcNow.Ticks >= num)
					{
						break;
					}
				}
			}
		}
	}
}
