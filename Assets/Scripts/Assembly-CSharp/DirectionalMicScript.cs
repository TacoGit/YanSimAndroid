using UnityEngine;

public class DirectionalMicScript : MonoBehaviour
{
	public PromptScript Prompt;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			InventoryScript inventory = Prompt.Yandere.Inventory;
			inventory.DirectionalMic = true;
			Object.Destroy(base.gameObject);
		}
	}
}
