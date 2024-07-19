using UnityEngine;

public class CigsScript : MonoBehaviour
{
	public PromptScript Prompt;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			SchemeGlobals.SetSchemeStage(3, 3);
			Prompt.Yandere.Inventory.Schemes.UpdateInstructions();
			Prompt.Yandere.Inventory.Cigs = true;
			Object.Destroy(base.gameObject);
			Prompt.Yandere.StudentManager.TaskManager.CheckTaskPickups();
		}
	}
}
