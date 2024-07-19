using UnityEngine;

public class CounselorScript : MonoBehaviour
{
	public CutsceneManagerScript CutsceneManager;

	public StudentManagerScript StudentManager;

	public CounselorDoorScript CounselorDoor;

	public InputManagerScript InputManager;

	public PromptBarScript PromptBar;

	public EndOfDayScript EndOfDay;

	public SubtitleScript Subtitle;

	public SchemesScript Schemes;

	public StudentScript Student;

	public YandereScript Yandere;

	public Animation MyAnimation;

	public AudioSource MyAudio;

	public PromptScript Prompt;

	public AudioClip[] CounselorGreetingClips;

	public AudioClip[] CounselorLectureClips;

	public AudioClip[] CounselorReportClips;

	public AudioClip[] RivalClips;

	public AudioClip CounselorFarewellClip;

	public readonly string CounselorFarewellText = "Don't misbehave.";

	public AudioClip CounselorBusyClip;

	public readonly string CounselorBusyText = "I'm sorry, I've got my hands full for the rest of today. I won't be available until tomorrow.";

	public readonly string[] CounselorGreetingText = new string[3]
	{
		string.Empty,
		"What can I help you with?",
		"Can I help you?"
	};

	public readonly string[] CounselorLectureText = new string[7]
	{
		string.Empty,
		"Your \"after-school activities\" are completely unacceptable. You should not be conducting yourself in such a manner. This kind of behavior could cause a scandal! You could run the school's reputation into the ground!",
		"May I take a look inside your bag? ...this doesn't belong to you, does it?! What are you doing with someone else's property?",
		"I need to take a look in your bag. ...cigarettes?! You have absolutely no excuse to be carrying something like this around!",
		"May I see your phone for a moment? ...what is THIS?! Would you care to explain why something like this is on your phone?",
		"Obviously, we need to have a long talk about the kind of behavior that will not tolerated at this school!",
		"That's it! I've given you enough second chances. You have repeatedly broken school rules and ignored every warning that I have given you. You have left me with no choice but to permanently expel you!"
	};

	public readonly string[] CounselorReportText = new string[6]
	{
		string.Empty,
		"This is...! Thank you for bringing this to my attention. This kind of conduct will definitely harm the school's reputation. I'll have to have a word with her later today.",
		"Is that true? I'd hate to think we have a thief here at school. Don't worry - I'll get to the bottom of this.",
		"That's a clear violation of school rules, not to mention completely illegal. If what you're saying is true, she will face serious consequences. I'll confront her about this.",
		"That's a very serious accusation. I hope you're not lying to me. Hopefully, it's just a misunderstanding. I'll investigate the matter.",
		"That's a bold claim. Are you certain? I'll investigate the matter. If she is cheating, I'll catch her in the act."
	};

	public readonly string[] LectureIntro = new string[6]
	{
		string.Empty,
		"The guidance counselor asks Kokona to visit her office after school ends...",
		"The guidance counselor asks Kokona to visit her office after school ends...",
		"The guidance counselor asks Kokona to visit her office after school ends...",
		"The guidance counselor asks Kokona to visit her office after school ends...",
		"The guidance counselor asks Kokona to visit her office after school ends..."
	};

	public readonly string[] RivalText = new string[7]
	{
		string.Empty,
		"It...it's not what you think...I was just...um...",
		"No! I'm not the one who did this! I would never steal from anyone!",
		"Huh? I don't smoke! I don't know why something like this was in my bag!",
		"What?! I've never taken any pictures like that! How did this get on my phone?!",
		"I'm telling the truth! I didn't steal the answer sheet! I don't know why it was in my desk!",
		"No! Please! Don't do this!"
	};

	public UILabel[] Labels;

	public Transform CounselorWindow;

	public Transform Highlight;

	public Transform Chibi;

	public SkinnedMeshRenderer Face;

	public UILabel CounselorSubtitle;

	public UISprite EndOfDayDarkness;

	public UILabel LectureSubtitle;

	public UISprite ExpelProgress;

	public UILabel LectureLabel;

	public bool ShowWindow;

	public bool Lecturing;

	public bool Busy;

	public int Selected = 1;

	public int LecturePhase = 1;

	public int LectureID = 5;

	public float ExpelTimer;

	public float ChinTimer;

	public float Timer;

	public Vector3 LookAtTarget;

	public bool LookAtPlayer;

	public Transform Default;

	public Transform Head;

	public bool Angry;

	public bool Stern;

	public bool Sad;

	public float MouthTarget;

	public float MouthTimer;

	public float TimerLimit;

	public float MouthOpen;

	public float TalkSpeed;

	public float BS_SadMouth;

	public float BS_MadBrow;

	public float BS_SadBrow;

	public float BS_AngryEyes;

	public DetectClickScript[] CounselorOption;

	public InputDeviceScript InputDevice;

	public StudentWitnessType Crime;

	public UITexture GenkaChibi;

	public CameraShake Shake;

	public Texture HappyChibi;

	public Texture AnnoyedChibi;

	public Texture MadChibi;

	public GameObject CounselorOptions;

	public GameObject CounselorBar;

	public GameObject Reticle;

	public GameObject Laptop;

	public Transform CameraTarget;

	public int InterrogationPhase;

	public int Patience;

	public int CrimeID;

	public int Answer;

	public bool MustExpelDelinquents;

	public bool ExpelledDelinquents;

	public bool SilentTreatment;

	public bool Interrogating;

	public bool Expelled;

	public bool Slammed;

	public AudioSource Rumble;

	public AudioClip Countdown;

	public AudioClip Choice;

	public AudioClip Slam;

	public AudioClip[] GreetingClips;

	public string[] Greetings;

	public AudioClip[] BloodLectureClips;

	public string[] BloodLectures;

	public AudioClip[] InsanityLectureClips;

	public string[] InsanityLectures;

	public AudioClip[] LewdLectureClips;

	public string[] LewdLectures;

	public AudioClip[] TheftLectureClips;

	public string[] TheftLectures;

	public AudioClip[] TrespassLectureClips;

	public string[] TrespassLectures;

	public AudioClip[] WeaponLectureClips;

