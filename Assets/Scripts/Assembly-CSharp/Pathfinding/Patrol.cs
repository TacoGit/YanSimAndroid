using UnityEngine;

namespace Pathfinding
{
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_patrol.php")]
	public class Patrol : VersionedMonoBehaviour
	{
		public Transform[] targets;

		public float delay;

		private int index;

		private IAstarAI agent;

		private float switchTime = float.PositiveInfinity;

		protected override void Awake()
		{
			base.Awake();
			agent = GetComponent<IAstarAI>();
		}

		private void Update()
		{
			if (targets.Length != 0)
			{
				bool flag = false;
				if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
				{
					switchTime = Time.time + delay;
				}
				if (Time.time >= switchTime)
				{
					index++;
					flag = true;
					switchTime = float.PositiveInfinity;
				}
				index %= targets.Length;
				agent.destination = targets[index].position;
				if (flag)
				{
					agent.SearchPath();
				}
			}
		}
	}
}
