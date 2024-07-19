using UnityEngine;

public class StolenPhoneSpotScript : MonoBehaviour
{
	public PromptScript Prompt;

	public GameObject RivalPhone;

	private void Update()
	{
		if (!Prompt.Yandere.Inventory.RivalPhone)
		{
			return;
		}
		Prompt.enabled = true;
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (SchemeGlobals.GetSchemeStage(4) == 3)
			{
				SchemeGlobals.SetSchemeStage(4, 4);
			}
			Prompt.Yandere.SmartphoneRenderer.material.mainTexture = Prompt.Yandere.YanderePhoneTexture;
			Prompt.Yandere.Inventory.Schemes.UpdateInstructions();
			Prompt.Yandere.Inventory.RivalPhone = false;
			Prompt.Yandere.RivalPhone = false;
			RivalPhone.transform.parent = null;
			RivalPhone.transform.position = base.transform.position;
			RivalPhone.transform.eulerAngles = base.transform.eulerAngles;
			RivalPhone.SetActive(true);
			Object.Destroy(base.gameObject);
		}
	}
}
