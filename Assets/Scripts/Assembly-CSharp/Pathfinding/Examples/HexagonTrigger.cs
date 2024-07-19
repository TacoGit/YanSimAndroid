using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding.Examples
{
	[RequireComponent(typeof(Animator))]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_hexagon_trigger.php")]
	public class HexagonTrigger : MonoBehaviour
	{
		public Button button;

		private Animator anim;

		private bool visible;

		private void Awake()
		{
			anim = GetComponent<Animator>();
			button.interactable = false;
		}

		private void OnTriggerEnter(Collider coll)
		{
			TurnBasedAI componentInParent = coll.GetComponentInParent<TurnBasedAI>();
			GraphNode node = AstarPath.active.GetNearest(base.transform.position).node;
			if (componentInParent != null && componentInParent.targetNode == node)
			{
				button.interactable = true;
				visible = true;
				anim.CrossFade("show", 0.1f);
			}
		}

		private void OnTriggerExit(Collider coll)
		{
			if (coll.GetComponentInParent<TurnBasedAI>() != null && visible)
			{
				button.interactable = false;
				anim.CrossFade("hide", 0.1f);
			}
		}
	}
}
