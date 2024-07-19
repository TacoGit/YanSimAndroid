using UnityEngine;

public class RingScript : MonoBehaviour
{
	public PromptScript Prompt;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			SchemeGlobals.SetSchemeStage(2, 2);
			Prompt.Yandere.Inventory.Schemes.UpdateInstructions();
			Prompt.Yandere.Inventory.Ring = true;
			Object.Destroy(base.gameObject);
		}
	}
}
