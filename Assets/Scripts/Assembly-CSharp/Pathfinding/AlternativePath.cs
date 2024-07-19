using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	[AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_alternative_path.php")]
	public class AlternativePath : MonoModifier
	{
		public int penalty = 1000;

		public int randomStep = 10;

		private List<GraphNode> prevNodes = new List<GraphNode>();

		private int prevPenalty;

		private readonly System.Random rnd = new System.Random();

		private bool destroyed;

		public override int Order
		{
			get
			{
				return 10;
			}
		}

		public override void Apply(Path p)
		{
			if (!(this == null))
			{
				ApplyNow(p.path);
			}
		}

		protected void OnDestroy()
		{
			destroyed = true;
			ClearOnDestroy();
		}

		private void ClearOnDestroy()
		{
			InversePrevious();
		}

		private void InversePrevious()
		{
			if (prevNodes == null)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < prevNodes.Count; i++)
			{
				if (prevNodes[i].Penalty < prevPenalty)
				{
					flag = true;
					prevNodes[i].Penalty = 0u;
				}
				else
				{
					prevNodes[i].Penalty = (uint)(prevNodes[i].Penalty - prevPenalty);
				}
			}
			if (flag)
			{
				Debug.LogWarning("Penalty for some nodes has been reset while the AlternativePath modifier was active (possibly because of a graph update). Some penalties might be incorrect (they may be lower than expected for the affected nodes)");
			}
		}

		private void ApplyNow(List<GraphNode> nodes)
		{
			InversePrevious();
			prevNodes.Clear();
			if (destroyed)
			{
				return;
			}
			if (nodes != null)
			{
				int num = rnd.Next(randomStep);
				for (int i = num; i < nodes.Count; i += rnd.Next(1, randomStep))
				{
					nodes[i].Penalty = (uint)(nodes[i].Penalty + penalty);
					prevNodes.Add(nodes[i]);
				}
			}
			prevPenalty = penalty;
		}
	}
}
