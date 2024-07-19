using UnityEngine;

public class IDCardScript : MonoBehaviour
{
	public PromptScript Prompt;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			Prompt.Yandere.StolenObject = base.gameObject;
			Prompt.Yandere.Inventory.IDCard = true;
			Prompt.Yandere.TheftTimer = 1f;
			Prompt.Hide();
			base.gameObject.SetActive(false);
		}
	}
}
