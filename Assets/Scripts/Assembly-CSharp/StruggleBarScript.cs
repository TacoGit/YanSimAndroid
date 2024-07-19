using UnityEngine;

public class StruggleBarScript : MonoBehaviour
{
	public ShoulderCameraScript ShoulderCamera;

	public PromptSwapScript ButtonPrompt;

	public UISprite[] ButtonPrompts;

	public YandereScript Yandere;

	public StudentScript Student;

	public Transform Spikes;

	public string CurrentButton = string.Empty;

	public bool Struggling;

	public bool Invincible;

	public float ButtonTimer;

	public float Intensity;

	public float Strength = 1f;

	public float Struggle;

	public float Victory;

	public int ButtonID;

	private void Start()
	{
		base.transform.localScale = Vector3.zero;
		ChooseButton();
	}

	private void Update()
	{
		if (Struggling)
		{
			Intensity = Mathf.MoveTowards(Intensity, 1f, Time.deltaTime);
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			Spikes.localEulerAngles = new Vector3(Spikes.localEulerAngles.x, Spikes.localEulerAngles.y, Spikes.localEulerAngles.z - Time.deltaTime * 360f);
			Victory -= Time.deltaTime * 10f * Strength * Intensity;
			if (ClubGlobals.Club == ClubType.MartialArts)
			{
				Victory = 100f;
			}
			if (Input.GetButtonDown(CurrentButton))
			{
				if (Invincible)
				{
					Victory += 100f;
				}
				Victory += Time.deltaTime * (500f + (float)(ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus) * 150f) * Intensity;
			}
			if (Victory >= 100f)
			{
				Victory = 100f;
			}
			if (Victory <= -100f)
			{
				Victory = -100f;
			}
			UISprite uISprite = ButtonPrompts[ButtonID];
			uISprite.transform.localPosition = new Vector3(Mathf.Lerp(uISprite.transform.localPosition.x, Victory * 6.5f, Time.deltaTime * 10f), uISprite.transform.localPosition.y, uISprite.transform.localPosition.z);
			Spikes.localPosition = new Vector3(uISprite.transform.localPosition.x, Spikes.localPosition.y, Spikes.localPosition.z);
			if (Victory == 100f)
			{
				Yandere.Won = true;
				Student.Lost = true;
				Struggling = false;
				Victory = 0f;
				return;
			}
			if (Victory == -100f)
			{
				if (!Invincible)
				{
					HeroWins();
				}
				return;
			}
			ButtonTimer += Time.deltaTime;
			if (ButtonTimer >= 1f)
			{
				ChooseButton();
				ButtonTimer = 0f;
				Intensity = 0f;
			}
		}
		else if (base.transform.localScale.x > 0.1f)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
		}
		else
		{
			base.transform.localScale = Vector3.zero;
			base.gameObject.SetActive(false);
		}
	}

	public void HeroWins()
	{
		if (Yandere.Armed)
		{
			Yandere.EquippedWeapon.Drop();
		}
		Yandere.Lost = true;
		Student.Won = true;
		Struggling = false;
		Victory = 0f;
	}

	private void ChooseButton()
	{
		int buttonID = ButtonID;
		for (int i = 1; i < 5; i++)
		{
			ButtonPrompts[i].enabled = false;
			ButtonPrompts[i].transform.localPosition = ButtonPrompts[buttonID].transform.localPosition;
		}
		while (ButtonID == buttonID)
		{
			ButtonID = Random.Range(1, 5);
		}
		if (ButtonID == 1)
		{
			CurrentButton = "A";
		}
		else if (ButtonID == 2)
		{
			CurrentButton = "B";
		}
		else if (ButtonID == 3)
		{
			CurrentButton = "X";
		}
		else if (ButtonID == 4)
		{
			CurrentButton = "Y";
		}
		ButtonPrompts[ButtonID].enabled = true;
	}
}
