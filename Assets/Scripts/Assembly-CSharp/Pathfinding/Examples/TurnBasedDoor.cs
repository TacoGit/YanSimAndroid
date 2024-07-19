using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(SingleNodeBlocker))]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_turn_based_door.php")]
	public class TurnBasedDoor : MonoBehaviour
	{
		private Animator animator;

		private SingleNodeBlocker blocker;

		private bool open;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			blocker = GetComponent<SingleNodeBlocker>();
		}

		private void Start()
		{
			blocker.BlockAtCurrentPosition();
			animator.CrossFade("close", 0.2f);
		}

		public void Close()
		{
			StartCoroutine(WaitAndClose());
		}

		private IEnumerator WaitAndClose()
		{
			List<SingleNodeBlocker> selector = new List<SingleNodeBlocker> { blocker };
			GraphNode node = AstarPath.active.GetNearest(base.transform.position).node;
			if (blocker.manager.NodeContainsAnyExcept(node, selector))
			{
				animator.CrossFade("blocked", 0.2f);
			}
			while (blocker.manager.NodeContainsAnyExcept(node, selector))
			{
				yield return null;
			}
			open = false;
			animator.CrossFade("close", 0.2f);
			blocker.BlockAtCurrentPosition();
		}

		public void Open()
		{
			StopAllCoroutines();
			animator.CrossFade("open", 0.2f);
			open = true;
			blocker.Unblock();
		}

		public void Toggle()
		{
			if (open)
			{
				Close();
			}
			else
			{
				Open();
			}
		}
	}
}
