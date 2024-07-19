using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	[AddComponentMenu("Pathfinding/Modifiers/Funnel")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_funnel_modifier.php")]
	public class FunnelModifier : MonoModifier
	{
		public bool unwrap = true;

		public bool splitAtEveryPortal;

		public override int Order
		{
			get
			{
				return 10;
			}
		}

		public override void Apply(Path p)
		{
			if (p.path == null || p.path.Count == 0 || p.vectorPath == null || p.vectorPath.Count == 0)
			{
				return;
			}
			List<Vector3> list = ListPool<Vector3>.Claim();
			List<Funnel.PathPart> list2 = Funnel.SplitIntoParts(p);
			if (list2.Count == 0)
			{
				return;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				Funnel.PathPart part = list2[i];
				if (!part.isLink)
				{
					Funnel.FunnelPortals funnel = Funnel.ConstructFunnelPortals(p.path, part);
					List<Vector3> list3 = Funnel.Calculate(funnel, unwrap, splitAtEveryPortal);
					list.AddRange(list3);
					ListPool<Vector3>.Release(ref funnel.left);
					ListPool<Vector3>.Release(ref funnel.right);
					ListPool<Vector3>.Release(ref list3);
				}
				else
				{
					if (i == 0 || list2[i - 1].isLink)
					{
						list.Add(part.startPoint);
					}
					if (i == list2.Count - 1 || list2[i + 1].isLink)
					{
						list.Add(part.endPoint);
					}
				}
			}
			ListPool<Funnel.PathPart>.Release(ref list2);
			ListPool<Vector3>.Release(ref p.vectorPath);
			p.vectorPath = list;
		}
	}
}
