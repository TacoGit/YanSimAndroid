using UnityEngine;

public class PaintBucketScript : MonoBehaviour
{
	public PromptScript Prompt;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (Prompt.Yandere.Bloodiness == 0f)
			{
				Prompt.Yandere.Bloodiness += 100f;
				Prompt.Yandere.RedPaint = true;
			}
		}
	}
}
