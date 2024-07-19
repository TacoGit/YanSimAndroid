using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_AnimatorPlayState : MonoBehaviour
	{
		public string AnimationStateName = "Idle";

		public int AnimationLayer;

		[Tooltip("Normalized time so go from 0 to 1")]
		public Vector2 TimeOffset = Vector2.zero;

		private void Start()
		{
			Animator componentInChildren = GetComponentInChildren<Animator>();
			if ((bool)componentInChildren)
			{
				componentInChildren.Play(AnimationStateName, AnimationLayer, Random.Range(TimeOffset.x, TimeOffset.y));
			}
			Object.Destroy(this);
		}
	}
}
