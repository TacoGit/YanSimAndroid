using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePrisonerScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public HomePrisonerChanScript Prisoner;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeWindowScript HomeWindow;

	public HomeDarknessScript Darkness;

	public UILabel[] OptionLabels;

	public string[] Descriptions;

	public Transform TortureDestination;

	public Transform TortureTarget;

	public GameObject NowLoading;

	public Transform Highlight;

	public AudioSource Jukebox;

	public UILabel SanityLabel;

	public UILabel DescLabel;

	public UILabel Subtitle;

	public bool PlayedAudio;

	public bool ZoomIn;

	public float Sanity = 100f;

	public float Timer;

	public int ID = 1;

	public AudioClip FirstTorture;

	public AudioClip Under50Torture;

	public AudioClip Over50Torture;

	public AudioClip TortureHit;

	public string[] FullSanityBanterText;

	public string[] HighSanityBanterText;

	public string[] LowSanityBanterText;

	public string[] NoSanityBanterText;

	public string[] BanterText;

	public AudioClip[] FullSanityBanter;

	public AudioClip[] HighSanityBanter;

	public AudioClip[] LowSanityBanter;

	public AudioClip[] NoSanityBanter;

	public AudioClip[] Banter;

	public float BanterTimer;

	public bool Bantering;

	public int BanterID;

	private void Start()
	{
		Sanity = StudentGlobals.GetStudentSanity(SchoolGlobals.KidnapVictim);
		SanityLabel.text = "Sanity: " + Sanity + "%";
		Prisoner.Sanity = Sanity;
		Subtitle.text = string.Empty;
		if (Sanity == 100f)
		{
			BanterText = FullSanityBanterText;
			Banter = FullSanityBanter;
		}
		else if (Sanity >= 50f)
		{
			BanterText = HighSanityBanterText;
			Banter = HighSanityBanter;
		}
		else if (Sanity == 0f)
		{
			BanterText = NoSanityBanterText;
			Banter = NoSanityBanter;
		}
		else
		{
			BanterText = LowSanityBanterText;
			Banter = LowSanityBanter;
		}
		if (Sanity < 100f)
		{
			Prisoner.Character.GetComponent<Animation>().CrossFade("f02_kidnapIdle_02");
		}
		if (!HomeGlobals.Night)
		{
			UILabel uILabel = OptionLabels[2];
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
			if (HomeGlobals.LateForSchool)
			{
				UILabel uILabel2 = OptionLabels[1];
				uILabel2.color = new Color(uILabel2.color.r, uILabel2.color.g, uILabel2.color.b, 0.5f);
			}
			if (DateGlobals.Weekday == DayOfWeek.Friday)
			{
				UILabel uILabel3 = OptionLabels[3];
				uILabel3.color = new Color(uILabel3.color.r, uILabel3.color.g, uILabel3.color.b, 0.5f);
				UILabel uILabel4 = OptionLabels[4];
				uILabel4.color = new Color(uILabel4.color.r, uILabel4.color.g, uILabel4.color.b, 0.5f);
			}
		}
		else
		{
			UILabel uILabel5 = OptionLabels[1];
			uILabel5.color = new Color(uILabel5.color.r, uILabel5.color.g, uILabel5.color.b, 0.5f);
			UILabel uILabel6 = OptionLabels[3];
			uILabel6.color = new Color(uILabel6.color.r, uILabel6.color.g, uILabel6.color.b, 0.5f);
			UILabel uILabel7 = OptionLabels[4];
			uILabel7.color = new Color(uILabel7.color.r, uILabel7.color.g, uILabel7.color.b, 0.5f);
		}
		if (Sanity > 0f)
		{
			OptionLabels[5].text = "?????";
			UILabel uILabel8 = OptionLabels[5];
			uILabel8.color = new Color(uILabel8.color.r, uILabel8.color.g, uILabel8.color.b, 0.5f);
		}
		else
		{
			OptionLabels[5].text = "Bring to School";
			UILabel uILabel9 = OptionLabels[1];
			uILabel9.color = new Color(uILabel9.color.r, uILabel9.color.g, uILabel9.color.b, 0.5f);
			UILabel uILabel10 = OptionLabels[2];
			uILabel10.color = new Color(uILabel10.color.r, uILabel10.color.g, uILabel10.color.b, 0.5f);
			UILabel uILabel11 = OptionLabels[3];
			uILabel11.color = new Color(uILabel11.color.r, uILabel11.color.g, uILabel11.color.b, 0.5f);
			UILabel uILabel12 = OptionLabels[4];
			uILabel12.color = new Color(uILabel12.color.r, uILabel12.color.g, uILabel12.color.b, 0.5f);
			UILabel uILabel13 = OptionLabels[5];
			uILabel13.color = new Color(uILabel13.color.r, uILabel13.color.g, uILabel13.color.b, 1f);
			if (HomeGlobals.Night)
			{
				uILabel13.color = new Color(uILabel13.color.r, uILabel13.color.g, uILabel13.color.b, 0.5f);
			}
		}
		UpdateDesc();
		if (SchoolGlobals.KidnapVictim == 0)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (Vector3.Distance(HomeYandere.transform.position, Prisoner.transform.position) < 2f && HomeYandere.CanMove)
		{
			BanterTimer += Time.deltaTime;
			if (BanterTimer > 5f && !Bantering)
			{
				Bantering = true;
				if (BanterID < Banter.Length - 1)
				{
					BanterID++;
					Subtitle.text = BanterText[BanterID];
					component.clip = Banter[BanterID];
					component.Play();
				}
			}
		}
		if (Bantering && !component.isPlaying)
		{
			Subtitle.text = string.Empty;
			Bantering = false;
			BanterTimer = 0f;
		}
		if (HomeYandere.CanMove || (!(HomeCamera.Destination == HomeCamera.Destinations[10]) && !(HomeCamera.Destination == TortureDestination)))
		{
			return;
		}
		if (InputManager.TappedDown)
		{
			ID++;
			if (ID > 5)
			{
				ID = 1;
			}
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, 465f - (float)ID * 40f, Highlight.localPosition.z);
			UpdateDesc();
		}
		if (InputManager.TappedUp)
		{
			ID--;
			if (ID < 1)
			{
				ID = 5;
			}
			Highlight.localPosition = new Vector3(Highlight.localPosition.x, 465f - (float)ID * 40f, Highlight.localPosition.z);
			UpdateDesc();
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			Sanity -= 10f;
			if (Sanity < 0f)
			{
				Sanity = 100f;
			}
			StudentGlobals.SetStudentSanity(SchoolGlobals.KidnapVictim, Sanity);
			SanityLabel.text = "Sanity: " + Sanity.ToString("f0") + "%";
			Prisoner.UpdateSanity();
		}
		if (!ZoomIn)
		{
			if (Input.GetButtonDown("A") && OptionLabels[ID].color.a == 1f)
			{
				if (Sanity > 0f)
				{
					if (Sanity == 100f)
					{
						Prisoner.Character.GetComponent<Animation>().CrossFade("f02_kidnapTorture_01");
					}
					else if (Sanity >= 50f)
					{
						Prisoner.Character.GetComponent<Animation>().CrossFade("f02_kidnapTorture_02");
					}
					else
					{
						Prisoner.Character.GetComponent<Animation>().CrossFade("f02_kidnapSurrender_00");
						TortureDestination.localPosition = new Vector3(TortureDestination.localPosition.x, 0f, 1f);
						TortureTarget.localPosition = new Vector3(TortureTarget.localPosition.x, 1.1f, TortureTarget.localPosition.z);
					}
					HomeCamera.Destination = TortureDestination;
					HomeCamera.Target = TortureTarget;
					HomeCamera.Torturing = true;
					Prisoner.Tortured = true;
					Prisoner.RightEyeRotOrigin.x = -6f;
					Prisoner.LeftEyeRotOrigin.x = 6f;
					ZoomIn = true;
				}
				else
				{
					Darkness.FadeOut = true;
				}
				HomeWindow.Show = false;
				HomeCamera.PromptBar.ClearButtons();
				HomeCamera.PromptBar.Show = false;
				Jukebox.volume -= 0.5f;
			}
			if (Input.GetButtonDown("B"))
			{
				HomeCamera.Destination = HomeCamera.Destinations[0];
				HomeCamera.Target = HomeCamera.Targets[0];
				HomeCamera.PromptBar.ClearButtons();
				HomeCamera.PromptBar.Show = false;
				HomeYandere.CanMove = true;
				HomeYandere.gameObject.SetActive(true);
				HomeWindow.Show = false;
			}
			return;
		}
		TortureDestination.Translate(Vector3.forward * (Time.deltaTime * 0.02f));
		Jukebox.volume -= Time.deltaTime * 0.05f;
		Timer += Time.deltaTime;
		if (Sanity >= 50f)
		{
			TortureDestination.localPosition = new Vector3(TortureDestination.localPosition.x, TortureDestination.localPosition.y + Time.deltaTime * 0.05f, TortureDestination.localPosition.z);
			if (Sanity == 100f)
			{
				if (Timer > 2f && !PlayedAudio)
				{
					component.clip = FirstTorture;
					PlayedAudio = true;
					component.Play();
				}
			}
			else if (Timer > 1.5f && !PlayedAudio)
			{
				component.clip = Over50Torture;
				PlayedAudio = true;
				component.Play();
			}
		}
		else if (Timer > 5f && !PlayedAudio)
		{
			component.clip = Under50Torture;
			PlayedAudio = true;
			component.Play();
		}
		if (Timer > 10f && Darkness.Sprite.color.a != 1f)
		{
			Darkness.enabled = false;
			Darkness.Sprite.color = new Color(Darkness.Sprite.color.r, Darkness.Sprite.color.g, Darkness.Sprite.color.b, 1f);
			component.clip = TortureHit;
			component.Play();
		}
		if (Timer > 15f)
		{
			if (ID == 1)
			{
				Time.timeScale = 1f;
				NowLoading.SetActive(true);
				HomeGlobals.LateForSchool = true;
				SceneManager.LoadScene("LoadingScene");
				StudentGlobals.SetStudentSanity(SchoolGlobals.KidnapVictim, Sanity - 2.5f);
			}
			else if (ID == 2)
			{
				SceneManager.LoadScene("CalendarScene");
				StudentGlobals.SetStudentSanity(SchoolGlobals.KidnapVictim, Sanity - 10f);
			}
			else if (ID == 3)
			{
				HomeGlobals.Night = true;
				SceneManager.LoadScene("HomeScene");
				StudentGlobals.SetStudentSanity(SchoolGlobals.KidnapVictim, Sanity - 30f);
				PlayerGlobals.Reputation -= 20f;
			}
			else if (ID == 4)
			{
				SceneManager.LoadScene("CalendarScene");
				StudentGlobals.SetStudentSanity(SchoolGlobals.KidnapVictim, Sanity - 45f);
				PlayerGlobals.Reputation -= 20f;
			}
			if (StudentGlobals.GetStudentSanity(SchoolGlobals.KidnapVictim) < 0f)
			{
				StudentGlobals.SetStudentSanity(SchoolGlobals.KidnapVictim, 0f);
			}
		}
	}

	public void UpdateDesc()
	{
		HomeCamera.PromptBar.Label[0].text = "Accept";
		DescLabel.text = Descriptions[ID];
		if (!HomeGlobals.Night)
		{
			if (HomeGlobals.LateForSchool && ID == 1)
			{
				DescLabel.text = "This option is unavailable if you are late for school.";
				HomeCamera.PromptBar.Label[0].text = string.Empty;
			}
			if (ID == 2)
			{
				DescLabel.text = "This option is unavailable in the daytime.";
				HomeCamera.PromptBar.Label[0].text = string.Empty;
			}
			if (DateGlobals.Weekday == DayOfWeek.Friday && (ID == 3 || ID == 4))
			{
				DescLabel.text = "This option is unavailable on Friday.";
				HomeCamera.PromptBar.Label[0].text = string.Empty;
			}
		}
		else if (ID != 2)
		{
			DescLabel.text = "This option is unavailable at nighttime.";
			HomeCamera.PromptBar.Label[0].text = string.Empty;
		}
		if (ID == 5)
		{
			if (Sanity > 0f)
			{
				DescLabel.text = "This option is unavailable until your prisoner's Sanity has reached zero.";
			}
			if (HomeGlobals.Night)
			{
				DescLabel.text = "This option is unavailable at nighttime.";
				HomeCamera.PromptBar.Label[0].text = string.Empty;
			}
		}
		HomeCamera.PromptBar.UpdateButtons();
	}
}
