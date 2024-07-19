using UnityEngine;

public class PortalScript : MonoBehaviour
{
	public RivalMorningEventManagerScript[] MorningEvents;

	public OsanaMorningFriendEventScript[] FriendEvents;

	public OsanaMondayBeforeClassEventScript OsanaEvent;

	public DelinquentManagerScript DelinquentManager;

	public StudentManagerScript StudentManager;

	public LoveManagerScript LoveManager;

	public ReputationScript Reputation;

	public PromptBarScript PromptBar;

	public YandereScript Yandere;

	public PoliceScript Police;

	public PromptScript Prompt;

	public ClassScript Class;

	public ClockScript Clock;

	public GameObject HeartbeatCamera;

	public GameObject Headmaster;

	public UISprite ClassDarkness;

	public Texture HomeMapMarker;

	public Renderer MapMarker;

	public Transform Teacher;

	public bool CanAttendClass;

	public bool LateReport1;

	public bool LateReport2;

	public bool Transition;

	public bool FadeOut;

	public bool Proceed;

	public float Timer;

	public int Late;

	private void Start()
	{
		ClassDarkness.enabled = false;
	}

	private void Update()
	{
		if (Clock.HourTime > 8.52f && Clock.HourTime < 8.53f && !Yandere.InClass && !LateReport1)
		{
			LateReport1 = true;
			Yandere.NotificationManager.DisplayNotification(NotificationType.Late);
		}
		if (Clock.HourTime > 13.52f && Clock.HourTime < 13.53f && !Yandere.InClass && !LateReport2)
		{
			LateReport2 = true;
			Yandere.NotificationManager.DisplayNotification(NotificationType.Late);
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			CheckForLateness();
			Reputation.UpdateRep();
			if (Police.PoisonScene || (Police.SuicideScene && Police.Corpses - Police.HiddenCorpses > 0) || Police.Corpses - Police.HiddenCorpses > 0 || Reputation.Reputation <= -100f)
			{
				EndDay();
			}
			else if (Clock.HourTime < 15.5f)
			{
				if (!Police.Show)
				{
					if (Late == 0)
					{
						ClassDarkness.enabled = true;
						Transition = true;
						FadeOut = true;
					}
					else
					{
						Yandere.Subtitle.UpdateLabel(SubtitleType.TeacherLateReaction, Late, 5.5f);
						Yandere.RPGCamera.enabled = false;
						Yandere.ShoulderCamera.Scolding = true;
						Yandere.ShoulderCamera.Teacher = Teacher;
					}
					Clock.StopTime = true;
				}
				else
				{
					EndDay();
				}
			}
			else
			{
				EndDay();
			}
			Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
			Yandere.YandereVision = false;
			Yandere.CanMove = false;
			if (Clock.HourTime < 15.5f)
			{
				Yandere.InClass = true;
				EndEvents();
			}
		}
		if (Transition)
		{
			if (FadeOut)
			{
				if (LoveManager.HoldingHands)
				{
					LoveManager.Rival.transform.position = new Vector3(0f, 0f, -50f);
				}
				ClassDarkness.color = new Color(ClassDarkness.color.r, ClassDarkness.color.g, ClassDarkness.color.b, ClassDarkness.color.a + Time.deltaTime);
				if (ClassDarkness.color.a >= 1f)
				{
					if (Yandere.Resting)
					{
						Yandere.IdleAnim = "f02_idleShort_00";
						Yandere.WalkAnim = "f02_newWalk_00";
						Yandere.OriginalIdleAnim = Yandere.IdleAnim;
						Yandere.OriginalWalkAnim = Yandere.WalkAnim;
						Yandere.CharacterAnimation.CrossFade(Yandere.IdleAnim);
						Yandere.MyRenderer.materials[2].SetFloat("_BlendAmount1", 0f);
						Yandere.Resting = false;
						Yandere.Health = 10;
						FadeOut = false;
						Proceed = true;
					}
					else
					{
						if (Yandere.Armed)
						{
							Yandere.Unequip();
						}
						HeartbeatCamera.SetActive(false);
						ClassDarkness.color = new Color(ClassDarkness.color.r, ClassDarkness.color.g, ClassDarkness.color.b, 1f);
						FadeOut = false;
						Proceed = false;
						Yandere.RPGCamera.enabled = false;
						PromptBar.Label[4].text = "Choose";
						PromptBar.Label[5].text = "Allocate";
						PromptBar.UpdateButtons();
						PromptBar.Show = true;
						Class.StudyPoints = ((PlayerGlobals.PantiesEquipped != 11) ? 5 : 10);
						Class.StudyPoints -= Late;
						Class.UpdateLabel();
						Class.gameObject.SetActive(true);
						Class.Show = true;
						if (Police.Show)
						{
							Police.Timer = 1E-06f;
						}
					}
				}
			}
			else if (Proceed)
			{
				if (ClassDarkness.color.a >= 1f)
				{
					HeartbeatCamera.SetActive(true);
					Clock.enabled = true;
					Yandere.FixCamera();
					Yandere.RPGCamera.enabled = false;
					if (Clock.HourTime < 13f)
					{
						if (StudentManager.Bully)
						{
							StudentManager.UpdateGrafitti();
						}
						Yandere.Incinerator.Timer -= 780f - Clock.PresentTime;
						DelinquentManager.CheckTime();
						Clock.PresentTime = 780f;
					}
					else
					{
						Yandere.Incinerator.Timer -= 930f - Clock.PresentTime;
						DelinquentManager.CheckTime();
						Clock.PresentTime = 930f;
					}
					Clock.HourTime = Clock.PresentTime / 60f;
					StudentManager.AttendClass();
				}
				ClassDarkness.color = new Color(ClassDarkness.color.r, ClassDarkness.color.g, ClassDarkness.color.b, ClassDarkness.color.a - Time.deltaTime);
				if (ClassDarkness.color.a <= 0f)
				{
					ClassDarkness.enabled = false;
					ClassDarkness.color = new Color(ClassDarkness.color.r, ClassDarkness.color.g, ClassDarkness.color.b, 0f);
					Clock.StopTime = false;
					Transition = false;
					Proceed = false;
					Yandere.RPGCamera.enabled = true;
					Yandere.InClass = false;
					Yandere.CanMove = true;
					StudentManager.ResumeMovement();
					if (!MissionModeGlobals.MissionMode)
					{
						if (Headmaster.activeInHierarchy)
						{
							Headmaster.SetActive(false);
						}
						else
						{
							Headmaster.SetActive(true);
						}
					}
				}
			}
		}
		if (Clock.HourTime > 15.5f)
		{
			if (base.transform.position.z < 0f)
			{
				StudentManager.RemovePapersFromDesks();
				if (!MissionModeGlobals.MissionMode)
				{
					MapMarker.material.mainTexture = HomeMapMarker;
					base.transform.position = new Vector3(0f, 1f, -75f);
					Prompt.Label[0].text = "     Go Home";
					Prompt.enabled = true;
				}
				else
				{
					base.transform.position = new Vector3(0f, -10f, 0f);
					Prompt.Hide();
					Prompt.enabled = false;
				}
			}
		}
		else
		{
			if (Yandere.Police.FadeOut || !(Vector3.Distance(Yandere.transform.position, base.transform.position) < 1.4f))
			{
				return;
			}
			CanAttendClass = true;
			CheckForProblems();
			if (!CanAttendClass)
			{
				if (Timer == 0f)
				{
					if (Yandere.Armed)
					{
						Yandere.NotificationManager.CustomText = "Carrying Weapon";
					}
					else if (Yandere.Bloodiness > 0f)
					{
						Yandere.NotificationManager.CustomText = "Bloody";
					}
					else if (Yandere.Sanity < 33.333f)
					{
						Yandere.NotificationManager.CustomText = "Visibly Insane";
					}
					else if (Yandere.Attacking)
					{
						Yandere.NotificationManager.CustomText = "In Combat";
					}
					else if (Yandere.Dragging || Yandere.Carrying)
					{
						Yandere.NotificationManager.CustomText = "Holding Corpse";
					}
					else if (Yandere.PickUp != null)
					{
						Yandere.NotificationManager.CustomText = "Carrying Item";
					}
					else if (Yandere.Chased || Yandere.Chasers > 0)
					{
						Yandere.NotificationManager.CustomText = "Chased";
					}
					else if ((bool)StudentManager.Reporter)
					{
						Yandere.NotificationManager.CustomText = "Murder being reported";
					}
					else if (StudentManager.MurderTakingPlace)
					{
						Yandere.NotificationManager.CustomText = "Murder taking place";
					}
					Yandere.NotificationManager.DisplayNotification(NotificationType.Custom);
					Yandere.NotificationManager.CustomText = "Cannot attend class. Reason:";
					Yandere.NotificationManager.DisplayNotification(NotificationType.Custom);
				}
				Prompt.Hide();
				Prompt.enabled = false;
				Timer += Time.deltaTime;
				if (Timer > 5f)
				{
					Timer = 0f;
				}
			}
			else
			{
				Prompt.enabled = true;
			}
		}
	}

