using UnityEngine;

// FIXED BY TANOS [x]
public class StudentManagerScript : MonoBehaviour
{
	private PortraitChanScript NewPortraitChan;

	private GameObject NewStudent;

	public StudentScript[] Students;

	public SelectiveGrayscale SmartphoneSelectiveGreyscale;

	public PickpocketMinigameScript PickpocketMinigame;

	public PopulationManagerScript PopulationManager;

	public SelectiveGrayscale HandSelectiveGreyscale;

	public SkinnedMeshRenderer FemaleShowerCurtain;

	public CleaningManagerScript CleaningManager;

	public StolenPhoneSpotScript StolenPhoneSpot;

	public SelectiveGrayscale SelectiveGreyscale;

	public CombatMinigameScript CombatMinigame;

	public DatingMinigameScript DatingMinigame;

	public TextureManagerScript TextureManager;

	public TutorialWindowScript TutorialWindow;

	public QualityManagerScript QualityManager;

	public ComputerGamesScript ComputerGames;

	public EmergencyExitScript EmergencyExit;

	public MemorialSceneScript MemorialScene;

	public TranqDetectorScript TranqDetector;

	public WitnessCameraScript WitnessCamera;

	public ConvoManagerScript ConvoManager;

	public TallLockerScript CommunalLocker;

	public CabinetDoorScript CabinetDoor;

	public LightSwitchScript LightSwitch;

	public LoveManagerScript LoveManager;

	public MiyukiEnemyScript MiyukiEnemy;

	public TaskManagerScript TaskManager;

	public StudentScript BloodReporter;

	public HeadmasterScript Headmaster;

	public NoteWindowScript NoteWindow;

	public ReputationScript Reputation;

	public WeaponScript FragileWeapon;

	public AudioSource PracticeVocals;

	public AudioSource PracticeMusic;

	public ContainerScript Container;

	public RedStringScript RedString;

	public RingEventScript RingEvent;

	public GazerEyesScript Shinigami;

	public HologramScript Holograms;

	public RobotArmScript RobotArms;

	public PickUpScript Flashlight;

	public FountainScript Fountain;

	public PoseModeScript PoseMode;

	public TrashCanScript TrashCan;

	public Collider LockerRoomArea;

	public StudentScript Reporter;

	public DoorScript GamingDoor;

	public GhostScript GhostChan;

	public YandereScript Yandere;

	public ListScript MeetSpots;

	public PoliceScript Police;

	public DoorScript ShedDoor;

	public UILabel ErrorLabel;

	public RestScript Rest;

	public TagScript Tag;

	public Collider EastBathroomArea;

	public Collider WestBathroomArea;

	public Collider IncineratorArea;

	public Collider HeadmasterArea;

	public Collider NEStairs;

	public Collider NWStairs;

	public Collider SEStairs;

	public Collider SWStairs;

	public DoorScript AltFemaleVomitDoor;

	public DoorScript FemaleVomitDoor;

	public CounselorDoorScript[] CounselorDoor;

	public ParticleSystem AltFemaleDrownSplashes;

	public ParticleSystem FemaleDrownSplashes;

	public OfferHelpScript FragileOfferHelp;

	public OfferHelpScript OfferHelp;

	public Transform AltFemaleVomitSpot;

	public ListScript SearchPatrols;

	public ListScript CleaningSpots;

	public ListScript Patrols;

	public ClockScript Clock;

	public JsonScript JSON;

	public GateScript Gate;

	public ListScript EntranceVectors;

	public ListScript GoAwaySpots;

	public ListScript HidingSpots;

	public ListScript LunchSpots;

	public ListScript Hangouts;

	public ListScript Lockers;

	public ListScript Podiums;

	public ListScript Clubs;

	public ChangingBoothScript[] ChangingBooths;

	public GradingPaperScript[] FacultyDesks;

	public StudentScript[] WitnessList;

	public StudentScript[] Teachers;

	public GameObject[] Graffiti;

	public ListScript[] Seats;

	public Collider[] Blood;

	public Collider[] Limbs;

	public Transform[] TeacherGuardLocation;

	public Transform[] CorpseGuardLocation;

	public Transform[] BloodGuardLocation;

	public Transform[] SleuthDestinations;

	public Transform[] GardeningPatrols;

	public Transform[] MartialArtsSpots;

	public Transform[] LockerPositions;

	public Transform[] BackstageSpots;

	public Transform[] SpawnPositions;

	public Transform[] GraffitiSpots;

	public Transform[] PracticeSpots;

	public Transform[] SunbatheSpots;

	public Transform[] MeetingSpots;

	public Transform[] PinDownSpots;

	public Transform[] ShockedSpots;

	public Transform[] MiyukiSpots;

	public Transform[] SocialSeats;

	public Transform[] SocialSpots;

	public Transform[] SupplySpots;

	public Transform[] DramaSpots;

	public Transform[] BullySpots;

	public Transform[] ClubZones;

	public Transform[] SulkSpots;

	public Transform[] FleeSpots;

	public Transform[] Plates;

	public Transform[] FemaleVomitSpots;

	public Transform[] MaleVomitSpots;

	public Transform[] FemaleWashSpots;

	public Transform[] MaleWashSpots;

	public DoorScript[] FemaleToiletDoors;

	public DoorScript[] MaleToiletDoors;

	public DrinkingFountainScript[] DrinkingFountains;

	public bool[] SeatsTaken11;

	public bool[] SeatsTaken12;

	public bool[] SeatsTaken21;

	public bool[] SeatsTaken22;

	public bool[] SeatsTaken31;

	public bool[] SeatsTaken32;

	public bool[] NoBully;

	public Quaternion[] OriginalClubRotations;

	public Vector3[] OriginalClubPositions;

	public Collider RivalDeskCollider;

	public Transform FollowerLookAtTarget;

	public Transform SuitorConfessionSpot;

	public Transform RivalConfessionSpot;

	public Transform OriginalLyricsSpot;

	public Transform FragileSlaveSpot;

	public Transform FemaleCoupleSpot;

	public Transform YandereStripSpot;

	public Transform FemaleBatheSpot;

	public Transform FemaleStalkSpot;

	public Transform FemaleStripSpot;

	public Transform FemaleVomitSpot;

	public Transform MedicineCabinet;

	public Transform ConfessionSpot;

	public Transform CorpseLocation;

	public Transform FemaleRestSpot;

	public Transform FemaleWashSpot;

	public Transform MaleCoupleSpot;

	public Transform AirGuitarSpot;

	public Transform BloodLocation;

	public Transform FastBatheSpot;

	public Transform InfirmarySeat;

	public Transform MaleBatheSpot;

	public Transform MaleStalkSpot;

	public Transform MaleStripSpot;

	public Transform MaleVomitSpot;

	public Transform SacrificeSpot;

	public Transform WeaponBoxSpot;

	public Transform FountainSpot;

	public Transform MaleWashSpot;

	public Transform SenpaiLocker;

	public Transform SuitorLocker;

	public Transform MaleRestSpot;

	public Transform RomanceSpot;

	public Transform BrokenSpot;

	public Transform BullyGroup;

	public Transform EdgeOfGrid;

	public Transform GoAwaySpot;

	public Transform LyricsSpot;

	public Transform MainCamera;

	public Transform SuitorSpot;

	public Transform ToolTarget;

	public Transform MiyukiCat;

	public Transform MournSpot;

	public Transform ShameSpot;

	public Transform SlaveSpot;

	public Transform Papers;

	public Transform Exit;

	public GameObject LovestruckCamera;

	public GameObject DelinquentRadio;

	public GameObject GardenBlockade;

	public GameObject PortraitChan;

	public GameObject RandomPatrol;

	public GameObject ChaseCamera;

	public GameObject EmptyObject;

	public GameObject PortraitKun;

	public GameObject StudentChan;

	public GameObject StudentKun;

	public GameObject RivalChan;

	public GameObject Medicine;

	public GameObject DrumSet;

	public GameObject Flowers;

	public GameObject Portal;

	public float[] SpawnTimes;

	public int LowDetailThreshold;

	public int FarAnimThreshold;

	public int MartialArtsPhase;

	public int StudentsSpawned;

	public int SedatedStudents;

	public int StudentsTotal = 13;

	public int TeachersTotal = 6;

	public int NPCsSpawned;

	public int SleuthPhase = 1;

	public int DramaPhase = 1;

	public int NPCsTotal;

	public int Witnesses;

	public int PinPhase;

	public int Bullies;

	public int Speaker = 21;

	public int Frame;

	public int GymTeacherID = 100;

	public int ObstacleID = 6;

	public int CurrentID;

	public int SuitorID = 13;

	public int VictimID;

	public int NurseID = 93;

	public int RivalID = 7;

	public int SpawnID;

	public int ID;

	public bool ReactedToGameLeader;

	public bool MurderTakingPlace;

	public bool TakingPortraits;

	public bool TeachersSpawned;

	public bool DisableFarAnims;

	public bool MetalDetectors;

	public bool YandereVisible;

	public bool NoClubMeeting;

	public bool UpdatedBlood;

	public bool YandereDying;

	public bool YandereLate;

	public bool FirstUpdate;

	public bool MissionMode;

	public bool OpenCurtain;

	public bool PinningDown;

	public bool ForceSpawn;

	public bool NoGravity;

	public bool Randomize;

	public bool LoveSick;