	public string[] WeaponLectures;

	public AudioClip[] SilentClips;

	public string[] Silents;

	public AudioClip[] SuspensionClips;

	public string[] Suspensions;

	public AudioClip[] AcceptExcuseClips;

	public string[] AcceptExcuses;

	public AudioClip[] RejectExcuseClips;

	public string[] RejectExcuses;

	public AudioClip[] RejectLieClips;

	public string[] RejectLies;

	public AudioClip[] AcceptBlameClips;

	public string[] AcceptBlames;

	public AudioClip[] RejectApologyClips;

	public string[] RejectApologies;

	public AudioClip[] RejectBlameClips;

	public string[] RejectBlames;

	public AudioClip[] RejectFlirtClips;

	public string[] RejectFlirts;

	public AudioClip[] BadClosingClips;

	public string[] BadClosings;

	public AudioClip[] BlameClosingClips;

	public string[] BlameClosings;

	public AudioClip[] FreeToLeaveClips;

	public string[] FreeToLeaves;

	public AudioClip AcceptApologyClip;

	public string AcceptApology;

	public AudioClip RejectThreatClip;

	public string RejectThreat;

	public AudioClip ExpelDelinquentsClip;

	public string ExpelDelinquents;

	public AudioClip DelinquentsDeadClip;

	public string DelinquentsDead;

	public AudioClip DelinquentsExpelledClip;

	public string DelinquentsExpelled;

	public AudioClip DelinquentsGoneClip;

	public string DelinquentsGone;

	public AudioClip[] ExcuseClips;

	public string[] Excuses;

	public AudioClip[] LieClips;

	public string[] Lies;

	public AudioClip[] DelinquentClips;

	public string[] Delinquents;

	public AudioClip ApologyClip;

	public string Apology;

	public AudioClip FlirtClip;

	public string Flirt;

	public AudioClip ThreatenClip;

	public string Threaten;

	public AudioClip Silence;

	private void Start()
	{
		CounselorWindow.localScale = Vector3.zero;
		CounselorWindow.gameObject.SetActive(false);
		CounselorOptions.SetActive(false);
		CounselorBar.SetActive(false);
		Reticle.SetActive(false);
		ExpelProgress.color = new Color(ExpelProgress.color.r, ExpelProgress.color.g, ExpelProgress.color.b, 0f);
		Chibi.localPosition = new Vector3(Chibi.localPosition.x, 250f + (float)StudentGlobals.ExpelProgress * -90f, Chibi.localPosition.z);
	}

