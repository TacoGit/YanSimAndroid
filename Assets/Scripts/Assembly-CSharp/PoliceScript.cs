using System;
using UnityEngine;

public class PoliceScript : MonoBehaviour
{
	public LowRepGameOverScript LowRepGameOver;

	public StudentManagerScript StudentManager;

	public ClubManagerScript ClubManager;

	public HeartbrokenScript Heartbroken;

	public PauseScreenScript PauseScreen;

	public ReputationScript Reputation;

	public TranqCaseScript TranqCase;

	public EndOfDayScript EndOfDay;

	public JukeboxScript Jukebox;

	public YandereScript Yandere;

	public ClockScript Clock;

	public JsonScript JSON;

	public UIPanel Panel;

	public GameObject HeartbeatCamera;

	public GameObject DetectionCamera;

	public GameObject SuicideStudent;

	public GameObject UICamera;

	public GameObject Icons;

	public GameObject FPS;

	public Transform BloodParent;

	public Transform LimbParent;

	public RagdollScript[] CorpseList;

	public UILabel[] ResultsLabels;

	public UILabel ContinueLabel;

	public UILabel TimeLabel;

	public UISprite ContinueButton;

	public UISprite Darkness;

	public UISprite BloodIcon;

	public UISprite UniformIcon;

	public UISprite WeaponIcon;

	public UISprite CorpseIcon;

	public UISprite PartsIcon;

	public UISprite SanityIcon;

	public string ElectrocutedStudentName = string.Empty;

	public string DrownedStudentName = string.Empty;

	public bool BloodDisposed;

	public bool UniformDisposed;

	public bool WeaponDisposed;

	public bool CorpseDisposed;

	public bool PartsDisposed;

	public bool SanityRestored;

	public bool MurderSuicideScene;

	public bool ElectroScene;

	public bool SuicideScene;

	public bool PoisonScene;

	public bool MurderScene;

	public bool DrownScene;

	public bool TeacherReport;

	public bool ClubActivity;

	public bool CouncilDeath;

	public bool MaskReported;

	public bool FadeResults;

	public bool ShowResults;

	public bool GameOver;

	public bool DayOver;

	public bool Delayed;

	public bool FadeOut;

	public bool Suicide;

	public bool Called;

	public bool LowRep;

	public bool Show;

	public int IncineratedWeapons;

	public int BloodyClothing;

	public int BloodyWeapons;

	public int HiddenCorpses;

	public int MurderWeapons;

	public int PhotoEvidence;

	public int BodyParts;

	public int Witnesses;

	public int Corpses;

	public int Deaths;

	public float ResultsTimer;

	public float Timer;

	public int Minutes;

	public int Seconds;

	public int SuspensionLength;

	public int RemainingDays;

	public bool Suspended;

