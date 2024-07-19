using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenScript : MonoBehaviour
{
	public StudentInfoMenuScript StudentInfoMenu;

	public PhotoGalleryScript PhotoGallery;

	public InputManagerScript InputManager;

	public SaveLoadMenuScript SaveLoadMenu;

	public HomeYandereScript HomeYandere;

	public MissionModeScript MissionMode;

	public HomeCameraScript HomeCamera;

	public ServicesScript ServiceMenu;

	public FavorMenuScript FavorMenu;

	public AudioMenuScript AudioMenu;

	public PromptBarScript PromptBar;

	public PassTimeScript PassTime;

	public SettingsScript Settings;

	public TaskListScript TaskList;

	public SchemesScript Schemes;

	public YandereScript Yandere;

	public RPG_Camera RPGCamera;

	public PoliceScript Police;

	public ClockScript Clock;

	public StatsScript Stats;

	public Blur ScreenBlur;

	public MapScript Map;

	public UILabel SelectionLabel;

	public UIPanel Panel;

	public UISprite Wifi;

	public GameObject MissionModeLabel;

	public GameObject MissionModeIcons;

	public GameObject LoadingScreen;

	public GameObject SchemesMenu;

	public GameObject StudentInfo;

	public GameObject DropsMenu;

	public GameObject MainMenu;

	public Transform PromptParent;

	public string[] SelectionNames;

	public UISprite[] PhoneIcons;

	public Transform[] Eggs;

	public int Prompts;

	public int Selected = 1;

	public float Speed;

	public bool ShowMissionModeDetails;

	public bool CorrectingTime;

	public bool BypassPhone;

	public bool EggsChecked;

	public bool PressedA;

	public bool PressedB;

	public bool Quitting;

	public bool Sideways;

	public bool Home;

	public bool Show;

	public int Row = 1;

	public int Column = 2;

	private void Start()
	{
		if (SceneManager.GetActiveScene().name != "SchoolScene")
		{
			MissionModeGlobals.MultiMission = false;
		}
		if (!MissionModeGlobals.MultiMission)
		{
			MissionModeLabel.SetActive(false);
		}
		StudentGlobals.SetStudentPhotographed(0, true);
		StudentGlobals.SetStudentPhotographed(1, true);
		base.transform.localPosition = new Vector3(1350f, 0f, 0f);
		base.transform.localScale = new Vector3(0.9133334f, 0.9133334f, 0.9133334f);
		base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, 0f);
		StudentInfoMenu.gameObject.SetActive(false);
		PhotoGallery.gameObject.SetActive(false);
		SaveLoadMenu.gameObject.SetActive(false);
		ServiceMenu.gameObject.SetActive(false);
		FavorMenu.gameObject.SetActive(false);
		AudioMenu.gameObject.SetActive(false);
		PassTime.gameObject.SetActive(false);
		Settings.gameObject.SetActive(false);
		Stats.gameObject.SetActive(false);
		LoadingScreen.SetActive(false);
		SchemesMenu.SetActive(false);
		StudentInfo.SetActive(false);
		DropsMenu.SetActive(false);
		MainMenu.SetActive(true);
		if (SceneManager.GetActiveScene().name == "SchoolScene")
		{
			Schemes.UpdateInstructions();
		}
		else
		{
			MissionModeIcons.SetActive(false);
			UISprite uISprite = PhoneIcons[5];
			uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0.5f);
			UISprite uISprite2 = PhoneIcons[7];
			uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0.5f);
			UISprite uISprite3 = PhoneIcons[8];
			uISprite3.color = new Color(uISprite3.color.r, uISprite3.color.g, uISprite3.color.b, 0.5f);
			UISprite uISprite4 = PhoneIcons[9];
			uISprite4.color = new Color(uISprite4.color.r, uISprite4.color.g, uISprite4.color.b, 0.5f);
		}
		if (MissionModeGlobals.MissionMode)
		{
			UISprite uISprite5 = PhoneIcons[7];
			uISprite5.color = new Color(uISprite5.color.r, uISprite5.color.g, uISprite5.color.b, 0.5f);
			UISprite uISprite6 = PhoneIcons[9];
			uISprite6.color = new Color(uISprite6.color.r, uISprite6.color.g, uISprite6.color.b, 0.5f);
			UISprite uISprite7 = PhoneIcons[10];
			uISprite7.color = new Color(uISprite7.color.r, uISprite7.color.g, uISprite7.color.b, 1f);
		}
		UpdateSelection();
		CorrectingTime = false;
	}

	private void Update()
	{
		Speed = Time.unscaledDeltaTime * 10f;
		if (Police.FadeOut || Map.Show)
		{
			return;
		}
		if (!Show)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(1350f, 50f, 0f), Speed);
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(0.9133334f, 0.9133334f, 0.9133334f), Speed);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, Mathf.Lerp(base.transform.localEulerAngles.z, 0f, Speed));
			if (base.transform.localPosition.x > 1349f && Panel.enabled)
			{
				Panel.enabled = false;
			}
			if (CorrectingTime && Time.timeScale < 0.9f)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Speed);
				if (Time.timeScale > 0.9f)
				{
					CorrectingTime = false;
					Time.timeScale = 1f;
				}
			}
			if (!Input.GetButtonDown("Start"))
			{
				return;
			}
			if (!Home)
			{
				if (!Yandere.Shutter.Snapping && !Yandere.TimeSkipping && !Yandere.Talking && !Yandere.Noticed && !Yandere.InClass && !Yandere.Struggling && !Yandere.Won && !Yandere.Dismembering && !Yandere.Attacked && Yandere.CanMove && Time.timeScale > 0.0001f)
				{
					Yandere.StopAiming();
					PromptParent.localScale = Vector3.zero;
					Yandere.Obscurance.enabled = false;
					Yandere.YandereVision = false;
					ScreenBlur.enabled = true;
					Yandere.YandereTimer = 0f;
					Yandere.Mopping = false;
					Panel.enabled = true;
					Sideways = false;
					Show = true;
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Accept";
					PromptBar.Label[1].text = "Exit";
					PromptBar.Label[4].text = "Choose";
					PromptBar.Label[5].text = "Choose";
					PromptBar.UpdateButtons();
					PromptBar.Show = true;
					UISprite uISprite = PhoneIcons[3];
					if (!Yandere.CanMove || Yandere.Dragging || (Police.Corpses - Police.HiddenCorpses > 0 && !Police.SuicideScene && !Police.PoisonScene))
					{
						uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0.5f);
					}
					else
					{
						uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 1f);
					}
				}
			}
			else if (HomeCamera.Destination == HomeCamera.Destinations[0])
			{
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Accept";
				PromptBar.Label[1].text = "Exit";
				PromptBar.Label[4].text = "Choose";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
				HomeYandere.CanMove = false;
				UISprite uISprite2 = PhoneIcons[3];
				uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0.5f);
				Panel.enabled = true;
				Sideways = false;
				Show = true;
			}
			return;
		}
		if (!EggsChecked)
		{
			float num = 99999f;
			for (int i = 0; i < Eggs.Length; i++)
			{
				if (Eggs[i] != null)
				{
					float num2 = Vector3.Distance(Yandere.transform.position, Eggs[i].position);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			if (num < 5f)
			{
				Wifi.spriteName = "5Bars";
			}
			else if (num < 10f)
			{
				Wifi.spriteName = "4Bars";
			}
			else if (num < 15f)
			{
				Wifi.spriteName = "3Bars";
			}
			else if (num < 20f)
			{
				Wifi.spriteName = "2Bars";
			}
			else if (num < 25f)
			{
				Wifi.spriteName = "1Bars";
			}
			else
			{
				Wifi.spriteName = "0Bars";
			}
			EggsChecked = true;
		}
		if (!Home)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, Speed);
			RPGCamera.enabled = false;
		}
		if (ShowMissionModeDetails)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Speed);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, 1200f, 0f), Speed);
		}
		else if (Quitting)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Speed);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, -1200f, 0f), Speed);
		}
		else if (!Sideways)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(0.9133334f, 0.9133334f, 0.9133334f), Speed);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, 50f, 0f), Speed);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, Mathf.Lerp(base.transform.localEulerAngles.z, 0f, Speed));
		}
		else
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1.78f, 1.78f, 1.78f), Speed);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, 14f, 0f), Speed);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, Mathf.Lerp(base.transform.localEulerAngles.z, 90f, Speed));
		}
		if (MainMenu.activeInHierarchy && !Quitting)
		{
			if (InputManager.TappedUp || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				Row--;
				UpdateSelection();
			}
			if (InputManager.TappedDown || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				Row++;
				UpdateSelection();
			}
			if (InputManager.TappedRight || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				Column++;
				UpdateSelection();
			}
			if (InputManager.TappedLeft || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				Column--;
				UpdateSelection();
			}
			if (Input.GetKeyDown("space"))
			{
				ShowMissionModeDetails = !ShowMissionModeDetails;
			}
			for (int j = 1; j < PhoneIcons.Length; j++)
			{
				if (PhoneIcons[j] != null)
				{
					Vector3 b = ((Selected == j) ? new Vector3(1.5f, 1.5f, 1.5f) : new Vector3(1f, 1f, 1f));
					PhoneIcons[j].transform.localScale = Vector3.Lerp(PhoneIcons[j].transform.localScale, b, Speed);
				}
			}
			if (!ShowMissionModeDetails)
			{
				if (Input.GetButtonDown("A"))
				{
					PressedA = true;
					if (PhoneIcons[Selected].color.a == 1f)
					{
						if (Selected == 1)
						{
							MainMenu.SetActive(false);
							LoadingScreen.SetActive(true);
							PromptBar.ClearButtons();
							PromptBar.Label[1].text = "Back";
							PromptBar.Label[4].text = "Choose";
							PromptBar.Label[5].text = "Choose";
							PromptBar.UpdateButtons();
							StartCoroutine(PhotoGallery.GetPhotos());
						}
						else if (Selected == 2)
						{
							TaskList.gameObject.SetActive(true);
							MainMenu.SetActive(false);
							Sideways = true;
							PromptBar.ClearButtons();
							PromptBar.Label[1].text = "Back";
							PromptBar.Label[4].text = "Choose";
							PromptBar.UpdateButtons();
							TaskList.UpdateTaskList();
							StartCoroutine(TaskList.UpdateTaskInfo());
						}
						else if (Selected == 3)
						{
							if (PhoneIcons[3].color.a == 1f && Yandere.CanMove && !Yandere.Dragging)
							{
								for (int k = 0; k < Yandere.ArmedAnims.Length; k++)
								{
									Yandere.CharacterAnimation[Yandere.ArmedAnims[k]].weight = 0f;
								}
								MainMenu.SetActive(false);
								PromptBar.ClearButtons();
								PromptBar.Label[0].text = "Begin";
								PromptBar.Label[1].text = "Back";
								PromptBar.Label[4].text = "Adjust";
								PromptBar.Label[5].text = "Choose";
								PromptBar.UpdateButtons();
								PassTime.gameObject.SetActive(true);
								PassTime.GetCurrentTime();
							}
						}
						else if (Selected == 4)
						{
							PromptBar.ClearButtons();
							PromptBar.Label[1].text = "Exit";
							PromptBar.UpdateButtons();
							Stats.gameObject.SetActive(true);
							Stats.UpdateStats();
							MainMenu.SetActive(false);
							Sideways = true;
						}
						else if (Selected == 5)
						{
							if (PhoneIcons[5].color.a == 1f)
							{
								PromptBar.ClearButtons();
								PromptBar.Label[0].text = "Accept";
								PromptBar.Label[1].text = "Exit";
								PromptBar.Label[5].text = "Choose";
								PromptBar.UpdateButtons();
								FavorMenu.gameObject.SetActive(true);
								FavorMenu.gameObject.GetComponent<AudioSource>().Play();
								MainMenu.SetActive(false);
								Sideways = true;
							}
						}
						else if (Selected == 6)
						{
							StudentInfoMenu.gameObject.SetActive(true);
							StartCoroutine(StudentInfoMenu.UpdatePortraits());
							MainMenu.SetActive(false);
							Sideways = true;
							PromptBar.ClearButtons();
							PromptBar.Label[0].text = "View Info";
							PromptBar.Label[1].text = "Back";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
						}
						else if (Selected == 7)
						{
							SaveLoadMenu.gameObject.SetActive(true);
							SaveLoadMenu.Header.text = "Load Data";
							SaveLoadMenu.Loading = true;
							SaveLoadMenu.Saving = false;
							SaveLoadMenu.Column = 1;
							SaveLoadMenu.Row = 1;
							SaveLoadMenu.UpdateHighlight();
							StartCoroutine(SaveLoadMenu.GetThumbnails());
							MainMenu.SetActive(false);
							Sideways = true;
							PromptBar.ClearButtons();
							PromptBar.Label[0].text = "Choose";
							PromptBar.Label[1].text = "Back";
							PromptBar.Label[2].text = "Debug";
							PromptBar.Label[4].text = "Change";
							PromptBar.Label[5].text = "Change";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
						}
						else if (Selected == 8)
						{
							Settings.gameObject.SetActive(true);
							ScreenBlur.enabled = false;
							Settings.UpdateText();
							MainMenu.SetActive(false);
							PromptBar.ClearButtons();
							PromptBar.Label[1].text = "Back";
							PromptBar.Label[4].text = "Choose";
							PromptBar.Label[5].text = "Change";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
						}
						else if (Selected == 9)
						{
							SaveLoadMenu.gameObject.SetActive(true);
							SaveLoadMenu.Header.text = "Save Data";
							SaveLoadMenu.Loading = false;
							SaveLoadMenu.Saving = true;
							SaveLoadMenu.Column = 1;
							SaveLoadMenu.Row = 1;
							SaveLoadMenu.UpdateHighlight();
							StartCoroutine(SaveLoadMenu.GetThumbnails());
							MainMenu.SetActive(false);
							Sideways = true;
							PromptBar.ClearButtons();
							PromptBar.Label[0].text = "Choose";
							PromptBar.Label[1].text = "Back";
							PromptBar.Label[4].text = "Change";
							PromptBar.Label[5].text = "Change";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
						}
						else if (Selected == 10)
						{
							if (!MissionModeGlobals.MissionMode)
							{
								AudioMenu.gameObject.SetActive(true);
								AudioMenu.UpdateText();
								MainMenu.SetActive(false);
								PromptBar.ClearButtons();
								PromptBar.Label[0].text = "Play";
								PromptBar.Label[1].text = "Back";
								PromptBar.Label[4].text = "Choose";
								PromptBar.UpdateButtons();
								PromptBar.Show = true;
							}
							else
							{
								PhoneIcons[Selected].transform.localScale = new Vector3(1f, 1f, 1f);
								MissionMode.ChangeMusic();
							}
						}
						else if (Selected == 11)
						{
							PromptBar.ClearButtons();
							PromptBar.Show = false;
							Quitting = true;
						}
						else if (Selected != 12)
						{
						}
					}
				}
				if (!PressedB && (Input.GetButtonDown("Start") || Input.GetButtonDown("B")))
				{
					ExitPhone();
				}
				if (Input.GetButtonUp("B"))
				{
					PressedB = false;
				}
			}
		}
		if (!PressedA)
		{
			if (PassTime.gameObject.activeInHierarchy)
			{
				if (Input.GetButtonDown("A"))
				{
					if (Yandere.PickUp != null)
					{
						Yandere.PickUp.Drop();
					}
					Yandere.Unequip();
					ScreenBlur.enabled = false;
					RPGCamera.enabled = true;
					PassTime.gameObject.SetActive(false);
					MainMenu.SetActive(true);
					PromptBar.Show = false;
					Show = false;
					Clock.TargetTime = PassTime.TargetTime;
					Clock.TimeSkip = true;
					Time.timeScale = 1f;
				}
				if (Input.GetButtonDown("B"))
				{
					MainMenu.SetActive(true);
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Accept";
					PromptBar.Label[1].text = "Exit";
					PromptBar.Label[4].text = "Choose";
					PromptBar.Label[5].text = "Choose";
					PromptBar.UpdateButtons();
					PassTime.gameObject.SetActive(false);
				}
			}
			if (Quitting)
			{
				if (Input.GetButtonDown("A"))
				{
					SceneManager.LoadScene("TitleScene");
				}
				if (Input.GetButtonDown("B"))
				{
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Accept";
					PromptBar.Label[1].text = "Exit";
					PromptBar.Label[4].text = "Choose";
					PromptBar.Label[5].text = "Choose";
					PromptBar.UpdateButtons();
					PromptBar.Show = true;
					Quitting = false;
					if (BypassPhone)
					{
						base.transform.localPosition = new Vector3(1350f, 0f, 0f);
						ExitPhone();
					}
				}
			}
		}
		if (Input.GetButtonUp("A"))
		{
			PressedA = false;
		}
	}

	public void JumpToQuit()
	{
		if (!Police.FadeOut && !Clock.TimeSkip && !Yandere.Noticed)
		{
			base.transform.localPosition = new Vector3(0f, -1200f, 0f);
			Yandere.YandereVision = false;
			if (!Yandere.Talking && !Yandere.Dismembering)
			{
				RPGCamera.enabled = false;
				Yandere.StopAiming();
			}
			ScreenBlur.enabled = true;
			Panel.enabled = true;
			BypassPhone = true;
			Quitting = true;
			Show = true;
		}
	}

	public void ExitPhone()
	{
		if (!Home)
		{
			PromptParent.localScale = new Vector3(1f, 1f, 1f);
			ScreenBlur.enabled = false;
			CorrectingTime = true;
			if (!Yandere.Talking && !Yandere.Dismembering)
			{
				RPGCamera.enabled = true;
			}
			if (Yandere.Laughing)
			{
				Yandere.GetComponent<AudioSource>().volume = 1f;
			}
		}
		else
		{
			HomeYandere.CanMove = true;
		}
		PromptBar.ClearButtons();
		PromptBar.Show = false;
		BypassPhone = false;
		EggsChecked = false;
		PressedA = false;
		Show = false;
	}

	private void UpdateSelection()
	{
		if (Row < 0)
		{
			Row = 3;
		}
		else if (Row > 3)
		{
			Row = 0;
		}
		if (Column < 1)
		{
			Column = 3;
		}
		else if (Column > 3)
		{
			Column = 1;
		}
		Selected = Row * 3 + Column;
		SelectionLabel.text = SelectionNames[Selected];
	}
}