	public void CheckForProblems()
	{
		if (Yandere.Armed || Yandere.Bloodiness > 0f || Yandere.Sanity < 33.333f || Yandere.Attacking || Yandere.Dragging || Yandere.Carrying || Yandere.PickUp != null || Yandere.Chased || Yandere.Chasers > 0 || StudentManager.Reporter != null || StudentManager.MurderTakingPlace)
		{
			CanAttendClass = false;
		}
	}

	public void EndDay()
	{
		StudentManager.StopMoving();
		Yandere.StopLaughing();
		Yandere.EmptyHands();
		Clock.StopTime = true;
		Police.Darkness.enabled = true;
		Police.FadeOut = true;
		Police.DayOver = true;
	}

	private void CheckForLateness()
	{
		Late = 0;
		if (!(StudentManager.Teachers[21] != null) || !(StudentManager.Teachers[21].DistanceToDestination < 1f))
		{
			return;
		}
		if (Clock.HourTime < 13f)
		{
			if (Clock.HourTime < 8.52f)
			{
				Late = 0;
			}
			else if (Clock.HourTime < 10f)
			{
				Late = 1;
			}
			else if (Clock.HourTime < 11f)
			{
				Late = 2;
			}
			else if (Clock.HourTime < 12f)
			{
				Late = 3;
			}
			else if (Clock.HourTime < 13f)
			{
				Late = 4;
			}
		}
		else if (Clock.HourTime < 13.52f)
		{
			Late = 0;
		}
		else if (Clock.HourTime < 14f)
		{
			Late = 1;
		}
		else if (Clock.HourTime < 14.5f)
		{
			Late = 2;
		}
		else if (Clock.HourTime < 15f)
		{
			Late = 3;
		}
		else if (Clock.HourTime < 15.5f)
		{
			Late = 4;
		}
		Reputation.PendingRep -= 5 * Late;
	}

	public void EndEvents()
	{
		for (int i = 0; i < MorningEvents.Length; i++)
		{
			if (MorningEvents[i].enabled)
			{
				MorningEvents[i].EndEvent();
			}
		}
	}
}
