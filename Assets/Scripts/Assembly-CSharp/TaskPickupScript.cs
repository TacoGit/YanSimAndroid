using UnityEngine;

public class TaskPickupScript : MonoBehaviour
{
	public PromptScript Prompt;

	public int ButtonID = 3;

	private void Update()
	{
		if (Prompt.Circle[ButtonID].fillAmount == 0f)
		{
			Prompt.Yandere.StudentManager.TaskManager.CheckTaskPickups();
		}
	}
}
