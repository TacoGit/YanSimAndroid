using System;
using System.Collections.Generic;
using FIMSpace.FLook;
using Pathfinding;
using UnityEngine;

// THIS IS NOT PURELY ALL HIS CODE, IM TRYING TO FIX EVERYTHING TO KEEP THE MEMORY STABLE *NOTE BY TANOS

public class StudentScript : MonoBehaviour
{
	public Quaternion targetRotation;

	public Quaternion OriginalRotation;

	public Quaternion OriginalPlateRotation;

	public SelectiveGrayscale ChaseSelectiveGrayscale;

	public DrinkingFountainScript DrinkingFountain;

	public DetectionMarkerScript DetectionMarker;

	public ChemistScannerScript ChemistScanner;

	public StudentManagerScript StudentManager;

	public CameraEffectsScript CameraEffects;

	public ChangingBoothScript ChangingBooth;

	public DialogueWheelScript DialogueWheel;

	public WitnessCameraScript WitnessCamera;

	public StudentScript DistractionTarget;

	public CookingEventScript CookingEvent;

	public EventManagerScript EventManager;

	public GradingPaperScript GradingPaper;

	public ClubManagerScript ClubManager;

	public LightSwitchScript LightSwitch;

	public MovingEventScript MovingEvent;

	public ShoeRemovalScript ShoeRemoval;

	public StruggleBarScript StruggleBar;

	public ToiletEventScript ToiletEvent;

	public WeaponScript WeaponToTakeAway;

	public DynamicGridObstacle Obstacle;

	public PhoneEventScript PhoneEvent;

	public PickpocketScript PickPocket;

	public ReputationScript Reputation;

	public StudentScript TargetStudent;

	public GenericBentoScript MyBento;

	public StudentScript FollowTarget;

	public CountdownScript Countdown;

	public Renderer SmartPhoneScreen;

	public StudentScript Distractor;

	public StudentScript HuntTarget;

	public StudentScript MyReporter;

	public StudentScript MyTeacher;

	public BoneSetsScript BoneSets;

	public CosmeticScript Cosmetic;

	public SaveLoadScript SaveLoad;

	public SubtitleScript Subtitle;

	public DynamicBone OsanaHairL;

	public DynamicBone OsanaHairR;

	public ARMiyukiScript Miyuki;

	public WeaponScript MyWeapon;

	public StudentScript Partner;

	public RagdollScript Ragdoll;

	public YandereScript Yandere;

	public Camera DramaticCamera;

	public RagdollScript Corpse;

	public DoorScript VomitDoor;

	public BrokenScript Broken;

	public PoliceScript Police;

	public PromptScript Prompt;

	public AIPath Pathfinding;

	public TalkingScript Talk;

	public ClockScript Clock;

	public RadioScript Radio;

	public JsonScript JSON;

	public SuckScript Suck;

	public Renderer Tears;

	public Rigidbody MyRigidbody;

	public Collider MyCollider;

	public CharacterController MyController;

	public Animation CharacterAnimation;

	public Projector LiquidProjector;

	public float VisionFOV;

	public float VisionDistance;

	public ParticleSystem DelinquentSpeechLines;

	public ParticleSystem PepperSprayEffect;

	public ParticleSystem DrowningSplashes;

	public ParticleSystem BloodFountain;

	public ParticleSystem VomitEmitter;

	public ParticleSystem SpeechLines;

	public ParticleSystem BullyDust;

	public ParticleSystem ChalkDust;

	public ParticleSystem Hearts;

	public Texture KokonaPhoneTexture;

	public Texture MidoriPhoneTexture;

	public Texture OsanaPhoneTexture;

	public Texture RedBookTexture;

	public Texture BloodTexture;

	public Texture WaterTexture;

	public Texture GasTexture;

	public SkinnedMeshRenderer MyRenderer;

	public Renderer BookRenderer;

	public Transform LastSuspiciousObject2;

	public Transform LastSuspiciousObject;

	public Transform CurrentDestination;

	public Transform TeacherTalkPoint;

	public Transform LeftMiddleFinger;

	public Transform WeaponBagParent;

	public Transform LeftItemParent;

	public Transform PetDestination;

	public Transform CleaningSpot;

	public Transform SleuthTarget;

	public Transform Distraction;

	public Transform StalkTarget;

	public Transform ItemParent;

	public Transform WitnessPOV;

	public Transform RightDrill;

	public Transform BloodPool;

	public Transform LeftDrill;

	public Transform LeftPinky;

	public Transform MapMarker;

	public Transform RightHand;

	public Transform LeftHand;

	public Transform MeetSpot;

	public Transform MyLocker;

	public Transform MyPlate;

	public Transform Spine;

	public Transform Eyes;

	public Transform Head;

	public Transform Hips;

	public Transform Neck;

	public Transform Seat;

	public ParticleSystem[] LiquidEmitters;

	public ParticleSystem[] SplashEmitters;

	public ParticleSystem[] FireEmitters;

	public ScheduleBlock[] ScheduleBlocks;

	public Transform[] Destinations;

	public Transform[] Skirt;

	public Transform[] Arm;

	public DynamicBone[] BlackHoleEffect;

	public OutlineScript[] Outlines;

	public GameObject[] InstrumentBag;

	public GameObject[] ScienceProps;

	public GameObject[] Instruments;

	public GameObject[] Chopsticks;

	public GameObject[] Drumsticks;

	public GameObject[] Fingerfood;

	public GameObject[] Bones;

	public string[] DelinquentAnims;

	public string[] AnimationNames;

	public string[] GossipAnims;

	public string[] SleuthAnims;

	public string[] CheerAnims;

	[SerializeField]
	private List<int> VisibleCorpses = new List<int>();

	[SerializeField]
	private int[] CorpseLayers = new int[2] { 11, 14 };

	[SerializeField]
	private LayerMask Mask;

	public StudentActionType CurrentAction;

	public StudentActionType[] Actions;

	public StudentActionType[] OriginalActions;

	public AudioClip MurderSuicideKiller;

	public AudioClip MurderSuicideVictim;

	public AudioClip MurderSuicideSounds;

	public AudioClip PoisonDeathClip;

	public AudioClip PepperSpraySFX;

	public AudioClip BurningClip;

	public AudioSource AirGuitar;

	public AudioClip[] FemaleAttacks;

	public AudioClip[] BullyGiggles;

	public AudioClip[] BullyLaughs;

	public AudioClip[] MaleAttacks;

	public SphereCollider HipCollider;

	public Collider RightHandCollider;

	public Collider LeftHandCollider;

	public Collider NotFaceCollider;

	public Collider PantyCollider;

	public Collider SkirtCollider;

	public Collider FaceCollider;

	public Collider NEStairs;

	public Collider NWStairs;

	public Collider SEStairs;

	public Collider SWStairs;

	public GameObject BloodSprayCollider;

	public GameObject BullyPhotoCollider;

	public GameObject SquishyBloodEffect;

	public GameObject WhiteQuestionMark;

	public GameObject MiyukiGameScreen;

	public GameObject EmptyGameObject;

	public GameObject StabBloodEffect;

	public GameObject BigWaterSplash;

	public GameObject SecurityCamera;

	public GameObject RightEmptyEye;

	public GameObject Handkerchief;

	public GameObject LeftEmptyEye;

	public GameObject AnimatedBook;

	public GameObject BloodyScream;

	public GameObject BloodEffect;

	public GameObject CameraFlash;

	public GameObject ChaseCamera;

	public GameObject DeathScream;

	public GameObject PepperSpray;

	public GameObject PinkSeifuku;

	public GameObject WateringCan;

	public GameObject BagOfChips;

	public GameObject BloodSpray;

	public GameObject Sketchbook;

	public GameObject SmartPhone;

	public GameObject OccultBook;

	public GameObject Paintbrush;

	public GameObject AlarmDisc;

	public GameObject Character;

	public GameObject Cigarette;

	public GameObject EventBook;

	public GameObject Handcuffs;

	public GameObject HealthBar;

	public GameObject OsanaHair;

	public GameObject WeaponBag;

	public GameObject CandyBar;

	public GameObject Earpiece;

	public GameObject Scrubber;

	public GameObject Armband;

	public GameObject BookBag;

	public GameObject Lighter;

	public GameObject MyPaper;

	public GameObject Octodog;

	public GameObject Palette;

	public GameObject Eraser;

	public GameObject Giggle;

	public GameObject Marker;

	public GameObject Pencil;

	public GameObject Weapon;

	public GameObject Bento;

	public GameObject Paper;

	public GameObject Note;

	public GameObject Pen;

	public GameObject Lid;

	public bool InvestigatingPossibleDeath;

	public bool InvestigatingPossibleLimb;

	public bool WitnessedMindBrokenMurder;

	public bool ReturningMisplacedWeapon;

	public bool TargetedForDistraction;

	public bool SchoolwearUnavailable;

	public bool WitnessedBloodyWeapon;

	public bool MustChangeClothing;

	public bool WitnessedBloodPool;

	public bool WitnessedSomething;

	public bool FoundFriendCorpse;

	public bool OriginallyTeacher;

	public bool DramaticReaction;

	public bool EventInterrupted;

	public bool FoundEnemyCorpse;

	public bool LostTeacherTrust;

	public bool WitnessedCoverUp;

	public bool WitnessedCorpse;

	public bool WitnessedMurder;

	public bool WitnessedWeapon;

	public bool YandereInnocent;

	public bool GetNewAnimation = true;

	public bool FocusOnYandere;

	public bool PinDownWitness;

	public bool RepeatReaction;

	public bool StalkerFleeing;

	public bool YandereVisible;

	public bool CrimeReported;

	public bool FleeWhenClean;

	public bool MurderSuicide;

	public bool PhotoEvidence;

	public bool WitnessedLimb;

	public bool BeenSplashed;

	public bool BoobsResized;

	public bool CheckingNote;

	public bool ClubActivity;

	public bool Complimented;

	public bool Electrocuted;

	public bool FragileSlave;

	public bool HoldingHands;

	public bool PlayingAudio;

	public bool StopRotating;

	public bool SawFriendDie;

	public bool SentToLocker;

	public bool TurnOffRadio;

	public bool BusyAtLunch;

	public bool Electrified;

	public bool IgnoreBlood;

	public bool MusumeRight;

	public bool UpdateSkirt;

	public bool ClubAttire;

	public bool Confessing;

	public bool Distracted;

	public bool KilledMood;

	public bool LewdPhotos;

	public bool InDarkness;

	public bool SwitchBack;

	public bool Threatened;

	public bool BatheFast;

	public bool Counselor;

	public bool Depressed;

	public bool DiscCheck;

	public bool DressCode;

	public bool Drownable;

	public bool EndSearch;

	public bool KnifeDown;

	public bool LongSkirt;

	public bool NoBreakUp;

	public bool Phoneless;

	public bool TrueAlone;

	public bool WillChase;

	public bool Attacked;

	public bool Headache;

	public bool Gossiped;

	public bool Pushable;

	public bool Replaced;

	public bool SentHome;

	public bool Splashed;

	public bool Tranquil;

	public bool WalkBack;

	public bool Alarmed;

	public bool BadTime;

	public bool Bullied;

	public bool Drowned;

	public bool Forgave;

	public bool Indoors;

	public bool InEvent;

	public bool Injured;

	public bool Nemesis;

	public bool Private;

	public bool Reacted;

	public bool SawMask;

	public bool Sedated;

	public bool Spawned;

	public bool Started;

	public bool Suicide;

	public bool Teacher;

	public bool Tripped;

	public bool Witness;

	public bool Bloody;

	public bool CanTalk = true;

	public bool Emetic;

	public bool Lethal;

	public bool Routine = true;

	public bool GoAway;

	public bool Grudge;

	public bool NoTalk;

	public bool Paired;

	public bool Pushed;

	public bool Warned;

	public bool Alone;

	public bool Blind;

	public bool Eaten;

	public bool Hurry;

	public bool Rival;

	public bool Slave;

	public bool Halt;

	public bool Lost;

	public bool Male;

	public bool Rose;

	public bool Safe;

	public bool Stop;

	public bool AoT;

	public bool Fed;

	public bool Gas;

	public bool Shy;

	public bool Wet;

	public bool Won;

	public bool DK;

	public bool InvestigatingBloodPool;

	public bool RetreivingMedicine;

	public bool ResumeDistracting;

	public bool BreakingUpFight;

	public bool SeekingMedicine;

	public bool ReportingMurder;

	public bool CameraReacting;

	public bool UsingRigidbody;

	public bool ReportingBlood;

	public bool Investigating;

	public bool Distracting;

	public bool EatingSnack;

	public bool HitReacting;

	public bool PinningDown;

	public bool Struggling;

	public bool Following;

	public bool Sleuthing;

	public bool Fighting;

	public bool Guarding;

	public bool Ignoring;

	public bool Spraying;

	public bool Tripping;

	public bool Vomiting;

	public bool Burning;

	public bool Chasing;

	public bool Fleeing;

	public bool Hunting;

	public bool Leaving;

	public bool Meeting;

	public bool Shoving;

	public bool Talking;

	public bool Waiting;

	public bool Posing;

	public bool Dodging;

	public bool Dying;

	public float DistanceToDestination;

	public float FollowTargetDistance;

	public float DistanceToPlayer;

	public float TargetDistance;

	public float ThreatDistance;

	public float WitnessCooldownTimer;

	public float InvestigationTimer;

	public float CameraPoseTimer;

	public float DistractTimer;

	public float DramaticTimer;

	public float MedicineTimer;

	public float ReactionTimer;

	public float WalkBackTimer;

	public float ElectroTimer;

	public float ThreatTimer;

	public float GiggleTimer;

	public float GoAwayTimer;

	public float IgnoreTimer;

	public float LyricsTimer;

	public float MiyukiTimer;

	public float MusumeTimer;

	public float PatrolTimer;

	public float ReportTimer;

	public float SplashTimer;

	public float UpdateTimer;

	public float AlarmTimer;

	public float BatheTimer;

	public float CheerTimer;

	public float CleanTimer;

	public float LaughTimer;

	public float RadioTimer;

	public float SnackTimer;

	public float SprayTimer;

	public float StuckTimer;

	public float ClubTimer;

	public float MeetTimer;

	public float SulkTimer;

	public float TalkTimer;

	public float WaitTimer;

	public float SewTimer;

	public float OriginalYPosition;

	public float PreviousEyeShrink;

	public float PhotoPatience;

	public float PreviousAlarm;

	public float ClubThreshold = 6f;

	public float RepDeduction;

	public float RepRecovery;

	public float BreastSize;

	public float DodgeSpeed = 2f;

	public float Hesitation;

	public float PendingRep;

	public float Perception = 1f;

	public float EyeShrink;

	public float MeetTime;

	public float Paranoia;

	public float RepLoss;

	public float Health = 100f;

	public float Alarm;

	public int ReturningMisplacedWeaponPhase;

	public int RetrieveMedicinePhase;

	public int ChangeClothingPhase;

	public int InvestigationPhase;

	public int MurderSuicidePhase;

	public int ClubActivityPhase;

	public int SeekMedicinePhase;

	public int CameraReactPhase;

	public int DramaticPhase;

	public int GraffitiPhase;

	public int SentHomePhase;

	public int SunbathePhase;

	public int SciencePhase;

	public int LyricsPhase;

	public int SplashPhase;

	public int ThreatPhase = 1;

	public int BathePhase;

	public int BullyPhase;

	public int SnackPhase;

	public int VomitPhase;

	public int ClubPhase;

	public int SulkPhase;

	public int TaskPhase;

	public int ReadPhase;

	public int PinPhase;

	public int Phase;

	public PersonaType OriginalPersona;

	public StudentInteractionType Interaction;

	public int MurdersWitnessed;

	public int WeaponWitnessed;

	public int MurderReaction;

	public int GossipBonus;

	public int CleaningRole;

	public int TimesAnnoyed;

	public int DeathCause;

	public int Schoolwear;

	public int SkinColor = 3;

	public int Attempts;

	public int Patience = 5;

	public int Pestered;

	public int RepBonus;

	public int Strength;

	public int Concern;

	public int Defeats;

	public int Crush;

	public StudentWitnessType PreviouslyWitnessed;

	public StudentWitnessType Witnessed;

	public GameOverType GameOverCause;

	public DeathType DeathType;

	public string CurrentAnim = string.Empty;

	public string RivalPrefix = string.Empty;

	public string RandomAnim = string.Empty;

	public string Accessory = string.Empty;

	public string Hairstyle = string.Empty;

	public string Suffix = string.Empty;

	public string Name = string.Empty;

	public string OriginalIdleAnim = string.Empty;

	public string OriginalWalkAnim = string.Empty;

	public string OriginalSprintAnim = string.Empty;

	public string OriginalLeanAnim = string.Empty;

	public string WalkAnim = string.Empty;

	public string RunAnim = string.Empty;

	public string SprintAnim = string.Empty;

	public string IdleAnim = string.Empty;

	public string Nod1Anim = string.Empty;

	public string Nod2Anim = string.Empty;

	public string DefendAnim = string.Empty;

	public string DeathAnim = string.Empty;

	public string ScaredAnim = string.Empty;

	public string EvilWitnessAnim = string.Empty;

	public string LookDownAnim = string.Empty;

	public string PhoneAnim = string.Empty;

	public string AngryFaceAnim = string.Empty;

	public string ToughFaceAnim = string.Empty;

	public string InspectAnim = string.Empty;

	public string GuardAnim = string.Empty;

	public string CallAnim = string.Empty;

	public string CounterAnim = string.Empty;

	public string PushedAnim = string.Empty;

	public string GameAnim = string.Empty;

	public string BentoAnim = string.Empty;

	public string EatAnim = string.Empty;

	public string DrownAnim = string.Empty;

	public string WetAnim = string.Empty;

	public string SplashedAnim = string.Empty;

	public string StripAnim = string.Empty;

	public string ParanoidAnim = string.Empty;

	public string GossipAnim = string.Empty;

	public string SadSitAnim = string.Empty;

	public string BrokenAnim = string.Empty;

	public string BrokenSitAnim = string.Empty;

	public string BrokenWalkAnim = string.Empty;

	public string FistAnim = string.Empty;

	public string AttackAnim = string.Empty;

	public string SuicideAnim = string.Empty;

	public string RelaxAnim = string.Empty;

	public string SitAnim = string.Empty;

	public string ShyAnim = string.Empty;

	public string PeekAnim = string.Empty;

	public string ClubAnim = string.Empty;

	public string StruggleAnim = string.Empty;

	public string StruggleWonAnim = string.Empty;

	public string StruggleLostAnim = string.Empty;

	public string SocialSitAnim = string.Empty;

	public string CarryAnim = string.Empty;

	public string ActivityAnim = string.Empty;

	public string GrudgeAnim = string.Empty;

	public string SadFaceAnim = string.Empty;

	public string CowardAnim = string.Empty;

	public string EvilAnim = string.Empty;

	public string SocialReportAnim = string.Empty;

	public string SocialFearAnim = string.Empty;

	public string SocialTerrorAnim = string.Empty;

	public string BuzzSawDeathAnim = string.Empty;

	public string SwingDeathAnim = string.Empty;

	public string CyborgDeathAnim = string.Empty;

	public string WalkBackAnim = string.Empty;

	public string PatrolAnim = string.Empty;

	public string RadioAnim = string.Empty;

	public string BookSitAnim = string.Empty;

	public string BookReadAnim = string.Empty;

	public string LovedOneAnim = string.Empty;

	public string CuddleAnim = string.Empty;

	public string VomitAnim = string.Empty;

	public string WashFaceAnim = string.Empty;

	public string EmeticAnim = string.Empty;

	public string BurningAnim = string.Empty;

	public string JojoReactAnim = string.Empty;

	public string TeachAnim = string.Empty;

	public string LeanAnim = string.Empty;

	public string DeskTextAnim = string.Empty;

	public string CarryShoulderAnim = string.Empty;

	public string ReadyToFightAnim = string.Empty;

	public string SearchPatrolAnim = string.Empty;

	public string DiscoverPhoneAnim = string.Empty;

	public string WaitAnim = string.Empty;

	public string ShoveAnim = string.Empty;

	public string SprayAnim = string.Empty;

	public string SithReactAnim = string.Empty;

	public string EatVictimAnim = string.Empty;

	public string RandomGossipAnim = string.Empty;

	public string CuteAnim = string.Empty;

	public string BulliedIdleAnim = string.Empty;

	public string BulliedWalkAnim = string.Empty;

	public string BullyVictimAnim = string.Empty;

	public string SadDeskSitAnim = string.Empty;

	public string ConfusedSitAnim = string.Empty;

	public string SentHomeAnim = string.Empty;

	public string RandomCheerAnim = string.Empty;

	public string ParanoidWalkAnim = string.Empty;

	public string SleuthIdleAnim = string.Empty;

	public string SleuthWalkAnim = string.Empty;

	public string SleuthCalmAnim = string.Empty;

	public string SleuthScanAnim = string.Empty;

	public string SleuthReactAnim = string.Empty;

	public string SleuthSprintAnim = string.Empty;

	public string SleuthReportAnim = string.Empty;

	public string RandomSleuthAnim = string.Empty;

	public string BreakUpAnim = string.Empty;

	public string PaintAnim = string.Empty;

	public string SketchAnim = string.Empty;

	public string RummageAnim = string.Empty;

	public string ThinkAnim = string.Empty;

	public string ActAnim = string.Empty;

	public string OriginalClubAnim = string.Empty;

	public string MiyukiAnim = string.Empty;

	public string VictoryAnim = string.Empty;

	public string PlateIdleAnim = string.Empty;

	public string PlateWalkAnim = string.Empty;

	public string PlateEatAnim = string.Empty;

	public string PrepareFoodAnim = string.Empty;

	public string PoisonDeathAnim = string.Empty;

	public string HeadacheAnim = string.Empty;

	public string HeadacheSitAnim = string.Empty;

	public string ElectroAnim = string.Empty;

	public string EatChipsAnim = string.Empty;

	public string DrinkFountainAnim = string.Empty;

	public string PullBoxCutterAnim = string.Empty;

	public string TossNoteAnim = string.Empty;

	public string KeepNoteAnim = string.Empty;

	public string BathingAnim = string.Empty;

	public string DodgeAnim = string.Empty;

	public string InspectBloodAnim = string.Empty;

	public string PickUpAnim = string.Empty;

	public string[] CleanAnims;

	public string[] CameraAnims;

	public string[] SocialAnims;

	public string[] CowardAnims;

	public string[] EvilAnims;

	public string[] HeroAnims;

	public string[] TaskAnims;

	public string[] PhoneAnims;

	public int ClubMemberID;

	public int ConfessPhase = 1;

	public int ReportPhase;

	public int RadioPhase = 1;

	public int StudentID;

	public int PatrolID;

	public int SleuthID;

	public int BullyID;

	public int CleanID;

	public int Class;

	public int ID;

	public PersonaType Persona;

	public ClubType OriginalClub;

	public ClubType Club;

	public Vector3 OriginalPlatePosition;

	public Vector3 OriginalPosition;

	public Vector3 LastKnownCorpse;

	public Vector3 DistractionSpot;

	public Vector3 LastKnownBlood;

	public Vector3 RightEyeOrigin;

	public Vector3 LeftEyeOrigin;

	public Vector3 PreviousSkirt;

	public Vector3 LastPosition;

	public Vector3 BurnTarget;

	public Transform RightBreast;

	public Transform LeftBreast;

	public Transform RightEye;

	public Transform LeftEye;

	public int Frame;

	private float MaxSpeed = 10f;

	private const string RIVAL_PREFIX = "Rival ";

	public Vector3[] SkirtPositions;

	public Vector3[] SkirtRotations;

	public Vector3[] SkirtOrigins;

	public Transform DefaultTarget;

	public Transform GushTarget;

	public bool Gush;

	public float LookSpeed = 2f;

	public float TimeOfDeath;

	public int Fate;

	public LowPolyStudentScript LowPoly;

	public GameObject JojoHitEffect;

	public GameObject[] ElectroSteam;

	public GameObject[] CensorSteam;

	public Texture NudeTexture;

	public Mesh BaldNudeMesh;

	public Mesh NudeMesh;

	public Texture TowelTexture;

	public Mesh TowelMesh;

	public Mesh SwimmingTrunks;

	public Mesh SchoolSwimsuit;

	public Mesh GymUniform;

	public Texture UniformTexture;

	public Texture SwimsuitTexture;

	public Texture GyaruSwimsuitTexture;

	public Texture GymTexture;

	public Texture TitanBodyTexture;

	public Texture TitanFaceTexture;

	public bool Spooky;

	public Mesh JudoGiMesh;

	public Texture JudoGiTexture;

	public RiggedAccessoryAttacher Attacher;

	public Mesh NoArmsNoTorso;

	public GameObject RiggedAccessory;

	public int CoupleID;

	public float ChameleonBonus;

	public bool Chameleon;

	public RiggedAccessoryAttacher LabcoatAttacher;

	public RiggedAccessoryAttacher ApronAttacher;

	public Mesh HeadAndHands;

	public bool Alive
	{
		get
		{
			return DeathType == DeathType.None;
		}
	}

	private SubtitleType LostPhoneSubtitleType
	{
		get
		{
			if (RivalPrefix == string.Empty)
			{
				return SubtitleType.LostPhone;
			}
			if (RivalPrefix == "Rival ")
			{
				return SubtitleType.RivalLostPhone;
			}
			throw new NotImplementedException("\"" + RivalPrefix + "\" case not implemented.");
		}
	}

	private SubtitleType PickpocketSubtitleType
	{
		get
		{
			if (RivalPrefix == string.Empty)
			{
				return SubtitleType.PickpocketReaction;
			}
			if (RivalPrefix == "Rival ")
			{
				return SubtitleType.RivalPickpocketReaction;
			}
			throw new NotImplementedException("\"" + RivalPrefix + "\" case not implemented.");
		}
	}

	private SubtitleType SplashSubtitleType
	{
		get
		{
			if (RivalPrefix == string.Empty)
			{
				if (!Male)
				{
					return SubtitleType.SplashReaction;
				}
				return SubtitleType.SplashReactionMale;
			}
			if (RivalPrefix == "Rival ")
			{
				return SubtitleType.RivalSplashReaction;
			}
			throw new NotImplementedException("\"" + RivalPrefix + "\" case not implemented.");
		}
	}

	public SubtitleType TaskLineResponseType
	{
		get
		{
			if (StudentID == 6)
			{
				return SubtitleType.Task6Line;
			}
			if (StudentID == 7)
			{
				return SubtitleType.Task7Line;
			}
			if (StudentID == 8)
			{
				return SubtitleType.Task8Line;
			}
			if (StudentID == 11)
			{
				return SubtitleType.Task11Line;
			}
			if (StudentID == 13)
			{
				return SubtitleType.Task13Line;
			}
			if (StudentID == 14)
			{
				return SubtitleType.Task14Line;
			}
			if (StudentID == 15)
			{
				return SubtitleType.Task15Line;
			}
			if (StudentID == 25)
			{
				return SubtitleType.Task25Line;
			}
			if (StudentID == 28)
			{
				return SubtitleType.Task28Line;
			}
			if (StudentID == 30)
			{
				return SubtitleType.Task30Line;
			}
			if (StudentID == 33)
			{
				return SubtitleType.Task33Line;
			}
			if (StudentID == 34)
			{
				return SubtitleType.Task34Line;
			}
			if (StudentID == 36)
			{
				return SubtitleType.Task36Line;
			}
			if (StudentID == 37)
			{
				return SubtitleType.Task37Line;
			}
			if (StudentID == 38)
			{
				return SubtitleType.Task38Line;
			}
			if (StudentID == 52)
			{
				return SubtitleType.Task52Line;
			}
			if (StudentID == 81)
			{
				return SubtitleType.Task81Line;
			}
			throw new NotImplementedException("\"" + StudentID + "\" case not implemented.");
		}
	}

	public SubtitleType ClubInfoResponseType
	{
		get
		{
			if (GameGlobals.EmptyDemon)
				return SubtitleType.ClubPlaceholderInfo;

			switch (Club)
			{
				case ClubType.Cooking:
					return SubtitleType.ClubCookingInfo;
				case ClubType.Drama:
					return SubtitleType.ClubDramaInfo;
				case ClubType.Occult:
					return SubtitleType.ClubOccultInfo;
				case ClubType.Art:
					return SubtitleType.ClubArtInfo;
				case ClubType.LightMusic:
					return SubtitleType.ClubLightMusicInfo;
				case ClubType.MartialArts:
					return SubtitleType.ClubMartialArtsInfo;
				case ClubType.Photography:
					return Sleuthing ? SubtitleType.ClubPhotoInfoDark : SubtitleType.ClubPhotoInfoLight;
				case ClubType.Science:
					return SubtitleType.ClubScienceInfo;
				case ClubType.Sports:
					return SubtitleType.ClubSportsInfo;
				case ClubType.Gardening:
					return SubtitleType.ClubGardenInfo;
				case ClubType.Gaming:
					return SubtitleType.ClubGamingInfo;
				default:
					return SubtitleType.ClubPlaceholderInfo;
			}
			return SubtitleType.ClubPlaceholderInfo;
		}
	}

	public bool InCouple
	{
		get
		{
			return CoupleID > 0;
		}
	}

	public void Start()
	{
		if (!Started)
		{
			CharacterAnimation = Character.GetComponent<Animation>();
			MyBento = Bento.GetComponent<GenericBentoScript>();
			Pathfinding.repathRate += (float)StudentID * 0.01f;
			OriginalIdleAnim = IdleAnim;
			OriginalLeanAnim = LeanAnim;
			if (!StudentManager.LoveSick && SchoolAtmosphere.Type == SchoolAtmosphereType.Low && Club <= ClubType.Gaming)
			{
				IdleAnim = ParanoidAnim;
			}
			if (ClubGlobals.Club == ClubType.Occult)
			{
				Perception = 0.5f;
			}
			ParticleSystem.EmissionModule emission = Hearts.emission;
			emission.enabled = false;
			Prompt.OwnerType = PromptOwnerType.Person;
			Paranoia = 2f - SchoolGlobals.SchoolAtmosphere;
			VisionDistance = ((PlayerGlobals.PantiesEquipped != 4) ? 10f : 5f) * Paranoia;
			if (GameObject.Find("DetectionCamera") != null)
			{
				SpawnDetectionMarker();
			}
			StudentJson studentJson = JSON.Students[StudentID];
			ScheduleBlocks = studentJson.ScheduleBlocks;
			Persona = studentJson.Persona;
			Class = studentJson.Class;
			Crush = studentJson.Crush;
			Club = studentJson.Club;
			BreastSize = studentJson.BreastSize;
			Strength = studentJson.Strength;
			Hairstyle = studentJson.Hairstyle;
			Accessory = studentJson.Accessory;
			Name = studentJson.Name;
			OriginalClub = Club;
			if (StudentGlobals.GetStudentBroken(StudentID))
			{
				Cosmetic.RightEyeRenderer.gameObject.SetActive(false);
				Cosmetic.LeftEyeRenderer.gameObject.SetActive(false);
				Cosmetic.RightIrisLight.SetActive(false);
				Cosmetic.LeftIrisLight.SetActive(false);
				RightEmptyEye.SetActive(true);
				LeftEmptyEye.SetActive(true);
				Shy = true;
				Persona = PersonaType.Coward;
			}
			if (Name == "Random")
			{
				Persona = (PersonaType)UnityEngine.Random.Range(1, 8);
				if (Persona == PersonaType.Lovestruck)
				{
					Persona = PersonaType.PhoneAddict;
				}
				studentJson.Persona = Persona;
				if (Persona == PersonaType.Heroic)
				{
					Strength = UnityEngine.Random.Range(1, 5);
					studentJson.Strength = Strength;
				}
			}
			Seat = StudentManager.Seats[Class].List[studentJson.Seat];
			base.gameObject.name = "Student_" + StudentID + " (" + Name + ")";
			OriginalPersona = Persona;
			if (StudentID == 81 && StudentGlobals.GetStudentBroken(81))
			{
				Persona = PersonaType.Coward;
			}

			switch (Persona)
			{
				case PersonaType.Loner:
				case PersonaType.Coward:
				case PersonaType.Fragile:
					CameraAnims = CowardAnims;
					break;
				case PersonaType.TeachersPet:
				case PersonaType.Heroic:
				case PersonaType.Strict:
					CameraAnims = HeroAnims;
					break;
				case PersonaType.Evil:
				case PersonaType.Spiteful:
				case PersonaType.Violent:
					CameraAnims = EvilAnims;
					break;
				case PersonaType.SocialButterfly:
				case PersonaType.Lovestruck:
				case PersonaType.PhoneAddict:
				case PersonaType.Sleuth:
					CameraAnims = SocialAnims;
					break;
			}

			if (ClubGlobals.GetClubClosed(Club))
			{
				Club = ClubType.None;
			}
			DialogueWheel = GameObject.Find("DialogueWheel").GetComponent<DialogueWheelScript>();
			ClubManager = GameObject.Find("ClubManager").GetComponent<ClubManagerScript>();
			Reputation = GameObject.Find("Reputation").GetComponent<ReputationScript>();
			Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
			Police = GameObject.Find("Police").GetComponent<PoliceScript>();
			Clock = GameObject.Find("Clock").GetComponent<ClockScript>();
			Subtitle = Yandere.Subtitle;
			CameraEffects = Yandere.MainCamera.GetComponent<CameraEffectsScript>();
			RightEyeOrigin = RightEye.localPosition;
			LeftEyeOrigin = LeftEye.localPosition;
			Bento.GetComponent<GenericBentoScript>().StudentID = StudentID;
			HealthBar.transform.parent.gameObject.SetActive(false);
			VomitEmitter.gameObject.SetActive(false);
			ChaseCamera.gameObject.SetActive(false);
			Countdown.gameObject.SetActive(false);
			MiyukiGameScreen.SetActive(false);
			Chopsticks[0].SetActive(false);
			Chopsticks[1].SetActive(false);
			Sketchbook.SetActive(false);
			SmartPhone.SetActive(false);
			OccultBook.SetActive(false);
			Paintbrush.SetActive(false);
			EventBook.SetActive(false);
			Handcuffs.SetActive(false);
			Scrubber.SetActive(false);
			Octodog.SetActive(false);
			Palette.SetActive(false);
			Eraser.SetActive(false);
			Pencil.SetActive(false);
			Bento.SetActive(false);
			Pen.SetActive(false);
			SpeechLines.Stop();
			GameObject[] scienceProps = ScienceProps;
			foreach (GameObject gameObject in scienceProps)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			GameObject[] fingerfood = Fingerfood;
			foreach (GameObject gameObject2 in fingerfood)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
			}
			string walkAnim = WalkAnim;
			OriginalSprintAnim = SprintAnim;
			OriginalWalkAnim = WalkAnim;
			if (Persona == PersonaType.Evil)
			{
				ScaredAnim = EvilWitnessAnim;
			}
			if (Persona == PersonaType.PhoneAddict)
			{
				SmartPhone.transform.localPosition = new Vector3(0.01f, 0.005f, 0.01f);
				SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, 165f);
				Countdown.Speed = 0.1f;
				SprintAnim = PhoneAnims[2];
				PatrolAnim = PhoneAnims[3];
			}
			if (Club == ClubType.Bully)
			{
				if (!StudentGlobals.GetStudentBroken(StudentID))
				{
					IdleAnim = PhoneAnims[0];
					BullyID = StudentID - 80;
				}
				if (TaskGlobals.GetTaskStatus(36) == 3 && !SchoolGlobals.ReactedToGameLeader)
				{
					StudentManager.ReactedToGameLeader = true;
					ScheduleBlock scheduleBlock = ScheduleBlocks[4];
					scheduleBlock.destination = "Shock";
					scheduleBlock.action = "Shock";
				}
			}
			if (!Male)
			{
				SkirtOrigins[0] = Skirt[0].transform.localPosition;
				SkirtOrigins[1] = Skirt[1].transform.localPosition;
				SkirtOrigins[2] = Skirt[2].transform.localPosition;
				SkirtOrigins[3] = Skirt[3].transform.localPosition;
				InstrumentBag[1].SetActive(false);
				InstrumentBag[2].SetActive(false);
				InstrumentBag[3].SetActive(false);
				InstrumentBag[4].SetActive(false);
				InstrumentBag[5].SetActive(false);
				PickRandomGossipAnim();
				DramaticCamera.gameObject.SetActive(false);
				AnimatedBook.SetActive(false);
				Handkerchief.SetActive(false);
				PepperSpray.SetActive(false);
				WateringCan.SetActive(false);
				Sketchbook.SetActive(false);
				Cigarette.SetActive(false);
				CandyBar.SetActive(false);
				Lighter.SetActive(false);
				GameObject[] instruments = Instruments;
				foreach (GameObject gameObject3 in instruments)
				{
					if (gameObject3 != null)
					{
						gameObject3.SetActive(false);
					}
				}
				Drumsticks[0].SetActive(false);
				Drumsticks[1].SetActive(false);
				if (Club >= ClubType.Teacher)
				{
					BecomeTeacher();
				}
				if (StudentManager.Censor && !Teacher)
				{
					Cosmetic.CensorPanties();
				}
				DisableEffects();
			}
			else
			{
				RandomCheerAnim = CheerAnims[UnityEngine.Random.Range(0, CheerAnims.Length)];
				MapMarker.gameObject.SetActive(false);
				DelinquentSpeechLines.Stop();
				PinkSeifuku.SetActive(false);
				WeaponBag.SetActive(false);
				Earpiece.SetActive(false);
				SetSplashes(false);
				ParticleSystem[] liquidEmitters = LiquidEmitters;
				foreach (ParticleSystem particleSystem in liquidEmitters)
				{
					particleSystem.gameObject.SetActive(false);
				}
			}
			if (StudentID == 1)
			{
				MapMarker.gameObject.SetActive(true);
			}
			else if (StudentID == 2)
			{
				if (StudentGlobals.GetStudentDead(3) || StudentGlobals.GetStudentKidnapped(3) || StudentManager.Students[3].Slave)
				{
					ScheduleBlock scheduleBlock2 = ScheduleBlocks[2];
					scheduleBlock2.destination = "Mourn";
					scheduleBlock2.action = "Mourn";
					IdleAnim = BulliedIdleAnim;
					WalkAnim = BulliedWalkAnim;
				}
			}
			else if (StudentID == 3)
			{
				if (StudentGlobals.GetStudentDead(2) || StudentGlobals.GetStudentKidnapped(2) || StudentManager.Students[2] == null || (StudentManager.Students[2] != null && StudentManager.Students[2].Slave))
				{
					ScheduleBlock scheduleBlock3 = ScheduleBlocks[2];
					scheduleBlock3.destination = "Mourn";
					scheduleBlock3.action = "Mourn";
					IdleAnim = BulliedIdleAnim;
					WalkAnim = BulliedWalkAnim;
				}
			}
			else if (StudentID == 4)
			{
				IdleAnim = "f02_idleShort_00";
				WalkAnim = "f02_newWalk_00";
			}
			else if (StudentID == 5)
			{
				LongSkirt = true;
				Shy = true;
			}
			else if (StudentID == 6)
			{
				RelaxAnim = "crossarms_00";
				CameraAnims = HeroAnims;
			}
			else if (StudentID == 7)
			{
				RunAnim = "runFeminine_00";
				SprintAnim = "runFeminine_00";
				RelaxAnim = "infirmaryRest_00";
				OriginalSprintAnim = SprintAnim;
				Cosmetic.Start();
				base.gameObject.SetActive(false);
			}
			else if (StudentID == 8)
			{
				IdleAnim = BulliedIdleAnim;
				WalkAnim = BulliedWalkAnim;
			}
			else if (StudentID == 9)
			{
				IdleAnim = "idleScholarly_00";
				WalkAnim = "walkScholarly_00";
				CameraAnims = HeroAnims;
			}
			else if (StudentID == 10)
			{
				if (StudentManager.Students[11] != null)
				{
					FollowTarget = StudentManager.Students[11];
				}
				else
				{
					ScheduleBlock scheduleBlock4 = ScheduleBlocks[2];
					scheduleBlock4.destination = "Mourn";
					scheduleBlock4.action = "Mourn";
					ScheduleBlock scheduleBlock5 = ScheduleBlocks[4];
					scheduleBlock5.destination = "Mourn";
					scheduleBlock5.action = "Mourn";
					ScheduleBlock scheduleBlock6 = ScheduleBlocks[6];
					scheduleBlock6.destination = "Mourn";
					scheduleBlock6.action = "Mourn";
					ScheduleBlock scheduleBlock7 = ScheduleBlocks[7];
					scheduleBlock7.destination = "Mourn";
					scheduleBlock7.action = "Mourn";
				}
				PhotoPatience = 0f;
				IdleAnim = "f02_idleGirly_00";
				WalkAnim = "f02_walkGirly_00";
				OriginalWalkAnim = WalkAnim;
			}
			else if (StudentID == 11)
			{
				PhotoPatience = 0f;
			}
			else if (StudentID == 24 && StudentID == 25)
			{
				IdleAnim = "f02_idleGirly_00";
				WalkAnim = "f02_walkGirly_00";
			}
			else if (StudentID == 26)
			{
				IdleAnim = "idleHaughty_00";
				WalkAnim = "walkHaughty_00";
			}
			else if (StudentID == 28 || StudentID == 30)
			{
				if (StudentID == 30)
				{
					SmartPhone.GetComponent<Renderer>().material.mainTexture = KokonaPhoneTexture;
				}
				if (DatingGlobals.SuitorProgress == 2)
				{
					Partner = ((StudentID != 30) ? StudentManager.Students[30] : StudentManager.Students[28]);
					ScheduleBlock scheduleBlock8 = ScheduleBlocks[4];
					scheduleBlock8.destination = "Cuddle";
					scheduleBlock8.action = "Cuddle";
				}
			}
			else if (StudentID == 31)
			{
				IdleAnim = BulliedIdleAnim;
				WalkAnim = BulliedWalkAnim;
			}
			else if (StudentID == 34 || StudentID == 35)
			{
				IdleAnim = "f02_idleShort_00";
				WalkAnim = "f02_newWalk_00";
				if (Paranoia > 1.66666f)
				{
					IdleAnim = ParanoidAnim;
					WalkAnim = ParanoidWalkAnim;
				}
			}
			else if (StudentID == 36)
			{
				if (TaskGlobals.GetTaskStatus(36) < 3)
				{
					IdleAnim = "slouchIdle_00";
					WalkAnim = "slouchWalk_00";
					ClubAnim = "slouchGaming_00";
				}
				else
				{
					IdleAnim = "properIdle_00";
					WalkAnim = "properWalk_00";
					ClubAnim = "properGaming_00";
				}
				if (Paranoia > 1.66666f)
				{
					IdleAnim = ParanoidAnim;
					WalkAnim = ParanoidWalkAnim;
				}
			}
			else if (StudentID == 39)
			{
				SmartPhone.GetComponent<Renderer>().material.mainTexture = MidoriPhoneTexture;
				PatrolAnim = "f02_midoriTexting_00";
			}
			else if (StudentID == 51)
			{
				IdleAnim = "f02_idleConfident_01";
				WalkAnim = "f02_walkConfident_01";
				if (ClubGlobals.GetClubClosed(ClubType.LightMusic))
				{
					IdleAnim = BulliedIdleAnim;
					WalkAnim = BulliedWalkAnim;
					CameraAnims = CowardAnims;
					Persona = PersonaType.Loner;
					if (!SchoolGlobals.RoofFence)
					{
						ScheduleBlock scheduleBlock9 = ScheduleBlocks[2];
						scheduleBlock9.destination = "Sulk";
						scheduleBlock9.action = "Sulk";
						ScheduleBlock scheduleBlock10 = ScheduleBlocks[4];
						scheduleBlock10.destination = "Sulk";
						scheduleBlock10.action = "Sulk";
						ScheduleBlock scheduleBlock11 = ScheduleBlocks[7];
						scheduleBlock11.destination = "Sulk";
						scheduleBlock11.action = "Sulk";
						ScheduleBlock scheduleBlock12 = ScheduleBlocks[8];
						scheduleBlock12.destination = "Sulk";
						scheduleBlock12.action = "Sulk";
					}
					else
					{
						ScheduleBlock scheduleBlock13 = ScheduleBlocks[2];
						scheduleBlock13.destination = "Seat";
						scheduleBlock13.action = "Sit";
						ScheduleBlock scheduleBlock14 = ScheduleBlocks[4];
						scheduleBlock14.destination = "Seat";
						scheduleBlock14.action = "Sit";
						ScheduleBlock scheduleBlock15 = ScheduleBlocks[7];
						scheduleBlock15.destination = "Seat";
						scheduleBlock15.action = "Sit";
						ScheduleBlock scheduleBlock16 = ScheduleBlocks[8];
						scheduleBlock16.destination = "Seat";
						scheduleBlock16.action = "Sit";
					}
				}
			}
			else if (StudentID == 56)
			{
				IdleAnim = "idleConfident_00";
				WalkAnim = "walkConfident_00";
				SleuthID = 0;
			}
			else if (StudentID == 57)
			{
				IdleAnim = "idleChill_01";
				WalkAnim = "walkChill_01";
				SleuthID = 20;
			}
			else if (StudentID == 58)
			{
				IdleAnim = "idleChill_00";
				WalkAnim = "walkChill_00";
				SleuthID = 40;
			}
			else if (StudentID == 59)
			{
				IdleAnim = "f02_idleGraceful_00";
				WalkAnim = "f02_walkGraceful_00";
				SleuthID = 60;
			}
			else if (StudentID == 60)
			{
				IdleAnim = "f02_idleScholarly_00";
				WalkAnim = "f02_walkScholarly_00";
				CameraAnims = HeroAnims;
				SleuthID = 80;
			}
			else if (StudentID == 61)
			{
				IdleAnim = "scienceIdle_00";
				WalkAnim = "scienceWalk_00";
				OriginalWalkAnim = "scienceWalk_00";
			}
			else if (StudentID == 62)
			{
				IdleAnim = "idleFrown_00";
				WalkAnim = "walkFrown_00";
				if (Paranoia > 1.66666f)
				{
					IdleAnim = ParanoidAnim;
					WalkAnim = ParanoidWalkAnim;
				}
			}
			else if (StudentID == 64 || StudentID == 65)
			{
				IdleAnim = "f02_idleShort_00";
				WalkAnim = "f02_newWalk_00";
				if (Paranoia > 1.66666f)
				{
					IdleAnim = ParanoidAnim;
					WalkAnim = ParanoidWalkAnim;
				}
			}
			else if (StudentID == 66)
			{
				IdleAnim = "pose_03";
				OriginalWalkAnim = "walkConfident_00";
				WalkAnim = "walkConfident_00";
				ClubThreshold = 100f;
			}
			else if (StudentID == 69)
			{
				IdleAnim = "idleFrown_00";
				WalkAnim = "walkFrown_00";
				if (Paranoia > 1.66666f)
				{
					IdleAnim = ParanoidAnim;
					WalkAnim = ParanoidWalkAnim;
				}
			}
			else if (StudentID == 71)
			{
				IdleAnim = "f02_idleGirly_00";
				WalkAnim = "f02_walkGirly_00";
			}
			OriginalWalkAnim = WalkAnim;
			if (StudentGlobals.GetStudentGrudge(StudentID))
			{
				if (Persona != PersonaType.Coward && Persona != PersonaType.Evil)
				{
					CameraAnims = EvilAnims;
					Reputation.PendingRep -= 10f;
					PendingRep -= 10f;
					for (ID = 0; ID < Outlines.Length; ID++)
					{
						Outlines[ID].color = new Color(1f, 1f, 0f, 1f);
					}
				}
				Grudge = true;
				CameraAnims = EvilAnims;
			}
			if (Persona == PersonaType.Sleuth)
			{
				if (SchoolGlobals.SchoolAtmosphere <= 0.8f || Grudge)
				{
					SprintAnim = SleuthSprintAnim;
					IdleAnim = SleuthIdleAnim;
					WalkAnim = SleuthWalkAnim;
					CameraAnims = HeroAnims;
					SmartPhone.SetActive(true);
					Countdown.Speed = 0.075f;
					Sleuthing = true;
					if (Male)
					{
						SmartPhone.transform.localPosition = new Vector3(0.06f, -0.02f, -0.02f);
					}
					else
					{
						SmartPhone.transform.localPosition = new Vector3(0.033333f, -0.015f, -0.015f);
					}
					SmartPhone.transform.localEulerAngles = new Vector3(12.5f, 120f, 180f);
					if (Club == ClubType.None)
					{
						GetSleuthTarget();
					}
					else
					{
						SleuthTarget = StudentManager.Clubs.List[StudentID];
					}
					if (!Grudge)
					{
						ScheduleBlock scheduleBlock17 = ScheduleBlocks[2];
						scheduleBlock17.destination = "Sleuth";
						scheduleBlock17.action = "Sleuth";
						ScheduleBlock scheduleBlock18 = ScheduleBlocks[4];
						scheduleBlock18.destination = "Sleuth";
						scheduleBlock18.action = "Sleuth";
						ScheduleBlock scheduleBlock19 = ScheduleBlocks[7];
						scheduleBlock19.destination = "Sleuth";
						scheduleBlock19.action = "Sleuth";
					}
					else
					{
						StalkTarget = Yandere.transform;
						SleuthTarget = Yandere.transform;
						ScheduleBlock scheduleBlock20 = ScheduleBlocks[2];
						scheduleBlock20.destination = "Stalk";
						scheduleBlock20.action = "Stalk";
						ScheduleBlock scheduleBlock21 = ScheduleBlocks[4];
						scheduleBlock21.destination = "Stalk";
						scheduleBlock21.action = "Stalk";
						ScheduleBlock scheduleBlock22 = ScheduleBlocks[7];
						scheduleBlock22.destination = "Stalk";
						scheduleBlock22.action = "Stalk";
					}
				}
				else if (SchoolGlobals.SchoolAtmosphere <= 0.9f)
				{
					WalkAnim = ParanoidWalkAnim;
					CameraAnims = HeroAnims;
				}
			}
			if (!Slave)
			{
				if (StudentManager.Bullies > 1)
				{
					if (StudentID == 81 || StudentID == 83 || StudentID == 85)
					{
						if (Persona != PersonaType.Coward)
						{
							Pathfinding.canSearch = false;
							Pathfinding.canMove = false;
							Paired = true;
							CharacterAnimation["f02_walkTalk_00"].time += StudentID - 81;
							WalkAnim = "f02_walkTalk_00";
							SpeechLines.Play();
						}
					}
					else if (StudentID == 82 || StudentID == 84)
					{
						Pathfinding.canSearch = false;
						Pathfinding.canMove = false;
						Paired = true;
						CharacterAnimation["f02_walkTalk_01"].time += StudentID - 81;
						WalkAnim = "f02_walkTalk_01";
						SpeechLines.Play();
					}
				}
				if (Club == ClubType.Delinquent)
				{
					if (PlayerPrefs.GetInt("WeaponsBanned") == 0)
					{
						MyWeapon = Yandere.WeaponManager.DelinquentWeapons[StudentID - 75];
						MyWeapon.transform.parent = WeaponBagParent;
						MyWeapon.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
						MyWeapon.transform.localPosition = new Vector3(0f, 0f, 0f);
						MyWeapon.FingerprintID = StudentID;
						MyWeapon.MyCollider.enabled = false;
						WeaponBag.SetActive(true);
					}
					else
					{
						OriginalPersona = PersonaType.Heroic;
						Persona = PersonaType.Heroic;
					}
					ScaredAnim = "delinquentCombatIdle_00";
					LeanAnim = "delinquentConcern_00";
					ShoveAnim = "pushTough_00";
					WalkAnim = "walkTough_00";
					IdleAnim = "idleTough_00";
					SpeechLines = DelinquentSpeechLines;
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Paired = true;
				}
			}
			if (StudentID == StudentManager.RivalID)
			{
				RivalPrefix = "Rival ";
				if (DateGlobals.Weekday == DayOfWeek.Friday)
				{
					ScheduleBlock scheduleBlock23 = ScheduleBlocks[7];
					scheduleBlock23.time = 17f;
				}
			}
			if (Club == ClubType.None)
			{
				if (StudentID == 11)
				{
					SmartPhone.transform.localPosition = new Vector3(-0.0075f, -0.0025f, -0.0075f);
					SmartPhone.transform.localEulerAngles = new Vector3(5f, -150f, 170f);
					SmartPhone.GetComponent<Renderer>().material.mainTexture = OsanaPhoneTexture;
					IdleAnim = "f02_tsunIdle_00";
					WalkAnim = "f02_tsunWalk_00";
					OriginalWalkAnim = WalkAnim;
					TaskAnims[0] = "f02_Task33_Line0";
					TaskAnims[1] = "f02_Task33_Line1";
					TaskAnims[2] = "f02_Task33_Line2";
					TaskAnims[3] = "f02_Task33_Line3";
					TaskAnims[4] = "f02_Task33_Line4";
					TaskAnims[5] = "f02_Task33_Line5";
				}
			}
			else if (Club == ClubType.Cooking)
			{
				SleuthID = (StudentID - 21) * 20;
				ClubAnim = PrepareFoodAnim;
				ClubMemberID = StudentID - 20;
				MyPlate = StudentManager.Plates[ClubMemberID];
				OriginalPlatePosition = MyPlate.position;
				OriginalPlateRotation = MyPlate.rotation;
				if (!GameGlobals.EmptyDemon)
				{
					ApronAttacher.enabled = true;
				}
			}
			else if (Club == ClubType.Drama)
			{
				if (StudentID == 26)
				{
					ClubAnim = "teaching_00";
				}
				else if (StudentID == 27 || StudentID == 28)
				{
					ClubAnim = "sit_00";
				}
				else if (StudentID == 29 || StudentID == 30)
				{
					ClubAnim = "f02_sit_00";
				}
				OriginalClubAnim = ClubAnim;
			}
			else if (Club == ClubType.Occult)
			{
				PatrolAnim = ThinkAnim;
				CharacterAnimation[PatrolAnim].speed = 0.2f;
			}
			else if (Club == ClubType.Gaming)
			{
				MiyukiGameScreen.SetActive(true);
				ClubMemberID = StudentID - 35;
				if (StudentID > 36)
				{
					ClubAnim = GameAnim;
				}
				ActivityAnim = GameAnim;
			}
			else if (Club == ClubType.Art)
			{
				ChangingBooth = StudentManager.ChangingBooths[4];
				ActivityAnim = PaintAnim;
				Attacher.ArtClub = true;
				ClubAnim = PaintAnim;
				DressCode = true;
			}
			else if (Club == ClubType.LightMusic)
			{
				ClubMemberID = StudentID - 50;
				InstrumentBag[ClubMemberID].SetActive(true);
				if (StudentID == 51)
				{
					ClubAnim = "f02_practiceGuitar_01";
				}
				else if (StudentID == 52 || StudentID == 53)
				{
					ClubAnim = "f02_practiceGuitar_00";
				}
				else if (StudentID == 54)
				{
					ClubAnim = "f02_practiceDrums_00";
					Instruments[4] = StudentManager.DrumSet;
				}
				else if (StudentID == 55)
				{
					ClubAnim = "f02_practiceKeytar_00";
				}
			}
			else if (Club == ClubType.MartialArts)
			{
				ChangingBooth = StudentManager.ChangingBooths[6];
				DressCode = true;
				if (StudentID == 46)
				{
					IdleAnim = "pose_03";
					ClubAnim = "pose_03";
				}
				else if (StudentID == 47)
				{
					GetNewAnimation = true;
					ClubAnim = "idle_20";
					ActivityAnim = "kick_24";
				}
				else if (StudentID == 48)
				{
					ClubAnim = "sit_04";
					ActivityAnim = "kick_24";
				}
				else if (StudentID == 49)
				{
					GetNewAnimation = true;
					ClubAnim = "f02_idle_20";
					ActivityAnim = "f02_kick_23";
				}
				else if (StudentID == 50)
				{
					ClubAnim = "f02_sit_05";
					ActivityAnim = "f02_kick_23";
				}
				ClubMemberID = StudentID - 45;
			}
			else if (Club == ClubType.Science)
			{
				ChangingBooth = StudentManager.ChangingBooths[8];
				Attacher.ScienceClub = true;
				DressCode = true;
				if (StudentID == 61)
				{
					ClubAnim = "scienceMad_00";
				}
				else if (StudentID == 62)
				{
					ClubAnim = "scienceTablet_00";
				}
				else if (StudentID == 63)
				{
					ClubAnim = "scienceChemistry_00";
				}
				else if (StudentID == 64)
				{
					ClubAnim = "f02_scienceLeg_00";
				}
				else if (StudentID == 65)
				{
					ClubAnim = "f02_scienceConsole_00";
				}
				ClubMemberID = StudentID - 60;
			}
			else if (Club == ClubType.Sports)
			{
				ChangingBooth = StudentManager.ChangingBooths[9];
				DressCode = true;
				ClubAnim = "stretch_00";
				ClubMemberID = StudentID - 65;
			}
			else if (Club == ClubType.Gardening)
			{
				if (StudentID == 71)
				{
					PatrolAnim = "f02_thinking_00";
					ClubAnim = "f02_thinking_00";
					CharacterAnimation[PatrolAnim].speed = 0.9f;
				}
				else
				{
					ClubAnim = "f02_waterPlant_00";
					WateringCan.SetActive(true);
				}
			}
			if (OriginalClub != ClubType.Gaming)
			{
				Miyuki.gameObject.SetActive(false);
			}
			if (Cosmetic.Hairstyle == 20)
			{
				IdleAnim = "f02_tsunIdle_00";
			}
			if (!Teacher && Name != "Random")
			{
				StudentManager.CleaningManager.GetRole(StudentID);
				CleaningSpot = StudentManager.CleaningManager.Spot;
				CleaningRole = StudentManager.CleaningManager.Role;
			}
			GetDestinations();
			OriginalActions = new StudentActionType[Actions.Length];
			Array.Copy(Actions, OriginalActions, Actions.Length);
			if (AoT)
			{
				AttackOnTitan();
			}
			if (Slave)
			{
				NEStairs = GameObject.Find("NEStairs").GetComponent<Collider>();
				NWStairs = GameObject.Find("NWStairs").GetComponent<Collider>();
				SEStairs = GameObject.Find("SEStairs").GetComponent<Collider>();
				SWStairs = GameObject.Find("SWStairs").GetComponent<Collider>();
				IdleAnim = BrokenAnim;
				WalkAnim = BrokenWalkAnim;
				RightEmptyEye.SetActive(true);
				LeftEmptyEye.SetActive(true);
				SmartPhone.SetActive(false);
				Distracted = true;
				Indoors = true;
				Safe = false;
				Shy = false;
				for (ID = 0; ID < ScheduleBlocks.Length; ID++)
				{
					ScheduleBlocks[ID].time = 0f;
				}
				if (FragileSlave)
				{
					HuntTarget = StudentManager.Students[StudentGlobals.GetFragileTarget()];
				}
				else
				{
					SchoolGlobals.KidnapVictim = 0;
				}
			}
			if (Spooky)
			{
				Spook();
			}
			Prompt.HideButton[0] = true;
			Prompt.HideButton[2] = true;
			for (ID = 0; ID < Ragdoll.AllRigidbodies.Length; ID++)
			{
				Ragdoll.AllRigidbodies[ID].isKinematic = true;
				Ragdoll.AllColliders[ID].enabled = false;
			}
			Ragdoll.AllColliders[10].enabled = true;
			if (StudentID == 1)
			{
				DetectionMarker.GetComponent<DetectionMarkerScript>().Tex.color = new Color(1f, 0f, 0f, 0f);
				Yandere.Senpai = base.transform;
				Yandere.LookAt.target = Head;
				for (ID = 0; ID < Outlines.Length; ID++)
				{
					if (Outlines[ID] != null)
					{
						Outlines[ID].enabled = true;
						Outlines[ID].color = new Color(1f, 0f, 1f);
					}
				}
				Prompt.ButtonActive[0] = false;
				Prompt.ButtonActive[1] = false;
				Prompt.ButtonActive[2] = false;
				Prompt.ButtonActive[3] = false;
				if (StudentManager.MissionMode)
				{
					base.transform.position = Vector3.zero;
					base.gameObject.SetActive(false);
				}
			}
			else if (!StudentGlobals.GetStudentPhotographed(StudentID))
			{
				for (ID = 0; ID < Outlines.Length; ID++)
				{
					if (Outlines[ID] != null)
					{
						Outlines[ID].enabled = false;
						Outlines[ID].color = new Color(0f, 1f, 0f);
					}
				}
			}
			PickRandomAnim();
			PickRandomSleuthAnim();
			Renderer component = Armband.GetComponent<Renderer>();
			Armband.SetActive(false);
			if (Club != 0 && (StudentID == 21 || StudentID == 26 || StudentID == 31 || StudentID == 36 || StudentID == 41 || StudentID == 46 || StudentID == 51 || StudentID == 56 || StudentID == 61 || StudentID == 66 || StudentID == 71))
			{
				Armband.SetActive(true);
				if (GameGlobals.EmptyDemon)
				{
					IdleAnim = OriginalIdleAnim;
					WalkAnim = walkAnim;
					OriginalPersona = PersonaType.Evil;
					Persona = PersonaType.Evil;
					ScaredAnim = EvilWitnessAnim;
				}
			}
			if (!Teacher)
			{
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
			}
			else
			{
				Indoors = true;
			}
			if (StudentID == 1 || StudentID == 4 || StudentID == 5 || StudentID == 11)
			{
				BookRenderer.material.mainTexture = RedBookTexture;
			}
			CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
			if (StudentManager.MissionMode && StudentID == MissionModeGlobals.MissionTarget)
			{
				for (ID = 0; ID < Outlines.Length; ID++)
				{
					Outlines[ID].enabled = true;
					Outlines[ID].color = new Color(1f, 0f, 0f);
				}
			}
			UnityEngine.Object.Destroy(MyRigidbody);
			Started = true;
			if (Club == ClubType.Council)
			{
				component.material.SetTextureOffset("_MainTex", new Vector2(-103f / 160f, 0f));
				Armband.SetActive(true);
				Indoors = true;
				Spawned = true;
				if (ShoeRemoval.Locker == null)
				{
					ShoeRemoval.Start();
				}
				ShoeRemoval.PutOnShoes();
				if (StudentID == 86)
				{
					Suffix = "Strict";
				}
				else if (StudentID == 87)
				{
					Suffix = "Casual";
				}
				else if (StudentID == 88)
				{
					Suffix = "Grace";
				}
				else if (StudentID == 89)
				{
					Suffix = "Edgy";
				}
				IdleAnim = "f02_idleCouncil" + Suffix + "_00";
				WalkAnim = "f02_walkCouncil" + Suffix + "_00";
				ShoveAnim = "f02_pushCouncil" + Suffix + "_00";
				PatrolAnim = "f02_scanCouncil" + Suffix + "_00";
				RelaxAnim = "f02_relaxCouncil" + Suffix + "_00";
				SprayAnim = "f02_sprayCouncil" + Suffix + "_00";
				BreakUpAnim = "f02_stopCouncil" + Suffix + "_00";
				PickUpAnim = "f02_teacherPickUp_00";
				ScaredAnim = ReadyToFightAnim;
				ParanoidAnim = GuardAnim;
				CameraAnims[1] = IdleAnim;
				CameraAnims[2] = IdleAnim;
				CameraAnims[3] = IdleAnim;
				ClubMemberID = StudentID - 85;
				VisionDistance *= 2f;
			}
			if (!StudentManager.NoClubMeeting && Armband.activeInHierarchy && Clock.Weekday == 5)
			{
				if (StudentID < 86)
				{
					ScheduleBlock scheduleBlock24 = ScheduleBlocks[6];
					scheduleBlock24.destination = "Meeting";
					scheduleBlock24.action = "Meeting";
				}
				else
				{
					ScheduleBlock scheduleBlock25 = ScheduleBlocks[5];
					scheduleBlock25.destination = "Meeting";
					scheduleBlock25.action = "Meeting";
				}
				GetDestinations();
			}
			if (StudentID == 81 && StudentGlobals.GetStudentBroken(81))
			{
				Destinations[2] = StudentManager.BrokenSpot;
				Destinations[4] = StudentManager.BrokenSpot;
				Actions[2] = StudentActionType.Shamed;
				Actions[4] = StudentActionType.Shamed;
			}
		}
		UpdateAnimLayers();
		if (!Male)
		{
			GetComponent<FLookAnimatorWEyes>().ObjectToFollow = Yandere.Head;
			GetComponent<FLookAnimatorWEyes>().EyesTarget = Yandere.Head;
		}
		if (StudentID > 9 && StudentID < 21)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private float GetPerceptionPercent(float distance)
	{
		float num = Mathf.Clamp01(distance / VisionDistance);
		return 1f - num * num;
	}

	private bool PointIsInFOV(Vector3 point)
	{
		Vector3 position = Eyes.transform.position;
		Vector3 to = point - position;
		float num = VisionFOV * 0.5f;
		return Vector3.Angle(Head.transform.forward, to) <= num;
	}

	public bool CanSeeObject(GameObject obj, Vector3 targetPoint, int[] layers, int mask)
	{
		Vector3 position = Eyes.transform.position;
		Vector3 vector = targetPoint - position;
		if (PointIsInFOV(targetPoint))
		{
			float num = VisionDistance * VisionDistance;
			if (vector.sqrMagnitude <= num)
			{
				Debug.DrawLine(position, targetPoint, Color.green);
				RaycastHit hitInfo;
				if (Physics.Linecast(position, targetPoint, out hitInfo, mask))
				{
					foreach (int num2 in layers)
					{
						if (hitInfo.collider.gameObject.layer == num2)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public bool CanSeeObject(GameObject obj, Vector3 targetPoint)
	{
		if (!Blind)
		{
			Debug.DrawLine(Eyes.position, targetPoint, Color.green);
			Vector3 position = Eyes.transform.position;
			Vector3 vector = targetPoint - position;
			float num = VisionDistance * VisionDistance;
			bool flag = PointIsInFOV(targetPoint);
			bool flag2 = vector.sqrMagnitude <= num;
			RaycastHit hitInfo;
			if (flag && flag2 && Physics.Linecast(position, targetPoint, out hitInfo, Mask) && hitInfo.collider.gameObject == obj)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanSeeObject(GameObject obj)
	{
		return CanSeeObject(obj, obj.transform.position);
	}

	private void Update()
	{ // Minified by tanos [v]
		if (!Stop) {
    		DistanceToPlayer = Vector3.Distance(transform.position, Yandere.transform.position);
    		if (Yandere.Egg && StudentID > 1) {
        		if (DistanceToPlayer < 1f && Yandere.EbolaHair.activeInHierarchy) {
            		Instantiate(Yandere.EbolaEffect, transform.position + Vector3.up, Quaternion.identity);
            		SpawnAlarmDisc(); BecomeRagdoll(); DeathType = DeathType.EasterEgg;
        		}
        	if (DistanceToPlayer < 5f && Yandere.Hunger >= 5) {
            	Instantiate(Yandere.DarkHelix, transform.position + Vector3.up, Quaternion.identity);
            	SpawnAlarmDisc(); BecomeRagdoll(); DeathType = DeathType.EasterEgg;
        	}
    	}
    	UpdateRoutine(); UpdateDetectionMarker();
    	if (Dying) {
    	    UpdateDying();
    	    if (Burning) UpdateBurning();
    	} else {
    	    if (DistanceToPlayer <= 2f) UpdateTalkInput();
    	    UpdateVision();
    	    if (Pushed) UpdatePushed();
    	    else if (Drowned) UpdateDrowned();
    	    else if (WitnessedMurder) UpdateWitnessedMurder();
    	    else if (Alarmed) UpdateAlarmed();
    	    else if (TurnOffRadio) UpdateTurningOffRadio();
    	    else if (Confessing) UpdateConfessing();
    	    else if (Vomiting) UpdateVomiting();
    	    else if (Splashed) UpdateSplashed();
    		}
    		UpdateMisc();
		}

		else
		{
			if (StudentManager.Pose)
			{
				if (Prompt.Circle[0].fillAmount == 0f)
				{
					Pose();
				}
			}
			else if (!ClubActivity)
			{
				if (!Yandere.Talking)
				{
					if (Yandere.Sprayed)
					{
						CharacterAnimation.CrossFade(ScaredAnim);
					}
					if (Yandere.Noticed || StudentManager.YandereDying)
					{
						targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					}
				}
			}
			else if (Police.Darkness.color.a < 1f)
			{
				if (Club == ClubType.Cooking)
				{
					CharacterAnimation[SocialSitAnim].layer = 99;
					CharacterAnimation.Play(SocialSitAnim);
					CharacterAnimation[SocialSitAnim].weight = 1f;
					SmartPhone.SetActive(false);
					SpeechLines.Play();
					CharacterAnimation.CrossFade(RandomAnim);
					if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
					{
						PickRandomAnim();
					}
				}
				else if (Club == ClubType.MartialArts)
				{
					CharacterAnimation.Play(ActivityAnim);
					AudioSource component = GetComponent<AudioSource>();
					if (!Male)
					{
						if (CharacterAnimation["f02_kick_23"].time > 1f)
						{
							if (!PlayingAudio)
							{
								component.clip = FemaleAttacks[UnityEngine.Random.Range(0, FemaleAttacks.Length)];
								component.Play();
								PlayingAudio = true;
							}
							if (CharacterAnimation["f02_kick_23"].time >= CharacterAnimation["f02_kick_23"].length)
							{
								CharacterAnimation["f02_kick_23"].time = 0f;
								PlayingAudio = false;
							}
						}
					}
					else if (CharacterAnimation["kick_24"].time > 1f)
					{
						if (!PlayingAudio)
						{
							component.clip = MaleAttacks[UnityEngine.Random.Range(0, MaleAttacks.Length)];
							component.Play();
							PlayingAudio = true;
						}
						if (CharacterAnimation["kick_24"].time >= CharacterAnimation["kick_24"].length)
						{
							CharacterAnimation["kick_24"].time = 0f;
							PlayingAudio = false;
						}
					}
				}
				else if (Club == ClubType.Drama)
				{
					Stop = false;
				}
				else if (Club == ClubType.Photography)
				{
					CharacterAnimation.CrossFade(RandomSleuthAnim);
					if (CharacterAnimation[RandomSleuthAnim].time >= CharacterAnimation[RandomSleuthAnim].length)
					{
						PickRandomSleuthAnim();
					}
				}
				else if (Club == ClubType.Art)
				{
					CharacterAnimation.Play(ActivityAnim);
					Paintbrush.SetActive(true);
					Palette.SetActive(true);
				}
				else if (Club == ClubType.Science)
				{
					CharacterAnimation.Play(ClubAnim);
					if (StudentID == 62)
					{
						ScienceProps[0].SetActive(true);
					}
					else if (StudentID == 63)
					{
						ScienceProps[1].SetActive(true);
						ScienceProps[2].SetActive(true);
					}
					else if (StudentID == 64)
					{
						ScienceProps[0].SetActive(true);
					}
				}
				else if (Club == ClubType.Sports)
				{
					Stop = false;
				}
				else if (Club == ClubType.Gardening)
				{
					CharacterAnimation.Play(ClubAnim);
					Stop = false;
				}
				else if (Club == ClubType.Gaming)
				{
					CharacterAnimation.CrossFade(ClubAnim);
				}
			}
			Alarm = Mathf.MoveTowards(Alarm, 0f, Time.deltaTime);
			UpdateDetectionMarker();
		}
		if (AoT)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(10f, 10f, 10f), Time.deltaTime);
		}
		if (Prompt.Label[0] != null)
		{
			if (StudentManager.Pose)
			{
				Prompt.Label[0].text = "     Pose";
			}
			else if (BadTime)
			{
				Prompt.Label[0].text = "     Psychokinesis";
			}
		}
	}

	private void UpdateRoutine()
	{
		if (Routine)
		{
			if (CurrentDestination != null)
			{
				DistanceToDestination = Vector3.Distance(base.transform.position, CurrentDestination.position);
			}
			if (StalkerFleeing)
			{
				if (Actions[Phase] == StudentActionType.Stalk && DistanceToPlayer > 10f)
				{
					Pathfinding.target = Yandere.transform;
					CurrentDestination = Yandere.transform;
					StalkerFleeing = false;
					Pathfinding.speed = 1f;
				}
			}
			else if (Actions[Phase] == StudentActionType.Stalk)
			{
				if (DistanceToPlayer > 20f)
				{
					Pathfinding.speed = 4f;
				}
				else if (DistanceToPlayer < 10f)
				{
					Pathfinding.speed = 1f;
				}
			}
			if (!Indoors)
			{
				if (Hurry && !Tripped && base.transform.position.z > -50.5f && base.transform.position.x < 6f)
				{
					CharacterAnimation.CrossFade("trip_00");
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Tripping = true;
					Routine = false;
				}
				if (Paired)
				{
					if (base.transform.position.z < -50f)
					{
						if (base.transform.localEulerAngles != Vector3.zero)
						{
							base.transform.localEulerAngles = Vector3.zero;
						}
						MyController.Move(base.transform.forward * Time.deltaTime);
						if (StudentID == 81)
						{
							MusumeTimer += Time.deltaTime;
							if (MusumeTimer > 5f)
							{
								MusumeTimer = 0f;
								MusumeRight = !MusumeRight;
								WalkAnim = ((!MusumeRight) ? "f02_walkTalk_01" : "f02_walkTalk_00");
							}
						}
					}
					else
					{
						if (Persona == PersonaType.PhoneAddict)
						{
							SpeechLines.Stop();
							SmartPhone.SetActive(true);
						}
						Pathfinding.canSearch = true;
						Pathfinding.canMove = true;
						StopPairing();
					}
				}
				if (Phase == 0)
				{
					if (DistanceToDestination < 1f)
					{
						CurrentDestination = MyLocker;
						Pathfinding.target = MyLocker;
						Phase++;
					}
				}
				else if (DistanceToDestination < 0.5f && !ShoeRemoval.enabled)
				{
					if (Shy)
					{
						CharacterAnimation[ShyAnim].weight = 0.5f;
					}
					SmartPhone.SetActive(false);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					ShoeRemoval.StartChangingShoes();
					ShoeRemoval.enabled = true;
					CanTalk = false;
					Routine = false;
				}
			}
			else if (Phase < ScheduleBlocks.Length - 1)
			{
				if (Clock.HourTime >= ScheduleBlocks[Phase].time && !InEvent && !Meeting && ClubActivityPhase < 16)
				{
					MyRenderer.updateWhenOffscreen = false;
					SprintAnim = OriginalSprintAnim;
					Headache = false;
					Sedated = false;
					Hurry = false;
					Phase++;
					if (Drownable)
					{
						Drownable = false;
						StudentManager.UpdateMe(StudentID);
					}
					if (StudentID == 11)
					{
					}
					if (Actions[Phase] == StudentActionType.Follow)
					{
						TargetDistance = 1f;
					}
					if (Schoolwear > 1 && Destinations[Phase] == MyLocker)
					{
						Phase++;
					}
					if (Actions[Phase] == StudentActionType.Graffiti && !StudentManager.Bully)
					{
						ScheduleBlock scheduleBlock = ScheduleBlocks[2];
						scheduleBlock.destination = "Patrol";
						scheduleBlock.action = "Patrol";
						GetDestinations();
					}
					if (!StudentManager.ReactedToGameLeader && Actions[Phase] == StudentActionType.Bully && !StudentManager.Bully)
					{
						ScheduleBlock scheduleBlock2 = ScheduleBlocks[4];
						scheduleBlock2.destination = "Sunbathe";
						scheduleBlock2.action = "Sunbathe";
						GetDestinations();
					}
					if (Sedated)
					{
						SprintAnim = OriginalSprintAnim;
						Sedated = false;
					}
					CurrentAction = Actions[Phase];
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
					if (((StudentID == 30 && StudentManager.DatingMinigame.Affection == 100f) || (StudentID == StudentManager.RivalID && DateGlobals.Weekday == DayOfWeek.Friday && !InCouple)) && Actions[Phase] == StudentActionType.ChangeShoes)
					{
						if (StudentID == 30)
						{
							CurrentDestination = StudentManager.SuitorLocker;
							Pathfinding.target = StudentManager.SuitorLocker;
						}
						else
						{
							CurrentDestination = StudentManager.SenpaiLocker;
							Pathfinding.target = StudentManager.SenpaiLocker;
						}
						Confessing = true;
						Routine = false;
						CanTalk = false;
					}
					if (CurrentDestination != null)
					{
						DistanceToDestination = Vector3.Distance(base.transform.position, CurrentDestination.position);
					}
					if (Bento != null && Bento.activeInHierarchy)
					{
						Bento.SetActive(false);
						Chopsticks[0].SetActive(false);
						Chopsticks[1].SetActive(false);
					}
					if (Male)
					{
						Cosmetic.MyRenderer.materials[Cosmetic.FaceID].SetFloat("_BlendAmount", 0f);
						PinkSeifuku.SetActive(false);
					}
					if (StudentID == 81)
					{
						Cigarette.SetActive(false);
						Lighter.SetActive(false);
					}
					if (!Paired)
					{
						Pathfinding.canSearch = true;
						Pathfinding.canMove = true;
					}
					if (Persona != PersonaType.PhoneAddict && !Sleuthing)
					{
						SmartPhone.SetActive(false);
					}
					else if (!Slave)
					{
						IdleAnim = PhoneAnims[0];
						SmartPhone.SetActive(true);
					}
					Paintbrush.SetActive(false);
					Sketchbook.SetActive(false);
					Palette.SetActive(false);
					Pencil.SetActive(false);
					OccultBook.SetActive(false);
					Scrubber.SetActive(false);
					Eraser.SetActive(false);
					Pen.SetActive(false);
					GameObject[] scienceProps = ScienceProps;
					foreach (GameObject gameObject in scienceProps)
					{
						if (gameObject != null)
						{
							gameObject.SetActive(false);
						}
					}
					GameObject[] fingerfood = Fingerfood;
					foreach (GameObject gameObject2 in fingerfood)
					{
						if (gameObject2 != null)
						{
							gameObject2.SetActive(false);
						}
					}
					SpeechLines.Stop();
					GoAway = false;
					ReadPhase = 0;
					PatrolID = 0;
					if (Actions[Phase] == StudentActionType.Clean)
					{
						EquipCleaningItems();
					}
					else if (!Slave)
					{
						if (Persona == PersonaType.PhoneAddict)
						{
							SmartPhone.transform.localPosition = new Vector3(0.01f, 0.005f, 0.01f);
							SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, 165f);
							WalkAnim = PhoneAnims[1];
						}
						else if (Sleuthing)
						{
							WalkAnim = SleuthWalkAnim;
						}
					}
					if (Actions[Phase] == StudentActionType.Sleuth && StudentManager.SleuthPhase == 3)
					{
						GetSleuthTarget();
					}
					if (Actions[Phase] == StudentActionType.Stalk)
					{
						TargetDistance = 10f;
					}
					else
					{
						TargetDistance = 0.5f;
					}
					if (Club == ClubType.Gardening && StudentID != 71)
					{
						WateringCan.transform.parent = Hips;
						WateringCan.transform.localPosition = new Vector3(0f, 0.0135f, -0.184f);
						WateringCan.transform.localEulerAngles = new Vector3(0f, 90f, 30f);
						if (Clock.Period == 6)
						{
							StudentManager.Patrols.List[StudentID] = StudentManager.GardeningPatrols[StudentID - 71];
							ClubAnim = "f02_waterPlantLow_00";
							CurrentDestination = StudentManager.Patrols.List[StudentID].GetChild(PatrolID);
							Pathfinding.target = CurrentDestination;
						}
					}
					if (Club == ClubType.LightMusic)
					{
						if (StudentID == 51)
						{
							if (InstrumentBag[ClubMemberID].transform.parent == null)
							{
								Instruments[ClubMemberID].GetComponent<AudioSource>().playOnAwake = false;
								Instruments[ClubMemberID].GetComponent<AudioSource>().Stop();
								Instruments[ClubMemberID].transform.parent = null;
								Instruments[ClubMemberID].transform.position = new Vector3(-0.5f, 4.5f, 22.45666f);
								Instruments[ClubMemberID].transform.eulerAngles = new Vector3(-15f, 0f, 0f);
							}
							else
							{
								Instruments[ClubMemberID].SetActive(false);
							}
						}
						else
						{
							Instruments[ClubMemberID].SetActive(false);
						}
						Drumsticks[0].SetActive(false);
						Drumsticks[1].SetActive(false);
						AirGuitar.Stop();
					}
					if (Phase == 8 && StudentID == 36)
					{
						StudentManager.Clubs.List[StudentID].position = StudentManager.Clubs.List[71].position;
						StudentManager.Clubs.List[StudentID].rotation = StudentManager.Clubs.List[71].rotation;
						ClubAnim = GameAnim;
					}
					if (MyPlate != null && MyPlate.parent == RightHand)
					{
						CurrentDestination = StudentManager.Clubs.List[StudentID];
						Pathfinding.target = StudentManager.Clubs.List[StudentID];
					}
					if (Persona == PersonaType.Sleuth)
					{
						if (Male)
						{
							SmartPhone.transform.localPosition = new Vector3(0.06f, -0.02f, -0.02f);
						}
						else
						{
							SmartPhone.transform.localPosition = new Vector3(0.033333f, -0.015f, -0.015f);
						}
						SmartPhone.transform.localEulerAngles = new Vector3(12.5f, 120f, 180f);
					}
				}
				if (!Teacher && Club != ClubType.Delinquent && (Clock.Period == 2 || Clock.Period == 4) && ClubActivityPhase < 16)
				{
					Pathfinding.speed = 4f;
				}
			}
			if (MeetTime > 0f && Clock.HourTime > MeetTime)
			{
				CurrentDestination = MeetSpot;
				Pathfinding.target = MeetSpot;
				DistanceToDestination = Vector3.Distance(base.transform.position, CurrentDestination.position);
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				Pathfinding.speed = 4f;
				SpeechLines.Stop();
				Meeting = true;
				MeetTime = 0f;
			}
			if (DistanceToDestination > TargetDistance)
			{
				if (CurrentDestination == null && Actions[Phase] == StudentActionType.Sleuth)
				{
					GetSleuthTarget();
				}
				if (Actions[Phase] == StudentActionType.Follow)
				{
					if (DistanceToDestination > 5f)
					{
						Pathfinding.speed = 5f;
					}
					else
					{
						Pathfinding.speed = 1f;
					}
				}
				if (Actions[Phase] == StudentActionType.Follow && (Actions[Phase] != StudentActionType.Follow || !(DistanceToDestination > TargetDistance + 0.1f)))
				{
					return;
				}
				if (((Clock.Period == 1 && Clock.HourTime > 8.483334f) || (Clock.Period == 3 && Clock.HourTime > 13.483334f)) && !Teacher)
				{
					Pathfinding.speed = 4f;
				}
				if (!InEvent && !Meeting)
				{
					if (DressCode)
					{
						if (Actions[Phase] == StudentActionType.ClubAction)
						{
							if (!ClubAttire)
							{
								if (!ChangingBooth.Occupied)
								{
									CurrentDestination = ChangingBooth.transform;
									Pathfinding.target = ChangingBooth.transform;
								}
								else
								{
									CurrentDestination = ChangingBooth.WaitSpots[ClubMemberID];
									Pathfinding.target = ChangingBooth.WaitSpots[ClubMemberID];
								}
							}
							else if (Indoors && !GoAway)
							{
								CurrentDestination = Destinations[Phase];
								Pathfinding.target = Destinations[Phase];
								DistanceToDestination = 100f;
							}
						}
						else if (ClubAttire)
						{
							if (!ChangingBooth.Occupied)
							{
								CurrentDestination = ChangingBooth.transform;
								Pathfinding.target = ChangingBooth.transform;
							}
							else
							{
								CurrentDestination = ChangingBooth.WaitSpots[ClubMemberID];
								Pathfinding.target = ChangingBooth.WaitSpots[ClubMemberID];
							}
						}
						else if (Indoors && Actions[Phase] != StudentActionType.Clean && Actions[Phase] != StudentActionType.Sketch)
						{
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
						}
					}
					else if (Actions[Phase] == StudentActionType.SitAndTakeNotes && Schoolwear > 1 && !SchoolwearUnavailable)
					{
						CurrentDestination = StudentManager.FemaleStripSpot;
						Pathfinding.target = StudentManager.FemaleStripSpot;
					}
				}
				if (!Pathfinding.canMove)
				{
					if (!Spawned)
					{
						base.transform.position = StudentManager.SpawnPositions[StudentID].position;
						Spawned = true;
					}
					if (!Paired && !Alarmed)
					{
						Pathfinding.canSearch = true;
						Pathfinding.canMove = true;
						Obstacle.enabled = false;
					}
				}
				if (Pathfinding.speed > 0f)
				{
					if (Pathfinding.speed == 1f)
					{
						if (!CharacterAnimation.IsPlaying(WalkAnim))
						{
							if (Persona == PersonaType.PhoneAddict && Actions[Phase] == StudentActionType.Clean)
							{
								CharacterAnimation.CrossFade(OriginalWalkAnim);
							}
							else
							{
								CharacterAnimation.CrossFade(WalkAnim);
							}
						}
					}
					else if (!Dying)
					{
						CharacterAnimation.CrossFade(SprintAnim);
					}
				}
				if (Club == ClubType.Occult && Actions[Phase] != StudentActionType.ClubAction)
				{
					OccultBook.SetActive(false);
				}
				if (Meeting || GoAway || InEvent)
				{
					return;
				}
				if (Actions[Phase] == StudentActionType.Clean)
				{
					if (SmartPhone.activeInHierarchy)
					{
						SmartPhone.SetActive(false);
					}
					if (CurrentDestination != CleaningSpot.GetChild(CleanID))
					{
						CurrentDestination = CleaningSpot.GetChild(CleanID);
						Pathfinding.target = CurrentDestination;
					}
				}
				if (Actions[Phase] == StudentActionType.Patrol && CurrentDestination != StudentManager.Patrols.List[StudentID].GetChild(PatrolID))
				{
					CurrentDestination = StudentManager.Patrols.List[StudentID].GetChild(PatrolID);
					Pathfinding.target = CurrentDestination;
				}
				return;
			}
			if (CurrentDestination != null)
			{
				bool flag = false;
				if ((Actions[Phase] == StudentActionType.Sleuth && StudentManager.SleuthPhase == 3) || Actions[Phase] == StudentActionType.Stalk)
				{
					flag = true;
				}
				if (Actions[Phase] == StudentActionType.Follow)
				{
					FollowTargetDistance = Vector3.Distance(FollowTarget.transform.position, StudentManager.Hangouts.List[StudentID].transform.position);
					if (FollowTargetDistance < 1f && !FollowTarget.Alone)
					{
						MoveTowardsTarget(StudentManager.Hangouts.List[StudentID].position);
						float num = Quaternion.Angle(base.transform.rotation, StudentManager.Hangouts.List[StudentID].rotation);
						if (num > 1f && !StopRotating)
						{
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, StudentManager.Hangouts.List[StudentID].rotation, 10f * Time.deltaTime);
						}
					}
					else
					{
						targetRotation = Quaternion.LookRotation(FollowTarget.transform.position - base.transform.position);
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					}
				}
				else if (!flag)
				{
					MoveTowardsTarget(CurrentDestination.position);
					float num2 = Quaternion.Angle(base.transform.rotation, CurrentDestination.rotation);
					if (num2 > 1f && !StopRotating)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, 10f * Time.deltaTime);
					}
				}
				else
				{
					targetRotation = Quaternion.LookRotation(SleuthTarget.position - base.transform.position);
					float num3 = Quaternion.Angle(base.transform.rotation, targetRotation);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					if (num3 > 1f)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					}
				}
				if (!Hurry)
				{
					Pathfinding.speed = 1f;
				}
				else
				{
					Pathfinding.speed = 4f;
				}
			}
			if (Pathfinding.canMove)
			{
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				Obstacle.enabled = true;
			}
			if (!InEvent && !Meeting && DressCode)
			{
				if (Actions[Phase] == StudentActionType.ClubAction)
				{
					if (!ClubAttire)
					{
						if (!ChangingBooth.Occupied)
						{
							if (CurrentDestination == ChangingBooth.transform)
							{
								ChangingBooth.Occupied = true;
								ChangingBooth.Student = this;
								ChangingBooth.CheckYandereClub();
								Obstacle.enabled = false;
							}
							CurrentDestination = ChangingBooth.transform;
							Pathfinding.target = ChangingBooth.transform;
						}
						else
						{
							CharacterAnimation.CrossFade(IdleAnim);
						}
					}
					else if (!GoAway)
					{
						CurrentDestination = Destinations[Phase];
						Pathfinding.target = Destinations[Phase];
					}
				}
				else if (ClubAttire)
				{
					if (!ChangingBooth.Occupied)
					{
						if (CurrentDestination == ChangingBooth.transform)
						{
							ChangingBooth.Occupied = true;
							ChangingBooth.Student = this;
							ChangingBooth.CheckYandereClub();
						}
						CurrentDestination = ChangingBooth.transform;
						Pathfinding.target = ChangingBooth.transform;
					}
					else
					{
						CharacterAnimation.CrossFade(IdleAnim);
					}
				}
				else if (Actions[Phase] != StudentActionType.Clean)
				{
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
				}
			}
			if (InEvent)
			{
				return;
			}
			if (!Meeting)
			{
				if (!GoAway)
				{
					if (Actions[Phase] == StudentActionType.AtLocker)
					{
						CharacterAnimation.CrossFade(IdleAnim);
						Pathfinding.canSearch = false;
						Pathfinding.canMove = false;
					}
					else if (Actions[Phase] == StudentActionType.Socializing || (Actions[Phase] == StudentActionType.Follow && FollowTargetDistance < 1f && !FollowTarget.Alone && !FollowTarget.InEvent))
					{
						if (MyPlate != null && MyPlate.parent == RightHand)
						{
							MyPlate.parent = null;
							MyPlate.position = OriginalPlatePosition;
							MyPlate.rotation = OriginalPlateRotation;
							IdleAnim = OriginalIdleAnim;
							WalkAnim = OriginalWalkAnim;
							LeanAnim = OriginalLeanAnim;
							ResumeDistracting = false;
							Distracting = false;
							Distracted = false;
							CanTalk = true;
						}
						if (Paranoia > 1.66666f && !StudentManager.LoveSick && Club != ClubType.Delinquent)
						{
							CharacterAnimation.CrossFade(IdleAnim);
							return;
						}
						StudentManager.ConvoManager.CheckMe(StudentID);
						if (Club == ClubType.Delinquent)
						{
							if (Alone)
							{
								if (TrueAlone)
								{
									if (!SmartPhone.activeInHierarchy)
									{
										CharacterAnimation.CrossFade("delinquentTexting_00");
										SmartPhone.SetActive(true);
										SpeechLines.Stop();
									}
								}
								else
								{
									CharacterAnimation.CrossFade(IdleAnim);
									SpeechLines.Stop();
								}
								return;
							}
							if (!InEvent)
							{
								if (!Grudge)
								{
									if (!SpeechLines.isPlaying)
									{
										SmartPhone.SetActive(false);
										SpeechLines.Play();
									}
								}
								else
								{
									SmartPhone.SetActive(false);
								}
							}
							CharacterAnimation.CrossFade(RandomAnim);
							if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
							{
								PickRandomAnim();
							}
							return;
						}
						if (Alone)
						{
							if (!Male)
							{
								CharacterAnimation.CrossFade("f02_standTexting_00");
							}
							else if (StudentID == 36)
							{
								CharacterAnimation.CrossFade(ClubAnim);
							}
							else if (StudentID == 66)
							{
								CharacterAnimation.CrossFade("delinquentTexting_00");
							}
							else
							{
								CharacterAnimation.CrossFade("standTexting_00");
							}
							if (!SmartPhone.activeInHierarchy && !Shy)
							{
								if (StudentID == 36)
								{
									SmartPhone.transform.localPosition = new Vector3(0.0566666f, -0.02f, 0f);
									SmartPhone.transform.localEulerAngles = new Vector3(10f, 115f, 180f);
								}
								else
								{
									SmartPhone.transform.localPosition = new Vector3(0.015f, 0.01f, 0.025f);
									SmartPhone.transform.localEulerAngles = new Vector3(10f, -160f, 165f);
								}
								SmartPhone.SetActive(true);
								SpeechLines.Stop();
							}
							return;
						}
						if (!InEvent)
						{
							if (!Grudge)
							{
								if (!SpeechLines.isPlaying)
								{
									SmartPhone.SetActive(false);
									SpeechLines.Play();
								}
							}
							else
							{
								SmartPhone.SetActive(false);
							}
						}
						if (Club != ClubType.Photography)
						{
							CharacterAnimation.CrossFade(RandomAnim);
							if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
							{
								PickRandomAnim();
							}
						}
						else
						{
							CharacterAnimation.CrossFade(RandomSleuthAnim);
							if (CharacterAnimation[RandomSleuthAnim].time >= CharacterAnimation[RandomSleuthAnim].length)
							{
								PickRandomSleuthAnim();
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Gossip)
					{
						if (Paranoia > 1.66666f && !StudentManager.LoveSick)
						{
							CharacterAnimation.CrossFade(IdleAnim);
							return;
						}
						StudentManager.ConvoManager.CheckMe(StudentID);
						if (Alone)
						{
							if (!Shy)
							{
								CharacterAnimation.CrossFade("f02_standTexting_00");
								if (!SmartPhone.activeInHierarchy)
								{
									SmartPhone.SetActive(true);
									SpeechLines.Stop();
								}
							}
							return;
						}
						if (!SpeechLines.isPlaying)
						{
							SmartPhone.SetActive(false);
							SpeechLines.Play();
						}
						CharacterAnimation.CrossFade(RandomGossipAnim);
						if (CharacterAnimation[RandomGossipAnim].time >= CharacterAnimation[RandomGossipAnim].length)
						{
							PickRandomGossipAnim();
						}
					}
					else if (Actions[Phase] == StudentActionType.Gaming)
					{
						CharacterAnimation.CrossFade(GameAnim);
					}
					else if (Actions[Phase] == StudentActionType.Shamed)
					{
						CharacterAnimation.CrossFade(SadSitAnim);
					}
					else if (Actions[Phase] == StudentActionType.Slave)
					{
						CharacterAnimation.CrossFade(BrokenSitAnim);
						if (FragileSlave)
						{
							if (HuntTarget == null)
							{
								HuntTarget = this;
								GoCommitMurder();
							}
							else if (HuntTarget.Indoors)
							{
								GoCommitMurder();
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Relax)
					{
						CharacterAnimation.CrossFade(RelaxAnim);
					}
					else if (Actions[Phase] == StudentActionType.SitAndTakeNotes)
					{
						if (MyPlate != null && MyPlate.parent == RightHand)
						{
							MyPlate.parent = null;
							MyPlate.position = OriginalPlatePosition;
							MyPlate.rotation = OriginalPlateRotation;
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
							IdleAnim = OriginalIdleAnim;
							WalkAnim = OriginalWalkAnim;
							LeanAnim = OriginalLeanAnim;
							ResumeDistracting = false;
							Distracting = false;
							Distracted = false;
							CanTalk = true;
						}
						if (MustChangeClothing)
						{
							if (ChangeClothingPhase == 0)
							{
								if (StudentManager.CommunalLocker.Student == null)
								{
									StudentManager.CommunalLocker.Open = true;
									StudentManager.CommunalLocker.Student = this;
									StudentManager.CommunalLocker.SpawnSteam();
									Schoolwear = 1;
									ChangeClothingPhase++;
								}
								else
								{
									CharacterAnimation.CrossFade(IdleAnim);
								}
							}
							else if (ChangeClothingPhase == 1 && !StudentManager.CommunalLocker.SteamCountdown)
							{
								Pathfinding.target = Seat;
								CurrentDestination = Seat;
								StudentManager.CommunalLocker.Student = null;
								ChangeClothingPhase++;
								MustChangeClothing = false;
							}
							return;
						}
						if (Bullied)
						{
							if (SmartPhone.activeInHierarchy)
							{
								SmartPhone.SetActive(false);
							}
							CharacterAnimation.CrossFade(SadDeskSitAnim, 1f);
							return;
						}
						if (Rival && Phoneless && StudentManager.CommunalLocker.RivalPhone.gameObject.activeInHierarchy && !EndSearch && Yandere.CanMove)
						{
							CharacterAnimation.CrossFade(DiscoverPhoneAnim);
							Subtitle.UpdateLabel(LostPhoneSubtitleType, 2, 5f);
							EndSearch = true;
							Routine = false;
						}
						if (EndSearch)
						{
							return;
						}
						if (Clock.Period != 2 && Clock.Period != 4)
						{
							if (DressCode && ClubAttire)
							{
								CharacterAnimation.CrossFade(IdleAnim);
							}
							else
							{
								if (!((double)Vector3.Distance(base.transform.position, Seat.position) < 0.5))
								{
									return;
								}
								if (!Phoneless)
								{
									if (Club != ClubType.Delinquent)
									{
										if (!SmartPhone.activeInHierarchy)
										{
											if (Male)
											{
												SmartPhone.transform.localPosition = new Vector3(0.025f, 0.0025f, 0.025f);
												SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, 180f);
											}
											else
											{
												SmartPhone.transform.localPosition = new Vector3(0.01f, 0.01f, 0.01f);
												SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, 165f);
											}
											SmartPhone.SetActive(true);
										}
										CharacterAnimation.CrossFade(DeskTextAnim);
									}
									else
									{
										CharacterAnimation.CrossFade("delinquentSit_00");
									}
								}
								else
								{
									CharacterAnimation.CrossFade(SadDeskSitAnim);
								}
							}
						}
						else if (StudentManager.Teachers[Class].SpeechLines.isPlaying && !StudentManager.Teachers[Class].Alarmed)
						{
							if (DressCode && ClubAttire)
							{
								CharacterAnimation.CrossFade(IdleAnim);
								return;
							}
							if (!Depressed && !Pen.activeInHierarchy)
							{
								Pen.SetActive(true);
							}
							if (SmartPhone.activeInHierarchy)
							{
								SmartPhone.SetActive(false);
							}
							if (MyPaper == null)
							{
								if (base.transform.position.x < 0f)
								{
									MyPaper = UnityEngine.Object.Instantiate(Paper, Seat.position + new Vector3(-0.4f, 0.772f, 0f), Quaternion.identity);
								}
								else
								{
									MyPaper = UnityEngine.Object.Instantiate(Paper, Seat.position + new Vector3(0.4f, 0.772f, 0f), Quaternion.identity);
								}
								MyPaper.transform.eulerAngles = new Vector3(0f, 0f, -90f);
								MyPaper.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
								MyPaper.transform.parent = StudentManager.Papers;
							}
							CharacterAnimation.CrossFade(SitAnim);
						}
						else if (Club != ClubType.Delinquent)
						{
							CharacterAnimation.CrossFade(ConfusedSitAnim);
						}
						else
						{
							CharacterAnimation.CrossFade("delinquentSit_00");
						}
					}
					else if (Actions[Phase] == StudentActionType.Peek)
					{
						CharacterAnimation.CrossFade(PeekAnim);
						if (Male)
						{
							Cosmetic.MyRenderer.materials[Cosmetic.FaceID].SetFloat("_BlendAmount", 1f);
						}
					}
					else if (Actions[Phase] == StudentActionType.ClubAction)
					{
						if (DressCode && !ClubAttire)
						{
							CharacterAnimation.CrossFade(IdleAnim);
						}
						else
						{
							if (StudentID == 47 || StudentID == 49)
							{
								if (GetNewAnimation)
								{
									StudentManager.ConvoManager.MartialArtsCheck();
								}
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									GetNewAnimation = true;
								}
							}
							if (Club != ClubType.Occult)
							{
								CharacterAnimation.CrossFade(ClubAnim);
							}
						}
						if (Club == ClubType.Cooking)
						{
							if (ClubActivityPhase == 0)
							{
								ClubTimer += Time.deltaTime;
								if (ClubTimer > 60f)
								{
									MyPlate.parent = RightHand;
									MyPlate.localPosition = new Vector3(0.02f, -0.02f, -0.15f);
									MyPlate.localEulerAngles = new Vector3(-5f, -90f, 172.5f);
									IdleAnim = PlateIdleAnim;
									WalkAnim = PlateWalkAnim;
									LeanAnim = PlateIdleAnim;
									GetFoodTarget();
									ClubTimer = 0f;
									ClubActivityPhase++;
								}
							}
							else
							{
								GetFoodTarget();
							}
						}
						else if (Club == ClubType.Drama)
						{
							StudentManager.DramaTimer += Time.deltaTime;
							if (StudentManager.DramaPhase == 1)
							{
								StudentManager.ConvoManager.CheckMe(StudentID);
								if (Alone)
								{
									if (Phoneless)
									{
										CharacterAnimation.CrossFade("f02_sit_01");
									}
									else
									{
										if (Male)
										{
											CharacterAnimation.CrossFade("standTexting_00");
										}
										else
										{
											CharacterAnimation.CrossFade("f02_standTexting_00");
										}
										if (!SmartPhone.activeInHierarchy)
										{
											SmartPhone.transform.localPosition = new Vector3(0.02f, 0.01f, 0.03f);
											SmartPhone.transform.localEulerAngles = new Vector3(5f, -160f, 180f);
											SmartPhone.SetActive(true);
											SpeechLines.Stop();
										}
									}
								}
								else if (StudentID == 26 && !SpeechLines.isPlaying)
								{
									SmartPhone.SetActive(false);
									SpeechLines.Play();
								}
								if (StudentManager.DramaTimer > 100f)
								{
									StudentManager.DramaTimer = 0f;
									StudentManager.UpdateDrama();
								}
							}
							else if (StudentManager.DramaPhase == 2)
							{
								if (StudentManager.DramaTimer > 300f)
								{
									StudentManager.DramaTimer = 0f;
									StudentManager.UpdateDrama();
								}
							}
							else if (StudentManager.DramaPhase == 3)
							{
								SpeechLines.Play();
								CharacterAnimation.CrossFade(RandomAnim);
								if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
								{
									PickRandomAnim();
								}
								if (StudentManager.DramaTimer > 100f)
								{
									StudentManager.DramaTimer = 0f;
									StudentManager.UpdateDrama();
								}
							}
						}
						else if (Club == ClubType.Occult)
						{
							if (ReadPhase == 0)
							{
								CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
								CharacterAnimation.CrossFade(BookSitAnim);
								if (CharacterAnimation[BookSitAnim].time > CharacterAnimation[BookSitAnim].length)
								{
									CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
									CharacterAnimation.CrossFade(BookReadAnim);
									ReadPhase++;
								}
								else if (CharacterAnimation[BookSitAnim].time > 1f)
								{
									OccultBook.SetActive(true);
								}
							}
						}
						else if (Club == ClubType.Art)
						{
							if (ClubAttire && !Paintbrush.activeInHierarchy && (double)Vector3.Distance(base.transform.position, CurrentDestination.position) < 0.5)
							{
								Paintbrush.SetActive(true);
								Palette.SetActive(true);
							}
						}
						else if (Club == ClubType.LightMusic)
						{
							if ((double)Clock.HourTime < 16.9)
							{
								Instruments[ClubMemberID].SetActive(true);
								CharacterAnimation.CrossFade(ClubAnim);
								if (StudentID == 51)
								{
									if (InstrumentBag[ClubMemberID].transform.parent != null)
									{
										InstrumentBag[ClubMemberID].transform.parent = null;
										InstrumentBag[ClubMemberID].transform.position = new Vector3(0.5f, 4.5f, 22.45666f);
										InstrumentBag[ClubMemberID].transform.eulerAngles = new Vector3(-15f, 0f, 0f);
									}
									if (Instruments[ClubMemberID].transform.parent == null)
									{
										Instruments[ClubMemberID].GetComponent<AudioSource>().Play();
										Instruments[ClubMemberID].transform.parent = base.transform;
										Instruments[ClubMemberID].transform.localPosition = new Vector3(0.340493f, 0.653502f, -0.286104f);
										Instruments[ClubMemberID].transform.localEulerAngles = new Vector3(-13.6139f, 16.16775f, 72.5293f);
									}
								}
								else if (StudentID == 54 && !Drumsticks[0].activeInHierarchy)
								{
									Drumsticks[0].SetActive(true);
									Drumsticks[1].SetActive(true);
								}
							}
							else if (StudentID == 51)
							{
								InstrumentBag[ClubMemberID].transform.position = new Vector3(0.5f, 4.5f, 22.45666f);
								InstrumentBag[ClubMemberID].transform.eulerAngles = new Vector3(-15f, 0f, 0f);
								InstrumentBag[ClubMemberID].transform.parent = null;
								if (!StudentManager.PracticeMusic.isPlaying)
								{
									CharacterAnimation.CrossFade("f02_vocalIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 114.5f)
								{
									CharacterAnimation.CrossFade("f02_vocalCelebrate_00");
								}
								else if (StudentManager.PracticeMusic.time > 104f)
								{
									CharacterAnimation.CrossFade("f02_vocalWait_00");
								}
								else if (StudentManager.PracticeMusic.time > 32f)
								{
									CharacterAnimation.CrossFade("f02_vocalSingB_00");
								}
								else if (StudentManager.PracticeMusic.time > 24f)
								{
									CharacterAnimation.CrossFade("f02_vocalSingB_00");
								}
								else if (StudentManager.PracticeMusic.time > 17f)
								{
									CharacterAnimation.CrossFade("f02_vocalSingB_00");
								}
								else if (StudentManager.PracticeMusic.time > 14f)
								{
									CharacterAnimation.CrossFade("f02_vocalWait_00");
								}
								else if (StudentManager.PracticeMusic.time > 8f)
								{
									CharacterAnimation.CrossFade("f02_vocalSingA_00");
								}
								else if (StudentManager.PracticeMusic.time > 0f)
								{
									CharacterAnimation.CrossFade("f02_vocalWait_00");
								}
							}
							else if (StudentID == 52)
							{
								if (!Instruments[ClubMemberID].activeInHierarchy)
								{
									Instruments[ClubMemberID].SetActive(true);
									Instruments[ClubMemberID].GetComponent<AudioSource>().Stop();
									Instruments[ClubMemberID].GetComponent<AudioSource>().playOnAwake = false;
									Instruments[ClubMemberID].transform.parent = Spine;
									Instruments[ClubMemberID].transform.localPosition = new Vector3(0.275f, -0.16f, 0.095f);
									Instruments[ClubMemberID].transform.localEulerAngles = new Vector3(-22.5f, 30f, 60f);
									InstrumentBag[ClubMemberID].transform.parent = null;
									InstrumentBag[ClubMemberID].transform.position = new Vector3(5.5825f, 4.01f, 25f);
									InstrumentBag[ClubMemberID].transform.eulerAngles = new Vector3(-15f, -90f, 0f);
								}
								if (!StudentManager.PracticeMusic.isPlaying)
								{
									CharacterAnimation.CrossFade("f02_guitarIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 114.5f)
								{
									CharacterAnimation.CrossFade("f02_guitarCelebrate_00");
								}
								else if (StudentManager.PracticeMusic.time > 112f)
								{
									CharacterAnimation.CrossFade("f02_guitarWait_00");
								}
								else if (StudentManager.PracticeMusic.time > 64f)
								{
									CharacterAnimation.CrossFade("f02_guitarPlayA_01");
								}
								else if (StudentManager.PracticeMusic.time > 8f)
								{
									CharacterAnimation.CrossFade("f02_guitarPlayA_00");
								}
								else if (StudentManager.PracticeMusic.time > 0f)
								{
									CharacterAnimation.CrossFade("f02_guitarWait_00");
								}
							}
							else if (StudentID == 53)
							{
								if (!Instruments[ClubMemberID].activeInHierarchy)
								{
									Instruments[ClubMemberID].SetActive(true);
									Instruments[ClubMemberID].GetComponent<AudioSource>().Stop();
									Instruments[ClubMemberID].GetComponent<AudioSource>().playOnAwake = false;
									Instruments[ClubMemberID].transform.parent = Spine;
									Instruments[ClubMemberID].transform.localPosition = new Vector3(0.275f, -0.16f, 0.095f);
									Instruments[ClubMemberID].transform.localEulerAngles = new Vector3(-22.5f, 30f, 60f);
									InstrumentBag[ClubMemberID].transform.parent = null;
									InstrumentBag[ClubMemberID].transform.position = new Vector3(5.5825f, 4.01f, 26f);
									InstrumentBag[ClubMemberID].transform.eulerAngles = new Vector3(-15f, -90f, 0f);
								}
								if (!StudentManager.PracticeMusic.isPlaying)
								{
									CharacterAnimation.CrossFade("f02_guitarIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 114.5f)
								{
									CharacterAnimation.CrossFade("f02_guitarCelebrate_00");
								}
								else if (StudentManager.PracticeMusic.time > 112f)
								{
									CharacterAnimation.CrossFade("f02_guitarWait_00");
								}
								else if (StudentManager.PracticeMusic.time > 88f)
								{
									CharacterAnimation.CrossFade("f02_guitarPlayA_00");
								}
								else if (StudentManager.PracticeMusic.time > 80f)
								{
									CharacterAnimation.CrossFade("f02_guitarWait_00");
								}
								else if (StudentManager.PracticeMusic.time > 64f)
								{
									CharacterAnimation.CrossFade("f02_guitarPlayB_00");
								}
								else if (StudentManager.PracticeMusic.time > 0f)
								{
									CharacterAnimation.CrossFade("f02_guitarPlaySlowA_01");
								}
							}
							else if (StudentID == 54)
							{
								if (InstrumentBag[ClubMemberID].transform.parent != null)
								{
									InstrumentBag[ClubMemberID].transform.parent = null;
									InstrumentBag[ClubMemberID].transform.position = new Vector3(5.5825f, 4.01f, 23f);
									InstrumentBag[ClubMemberID].transform.eulerAngles = new Vector3(-15f, -90f, 0f);
								}
								Drumsticks[0].SetActive(true);
								Drumsticks[1].SetActive(true);
								if (!StudentManager.PracticeMusic.isPlaying)
								{
									CharacterAnimation.CrossFade("f02_drumsIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 114.5f)
								{
									CharacterAnimation.CrossFade("f02_drumsCelebrate_00");
								}
								else if (StudentManager.PracticeMusic.time > 108f)
								{
									CharacterAnimation.CrossFade("f02_drumsIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 96f)
								{
									CharacterAnimation.CrossFade("f02_drumsPlaySlow_00");
								}
								else if (StudentManager.PracticeMusic.time > 80f)
								{
									CharacterAnimation.CrossFade("f02_drumsIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 38f)
								{
									CharacterAnimation.CrossFade("f02_drumsPlay_00");
								}
								else if (StudentManager.PracticeMusic.time > 46f)
								{
									CharacterAnimation.CrossFade("f02_drumsIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 16f)
								{
									CharacterAnimation.CrossFade("f02_drumsPlay_00");
								}
								else if (StudentManager.PracticeMusic.time > 0f)
								{
									CharacterAnimation.CrossFade("f02_drumsIdle_00");
								}
							}
							else if (StudentID == 55)
							{
								if (InstrumentBag[ClubMemberID].transform.parent != null)
								{
									InstrumentBag[ClubMemberID].transform.parent = null;
									InstrumentBag[ClubMemberID].transform.position = new Vector3(5.5825f, 4.01f, 24f);
									InstrumentBag[ClubMemberID].transform.eulerAngles = new Vector3(-15f, -90f, 0f);
								}
								if (!StudentManager.PracticeMusic.isPlaying)
								{
									CharacterAnimation.CrossFade("f02_keysIdle_00");
								}
								else if (StudentManager.PracticeMusic.time > 114.5f)
								{
									CharacterAnimation.CrossFade("f02_keysCelebrate_00");
								}
								else if (StudentManager.PracticeMusic.time > 80f)
								{
									CharacterAnimation.CrossFade("f02_keysWait_00");
								}
								else if (StudentManager.PracticeMusic.time > 24f)
								{
									CharacterAnimation.CrossFade("f02_keysPlay_00");
								}
								else if (StudentManager.PracticeMusic.time > 0f)
								{
									CharacterAnimation.CrossFade("f02_keysWait_00");
								}
							}
						}
						else if (Club == ClubType.Science)
						{
							if (ClubAttire && !GoAway)
							{
								if (SciencePhase == 0)
								{
									CharacterAnimation.CrossFade(ClubAnim);
								}
								else
								{
									CharacterAnimation.CrossFade(RummageAnim);
								}
								if (!((double)Vector3.Distance(base.transform.position, CurrentDestination.position) < 0.5))
								{
									return;
								}
								if (SciencePhase == 0)
								{
									if (StudentID == 62)
									{
										ScienceProps[0].SetActive(true);
									}
									else if (StudentID == 63)
									{
										ScienceProps[1].SetActive(true);
										ScienceProps[2].SetActive(true);
									}
									else if (StudentID == 64)
									{
										ScienceProps[0].SetActive(true);
									}
								}
								if (StudentID <= 61)
								{
									return;
								}
								ClubTimer += Time.deltaTime;
								if (!(ClubTimer > 60f))
								{
									return;
								}
								ClubTimer = 0f;
								SciencePhase++;
								if (SciencePhase == 1)
								{
									ClubTimer = 50f;
									OriginalPosition = CurrentDestination.transform.position;
									OriginalRotation = CurrentDestination.transform.rotation;
									CurrentDestination.transform.position = StudentManager.SupplySpots[StudentID - 61].position;
									Pathfinding.target.position = StudentManager.SupplySpots[StudentID - 61].position;
									CurrentDestination.transform.rotation = StudentManager.SupplySpots[StudentID - 61].rotation;
									Pathfinding.target.rotation = StudentManager.SupplySpots[StudentID - 61].rotation;
									GameObject[] scienceProps2 = ScienceProps;
									foreach (GameObject gameObject3 in scienceProps2)
									{
										if (gameObject3 != null)
										{
											gameObject3.SetActive(false);
										}
									}
								}
								else
								{
									SciencePhase = 0;
									ClubTimer = 0f;
									CurrentDestination.transform.position = OriginalPosition;
									Pathfinding.target.position = OriginalPosition;
									CurrentDestination.transform.rotation = OriginalRotation;
									Pathfinding.target.rotation = OriginalRotation;
								}
							}
							else
							{
								CharacterAnimation.CrossFade(IdleAnim);
							}
						}
						else if (Club == ClubType.Sports)
						{
							CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
							if (ClubActivityPhase == 0)
							{
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									ClubActivityPhase++;
									ClubAnim = "stretch_01";
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
								}
							}
							else if (ClubActivityPhase == 1)
							{
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									ClubActivityPhase++;
									ClubAnim = "stretch_02";
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
								}
							}
							else if (ClubActivityPhase == 2)
							{
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									WalkAnim = "trackJog_00";
									Hurry = true;
									ClubActivityPhase++;
									CharacterAnimation[ClubAnim].time = 0f;
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
								}
							}
							else if (ClubActivityPhase < 14)
							{
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									ClubActivityPhase++;
									CharacterAnimation[ClubAnim].time = 0f;
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
								}
							}
							else if (ClubActivityPhase == 14)
							{
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									WalkAnim = OriginalWalkAnim;
									Hurry = false;
									ClubActivityPhase = 0;
									ClubAnim = "stretch_00";
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
								}
							}
							else if (ClubActivityPhase == 15)
							{
								if (CharacterAnimation[ClubAnim].time >= 1f && MyController.radius > 0f)
								{
									MyRenderer.updateWhenOffscreen = true;
									Obstacle.enabled = false;
									MyController.radius = 0f;
									Distracted = true;
								}
								if (CharacterAnimation[ClubAnim].time >= 2f)
								{
									float value = Cosmetic.Goggles[StudentID].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) + Time.deltaTime * 200f;
									Cosmetic.Goggles[StudentID].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, value);
								}
								if (CharacterAnimation[ClubAnim].time >= 5f)
								{
									ClubActivityPhase++;
								}
							}
							else if (ClubActivityPhase == 16)
							{
								if ((double)CharacterAnimation[ClubAnim].time >= 6.1)
								{
									Cosmetic.Goggles[StudentID].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100f);
									Cosmetic.MaleHair[Cosmetic.Hairstyle].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100f);
									GameObject gameObject4 = UnityEngine.Object.Instantiate(BigWaterSplash, RightHand.transform.position, Quaternion.identity);
									gameObject4.transform.eulerAngles = new Vector3(-90f, gameObject4.transform.eulerAngles.y, gameObject4.transform.eulerAngles.z);
									SetSplashes(true);
									ClubActivityPhase++;
								}
							}
							else if (ClubActivityPhase == 17)
							{
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									WalkAnim = "poolSwim_00";
									ClubAnim = "poolSwim_00";
									ClubActivityPhase++;
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase - 2);
									base.transform.position = Hips.transform.position;
									Character.transform.localPosition = new Vector3(0f, -0.25f, 0f);
									Physics.SyncTransforms();
									CharacterAnimation.Play(WalkAnim);
								}
							}
							else if (ClubActivityPhase == 18)
							{
								ClubActivityPhase++;
								Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase - 2);
								DistanceToDestination = 100f;
							}
							else
							{
								if (ClubActivityPhase != 19)
								{
									return;
								}
								ClubAnim = "poolExit_00";
								if (CharacterAnimation[ClubAnim].time >= 0.1f)
								{
									SetSplashes(false);
								}
								if (CharacterAnimation[ClubAnim].time >= 4.66666f)
								{
									float value2 = Cosmetic.Goggles[StudentID].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) - Time.deltaTime * 200f;
									Cosmetic.Goggles[StudentID].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, value2);
								}
								if (CharacterAnimation[ClubAnim].time >= CharacterAnimation[ClubAnim].length)
								{
									ClubActivityPhase = 15;
									ClubAnim = "poolDive_00";
									MyController.radius = 0.1f;
									WalkAnim = OriginalWalkAnim;
									MyRenderer.updateWhenOffscreen = false;
									Character.transform.localPosition = new Vector3(0f, 0f, 0f);
									Cosmetic.Goggles[StudentID].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
									Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
									base.transform.position = new Vector3(Hips.position.x, 4f, Hips.position.z);
									Physics.SyncTransforms();
									CharacterAnimation.Play(IdleAnim);
									Distracted = false;
									if (Clock.Period == 2 || Clock.Period == 4)
									{
										Pathfinding.speed = 4f;
									}
								}
							}
						}
						else if (Club == ClubType.Gardening)
						{
							if (WateringCan.transform.parent != RightHand)
							{
								WateringCan.transform.parent = RightHand;
								WateringCan.transform.localPosition = new Vector3(0.14f, -0.15f, -0.05f);
								WateringCan.transform.localEulerAngles = new Vector3(10f, 15f, 45f);
							}
							PatrolTimer += Time.deltaTime * CharacterAnimation[PatrolAnim].speed;
							if (PatrolTimer >= CharacterAnimation[ClubAnim].length)
							{
								PatrolID++;
								if (PatrolID == StudentManager.Patrols.List[StudentID].childCount)
								{
									PatrolID = 0;
								}
								CurrentDestination = StudentManager.Patrols.List[StudentID].GetChild(PatrolID);
								Pathfinding.target = CurrentDestination;
								PatrolTimer = 0f;
								WateringCan.transform.parent = Hips;
								WateringCan.transform.localPosition = new Vector3(0f, 0.0135f, -0.184f);
								WateringCan.transform.localEulerAngles = new Vector3(0f, 90f, 30f);
							}
						}
						else
						{
							if (Club != ClubType.Gaming)
							{
								return;
							}
							if (Phase < 8)
							{
								if (StudentID == 36 && !SmartPhone.activeInHierarchy)
								{
									SmartPhone.SetActive(true);
									SmartPhone.transform.localPosition = new Vector3(0.0566666f, -0.02f, 0f);
									SmartPhone.transform.localEulerAngles = new Vector3(10f, 115f, 180f);
								}
								return;
							}
							if (!ClubManager.GameScreens[ClubMemberID].activeInHierarchy)
							{
								ClubManager.GameScreens[ClubMemberID].SetActive(true);
							}
							if (SmartPhone.activeInHierarchy)
							{
								SmartPhone.SetActive(false);
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.SitAndSocialize)
					{
						if (Paranoia > 1.66666f)
						{
							CharacterAnimation.CrossFade(IdleAnim);
							return;
						}
						StudentManager.ConvoManager.CheckMe(StudentID);
						if (Alone)
						{
							if (!Male)
							{
								CharacterAnimation.CrossFade("f02_standTexting_00");
							}
							else
							{
								CharacterAnimation.CrossFade("standTexting_00");
							}
							if (!SmartPhone.activeInHierarchy)
							{
								SmartPhone.SetActive(true);
								SpeechLines.Stop();
							}
							return;
						}
						if (!InEvent && !SpeechLines.isPlaying)
						{
							SmartPhone.SetActive(false);
							SpeechLines.Play();
						}
						if (Club != ClubType.Photography)
						{
							CharacterAnimation.CrossFade(RandomAnim);
							if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
							{
								PickRandomAnim();
							}
						}
						else
						{
							CharacterAnimation.CrossFade(RandomSleuthAnim);
							if (CharacterAnimation[RandomSleuthAnim].time >= CharacterAnimation[RandomSleuthAnim].length)
							{
								PickRandomSleuthAnim();
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.SitAndEatBento)
					{
						if (!Ragdoll.Poisoned && (!Bento.activeInHierarchy || Bento.transform.parent == null))
						{
							SmartPhone.SetActive(false);
							if (!Male)
							{
								Bento.transform.parent = LeftItemParent;
								Bento.transform.localPosition = new Vector3(-0.025f, -0.105f, 0f);
								Bento.transform.localEulerAngles = new Vector3(0f, 165f, 82.5f);
							}
							else
							{
								Bento.transform.parent = LeftItemParent;
								Bento.transform.localPosition = new Vector3(-0.05f, -0.085f, 0f);
								Bento.transform.localEulerAngles = new Vector3(-3.2f, -24.4f, -94f);
							}
							Chopsticks[0].SetActive(true);
							Chopsticks[1].SetActive(true);
							Bento.SetActive(true);
							Lid.SetActive(false);
							MyBento.Prompt.Hide();
							MyBento.Prompt.enabled = false;
							if (MyBento.Tampered)
							{
								if (MyBento.Emetic)
								{
									Emetic = true;
								}
								else if (MyBento.Lethal)
								{
									Lethal = true;
								}
								else if (MyBento.Tranquil)
								{
									Sedated = true;
								}
								else if (MyBento.Headache)
								{
									Headache = true;
								}
							}
						}
						if (!Emetic && !Lethal && !Sedated && !Headache)
						{
							CharacterAnimation.CrossFade(EatAnim);
							if (FollowTarget != null && (double)FollowTarget.transform.position.x > 6.5)
							{
								ScheduleBlock scheduleBlock3 = ScheduleBlocks[4];
								scheduleBlock3.destination = "Follow";
								scheduleBlock3.action = "Follow";
								GetDestinations();
							}
						}
						else if (Emetic)
						{
							if (!Private)
							{
								CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
								CharacterAnimation.CrossFade(EmeticAnim);
								Private = true;
								CanTalk = false;
							}
							if (CharacterAnimation[EmeticAnim].time >= CharacterAnimation[EmeticAnim].length)
							{
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								if (Male)
								{
									WalkAnim = "stomachPainWalk_00";
									StudentManager.GetMaleVomitSpot(this);
									Pathfinding.target = StudentManager.MaleVomitSpot;
									CurrentDestination = StudentManager.MaleVomitSpot;
								}
								else
								{
									WalkAnim = "f02_stomachPainWalk_00";
									StudentManager.GetFemaleVomitSpot(this);
									Pathfinding.target = StudentManager.FemaleVomitSpot;
									CurrentDestination = StudentManager.FemaleVomitSpot;
								}
								if (StudentID == 6)
								{
									Pathfinding.target = StudentManager.AltFemaleVomitSpot;
									CurrentDestination = StudentManager.AltFemaleVomitSpot;
								}
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								CharacterAnimation.CrossFade(WalkAnim);
								DistanceToDestination = 100f;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								Pathfinding.speed = 2f;
								MyBento.Tampered = false;
								Vomiting = true;
								Routine = false;
								Chopsticks[0].SetActive(false);
								Chopsticks[1].SetActive(false);
								Bento.SetActive(false);
							}
						}
						else if (Lethal)
						{
							if (!Private)
							{
								AudioSource component = GetComponent<AudioSource>();
								component.clip = PoisonDeathClip;
								component.Play();
								if (StudentManager.Students[11] != null)
								{
									StudentManager.Students[1].CharacterAnimation.CrossFade("witnessPoisoning_00");
									StudentManager.Students[1].Distracted = true;
									StudentManager.Students[1].Routine = false;
								}
								if (Male)
								{
									CharacterAnimation.CrossFade("poisonDeath_00");
									PoisonDeathAnim = "poisonDeath_00";
								}
								else
								{
									CharacterAnimation.CrossFade("f02_poisonDeath_00");
									PoisonDeathAnim = "f02_poisonDeath_00";
								}
								CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
								Ragdoll.Poisoned = true;
								Private = true;
								Prompt.Hide();
								Prompt.enabled = false;
							}
							if (!Distracted && CharacterAnimation[PoisonDeathAnim].time >= 2.5f)
							{
								Distracted = true;
							}
							if (CharacterAnimation[PoisonDeathAnim].time >= 17.5f && Bento.activeInHierarchy)
							{
								Police.CorpseList[Police.Corpses] = Ragdoll;
								Police.Corpses++;
								GameObjectUtils.SetLayerRecursively(base.gameObject, 11);
								base.tag = "Blood";
								Ragdoll.ChokingAnimation = true;
								Ragdoll.Disturbing = true;
								Ragdoll.Choking = true;
								Dying = true;
								MurderSuicidePhase = 100;
								SpawnAlarmDisc();
								if (StudentManager.Students[11] != null)
								{
									StudentManager.Students[1].Chopsticks[0].SetActive(false);
									StudentManager.Students[1].Chopsticks[1].SetActive(false);
									StudentManager.Students[1].Bento.SetActive(false);
								}
								Chopsticks[0].SetActive(false);
								Chopsticks[1].SetActive(false);
								Bento.SetActive(false);
							}
							if (CharacterAnimation[PoisonDeathAnim].time >= CharacterAnimation[PoisonDeathAnim].length)
							{
								BecomeRagdoll();
								DeathType = DeathType.Poison;
								Ragdoll.Choking = false;
							}
						}
						else if (Sedated)
						{
							if (!Private)
							{
								CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
								CharacterAnimation.CrossFade(HeadacheAnim);
								CanTalk = false;
								Private = true;
							}
							if (CharacterAnimation[HeadacheAnim].time >= CharacterAnimation[HeadacheAnim].length)
							{
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								if (Male)
								{
									SprintAnim = "headacheWalk_00";
									RelaxAnim = "infirmaryRest_00";
								}
								else
								{
									SprintAnim = "f02_headacheWalk_00";
									RelaxAnim = "f02_infirmaryRest_00";
								}
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								CharacterAnimation.CrossFade(SprintAnim);
								DistanceToDestination = 100f;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								Pathfinding.speed = 2f;
								MyBento.Tampered = false;
								Distracted = true;
								Private = true;
								ScheduleBlock scheduleBlock4 = ScheduleBlocks[4];
								scheduleBlock4.destination = "InfirmaryBed";
								scheduleBlock4.action = "Relax";
								GetDestinations();
								CurrentDestination = Destinations[Phase];
								Pathfinding.target = Destinations[Phase];
								Chopsticks[0].SetActive(false);
								Chopsticks[1].SetActive(false);
								Bento.SetActive(false);
							}
						}
						else
						{
							if (!Headache)
							{
								return;
							}
							if (!Private)
							{
								CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
								CharacterAnimation.CrossFade(HeadacheAnim);
								CanTalk = false;
								Private = true;
							}
							if (CharacterAnimation[HeadacheAnim].time >= CharacterAnimation[HeadacheAnim].length)
							{
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								if (Male)
								{
									SprintAnim = "headacheWalk_00";
									IdleAnim = "headacheIdle_00";
								}
								else
								{
									SprintAnim = "f02_headacheWalk_00";
									IdleAnim = "f02_headacheIdle_00";
								}
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								CharacterAnimation.CrossFade(SprintAnim);
								DistanceToDestination = 100f;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								Pathfinding.speed = 2f;
								MyBento.Tampered = false;
								SeekingMedicine = true;
								Routine = false;
								Private = true;
								Chopsticks[0].SetActive(false);
								Chopsticks[1].SetActive(false);
								Bento.SetActive(false);
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.ChangeShoes)
					{
						if (MeetTime == 0f)
						{
							if (StudentManager.LoveManager.Suitor == this && StudentManager.LoveManager.LeftNote)
							{
								CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
								CharacterAnimation.CrossFade("keepNote_00");
								ShoeRemoval.Locker.GetComponent<Animation>().CrossFade("lockerKeepNote");
								Pathfinding.canSearch = false;
								Pathfinding.canMove = false;
								Confessing = true;
								CanTalk = false;
								Routine = false;
							}
							else
							{
								SmartPhone.SetActive(false);
								Pathfinding.canSearch = false;
								Pathfinding.canMove = false;
								ShoeRemoval.enabled = true;
								CanTalk = false;
								Routine = false;
								ShoeRemoval.LeavingSchool();
							}
						}
						else
						{
							CharacterAnimation.CrossFade(IdleAnim);
						}
					}
					else if (Actions[Phase] == StudentActionType.GradePapers)
					{
						CharacterAnimation.CrossFade("f02_deskWrite");
						GradingPaper.Writing = true;
						Obstacle.enabled = true;
						Pen.SetActive(true);
					}
					else if (Actions[Phase] == StudentActionType.Patrol)
					{
						PatrolTimer += Time.deltaTime * CharacterAnimation[PatrolAnim].speed;
						CharacterAnimation.CrossFade(PatrolAnim);
						if (PatrolTimer >= CharacterAnimation[PatrolAnim].length)
						{
							PatrolID++;
							if (PatrolID == StudentManager.Patrols.List[StudentID].childCount)
							{
								PatrolID = 0;
							}
							CurrentDestination = StudentManager.Patrols.List[StudentID].GetChild(PatrolID);
							Pathfinding.target = CurrentDestination;
							if (StudentID == 39)
							{
								CharacterAnimation["f02_topHalfTexting_00"].weight = 1f;
							}
							PatrolTimer = 0f;
						}
					}
					else if (Actions[Phase] == StudentActionType.Read)
					{
						if (ReadPhase == 0)
						{
							CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
							CharacterAnimation.CrossFade(BookSitAnim);
							if (CharacterAnimation[BookSitAnim].time > CharacterAnimation[BookSitAnim].length)
							{
								CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
								CharacterAnimation.CrossFade(BookReadAnim);
								ReadPhase++;
							}
							else if (CharacterAnimation[BookSitAnim].time > 1f)
							{
								OccultBook.SetActive(true);
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Texting)
					{
						CharacterAnimation.CrossFade("f02_midoriTexting_00");
						if (SmartPhone.transform.localPosition.x != 0.02f)
						{
							SmartPhone.transform.localPosition = new Vector3(0.02f, -0.0075f, 0f);
							SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, -164f);
						}
						if (!SmartPhone.activeInHierarchy && base.transform.position.y > 11f)
						{
							SmartPhone.SetActive(true);
						}
					}
					else if (Actions[Phase] == StudentActionType.Mourn)
					{
						CharacterAnimation.CrossFade("f02_brokenSit_00");
					}
					else if (Actions[Phase] == StudentActionType.Cuddle)
					{
						if (Vector3.Distance(base.transform.position, Partner.transform.position) < 1f)
						{
							ParticleSystem.EmissionModule emission = Hearts.emission;
							if (!emission.enabled)
							{
								emission.enabled = true;
								if (!Male)
								{
									Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 1f);
								}
								else
								{
									Cosmetic.MyRenderer.materials[Cosmetic.FaceID].SetFloat("_BlendAmount", 1f);
								}
							}
							CharacterAnimation.CrossFade(CuddleAnim);
						}
						else
						{
							CharacterAnimation.CrossFade(IdleAnim);
						}
					}
					else if (Actions[Phase] == StudentActionType.Teaching)
					{
						if (Clock.Period != 2 && Clock.Period != 4)
						{
							CharacterAnimation.CrossFade("f02_teacherPodium_00");
							return;
						}
						if (!SpeechLines.isPlaying)
						{
							SpeechLines.Play();
						}
						CharacterAnimation.CrossFade(TeachAnim);
					}
					else if (Actions[Phase] == StudentActionType.SearchPatrol)
					{
						if (PatrolID == 0 && StudentManager.CommunalLocker.RivalPhone.gameObject.activeInHierarchy && !EndSearch)
						{
							CharacterAnimation.CrossFade(DiscoverPhoneAnim);
							Subtitle.UpdateLabel(LostPhoneSubtitleType, 2, 5f);
							EndSearch = true;
							Routine = false;
						}
						if (EndSearch)
						{
							return;
						}
						PatrolTimer += Time.deltaTime * CharacterAnimation[PatrolAnim].speed;
						CharacterAnimation.CrossFade(SearchPatrolAnim);
						if (PatrolTimer >= CharacterAnimation[SearchPatrolAnim].length)
						{
							PatrolID++;
							if (PatrolID == StudentManager.SearchPatrols.List[StudentID].childCount)
							{
								PatrolID = 0;
							}
							CurrentDestination = StudentManager.SearchPatrols.List[StudentID].GetChild(PatrolID);
							Pathfinding.target = CurrentDestination;
							DistanceToDestination = 100f;
							if (StudentID == 39)
							{
								CharacterAnimation["f02_topHalfTexting_00"].weight = 1f;
							}
							PatrolTimer = 0f;
						}
					}
					else if (Actions[Phase] == StudentActionType.Wait)
					{
						if (!Cigarette.active && TaskGlobals.GetTaskStatus(81) == 3)
						{
							WaitAnim = "f02_smokeAttempt_00";
							SmartPhone.SetActive(false);
							Cigarette.SetActive(true);
							Lighter.SetActive(true);
						}
						CharacterAnimation.CrossFade(WaitAnim);
					}
					else if (Actions[Phase] == StudentActionType.Clean)
					{
						CleanTimer += Time.deltaTime;
						if (CleaningRole == 5)
						{
							if (CleanID == 0)
							{
								CharacterAnimation.CrossFade(CleanAnims[1]);
							}
							else
							{
								Prompt.Label[0].text = "     Push";
								Prompt.HideButton[0] = false;
								Pushable = true;
								CharacterAnimation.CrossFade(CleanAnims[CleaningRole]);
								if ((double)CleanTimer >= 1.166666 && (double)CleanTimer <= 6.166666 && !ChalkDust.isPlaying)
								{
									ChalkDust.Play();
								}
							}
						}
						else if (CleaningRole == 4)
						{
							CharacterAnimation.CrossFade(CleanAnims[CleaningRole]);
							if (!Drownable)
							{
								Prompt.Label[0].text = "     Drown";
								Prompt.HideButton[0] = false;
								Drownable = true;
							}
						}
						else
						{
							CharacterAnimation.CrossFade(CleanAnims[CleaningRole]);
						}
						if (CleanTimer >= CharacterAnimation[CleanAnims[CleaningRole]].length)
						{
							CleanID++;
							if (CleanID == CleaningSpot.childCount)
							{
								CleanID = 0;
							}
							CurrentDestination = CleaningSpot.GetChild(CleanID);
							Pathfinding.target = CurrentDestination;
							DistanceToDestination = 100f;
							CleanTimer = 0f;
							if (Pushable)
							{
								Prompt.Label[0].text = "     Talk";
								Pushable = false;
							}
							if (Drownable)
							{
								Drownable = false;
								StudentManager.UpdateMe(StudentID);
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Graffiti)
					{
						if (KilledMood)
						{
							Subtitle.UpdateLabel(SubtitleType.KilledMood, 0, 5f);
							GraffitiPhase = 4;
							KilledMood = false;
						}
						if (GraffitiPhase == 0)
						{
							AudioSource.PlayClipAtPoint(BullyGiggles[UnityEngine.Random.Range(0, BullyGiggles.Length)], Head.position);
							CharacterAnimation.CrossFade("f02_bullyDesk_00");
							SmartPhone.SetActive(false);
							GraffitiPhase++;
						}
						else if (GraffitiPhase == 1)
						{
							if (CharacterAnimation["f02_bullyDesk_00"].time >= 8f)
							{
								StudentManager.Graffiti[BullyID].SetActive(true);
								GraffitiPhase++;
							}
						}
						else if (GraffitiPhase == 2)
						{
							if (CharacterAnimation["f02_bullyDesk_00"].time >= 9.66666f)
							{
								AudioSource.PlayClipAtPoint(BullyGiggles[UnityEngine.Random.Range(0, BullyGiggles.Length)], Head.position);
								GraffitiPhase++;
							}
						}
						else if (GraffitiPhase == 3)
						{
							if (CharacterAnimation["f02_bullyDesk_00"].time >= CharacterAnimation["f02_bullyDesk_00"].length)
							{
								GraffitiPhase++;
							}
						}
						else if (GraffitiPhase == 4)
						{
							DistanceToDestination = 100f;
							ScheduleBlock scheduleBlock5 = ScheduleBlocks[2];
							scheduleBlock5.destination = "Patrol";
							scheduleBlock5.action = "Patrol";
							GetDestinations();
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
							SmartPhone.SetActive(true);
						}
					}
					else if (Actions[Phase] == StudentActionType.Bully)
					{
						if (StudentManager.Students[StudentManager.VictimID] != null)
						{
							if (StudentManager.Students[StudentManager.VictimID].Distracted || StudentManager.Students[StudentManager.VictimID].Tranquil)
							{
								StudentManager.NoBully[BullyID] = true;
								KilledMood = true;
							}
							if (KilledMood)
							{
								Subtitle.UpdateLabel(SubtitleType.KilledMood, 0, 5f);
								BullyPhase = 4;
								KilledMood = false;
								BullyDust.Stop();
							}
							if (StudentManager.Students[81] == null)
							{
								ScheduleBlock scheduleBlock6 = ScheduleBlocks[4];
								scheduleBlock6.destination = "Patrol";
								scheduleBlock6.action = "Patrol";
								GetDestinations();
								CurrentDestination = Destinations[Phase];
								Pathfinding.target = Destinations[Phase];
								return;
							}
							SmartPhone.SetActive(false);
							if (BullyID == 1)
							{
								if (BullyPhase == 0)
								{
									Scrubber.GetComponent<Renderer>().material.mainTexture = Eraser.GetComponent<Renderer>().material.mainTexture;
									Scrubber.SetActive(true);
									Eraser.SetActive(true);
									StudentManager.Students[StudentManager.VictimID].CharacterAnimation.CrossFade(StudentManager.Students[StudentManager.VictimID].BullyVictimAnim);
									StudentManager.Students[StudentManager.VictimID].Routine = false;
									CharacterAnimation.CrossFade("f02_bullyEraser_00");
									BullyPhase++;
								}
								else if (BullyPhase == 1)
								{
									if (CharacterAnimation["f02_bullyEraser_00"].time >= 0.833333f)
									{
										BullyDust.Play();
										BullyPhase++;
									}
								}
								else if (BullyPhase == 2)
								{
									if (CharacterAnimation["f02_bullyEraser_00"].time >= CharacterAnimation["f02_bullyEraser_00"].length)
									{
										AudioSource.PlayClipAtPoint(BullyLaughs[BullyID], Head.position);
										CharacterAnimation.CrossFade("f02_bullyLaugh_00");
										Scrubber.SetActive(false);
										Eraser.SetActive(false);
										BullyPhase++;
									}
								}
								else if (BullyPhase == 3)
								{
									if (CharacterAnimation["f02_bullyLaugh_00"].time >= CharacterAnimation["f02_bullyLaugh_00"].length)
									{
										BullyPhase++;
									}
								}
								else if (BullyPhase == 4)
								{
									StudentManager.Students[StudentManager.VictimID].Routine = true;
									ScheduleBlock scheduleBlock7 = ScheduleBlocks[4];
									scheduleBlock7.destination = "LunchSpot";
									scheduleBlock7.action = "Wait";
									GetDestinations();
									CurrentDestination = Destinations[Phase];
									Pathfinding.target = Destinations[Phase];
									SmartPhone.SetActive(true);
									Scrubber.SetActive(false);
									Eraser.SetActive(false);
								}
								return;
							}
							if (StudentManager.Students[81].BullyPhase < 2)
							{
								if (GiggleTimer == 0f)
								{
									AudioSource.PlayClipAtPoint(BullyGiggles[UnityEngine.Random.Range(0, BullyGiggles.Length)], Head.position);
									GiggleTimer = 5f;
								}
								GiggleTimer = Mathf.MoveTowards(GiggleTimer, 0f, Time.deltaTime);
								CharacterAnimation.CrossFade("f02_bullyGiggle_00");
							}
							else if (StudentManager.Students[81].BullyPhase < 4)
							{
								if (LaughTimer == 0f)
								{
									AudioSource.PlayClipAtPoint(BullyLaughs[BullyID], Head.position);
									LaughTimer = 5f;
								}
								LaughTimer = Mathf.MoveTowards(LaughTimer, 0f, Time.deltaTime);
								CharacterAnimation.CrossFade("f02_bullyLaugh_00");
							}
							if (CharacterAnimation["f02_bullyLaugh_00"].time >= CharacterAnimation["f02_bullyLaugh_00"].length || StudentManager.Students[81].BullyPhase == 4 || BullyPhase == 4)
							{
								DistanceToDestination = 100f;
								ScheduleBlock scheduleBlock8 = ScheduleBlocks[4];
								scheduleBlock8.destination = "Patrol";
								scheduleBlock8.action = "Patrol";
								GetDestinations();
								CurrentDestination = Destinations[Phase];
								Pathfinding.target = Destinations[Phase];
								SmartPhone.SetActive(true);
							}
						}
						else
						{
							DistanceToDestination = 100f;
							ScheduleBlock scheduleBlock9 = ScheduleBlocks[4];
							scheduleBlock9.destination = "Patrol";
							scheduleBlock9.action = "Patrol";
							GetDestinations();
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
							SmartPhone.SetActive(true);
						}
					}
					else if (Actions[Phase] == StudentActionType.Follow)
					{
						CharacterAnimation.CrossFade(IdleAnim);
					}
					else if (Actions[Phase] == StudentActionType.Sulk)
					{
						if (Male)
						{
							CharacterAnimation.CrossFade("delinquentSulk_00");
							return;
						}
						CharacterAnimation.CrossFade("f02_railingSulk_0" + SulkPhase, 1f);
						SulkTimer += Time.deltaTime;
						if (SulkTimer > 7.66666f)
						{
							SulkTimer = 0f;
							SulkPhase++;
							if (SulkPhase == 3)
							{
								SulkPhase = 0;
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Sleuth)
					{
						if (StudentManager.SleuthPhase != 3)
						{
							StudentManager.ConvoManager.CheckMe(StudentID);
							if (Alone)
							{
								if (Male)
								{
									CharacterAnimation.CrossFade("standTexting_00");
								}
								else
								{
									CharacterAnimation.CrossFade("f02_standTexting_00");
								}
								if (Male)
								{
									SmartPhone.transform.localPosition = new Vector3(0.025f, 0.0025f, 0.025f);
									SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, 180f);
								}
								else
								{
									SmartPhone.transform.localPosition = new Vector3(0.01f, 0.01f, 0.01f);
									SmartPhone.transform.localEulerAngles = new Vector3(0f, -160f, 165f);
								}
								SmartPhone.SetActive(true);
								SpeechLines.Stop();
								return;
							}
							if (!SpeechLines.isPlaying)
							{
								SmartPhone.SetActive(false);
								SpeechLines.Play();
							}
							CharacterAnimation.CrossFade(RandomSleuthAnim, 1f);
							if (CharacterAnimation[RandomSleuthAnim].time >= CharacterAnimation[RandomSleuthAnim].length)
							{
								PickRandomSleuthAnim();
							}
							StudentManager.SleuthTimer += Time.deltaTime;
							if (StudentManager.SleuthTimer > 100f)
							{
								StudentManager.SleuthTimer = 0f;
								StudentManager.UpdateSleuths();
							}
						}
						else
						{
							CharacterAnimation.CrossFade(SleuthScanAnim);
							if (CharacterAnimation[SleuthScanAnim].time >= CharacterAnimation[SleuthScanAnim].length)
							{
								GetSleuthTarget();
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Stalk)
					{
						CharacterAnimation.CrossFade(SleuthIdleAnim);
						if (DistanceToPlayer < 5f)
						{
							if (Vector3.Distance(base.transform.position, StudentManager.FleeSpots[0].position) > 10f)
							{
								Pathfinding.target = StudentManager.FleeSpots[0];
								CurrentDestination = StudentManager.FleeSpots[0];
							}
							else
							{
								Pathfinding.target = StudentManager.FleeSpots[1];
								CurrentDestination = StudentManager.FleeSpots[1];
							}
							Pathfinding.speed = 4f;
							StalkerFleeing = true;
						}
					}
					else if (Actions[Phase] == StudentActionType.Sketch)
					{
						CharacterAnimation.CrossFade(SketchAnim);
						Sketchbook.SetActive(true);
						Pencil.SetActive(true);
					}
					else if (Actions[Phase] == StudentActionType.Sunbathe)
					{
						if (SunbathePhase == 0)
						{
							if (StudentManager.CommunalLocker.Student == null)
							{
								StudentManager.CommunalLocker.Open = true;
								StudentManager.CommunalLocker.Student = this;
								StudentManager.CommunalLocker.SpawnSteam();
								MustChangeClothing = true;
								Schoolwear = 2;
								SunbathePhase++;
							}
							else
							{
								CharacterAnimation.CrossFade(IdleAnim);
							}
						}
						else if (SunbathePhase == 1)
						{
							if (!StudentManager.CommunalLocker.SteamCountdown)
							{
								Pathfinding.target = StudentManager.SunbatheSpots[StudentID - 80];
								CurrentDestination = StudentManager.SunbatheSpots[StudentID - 80];
								StudentManager.CommunalLocker.Student = null;
								SunbathePhase++;
							}
						}
						else if (SunbathePhase == 2)
						{
							MyRenderer.updateWhenOffscreen = true;
							CharacterAnimation.CrossFade("f02_sunbatheStart_00");
							SmartPhone.SetActive(false);
							if (CharacterAnimation["f02_sunbatheStart_00"].time >= CharacterAnimation["f02_sunbatheStart_00"].length)
							{
								MyController.radius = 0f;
								SunbathePhase++;
							}
						}
						else if (SunbathePhase == 3)
						{
							CharacterAnimation.CrossFade("f02_sunbatheLoop_00");
						}
					}
					else if (Actions[Phase] == StudentActionType.Shock)
					{
						if (StudentManager.Students[36] == null)
						{
							Phase++;
						}
						else if (StudentManager.Students[36].Routine && StudentManager.Students[36].DistanceToDestination < 1f)
						{
							if (!StudentManager.GamingDoor.Open)
							{
								StudentManager.GamingDoor.OpenDoor();
							}
							ParticleSystem.EmissionModule emission2 = Hearts.emission;
							if (SmartPhone.activeInHierarchy)
							{
								Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 1f);
								SmartPhone.SetActive(false);
								MyController.radius = 0f;
								emission2.rateOverTime = 5f;
								emission2.enabled = true;
								Hearts.Play();
							}
							CharacterAnimation.CrossFade("f02_peeking_0" + (StudentID - 80));
						}
						else
						{
							CharacterAnimation.CrossFade(PatrolAnim);
							if (!SmartPhone.activeInHierarchy)
							{
								SmartPhone.SetActive(true);
								MyController.radius = 0.1f;
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.Miyuki)
					{
						if (StudentManager.MiyukiEnemy.Enemy.activeInHierarchy)
						{
							CharacterAnimation.CrossFade(MiyukiAnim);
							MiyukiTimer += Time.deltaTime;
							if (MiyukiTimer > 1f)
							{
								MiyukiTimer = 0f;
								Miyuki.Shoot();
							}
						}
						else
						{
							CharacterAnimation.CrossFade(VictoryAnim);
						}
					}
					else if (Actions[Phase] == StudentActionType.Meeting)
					{
						if (StudentID != 36)
						{
							StudentManager.Meeting = true;
							if (StudentManager.Speaker == StudentID)
							{
								if (!SpeechLines.isPlaying)
								{
									CharacterAnimation.CrossFade(RandomAnim);
									SpeechLines.Play();
								}
							}
							else
							{
								CharacterAnimation.CrossFade(IdleAnim);
								if (SpeechLines.isPlaying)
								{
									SpeechLines.Stop();
								}
							}
						}
						else
						{
							CharacterAnimation.CrossFade(PeekAnim);
						}
					}
					else if (Actions[Phase] == StudentActionType.Lyrics)
					{
						LyricsTimer += Time.deltaTime;
						if (LyricsPhase == 0)
						{
							CharacterAnimation.CrossFade("f02_writingLyrics_00");
							if (!Pencil.activeInHierarchy)
							{
								Pencil.SetActive(true);
							}
							if (LyricsTimer > 18f)
							{
								StudentManager.LyricsSpot.position = StudentManager.AirGuitarSpot.position;
								StudentManager.LyricsSpot.eulerAngles = StudentManager.AirGuitarSpot.eulerAngles;
								Pencil.SetActive(false);
								LyricsPhase = 1;
								LyricsTimer = 0f;
							}
						}
						else
						{
							CharacterAnimation.CrossFade("f02_airGuitar_00");
							if (!AirGuitar.isPlaying)
							{
								AirGuitar.Play();
							}
							if (LyricsTimer > 18f)
							{
								StudentManager.LyricsSpot.position = StudentManager.OriginalLyricsSpot.position;
								StudentManager.LyricsSpot.eulerAngles = StudentManager.OriginalLyricsSpot.eulerAngles;
								AirGuitar.Stop();
								LyricsPhase = 0;
								LyricsTimer = 0f;
							}
						}
					}
					else
					{
						if (Actions[Phase] != StudentActionType.Sew)
						{
							return;
						}
						CharacterAnimation.CrossFade("sewing_00");
						PinkSeifuku.SetActive(true);
						if (SewTimer < 10f && TaskGlobals.GetTaskStatus(8) == 3)
						{
							SewTimer += Time.deltaTime;
							if (SewTimer > 10f)
							{
								UnityEngine.Object.Instantiate(Yandere.PauseScreen.DropsMenu.GetComponent<DropsScript>().InfoChanWindow.Drops[1], new Vector3(28.289f, 0.7718928f, 5.196f), Quaternion.identity);
							}
						}
					}
				}
				else
				{
					CharacterAnimation.CrossFade(IdleAnim);
					GoAwayTimer += Time.deltaTime;
					if (GoAwayTimer > 10f)
					{
						CurrentDestination = Destinations[Phase];
						Pathfinding.target = Destinations[Phase];
						GoAwayTimer = 0f;
						GoAway = false;
					}
				}
				return;
			}
			if (MeetTimer == 0f)
			{
				if (Yandere.Bloodiness == 0f && (double)Yandere.Sanity >= 66.66666 && (CurrentDestination == StudentManager.MeetSpots.List[8] || CurrentDestination == StudentManager.MeetSpots.List[9] || CurrentDestination == StudentManager.MeetSpots.List[10]))
				{
					if (StudentID == 30)
					{
						StudentManager.OfferHelp.UpdateLocation();
						StudentManager.OfferHelp.enabled = true;
					}
					else if (StudentID == 5)
					{
						Yandere.BullyPhotoCheck();
						if (Yandere.BullyPhoto)
						{
							StudentManager.FragileOfferHelp.UpdateLocation();
							StudentManager.FragileOfferHelp.enabled = true;
						}
					}
				}
				if (!SchoolGlobals.RoofFence && base.transform.position.y > 11f)
				{
					Prompt.Label[0].text = "     Push";
					Prompt.HideButton[0] = false;
					Pushable = true;
				}
				if (CurrentDestination == StudentManager.FountainSpot)
				{
					Prompt.Label[0].text = "     Drown";
					Prompt.HideButton[0] = false;
					Drownable = true;
				}
			}
			CharacterAnimation.CrossFade(IdleAnim);
			MeetTimer += Time.deltaTime;
			if (MeetTimer > 60f)
			{
				if (!Male)
				{
					Subtitle.UpdateLabel(SubtitleType.NoteReaction, 4, 3f);
				}
				else if (StudentID == 28)
				{
					Subtitle.UpdateLabel(SubtitleType.NoteReactionMale, 6, 3f);
				}
				else
				{
					Subtitle.UpdateLabel(SubtitleType.NoteReactionMale, 4, 3f);
				}
				while (Clock.HourTime >= ScheduleBlocks[Phase].time)
				{
					Phase++;
				}
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
				StopMeeting();
			}
			return;
		}
		if (CurrentDestination != null)
		{
			DistanceToDestination = Vector3.Distance(base.transform.position, CurrentDestination.position);
		}
		if (Fleeing && !Dying)
		{
			if (!PinningDown)
			{
				if (Persona == PersonaType.Dangerous)
				{
					Yandere.Pursuer = this;
					if (StudentManager.CombatMinigame.Path > 3 && StudentManager.CombatMinigame.Path < 7)
					{
						ReturnToRoutine();
					}
				}
				if (Yandere.Chased)
				{
					Pathfinding.speed += Time.deltaTime;
				}
				if (Pathfinding.target != null)
				{
					DistanceToDestination = Vector3.Distance(base.transform.position, Pathfinding.target.position);
				}
				if (AlarmTimer > 0f)
				{
					AlarmTimer = Mathf.MoveTowards(AlarmTimer, 0f, Time.deltaTime);
					if (StudentID == 1)
					{
					}
					CharacterAnimation.CrossFade(ScaredAnim);
					if (AlarmTimer == 0f)
					{
						WalkBack = false;
						Alarmed = false;
					}
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					if (WitnessedMurder)
					{
						targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					}
					else if (WitnessedCorpse)
					{
						targetRotation = Quaternion.LookRotation(new Vector3(Corpse.AllColliders[0].transform.position.x, base.transform.position.y, Corpse.AllColliders[0].transform.position.z) - base.transform.position);
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					}
				}
				else
				{
					if (Persona == PersonaType.TeachersPet && WitnessedMurder && ReportPhase == 0 && StudentManager.Reporter == null && !Police.Called)
					{
						Pathfinding.target = StudentManager.Teachers[Class].transform;
						CurrentDestination = StudentManager.Teachers[Class].transform;
						StudentManager.Reporter = this;
						ReportingMurder = true;
						DetermineCorpseLocation();
					}
					if (base.transform.position.y < -11f)
					{
						if (Persona != PersonaType.Coward && Persona != PersonaType.Evil && Persona != PersonaType.Fragile && OriginalPersona != PersonaType.Evil)
						{
							Police.Witnesses--;
							Police.Show = true;
							if (Countdown.gameObject.activeInHierarchy)
							{
								PhoneAddictGameOver();
							}
						}
						base.transform.position = new Vector3(base.transform.position.x, -100f, base.transform.position.z);
						base.gameObject.SetActive(false);
					}
					if (base.transform.position.z < -99f)
					{
						Prompt.Hide();
						Prompt.enabled = false;
						Safe = true;
					}
					if (DistanceToDestination > TargetDistance)
					{
						CharacterAnimation.CrossFade(SprintAnim);
						Pathfinding.canSearch = true;
						Pathfinding.canMove = true;
						if (Yandere.Chased)
						{
							Pathfinding.repathRate = 0f;
							Pathfinding.speed = 7.5f;
						}
						else
						{
							Pathfinding.speed = 4f;
						}
						if (Persona == PersonaType.PhoneAddict && !CrimeReported)
						{
							if (Countdown.Sprite.fillAmount == 0f)
							{
								Countdown.Sprite.fillAmount = 1f;
								CrimeReported = true;
								if (WitnessedMurder && !Countdown.MaskedPhoto)
								{
									PhoneAddictGameOver();
								}
								else
								{
									if (StudentManager.ChaseCamera == ChaseCamera)
									{
										StudentManager.ChaseCamera = null;
									}
									SprintAnim = OriginalSprintAnim;
									Countdown.gameObject.SetActive(false);
									ChaseCamera.SetActive(false);
									Police.Called = true;
									Police.Show = true;
								}
							}
							else
							{
								SprintAnim = PhoneAnims[2];
								if (StudentManager.ChaseCamera == null)
								{
									StudentManager.ChaseCamera = ChaseCamera;
									ChaseCamera.SetActive(true);
								}
							}
						}
					}
					else
					{
						Pathfinding.canSearch = false;
						Pathfinding.canMove = false;
						if (!Halt)
						{
							if (StudentID > 1)
							{
								MoveTowardsTarget(Pathfinding.target.position);
								if (!Teacher)
								{
									base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Pathfinding.target.rotation, 10f * Time.deltaTime);
								}
							}
						}
						else if (Persona == PersonaType.TeachersPet)
						{
							targetRotation = Quaternion.LookRotation(new Vector3(StudentManager.Teachers[Class].transform.position.x, base.transform.position.y, StudentManager.Teachers[Class].transform.position.z) - base.transform.position);
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
						}
						else if (Persona == PersonaType.Dangerous && !BreakingUpFight)
						{
							targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
						}
						if (Persona == PersonaType.TeachersPet)
						{
							if (ReportingMurder || ReportingBlood)
							{
								if (StudentManager.Teachers[Class].Alarmed)
								{
									if (Club == ClubType.Council)
									{
										Pathfinding.target = StudentManager.CorpseLocation;
										CurrentDestination = StudentManager.CorpseLocation;
									}
									else
									{
										if (PetDestination == null)
										{
											PetDestination = UnityEngine.Object.Instantiate(EmptyGameObject, Seat.position + Seat.forward * -0.5f, Quaternion.identity).transform;
										}
										Pathfinding.target = PetDestination;
										CurrentDestination = PetDestination;
									}
									ReportPhase = 3;
								}
								if (ReportPhase == 0)
								{
									if (WitnessedMurder)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.PetMurderReport, 2, 3f);
										CharacterAnimation.CrossFade(ScaredAnim);
									}
									else if (WitnessedCorpse)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.PetCorpseReport, 2, 3f);
										CharacterAnimation.CrossFade(ScaredAnim);
									}
									else if (WitnessedLimb)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.PetLimbReport, 2, 3f);
										CharacterAnimation.CrossFade(ScaredAnim);
									}
									else if (WitnessedBloodyWeapon)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.PetBloodyWeaponReport, 2, 3f);
										CharacterAnimation.CrossFade(ScaredAnim);
									}
									else if (WitnessedBloodPool)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.PetBloodReport, 2, 3f);
										CharacterAnimation.CrossFade(IdleAnim);
									}
									else if (WitnessedWeapon)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.PetWeaponReport, 2, 3f);
										CharacterAnimation.CrossFade(ScaredAnim);
									}
									StudentManager.Teachers[Class].CharacterAnimation.CrossFade(StudentManager.Teachers[Class].IdleAnim);
									StudentManager.Teachers[Class].Routine = false;
									if (StudentManager.Teachers[Class].Investigating)
									{
										StudentManager.Teachers[Class].StopInvestigating();
									}
									Halt = true;
									ReportPhase++;
								}
								else if (ReportPhase == 1)
								{
									Pathfinding.target = StudentManager.Teachers[Class].transform;
									CurrentDestination = StudentManager.Teachers[Class].transform;
									if (WitnessedBloodPool || (WitnessedWeapon && !WitnessedBloodyWeapon))
									{
										CharacterAnimation.CrossFade(IdleAnim);
									}
									else if (WitnessedMurder || WitnessedCorpse || WitnessedLimb || WitnessedBloodyWeapon)
									{
										CharacterAnimation.CrossFade(ScaredAnim);
									}
									StudentManager.Teachers[Class].targetRotation = Quaternion.LookRotation(base.transform.position - StudentManager.Teachers[Class].transform.position);
									StudentManager.Teachers[Class].transform.rotation = Quaternion.Slerp(StudentManager.Teachers[Class].transform.rotation, StudentManager.Teachers[Class].targetRotation, 10f * Time.deltaTime);
									ReportTimer += Time.deltaTime;
									if (ReportTimer >= 3f)
									{
										base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.1f, base.transform.position.z);
										StudentManager.Teachers[Class].MyReporter = this;
										StudentManager.Teachers[Class].Routine = false;
										StudentManager.Teachers[Class].Fleeing = true;
										ReportTimer = 0f;
										ReportPhase++;
									}
								}
								else if (ReportPhase == 2)
								{
									Pathfinding.target = StudentManager.Teachers[Class].transform;
									CurrentDestination = StudentManager.Teachers[Class].transform;
									if (WitnessedBloodPool || (WitnessedWeapon && !WitnessedBloodyWeapon))
									{
										CharacterAnimation.CrossFade(IdleAnim);
									}
									else if (WitnessedMurder || WitnessedCorpse || WitnessedLimb || WitnessedBloodyWeapon)
									{
										CharacterAnimation.CrossFade(ScaredAnim);
									}
								}
								else if (ReportPhase == 3)
								{
									Pathfinding.target = base.transform;
									CurrentDestination = base.transform;
									if (WitnessedBloodPool || (WitnessedWeapon && !WitnessedBloodyWeapon))
									{
										CharacterAnimation.CrossFade(IdleAnim);
									}
									else if (WitnessedMurder || WitnessedCorpse || WitnessedLimb || WitnessedBloodyWeapon)
									{
										CharacterAnimation.CrossFade(ParanoidAnim);
									}
								}
								else if (ReportPhase < 100)
								{
									CharacterAnimation.CrossFade(ParanoidAnim);
								}
								else
								{
									Pathfinding.target = base.transform;
									CurrentDestination = base.transform;
									CharacterAnimation.CrossFade(ScaredAnim);
									ReportTimer += Time.deltaTime;
									if (ReportTimer >= 5f)
									{
										if (StudentManager.Reporter == this)
										{
											StudentManager.CorpseLocation.position = Vector3.zero;
											StudentManager.Reporter = null;
										}
										else if (StudentManager.BloodReporter == this)
										{
											StudentManager.BloodLocation.position = Vector3.zero;
											StudentManager.BloodReporter = null;
										}
										StudentManager.UpdateStudents();
										CurrentDestination = Destinations[Phase];
										Pathfinding.target = Destinations[Phase];
										Pathfinding.speed = 1f;
										TargetDistance = 1f;
										ReportPhase = 0;
										ReportTimer = 0f;
										AlarmTimer = 0f;
										RandomAnim = BulliedIdleAnim;
										IdleAnim = BulliedIdleAnim;
										WalkAnim = BulliedWalkAnim;
										if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
										{
											Persona = OriginalPersona;
										}
										BloodPool = null;
										WitnessedBloodyWeapon = false;
										WitnessedBloodPool = false;
										WitnessedSomething = false;
										WitnessedCorpse = false;
										WitnessedMurder = false;
										WitnessedWeapon = false;
										WitnessedLimb = false;
										SmartPhone.SetActive(false);
										LostTeacherTrust = true;
										ReportingMurder = false;
										ReportingBlood = false;
										Reacted = false;
										Alarmed = false;
										Fleeing = false;
										Routine = true;
										Halt = false;
										if (Club == ClubType.Council)
										{
											Persona = PersonaType.Dangerous;
										}
										for (ID = 0; ID < Outlines.Length; ID++)
										{
											Outlines[ID].color = new Color(1f, 1f, 0f, 1f);
										}
									}
								}
							}
							else if (Club == ClubType.Council)
							{
								CharacterAnimation.CrossFade(GuardAnim);
								Persona = PersonaType.Dangerous;
								Guarding = true;
								Fleeing = false;
							}
							else
							{
								CharacterAnimation.CrossFade(ParanoidAnim);
								ReportPhase = 100;
							}
						}
						else if (Persona == PersonaType.Heroic)
						{
							if (!Yandere.Attacking && !StudentManager.PinningDown && !Yandere.Shoved)
							{
								if (StudentID == 1)
								{
									Yandere.EmptyHands();
									Pathfinding.canSearch = false;
									Pathfinding.canMove = false;
									TargetDistance = 1f;
									Yandere.CharacterAnimation.CrossFade("f02_unmasking_00");
									CharacterAnimation.CrossFade("unmasking_00");
									Yandere.CanMove = false;
									targetRotation = Quaternion.LookRotation(Yandere.transform.position - base.transform.position);
									base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
									MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward * 1f);
									if (CharacterAnimation["unmasking_00"].time == 0f)
									{
										Yandere.ShoulderCamera.YandereNo();
										Yandere.Jukebox.GameOver();
									}
									if (CharacterAnimation["unmasking_00"].time >= 0.66666f && Yandere.Mask.transform.parent != LeftHand)
									{
										Yandere.Mask.transform.parent = LeftHand;
										Yandere.Mask.transform.localPosition = new Vector3(-0.1f, -0.05f, 0f);
										Yandere.Mask.transform.localEulerAngles = new Vector3(-90f, 90f, 0f);
										Yandere.Mask.transform.localScale = new Vector3(1f, 1f, 1f);
									}
									if (CharacterAnimation["unmasking_00"].time >= CharacterAnimation["unmasking_00"].length)
									{
										Yandere.Unmasked = true;
										Yandere.ShoulderCamera.GameOver();
									}
								}
								else
								{
									if (!Yandere.Struggling)
									{
										BeginStruggle();
									}
									if (!Teacher)
									{
										CharacterAnimation[StruggleAnim].time = Yandere.CharacterAnimation["f02_struggleA_00"].time;
									}
									else
									{
										CharacterAnimation[StruggleAnim].time = Yandere.CharacterAnimation["f02_teacherStruggleA_00"].time;
									}
									base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Yandere.transform.rotation, 10f * Time.deltaTime);
									MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward * 0.425f);
									if (!Yandere.Armed || !Yandere.EquippedWeapon.Concealable)
									{
										Yandere.StruggleBar.HeroWins();
									}
									if (Lost)
									{
										CharacterAnimation.CrossFade(StruggleWonAnim);
										if (CharacterAnimation[StruggleWonAnim].time > 1f)
										{
											EyeShrink = 1f;
										}
										if (!(CharacterAnimation[StruggleWonAnim].time >= CharacterAnimation[StruggleWonAnim].length))
										{
										}
									}
									else if (Won)
									{
										CharacterAnimation.CrossFade(StruggleLostAnim);
									}
								}
							}
							else
							{
								CharacterAnimation.CrossFade(ReadyToFightAnim);
							}
						}
						else if (Persona == PersonaType.Coward || Persona == PersonaType.Fragile)
						{
							targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
							CharacterAnimation.CrossFade(CowardAnim);
							ReactionTimer += Time.deltaTime;
							if (ReactionTimer > 5f)
							{
								CurrentDestination = StudentManager.Exit;
								Pathfinding.target = StudentManager.Exit;
							}
						}
						else if (Persona == PersonaType.Evil)
						{
							targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
							CharacterAnimation.CrossFade(EvilAnim);
							ReactionTimer += Time.deltaTime;
							if (ReactionTimer > 5f)
							{
								CurrentDestination = StudentManager.Exit;
								Pathfinding.target = StudentManager.Exit;
							}
						}
						else if (Persona == PersonaType.SocialButterfly)
						{
							if (ReportPhase < 4)
							{
								base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Pathfinding.target.rotation, 10f * Time.deltaTime);
							}
							if (ReportPhase == 1)
							{
								if (!SmartPhone.activeInHierarchy)
								{
									if (StudentManager.Reporter == null && !Police.Called)
									{
										CharacterAnimation.CrossFade(SocialReportAnim);
										Subtitle.UpdateLabel(SubtitleType.SocialReport, 1, 5f);
										StudentManager.Reporter = this;
										SmartPhone.SetActive(true);
										SmartPhone.transform.localPosition = new Vector3(-0.015f, -0.01f, 0f);
										SmartPhone.transform.localEulerAngles = new Vector3(0f, -170f, 165f);
									}
									else
									{
										ReportTimer = 5f;
									}
								}
								ReportTimer += Time.deltaTime;
								if (ReportTimer > 5f)
								{
									if (StudentManager.Reporter == this)
									{
										Police.Called = true;
										Police.Show = true;
									}
									CharacterAnimation.CrossFade(ParanoidAnim);
									SmartPhone.SetActive(false);
									ReportPhase++;
									Halt = false;
								}
							}
							else if (ReportPhase == 2)
							{
								if (WitnessedMurder && (!SawMask || (SawMask && Yandere.Mask != null)))
								{
									LookForYandere();
								}
							}
							else if (ReportPhase == 3)
							{
								CharacterAnimation.CrossFade(SocialFearAnim);
								Subtitle.UpdateLabel(SubtitleType.SocialFear, 1, 5f);
								SpawnAlarmDisc();
								ReportPhase++;
								Halt = true;
							}
							else if (ReportPhase == 4)
							{
								targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
								base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
								if (Yandere.Attacking)
								{
									LookForYandere();
								}
							}
							else if (ReportPhase == 5)
							{
								CharacterAnimation.CrossFade(SocialTerrorAnim);
								Subtitle.UpdateLabel(SubtitleType.SocialTerror, 1, 5f);
								VisionDistance = 0f;
								SpawnAlarmDisc();
								ReportPhase++;
							}
						}
						else if (Persona == PersonaType.Lovestruck)
						{
							if (ReportPhase < 3 && StudentManager.Students[1].Fleeing)
							{
								Pathfinding.target = StudentManager.Exit;
								CurrentDestination = StudentManager.Exit;
								ReportPhase = 3;
							}
							if (ReportPhase == 1)
							{
								StudentManager.Students[1].CharacterAnimation.CrossFade("surprised_00");
								CharacterAnimation.CrossFade(ScaredAnim);
								StudentManager.Students[1].Pathfinding.canSearch = false;
								StudentManager.Students[1].Pathfinding.canMove = false;
								StudentManager.Students[1].Pathfinding.enabled = false;
								StudentManager.Students[1].Routine = false;
								Pathfinding.enabled = false;
								if (WitnessedMurder && !SawMask)
								{
									Yandere.Jukebox.gameObject.active = false;
									Yandere.MainCamera.enabled = false;
									Yandere.RPGCamera.enabled = false;
									Yandere.Jukebox.Volume = 0f;
									Yandere.CanMove = false;
									StudentManager.LovestruckCamera.transform.parent = base.transform;
									StudentManager.LovestruckCamera.transform.localPosition = new Vector3(1f, 1f, -1f);
									StudentManager.LovestruckCamera.transform.localEulerAngles = new Vector3(0f, -30f, 0f);
									StudentManager.LovestruckCamera.active = true;
								}
								if (WitnessedMurder && !SawMask)
								{
									Subtitle.UpdateLabel(SubtitleType.LovestruckMurderReport, 1, 5f);
								}
								else
								{
									Subtitle.UpdateLabel(SubtitleType.LovestruckCorpseReport, 1, 5f);
								}
								ReportPhase++;
							}
							else if (ReportPhase == 2)
							{
								targetRotation = Quaternion.LookRotation(new Vector3(StudentManager.Students[1].transform.position.x, base.transform.position.y, StudentManager.Students[1].transform.position.z) - base.transform.position);
								base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
								targetRotation = Quaternion.LookRotation(new Vector3(base.transform.position.x, StudentManager.Students[1].transform.position.y, base.transform.position.z) - StudentManager.Students[1].transform.position);
								StudentManager.Students[1].transform.rotation = Quaternion.Slerp(StudentManager.Students[1].transform.rotation, targetRotation, 10f * Time.deltaTime);
								ReportTimer += Time.deltaTime;
								if (ReportTimer > 5f)
								{
									if (WitnessedMurder && !SawMask)
									{
										Yandere.ShoulderCamera.HeartbrokenCamera.SetActive(true);
										Yandere.Police.EndOfDay.Heartbroken.Exposed = true;
										Yandere.Character.GetComponent<Animation>().CrossFade("f02_down_22");
										Yandere.Collapse = true;
										StudentManager.StopMoving();
										ReportPhase++;
									}
									else
									{
										StudentManager.Students[1].Pathfinding.target = StudentManager.Exit;
										StudentManager.Students[1].CurrentDestination = StudentManager.Exit;
										StudentManager.Students[1].CharacterAnimation.CrossFade(StudentManager.Students[1].SprintAnim);
										StudentManager.Students[1].Pathfinding.canSearch = true;
										StudentManager.Students[1].Pathfinding.canMove = true;
										StudentManager.Students[1].Pathfinding.enabled = true;
										StudentManager.Students[1].Pathfinding.speed = 4f;
										Pathfinding.target = StudentManager.Exit;
										CurrentDestination = StudentManager.Exit;
										Pathfinding.enabled = true;
										ReportPhase++;
									}
								}
							}
						}
						else if (Persona == PersonaType.Dangerous)
						{
							if (!Yandere.Attacking && !StudentManager.PinningDown && !Yandere.Struggling)
							{
								Spray();
							}
							else
							{
								CharacterAnimation.CrossFade(ReadyToFightAnim);
							}
						}
						else if (Persona == PersonaType.Protective)
						{
							if (!Yandere.Dumping && !Yandere.Attacking)
							{
								if (Yandere.Aiming)
								{
									Yandere.StopAiming();
								}
								Yandere.Mopping = false;
								Yandere.EmptyHands();
								AttackReaction();
								CharacterAnimation[CounterAnim].time = 5f;
								Yandere.CharacterAnimation["f02_counterA_00"].time = 5f;
								Yandere.ShoulderCamera.DoNotMove = true;
								Yandere.ShoulderCamera.Timer = 5f;
								Yandere.ShoulderCamera.Phase = 3;
								Police.Show = false;
								Yandere.CameraEffects.MurderWitnessed();
								Yandere.Jukebox.GameOver();
							}
							else
							{
								CharacterAnimation.CrossFade(ReadyToFightAnim);
							}
						}
						else if (Persona == PersonaType.Violent)
						{
							if (!Yandere.Attacking && !Yandere.Struggling && !StudentManager.PinningDown)
							{
								if (!Yandere.DelinquentFighting)
								{
									SmartPhone.SetActive(false);
									Threatened = true;
									Fleeing = false;
									Alarmed = true;
									NoTalk = false;
									Patience = 0;
								}
							}
							else
							{
								CharacterAnimation.CrossFade(ReadyToFightAnim);
							}
						}
						else if (Persona == PersonaType.Strict)
						{
							if (!WitnessedMurder)
							{
								if (ReportPhase == 0)
								{
									if (MyReporter.WitnessedMurder || MyReporter.WitnessedCorpse)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherReportReaction, 0, 3f);
										InvestigatingPossibleDeath = true;
									}
									else if (MyReporter.WitnessedLimb)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherReportReaction, 2, 3f);
									}
									else if (MyReporter.WitnessedBloodyWeapon)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherReportReaction, 3, 3f);
									}
									else if (MyReporter.WitnessedBloodPool)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherReportReaction, 1, 3f);
									}
									else if (MyReporter.WitnessedWeapon)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherReportReaction, 4, 3f);
									}
									ReportPhase++;
								}
								else if (ReportPhase == 1)
								{
									CharacterAnimation.CrossFade(IdleAnim);
									ReportTimer += Time.deltaTime;
									if (ReportTimer >= 3f)
									{
										base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.1f, base.transform.position.z);
										StudentScript studentScript = null;
										if (MyReporter.WitnessedMurder || MyReporter.WitnessedCorpse)
										{
											studentScript = StudentManager.Reporter;
										}
										else if (MyReporter.WitnessedBloodPool || MyReporter.WitnessedLimb || MyReporter.WitnessedWeapon)
										{
											studentScript = StudentManager.BloodReporter;
										}
										if (MyReporter.WitnessedLimb)
										{
											InvestigatingPossibleLimb = true;
										}
										if (!studentScript.Teacher)
										{
											if (MyReporter.WitnessedMurder || MyReporter.WitnessedCorpse)
											{
												StudentManager.Reporter.CurrentDestination = StudentManager.CorpseLocation;
												StudentManager.Reporter.Pathfinding.target = StudentManager.CorpseLocation;
												CurrentDestination = StudentManager.CorpseLocation;
												Pathfinding.target = StudentManager.CorpseLocation;
												StudentManager.Reporter.TargetDistance = 2f;
											}
											else if (MyReporter.WitnessedBloodPool || MyReporter.WitnessedLimb || MyReporter.WitnessedWeapon)
											{
												StudentManager.BloodReporter.CurrentDestination = StudentManager.BloodLocation;
												StudentManager.BloodReporter.Pathfinding.target = StudentManager.BloodLocation;
												CurrentDestination = StudentManager.BloodLocation;
												Pathfinding.target = StudentManager.BloodLocation;
												StudentManager.BloodReporter.TargetDistance = 2f;
											}
										}
										TargetDistance = 1f;
										ReportTimer = 0f;
										ReportPhase++;
									}
								}
								else if (ReportPhase == 2)
								{
									if (WitnessedCorpse)
									{
										DetermineCorpseLocation();
										if (!Corpse.Poisoned)
										{
											Subtitle.Speaker = this;
											Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 1, 5f);
										}
										else
										{
											Subtitle.Speaker = this;
											Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 2, 2f);
										}
										ReportPhase++;
									}
									else if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
									{
										DetermineBloodLocation();
										if (WitnessedLimb)
										{
											Subtitle.Speaker = this;
											Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 4, 5f);
										}
										else if (WitnessedBloodPool || WitnessedBloodyWeapon)
										{
											Subtitle.Speaker = this;
											Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 3, 5f);
										}
										else if (WitnessedWeapon)
										{
											Subtitle.Speaker = this;
											Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 5, 5f);
										}
										ReportPhase++;
									}
									else
									{
										CharacterAnimation.CrossFade(GuardAnim);
										ReportTimer += Time.deltaTime;
										if (ReportTimer > 5f)
										{
											Subtitle.UpdateLabel(SubtitleType.TeacherPrankReaction, 1, 7f);
											ReportPhase = 98;
											ReportTimer = 0f;
										}
									}
								}
								else if (ReportPhase == 3)
								{
									if (WitnessedCorpse)
									{
										targetRotation = Quaternion.LookRotation(new Vector3(Corpse.AllColliders[0].transform.position.x, base.transform.position.y, Corpse.AllColliders[0].transform.position.z) - base.transform.position);
										base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
										CharacterAnimation.CrossFade(InspectAnim);
									}
									else if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
									{
										targetRotation = Quaternion.LookRotation(new Vector3(BloodPool.transform.position.x, base.transform.position.y, BloodPool.transform.position.z) - base.transform.position);
										base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
										CharacterAnimation[InspectBloodAnim].speed = 0.66666f;
										CharacterAnimation.CrossFade(InspectBloodAnim);
									}
									ReportTimer += Time.deltaTime;
									if (ReportTimer >= 6f)
									{
										ReportTimer = 0f;
										if (WitnessedWeapon && !WitnessedBloodyWeapon)
										{
											ReportPhase = 7;
										}
										else
										{
											ReportPhase++;
										}
									}
								}
								else if (ReportPhase == 4)
								{
									if (WitnessedCorpse)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherPoliceReport, 0, 5f);
									}
									else if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
									{
										Subtitle.Speaker = this;
										Subtitle.UpdateLabel(SubtitleType.TeacherPoliceReport, 1, 5f);
									}
									SmartPhone.transform.localPosition = new Vector3(-0.01f, -0.005f, -0.02f);
									SmartPhone.transform.localEulerAngles = new Vector3(-10f, -145f, 170f);
									SmartPhone.SetActive(true);
									ReportPhase++;
								}
								else if (ReportPhase == 5)
								{
									CharacterAnimation.CrossFade(CallAnim);
									ReportTimer += Time.deltaTime;
									if (ReportTimer >= 5f)
									{
										CharacterAnimation.CrossFade(GuardAnim);
										SmartPhone.SetActive(false);
										WitnessedBloodyWeapon = false;
										WitnessedBloodPool = false;
										WitnessedSomething = false;
										WitnessedWeapon = false;
										WitnessedLimb = false;
										Police.Called = true;
										Police.Show = true;
										ReportTimer = 0f;
										Guarding = true;
										Alarmed = false;
										Fleeing = false;
										Reacted = false;
										ReportPhase++;
										if (MyReporter != null && MyReporter.ReportingBlood)
										{
											MyReporter.ReportPhase++;
										}
									}
								}
								else if (ReportPhase != 6)
								{
									if (ReportPhase == 7)
									{
										StudentManager.BloodReporter = null;
										StudentManager.UpdateStudents();
										if (MyReporter != null)
										{
											MyReporter.CurrentDestination = MyReporter.Destinations[MyReporter.Phase];
											MyReporter.Pathfinding.target = MyReporter.Destinations[MyReporter.Phase];
											MyReporter.Pathfinding.speed = 1f;
											MyReporter.ReportTimer = 0f;
											MyReporter.AlarmTimer = 0f;
											MyReporter.TargetDistance = 1f;
											MyReporter.ReportPhase = 0;
											MyReporter.WitnessedSomething = false;
											MyReporter.WitnessedWeapon = false;
											MyReporter.Reacted = false;
											MyReporter.Alarmed = false;
											MyReporter.Fleeing = false;
											MyReporter.Routine = true;
											MyReporter.Halt = false;
											MyReporter.Persona = OriginalPersona;
											if (MyReporter.Club == ClubType.Council)
											{
												MyReporter.Persona = PersonaType.Dangerous;
											}
											for (ID = 0; ID < MyReporter.Outlines.Length; ID++)
											{
												MyReporter.Outlines[ID].color = new Color(1f, 1f, 0f, 1f);
											}
										}
										BloodPool.GetComponent<WeaponScript>().Prompt.enabled = false;
										BloodPool.GetComponent<WeaponScript>().Prompt.Hide();
										BloodPool.GetComponent<WeaponScript>().enabled = false;
										ReportPhase++;
									}
									else if (ReportPhase == 8)
									{
										CharacterAnimation.CrossFade("f02_teacherPickUp_00");
										if (CharacterAnimation["f02_teacherPickUp_00"].time >= 0.33333f)
										{
											Handkerchief.SetActive(true);
										}
										if (CharacterAnimation["f02_teacherPickUp_00"].time >= 2f)
										{
											BloodPool.parent = RightHand;
											BloodPool.localPosition = new Vector3(0f, 0f, 0f);
											BloodPool.localEulerAngles = new Vector3(0f, 0f, 0f);
										}
										if (CharacterAnimation["f02_teacherPickUp_00"].time >= CharacterAnimation["f02_teacherPickUp_00"].length)
										{
											CurrentDestination = StudentManager.WeaponBoxSpot;
											Pathfinding.target = StudentManager.WeaponBoxSpot;
											Pathfinding.speed = 1f;
											Hurry = false;
											ReportPhase++;
										}
									}
									else if (ReportPhase == 9)
									{
										StudentManager.BloodLocation.position = Vector3.zero;
										BloodPool.parent = null;
										BloodPool.position = StudentManager.WeaponBoxSpot.parent.position + new Vector3(0f, 1f, 0f);
										BloodPool.eulerAngles = new Vector3(0f, 90f, 0f);
										BloodPool.GetComponent<WeaponScript>().Prompt.enabled = true;
										BloodPool.GetComponent<WeaponScript>().enabled = true;
										BloodPool.GetComponent<WeaponScript>().Drop();
										BloodPool = null;
										CharacterAnimation.CrossFade(RunAnim);
										CurrentDestination = Destinations[Phase];
										Pathfinding.target = Destinations[Phase];
										Handkerchief.SetActive(false);
										Pathfinding.canSearch = true;
										Pathfinding.canMove = true;
										Pathfinding.speed = 1f;
										WitnessedSomething = false;
										Alarmed = false;
										Fleeing = false;
										Routine = true;
										ReportTimer = 0f;
										ReportPhase = 0;
									}
									else if (ReportPhase == 98)
									{
										CharacterAnimation.CrossFade(IdleAnim);
										targetRotation = Quaternion.LookRotation(MyReporter.transform.position - base.transform.position);
										base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
										ReportTimer += Time.deltaTime;
										if (ReportTimer > 7f)
										{
											ReportPhase++;
										}
									}
									else if (ReportPhase == 99)
									{
										Subtitle.UpdateLabel(SubtitleType.PrankReaction, 1, 5f);
										CharacterAnimation.CrossFade(RunAnim);
										CurrentDestination = Destinations[Phase];
										Pathfinding.target = Destinations[Phase];
										Pathfinding.canSearch = true;
										Pathfinding.canMove = true;
										MyReporter.Persona = PersonaType.TeachersPet;
										MyReporter.ReportPhase = 100;
										MyReporter.Fleeing = true;
										ReportTimer = 0f;
										ReportPhase = 0;
										InvestigatingPossibleDeath = false;
										InvestigatingPossibleLimb = false;
										Alarmed = false;
										Fleeing = false;
										Routine = true;
									}
								}
							}
							else if (!Yandere.Dumping && !Yandere.Attacking)
							{
								if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus == 0)
								{
									if (Yandere.Aiming)
									{
										Yandere.StopAiming();
									}
									Yandere.Mopping = false;
									Yandere.EmptyHands();
									AttackReaction();
									CharacterAnimation[CounterAnim].time = 5f;
									Yandere.CharacterAnimation["f02_counterA_00"].time = 5f;
									Yandere.ShoulderCamera.DoNotMove = true;
									Yandere.ShoulderCamera.Timer = 5f;
									Yandere.ShoulderCamera.Phase = 3;
									Police.Show = false;
									Yandere.CameraEffects.MurderWitnessed();
									Yandere.Jukebox.GameOver();
								}
								else
								{
									Persona = PersonaType.Heroic;
								}
							}
							else
							{
								CharacterAnimation.CrossFade(ReadyToFightAnim);
							}
						}
					}
					if (Persona == PersonaType.Strict && BloodPool != null && BloodPool.parent == Yandere.RightHand)
					{
						WitnessedBloodyWeapon = false;
						WitnessedBloodPool = false;
						WitnessedSomething = false;
						WitnessedCorpse = false;
						WitnessedMurder = false;
						WitnessedWeapon = false;
						WitnessedLimb = false;
						YandereVisible = true;
						ReportTimer = 0f;
						BloodPool = null;
						ReportPhase = 0;
						Alarmed = false;
						Fleeing = false;
						Routine = true;
						Reacted = false;
						AlarmTimer = 0f;
						Alarm = 200f;
						BecomeAlarmed();
					}
				}
			}
			else if (PinPhase == 0)
			{
				if (DistanceToDestination < 1f)
				{
					if (Pathfinding.canSearch)
					{
						Pathfinding.canSearch = false;
						Pathfinding.canMove = false;
					}
					targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
					CharacterAnimation.CrossFade(ReadyToFightAnim);
					MoveTowardsTarget(CurrentDestination.position);
				}
				else
				{
					CharacterAnimation.CrossFade(SprintAnim);
					if (!Pathfinding.canSearch)
					{
						Pathfinding.canSearch = true;
						Pathfinding.canMove = true;
					}
				}
			}
			else
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
				MoveTowardsTarget(CurrentDestination.position);
			}
		}
		if (Following && !Waiting)
		{
			DistanceToDestination = Vector3.Distance(base.transform.position, Pathfinding.target.position);
			if (DistanceToDestination > 2f)
			{
				CharacterAnimation.CrossFade(RunAnim);
				Pathfinding.speed = 4f;
				Obstacle.enabled = false;
			}
			else if (DistanceToDestination > 1f)
			{
				CharacterAnimation.CrossFade(OriginalWalkAnim);
				Pathfinding.canMove = true;
				Pathfinding.speed = 1f;
				Obstacle.enabled = false;
			}
			else
			{
				CharacterAnimation.CrossFade(IdleAnim);
				Pathfinding.canMove = false;
				Obstacle.enabled = true;
			}
			if (Phase < ScheduleBlocks.Length - 1 && (Clock.HourTime >= ScheduleBlocks[Phase].time || StudentManager.LockerRoomArea.bounds.Contains(Yandere.transform.position) || StudentManager.WestBathroomArea.bounds.Contains(Yandere.transform.position) || StudentManager.EastBathroomArea.bounds.Contains(Yandere.transform.position) || StudentManager.IncineratorArea.bounds.Contains(Yandere.transform.position) || StudentManager.HeadmasterArea.bounds.Contains(Yandere.transform.position)))
			{
				if (Clock.HourTime >= ScheduleBlocks[Phase].time)
				{
					Phase++;
				}
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
				ParticleSystem.EmissionModule emission3 = Hearts.emission;
				emission3.enabled = false;
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				Pathfinding.speed = 1f;
				Yandere.Followers--;
				Following = false;
				Routine = true;
				if (StudentManager.LockerRoomArea.bounds.Contains(Yandere.transform.position) || StudentManager.WestBathroomArea.bounds.Contains(Yandere.transform.position) || StudentManager.EastBathroomArea.bounds.Contains(Yandere.transform.position) || StudentManager.IncineratorArea.bounds.Contains(Yandere.transform.position) || StudentManager.HeadmasterArea.bounds.Contains(Yandere.transform.position))
				{
					Subtitle.UpdateLabel(SubtitleType.StopFollowApology, 1, 3f);
				}
				else
				{
					Subtitle.UpdateLabel(SubtitleType.StopFollowApology, 0, 3f);
				}
				Prompt.Label[0].text = "     Talk";
			}
		}
		if (Wet)
		{
			if (DistanceToDestination < TargetDistance)
			{
				if (!Splashed)
				{
					if (!InDarkness)
					{
						if (BathePhase == 1)
						{
							StudentManager.CommunalLocker.Open = true;
							StudentManager.CommunalLocker.Student = this;
							StudentManager.CommunalLocker.SpawnSteam();
							Pathfinding.speed = 1f;
							Schoolwear = 0;
							BathePhase++;
						}
						else if (BathePhase == 2)
						{
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
							MoveTowardsTarget(CurrentDestination.position);
						}
						else if (BathePhase == 3)
						{
							StudentManager.CommunalLocker.Open = false;
							CharacterAnimation.CrossFade(WalkAnim);
							if (!BatheFast)
							{
								if (!Male)
								{
									CurrentDestination = StudentManager.FemaleBatheSpot;
									Pathfinding.target = StudentManager.FemaleBatheSpot;
								}
								else
								{
									CurrentDestination = StudentManager.MaleBatheSpot;
									Pathfinding.target = StudentManager.MaleBatheSpot;
								}
							}
							else if (!Male)
							{
								CurrentDestination = StudentManager.FastBatheSpot;
								Pathfinding.target = StudentManager.FastBatheSpot;
							}
							else
							{
								CurrentDestination = StudentManager.MaleBatheSpot;
								Pathfinding.target = StudentManager.MaleBatheSpot;
							}
							Pathfinding.canSearch = true;
							Pathfinding.canMove = true;
							BathePhase++;
						}
						else if (BathePhase == 4)
						{
							StudentManager.OpenValue = Mathf.Lerp(StudentManager.OpenValue, 0f, Time.deltaTime * 10f);
							StudentManager.FemaleShowerCurtain.SetBlendShapeWeight(0, StudentManager.OpenValue);
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
							MoveTowardsTarget(CurrentDestination.position);
							CharacterAnimation.CrossFade(BathingAnim);
							if (CharacterAnimation[BathingAnim].time >= CharacterAnimation[BathingAnim].length)
							{
								StudentManager.OpenCurtain = true;
								LiquidProjector.enabled = false;
								Bloody = false;
								BathePhase++;
								Gas = false;
								GoChange();
								UnWet();
							}
						}
						else if (BathePhase == 5)
						{
							StudentManager.CommunalLocker.Open = true;
							StudentManager.CommunalLocker.Student = this;
							StudentManager.CommunalLocker.SpawnSteam();
							Schoolwear = (InEvent ? 1 : 3);
							BathePhase++;
						}
						else if (BathePhase == 6)
						{
							base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
							MoveTowardsTarget(CurrentDestination.position);
						}
						else if (BathePhase == 7)
						{
							if (StudentID == 30 || StudentID == StudentManager.RivalID)
							{
								if (StudentManager.CommunalLocker.RivalPhone.Stolen)
								{
									CharacterAnimation.CrossFade("f02_losingPhone_00");
									Subtitle.UpdateLabel(LostPhoneSubtitleType, 1, 5f);
									if (StudentID == StudentManager.RivalID)
									{
										RealizePhoneIsMissing();
									}
									Phoneless = true;
									BatheTimer = 6f;
									BathePhase++;
								}
								else
								{
									StudentManager.CommunalLocker.RivalPhone.gameObject.SetActive(false);
									BathePhase++;
								}
							}
							else
							{
								BathePhase += 2;
							}
						}
						else if (BathePhase == 8)
						{
							if (BatheTimer == 0f)
							{
								BathePhase++;
							}
							else
							{
								BatheTimer = Mathf.MoveTowards(BatheTimer, 0f, Time.deltaTime);
							}
						}
						else if (BathePhase == 9)
						{
							StudentManager.CommunalLocker.Student = null;
							StudentManager.CommunalLocker.Open = false;
							if (Phase == 1)
							{
								Phase++;
							}
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
							Pathfinding.canSearch = true;
							Pathfinding.canMove = true;
							DistanceToDestination = 100f;
							Routine = true;
							Wet = false;
							if (FleeWhenClean)
							{
								CurrentDestination = StudentManager.Exit;
								Pathfinding.target = StudentManager.Exit;
								TargetDistance = 0f;
								Routine = false;
								Fleeing = true;
							}
						}
					}
					else if (BathePhase == -1)
					{
						CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
						Subtitle.UpdateLabel(SubtitleType.LightSwitchReaction, 2, 5f);
						CharacterAnimation.CrossFade("f02_electrocution_00");
						Pathfinding.canSearch = false;
						Pathfinding.canMove = false;
						Distracted = true;
						BathePhase++;
					}
					else if (BathePhase == 0)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
						MoveTowardsTarget(CurrentDestination.position);
						if (CharacterAnimation["f02_electrocution_00"].time > 2f && CharacterAnimation["f02_electrocution_00"].time < 5.9500003f)
						{
							if (!LightSwitch.Panel.useGravity)
							{
								if (!Bloody)
								{
									Subtitle.Speaker = this;
									Subtitle.UpdateLabel(SplashSubtitleType, 2, 5f);
								}
								else
								{
									Subtitle.Speaker = this;
									Subtitle.UpdateLabel(SplashSubtitleType, 4, 5f);
								}
								CurrentDestination = StudentManager.FemaleStripSpot;
								Pathfinding.target = StudentManager.FemaleStripSpot;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								Pathfinding.speed = 4f;
								CharacterAnimation.CrossFade(WalkAnim);
								BathePhase++;
								LightSwitch.Prompt.Label[0].text = "     Turn Off";
								LightSwitch.BathroomLight.SetActive(true);
								LightSwitch.GetComponent<AudioSource>().clip = LightSwitch.Flick[0];
								LightSwitch.GetComponent<AudioSource>().Play();
								InDarkness = false;
							}
							else
							{
								if (!LightSwitch.Flicker)
								{
									CharacterAnimation["f02_electrocution_00"].speed = 0.85f;
									GameObject gameObject5 = UnityEngine.Object.Instantiate(LightSwitch.Electricity, base.transform.position, Quaternion.identity);
									gameObject5.transform.parent = Bones[1].transform;
									gameObject5.transform.localPosition = Vector3.zero;
									Subtitle.UpdateLabel(SubtitleType.LightSwitchReaction, 3, 0f);
									LightSwitch.GetComponent<AudioSource>().clip = LightSwitch.Flick[2];
									LightSwitch.Flicker = true;
									LightSwitch.GetComponent<AudioSource>().Play();
									EyeShrink = 1f;
									ElectroSteam[0].SetActive(true);
									ElectroSteam[1].SetActive(true);
									ElectroSteam[2].SetActive(true);
									ElectroSteam[3].SetActive(true);
								}
								RightDrill.eulerAngles = new Vector3(UnityEngine.Random.Range(-360f, 360f), UnityEngine.Random.Range(-360f, 360f), UnityEngine.Random.Range(-360f, 360f));
								LeftDrill.eulerAngles = new Vector3(UnityEngine.Random.Range(-360f, 360f), UnityEngine.Random.Range(-360f, 360f), UnityEngine.Random.Range(-360f, 360f));
								ElectroTimer += Time.deltaTime;
								if (ElectroTimer > 0.1f)
								{
									ElectroTimer = 0f;
									if (MyRenderer.enabled)
									{
										Spook();
									}
									else
									{
										Unspook();
									}
								}
							}
						}
						else if (CharacterAnimation["f02_electrocution_00"].time > 5.9500003f && CharacterAnimation["f02_electrocution_00"].time < CharacterAnimation["f02_electrocution_00"].length)
						{
							if (LightSwitch.Flicker)
							{
								CharacterAnimation["f02_electrocution_00"].speed = 1f;
								Prompt.Label[0].text = "     Turn Off";
								LightSwitch.BathroomLight.SetActive(true);
								RightDrill.localEulerAngles = new Vector3(0f, 0f, 68.30447f);
								LeftDrill.localEulerAngles = new Vector3(0f, -180f, -159.417f);
								LightSwitch.Flicker = false;
								Unspook();
								UnWet();
							}
						}
						else if (CharacterAnimation["f02_electrocution_00"].time >= CharacterAnimation["f02_electrocution_00"].length)
						{
							Police.ElectrocutedStudentName = Name;
							Police.ElectroScene = true;
							Electrocuted = true;
							BecomeRagdoll();
							DeathType = DeathType.Electrocution;
						}
					}
				}
			}
			else if (Pathfinding.canMove)
			{
				if (BathePhase == 1 || FleeWhenClean)
				{
					CharacterAnimation.CrossFade(SprintAnim);
					Pathfinding.speed = 4f;
				}
				else
				{
					CharacterAnimation.CrossFade(WalkAnim);
					Pathfinding.speed = 1f;
				}
			}
		}
		if (Distracting)
		{
			if (DistractionTarget == null)
			{
				Distracting = false;
			}
			else if (DistractionTarget.Dying)
			{
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
				DistractionTarget.TargetedForDistraction = false;
				DistractionTarget.Distracted = false;
				DistractionTarget.EmptyHands();
				Pathfinding.speed = 1f;
				Distracting = false;
				Distracted = false;
				CanTalk = true;
				Routine = true;
			}
			else
			{
				if (Actions[Phase] == StudentActionType.ClubAction && Club == ClubType.Cooking && DistractionTarget.InEvent)
				{
					GetFoodTarget();
				}
				if (DistanceToDestination < 5f)
				{
					if (DistractionTarget.InEvent || DistractionTarget.Talking || DistractionTarget.Following || DistractionTarget.TurnOffRadio || DistractionTarget.Splashed || DistractionTarget.Shoving || DistractionTarget.Spraying || DistractionTarget.FocusOnYandere || DistractionTarget.ShoeRemoval.enabled || DistractionTarget.Posing || DistractionTarget.ClubActivityPhase >= 16 || !DistractionTarget.enabled || DistractionTarget.Alarmed || DistractionTarget.Fleeing || DistractionTarget.Wet || DistractionTarget.EatingSnack || DistractionTarget.InvestigatingBloodPool || DistractionTarget.ReturningMisplacedWeapon || StudentManager.LockerRoomArea.bounds.Contains(DistractionTarget.transform.position) || StudentManager.WestBathroomArea.bounds.Contains(DistractionTarget.transform.position) || StudentManager.EastBathroomArea.bounds.Contains(DistractionTarget.transform.position) || StudentManager.HeadmasterArea.bounds.Contains(DistractionTarget.transform.position) || (DistractionTarget.Actions[DistractionTarget.Phase] == StudentActionType.Bully && DistractionTarget.DistanceToDestination < 1f))
					{
						CurrentDestination = Destinations[Phase];
						Pathfinding.target = Destinations[Phase];
						DistractionTarget.TargetedForDistraction = false;
						Pathfinding.speed = 1f;
						Distracting = false;
						Distracted = false;
						SpeechLines.Stop();
						CanTalk = true;
						Routine = true;
						if (Actions[Phase] == StudentActionType.ClubAction && Club == ClubType.Cooking)
						{
							GetFoodTarget();
						}
					}
					else if (DistanceToDestination < TargetDistance)
					{
						if (!DistractionTarget.Distracted)
						{
							if (StudentID > 1 && DistractionTarget.StudentID > 1 && Persona != PersonaType.Fragile && DistractionTarget.Persona != PersonaType.Fragile && ((Club != ClubType.Bully && DistractionTarget.Club == ClubType.Bully) || (Club == ClubType.Bully && DistractionTarget.Club != ClubType.Bully)))
							{
								BullyPhotoCollider.SetActive(true);
							}
							if (DistractionTarget.Investigating)
							{
								DistractionTarget.StopInvestigating();
							}
							StudentManager.UpdateStudents(DistractionTarget.StudentID);
							DistractionTarget.Pathfinding.canSearch = false;
							DistractionTarget.Pathfinding.canMove = false;
							DistractionTarget.OccultBook.SetActive(false);
							DistractionTarget.SmartPhone.SetActive(false);
							DistractionTarget.Distraction = base.transform;
							DistractionTarget.CameraReacting = false;
							DistractionTarget.Pathfinding.speed = 0f;
							DistractionTarget.Pen.SetActive(false);
							DistractionTarget.Drownable = false;
							DistractionTarget.Distracted = true;
							DistractionTarget.Pushable = false;
							DistractionTarget.Routine = false;
							DistractionTarget.CanTalk = false;
							DistractionTarget.ReadPhase = 0;
							DistractionTarget.SpeechLines.Stop();
							DistractionTarget.ChalkDust.Stop();
							DistractionTarget.CleanTimer = 0f;
							DistractionTarget.EmptyHands();
							DistractionTarget.Distractor = this;
							Pathfinding.speed = 0f;
							Distracted = true;
						}
						targetRotation = Quaternion.LookRotation(new Vector3(DistractionTarget.transform.position.x, base.transform.position.y, DistractionTarget.transform.position.z) - base.transform.position);
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
						if (Actions[Phase] == StudentActionType.ClubAction && Club == ClubType.Cooking)
						{
							CharacterAnimation.CrossFade(IdleAnim);
						}
						else
						{
							DistractionTarget.SpeechLines.Play();
							SpeechLines.Play();
							CharacterAnimation.CrossFade(RandomAnim);
							if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
							{
								PickRandomAnim();
							}
						}
						DistractTimer -= Time.deltaTime;
						if (DistractTimer <= 0f)
						{
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
							DistractionTarget.TargetedForDistraction = false;
							DistractionTarget.Pathfinding.canSearch = true;
							DistractionTarget.Pathfinding.canMove = true;
							DistractionTarget.Pathfinding.speed = 1f;
							DistractionTarget.Octodog.SetActive(false);
							DistractionTarget.Distraction = null;
							DistractionTarget.Distracted = false;
							DistractionTarget.CanTalk = true;
							DistractionTarget.Routine = true;
							DistractionTarget.EquipCleaningItems();
							DistractionTarget.EatingSnack = false;
							Private = false;
							DistractionTarget.CurrentDestination = DistractionTarget.Destinations[Phase];
							DistractionTarget.Pathfinding.target = DistractionTarget.Destinations[Phase];
							if (DistractionTarget.Persona == PersonaType.PhoneAddict)
							{
								DistractionTarget.SmartPhone.SetActive(true);
							}
							DistractionTarget.Distractor = null;
							DistractionTarget.SpeechLines.Stop();
							SpeechLines.Stop();
							BullyPhotoCollider.SetActive(false);
							Pathfinding.speed = 1f;
							Distracting = false;
							Distracted = false;
							CanTalk = true;
							Routine = true;
							if (Actions[Phase] == StudentActionType.ClubAction && Club == ClubType.Cooking)
							{
								GetFoodTarget();
							}
						}
					}
					else if (Actions[Phase] == StudentActionType.ClubAction && Club == ClubType.Cooking)
					{
						Pathfinding.canSearch = true;
						Pathfinding.canMove = true;
					}
					else if (Pathfinding.speed == 1f)
					{
						CharacterAnimation.CrossFade(WalkAnim);
					}
					else
					{
						CharacterAnimation.CrossFade(SprintAnim);
					}
				}
				else if (Actions[Phase] == StudentActionType.ClubAction && Club == ClubType.Cooking)
				{
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					if (Phase < ScheduleBlocks.Length - 1 && Clock.HourTime >= ScheduleBlocks[Phase].time)
					{
						Routine = true;
					}
				}
				else if (Pathfinding.speed == 1f)
				{
					CharacterAnimation.CrossFade(WalkAnim);
				}
				else
				{
					CharacterAnimation.CrossFade(SprintAnim);
				}
			}
		}
		if (Hunting)
		{
			if (HuntTarget != null)
			{
				if (HuntTarget.Prompt.enabled)
				{
					HuntTarget.Prompt.Hide();
					HuntTarget.Prompt.enabled = false;
				}
				Pathfinding.target = HuntTarget.transform;
				CurrentDestination = HuntTarget.transform;
				if (HuntTarget.Alive && !HuntTarget.Tranquil && !HuntTarget.PinningDown)
				{
					if (DistanceToDestination > TargetDistance)
					{
						if (MurderSuicidePhase == 0)
						{
							if (CharacterAnimation["f02_brokenStandUp_00"].time >= CharacterAnimation["f02_brokenStandUp_00"].length)
							{
								MurderSuicidePhase++;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								CharacterAnimation.CrossFade(WalkAnim);
								Pathfinding.speed = 1f;
							}
						}
						else if (MurderSuicidePhase > 1)
						{
							CharacterAnimation.CrossFade(WalkAnim);
							HuntTarget.MoveTowardsTarget(base.transform.position + base.transform.forward * 0.01f);
						}
						if (HuntTarget.Dying || HuntTarget.Ragdoll.enabled)
						{
							Hunting = false;
							Suicide = true;
						}
					}
					else if (HuntTarget.ClubActivityPhase >= 16)
					{
						CharacterAnimation.CrossFade(IdleAnim);
					}
					else if (!NEStairs.bounds.Contains(base.transform.position) && !NWStairs.bounds.Contains(base.transform.position) && !SEStairs.bounds.Contains(base.transform.position) && !SWStairs.bounds.Contains(base.transform.position) && !NEStairs.bounds.Contains(HuntTarget.transform.position) && !NWStairs.bounds.Contains(HuntTarget.transform.position) && !SEStairs.bounds.Contains(HuntTarget.transform.position) && !SWStairs.bounds.Contains(HuntTarget.transform.position))
					{
						if (Pathfinding.canMove)
						{
							CharacterAnimation.CrossFade("f02_murderSuicide_00");
							Pathfinding.canSearch = false;
							Pathfinding.canMove = false;
							Broken.Subtitle.text = string.Empty;
							MyController.radius = 0f;
							Broken.Done = true;
							AudioSource.PlayClipAtPoint(MurderSuicideSounds, base.transform.position + new Vector3(0f, 1f, 0f));
							AudioSource component2 = GetComponent<AudioSource>();
							component2.clip = MurderSuicideKiller;
							component2.Play();
							if (HuntTarget.Shoving)
							{
								Yandere.CannotRecover = false;
							}
							if (StudentManager.CombatMinigame.Delinquent == HuntTarget)
							{
								StudentManager.CombatMinigame.Stop();
								StudentManager.CombatMinigame.ReleaseYandere();
							}
							HuntTarget.HipCollider.enabled = true;
							HuntTarget.HipCollider.radius = 1f;
							HuntTarget.DetectionMarker.Tex.enabled = false;
							HuntTarget.TargetedForDistraction = false;
							HuntTarget.Pathfinding.canSearch = false;
							HuntTarget.Pathfinding.canMove = false;
							HuntTarget.WitnessCamera.Show = false;
							HuntTarget.CameraReacting = false;
							HuntTarget.Investigating = false;
							HuntTarget.Distracting = false;
							HuntTarget.Splashed = false;
							HuntTarget.Alarmed = false;
							HuntTarget.Burning = false;
							HuntTarget.Fleeing = false;
							HuntTarget.Routine = false;
							HuntTarget.Shoving = false;
							HuntTarget.Tripped = false;
							HuntTarget.Wet = false;
							HuntTarget.Prompt.Hide();
							HuntTarget.Prompt.enabled = false;
							if (Yandere.Pursuer == HuntTarget)
							{
								Yandere.Chased = false;
								Yandere.Pursuer = null;
							}
							if (!HuntTarget.Male)
							{
								HuntTarget.CharacterAnimation.CrossFade("f02_murderSuicide_01");
							}
							else
							{
								HuntTarget.CharacterAnimation.CrossFade("murderSuicide_01");
							}
							HuntTarget.Subtitle.UpdateLabel(SubtitleType.Dying, 0, 1f);
							HuntTarget.MyController.radius = 0f;
							HuntTarget.SpeechLines.Stop();
							HuntTarget.EyeShrink = 1f;
							HuntTarget.GetComponent<AudioSource>().clip = HuntTarget.MurderSuicideVictim;
							HuntTarget.GetComponent<AudioSource>().Play();
							Police.CorpseList[Police.Corpses] = HuntTarget.Ragdoll;
							Police.Corpses++;
							GameObjectUtils.SetLayerRecursively(HuntTarget.gameObject, 11);
							HuntTarget.tag = "Blood";
							HuntTarget.Ragdoll.MurderSuicideAnimation = true;
							HuntTarget.Ragdoll.Disturbing = true;
							HuntTarget.Dying = true;
							HuntTarget.MurderSuicidePhase = 100;
							HuntTarget.SpawnAlarmDisc();
							if (HuntTarget.Following)
							{
								Yandere.Followers--;
								ParticleSystem.EmissionModule emission4 = Hearts.emission;
								emission4.enabled = false;
								HuntTarget.Following = false;
							}
							OriginalYPosition = HuntTarget.transform.position.y;
							if (MurderSuicidePhase == 0)
							{
								MurderSuicidePhase++;
							}
						}
						else
						{
							if (MurderSuicidePhase == 0 && CharacterAnimation["f02_brokenStandUp_00"].time >= CharacterAnimation["f02_brokenStandUp_00"].length)
							{
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
							}
							if (MurderSuicidePhase > 0)
							{
								HuntTarget.targetRotation = Quaternion.LookRotation(HuntTarget.transform.position - base.transform.position);
								HuntTarget.transform.rotation = Quaternion.Slerp(HuntTarget.transform.rotation, HuntTarget.targetRotation, Time.deltaTime * 10f);
								HuntTarget.MoveTowardsTarget(base.transform.position + base.transform.forward * 0.01f);
								HuntTarget.transform.position = new Vector3(HuntTarget.transform.position.x, OriginalYPosition, HuntTarget.transform.position.z);
								targetRotation = Quaternion.LookRotation(HuntTarget.transform.position - base.transform.position);
								base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 10f);
							}
							if (MurderSuicidePhase == 1)
							{
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 2.4f)
								{
									MyWeapon.transform.parent = ItemParent;
									MyWeapon.transform.localScale = new Vector3(1f, 1f, 1f);
									MyWeapon.transform.localPosition = Vector3.zero;
									MyWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
									MurderSuicidePhase++;
								}
							}
							else if (MurderSuicidePhase == 2)
							{
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 3.3f)
								{
									GameObject gameObject6 = UnityEngine.Object.Instantiate(Ragdoll.BloodPoolSpawner.BloodPool, base.transform.position + base.transform.up * 0.012f + base.transform.forward, Quaternion.identity);
									gameObject6.transform.localEulerAngles = new Vector3(90f, UnityEngine.Random.Range(0f, 360f), 0f);
									gameObject6.transform.parent = Police.BloodParent;
									MyWeapon.Victims[HuntTarget.StudentID] = true;
									MyWeapon.Blood.enabled = true;
									if (!MyWeapon.Evidence)
									{
										MyWeapon.MurderWeapon = true;
										MyWeapon.Evidence = true;
										Police.MurderWeapons++;
									}
									UnityEngine.Object.Instantiate(BloodEffect, MyWeapon.transform.position, Quaternion.identity);
									KnifeDown = true;
									MurderSuicidePhase++;
								}
							}
							else if (MurderSuicidePhase == 3)
							{
								if (!KnifeDown)
								{
									if (MyWeapon.transform.position.y < base.transform.position.y + 1f / 3f)
									{
										UnityEngine.Object.Instantiate(BloodEffect, MyWeapon.transform.position, Quaternion.identity);
										KnifeDown = true;
									}
								}
								else if (MyWeapon.transform.position.y > base.transform.position.y + 1f / 3f)
								{
									KnifeDown = false;
								}
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 16.666666f)
								{
									MyWeapon.transform.parent = null;
									MurderSuicidePhase++;
								}
							}
							else if (MurderSuicidePhase == 4)
							{
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 18.9f)
								{
									UnityEngine.Object.Instantiate(BloodEffect, MyWeapon.transform.position, Quaternion.identity);
									MyWeapon.transform.parent = ItemParent;
									MyWeapon.transform.localPosition = Vector3.zero;
									MyWeapon.transform.localEulerAngles = Vector3.zero;
									MurderSuicidePhase++;
								}
							}
							else if (MurderSuicidePhase == 5)
							{
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 26.166666f)
								{
									UnityEngine.Object.Instantiate(BloodEffect, MyWeapon.transform.position, Quaternion.identity);
									MyWeapon.Victims[StudentID] = true;
									MurderSuicidePhase++;
								}
							}
							else if (MurderSuicidePhase == 6)
							{
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 30.5f)
								{
									BloodFountain.Play();
									MurderSuicidePhase++;
								}
							}
							else if (MurderSuicidePhase == 7)
							{
								if (CharacterAnimation["f02_murderSuicide_00"].time >= 31.5f)
								{
									BloodSprayCollider.SetActive(true);
									MurderSuicidePhase++;
								}
							}
							else if (CharacterAnimation["f02_murderSuicide_00"].time >= CharacterAnimation["f02_murderSuicide_00"].length)
							{
								MyWeapon.transform.parent = null;
								MyWeapon.Drop();
								MyWeapon = null;
								StudentManager.StopHesitating();
								HuntTarget.HipCollider.radius = 0.5f;
								HuntTarget.BecomeRagdoll();
								HuntTarget.Ragdoll.Disturbing = false;
								HuntTarget.Ragdoll.MurderSuicide = true;
								HuntTarget.DeathType = DeathType.Weapon;
								if (BloodSprayCollider != null)
								{
									BloodSprayCollider.SetActive(false);
								}
								BecomeRagdoll();
								DeathType = DeathType.Weapon;
								StudentManager.MurderTakingPlace = false;
								Police.MurderSuicideScene = true;
								Ragdoll.MurderSuicide = true;
								MurderSuicide = true;
								Broken.HairPhysics[0].enabled = true;
								Broken.HairPhysics[1].enabled = true;
								Broken.enabled = false;
							}
						}
					}
				}
				else
				{
					Hunting = false;
					Suicide = true;
				}
			}
			else
			{
				Hunting = false;
				Suicide = true;
			}
		}
		if (Suicide)
		{
			if (MurderSuicidePhase == 0)
			{
				if (CharacterAnimation["f02_brokenStandUp_00"].time >= CharacterAnimation["f02_brokenStandUp_00"].length)
				{
					MurderSuicidePhase++;
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Pathfinding.speed = 0f;
					CharacterAnimation.CrossFade("f02_suicide_00");
				}
			}
			else if (MurderSuicidePhase == 1)
			{
				if (Pathfinding.speed > 0f)
				{
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Pathfinding.speed = 0f;
					CharacterAnimation.CrossFade("f02_suicide_00");
				}
				if (CharacterAnimation["f02_suicide_00"].time >= 11f / 15f)
				{
					MyWeapon.transform.parent = ItemParent;
					MyWeapon.transform.localScale = new Vector3(1f, 1f, 1f);
					MyWeapon.transform.localPosition = Vector3.zero;
					MyWeapon.transform.localEulerAngles = Vector3.zero;
					Broken.Subtitle.text = string.Empty;
					Broken.Done = true;
					MurderSuicidePhase++;
				}
			}
			else if (MurderSuicidePhase == 2)
			{
				if (CharacterAnimation["f02_suicide_00"].time >= 4.1666665f)
				{
					UnityEngine.Object.Instantiate(StabBloodEffect, MyWeapon.transform.position, Quaternion.identity);
					MyWeapon.Victims[StudentID] = true;
					MyWeapon.Blood.enabled = true;
					if (!MyWeapon.Evidence)
					{
						MyWeapon.Evidence = true;
						Police.MurderWeapons++;
					}
					MurderSuicidePhase++;
				}
			}
			else if (MurderSuicidePhase == 3)
			{
				if (CharacterAnimation["f02_suicide_00"].time >= 6.1666665f)
				{
					BloodFountain.gameObject.GetComponent<AudioSource>().Play();
					BloodFountain.Play();
					MurderSuicidePhase++;
				}
			}
			else if (MurderSuicidePhase == 4)
			{
				if (CharacterAnimation["f02_suicide_00"].time >= 7f)
				{
					Ragdoll.BloodPoolSpawner.SpawnPool(base.transform);
					BloodSprayCollider.SetActive(true);
					MurderSuicidePhase++;
				}
			}
			else if (MurderSuicidePhase == 5 && CharacterAnimation["f02_suicide_00"].time >= CharacterAnimation["f02_suicide_00"].length)
			{
				MyWeapon.transform.parent = null;
				MyWeapon.Drop();
				MyWeapon = null;
				StudentManager.StopHesitating();
				if (BloodSprayCollider != null)
				{
					BloodSprayCollider.SetActive(false);
				}
				BecomeRagdoll();
				DeathType = DeathType.Weapon;
				Broken.HairPhysics[0].enabled = true;
				Broken.HairPhysics[1].enabled = true;
				Broken.enabled = false;
			}
		}
		if (CameraReacting)
		{
			PhotoPatience = Mathf.MoveTowards(PhotoPatience, 0f, Time.deltaTime);
			Prompt.Circle[0].fillAmount = 1f;
			targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			if (!Yandere.ClubAccessories[7].activeInHierarchy || Club == ClubType.Delinquent)
			{
				if (CameraReactPhase == 1)
				{
					if (CharacterAnimation[CameraAnims[1]].time >= CharacterAnimation[CameraAnims[1]].length)
					{
						CharacterAnimation.CrossFade(CameraAnims[2]);
						CameraReactPhase = 2;
						CameraPoseTimer = 1f;
					}
				}
				else if (CameraReactPhase == 2)
				{
					CameraPoseTimer -= Time.deltaTime;
					if (CameraPoseTimer <= 0f)
					{
						CharacterAnimation.CrossFade(CameraAnims[3]);
						CameraReactPhase = 3;
					}
				}
				else if (CameraReactPhase == 3)
				{
					if (CameraPoseTimer == 1f)
					{
						CharacterAnimation.CrossFade(CameraAnims[2]);
						CameraReactPhase = 2;
					}
					if (CharacterAnimation[CameraAnims[3]].time >= CharacterAnimation[CameraAnims[3]].length)
					{
						if (Persona == PersonaType.PhoneAddict || Sleuthing)
						{
							SmartPhone.SetActive(true);
						}
						if (!Male && Cigarette.activeInHierarchy)
						{
							SmartPhone.SetActive(false);
						}
						CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
						Obstacle.enabled = false;
						CameraReacting = false;
						Routine = true;
						ReadPhase = 0;
						if (Actions[Phase] == StudentActionType.Clean)
						{
							Scrubber.SetActive(true);
							if (CleaningRole == 5)
							{
								Eraser.SetActive(true);
							}
						}
					}
				}
			}
			else if (Yandere.Shutter.TargetStudent != StudentID)
			{
				CameraPoseTimer = Mathf.MoveTowards(CameraPoseTimer, 0f, Time.deltaTime);
				if (CameraPoseTimer == 0f)
				{
					if (Persona == PersonaType.PhoneAddict || Sleuthing)
					{
						SmartPhone.SetActive(true);
					}
					CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
					Obstacle.enabled = false;
					CameraReacting = false;
					Routine = true;
					ReadPhase = 0;
					if (Actions[Phase] == StudentActionType.Clean)
					{
						Scrubber.SetActive(true);
						if (CleaningRole == 5)
						{
							Eraser.SetActive(true);
						}
					}
				}
			}
			else
			{
				CameraPoseTimer = 1f;
			}
			if (InEvent)
			{
				CameraReacting = false;
				ReadPhase = 0;
			}
		}
		if (Investigating)
		{
			if (!YandereInnocent && InvestigationPhase < 100 && CanSeeObject(Yandere.gameObject, Yandere.HeadPosition))
			{
				if (Vector3.Distance(Yandere.transform.position, Giggle.transform.position) > 2.5f)
				{
					YandereInnocent = true;
				}
				else
				{
					CharacterAnimation.CrossFade(IdleAnim);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					InvestigationPhase = 100;
					InvestigationTimer = 0f;
				}
			}
			if (InvestigationPhase == 0)
			{
				if (InvestigationTimer < 5f)
				{
					if (Persona == PersonaType.Heroic)
					{
						InvestigationTimer += Time.deltaTime * 5f;
					}
					else
					{
						InvestigationTimer += Time.deltaTime;
					}
					targetRotation = Quaternion.LookRotation(new Vector3(Giggle.transform.position.x, base.transform.position.y, Giggle.transform.position.z) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				}
				else
				{
					CharacterAnimation.CrossFade(IdleAnim);
					Pathfinding.target = Giggle.transform;
					CurrentDestination = Giggle.transform;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					if (Persona == PersonaType.Heroic)
					{
						Pathfinding.speed = 4f;
					}
					else
					{
						Pathfinding.speed = 1f;
					}
					InvestigationPhase++;
				}
			}
			else if (InvestigationPhase == 1)
			{
				if (DistanceToDestination > 1f)
				{
					if (Persona == PersonaType.Heroic)
					{
						CharacterAnimation.CrossFade(SprintAnim);
					}
					else
					{
						CharacterAnimation.CrossFade(WalkAnim);
					}
				}
				else if (InvestigationPhase == 1)
				{
					CharacterAnimation.CrossFade(IdleAnim);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					InvestigationPhase++;
				}
			}
			else if (InvestigationPhase == 2)
			{
				InvestigationTimer += Time.deltaTime;
				if (InvestigationTimer > 10f)
				{
					StopInvestigating();
				}
			}
			else if (InvestigationPhase == 100)
			{
				targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				InvestigationTimer += Time.deltaTime;
				if (InvestigationTimer > 2f)
				{
					StopInvestigating();
				}
			}
		}
		if (EndSearch)
		{
			MoveTowardsTarget(Pathfinding.target.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Pathfinding.target.rotation, 10f * Time.deltaTime);
			PatrolTimer += Time.deltaTime * CharacterAnimation[PatrolAnim].speed;
			if (PatrolTimer > 5f)
			{
				StudentManager.CommunalLocker.RivalPhone.gameObject.SetActive(false);
				ScheduleBlock scheduleBlock10 = ScheduleBlocks[2];
				scheduleBlock10.destination = "Hangout";
				scheduleBlock10.action = "Hangout";
				ScheduleBlock scheduleBlock11 = ScheduleBlocks[4];
				scheduleBlock11.destination = "LunchSpot";
				scheduleBlock11.action = "Eat";
				ScheduleBlock scheduleBlock12 = ScheduleBlocks[7];
				scheduleBlock12.destination = "Hangout";
				scheduleBlock12.action = "Hangout";
				GetDestinations();
				Array.Copy(OriginalActions, Actions, OriginalActions.Length);
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
				DistanceToDestination = 100f;
				LewdPhotos = StudentManager.CommunalLocker.RivalPhone.LewdPhotos;
				EndSearch = false;
				Phoneless = false;
				Routine = true;
			}
		}
		if (Shoving)
		{
			CharacterAnimation.CrossFade(ShoveAnim);
			if (CharacterAnimation[ShoveAnim].time > CharacterAnimation[ShoveAnim].length)
			{
				if (Club != ClubType.Council && Persona != PersonaType.Violent)
				{
					Patience = 999;
				}
				if (Patience > 0)
				{
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					Distracted = false;
					Shoving = false;
					Routine = true;
					Paired = false;
				}
				else if (Club == ClubType.Council)
				{
					Shoving = false;
					Spray();
				}
				else
				{
					SpawnAlarmDisc();
					SmartPhone.SetActive(false);
					Distracted = true;
					Threatened = true;
					Shoving = false;
					Alarmed = true;
				}
			}
		}
		if (Spraying && (double)CharacterAnimation[SprayAnim].time > 0.66666)
		{
			if (Yandere.Armed)
			{
				Yandere.EquippedWeapon.Drop();
			}
			Yandere.EmptyHands();
			PepperSprayEffect.Play();
			Spraying = false;
		}
		if (SentHome)
		{
			if (SentHomePhase == 0)
			{
				if (Shy)
				{
					CharacterAnimation[ShyAnim].weight = 0f;
				}
				CharacterAnimation.CrossFade(SentHomeAnim);
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				SentHomePhase++;
				if (SmartPhone.activeInHierarchy)
				{
					CharacterAnimation[SentHomeAnim].time = 1.5f;
					SentHomePhase++;
				}
			}
			else if (SentHomePhase == 1)
			{
				if (CharacterAnimation[SentHomeAnim].time > 0.66666f)
				{
					SmartPhone.SetActive(true);
					SentHomePhase++;
				}
			}
			else if (SentHomePhase == 2 && CharacterAnimation[SentHomeAnim].time > CharacterAnimation[SentHomeAnim].length)
			{
				SprintAnim = OriginalSprintAnim;
				CharacterAnimation.CrossFade(SprintAnim);
				CurrentDestination = StudentManager.Exit;
				Pathfinding.target = StudentManager.Exit;
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				SmartPhone.SetActive(false);
				Pathfinding.speed = 4f;
				SentHomePhase++;
			}
		}
		if (DramaticReaction)
		{
			DramaticCamera.transform.Translate(Vector3.forward * Time.deltaTime * 0.01f);
			if (DramaticPhase == 0)
			{
				DramaticCamera.rect = new Rect(0f, Mathf.Lerp(DramaticCamera.rect.y, 0.25f, Time.deltaTime * 10f), 1f, Mathf.Lerp(DramaticCamera.rect.height, 0.5f, Time.deltaTime * 10f));
				DramaticTimer += Time.deltaTime;
				if (DramaticTimer > 1f)
				{
					DramaticTimer = 0f;
					DramaticPhase++;
				}
			}
			else if (DramaticPhase == 1)
			{
				DramaticCamera.rect = new Rect(0f, Mathf.Lerp(DramaticCamera.rect.y, 0.5f, Time.deltaTime * 10f), 1f, Mathf.Lerp(DramaticCamera.rect.height, 0f, Time.deltaTime * 10f));
				DramaticTimer += Time.deltaTime;
				if (DramaticCamera.rect.height < 0.01f || DramaticTimer > 0.5f)
				{
					DramaticCamera.gameObject.SetActive(false);
					AttackReaction();
					DramaticPhase++;
				}
			}
		}
		if (HitReacting && CharacterAnimation[SithReactAnim].time >= CharacterAnimation[SithReactAnim].length)
		{
			Persona = PersonaType.SocialButterfly;
			PersonaReaction();
			HitReacting = false;
		}
		if (Eaten)
		{
			if (Yandere.Eating)
			{
				targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			}
			if (CharacterAnimation[EatVictimAnim].time >= CharacterAnimation[EatVictimAnim].length)
			{
				BecomeRagdoll();
			}
		}
		if (Electrified && CharacterAnimation[ElectroAnim].time >= CharacterAnimation[ElectroAnim].length)
		{
			Electrocuted = true;
			BecomeRagdoll();
			DeathType = DeathType.Electrocution;
		}
		if (BreakingUpFight)
		{
			targetRotation = Yandere.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward * 0.5f);
			if (StudentID == 87)
			{
				if (CharacterAnimation[BreakUpAnim].time >= 4f)
				{
					CandyBar.SetActive(false);
				}
				else if ((double)CharacterAnimation[BreakUpAnim].time >= 0.16666666666)
				{
					CandyBar.SetActive(true);
				}
			}
			if (CharacterAnimation[BreakUpAnim].time >= CharacterAnimation[BreakUpAnim].length)
			{
				ReturnToRoutine();
			}
		}
		if (Tripping)
		{
			MoveTowardsTarget(new Vector3(0f, 0f, base.transform.position.z));
			CharacterAnimation.CrossFade("trip_00");
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			if (CharacterAnimation["trip_00"].time >= 0.5f && CharacterAnimation["trip_00"].time <= 5.5f && StudentManager.Gate.Crushing)
			{
				BecomeRagdoll();
				Ragdoll.Decapitated = true;
				UnityEngine.Object.Instantiate(SquishyBloodEffect, Head.position, Quaternion.identity);
			}
			if (CharacterAnimation["trip_00"].time >= CharacterAnimation["trip_00"].length)
			{
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				Distracted = true;
				Tripping = false;
				Routine = true;
				Tripped = true;
			}
		}
		if (SeekingMedicine)
		{
			if (StudentManager.Students[90] == null)
			{
				if (SeekMedicinePhase < 5)
				{
					SeekMedicinePhase = 5;
				}
			}
			else if ((!StudentManager.Students[90].gameObject.activeInHierarchy || StudentManager.Students[90].Dying) && SeekMedicinePhase < 5)
			{
				SeekMedicinePhase = 5;
			}
			if (SeekMedicinePhase == 0)
			{
				CurrentDestination = StudentManager.Students[90].transform;
				Pathfinding.target = StudentManager.Students[90].transform;
				SeekMedicinePhase++;
			}
			else if (SeekMedicinePhase == 1)
			{
				CharacterAnimation.CrossFade(SprintAnim);
				if (DistanceToDestination < 1f)
				{
					StudentScript studentScript2 = StudentManager.Students[90];
					if (studentScript2.Investigating)
					{
						studentScript2.StopInvestigating();
					}
					StudentManager.UpdateStudents(studentScript2.StudentID);
					studentScript2.Pathfinding.canSearch = false;
					studentScript2.Pathfinding.canMove = false;
					studentScript2.RetreivingMedicine = true;
					studentScript2.Pathfinding.speed = 0f;
					studentScript2.CameraReacting = false;
					studentScript2.TargetStudent = this;
					studentScript2.Routine = false;
					CharacterAnimation.CrossFade(IdleAnim);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Pathfinding.speed = 0f;
					Subtitle.UpdateLabel(SubtitleType.RequestMedicine, 0, 5f);
					SeekMedicinePhase++;
				}
			}
			else if (SeekMedicinePhase == 2)
			{
				StudentScript studentScript3 = StudentManager.Students[90];
				targetRotation = Quaternion.LookRotation(new Vector3(studentScript3.transform.position.x, base.transform.position.y, studentScript3.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				MedicineTimer += Time.deltaTime;
				if (MedicineTimer > 5f)
				{
					SeekMedicinePhase++;
					MedicineTimer = 0f;
					studentScript3.Pathfinding.canSearch = true;
					studentScript3.Pathfinding.canMove = true;
					studentScript3.RetrieveMedicinePhase++;
				}
			}
			else if (SeekMedicinePhase != 3)
			{
				if (SeekMedicinePhase == 4)
				{
					StudentScript studentScript4 = StudentManager.Students[90];
					targetRotation = Quaternion.LookRotation(new Vector3(studentScript4.transform.position.x, base.transform.position.y, studentScript4.transform.position.z) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				}
				else if (SeekMedicinePhase == 5)
				{
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					ScheduleBlock scheduleBlock13 = ScheduleBlocks[Phase];
					scheduleBlock13.destination = "InfirmarySeat";
					scheduleBlock13.action = "Relax";
					GetDestinations();
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
					Pathfinding.speed = 2f;
					RelaxAnim = HeadacheSitAnim;
					SeekingMedicine = false;
					Routine = true;
				}
			}
		}
		if (RetreivingMedicine)
		{
			if (RetrieveMedicinePhase == 0)
			{
				CharacterAnimation.CrossFade(IdleAnim);
				targetRotation = Quaternion.LookRotation(new Vector3(TargetStudent.transform.position.x, base.transform.position.y, TargetStudent.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			}
			else if (RetrieveMedicinePhase == 1)
			{
				CharacterAnimation.CrossFade(WalkAnim);
				CurrentDestination = StudentManager.MedicineCabinet;
				Pathfinding.target = StudentManager.MedicineCabinet;
				Pathfinding.speed = 1f;
				RetrieveMedicinePhase++;
			}
			else if (RetrieveMedicinePhase == 2)
			{
				if (DistanceToDestination < 1f)
				{
					StudentManager.CabinetDoor.Locked = false;
					StudentManager.CabinetDoor.Open = true;
					StudentManager.CabinetDoor.Timer = 0f;
					CurrentDestination = TargetStudent.transform;
					Pathfinding.target = TargetStudent.transform;
					RetrieveMedicinePhase++;
				}
			}
			else if (RetrieveMedicinePhase == 3)
			{
				if (DistanceToDestination < 1f)
				{
					CurrentDestination = TargetStudent.transform;
					Pathfinding.target = TargetStudent.transform;
					RetrieveMedicinePhase++;
				}
			}
			else if (RetrieveMedicinePhase == 4)
			{
				if (DistanceToDestination < 1f)
				{
					CharacterAnimation.CrossFade("f02_giveItem_00");
					if (TargetStudent.Male)
					{
						TargetStudent.CharacterAnimation.CrossFade("eatItem_00");
					}
					else
					{
						TargetStudent.CharacterAnimation.CrossFade("f02_eatItem_00");
					}
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					TargetStudent.SeekMedicinePhase++;
					RetrieveMedicinePhase++;
				}
			}
			else if (RetrieveMedicinePhase == 5)
			{
				targetRotation = Quaternion.LookRotation(new Vector3(TargetStudent.transform.position.x, base.transform.position.y, TargetStudent.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				MedicineTimer += Time.deltaTime;
				if (MedicineTimer > 3f)
				{
					CharacterAnimation.CrossFade(WalkAnim);
					CurrentDestination = StudentManager.MedicineCabinet;
					Pathfinding.target = StudentManager.MedicineCabinet;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					TargetStudent.SeekMedicinePhase++;
					RetrieveMedicinePhase++;
				}
			}
			else if (RetrieveMedicinePhase == 6 && DistanceToDestination < 1f)
			{
				StudentManager.CabinetDoor.Locked = true;
				StudentManager.CabinetDoor.Open = false;
				StudentManager.CabinetDoor.Timer = 0f;
				RetreivingMedicine = false;
				Routine = true;
			}
		}
		if (EatingSnack)
		{
			if (SnackPhase == 0)
			{
				CharacterAnimation.CrossFade(EatChipsAnim);
				SmartPhone.SetActive(false);
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				SnackTimer += Time.deltaTime;
				if (SnackTimer > 10f)
				{
					UnityEngine.Object.Destroy(BagOfChips);
					StudentManager.GetNearestFountain(this);
					Pathfinding.target = DrinkingFountain.DrinkPosition;
					CurrentDestination = DrinkingFountain.DrinkPosition;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					SnackTimer = 0f;
					SnackPhase++;
				}
			}
			else if (SnackPhase == 1)
			{
				CharacterAnimation.CrossFade(WalkAnim);
				if (Persona == PersonaType.PhoneAddict)
				{
					SmartPhone.SetActive(true);
				}
				if (DistanceToDestination < 1f)
				{
					SmartPhone.SetActive(false);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					SnackPhase++;
				}
			}
			else if (SnackPhase == 2)
			{
				CharacterAnimation.CrossFade(DrinkFountainAnim);
				MoveTowardsTarget(DrinkingFountain.DrinkPosition.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, DrinkingFountain.DrinkPosition.rotation, 10f * Time.deltaTime);
				if (CharacterAnimation[DrinkFountainAnim].time >= CharacterAnimation[DrinkFountainAnim].length)
				{
					DrinkingFountain.Occupied = false;
					EquipCleaningItems();
					EatingSnack = false;
					Private = false;
					Routine = true;
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
				}
				else if (CharacterAnimation[DrinkFountainAnim].time > 0.5f && CharacterAnimation[DrinkFountainAnim].time < 1.5f)
				{
					DrinkingFountain.WaterStream.Play();
				}
			}
		}
		if (Dodging)
		{
			if (CharacterAnimation[DodgeAnim].time >= CharacterAnimation[DodgeAnim].length)
			{
				Routine = true;
				Dodging = false;
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
			}
			else if (CharacterAnimation[DodgeAnim].time < 0.66666f)
			{
				MyController.Move(base.transform.forward * -1f * DodgeSpeed * Time.deltaTime);
				MyController.Move(Physics.gravity * 0.1f);
				if (DodgeSpeed > 0f)
				{
					DodgeSpeed = Mathf.MoveTowards(DodgeSpeed, 0f, Time.deltaTime * 3f);
				}
			}
		}
		if (!Guarding && InvestigatingBloodPool)
		{
			if (DistanceToDestination < 1f)
			{
				CharacterAnimation[InspectBloodAnim].speed = 1f;
				CharacterAnimation.CrossFade(InspectBloodAnim);
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				if (CharacterAnimation[InspectBloodAnim].time >= CharacterAnimation[InspectBloodAnim].length || Persona == PersonaType.Strict)
				{
					bool flag2 = false;
					if (Club == ClubType.Cooking && CurrentAction == StudentActionType.ClubAction)
					{
						flag2 = true;
					}
					if (WitnessedWeapon)
					{
						bool flag3 = false;
						if (!Teacher && BloodPool.GetComponent<WeaponScript>().Metal && StudentManager.MetalDetectors)
						{
							flag3 = true;
						}
						if (!WitnessedBloodyWeapon && StudentID > 1 && !flag3 && CurrentAction != StudentActionType.SitAndTakeNotes && Indoors && !flag2 && !BloodPool.GetComponent<WeaponScript>().Dangerous && BloodPool.GetComponent<WeaponScript>().Returner == null)
						{
							CharacterAnimation[PickUpAnim].time = 0f;
							BloodPool.GetComponent<WeaponScript>().Prompt.Hide();
							BloodPool.GetComponent<WeaponScript>().Prompt.enabled = false;
							BloodPool.GetComponent<WeaponScript>().enabled = false;
							BloodPool.GetComponent<WeaponScript>().Returner = this;
							Subtitle.UpdateLabel(SubtitleType.ReturningWeapon, 0, 5f);
							ReturningMisplacedWeapon = true;
						}
					}
					InvestigatingBloodPool = false;
					WitnessCooldownTimer = 5f;
					if (WitnessedLimb)
					{
						SpawnAlarmDisc();
					}
					if (!ReturningMisplacedWeapon)
					{
						if (StudentManager.BloodReporter == this && WitnessedWeapon && !BloodPool.GetComponent<WeaponScript>().Dangerous)
						{
							StudentManager.BloodReporter = null;
						}
						if (StudentManager.BloodReporter == this && StudentID > 1)
						{
							if (Persona != PersonaType.Strict)
							{
								Persona = PersonaType.TeachersPet;
							}
							PersonaReaction();
						}
						else if (WitnessedWeapon && !WitnessedBloodyWeapon)
						{
							CurrentDestination = Destinations[Phase];
							Pathfinding.target = Destinations[Phase];
							LastSuspiciousObject2 = LastSuspiciousObject;
							LastSuspiciousObject = BloodPool;
							Pathfinding.canSearch = true;
							Pathfinding.canMove = true;
							Pathfinding.speed = 1f;
							if (BloodPool.GetComponent<WeaponScript>().Dangerous)
							{
								Subtitle.UpdateLabel(SubtitleType.PetWeaponReaction, 0, 3f);
							}
							else
							{
								Subtitle.UpdateLabel(SubtitleType.PetWeaponReaction, 1, 3f);
							}
							WitnessedSomething = false;
							WitnessedWeapon = false;
							Routine = true;
							BloodPool = null;
							if (StudentManager.BloodReporter == this)
							{
								if (Persona != PersonaType.Strict)
								{
									Persona = PersonaType.TeachersPet;
								}
								PersonaReaction();
							}
						}
						else
						{
							if (Persona != PersonaType.Strict)
							{
								Persona = PersonaType.TeachersPet;
							}
							PersonaReaction();
						}
					}
				}
			}
			else if (Persona == PersonaType.Strict)
			{
				if (WitnessedWeapon && (bool)BloodPool.GetComponent<WeaponScript>().Returner)
				{
					Subtitle.UpdateLabel(SubtitleType.StudentFarewell, 0, 3f);
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
					InvestigatingBloodPool = false;
					WitnessedBloodyWeapon = false;
					WitnessedBloodPool = false;
					WitnessedSomething = false;
					WitnessedWeapon = false;
					Routine = true;
					BloodPool = null;
					WitnessCooldownTimer = 5f;
				}
				else if (BloodPool.parent == Yandere.RightHand)
				{
					InvestigatingBloodPool = false;
					WitnessedBloodyWeapon = false;
					WitnessedBloodPool = false;
					WitnessedSomething = false;
					WitnessedWeapon = false;
					Routine = true;
					BloodPool = null;
					WitnessCooldownTimer = 5f;
					AlarmTimer = 0f;
					Alarm = 200f;
				}
			}
			else if (BloodPool == null || (WitnessedWeapon && BloodPool.parent != null) || (WitnessedBloodPool && BloodPool.parent == Yandere.RightHand) || (WitnessedWeapon && (bool)BloodPool.GetComponent<WeaponScript>().Returner))
			{
				Subtitle.UpdateLabel(SubtitleType.StudentFarewell, 0, 3f);
				if (Club == ClubType.Cooking && CurrentAction == StudentActionType.ClubAction)
				{
					GetFoodTarget();
				}
				else
				{
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
				}
				InvestigatingBloodPool = false;
				WitnessedBloodyWeapon = false;
				WitnessedBloodPool = false;
				WitnessedSomething = false;
				WitnessedWeapon = false;
				Routine = true;
				BloodPool = null;
				WitnessCooldownTimer = 5f;
			}
		}
		if (ReturningMisplacedWeapon)
		{
			if (ReturningMisplacedWeaponPhase == 0)
			{
				CharacterAnimation.CrossFade(PickUpAnim);
				if ((Club == ClubType.Council || Teacher) && CharacterAnimation[PickUpAnim].time >= 0.33333f)
				{
					Handkerchief.SetActive(true);
				}
				if (CharacterAnimation[PickUpAnim].time >= 2f)
				{
					BloodPool.parent = RightHand;
					BloodPool.localPosition = new Vector3(0f, 0f, 0f);
					BloodPool.localEulerAngles = new Vector3(0f, 0f, 0f);
					if (Club != ClubType.Council && !Teacher)
					{
						BloodPool.GetComponent<WeaponScript>().FingerprintID = StudentID;
					}
				}
				if (CharacterAnimation[PickUpAnim].time >= CharacterAnimation[PickUpAnim].length)
				{
					CurrentDestination = BloodPool.GetComponent<WeaponScript>().Origin;
					Pathfinding.target = BloodPool.GetComponent<WeaponScript>().Origin;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					Pathfinding.speed = 1f;
					Hurry = false;
					ReturningMisplacedWeaponPhase++;
				}
			}
			else
			{
				CharacterAnimation.CrossFade(WalkAnim);
				if (DistanceToDestination < 1.1f)
				{
					BloodPool.parent = null;
					BloodPool.position = BloodPool.GetComponent<WeaponScript>().StartingPosition;
					BloodPool.eulerAngles = BloodPool.GetComponent<WeaponScript>().StartingRotation;
					BloodPool.GetComponent<WeaponScript>().Prompt.enabled = true;
					BloodPool.GetComponent<WeaponScript>().enabled = true;
					BloodPool.GetComponent<WeaponScript>().Drop();
					BloodPool.GetComponent<WeaponScript>().MyRigidbody.useGravity = false;
					BloodPool.GetComponent<WeaponScript>().MyRigidbody.isKinematic = false;
					BloodPool.GetComponent<WeaponScript>().Returner = null;
					BloodPool = null;
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
					if (Club == ClubType.Council || Teacher)
					{
						Handkerchief.SetActive(false);
					}
					Pathfinding.speed = 1f;
					ReturningMisplacedWeapon = false;
					WitnessedSomething = false;
					WitnessedWeapon = false;
					ReportingBlood = false;
					Routine = true;
					ReturningMisplacedWeaponPhase = 0;
					WitnessCooldownTimer = 0f;
				}
			}
		}
		if (SentToLocker && !CheckingNote)
		{
			CharacterAnimation.CrossFade(RunAnim);
		}
	}

	private void UpdateVisibleCorpses()
	{
		VisibleCorpses.Clear();
		for (ID = 0; ID < Police.Corpses; ID++)
		{
			RagdollScript ragdollScript = Police.CorpseList[ID];
			if (ragdollScript != null && !ragdollScript.Hidden)
			{
				Collider collider = ragdollScript.AllColliders[0];
				if (CanSeeObject(collider.gameObject, collider.transform.position, CorpseLayers, Mask))
				{
					VisibleCorpses.Add(ragdollScript.StudentID);
					Corpse = ragdollScript;
					if (Club == ClubType.Delinquent && Corpse.Student.Club == ClubType.Bully)
					{
						ScaredAnim = EvilWitnessAnim;
						Persona = PersonaType.Evil;
					}
					if (Persona == PersonaType.TeachersPet && StudentManager.Reporter == null && !Police.Called)
					{
						StudentManager.CorpseLocation.position = Corpse.AllColliders[0].transform.position;
						StudentManager.CorpseLocation.LookAt(base.transform.position);
						StudentManager.CorpseLocation.Translate(StudentManager.CorpseLocation.forward);
						StudentManager.LowerCorpsePosition();
						StudentManager.Reporter = this;
						ReportingMurder = true;
						DetermineCorpseLocation();
					}
				}
			}
		}
	}

	private void UpdateVisibleBlood()
	{
		ID = 0;
		while (ID < StudentManager.Blood.Length && BloodPool == null)
		{
			Collider collider = StudentManager.Blood[ID];
			if (collider != null && CanSeeObject(collider.gameObject, collider.transform.position))
			{
				BloodPool = collider.transform;
				WitnessedBloodPool = true;
				if (Club != ClubType.Delinquent && StudentManager.BloodReporter == null && !Police.Called && !LostTeacherTrust)
				{
					StudentManager.BloodLocation.position = BloodPool.position;
					StudentManager.BloodLocation.LookAt(new Vector3(base.transform.position.x, StudentManager.BloodLocation.position.y, base.transform.position.z));
					StudentManager.BloodLocation.Translate(StudentManager.BloodLocation.forward);
					StudentManager.LowerBloodPosition();
					StudentManager.BloodReporter = this;
					ReportingBlood = true;
					DetermineBloodLocation();
				}
			}
			ID++;
		}
	}

	private void UpdateVisibleLimbs()
	{
		ID = 0;
		while (ID < StudentManager.Limbs.Length && BloodPool == null)
		{
			Collider collider = StudentManager.Limbs[ID];
			if (collider != null && CanSeeObject(collider.gameObject, collider.transform.position))
			{
				BloodPool = collider.transform;
				WitnessedLimb = true;
				if (Club != ClubType.Delinquent && StudentManager.BloodReporter == null && !Police.Called && !LostTeacherTrust)
				{
					StudentManager.BloodLocation.position = BloodPool.position;
					StudentManager.BloodLocation.LookAt(new Vector3(base.transform.position.x, StudentManager.BloodLocation.position.y, base.transform.position.z));
					StudentManager.BloodLocation.Translate(StudentManager.BloodLocation.forward);
					StudentManager.LowerBloodPosition();
					StudentManager.BloodReporter = this;
					ReportingBlood = true;
					DetermineBloodLocation();
				}
			}
			ID++;
		}
	}

	private void UpdateVisibleWeapons()
	{
		for (ID = 0; ID < Yandere.WeaponManager.Weapons.Length; ID++)
		{
			if (Yandere.WeaponManager.Weapons[ID] != null && (Yandere.WeaponManager.Weapons[ID].Blood.enabled || Yandere.WeaponManager.Weapons[ID].Misplaced))
			{
				if (!(BloodPool == null))
				{
					break;
				}
				if (Yandere.WeaponManager.Weapons[ID].transform != LastSuspiciousObject && Yandere.WeaponManager.Weapons[ID].transform != LastSuspiciousObject2 && Yandere.WeaponManager.Weapons[ID].enabled)
				{
					Collider myCollider = Yandere.WeaponManager.Weapons[ID].MyCollider;
					if (myCollider != null && CanSeeObject(myCollider.gameObject, myCollider.transform.position))
					{
						BloodPool = myCollider.transform;
						if (Yandere.WeaponManager.Weapons[ID].Blood.enabled)
						{
							WitnessedBloodyWeapon = true;
						}
						WitnessedWeapon = true;
						if (Club != ClubType.Delinquent && StudentManager.BloodReporter == null && !Police.Called && !LostTeacherTrust)
						{
							StudentManager.BloodLocation.position = BloodPool.position;
							StudentManager.BloodLocation.LookAt(new Vector3(base.transform.position.x, StudentManager.BloodLocation.position.y, base.transform.position.z));
							StudentManager.BloodLocation.Translate(StudentManager.BloodLocation.forward);
							StudentManager.LowerBloodPosition();
							StudentManager.BloodReporter = this;
							ReportingBlood = true;
							DetermineBloodLocation();
						}
					}
				}
			}
		}
	}

	private void UpdateVision()
	{
		if (!Distracted)
		{
			bool flag = true;
			if (Yandere.Pursuer == null && Yandere.Pursuer == this)
			{
				flag = false;
			}
			if (!WitnessedMurder && !CheckingNote && !Shoving && !Struggling && flag)
			{
				if (Police.Corpses > 0)
				{
					UpdateVisibleCorpses();
					if (VisibleCorpses.Count > 0)
					{
						if (!WitnessedCorpse)
						{
							if (Club == ClubType.Delinquent && Corpse.Student.Club == ClubType.Bully)
							{
								FoundEnemyCorpse = true;
							}
							if (Persona == PersonaType.Sleuth)
							{
								if (Sleuthing)
								{
									Persona = PersonaType.PhoneAddict;
									SmartPhone.SetActive(true);
								}
								else
								{
									Persona = PersonaType.SocialButterfly;
								}
							}
							Pathfinding.canSearch = false;
							Pathfinding.canMove = false;
							if (!Male)
							{
								CharacterAnimation["f02_smile_00"].weight = 0f;
							}
							AlarmTimer = 0f;
							Alarm = 200f;
							LastKnownCorpse = Corpse.AllColliders[0].transform.position;
							WitnessedBloodyWeapon = false;
							WitnessedBloodPool = false;
							WitnessedSomething = false;
							WitnessedWeapon = false;
							WitnessedCorpse = true;
							WitnessedLimb = false;
							if (ReturningMisplacedWeapon && ReturningMisplacedWeapon)
							{
								DropMisplacedWeapon();
							}
							InvestigatingBloodPool = false;
							Investigating = false;
							EatingSnack = false;
							Threatened = false;
							SentHome = false;
							Routine = false;
							if (Persona == PersonaType.Spiteful && ((Bullied && Corpse.Student.Club == ClubType.Bully) || Corpse.Student.Bullied))
							{
								ScaredAnim = EvilWitnessAnim;
								Persona = PersonaType.Evil;
							}
							ForgetRadio();
							if (Wet)
							{
								Persona = PersonaType.Loner;
							}
							if (Corpse.Disturbing)
							{
								if (Corpse.BurningAnimation)
								{
									WalkBackTimer = 5f;
									WalkBack = true;
									Hesitation = 0.5f;
								}
								if (Corpse.MurderSuicideAnimation)
								{
									WitnessedMindBrokenMurder = true;
									WalkBackTimer = 5f;
									WalkBack = true;
									Hesitation = 1f;
								}
								if (Corpse.ChokingAnimation)
								{
									WalkBackTimer = 0f;
									WalkBack = true;
									Hesitation = 0.6f;
								}
								if (Corpse.ElectrocutionAnimation)
								{
									WalkBackTimer = 0f;
									WalkBack = true;
									Hesitation = 0.5f;
								}
							}
							StudentManager.UpdateMe(StudentID);
							if (!Teacher)
							{
								SpawnAlarmDisc();
							}
							else
							{
								AlarmTimer = 3f;
							}
							ParticleSystem.EmissionModule emission = Hearts.emission;
							if (Talking)
							{
								DialogueWheel.End();
								emission.enabled = false;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								Obstacle.enabled = false;
								Talk.enabled = false;
								Talking = false;
								Waiting = false;
								StudentManager.EnablePrompts();
							}
							if (Following)
							{
								emission.enabled = false;
								Yandere.Followers--;
								Following = false;
							}
						}
						if (Corpse.Dragged || Corpse.Dumped)
						{
							if (Teacher)
							{
								Subtitle.UpdateLabel(SubtitleType.TeacherMurderReaction, UnityEngine.Random.Range(1, 3), 3f);
								StudentManager.Portal.SetActive(false);
							}
							if (!Yandere.Egg)
							{
								WitnessMurder();
							}
						}
					}
				}
				if (VisibleCorpses.Count == 0 && !WitnessedCorpse)
				{
					if (WitnessCooldownTimer > 0f)
					{
						WitnessCooldownTimer = Mathf.MoveTowards(WitnessCooldownTimer, 0f, Time.deltaTime);
					}
					else if ((StudentID == StudentManager.CurrentID || (Persona == PersonaType.Strict && Fleeing)) && !Wet && !Guarding && !IgnoreBlood && !InvestigatingPossibleDeath && !Emetic && !Sedated && !Headache)
					{
						if (BloodPool == null && StudentManager.Police.LimbParent.childCount > 0)
						{
							UpdateVisibleLimbs();
						}
						if (BloodPool == null && (Police.BloodyWeapons > 0 || Yandere.WeaponManager.MisplacedWeapons > 0) && !InvestigatingPossibleLimb)
						{
							UpdateVisibleWeapons();
						}
						if (BloodPool == null && StudentManager.Police.BloodParent.childCount > 0 && !InvestigatingPossibleLimb)
						{
							UpdateVisibleBlood();
						}
						if (BloodPool != null && !WitnessedSomething)
						{
							Pathfinding.canSearch = false;
							Pathfinding.canMove = false;
							if (!Male)
							{
								CharacterAnimation["f02_smile_00"].weight = 0f;
							}
							AlarmTimer = 0f;
							Alarm = 200f;
							LastKnownBlood = BloodPool.transform.position;
							WitnessedSomething = true;
							Investigating = false;
							EatingSnack = false;
							Threatened = false;
							SentHome = false;
							Routine = false;
							ForgetRadio();
							StudentManager.UpdateMe(StudentID);
							if (Teacher)
							{
								AlarmTimer = 3f;
							}
							ParticleSystem.EmissionModule emission2 = Hearts.emission;
							if (Talking)
							{
								DialogueWheel.End();
								emission2.enabled = false;
								Pathfinding.canSearch = true;
								Pathfinding.canMove = true;
								Obstacle.enabled = false;
								Talk.enabled = false;
								Talking = false;
								Waiting = false;
								StudentManager.EnablePrompts();
							}
							if (Following)
							{
								emission2.enabled = false;
								Yandere.Followers--;
								Following = false;
							}
						}
					}
				}
				PreviousAlarm = Alarm;
				if (DistanceToPlayer < VisionDistance - ChameleonBonus)
				{
					if (!Talking && !Spraying && !SentHome)
					{
						if (!Yandere.Noticed)
						{
							bool flag2 = false;
							if (Guarding || Fleeing || InvestigatingBloodPool)
							{
								flag2 = true;
							}
							if ((Yandere.Armed && Yandere.EquippedWeapon.Suspicious) || (!Teacher && StudentID > 1 && !Teacher && Yandere.PickUp != null && Yandere.PickUp.Suspicious) || (Teacher && Yandere.PickUp != null && Yandere.PickUp.Suspicious && !Yandere.PickUp.CleaningProduct) || (Yandere.Bloodiness > 0f && !Yandere.Paint) || Yandere.Sanity < 33.333f || Yandere.Attacking || Yandere.Struggling || Yandere.Dragging || Yandere.Lewd || Yandere.Carrying || Yandere.Medusa || Yandere.Poisoning || Yandere.WeaponTimer > 0f || Yandere.MurderousActionTimer > 0f || (Yandere.PickUp != null && Yandere.PickUp.BodyPart != null) || (Yandere.Laughing && Yandere.LaughIntensity > 15f) || Yandere.Stance.Current == StanceType.Crouching || Yandere.Stance.Current == StanceType.Crawling || (Private && Yandere.Trespassing) || (Teacher && !WitnessedCorpse && Yandere.Trespassing) || (Teacher && Yandere.Rummaging) || (Teacher && Yandere.TheftTimer > 0f) || (StudentID == 1 && Yandere.NearSenpai && !Yandere.Talking) || (Yandere.Eavesdropping && Private) || (!StudentManager.CombatMinigame.Practice && Yandere.DelinquentFighting && StudentManager.CombatMinigame.Path < 4) || (flag2 && Yandere.PickUp != null && Yandere.PickUp.Mop != null && Yandere.PickUp.Mop.Bloodiness > 0f) || (flag2 && Yandere.PickUp != null && Yandere.PickUp.BodyPart != null) || (Yandere.PickUp != null && Yandere.PickUp.Clothing && Yandere.PickUp.Evidence))
							{
								if (CanSeeObject(Yandere.gameObject, Yandere.HeadPosition))
								{
									YandereVisible = true;
									if (Yandere.Attacking || Yandere.Struggling || (Yandere.NearBodies > 0 && Yandere.Bloodiness > 0f && !Yandere.Paint) || (Yandere.NearBodies > 0 && Yandere.Armed) || (Yandere.NearBodies > 0 && Yandere.Sanity < 66.66666f) || Yandere.Carrying || Yandere.Dragging || Yandere.MurderousActionTimer > 0f || (Guarding && Yandere.Bloodiness > 0f && !Yandere.Paint) || (Guarding && Yandere.Armed) || (Guarding && Yandere.Sanity < 66.66666f) || (Club == ClubType.Council && Yandere.DelinquentFighting && StudentManager.CombatMinigame.Path < 4 && !StudentManager.CombatMinigame.Practice) || (Teacher && Yandere.DelinquentFighting && StudentManager.CombatMinigame.Path < 4 && !StudentManager.CombatMinigame.Practice) || (flag2 && Yandere.PickUp != null && Yandere.PickUp.Mop != null && Yandere.PickUp.Mop.Bloodiness > 0f) || (flag2 && Yandere.PickUp != null && Yandere.PickUp.BodyPart != null) || (Yandere.PickUp != null && Teacher && Yandere.PickUp.Clothing && Yandere.PickUp.Evidence) || (StudentManager.Atmosphere < 0.33333f && Yandere.Bloodiness > 0f && Yandere.Armed))
									{
										if (Yandere.Hungry || !Yandere.Egg)
										{
											if (Yandere.PickUp != null)
											{
												if (flag2)
												{
													if (Yandere.PickUp.Mop != null)
													{
														if (Yandere.PickUp.Mop.Bloodiness > 0f)
														{
															WitnessedCoverUp = true;
														}
													}
													else if (Yandere.PickUp.BodyPart != null)
													{
														WitnessedCoverUp = true;
													}
												}
												if (Teacher && Yandere.PickUp.Clothing && Yandere.PickUp.Evidence)
												{
													WitnessedCoverUp = true;
												}
											}
											if (Persona == PersonaType.PhoneAddict && CrimeReported)
											{
												CrimeReported = false;
												Fleeing = false;
											}
											if (!Yandere.DelinquentFighting)
											{
												NoBreakUp = true;
											}
											WitnessMurder();
										}
									}
									else if (!Fleeing && !Alarmed)
									{
										if (Teacher && (Yandere.Rummaging || Yandere.TheftTimer > 0f))
										{
											Alarm = 200f;
										}
										if (Yandere.WeaponTimer > 0f)
										{
											Alarm = 200f;
										}
										if (IgnoreTimer <= 0f)
										{
											if (Teacher)
											{
												StudentManager.TutorialWindow.ShowTeacherMessage = true;
											}
											Alarm += Time.deltaTime * (100f / DistanceToPlayer) * Paranoia * Perception;
											if (StudentID == 1 && Yandere.TimeSkipping)
											{
												Clock.EndTimeSkip();
											}
										}
									}
								}
								else
								{
									Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
								}
							}
							else
							{
								Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
							}
						}
						else
						{
							Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
						}
					}
					else
					{
						Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
					}
				}
				else
				{
					Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
				}
				if (PreviousAlarm > Alarm && Alarm < 100f)
				{
					YandereVisible = false;
				}
				if (Teacher && !Yandere.Medusa && Yandere.Egg)
				{
					Alarm = 0f;
				}
				if (Alarm > 100f)
				{
					BecomeAlarmed();
				}
			}
			else
			{
				Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
			}
		}
		else
		{
			if (!(Distraction != null))
			{
				return;
			}
			targetRotation = Quaternion.LookRotation(new Vector3(Distraction.position.x, base.transform.position.y, Distraction.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			if (!(Distractor != null))
			{
				return;
			}
			if (Distractor.Club == ClubType.Cooking && Distractor.Actions[Distractor.Phase] == StudentActionType.ClubAction)
			{
				CharacterAnimation.CrossFade(PlateEatAnim);
				if ((double)CharacterAnimation[PlateEatAnim].time > 6.83333)
				{
					Fingerfood[Distractor.StudentID - 20].SetActive(false);
				}
				else if ((double)CharacterAnimation[PlateEatAnim].time > 2.66666)
				{
					Fingerfood[Distractor.StudentID - 20].SetActive(true);
				}
			}
			else
			{
				CharacterAnimation.CrossFade(RandomAnim);
				if (CharacterAnimation[RandomAnim].time >= CharacterAnimation[RandomAnim].length)
				{
					PickRandomAnim();
				}
			}
		}
	}

	private void BecomeAlarmed()
	{
		if (Yandere.Medusa && YandereVisible)
		{
			TurnToStone();
		}
		else
		{
			if (Alarmed && !DiscCheck)
			{
				return;
			}
			if (Persona == PersonaType.PhoneAddict)
			{
				SmartPhone.SetActive(true);
			}
			Yandere.Alerts++;
			if (ReturningMisplacedWeapon)
			{
				DropMisplacedWeapon();
			}
			if (StudentID > 1)
			{
				for (ID = 0; ID < Outlines.Length; ID++)
				{
					if (Outlines[ID] != null)
					{
						Outlines[ID].color = new Color(1f, 1f, 0f, 1f);
					}
				}
			}
			if (DistractionTarget != null)
			{
				DistractionTarget.TargetedForDistraction = false;
			}
			CharacterAnimation.CrossFade(IdleAnim);
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			CameraReacting = false;
			Distracting = false;
			Distracted = false;
			DiscCheck = false;
			Routine = false;
			Alarmed = true;
			ReadPhase = 0;
			if (Yandere.Mask == null)
			{
				Witness = true;
			}
			EmptyHands();
			if (Club == ClubType.Cooking && Actions[Phase] == StudentActionType.ClubAction && ClubActivityPhase == 1 && !WitnessedCorpse)
			{
				ResumeDistracting = true;
			}
			SpeechLines.Stop();
			StopPairing();
			if (Witnessed != StudentWitnessType.Weapon)
			{
				PreviouslyWitnessed = Witnessed;
			}
			if (DistanceToDestination < 5f && (Actions[Phase] == StudentActionType.Graffiti || Actions[Phase] == StudentActionType.Bully))
			{
				StudentManager.NoBully[BullyID] = true;
				KilledMood = true;
			}
			if (WitnessedCorpse && !WitnessedMurder)
			{
				Witnessed = StudentWitnessType.Corpse;
				EyeShrink = 0.9f;
			}
			else if (WitnessedLimb)
			{
				Witnessed = StudentWitnessType.SeveredLimb;
			}
			else if (WitnessedBloodyWeapon)
			{
				Witnessed = StudentWitnessType.BloodyWeapon;
			}
			else if (WitnessedBloodPool)
			{
				Witnessed = StudentWitnessType.BloodPool;
			}
			else if (WitnessedWeapon)
			{
				Witnessed = StudentWitnessType.DroppedWeapon;
			}
			if (YandereVisible)
			{
				if ((!Injured && Persona == PersonaType.Violent && Yandere.Armed && !WitnessedCorpse) || (Persona == PersonaType.Violent && Yandere.DelinquentFighting))
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentWeaponReaction, 0, 3f);
					ThreatDistance = DistanceToPlayer;
					CheerTimer = UnityEngine.Random.Range(1f, 2f);
					SmartPhone.SetActive(false);
					Threatened = true;
					ThreatPhase = 1;
					ForgetGiggle();
				}
				if (Yandere.Attacking || Yandere.Struggling || Yandere.Carrying || (Yandere.PickUp != null && (bool)Yandere.PickUp.BodyPart))
				{
					if (!Yandere.Egg)
					{
						WitnessMurder();
					}
				}
				else if (Witnessed != StudentWitnessType.Corpse)
				{
					DetermineWhatWasWitnessed();
				}
				if (Teacher && WitnessedCorpse)
				{
					Concern = 1;
				}
				if (StudentID == 1 && Yandere.Mask == null && !Yandere.Egg)
				{
					if (Concern == 5)
					{
						SenpaiNoticed();
						if (Witnessed == StudentWitnessType.Stalking || Witnessed == StudentWitnessType.Lewd)
						{
							CharacterAnimation.CrossFade(IdleAnim);
							CharacterAnimation[AngryFaceAnim].weight = 1f;
						}
						else
						{
							CharacterAnimation.CrossFade(ScaredAnim);
							CharacterAnimation["scaredFace_00"].weight = 1f;
						}
						CameraEffects.MurderWitnessed();
					}
					else
					{
						CharacterAnimation.CrossFade("suspicious_00");
						CameraEffects.Alarm();
					}
				}
				else if (!Teacher)
				{
					CameraEffects.Alarm();
				}
				else
				{
					if (!Fleeing)
					{
						if (Concern < 5)
						{
							CameraEffects.Alarm();
						}
						else
						{
							if (!Yandere.Struggling)
							{
								SenpaiNoticed();
							}
							CameraEffects.MurderWitnessed();
						}
					}
					else
					{
						PersonaReaction();
						AlarmTimer = 0f;
						if (Concern < 5)
						{
							CameraEffects.Alarm();
						}
						else
						{
							CameraEffects.MurderWitnessed();
						}
					}
				}
				if (!Teacher && Club != ClubType.Delinquent && Witnessed == PreviouslyWitnessed)
				{
					RepeatReaction = true;
				}
				if (Yandere.Mask == null)
				{
					RepDeduction = 0f;
					CalculateReputationPenalty();
					if (RepDeduction >= 0f)
					{
						RepLoss -= RepDeduction;
					}
					Reputation.PendingRep -= RepLoss * Paranoia;
					PendingRep -= RepLoss * Paranoia;
				}
				if (ToiletEvent != null && ToiletEvent.EventDay == DayOfWeek.Monday)
				{
					ToiletEvent.EndEvent();
				}
			}
			else if (!WitnessedCorpse)
			{
				if (Yandere.Caught)
				{
					if (Yandere.Mask == null)
					{
						if (Yandere.Pickpocketing)
						{
							Witnessed = StudentWitnessType.Pickpocketing;
							RepLoss += 10f;
						}
						else
						{
							Witnessed = StudentWitnessType.Theft;
						}
						RepDeduction = 0f;
						CalculateReputationPenalty();
						if (RepDeduction >= 0f)
						{
							RepLoss -= RepDeduction;
						}
						Reputation.PendingRep -= RepLoss * Paranoia;
						PendingRep -= RepLoss * Paranoia;
					}
				}
				else if (WitnessedLimb)
				{
					Witnessed = StudentWitnessType.SeveredLimb;
				}
				else if (WitnessedBloodyWeapon)
				{
					Witnessed = StudentWitnessType.BloodyWeapon;
				}
				else if (WitnessedBloodPool)
				{
					Witnessed = StudentWitnessType.BloodPool;
				}
				else if (WitnessedWeapon)
				{
					Witnessed = StudentWitnessType.DroppedWeapon;
				}
				else
				{
					Witnessed = StudentWitnessType.None;
					DiscCheck = true;
					Witness = false;
				}
			}
			else
			{
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
			}
		}
	}

	private void UpdateDetectionMarker()
	{
		if (Alarm < 0f)
		{
			Alarm = 0f;
		}
		if (DetectionMarker != null)
		{
			if (Alarm > 0f)
			{
				if (!DetectionMarker.Tex.enabled)
				{
					DetectionMarker.Tex.enabled = true;
				}
				DetectionMarker.Tex.transform.localScale = new Vector3(DetectionMarker.Tex.transform.localScale.x, (!(Alarm <= 100f)) ? 1f : (Alarm / 100f), DetectionMarker.Tex.transform.localScale.z);
				DetectionMarker.Tex.color = new Color(DetectionMarker.Tex.color.r, DetectionMarker.Tex.color.g, DetectionMarker.Tex.color.b, Alarm / 100f);
			}
			else if (DetectionMarker.Tex.color.a != 0f)
			{
				DetectionMarker.Tex.enabled = false;
				DetectionMarker.Tex.color = new Color(DetectionMarker.Tex.color.r, DetectionMarker.Tex.color.g, DetectionMarker.Tex.color.b, 0f);
			}
		}
		else
		{
			SpawnDetectionMarker();
		}
	}

	private void UpdateTalkInput()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (!GameGlobals.EmptyDemon && (Alarm > 0f || AlarmTimer > 0f || Yandere.Armed || Waiting || InEvent || SentHome || Threatened || Yandere.Shoved || (Distracted && !Drownable) || StudentID == 1) && !Slave && !BadTime && !Yandere.Gazing)
			{
				Prompt.Circle[0].fillAmount = 1f;
			}
			if (Prompt.Circle[0].fillAmount == 0f)
			{
				bool flag = false;
				if (StudentID < 86 && Armband.activeInHierarchy && (Actions[Phase] == StudentActionType.ClubAction || Actions[Phase] == StudentActionType.SitAndSocialize || Actions[Phase] == StudentActionType.Socializing || Actions[Phase] == StudentActionType.Sleuth || Actions[Phase] == StudentActionType.Lyrics || Actions[Phase] == StudentActionType.Patrol || Actions[Phase] == StudentActionType.SitAndEatBento) && (DistanceToDestination < 1f || (base.transform.position.y > StudentManager.ClubZones[(int)Club].position.y - 1f && base.transform.position.y < StudentManager.ClubZones[(int)Club].position.y + 1f && Vector3.Distance(base.transform.position, StudentManager.ClubZones[(int)Club].position) < ClubThreshold) || Vector3.Distance(base.transform.position, StudentManager.DramaSpots[1].position) < 12f))
				{
					flag = true;
					Warned = false;
				}
				if (StudentManager.Pose)
				{
					MyController.enabled = false;
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Stop = true;
					Pose();
				}
				else if (BadTime)
				{
					Yandere.EmptyHands();
					BecomeRagdoll();
					Yandere.RagdollPK.connectedBody = Ragdoll.AllRigidbodies[5];
					Yandere.RagdollPK.connectedAnchor = Ragdoll.LimbAnchor[4];
					DialogueWheel.PromptBar.ClearButtons();
					DialogueWheel.PromptBar.Label[1].text = "Back";
					DialogueWheel.PromptBar.UpdateButtons();
					DialogueWheel.PromptBar.Show = true;
					Yandere.Ragdoll = Ragdoll.gameObject;
					Yandere.SansEyes[0].SetActive(true);
					Yandere.SansEyes[1].SetActive(true);
					Yandere.GlowEffect.Play();
					Yandere.CanMove = false;
					Yandere.PK = true;
					DeathType = DeathType.EasterEgg;
				}
				else if (StudentManager.Six)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(AlarmDisc, base.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
					gameObject.GetComponent<AlarmDiscScript>().Originator = this;
					AudioSource.PlayClipAtPoint(Yandere.SixTakedown, base.transform.position);
					AudioSource.PlayClipAtPoint(Yandere.Snarls[UnityEngine.Random.Range(0, Yandere.Snarls.Length)], base.transform.position);
					Yandere.CharacterAnimation.CrossFade("f02_sixEat_00");
					Yandere.TargetStudent = this;
					Yandere.FollowHips = true;
					Yandere.Attacking = true;
					Yandere.CanMove = false;
					Yandere.Eating = true;
					CharacterAnimation.CrossFade(EatVictimAnim);
					Pathfinding.enabled = false;
					Routine = false;
					Dying = true;
					Eaten = true;
					GameObject gameObject2 = UnityEngine.Object.Instantiate(EmptyGameObject, base.transform.position, Quaternion.identity);
					Yandere.SixTarget = gameObject2.transform;
					Yandere.SixTarget.LookAt(Yandere.transform.position);
					Yandere.SixTarget.Translate(Yandere.SixTarget.forward);
				}
				else if (Yandere.SpiderGrow)
				{
					if (!Eaten && !Cosmetic.Empty)
					{
						AudioSource.PlayClipAtPoint(Yandere.SixTakedown, base.transform.position);
						AudioSource.PlayClipAtPoint(Yandere.Snarls[UnityEngine.Random.Range(0, Yandere.Snarls.Length)], base.transform.position);
						GameObject gameObject3 = UnityEngine.Object.Instantiate(Yandere.EmptyHusk, base.transform.position + base.transform.forward * 0.5f, Quaternion.identity);
						gameObject3.GetComponent<EmptyHuskScript>().TargetStudent = this;
						gameObject3.transform.LookAt(base.transform.position);
						CharacterAnimation.CrossFade(EatVictimAnim);
						Pathfinding.enabled = false;
						Distracted = false;
						Routine = false;
						Dying = true;
						Eaten = true;
						if (Investigating)
						{
							StopInvestigating();
						}
						if (Following)
						{
							Yandere.Followers--;
							Following = false;
						}
						GameObject gameObject4 = UnityEngine.Object.Instantiate(EmptyGameObject, base.transform.position, Quaternion.identity);
					}
				}
				else if (StudentManager.Gaze)
				{
					Yandere.CharacterAnimation.CrossFade("f02_gazerPoint_00");
					Yandere.GazerEyes.Attacking = true;
					Yandere.TargetStudent = this;
					Yandere.GazeAttacking = true;
					Yandere.CanMove = false;
					Routine = false;
				}
				else if (Slave)
				{
					Yandere.TargetStudent = this;
					Yandere.PauseScreen.StudentInfoMenu.Targeting = true;
					Yandere.PauseScreen.StudentInfoMenu.gameObject.SetActive(true);
					Yandere.PauseScreen.StudentInfoMenu.Column = 0;
					Yandere.PauseScreen.StudentInfoMenu.Row = 0;
					Yandere.PauseScreen.StudentInfoMenu.UpdateHighlight();
					StartCoroutine(Yandere.PauseScreen.StudentInfoMenu.UpdatePortraits());
					Yandere.PauseScreen.MainMenu.SetActive(false);
					Yandere.PauseScreen.Panel.enabled = true;
					Yandere.PauseScreen.Sideways = true;
					Yandere.PauseScreen.Show = true;
					Time.timeScale = 0.0001f;
					Yandere.PromptBar.ClearButtons();
					Yandere.PromptBar.Label[1].text = "Cancel";
					Yandere.PromptBar.UpdateButtons();
					Yandere.PromptBar.Show = true;
				}
				else if (Following)
				{
					Subtitle.UpdateLabel(SubtitleType.StudentFarewell, 0, 3f);
					Prompt.Label[0].text = "     Talk";
					Prompt.Circle[0].fillAmount = 1f;
					ParticleSystem.EmissionModule emission = Hearts.emission;
					emission.enabled = false;
					Yandere.Followers--;
					Following = false;
					Routine = true;
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					Pathfinding.speed = 1f;
				}
				else if (Pushable)
				{
					CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
					if (!Male)
					{
						Subtitle.UpdateLabel(SubtitleType.NoteReaction, 5, 3f);
					}
					else
					{
						Subtitle.UpdateLabel(SubtitleType.NoteReactionMale, 5, 3f);
					}
					Prompt.Label[0].text = "     Talk";
					Prompt.Circle[0].fillAmount = 1f;
					Yandere.TargetStudent = this;
					Yandere.Attacking = true;
					Yandere.RoofPush = true;
					Yandere.CanMove = false;
					Yandere.EmptyHands();
					Distracted = true;
					Routine = false;
					Pushed = true;
					CharacterAnimation.CrossFade(PushedAnim);
				}
				else if (Drownable)
				{
					if (VomitDoor != null)
					{
						VomitDoor.Prompt.enabled = true;
						VomitDoor.enabled = true;
					}
					Yandere.EmptyHands();
					Prompt.Hide();
					Prompt.enabled = false;
					Prompt.Circle[0].fillAmount = 1f;
					VomitEmitter.gameObject.SetActive(false);
					Police.DrownedStudentName = Name;
					MyController.enabled = false;
					VomitEmitter.gameObject.SetActive(false);
					Police.DrownScene = true;
					Distracted = true;
					Drownable = false;
					Routine = false;
					Drowned = true;
					Subtitle.UpdateLabel(SubtitleType.DrownReaction, 0, 3f);
					Yandere.TargetStudent = this;
					Yandere.Attacking = true;
					Yandere.CanMove = false;
					Yandere.Drown = true;
					Yandere.DrownAnim = "f02_fountainDrownA_00";
					if (Male)
					{
						if (Vector3.Distance(base.transform.position, StudentManager.transform.position) < 5f)
						{
							DrownAnim = "fountainDrown_00_B";
						}
						else
						{
							DrownAnim = "toiletDrown_00_B";
						}
					}
					else if (Vector3.Distance(base.transform.position, StudentManager.transform.position) < 5f)
					{
						DrownAnim = "f02_fountainDrownB_00";
					}
					else
					{
						DrownAnim = "f02_toiletDrownB_00";
					}
					CharacterAnimation.CrossFade(DrownAnim);
				}
				else if (Clock.Period == 2 || Clock.Period == 4 || CurrentDestination == Seat)
				{
					Subtitle.UpdateLabel(SubtitleType.ClassApology, 0, 3f);
					Prompt.Circle[0].fillAmount = 1f;
				}
				else if (InEvent || !CanTalk || GoAway || Fleeing || (Meeting && !Drownable) || Wet || TurnOffRadio || (MyPlate != null && MyPlate.parent == RightHand) || ReturningMisplacedWeapon || Actions[Phase] == StudentActionType.Bully || Actions[Phase] == StudentActionType.Graffiti)
				{
					Subtitle.UpdateLabel(SubtitleType.EventApology, 1, 3f);
					Prompt.Circle[0].fillAmount = 1f;
				}
				else if (Clock.Period == 3 && BusyAtLunch)
				{
					Subtitle.UpdateLabel(SubtitleType.SadApology, 1, 3f);
					Prompt.Circle[0].fillAmount = 1f;
				}
				else if (Warned)
				{
					Subtitle.UpdateLabel(SubtitleType.GrudgeRefusal, 0, 3f);
					Prompt.Circle[0].fillAmount = 1f;
				}
				else if (Ignoring)
				{
					Subtitle.UpdateLabel(SubtitleType.PhotoAnnoyance, 0, 3f);
					Prompt.Circle[0].fillAmount = 1f;
				}
				else
				{
					bool flag2 = false;
					if (Yandere.Bloodiness > 0f && !Yandere.Paint)
					{
						flag2 = true;
					}
					if (!Witness && flag2)
					{
						Prompt.Circle[0].fillAmount = 1f;
						YandereVisible = true;
						Alarm = 200f;
					}
					else
					{
						SpeechLines.Stop();
						Yandere.TargetStudent = this;
						if (!Grudge)
						{
							ClubManager.CheckGrudge(Club);
							if (ClubGlobals.GetClubKicked(Club) && flag)
							{
								Interaction = StudentInteractionType.ClubGrudge;
								TalkTimer = 5f;
								Warned = true;
							}
							else if (ClubGlobals.Club == Club && flag && ClubManager.ClubGrudge)
							{
								Interaction = StudentInteractionType.ClubKick;
								ClubGlobals.SetClubKicked(Club, true);
								TalkTimer = 5f;
								Warned = true;
							}
							else if (Prompt.Label[0].text == "     Feed")
							{
								Interaction = StudentInteractionType.Feeding;
								TalkTimer = 10f;
							}
							else if (Prompt.Label[0].text == "     Give Snack")
							{
								Yandere.Interaction = YandereInteractionType.GivingSnack;
								Yandere.TalkTimer = 3f;
								Interaction = StudentInteractionType.Idle;
							}
							else if (Prompt.Label[0].text == "     Ask For Help")
							{
								Yandere.Interaction = YandereInteractionType.AskingForHelp;
								Yandere.TalkTimer = 5f;
								Interaction = StudentInteractionType.Idle;
							}
							else
							{
								DistanceToDestination = Vector3.Distance(base.transform.position, Destinations[Phase].position);
								if (Sleuthing)
								{
									DistanceToDestination = Vector3.Distance(base.transform.position, SleuthTarget.position);
								}
								if (flag)
								{
									int num = 0;
									num = (Sleuthing ? 5 : 0);
									if (GameGlobals.EmptyDemon)
									{
										num = (int)Club * -1;
									}
									Subtitle.UpdateLabel(SubtitleType.ClubGreeting, (int)(Club + num), 4f);
									DialogueWheel.ClubLeader = true;
								}
								else
								{
									Subtitle.UpdateLabel(SubtitleType.Greeting, 0, 3f);
								}
								if (Club != ClubType.Council && Club != ClubType.Delinquent && ((Male && PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 0) || PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 4))
								{
									ParticleSystem.EmissionModule emission2 = Hearts.emission;
									emission2.rateOverTime = PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus;
									emission2.enabled = true;
									Hearts.Play();
								}
								StudentManager.DisablePrompts();
								StudentManager.VolumeDown();
								DialogueWheel.HideShadows();
								DialogueWheel.Show = true;
								DialogueWheel.Panel.enabled = true;
								TalkTimer = 0f;
								if (!ConversationGlobals.GetTopicDiscovered(20))
								{
									Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
									ConversationGlobals.SetTopicDiscovered(20, true);
								}
								if (!ConversationGlobals.GetTopicLearnedByStudent(20, StudentID))
								{
									Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
									ConversationGlobals.SetTopicLearnedByStudent(20, StudentID, true);
								}
								if (!ConversationGlobals.GetTopicDiscovered(21))
								{
									Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
									ConversationGlobals.SetTopicDiscovered(21, true);
								}
								if (!ConversationGlobals.GetTopicLearnedByStudent(21, StudentID))
								{
									Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
									ConversationGlobals.SetTopicLearnedByStudent(21, StudentID, true);
								}
							}
						}
						else if (flag)
						{
							Interaction = StudentInteractionType.ClubUnwelcome;
							TalkTimer = 5f;
							Warned = true;
						}
						else
						{
							Interaction = StudentInteractionType.PersonalGrudge;
							TalkTimer = 5f;
							Warned = true;
						}
						Yandere.ShoulderCamera.OverShoulder = true;
						Pathfinding.canSearch = false;
						Pathfinding.canMove = false;
						Obstacle.enabled = true;
						Giggle = null;
						Yandere.WeaponMenu.KeyboardShow = false;
						Yandere.Obscurance.enabled = false;
						Yandere.WeaponMenu.Show = false;
						Yandere.YandereVision = false;
						Yandere.CanMove = false;
						Yandere.Talking = true;
						Investigating = false;
						Talk.enabled = true;
						Reacted = false;
						Routine = false;
						Talking = true;
						ReadPhase = 0;
						EmptyHands();
						bool flag3 = false;
						if (CurrentAction == StudentActionType.Sunbathe && SunbathePhase > 2)
						{
							SunbathePhase = 2;
							flag3 = true;
						}
						if (Sleuthing)
						{
							if (!Scrubber.activeInHierarchy)
							{
								SmartPhone.SetActive(true);
							}
							else
							{
								SmartPhone.SetActive(false);
							}
						}
						else if (Persona != PersonaType.PhoneAddict)
						{
							SmartPhone.SetActive(false);
						}
						else if (!Scrubber.activeInHierarchy && !flag3)
						{
							SmartPhone.SetActive(true);
						}
						ChalkDust.Stop();
						StopPairing();
					}
				}
			}
		}
		if ((Prompt.Circle[2].fillAmount != 0f && (!(DistanceToPlayer <= 1f) || Club == ClubType.Council || !Yandere.Armed || !Yandere.CanMove || !(Yandere.Sanity < 33.33333f))) || ClubActivityPhase >= 16)
		{
			return;
		}
		float f = Vector3.Angle(-base.transform.forward, Yandere.transform.position - base.transform.position);
		Yandere.AttackManager.Stealth = Mathf.Abs(f) <= 45f;
		bool flag4 = false;
		if (Club == ClubType.Delinquent && !Injured && !Yandere.AttackManager.Stealth)
		{
			flag4 = true;
			Fleeing = false;
			Patience = 1;
			Shove();
			SpawnAlarmDisc();
		}
		if (flag4 || Yandere.NearSenpai || Yandere.Attacking || Yandere.Stance.Current == StanceType.Crouching)
		{
			return;
		}
		if (Yandere.EquippedWeapon.Flaming || Yandere.CyborgParts[1].activeInHierarchy)
		{
			Yandere.SanityBased = false;
		}
		if (Strength == 9)
		{
			if (!Yandere.AttackManager.Stealth)
			{
				CharacterAnimation.CrossFade("f02_dramaticFrontal_00");
			}
			else
			{
				CharacterAnimation.CrossFade("f02_dramaticStealth_00");
			}
			Yandere.CharacterAnimation.CrossFade("f02_readyToFight_00");
			Yandere.CanMove = false;
			DramaticCamera.rect = new Rect(0f, 0.5f, 1f, 0f);
			DramaticCamera.gameObject.SetActive(true);
			DramaticCamera.gameObject.GetComponent<AudioSource>().Play();
			DramaticReaction = true;
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			Routine = false;
		}
		else
		{
			AttackReaction();
		}
	}

	private void UpdateDying()
	{
		CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
		if (!Attacked)
		{
			return;
		}
		if (!Teacher)
		{
			if (Strength == 9)
			{
				if (!StudentManager.Stop)
				{
					StudentManager.StopMoving();
					Yandere.RPGCamera.enabled = false;
					SmartPhone.SetActive(false);
					Police.Show = false;
				}
				CharacterAnimation.CrossFade(CounterAnim);
				targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 10f);
				MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward);
				return;
			}
			EyeShrink = Mathf.Lerp(EyeShrink, 1f, Time.deltaTime * 10f);
			if (Alive && !Tranquil)
			{
				if (!Yandere.SanityBased)
				{
					targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 10f);
					if (Yandere.EquippedWeapon.WeaponID == 11)
					{
						CharacterAnimation.CrossFade(CyborgDeathAnim);
						MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward);
						if (CharacterAnimation[CyborgDeathAnim].time >= CharacterAnimation[CyborgDeathAnim].length - 0.25f && Yandere.EquippedWeapon.WeaponID == 11)
						{
							UnityEngine.Object.Instantiate(BloodyScream, base.transform.position + Vector3.up, Quaternion.identity);
							DeathType = DeathType.EasterEgg;
							BecomeRagdoll();
							Ragdoll.Dismember();
						}
					}
					else if (Yandere.EquippedWeapon.WeaponID == 7)
					{
						CharacterAnimation.CrossFade(BuzzSawDeathAnim);
						MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward);
					}
					else if (!Yandere.EquippedWeapon.Concealable)
					{
						CharacterAnimation.CrossFade(SwingDeathAnim);
						MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward);
					}
					else
					{
						CharacterAnimation.CrossFade(DefendAnim);
						MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward * 0.1f);
					}
				}
				else
				{
					MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward * Yandere.AttackManager.Distance);
					if (!Yandere.AttackManager.Stealth)
					{
						targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
					}
					else
					{
						targetRotation = Quaternion.LookRotation(base.transform.position - new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z));
					}
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 10f);
				}
			}
			else
			{
				CharacterAnimation.CrossFade(DeathAnim);
				if (CharacterAnimation[DeathAnim].time < 1f)
				{
					base.transform.Translate(Vector3.back * Time.deltaTime);
					return;
				}
				BecomeRagdoll();
			}
		}
		else
		{
			if (!StudentManager.Stop)
			{
				StudentManager.StopMoving();
				Yandere.Laughing = false;
				Yandere.Dipping = false;
				Yandere.RPGCamera.enabled = false;
				SmartPhone.SetActive(false);
				Police.Show = false;
			}
			CharacterAnimation.CrossFade(CounterAnim);
			targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, Time.deltaTime * 10f);
			MoveTowardsTarget(Yandere.transform.position + Yandere.transform.forward);
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		}
	}

	private void UpdatePushed()
	{
		Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
		EyeShrink = Mathf.Lerp(EyeShrink, 1f, Time.deltaTime * 10f);
		if (CharacterAnimation[PushedAnim].time >= CharacterAnimation[PushedAnim].length)
		{
			BecomeRagdoll();
		}
	}

	private void UpdateDrowned()
	{
		SplashTimer += Time.deltaTime;
		if (SplashTimer > 3f && SplashTimer < 100f)
		{
			DrowningSplashes.Play();
			SplashTimer += 100f;
		}
		Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
		EyeShrink = Mathf.Lerp(EyeShrink, 1f, Time.deltaTime * 10f);
		if (CharacterAnimation[DrownAnim].time >= CharacterAnimation[DrownAnim].length)
		{
			BecomeRagdoll();
		}
	}

	private void UpdateWitnessedMurder()
	{
		if (Threatened)
		{
			UpdateAlarmed();
		}
		else
		{
			if (Fleeing || Shoving)
			{
				return;
			}
			if (StudentID > 1 && Persona != PersonaType.Evil)
			{
				EyeShrink += Time.deltaTime * 0.2f;
			}
			if (Yandere.TargetStudent != null && LovedOneIsTargeted(Yandere.TargetStudent.StudentID))
			{
				Strength = 5;
				Persona = PersonaType.Heroic;
				SmartPhone.SetActive(false);
				SprintAnim = OriginalSprintAnim;
			}
			if ((Club != ClubType.Delinquent || (Club == ClubType.Delinquent && Injured)) && Yandere.TargetStudent == null && LovedOneIsTargeted(Yandere.NearestCorpseID))
			{
				Strength = 5;
				if (Injured)
				{
					Strength = 1;
				}
				Persona = PersonaType.Heroic;
			}
			if (Yandere.PickUp != null && Yandere.PickUp.BodyPart != null && Yandere.PickUp.BodyPart.Type == 1 && LovedOneIsTargeted(Yandere.PickUp.BodyPart.StudentID))
			{
				Strength = 5;
				Persona = PersonaType.Heroic;
				SmartPhone.SetActive(false);
				SprintAnim = OriginalSprintAnim;
			}
			if (Persona != PersonaType.PhoneAddict)
			{
				CharacterAnimation.CrossFade(ScaredAnim);
			}
			else if (!Attacked)
			{
				PhoneAddictCameraUpdate();
			}
			targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.position.x, base.transform.position.y, Yandere.Hips.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			if (!Yandere.Struggling)
			{
				if (Persona != PersonaType.Heroic && Persona != PersonaType.Dangerous && Persona != PersonaType.Protective && Persona != PersonaType.Violent)
				{
					AlarmTimer += Time.deltaTime * (float)MurdersWitnessed;
				}
				else
				{
					AlarmTimer += Time.deltaTime * ((float)MurdersWitnessed * 5f);
				}
			}
			if (AlarmTimer > 5f)
			{
				PersonaReaction();
				AlarmTimer = 0f;
			}
			else
			{
				if (!(AlarmTimer > 1f) || Reacted)
				{
					return;
				}
				if (StudentID > 1 || Yandere.Mask != null)
				{
					if (StudentID == 1)
					{
						Persona = PersonaType.Heroic;
						PersonaReaction();
					}
					if (!Teacher)
					{
						if (Persona != PersonaType.Evil)
						{
							if (Club == ClubType.Delinquent)
							{
								SmartPhone.SetActive(false);
							}
							else
							{
								Subtitle.UpdateLabel(SubtitleType.MurderReaction, 1, 3f);
							}
						}
					}
					else
					{
						if (WitnessedCoverUp)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherCoverUpHostile, 1, 5f);
						}
						else
						{
							DetermineWhatWasWitnessed();
							DetermineTeacherSubtitle();
						}
						StudentManager.Portal.SetActive(false);
					}
				}
				else
				{
					MurderReaction = UnityEngine.Random.Range(1, 6);
					CharacterAnimation.CrossFade("senpaiMurderReaction_0" + MurderReaction);
					GameOverCause = GameOverType.Murder;
					SenpaiNoticed();
					CharacterAnimation["scaredFace_00"].weight = 0f;
					CharacterAnimation[AngryFaceAnim].weight = 0f;
					Yandere.ShoulderCamera.enabled = true;
					Yandere.ShoulderCamera.Noticed = true;
					Yandere.RPGCamera.enabled = false;
					Stop = true;
				}
				Reacted = true;
			}
		}
	}

	private void UpdateAlarmed()
	{
		if (!Threatened)
		{
			if (Yandere.Medusa && YandereVisible)
			{
				TurnToStone();
				return;
			}
			if (!Male)
			{
				SpeechLines.Stop();
			}
			if (Persona != PersonaType.PhoneAddict && !Sleuthing)
			{
				SmartPhone.SetActive(false);
			}
			OccultBook.SetActive(false);
			SpeechLines.Stop();
			Pen.SetActive(false);
			ReadPhase = 0;
			if (WitnessedCorpse)
			{
				if (!WalkBack)
				{
					if (StudentID == 1)
					{
					}
					if (Persona != PersonaType.PhoneAddict)
					{
						CharacterAnimation.CrossFade(ScaredAnim);
					}
					else if (!Attacked)
					{
						PhoneAddictCameraUpdate();
					}
				}
				else
				{
					MyController.Move(base.transform.forward * (-0.5f * Time.deltaTime));
					CharacterAnimation.CrossFade(WalkBackAnim);
					WalkBackTimer -= Time.deltaTime;
					if (WalkBackTimer <= 0f)
					{
						WalkBack = false;
					}
				}
			}
			else if (!WitnessedLimb && !WitnessedBloodyWeapon && !WitnessedBloodPool && !WitnessedWeapon && StudentID > 1)
			{
				if (Witness)
				{
					CharacterAnimation.CrossFade(LeanAnim);
				}
				else
				{
					CharacterAnimation.CrossFade(IdleAnim);
					if (FocusOnYandere)
					{
						if (DistanceToPlayer < 1f && !Injured)
						{
							AlarmTimer = 0f;
							if (Club == ClubType.Council || (Club == ClubType.Delinquent && !Injured))
							{
								ThreatTimer += Time.deltaTime;
								if (ThreatTimer > 5f && !Yandere.Struggling && !Yandere.DelinquentFighting && Prompt.InSight)
								{
									ThreatTimer = 0f;
									Shove();
								}
							}
						}
						DistractionSpot = new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z);
					}
				}
			}
			if (WitnessedMurder)
			{
				targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			}
			else if (WitnessedCorpse)
			{
				if (Corpse != null && Corpse.AllColliders[0] != null)
				{
					targetRotation = Quaternion.LookRotation(new Vector3(Corpse.AllColliders[0].transform.position.x, base.transform.position.y, Corpse.AllColliders[0].transform.position.z) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				}
			}
			else if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
			{
				if (BloodPool != null)
				{
					targetRotation = Quaternion.LookRotation(new Vector3(BloodPool.transform.position.x, base.transform.position.y, BloodPool.transform.position.z) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				}
			}
			else if (!DiscCheck)
			{
				targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, base.transform.position.y, Yandere.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			}
			else
			{
				targetRotation = Quaternion.LookRotation(DistractionSpot - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
			}
			if (!Fleeing && !Yandere.DelinquentFighting)
			{
				AlarmTimer += Time.deltaTime * (1f - Hesitation);
			}
			Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
			if (AlarmTimer > 5f)
			{
				EndAlarm();
			}
			else if (AlarmTimer > 1f && !Reacted)
			{
				if (Teacher)
				{
					if (!WitnessedCorpse)
					{
						CharacterAnimation.CrossFade(IdleAnim);
						if (Witnessed == StudentWitnessType.WeaponAndBloodAndInsanity)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherInsanityReaction, 1, 6f);
							GameOverCause = GameOverType.Insanity;
						}
						else if (Witnessed == StudentWitnessType.WeaponAndBlood)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherWeaponReaction, 1, 6f);
							GameOverCause = GameOverType.Weapon;
						}
						else if (Witnessed == StudentWitnessType.WeaponAndInsanity)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherInsanityReaction, 1, 6f);
							GameOverCause = GameOverType.Insanity;
						}
						else if (Witnessed == StudentWitnessType.BloodAndInsanity)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherInsanityReaction, 1, 6f);
							GameOverCause = GameOverType.Insanity;
						}
						else if (Witnessed == StudentWitnessType.Weapon)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherWeaponReaction, 1, 6f);
							GameOverCause = GameOverType.Weapon;
						}
						else if (Witnessed == StudentWitnessType.Blood)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherBloodReaction, 1, 6f);
							GameOverCause = GameOverType.Blood;
						}
						else if (Witnessed == StudentWitnessType.Insanity || Witnessed == StudentWitnessType.Poisoning)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherInsanityReaction, 1, 6f);
							GameOverCause = GameOverType.Insanity;
						}
						else if (Witnessed == StudentWitnessType.Lewd)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherLewdReaction, 1, 6f);
							GameOverCause = GameOverType.Lewd;
						}
						else if (Witnessed == StudentWitnessType.Violence)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherTrespassingReaction, 5, 5f);
							GameOverCause = GameOverType.Violence;
							Concern = 5;
						}
						else if (Witnessed == StudentWitnessType.Trespassing)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherTrespassingReaction, Concern, 5f);
						}
						else if (Witnessed == StudentWitnessType.Theft || Witnessed == StudentWitnessType.Pickpocketing)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherTheftReaction, 1, 6f);
						}
						else if (Witnessed == StudentWitnessType.CleaningItem)
						{
							Subtitle.UpdateLabel(SubtitleType.TeacherInsanityReaction, 1, 6f);
							GameOverCause = GameOverType.Insanity;
						}
						if (Club == ClubType.Council)
						{
							UnityEngine.Object.Destroy(Subtitle.CurrentClip);
							Subtitle.UpdateLabel(SubtitleType.CouncilToCounselor, ClubMemberID, 5f);
						}
					}
					else
					{
						Concern = 1;
						DetermineWhatWasWitnessed();
						DetermineTeacherSubtitle();
						if (WitnessedMurder)
						{
							MurdersWitnessed++;
							if (!Yandere.Chased)
							{
								ChaseYandere();
							}
						}
					}
					if (!Guarding && !Chasing && ((YandereVisible && Concern == 5) || Yandere.Noticed))
					{
						if (Witnessed == StudentWitnessType.Theft && Yandere.StolenObject != null)
						{
							Yandere.StolenObject.SetActive(true);
							Yandere.StolenObject = null;
							Yandere.Inventory.IDCard = false;
						}
						StudentManager.CombatMinigame.Stop();
						CharacterAnimation[AngryFaceAnim].weight = 1f;
						Yandere.ShoulderCamera.enabled = true;
						Yandere.ShoulderCamera.Noticed = true;
						Yandere.RPGCamera.enabled = false;
						Stop = true;
					}
				}
				else if (StudentID > 1 || Yandere.Mask != null)
				{
					if (StudentID == 41)
					{
						Subtitle.UpdateLabel(SubtitleType.Impatience, 6, 5f);
					}
					else if (RepeatReaction)
					{
						Subtitle.UpdateLabel(SubtitleType.RepeatReaction, 1, 3f);
						RepeatReaction = false;
					}
					else if (Club != ClubType.Delinquent)
					{
						if (Witnessed == StudentWitnessType.WeaponAndBloodAndInsanity)
						{
							Subtitle.UpdateLabel(SubtitleType.WeaponAndBloodAndInsanityReaction, 1, 3f);
						}
						else if (Witnessed == StudentWitnessType.WeaponAndBlood)
						{
							Subtitle.UpdateLabel(SubtitleType.WeaponAndBloodReaction, 1, 3f);
						}
						else if (Witnessed == StudentWitnessType.WeaponAndInsanity)
						{
							Subtitle.UpdateLabel(SubtitleType.WeaponAndInsanityReaction, 1, 3f);
						}
						else if (Witnessed == StudentWitnessType.BloodAndInsanity)
						{
							Subtitle.UpdateLabel(SubtitleType.BloodAndInsanityReaction, 1, 3f);
						}
						else if (Witnessed == StudentWitnessType.Weapon)
						{
							Subtitle.StudentID = StudentID;
							Subtitle.UpdateLabel(SubtitleType.WeaponReaction, WeaponWitnessed, 3f);
						}
						else if (Witnessed == StudentWitnessType.Blood)
						{
							if (!Bloody)
							{
								Subtitle.UpdateLabel(SubtitleType.BloodReaction, 1, 3f);
							}
							else
							{
								Subtitle.UpdateLabel(SubtitleType.WetBloodReaction, 1, 3f);
								Witnessed = StudentWitnessType.None;
								Witness = false;
							}
						}
						else if (Witnessed == StudentWitnessType.Insanity)
						{
							Subtitle.UpdateLabel(SubtitleType.InsanityReaction, 1, 3f);
						}
						else if (Witnessed == StudentWitnessType.Lewd)
						{
							Subtitle.UpdateLabel(SubtitleType.LewdReaction, 1, 3f);
						}
						else if (Witnessed == StudentWitnessType.CleaningItem)
						{
							Subtitle.UpdateLabel(SubtitleType.SuspiciousReaction, 0, 5f);
						}
						else if (Witnessed == StudentWitnessType.Suspicious)
						{
							Subtitle.UpdateLabel(SubtitleType.SuspiciousReaction, 1, 5f);
						}
						else if (Witnessed == StudentWitnessType.Corpse)
						{
							if (Club == ClubType.Council)
							{
								if (StudentID == 86)
								{
									Subtitle.UpdateLabel(SubtitleType.CouncilCorpseReaction, 1, 5f);
								}
								else if (StudentID == 87)
								{
									Subtitle.UpdateLabel(SubtitleType.CouncilCorpseReaction, 2, 5f);
								}
								else if (StudentID == 88)
								{
									Subtitle.UpdateLabel(SubtitleType.CouncilCorpseReaction, 3, 5f);
								}
								else if (StudentID == 89)
								{
									Subtitle.UpdateLabel(SubtitleType.CouncilCorpseReaction, 4, 5f);
								}
							}
							else if (Persona == PersonaType.Evil)
							{
								Subtitle.UpdateLabel(SubtitleType.EvilCorpseReaction, 1, 5f);
							}
							else if (!Corpse.Choking)
							{
								Subtitle.UpdateLabel(SubtitleType.CorpseReaction, 0, 5f);
							}
							else
							{
								Subtitle.UpdateLabel(SubtitleType.CorpseReaction, 1, 5f);
							}
						}
						else if (Witnessed == StudentWitnessType.Interruption)
						{
							Subtitle.UpdateLabel(SubtitleType.InterruptionReaction, 1, 5f);
						}
						else if (Witnessed == StudentWitnessType.Eavesdropping)
						{
							if (StudentID == StudentManager.RivalID)
							{
								Subtitle.UpdateLabel(SubtitleType.RivalEavesdropReaction, 1, 5f);
							}
							else if (EventInterrupted)
							{
								Subtitle.UpdateLabel(SubtitleType.EventEavesdropReaction, 1, 5f);
								EventInterrupted = false;
							}
							else
							{
								Subtitle.UpdateLabel(SubtitleType.EavesdropReaction, 1, 5f);
							}
						}
						else if (Witnessed == StudentWitnessType.Pickpocketing)
						{
							Subtitle.UpdateLabel(PickpocketSubtitleType, 1, 5f);
						}
						else if (Witnessed == StudentWitnessType.Violence)
						{
							Subtitle.UpdateLabel(SubtitleType.ViolenceReaction, 5, 5f);
						}
						else if (Witnessed == StudentWitnessType.Poisoning)
						{
							if (Yandere.TargetBento.StudentID != StudentID)
							{
								Subtitle.UpdateLabel(SubtitleType.PoisonReaction, 1, 5f);
							}
							else
							{
								Subtitle.UpdateLabel(SubtitleType.PoisonReaction, 2, 5f);
								Phase++;
								Pathfinding.target = Destinations[Phase];
								CurrentDestination = Destinations[Phase];
							}
						}
						else if (Witnessed == StudentWitnessType.SeveredLimb)
						{
							Subtitle.UpdateLabel(SubtitleType.LimbReaction, 0, 5f);
						}
						else if (Witnessed == StudentWitnessType.BloodyWeapon)
						{
							Subtitle.UpdateLabel(SubtitleType.BloodyWeaponReaction, 0, 5f);
						}
						else if (Witnessed == StudentWitnessType.DroppedWeapon)
						{
							Subtitle.UpdateLabel(SubtitleType.BloodyWeaponReaction, 0, 5f);
						}
						else if (Witnessed == StudentWitnessType.BloodPool)
						{
							Subtitle.UpdateLabel(SubtitleType.BloodPoolReaction, 0, 5f);
						}
						else if (Witnessed == StudentWitnessType.HoldingBloodyClothing)
						{
							Subtitle.UpdateLabel(SubtitleType.HoldingBloodyClothingReaction, 0, 5f);
						}
						else
						{
							Subtitle.UpdateLabel(SubtitleType.HmmReaction, 1, 3f);
						}
					}
					else if (Witnessed == StudentWitnessType.None)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentHmm, 0, 5f);
					}
					else if (Witnessed == StudentWitnessType.Corpse)
					{
						if (FoundEnemyCorpse)
						{
							Subtitle.UpdateLabel(SubtitleType.EvilDelinquentCorpseReaction, 1, 5f);
						}
						else if (Corpse.Student.Club == ClubType.Delinquent)
						{
							Subtitle.Speaker = this;
							Subtitle.UpdateLabel(SubtitleType.DelinquentFriendCorpseReaction, 1, 5f);
							FoundFriendCorpse = true;
						}
						else
						{
							Subtitle.Speaker = this;
							Subtitle.UpdateLabel(SubtitleType.DelinquentCorpseReaction, 1, 5f);
						}
					}
					else if (Witnessed == StudentWitnessType.Weapon && !Injured)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentWeaponReaction, 0, 3f);
					}
					else
					{
						Subtitle.Speaker = this;
						if (!WitnessedWeapon)
						{
							Subtitle.UpdateLabel(SubtitleType.DelinquentReaction, 0, 5f);
						}
						else
						{
							Subtitle.UpdateLabel(SubtitleType.LimbReaction, 0, 5f);
						}
					}
				}
				else if (!Yandere.Egg)
				{
					if (Witnessed == StudentWitnessType.WeaponAndBloodAndInsanity)
					{
						CharacterAnimation.CrossFade("senpaiInsanityReaction_00");
						GameOverCause = GameOverType.Insanity;
					}
					else if (Witnessed == StudentWitnessType.WeaponAndBlood)
					{
						CharacterAnimation.CrossFade("senpaiWeaponReaction_00");
						GameOverCause = GameOverType.Weapon;
					}
					else if (Witnessed == StudentWitnessType.WeaponAndInsanity)
					{
						CharacterAnimation.CrossFade("senpaiInsanityReaction_00");
						GameOverCause = GameOverType.Insanity;
					}
					else if (Witnessed == StudentWitnessType.BloodAndInsanity)
					{
						CharacterAnimation.CrossFade("senpaiInsanityReaction_00");
						GameOverCause = GameOverType.Insanity;
					}
					else if (Witnessed == StudentWitnessType.Weapon)
					{
						CharacterAnimation.CrossFade("senpaiWeaponReaction_00");
						GameOverCause = GameOverType.Weapon;
					}
					else if (Witnessed == StudentWitnessType.Blood)
					{
						CharacterAnimation.CrossFade("senpaiBloodReaction_00");
						GameOverCause = GameOverType.Blood;
					}
					else if (Witnessed == StudentWitnessType.Insanity)
					{
						CharacterAnimation.CrossFade("senpaiInsanityReaction_00");
						GameOverCause = GameOverType.Insanity;
					}
					else if (Witnessed == StudentWitnessType.Lewd)
					{
						CharacterAnimation.CrossFade("senpaiLewdReaction_00");
						GameOverCause = GameOverType.Lewd;
					}
					else if (Witnessed == StudentWitnessType.Stalking)
					{
						if (Concern < 5)
						{
							Subtitle.UpdateLabel(SubtitleType.SenpaiStalkingReaction, Concern, 4.5f);
						}
						else
						{
							CharacterAnimation.CrossFade("senpaiCreepyReaction_00");
							GameOverCause = GameOverType.Stalking;
						}
						Witnessed = StudentWitnessType.None;
					}
					else if (Witnessed == StudentWitnessType.Corpse)
					{
						Subtitle.UpdateLabel(SubtitleType.SenpaiCorpseReaction, 1, 5f);
					}
					else if (Witnessed == StudentWitnessType.Violence)
					{
						CharacterAnimation.CrossFade("senpaiFightReaction_00");
						GameOverCause = GameOverType.Violence;
						Concern = 5;
					}
					if (Concern == 5)
					{
						CharacterAnimation["scaredFace_00"].weight = 0f;
						CharacterAnimation[AngryFaceAnim].weight = 0f;
						Yandere.ShoulderCamera.enabled = true;
						Yandere.ShoulderCamera.Noticed = true;
						Yandere.RPGCamera.enabled = false;
						Stop = true;
					}
				}
				Reacted = true;
			}
			if (Club == ClubType.Council && (double)DistanceToPlayer < 1.1 && (Yandere.Armed || Yandere.Carrying || Yandere.Dragging) && Prompt.InSight)
			{
				Spray();
			}
			return;
		}
		Alarm -= Time.deltaTime * 100f * (1f / Paranoia);
		if (StudentManager.CombatMinigame.Delinquent == null || StudentManager.CombatMinigame.Delinquent == this)
		{
			targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
		}
		else
		{
			targetRotation = Quaternion.LookRotation(new Vector3(StudentManager.CombatMinigame.Midpoint.position.x, base.transform.position.y, StudentManager.CombatMinigame.Midpoint.position.z) - base.transform.position);
		}
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
		if (Yandere.DelinquentFighting && StudentManager.CombatMinigame.Delinquent != this)
		{
			if (StudentManager.CombatMinigame.Path < 4)
			{
				if (DistanceToPlayer < 1f)
				{
					MyController.Move(base.transform.forward * Time.deltaTime * -1f);
				}
				if (Vector3.Distance(base.transform.position, StudentManager.CombatMinigame.Delinquent.transform.position) < 1f)
				{
					MyController.Move(base.transform.forward * Time.deltaTime * -1f);
				}
				if (Yandere.enabled)
				{
					CheerTimer = Mathf.MoveTowards(CheerTimer, 0f, Time.deltaTime);
					if (CheerTimer == 0f)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentCheer, 0, 5f);
						CheerTimer = UnityEngine.Random.Range(2f, 3f);
					}
				}
				CharacterAnimation.CrossFade(RandomCheerAnim);
				if (CharacterAnimation[RandomCheerAnim].time >= CharacterAnimation[RandomCheerAnim].length)
				{
					RandomCheerAnim = CheerAnims[UnityEngine.Random.Range(0, CheerAnims.Length)];
				}
				ThreatPhase = 3;
				ThreatTimer = 0f;
				if (WitnessedMurder)
				{
					Injured = true;
				}
			}
			else
			{
				CharacterAnimation.CrossFade(IdleAnim, 5f);
				NoTalk = true;
			}
			return;
		}
		if (!Injured)
		{
			if (DistanceToPlayer > 5f + ThreatDistance && ThreatPhase < 4)
			{
				ThreatPhase = 3;
				ThreatTimer = 0f;
			}
			if (Yandere.Shoved || Yandere.Dumping)
			{
				return;
			}
			if (DistanceToPlayer > 1f && Patience > 0)
			{
				if (ThreatPhase == 1)
				{
					CharacterAnimation.CrossFade("delinquentShock_00");
					if (CharacterAnimation["delinquentShock_00"].time >= CharacterAnimation["delinquentShock_00"].length)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentThreatened, 0, 3f);
						CharacterAnimation.CrossFade("delinquentCombatIdle_00");
						ThreatTimer = 5f;
						ThreatPhase++;
					}
				}
				else if (ThreatPhase == 2)
				{
					ThreatTimer -= Time.deltaTime;
					if (ThreatTimer < 0f)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentTaunt, 0, 5f);
						ThreatTimer = 5f;
						ThreatPhase++;
					}
				}
				else if (ThreatPhase == 3)
				{
					ThreatTimer -= Time.deltaTime;
					if (ThreatTimer < 0f)
					{
						if (!NoTalk)
						{
							Subtitle.Speaker = this;
							Subtitle.UpdateLabel(SubtitleType.DelinquentCalm, 0, 5f);
						}
						CharacterAnimation.CrossFade(IdleAnim, 5f);
						ThreatTimer = 5f;
						ThreatPhase++;
					}
				}
				else
				{
					if (ThreatPhase != 4)
					{
						return;
					}
					ThreatTimer -= Time.deltaTime;
					if (ThreatTimer < 0f)
					{
						if (CurrentDestination != Destinations[Phase])
						{
							StopInvestigating();
						}
						Distracted = false;
						Threatened = false;
						Alarmed = false;
						Routine = true;
						NoTalk = false;
						IgnoreTimer = 5f;
						AlarmTimer = 0f;
					}
				}
				return;
			}
			if (!NoTalk)
			{
				string text = string.Empty;
				if (!Male)
				{
					text = "Female_";
				}
				if (StudentID == 46)
				{
					CharacterAnimation.CrossFade("delinquentDrawWeapon_00");
				}
				if (StudentManager.CombatMinigame.Delinquent == null)
				{
					Yandere.CharacterAnimation.CrossFade("Yandere_CombatIdle");
					if (MyWeapon.transform.parent != ItemParent)
					{
						CharacterAnimation.CrossFade(text + "delinquentDrawWeapon_00");
					}
					else
					{
						CharacterAnimation.CrossFade("delinquentCombatIdle_00");
					}
					if (Yandere.Carrying || Yandere.Dragging)
					{
						Yandere.EmptyHands();
					}
					if (Yandere.Pickpocketing)
					{
						Yandere.Caught = true;
						PickPocket.PickpocketMinigame.End();
					}
					Yandere.StopLaughing();
					Yandere.StopAiming();
					Yandere.Unequip();
					if (Yandere.PickUp != null)
					{
						Yandere.EmptyHands();
					}
					Yandere.DelinquentFighting = true;
					Yandere.NearSenpai = false;
					Yandere.Degloving = false;
					Yandere.CanMove = false;
					Yandere.GloveTimer = 0f;
					Distracted = true;
					Fighting = true;
					ThreatTimer = 0f;
					StudentManager.CombatMinigame.Delinquent = this;
					StudentManager.CombatMinigame.enabled = true;
					StudentManager.CombatMinigame.StartCombat();
					SpeechLines.Stop();
					SpawnAlarmDisc();
					if (WitnessedMurder)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentAvenge, 0, 5f);
					}
					else if (!StudentManager.CombatMinigame.Practice)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentFight, 0, 5f);
					}
				}
				Yandere.transform.rotation = Quaternion.LookRotation(new Vector3(Hips.transform.position.x, Yandere.transform.position.y, Hips.transform.position.z) - Yandere.transform.position);
				if (CharacterAnimation[text + "delinquentDrawWeapon_00"].time >= 0.5f)
				{
					MyWeapon.transform.parent = ItemParent;
					MyWeapon.transform.localEulerAngles = new Vector3(0f, 15f, 0f);
					MyWeapon.transform.localPosition = new Vector3(0.01f, 0f, 0f);
				}
				if (CharacterAnimation[text + "delinquentDrawWeapon_00"].time >= CharacterAnimation[text + "delinquentDrawWeapon_00"].length)
				{
					Threatened = false;
					Alarmed = false;
					base.enabled = false;
				}
				return;
			}
			ThreatTimer -= Time.deltaTime;
			if (ThreatTimer < 0f)
			{
				if (CurrentDestination != Destinations[Phase])
				{
					StopInvestigating();
				}
				Distracted = false;
				Threatened = false;
				Alarmed = false;
				Routine = true;
				NoTalk = false;
				IgnoreTimer = 5f;
				AlarmTimer = 0f;
			}
			return;
		}
		ThreatTimer += Time.deltaTime;
		if (ThreatTimer > 5f)
		{
			DistanceToDestination = 100f;
			if (!WitnessedMurder)
			{
				Distracted = false;
				Threatened = false;
				EndAlarm();
			}
			else
			{
				Threatened = false;
				Alarmed = false;
				Injured = false;
				PersonaReaction();
			}
		}
	}

	private void UpdateBurning()
	{
		if (DistanceToPlayer < 1f && !Yandere.Shoved)
		{
			PushYandereAway();
		}
		if (BurnTarget != Vector3.zero)
		{
			MoveTowardsTarget(BurnTarget);
		}
		if (CharacterAnimation[BurningAnim].time > CharacterAnimation[BurningAnim].length)
		{
			DeathType = DeathType.Burning;
			BecomeRagdoll();
		}
	}

	private void UpdateSplashed()
	{
		if (Yandere.Tripping)
		{
			targetRotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
		}
		SplashTimer += Time.deltaTime;
		if (SplashTimer > 2f && SplashPhase == 1)
		{
			if (Gas)
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 5, 5f);
			}
			else if (Bloody)
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 3, 5f);
			}
			else if (Yandere.Tripping)
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 7, 5f);
			}
			else
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 1, 5f);
			}
			CharacterAnimation[SplashedAnim].speed = 0.5f;
			SplashPhase++;
		}
		if (!(SplashTimer > 12f) || SplashPhase != 2)
		{
			return;
		}
		if (LightSwitch == null)
		{
			if (Gas)
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 6, 5f);
			}
			else if (Bloody)
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 4, 5f);
			}
			else
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 2, 5f);
			}
			SplashPhase++;
			if (!Male)
			{
				CurrentDestination = StudentManager.FemaleStripSpot;
				Pathfinding.target = StudentManager.FemaleStripSpot;
			}
			else
			{
				CurrentDestination = StudentManager.MaleStripSpot;
				Pathfinding.target = StudentManager.MaleStripSpot;
			}
		}
		else if (!LightSwitch.BathroomLight.activeInHierarchy)
		{
			if (LightSwitch.Panel.useGravity)
			{
				LightSwitch.Prompt.Hide();
				LightSwitch.Prompt.enabled = false;
				Prompt.Hide();
				Prompt.enabled = false;
			}
			Subtitle.UpdateLabel(SubtitleType.LightSwitchReaction, 1, 5f);
			CurrentDestination = LightSwitch.ElectrocutionSpot;
			Pathfinding.target = LightSwitch.ElectrocutionSpot;
			Pathfinding.speed = 1f;
			BathePhase = -1;
			InDarkness = true;
		}
		else
		{
			if (!Bloody)
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 2, 5f);
			}
			else
			{
				Subtitle.Speaker = this;
				Subtitle.UpdateLabel(SplashSubtitleType, 4, 5f);
			}
			SplashPhase++;
			CurrentDestination = StudentManager.FemaleStripSpot;
			Pathfinding.target = StudentManager.FemaleStripSpot;
		}
		Pathfinding.canSearch = true;
		Pathfinding.canMove = true;
		Splashed = false;
	}

	private void UpdateTurningOffRadio()
	{
		if (Radio.On || (RadioPhase == 3 && Radio.transform.parent == null))
		{
			if (RadioPhase == 1)
			{
				targetRotation = Quaternion.LookRotation(new Vector3(Radio.transform.position.x, base.transform.position.y, Radio.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				RadioTimer += Time.deltaTime;
				if (RadioTimer > 3f)
				{
					if (Persona == PersonaType.PhoneAddict)
					{
						SmartPhone.SetActive(true);
					}
					CharacterAnimation.CrossFade(WalkAnim);
					CurrentDestination = Radio.transform;
					Pathfinding.target = Radio.transform;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					RadioTimer = 0f;
					RadioPhase++;
				}
			}
			else if (RadioPhase == 2)
			{
				if (DistanceToDestination < 0.5f)
				{
					CharacterAnimation.CrossFade(RadioAnim);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					SmartPhone.SetActive(false);
					RadioPhase++;
				}
			}
			else
			{
				if (RadioPhase != 3)
				{
					return;
				}
				targetRotation = Quaternion.LookRotation(new Vector3(Radio.transform.position.x, base.transform.position.y, Radio.transform.position.z) - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, targetRotation, 10f * Time.deltaTime);
				RadioTimer += Time.deltaTime;
				if (RadioTimer > 4f)
				{
					if (Persona == PersonaType.PhoneAddict)
					{
						SmartPhone.SetActive(true);
					}
					CurrentDestination = Destinations[Phase];
					Pathfinding.target = Destinations[Phase];
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					ForgetRadio();
				}
				else if (RadioTimer > 2f)
				{
					Radio.Victim = null;
					Radio.TurnOff();
				}
			}
		}
		else
		{
			if (RadioPhase < 100)
			{
				CharacterAnimation.CrossFade(IdleAnim);
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				RadioPhase = 100;
				RadioTimer = 0f;
			}
			targetRotation = Quaternion.LookRotation(new Vector3(Radio.transform.position.x, base.transform.position.y, Radio.transform.position.z) - base.transform.position);
			RadioTimer += Time.deltaTime;
			if (RadioTimer > 1f || Radio.transform.parent != null)
			{
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				ForgetRadio();
			}
		}
	}

	private void UpdateVomiting()
	{
		if (VomitPhase != 0 && VomitPhase != 4)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
			MoveTowardsTarget(CurrentDestination.position);
		}
		if (VomitPhase == 0)
		{
			if (DistanceToDestination < 0.5f)
			{
				Prompt.Label[0].text = "     Drown";
				Prompt.enabled = true;
				Drownable = true;
				VomitDoor.Prompt.enabled = false;
				VomitDoor.Prompt.Hide();
				VomitDoor.enabled = false;
				CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
				CharacterAnimation.CrossFade(VomitAnim);
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				VomitPhase++;
			}
		}
		else if (VomitPhase == 1)
		{
			if (CharacterAnimation[VomitAnim].time > 1f)
			{
				VomitEmitter.gameObject.SetActive(true);
				VomitPhase++;
			}
		}
		else if (VomitPhase == 2)
		{
			if (CharacterAnimation[VomitAnim].time > 13f)
			{
				VomitEmitter.gameObject.SetActive(false);
				VomitPhase++;
			}
		}
		else if (VomitPhase == 3)
		{
			if (CharacterAnimation[VomitAnim].time >= CharacterAnimation[VomitAnim].length)
			{
				Prompt.Label[0].text = "     Talk";
				Drownable = false;
				WalkAnim = OriginalWalkAnim;
				CharacterAnimation.CrossFade(WalkAnim);
				if (Male)
				{
					StudentManager.GetMaleWashSpot(this);
					Pathfinding.target = StudentManager.MaleWashSpot;
					CurrentDestination = StudentManager.MaleWashSpot;
				}
				else
				{
					StudentManager.GetFemaleWashSpot(this);
					Pathfinding.target = StudentManager.FemaleWashSpot;
					CurrentDestination = StudentManager.FemaleWashSpot;
				}
				VomitDoor.Prompt.enabled = true;
				VomitDoor.enabled = true;
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				Pathfinding.speed = 1f;
				DistanceToDestination = 100f;
				VomitPhase++;
			}
		}
		else if (VomitPhase == 4)
		{
			if (DistanceToDestination < 0.5f)
			{
				CharacterAnimation.CrossFade(WashFaceAnim);
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				VomitPhase++;
			}
		}
		else if (VomitPhase == 5 && CharacterAnimation[WashFaceAnim].time > CharacterAnimation[WashFaceAnim].length)
		{
			CharacterAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
			Prompt.Label[0].text = "     Talk";
			Pathfinding.canSearch = true;
			Pathfinding.canMove = true;
			Distracted = false;
			Drownable = false;
			Vomiting = false;
			Private = false;
			CanTalk = true;
			Routine = true;
			Emetic = false;
			VomitPhase = 0;
			WalkAnim = OriginalWalkAnim;
			Phase++;
			Pathfinding.target = Destinations[Phase];
			CurrentDestination = Destinations[Phase];
			DistanceToDestination = 100f;
		}
	}

	private void UpdateConfessing()
	{
		if (!Male)
		{
			if (ConfessPhase == 1)
			{
				if (DistanceToDestination < 0.5f)
				{
					Cosmetic.MyRenderer.materials[2].SetFloat("_BlendAmount", 1f);
					CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
					CharacterAnimation.CrossFade("f02_insertNote_00");
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Note.SetActive(true);
					ConfessPhase++;
				}
			}
			else if (ConfessPhase == 2)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
				MoveTowardsTarget(CurrentDestination.position);
				if (CharacterAnimation["f02_insertNote_00"].time >= 9f)
				{
					Note.SetActive(false);
					ConfessPhase++;
				}
			}
			else if (ConfessPhase == 3)
			{
				if (CharacterAnimation["f02_insertNote_00"].time >= CharacterAnimation["f02_insertNote_00"].length)
				{
					CurrentDestination = StudentManager.RivalConfessionSpot;
					Pathfinding.target = StudentManager.RivalConfessionSpot;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					Pathfinding.speed = 4f;
					StudentManager.LoveManager.LeftNote = true;
					CharacterAnimation.CrossFade(SprintAnim);
					ConfessPhase++;
				}
			}
			else if (ConfessPhase == 4)
			{
				if (DistanceToDestination < 0.5f)
				{
					CharacterAnimation.CrossFade(IdleAnim);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					ConfessPhase++;
				}
			}
			else if (ConfessPhase == 5)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
				CharacterAnimation[ShyAnim].weight = Mathf.Lerp(CharacterAnimation[ShyAnim].weight, 1f, Time.deltaTime);
				MoveTowardsTarget(CurrentDestination.position);
			}
		}
		else if (ConfessPhase == 1)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
			MoveTowardsTarget(CurrentDestination.position);
			if (CharacterAnimation["keepNote_00"].time > 14f)
			{
				Note.SetActive(false);
			}
			else if ((double)CharacterAnimation["keepNote_00"].time > 4.5)
			{
				Note.SetActive(true);
			}
			if (CharacterAnimation["keepNote_00"].time >= CharacterAnimation["keepNote_00"].length)
			{
				CurrentDestination = StudentManager.SuitorConfessionSpot;
				Pathfinding.target = StudentManager.SuitorConfessionSpot;
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				Pathfinding.speed = 4f;
				CharacterAnimation.CrossFade(SprintAnim);
				ConfessPhase++;
			}
		}
		else if (ConfessPhase == 2)
		{
			if (DistanceToDestination < 0.5f)
			{
				CharacterAnimation.CrossFade("exhausted_00");
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				ConfessPhase++;
			}
		}
		else if (ConfessPhase == 3)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, CurrentDestination.rotation, Time.deltaTime * 10f);
			MoveTowardsTarget(CurrentDestination.position);
		}
	}

	private void UpdateMisc()
	{
		if (IgnoreTimer > 0f)
		{
			IgnoreTimer -= Time.deltaTime;
		}
		if (!Fleeing)
		{
			if (base.transform.position.z < -100f)
			{
				if (base.transform.position.y < -11f && StudentID > 1)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			else
			{
				if (base.transform.position.y < -0f)
				{
					base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
				}
				if (Routine && Yandere.CanMove && !Yandere.Egg && !StudentManager.Pose && !ShoeRemoval.enabled)
				{
					if (StudentManager.MissionMode && (double)DistanceToPlayer < 0.5)
					{
						Yandere.Shutter.FaceStudent = this;
						Yandere.Shutter.Penalize();
					}
					if ((Club == ClubType.Council || (Club == ClubType.Delinquent && !Injured)) && (double)DistanceToPlayer < 0.5 && (Yandere.h != 0f || Yandere.v != 0f))
					{
						if (Club == ClubType.Delinquent)
						{
							Subtitle.Speaker = this;
							Subtitle.UpdateLabel(SubtitleType.DelinquentShove, 0, 3f);
						}
						Shove();
					}
					if (Club == ClubType.Council)
					{
						if (DistanceToPlayer < 5f)
						{
							if (DistanceToPlayer < 2f)
							{
								StudentManager.TutorialWindow.ShowCouncilMessage = true;
							}
							float f = Vector3.Angle(-base.transform.forward, Yandere.transform.position - base.transform.position);
							if (Mathf.Abs(f) <= 45f && Yandere.Stance.Current != StanceType.Crouching && Yandere.Stance.Current != StanceType.Crawling && (Yandere.h != 0f || Yandere.v != 0f) && (Input.GetButton("LB") || DistanceToPlayer < 2f))
							{
								DistractionSpot = Yandere.transform.position;
								Alarm = 100f + Time.deltaTime * 100f * (1f / Paranoia);
								FocusOnYandere = true;
								Pathfinding.canSearch = false;
								Pathfinding.canMove = false;
								StopInvestigating();
							}
						}
						if (DistanceToPlayer < 1f)
						{
							float f2 = Vector3.Angle(-base.transform.forward, Yandere.transform.position - base.transform.position);
							if (Mathf.Abs(f2) > 45f && (Yandere.Armed || Yandere.Carrying || Yandere.Dragging) && Prompt.InSight)
							{
								Spray();
							}
						}
					}
				}
			}
		}
		if (Wet && !Splashed && BathePhase == 1 && !Burning && !Dying)
		{
			if (CharacterAnimation[WetAnim].weight < 1f)
			{
				CharacterAnimation[WetAnim].weight = 1f;
			}
		}
		else if (CharacterAnimation[WetAnim].weight > 0f)
		{
			CharacterAnimation[WetAnim].weight = 0f;
		}
	}

	private void LateUpdate()
	{
		if (StudentManager.DisableFarAnims && DistanceToPlayer >= (float)StudentManager.FarAnimThreshold && CharacterAnimation.cullingType != 0 && !WitnessCamera.Show)
		{
			CharacterAnimation.enabled = false;
		}
		else
		{
			CharacterAnimation.enabled = true;
		}
		if (EyeShrink > 0f)
		{
			if (EyeShrink > 1f)
			{
				EyeShrink = 1f;
			}
			LeftEye.localPosition = new Vector3(LeftEye.localPosition.x, LeftEye.localPosition.y, LeftEyeOrigin.z - EyeShrink * 0.01f);
			RightEye.localPosition = new Vector3(RightEye.localPosition.x, RightEye.localPosition.y, RightEyeOrigin.z + EyeShrink * 0.01f);
			LeftEye.localScale = new Vector3(1f - EyeShrink * 0.5f, 1f - EyeShrink * 0.5f, LeftEye.localScale.z);
			RightEye.localScale = new Vector3(1f - EyeShrink * 0.5f, 1f - EyeShrink * 0.5f, RightEye.localScale.z);
			PreviousEyeShrink = EyeShrink;
		}
		if (!Male)
		{
			if (Shy)
			{
				if (Routine)
				{
					if ((Phase == 2 && DistanceToDestination < 1f) || (Phase == 4 && DistanceToDestination < 1f) || (Actions[Phase] == StudentActionType.SitAndTakeNotes && DistanceToDestination < 1f) || (Actions[Phase] == StudentActionType.Clean && DistanceToDestination < 1f) || (Actions[Phase] == StudentActionType.Read && DistanceToDestination < 1f))
					{
						CharacterAnimation[ShyAnim].weight = Mathf.Lerp(CharacterAnimation[ShyAnim].weight, 0f, Time.deltaTime);
					}
					else
					{
						CharacterAnimation[ShyAnim].weight = Mathf.Lerp(CharacterAnimation[ShyAnim].weight, 1f, Time.deltaTime);
					}
				}
				else if (!Headache)
				{
					CharacterAnimation[ShyAnim].weight = Mathf.Lerp(CharacterAnimation[ShyAnim].weight, 0f, Time.deltaTime);
				}
			}
			if (!BoobsResized)
			{
				RightBreast.localScale = new Vector3(BreastSize, BreastSize, BreastSize);
				LeftBreast.localScale = new Vector3(BreastSize, BreastSize, BreastSize);
				if (!Cosmetic.CustomEyes)
				{
					RightBreast.gameObject.name = "RightBreastRENAMED";
					LeftBreast.gameObject.name = "LeftBreastRENAMED";
				}
				BoobsResized = true;
			}
			if (Following)
			{
				if (Gush)
				{
					Neck.LookAt(GushTarget);
				}
				else
				{
					Neck.LookAt(DefaultTarget);
				}
			}
			if (StudentManager.Egg && SecurityCamera.activeInHierarchy)
			{
				Head.localScale = new Vector3(0f, 0f, 0f);
			}
			if (Club == ClubType.Bully)
			{
				for (int i = 0; i < 4; i++)
				{
					if (Skirt[i] != null)
					{
						Transform transform = Skirt[i].transform;
						transform.localScale = new Vector3(transform.localScale.x, 2f / 3f, transform.localScale.z);
					}
				}
			}
		}
		if (Routine && !InEvent && !Meeting && !GoAway)
		{
			if ((DistanceToDestination < TargetDistance && Actions[Phase] == StudentActionType.SitAndSocialize) || (DistanceToDestination < TargetDistance && StudentID != 36 && Actions[Phase] == StudentActionType.Meeting) || (DistanceToDestination < TargetDistance && Actions[Phase] == StudentActionType.Sleuth && StudentManager.SleuthPhase != 2 && StudentManager.SleuthPhase != 3) || (Club == ClubType.Photography && ClubActivity))
			{
				if (CharacterAnimation[SocialSitAnim].weight != 1f)
				{
					CharacterAnimation[SocialSitAnim].weight = Mathf.Lerp(CharacterAnimation[SocialSitAnim].weight, 1f, Time.deltaTime * 10f);
					if ((double)CharacterAnimation[SocialSitAnim].weight > 0.99)
					{
						CharacterAnimation[SocialSitAnim].weight = 1f;
					}
				}
			}
			else if (CharacterAnimation[SocialSitAnim].weight != 0f)
			{
				CharacterAnimation[SocialSitAnim].weight = Mathf.Lerp(CharacterAnimation[SocialSitAnim].weight, 0f, Time.deltaTime * 10f);
				if ((double)CharacterAnimation[SocialSitAnim].weight < 0.01)
				{
					CharacterAnimation[SocialSitAnim].weight = 0f;
				}
			}
		}
		else if (CharacterAnimation[SocialSitAnim].weight != 0f)
		{
			CharacterAnimation[SocialSitAnim].weight = Mathf.Lerp(CharacterAnimation[SocialSitAnim].weight, 0f, Time.deltaTime * 10f);
			if ((double)CharacterAnimation[SocialSitAnim].weight < 0.01)
			{
				CharacterAnimation[SocialSitAnim].weight = 0f;
			}
		}
		if (DK)
		{
			Arm[0].localScale = new Vector3(2f, 2f, 2f);
			Arm[1].localScale = new Vector3(2f, 2f, 2f);
			Head.localScale = new Vector3(2f, 2f, 2f);
		}
		if (Fate > 0 && Clock.HourTime > TimeOfDeath)
		{
			Yandere.TargetStudent = this;
			StudentManager.Shinigami.Effect = Fate - 1;
			StudentManager.Shinigami.Attack();
			Yandere.TargetStudent = null;
			Fate = 0;
		}
		if (Yandere.BlackHole && DistanceToPlayer < 2.5f)
		{
			if (DeathScream != null)
			{
				UnityEngine.Object.Instantiate(DeathScream, base.transform.position + Vector3.up, Quaternion.identity);
			}
			BlackHoleEffect[0].enabled = true;
			BlackHoleEffect[1].enabled = true;
			BlackHoleEffect[2].enabled = true;
			DeathType = DeathType.EasterEgg;
			CharacterAnimation.Stop();
			Suck.enabled = true;
			BecomeRagdoll();
			Dying = true;
		}
		if (CameraReacting && (StudentManager.NEStairs.bounds.Contains(base.transform.position) || StudentManager.NWStairs.bounds.Contains(base.transform.position) || StudentManager.SEStairs.bounds.Contains(base.transform.position) || StudentManager.SWStairs.bounds.Contains(base.transform.position)))
		{
			Spine.LookAt(Yandere.Spine[0]);
			Head.LookAt(Yandere.Head);
		}
	}

	public void CalculateReputationPenalty()
	{
		if ((Male && PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 2) || PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 4)
		{
			RepDeduction += RepLoss * 0.2f;
		}
		if (PlayerGlobals.Reputation < -33.33333f)
		{
			RepDeduction += RepLoss * 0.2f;
		}
		if (PlayerGlobals.Reputation > 33.33333f)
		{
			RepDeduction -= RepLoss * 0.2f;
		}
		if (PlayerGlobals.GetStudentFriend(StudentID))
		{
			RepDeduction += RepLoss * 0.2f;
		}
		if (PlayerGlobals.PantiesEquipped == 1)
		{
			RepDeduction += RepLoss * 0.2f;
		}
		if (PlayerGlobals.SocialBonus > 0)
		{
			RepDeduction += RepLoss * 0.2f;
		}
		ChameleonCheck();
		if (Chameleon)
		{
			RepLoss *= 0.5f;
		}
		if (Yandere.Persona == YanderePersonaType.Aggressive)
		{
			RepLoss *= 2f;
		}
		if (Club == ClubType.Bully)
		{
			RepLoss *= 2f;
		}
		if (Club == ClubType.Delinquent)
		{
			RepDeduction = 0f;
			RepLoss = 0f;
		}
	}

	public void MoveTowardsTarget(Vector3 target)
	{
		if (Time.timeScale > 0.0001f && MyController.enabled)
		{
			Vector3 vector = target - base.transform.position;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude > 1E-06f)
			{
				MyController.Move(vector * (Time.deltaTime * 5f / Time.timeScale));
			}
		}
	}

	private void LookTowardsTarget(Vector3 target)
	{
		if (!(Time.timeScale > 0.0001f))
		{
		}
	}

	public void AttackReaction()
	{
		if (PhotoEvidence)
		{
			SmartPhone.GetComponent<SmartphoneScript>().enabled = true;
			SmartPhone.GetComponent<PromptScript>().enabled = true;
			SmartPhone.GetComponent<Rigidbody>().useGravity = true;
			SmartPhone.GetComponent<Collider>().enabled = true;
			SmartPhone.transform.parent = null;
			SmartPhone.layer = 15;
		}
		if (!WitnessedMurder)
		{
			float f = Vector3.Angle(-base.transform.forward, Yandere.transform.position - base.transform.position);
			Yandere.AttackManager.Stealth = Mathf.Abs(f) <= 45f;
		}
		if (ReturningMisplacedWeapon)
		{
			DropMisplacedWeapon();
		}
		if (!Male)
		{
			if (Club != ClubType.Council)
			{
				StudentManager.TranqDetector.TranqCheck();
			}
			CharacterAnimation["f02_smile_00"].weight = 0f;
			SmartPhone.SetActive(false);
		}
		WitnessCamera.Show = false;
		Pathfinding.canSearch = false;
		Pathfinding.canMove = false;
		Yandere.CharacterAnimation["f02_idleShort_00"].time = 0f;
		Yandere.CharacterAnimation["f02_swingA_00"].time = 0f;
		Yandere.MyController.radius = 0f;
		Yandere.TargetStudent = this;
		Yandere.Obscurance.enabled = false;
		Yandere.YandereVision = false;
		Yandere.Attacking = true;
		Yandere.CanMove = false;
		if (Yandere.Equipped > 0 && Yandere.EquippedWeapon.AnimID == 2)
		{
			Yandere.CharacterAnimation[Yandere.ArmedAnims[2]].weight = 0f;
		}
		if (DetectionMarker != null)
		{
			DetectionMarker.Tex.enabled = false;
		}
		EmptyHands();
		DropPlate();
		MyController.radius = 0f;
		if (Persona == PersonaType.PhoneAddict)
		{
			Countdown.gameObject.SetActive(false);
			ChaseCamera.SetActive(false);
			if (StudentManager.ChaseCamera == ChaseCamera)
			{
				StudentManager.ChaseCamera = null;
			}
		}
		InvestigatingBloodPool = false;
		Investigating = false;
		Pen.SetActive(false);
		EatingSnack = false;
		SpeechLines.Stop();
		Attacked = true;
		Alarmed = false;
		Fleeing = false;
		Routine = false;
		ReadPhase = 0;
		Dying = true;
		Wet = false;
		Prompt.Hide();
		Prompt.enabled = false;
		if (Following)
		{
			ParticleSystem.EmissionModule emission = Hearts.emission;
			emission.enabled = false;
			Yandere.Followers--;
			Following = false;
		}
		if (Distracting)
		{
			DistractionTarget.TargetedForDistraction = false;
			DistractionTarget.Octodog.SetActive(false);
			DistractionTarget.Distracted = false;
			Distracting = false;
		}
		if (Teacher)
		{
			if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus > 0 && Yandere.EquippedWeapon.Type == WeaponType.Knife)
			{
				Pathfinding.target = Yandere.transform;
				CurrentDestination = Yandere.transform;
				Yandere.Attacking = false;
				Attacked = false;
				Fleeing = true;
				Dying = false;
				Persona = PersonaType.Heroic;
				BeginStruggle();
			}
			else
			{
				Yandere.HeartRate.gameObject.SetActive(false);
				Yandere.ShoulderCamera.Counter = true;
				Yandere.ShoulderCamera.OverShoulder = false;
				Yandere.RPGCamera.enabled = false;
				Yandere.Senpai = base.transform;
				Yandere.Attacking = true;
				Yandere.CanMove = false;
				Yandere.Talking = false;
				Yandere.Noticed = true;
				Yandere.HUD.alpha = 0f;
			}
		}
		else if (Strength == 9)
		{
			Yandere.CharacterAnimation.CrossFade("f02_counterA_00");
			Yandere.HeartRate.gameObject.SetActive(false);
			Yandere.ShoulderCamera.Counter = true;
			Yandere.ShoulderCamera.OverShoulder = false;
			Yandere.RPGCamera.enabled = false;
			Yandere.Senpai = base.transform;
			Yandere.Attacking = true;
			Yandere.CanMove = false;
			Yandere.Talking = false;
			Yandere.Noticed = true;
			Yandere.HUD.alpha = 0f;
		}
		else
		{
			if (!Yandere.AttackManager.Stealth)
			{
				Subtitle.UpdateLabel(SubtitleType.Dying, 0, 1f);
				SpawnAlarmDisc();
			}
			if (Yandere.SanityBased)
			{
				Yandere.AttackManager.Attack(Character, Yandere.EquippedWeapon);
			}
		}
		if (StudentManager.Reporter == this)
		{
			StudentManager.Reporter = null;
			if (ReportPhase == 0)
			{
				StudentManager.CorpseLocation.position = Vector3.zero;
			}
		}
		if (Club == ClubType.Delinquent && MyWeapon != null && MyWeapon.transform.parent == ItemParent)
		{
			MyWeapon.transform.parent = null;
			MyWeapon.MyCollider.enabled = true;
			MyWeapon.Prompt.enabled = true;
			Rigidbody component = MyWeapon.GetComponent<Rigidbody>();
			component.constraints = RigidbodyConstraints.None;
			component.isKinematic = false;
			component.useGravity = true;
		}
		if (PhotoEvidence)
		{
			CameraFlash.SetActive(false);
			SmartPhone.SetActive(true);
		}
	}

	public void DropPlate()
	{
		if (MyPlate != null && MyPlate.parent == RightHand)
		{
			MyPlate.GetComponent<Rigidbody>().isKinematic = false;
			MyPlate.GetComponent<Rigidbody>().useGravity = true;
			MyPlate.GetComponent<Collider>().enabled = true;
			MyPlate.parent = null;
			MyPlate.gameObject.SetActive(true);
		}
	}

	public void SenpaiNoticed()
	{
		Yandere.Noticed = true;
		if (WeaponToTakeAway != null && Teacher && !Yandere.Attacking)
		{
			Yandere.EquippedWeapon.Drop();
			WeaponToTakeAway.transform.position = StudentManager.WeaponBoxSpot.parent.position + new Vector3(0f, 1f, 0f);
			WeaponToTakeAway.transform.eulerAngles = new Vector3(0f, 90f, 0f);
		}
		WeaponToTakeAway = null;
		if (!Yandere.Attacking)
		{
			Yandere.EmptyHands();
		}
		Yandere.Senpai = base.transform;
		if (Yandere.Aiming)
		{
			Yandere.StopAiming();
		}
		Yandere.DetectionPanel.alpha = 0f;
		Yandere.RPGCamera.mouseSpeed = 0f;
		Yandere.LaughIntensity = 0f;
		Yandere.HUD.alpha = 0f;
		Yandere.EyeShrink = 0f;
		Yandere.Sanity = 100f;
		Yandere.ProgressBar.transform.parent.gameObject.SetActive(false);
		Yandere.HeartRate.gameObject.SetActive(false);
		Yandere.Stance.Current = StanceType.Standing;
		Yandere.ShoulderCamera.OverShoulder = false;
		Yandere.Obscurance.enabled = false;
		Yandere.DelinquentFighting = false;
		Yandere.YandereVision = false;
		Yandere.CannotRecover = true;
		Yandere.Police.Show = false;
		Yandere.Rummaging = false;
		Yandere.Laughing = false;
		Yandere.CanMove = false;
		Yandere.Dipping = false;
		Yandere.Mopping = false;
		Yandere.Talking = false;
		Yandere.Jukebox.GameOver();
		StudentManager.StopMoving();
		if (Teacher || StudentID == 1)
		{
			if (Club != ClubType.Council)
			{
				IdleAnim = OriginalIdleAnim;
			}
			base.enabled = true;
			Stop = false;
		}
	}

	private void WitnessMurder()
	{
		if ((Fleeing && WitnessedBloodPool) || ReportPhase == 2)
		{
			WitnessedBloodyWeapon = false;
			WitnessedBloodPool = false;
			WitnessedSomething = false;
			WitnessedWeapon = false;
			WitnessedLimb = false;
			Fleeing = false;
			ReportPhase = 0;
		}
		CharacterAnimation[ScaredAnim].time = 0f;
		CameraFlash.SetActive(false);
		if (!Male)
		{
			CharacterAnimation["f02_smile_00"].weight = 0f;
			WateringCan.SetActive(false);
		}
		if (MyPlate != null && MyPlate.parent == RightHand)
		{
			MyPlate.GetComponent<Rigidbody>().isKinematic = false;
			MyPlate.GetComponent<Rigidbody>().useGravity = true;
			MyPlate.GetComponent<Collider>().enabled = true;
			MyPlate.parent = null;
		}
		OccultBook.SetActive(false);
		Sketchbook.SetActive(false);
		Pencil.SetActive(false);
		Pen.SetActive(false);
		GameObject[] scienceProps = ScienceProps;
		foreach (GameObject gameObject in scienceProps)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		GameObject[] fingerfood = Fingerfood;
		foreach (GameObject gameObject2 in fingerfood)
		{
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
		}
		MurdersWitnessed++;
		SpeechLines.Stop();
		WitnessedBloodyWeapon = false;
		WitnessedBloodPool = false;
		WitnessedSomething = false;
		WitnessedWeapon = false;
		WitnessedLimb = false;
		if (ReturningMisplacedWeapon)
		{
			DropMisplacedWeapon();
		}
		ReturningMisplacedWeapon = false;
		InvestigatingBloodPool = false;
		CameraReacting = false;
		WitnessedMurder = true;
		Investigating = false;
		Distracting = false;
		EatingSnack = false;
		Threatened = false;
		Distracted = false;
		Reacted = false;
		Routine = false;
		Alarmed = true;
		NoTalk = false;
		Wet = false;
		if (OriginalPersona != PersonaType.Violent && Persona != OriginalPersona)
		{
			Persona = OriginalPersona;
			SwitchBack = false;
		}
		if (Persona == PersonaType.Spiteful && Yandere.TargetStudent != null)
		{
			if ((Bullied && Yandere.TargetStudent.Club == ClubType.Bully) || Yandere.TargetStudent.Bullied)
			{
				ScaredAnim = EvilWitnessAnim;
				Persona = PersonaType.Evil;
			}
		}
		if (Club == ClubType.Delinquent)
		{
			if (Yandere.TargetStudent != null && Yandere.TargetStudent.Club == ClubType.Bully)
			{
				ScaredAnim = EvilWitnessAnim;
				Persona = PersonaType.Evil;
			}
			if ((Yandere.Lifting || Yandere.Carrying || Yandere.Dragging) && Yandere.CurrentRagdoll.Student.Club == ClubType.Bully)
			{
				ScaredAnim = EvilWitnessAnim;
				Persona = PersonaType.Evil;
			}
		}
		if (Persona == PersonaType.Sleuth)
		{
			if (Yandere.Attacking || Yandere.Struggling || Yandere.Carrying || Yandere.Lifting || (Yandere.PickUp != null && (bool)Yandere.PickUp.BodyPart))
			{
				if (!Sleuthing)
				{
					if (StudentID == 56)
					{
						Persona = PersonaType.Heroic;
					}
					else
					{
						Persona = PersonaType.SocialButterfly;
					}
				}
				else
				{
					Persona = PersonaType.SocialButterfly;
				}
			}
		}
		if (StudentID > 1 || Yandere.Mask != null)
		{
			for (ID = 0; ID < Outlines.Length; ID++)
			{
				Outlines[ID].color = new Color(1f, 0f, 0f, 1f);
				Outlines[ID].enabled = true;
			}
			WitnessCamera.transform.parent = WitnessPOV;
			WitnessCamera.transform.localPosition = Vector3.zero;
			WitnessCamera.transform.localEulerAngles = Vector3.zero;
			WitnessCamera.MyCamera.enabled = true;
			WitnessCamera.Show = true;
			CameraEffects.MurderWitnessed();
			Witnessed = StudentWitnessType.Murder;
			if (Persona != PersonaType.Evil)
			{
				Police.Witnesses++;
			}
			if (Teacher)
			{
				StudentManager.Reporter = this;
			}
			if (Talking)
			{
				DialogueWheel.End();
				ParticleSystem.EmissionModule emission = Hearts.emission;
				emission.enabled = false;
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				Obstacle.enabled = false;
				Talk.enabled = false;
				Talking = false;
				Waiting = false;
				StudentManager.EnablePrompts();
			}
			if (Prompt.Label[0] != null && !GameGlobals.EmptyDemon)
			{
				Prompt.Label[0].text = "     Talk";
				Prompt.HideButton[0] = true;
			}
		}
		else
		{
			if (!Yandere.Attacking)
			{
				SenpaiNoticed();
			}
			Fleeing = false;
			EyeShrink = 0f;
			Yandere.Noticed = true;
			Yandere.Talking = false;
			CameraEffects.MurderWitnessed();
			Yandere.ShoulderCamera.OverShoulder = false;
			CharacterAnimation.CrossFade(ScaredAnim);
			CharacterAnimation["scaredFace_00"].weight = 1f;
			if (StudentID == 1)
			{
			}
		}
		if (Persona == PersonaType.TeachersPet && StudentManager.Reporter == null && !Police.Called)
		{
			StudentManager.CorpseLocation.position = Yandere.transform.position;
			StudentManager.LowerCorpsePosition();
			StudentManager.Reporter = this;
			ReportingMurder = true;
		}
		if (Following)
		{
			ParticleSystem.EmissionModule emission2 = Hearts.emission;
			emission2.enabled = false;
			Yandere.Followers--;
			Following = false;
		}
		Pathfinding.canSearch = false;
		Pathfinding.canMove = false;
		if (Persona == PersonaType.PhoneAddict || Sleuthing)
		{
			SmartPhone.SetActive(true);
		}
		if (SmartPhone.activeInHierarchy)
		{
			if (Persona != PersonaType.Heroic && Persona != PersonaType.Dangerous && Persona != PersonaType.Evil && Persona != PersonaType.Violent && Persona != PersonaType.Coward && !Teacher)
			{
				Persona = PersonaType.PhoneAddict;
				if (!Sleuthing)
				{
					SprintAnim = PhoneAnims[2];
				}
				else
				{
					SprintAnim = SleuthReportAnim;
				}
			}
			else
			{
				SmartPhone.SetActive(false);
			}
		}
		StopPairing();
		if (Persona != PersonaType.Heroic)
		{
			AlarmTimer = 0f;
			Alarm = 0f;
		}
		if (Teacher)
		{
			if (!Yandere.Chased)
			{
				ChaseYandere();
			}
		}
		else
		{
			SpawnAlarmDisc();
		}
		if (!PinDownWitness && Persona != PersonaType.Evil)
		{
			StudentManager.Witnesses++;
			StudentManager.WitnessList[StudentManager.Witnesses] = this;
			StudentManager.PinDownCheck();
			PinDownWitness = true;
		}
		if (Persona == PersonaType.Violent)
		{
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
		}
		if (Yandere.Mask == null)
		{
			SawMask = false;
			if (Persona != PersonaType.Evil)
			{
				Grudge = true;
			}
		}
		else
		{
			SawMask = true;
		}
		StudentManager.UpdateMe(StudentID);
	}

	private void DropMisplacedWeapon()
	{
		InvestigatingBloodPool = false;
		ReturningMisplacedWeaponPhase = 0;
		ReturningMisplacedWeapon = false;
		BloodPool.GetComponent<WeaponScript>().Returner = null;
		BloodPool.GetComponent<WeaponScript>().Drop();
		BloodPool.GetComponent<WeaponScript>().enabled = true;
		BloodPool = null;
	}

	private void ChaseYandere()
	{
		CurrentDestination = Yandere.transform;
		Pathfinding.target = Yandere.transform;
		Pathfinding.speed = 7.5f;
		StudentManager.Portal.SetActive(false);
		if (Yandere.Pursuer == null)
		{
			Yandere.Pursuer = this;
		}
		TargetDistance = 0.5f;
		AlarmTimer = 0f;
		Chasing = false;
		Fleeing = false;
		StudentManager.UpdateStudents();
	}

	private void PersonaReaction()
	{
		if (Persona == PersonaType.Sleuth)
		{
			if (Sleuthing)
			{
				Persona = PersonaType.PhoneAddict;
				SmartPhone.SetActive(true);
			}
			else
			{
				Persona = PersonaType.SocialButterfly;
			}
		}
		if (!Indoors && WitnessedMurder && StudentID != StudentManager.RivalID)
		{
			if ((Persona != PersonaType.Evil && Persona != PersonaType.Heroic && Persona != PersonaType.Coward && Persona != PersonaType.PhoneAddict && Persona != PersonaType.Protective && Persona != PersonaType.Violent) || Injured)
			{
				Persona = PersonaType.Loner;
			}
		}
		if (!WitnessedMurder)
		{
			if (Persona == PersonaType.Heroic)
			{
				SwitchBack = true;
				Persona = ((!(Corpse != null)) ? PersonaType.Loner : PersonaType.TeachersPet);
			}
			else if (Persona == PersonaType.Coward || Persona == PersonaType.Evil || Persona == PersonaType.Fragile)
			{
				Persona = PersonaType.Loner;
			}
		}
		if (Persona == PersonaType.Loner || Persona == PersonaType.Spiteful)
		{
			if (Club == ClubType.Delinquent)
			{
				if (Injured)
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentInjuredFlee, 1, 3f);
				}
				else if (FoundFriendCorpse)
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentFriendFlee, 1, 3f);
				}
				else if (FoundEnemyCorpse)
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentEnemyFlee, 1, 3f);
				}
				else
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentFlee, 1, 3f);
				}
			}
			else if (WitnessedMurder)
			{
				Subtitle.UpdateLabel(SubtitleType.LonerMurderReaction, 1, 3f);
			}
			else
			{
				Subtitle.UpdateLabel(SubtitleType.LonerCorpseReaction, 1, 3f);
			}
			if (Schoolwear > 0)
			{
				if (!Bloody)
				{
					Pathfinding.target = StudentManager.Exit;
					TargetDistance = 0f;
					Routine = false;
					Fleeing = true;
				}
				else
				{
					FleeWhenClean = true;
					TargetDistance = 1f;
					BatheFast = true;
				}
			}
			else
			{
				FleeWhenClean = true;
				if (!Bloody)
				{
					BathePhase = 5;
					GoChange();
					return;
				}
				CurrentDestination = StudentManager.FastBatheSpot;
				Pathfinding.target = StudentManager.FastBatheSpot;
				TargetDistance = 1f;
				BatheFast = true;
			}
		}
		else if (Persona == PersonaType.TeachersPet)
		{
			if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
			{
				if (StudentManager.BloodReporter == null)
				{
					if (Club != ClubType.Delinquent && !Police.Called && !LostTeacherTrust)
					{
						StudentManager.BloodLocation.position = BloodPool.transform.position;
						StudentManager.LowerBloodPosition();
						StudentManager.BloodReporter = this;
						ReportingBlood = true;
						DetermineBloodLocation();
					}
					else
					{
						ReportingBlood = false;
					}
				}
			}
			else if (StudentManager.Reporter == null && !Police.Called)
			{
				StudentManager.CorpseLocation.position = Corpse.AllColliders[0].transform.position;
				StudentManager.LowerCorpsePosition();
				StudentManager.Reporter = this;
				ReportingMurder = true;
				DetermineCorpseLocation();
			}
			if (StudentManager.Reporter == this)
			{
				Pathfinding.target = StudentManager.Teachers[Class].transform;
				CurrentDestination = StudentManager.Teachers[Class].transform;
				TargetDistance = 2f;
				if (WitnessedMurder)
				{
					Subtitle.UpdateLabel(SubtitleType.PetMurderReport, 1, 3f);
				}
				else if (WitnessedCorpse)
				{
					Subtitle.UpdateLabel(SubtitleType.PetCorpseReport, 1, 3f);
				}
			}
			else if (StudentManager.BloodReporter == this)
			{
				DropPlate();
				Pathfinding.target = StudentManager.Teachers[Class].transform;
				CurrentDestination = StudentManager.Teachers[Class].transform;
				TargetDistance = 2f;
				if (WitnessedLimb)
				{
					Subtitle.UpdateLabel(SubtitleType.LimbReaction, 1, 3f);
				}
				else if (WitnessedBloodyWeapon)
				{
					Subtitle.UpdateLabel(SubtitleType.BloodyWeaponReaction, 1, 3f);
				}
				else if (WitnessedBloodPool)
				{
					Subtitle.UpdateLabel(SubtitleType.BloodPoolReaction, 1, 3f);
				}
				else if (WitnessedWeapon)
				{
					Subtitle.UpdateLabel(SubtitleType.PetWeaponReport, 1, 3f);
				}
			}
			else
			{
				if (Club == ClubType.Council)
				{
					if (WitnessedCorpse)
					{
						if (StudentManager.CorpseLocation.position == Vector3.zero)
						{
							StudentManager.CorpseLocation.position = Corpse.AllColliders[0].transform.position;
							AssignCorpseGuardLocations();
						}
						if (StudentID == 86)
						{
							Pathfinding.target = StudentManager.CorpseGuardLocation[1];
						}
						else if (StudentID == 87)
						{
							Pathfinding.target = StudentManager.CorpseGuardLocation[2];
						}
						else if (StudentID == 88)
						{
							Pathfinding.target = StudentManager.CorpseGuardLocation[3];
						}
						else if (StudentID == 89)
						{
							Pathfinding.target = StudentManager.CorpseGuardLocation[4];
						}
						CurrentDestination = Pathfinding.target;
					}
					else
					{
						if (StudentManager.BloodLocation.position == Vector3.zero)
						{
							StudentManager.BloodLocation.position = BloodPool.transform.position;
							AssignBloodGuardLocations();
						}
						if (StudentManager.BloodGuardLocation[1].position == Vector3.zero)
						{
							AssignBloodGuardLocations();
						}
						if (StudentID == 86)
						{
							Pathfinding.target = StudentManager.BloodGuardLocation[1];
						}
						else if (StudentID == 87)
						{
							Pathfinding.target = StudentManager.BloodGuardLocation[2];
						}
						else if (StudentID == 88)
						{
							Pathfinding.target = StudentManager.BloodGuardLocation[3];
						}
						else if (StudentID == 89)
						{
							Pathfinding.target = StudentManager.BloodGuardLocation[4];
						}
						CurrentDestination = Pathfinding.target;
						Guarding = true;
					}
				}
				else
				{
					PetDestination = UnityEngine.Object.Instantiate(EmptyGameObject, Seat.position + Seat.forward * -0.5f, Quaternion.identity).transform;
					Pathfinding.target = PetDestination;
					CurrentDestination = PetDestination;
					Distracting = false;
					DropPlate();
				}
				if (WitnessedMurder)
				{
					Subtitle.UpdateLabel(SubtitleType.PetMurderReaction, 1, 3f);
				}
				else if (WitnessedCorpse)
				{
					Subtitle.UpdateLabel(SubtitleType.PetCorpseReaction, 1, 3f);
				}
				else if (WitnessedLimb)
				{
					Subtitle.UpdateLabel(SubtitleType.PetLimbReaction, 1, 3f);
				}
				else if (WitnessedBloodyWeapon)
				{
					Subtitle.UpdateLabel(SubtitleType.PetBloodyWeaponReaction, 1, 3f);
				}
				else if (WitnessedBloodPool)
				{
					Subtitle.UpdateLabel(SubtitleType.PetBloodReaction, 1, 3f);
				}
				else if (WitnessedWeapon)
				{
					Subtitle.UpdateLabel(SubtitleType.PetWeaponReaction, 1, 3f);
				}
				TargetDistance = 1f;
				ReportingMurder = false;
				ReportingBlood = false;
			}
			Routine = false;
			Fleeing = true;
		}
		else if (Persona == PersonaType.Heroic || Persona == PersonaType.Protective)
		{
			if (Yandere.Chased)
			{
				return;
			}
			StudentManager.PinDownCheck();
			if (!StudentManager.PinningDown)
			{
				if (Persona != PersonaType.Violent)
				{
					Subtitle.UpdateLabel(SubtitleType.HeroMurderReaction, 3, 3f);
				}
				else if (Defeats > 0)
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentResume, 3, 3f);
				}
				else
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentMurderReaction, 3, 3f);
				}
				Pathfinding.target = Yandere.transform;
				Pathfinding.speed = 7.5f;
				StudentManager.Portal.SetActive(false);
				Yandere.Pursuer = this;
				Yandere.Chased = true;
				TargetDistance = 0.5f;
				StudentManager.UpdateStudents();
				Routine = false;
				Fleeing = true;
			}
		}
		else if (Persona == PersonaType.Coward || Persona == PersonaType.Fragile)
		{
			CurrentDestination = base.transform;
			Pathfinding.target = base.transform;
			Subtitle.UpdateLabel(SubtitleType.CowardMurderReaction, 1, 5f);
			Routine = false;
			Fleeing = true;
		}
		else if (Persona == PersonaType.Evil)
		{
			CurrentDestination = base.transform;
			Pathfinding.target = base.transform;
			Subtitle.UpdateLabel(SubtitleType.EvilMurderReaction, 1, 5f);
			Routine = false;
			Fleeing = true;
		}
		else if (Persona == PersonaType.SocialButterfly)
		{
			StudentManager.HidingSpots.List[StudentID].position = StudentManager.PopulationManager.GetCrowdedLocation();
			CurrentDestination = StudentManager.HidingSpots.List[StudentID];
			Pathfinding.target = StudentManager.HidingSpots.List[StudentID];
			Subtitle.UpdateLabel(SubtitleType.SocialDeathReaction, 1, 5f);
			TargetDistance = 2f;
			ReportPhase = 1;
			Routine = false;
			Fleeing = true;
			Halt = true;
		}
		else if (Persona == PersonaType.Lovestruck)
		{
			if (!StudentManager.Students[1].WitnessedMurder)
			{
				CurrentDestination = StudentManager.Students[1].transform;
				Pathfinding.target = StudentManager.Students[1].transform;
				TargetDistance = 1f;
				ReportPhase = 1;
			}
			else
			{
				CurrentDestination = StudentManager.Exit;
				Pathfinding.target = StudentManager.Exit;
				TargetDistance = 0f;
				ReportPhase = 3;
			}
			Subtitle.UpdateLabel(SubtitleType.LovestruckDeathReaction, 1, 5f);
			Routine = false;
			Fleeing = true;
			Halt = true;
		}
		else if (Persona == PersonaType.Dangerous)
		{
			if (WitnessedMurder)
			{
				if (StudentID == 86)
				{
					Subtitle.UpdateLabel(SubtitleType.Chasing, 1, 5f);
				}
				else if (StudentID == 87)
				{
					Subtitle.UpdateLabel(SubtitleType.Chasing, 2, 5f);
				}
				else if (StudentID == 88)
				{
					Subtitle.UpdateLabel(SubtitleType.Chasing, 3, 5f);
				}
				else if (StudentID == 89)
				{
					Subtitle.UpdateLabel(SubtitleType.Chasing, 4, 5f);
				}
				Pathfinding.target = Yandere.transform;
				Pathfinding.speed = 7.5f;
				StudentManager.Portal.SetActive(false);
				Yandere.Chased = true;
				TargetDistance = 1f;
				StudentManager.UpdateStudents();
				CharacterAnimation.CrossFade(SprintAnim);
				Routine = false;
				Fleeing = true;
				Halt = true;
			}
			else
			{
				Persona = PersonaType.TeachersPet;
				PersonaReaction();
			}
		}
		else if (Persona == PersonaType.PhoneAddict)
		{
			CurrentDestination = StudentManager.Exit;
			Pathfinding.target = StudentManager.Exit;
			Countdown.gameObject.SetActive(true);
			Routine = false;
			Fleeing = true;
			if (StudentManager.ChaseCamera == null)
			{
				StudentManager.ChaseCamera = ChaseCamera;
				ChaseCamera.SetActive(true);
			}
		}
		else if (Persona == PersonaType.Violent)
		{
			if (WitnessedMurder)
			{
				if (Yandere.Chased)
				{
					return;
				}
				StudentManager.PinDownCheck();
				if (!StudentManager.PinningDown)
				{
					if (Defeats > 0)
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentResume, 3, 3f);
					}
					else
					{
						Subtitle.Speaker = this;
						Subtitle.UpdateLabel(SubtitleType.DelinquentMurderReaction, 3, 3f);
					}
					Pathfinding.target = Yandere.transform;
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					Pathfinding.speed = 7.5f;
					StudentManager.Portal.SetActive(false);
					Yandere.Chased = true;
					TargetDistance = 1f;
					StudentManager.UpdateStudents();
					Routine = false;
					Fleeing = true;
				}
			}
			else
			{
				if (FoundFriendCorpse)
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentFriendFlee, 1, 3f);
				}
				else
				{
					Subtitle.Speaker = this;
					Subtitle.UpdateLabel(SubtitleType.DelinquentFlee, 1, 3f);
				}
				Pathfinding.target = StudentManager.Exit;
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				TargetDistance = 0f;
				Routine = false;
				Fleeing = true;
			}
		}
		else
		{
			if (Persona != PersonaType.Strict)
			{
				return;
			}
			if (Yandere.Pursuer == this)
			{
			}
			if (WitnessedMurder)
			{
				if (Yandere.Pursuer == this)
				{
					Subtitle.UpdateLabel(SubtitleType.TeacherMurderReaction, 3, 3f);
					Pathfinding.target = Yandere.transform;
					Pathfinding.speed = 7.5f;
					StudentManager.Portal.SetActive(false);
					Yandere.Chased = true;
					TargetDistance = 0.5f;
					StudentManager.UpdateStudents();
					base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.1f, base.transform.position.z);
					Routine = false;
					Fleeing = true;
				}
				else if (!Yandere.Chased)
				{
					ChaseYandere();
				}
				return;
			}
			if (WitnessedCorpse)
			{
				if (ReportPhase == 0)
				{
					Subtitle.UpdateLabel(SubtitleType.TeacherCorpseReaction, 1, 3f);
				}
				Pathfinding.target = UnityEngine.Object.Instantiate(EmptyGameObject, new Vector3(Corpse.AllColliders[0].transform.position.x, base.transform.position.y, Corpse.AllColliders[0].transform.position.z), Quaternion.identity).transform;
				TargetDistance = 1f;
				ReportPhase = 2;
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.1f, base.transform.position.z);
				Routine = false;
				Fleeing = true;
				return;
			}
			if (ReportPhase == 0)
			{ // Minified by tanos [v]
				if (WitnessedBloodPool || WitnessedBloodyWeapon) Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 3, 3f);
				else if (WitnessedLimb)Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 4, 3f);
				else if (WitnessedWeapon) Subtitle.UpdateLabel(SubtitleType.TeacherCorpseInspection, 5, 3f);
			}
			TargetDistance = 1f;
			ReportPhase = 2;
			Routine = false;
			Fleeing = true;
			Halt = true;
		}
	}

	private void BeginStruggle()
	{
		SpawnAlarmDisc();

		if (Yandere.Dragging) Yandere.Ragdoll.GetComponent<RagdollScript>().StopDragging();
		if (Yandere.Armed) Yandere.EquippedWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

		Yandere.StruggleBar.Strength = Strength;
		Yandere.StruggleBar.Struggling = true;
		Yandere.StruggleBar.Student = this;
		Yandere.StruggleBar.gameObject.SetActive(true);
		CharacterAnimation.CrossFade(StruggleAnim);
		Yandere.ShoulderCamera.LastPosition = Yandere.ShoulderCamera.transform.position;
		Yandere.ShoulderCamera.Struggle = true;
		Pathfinding.canSearch = false;
		Pathfinding.canMove = false;
		Obstacle.enabled = true;
		Struggling = true;
		Alarmed = false;
		Halt = true;
		if (!Teacher)
		{
			Yandere.CharacterAnimation["f02_struggleA_00"].time = 0f;
		}
		else
		{
			Yandere.CharacterAnimation["f02_teacherStruggleA_00"].time = 0f;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if (Yandere.Aiming) Yandere.StopAiming();
		Yandere.StopLaughing();
		Yandere.TargetStudent = this;
		Yandere.Obscurance.enabled = false;
		Yandere.YandereVision = false;
		Yandere.NearSenpai = false;
		Yandere.Struggling = true;
		Yandere.CanMove = false;
		if (Yandere.DelinquentFighting) StudentManager.CombatMinigame.Stop();
		Yandere.EmptyHands();
		Yandere.MyController.enabled = false;
		Yandere.RPGCamera.enabled = false;
		MyController.radius = 0f;
		TargetDistance = 100f;
		AlarmTimer = 0f;
		SpawnAlarmDisc();
	}

	public void GetDestinations()
	{
		if (!Teacher) MyLocker = StudentManager.LockerPositions[StudentID];

		if (Slave)
		{
			ScheduleBlock[] scheduleBlocks = ScheduleBlocks;
			foreach (ScheduleBlock scheduleBlock in scheduleBlocks)
			{
				scheduleBlock.destination = "Slave";
				scheduleBlock.action = "Slave";
			}
		}
		for (ID = 1; ID < JSON.Students[StudentID].ScheduleBlocks.Length; ID++)
		{
			ScheduleBlock scheduleBlock2 = ScheduleBlocks[ID];
			if (scheduleBlock2.destination == "Locker")
			{
				Destinations[ID] = MyLocker;
			}
			else if (scheduleBlock2.destination == "Seat")
			{
				Destinations[ID] = Seat;
			}
			else if (scheduleBlock2.destination == "SocialSeat")
			{
				Destinations[ID] = StudentManager.SocialSeats[StudentID];
			}
			else if (scheduleBlock2.destination == "Podium")
			{
				Destinations[ID] = StudentManager.Podiums.List[Class];
			}
			else if (scheduleBlock2.destination == "Exit")
			{
				Destinations[ID] = StudentManager.Hangouts.List[0];
			}
			else if (scheduleBlock2.destination == "Hangout")
			{
				Destinations[ID] = StudentManager.Hangouts.List[StudentID];
			}
			else if (scheduleBlock2.destination == "LunchSpot")
			{
				Destinations[ID] = StudentManager.LunchSpots.List[StudentID];
			}
			else if (scheduleBlock2.destination == "Slave")
			{
				if (!FragileSlave)
				{
					Destinations[ID] = StudentManager.SlaveSpot;
				}
				else
				{
					Destinations[ID] = StudentManager.FragileSlaveSpot;
				}
			}
			else if (scheduleBlock2.destination == "Patrol")
			{
				Destinations[ID] = StudentManager.Patrols.List[StudentID].GetChild(0);
				if ((OriginalClub == ClubType.Gardening || OriginalClub == ClubType.Occult) && Club == ClubType.None)
				{
					Destinations[ID] = StudentManager.Hangouts.List[StudentID];
				}
			}
			else if (scheduleBlock2.destination == "Search Patrol")
			{
				Destinations[ID] = StudentManager.SearchPatrols.List[StudentID].GetChild(0);
			}
			else if (scheduleBlock2.destination == "Graffiti")
			{
				Destinations[ID] = StudentManager.GraffitiSpots[BullyID];
			}
			else if (scheduleBlock2.destination == "Bully")
			{
				Destinations[ID] = StudentManager.BullySpots[BullyID];
			}
			else if (scheduleBlock2.destination == "Mourn")
			{
				Destinations[ID] = StudentManager.MournSpot;
			}
			else if (scheduleBlock2.destination == "Clean")
			{
				Destinations[ID] = CleaningSpot.GetChild(0);
			}
			else if (scheduleBlock2.destination == "ShameSpot")
			{
				Destinations[ID] = StudentManager.ShameSpot;
			}
			else if (scheduleBlock2.destination == "Follow")
			{
				Destinations[ID] = StudentManager.Students[11].transform;
			}
			else if (scheduleBlock2.destination == "Cuddle")
			{
				if (!Male)
				{
					Destinations[ID] = StudentManager.FemaleCoupleSpot;
				}
				else
				{
					Destinations[ID] = StudentManager.MaleCoupleSpot;
				}
			}
			else if (scheduleBlock2.destination == "Peek")
			{
				if (!Male)
				{
					Destinations[ID] = StudentManager.FemaleStalkSpot;
				}
				else
				{
					Destinations[ID] = StudentManager.MaleStalkSpot;
				}
			}
			else if (scheduleBlock2.destination == "Club")
			{
				if (Club > ClubType.None)
				{
					if (Club == ClubType.Sports)
					{
						Destinations[ID] = StudentManager.Clubs.List[StudentID].GetChild(0);
					}
					else
					{
						Destinations[ID] = StudentManager.Clubs.List[StudentID];
					}
				}
				else
				{
					Destinations[ID] = StudentManager.Hangouts.List[StudentID];
				}
			}
			else if (scheduleBlock2.destination == "Sulk")
			{
				Destinations[ID] = StudentManager.SulkSpots[StudentID];
			}
			else if (scheduleBlock2.destination == "Sleuth")
			{
				Destinations[ID] = SleuthTarget;
			}
			else if (scheduleBlock2.destination == "Stalk")
			{
				Destinations[ID] = StalkTarget;
			}
			else if (scheduleBlock2.destination == "Sunbathe")
			{
				Destinations[ID] = StudentManager.FemaleStripSpot;
			}
			else if (scheduleBlock2.destination == "Shock")
			{
				Destinations[ID] = StudentManager.ShockedSpots[StudentID - 80];
			}
			else if (scheduleBlock2.destination == "Miyuki")
			{
				ClubMemberID = StudentID - 35;
				Destinations[ID] = StudentManager.MiyukiSpots[ClubMemberID].transform;
			}
			else if (scheduleBlock2.destination == "Practice")
			{
				if (Club > ClubType.None)
				{
					Destinations[ID] = StudentManager.PracticeSpots[ClubMemberID];
				}
				else
				{
					Destinations[ID] = StudentManager.Hangouts.List[StudentID];
				}
			}
			else if (scheduleBlock2.destination == "Lyrics")
			{
				Destinations[ID] = StudentManager.LyricsSpot;
			}
			else if (scheduleBlock2.destination == "Meeting")
			{
				Destinations[ID] = StudentManager.MeetingSpots[StudentID].transform;
			}
			else if (scheduleBlock2.destination == "InfirmaryBed")
			{
				if (StudentManager.SedatedStudents == 0)
				{
					Destinations[ID] = StudentManager.MaleRestSpot;
					StudentManager.SedatedStudents++;
				}
				else
				{
					Destinations[ID] = StudentManager.FemaleRestSpot;
				}
			}
			else if (scheduleBlock2.destination == "InfirmarySeat")
			{
				Destinations[ID] = StudentManager.InfirmarySeat;
			}
			if (scheduleBlock2.action == "Stand")
			{
				Actions[ID] = StudentActionType.AtLocker;
			}
			else if (scheduleBlock2.action == "Socialize")
			{
				Actions[ID] = StudentActionType.Socializing;
			}
			else if (scheduleBlock2.action == "Game")
			{
				Actions[ID] = StudentActionType.Gaming;
			}
			else if (scheduleBlock2.action == "Slave")
			{
				Actions[ID] = StudentActionType.Slave;
			}
			else if (scheduleBlock2.action == "Relax")
			{
				Actions[ID] = StudentActionType.Relax;
			}
			else if (scheduleBlock2.action == "Sit")
			{
				Actions[ID] = StudentActionType.SitAndTakeNotes;
			}
			else if (scheduleBlock2.action == "Peek")
			{
				Actions[ID] = StudentActionType.Peek;
			}
			else if (scheduleBlock2.action == "SocialSit")
			{
				Actions[ID] = StudentActionType.SitAndSocialize;
				if (Persona == PersonaType.Sleuth && Club == ClubType.None)
				{
					Actions[ID] = StudentActionType.Socializing;
				}
			}
			else if (scheduleBlock2.action == "Eat")
			{
				Actions[ID] = StudentActionType.SitAndEatBento;
			}
			else if (scheduleBlock2.action == "Shoes")
			{
				Actions[ID] = StudentActionType.ChangeShoes;
			}
			else if (scheduleBlock2.action == "Grade")
			{
				Actions[ID] = StudentActionType.GradePapers;
			}
			else if (scheduleBlock2.action == "Patrol")
			{
				Actions[ID] = StudentActionType.Patrol;
				if (OriginalClub == ClubType.Occult && Club == ClubType.None)
				{
					Actions[ID] = StudentActionType.Socializing;
				}
			}
			else if (scheduleBlock2.action == "Search Patrol")
			{
				Actions[ID] = StudentActionType.SearchPatrol;
			}
			else if (scheduleBlock2.action == "Gossip")
			{
				Actions[ID] = StudentActionType.Gossip;
			}
			else if (scheduleBlock2.action == "Graffiti")
			{
				Actions[ID] = StudentActionType.Graffiti;
			}
			else if (scheduleBlock2.action == "Bully")
			{
				Actions[ID] = StudentActionType.Bully;
			}
			else if (scheduleBlock2.action == "Read")
			{
				Actions[ID] = StudentActionType.Read;
			}
			else if (scheduleBlock2.action == "Text")
			{
				Actions[ID] = StudentActionType.Texting;
			}
			else if (scheduleBlock2.action == "Mourn")
			{
				Actions[ID] = StudentActionType.Mourn;
			}
			else if (scheduleBlock2.action == "Cuddle")
			{
				Actions[ID] = StudentActionType.Cuddle;
			}
			else if (scheduleBlock2.action == "Teach")
			{
				Actions[ID] = StudentActionType.Teaching;
			}
			else if (scheduleBlock2.action == "Wait")
			{
				Actions[ID] = StudentActionType.Wait;
			}
			else if (scheduleBlock2.action == "Clean")
			{
				Actions[ID] = StudentActionType.Clean;
			}
			else if (scheduleBlock2.action == "Shamed")
			{
				Actions[ID] = StudentActionType.Shamed;
			}
			else if (scheduleBlock2.action == "Follow")
			{
				Actions[ID] = StudentActionType.Follow;
			}
			else if (scheduleBlock2.action == "Sulk")
			{
				Actions[ID] = StudentActionType.Sulk;
			}
			else if (scheduleBlock2.action == "Sleuth")
			{
				Actions[ID] = StudentActionType.Sleuth;
			}
			else if (scheduleBlock2.action == "Stalk")
			{
				Actions[ID] = StudentActionType.Stalk;
			}
			else if (scheduleBlock2.action == "Sketch")
			{
				Actions[ID] = StudentActionType.Sketch;
			}
			else if (scheduleBlock2.action == "Sunbathe")
			{
				Actions[ID] = StudentActionType.Sunbathe;
			}
			else if (scheduleBlock2.action == "Shock")
			{
				Actions[ID] = StudentActionType.Shock;
			}
			else if (scheduleBlock2.action == "Miyuki")
			{
				Actions[ID] = StudentActionType.Miyuki;
			}
			else if (scheduleBlock2.action == "Meeting")
			{
				Actions[ID] = StudentActionType.Meeting;
			}
			else if (scheduleBlock2.action == "Lyrics")
			{
				Actions[ID] = StudentActionType.Lyrics;
			}
			else if (scheduleBlock2.action == "Practice")
			{
				Actions[ID] = StudentActionType.Practice;
			}
			else if (scheduleBlock2.action == "Sew")
			{
				Actions[ID] = StudentActionType.Sew;
			}
			else if (scheduleBlock2.action == "Club")
			{
				if (Club > ClubType.None)
				{
					Actions[ID] = StudentActionType.ClubAction;
				}
				else if (OriginalClub == ClubType.Art)
				{
					Actions[ID] = StudentActionType.Sketch;
				}
				else
				{
					Actions[ID] = StudentActionType.Socializing;
				}
			}
		}
	}

	private void UpdateOutlines()
	{
		for (ID = 0; ID < Outlines.Length; ID++)
		{
			if (Outlines[ID] != null)
			{
				Outlines[ID].color = new Color(1f, 0.5f, 0f, 1f);
				Outlines[ID].enabled = true;
			}
		}
	}

	public void PickRandomAnim()
	{
		if (Grudge)
		{
			RandomAnim = BulliedIdleAnim;
		}
		else if (Club != ClubType.Delinquent)
		{
			RandomAnim = AnimationNames[UnityEngine.Random.Range(0, AnimationNames.Length)];
		}
		else
		{
			RandomAnim = DelinquentAnims[UnityEngine.Random.Range(0, DelinquentAnims.Length)];
		}
	}

	private void PickRandomGossipAnim()
	{
		if (Grudge)
		{
			RandomAnim = BulliedIdleAnim;
			return;
		}
		RandomGossipAnim = GossipAnims[UnityEngine.Random.Range(0, GossipAnims.Length)];
		if (Actions[Phase] == StudentActionType.Gossip && DistanceToPlayer < 3f)
		{
			if (!ConversationGlobals.GetTopicDiscovered(19))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(19, true);
			}
			if (!ConversationGlobals.GetTopicLearnedByStudent(19, StudentID))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
				ConversationGlobals.SetTopicLearnedByStudent(19, StudentID, true);
			}
		}
	}

	private void PickRandomSleuthAnim()
	{
		if (!Sleuthing)
		{
			RandomSleuthAnim = SleuthAnims[UnityEngine.Random.Range(0, 3)];
		}
		else
		{
			RandomSleuthAnim = SleuthAnims[UnityEngine.Random.Range(3, 6)];
		}
	}

	private void BecomeTeacher()
	{
		base.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
		StudentManager.Teachers[Class] = this;
		if (Class != 1)
		{
			GradingPaper = StudentManager.FacultyDesks[Class];
			GradingPaper.LeftHand = LeftHand.parent;
			GradingPaper.Character = Character;
			GradingPaper.Teacher = this;
			SkirtCollider.gameObject.SetActive(false);
			LowPoly.MyMesh = LowPoly.TeacherMesh;
			PantyCollider.enabled = false;
		}
		if (Class > 1)
		{
			VisionDistance = 12f * Paranoia;
			base.name = "Teacher_" + Class;
		}
		else if (Class == 1)
		{
			VisionDistance = 12f * Paranoia;
			PatrolAnim = "f02_idle_00";
			base.name = "Nurse";
		}
		else
		{
			VisionDistance = 16f * Paranoia;
			PatrolAnim = "f02_stretch_00";
			IdleAnim = "f02_tsunIdle_00";
			base.name = "Coach";
		}
		StruggleAnim = "f02_teacherStruggleB_00";
		StruggleWonAnim = "f02_teacherStruggleWinB_00";
		StruggleLostAnim = "f02_teacherStruggleLoseB_00";
		OriginallyTeacher = true;
		Spawned = true;
		Teacher = true;
		base.gameObject.tag = "Untagged";
	}

	public void RemoveShoes()
	{
		if (!Male)
		{
			MyRenderer.materials[0].mainTexture = Cosmetic.SocksTexture;
			MyRenderer.materials[1].mainTexture = Cosmetic.SocksTexture;
		}
		else
		{
			MyRenderer.materials[Cosmetic.UniformID].mainTexture = Cosmetic.SocksTexture;
		}
	}

	public void BecomeRagdoll()
	{
		if (DrinkingFountain != null)
		{
			DrinkingFountain.Occupied = false;
		}
		if (Ragdoll.enabled)
		{
			return;
		}
		EmptyHands();
		if (Broken != null)
		{
			Broken.enabled = false;
			Broken.MyAudio.Stop();
		}
		if (Club == ClubType.Delinquent && MyWeapon != null)
		{
			MyWeapon.transform.parent = null;
			MyWeapon.MyCollider.enabled = true;
			MyWeapon.Prompt.enabled = true;
			Rigidbody component = MyWeapon.GetComponent<Rigidbody>();
			component.constraints = RigidbodyConstraints.None;
			component.isKinematic = false;
			component.useGravity = true;
			MyWeapon = null;
		}
		if (StudentManager.ChaseCamera == ChaseCamera)
		{
			StudentManager.ChaseCamera = null;
		}
		Countdown.gameObject.SetActive(false);
		ChaseCamera.SetActive(false);
		if (Club == ClubType.Council)
		{
			Police.CouncilDeath = true;
		}
		if (WillChase)
		{
			Yandere.Chasers--;
		}
		ParticleSystem.EmissionModule emission = Hearts.emission;
		if (Following)
		{
			emission.enabled = false;
			Yandere.Followers--;
			Following = false;
		}
		if (this == StudentManager.Reporter)
		{
			StudentManager.Reporter = null;
		}
		if (Pushed)
		{
			Police.SuicideStudent = base.gameObject;
			Police.SuicideScene = true;
			Ragdoll.Suicide = true;
			Police.Suicide = true;
		}
		if (!Tranquil)
		{
			StudentGlobals.SetStudentDying(StudentID, true);
			if (!Ragdoll.Burning && !Ragdoll.Disturbing)
			{
				Police.CorpseList[Police.Corpses] = Ragdoll;
				Police.Corpses++;
			}
		}
		if (!Male)
		{
			LiquidProjector.ignoreLayers = -2049;
			RightHandCollider.enabled = false;
			LeftHandCollider.enabled = false;
			PantyCollider.enabled = false;
			SkirtCollider.gameObject.SetActive(false);
		}
		CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		Ragdoll.AllColliders[10].isTrigger = false;
		NotFaceCollider.enabled = false;
		FaceCollider.enabled = false;
		MyController.enabled = false;
		emission.enabled = false;
		SpeechLines.Stop();
		if (MyRenderer.enabled)
		{
			MyRenderer.updateWhenOffscreen = true;
		}
		Pathfinding.enabled = false;
		HipCollider.enabled = true;
		base.enabled = false;
		UnWet();
		Prompt.Hide();
		Prompt.enabled = false;
		Prompt.Hide();
		Ragdoll.CharacterAnimation = CharacterAnimation;
		Ragdoll.DetectionMarker = DetectionMarker;
		Ragdoll.RightEyeOrigin = RightEyeOrigin;
		Ragdoll.LeftEyeOrigin = LeftEyeOrigin;
		Ragdoll.Electrocuted = Electrocuted;
		Ragdoll.BreastSize = BreastSize;
		Ragdoll.EyeShrink = EyeShrink;
		Ragdoll.StudentID = StudentID;
		Ragdoll.Tranquil = Tranquil;
		Ragdoll.Burning = Burning;
		Ragdoll.Drowned = Drowned;
		Ragdoll.Yandere = Yandere;
		Ragdoll.Police = Police;
		Ragdoll.Pushed = Pushed;
		Ragdoll.Male = Male;
		Police.Deaths++;
		Ragdoll.enabled = true;
		Reputation.PendingRep -= PendingRep;
		if (WitnessedMurder && Persona != PersonaType.Evil)
		{
			Police.Witnesses--;
		}
		UpdateOutlines();
		if (DetectionMarker != null)
		{
			DetectionMarker.Tex.enabled = false;
		}
		GameObjectUtils.SetLayerRecursively(base.gameObject, 11);
		base.tag = "Blood";
		LowPoly.transform.parent = Hips;
		LowPoly.transform.localPosition = new Vector3(0f, -1f, 0f);
		LowPoly.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
	}

	public void GetWet()
	{
		BeenSplashed = true;
		BatheFast = true;
		LiquidProjector.enabled = true;
		if (Gas)
		{
			LiquidProjector.material.mainTexture = GasTexture;
		}
		else if (Bloody)
		{
			LiquidProjector.material.mainTexture = BloodTexture;
		}
		else
		{
			LiquidProjector.material.mainTexture = WaterTexture;
		}
		for (ID = 0; ID < LiquidEmitters.Length; ID++)
		{
			ParticleSystem particleSystem = LiquidEmitters[ID];
			particleSystem.gameObject.SetActive(true);
			ParticleSystem.MainModule main = particleSystem.main;
			if (Gas)
			{
				main.startColor = new Color(1f, 1f, 0f, 1f);
			}
			else if (Bloody)
			{
				main.startColor = new Color(1f, 0f, 0f, 1f);
			}
			else
			{
				main.startColor = new Color(0f, 1f, 1f, 1f);
			}
		}
		if (!Slave)
		{
			CharacterAnimation[SplashedAnim].speed = 1f;
			CharacterAnimation.CrossFade(SplashedAnim);
			Subtitle.UpdateLabel(SplashSubtitleType, 0, 1f);
			SpeechLines.Stop();
			Hearts.Stop();
			StopMeeting();
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			SplashTimer = 0f;
			SplashPhase = 1;
			BathePhase = 1;
			ForgetRadio();
			if (Distracting)
			{
				DistractionTarget.TargetedForDistraction = false;
				DistractionTarget.Octodog.SetActive(false);
				DistractionTarget.Distracted = false;
				Distracting = false;
				CanTalk = true;
			}
			SchoolwearUnavailable = true;
			Distracted = false;
			Splashed = true;
			Routine = false;
			Wet = true;
			if (Following)
			{
				Yandere.Followers--;
				Following = false;
			}
			SpawnAlarmDisc();
		}
	}

	public void UnWet()
	{
		for (ID = 0; ID < LiquidEmitters.Length; ID++)
		{
			LiquidEmitters[ID].gameObject.SetActive(false);
		}
	}

	public void SetSplashes(bool Bool)
	{
		for (ID = 0; ID < SplashEmitters.Length; ID++)
		{
			SplashEmitters[ID].gameObject.SetActive(Bool);
		}
	}

	private void StopMeeting()
	{
		Prompt.Label[0].text = "     Talk";
		Pathfinding.canSearch = true;
		Pathfinding.canMove = true;
		Drownable = false;
		Pushable = false;
		Meeting = false;
		MeetTimer = 0f;
		if (StudentID == 30)
		{
			StudentManager.OfferHelp.gameObject.SetActive(false);
			StudentManager.LoveManager.RivalWaiting = false;
		}
		else if (StudentID == 5)
		{
			StudentManager.FragileOfferHelp.gameObject.SetActive(false);
		}
	}

	public void Combust()
	{
		Police.CorpseList[Police.Corpses] = Ragdoll;
		Police.Corpses++;
		GameObjectUtils.SetLayerRecursively(base.gameObject, 11);
		base.tag = "Blood";
		Dying = true;
		SpawnAlarmDisc();
		Character.GetComponent<Animation>().CrossFade(BurningAnim);
		Pathfinding.canSearch = false;
		Pathfinding.canMove = false;
		Ragdoll.BurningAnimation = true;
		Ragdoll.Disturbing = true;
		Ragdoll.Burning = true;
		WitnessedCorpse = false;
		Investigating = false;
		EatingSnack = false;
		DiscCheck = false;
		WalkBack = false;
		Alarmed = false;
		CanTalk = false;
		Fleeing = false;
		Routine = false;
		Reacted = false;
		Burning = true;
		Wet = false;
		AudioSource component = GetComponent<AudioSource>();
		component.clip = BurningClip;
		component.Play();
		LiquidProjector.enabled = false;
		UnWet();
		if (Following)
		{
			Yandere.Followers--;
			Following = false;
		}
		for (ID = 0; ID < FireEmitters.Length; ID++)
		{
			FireEmitters[ID].gameObject.SetActive(true);
		}
		if (Attacked)
		{
			BurnTarget = Yandere.transform.position + Yandere.transform.forward;
			Attacked = false;
		}
	}

	public void JojoReact()
	{
		UnityEngine.Object.Instantiate(JojoHitEffect, base.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
		if (!Dying)
		{
			Dying = true;
			SpawnAlarmDisc();
			Character.GetComponent<Animation>().CrossFade(JojoReactAnim);
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			WitnessedCorpse = false;
			Investigating = false;
			EatingSnack = false;
			DiscCheck = false;
			WalkBack = false;
			Alarmed = false;
			CanTalk = false;
			Fleeing = false;
			Routine = false;
			Reacted = false;
			Wet = false;
			AudioSource component = GetComponent<AudioSource>();
			component.Play();
			if (Following)
			{
				Yandere.Followers--;
				Following = false;
			}
		}
	}

	private void Nude()
	{
		if (!Male)
		{
			PantyCollider.enabled = false;
			SkirtCollider.gameObject.SetActive(false);
		}
		if (!Male)
		{
			MyRenderer.sharedMesh = TowelMesh;
			MyRenderer.materials[0].SetFloat("_BlendAmount", 0f);
			MyRenderer.materials[0].mainTexture = TowelTexture;
			MyRenderer.materials[1].mainTexture = TowelTexture;
			MyRenderer.materials[2].mainTexture = Cosmetic.FaceTexture;
			Cosmetic.MyRenderer.materials[1].SetFloat("_BlendAmount", 0f);
		}
		else
		{
			MyRenderer.sharedMesh = BaldNudeMesh;
			MyRenderer.materials[0].mainTexture = NudeTexture;
			MyRenderer.materials[1].mainTexture = null;
			MyRenderer.materials[2].mainTexture = Cosmetic.FaceTextures[SkinColor];
		}
		Cosmetic.RemoveCensor();
		if (!AoT && Male)
		{
			for (ID = 0; ID < CensorSteam.Length; ID++)
			{
				CensorSteam[ID].SetActive(true);
			}
		}
	}

	public void ChangeSchoolwear()
	{
		for (ID = 0; ID < CensorSteam.Length; ID++)
		{
			CensorSteam[ID].SetActive(false);
		}
		if (Attacher.gameObject.activeInHierarchy)
		{
			Attacher.RemoveAccessory();
		}
		if (LabcoatAttacher.enabled)
		{
			UnityEngine.Object.Destroy(LabcoatAttacher.newRenderer);
			LabcoatAttacher.enabled = false;
		}
		if (Schoolwear == 0)
		{
			Nude();
		}
		else if (Schoolwear == 1)
		{
			if (!Male)
			{
				Cosmetic.SetFemaleUniform();
				SkirtCollider.gameObject.SetActive(true);
				PantyCollider.enabled = true;
				if (Club == ClubType.Bully)
				{
					Cosmetic.RightWristband.SetActive(true);
					Cosmetic.LeftWristband.SetActive(true);
					Cosmetic.Bookbag.SetActive(true);
					Cosmetic.Hoodie.SetActive(true);
				}
			}
			else
			{
				Cosmetic.SetMaleUniform();
			}
		}
		else if (Schoolwear == 2)
		{
			if (Club == ClubType.Sports)
			{
				MyRenderer.sharedMesh = SwimmingTrunks;
				MyRenderer.SetBlendShapeWeight(0, 20 * (6 - ClubMemberID));
				MyRenderer.SetBlendShapeWeight(1, 20 * (6 - ClubMemberID));
				MyRenderer.materials[0].mainTexture = Cosmetic.Trunks[StudentID];
				MyRenderer.materials[1].mainTexture = Cosmetic.FaceTexture;
				MyRenderer.materials[2].mainTexture = Cosmetic.Trunks[StudentID];
			}
			else
			{
				MyRenderer.sharedMesh = SchoolSwimsuit;
				if (!Male)
				{
					if (Club == ClubType.Bully)
					{
						MyRenderer.materials[0].mainTexture = Cosmetic.GanguroSwimsuitTextures[BullyID];
						MyRenderer.materials[1].mainTexture = Cosmetic.GanguroSwimsuitTextures[BullyID];
						Cosmetic.RightWristband.SetActive(false);
						Cosmetic.LeftWristband.SetActive(false);
						Cosmetic.Bookbag.SetActive(false);
						Cosmetic.Hoodie.SetActive(false);
					}
					else
					{
						MyRenderer.materials[0].mainTexture = SwimsuitTexture;
						MyRenderer.materials[1].mainTexture = SwimsuitTexture;
					}
					MyRenderer.materials[2].mainTexture = Cosmetic.FaceTexture;
				}
				else
				{
					MyRenderer.materials[0].mainTexture = SwimsuitTexture;
					MyRenderer.materials[1].mainTexture = Cosmetic.FaceTexture;
					MyRenderer.materials[2].mainTexture = SwimsuitTexture;
				}
			}
		}
		else if (Schoolwear == 3)
		{
			MyRenderer.sharedMesh = GymUniform;
			if (!Male)
			{
				MyRenderer.materials[0].mainTexture = GymTexture;
				MyRenderer.materials[1].mainTexture = GymTexture;
				MyRenderer.materials[2].mainTexture = Cosmetic.FaceTexture;
			}
			else
			{
				MyRenderer.materials[0].mainTexture = GymTexture;
				MyRenderer.materials[1].mainTexture = Cosmetic.SkinTextures[Cosmetic.SkinColor];
				MyRenderer.materials[2].mainTexture = Cosmetic.FaceTexture;
			}
		}
		if (!Male)
		{
			Cosmetic.Stockings = ((Schoolwear != 1) ? string.Empty : Cosmetic.OriginalStockings);
			StartCoroutine(Cosmetic.PutOnStockings());
			if (StudentManager.Censor)
			{
				Cosmetic.CensorPanties();
			}
		}
		while (ID < Outlines.Length)
		{
			if (Outlines[ID] != null && Outlines[ID].h != null)
			{
				Outlines[ID].h.ReinitMaterials();
			}
			ID++;
		}
	}

	public void AttackOnTitan()
	{
		CharacterAnimation.CrossFade(WalkAnim);
		AoT = true;
		MyController.center = new Vector3(MyController.center.x, 0.0825f, MyController.center.z);
		MyController.radius = 0.015f;
		MyController.height = 0.15f;
		if (!Male)
		{
			Cosmetic.FaceTexture = TitanFaceTexture;
		}
		else
		{
			Cosmetic.FaceTextures[SkinColor] = TitanFaceTexture;
		}
		NudeTexture = TitanBodyTexture;
		Nude();
		for (ID = 0; ID < Outlines.Length; ID++)
		{
			OutlineScript outlineScript = Outlines[ID];
			if (outlineScript.h == null)
			{
				outlineScript.Awake();
			}
			outlineScript.h.ReinitMaterials();
		}
		if (!Male && !Teacher)
		{
			PantyCollider.enabled = false;
			SkirtCollider.gameObject.SetActive(false);
		}
	}

	public void Spook()
	{
		if (!Male)
		{
			RightEye.gameObject.SetActive(false);
			LeftEye.gameObject.SetActive(false);
			MyRenderer.enabled = false;
			for (ID = 0; ID < Bones.Length; ID++)
			{
				Bones[ID].SetActive(true);
			}
		}
	}

	private void Unspook()
	{
		MyRenderer.enabled = true;
		for (ID = 0; ID < Bones.Length; ID++)
		{
			Bones[ID].SetActive(false);
		}
	}

	private void GoChange()
	{
		if (!Male)
		{
			CurrentDestination = StudentManager.FemaleStripSpot;
			Pathfinding.target = StudentManager.FemaleStripSpot;
		}
		else
		{
			CurrentDestination = StudentManager.MaleStripSpot;
			Pathfinding.target = StudentManager.MaleStripSpot;
		}
		Pathfinding.canSearch = true;
		Pathfinding.canMove = true;
		Distracted = false;
	}

	public void SpawnAlarmDisc()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(AlarmDisc, base.transform.position + Vector3.up, Quaternion.identity);
		gameObject.GetComponent<AlarmDiscScript>().Male = Male;
		gameObject.GetComponent<AlarmDiscScript>().Originator = this;
		if (Splashed)
		{
			gameObject.GetComponent<AlarmDiscScript>().Shocking = true;
			gameObject.GetComponent<AlarmDiscScript>().NoScream = true;
		}
		if (Struggling || Shoving || MurderSuicidePhase == 100 || StudentManager.CombatMinigame.Delinquent == this)
		{
			gameObject.GetComponent<AlarmDiscScript>().NoScream = true;
		}
		if (Club == ClubType.Delinquent)
		{
			gameObject.GetComponent<AlarmDiscScript>().Delinquent = true;
		}
		if (Dying && Yandere.Equipped > 0 && Yandere.EquippedWeapon.WeaponID == 7)
		{
			gameObject.GetComponent<AlarmDiscScript>().Long = true;
		}
	}

	public void ChangeClubwear()
	{
		if (!ClubAttire)
		{
			Cosmetic.RemoveCensor();
			DistanceToDestination = 100f;
			ClubAttire = true;
			if (Club == ClubType.Art)
			{
				if (!Attacher.gameObject.activeInHierarchy)
				{
					Attacher.gameObject.SetActive(true);
				}
				else
				{
					Attacher.Start();
				}
			}
			else if (Club == ClubType.MartialArts)
			{
				MyRenderer.sharedMesh = JudoGiMesh;
				if (!Male)
				{
					MyRenderer.materials[0].mainTexture = JudoGiTexture;
					MyRenderer.materials[1].mainTexture = JudoGiTexture;
					MyRenderer.materials[2].mainTexture = Cosmetic.FaceTexture;
					SkirtCollider.gameObject.SetActive(false);
					PantyCollider.enabled = false;
				}
				else
				{
					MyRenderer.materials[0].mainTexture = JudoGiTexture;
					MyRenderer.materials[1].mainTexture = Cosmetic.FaceTexture;
					MyRenderer.materials[2].mainTexture = JudoGiTexture;
				}
			}
			else if (Club == ClubType.Science)
			{
				WearLabCoat();
			}
			else if (Club == ClubType.Sports)
			{
				if (Clock.Period < 3)
				{
					MyRenderer.sharedMesh = GymUniform;
					MyRenderer.materials[0].mainTexture = GymTexture;
					MyRenderer.materials[1].mainTexture = Cosmetic.SkinTextures[Cosmetic.SkinID];
					MyRenderer.materials[2].mainTexture = Cosmetic.FaceTexture;
				}
				else
				{
					MyRenderer.sharedMesh = SwimmingTrunks;
					MyRenderer.SetBlendShapeWeight(0, 20 * (6 - ClubMemberID));
					MyRenderer.SetBlendShapeWeight(1, 20 * (6 - ClubMemberID));
					MyRenderer.materials[0].mainTexture = Cosmetic.Trunks[StudentID];
					MyRenderer.materials[1].mainTexture = Cosmetic.FaceTexture;
					MyRenderer.materials[2].mainTexture = Cosmetic.Trunks[StudentID];
					ClubAnim = "poolDive_00";
					ClubActivityPhase = 15;
					Destinations[Phase] = StudentManager.Clubs.List[StudentID].GetChild(ClubActivityPhase);
				}
			}
			if (StudentID == 46)
			{
				Armband.transform.localPosition = new Vector3(Armband.transform.localPosition.x, Armband.transform.localPosition.y, 0.01f);
				Armband.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
			}
			return;
		}
		ClubAttire = false;
		if (Club == ClubType.Art)
		{
			Attacher.RemoveAccessory();
			return;
		}
		if (Club == ClubType.Science)
		{
			WearLabCoat();
			return;
		}
		ChangeSchoolwear();
		if (StudentID == 46)
		{
			Armband.transform.localPosition = new Vector3(Armband.transform.localPosition.x, Armband.transform.localPosition.y, 0.012f);
			Armband.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		}
		else if (StudentID == 47)
		{
			StudentManager.ConvoManager.Confirmed = false;
			ClubAnim = "idle_20";
		}
		else if (StudentID == 49)
		{
			StudentManager.ConvoManager.Confirmed = false;
			ClubAnim = "f02_idle_20";
		}
	}

	private void WearLabCoat()
	{
		if (!LabcoatAttacher.enabled)
		{
			MyRenderer.sharedMesh = HeadAndHands;
			LabcoatAttacher.enabled = true;
			if (!Male)
			{
				RightBreast.gameObject.name = "RightBreastRENAMED";
				LeftBreast.gameObject.name = "LeftBreastRENAMED";
			}
			if (LabcoatAttacher.Initialized)
			{
				LabcoatAttacher.AttachAccessory();
			}
			if (!Male)
			{
				MyRenderer.materials[0].SetFloat("_BlendAmount", 0f);
				MyRenderer.materials[0].mainTexture = Cosmetic.FaceTexture;
				MyRenderer.materials[1].mainTexture = NudeTexture;
				MyRenderer.materials[2].mainTexture = null;
				Cosmetic.MyRenderer.materials[1].SetFloat("_BlendAmount", 0f);
				SkirtCollider.gameObject.SetActive(false);
				PantyCollider.enabled = false;
			}
			else
			{
				MyRenderer.materials[0].mainTexture = Cosmetic.FaceTextures[SkinColor];
				MyRenderer.materials[1].mainTexture = NudeTexture;
				MyRenderer.materials[2].mainTexture = NudeTexture;
			}
		}
		else
		{
			if (!Male)
			{
				RightBreast.gameObject.name = "RightBreastRENAMED";
				LeftBreast.gameObject.name = "LeftBreastRENAMED";
				SkirtCollider.gameObject.SetActive(true);
				PantyCollider.enabled = true;
			}
			UnityEngine.Object.Destroy(LabcoatAttacher.newRenderer);
			LabcoatAttacher.enabled = false;
			ChangeSchoolwear();
		}
	}

	public void AttachRiggedAccessory()
	{
		RiggedAccessory.GetComponent<RiggedAccessoryAttacher>().ID = StudentID;
		if (Cosmetic.Accessory > 0)
		{
			Cosmetic.FemaleAccessories[Cosmetic.Accessory].SetActive(false);
		}
		if (StudentID == 26)
		{
			MyRenderer.sharedMesh = NoArmsNoTorso;
		}
		RiggedAccessory.SetActive(true);
	}

	public void CameraReact()
	{
		CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		Pathfinding.canSearch = false;
		Pathfinding.canMove = false;
		Obstacle.enabled = true;
		CameraReacting = true;
		CameraReactPhase = 1;
		SpeechLines.Stop();
		Routine = false;
		StopPairing();
		if (!Sleuthing)
		{
			SmartPhone.SetActive(false);
		}
		OccultBook.SetActive(false);
		Scrubber.SetActive(false);
		Eraser.SetActive(false);
		Pen.SetActive(false);
		Pencil.SetActive(false);
		Sketchbook.SetActive(false);
		if (Club == ClubType.Gardening)
		{
			WateringCan.transform.parent = Hips;
			WateringCan.transform.localPosition = new Vector3(0f, 0.0135f, -0.184f);
			WateringCan.transform.localEulerAngles = new Vector3(0f, 90f, 30f);
		}
		else if (Club == ClubType.LightMusic)
		{
			if (StudentID == 51)
			{
				if (InstrumentBag[ClubMemberID].transform.parent == null)
				{
					Instruments[ClubMemberID].transform.parent = null;
					Instruments[ClubMemberID].transform.position = new Vector3(-0.5f, 4.5f, 22.45666f);
					Instruments[ClubMemberID].transform.eulerAngles = new Vector3(-15f, 0f, 0f);
					Instruments[ClubMemberID].GetComponent<AudioSource>().playOnAwake = false;
					Instruments[ClubMemberID].GetComponent<AudioSource>().Stop();
				}
				else
				{
					Instruments[ClubMemberID].SetActive(false);
				}
			}
			else
			{
				Instruments[ClubMemberID].SetActive(false);
			}
			Drumsticks[0].SetActive(false);
			Drumsticks[1].SetActive(false);
		}
		GameObject[] scienceProps = ScienceProps;
		foreach (GameObject gameObject in scienceProps)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		GameObject[] fingerfood = Fingerfood;
		foreach (GameObject gameObject2 in fingerfood)
		{
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
		}
		if (!Yandere.ClubAccessories[7].activeInHierarchy || Club == ClubType.Delinquent)
		{
			CharacterAnimation.CrossFade(CameraAnims[1]);
			return;
		}
		if (Club == ClubType.Bully)
		{
			SmartPhone.SetActive(true);
		}
		CharacterAnimation.CrossFade(IdleAnim);
	}

	private void LookForYandere()
	{
		if (!Yandere.Chased && CanSeeObject(Yandere.gameObject, Yandere.HeadPosition)) ReportPhase++;
	}

	public void UpdatePerception() {
		Perception = (ClubGlobals.Club == ClubType.Occult || PlayerGlobals.StealthBonus > 0) ? 0.5f : 1f;
		ChameleonCheck();
		if (Chameleon) Perception *= 0.5f;
	}

	public void StopInvestigating()
	{
		Giggle = null;
		if (!Sleuthing)
		{
			CurrentDestination = Destinations[Phase];
			Pathfinding.target = Destinations[Phase];
			if (Actions[Phase] == StudentActionType.Sunbathe && SunbathePhase > 1)
			{
				CurrentDestination = StudentManager.SunbatheSpots[StudentID - 80];
				Pathfinding.target = StudentManager.SunbatheSpots[StudentID - 80];
			}
		}
		else
		{
			CurrentDestination = SleuthTarget;
			Pathfinding.target = SleuthTarget;
		}
		InvestigationTimer = 0f;
		InvestigationPhase = 0;
		if (!Hurry) Pathfinding.speed = 1f;
		else Pathfinding.speed = 4f;

		if (CurrentAction == StudentActionType.Clean) {
			SmartPhone.SetActive(false);
			Scrubber.SetActive(true);
			if (CleaningRole == 5) {
				Scrubber.GetComponent<Renderer>().material.mainTexture = Eraser.GetComponent<Renderer>().material.mainTexture;
				Eraser.SetActive(true);
			}
		}
		YandereInnocent = false;
		Investigating = false;
		EatingSnack = false;
		DiscCheck = false;
		Routine = true;
	}

	public void ForgetGiggle()
	{
		Giggle = null;
		InvestigationTimer = 0f;
		InvestigationPhase = 0;
		YandereInnocent = false;
		Investigating = false;
		DiscCheck = false;
	}

	private bool LovedOneIsTargeted(int yandereTargetID)
	{
		bool flag = InCouple && CoupleID == yandereTargetID;
		bool flag2 = StudentID == 3 && yandereTargetID == 2;
		bool flag3 = StudentID == 2 && yandereTargetID == 3;
		bool flag4 = StudentID == 38 && yandereTargetID == 37;
		bool flag5 = StudentID == 37 && yandereTargetID == 38;
		bool flag6 = StudentID == 30 && yandereTargetID == 25;
		bool flag7 = StudentID == 25 && yandereTargetID == 30;
		bool flag8 = StudentID == 28 && yandereTargetID == 30;
		bool flag9 = false;
		bool flag10 = StudentID > 55 && StudentID < 61 && yandereTargetID > 55 && yandereTargetID < 61;
		if (Injured)
		{
			flag9 = Club == ClubType.Delinquent && StudentManager.Students[yandereTargetID].Club == ClubType.Delinquent;
		}
		return flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7 || flag8 || flag9 || flag10;
	}

	private void Pose()
	{
		StudentManager.PoseMode.ChoosingAction = true;
		StudentManager.PoseMode.Panel.enabled = true;
		StudentManager.PoseMode.Student = this;
		StudentManager.PoseMode.UpdateLabels();
		StudentManager.PoseMode.Show = true;
		DialogueWheel.PromptBar.ClearButtons();
		DialogueWheel.PromptBar.Label[0].text = "Confirm";
		DialogueWheel.PromptBar.Label[1].text = "Back";
		DialogueWheel.PromptBar.Label[4].text = "Change";
		DialogueWheel.PromptBar.Label[5].text = "Increase/Decrease";
		DialogueWheel.PromptBar.UpdateButtons();
		DialogueWheel.PromptBar.Show = true;
		Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
		Yandere.CanMove = false;
		Posing = true;
	}

	public void DisableEffects()
	{
		LiquidProjector.enabled = false;
		ElectroSteam[0].SetActive(false);
		ElectroSteam[1].SetActive(false);
		ElectroSteam[2].SetActive(false);
		ElectroSteam[3].SetActive(false);
		CensorSteam[0].SetActive(false);
		CensorSteam[1].SetActive(false);
		CensorSteam[2].SetActive(false);
		CensorSteam[3].SetActive(false);
		ParticleSystem[] liquidEmitters = LiquidEmitters;
		foreach (ParticleSystem particleSystem in liquidEmitters)
		{
			particleSystem.gameObject.SetActive(false);
		}
		ParticleSystem[] fireEmitters = FireEmitters;
		foreach (ParticleSystem particleSystem2 in fireEmitters)
		{
			particleSystem2.gameObject.SetActive(false);
		}
		for (ID = 0; ID < Bones.Length; ID++)
		{
			if (Bones[ID] != null)
			{
				Bones[ID].SetActive(false);
			}
		}
		if (Persona != PersonaType.PhoneAddict)
		{
			SmartPhone.SetActive(false);
		}
		Note.SetActive(false);
		if (!Slave)
		{
			UnityEngine.Object.Destroy(Broken);
		}
	}

	public void DetermineSenpaiReaction()
	{
		if (Witnessed == StudentWitnessType.WeaponAndBloodAndInsanity) Subtitle.UpdateLabel(SubtitleType.SenpaiInsanityReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.WeaponAndBlood) Subtitle.UpdateLabel(SubtitleType.SenpaiWeaponReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.WeaponAndInsanity) Subtitle.UpdateLabel(SubtitleType.SenpaiInsanityReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.BloodAndInsanity) Subtitle.UpdateLabel(SubtitleType.SenpaiInsanityReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.Weapon) Subtitle.UpdateLabel(SubtitleType.SenpaiWeaponReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.Blood) Subtitle.UpdateLabel(SubtitleType.SenpaiBloodReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.Insanity) Subtitle.UpdateLabel(SubtitleType.SenpaiInsanityReaction, 1, 4.5f);
		else if (Witnessed == StudentWitnessType.Lewd) Subtitle.UpdateLabel(SubtitleType.SenpaiLewdReaction, 1, 4.5f);
		else if (GameOverCause == GameOverType.Stalking) Subtitle.UpdateLabel(SubtitleType.SenpaiStalkingReaction, Concern, 4.5f);
		else if (GameOverCause == GameOverType.Murder) Subtitle.UpdateLabel(SubtitleType.SenpaiMurderReaction, MurderReaction, 4.5f);
		else if (GameOverCause == GameOverType.Violence) Subtitle.UpdateLabel(SubtitleType.SenpaiViolenceReaction, 1, 4.5f);
	}

	public void ForgetRadio()
	{
		TurnOffRadio = false;
		RadioTimer = 0f;
		RadioPhase = 1;
		Routine = true;
		Radio = null;
	}

	public void RealizePhoneIsMissing()
	{
		ScheduleBlock scheduleBlock = ScheduleBlocks[2];
		scheduleBlock.destination = "Search Patrol";
		scheduleBlock.action = "Search Patrol";
		GetDestinations();
		ScheduleBlock scheduleBlock2 = ScheduleBlocks[4];
		scheduleBlock2.destination = "Search Patrol";
		scheduleBlock2.action = "Search Patrol";
		GetDestinations();
		ScheduleBlock scheduleBlock3 = ScheduleBlocks[7];
		scheduleBlock3.destination = "Search Patrol";
		scheduleBlock3.action = "Search Patrol";
		GetDestinations();
	}

	public void TeleportToDestination()
	{
		if (Clock.HourTime >= ScheduleBlocks[Phase].time)
		{
			Phase++;
			if (Actions[Phase] == StudentActionType.Patrol)
			{
				CurrentDestination = StudentManager.Patrols.List[StudentID].GetChild(PatrolID);
				Pathfinding.target = CurrentDestination;
			}
			else
			{
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
			}
			base.transform.position = CurrentDestination.position;
		}
	}

	public void GoCommitMurder()
	{
		StudentManager.MurderTakingPlace = true;
		if (!FragileSlave)
		{
			Yandere.EquippedWeapon.transform.parent = HipCollider.transform;
			Yandere.EquippedWeapon.transform.localPosition = Vector3.zero;
			Yandere.EquippedWeapon.transform.localScale = Vector3.zero;
			MyWeapon = Yandere.EquippedWeapon;
			MyWeapon.FingerprintID = StudentID;
			Yandere.EquippedWeapon = null;
			Yandere.Equipped = 0;
			StudentManager.UpdateStudents();
			Yandere.WeaponManager.UpdateLabels();
			Yandere.WeaponMenu.UpdateSprites();
			Yandere.WeaponWarning = false;
		}
		else
		{
			StudentManager.FragileWeapon.transform.parent = HipCollider.transform;
			StudentManager.FragileWeapon.transform.localPosition = Vector3.zero;
			StudentManager.FragileWeapon.transform.localScale = Vector3.zero;
			MyWeapon = StudentManager.FragileWeapon;
			MyWeapon.FingerprintID = StudentID;
			MyWeapon.MyCollider.enabled = false;
		}
		CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		CharacterAnimation.CrossFade("f02_brokenStandUp_00");
		if (HuntTarget != this)
		{
			DistanceToDestination = 100f;
			Broken.Hunting = true;
			TargetDistance = 1f;
			Routine = false;
			Hunting = true;
		}
		else
		{
			Broken.Done = true;
			Routine = false;
			Suicide = true;
		}
		Prompt.Hide();
		Prompt.enabled = false;
	}

	public void Shove()
	{
		if (!Yandere.Shoved && !Dying && !Yandere.Egg && !ShoeRemoval.enabled && !Yandere.Talking)
		{
			ForgetRadio();
			AudioSource component = GetComponent<AudioSource>();
			if (StudentID == 86)
			{
				Subtitle.UpdateLabel(SubtitleType.Shoving, 1, 5f);
			}
			else if (StudentID == 87)
			{
				Subtitle.UpdateLabel(SubtitleType.Shoving, 2, 5f);
			}
			else if (StudentID == 88)
			{
				Subtitle.UpdateLabel(SubtitleType.Shoving, 3, 5f);
			}
			else if (StudentID == 89)
			{
				Subtitle.UpdateLabel(SubtitleType.Shoving, 4, 5f);
			}
			if (Yandere.Aiming)
			{
				Yandere.StopAiming();
			}
			if (Yandere.Laughing)
			{
				Yandere.StopLaughing();
			}
			base.transform.rotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
			Yandere.transform.rotation = Quaternion.LookRotation(new Vector3(Hips.transform.position.x, Yandere.transform.position.y, Hips.transform.position.z) - Yandere.transform.position);
			CharacterAnimation[ShoveAnim].time = 0f;
			CharacterAnimation.CrossFade(ShoveAnim);
			FocusOnYandere = false;
			Investigating = false;
			Distracted = true;
			Alarmed = false;
			Routine = false;
			Shoving = true;
			NoTalk = false;
			Patience--;
			if (Club != ClubType.Council && Persona != PersonaType.Violent)
			{
				Patience = 999;
			}
			if (Patience < 1)
			{
				Yandere.CannotRecover = true;
			}
			if (ReturningMisplacedWeapon)
			{
				DropMisplacedWeapon();
			}
			Yandere.CharacterAnimation["f02_shoveA_01"].time = 0f;
			Yandere.CharacterAnimation.CrossFade("f02_shoveA_01");
			Yandere.YandereVision = false;
			Yandere.NearSenpai = false;
			Yandere.Degloving = false;
			Yandere.Flicking = false;
			Yandere.Punching = false;
			Yandere.CanMove = false;
			Yandere.Shoved = true;
			Yandere.EmptyHands();
			Yandere.GloveTimer = 0f;
			Yandere.h = 0f;
			Yandere.v = 0f;
			Yandere.ShoveSpeed = 2f;
			if (Distraction != null)
			{
				TargetedForDistraction = false;
				Pathfinding.speed = 1f;
				SpeechLines.Stop();
				Distraction = null;
				CanTalk = true;
			}
			if (Actions[Phase] != StudentActionType.Patrol)
			{
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = CurrentDestination;
			}
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
		}
	}

	public void PushYandereAway()
	{
		if (Yandere.Aiming)
		{
			Yandere.StopAiming();
		}
		if (Yandere.Laughing)
		{
			Yandere.StopLaughing();
		}
		Yandere.transform.rotation = Quaternion.LookRotation(new Vector3(Hips.transform.position.x, Yandere.transform.position.y, Hips.transform.position.z) - Yandere.transform.position);
		Yandere.CharacterAnimation["f02_shoveA_01"].time = 0f;
		Yandere.CharacterAnimation.CrossFade("f02_shoveA_01");
		Yandere.YandereVision = false;
		Yandere.NearSenpai = false;
		Yandere.Degloving = false;
		Yandere.Flicking = false;
		Yandere.Punching = false;
		Yandere.CanMove = false;
		Yandere.Shoved = true;
		Yandere.EmptyHands();
		Yandere.GloveTimer = 0f;
		Yandere.h = 0f;
		Yandere.v = 0f;
		Yandere.ShoveSpeed = 2f;
	}

	public void Spray()
	{
		bool flag = false;
		if (Yandere.DelinquentFighting && !NoBreakUp && !StudentManager.CombatMinigame.Delinquent.WitnessedMurder)
		{
			flag = true;
		}
		if (!flag)
		{
			if (!Yandere.Sprayed && !Dying && !Yandere.Egg && !Yandere.Dumping && !Yandere.Bathing)
			{
				if (SprayTimer > 0f)
				{
					SprayTimer = Mathf.MoveTowards(SprayTimer, 0f, Time.deltaTime);
				}
				else
				{
					AudioSource.PlayClipAtPoint(PepperSpraySFX, base.transform.position);
					if (StudentID == 86)
					{
						Subtitle.UpdateLabel(SubtitleType.Spraying, 1, 5f);
					}
					else if (StudentID == 87)
					{
						Subtitle.UpdateLabel(SubtitleType.Spraying, 2, 5f);
					}
					else if (StudentID == 88)
					{
						Subtitle.UpdateLabel(SubtitleType.Spraying, 3, 5f);
					}
					else if (StudentID == 89)
					{
						Subtitle.UpdateLabel(SubtitleType.Spraying, 4, 5f);
					}
					if (Yandere.Aiming)
					{
						Yandere.StopAiming();
					}
					if (Yandere.Laughing)
					{
						Yandere.StopLaughing();
					}
					base.transform.rotation = Quaternion.LookRotation(new Vector3(Yandere.Hips.transform.position.x, base.transform.position.y, Yandere.Hips.transform.position.z) - base.transform.position);
					Yandere.transform.rotation = Quaternion.LookRotation(new Vector3(Hips.transform.position.x, Yandere.transform.position.y, Hips.transform.position.z) - Yandere.transform.position);
					CharacterAnimation.CrossFade(SprayAnim);
					PepperSpray.SetActive(true);
					Distracted = true;
					Spraying = true;
					Alarmed = false;
					Routine = false;
					Yandere.CharacterAnimation.CrossFade("f02_sprayed_00");
					Yandere.YandereVision = false;
					Yandere.NearSenpai = false;
					Yandere.FollowHips = true;
					Yandere.Punching = false;
					Yandere.CanMove = false;
					Yandere.Sprayed = true;
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					StudentManager.YandereDying = true;
					StudentManager.StopMoving();
					Yandere.Blur.blurIterations = 1;
					Yandere.Jukebox.Volume = 0f;
					if (Yandere.DelinquentFighting)
					{
						StudentManager.CombatMinigame.Stop();
					}
				}
			}
			else if (!Yandere.Sprayed)
			{
				CharacterAnimation.CrossFade(ReadyToFightAnim);
			}
		}
		else
		{
			StudentManager.CombatMinigame.Delinquent.CharacterAnimation.Play("stopFighting_00");
			Yandere.CharacterAnimation.Play("f02_stopFighting_00");
			StudentManager.CombatMinigame.Path = 7;
			CharacterAnimation.Play(BreakUpAnim);
			BreakingUpFight = true;
			SprayTimer = 1f;
		}
		StudentManager.CombatMinigame.DisablePrompts();
		StudentManager.CombatMinigame.MyVocals.Stop();
		StudentManager.CombatMinigame.MyAudio.Stop();
		Time.timeScale = 1f;
	}

	private void DetermineCorpseLocation()
	{
		if (StudentManager.Reporter == null)
		{
			StudentManager.Reporter = this;
		}
		if (Teacher)
		{
			StudentManager.CorpseLocation.position = Corpse.AllColliders[0].transform.position;
			StudentManager.CorpseLocation.LookAt(new Vector3(base.transform.position.x, StudentManager.CorpseLocation.position.y, base.transform.position.z));
			StudentManager.CorpseLocation.Translate(StudentManager.CorpseLocation.forward);
			StudentManager.LowerCorpsePosition();
		}
		AssignCorpseGuardLocations();
	}

	private void DetermineBloodLocation()
	{
		if (StudentManager.BloodReporter == null)
		{
			StudentManager.BloodReporter = this;
		}
		if (Teacher)
		{
			StudentManager.BloodLocation.position = BloodPool.transform.position;
			StudentManager.BloodLocation.LookAt(new Vector3(base.transform.position.x, StudentManager.BloodLocation.position.y, base.transform.position.z));
			StudentManager.BloodLocation.Translate(StudentManager.BloodLocation.forward);
			StudentManager.LowerBloodPosition();
		}
	}

	private void AssignCorpseGuardLocations()
	{
		StudentManager.CorpseGuardLocation[1].position = StudentManager.CorpseLocation.position + new Vector3(0f, 0f, 1f);
		LookAway(StudentManager.CorpseGuardLocation[1], StudentManager.CorpseLocation);
		StudentManager.CorpseGuardLocation[2].position = StudentManager.CorpseLocation.position + new Vector3(1f, 0f, 0f);
		LookAway(StudentManager.CorpseGuardLocation[2], StudentManager.CorpseLocation);
		StudentManager.CorpseGuardLocation[3].position = StudentManager.CorpseLocation.position + new Vector3(0f, 0f, -1f);
		LookAway(StudentManager.CorpseGuardLocation[3], StudentManager.CorpseLocation);
		StudentManager.CorpseGuardLocation[4].position = StudentManager.CorpseLocation.position + new Vector3(-1f, 0f, 0f);
		LookAway(StudentManager.CorpseGuardLocation[4], StudentManager.CorpseLocation);
	}

	private void AssignBloodGuardLocations()
	{
		StudentManager.BloodGuardLocation[1].position = StudentManager.BloodLocation.position + new Vector3(0f, 0f, 1f);
		LookAway(StudentManager.BloodGuardLocation[1], StudentManager.BloodLocation);
		StudentManager.BloodGuardLocation[2].position = StudentManager.BloodLocation.position + new Vector3(1f, 0f, 0f);
		LookAway(StudentManager.BloodGuardLocation[2], StudentManager.BloodLocation);
		StudentManager.BloodGuardLocation[3].position = StudentManager.BloodLocation.position + new Vector3(0f, 0f, -1f);
		LookAway(StudentManager.BloodGuardLocation[3], StudentManager.BloodLocation);
		StudentManager.BloodGuardLocation[4].position = StudentManager.BloodLocation.position + new Vector3(-1f, 0f, 0f);
		LookAway(StudentManager.BloodGuardLocation[4], StudentManager.BloodLocation);
	}

	private void AssignTeacherGuardLocations()
	{
		StudentManager.TeacherGuardLocation[1].position = StudentManager.CorpseLocation.position + new Vector3(0.75f, 0f, 0.75f);
		LookAway(StudentManager.TeacherGuardLocation[1], StudentManager.CorpseLocation);
		StudentManager.TeacherGuardLocation[2].position = StudentManager.CorpseLocation.position + new Vector3(0.75f, 0f, -0.75f);
		LookAway(StudentManager.TeacherGuardLocation[2], StudentManager.CorpseLocation);
		StudentManager.TeacherGuardLocation[3].position = StudentManager.CorpseLocation.position + new Vector3(-0.75f, 0f, -0.75f);
		LookAway(StudentManager.TeacherGuardLocation[3], StudentManager.CorpseLocation);
		StudentManager.TeacherGuardLocation[4].position = StudentManager.CorpseLocation.position + new Vector3(-0.75f, 0f, 0.75f);
		LookAway(StudentManager.TeacherGuardLocation[4], StudentManager.CorpseLocation);
		StudentManager.TeacherGuardLocation[5].position = StudentManager.CorpseLocation.position + new Vector3(0f, 0f, 0.5f);
		LookAway(StudentManager.TeacherGuardLocation[5], StudentManager.CorpseLocation);
		StudentManager.TeacherGuardLocation[6].position = StudentManager.CorpseLocation.position + new Vector3(0f, 0f, -0.5f);
		LookAway(StudentManager.TeacherGuardLocation[6], StudentManager.CorpseLocation);
	}

	private void LookAway(Transform T1, Transform T2)
	{
		T1.LookAt(T2);
		float y = T1.eulerAngles.y + 180f;
		T1.eulerAngles = new Vector3(T1.eulerAngles.x, y, T1.eulerAngles.z);
	}

	public void TurnToStone()
	{
		Cosmetic.RightEyeRenderer.material.mainTexture = Yandere.Stone;
		Cosmetic.LeftEyeRenderer.material.mainTexture = Yandere.Stone;
		Cosmetic.HairRenderer.material.mainTexture = Yandere.Stone;
		if (Cosmetic.HairRenderer.materials.Length > 1)
		{
			Cosmetic.HairRenderer.materials[1].mainTexture = Yandere.Stone;
		}
		Cosmetic.RightEyeRenderer.material.color = new Color(1f, 1f, 1f, 1f);
		Cosmetic.LeftEyeRenderer.material.color = new Color(1f, 1f, 1f, 1f);
		Cosmetic.HairRenderer.material.color = new Color(1f, 1f, 1f, 1f);
		MyRenderer.materials[0].mainTexture = Yandere.Stone;
		MyRenderer.materials[1].mainTexture = Yandere.Stone;
		MyRenderer.materials[2].mainTexture = Yandere.Stone;
		if (Teacher && Cosmetic.TeacherAccessories[8].activeInHierarchy)
		{
			MyRenderer.materials[3].mainTexture = Yandere.Stone;
		}
		if (PickPocket != null)
		{
			PickPocket.enabled = false;
			PickPocket.Prompt.Hide();
			PickPocket.Prompt.enabled = false;
		}
		MyRenderer.materials[0].SetFloat("_BlendAmount", 0f);
		MyRenderer.materials[1].SetFloat("_BlendAmount", 0f);
		UnityEngine.Object.Destroy(DetectionMarker.gameObject);
		AudioSource.PlayClipAtPoint(Yandere.Petrify, base.transform.position + new Vector3(0f, 1f, 0f));
		UnityEngine.Object.Instantiate(Yandere.Pebbles, Hips.position, Quaternion.identity);
		Pathfinding.enabled = false;
		ShoeRemoval.enabled = false;
		CharacterAnimation.Stop();
		Prompt.enabled = false;
		SpeechLines.Stop();
		Prompt.Hide();
		base.enabled = false;
	}

	public void StopPairing()
	{
		if (Actions[Phase] != StudentActionType.Clean && Persona == PersonaType.PhoneAddict && !LostTeacherTrust)
		{
			WalkAnim = PhoneAnims[1];
		}
		Paired = false;
	}

	public void ChameleonCheck()
	{
		ChameleonBonus = 0f;
		Chameleon = false;
		if (Yandere != null && ((Yandere.Persona == YanderePersonaType.Scholarly && Persona == PersonaType.TeachersPet) || (Yandere.Persona == YanderePersonaType.Scholarly && Club == ClubType.Science) || (Yandere.Persona == YanderePersonaType.Scholarly && Club == ClubType.Art) || (Yandere.Persona == YanderePersonaType.Chill && Persona == PersonaType.SocialButterfly) || (Yandere.Persona == YanderePersonaType.Chill && Club == ClubType.Photography) || (Yandere.Persona == YanderePersonaType.Chill && Club == ClubType.Gaming) || (Yandere.Persona == YanderePersonaType.Confident && Persona == PersonaType.Heroic) || (Yandere.Persona == YanderePersonaType.Confident && Club == ClubType.MartialArts) || (Yandere.Persona == YanderePersonaType.Elegant && Club == ClubType.Drama) || (Yandere.Persona == YanderePersonaType.Girly && Persona == PersonaType.SocialButterfly) || (Yandere.Persona == YanderePersonaType.Girly && Club == ClubType.Cooking) || (Yandere.Persona == YanderePersonaType.Graceful && Club == ClubType.Gardening) || (Yandere.Persona == YanderePersonaType.Haughty && Club == ClubType.Bully) || (Yandere.Persona == YanderePersonaType.Lively && Persona == PersonaType.SocialButterfly) || (Yandere.Persona == YanderePersonaType.Lively && Club == ClubType.LightMusic) || (Yandere.Persona == YanderePersonaType.Lively && Club == ClubType.Sports) || (Yandere.Persona == YanderePersonaType.Shy && Persona == PersonaType.Loner) || (Yandere.Persona == YanderePersonaType.Shy && Club == ClubType.Occult) || (Yandere.Persona == YanderePersonaType.Tough && Persona == PersonaType.Spiteful) || (Yandere.Persona == YanderePersonaType.Tough && Club == ClubType.Delinquent)))
		{
			ChameleonBonus = VisionDistance * 0.5f;
			Chameleon = true;
		}
	}

	private void PhoneAddictGameOver()
	{
		if (!Yandere.Lost)
		{
			Yandere.Character.GetComponent<Animation>().CrossFade("f02_down_22");
			Yandere.ShoulderCamera.HeartbrokenCamera.SetActive(true);
			Yandere.RPGCamera.enabled = false;
			Yandere.Jukebox.GameOver();
			Yandere.enabled = false;
			Yandere.EmptyHands();
			Countdown.gameObject.SetActive(false);
			ChaseCamera.SetActive(false);
			Police.Heartbroken.Exposed = true;
			StudentManager.StopMoving();
			Fleeing = false;
		}
	}

	private void EndAlarm()
	{
		Pathfinding.canSearch = true;
		Pathfinding.canMove = true;
		if (StudentID == 1 || Teacher)
		{
			IgnoreTimer = 0.0001f;
		}
		else
		{
			IgnoreTimer = 5f;
		}
		if (Persona == PersonaType.PhoneAddict)
		{
			SmartPhone.SetActive(true);
		}
		FocusOnYandere = false;
		DiscCheck = false;
		Alarmed = false;
		Reacted = false;
		Hesitation = 0f;
		AlarmTimer = 0f;
		if (WitnessedCorpse)
		{
			PersonaReaction();
		}
		else if (WitnessedBloodPool || WitnessedLimb || WitnessedWeapon)
		{
			if (Following)
			{
				ParticleSystem.EmissionModule emission = Hearts.emission;
				emission.enabled = false;
				Yandere.Followers--;
				Following = false;
			}
			CharacterAnimation.CrossFade(WalkAnim);
			CurrentDestination = BloodPool;
			Pathfinding.target = BloodPool;
			Pathfinding.canSearch = true;
			Pathfinding.canMove = true;
			Pathfinding.speed = 1f;
			InvestigatingBloodPool = true;
			Routine = false;
		}
		else if (!Following && !Wet && !Investigating)
		{
			Routine = true;
		}
		if (ResumeDistracting)
		{
			CharacterAnimation.CrossFade(WalkAnim);
			Distracting = true;
			Routine = false;
		}
		if (CurrentAction == StudentActionType.Clean)
		{
			SmartPhone.SetActive(false);
			Scrubber.SetActive(true);
			if (CleaningRole == 5)
			{
				Scrubber.GetComponent<Renderer>().material.mainTexture = Eraser.GetComponent<Renderer>().material.mainTexture;
				Eraser.SetActive(true);
			}
		}
	}

	public void GetSleuthTarget()
	{
		TargetDistance = 2f;
		SleuthID++;
		if (SleuthID < 98)
		{
			if (StudentManager.Students[SleuthID] == null)
			{
				GetSleuthTarget();
				return;
			}
			if (!StudentManager.Students[SleuthID].gameObject.activeInHierarchy)
			{
				GetSleuthTarget();
				return;
			}
			SleuthTarget = StudentManager.Students[SleuthID].transform;
			Pathfinding.target = SleuthTarget;
			CurrentDestination = SleuthTarget;
		}
		else if (SleuthID == 98)
		{
			if (ClubGlobals.Club == ClubType.Photography)
			{
				SleuthID = 0;
				GetSleuthTarget();
			}
			else
			{
				SleuthTarget = Yandere.transform;
				Pathfinding.target = SleuthTarget;
				CurrentDestination = SleuthTarget;
			}
		}
		else
		{
			SleuthID = 0;
			GetSleuthTarget();
		}
	}

	public void GetFoodTarget()
	{
		Attempts++;
		if (Attempts >= 100)
		{
			Phase++;
			return;
		}
		SleuthID++;
		if (SleuthID < 90)
		{
			if (SleuthID == StudentID)
			{
				GetFoodTarget();
				return;
			}
			if (StudentManager.Students[SleuthID] == null)
			{
				GetFoodTarget();
				return;
			}
			if (!StudentManager.Students[SleuthID].gameObject.activeInHierarchy)
			{
				GetFoodTarget();
				return;
			}
			if (StudentManager.Students[SleuthID].CurrentAction == StudentActionType.SitAndEatBento || StudentManager.Students[SleuthID].Club == ClubType.Cooking || StudentManager.Students[SleuthID].Club == ClubType.Delinquent || StudentManager.Students[SleuthID].Club == ClubType.Sports || StudentManager.Students[SleuthID].TargetedForDistraction || StudentManager.Students[SleuthID].ClubActivityPhase >= 16 || StudentManager.Students[SleuthID].InEvent || !StudentManager.Students[SleuthID].Routine || StudentManager.Students[SleuthID].Posing || StudentManager.Students[SleuthID].Slave || StudentManager.Students[SleuthID].Wet || (StudentManager.Students[SleuthID].Club == ClubType.LightMusic && StudentManager.PracticeMusic.isPlaying))
			{
				GetFoodTarget();
				return;
			}
			CharacterAnimation.CrossFade(WalkAnim);
			DistractionTarget = StudentManager.Students[SleuthID];
			DistractionTarget.TargetedForDistraction = true;
			SleuthTarget = StudentManager.Students[SleuthID].transform;
			Pathfinding.target = SleuthTarget;
			CurrentDestination = SleuthTarget;
			TargetDistance = 0.75f;
			DistractTimer = 8f;
			Distracting = true;
			CanTalk = false;
			Routine = false;
			Attempts = 0;
		}
		else
		{
			SleuthID = 0;
			GetFoodTarget();
		}
	}

	private void PhoneAddictCameraUpdate()
	{
		if (!(SmartPhone.transform.parent != null))
		{
			return;
		}
		CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		SmartPhone.transform.localPosition = new Vector3(0f, 0.005f, -0.01f);
		SmartPhone.transform.localEulerAngles = new Vector3(7.33333f, -154f, 173.66666f);
		SmartPhone.SetActive(true);
		if (Sleuthing)
		{
			if (AlarmTimer < 2f)
			{
				AlarmTimer = 2f;
				ScaredAnim = SleuthReactAnim;
				SprintAnim = SleuthReportAnim;
				CharacterAnimation.CrossFade(ScaredAnim);
			}
			if (!CameraFlash.activeInHierarchy && CharacterAnimation[ScaredAnim].time > 2f)
			{
				CameraFlash.SetActive(true);
				if (Yandere.Mask != null)
				{
					Countdown.MaskedPhoto = true;
				}
			}
			return;
		}
		ScaredAnim = PhoneAnims[4];
		CharacterAnimation.CrossFade(ScaredAnim);
		if (!CameraFlash.activeInHierarchy && (double)CharacterAnimation[ScaredAnim].time > 3.66666)
		{
			CameraFlash.SetActive(true);
			if (Yandere.Mask != null)
			{
				Countdown.MaskedPhoto = true;
			}
			else if (Grudge)
			{
				Police.PhotoEvidence++;
				PhotoEvidence = true;
			}
		}
	}

	private void ReturnToRoutine()
	{
		if (Actions[Phase] == StudentActionType.Patrol)
		{
			CurrentDestination = StudentManager.Patrols.List[StudentID].GetChild(PatrolID);
			Pathfinding.target = CurrentDestination;
		}
		else
		{
			CurrentDestination = Destinations[Phase];
			Pathfinding.target = Destinations[Phase];
		}
		BreakingUpFight = false;
		WitnessedMurder = false;
		Pathfinding.speed = 1f;
		Prompt.enabled = true;
		Alarmed = false;
		Fleeing = false;
		Routine = true;
		Grudge = false;
	}

	public void EmptyHands()
	{
		bool flag = false;
		if ((SentHome && SmartPhone.activeInHierarchy) || PhotoEvidence || (Persona == PersonaType.PhoneAddict && !Dying))
		{
			flag = true;
		}
		if ((WitnessedMurder || WitnessedCorpse) && MyPlate != null)
		{
			MyPlate.gameObject.SetActive(false);
		}
		if (Club == ClubType.Gardening)
		{
			WateringCan.transform.parent = Hips;
			WateringCan.transform.localPosition = new Vector3(0f, 0.0135f, -0.184f);
			WateringCan.transform.localEulerAngles = new Vector3(0f, 90f, 30f);
		}
		if (Club == ClubType.LightMusic)
		{
			if (StudentID == 51)
			{
				if (InstrumentBag[ClubMemberID].transform.parent == null)
				{
					Instruments[ClubMemberID].transform.parent = null;
					Instruments[ClubMemberID].transform.position = new Vector3(-0.5f, 4.5f, 22.45666f);
					Instruments[ClubMemberID].transform.eulerAngles = new Vector3(-15f, 0f, 0f);
					Instruments[ClubMemberID].GetComponent<AudioSource>().playOnAwake = false;
					Instruments[ClubMemberID].GetComponent<AudioSource>().Stop();
				}
				else
				{
					Instruments[ClubMemberID].SetActive(false);
				}
			}
			else
			{
				Instruments[ClubMemberID].SetActive(false);
			}
			Drumsticks[0].SetActive(false);
			Drumsticks[1].SetActive(false);
			AirGuitar.Stop();
		}
		if (!Male)
		{
			Handkerchief.SetActive(false);
			Cigarette.SetActive(false);
			Lighter.SetActive(false);
		}
		else
		{
			PinkSeifuku.SetActive(false);
		}
		if (!flag) SmartPhone.SetActive(false);
		if (BagOfChips != null) BagOfChips.SetActive(false);

		GameObject[] itemsToDeactivate = {
			Chopsticks[0], Chopsticks[1], Sketchbook, OccultBook, Paintbrush, EventBook,
			Scrubber, Octodog, Palette, Eraser, Pencil, Bento, Pen
		};

		foreach (GameObject item in itemsToDeactivate)
		{
			item.SetActive(false);
		}
		GameObject[] scienceProps = ScienceProps;
		foreach (GameObject gameObject in scienceProps)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		GameObject[] fingerfood = Fingerfood;
		foreach (GameObject gameObject2 in fingerfood)
		{
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
		}
	}

	public void UpdateAnimLayers()
	{
		CharacterAnimation[LeanAnim].speed += (float)StudentID * 0.01f;
		CharacterAnimation[ConfusedSitAnim].speed += (float)StudentID * 0.01f;
		CharacterAnimation[WalkAnim].time = UnityEngine.Random.Range(0f, CharacterAnimation[WalkAnim].length);
		CharacterAnimation[WetAnim].layer = 9;
		CharacterAnimation.Play(WetAnim);
		CharacterAnimation[WetAnim].weight = 0f;
		if (!Male)
		{
			CharacterAnimation[StripAnim].speed = 1.5f;
			CharacterAnimation[GameAnim].speed = 2f;
			CharacterAnimation["f02_wetIdle_00"].speed = 1.25f;
			CharacterAnimation["f02_sleuthScan_00"].speed = 1.4f;

			string[] femaleAnims = { "f02_topHalfTexting_00", CarryAnim, SocialSitAnim, ShyAnim, FistAnim, BentoAnim, AngryFaceAnim };
			int[] layers = { 8, 7, 6, 5, 4, 3, 2 };

			for (int i = 0; i < femaleAnims.Length; i++)
			{
				CharacterAnimation[femaleAnims[i]].layer = layers[i];
				CharacterAnimation.Play(femaleAnims[i]);
				CharacterAnimation[femaleAnims[i]].weight = 0f;
			}
		}
		else
		{
			CharacterAnimation[ConfusedSitAnim].speed *= -1f;
			CharacterAnimation["sleuthScan_00"].speed = 1.4f;

			string[] maleAnims = { ToughFaceAnim, SocialSitAnim, CarryShoulderAnim, "scaredFace_00", SadFaceAnim, AngryFaceAnim };
			int[] layers = { 7, 6, 5, 4, 3, 2 };

			for (int i = 0; i < maleAnims.Length; i++)
			{
				CharacterAnimation[maleAnims[i]].layer = layers[i];
				CharacterAnimation.Play(maleAnims[i]);
				CharacterAnimation[maleAnims[i]].weight = 0f;
			}
		}
		if (Persona == PersonaType.Sleuth) CharacterAnimation[WalkAnim].time = UnityEngine.Random.Range(0f, CharacterAnimation[WalkAnim].length);
		if (Club == ClubType.Bully)
		{
			if (!StudentGlobals.GetStudentBroken(StudentID) && BullyID > 1) CharacterAnimation["f02_bullyLaugh_00"].speed = 0.9f + (float)BullyID * 0.1f;
		}
		else if (Club == ClubType.Delinquent)
		{
			CharacterAnimation[WalkAnim].time = UnityEngine.Random.Range(0f, CharacterAnimation[WalkAnim].length);
			CharacterAnimation[LeanAnim].speed = 0.5f;
		}
		else if (Club == ClubType.Council)
		{
			CharacterAnimation["f02_faceCouncil" + Suffix + "_00"].layer = 10;
			CharacterAnimation.Play("f02_faceCouncil" + Suffix + "_00");
		}
		else if (Club == ClubType.Gaming)
		{
			CharacterAnimation[VictoryAnim].speed -= 0.1f * (float)(StudentID - 36);
			CharacterAnimation[VictoryAnim].speed = 0.866666f;
		}
		else if (Club == ClubType.Cooking && ClubActivityPhase > 1)
		{
			WalkAnim = PlateWalkAnim;
		}
		if (StudentID == 36)
		{
			CharacterAnimation[ToughFaceAnim].weight = 1f;
		}
		else if (StudentID == 66)
		{
			CharacterAnimation[ToughFaceAnim].weight = 1f;
		}
	}

	private void SpawnDetectionMarker()
	{
		DetectionMarker = UnityEngine.Object.Instantiate(Marker, GameObject.Find("DetectionPanel").transform.position, Quaternion.identity).GetComponent<DetectionMarkerScript>();
		DetectionMarker.transform.parent = GameObject.Find("DetectionPanel").transform;
		DetectionMarker.Target = base.transform;
	}

	public void EquipCleaningItems()
	{
		if (CurrentAction == StudentActionType.Clean)
		{
			if (Persona == PersonaType.PhoneAddict || Persona == PersonaType.Sleuth) WalkAnim = OriginalWalkAnim;
			SmartPhone.SetActive(false);
			Scrubber.SetActive(true);
			if (CleaningRole == 5)
			{
				Scrubber.GetComponent<Renderer>().material.mainTexture = Eraser.GetComponent<Renderer>().material.mainTexture;
				Eraser.SetActive(true);
			}
		}
	}

	public void DetermineWhatWasWitnessed()
	{
		if (Witnessed == StudentWitnessType.Murder) Concern = 5;
		else if (YandereVisible)
		{
			bool flag = false;
			if (Yandere.Bloodiness > 0f && !Yandere.Paint)
			{
				flag = true;
			}
			bool flag2 = Yandere.Armed && Yandere.EquippedWeapon.Suspicious;
			bool flag3 = Yandere.PickUp != null && Yandere.PickUp.Suspicious;
			bool flag4 = Yandere.PickUp != null && Yandere.PickUp.BodyPart != null;
			bool flag5 = Yandere.PickUp != null && Yandere.PickUp.Clothing && Yandere.PickUp.Evidence;
			int concern = Concern;
			if (flag2) WeaponToTakeAway = Yandere.EquippedWeapon;
			if (Yandere.Rummaging || Yandere.TheftTimer > 0f)
			{
				Witnessed = StudentWitnessType.Theft;
				Concern = 5;
			}
			else if (Yandere.Pickpocketing || Yandere.Caught)
			{
				Yandere.Pickpocketing = false;
				Witnessed = StudentWitnessType.Pickpocketing;
				Concern = 5;
				if (Teacher) Witnessed = StudentWitnessType.Theft;
			}
			else if (flag2 && flag && Yandere.Sanity < 33.333f)
			{
				Witnessed = StudentWitnessType.WeaponAndBloodAndInsanity;
				RepLoss = 30f;
				Concern = 5;
			}
			else if (flag2 && Yandere.Sanity < 33.333f)
			{
				Witnessed = StudentWitnessType.WeaponAndInsanity;
				RepLoss = 20f;
				Concern = 5;
			}
			else if (flag && Yandere.Sanity < 33.333f)
			{
				Witnessed = StudentWitnessType.BloodAndInsanity;
				RepLoss = 20f;
				Concern = 5;
			}
			else if (flag2 && flag)
			{
				Witnessed = StudentWitnessType.WeaponAndBlood;
				RepLoss = 20f;
				Concern = 5;
			}
			else if (flag2)
			{
				WeaponWitnessed = Yandere.EquippedWeapon.WeaponID;
				Witnessed = StudentWitnessType.Weapon;
				RepLoss = 10f;
				Concern = 5;
			}
			else if (flag3)
			{
				if (Yandere.PickUp.CleaningProduct)
				{
					Witnessed = StudentWitnessType.CleaningItem;
				}
				else if (Teacher)
				{
					Witnessed = StudentWitnessType.Suspicious;
				}
				else
				{
					Witnessed = StudentWitnessType.Weapon;
				}
				RepLoss = 10f;
				Concern = 5;
			}
			else if (flag)
			{
				Witnessed = StudentWitnessType.Blood;
				if (!Bloody)
				{
					RepLoss = 10f;
					Concern = 5;
				}
				else
				{
					RepLoss = 0f;
					Concern = 0;
				}
			}
			else if (Yandere.Sanity < 33.333f)
			{
				Witnessed = StudentWitnessType.Insanity;
				RepLoss = 10f;
				Concern = 5;
			}
			else if ((Yandere.Laughing && Yandere.LaughIntensity > 15f) || Yandere.Stance.Current == StanceType.Crouching || Yandere.Stance.Current == StanceType.Crawling)
			{
				Witnessed = StudentWitnessType.Insanity;
				RepLoss = 10f;
				Concern = 5;
			}
			else if (Yandere.Lewd)
			{
				Witnessed = StudentWitnessType.Lewd;
				RepLoss = 10f;
				Concern = 5;
			}
			else if (Yandere.Poisoning)
			{
				Witnessed = StudentWitnessType.Poisoning;
				RepLoss = 10f;
				Concern = 5;
			}
			else if (Yandere.Trespassing && StudentID > 1)
			{
				Witnessed = ((!Private) ? StudentWitnessType.Trespassing : StudentWitnessType.Interruption);
				Witness = false;
				Concern++;
			}
			else if (Yandere.NearSenpai)
			{
				Witnessed = StudentWitnessType.Stalking;
				Concern++;
			}
			else if (Yandere.Eavesdropping)
			{
				if (StudentID == 1)
				{
					Witnessed = StudentWitnessType.Stalking;
					Concern++;
				}
				else
				{
					if (InEvent) EventInterrupted = true;
					Witnessed = StudentWitnessType.Eavesdropping;
					RepLoss = 10f;
					Concern = 5;
				}
			}
			else if (Yandere.Aiming)
			{
				Witnessed = StudentWitnessType.Stalking;
				Concern++;
			}
			else if (Yandere.DelinquentFighting)
			{
				Witnessed = StudentWitnessType.Violence;
				RepLoss = 10f;
				Concern = 5;
			}
			else if (Yandere.PickUp != null && Yandere.PickUp.Clothing && Yandere.PickUp.Evidence)
			{
				Witnessed = StudentWitnessType.HoldingBloodyClothing;
				RepLoss = 10f;
				Concern = 5;
			}
			else if (flag4 || flag5)
			{
				Witnessed = StudentWitnessType.CoverUp;
			}
			if (StudentID == 1 && Witnessed == StudentWitnessType.Insanity && (Yandere.Stance.Current == StanceType.Crouching || Yandere.Stance.Current == StanceType.Crawling))
			{
				Witnessed = StudentWitnessType.Stalking;
				Concern = concern;
				Concern++;
			}
		}
		else
		{
			if (WitnessedLimb) Witnessed = StudentWitnessType.SeveredLimb;
			else if (WitnessedBloodyWeapon) Witnessed = StudentWitnessType.BloodyWeapon;
			else if (WitnessedBloodPool) Witnessed = StudentWitnessType.BloodPool;
			else if (WitnessedWeapon) Witnessed = StudentWitnessType.DroppedWeapon;
			else if (WitnessedCorpse) Witnessed = StudentWitnessType.Corpse;
			else {
				Witnessed = StudentWitnessType.None;
				DiscCheck = true;
				Witness = false;
			}
		}
		if (Concern == 5 && Club == ClubType.Council) Teacher = true;
	}

	public void DetermineTeacherSubtitle()
	{
		if (Witnessed == StudentWitnessType.Murder)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherMurderReaction, WitnessedMindBrokenMurder ? 4 : 1, 6f);
			GameOverCause = GameOverType.Murder;
			WitnessedMurder = true;
		}
		else if (Witnessed == StudentWitnessType.WeaponAndBloodAndInsanity || Witnessed == StudentWitnessType.WeaponAndInsanity || Witnessed == StudentWitnessType.BloodAndInsanity || Witnessed == StudentWitnessType.Insanity || Witnessed == StudentWitnessType.Poisoning)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherInsanityHostile, 1, 6f);
			GameOverCause = GameOverType.Insanity;
			WitnessedMurder = true;
		}
		else if (Witnessed == StudentWitnessType.WeaponAndBlood || Witnessed == StudentWitnessType.Weapon)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherWeaponHostile, 1, 6f);
			GameOverCause = GameOverType.Weapon;
			WitnessedMurder = true;
		}
		else if (Witnessed == StudentWitnessType.Blood)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherBloodHostile, 1, 6f);
			GameOverCause = GameOverType.Blood;
			WitnessedMurder = true;
		}
		else if (Witnessed == StudentWitnessType.Lewd)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherLewdReaction, 1, 6f);
			GameOverCause = GameOverType.Lewd;
		}
		else if (Witnessed == StudentWitnessType.Trespassing)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherTrespassingReaction, Concern, 5f);
		}
		else if (Witnessed == StudentWitnessType.Corpse)
		{
			DetermineCorpseLocation();
			Subtitle.UpdateLabel(SubtitleType.TeacherCorpseReaction, 1, 3f);
			Police.Called = true;
		}
		else if (Witnessed == StudentWitnessType.CoverUp)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherCoverUpHostile, 1, 6f);
			GameOverCause = GameOverType.Blood;
			WitnessedMurder = true;
		}
		else if (Witnessed == StudentWitnessType.CleaningItem)
		{
			Subtitle.UpdateLabel(SubtitleType.TeacherInsanityReaction, 1, 6f);
			GameOverCause = GameOverType.Insanity;
		}
	}
}
