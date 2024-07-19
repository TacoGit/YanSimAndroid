using UnityEngine;

public class PowerSwitchScript : MonoBehaviour
{
	public DrinkingFountainScript DrinkingFountain;

	public PowerOutletScript PowerOutlet;

	public GameObject Electricity;

	public PromptScript Prompt;

	public AudioSource MyAudio;

	public AudioClip[] Flick;

	public bool On;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			On = !On;
			if (On)
			{
				Prompt.Label[0].text = "     Turn Off";
				MyAudio.clip = Flick[1];
			}
			else
			{
				Prompt.Label[0].text = "     Turn On";
				MyAudio.clip = Flick[0];
			}
			CheckPuddle();
			MyAudio.Play();
		}
	}

	public void CheckPuddle()
	{
		if (On)
		{
			if (DrinkingFountain.Puddle != null && DrinkingFountain.Puddle.gameObject.activeInHierarchy && PowerOutlet.SabotagedOutlet.activeInHierarchy)
			{
				Electricity.SetActive(true);
			}
		}
		else
		{
			Electricity.SetActive(false);
		}
	}
}