	public bool NoSpeech;

	public bool Meeting;

	public bool Censor;

	public bool Spooky;

	public bool Bully;

	public bool Gaze;

	public bool Pose;

	public bool Sans;

	public bool Stop;

	public bool Egg;

	public bool Six;

	public bool AoT;

	public bool DK;

	public float Atmosphere;

	public float OpenValue = 100f;

	public float MeetingTimer;

	public float PinDownTimer;

	public float ChangeTimer;

	public float SleuthTimer;

	public float DramaTimer;

	public float LowestRep;

	public float PinTimer;

	public float Timer;

	public string[] ColorNames;

	public string[] MaleNames;

	public string[] FirstNames;

	public string[] LastNames;

	public AudioClip YanderePinDown;

	public AudioClip PinDownSFX;

	[SerializeField]
	private int ProblemID = -1;

	public GameObject Cardigan;

	public SkinnedMeshRenderer CardiganRenderer;

	public Mesh OpenChipBag;

	public bool SeatOccupied;

	public int Class = 1;

	public int Thins;

	public int Seriouses;

	public int Rounds;

	public int Sads;

	public int Means;

	public int Smugs;

	public int Gentles;

	public int Rival1s;

	public DoorScript[] Doors;

	public int DoorID;

	private void Start()
	{
		LoveSick = GameGlobals.LoveSick;
		MetalDetectors = SchoolGlobals.HighSecurity;
		if (ClubGlobals.GetClubClosed(ClubType.LightMusic))
		{
			SpawnPositions[51].position = new Vector3(3f, 0f, -95f);
		}
		if (HomeGlobals.LateForSchool)
		{
			HomeGlobals.LateForSchool = false;
			YandereLate = true;
			Debug.Log("Yandere-chan is late for school!");
		}
		if (!YandereLate && StudentGlobals.MemorialStudents > 0)
		{
			Yandere.HUD.alpha = 0f;
			Yandere.HeartCamera.enabled = false;
		}
		if (GameGlobals.Profile == 0)
		{
			GameGlobals.Profile = 1;
		}
		for (ID = 76; ID < 81; ID++)
		{
			if (StudentGlobals.GetStudentReputation(ID) > -67)
			{
				StudentGlobals.SetStudentReputation(ID, -67);
			}
		}
		if (ClubGlobals.GetClubClosed(ClubType.Gardening))
		{
			GardenBlockade.SetActive(true);
			Flowers.SetActive(false);
		}
		ID = 0;
		for (ID = 1; ID < JSON.Students.Length; ID++)
		{
			if (!JSON.Students[ID].Success)
			{
				ProblemID = ID;
				break;
			}
		}
		if (ProblemID != -1)
		{
			if (ErrorLabel != null)
			{
				ErrorLabel.text = string.Empty;
				ErrorLabel.enabled = false;
			}
			if (MissionModeGlobals.MissionMode)
			{
				StudentGlobals.FemaleUniform = 5;
				StudentGlobals.MaleUniform = 5;
			}
			SetAtmosphere();
			GameGlobals.Paranormal = false;
			if (StudentGlobals.GetStudentSlave() > 0 && !StudentGlobals.GetStudentDead(StudentGlobals.GetStudentSlave()))
			{
				int studentSlave = StudentGlobals.GetStudentSlave();
				ForceSpawn = true;
				SpawnPositions[studentSlave] = SlaveSpot;
				SpawnID = studentSlave;
				StudentGlobals.SetStudentDead(studentSlave, false);
				SpawnStudent(SpawnID);
				Students[studentSlave].Slave = true;
				SpawnID = 0;
			}
			if (StudentGlobals.GetStudentFragileSlave() > 0 && !StudentGlobals.GetStudentDead(StudentGlobals.GetStudentFragileSlave()))
			{
				int studentFragileSlave = StudentGlobals.GetStudentFragileSlave();
				ForceSpawn = true;
				SpawnPositions[studentFragileSlave] = FragileSlaveSpot;
				SpawnID = studentFragileSlave;
				StudentGlobals.SetStudentDead(studentFragileSlave, false);
				SpawnStudent(SpawnID);
				Students[studentFragileSlave].FragileSlave = true;
				Students[studentFragileSlave].Slave = true;
				SpawnID = 0;
			}
			NPCsTotal = StudentsTotal + TeachersTotal;
			SpawnID = 1;
			if (StudentGlobals.MaleUniform == 0)
			{
				StudentGlobals.MaleUniform = 1;
			}
			for (ID = 1; ID < NPCsTotal + 1; ID++)
			{
				if (!StudentGlobals.GetStudentDead(ID))
				{
					StudentGlobals.SetStudentDying(ID, false);
				}
			}
			if (!TakingPortraits)
			{
				for (ID = 1; ID < Lockers.List.Length; ID++)
				{
					LockerPositions[ID].transform.position = Lockers.List[ID].position + Lockers.List[ID].forward * 0.5f;
					LockerPositions[ID].LookAt(Lockers.List[ID].position);
				}
				for (ID = 1; ID < HidingSpots.List.Length; ID++)
				{
					if (HidingSpots.List[ID] == null)
					{
						GameObject gameObject = Object.Instantiate(EmptyObject, new Vector3(Random.Range(-17f, 17f), 0f, Random.Range(-17f, 17f)), Quaternion.identity);
						while (gameObject.transform.position.x < 2.5f && gameObject.transform.position.x > -2.5f && gameObject.transform.position.z > -2.5f && gameObject.transform.position.z < 2.5f)
						{
							gameObject.transform.position = new Vector3(Random.Range(-17f, 17f), 0f, Random.Range(-17f, 17f));
						}
						gameObject.transform.parent = HidingSpots.transform;
						HidingSpots.List[ID] = gameObject.transform;
					}
				}
			}
			if (YandereLate)
			{
				Clock.PresentTime = 480f;
				Clock.HourTime = 8f;
				SkipTo8();
			}
			if (!TakingPortraits)
			{
				while (SpawnID < NPCsTotal + 1)
				{
					SpawnStudent(SpawnID);
					SpawnID++;
				}
				Graffiti[1].SetActive(false);
				Graffiti[2].SetActive(false);
				Graffiti[3].SetActive(false);
				Graffiti[4].SetActive(false);
				Graffiti[5].SetActive(false);
			}
		}
		else
		{
			string text = string.Empty;
			if (ProblemID > 1)
			{
				text = "The problem may be caused by Student " + ProblemID + ".";
			}
			if (ErrorLabel != null)
			{
				ErrorLabel.text = "The game cannot compile Students.JSON! There is a typo somewhere in the JSON file. The problem might be a missing quotation mark, a missing colon, a missing comma, or something else like that. Please find your typo and fix it, or revert to a backup of the JSON file. " + text;
				ErrorLabel.enabled = true;
			}
		}
		NEStairs = GameObject.Find("NEStairs").GetComponent<Collider>();
		NWStairs = GameObject.Find("NWStairs").GetComponent<Collider>();
		SEStairs = GameObject.Find("SEStairs").GetComponent<Collider>();
		SWStairs = GameObject.Find("SWStairs").GetComponent<Collider>();
	}

	public void SetAtmosphere()
	{
		if (GameGlobals.LoveSick)
		{
			SchoolGlobals.SchoolAtmosphereSet = true;
			SchoolGlobals.SchoolAtmosphere = 0f;
		}
		if (!MissionModeGlobals.MissionMode)
		{
			if (!SchoolGlobals.SchoolAtmosphereSet)
			{
				SchoolGlobals.SchoolAtmosphereSet = true;
				SchoolGlobals.SchoolAtmosphere = 1f;
			}
			Atmosphere = SchoolGlobals.SchoolAtmosphere;
		}
		Vignetting[] components = Camera.main.GetComponents<Vignetting>();
		float num = 1f - Atmosphere;
		if (!TakingPortraits)
		{
			SelectiveGreyscale.desaturation = num;
			if (HandSelectiveGreyscale != null)
			{
				HandSelectiveGreyscale.desaturation = num;
				SmartphoneSelectiveGreyscale.desaturation = num;
			}
			components[2].intensity = num * 5f;
			components[2].blur = num;
			components[2].chromaticAberration = num * 5f;
			float num2 = 1f - num;
			RenderSettings.fogColor = new Color(num2, num2, num2, 1f);
			Camera.main.backgroundColor = new Color(num2, num2, num2, 1f);
			RenderSettings.fogDensity = num * 0.1f;
		}
	}

