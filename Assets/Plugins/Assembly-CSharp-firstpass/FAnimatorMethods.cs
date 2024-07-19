using UnityEngine;

public static class FAnimatorMethods
{
	public static void LerpFloatValue(Animator animator, string name = "RunWalk", float value = 0f, float deltaSpeed = 8f)
	{
		float @float = animator.GetFloat(name);
		@float = FLogicMethods.FLerp(@float, value, Time.deltaTime * deltaSpeed);
		animator.SetFloat(name, @float);
	}

	public static bool CheckAnimationEnd(Animator animator, int layer = 0, bool reverse = false, bool checkAnimLoop = true)
	{
		AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
		if (!animator.IsInTransition(layer))
		{
			if (checkAnimLoop)
			{
				if (!currentAnimatorStateInfo.loop && !reverse)
				{
					if (currentAnimatorStateInfo.normalizedTime > 0.98f)
					{
						return true;
					}
					if (currentAnimatorStateInfo.normalizedTime < 0.02f)
					{
						return true;
					}
				}
			}
			else if (!reverse)
			{
				if (currentAnimatorStateInfo.normalizedTime > 0.98f)
				{
					return true;
				}
				if (currentAnimatorStateInfo.normalizedTime < 0.02f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void ResetLayersWeights(Animator animator, float speed = 10f)
	{
		for (int i = 1; i < animator.layerCount; i++)
		{
			animator.SetLayerWeight(i, FLogicMethods.FLerp(animator.GetLayerWeight(i), 0f, Time.deltaTime * speed));
		}
	}

	public static void LerpLayerWeight(Animator animator, int layer = 0, float newValue = 1f, float speed = 8f)
	{
		float layerWeight = animator.GetLayerWeight(layer);
		layerWeight = FLogicMethods.FLerp(layerWeight, newValue, Time.deltaTime * speed);
		animator.SetLayerWeight(layer, layerWeight);
	}

	public static bool StateExists(Animator animator, string clipName, int layer = 0)
	{
		int stateID = Animator.StringToHash(clipName);
		return animator.HasState(layer, stateID);
	}
}
