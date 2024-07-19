using System;
using UnityEngine;

namespace FIMSpace.Basics
{
	[Serializable]
	public class FBasic_FheelekAnimator
	{
		public bool AnimationHolder;

		protected FBasic_FheelekController controller;

		protected Animator animator;

		protected string lastAnimation = string.Empty;

		protected bool waitForIdle;

		protected float landingTimer;

		protected string defaultIdle = "Idle";

		protected string defaultRun = "Run";

		protected int locomotionLayer;

		public FBasic_FheelekAnimator(FBasic_FheelekController contr)
		{
			controller = contr;
			animator = controller.GetComponent<Animator>();
		}

		internal void Animate(float acc)
		{
			if (AnimationHolder && waitForIdle)
			{
				if (!animator.GetNextAnimatorStateInfo(locomotionLayer).IsName(lastAnimation))
				{
					if (animator.IsInTransition(locomotionLayer))
					{
						waitForIdle = false;
					}
					else if (!animator.GetCurrentAnimatorStateInfo(locomotionLayer).IsName(lastAnimation))
					{
						waitForIdle = false;
					}
				}
				if (!waitForIdle)
				{
					AnimationHolder = false;
				}
				return;
			}
			if (!AnimationHolder)
			{
				if (!controller.Grounded && controller.CharacterRigidbody.velocity.y < 0f && Physics.Raycast(controller.transform.position, -controller.transform.up, 0.1f - controller.CharacterRigidbody.velocity.y * Time.fixedDeltaTime))
				{
					Landing();
				}
				landingTimer -= Time.deltaTime;
			}
			else if (!animator.IsInTransition(locomotionLayer) && animator.GetCurrentAnimatorStateInfo(locomotionLayer).normalizedTime > 0.7f)
			{
				AnimationHolder = false;
			}
			if (AnimationHolder)
			{
				return;
			}
			if (controller.Grounded)
			{
				if (acc < -0.1f || acc > 0.1f)
				{
					CrossfadeTo(defaultRun, 0.25f, locomotionLayer);
					LerpValue("AnimSpeed", acc / controller.MaxSpeed * 8f);
				}
				else
				{
					CrossfadeTo(defaultIdle, 0.25f, locomotionLayer);
				}
			}
			else if (controller.CharacterRigidbody.velocity.y > 0f)
			{
				CrossfadeTo("Jump", 0.15f, locomotionLayer);
			}
			else
			{
				CrossfadeTo("Falling", 0.24f, locomotionLayer);
			}
		}

		public void PlayAnimationHoldUntilIdle(string animation, float crossfadeTime = 0.2f, int animationLayer = 0)
		{
			CrossfadeTo(animation, crossfadeTime, animationLayer);
			AnimationHolder = true;
			waitForIdle = true;
		}

		internal void SetDefaultIdle(string stateName)
		{
			defaultIdle = stateName;
		}

		internal void SetDefaultRun(string stateName)
		{
			defaultRun = stateName;
		}

		protected void CrossfadeTo(string animation, float time = 0.25f, int animationLayer = 0)
		{
			if (lastAnimation != animation)
			{
				animator.CrossFadeInFixedTime(animation, time, animationLayer);
				lastAnimation = animation;
			}
		}

		public virtual void Jump()
		{
		}

		protected virtual void Landing()
		{
			if (!(landingTimer > 0f))
			{
				if (controller.CharacterRigidbody.velocity.y < -4.5f)
				{
					CrossfadeTo("Landing", 0.1f, locomotionLayer);
					AnimationHolder = true;
				}
				landingTimer = 0.5f;
			}
		}

		private void LerpValue(string parameter, float value)
		{
			FAnimatorMethods.LerpFloatValue(animator, parameter, value, 5f);
		}
	}
}
