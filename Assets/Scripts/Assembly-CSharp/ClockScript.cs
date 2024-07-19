using System;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
	private string MinuteNumber = string.Empty;

	private string HourNumber = string.Empty;

	public Collider[] TrespassZones;

	public StudentManagerScript StudentManager;

	public YandereScript Yandere;

	public PoliceScript Police;

	public ClockScript Clock;

	public Bloom BloomEffect;

	public MotionBlur Blur;

	public Transform PromptParent;

	public Transform MinuteHand;

	public Transform HourHand;

	public Transform Sun;

	public GameObject SunFlare;

	public UILabel PeriodLabel;

	public UILabel TimeLabel;

	public UILabel DayLabel;

	public Light MainLight;

	public float HalfwayTime;

	public float PresentTime;

	public float TargetTime;

	public float StartTime;

	public float HourTime;

	public float AmbientLightDim;

	public float CameraTimer;

	public float DayProgress;

	public float StartHour;

	public float TimeSpeed;

	public float Minute;

	public float Timer;

	public float Hour;

	public PhaseOfDay Phase;

	public int Period;

	public int Weekday;

	public int ID;

	public string TimeText = string.Empty;

	public bool IgnorePhotographyClub;

	public bool LateStudent;

	public bool UpdateBloom;

	public bool MissionMode;

	public bool StopTime;

	public bool TimeSkip;

	public bool FadeIn;

	public bool Horror;

	public AudioSource SchoolBell;

	public Color SkyboxColor;

	private void Start()
	{
		RenderSettings.ambientLight = new Color(0.75f, 0.75f, 0.75f);
		PeriodLabel.text = "BEFORE CLASS";
		PresentTime = StartHour * 60f;
		if (PlayerPrefs.GetInt("LoadingSave") == 1)
		{
			int profile = GameGlobals.Profile;
			int @int = PlayerPrefs.GetInt("SaveSlot");
			Debug.Log("Loading time! Profile_" + profile + "_Slot_" + @int + "_Time is " + PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_Time"));
			PresentTime = PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_Time");
			Weekday = PlayerPrefs.GetInt("Profile_" + profile + "_Slot_" + @int + "_Weekday");
			if (Weekday == 1)
			{
				DateGlobals.Weekday = DayOfWeek.Monday;
			}
			else if (Weekday == 2)
			{
				DateGlobals.Weekday = DayOfWeek.Tuesday;
			}
			else if (Weekday == 3)
			{
				DateGlobals.Weekday = DayOfWeek.Wednesday;
			}
			else if (Weekday == 4)
			{
				DateGlobals.Weekday = DayOfWeek.Thursday;
			}
			else if (Weekday == 5)
			{
				DateGlobals.Weekday = DayOfWeek.Friday;
			}
		}
		if (DateGlobals.Weekday == DayOfWeek.Sunday)
		{
			DateGlobals.Weekday = DayOfWeek.Monday;
		}
		if (!SchoolGlobals.SchoolAtmosphereSet)
		{
			SchoolGlobals.SchoolAtmosphereSet = true;
			SchoolGlobals.SchoolAtmosphere = 1f;
		}
		if (SchoolGlobals.SchoolAtmosphere < 0.5f)
		{
			BloomEffect.bloomIntensity = 0.2f;
			BloomEffect.bloomThreshhold = 0f;
			Police.Darkness.enabled = true;
			Police.Darkness.color = new Color(Police.Darkness.color.r, Police.Darkness.color.g, Police.Darkness.color.b, 1f);
			FadeIn = true;
		}
		else
		{
			BloomEffect.bloomIntensity = 10f;
			BloomEffect.bloomThreshhold = 0f;
			UpdateBloom = true;
		}
		BloomEffect.bloomThreshhold = 0f;
		DayLabel.text = GetWeekdayText(DateGlobals.Weekday);
		MainLight.color = new Color(1f, 1f, 1f, 1f);
		RenderSettings.ambientLight = new Color(0.75f, 0.75f, 0.75f, 1f);
		RenderSettings.skybox.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
		if (ClubGlobals.GetClubClosed(ClubType.Photography) || StudentGlobals.GetStudentGrudge(56) || StudentGlobals.GetStudentGrudge(57) || StudentGlobals.GetStudentGrudge(58) || StudentGlobals.GetStudentGrudge(59) || StudentGlobals.GetStudentGrudge(60))
		{
			IgnorePhotographyClub = true;
		}
		MissionMode = MissionModeGlobals.MissionMode;
	}

	private void Update()
	{
		if (FadeIn && Time.deltaTime < 1f)
		{
			Police.Darkness.color = new Color(Police.Darkness.color.r, Police.Darkness.color.g, Police.Darkness.color.b, Mathf.MoveTowards(Police.Darkness.color.a, 0f, Time.deltaTime));
			if (Police.Darkness.color.a == 0f)
			{
				Police.Darkness.enabled = false;
				FadeIn = false;
			}
		}
		if (!MissionMode && CameraTimer < 1f)
		{
			CameraTimer += Time.deltaTime;
			if (CameraTimer > 1f && !StudentManager.MemorialScene.enabled)
			{
				Yandere.RPGCamera.enabled = true;
				Yandere.CanMove = true;
			}
		}
		if (PresentTime < 1080f)
		{
			if (UpdateBloom)
			{
				BloomEffect.bloomIntensity = Mathf.MoveTowards(BloomEffect.bloomIntensity, 0.2f, Time.deltaTime * 5f);
				if (BloomEffect.bloomIntensity == 0.2f)
				{
					UpdateBloom = false;
				}
			}
		}
		else if (!Police.FadeOut && !Yandere.Attacking && !Yandere.Struggling && !Yandere.DelinquentFighting && !Yandere.Pickpocketing && !Yandere.Noticed)
		{
			Police.DayOver = true;
			Yandere.StudentManager.StopMoving();
			Police.Darkness.enabled = true;
			Police.FadeOut = true;
			StopTime = true;
		}
		if (!StopTime)
		{
			if (Period == 3)
			{
				PresentTime += Time.deltaTime * (1f / 60f) * TimeSpeed * 0.5f;
			}
			else
			{
				PresentTime += Time.deltaTime * (1f / 60f) * TimeSpeed;
			}
		}
		HourTime = PresentTime / 60f;
		Hour = Mathf.Floor(PresentTime / 60f);
		Minute = Mathf.Floor((PresentTime / 60f - Hour) * 60f);
		if (Hour == 0f || Hour == 12f)
		{
			HourNumber = "12";
		}
		else if (Hour < 12f)
		{
			HourNumber = Hour.ToString("f0");
		}
		else
		{
			HourNumber = (Hour - 12f).ToString("f0");
		}
		if (Minute < 10f)
		{
			MinuteNumber = "0" + Minute.ToString("f0");
		}
		else
		{
			MinuteNumber = Minute.ToString("f0");
		}
		TimeText = HourNumber + ":" + MinuteNumber + ((!(Hour < 12f)) ? " PM" : " AM");
		TimeLabel.text = TimeText;
		MinuteHand.localEulerAngles = new Vector3(MinuteHand.localEulerAngles.x, MinuteHand.localEulerAngles.y, Minute * 6f);
		HourHand.localEulerAngles = new Vector3(HourHand.localEulerAngles.x, HourHand.localEulerAngles.y, Hour * 30f);
		if (LateStudent && HourTime > 7.9f)
		{
			ActivateLateStudent();
		}
		if (HourTime < 8.5f)
		{
			if (Period < 1)
			{
				PeriodLabel.text = "BEFORE CLASS";
				DeactivateTrespassZones();
				Period++;
			}
		}
		else if (HourTime < 13f)
		{
			if (Period < 2)
			{
				PeriodLabel.text = "CLASS TIME";
				ActivateTrespassZones();
				Period++;
			}
		}
		else if (HourTime < 13.5f)
		{
			if (Period < 3)
			{
				PeriodLabel.text = "LUNCH TIME";
				StudentManager.DramaPhase = 0;
				StudentManager.UpdateDrama();
				DeactivateTrespassZones();
				Period++;
			}
		}
		else if (HourTime < 15.5f)
		{
			if (Period < 4)
			{
				PeriodLabel.text = "CLASS TIME";
				ActivateTrespassZones();
				Period++;
			}
		}
		else if (HourTime < 16f)
		{
			if (Period < 5)
			{
				GameObject[] graffiti = StudentManager.Graffiti;
				foreach (GameObject gameObject in graffiti)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(false);
					}
				}
				PeriodLabel.text = "CLEANING TIME";
				DeactivateTrespassZones();
				Period++;
			}
		}
		else if (Period < 6)
		{
			PeriodLabel.text = "AFTER SCHOOL";
			StudentManager.DramaPhase = 0;
			StudentManager.UpdateDrama();
			Period++;
		}
		if (!IgnorePhotographyClub && HourTime > 16.75f && StudentManager.SleuthPhase < 4)
		{
			StudentManager.SleuthPhase = 3;
			StudentManager.UpdateSleuths();
		}
		Sun.eulerAngles = new Vector3(Sun.eulerAngles.x, Sun.eulerAngles.y, -45f + 90f * (PresentTime - 420f) / 660f);
		if (!Horror)
		{
			if (StudentManager.WestBathroomArea.bounds.Contains(Yandere.transform.position) || StudentManager.EastBathroomArea.bounds.Contains(Yandere.transform.position))
			{
				AmbientLightDim = Mathf.MoveTowards(AmbientLightDim, 0.1f, Time.deltaTime);
			}
			else
			{
				AmbientLightDim = Mathf.MoveTowards(AmbientLightDim, 0.75f, Time.deltaTime);
			}
			if (PresentTime > 930f)
			{
				DayProgress = (PresentTime - 930f) / 150f;
				MainLight.color = new Color(1f - 0.14901961f * DayProgress, 1f - 0.40392157f * DayProgress, 1f - 0.70980394f * DayProgress);
				RenderSettings.ambientLight = new Color(1f - 0.14901961f * DayProgress - (1f - AmbientLightDim) * (1f - DayProgress), 1f - 0.40392157f * DayProgress - (1f - AmbientLightDim) * (1f - DayProgress), 1f - 0.70980394f * DayProgress - (1f - AmbientLightDim) * (1f - DayProgress));
				SkyboxColor = new Color(1f - 0.14901961f * DayProgress - 0.5f * (1f - DayProgress), 1f - 0.40392157f * DayProgress - 0.5f * (1f - DayProgress), 1f - 0.70980394f * DayProgress - 0.5f * (1f - DayProgress));
				RenderSettings.skybox.SetColor("_Tint", new Color(SkyboxColor.r, SkyboxColor.g, SkyboxColor.b));
			}
			else
			{
				RenderSettings.ambientLight = new Color(AmbientLightDim, AmbientLightDim, AmbientLightDim);
			}
		}
		if (!TimeSkip)
		{
			return;
		}
		if (HalfwayTime == 0f)
		{
			HalfwayTime = PresentTime + (TargetTime - PresentTime) * 0.5f;
			Yandere.TimeSkipHeight = Yandere.transform.position.y;
			Yandere.Phone.SetActive(true);
			Yandere.TimeSkipping = true;
			Yandere.CanMove = false;
			Blur.enabled = true;
			if (Yandere.Armed)
			{
				Yandere.Unequip();
			}
		}
		if (Time.timeScale < 25f)
		{
			Time.timeScale += 1f;
		}
		Yandere.Character.GetComponent<Animation>()["f02_timeSkip_00"].speed = 1f / Time.timeScale;
		Blur.blurAmount = 0.92f * (Time.timeScale / 100f);
		if (PresentTime > TargetTime)
		{
			EndTimeSkip();
		}
		if (Yandere.CameraEffects.Streaks.color.a > 0f || Yandere.CameraEffects.MurderStreaks.color.a > 0f || Yandere.NearSenpai || Input.GetButtonDown("Start"))
		{
			EndTimeSkip();
		}
	}

	public void EndTimeSkip()
	{
		PromptParent.localScale = new Vector3(1f, 1f, 1f);
		Yandere.Phone.SetActive(false);
		Yandere.TimeSkipping = false;
		Blur.enabled = false;
		Time.timeScale = 1f;
		TimeSkip = false;
		HalfwayTime = 0f;
		if (!Yandere.Noticed && !Police.FadeOut)
		{
			Yandere.CharacterAnimation.CrossFade(Yandere.IdleAnim);
			Yandere.CanMoveTimer = 0.5f;
		}
	}

	public string GetWeekdayText(DayOfWeek weekday)
	{
		switch (weekday)
		{
		case DayOfWeek.Sunday:
			Weekday = 0;
			return "SUNDAY";
		case DayOfWeek.Monday:
			Weekday = 1;
			return "MONDAY";
		case DayOfWeek.Tuesday:
			Weekday = 2;
			return "TUESDAY";
		case DayOfWeek.Wednesday:
			Weekday = 3;
			return "WEDNESDAY";
		case DayOfWeek.Thursday:
			Weekday = 4;
			return "THURSDAY";
		case DayOfWeek.Friday:
			Weekday = 5;
			return "FRIDAY";
		default:
			Weekday = 6;
			return "SATURDAY";
		}
	}

	private void ActivateTrespassZones()
	{
		SchoolBell.Play();
		Collider[] trespassZones = TrespassZones;
		foreach (Collider collider in trespassZones)
		{
			collider.enabled = true;
		}
	}

	public void DeactivateTrespassZones()
	{
		Yandere.Trespassing = false;
		SchoolBell.Play();
		Collider[] trespassZones = TrespassZones;
		foreach (Collider collider in trespassZones)
		{
			if (!collider.GetComponent<TrespassScript>().OffLimits)
			{
				collider.enabled = false;
			}
		}
	}

	public void ActivateLateStudent()
	{
		if (StudentManager.Students[7] != null)
		{
			StudentManager.Students[7].gameObject.SetActive(true);
			StudentManager.Students[7].Pathfinding.speed = 4f;
			StudentManager.Students[7].Spawned = true;
			StudentManager.Students[7].Hurry = true;
		}
		LateStudent = false;
	}
}