	private void Update()
	{
		if (LookAtPlayer)
		{
			if (InputManager.TappedUp)
			{
				Selected--;
				if (Selected == 6)
				{
					Selected = 5;
				}
				UpdateHighlight();
			}
			if (InputManager.TappedDown)
			{
				Selected++;
				if (Selected == 6)
				{
					Selected = 7;
				}
				UpdateHighlight();
			}
			if (ShowWindow)
			{
				if (CounselorDoor.Darkness.color.a == 0f && Input.GetButtonDown("A"))
				{
					if (Selected == 7)
					{
						if (!CounselorDoor.Exit)
						{
							CounselorSubtitle.text = CounselorFarewellText;
							MyAudio.clip = CounselorFarewellClip;
							MyAudio.Play();
							CounselorDoor.FadeOut = true;
							CounselorDoor.FadeIn = false;
							CounselorDoor.Exit = true;
						}
					}
					else if (Labels[Selected].color.a == 1f)
					{
						if (Selected == 1)
						{
							SchemeGlobals.SetSchemeStage(1, 100);
							Schemes.UpdateInstructions();
						}
						else if (Selected == 2)
						{
							SchemeGlobals.SetSchemeStage(2, 100);
							Schemes.UpdateInstructions();
						}
						else if (Selected == 3)
						{
							SchemeGlobals.SetSchemeStage(3, 100);
							Schemes.UpdateInstructions();
						}
						else if (Selected == 4)
						{
							SchemeGlobals.SetSchemeStage(4, 100);
							Schemes.UpdateInstructions();
						}
						else if (Selected == 5)
						{
							SchemeGlobals.SetSchemeStage(5, 7);
							Schemes.UpdateInstructions();
						}
						CounselorSubtitle.text = CounselorReportText[Selected];
						MyAudio.clip = CounselorReportClips[Selected];
						MyAudio.Play();
						ShowWindow = false;
						Angry = true;
						LectureID = Selected;
						PromptBar.ClearButtons();
						PromptBar.Show = false;
						Busy = true;
					}
				}
			}
			else if (!Interrogating)
			{
				if (Input.GetButtonDown("A"))
				{
					MyAudio.Stop();
				}
				if (!MyAudio.isPlaying)
				{
					Timer += Time.deltaTime;
					if (Timer > 0.5f)
					{
						CounselorDoor.FadeOut = true;
						CounselorDoor.Exit = true;
						LookAtPlayer = false;
						UpdateList();
					}
				}
			}
		}
		else if (Interrogating)
		{
		}
		if (ShowWindow)
		{
			CounselorWindow.localScale = Vector3.Lerp(CounselorWindow.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		}
		else if (CounselorWindow.localScale.x > 0.1f)
		{
			CounselorWindow.localScale = Vector3.Lerp(CounselorWindow.localScale, Vector3.zero, Time.deltaTime * 10f);
		}
		else if (CounselorWindow.gameObject.activeInHierarchy)
		{
			CounselorWindow.localScale = Vector3.zero;
			CounselorWindow.gameObject.SetActive(false);
		}
		if (Lecturing)
		{
			Chibi.localPosition = new Vector3(Chibi.localPosition.x, Mathf.Lerp(Chibi.localPosition.y, 250f + (float)StudentGlobals.ExpelProgress * -90f, Time.deltaTime * 3f), Chibi.localPosition.z);
			if (LecturePhase == 1)
			{
				LectureLabel.text = LectureIntro[LectureID];
				EndOfDayDarkness.color = new Color(EndOfDayDarkness.color.r, EndOfDayDarkness.color.g, EndOfDayDarkness.color.b, Mathf.MoveTowards(EndOfDayDarkness.color.a, 0f, Time.deltaTime));
				if (EndOfDayDarkness.color.a == 0f)
				{
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Continue";
					PromptBar.UpdateButtons();
					PromptBar.Show = true;
					if (Input.GetButtonDown("A"))
					{
						LecturePhase++;
						PromptBar.ClearButtons();
						PromptBar.Show = false;
					}
				}
			}
			else if (LecturePhase == 2)
			{
				LectureLabel.color = new Color(LectureLabel.color.r, LectureLabel.color.g, LectureLabel.color.b, Mathf.MoveTowards(LectureLabel.color.a, 0f, Time.deltaTime));
				if (LectureLabel.color.a == 0f)
				{
					EndOfDay.TextWindow.SetActive(false);
					EndOfDay.EODCamera.GetComponent<AudioListener>().enabled = true;
					LectureSubtitle.text = CounselorLectureText[LectureID];
					MyAudio.clip = CounselorLectureClips[LectureID];
					MyAudio.Play();
					LecturePhase++;
				}
			}
			else if (LecturePhase == 3)
			{
				if (!MyAudio.isPlaying || Input.GetButtonDown("A"))
				{
					LectureSubtitle.text = RivalText[LectureID];
					MyAudio.clip = RivalClips[LectureID];
					MyAudio.Play();
					LecturePhase++;
				}
			}
			else if (LecturePhase == 4)
			{
				if (!MyAudio.isPlaying || Input.GetButtonDown("A"))
				{
					LectureSubtitle.text = string.Empty;
					if (StudentGlobals.ExpelProgress < 5)
					{
						LecturePhase++;
					}
					else
					{
						LecturePhase = 7;
						ExpelTimer = 11f;
					}
				}
			}
			else if (LecturePhase == 5)
			{
				ExpelProgress.color = new Color(ExpelProgress.color.r, ExpelProgress.color.g, ExpelProgress.color.b, Mathf.MoveTowards(ExpelProgress.color.a, 1f, Time.deltaTime));
				ExpelTimer += Time.deltaTime;
				if (ExpelTimer > 2f)
				{
					StudentGlobals.ExpelProgress++;
					LecturePhase++;
				}
			}
			else if (LecturePhase == 6)
			{
				ExpelTimer += Time.deltaTime;
				if (ExpelTimer > 4f)
				{
					LecturePhase++;
				}
			}
			else if (LecturePhase == 7)
			{
				ExpelProgress.color = new Color(ExpelProgress.color.r, ExpelProgress.color.g, ExpelProgress.color.b, Mathf.MoveTowards(ExpelProgress.color.a, 0f, Time.deltaTime));
				ExpelTimer += Time.deltaTime;
				if (ExpelTimer > 6f)
				{
					if ((StudentGlobals.ExpelProgress == 5 && !StudentGlobals.GetStudentExpelled(30) && StudentManager.Police.TranqCase.VictimID != 30) || StudentManager.Students[30].SentHome)
					{
						Debug.Log("Kokona has now been expelled.");
						StudentGlobals.SetStudentExpelled(30, true);
						EndOfDayDarkness.color = new Color(EndOfDayDarkness.color.r, EndOfDayDarkness.color.g, EndOfDayDarkness.color.b, 0f);
						LectureLabel.color = new Color(LectureLabel.color.r, LectureLabel.color.g, LectureLabel.color.b, 0f);
						LecturePhase = 2;
						ExpelTimer = 0f;
						LectureID = 6;
					}
					else if (LectureID < 6)
					{
						Debug.Log("We are heading to the end-of-day scene.");
						float a = EndOfDayDarkness.color.a;
						a = Mathf.MoveTowards(a, 1f, Time.deltaTime);
						EndOfDayDarkness.color = new Color(0f, 0f, 0f, a);
						if (a == 1f)
						{
							EndOfDay.enabled = true;
							EndOfDay.Phase++;
							EndOfDay.UpdateScene();
							base.enabled = false;
						}
					}
					else
					{
						Debug.Log("We are leaving the end-of-day scene.");
						EndOfDay.gameObject.SetActive(false);
						EndOfDay.Phase = 1;
						CutsceneManager.Phase++;
						Lecturing = false;
						LectureID = 0;
						Yandere.MainCamera.gameObject.SetActive(true);
						Yandere.gameObject.SetActive(true);
						StudentManager.ComeBack();
					}
				}
			}
		}
		if (!MyAudio.isPlaying)
		{
			CounselorSubtitle.text = string.Empty;
		}
		if (Interrogating)
		{
			UpdateInterrogation();
		}
	}

	public void Talk()
	{
		MyAnimation.CrossFade("CounselorComputerAttention", 1f);
		ChinTimer = 0f;
		Yandere.TargetStudent = Student;
		int num = Random.Range(1, 3);
		CounselorSubtitle.text = CounselorGreetingText[num];
		MyAudio.clip = CounselorGreetingClips[num];
		MyAudio.Play();
		StudentManager.DisablePrompts();
		CounselorWindow.gameObject.SetActive(true);
		LookAtPlayer = true;
		ShowWindow = true;
		Yandere.ShoulderCamera.OverShoulder = true;
		Yandere.WeaponMenu.KeyboardShow = false;
		Yandere.Obscurance.enabled = false;
		Yandere.WeaponMenu.Show = false;
		Yandere.YandereVision = false;
		Yandere.CanMove = false;
		Yandere.Talking = true;
		PromptBar.ClearButtons();
		PromptBar.Label[0].text = "Accept";
		PromptBar.Label[4].text = "Choose";
		PromptBar.UpdateButtons();
		PromptBar.Show = true;
		UpdateList();
	}

	private void UpdateList()
	{
		for (int i = 1; i < Labels.Length; i++)
		{
			UILabel uILabel = Labels[i];
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
		}
		if (StudentManager.Students[30] != null)
		{
			if (SchemeGlobals.GetSchemeStage(1) == 2)
			{
				UILabel uILabel2 = Labels[1];
				uILabel2.color = new Color(uILabel2.color.r, uILabel2.color.g, uILabel2.color.b, 1f);
			}
			if (SchemeGlobals.GetSchemeStage(2) == 3)
			{
				UILabel uILabel3 = Labels[2];
				uILabel3.color = new Color(uILabel3.color.r, uILabel3.color.g, uILabel3.color.b, 1f);
			}
			if (SchemeGlobals.GetSchemeStage(3) == 4)
			{
				UILabel uILabel4 = Labels[3];
				uILabel4.color = new Color(uILabel4.color.r, uILabel4.color.g, uILabel4.color.b, 1f);
			}
			if (SchemeGlobals.GetSchemeStage(4) == 5)
			{
				UILabel uILabel5 = Labels[4];
				uILabel5.color = new Color(uILabel5.color.r, uILabel5.color.g, uILabel5.color.b, 1f);
			}
			if (SchemeGlobals.GetSchemeStage(5) == 6)
			{
				UILabel uILabel6 = Labels[5];
				uILabel6.color = new Color(uILabel6.color.r, uILabel6.color.g, uILabel6.color.b, 1f);
			}
		}
	}

	private void UpdateHighlight()
	{
		if (Selected < 1)
		{
			Selected = 7;
		}
		else if (Selected > 7)
		{
			Selected = 1;
		}
		Highlight.transform.localPosition = new Vector3(Highlight.transform.localPosition.x, 200f - 50f * (float)Selected, Highlight.transform.localPosition.z);
	}

	private void LateUpdate()
	{
		if (!(Vector3.Distance(base.transform.position, Yandere.transform.position) < 5f))
		{
			return;
		}
		if (Angry)
		{
			BS_SadMouth = Mathf.Lerp(BS_SadMouth, 100f, Time.deltaTime * 10f);
			BS_MadBrow = Mathf.Lerp(BS_MadBrow, 100f, Time.deltaTime * 10f);
			BS_SadBrow = Mathf.Lerp(BS_SadBrow, 0f, Time.deltaTime * 10f);
			BS_AngryEyes = Mathf.Lerp(BS_AngryEyes, 100f, Time.deltaTime * 10f);
		}
		else if (Stern)
		{
			BS_SadMouth = Mathf.Lerp(BS_SadMouth, 0f, Time.deltaTime * 10f);
			BS_MadBrow = Mathf.Lerp(BS_MadBrow, 100f, Time.deltaTime * 10f);
			BS_SadBrow = Mathf.Lerp(BS_SadBrow, 0f, Time.deltaTime * 10f);
			BS_AngryEyes = Mathf.Lerp(BS_AngryEyes, 0f, Time.deltaTime * 10f);
		}
		else if (Sad)
		{
			BS_SadMouth = Mathf.Lerp(BS_SadMouth, 100f, Time.deltaTime * 10f);
			BS_MadBrow = Mathf.Lerp(BS_MadBrow, 0f, Time.deltaTime * 10f);
			BS_SadBrow = Mathf.Lerp(BS_SadBrow, 100f, Time.deltaTime * 10f);
			BS_AngryEyes = Mathf.Lerp(BS_AngryEyes, 0f, Time.deltaTime * 10f);
		}
		else
		{
			BS_SadMouth = Mathf.Lerp(BS_SadMouth, 0f, Time.deltaTime * 10f);
			BS_MadBrow = Mathf.Lerp(BS_MadBrow, 0f, Time.deltaTime * 10f);
			BS_SadBrow = Mathf.Lerp(BS_SadBrow, 0f, Time.deltaTime * 10f);
			BS_AngryEyes = Mathf.Lerp(BS_AngryEyes, 0f, Time.deltaTime * 10f);
		}
		Face.SetBlendShapeWeight(1, BS_SadMouth);
		Face.SetBlendShapeWeight(5, BS_MadBrow);
		Face.SetBlendShapeWeight(6, BS_SadBrow);
		Face.SetBlendShapeWeight(9, BS_AngryEyes);
		if (MyAudio.isPlaying)
		{
			if (InterrogationPhase != 6)
			{
				MouthTimer += Time.deltaTime;
				if (MouthTimer > TimerLimit)
				{
					MouthTarget = Random.Range(0f, 100f);
					MouthTimer = 0f;
				}
				MouthOpen = Mathf.Lerp(MouthOpen, MouthTarget, Time.deltaTime * TalkSpeed);
			}
			else
			{
				MouthOpen = Mathf.Lerp(MouthOpen, 0f, Time.deltaTime * TalkSpeed);
			}
		}
		else
		{
			MouthOpen = Mathf.Lerp(MouthOpen, 0f, Time.deltaTime * TalkSpeed);
		}
		Face.SetBlendShapeWeight(2, MouthOpen);
		LookAtTarget = Vector3.Lerp(LookAtTarget, (!LookAtPlayer) ? Default.position : Yandere.Head.position, Time.deltaTime * 2f);
		Head.LookAt(LookAtTarget);
	}

	public void Quit()
	{
		Yandere.Senpai = StudentManager.Students[1].transform;
		Yandere.DetectionPanel.alpha = 1f;
		Yandere.RPGCamera.mouseSpeed = 8f;
		Yandere.HUD.alpha = 1f;
		Yandere.HeartRate.gameObject.SetActive(true);
		Yandere.CannotRecover = false;
		Yandere.Noticed = false;
		Yandere.Talking = true;
		Yandere.ShoulderCamera.GoingToCounselor = false;
		Yandere.ShoulderCamera.HUD.SetActive(true);
		Yandere.ShoulderCamera.Noticed = false;
		Yandere.ShoulderCamera.enabled = true;
		Yandere.TargetStudent = Student;
		if (!Yandere.Jukebox.FullSanity.isPlaying)
		{
			Yandere.Jukebox.FullSanity.volume = 0f;
			Yandere.Jukebox.HalfSanity.volume = 0f;
			Yandere.Jukebox.NoSanity.volume = 0f;
			Yandere.Jukebox.FullSanity.Play();
			Yandere.Jukebox.HalfSanity.Play();
			Yandere.Jukebox.NoSanity.Play();
		}
		Yandere.transform.position = new Vector3(-21.5f, 0f, 8f);
		Yandere.transform.eulerAngles = new Vector3(0f, 90f, 0f);
		Yandere.ShoulderCamera.OverShoulder = false;
		CounselorBar.SetActive(false);
		StudentManager.EnablePrompts();
		Laptop.SetActive(true);
		LookAtPlayer = false;
		ShowWindow = false;
		Patience = 0;
		Stern = false;
		Angry = false;
		Sad = false;
		PromptBar.ClearButtons();
		PromptBar.Show = false;
		StudentManager.ComeBack();
		StudentManager.Reputation.UpdateRep();
		Physics.SyncTransforms();
	}

	private void UpdateInterrogation()
	{
		Timer += Time.deltaTime;
		if (Input.GetButtonDown("A") && InterrogationPhase != 4)
		{
			Timer += 20f;
		}
		if (InterrogationPhase == 0)
		{
			if (Timer > 1f || Input.GetButtonDown("A"))
			{
				Debug.Log("Previous Punishments: " + PlayerPrefs.GetInt("CounselorPunishments"));
				Patience -= PlayerPrefs.GetInt("CounselorPunishments");
				if (Patience < -6)
				{
					Patience = -6;
				}
				GenkaChibi.transform.localPosition = new Vector3(0f, 90 * Patience, 0f);
				Yandere.MainCamera.transform.eulerAngles = CameraTarget.eulerAngles;
				Yandere.MainCamera.transform.position = CameraTarget.position;
				Yandere.MainCamera.transform.Translate(Vector3.forward * -1f);
				if (PlayerPrefs.GetInt("CounselorVisits") < 3)
				{
					PlayerPrefs.SetInt("CounselorVisits", PlayerPrefs.GetInt("CounselorVisits") + 1);
				}
				if (PlayerPrefs.GetInt("CounselorTape") == 0)
				{
					CounselorOption[4].Label.color = new Color(0f, 0f, 0f, 0.5f);
				}
				else
				{
					CounselorOption[4].Label.color = new Color(0f, 0f, 0f, 1f);
					CounselorOption[4].Label.text = "Blame Delinquents";
				}
				if (Yandere.Subtitle.CurrentClip != null)
				{
					Object.Destroy(Yandere.Subtitle.CurrentClip);
				}
				GenkaChibi.mainTexture = AnnoyedChibi;
				CounselorBar.SetActive(true);
				Subtitle.Label.text = string.Empty;
				InterrogationPhase++;
				Time.timeScale = 1f;
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 1)
		{
			Yandere.Police.Darkness.color -= new Color(0f, 0f, 0f, Time.deltaTime);
			Yandere.MainCamera.transform.position = Vector3.Lerp(Yandere.MainCamera.transform.position, CameraTarget.position, Timer * Time.deltaTime * 0.5f);
			if (Timer > 5f || Input.GetButtonDown("A"))
			{
				Yandere.MainCamera.transform.position = CameraTarget.position;
				MyAudio.clip = GreetingClips[PlayerPrefs.GetInt("CounselorVisits")];
				CounselorSubtitle.text = Greetings[PlayerPrefs.GetInt("CounselorVisits")];
				Yandere.Police.Darkness.color = new Color(0f, 0f, 0f, 0f);
				InterrogationPhase++;
				MyAudio.Play();
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 2)
		{
			if (Input.GetButtonDown("A"))
			{
				MyAudio.Stop();
			}
			if (Timer > MyAudio.clip.length + 0.5f)
			{
				if (Crime == StudentWitnessType.Blood || Crime == StudentWitnessType.BloodAndInsanity)
				{
					MyAudio.clip = BloodLectureClips[PlayerPrefs.GetInt("BloodVisits")];
					CounselorSubtitle.text = BloodLectures[PlayerPrefs.GetInt("BloodVisits")];
					if (PlayerPrefs.GetInt("BloodVisits") < 2)
					{
						PlayerPrefs.SetInt("BloodVisits", PlayerPrefs.GetInt("BloodVisits") + 1);
					}
					CrimeID = 1;
				}
				else if (Crime == StudentWitnessType.Insanity || Crime == StudentWitnessType.CleaningItem)
				{
					MyAudio.clip = InsanityLectureClips[PlayerPrefs.GetInt("InsanityVisits")];
					CounselorSubtitle.text = InsanityLectures[PlayerPrefs.GetInt("InsanityVisits")];
					if (PlayerPrefs.GetInt("InsanityVisits") < 2)
					{
						PlayerPrefs.SetInt("InsanityVisits", PlayerPrefs.GetInt("InsanityVisits") + 1);
					}
					CrimeID = 2;
				}
				else if (Crime == StudentWitnessType.Lewd)
				{
					MyAudio.clip = LewdLectureClips[PlayerPrefs.GetInt("LewdVisits")];
					CounselorSubtitle.text = LewdLectures[PlayerPrefs.GetInt("LewdVisits")];
					if (PlayerPrefs.GetInt("LewdVisits") < 2)
					{
						PlayerPrefs.SetInt("LewdVisits", PlayerPrefs.GetInt("LewdVisits") + 1);
					}
					CrimeID = 3;
				}
				else if (Crime == StudentWitnessType.Theft || Crime == StudentWitnessType.Pickpocketing)
				{
					MyAudio.clip = TheftLectureClips[PlayerPrefs.GetInt("TheftVisits")];
					CounselorSubtitle.text = TheftLectures[PlayerPrefs.GetInt("TheftVisits")];
					if (PlayerPrefs.GetInt("TheftVisits") < 2)
					{
						PlayerPrefs.SetInt("TheftVisits", PlayerPrefs.GetInt("TheftVisits") + 1);
					}
					CrimeID = 4;
				}
				else if (Crime == StudentWitnessType.Trespassing)
				{
					MyAudio.clip = TrespassLectureClips[PlayerPrefs.GetInt("TrespassVisits")];
					CounselorSubtitle.text = TrespassLectures[PlayerPrefs.GetInt("TrespassVisits")];
					if (PlayerPrefs.GetInt("TrespassVisits") < 2)
					{
						PlayerPrefs.SetInt("TrespassVisits", PlayerPrefs.GetInt("TrespassVisits") + 1);
					}
					CrimeID = 5;
				}
				else if (Crime == StudentWitnessType.Weapon || Crime == StudentWitnessType.WeaponAndBlood || Crime == StudentWitnessType.WeaponAndInsanity || Crime == StudentWitnessType.WeaponAndBloodAndInsanity)
				{
					MyAudio.clip = WeaponLectureClips[PlayerPrefs.GetInt("WeaponVisits")];
					CounselorSubtitle.text = WeaponLectures[PlayerPrefs.GetInt("WeaponVisits")];
					if (PlayerPrefs.GetInt("WeaponVisits") < 2)
					{
						PlayerPrefs.SetInt("WeaponVisits", PlayerPrefs.GetInt("WeaponVisits") + 1);
					}
					CrimeID = 6;
				}
				InterrogationPhase++;
				MyAudio.Play();
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 3)
		{
			if (Input.GetButtonDown("A"))
			{
				MyAudio.Stop();
			}
			if (Timer > MyAudio.clip.length + 0.5f)
			{
				for (int i = 1; i < 7; i++)
				{
					CounselorOption[i].transform.localPosition = CounselorOption[i].OriginalPosition;
					CounselorOption[i].Sprite.color = CounselorOption[i].OriginalColor;
					CounselorOption[i].transform.localScale = new Vector3(0.9f, 0.9f, 1f);
					CounselorOption[i].gameObject.SetActive(true);
					CounselorOption[i].Clicked = false;
				}
				Yandere.CharacterAnimation["f02_countdown_00"].speed = 1f;
				Yandere.CharacterAnimation.Play("f02_countdown_00");
				Yandere.transform.position = new Vector3(-27.818f, 0f, 12f);
				Yandere.transform.eulerAngles = new Vector3(0f, -90f, 0f);
				Yandere.MainCamera.transform.position = new Vector3(-28f, 1.1f, 12f);
				Yandere.MainCamera.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				Reticle.transform.localPosition = new Vector3(0f, 0f, 0f);
				CounselorOptions.SetActive(true);
				CounselorBar.SetActive(false);
				CounselorSubtitle.text = string.Empty;
				MyAudio.clip = Countdown;
				MyAudio.Play();
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				InterrogationPhase++;
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 4)
		{
			Yandere.MainCamera.transform.Translate(Vector3.forward * Time.deltaTime * 0.01f);
			CounselorOptions.transform.localEulerAngles += new Vector3(0f, 0f, Time.deltaTime * -36f);
			if (InputDevice.Type == InputDeviceType.Gamepad)
			{
				Reticle.SetActive(true);
				Cursor.visible = false;
				Reticle.transform.localPosition += new Vector3(Input.GetAxis("Horizontal") * 20f, Input.GetAxis("Vertical") * 20f, 0f);
				if (Reticle.transform.localPosition.x > 975f)
				{
					Reticle.transform.localPosition = new Vector3(975f, Reticle.transform.localPosition.y, Reticle.transform.localPosition.z);
				}
				if (Reticle.transform.localPosition.x < -975f)
				{
					Reticle.transform.localPosition = new Vector3(-975f, Reticle.transform.localPosition.y, Reticle.transform.localPosition.z);
				}
				if (Reticle.transform.localPosition.y > 525f)
				{
					Reticle.transform.localPosition = new Vector3(Reticle.transform.localPosition.x, 525f, Reticle.transform.localPosition.z);
				}
				if (Reticle.transform.localPosition.y < -525f)
				{
					Reticle.transform.localPosition = new Vector3(Reticle.transform.localPosition.x, -525f, Reticle.transform.localPosition.z);
				}
			}
			else
			{
				Reticle.SetActive(false);
				Cursor.visible = true;
			}
			for (int j = 1; j < 7; j++)
			{
				CounselorOption[j].transform.eulerAngles = new Vector3(CounselorOption[j].transform.eulerAngles.x, CounselorOption[j].transform.eulerAngles.y, 0f);
				if (!CounselorOption[j].Clicked && (!(CounselorOption[j].Sprite.color != CounselorOption[j].OriginalColor) || !Input.GetButtonDown("A")))
				{
					continue;
				}
				for (int k = 1; k < 7; k++)
				{
					if (k != j)
					{
						CounselorOption[k].gameObject.SetActive(false);
					}
				}
				Yandere.CharacterAnimation["f02_countdown_00"].time = 10f;
				MyAudio.clip = Choice;
				MyAudio.pitch = 1f;
				MyAudio.Play();
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				Reticle.SetActive(false);
				InterrogationPhase++;
				Answer = j;
				Timer = 0f;
			}
			if (Timer > 10f)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				Reticle.SetActive(false);
				SilentTreatment = true;
				InterrogationPhase++;
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 5)
		{
			int l = 1;
			if (SilentTreatment)
			{
				CounselorOptions.transform.localScale += new Vector3(Time.deltaTime * 2f, Time.deltaTime * 2f, Time.deltaTime * 2f);
				for (; l < 7; l++)
				{
					CounselorOption[l].transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
				}
			}
			if (Timer > 3f || Input.GetButtonDown("A"))
			{
				CounselorOptions.transform.localScale = new Vector3(1f, 1f, 1f);
				CounselorOptions.SetActive(false);
				CounselorBar.SetActive(true);
				Yandere.transform.position = new Vector3(-27.51f, 0f, 12f);
				Yandere.MainCamera.transform.position = CameraTarget.position;
				Yandere.MainCamera.transform.eulerAngles = CameraTarget.eulerAngles;
				if (SilentTreatment)
				{
					MyAudio.clip = Silence;
					CounselorSubtitle.text = "...";
				}
				else if (Answer == 1)
				{
					MyAudio.clip = ExcuseClips[CrimeID];
					CounselorSubtitle.text = Excuses[CrimeID];
					PlayerPrefs.SetInt(string.Concat(Crime, "ExcuseUsed"), PlayerPrefs.GetInt(string.Concat(Crime, "ExcuseUsed")) + 1);
				}
				else if (Answer == 2)
				{
					MyAudio.clip = ApologyClip;
					CounselorSubtitle.text = Apology;
					PlayerPrefs.SetInt("ApologyUsed", PlayerPrefs.GetInt("ApologyUsed") + 1);
				}
				else if (Answer == 3)
				{
					MyAudio.clip = LieClips[CrimeID];
					CounselorSubtitle.text = Lies[CrimeID];
				}
				else if (Answer == 4)
				{
					MyAudio.clip = DelinquentClips[CrimeID];
					CounselorSubtitle.text = Delinquents[CrimeID];
				}
				else if (Answer == 5)
				{
					MyAudio.clip = FlirtClip;
					CounselorSubtitle.text = Flirt;
				}
				else if (Answer == 6)
				{
					MyAudio.clip = ThreatenClip;
					CounselorSubtitle.text = Threaten;
				}
				Yandere.CharacterAnimation.Play("f02_sit_00");
				InterrogationPhase++;
				MyAudio.Play();
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 6)
		{
			if (Answer == 6)
			{
				Yandere.Sanity = Mathf.MoveTowards(Yandere.Sanity, 0f, Time.deltaTime * 7.5f);
				Rumble.volume += Time.deltaTime * 0.075f;
			}
			if (Timer > MyAudio.clip.length + 0.5f || Input.GetButtonDown("A"))
			{
				if (SilentTreatment)
				{
					int num = Random.Range(0, 3);
					MyAudio.clip = SilentClips[num];
					CounselorSubtitle.text = Silents[num];
					Patience--;
				}
				else if (Answer == 1)
				{
					if (PlayerPrefs.GetInt(string.Concat(Crime, "ExcuseUsed")) == 1)
					{
						MyAudio.clip = AcceptExcuseClips[CrimeID];
						CounselorSubtitle.text = AcceptExcuses[CrimeID];
						MyAnimation.CrossFade("CounselorRelief", 1f);
						Stern = false;
						Patience = 1;
					}
					else
					{
						int num2 = Random.Range(0, 3);
						MyAudio.clip = RejectExcuseClips[num2];
						CounselorSubtitle.text = RejectExcuses[num2];
						MyAnimation.CrossFade("CounselorAnnoyed");
						Angry = true;
						Patience--;
					}
				}
				else if (Answer == 2)
				{
					if (PlayerPrefs.GetInt("ApologyUsed") == 1)
					{
						MyAudio.clip = AcceptApologyClip;
						CounselorSubtitle.text = AcceptApology;
						MyAnimation.CrossFade("CounselorRelief", 1f);
						Stern = false;
						Patience = 1;
					}
					else
					{
						int num3 = Random.Range(0, 3);
						MyAudio.clip = RejectApologyClips[num3];
						CounselorSubtitle.text = RejectApologies[num3];
						MyAnimation.CrossFade("CounselorAnnoyed");
						Patience--;
					}
				}
				else if (Answer == 3)
				{
					int num4 = Random.Range(0, 5);
					MyAudio.clip = RejectLieClips[num4];
					CounselorSubtitle.text = RejectLies[num4];
					MyAnimation.CrossFade("CounselorAnnoyed");
					Angry = true;
					Patience--;
				}
				else if (Answer == 4)
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					int num5 = 5;
					if (StudentGlobals.GetStudentDead(76) && StudentGlobals.GetStudentDead(77) && StudentGlobals.GetStudentDead(78) && StudentGlobals.GetStudentDead(79) && StudentGlobals.GetStudentDead(80))
					{
						flag3 = true;
					}
					else if (StudentGlobals.GetStudentExpelled(76) && StudentGlobals.GetStudentExpelled(77) && StudentGlobals.GetStudentExpelled(78) && StudentGlobals.GetStudentExpelled(79) && StudentGlobals.GetStudentExpelled(80))
					{
						flag2 = true;
					}
					else
					{
						if (StudentManager.Students[76] == null)
						{
							num5--;
						}
						else if (!StudentManager.Students[76].gameObject.activeInHierarchy)
						{
							num5--;
						}
						if (StudentManager.Students[76] == null)
						{
							num5--;
						}
						else if (!StudentManager.Students[76].gameObject.activeInHierarchy)
						{
							num5--;
						}
						if (StudentManager.Students[76] == null)
						{
							num5--;
						}
						else if (!StudentManager.Students[76].gameObject.activeInHierarchy)
						{
							num5--;
						}
						if (StudentManager.Students[76] == null)
						{
							num5--;
						}
						else if (!StudentManager.Students[76].gameObject.activeInHierarchy)
						{
							num5--;
						}
						if (StudentManager.Students[76] == null)
						{
							num5--;
						}
						else if (!StudentManager.Students[76].gameObject.activeInHierarchy)
						{
							num5--;
						}
						if (num5 == 0)
						{
							flag = true;
						}
					}
					if (flag3)
					{
						MyAudio.clip = DelinquentsDeadClip;
						CounselorSubtitle.text = DelinquentsDead;
						MyAnimation.CrossFade("CounselorAnnoyed");
						Angry = true;
						Patience--;
					}
					else if (flag2)
					{
						MyAudio.clip = DelinquentsExpelledClip;
						CounselorSubtitle.text = DelinquentsExpelled;
						MyAnimation.CrossFade("CounselorAnnoyed");
						Patience--;
					}
					else if (flag)
					{
						MyAudio.clip = DelinquentsGoneClip;
						CounselorSubtitle.text = DelinquentsGone;
						MyAnimation.CrossFade("CounselorAnnoyed");
						Patience--;
					}
					else if (PlayerPrefs.GetInt(string.Concat(Crime, "BlameUsed")) == 0)
					{
						if (CrimeID == 1)
						{
							Debug.Log("Banning weapons.");
							PlayerPrefs.SetInt("WeaponsBanned", 1);
						}
						MyAudio.clip = AcceptBlameClips[CrimeID];
						CounselorSubtitle.text = AcceptBlames[CrimeID];
						MyAnimation.CrossFade("CounselorSad", 1f);
						Stern = false;
						Sad = true;
						Patience = 1;
						PlayerPrefs.SetInt("DelinquentPunishments", PlayerPrefs.GetInt("DelinquentPunishments") + 1);
						PlayerPrefs.SetInt(string.Concat(Crime, "BlameUsed"), PlayerPrefs.GetInt(string.Concat(Crime, "BlameUsed")) + 1);
						if (PlayerPrefs.GetInt("DelinquentPunishments") > 5)
						{
							MustExpelDelinquents = true;
						}
					}
					else
					{
						int num6 = Random.Range(0, 3);
						MyAudio.clip = RejectBlameClips[num6];
						CounselorSubtitle.text = RejectBlames[num6];
						MyAnimation.CrossFade("CounselorAnnoyed");
						Patience--;
					}
				}
				else if (Answer == 5)
				{
					int num7 = Random.Range(0, 3);
					MyAudio.clip = RejectFlirtClips[num7];
					CounselorSubtitle.text = RejectFlirts[num7];
					MyAnimation.CrossFade("CounselorAnnoyed");
					Angry = true;
					Patience--;
				}
				else if (Answer == 6)
				{
					MyAudio.clip = RejectThreatClip;
					CounselorSubtitle.text = RejectThreat;
					MyAnimation.CrossFade("CounselorAnnoyed");
					InterrogationPhase += 2;
					Patience = -6;
					Angry = true;
				}
				if (Patience < -6)
				{
					Patience = -6;
				}
				if (Patience == 1)
				{
					GenkaChibi.mainTexture = HappyChibi;
				}
				else if (Patience == -6)
				{
					GenkaChibi.mainTexture = MadChibi;
				}
				else
				{
					GenkaChibi.mainTexture = AnnoyedChibi;
				}
				InterrogationPhase++;
				MyAudio.Play();
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 7)
		{
			if (Timer > MyAudio.clip.length + 0.5f || Input.GetButtonDown("A"))
			{
				if (Patience < 0)
				{
					int num8 = Random.Range(0, 3);
					MyAudio.clip = BadClosingClips[num8];
					CounselorSubtitle.text = BadClosings[num8];
					MyAnimation.CrossFade("CounselorArmsCrossed", 1f);
					InterrogationPhase += 2;
				}
				else
				{
					if (MustExpelDelinquents)
					{
						MyAudio.clip = ExpelDelinquentsClip;
						CounselorSubtitle.text = ExpelDelinquents;
						MustExpelDelinquents = false;
						StudentGlobals.SetStudentExpelled(76, true);
						StudentGlobals.SetStudentExpelled(77, true);
						StudentGlobals.SetStudentExpelled(78, true);
						StudentGlobals.SetStudentExpelled(79, true);
						StudentGlobals.SetStudentExpelled(80, true);
						ExpelledDelinquents = true;
					}
					else if (Answer == 4)
					{
						MyAudio.clip = BlameClosingClips[CrimeID];
						CounselorSubtitle.text = BlameClosings[CrimeID];
					}
					else
					{
						int num9 = Random.Range(0, 3);
						MyAudio.clip = FreeToLeaveClips[num9];
						CounselorSubtitle.text = FreeToLeaves[num9];
						MyAnimation.CrossFade("CounselorArmsCrossed", 1f);
						Stern = true;
					}
					InterrogationPhase++;
				}
				MyAudio.Play();
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 8)
		{
			if (Timer > MyAudio.clip.length + 0.5f || Input.GetButtonDown("A"))
			{
				CounselorDoor.FadeOut = true;
				CounselorDoor.Exit = true;
				Interrogating = false;
				InterrogationPhase = 0;
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 9)
		{
			if (Timer > MyAudio.clip.length + 0.5f || Input.GetButtonDown("A"))
			{
				MyAnimation.Play("CounselorSlamDesk");
				InterrogationPhase++;
				MyAudio.Stop();
				Stern = false;
				Angry = true;
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 10)
		{
			if (Timer > 0.5f)
			{
				if (!Slammed)
				{
					AudioSource.PlayClipAtPoint(Slam, base.transform.position);
					Shake.shakeAmount = 0.1f;
					Shake.enabled = true;
					Shake.shake = 0.5f;
					Slammed = true;
				}
				Shake.shakeAmount = Mathf.Lerp(Shake.shakeAmount, 0f, Time.deltaTime);
			}
			Shake.shakeAmount = Mathf.Lerp(Shake.shakeAmount, 0f, Time.deltaTime * 10f);
			if (Timer > 1.5f || Input.GetButtonDown("A"))
			{
				MyAudio.clip = SuspensionClips[Mathf.Abs(Patience)];
				CounselorSubtitle.text = Suspensions[Mathf.Abs(Patience)];
				MyAnimation.Play("CounselorSlamIdle");
				Shake.enabled = false;
				InterrogationPhase++;
				MyAudio.Play();
				Timer = 0f;
			}
		}
		else if (InterrogationPhase == 11 && (Timer > MyAudio.clip.length + 0.5f || Input.GetButtonDown("A")) && !Yandere.Police.FadeOut)
		{
			PlayerPrefs.SetInt("CounselorPunishments", PlayerPrefs.GetInt("CounselorPunishments") + 1);
			Yandere.Police.Darkness.color = new Color(0f, 0f, 0f, 0f);
			Yandere.Police.SuspensionLength = Mathf.Abs(Patience);
			Yandere.Police.Darkness.enabled = true;
			Yandere.Police.ClubActivity = false;
			Yandere.Police.Suspended = true;
			Yandere.Police.FadeOut = true;
			Yandere.ShoulderCamera.HUD.SetActive(true);
			InterrogationPhase++;
			Expelled = true;
			Timer = 0f;
			Yandere.Senpai = StudentManager.Students[1].transform;
			StudentManager.Reputation.PendingRep -= 10f;
			StudentManager.Reputation.UpdateRep();
		}
		if (InterrogationPhase > 6)
		{
			Yandere.Sanity = Mathf.Lerp(Yandere.Sanity, 100f, Time.deltaTime);
			Rumble.volume = Mathf.Lerp(Rumble.volume, 0f, Time.deltaTime);
			GenkaChibi.transform.localPosition = Vector3.Lerp(GenkaChibi.transform.localPosition, new Vector3(0f, 90 * Patience, 0f), Time.deltaTime * 10f);
		}
	}
}