	private void Start()
	{
		if (SchoolGlobals.SchoolAtmosphere > 0.5f)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 0f);
			Darkness.enabled = false;
		}
		base.transform.localPosition = new Vector3(-260f, base.transform.localPosition.y, base.transform.localPosition.z);
		UILabel[] resultsLabels = ResultsLabels;
		foreach (UILabel uILabel in resultsLabels)
		{
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0f);
		}
		ContinueLabel.color = new Color(ContinueLabel.color.r, ContinueLabel.color.g, ContinueLabel.color.b, 0f);
		ContinueButton.color = new Color(ContinueButton.color.r, ContinueButton.color.g, ContinueButton.color.b, 0f);
		Icons.SetActive(false);
	}

	private void Update()
	{
		if (Show)
		{
			StudentManager.TutorialWindow.ShowPoliceMessage = true;
			if (PoisonScene)
			{
			}
			if (!Icons.activeInHierarchy)
			{
				Icons.SetActive(true);
			}
			base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, 0f, Time.deltaTime * 10f), base.transform.localPosition.y, base.transform.localPosition.z);
			if (BloodParent.childCount == 0)
			{
				if (!BloodDisposed)
				{
					BloodIcon.spriteName = "Yes";
					BloodDisposed = true;
				}
			}
			else if (BloodDisposed)
			{
				BloodIcon.spriteName = "No";
				BloodDisposed = false;
			}
			if (BloodyClothing == 0)
			{
				if (!UniformDisposed)
				{
					UniformIcon.spriteName = "Yes";
					UniformDisposed = true;
				}
			}
			else if (UniformDisposed)
			{
				UniformIcon.spriteName = "No";
				UniformDisposed = false;
			}
			if (IncineratedWeapons == MurderWeapons)
			{
				if (!WeaponDisposed)
				{
					WeaponIcon.spriteName = "Yes";
					WeaponDisposed = true;
				}
			}
			else if (WeaponDisposed)
			{
				WeaponIcon.spriteName = "No";
				WeaponDisposed = false;
			}
			if (Corpses == 0)
			{
				if (!CorpseDisposed)
				{
					CorpseIcon.spriteName = "Yes";
					CorpseDisposed = true;
				}
			}
			else if (CorpseDisposed)
			{
				CorpseIcon.spriteName = "No";
				CorpseDisposed = false;
			}
			if (BodyParts == 0)
			{
				if (!PartsDisposed)
				{
					PartsIcon.spriteName = "Yes";
					PartsDisposed = true;
				}
			}
			else if (PartsDisposed)
			{
				PartsIcon.spriteName = "No";
				PartsDisposed = false;
			}
			if (Yandere.Sanity == 100f)
			{
				if (!SanityRestored)
				{
					SanityIcon.spriteName = "Yes";
					SanityRestored = true;
				}
			}
			else if (SanityRestored)
			{
				SanityIcon.spriteName = "No";
				SanityRestored = false;
			}
			Timer = Mathf.MoveTowards(Timer, 0f, Time.deltaTime);
			if (Timer <= 0f)
			{
				Timer = 0f;
				if (!Yandere.Attacking && !Yandere.Struggling && !Yandere.Egg && !FadeOut)
				{
					BeginFadingOut();
				}
			}
			int num = Mathf.CeilToInt(Timer);
			Minutes = num / 60;
			Seconds = num % 60;
			TimeLabel.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);
		}
		if (FadeOut)
		{
			if (Yandere.Laughing)
			{
				Yandere.StopLaughing();
			}
			if (Clock.TimeSkip || Yandere.CanMove)
			{
				if (Clock.TimeSkip)
				{
					Clock.EndTimeSkip();
				}
				Yandere.StopAiming();
				Yandere.CanMove = false;
				Yandere.YandereVision = false;
				Yandere.PauseScreen.enabled = false;
				Yandere.Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
				if (Yandere.Mask != null)
				{
					Yandere.Mask.Drop();
				}
				if (Yandere.PickUp != null)
				{
					Yandere.EmptyHands();
				}
				if (Yandere.Dragging || Yandere.Carrying)
				{
					Yandere.EmptyHands();
				}
			}
			PauseScreen.Panel.alpha = Mathf.MoveTowards(PauseScreen.Panel.alpha, 0f, Time.deltaTime);
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			if (Darkness.color.a >= 1f && !ShowResults)
			{
				HeartbeatCamera.SetActive(false);
				DetectionCamera.SetActive(false);
				if (ClubActivity)
				{
					ClubManager.Club = ClubGlobals.Club;
					ClubManager.ClubActivity();
					FadeOut = false;
				}
				else
				{
					Yandere.MyController.enabled = false;
					Yandere.enabled = false;
					DetermineResults();
					ShowResults = true;
					Time.timeScale = 2f;
					Jukebox.Volume = 0f;
				}
			}
		}
		if (ShowResults)
		{
			ResultsTimer += Time.deltaTime;
			if (ResultsTimer > 1f)
			{
				UILabel uILabel = ResultsLabels[0];
				uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, uILabel.color.a + Time.deltaTime);
			}
			if (ResultsTimer > 2f)
			{
				UILabel uILabel2 = ResultsLabels[1];
				uILabel2.color = new Color(uILabel2.color.r, uILabel2.color.g, uILabel2.color.b, uILabel2.color.a + Time.deltaTime);
			}
			if (ResultsTimer > 3f)
			{
				UILabel uILabel3 = ResultsLabels[2];
				uILabel3.color = new Color(uILabel3.color.r, uILabel3.color.g, uILabel3.color.b, uILabel3.color.a + Time.deltaTime);
			}
			if (ResultsTimer > 4f)
			{
				UILabel uILabel4 = ResultsLabels[3];
				uILabel4.color = new Color(uILabel4.color.r, uILabel4.color.g, uILabel4.color.b, uILabel4.color.a + Time.deltaTime);
			}
			if (ResultsTimer > 5f)
			{
				UILabel uILabel5 = ResultsLabels[4];
				uILabel5.color = new Color(uILabel5.color.r, uILabel5.color.g, uILabel5.color.b, uILabel5.color.a + Time.deltaTime);
			}
			if (ResultsTimer > 6f)
			{
				ContinueButton.color = new Color(ContinueButton.color.r, ContinueButton.color.g, ContinueButton.color.b, ContinueButton.color.a + Time.deltaTime);
				ContinueLabel.color = new Color(ContinueLabel.color.r, ContinueLabel.color.g, ContinueLabel.color.b, ContinueLabel.color.a + Time.deltaTime);
				if (ContinueButton.color.a > 1f)
				{
					ContinueButton.color = new Color(ContinueButton.color.r, ContinueButton.color.g, ContinueButton.color.b, 1f);
				}
				if (ContinueLabel.color.a > 1f)
				{
					ContinueLabel.color = new Color(ContinueLabel.color.r, ContinueLabel.color.g, ContinueLabel.color.b, 1f);
				}
			}
			if (Input.GetKeyDown("space"))
			{
				ShowResults = false;
				FadeResults = true;
				FadeOut = false;
				ResultsTimer = 0f;
			}
			if (ResultsTimer > 7f && Input.GetButtonDown("A"))
			{
				ShowResults = false;
				FadeResults = true;
				FadeOut = false;
				ResultsTimer = 0f;
			}
		}
		UILabel[] resultsLabels = ResultsLabels;
		foreach (UILabel uILabel6 in resultsLabels)
		{
			if (uILabel6.color.a > 1f)
			{
				uILabel6.color = new Color(uILabel6.color.r, uILabel6.color.g, uILabel6.color.b, 1f);
			}
		}
		if (!FadeResults)
		{
			return;
		}
		UILabel[] resultsLabels2 = ResultsLabels;
		foreach (UILabel uILabel7 in resultsLabels2)
		{
			uILabel7.color = new Color(uILabel7.color.r, uILabel7.color.g, uILabel7.color.b, uILabel7.color.a - Time.deltaTime);
		}
		ContinueButton.color = new Color(ContinueButton.color.r, ContinueButton.color.g, ContinueButton.color.b, ContinueButton.color.a - Time.deltaTime);
		ContinueLabel.color = new Color(ContinueLabel.color.r, ContinueLabel.color.g, ContinueLabel.color.b, ContinueLabel.color.a - Time.deltaTime);
		if (!(ResultsLabels[0].color.a <= 0f))
		{
			return;
		}
		if (GameOver)
		{
			Heartbroken.transform.parent.transform.parent = null;
			Heartbroken.transform.parent.gameObject.SetActive(true);
			Heartbroken.Noticed = false;
			base.transform.parent.transform.parent.gameObject.SetActive(false);
			if (!EndOfDay.gameObject.activeInHierarchy)
			{
				Time.timeScale = 1f;
			}
		}
		else if (LowRep)
		{
			Yandere.RPGCamera.enabled = false;
			Yandere.RPGCamera.transform.parent = LowRepGameOver.MyCamera;
			Yandere.RPGCamera.transform.localPosition = new Vector3(0f, 0f, 0f);
			Yandere.RPGCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			LowRepGameOver.gameObject.SetActive(true);
			UICamera.SetActive(false);
			FPS.SetActive(false);
			Time.timeScale = 1f;
			base.enabled = false;
		}
		else if (!TeacherReport)
		{
			if (EndOfDay.Phase == 1)
			{
				EndOfDay.gameObject.SetActive(true);
				EndOfDay.enabled = true;
				EndOfDay.Phase = 12;
				if (EndOfDay.PreviouslyActivated)
				{
					EndOfDay.Start();
				}
				for (int k = 0; k < 5; k++)
				{
					ResultsLabels[k].text = string.Empty;
				}
				base.enabled = false;
			}
		}
		else
		{
			DetermineResults();
			TeacherReport = false;
			FadeResults = false;
			ShowResults = true;
		}
	}

	private void DetermineResults()
	{
		ResultsLabels[0].transform.parent.gameObject.SetActive(true);
		if (Show)
		{
			EndOfDay.gameObject.SetActive(true);
			base.enabled = false;
			for (int i = 0; i < 5; i++)
			{
				ResultsLabels[i].text = string.Empty;
			}
		}
		else if (Yandere.ShoulderCamera.GoingToCounselor)
		{
			ResultsLabels[0].text = "While Ayano was in the counselor's office,";
			ResultsLabels[1].text = "a corpse was discovered on school grounds.";
			ResultsLabels[2].text = "The school faculty was informed of the corpse,";
			ResultsLabels[3].text = "and the police were called to the school.";
			ResultsLabels[4].text = "No one is allowed to leave school until a police investigation has taken place.";
			TeacherReport = true;
			Show = true;
		}
		else if (Reputation.Reputation <= -100f)
		{
			ResultsLabels[0].text = "Ayano's bizarre conduct has been observed and discussed by many people.";
			ResultsLabels[1].text = "Word of Ayano's strange behavior has reached Senpai.";
			ResultsLabels[2].text = "Senpai is now aware that Ayano is a deranged person.";
			ResultsLabels[3].text = "From this day forward, Senpai will fear and avoid Ayano.";
			ResultsLabels[4].text = "Ayano will never have her Senpai's love.";
			LowRep = true;
		}
		else if (DateGlobals.Weekday == DayOfWeek.Friday)
		{
			ResultsLabels[0].text = "This is the part where the game will determine whether or not the player has eliminated their rival.";
			ResultsLabels[1].text = "This game is still in development.";
			ResultsLabels[2].text = "The ''player eliminated rival'' state has not yet been implemented.";
			ResultsLabels[3].text = "Thank you for playtesting Yandere Simulator!";
			ResultsLabels[4].text = "Please check back soon for more updates!";
			GameOver = true;
		}
		else if (!Suicide && !PoisonScene)
		{
			if (Clock.HourTime < 18f)
			{
				if (Yandere.InClass)
				{
					ResultsLabels[0].text = "Ayano attempts to attend class without disposing of a corpse.";
				}
				else if (Yandere.Resting && Corpses > 0)
				{
					ResultsLabels[0].text = "Ayano rests without disposing of a corpse.";
				}
				else if (Yandere.Resting)
				{
					ResultsLabels[0].text = "Ayano recovers from her injuries, then stands near the school gate and waits for Senpai to leave school.";
				}
				else
				{
					ResultsLabels[0].text = "Ayano stands near the school gate and waits for Senpai to leave school.";
				}
			}
			else
			{
				ResultsLabels[0].text = "The school day has ended. Faculty members must walk through the school and tell any lingering students to leave.";
			}
			if (Suspended)
			{
				if (Clock.Weekday == 1)
				{
					RemainingDays = 5;
				}
				else if (Clock.Weekday == 2)
				{
					RemainingDays = 4;
				}
				else if (Clock.Weekday == 3)
				{
					RemainingDays = 3;
				}
				else if (Clock.Weekday == 4)
				{
					RemainingDays = 2;
				}
				else if (Clock.Weekday == 5)
				{
					RemainingDays = 1;
				}
				if (RemainingDays - SuspensionLength <= 0)
				{
					ResultsLabels[0].text = "Due to her suspension,";
					ResultsLabels[1].text = "Ayano will be unable";
					ResultsLabels[2].text = "to prevent her rival";
					ResultsLabels[3].text = "from confessing to Senpai.";
					ResultsLabels[4].text = "Ayano will never have Senpai.";
					GameOver = true;
				}
				else if (SuspensionLength == 1)
				{
					ResultsLabels[0].text = "Ayano has been sent home early.";
					ResultsLabels[1].text = string.Empty;
					ResultsLabels[2].text = "She won't be able to see Senpai again until tomorrow.";
					ResultsLabels[3].text = string.Empty;
					ResultsLabels[4].text = "Ayano's heart aches as she thinks of Senpai.";
				}
				else if (SuspensionLength == 2)
				{
					ResultsLabels[0].text = "Ayano has been sent home early.";
					ResultsLabels[1].text = string.Empty;
					ResultsLabels[2].text = "She will have to wait one day before returning to school.";
					ResultsLabels[3].text = string.Empty;
					ResultsLabels[4].text = "Ayano's heart aches as she thinks of Senpai.";
				}
				else
				{
					ResultsLabels[0].text = "Ayano has been sent home early.";
					ResultsLabels[1].text = string.Empty;
					ResultsLabels[2].text = "She will have to wait " + (SuspensionLength - 1) + " days before returning to school.";
					ResultsLabels[3].text = string.Empty;
					ResultsLabels[4].text = "Ayano's heart aches as she thinks of Senpai.";
				}
				return;
			}
			if (Yandere.RedPaint)
			{
				BloodyClothing--;
			}
			if (Corpses == 0 && LimbParent.childCount == 0 && BloodParent.childCount == 0 && BloodyWeapons == 0 && BloodyClothing == 0 && !SuicideScene)
			{
				if (Yandere.Sanity < 66.66666f || (Yandere.Bloodiness > 0f && !Yandere.RedPaint))
				{
					ResultsLabels[1].text = "Ayano is approached by a faculty member.";
					if (Yandere.Bloodiness > 0f)
					{
						ResultsLabels[2].text = "The faculty member immediately notices the blood staining her clothing.";
						ResultsLabels[3].text = "Ayano is not able to convince the faculty member that nothing is wrong.";
						ResultsLabels[4].text = "The faculty member calls the police.";
						TeacherReport = true;
						Show = true;
					}
					else
					{
						ResultsLabels[2].text = "Ayano exhibited extremely erratic behavior, frightening the faculty member.";
						ResultsLabels[3].text = "The faculty member becomes angry with Ayano, but Ayano leaves before the situation gets worse.";
						ResultsLabels[4].text = "Ayano returns home.";
					}
				}
				else if (Clock.HourTime < 18f)
				{
					ResultsLabels[1].text = "Finally, Senpai exits the school.";
					ResultsLabels[2].text = "Ayano's heart skips a beat when she sees him.";
					ResultsLabels[3].text = "Ayano leaves school and watches Senpai walk home.";
					ResultsLabels[4].text = "Once he is safely home, Ayano returns to her own home.";
				}
				else
				{
					ResultsLabels[1].text = "Like all other students, Ayano is instructed to leave school.";
					ResultsLabels[2].text = "Senpai leaves school, too.";
					ResultsLabels[3].text = "Ayano leaves school and watches Senpai walk home.";
					ResultsLabels[4].text = "Once he is safely home, Ayano returns to her own home.";
				}
			}
			else if (Corpses > 0)
			{
				ResultsLabels[1].text = "While walking around the school, a faculty member discovers a corpse.";
				ResultsLabels[2].text = "The faculty member immediately calls the police.";
				ResultsLabels[3].text = "The faculty member informs the rest of the faculty about her discovery.";
				ResultsLabels[4].text = "The faculty do not allow any students to leave the school until a police investigation has taken place.";
				TeacherReport = true;
				Show = true;
			}
			else if (LimbParent.childCount > 0)
			{
				ResultsLabels[1].text = "While walking around the school, a faculty member discovers a dismembered body part.";
				ResultsLabels[2].text = "The faculty member decides to call the police.";
				ResultsLabels[3].text = "The faculty member informs the rest of the faculty about her discovery.";
				ResultsLabels[4].text = "The faculty do not allow any students to leave the school until a police investigation has taken place.";
				TeacherReport = true;
				Show = true;
			}
			else if (BloodParent.childCount > 0 || BloodyClothing > 0)
			{
				ResultsLabels[1].text = "While walking around the school, a faculty member discovers a mysterious blood stain.";
				ResultsLabels[2].text = "The faculty member decides to call the police.";
				ResultsLabels[3].text = "The faculty member informs the rest of the faculty about her discovery.";
				ResultsLabels[4].text = "The faculty do not allow any students to leave the school until a police investigation has taken place.";
				TeacherReport = true;
				Show = true;
			}
			else if (BloodyWeapons > 0)
			{
				ResultsLabels[1].text = "While walking around the school, a faculty member discovers a mysterious bloody weapon.";
				ResultsLabels[2].text = "The faculty member decides to call the police.";
				ResultsLabels[3].text = "The faculty member informs the rest of the faculty about her discovery.";
				ResultsLabels[4].text = "The faculty do not allow any students to leave the school until a police investigation has taken place.";
				TeacherReport = true;
				Show = true;
			}
			else if (SuicideScene)
			{
				ResultsLabels[1].text = "While walking around the school, a faculty member discovers a pair of shoes on the rooftop.";
				ResultsLabels[2].text = "The faculty member fears that there has been a suicide, but cannot find a corpse anywhere. The faculty member does not take any action.";
				ResultsLabels[3].text = "Ayano leaves school and watches Senpai walk home.";
				ResultsLabels[4].text = "Once he is safely home, Ayano returns to her own home.";
			}
		}
		else if (Suicide)
		{
			if (!Yandere.InClass)
			{
				ResultsLabels[0].text = "The school day has ended. Faculty members must walk through the school and tell any lingering students to leave.";
			}
			else
			{
				ResultsLabels[0].text = "Ayano attempts to attend class without disposing of a corpse.";
			}
			ResultsLabels[1].text = "While walking around the school, a faculty member discovers a corpse.";
			ResultsLabels[2].text = "It appears as though a student has committed suicide.";
			ResultsLabels[3].text = "The faculty member informs the rest of the faculty about her discovery.";
			ResultsLabels[4].text = "The faculty members agree to call the police and report the student's death.";
			TeacherReport = true;
			Show = true;
		}
		else if (PoisonScene)
		{
			ResultsLabels[0].text = "A faculty member discovers the student who Ayano poisoned.";
			ResultsLabels[1].text = "The faculty member calls for an ambulance immediately.";
			ResultsLabels[2].text = "The faculty member suspects that the student's death was a murder.";
			ResultsLabels[3].text = "The faculty member also calls for the police.";
			ResultsLabels[4].text = "The school's students are not allowed to leave until a police investigation has taken place.";
			TeacherReport = true;
			Show = true;
		}
	}

	public void KillStudents()
	{
		if (Deaths > 0)
		{
			for (int i = 2; i < StudentManager.NPCsTotal + 1; i++)
			{
				if (StudentGlobals.GetStudentDying(i))
				{
					if (i < 90)
					{
						SchoolGlobals.SchoolAtmosphere -= 0.05f;
					}
					else
					{
						SchoolGlobals.SchoolAtmosphere -= 0.15f;
					}
					if (JSON.Students[i].Club == ClubType.Council)
					{
						SchoolGlobals.SchoolAtmosphere -= 1f;
						SchoolGlobals.HighSecurity = true;
					}
					StudentGlobals.SetStudentDead(i, true);
					PlayerGlobals.Kills++;
				}
			}
			SchoolGlobals.SchoolAtmosphere -= (float)Corpses * 0.05f;
			if (Corpses > 0)
			{
				RagdollScript[] corpseList = CorpseList;
				foreach (RagdollScript ragdollScript in corpseList)
				{
					if (ragdollScript != null && StudentGlobals.MemorialStudents < 9)
					{
						Debug.Log("''MemorialStudents'' is being incremented upwards.");
						StudentGlobals.MemorialStudents++;
						if (StudentGlobals.MemorialStudents == 1)
						{
							StudentGlobals.MemorialStudent1 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 2)
						{
							StudentGlobals.MemorialStudent2 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 3)
						{
							StudentGlobals.MemorialStudent3 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 4)
						{
							StudentGlobals.MemorialStudent4 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 5)
						{
							StudentGlobals.MemorialStudent5 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 6)
						{
							StudentGlobals.MemorialStudent6 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 7)
						{
							StudentGlobals.MemorialStudent7 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 8)
						{
							StudentGlobals.MemorialStudent8 = ragdollScript.Student.StudentID;
						}
						else if (StudentGlobals.MemorialStudents == 9)
						{
							StudentGlobals.MemorialStudent9 = ragdollScript.Student.StudentID;
						}
					}
				}
			}
		}
		else if (!SchoolGlobals.HighSecurity)
		{
			SchoolGlobals.SchoolAtmosphere += 0.2f;
		}
		SchoolGlobals.SchoolAtmosphere = Mathf.Clamp01(SchoolGlobals.SchoolAtmosphere);
		for (int k = 1; k < StudentManager.StudentsTotal; k++)
		{
			StudentScript studentScript = StudentManager.Students[k];
			if (studentScript != null && studentScript.Grudge && studentScript.Persona != PersonaType.Evil)
			{
				StudentGlobals.SetStudentGrudge(k, true);
				if (studentScript.OriginalPersona == PersonaType.Sleuth && !StudentGlobals.GetStudentDying(k))
				{
					StudentGlobals.SetStudentGrudge(56, true);
					StudentGlobals.SetStudentGrudge(57, true);
					StudentGlobals.SetStudentGrudge(58, true);
					StudentGlobals.SetStudentGrudge(59, true);
					StudentGlobals.SetStudentGrudge(60, true);
				}
			}
		}
	}

	public void BeginFadingOut()
	{
		Debug.Log("BeginFadingOut() has been called.");
		DayOver = true;
		StudentManager.StopMoving();
		Darkness.enabled = true;
		Yandere.StopLaughing();
		Clock.StopTime = true;
		FadeOut = true;
		if (!EndOfDay.gameObject.activeInHierarchy)
		{
			Time.timeScale = 1f;
		}
	}

	public void UpdateCorpses()
	{
		RagdollScript[] corpseList = CorpseList;
		foreach (RagdollScript ragdollScript in corpseList)
		{
			if (ragdollScript != null)
			{
				ragdollScript.Prompt.HideButton[3] = true;
				if (ClassGlobals.PhysicalGrade + ClassGlobals.PhysicalBonus > 0 && !ragdollScript.Tranquil)
				{
					ragdollScript.Prompt.HideButton[3] = false;
				}
			}
		}
	}
}
