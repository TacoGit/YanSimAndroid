using UnityEngine;

public class MaskingTapeScript : MonoBehaviour
{
	public CarryableCardboardBoxScript Box;

	public PromptScript Prompt;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Yandere.Inventory.MaskingTape = true;
			Box.Prompt.enabled = true;
			Box.enabled = true;
			Object.Destroy(base.gameObject);
		}
	}
}