	private void Update()
	{
		if (!TakingPortraits)
		{
			if (!Yandere.ShoulderCamera.Counselor.Interrogating)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			Frame++;
			if (!FirstUpdate)
			{
				QualityManager.UpdateOutlines();
				FirstUpdate = true;
				AssignTeachers();
			}
			if (Frame == 3)
			{
				LoveManager.CoupleCheck();
				if (Bullies > 0)
				{
					DetermineVictim();
				}
				UpdateStudents();
				if (!OptionGlobals.RimLight)
				{
					QualityManager.RimLight();
				}
				for (ID = 26; ID < 31; ID++)
				{
					if (Students[ID] != null)
					{
						OriginalClubPositions[ID - 25] = Clubs.List[ID].position;
						OriginalClubRotations[ID - 25] = Clubs.List[ID].rotation;
					}
				}
				if (!TakingPortraits)
				{
					TaskManager.UpdateTaskStatus();
				}
				Yandere.GloveAttacher.newRenderer.enabled = false;
				UpdateAprons();
				if (PlayerPrefs.GetInt("LoadingSave") == 1)
				{
					Load();
					PlayerPrefs.SetInt("LoadingSave", 0);
				}
				if (!YandereLate && StudentGlobals.MemorialStudents > 0)
				{
					Yandere.HUD.alpha = 0f;
					Yandere.RPGCamera.transform.position = new Vector3(38f, 4.125f, 68.825f);
					Yandere.RPGCamera.transform.eulerAngles = new Vector3(22.5f, 67.5f, 0f);
					Yandere.RPGCamera.transform.Translate(Vector3.forward, Space.Self);
					Yandere.RPGCamera.enabled = false;
					Yandere.HeartCamera.enabled = false;
					Yandere.CanMove = false;
					Clock.StopTime = true;
					StopMoving();
					MemorialScene.gameObject.SetActive(true);
					MemorialScene.enabled = true;
				}
				for (ID = 1; ID < 90; ID++)
				{
					if (Students[ID] != null)
					{
						Students[ID].ShoeRemoval.Start();
					}
				}
			}
			if ((double)Clock.HourTime > 16.9)
			{
				CheckMusic();
			}
		}
		else if (NPCsSpawned < StudentsTotal + TeachersTotal)
		{
			Frame++;
			if (Frame == 1)
			{
				if (NewStudent != null)
				{
					Object.Destroy(NewStudent);
				}
				if (Randomize)
				{
					int num = Random.Range(0, 2);
					NewStudent = Object.Instantiate((num != 0) ? PortraitKun : PortraitChan, Vector3.zero, Quaternion.identity);
				}
				else
				{
					NewStudent = Object.Instantiate((JSON.Students[NPCsSpawned + 1].Gender != 0) ? PortraitKun : PortraitChan, Vector3.zero, Quaternion.identity);
				}
				NewStudent.GetComponent<CosmeticScript>().StudentID = NPCsSpawned + 1;
				NewStudent.GetComponent<CosmeticScript>().StudentManager = this;
				NewStudent.GetComponent<CosmeticScript>().TakingPortrait = true;
				NewStudent.GetComponent<CosmeticScript>().Randomize = Randomize;
				NewStudent.GetComponent<CosmeticScript>().JSON = JSON;
				if (!Randomize)
				{
					NPCsSpawned++;
				}
			}
			if (Frame == 2)
			{
				ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Portraits/Student_" + NPCsSpawned + ".png");
				Frame = 0;
			}
		}
		else
		{
			ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Portraits/Student_" + NPCsSpawned + ".png");
			base.gameObject.SetActive(false);
		}
		if (Witnesses > 0)
		{
			for (ID = 1; ID < WitnessList.Length; ID++)
			{
				StudentScript studentScript = WitnessList[ID];
				if (studentScript != null && (!studentScript.Alive || studentScript.Attacked || studentScript.Dying || (studentScript.Fleeing && !studentScript.PinningDown)))
				{
					studentScript.PinDownWitness = false;
					studentScript = null;
					if (ID != WitnessList.Length - 1)
					{
						Shuffle(ID);
					}
					Witnesses--;
				}
			}
			if (PinningDown && Witnesses < 4)
			{
				Debug.Log("Students were going to pin Yandere-chan down, but now there are less than 4 witnesses, so it's not going to happen.");
				if (!Yandere.Chased && Yandere.Chasers == 0)
				{
					Yandere.CanMove = true;
				}
				PinningDown = false;
				PinDownTimer = 0f;
				PinPhase = 0;
			}
		}
		if (PinningDown)
		{
			if (!Yandere.Attacking && Yandere.CanMove)
			{
				Yandere.CharacterAnimation.CrossFade("f02_pinDownPanic_00");
				Yandere.EmptyHands();
				Yandere.CanMove = false;
			}
			if (PinPhase == 1)
			{
				if (!Yandere.Attacking && !Yandere.Struggling)
				{
					PinTimer += Time.deltaTime;
				}
				if (PinTimer > 1f)
				{
					for (ID = 1; ID < 5; ID++)
					{
						StudentScript studentScript2 = WitnessList[ID];
						if (studentScript2 != null)
						{
							studentScript2.transform.position = new Vector3(studentScript2.transform.position.x, studentScript2.transform.position.y + 0.1f, studentScript2.transform.position.z);
							studentScript2.CurrentDestination = PinDownSpots[ID];
							studentScript2.Pathfinding.target = PinDownSpots[ID];
							studentScript2.SprintAnim = studentScript2.OriginalSprintAnim;
							studentScript2.DistanceToDestination = 100f;
							studentScript2.Pathfinding.speed = 5f;
							studentScript2.MyController.radius = 0f;
							studentScript2.PinningDown = true;
							studentScript2.Alarmed = false;
							studentScript2.Routine = false;
							studentScript2.Fleeing = true;
							studentScript2.AlarmTimer = 0f;
							studentScript2.SmartPhone.SetActive(false);
							studentScript2.Safe = true;
							studentScript2.Prompt.Hide();
							studentScript2.Prompt.enabled = false;
							Debug.Log(string.Concat(studentScript2, "'s current destination is ", studentScript2.CurrentDestination));
						}
					}
					PinPhase++;
				}
			}
			else if (WitnessList[1].PinPhase == 0)
			{
				PinDownTimer += Time.deltaTime;
				if (PinDownTimer > 10f || (WitnessList[1].DistanceToDestination < 1f && WitnessList[2].DistanceToDestination < 1f && WitnessList[3].DistanceToDestination < 1f && WitnessList[4].DistanceToDestination < 1f))
				{
					Clock.StopTime = true;
					if (Yandere.Aiming)
					{
						Yandere.StopAiming();
						Yandere.enabled = false;
					}
					Yandere.Mopping = false;
					Yandere.EmptyHands();
					AudioSource component = GetComponent<AudioSource>();
					component.PlayOneShot(PinDownSFX);
					component.PlayOneShot(YanderePinDown);
					Yandere.CharacterAnimation.CrossFade("f02_pinDown_00");
					Yandere.CanMove = false;
					Yandere.ShoulderCamera.LookDown = true;
					Yandere.RPGCamera.enabled = false;
					StopMoving();
					Yandere.ShoulderCamera.HeartbrokenCamera.GetComponent<Camera>().cullingMask |= 512;
					for (ID = 1; ID < 5; ID++)
					{
						StudentScript studentScript3 = WitnessList[ID];
						if (studentScript3.MyWeapon != null)
						{
							GameObjectUtils.SetLayerRecursively(studentScript3.MyWeapon.gameObject, 13);
						}
						studentScript3.CharacterAnimation.CrossFade((((!studentScript3.Male) ? "f02_pinDown_0" : "pinDown_0") + ID).ToString());
						studentScript3.PinPhase++;
					}
				}
			}
			else
			{
				bool flag = false;
				if (!WitnessList[1].Male)
				{
					if (WitnessList[1].CharacterAnimation["f02_pinDown_01"].time >= WitnessList[1].CharacterAnimation["f02_pinDown_01"].length)
					{
						flag = true;
					}
				}
				else if (WitnessList[1].CharacterAnimation["pinDown_01"].time >= WitnessList[1].CharacterAnimation["pinDown_01"].length)
				{
					flag = true;
				}
				if (flag)
				{
					Yandere.CharacterAnimation.CrossFade("f02_pinDownLoop_00");
					for (ID = 1; ID < 5; ID++)
					{
						StudentScript studentScript4 = WitnessList[ID];
						studentScript4.CharacterAnimation.CrossFade((((!studentScript4.Male) ? "f02_pinDownLoop_0" : "pinDownLoop_0") + ID).ToString());
					}
					PinningDown = false;
				}
			}
		}
		if (Meeting)
		{
			UpdateMeeting();
		}
		if (Input.GetKeyDown("space"))
		{
			DetermineVictim();
		}
		if (Police.BloodParent.childCount > 0 || Police.LimbParent.childCount > 0 || Yandere.WeaponManager.MisplacedWeapons > 0)
		{
			CurrentID++;
			if (CurrentID > 97)
			{
				UpdateBlood();
				CurrentID = 1;
			}
			if (Students[CurrentID] == null)
			{
				CurrentID++;
			}
			else if (!Students[CurrentID].gameObject.activeInHierarchy)
			{
				CurrentID++;
			}
		}
		if (OpenCurtain)
		{
			OpenValue = Mathf.Lerp(OpenValue, 100f, Time.deltaTime * 10f);
			if (OpenValue > 99f)
			{
				OpenCurtain = false;
			}
			FemaleShowerCurtain.SetBlendShapeWeight(0, OpenValue);
		}
		YandereVisible = false;
	}

