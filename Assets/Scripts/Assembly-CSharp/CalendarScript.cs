using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalendarScript : MonoBehaviour
{
	public SelectiveGrayscale GrayscaleEffect;

	public ChallengeScript Challenge;

	public Vignetting Vignette;

	public GameObject ConfirmationWindow;

	public UILabel AtmosphereLabel;

	public UIPanel ChallengePanel;

	public UIPanel CalendarPanel;

	public UISprite Darkness;

	public UITexture Cloud;

	public UITexture Sun;

	public Transform Highlight;

	public Transform Continue;

	public bool Incremented;

	public bool LoveSick;

	public bool FadeOut;

	public bool Switch;

	public bool Reset;

	public float Timer;

	public int Phase = 1;

	private void Start()
	{
		LoveSickCheck();
		if (!SchoolGlobals.SchoolAtmosphereSet)
		{
			SchoolGlobals.SchoolAtmosphereSet = true;
			SchoolGlobals.SchoolAtmosphere = 1f;
		}
		if (SchoolGlobals.SchoolAtmosphere > 1f)
		{
			SchoolGlobals.SchoolAtmosphere = 1f;
		}
		if (DateGlobals.Weekday > DayOfWeek.Thursday)
		{
			DateGlobals.Weekday = DayOfWeek.Sunday;
			Globals.DeleteAll();
		}
		if (DateGlobals.PassDays < 1)
		{
			DateGlobals.PassDays = 1;
		}
		Sun.color = new Color(Sun.color.r, Sun.color.g, Sun.color.b, SchoolGlobals.SchoolAtmosphere);
		Cloud.color = new Color(Cloud.color.r, Cloud.color.g, Cloud.color.b, 1f - SchoolGlobals.SchoolAtmosphere);
		AtmosphereLabel.text = (SchoolGlobals.SchoolAtmosphere * 100f).ToString("f0") + "%";
		float num = 1f - SchoolGlobals.SchoolAtmosphere;
		GrayscaleEffect.desaturation = num;
		Vignette.intensity = num * 5f;
		Vignette.blur = num;
		Vignette.chromaticAberration = num;
		Continue.transform.localPosition = new Vector3(Continue.transform.localPosition.x, -610f, Continue.transform.localPosition.z);
		Challenge.ViewButton.SetActive(false);
		Challenge.LargeIcon.color = new Color(Challenge.LargeIcon.color.r, Challenge.LargeIcon.color.g, Challenge.LargeIcon.color.b, 0f);
		Challenge.Panels[1].alpha = 0.5f;
		Challenge.Shadow.color = new Color(Challenge.Shadow.color.r, Challenge.Shadow.color.g, Challenge.Shadow.color.b, 0f);
		ChallengePanel.alpha = 0f;
		CalendarPanel.alpha = 1f;
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 1f);
		Time.timeScale = 1f;
		Highlight.localPosition = new Vector3(-600f + 200f * (float)DateGlobals.Weekday, Highlight.localPosition.y, Highlight.localPosition.z);
		LoveSickCheck();
	}

	private void Update()
	{
		Timer += Time.deltaTime;
		if (!FadeOut)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a - Time.deltaTime);
			if (Darkness.color.a < 0f)
			{
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 0f);
			}
			if (Timer > 1f)
			{
				if (!Incremented)
				{
					while (DateGlobals.PassDays > 0)
					{
						DateGlobals.Weekday++;
						DateGlobals.PassDays--;
					}
					Incremented = true;
					GetComponent<AudioSource>().Play();
				}
				else
				{
					Highlight.localPosition = new Vector3(Mathf.Lerp(Highlight.localPosition.x, -600f + 200f * (float)DateGlobals.Weekday, Time.deltaTime * 10f), Highlight.localPosition.y, Highlight.localPosition.z);
				}
				if (Timer > 2f)
				{
					Continue.localPosition = new Vector3(Continue.localPosition.x, Mathf.Lerp(Continue.localPosition.y, -500f, Time.deltaTime * 10f), Continue.localPosition.z);
					if (!Switch)
					{
						if (!ConfirmationWindow.activeInHierarchy)
						{
							if (Input.GetButtonDown("A"))
							{
								FadeOut = true;
							}
							if (Input.GetButtonDown("Y"))
							{
								Switch = true;
							}
							if (Input.GetButtonDown("B"))
							{
								ConfirmationWindow.SetActive(true);
							}
							if (Input.GetKeyDown(KeyCode.Z))
							{
								if (SchoolGlobals.SchoolAtmosphere > 0f)
								{
									SchoolGlobals.SchoolAtmosphere -= 0.1f;
								}
								else
								{
									SchoolGlobals.SchoolAtmosphere = 100f;
								}
								SceneManager.LoadScene(SceneManager.GetActiveScene().name);
							}
						}
						else
						{
							if (Input.GetButtonDown("A"))
							{
								FadeOut = true;
								Reset = true;
							}
							if (Input.GetButtonDown("B"))
							{
								ConfirmationWindow.SetActive(false);
							}
						}
					}
				}
			}
		}
		else
		{
			Continue.localPosition = new Vector3(Continue.localPosition.x, Mathf.Lerp(Continue.localPosition.y, -610f, Time.deltaTime * 10f), Continue.localPosition.z);
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a + Time.deltaTime);
			if (Darkness.color.a >= 1f)
			{
				if (Reset)
				{
					int profile = GameGlobals.Profile;
					Globals.DeleteAll();
					PlayerPrefs.SetInt("ProfileCreated_" + profile, 1);
					GameGlobals.Profile = profile;
					GameGlobals.LoveSick = LoveSick;
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				else
				{
					if (HomeGlobals.Night)
					{
						HomeGlobals.Night = false;
					}
					SceneManager.LoadScene("HomeScene");
				}
			}
		}
		if (Switch)
		{
			if (Phase == 1)
			{
				CalendarPanel.alpha -= Time.deltaTime;
				if (CalendarPanel.alpha <= 0f)
				{
					Phase++;
				}
			}
			else
			{
				ChallengePanel.alpha += Time.deltaTime;
				if (ChallengePanel.alpha >= 1f)
				{
					Challenge.enabled = true;
					base.enabled = false;
					Switch = false;
					Phase = 1;
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			DateGlobals.Weekday = DayOfWeek.Monday;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			DateGlobals.Weekday = DayOfWeek.Tuesday;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			DateGlobals.Weekday = DayOfWeek.Wednesday;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			DateGlobals.Weekday = DayOfWeek.Thursday;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			DateGlobals.Weekday = DayOfWeek.Friday;
		}
	}

	public void LoveSickCheck()
	{
		if (!GameGlobals.LoveSick)
		{
			return;
		}
		SchoolGlobals.SchoolAtmosphereSet = true;
		SchoolGlobals.SchoolAtmosphere = 0f;
		LoveSick = true;
		Camera.main.backgroundColor = new Color(0f, 0f, 0f, 1f);
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			UISprite component = gameObject.GetComponent<UISprite>();
			if (component != null)
			{
				component.color = new Color(1f, 0f, 0f, component.color.a);
			}
			UITexture component2 = gameObject.GetComponent<UITexture>();
			if (component2 != null)
			{
				component2.color = new Color(1f, 0f, 0f, component2.color.a);
			}
			UILabel component3 = gameObject.GetComponent<UILabel>();
			if (component3 != null)
			{
				if (component3.color != Color.black)
				{
					component3.color = new Color(1f, 0f, 0f, component3.color.a);
				}
				if (component3.text == "?")
				{
					component3.color = new Color(1f, 0f, 0f, component3.color.a);
				}
			}
		}
		Darkness.color = Color.black;
		AtmosphereLabel.enabled = false;
		Cloud.enabled = false;
		Sun.enabled = false;
	}
}
