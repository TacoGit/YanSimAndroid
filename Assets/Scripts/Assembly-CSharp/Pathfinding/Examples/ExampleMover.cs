using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_example_mover.php")]
	public class ExampleMover : MonoBehaviour
	{
		private RVOExampleAgent agent;

		public Transform target;

		private void Awake()
		{
			agent = GetComponent<RVOExampleAgent>();
		}

		private void Start()
		{
			agent.SetTarget(target.position);
		}

		private void LateUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				agent.SetTarget(target.position);
			}
		}
	}
}