	public void SpawnStudent(int spawnID)
	{
		bool flag = false;
		if (spawnID > 9 && spawnID < 21)
		{
			flag = true;
		}
		if (!flag && Students[spawnID] == null && !StudentGlobals.GetStudentDead(spawnID) && !StudentGlobals.GetStudentKidnapped(spawnID) && !StudentGlobals.GetStudentArrested(spawnID) && !StudentGlobals.GetStudentExpelled(spawnID) && JSON.Students[spawnID].Name != "Unknown" && JSON.Students[spawnID].Name != "Reserved" && StudentGlobals.GetStudentReputation(spawnID) > -100)
		{
			int num = 0;
			if (JSON.Students[spawnID].Name == "Random")
			{
				GameObject gameObject = Object.Instantiate(EmptyObject, new Vector3(Random.Range(-17f, 17f), 0f, Random.Range(-17f, 17f)), Quaternion.identity);
				while (gameObject.transform.position.x < 2.5f && gameObject.transform.position.x > -2.5f && gameObject.transform.position.z > -2.5f && gameObject.transform.position.z < 2.5f)
				{
					gameObject.transform.position = new Vector3(Random.Range(-17f, 17f), 0f, Random.Range(-17f, 17f));
				}
				gameObject.transform.parent = HidingSpots.transform;
				HidingSpots.List[spawnID] = gameObject.transform;
				GameObject gameObject2 = Object.Instantiate(RandomPatrol, Vector3.zero, Quaternion.identity);
				gameObject2.transform.parent = Patrols.transform;
				Patrols.List[spawnID] = gameObject2.transform;
				GameObject gameObject3 = Object.Instantiate(RandomPatrol, Vector3.zero, Quaternion.identity);
				gameObject3.transform.parent = CleaningSpots.transform;
				CleaningSpots.List[spawnID] = gameObject3.transform;
				num = ((!MissionModeGlobals.MissionMode || MissionModeGlobals.MissionTarget != spawnID) ? Random.Range(0, 2) : 0);
				FindUnoccupiedSeat();
			}
			else
			{
				num = JSON.Students[spawnID].Gender;
			}
			NewStudent = Object.Instantiate((num != 0) ? StudentKun : StudentChan, SpawnPositions[spawnID].position, Quaternion.identity);
			NewStudent.GetComponent<CosmeticScript>().LoveManager = LoveManager;
			NewStudent.GetComponent<CosmeticScript>().StudentManager = this;
			NewStudent.GetComponent<CosmeticScript>().Randomize = Randomize;
			NewStudent.GetComponent<CosmeticScript>().StudentID = spawnID;
			NewStudent.GetComponent<CosmeticScript>().JSON = JSON;
			if (JSON.Students[spawnID].Name == "Random")
			{
				NewStudent.GetComponent<StudentScript>().CleaningSpot = CleaningSpots.List[spawnID];
				NewStudent.GetComponent<StudentScript>().CleaningRole = 3;
			}
			if (JSON.Students[spawnID].Club == ClubType.Bully)
			{
				Bullies++;
			}
			Students[spawnID] = NewStudent.GetComponent<StudentScript>();
			StudentScript studentScript = Students[spawnID];
			studentScript.ChaseSelectiveGrayscale.desaturation = 1f - SchoolGlobals.SchoolAtmosphere;
			studentScript.Cosmetic.TextureManager = TextureManager;
			studentScript.WitnessCamera = WitnessCamera;
			studentScript.StudentManager = this;
			studentScript.StudentID = spawnID;
			studentScript.JSON = JSON;
			if (studentScript.Miyuki != null)
			{
				studentScript.Miyuki.Enemy = MiyukiCat;
			}
			if (AoT)
			{
				studentScript.AoT = true;
			}
			if (DK)
			{
				studentScript.DK = true;
			}
			if (Spooky)
			{
				studentScript.Spooky = true;
			}
			if (Sans)
			{
				studentScript.BadTime = true;
			}
			if (spawnID == RivalID)
			{
				studentScript.Rival = true;
			}
			if (spawnID == 1)
			{
				RedString.Target = studentScript.LeftPinky;
			}
			OccupySeat();
		}
		NPCsSpawned++;
		ForceSpawn = false;
	}

