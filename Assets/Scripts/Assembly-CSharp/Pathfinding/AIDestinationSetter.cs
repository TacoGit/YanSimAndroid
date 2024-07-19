using System;
using UnityEngine;

namespace Pathfinding
{
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour
	{
		public Transform target;

		public IAstarAI ai;

		private void OnEnable()
		{
			ai = GetComponent<IAstarAI>();
			if (ai != null)
			{
				IAstarAI astarAI = ai;
				astarAI.onSearchPath = (Action)Delegate.Combine(astarAI.onSearchPath, new Action(Update));
			}
		}

		private void OnDisable()
		{
			if (ai != null)
			{
				IAstarAI astarAI = ai;
				astarAI.onSearchPath = (Action)Delegate.Remove(astarAI.onSearchPath, new Action(Update));
			}
		}

		private void Update()
		{
			if (target != null && ai != null)
			{
				ai.destination = target.position;
			}
		}
	}
}
