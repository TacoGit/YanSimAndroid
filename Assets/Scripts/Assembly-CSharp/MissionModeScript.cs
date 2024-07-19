using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionModeScript : MonoBehaviour
{
	public NotificationManagerScript NotificationManager;

	public NewMissionWindowScript NewMissionWindow;

	public MissionModeMenuScript MissionModeMenu;

	public StudentManagerScript StudentManager;

	public WeaponManagerScript WeaponManager;

	public PromptScript ExitPortalPrompt;

	public IncineratorScript Incinerator;

	public WoodChipperScript WoodChipper;

	public ReputationScript Reputation;

	public GrayscaleEffect Grayscale;

	public PromptBarScript PromptBar;

	public BoundaryScript Boundary;

	public JukeboxScript Jukebox;

	public YandereScript Yandere;

	public PoliceScript Police;

	public ClockScript Clock;

	public UILabel EventSubtitleLabel;

	public UILabel ReputationLabel;

	public UILabel GameOverHeader;

	public UILabel GameOverReason;

	public UILabel SubtitleLabel;

	public UILabel LoadingLabel;

	public UILabel SpottedLabel;

	public UILabel TimeLabel;

	public UISprite ReputationFace1;

	public UISprite ReputationFace2;

	public UISprite ReputationBG;

	public UISprite CautionSign;

	public UISprite MusicIcon;

	public UISprite Darkness;

	public Text FPS;

	public GardenHoleScript[] GardenHoles;

	public GameObject[] ReputationIcons;

	public string[] GameOverReasons;

	public AudioClip[] StealthMusic;

	public Transform[] SpawnPoints;

	public UISprite[] PoliceIcon;

	public UILabel[] PoliceLabel;

	public int[] Conditions;

	public GameObject SecurityCameraGroup;

	public GameObject MetalDetectorGroup;

	public GameObject HeartbrokenCamera;

	public GameObject DetectionCamera;

	public GameObject HeartbeatCamera;

	public GameObject MissionModeHUD;

	public GameObject SpottedWindow;

	public GameObject TranqDetector;

	public GameObject WitnessCamera;

	public GameObject GameOverText;

	public GameObject VoidGoddess;

	public GameObject Headmaster;

	public GameObject ExitPortal;

	public GameObject MurderKit;

	public GameObject Subtitle;

	public GameObject Nemesis;

	public GameObject Safe;

	public Transform LastKnownPosition;

	public int RequiredClothingID;

	public int RequiredDisposalID;

	public int RequiredWeaponID;

	public int NemesisDifficulty;

	public int DisposalMethod;

	public int MurderWeaponID;

	public int GameOverPhase;

	public int Destination;

	public int Difficulty;

	public int GameOverID;

	public int TargetID;

	public int MusicID = 1;

	public int Phase = 1;

	public int ID;

	public int[] Target;

	public int[] Method;

	public bool SecurityCameras;

	public bool MetalDetectors;

	public bool StealDocuments;

	public bool NoCollateral;

	public bool NoSuspicion;

	public bool NoWitnesses;

	public bool NoCorpses;

	public bool NoSpeech;

	public bool NoWeapon;

	public bool NoBlood;

	public bool TimeLimit;

	public bool CorrectClothingConfirmed;

	public bool DocumentsStolen;

	public bool CorpseDisposed;

	public bool WeaponDisposed;

	public bool CheckForBlood;

	public bool BloodCleaned;

	public bool MultiMission;

	public bool InfoRemark;

	public bool TargetDead;

	public bool Chastise;

	public bool FadeOut;

	public bool Enabled;

	public bool[] Checking;

	public string CauseOfFailure = string.Empty;

	public float TimeRemaining = 300f;

	public float TargetHeight;

	public float BloodTimer;

	public float Speed;

	public float Timer;

	public AudioClip InfoAccomplished;

	public AudioClip InfoExfiltrate;

	public AudioClip InfoObjective;

	public AudioClip InfoFailure;

	public AudioClip GameOverSound;

	public ColorCorrectionCurves[] ColorCorrections;

	public UILabel Watermark;

	public Font Arial;

	public int Frame;

	private void Start()
	{
		if (!SchoolGlobals.HighSecurity)
		{
			SecurityCameraGroup.SetActive(false);
			MetalDetectorGroup.SetActive(false);
		}
		NewMissionWindow.gameObject.SetActive(false);
		MissionModeHUD.SetActive(false);
		SpottedWindow.SetActive(false);
		ExitPortal.SetActive(false);
		Safe.SetActive(false);
		if (GameGlobals.LoveSick)
		{
			MurderKit.SetActive(false);
			Yandere.HeartRate.MediumColour = new Color(1f, 1f, 1f, 1f);
			Yandere.HeartRate.NormalColour = new Color(1f, 1f, 1f, 1f);
			Clock.PeriodLabel.color = new Color(1f, 0f, 0f, 1f);
			Clock.TimeLabel.color = new Color(1f, 0f, 0f, 1f);
			Clock.DayLabel.enabled = false;
			Reputation.PendingRepMarker.GetComponent<UISprite>().color = new Color(1f, 0f, 0f, 1f);
			Reputation.CurrentRepMarker.gameObject.SetActive(false);
			Reputation.PendingRepLabel.color = new Color(1f, 0f, 0f, 1f);
			ReputationFace1.color = new Color(1f, 0f, 0f, 1f);
			ReputationFace2.color = new Color(1f, 0f, 0f, 1f);
			ReputationBG.color = new Color(1f, 0f, 0f, 1f);
			ReputationLabel.color = new Color(1f, 0f, 0f, 1f);
			Watermark.color = new Color(1f, 0f, 0f, 1f);
			EventSubtitleLabel.color = new Color(1f, 0f, 0f, 1f);
			SubtitleLabel.color = new Color(1f, 0f, 0f, 1f);
			CautionSign.color = new Color(1f, 0f, 0f, 1f);
			FPS.color = new Color(1f, 0f, 0f, 1f);
			for (ID = 1; ID < PoliceLabel.Length; ID++)
			{
				PoliceLabel[ID].color = new Color(1f, 0f, 0f, 1f);
			}
			for (ID = 1; ID < PoliceIcon.Length; ID++)
			{
				PoliceIcon[ID].color = new Color(1f, 0f, 0f, 1f);
			}
		}
		if (MissionModeGlobals.MissionMode)
		{
			Headmaster.SetActive(false);
			Yandere.HeartRate.MediumColour = new Color(1f, 0.5f, 0.5f, 1f);
			Yandere.HeartRate.NormalColour = new Color(1f, 1f, 1f, 1f);
			Clock.PeriodLabel.color = new Color(1f, 1f, 1f, 1f);
			Clock.TimeLabel.color = new Color(1f, 1f, 1f, 1f);
			Clock.DayLabel.enabled = false;
			Reputation.PendingRepMarker.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
			Reputation.CurrentRepMarker.gameObject.SetActive(false);
			Reputation.PendingRepLabel.color = new Color(1f, 1f, 1f, 1f);
			ReputationLabel.fontStyle = FontStyle.Bold;
			ReputationLabel.trueTypeFont = Arial;
			ReputationLabel.color = new Color(1f, 1f, 1f, 1f);
			ReputationLabel.text = "AWARENESS";
			ReputationIcons[0].SetActive(true);
			ReputationIcons[1].SetActive(false);
			ReputationIcons[2].SetActive(false);
			ReputationIcons[3].SetActive(false);
			ReputationIcons[4].SetActive(false);
			ReputationIcons[5].SetActive(false);
			Clock.TimeLabel.fontStyle = FontStyle.Bold;
			Clock.TimeLabel.trueTypeFont = Arial;
			Clock.PeriodLabel.fontStyle = FontStyle.Bold;
			Clock.PeriodLabel.trueTypeFont = Arial;
			Watermark.fontStyle = FontStyle.Bold;
			Watermark.color = new Color(1f, 1f, 1f, 1f);
			Watermark.trueTypeFont = Arial;
			SubtitleLabel.color = new Color(1f, 1f, 1f, 1f);
			CautionSign.color = new Color(1f, 1f, 1f, 1f);
			FPS.color = new Color(1f, 1f, 1f, 1f);
			ColorCorrections = Camera.main.GetComponents<ColorCorrectionCurves>();
			StudentManager.MissionMode = true;
			NemesisDifficulty = MissionModeGlobals.NemesisDifficulty;
			Difficulty = MissionModeGlobals.MissionDifficulty;
			TargetID = MissionModeGlobals.MissionTarget;
			ClassGlobals.BiologyGrade = 1;
			ClassGlobals.ChemistryGrade = 1;
			ClassGlobals.LanguageGrade = 1;
			ClassGlobals.PhysicalGrade = 1;
			ClassGlobals.PsychologyGrade = 1;
			Yandere.StudentManager.TutorialWindow.gameObject.SetActive(false);
			TutorialGlobals.TutorialsOff = true;
			SchoolGlobals.SchoolAtmosphereSet = true;
			SchoolGlobals.SchoolAtmosphere = 1f - (float)Difficulty * 0.1f;
			Debug.Log("Mission Mode has lowered school atmosphere according to the difficulty level.");
			StudentManager.Atmosphere = 1f - (float)Difficulty * 0.1f;
			for (ID = 1; ID < PoliceLabel.Length; ID++)
			{
				PoliceLabel[ID].fontStyle = FontStyle.Bold;
				PoliceLabel[ID].color = new Color(1f, 1f, 1f, 1f);
				PoliceLabel[ID].trueTypeFont = Arial;
			}
			for (ID = 1; ID < PoliceIcon.Length; ID++)
			{
				PoliceIcon[ID].color = new Color(1f, 1f, 1f, 1f);
			}
			if (Difficulty > 1)
			{
				for (ID = 2; ID < Difficulty + 1; ID++)
				{
					int missionCondition = MissionModeGlobals.GetMissionCondition(ID);
					switch (missionCondition)
					{
					case 1:
						RequiredWeaponID = MissionModeGlobals.MissionRequiredWeapon;
						break;
					case 2:
						RequiredClothingID = MissionModeGlobals.MissionRequiredClothing;
						break;
					case 3:
						RequiredDisposalID = MissionModeGlobals.MissionRequiredDisposal;
						break;
					case 4:
						NoCollateral = true;
						break;
					case 5:
						NoWitnesses = true;
						break;
					case 6:
						NoCorpses = true;
						break;
					case 7:
						NoWeapon = true;
						break;
					case 8:
						NoBlood = true;
						break;
					case 9:
						TimeLimit = true;
						break;
					case 10:
						NoSuspicion = true;
						break;
					case 11:
						SecurityCameras = true;
						break;
					case 12:
						MetalDetectors = true;
						break;
					case 13:
						NoSpeech = true;
						break;
					case 14:
						StealDocuments = true;
						break;
					}
					Conditions[ID] = missionCondition;
				}
			}
			if (!StealDocuments)
			{
				DocumentsStolen = true;
			}
			else
			{
				Safe.SetActive(true);
			}
			if (SecurityCameras)
			{
				SecurityCameraGroup.SetActive(true);
			}
			if (MetalDetectors)
			{
				MetalDetectorGroup.SetActive(true);
			}
			if (TimeLimit)
			{
				TimeLabel.gameObject.SetActive(true);
			}
			if (NoSpeech)
			{
				StudentManager.NoSpeech = true;
			}
			if (RequiredDisposalID == 0)
			{
				CorpseDisposed = true;
			}
			if (NemesisDifficulty > 0)
			{
				Nemesis.SetActive(true);
			}
			if (!NoWeapon)
			{
				WeaponDisposed = true;
			}
			if (!NoBlood)
			{
				BloodCleaned = true;
			}
			Jukebox.Egg = true;
			Jukebox.KillVolume();
			Jukebox.MissionMode.enabled = true;
			Jukebox.MissionMode.volume = 0f;
			Yandere.FixCamera();
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 6.51505f, -76.9222f);
			Camera.main.transform.eulerAngles = new Vector3(15f, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
			Yandere.RPGCamera.enabled = false;
			Yandere.SanityBased = true;
			Yandere.CanMove = false;
			VoidGoddess.GetComponent<VoidGoddessScript>().Window.gameObject.SetActive(false);
			HeartbeatCamera.SetActive(false);
			TranqDetector.SetActive(false);
			VoidGoddess.SetActive(false);
			MurderKit.SetActive(false);
			TargetHeight = 1.51505f;
			Yandere.HUD.alpha = 0f;
			MusicIcon.color = new Color(MusicIcon.color.r, MusicIcon.color.g, MusicIcon.color.b, 1f);
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 1f);
			MissionModeMenu.UpdateGraphics();
			MissionModeMenu.gameObject.SetActive(true);
			if (MissionModeGlobals.MultiMission)
			{
				NewMissionWindow.gameObject.SetActive(true);
				MissionModeMenu.gameObject.SetActive(false);
				NewMissionWindow.FillOutInfo();
				NewMissionWindow.HideButtons();
				MultiMission = true;
				for (int i = 1; i < 11; i++)
				{
					Target[i] = PlayerPrefs.GetInt("MissionModeTarget" + i);
					Method[i] = PlayerPrefs.GetInt("MissionModeMethod" + i);
				}
			}
			Enabled = true;
		}
		else
		{
			MissionModeMenu.gameObject.SetActive(false);
			TimeLabel.gameObject.SetActive(false);
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (Phase == 1)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime / 3f));
			if (Darkness.color.a == 0f)
			{
				Speed += Time.deltaTime / 3f;
				Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Mathf.Lerp(Camera.main.transform.position.y, TargetHeight, Time.deltaTime * Speed), Camera.main.transform.position.z);
				if (Camera.main.transform.position.y < TargetHeight + 0.1f)
				{
					Yandere.HUD.alpha = Mathf.MoveTowards(Yandere.HUD.alpha, 1f, Time.deltaTime / 3f);
					if (Yandere.HUD.alpha == 1f)
					{
						Debug.Log("Incrementing phase.");
						Yandere.RPGCamera.enabled = true;
						HeartbeatCamera.SetActive(true);
						Yandere.CanMove = true;
						Phase++;
					}
				}
			}
			if (Input.GetButtonDown("A"))
			{
				Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, TargetHeight, Camera.main.transform.position.z);
				Yandere.RPGCamera.enabled = true;
				HeartbeatCamera.SetActive(true);
				Yandere.CanMove = true;
				Yandere.HUD.alpha = 1f;
				Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 0f);
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (MultiMission)
			{
				for (int i = 1; i < Target.Length; i++)
				{
					if (Target[i] == 0)
					{
						Checking[i] = false;
					}
					else
					{
						if (!Checking[i])
						{
							continue;
						}
						if (StudentManager.Students[Target[i]].transform.position.y < -11f)
						{
							GameOverID = 1;
							GameOver();
							Phase = 4;
						}
						else
						{
							if (StudentManager.Students[Target[i]].Alive)
							{
								continue;
							}
							if (Method[i] == 0)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Weapon)
								{
									NewMissionWindow.DeathSkulls[i].SetActive(true);
									Checking[i] = false;
									CheckForCompletion();
								}
								else
								{
									GameOverID = 18;
									GameOver();
									Phase = 4;
								}
							}
							else if (Method[i] == 1)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Drowning)
								{
									NewMissionWindow.DeathSkulls[i].SetActive(true);
									Checking[i] = false;
									CheckForCompletion();
								}
								else
								{
									GameOverID = 18;
									GameOver();
									Phase = 4;
								}
							}
							else if (Method[i] == 2)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Poison)
								{
									NewMissionWindow.DeathSkulls[i].SetActive(true);
									Checking[i] = false;
									CheckForCompletion();
								}
								else
								{
									GameOverID = 18;
									GameOver();
									Phase = 4;
								}
							}
							else if (Method[i] == 3)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Electrocution)
								{
									NewMissionWindow.DeathSkulls[i].SetActive(true);
									Checking[i] = false;
									CheckForCompletion();
								}
								else
								{
									GameOverID = 18;
									GameOver();
									Phase = 4;
								}
							}
							else if (Method[i] == 4)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Burning)
								{
									NewMissionWindow.DeathSkulls[i].SetActive(true);
									Checking[i] = false;
									CheckForCompletion();
								}
								else
								{
									GameOverID = 18;
									GameOver();
									Phase = 4;
								}
							}
							else if (Method[i] == 5)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Falling)
								{
									NewMissionWindow.DeathSkulls[i].SetActive(true);
									Checking[i] = false;
									CheckForCompletion();
								}
								else
								{
									GameOverID = 18;
									GameOver();
									Phase = 4;
								}
							}
							else if (Method[i] == 6)
							{
								if (StudentManager.Students[Target[i]].DeathType == DeathType.Weight)
								{
									Checking[i] = false;
									CheckForCompletion();
									continue;
								}
								NewMissionWindow.DeathSkulls[i].SetActive(true);
								GameOverID = 18;
								GameOver();
								Phase = 4;
							}
						}
					}
				}
			}
			if (!TargetDead && StudentManager.Students[TargetID] != null)
			{
				if (!StudentManager.Students[TargetID].Alive)
				{
					if (Yandere.Equipped > 0)
					{
						if (StudentManager.Students[TargetID].DeathType == DeathType.Weapon)
						{
							MurderWeaponID = Yandere.EquippedWeapon.WeaponID;
						}
						else
						{
							WeaponDisposed = true;
						}
					}
					else
					{
						WeaponDisposed = true;
					}
					TargetDead = true;
				}
				if (StudentManager.Students[TargetID].transform.position.y < -11f)
				{
					GameOverID = 1;
					GameOver();
					Phase = 4;
				}
			}
			if (RequiredWeaponID > 0 && StudentManager.Students[TargetID] != null && !StudentManager.Students[TargetID].Alive && StudentManager.Students[TargetID].DeathCause != RequiredWeaponID)
			{
				Chastise = true;
				GameOverID = 2;
				GameOver();
				Phase = 4;
			}
			if (!CorrectClothingConfirmed && RequiredClothingID > 0 && StudentManager.Students[TargetID] != null && !StudentManager.Students[TargetID].Alive)
			{
				if (Yandere.Schoolwear != RequiredClothingID)
				{
					Chastise = true;
					GameOverID = 3;
					GameOver();
					Phase = 4;
				}
				else
				{
					CorrectClothingConfirmed = true;
				}
			}
			if (RequiredDisposalID > 0 && DisposalMethod == 0 && TargetDead)
			{
				for (ID = 1; ID < Incinerator.Victims + 1; ID++)
				{
					if (Incinerator.VictimList[ID] == TargetID && Incinerator.Smoke.isPlaying)
					{
						DisposalMethod = 1;
					}
				}
				int num = 0;
				for (ID = 1; ID < Incinerator.Limbs + 1; ID++)
				{
					if (Incinerator.LimbList[ID] == TargetID)
					{
						num++;
					}
					if (num == 6 && Incinerator.Smoke.isPlaying)
					{
						DisposalMethod = 1;
					}
				}
				for (ID = 1; ID < WoodChipper.Victims + 1; ID++)
				{
					if (WoodChipper.VictimList[ID] == TargetID && WoodChipper.Shredding)
					{
						DisposalMethod = 2;
					}
				}
				for (ID = 1; ID < GardenHoles.Length; ID++)
				{
					if (GardenHoles[ID].VictimID == TargetID && !GardenHoles[ID].enabled)
					{
						DisposalMethod = 3;
					}
				}
				if (DisposalMethod > 0)
				{
					if (DisposalMethod != RequiredDisposalID)
					{
						Chastise = true;
						GameOverID = 4;
						GameOver();
						Phase = 4;
					}
					else
					{
						CorpseDisposed = true;
					}
				}
			}
			if (NoCollateral)
			{
				if (Police.Corpses == 1)
				{
					if (Police.CorpseList[0].StudentID != TargetID)
					{
						Chastise = true;
						GameOverID = 5;
						GameOver();
						Phase = 4;
					}
				}
				else if (Police.Corpses > 1)
				{
					GameOverID = 5;
					GameOver();
					Phase = 4;
				}
			}
			if (NoWitnesses)
			{
				for (ID = 1; ID < StudentManager.Students.Length; ID++)
				{
					if (StudentManager.Students[ID] != null && StudentManager.Students[ID].WitnessedMurder)
					{
						SpottedLabel.text = StudentManager.Students[ID].Name;
						SpottedWindow.SetActive(true);
						Chastise = true;
						GameOverID = 6;
						GameOver();
						Phase = 4;
					}
				}
			}
			if (NoCorpses)
			{
				for (ID = 1; ID < StudentManager.Students.Length; ID++)
				{
					if (StudentManager.Students[ID] != null && (StudentManager.Students[ID].WitnessedCorpse || StudentManager.Students[ID].WitnessedMurder))
					{
						SpottedLabel.text = StudentManager.Students[ID].Name;
						SpottedWindow.SetActive(true);
						Chastise = true;
						if (Yandere.DelinquentFighting)
						{
							GameOverID = 19;
						}
						else
						{
							GameOverID = 7;
						}
						GameOver();
						Phase = 4;
					}
				}
			}
			if (NoBlood)
			{
				if (Police.Deaths > 0)
				{
					CheckForBlood = true;
				}
				if (CheckForBlood)
				{
					if (Police.BloodParent.childCount == 0)
					{
						BloodTimer += Time.deltaTime;
						if (BloodTimer > 2f)
						{
							BloodCleaned = true;
						}
					}
					else
					{
						BloodTimer = 0f;
					}
				}
			}
			if (NoWeapon && !WeaponDisposed && Incinerator.Timer > 0f)
			{
				for (ID = 1; ID < Incinerator.DestroyedEvidence + 1; ID++)
				{
					if (Incinerator.EvidenceList[ID] == MurderWeaponID)
					{
						WeaponDisposed = true;
					}
				}
			}
			if (TimeLimit)
			{
				if (!Yandere.PauseScreen.Show)
				{
					TimeRemaining = Mathf.MoveTowards(TimeRemaining, 0f, Time.deltaTime);
				}
				int num2 = Mathf.CeilToInt(TimeRemaining);
				int num3 = num2 / 60;
				int num4 = num2 % 60;
				TimeLabel.text = string.Format("{0:00}:{1:00}", num3, num4);
				if (TimeRemaining == 0f)
				{
					Chastise = true;
					GameOverID = 10;
					GameOver();
					Phase = 4;
				}
			}
			if (Reputation.Reputation + Reputation.PendingRep <= -100f)
			{
				GameOverID = 14;
				GameOver();
				Phase = 4;
			}
			if (NoSuspicion && Reputation.Reputation + Reputation.PendingRep < 0f)
			{
				GameOverID = 14;
				GameOver();
				Phase = 4;
			}
			if (HeartbrokenCamera.activeInHierarchy)
			{
				HeartbrokenCamera.SetActive(false);
				GameOverID = 0;
				GameOver();
				Phase = 4;
			}
			if (Clock.PresentTime > 1080f)
			{
				GameOverID = 11;
				GameOver();
				Phase = 4;
			}
			else if (Police.FadeOut)
			{
				GameOverID = 12;
				GameOver();
				Phase = 4;
			}
			if (Yandere.ShoulderCamera.Noticed)
			{
				GameOverID = 17;
				GameOver();
				Phase = 4;
			}
			if (ExitPortal.activeInHierarchy)
			{
				if (Yandere.Chased || Yandere.Chasers > 0)
				{
					ExitPortalPrompt.Label[0].text = "     Cannot Exfiltrate!";
					ExitPortalPrompt.Circle[0].fillAmount = 1f;
				}
				else
				{
					ExitPortalPrompt.Label[0].text = "     Exfiltrate";
					if (ExitPortalPrompt.Circle[0].fillAmount == 0f)
					{
						Camera.main.transform.position = new Vector3(0.5f, 2.25f, -100.5f);
						Camera.main.transform.eulerAngles = Vector3.zero;
						Yandere.transform.eulerAngles = new Vector3(0f, 180f, 0f);
						Yandere.transform.position = new Vector3(0f, 0f, -94.5f);
						Yandere.Character.GetComponent<Animation>().Play(Yandere.WalkAnim);
						Yandere.RPGCamera.enabled = false;
						Yandere.HUD.gameObject.SetActive(false);
						Yandere.CanMove = false;
						AudioSource component = Jukebox.MissionMode.GetComponent<AudioSource>();
						component.clip = StealthMusic[10];
						component.loop = false;
						component.Play();
						GetComponent<AudioSource>().PlayOneShot(InfoAccomplished);
						HeartbeatCamera.SetActive(false);
						Boundary.enabled = false;
						Phase++;
					}
				}
			}
			if (TargetDead && CorpseDisposed && BloodCleaned && WeaponDisposed && DocumentsStolen && GameOverID == 0 && !ExitPortal.activeInHierarchy)
			{
				NotificationManager.DisplayNotification(NotificationType.Complete);
				NotificationManager.DisplayNotification(NotificationType.Exfiltrate);
				GetComponent<AudioSource>().PlayOneShot(InfoExfiltrate);
				ExitPortal.SetActive(true);
			}
			if (NoBlood && BloodCleaned && Police.BloodParent.childCount > 0)
			{
				ExitPortal.SetActive(false);
				BloodCleaned = false;
				BloodTimer = 0f;
			}
			if (!InfoRemark && GameOverID == 0 && TargetDead && (!CorpseDisposed || !BloodCleaned || !WeaponDisposed))
			{
				GetComponent<AudioSource>().PlayOneShot(InfoObjective);
				InfoRemark = true;
			}
		}
		else if (Phase == 3)
		{
			Timer += Time.deltaTime;
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - Time.deltaTime * 0.2f, Camera.main.transform.position.z);
			Yandere.transform.position = new Vector3(Yandere.transform.position.x, Yandere.transform.position.y, Yandere.transform.position.z - Time.deltaTime);
			if (Timer > 5f)
			{
				Success();
				Timer = 0f;
				Phase++;
			}
		}
		else
		{
			if (Phase != 4)
			{
				return;
			}
			Timer += 1f / 60f;
			if (Timer > 1f)
			{
				if (!FadeOut)
				{
					if (!PromptBar.Show)
					{
						PromptBar.Show = true;
					}
					else if (Input.GetButtonDown("A"))
					{
						PromptBar.Show = false;
						Destination = 1;
						FadeOut = true;
					}
					else if (Input.GetButtonDown("B"))
					{
						PromptBar.Show = false;
						Destination = 2;
						FadeOut = true;
					}
					else if (Input.GetButtonDown("X"))
					{
						PromptBar.Show = false;
						Destination = 3;
						FadeOut = true;
					}
				}
				else
				{
					Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, 1f / 60f));
					Jukebox.Dip = Mathf.MoveTowards(Jukebox.Dip, 0f, 1f / 60f);
					if (Darkness.color.a > 127f / 128f)
					{
						if (Destination == 1)
						{
							LoadingLabel.enabled = true;
							ResetGlobals();
							SceneManager.LoadScene(SceneManager.GetActiveScene().name);
						}
						else if (Destination == 2)
						{
							Globals.DeleteAll();
							SceneManager.LoadScene("MissionModeScene");
						}
						else if (Destination == 3)
						{
							Globals.DeleteAll();
							SceneManager.LoadScene("TitleScene");
						}
					}
				}
			}
			if (GameOverPhase == 1)
			{
				if (Timer > 2.5f)
				{
					if (Chastise)
					{
						GetComponent<AudioSource>().PlayOneShot(InfoFailure);
						GameOverPhase++;
					}
					else
					{
						GameOverPhase++;
						Timer += 5f;
					}
				}
			}
			else if (GameOverPhase == 2 && Timer > 7.5f)
			{
				Jukebox.MissionMode.GetComponent<AudioSource>().clip = StealthMusic[0];
				Jukebox.MissionMode.GetComponent<AudioSource>().Play();
				Jukebox.Volume = 0.5f;
				GameOverPhase++;
			}
		}
	}

	public void GameOver()
	{
		if (Yandere.Aiming)
		{
			Yandere.StopAiming();
		}
		if (Yandere.YandereVision)
		{
			Yandere.YandereVision = false;
			Yandere.ResetYandereEffects();
		}
		Yandere.enabled = false;
		GameOverReason.text = GameOverReasons[GameOverID];
		if (ColorCorrections.Length > 0)
		{
			ColorCorrections[2].enabled = true;
		}
		GetComponent<AudioSource>().PlayOneShot(GameOverSound);
		DetectionCamera.SetActive(false);
		HeartbeatCamera.SetActive(false);
		WitnessCamera.SetActive(false);
		GameOverText.SetActive(true);
		Yandere.HUD.gameObject.SetActive(false);
		Subtitle.SetActive(false);
		Time.timeScale = 0.0001f;
		GameOverPhase = 1;
		Jukebox.MissionMode.GetComponent<AudioSource>().Stop();
	}

	private void Success()
	{
		GameOverHeader.transform.localPosition = new Vector3(GameOverHeader.transform.localPosition.x, 0f, GameOverHeader.transform.localPosition.z);
		GameOverHeader.text = "MISSION ACCOMPLISHED";
		GameOverReason.gameObject.SetActive(false);
		ColorCorrections[2].enabled = true;
		DetectionCamera.SetActive(false);
		WitnessCamera.SetActive(false);
		GameOverText.SetActive(true);
		GameOverReason.text = string.Empty;
		Subtitle.SetActive(false);
		Jukebox.Volume = 1f;
		Time.timeScale = 0.0001f;
	}

	public void ChangeMusic()
	{
		MusicID++;
		if (MusicID > 9)
		{
			MusicID = 1;
		}
		Jukebox.MissionMode.GetComponent<AudioSource>().clip = StealthMusic[MusicID];
		Jukebox.MissionMode.GetComponent<AudioSource>().Play();
	}

	private void ResetGlobals()
	{
		int disableFarAnimations = OptionGlobals.DisableFarAnimations;
		bool disablePostAliasing = OptionGlobals.DisablePostAliasing;
		bool disableOutlines = OptionGlobals.DisableOutlines;
		int lowDetailStudents = OptionGlobals.LowDetailStudents;
		int particleCount = OptionGlobals.ParticleCount;
		bool disableShadows = OptionGlobals.DisableShadows;
		int drawDistance = OptionGlobals.DrawDistance;
		int drawDistanceLimit = OptionGlobals.DrawDistanceLimit;
		bool disableBloom = OptionGlobals.DisableBloom;
		bool fog = OptionGlobals.Fog;
		string missionTargetName = MissionModeGlobals.MissionTargetName;
		bool highPopulation = OptionGlobals.HighPopulation;
		Globals.DeleteAll();
		TutorialGlobals.TutorialsOff = true;
		for (int i = 1; i < 11; i++)
		{
			PlayerPrefs.SetInt("MissionModeTarget" + i, Target[i]);
			PlayerPrefs.SetInt("MissionModeMethod" + i, Method[i]);
		}
		SchoolGlobals.SchoolAtmosphere = 1f - (float)Difficulty * 0.1f;
		MissionModeGlobals.MissionTargetName = missionTargetName;
		MissionModeGlobals.MissionDifficulty = Difficulty;
		OptionGlobals.HighPopulation = highPopulation;
		MissionModeGlobals.MissionTarget = TargetID;
		SchoolGlobals.SchoolAtmosphereSet = true;
		MissionModeGlobals.MissionMode = true;
		MissionModeGlobals.MultiMission = MultiMission;
		MissionModeGlobals.MissionRequiredWeapon = RequiredWeaponID;
		MissionModeGlobals.MissionRequiredClothing = RequiredClothingID;
		MissionModeGlobals.MissionRequiredDisposal = RequiredDisposalID;
		ClassGlobals.BiologyGrade = 1;
		ClassGlobals.ChemistryGrade = 1;
		ClassGlobals.LanguageGrade = 1;
		ClassGlobals.PhysicalGrade = 1;
		ClassGlobals.PsychologyGrade = 1;
		for (ID = 2; ID < 11; ID++)
		{
			MissionModeGlobals.SetMissionCondition(ID, Conditions[ID]);
		}
		MissionModeGlobals.NemesisDifficulty = NemesisDifficulty;
		OptionGlobals.DisableFarAnimations = disableFarAnimations;
		OptionGlobals.DisablePostAliasing = disablePostAliasing;
		OptionGlobals.DisableOutlines = disableOutlines;
		OptionGlobals.LowDetailStudents = lowDetailStudents;
		OptionGlobals.ParticleCount = particleCount;
		OptionGlobals.DisableShadows = disableShadows;
		OptionGlobals.DrawDistance = drawDistance;
		OptionGlobals.DrawDistanceLimit = drawDistanceLimit;
		OptionGlobals.DisableBloom = disableBloom;
		OptionGlobals.Fog = fog;
	}

	private void ChangeAllText()
	{
		UILabel[] array = Object.FindObjectsOfType<UILabel>();
		UILabel[] array2 = array;
		foreach (UILabel uILabel in array2)
		{
			float a = uILabel.color.a;
			uILabel.color = new Color(1f, 1f, 1f, a);
			uILabel.trueTypeFont = Arial;
		}
		UISprite[] array3 = Object.FindObjectsOfType<UISprite>();
		UISprite[] array4 = array3;
		foreach (UISprite uISprite in array4)
		{
			float a2 = uISprite.color.a;
			if (uISprite.color != new Color(0f, 0f, 0f, a2))
			{
				uISprite.color = new Color(1f, 1f, 1f, a2);
			}
		}
	}

	private void CheckForCompletion()
	{
		if (!Checking[1] && !Checking[2] && !Checking[3] && !Checking[4] && !Checking[5] && !Checking[6] && !Checking[7] && !Checking[8] && !Checking[9] && !Checking[10])
		{
			TargetDead = true;
		}
	}
}
