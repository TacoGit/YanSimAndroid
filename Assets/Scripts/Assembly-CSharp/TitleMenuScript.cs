using System;
using UnityEngine;
using UnityEngine.SceneManagement;
// FIXED BY TANOS [x]
public class TitleMenuScript : MonoBehaviour
{
	public ColorCorrectionCurves ColorCorrection;

	public InputManagerScript InputManager;

	public TitleSaveFilesScript SaveFiles;

	public SelectiveGrayscale Grayscale;

	public TitleSponsorScript Sponsors;

	public PromptBarScript PromptBar;

	public SSAOEffect SSAO;

	public JsonScript JSON;

	public UISprite[] MediumSprites;

	public UISprite[] LightSprites;

	public UISprite[] DarkSprites;

	public UILabel TitleLabel;

	public UILabel SimulatorLabel;

	public UILabel[] ColoredLabels;

	public Color MediumColor;

	public Color LightColor;

	public Color DarkColor;

	public Transform VictimHead;

	public Transform RightHand;

	public Transform TwintailL;

	public Transform TwintailR;

	public Animation LoveSickYandere;

	public GameObject BloodProjector;

	public GameObject LoveSickLogo;

	public GameObject BloodCamera;

	public GameObject Yandere;

	public GameObject Knife;

	public GameObject Logo;

	public GameObject Sun;

	public AudioSource LoveSickMusic;

	public AudioSource CuteMusic;

	public AudioSource DarkMusic;

	public Renderer[] YandereEye;

	public Material CuteSkybox;

	public Material DarkSkybox;

	public Transform Highlight;

	public Transform[] Spine;

	public Transform[] Arm;

	public UISprite Darkness;

	public Vector3 PermaPositionL;

	public Vector3 PermaPositionR;

	public bool NeverChange;

	public bool LoveSick;

	public bool FadeOut;

	public bool Turning;

	public bool Fading = true;

	public int SelectionCount = 8;

	public int Selected;

	public float InputTimer;

	public float FadeSpeed = 1f;

	public float LateTimer;

	public float RotationY;

	public float RotationZ;

	public float Volume;

	public float Timer;

	private void Awake()
	{
		Animation component = Yandere.GetComponent<Animation>();
		component["f02_yanderePose_00"].layer = 1;
		component.Blend("f02_yanderePose_00");
		component["f02_fist_00"].layer = 2;
		component.Blend("f02_fist_00");
	}

