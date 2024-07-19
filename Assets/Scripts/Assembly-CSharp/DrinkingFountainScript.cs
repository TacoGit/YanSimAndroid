using UnityEngine;

public class DrinkingFountainScript : MonoBehaviour
{
	public PowerSwitchScript PowerSwitch;

	public ParticleSystem WaterStream;

	public Transform DrinkPosition;

	public GameObject Puddle;

	public GameObject Leak;

	public PromptScript Prompt;

	public AudioSource MyAudio;

	public bool Occupied;

	private void Update()
	{
		if (Prompt.Yandere.EquippedWeapon != null)
		{
			if (Prompt.Yandere.EquippedWeapon.Blood.enabled)
			{
				Prompt.HideButton[0] = false;
				Prompt.enabled = true;
			}
			else
			{
				Prompt.HideButton[0] = true;
			}
			if (!Leak.activeInHierarchy)
			{
				if (Prompt.Yandere.EquippedWeapon.WeaponID == 24)
				{
					Prompt.HideButton[1] = false;
					Prompt.enabled = true;
				}
				else
				{
					Prompt.HideButton[1] = true;
				}
			}
			if (Prompt.Circle[0].fillAmount == 0f)
			{
				Prompt.Circle[0].fillAmount = 1f;
				Prompt.Yandere.CharacterAnimation.CrossFade("f02_cleaningWeapon_00");
				Prompt.Yandere.Target = DrinkPosition;
				Prompt.Yandere.CleaningWeapon = true;
				Prompt.Yandere.CanMove = false;
				WaterStream.Play();
			}
			if (Prompt.Circle[1].fillAmount == 0f)
			{
				Prompt.HideButton[1] = true;
				Puddle.SetActive(true);
				Leak.SetActive(true);
				MyAudio.Play();
				PowerSwitch.CheckPuddle();
			}
		}
		else if (Prompt.enabled)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
	}
}
