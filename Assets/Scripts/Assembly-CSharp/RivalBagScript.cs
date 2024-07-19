using UnityEngine;

public class RivalBagScript : MonoBehaviour
{
	public SchemesScript Schemes;

	public ClockScript Clock;

	public PromptScript Prompt;

	private void Update()
	{
		if (Clock.Period == 2 || Clock.Period == 4)
		{
			Prompt.HideButton[0] = true;
		}
		else if (Prompt.Yandere.Inventory.Cigs)
		{
			Prompt.HideButton[0] = false;
		}
		else
		{
			Prompt.HideButton[0] = true;
		}
		if (Prompt.Yandere.Inventory.Cigs && Prompt.Circle[0].fillAmount == 0f)
		{
			SchemeGlobals.SetSchemeStage(3, 4);
			Schemes.UpdateInstructions();
			Prompt.Yandere.Inventory.Cigs = false;
			Prompt.HideButton[0] = true;
			base.enabled = false;
		}
		if (Clock.Period == 2 || Clock.Period == 4)
		{
			Prompt.HideButton[1] = true;
		}
		else if (Prompt.Yandere.Inventory.Ring)
		{
			Prompt.HideButton[1] = false;
		}
		else
		{
			Prompt.HideButton[1] = true;
		}
		if (Prompt.Yandere.Inventory.Ring && Prompt.Circle[1].fillAmount == 0f)
		{
			SchemeGlobals.SetSchemeStage(2, 3);
			Schemes.UpdateInstructions();
			Prompt.Yandere.Inventory.Ring = false;
			Prompt.HideButton[1] = true;
			base.enabled = false;
		}
	}
}