	private void Start()
	{
		if (GameGlobals.LoveSick)
		{
			LoveSick = true;
		}
		PromptBar.Label[0].text = "Confirm";
		PromptBar.Label[1].text = string.Empty;
		PromptBar.UpdateButtons();
		MediumColor = MediumSprites[0].color;
		LightColor = LightSprites[0].color;
		DarkColor = DarkSprites[0].color;
		if (!LoveSick)
		{
			base.transform.position = new Vector3(base.transform.position.x, 1.2f, base.transform.position.z);
			LoveSickLogo.SetActive(false);
			LoveSickMusic.volume = 0f;
			Grayscale.enabled = false;
			SSAO.enabled = false;
			Sun.SetActive(true);
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 1f);
			TurnCute();
			RenderSettings.ambientLight = new Color(0.75f, 0.75f, 0.75f, 1f);
			RenderSettings.skybox.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
		}
		else
		{
			base.transform.position = new Vector3(base.transform.position.x, 101.2f, base.transform.position.z);
			Sun.SetActive(false);
			SSAO.enabled = true;
			FadeSpeed = 0.2f;
			Darkness.color = new Color(0f, 0f, 0f, 1f);
			TurnLoveSick();
		}
		Time.timeScale = 1f;
		if (JSON.Students[11].Name != "Reserved")
		{
			if (Application.CanStreamedLevelBeLoaded("FunScene"))
			{
				SceneManager.LoadScene("FunScene");
			}
			else if (Application.CanStreamedLevelBeLoaded("MoreFunScene"))
			{
				SceneManager.LoadScene("MoreFunScene");
			}
			else
			{
				Application.Quit();
			}
		}
	}

	private void Update()
	{
		if (LoveSick)
		{
			Timer += Time.deltaTime * 0.001f;
			if (base.transform.position.z > -18f)
			{
				LateTimer = Mathf.Lerp(LateTimer, Timer, Time.deltaTime);
				RotationY = Mathf.Lerp(RotationY, -22.5f, Time.deltaTime * LateTimer);
			}
			RotationZ = Mathf.Lerp(RotationZ, 22.5f, Time.deltaTime * Timer);
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0.33333f, 101.45f, -16.5f), Time.deltaTime * Timer);
			base.transform.eulerAngles = new Vector3(0f, RotationY, RotationZ);
			if (!Turning)
			{
				if (base.transform.position.z > -17f)
				{
					LoveSickYandere.CrossFade("f02_edgyTurn_00");
					VictimHead.parent = RightHand;
					Turning = true;
				}
			}
			else if (LoveSickYandere["f02_edgyTurn_00"].time >= LoveSickYandere["f02_edgyTurn_00"].length)
			{
				LoveSickYandere.CrossFade("f02_edgyOverShoulder_00");
			}
		}
		if (!Sponsors.Show && !SaveFiles.Show)
		{
			InputTimer += Time.deltaTime;
			if (InputTimer > 1f)
			{
				if (InputManager.TappedDown)
				{
					Selected = ((Selected < SelectionCount - 1) ? (Selected + 1) : 0);
				}
				if (InputManager.TappedUp)
				{
					Selected = ((Selected <= 0) ? (SelectionCount - 1) : (Selected - 1));
				}
				if (InputManager.TappedUp || InputManager.TappedDown)
				{
					Highlight.localPosition = new Vector3(Highlight.localPosition.x, 225f - 75f * (float)Selected, Highlight.localPosition.z);
				}
				if (Input.GetButtonDown("A"))
				{
					if (Selected == 0 || Selected == 2 || Selected == 5 || Selected == 7)
					{
						Darkness.color = new Color(0f, 0f, 0f, Darkness.color.a);
						InputTimer = -10f;
						FadeOut = true;
						Fading = true;
					}
					else if (Selected == 1)
					{
						PromptBar.Label[0].text = "Select";
						PromptBar.Label[1].text = "Back";
						PromptBar.Label[2].text = "Delete";
						PromptBar.UpdateButtons();
						SaveFiles.Show = true;
					}
					else if (Selected == 3)
					{
						PromptBar.Label[0].text = "Visit";
						PromptBar.Label[1].text = "Back";
						PromptBar.UpdateButtons();
						Sponsors.Show = true;
					}
					if (!LoveSick)
					{
						TurnCute();
					}
				}
				if (Input.GetKeyDown("l"))
				{
					GameGlobals.LoveSick = !GameGlobals.LoveSick;
					SceneManager.LoadScene("TitleScene");
				}
				if (Input.GetKeyDown("m"))
				{
					TitleLabel.text = "MANDERE";
				}
				if (!LoveSick)
				{
					if (NeverChange)
					{
						Timer = 0f;
					}
					if (Input.GetKeyDown(KeyCode.Space))
					{
						Timer = 10f;
					}
					Timer += Time.deltaTime;
					if (Timer > 10f)
					{
						TurnDark();
					}
					if (Timer > 11f)
					{
						TurnCute();
					}
				}
			}
		}
		else
		{
			if (Sponsors.Show)
			{
				int sponsorIndex = Sponsors.GetSponsorIndex();
				if (Sponsors.SponsorHasWebsite(sponsorIndex))
				{
					PromptBar.Label[0].text = "Visit";
					PromptBar.UpdateButtons();
				}
				else
				{
					PromptBar.Label[0].text = string.Empty;
					PromptBar.UpdateButtons();
				}
			}
			else if (SaveFiles.Show)
			{
				if (SaveFiles.SaveDatas[SaveFiles.ID].EmptyFile.activeInHierarchy)
				{
					PromptBar.Label[0].text = "Create New";
					PromptBar.Label[2].text = string.Empty;
					PromptBar.UpdateButtons();
				}
				else
				{
					PromptBar.Label[0].text = "Load";
					PromptBar.Label[2].text = "Delete";
					PromptBar.UpdateButtons();
				}
			}
			if (Input.GetButtonDown("B") && !SaveFiles.ConfirmationWindow.activeInHierarchy)
			{
				SaveFiles.Show = false;
				Sponsors.Show = false;
				PromptBar.Label[0].text = "Confirm";
				PromptBar.Label[1].text = string.Empty;
				PromptBar.Label[2].text = string.Empty;
				PromptBar.UpdateButtons();
			}
		}
		if (Fading)
		{
			if (!FadeOut)
			{
				if (Darkness.color.a > 0f)
				{
					Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a - Time.deltaTime * FadeSpeed);
					if (Darkness.color.a <= 0f)
					{
						Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 0f);
						Fading = false;
					}
				}
			}
			else if (Darkness.color.a < 1f)
			{
				MissionModeGlobals.MissionMode = false;
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a + Time.deltaTime);
				if (Darkness.color.a >= 1f)
				{
					if (Selected == 0)
					{
						GameGlobals.Profile = 1;
						SceneManager.LoadScene("CalendarScene");
					}
					else if (Selected == 1)
					{
						if (LoveSick)
						{
							GameGlobals.LoveSick = true;
						}
						if (PlayerPrefs.GetInt("ProfileCreated_" + GameGlobals.Profile) == 0)
						{
							PlayerPrefs.SetInt("ProfileCreated_" + GameGlobals.Profile, 1);
							Debug.Log("ProfileCreated_" + GameGlobals.Profile + " is: " + PlayerPrefs.GetInt("ProfileCreated_" + GameGlobals.Profile));
							DateGlobals.Weekday = DayOfWeek.Sunday;
							SceneManager.LoadScene("SenpaiScene");
						}
						else
						{
							SceneManager.LoadScene("CalendarScene");
						}
					}
					else if (Selected == 2)
					{
						SceneManager.LoadScene("MissionModeScene");
					}
					else if (Selected == 5)
					{
						SceneManager.LoadScene("CreditsScene");
					}
					else if (Selected == 7)
					{
						Application.Quit();
					}
				}
				LoveSickMusic.volume -= Time.deltaTime;
				CuteMusic.volume -= Time.deltaTime;
			}
		}
		if (Timer < 10f)
		{
			Animation component = Yandere.GetComponent<Animation>();
			component["f02_yanderePose_00"].weight = 0f;
			component["f02_fist_00"].weight = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			Time.timeScale -= 1f;
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			Time.timeScale += 1f;
		}
	}

	private void LateUpdate()
	{
		if (Knife.activeInHierarchy)
		{
			Transform[] spine = Spine;
			foreach (Transform transform in spine)
			{
				transform.transform.localEulerAngles = new Vector3(transform.transform.localEulerAngles.x + 5f, transform.transform.localEulerAngles.y, transform.transform.localEulerAngles.z);
			}
			Transform transform2 = Arm[0];
			transform2.transform.localEulerAngles = new Vector3(transform2.transform.localEulerAngles.x, transform2.transform.localEulerAngles.y, transform2.transform.localEulerAngles.z - 15f);
			Transform transform3 = Arm[1];
			transform3.transform.localEulerAngles = new Vector3(transform3.transform.localEulerAngles.x, transform3.transform.localEulerAngles.y, transform3.transform.localEulerAngles.z + 15f);
		}
	}

	private void TurnDark()
	{
		GameObjectUtils.SetLayerRecursively(Yandere.transform.parent.gameObject, 14);
		Animation component = Yandere.GetComponent<Animation>();
		component["f02_yanderePose_00"].weight = 1f;
		component["f02_fist_00"].weight = 1f;
		component.Play("f02_fist_00");
		Renderer renderer = YandereEye[0];
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);
		Renderer renderer2 = YandereEye[1];
		renderer2.material.color = new Color(renderer2.material.color.r, renderer2.material.color.g, renderer2.material.color.b, 1f);
		ColorCorrection.enabled = true;
		BloodProjector.SetActive(true);
		BloodCamera.SetActive(true);
		Knife.SetActive(true);
		CuteMusic.volume = 0f;
		DarkMusic.volume = 1f;
		RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 1f);
		RenderSettings.skybox = DarkSkybox;
		RenderSettings.fog = true;
		UISprite[] mediumSprites = MediumSprites;
		foreach (UISprite uISprite in mediumSprites)
		{
			uISprite.color = new Color(1f, 0f, 0f, uISprite.color.a);
		}
		UISprite[] lightSprites = LightSprites;
		foreach (UISprite uISprite2 in lightSprites)
		{
			uISprite2.color = new Color(0f, 0f, 0f, uISprite2.color.a);
		}
		UISprite[] darkSprites = DarkSprites;
		foreach (UISprite uISprite3 in darkSprites)
		{
			uISprite3.color = new Color(0f, 0f, 0f, uISprite3.color.a);
		}
		UILabel[] coloredLabels = ColoredLabels;
		foreach (UILabel uILabel in coloredLabels)
		{
			uILabel.color = new Color(0f, 0f, 0f, uILabel.color.a);
		}
		SimulatorLabel.color = new Color(1f, 0f, 0f, 1f);
	}

	private void TurnCute()
	{
		GameObjectUtils.SetLayerRecursively(Yandere.transform.parent.gameObject, 9);
		Animation component = Yandere.GetComponent<Animation>();
		component["f02_yanderePose_00"].weight = 0f;
		component["f02_fist_00"].weight = 0f;
		component.Stop("f02_fist_00");
		Renderer renderer = YandereEye[0];
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0f);
		Renderer renderer2 = YandereEye[1];
		renderer2.material.color = new Color(renderer2.material.color.r, renderer2.material.color.g, renderer2.material.color.b, 0f);
		ColorCorrection.enabled = false;
		BloodProjector.SetActive(false);
		BloodCamera.SetActive(false);
		Knife.SetActive(false);
		CuteMusic.volume = 1f;
		DarkMusic.volume = 0f;
		Timer = 0f;
		RenderSettings.ambientLight = new Color(0.75f, 0.75f, 0.75f, 1f);
		RenderSettings.skybox = CuteSkybox;
		RenderSettings.fog = false;
		UISprite[] mediumSprites = MediumSprites;
		foreach (UISprite uISprite in mediumSprites)
		{
			uISprite.color = new Color(MediumColor.r, MediumColor.g, MediumColor.b, uISprite.color.a);
		}
		UISprite[] lightSprites = LightSprites;
		foreach (UISprite uISprite2 in lightSprites)
		{
			uISprite2.color = new Color(LightColor.r, LightColor.g, LightColor.b, uISprite2.color.a);
		}
		UISprite[] darkSprites = DarkSprites;
		foreach (UISprite uISprite3 in darkSprites)
		{
			uISprite3.color = new Color(DarkColor.r, DarkColor.g, DarkColor.b, uISprite3.color.a);
		}
		UILabel[] coloredLabels = ColoredLabels;
		foreach (UILabel uILabel in coloredLabels)
		{
			uILabel.color = new Color(1f, 1f, 1f, uILabel.color.a);
		}
		SimulatorLabel.color = MediumColor;
	}

	private void TurnLoveSick()
	{
		RenderSettings.ambientLight = new Color(0.25f, 0.25f, 0.25f, 1f);
		CuteMusic.volume = 0f;
		DarkMusic.volume = 0f;
		LoveSickMusic.volume = 1f;
		UISprite[] mediumSprites = MediumSprites;
		foreach (UISprite uISprite in mediumSprites)
		{
			uISprite.color = new Color(0f, 0f, 0f, uISprite.color.a);
		}
		UISprite[] lightSprites = LightSprites;
		foreach (UISprite uISprite2 in lightSprites)
		{
			uISprite2.color = new Color(1f, 0f, 0f, uISprite2.color.a);
		}
		UISprite[] darkSprites = DarkSprites;
		foreach (UISprite uISprite3 in darkSprites)
		{
			uISprite3.color = new Color(1f, 0f, 0f, uISprite3.color.a);
		}
		UILabel[] coloredLabels = ColoredLabels;
		foreach (UILabel uILabel in coloredLabels)
		{
			uILabel.color = new Color(1f, 0f, 0f, uILabel.color.a);
		}
		LoveSickLogo.SetActive(true);
		Logo.SetActive(false);
	}
}
