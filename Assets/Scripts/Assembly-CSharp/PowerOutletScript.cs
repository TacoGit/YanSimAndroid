using UnityEngine;

public class PowerOutletScript : MonoBehaviour
{
	public PromptScript Prompt;

	public PowerSwitchScript PowerSwitch;

	public GameObject PowerStrip;

	public GameObject PluggedOutlet;

	public GameObject SabotagedOutlet;

	public bool Sabotaged;

	private void Update()
	{
		if (PowerStrip == null)
		{
			if (Prompt.Yandere.PickUp != null)
			{
				if (Prompt.Yandere.PickUp.Electronic)
				{
					Prompt.enabled = true;
				}
				else if (Prompt.enabled)
				{
					Prompt.Hide();
					Prompt.enabled = false;
				}
				if (Prompt.Circle[0].fillAmount == 0f)
				{
					Prompt.Circle[0].fillAmount = 1f;
					PowerStrip = Prompt.Yandere.PickUp.gameObject;
					Prompt.Yandere.EmptyHands();
					PowerStrip.transform.parent = base.transform;
					PowerStrip.transform.localPosition = new Vector3(0f, 0f, 0f);
					PowerStrip.SetActive(false);
					PluggedOutlet.SetActive(true);
					Prompt.HideButton[0] = true;
				}
			}
			else if (Prompt.enabled)
			{
				Prompt.Hide();
				Prompt.enabled = false;
			}
		}
		else if (Prompt.Yandere.EquippedWeapon != null)
		{
			if (Prompt.Yandere.EquippedWeapon.WeaponID == 6)
			{
				Prompt.HideButton[1] = false;
				Prompt.enabled = true;
			}
			else if (Prompt.enabled)
			{
				Prompt.Hide();
				Prompt.enabled = false;
			}
			if (Prompt.Circle[1].fillAmount == 0f)
			{
				Prompt.Circle[1].fillAmount = 1f;
				SabotagedOutlet.SetActive(true);
				PluggedOutlet.SetActive(false);
				PowerSwitch.CheckPuddle();
				Prompt.Hide();
				Prompt.enabled = false;
				base.enabled = false;
			}
		}
		else if (Prompt.enabled)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
	}
}
