using UnityEngine;

public class LeaveGiftScript : MonoBehaviour
{
	public PromptScript Prompt;

	public GameObject Box;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Hide();
			Prompt.enabled = false;
			Box.SetActive(true);
			base.enabled = false;
		}
	}
}
