using UnityEngine;

public class RivalPhoneScript : MonoBehaviour
{
	public Renderer MyRenderer;

	public PromptScript Prompt;

	public bool LewdPhotos;

	public bool Stolen;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			SchemeGlobals.SetSchemeStage(4, 2);
			Prompt.Yandere.Inventory.Schemes.UpdateInstructions();
			Prompt.Yandere.RivalPhoneTexture = MyRenderer.material.mainTexture;
			Prompt.Yandere.Inventory.RivalPhone = true;
			Prompt.enabled = false;
			base.enabled = false;
			base.gameObject.SetActive(false);
			Stolen = true;
		}
	}
}
