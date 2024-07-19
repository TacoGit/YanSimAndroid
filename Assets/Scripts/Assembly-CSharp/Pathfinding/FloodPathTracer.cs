using System;
using UnityEngine;

namespace Pathfinding
{
	public class FloodPathTracer : ABPath
	{
		protected FloodPath flood;

		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		public static FloodPathTracer Construct(Vector3 start, FloodPath flood, OnPathDelegate callback = null)
		{
			FloodPathTracer floodPathTracer = PathPool.GetPath<FloodPathTracer>();
			floodPathTracer.Setup(start, flood, callback);
			return floodPathTracer;
		}

		protected void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
		{
			this.flood = flood;
			if (flood == null || flood.PipelineState < PathState.Returned)
			{
				throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
			}
			Setup(start, flood.originalStartPoint, callback);
			nnConstraint = new FloodPathConstraint(flood);
		}

		protected override void Reset()
		{
			base.Reset();
			flood = null;
		}

		protected override void Initialize()
		{
			if (startNode != null && flood.HasPathTo(startNode))
			{
				Trace(startNode);
				base.CompleteState = PathCompleteState.Complete;
			}
			else
			{
				FailWithError("Could not find valid start node");
			}
		}

		protected override void CalculateStep(long targetTick)
		{
			if (!IsDone())
			{
				throw new Exception("Something went wrong. At this point the path should be completed");
			}
		}

		public void Trace(GraphNode from)
		{
			GraphNode graphNode = from;
			int num = 0;
			while (graphNode != null)
			{
				path.Add(graphNode);
				vectorPath.Add((Vector3)graphNode.position);
				graphNode = flood.GetParent(graphNode);
				num++;
				if (num > 1024)
				{
					Debug.LogWarning("Inifinity loop? >1024 node path. Remove this message if you really have that long paths (FloodPathTracer.cs, Trace function)");
					break;
				}
			}
		}
	}
}
