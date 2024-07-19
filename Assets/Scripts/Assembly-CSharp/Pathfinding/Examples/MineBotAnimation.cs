using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_mine_bot_animation.php")]
	public class MineBotAnimation : VersionedMonoBehaviour
	{
		public Animation anim;

		public float sleepVelocity = 0.4f;

		public float animationSpeed = 0.2f;

		public GameObject endOfPathEffect;

		private bool isAtDestination;

		private IAstarAI ai;

		private Transform tr;

		protected Vector3 lastTarget;

		protected override void Awake()
		{
			base.Awake();
			ai = GetComponent<IAstarAI>();
			tr = GetComponent<Transform>();
		}

		private void Start()
		{
			anim["forward"].layer = 10;
			anim.Play("awake");
			anim.Play("forward");
			anim["awake"].wrapMode = WrapMode.Once;
			anim["awake"].speed = 0f;
			anim["awake"].normalizedTime = 1f;
		}

		private void OnTargetReached()
		{
			if (endOfPathEffect != null && Vector3.Distance(tr.position, lastTarget) > 1f)
			{
				Object.Instantiate(endOfPathEffect, tr.position, tr.rotation);
				lastTarget = tr.position;
			}
		}

		protected void Update()
		{
			if (ai.reachedEndOfPath)
			{
				if (!isAtDestination)
				{
					OnTargetReached();
				}
				isAtDestination = true;
			}
			else
			{
				isAtDestination = false;
			}
			Vector3 vector = tr.InverseTransformDirection(ai.velocity);
			vector.y = 0f;
			if (vector.sqrMagnitude <= sleepVelocity * sleepVelocity)
			{
				anim.Blend("forward", 0f, 0.2f);
				return;
			}
			anim.Blend("forward", 1f, 0.2f);
			AnimationState animationState = anim["forward"];
			float z = vector.z;
			animationState.speed = z * animationSpeed;
		}
	}
}