	public void UpdateStudents(int SpecificStudent = 0)
	{
		ID = 2;
		while (ID < Students.Length)
		{
			bool flag = false;
			if (SpecificStudent != 0)
			{
				ID = SpecificStudent;
				flag = true;
			}
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				if (studentScript.gameObject.activeInHierarchy)
				{
					if (!studentScript.Safe)
					{
						if (!studentScript.Slave)
						{
							if (studentScript.Pushable)
							{
								studentScript.Prompt.Label[0].text = "     Push";
							}
							else if (Yandere.SpiderGrow)
							{
								if (!studentScript.Cosmetic.Empty)
								{
									studentScript.Prompt.Label[0].text = "     Send Husk";
								}
								else
								{
									studentScript.Prompt.Label[0].text = "     Talk";
								}
							}
							else if (!studentScript.Following)
							{
								studentScript.Prompt.Label[0].text = "     Talk";
							}
							else
							{
								studentScript.Prompt.Label[0].text = "     Stop";
							}
							studentScript.Prompt.HideButton[0] = false;
							studentScript.Prompt.HideButton[2] = false;
							studentScript.Prompt.Attack = false;
							if (Yandere.Mask != null)
							{
								studentScript.Prompt.HideButton[0] = true;
							}
							if (Yandere.Dragging || Yandere.PickUp != null || Yandere.Chased)
							{
								studentScript.Prompt.HideButton[0] = true;
								studentScript.Prompt.HideButton[2] = true;
								if (Yandere.PickUp != null && !studentScript.Following)
								{
									if (Yandere.PickUp.Food > 0)
									{
										studentScript.Prompt.Label[0].text = "     Feed";
										studentScript.Prompt.HideButton[0] = false;
										studentScript.Prompt.HideButton[2] = true;
									}
									else if (Yandere.PickUp.Salty)
									{
										studentScript.Prompt.Label[0].text = "     Give Snack";
										studentScript.Prompt.HideButton[0] = false;
										studentScript.Prompt.HideButton[2] = true;
									}
									else if (Yandere.PickUp.StuckBoxCutter != null)
									{
										studentScript.Prompt.Label[0].text = "     Ask For Help";
										studentScript.Prompt.HideButton[0] = false;
										studentScript.Prompt.HideButton[2] = true;
									}
								}
							}
							if (Yandere.Armed)
							{
								studentScript.Prompt.HideButton[0] = true;
								studentScript.Prompt.MinimumDistance = 1f;
								studentScript.Prompt.Attack = true;
							}
							else
							{
								studentScript.Prompt.HideButton[2] = true;
								studentScript.Prompt.MinimumDistance = 2f;
								if (studentScript.WitnessedMurder || studentScript.WitnessedCorpse || studentScript.Private)
								{
									studentScript.Prompt.HideButton[0] = true;
								}
							}
							if (Yandere.NearBodies > 0 || Yandere.Sanity < 33.33333f)
							{
								studentScript.Prompt.HideButton[0] = true;
							}
							if (studentScript.Teacher)
							{
								studentScript.Prompt.HideButton[0] = true;
							}
						}
						else if (!studentScript.FragileSlave)
						{
							if (Yandere.Armed)
							{
								if (Yandere.EquippedWeapon.Concealable)
								{
									studentScript.Prompt.HideButton[0] = false;
									studentScript.Prompt.Label[0].text = "     Give Weapon";
								}
								else
								{
									studentScript.Prompt.HideButton[0] = true;
									studentScript.Prompt.Label[0].text = string.Empty;
								}
							}
							else
							{
								studentScript.Prompt.HideButton[0] = true;
								studentScript.Prompt.Label[0].text = string.Empty;
							}
						}
					}
					if (NoSpeech && !studentScript.Armband.activeInHierarchy)
					{
						studentScript.Prompt.HideButton[0] = true;
					}
				}
				if (studentScript.Prompt.Label[0] != null)
				{
					if (Sans)
					{
						studentScript.Prompt.HideButton[0] = false;
						studentScript.Prompt.Label[0].text = "     Psychokinesis";
					}
					if (Pose)
					{
						studentScript.Prompt.HideButton[0] = false;
						studentScript.Prompt.Label[0].text = "     Pose";
						studentScript.Prompt.BloodMask = 1;
						studentScript.Prompt.BloodMask |= 2;
						studentScript.Prompt.BloodMask |= 512;
						studentScript.Prompt.BloodMask |= 8192;
						studentScript.Prompt.BloodMask |= 16384;
						studentScript.Prompt.BloodMask |= 65536;
						studentScript.Prompt.BloodMask |= 2097152;
						studentScript.Prompt.BloodMask = ~studentScript.Prompt.BloodMask;
					}
					if (!studentScript.Teacher && Six)
					{
						studentScript.Prompt.MinimumDistance = 0.75f;
						studentScript.Prompt.HideButton[0] = false;
						studentScript.Prompt.Label[0].text = "     Eat";
					}
					if (Gaze)
					{
						studentScript.Prompt.MinimumDistance = 5f;
						studentScript.Prompt.HideButton[0] = false;
						studentScript.Prompt.Label[0].text = "     Gaze";
					}
				}
				if (GameGlobals.EmptyDemon)
				{
					studentScript.Prompt.HideButton[0] = false;
				}
			}
			ID++;
			if (flag)
			{
				ID = Students.Length;
			}
		}
		Container.UpdatePrompts();
		TrashCan.UpdatePrompt();
	}

	public void UpdateMe(int ID)
	{
		if (ID <= 1)
		{
			return;
		}
		StudentScript studentScript = Students[ID];
		if (!studentScript.Safe)
		{
			studentScript.Prompt.Label[0].text = "     Talk";
			studentScript.Prompt.HideButton[0] = false;
			studentScript.Prompt.HideButton[2] = false;
			studentScript.Prompt.Attack = false;
			if (Yandere.Armed)
			{
				studentScript.Prompt.HideButton[0] = true;
				studentScript.Prompt.MinimumDistance = 1f;
				studentScript.Prompt.Attack = true;
			}
			else
			{
				studentScript.Prompt.HideButton[2] = true;
				studentScript.Prompt.MinimumDistance = 2f;
				if (studentScript.WitnessedMurder || studentScript.WitnessedCorpse || studentScript.Private)
				{
					studentScript.Prompt.HideButton[0] = true;
				}
			}
			if (Yandere.Dragging || Yandere.PickUp != null || Yandere.Chased || Yandere.Chasers > 0)
			{
				studentScript.Prompt.HideButton[0] = true;
				studentScript.Prompt.HideButton[2] = true;
			}
			if (Yandere.NearBodies > 0 || Yandere.Sanity < 33.33333f)
			{
				studentScript.Prompt.HideButton[0] = true;
			}
			if (studentScript.Teacher)
			{
				studentScript.Prompt.HideButton[0] = true;
			}
		}
		if (Sans)
		{
			studentScript.Prompt.HideButton[0] = false;
			studentScript.Prompt.Label[0].text = "     Psychokinesis";
		}
		if (Pose)
		{
			studentScript.Prompt.HideButton[0] = false;
			studentScript.Prompt.Label[0].text = "     Pose";
		}
		if (NoSpeech)
		{
			studentScript.Prompt.HideButton[0] = true;
		}
	}

	public void AttendClass()
	{
		ConvoManager.Confirmed = false;
		SleuthPhase = 3;
		if (RingEvent.EventActive)
		{
			RingEvent.ReturnRing();
		}
		while (NPCsSpawned < NPCsTotal)
		{
			SpawnStudent(SpawnID);
			SpawnID++;
		}
		if (Clock.LateStudent)
		{
			Clock.ActivateLateStudent();
		}
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				if (studentScript.WitnessedBloodPool && !studentScript.WitnessedMurder && !studentScript.WitnessedCorpse)
				{
					studentScript.Fleeing = false;
					studentScript.Alarmed = false;
					studentScript.AlarmTimer = 0f;
					studentScript.ReportPhase = 0;
					studentScript.WitnessedBloodPool = false;
				}
				if (studentScript.Alive && !studentScript.Slave && !studentScript.Tranquil && !studentScript.Fleeing && studentScript.enabled && studentScript.gameObject.activeInHierarchy)
				{
					if (!studentScript.Started)
					{
						studentScript.Start();
					}
					if (!studentScript.Teacher)
					{
						if (!studentScript.Indoors)
						{
							if (studentScript.ShoeRemoval.Locker == null)
							{
								studentScript.ShoeRemoval.Start();
							}
							studentScript.ShoeRemoval.PutOnShoes();
						}
						studentScript.transform.position = studentScript.Seat.position + Vector3.up * 0.01f;
						studentScript.transform.rotation = studentScript.Seat.rotation;
						studentScript.Character.GetComponent<Animation>().Play(studentScript.SitAnim);
						studentScript.Pathfinding.canSearch = false;
						studentScript.Pathfinding.canMove = false;
						studentScript.Pathfinding.speed = 0f;
						studentScript.ClubActivityPhase = 0;
						studentScript.ClubTimer = 0f;
						studentScript.Pestered = 0;
						studentScript.Distracting = false;
						studentScript.Distracted = false;
						studentScript.Ignoring = false;
						studentScript.Pushable = false;
						studentScript.Vomiting = false;
						studentScript.Private = false;
						studentScript.Sedated = false;
						studentScript.Emetic = false;
						studentScript.Hurry = false;
						studentScript.Safe = false;
						studentScript.CanTalk = true;
						studentScript.Routine = true;
						if (studentScript.Wet)
						{
							CommunalLocker.Student = null;
							studentScript.Schoolwear = 3;
							studentScript.ChangeSchoolwear();
							studentScript.LiquidProjector.enabled = false;
							studentScript.Splashed = false;
							studentScript.Bloody = false;
							studentScript.BathePhase = 1;
							studentScript.Wet = false;
							studentScript.UnWet();
							if (studentScript.Rival && CommunalLocker.RivalPhone.Stolen)
							{
								studentScript.RealizePhoneIsMissing();
							}
						}
						if (studentScript.ClubAttire)
						{
							studentScript.ChangeSchoolwear();
							studentScript.ClubAttire = false;
						}
						if (studentScript.Schoolwear != 1)
						{
							studentScript.Schoolwear = 1;
							studentScript.ChangeSchoolwear();
						}
						if (studentScript.Meeting && Clock.HourTime > studentScript.MeetTime)
						{
							studentScript.Meeting = false;
						}
						if (studentScript.Club == ClubType.Sports)
						{
							studentScript.SetSplashes(false);
							studentScript.WalkAnim = studentScript.OriginalWalkAnim;
							studentScript.Character.transform.localPosition = new Vector3(0f, 0f, 0f);
							studentScript.Cosmetic.Goggles[studentScript.StudentID].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
							if (!studentScript.Cosmetic.Empty)
							{
								studentScript.Cosmetic.MaleHair[studentScript.Cosmetic.Hairstyle].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
							}
						}
						if (studentScript.MyPlate != null && studentScript.MyPlate.transform.parent == studentScript.RightHand)
						{
							studentScript.MyPlate.transform.parent = null;
							studentScript.MyPlate.transform.position = studentScript.OriginalPlatePosition;
							studentScript.MyPlate.transform.rotation = studentScript.OriginalPlateRotation;
							studentScript.IdleAnim = studentScript.OriginalIdleAnim;
							studentScript.WalkAnim = studentScript.OriginalWalkAnim;
						}
					}
					else if (ID != GymTeacherID && ID != NurseID)
					{
						studentScript.transform.position = Podiums.List[studentScript.Class].position + Vector3.up * 0.01f;
						studentScript.transform.rotation = Podiums.List[studentScript.Class].rotation;
					}
					else
					{
						studentScript.transform.position = studentScript.Seat.position + Vector3.up * 0.01f;
						studentScript.transform.rotation = studentScript.Seat.rotation;
					}
				}
			}
		}
		UpdateStudents();
		Physics.SyncTransforms();
	}

	public void SkipTo8()
	{
		while (NPCsSpawned < NPCsTotal)
		{
			SpawnStudent(SpawnID);
			SpawnID++;
		}
		int num = 0;
		int num2 = 0;
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && studentScript.Alive && !studentScript.Slave && !studentScript.Tranquil)
			{
				if (!studentScript.Started)
				{
					studentScript.Start();
				}
				bool flag = false;
				if (MemorialScene.enabled && studentScript.Teacher)
				{
					flag = true;
					studentScript.Teacher = false;
				}
				if (!studentScript.Teacher)
				{
					if (!studentScript.Indoors)
					{
						if (studentScript.ShoeRemoval.Locker == null)
						{
							studentScript.ShoeRemoval.Start();
						}
						studentScript.ShoeRemoval.PutOnShoes();
					}
					studentScript.transform.position = studentScript.Seat.position + Vector3.up * 0.01f;
					studentScript.transform.rotation = studentScript.Seat.rotation;
					studentScript.Pathfinding.canSearch = true;
					studentScript.Pathfinding.canMove = true;
					studentScript.Pathfinding.speed = 1f;
					studentScript.ClubActivityPhase = 0;
					studentScript.Distracted = false;
					studentScript.Spawned = true;
					studentScript.Routine = true;
					studentScript.Safe = false;
					studentScript.SprintAnim = studentScript.OriginalSprintAnim;
					if (studentScript.ClubAttire)
					{
						studentScript.ChangeSchoolwear();
						studentScript.ClubAttire = true;
					}
					studentScript.TeleportToDestination();
					studentScript.TeleportToDestination();
				}
				else
				{
					studentScript.TeleportToDestination();
					studentScript.TeleportToDestination();
				}
				if (MemorialScene.enabled)
				{
					if (flag)
					{
						studentScript.Teacher = true;
					}
					if (studentScript.Persona == PersonaType.PhoneAddict)
					{
						studentScript.SmartPhone.SetActive(true);
					}
					if (studentScript.Actions[studentScript.Phase] == StudentActionType.Graffiti && !Bully)
					{
						ScheduleBlock scheduleBlock = studentScript.ScheduleBlocks[2];
						scheduleBlock.destination = "Patrol";
						scheduleBlock.action = "Patrol";
						studentScript.GetDestinations();
					}
					studentScript.SpeechLines.Stop();
					studentScript.transform.position = new Vector3(20f + (float)num * 1.1f, 0f, 82 - num2 * 5);
					num2++;
					if (num2 > 4)
					{
						num++;
						num2 = 0;
					}
				}
			}
		}
	}

	public void ResumeMovement()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && !studentScript.Fleeing)
			{
				studentScript.Pathfinding.canSearch = true;
				studentScript.Pathfinding.canMove = true;
				studentScript.Pathfinding.speed = 1f;
				studentScript.Routine = true;
			}
		}
	}

	public void StopMoving()
	{
		CombatMinigame.enabled = false;
		Stop = true;
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				if (!studentScript.Dying && !studentScript.PinningDown && !studentScript.Spraying)
				{
					if (YandereDying && studentScript.Club != ClubType.Council)
					{
						studentScript.IdleAnim = studentScript.ScaredAnim;
					}
					if (Yandere.Attacking)
					{
						if (studentScript.MurderReaction == 0)
						{
							studentScript.Character.GetComponent<Animation>().CrossFade(studentScript.ScaredAnim);
						}
					}
					else if (ID > 1 && studentScript.CharacterAnimation != null)
					{
						studentScript.CharacterAnimation.CrossFade(studentScript.IdleAnim);
					}
					studentScript.Pathfinding.canSearch = false;
					studentScript.Pathfinding.canMove = false;
					studentScript.Pathfinding.speed = 0f;
					studentScript.Stop = true;
					if (studentScript.EventManager != null)
					{
						studentScript.EventManager.EndEvent();
					}
				}
				if (studentScript.Alive && studentScript.SawMask)
				{
					Police.MaskReported = true;
				}
				if (studentScript.Slave && Police.DayOver)
				{
					Debug.Log("A mind-broken slave committed suicide.");
					studentScript.Broken.Subtitle.text = string.Empty;
					studentScript.Broken.Done = true;
					Object.Destroy(studentScript.Broken);
					studentScript.BecomeRagdoll();
					studentScript.Slave = false;
					studentScript.Suicide = true;
					studentScript.DeathType = DeathType.Mystery;
					StudentGlobals.SetStudentSlave(studentScript.StudentID);
				}
			}
		}
	}

	public void ComeBack()
	{
		Stop = false;
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				if (!studentScript.Dying && !studentScript.Replaced && !StudentGlobals.GetStudentExpelled(ID))
				{
					studentScript.gameObject.SetActive(true);
					studentScript.Pathfinding.canSearch = true;
					studentScript.Pathfinding.canMove = true;
					studentScript.Pathfinding.speed = 1f;
					studentScript.Stop = false;
				}
				if (studentScript.Teacher)
				{
					studentScript.Alarmed = false;
					studentScript.Reacted = false;
					studentScript.Witness = false;
					studentScript.Routine = true;
					studentScript.AlarmTimer = 0f;
					studentScript.Concern = 0;
				}
				if (studentScript.Club == ClubType.Council)
				{
					studentScript.Teacher = false;
				}
			}
		}
	}

	public void StopFleeing()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && !studentScript.Teacher)
			{
				studentScript.Pathfinding.target = studentScript.Destinations[studentScript.Phase];
				studentScript.Pathfinding.speed = 1f;
				studentScript.WitnessedCorpse = false;
				studentScript.WitnessedMurder = false;
				studentScript.Alarmed = false;
				studentScript.Fleeing = false;
				studentScript.Reacted = false;
				studentScript.Witness = false;
				studentScript.Routine = true;
			}
		}
	}

	public void EnablePrompts()
	{
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.Prompt.enabled = true;
			}
		}
	}

	public void DisablePrompts()
	{
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.Prompt.Hide();
				studentScript.Prompt.enabled = false;
			}
		}
	}

	public void WipePendingRep()
	{
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.PendingRep = 0f;
			}
		}
	}

	public void AttackOnTitan()
	{
		AoT = true;
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && !studentScript.Teacher)
			{
				studentScript.AttackOnTitan();
			}
		}
	}

	public void Kong()
	{
		DK = true;
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.DK = true;
			}
		}
	}

	public void Spook()
	{
		Spooky = true;
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && !studentScript.Male)
			{
				studentScript.Spook();
			}
		}
	}

	public void BadTime()
	{
		Sans = true;
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.Prompt.HideButton[0] = false;
				studentScript.BadTime = true;
			}
		}
	}

	public void UpdateBooths()
	{
		for (ID = 0; ID < ChangingBooths.Length; ID++)
		{
			ChangingBoothScript changingBoothScript = ChangingBooths[ID];
			if (changingBoothScript != null)
			{
				changingBoothScript.CheckYandereClub();
			}
		}
	}

	public void UpdatePerception()
	{
		for (ID = 0; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.UpdatePerception();
			}
		}
	}

	public void StopHesitating()
	{
		for (ID = 0; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				if (studentScript.AlarmTimer > 0f)
				{
					studentScript.AlarmTimer = 1f;
				}
				studentScript.Hesitation = 0f;
			}
		}
	}

	public void Unstop()
	{
		for (ID = 0; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.Stop = false;
			}
		}
	}

	public void LowerCorpsePosition()
	{
		if (CorpseLocation.position.y < 4f)
		{
			CorpseLocation.position = new Vector3(CorpseLocation.position.x, 0f, CorpseLocation.position.z);
		}
		else if (CorpseLocation.position.y < 8f)
		{
			CorpseLocation.position = new Vector3(CorpseLocation.position.x, 4f, CorpseLocation.position.z);
		}
		else if (CorpseLocation.position.y < 12f)
		{
			CorpseLocation.position = new Vector3(CorpseLocation.position.x, 8f, CorpseLocation.position.z);
		}
		else
		{
			CorpseLocation.position = new Vector3(CorpseLocation.position.x, 12f, CorpseLocation.position.z);
		}
	}

	public void LowerBloodPosition()
	{
		if (BloodLocation.position.y < 4f)
		{
			BloodLocation.position = new Vector3(BloodLocation.position.x, 0f, BloodLocation.position.z);
		}
		else if (BloodLocation.position.y < 8f)
		{
			BloodLocation.position = new Vector3(BloodLocation.position.x, 4f, BloodLocation.position.z);
		}
		else if (BloodLocation.position.y < 12f)
		{
			BloodLocation.position = new Vector3(BloodLocation.position.x, 8f, BloodLocation.position.z);
		}
		else
		{
			BloodLocation.position = new Vector3(BloodLocation.position.x, 12f, BloodLocation.position.z);
		}
	}

	public void CensorStudents()
	{
		for (ID = 0; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && !studentScript.Male && studentScript.Club != ClubType.Teacher && studentScript.Club != ClubType.GymTeacher && studentScript.Club != ClubType.Nurse)
			{
				if (Censor)
				{
					studentScript.Cosmetic.CensorPanties();
				}
				else
				{
					studentScript.Cosmetic.RemoveCensor();
				}
			}
		}
	}

	private void OccupySeat()
	{
		int @class = JSON.Students[SpawnID].Class;
		int seat = JSON.Students[SpawnID].Seat;
		switch (@class)
		{
		case 11:
			SeatsTaken11[seat] = true;
			break;
		case 12:
			SeatsTaken12[seat] = true;
			break;
		case 21:
			SeatsTaken21[seat] = true;
			break;
		case 22:
			SeatsTaken22[seat] = true;
			break;
		case 31:
			SeatsTaken31[seat] = true;
			break;
		case 32:
			SeatsTaken32[seat] = true;
			break;
		}
	}

	private void FindUnoccupiedSeat()
	{
		SeatOccupied = false;
		if (Class == 1)
		{
			JSON.Students[SpawnID].Class = 11;
			ID = 1;
			while (ID < SeatsTaken11.Length && !SeatOccupied)
			{
				if (!SeatsTaken11[ID])
				{
					JSON.Students[SpawnID].Seat = ID;
					SeatsTaken11[ID] = true;
					SeatOccupied = true;
				}
				ID++;
				if (ID > 15)
				{
					Class++;
				}
			}
		}
		else if (Class == 2)
		{
			JSON.Students[SpawnID].Class = 12;
			ID = 1;
			while (ID < SeatsTaken12.Length && !SeatOccupied)
			{
				if (!SeatsTaken12[ID])
				{
					JSON.Students[SpawnID].Seat = ID;
					SeatsTaken12[ID] = true;
					SeatOccupied = true;
				}
				ID++;
				if (ID > 15)
				{
					Class++;
				}
			}
		}
		else if (Class == 3)
		{
			JSON.Students[SpawnID].Class = 21;
			ID = 1;
			while (ID < SeatsTaken21.Length && !SeatOccupied)
			{
				if (!SeatsTaken21[ID])
				{
					JSON.Students[SpawnID].Seat = ID;
					SeatsTaken21[ID] = true;
					SeatOccupied = true;
				}
				ID++;
				if (ID > 15)
				{
					Class++;
				}
			}
		}
		else if (Class == 4)
		{
			JSON.Students[SpawnID].Class = 22;
			ID = 1;
			while (ID < SeatsTaken22.Length && !SeatOccupied)
			{
				if (!SeatsTaken22[ID])
				{
					JSON.Students[SpawnID].Seat = ID;
					SeatsTaken22[ID] = true;
					SeatOccupied = true;
				}
				ID++;
				if (ID > 15)
				{
					Class++;
				}
			}
		}
		else if (Class == 5)
		{
			JSON.Students[SpawnID].Class = 31;
			ID = 1;
			while (ID < SeatsTaken31.Length && !SeatOccupied)
			{
				if (!SeatsTaken31[ID])
				{
					JSON.Students[SpawnID].Seat = ID;
					SeatsTaken31[ID] = true;
					SeatOccupied = true;
				}
				ID++;
				if (ID > 15)
				{
					Class++;
				}
			}
		}
		else if (Class == 6)
		{
			JSON.Students[SpawnID].Class = 32;
			ID = 1;
			while (ID < SeatsTaken32.Length && !SeatOccupied)
			{
				if (!SeatsTaken32[ID])
				{
					JSON.Students[SpawnID].Seat = ID;
					SeatsTaken32[ID] = true;
					SeatOccupied = true;
				}
				ID++;
				if (ID > 15)
				{
					Class++;
				}
			}
		}
		if (!SeatOccupied)
		{
			FindUnoccupiedSeat();
		}
	}

	public void PinDownCheck()
	{
		if (PinningDown || Witnesses <= 3)
		{
			return;
		}
		for (ID = 1; ID < WitnessList.Length; ID++)
		{
			StudentScript studentScript = WitnessList[ID];
			if (studentScript != null && (!studentScript.Alive || studentScript.Attacked || studentScript.Fleeing || studentScript.Dying))
			{
				studentScript = null;
				if (ID != WitnessList.Length - 1)
				{
					Shuffle(ID);
				}
				Witnesses--;
			}
		}
		if (Witnesses > 3)
		{
			PinningDown = true;
			PinPhase = 1;
		}
	}

	private void Shuffle(int Start)
	{
		for (int i = Start; i < WitnessList.Length - 1; i++)
		{
			WitnessList[i] = WitnessList[i + 1];
		}
	}

	public void RemovePapersFromDesks()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && studentScript.MyPaper != null)
			{
				studentScript.MyPaper.SetActive(false);
			}
		}
	}

	public void SetStudentsActive(bool active)
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.gameObject.SetActive(active);
			}
		}
	}

	public void AssignTeachers()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.MyTeacher = Teachers[JSON.Students[studentScript.StudentID].Class];
			}
		}
	}

	public void ToggleBookBags()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.BookBag.SetActive(!studentScript.BookBag.activeInHierarchy);
			}
		}
	}

	public void DetermineVictim()
	{
		Bully = false;
		for (ID = 2; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && (ID != 36 || TaskGlobals.GetTaskStatus(36) != 3) && !studentScript.Teacher && !studentScript.Slave && studentScript.Club != ClubType.Bully && studentScript.Club != ClubType.Council && studentScript.Club != ClubType.Photography && studentScript.Club != ClubType.Delinquent && (float)StudentGlobals.GetStudentReputation(ID) < LowestRep)
			{
				LowestRep = StudentGlobals.GetStudentReputation(ID);
				VictimID = ID;
				Bully = true;
			}
		}
		if (Bully)
		{
			Debug.Log("A student has been chosen to be bullied. It's Student #" + VictimID + ".");
			if (Students[VictimID].Seat.position.x > 0f)
			{
				BullyGroup.position = Students[VictimID].Seat.position + new Vector3(0.33333f, 0f, 0f);
			}
			else
			{
				BullyGroup.position = Students[VictimID].Seat.position - new Vector3(0.33333f, 0f, 0f);
				BullyGroup.eulerAngles = new Vector3(0f, 90f, 0f);
			}
			StudentScript studentScript2 = Students[VictimID];
			ScheduleBlock scheduleBlock = studentScript2.ScheduleBlocks[2];
			scheduleBlock.destination = "ShameSpot";
			scheduleBlock.action = "Shamed";
			ScheduleBlock scheduleBlock2 = studentScript2.ScheduleBlocks[4];
			scheduleBlock2.destination = "Seat";
			scheduleBlock2.action = "Sit";
			if (studentScript2.Male)
			{
				studentScript2.ChemistScanner.MyRenderer.materials[1].mainTexture = studentScript2.ChemistScanner.SadEyes;
				studentScript2.ChemistScanner.enabled = false;
			}
			studentScript2.IdleAnim = studentScript2.BulliedIdleAnim;
			studentScript2.WalkAnim = studentScript2.BulliedWalkAnim;
			studentScript2.Bullied = true;
			studentScript2.GetDestinations();
			studentScript2.CameraAnims = studentScript2.CowardAnims;
			studentScript2.BusyAtLunch = true;
			studentScript2.Shy = false;
		}
	}

	public void SecurityCameras()
	{
		Egg = true;
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null && studentScript.SecurityCamera != null && studentScript.Alive)
			{
				Debug.Log("Enabling security camera on this character's head.");
				studentScript.SecurityCamera.SetActive(true);
			}
		}
	}

	public void DisableEveryone()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.gameObject.SetActive(false);
			}
		}
	}

	public void DisableStudent(int DisableID)
	{
		StudentScript studentScript = Students[DisableID];
		if (studentScript != null)
		{
			if (studentScript.gameObject.activeInHierarchy)
			{
				studentScript.gameObject.SetActive(false);
				return;
			}
			studentScript.gameObject.SetActive(true);
			UpdateOneAnimLayer(DisableID);
			Students[DisableID].ReadPhase = 0;
		}
	}

	public void UpdateOneAnimLayer(int DisableID)
	{
		Students[DisableID].UpdateAnimLayers();
		Students[DisableID].ReadPhase = 0;
	}

	public void UpdateAllAnimLayers()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.UpdateAnimLayers();
				studentScript.ReadPhase = 0;
			}
		}
	}

	public void UpdateGrafitti()
	{
		for (ID = 1; ID < 6; ID++)
		{
			if (!NoBully[ID])
			{
				Graffiti[ID].SetActive(true);
			}
		}
	}

	public void UpdateAllBentos()
	{
		for (ID = 1; ID < Students.Length; ID++)
		{
			StudentScript studentScript = Students[ID];
			if (studentScript != null)
			{
				studentScript.Bento.GetComponent<GenericBentoScript>().Prompt.Yandere = Yandere;
				studentScript.Bento.GetComponent<GenericBentoScript>().UpdatePrompts();
			}
		}
	}

	public void UpdateSleuths()
	{
		SleuthPhase++;
		for (ID = 56; ID < 61; ID++)
		{
			if (Students[ID] != null && !Students[ID].Slave && !Students[ID].Following)
			{
				if (SleuthPhase < 3)
				{
					Students[ID].SleuthTarget = SleuthDestinations[ID - 55];
					Students[ID].Pathfinding.target = Students[ID].SleuthTarget;
					Students[ID].CurrentDestination = Students[ID].SleuthTarget;
				}
				else if (SleuthPhase == 3)
				{
					Students[ID].GetSleuthTarget();
				}
				else if (SleuthPhase == 4)
				{
					Students[ID].SleuthTarget = Clubs.List[ID];
					Students[ID].Pathfinding.target = Students[ID].SleuthTarget;
					Students[ID].CurrentDestination = Students[ID].SleuthTarget;
				}
				Students[ID].SmartPhone.SetActive(true);
				Students[ID].SpeechLines.Stop();
			}
		}
	}

	public void UpdateDrama()
	{
		if (MemorialScene.gameObject.activeInHierarchy)
		{
			return;
		}
		DramaPhase++;
		for (ID = 26; ID < 31; ID++)
		{
			if (Students[ID] != null)
			{
				if (DramaPhase == 1)
				{
					Clubs.List[ID].position = OriginalClubPositions[ID - 25];
					Clubs.List[ID].rotation = OriginalClubRotations[ID - 25];
					Students[ID].ClubAnim = Students[ID].OriginalClubAnim;
				}
				else if (DramaPhase == 2)
				{
					Clubs.List[ID].position = DramaSpots[ID - 25].position;
					Clubs.List[ID].rotation = DramaSpots[ID - 25].rotation;
					if (ID == 26)
					{
						Students[ID].ClubAnim = Students[ID].ActAnim;
					}
					else if (ID == 27)
					{
						Students[ID].ClubAnim = Students[ID].ThinkAnim;
					}
					else if (ID == 28)
					{
						Students[ID].ClubAnim = Students[ID].ThinkAnim;
					}
					else if (ID == 29)
					{
						Students[ID].ClubAnim = Students[ID].ActAnim;
					}
					else if (ID == 30)
					{
						Students[ID].ClubAnim = Students[ID].ThinkAnim;
					}
				}
				else if (DramaPhase == 3)
				{
					Clubs.List[ID].position = BackstageSpots[ID - 25].position;
					Clubs.List[ID].rotation = BackstageSpots[ID - 25].rotation;
				}
				else if (DramaPhase == 4)
				{
					DramaPhase = 1;
					UpdateDrama();
				}
				Students[ID].DistanceToDestination = 100f;
				Students[ID].SmartPhone.SetActive(false);
				Students[ID].SpeechLines.Stop();
			}
		}
	}

	public void UpdateMartialArts()
	{
		ConvoManager.Confirmed = false;
		MartialArtsPhase++;
		for (ID = 46; ID < 51; ID++)
		{
			if (Students[ID] != null)
			{
				if (MartialArtsPhase == 1)
				{
					Clubs.List[ID].position = MartialArtsSpots[ID - 45].position;
					Clubs.List[ID].rotation = MartialArtsSpots[ID - 45].rotation;
				}
				else if (MartialArtsPhase == 2)
				{
					Clubs.List[ID].position = MartialArtsSpots[ID - 40].position;
					Clubs.List[ID].rotation = MartialArtsSpots[ID - 40].rotation;
				}
				else if (MartialArtsPhase == 3)
				{
					Clubs.List[ID].position = MartialArtsSpots[ID - 35].position;
					Clubs.List[ID].rotation = MartialArtsSpots[ID - 35].rotation;
				}
				else if (MartialArtsPhase == 4)
				{
					MartialArtsPhase = 0;
					UpdateMartialArts();
				}
				Students[ID].DistanceToDestination = 100f;
				Students[ID].SmartPhone.SetActive(false);
				Students[ID].SpeechLines.Stop();
			}
		}
	}

	public void UpdateMeeting()
	{
		MeetingTimer += Time.deltaTime;
		if (MeetingTimer > 5f)
		{
			Speaker += 5;
			if (Speaker == 91)
			{
				Speaker = 21;
			}
			else if (Speaker == 76)
			{
				Speaker = 86;
			}
			else if (Speaker == 36)
			{
				Speaker = 41;
			}
			MeetingTimer = 0f;
		}
	}

	public void CheckMusic()
	{
		int num = 0;
		for (ID = 51; ID < 56; ID++)
		{
			if (Students[ID] != null && Students[ID].Routine && Students[ID].DistanceToDestination < 0.1f)
			{
				num++;
			}
		}
		if (num == 5)
		{
			PracticeVocals.pitch = Time.timeScale;
			PracticeMusic.pitch = Time.timeScale;
			if (!PracticeMusic.isPlaying)
			{
				PracticeVocals.Play();
				PracticeMusic.Play();
			}
		}
		else
		{
			PracticeVocals.Stop();
			PracticeMusic.Stop();
		}
	}

	public void UpdateAprons()
	{
		for (ID = 21; ID < 26; ID++)
		{
			if (Students[ID] != null && Students[ID].ClubMemberID > 0 && Students[ID].ApronAttacher != null && Students[ID].ApronAttacher.newRenderer != null)
			{
				Students[ID].ApronAttacher.newRenderer.material.mainTexture = Students[ID].Cosmetic.ApronTextures[Students[ID].ClubMemberID];
			}
		}
	}

	public void PreventAlarm()
	{
		for (ID = 1; ID < 101; ID++)
		{
			if (Students[ID] != null)
			{
				Students[ID].Alarm = 0f;
			}
		}
	}

	public void VolumeDown()
	{
		for (ID = 51; ID < 56; ID++)
		{
			if (Students[ID] != null && Students[ID].Instruments[Students[ID].ClubMemberID] != null)
			{
				Students[ID].Instruments[Students[ID].ClubMemberID].GetComponent<AudioSource>().volume = 0.2f;
			}
		}
	}

	public void VolumeUp()
	{
		for (ID = 51; ID < 56; ID++)
		{
			if (Students[ID] != null && Students[ID].Instruments[Students[ID].ClubMemberID] != null)
			{
				Students[ID].Instruments[Students[ID].ClubMemberID].GetComponent<AudioSource>().volume = 1f;
			}
		}
	}

	public void GetMaleVomitSpot(StudentScript VomitStudent)
	{
		if (VomitStudent.transform.position.y < 1f)
		{
			MaleVomitSpot = MaleVomitSpots[1];
			VomitStudent.VomitDoor = MaleToiletDoors[1];
		}
		else if (VomitStudent.transform.position.y < 5f)
		{
			MaleVomitSpot = MaleVomitSpots[2];
			VomitStudent.VomitDoor = MaleToiletDoors[2];
		}
		else
		{
			MaleVomitSpot = MaleVomitSpots[3];
			VomitStudent.VomitDoor = MaleToiletDoors[3];
		}
	}

	public void GetFemaleVomitSpot(StudentScript VomitStudent)
	{
		if (VomitStudent.transform.position.y < 1f)
		{
			FemaleVomitSpot = FemaleVomitSpots[1];
			VomitStudent.VomitDoor = FemaleToiletDoors[1];
		}
		else if (VomitStudent.transform.position.y < 5f)
		{
			FemaleVomitSpot = FemaleVomitSpots[2];
			VomitStudent.VomitDoor = FemaleToiletDoors[2];
		}
		else
		{
			FemaleVomitSpot = FemaleVomitSpots[3];
			VomitStudent.VomitDoor = FemaleToiletDoors[3];
		}
	}

	public void GetMaleWashSpot(StudentScript VomitStudent)
	{
		if (VomitStudent.transform.position.y < 1f)
		{
			MaleWashSpot = MaleWashSpots[1];
		}
		else if (VomitStudent.transform.position.y < 5f)
		{
			MaleWashSpot = MaleWashSpots[2];
		}
		else
		{
			MaleWashSpot = MaleWashSpots[3];
		}
	}

	public void GetFemaleWashSpot(StudentScript VomitStudent)
	{
		if (VomitStudent.transform.position.y < 1f)
		{
			FemaleWashSpot = FemaleWashSpots[1];
		}
		else if (VomitStudent.transform.position.y < 5f)
		{
			FemaleWashSpot = FemaleWashSpots[2];
		}
		else
		{
			FemaleWashSpot = FemaleWashSpots[3];
		}
	}

	public void GetNearestFountain(StudentScript Student)
	{
		DrinkingFountainScript drinkingFountainScript = DrinkingFountains[1];
		for (ID = 2; ID < 8; ID++)
		{
			if (Vector3.Distance(Student.transform.position, DrinkingFountains[ID].transform.position) < Vector3.Distance(Student.transform.position, drinkingFountainScript.transform.position) && !DrinkingFountains[ID].Occupied)
			{
				drinkingFountainScript = DrinkingFountains[ID];
			}
		}
		Student.DrinkingFountain = drinkingFountainScript;
		Student.DrinkingFountain.Occupied = true;
	}

	public void Save()
	{
		for (ID = 1; ID < 101; ID++)
		{
			if (Students[ID] != null)
			{
				Students[ID].SaveLoad.SaveData();
			}
		}
		int profile = GameGlobals.Profile;
		int @int = PlayerPrefs.GetInt("SaveSlot");
		DoorScript[] doors = Doors;
		foreach (DoorScript doorScript in doors)
		{
			if (doorScript != null)
			{
				if (doorScript.Open)
				{
					PlayerPrefs.SetInt("Profile_" + profile + "_Slot_" + @int + "_Door" + doorScript.DoorID + "_Open", 1);
				}
				else
				{
					PlayerPrefs.SetInt("Profile_" + profile + "_Slot_" + @int + "_Door" + doorScript.DoorID + "_Open", 0);
				}
			}
		}
	}

	public void Load()
	{
		for (ID = 1; ID < 101; ID++)
		{
			if (Students[ID] != null)
			{
				Students[ID].SaveLoad.LoadData();
			}
		}
		int profile = GameGlobals.Profile;
		int @int = PlayerPrefs.GetInt("SaveSlot");
		Yandere.transform.position = new Vector3(PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_YanderePosX"), PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_YanderePosY"), PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_YanderePosZ"));
		Yandere.transform.eulerAngles = new Vector3(PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_YandereRotX"), PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_YandereRotY"), PlayerPrefs.GetFloat("Profile_" + profile + "_Slot_" + @int + "_YandereRotZ"));
		Yandere.FixCamera();
		Physics.SyncTransforms();
		DoorScript[] doors = Doors;
		foreach (DoorScript doorScript in doors)
		{
			if (doorScript != null)
			{
				if (PlayerPrefs.GetInt("Profile_" + profile + "_Slot_" + @int + "_Door" + doorScript.DoorID + "_Open") == 1)
				{
					doorScript.Open = true;
					doorScript.OpenDoor();
				}
				else
				{
					doorScript.Open = false;
				}
			}
		}
	}

	public void UpdateBlood()
	{
		if (Police.BloodParent.childCount > 0)
		{
			ID = 0;
			foreach (Transform item in Police.BloodParent)
			{
				if (ID < 100)
				{
					Blood[ID] = item.gameObject.GetComponent<Collider>();
					ID++;
				}
			}
		}
		if (Police.BloodParent.childCount <= 0 && Police.LimbParent.childCount <= 0)
		{
			return;
		}
		ID = 0;
		foreach (Transform item2 in Police.LimbParent)
		{
			if (ID < 100)
			{
				Limbs[ID] = item2.gameObject.GetComponent<Collider>();
				ID++;
			}
		}
	}

	public void CanAnyoneSeeYandere()
	{
		YandereVisible = false;
		StudentScript[] students = Students;
		foreach (StudentScript studentScript in students)
		{
			if (studentScript != null && studentScript.CanSeeObject(studentScript.Yandere.gameObject, studentScript.Yandere.HeadPosition))
			{
				YandereVisible = true;
				break;
			}
		}
	}

	public void SetFaces(float alpha)
	{
		StudentScript[] students = Students;
		foreach (StudentScript studentScript in students)
		{
			if (studentScript != null && studentScript.StudentID > 1)
			{
				studentScript.MyRenderer.materials[0].color = new Color(1f - alpha, 1f - alpha, 1f - alpha, 1f);
				studentScript.MyRenderer.materials[1].color = new Color(1f - alpha, 1f - alpha, 1f - alpha, 1f);
				studentScript.MyRenderer.materials[2].color = new Color(1f - alpha, 1f - alpha, 1f - alpha, 1f);
				studentScript.Cosmetic.LeftEyeRenderer.material.color = new Color(1f - alpha, 1f - alpha, 1f - alpha, 1f);
				studentScript.Cosmetic.RightEyeRenderer.material.color = new Color(1f - alpha, 1f - alpha, 1f - alpha, 1f);
				studentScript.Cosmetic.HairRenderer.material.color = new Color(1f - alpha, 1f - alpha, 1f - alpha, 1f);
			}
		}
	}
}
