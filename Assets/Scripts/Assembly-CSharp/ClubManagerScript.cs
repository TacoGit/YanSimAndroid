using UnityEngine;

public class ClubManagerScript : MonoBehaviour
{
	public ShoulderCameraScript ShoulderCamera;

	public StudentManagerScript StudentManager;

	public ComputerGamesScript ComputerGames;

	public BloodCleanerScript BloodCleaner;

	public RefrigeratorScript Refrigerator;

	public ClubWindowScript ClubWindow;

	public ContainerScript Container;

	public PromptBarScript PromptBar;

	public TranqCaseScript TranqCase;

	public YandereScript Yandere;

	public RPG_Camera MainCamera;

	public DoorScript ShedDoor;

	public PoliceScript Police;

	public GloveScript Gloves;

	public UISprite Darkness;

	public GameObject Reputation;

	public GameObject Heartrate;

	public GameObject Watermark;

	public GameObject Padlock;

	public GameObject Ritual;

	public GameObject Clock;

	public GameObject Cake;

	public AudioClip[] MotivationalQuotes;

	public Transform[] ClubPatrolPoints;

	public GameObject[] ClubPosters;

	public GameObject[] GameScreens;

	public Transform[] ClubVantages;

	public MaskScript[] Masks;

	public GameObject[] Cultists;

	public Transform[] Club1ActivitySpots;

	public Transform[] Club4ActivitySpots;

	public Transform[] Club6ActivitySpots;

	public Transform Club7ActivitySpot;

	public Transform[] Club8ActivitySpots;

	public Transform[] Club10ActivitySpots;

	public int[] Club1Students;

	public int[] Club2Students;

	public int[] Club3Students;

	public int[] Club4Students;

	public int[] Club5Students;

	public int[] Club6Students;

	public int[] Club7Students;

	public int[] Club8Students;

	public int[] Club9Students;

	public int[] Club10Students;

	public int[] Club11Students;

	public bool LeaderAshamed;

	public bool ClubEffect;

	public AudioClip OccultAmbience;

	public int ClubPhase;

	public int Phase = 1;

	public ClubType Club;

	public int ID;

	public float TimeLimit;

	public float Timer;

	public ClubType[] ClubArray;

	public bool LeaderMissing;

	public bool LeaderDead;

	public int ClubMembers;

	public int[] Club1IDs;

	public int[] Club2IDs;

	public int[] Club3IDs;

	public int[] Club4IDs;

	public int[] Club5IDs;

	public int[] Club6IDs;

	public int[] Club7IDs;

	public int[] Club8IDs;

	public int[] Club9IDs;

	public int[] Club10IDs;

	public int[] Club11IDs;

	public int[] ClubIDs;

	public bool LeaderGrudge;

	public bool ClubGrudge;

	private void Start()
	{
		ClubWindow.ActivityWindow.localScale = Vector3.zero;
		ClubWindow.ActivityWindow.gameObject.SetActive(false);
		ActivateClubBenefit();
		int num = 0;
		for (ID = 1; ID < ClubArray.Length; ID++)
		{
			if (ClubGlobals.GetClubClosed(ClubArray[ID]))
			{
				ClubPosters[ID].SetActive(false);
				if (ClubArray[ID] == ClubType.Gardening)
				{
					ClubPatrolPoints[ID].transform.position = new Vector3(-36f, ClubPatrolPoints[ID].transform.position.y, ClubPatrolPoints[ID].transform.position.z);
				}
				else if (ClubArray[ID] == ClubType.Gaming)
				{
					ClubPatrolPoints[ID].transform.position = new Vector3(20f, ClubPatrolPoints[ID].transform.position.y, ClubPatrolPoints[ID].transform.position.z);
				}
				else if (ClubArray[ID] != ClubType.Sports)
				{
					ClubPatrolPoints[ID].transform.position = new Vector3(ClubPatrolPoints[ID].transform.position.x, ClubPatrolPoints[ID].transform.position.y, 20f);
				}
				num++;
			}
		}
		if (num > 10)
		{
			StudentManager.NoClubMeeting = true;
		}
		if (ClubGlobals.GetClubClosed(ClubArray[2]))
		{
			StudentManager.HidingSpots.List[56] = StudentManager.Hangouts.List[56];
			StudentManager.HidingSpots.List[57] = StudentManager.Hangouts.List[57];
			StudentManager.HidingSpots.List[58] = StudentManager.Hangouts.List[58];
			StudentManager.HidingSpots.List[59] = StudentManager.Hangouts.List[59];
			StudentManager.HidingSpots.List[60] = StudentManager.Hangouts.List[60];
			StudentManager.SleuthPhase = 3;
		}
		ID = 0;
	}

	private void Update()
	{
		if (Club == ClubType.None)
		{
			return;
		}
		if (Phase == 1)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
		}
		AudioSource component = GetComponent<AudioSource>();
		if (Darkness.color.a == 0f)
		{
			if (Phase == 1)
			{
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Continue";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
				ClubWindow.PerformingActivity = true;
				ClubWindow.ActivityWindow.gameObject.SetActive(true);
				ClubWindow.ActivityLabel.text = ClubWindow.ActivityDescs[(int)Club];
				Phase++;
			}
			else if (Phase == 2)
			{
				if (ClubWindow.ActivityWindow.localScale.x > 0.9f)
				{
					if (Club == ClubType.MartialArts)
					{
						if (ClubPhase == 0)
						{
							component.clip = MotivationalQuotes[Random.Range(0, MotivationalQuotes.Length)];
							component.Play();
							ClubEffect = true;
							ClubPhase++;
							TimeLimit = component.clip.length;
						}
						else if (ClubPhase == 1)
						{
							Timer += Time.deltaTime;
							if (Timer > TimeLimit)
							{
								for (ID = 0; ID < Club6Students.Length; ID++)
								{
									if (StudentManager.Students[ID] != null && !StudentManager.Students[ID].Tranquil)
									{
										StudentManager.Students[Club6Students[ID]].GetComponent<AudioSource>().volume = 1f;
									}
								}
								ClubPhase++;
							}
						}
					}
					if (Input.GetButtonDown("A"))
					{
						ClubWindow.PerformingActivity = false;
						PromptBar.Show = false;
						Phase++;
					}
				}
			}
			else if (ClubWindow.ActivityWindow.localScale.x < 0.1f)
			{
				Police.Darkness.enabled = true;
				Police.ClubActivity = false;
				Police.FadeOut = true;
			}
		}
		if (Club == ClubType.Occult)
		{
			component.volume = 1f - Darkness.color.a;
		}
	}

	public void ClubActivity()
	{
		StudentManager.StopMoving();
		ShoulderCamera.enabled = false;
		MainCamera.enabled = false;
		MainCamera.transform.position = ClubVantages[(int)Club].position;
		MainCamera.transform.rotation = ClubVantages[(int)Club].rotation;
		if (Club == ClubType.Cooking)
		{
			Cake.SetActive(true);
			for (ID = 0; ID < Club1Students.Length; ID++)
			{
				StudentScript studentScript = StudentManager.Students[Club1Students[ID]];
				if (studentScript != null && !studentScript.Tranquil && studentScript.Alive)
				{
					studentScript.transform.position = Club1ActivitySpots[ID].position;
					studentScript.transform.rotation = Club1ActivitySpots[ID].rotation;
					studentScript.CharacterAnimation[studentScript.SocialSitAnim].layer = 99;
					studentScript.CharacterAnimation.Play(studentScript.SocialSitAnim);
					studentScript.CharacterAnimation[studentScript.SocialSitAnim].weight = 1f;
					studentScript.SmartPhone.SetActive(false);
					studentScript.SpeechLines.Play();
					studentScript.ClubActivity = true;
					studentScript.Talking = false;
					studentScript.Routine = false;
					studentScript.GetComponent<AudioSource>().volume = 0.1f;
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			Yandere.CharacterAnimation.Play("f02_sit_00");
			Yandere.transform.position = Club1ActivitySpots[6].position;
			Yandere.transform.rotation = Club1ActivitySpots[6].rotation;
		}
		else if (Club == ClubType.Drama)
		{
			for (ID = 0; ID < Club2Students.Length; ID++)
			{
				StudentManager.DramaPhase = 1;
				StudentManager.UpdateDrama();
				StudentScript studentScript2 = StudentManager.Students[Club2Students[ID]];
				if (studentScript2 != null && !studentScript2.Tranquil && studentScript2.Alive)
				{
					if (!StudentManager.MemorialScene.gameObject.activeInHierarchy)
					{
						studentScript2.transform.position = studentScript2.CurrentDestination.position;
						studentScript2.transform.rotation = studentScript2.CurrentDestination.rotation;
					}
					else
					{
						studentScript2.transform.position = new Vector3(0f, 0f, 0f);
					}
					studentScript2.ClubActivity = true;
					studentScript2.Talking = false;
					studentScript2.Routine = true;
					studentScript2.GetComponent<AudioSource>().volume = 0.1f;
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			if (!StudentManager.MemorialScene.gameObject.activeInHierarchy)
			{
				Yandere.transform.position = new Vector3(42f, 1.3775f, 72f);
				Yandere.transform.eulerAngles = new Vector3(0f, -90f, 0f);
			}
		}
		else if (Club == ClubType.Occult)
		{
			for (ID = 0; ID < Club3Students.Length; ID++)
			{
				StudentScript studentScript3 = StudentManager.Students[Club3Students[ID]];
				if (studentScript3 != null && !studentScript3.Tranquil)
				{
					studentScript3.gameObject.SetActive(false);
				}
			}
			MainCamera.GetComponent<AudioListener>().enabled = true;
			AudioSource component = GetComponent<AudioSource>();
			component.clip = OccultAmbience;
			component.loop = true;
			component.volume = 0f;
			component.Play();
			Yandere.gameObject.SetActive(false);
			Ritual.SetActive(true);
			CheckClub(ClubType.Occult);
			GameObject[] cultists = Cultists;
			foreach (GameObject gameObject in cultists)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			while (ClubMembers > 0)
			{
				Cultists[ClubMembers].SetActive(true);
				ClubMembers--;
			}
			CheckClub(ClubType.Occult);
		}
		else if (Club == ClubType.Art)
		{
			for (ID = 0; ID < Club4Students.Length; ID++)
			{
				StudentScript studentScript4 = StudentManager.Students[Club4Students[ID]];
				if (studentScript4 != null && !studentScript4.Tranquil && studentScript4.Alive)
				{
					studentScript4.transform.position = Club4ActivitySpots[ID].position;
					studentScript4.transform.rotation = Club4ActivitySpots[ID].rotation;
					studentScript4.ClubActivity = true;
					studentScript4.Talking = false;
					studentScript4.Routine = true;
					if (!studentScript4.ClubAttire)
					{
						studentScript4.ChangeClubwear();
					}
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			Yandere.transform.position = Club4ActivitySpots[5].position;
			Yandere.transform.rotation = Club4ActivitySpots[5].rotation;
			if (!Yandere.ClubAttire)
			{
				Yandere.ChangeClubwear();
			}
		}
		else if (Club == ClubType.LightMusic)
		{
			for (ID = 0; ID < Club5Students.Length; ID++)
			{
				StudentScript studentScript5 = StudentManager.Students[Club5Students[ID]];
				if (studentScript5 != null && !studentScript5.Tranquil && studentScript5.Alive)
				{
					studentScript5.transform.position = studentScript5.CurrentDestination.position;
					studentScript5.transform.rotation = studentScript5.CurrentDestination.rotation;
					studentScript5.ClubActivity = false;
					studentScript5.Talking = false;
					studentScript5.Routine = true;
					studentScript5.Stop = false;
				}
			}
		}
		else if (Club == ClubType.MartialArts)
		{
			for (ID = 0; ID < Club6Students.Length; ID++)
			{
				StudentScript studentScript6 = StudentManager.Students[Club6Students[ID]];
				if (studentScript6 != null && !studentScript6.Tranquil && studentScript6.Alive)
				{
					studentScript6.transform.position = Club6ActivitySpots[ID].position;
					studentScript6.transform.rotation = Club6ActivitySpots[ID].rotation;
					studentScript6.ClubActivity = true;
					studentScript6.GetComponent<AudioSource>().volume = 0.1f;
					if (!studentScript6.ClubAttire)
					{
						studentScript6.ChangeClubwear();
					}
				}
			}
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			Yandere.transform.position = Club6ActivitySpots[5].position;
			Yandere.transform.rotation = Club6ActivitySpots[5].rotation;
			if (!Yandere.ClubAttire)
			{
				Yandere.ChangeClubwear();
			}
		}
		else if (Club == ClubType.Photography)
		{
			for (ID = 0; ID < Club7Students.Length; ID++)
			{
				StudentScript studentScript7 = StudentManager.Students[Club7Students[ID]];
				if (studentScript7 != null && !studentScript7.Tranquil && studentScript7.Alive)
				{
					studentScript7.transform.position = StudentManager.Clubs.List[studentScript7.StudentID].position;
					studentScript7.transform.rotation = StudentManager.Clubs.List[studentScript7.StudentID].rotation;
					studentScript7.CharacterAnimation[studentScript7.SocialSitAnim].weight = 1f;
					studentScript7.SmartPhone.SetActive(false);
					studentScript7.ClubActivity = true;
					studentScript7.SpeechLines.Play();
					studentScript7.Talking = false;
					studentScript7.Routine = true;
					studentScript7.Hearts.Stop();
				}
			}
			Yandere.CanMove = false;
			Yandere.Talking = false;
			Yandere.ClubActivity = true;
			Yandere.transform.position = Club7ActivitySpot.position;
			Yandere.transform.rotation = Club7ActivitySpot.rotation;
			if (!Yandere.ClubAttire)
			{
				Yandere.ChangeClubwear();
			}
		}
		else if (Club == ClubType.Science)
		{
			for (ID = 0; ID < Club8Students.Length; ID++)
			{
				StudentScript studentScript8 = StudentManager.Students[Club8Students[ID]];
				if (studentScript8 != null && !studentScript8.Tranquil && studentScript8.Alive)
				{
					studentScript8.transform.position = Club8ActivitySpots[ID].position;
					studentScript8.transform.rotation = Club8ActivitySpots[ID].rotation;
					studentScript8.ClubActivity = true;
					studentScript8.Talking = false;
					studentScript8.Routine = true;
					if (!studentScript8.ClubAttire)
					{
						studentScript8.ChangeClubwear();
					}
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			if (!Yandere.ClubAttire)
			{
				Yandere.ChangeClubwear();
			}
		}
		else if (Club == ClubType.Sports)
		{
			for (ID = 0; ID < Club9Students.Length; ID++)
			{
				StudentScript studentScript9 = StudentManager.Students[Club9Students[ID]];
				if (studentScript9 != null && !studentScript9.Tranquil && studentScript9.Alive)
				{
					studentScript9.transform.position = studentScript9.CurrentDestination.position;
					studentScript9.transform.rotation = studentScript9.CurrentDestination.rotation;
					studentScript9.ClubActivity = true;
					studentScript9.Talking = false;
					studentScript9.Routine = true;
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			Yandere.Schoolwear = 2;
			Yandere.ChangeSchoolwear();
		}
		else if (Club == ClubType.Gardening)
		{
			for (ID = 0; ID < Club10Students.Length; ID++)
			{
				StudentScript studentScript10 = StudentManager.Students[Club10Students[ID]];
				if (studentScript10 != null && !studentScript10.Tranquil && studentScript10.Alive)
				{
					studentScript10.transform.position = studentScript10.CurrentDestination.position;
					studentScript10.transform.rotation = studentScript10.CurrentDestination.rotation;
					studentScript10.ClubActivity = true;
					studentScript10.Talking = false;
					studentScript10.Routine = true;
					studentScript10.GetComponent<AudioSource>().volume = 0.1f;
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			if (!Yandere.ClubAttire)
			{
				Yandere.ChangeClubwear();
			}
		}
		else if (Club == ClubType.Gaming)
		{
			for (ID = 0; ID < Club11Students.Length; ID++)
			{
				StudentScript studentScript11 = StudentManager.Students[Club11Students[ID]];
				if (studentScript11 != null && !studentScript11.Tranquil && studentScript11.Alive)
				{
					studentScript11.transform.position = studentScript11.CurrentDestination.position;
					studentScript11.transform.rotation = studentScript11.CurrentDestination.rotation;
					studentScript11.ClubManager.GameScreens[ID].SetActive(true);
					studentScript11.SmartPhone.SetActive(false);
					studentScript11.ClubActivity = true;
					studentScript11.Talking = false;
					studentScript11.Routine = false;
					studentScript11.GetComponent<AudioSource>().volume = 0.1f;
				}
			}
			Yandere.Talking = false;
			Yandere.CanMove = false;
			Yandere.ClubActivity = true;
			Yandere.transform.position = StudentManager.ComputerGames.Chairs[1].transform.position;
			Yandere.transform.rotation = StudentManager.ComputerGames.Chairs[1].transform.rotation;
		}
		Clock.SetActive(false);
		Reputation.SetActive(false);
		Heartrate.SetActive(false);
		Watermark.SetActive(false);
	}

	public void CheckClub(ClubType Check)
	{
		switch (Check)
		{
		case ClubType.Cooking:
			ClubIDs = Club1IDs;
			break;
		case ClubType.Drama:
			ClubIDs = Club2IDs;
			break;
		case ClubType.Occult:
			ClubIDs = Club3IDs;
			break;
		case ClubType.Art:
			ClubIDs = Club4IDs;
			break;
		case ClubType.LightMusic:
			ClubIDs = Club5IDs;
			break;
		case ClubType.MartialArts:
			ClubIDs = Club6IDs;
			break;
		case ClubType.Photography:
			ClubIDs = Club7IDs;
			break;
		case ClubType.Science:
			ClubIDs = Club8IDs;
			break;
		case ClubType.Sports:
			ClubIDs = Club9IDs;
			break;
		case ClubType.Gardening:
			ClubIDs = Club10IDs;
			break;
		case ClubType.Gaming:
			ClubIDs = Club11IDs;
			break;
		}
		LeaderMissing = false;
		LeaderDead = false;
		ClubMembers = 0;
		for (ID = 1; ID < ClubIDs.Length; ID++)
		{
			if (!StudentGlobals.GetStudentDead(ClubIDs[ID]) && !StudentGlobals.GetStudentDying(ClubIDs[ID]) && !StudentGlobals.GetStudentKidnapped(ClubIDs[ID]) && !StudentGlobals.GetStudentArrested(ClubIDs[ID]) && !StudentGlobals.GetStudentExpelled(ClubIDs[ID]) && StudentGlobals.GetStudentReputation(ClubIDs[ID]) > -100)
			{
				ClubMembers++;
			}
		}
		if (TranqCase.VictimClubType == Check)
		{
			ClubMembers--;
		}
		if (Check == ClubType.LightMusic && ClubMembers < 5)
		{
			LeaderAshamed = true;
		}
		if (ClubGlobals.Club == Check)
		{
			ClubMembers++;
		}
		switch (Check)
		{
		case ClubType.Cooking:
		{
			int num3 = 21;
			if (StudentGlobals.GetStudentDead(num3) || StudentGlobals.GetStudentDying(num3) || StudentGlobals.GetStudentArrested(num3) || StudentGlobals.GetStudentReputation(num3) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num3) || StudentGlobals.GetStudentKidnapped(num3) || TranqCase.VictimID == num3)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Drama:
		{
			int num4 = 26;
			if (StudentGlobals.GetStudentDead(num4) || StudentGlobals.GetStudentDying(num4) || StudentGlobals.GetStudentArrested(num4) || StudentGlobals.GetStudentReputation(num4) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num4) || StudentGlobals.GetStudentKidnapped(num4) || TranqCase.VictimID == num4)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Occult:
		{
			int num9 = 31;
			if (StudentGlobals.GetStudentDead(num9) || StudentGlobals.GetStudentDying(num9) || StudentGlobals.GetStudentArrested(num9) || StudentGlobals.GetStudentReputation(num9) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num9) || StudentGlobals.GetStudentKidnapped(num9) || TranqCase.VictimID == num9)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Gaming:
		{
			int num7 = 36;
			if (StudentGlobals.GetStudentDead(num7) || StudentGlobals.GetStudentDying(num7) || StudentGlobals.GetStudentArrested(num7) || StudentGlobals.GetStudentReputation(num7) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num7) || StudentGlobals.GetStudentKidnapped(num7) || TranqCase.VictimID == num7)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Art:
		{
			int num2 = 41;
			if (StudentGlobals.GetStudentDead(num2) || StudentGlobals.GetStudentDying(num2) || StudentGlobals.GetStudentArrested(num2) || StudentGlobals.GetStudentReputation(num2) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num2) || StudentGlobals.GetStudentKidnapped(num2) || TranqCase.VictimID == num2)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.MartialArts:
		{
			int num6 = 46;
			if (StudentGlobals.GetStudentDead(num6) || StudentGlobals.GetStudentDying(num6) || StudentGlobals.GetStudentArrested(num6) || StudentGlobals.GetStudentReputation(num6) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num6) || StudentGlobals.GetStudentKidnapped(num6) || TranqCase.VictimID == num6)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.LightMusic:
		{
			int num11 = 51;
			if (StudentGlobals.GetStudentDead(num11) || StudentGlobals.GetStudentDying(num11) || StudentGlobals.GetStudentArrested(num11) || StudentGlobals.GetStudentReputation(num11) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num11) || StudentGlobals.GetStudentKidnapped(num11) || TranqCase.VictimID == num11)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Photography:
		{
			int num5 = 56;
			if (StudentGlobals.GetStudentDead(num5) || StudentGlobals.GetStudentDying(num5) || StudentGlobals.GetStudentArrested(num5) || StudentGlobals.GetStudentReputation(num5) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num5) || StudentGlobals.GetStudentKidnapped(num5) || TranqCase.VictimID == num5)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Science:
		{
			int num8 = 61;
			if (StudentGlobals.GetStudentDead(num8) || StudentGlobals.GetStudentDying(num8) || StudentGlobals.GetStudentArrested(num8) || StudentGlobals.GetStudentReputation(num8) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num8) || StudentGlobals.GetStudentKidnapped(num8) || TranqCase.VictimID == num8)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Sports:
		{
			int num10 = 66;
			if (StudentGlobals.GetStudentDead(num10) || StudentGlobals.GetStudentDying(num10) || StudentGlobals.GetStudentArrested(num10) || StudentGlobals.GetStudentReputation(num10) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num10) || StudentGlobals.GetStudentKidnapped(num10) || TranqCase.VictimID == num10)
			{
				LeaderMissing = true;
			}
			break;
		}
		case ClubType.Gardening:
		{
			int num = 71;
			if (StudentGlobals.GetStudentDead(num) || StudentGlobals.GetStudentDying(num) || StudentGlobals.GetStudentArrested(num) || StudentGlobals.GetStudentReputation(num) <= -100)
			{
				LeaderDead = true;
			}
			if (StudentGlobals.GetStudentMissing(num) || StudentGlobals.GetStudentKidnapped(num) || TranqCase.VictimID == num)
			{
				LeaderMissing = true;
			}
			break;
		}
		}
		if (!LeaderDead && !LeaderMissing && Check == ClubType.LightMusic && (double)StudentGlobals.GetStudentReputation(51) < -33.33333)
		{
			LeaderAshamed = true;
		}
	}

	public void CheckGrudge(ClubType Check)
	{
		switch (Check)
		{
		case ClubType.Cooking:
			ClubIDs = Club1IDs;
			break;
		case ClubType.Drama:
			ClubIDs = Club2IDs;
			break;
		case ClubType.Occult:
			ClubIDs = Club3IDs;
			break;
		case ClubType.LightMusic:
			ClubIDs = Club5IDs;
			break;
		case ClubType.MartialArts:
			ClubIDs = Club6IDs;
			break;
		case ClubType.Photography:
			ClubIDs = Club7IDs;
			break;
		case ClubType.Science:
			ClubIDs = Club8IDs;
			break;
		case ClubType.Sports:
			ClubIDs = Club9IDs;
			break;
		case ClubType.Gardening:
			ClubIDs = Club10IDs;
			break;
		case ClubType.Gaming:
			ClubIDs = Club11IDs;
			break;
		}
		LeaderGrudge = false;
		ClubGrudge = false;
		for (ID = 1; ID < ClubIDs.Length; ID++)
		{
			if (StudentManager.Students[ClubIDs[ID]] != null && StudentGlobals.GetStudentGrudge(ClubIDs[ID]))
			{
				ClubGrudge = true;
			}
		}
		switch (Check)
		{
		case ClubType.Cooking:
			if (StudentManager.Students[21].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Drama:
			if (StudentManager.Students[26].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Occult:
			if (StudentManager.Students[31].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Art:
			if (StudentManager.Students[41].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.MartialArts:
			if (StudentManager.Students[46].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.LightMusic:
			if (StudentManager.Students[51].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Photography:
			if (StudentManager.Students[56].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Science:
			if (StudentManager.Students[61].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Sports:
			if (StudentManager.Students[66].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Gardening:
			if (StudentManager.Students[71].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		case ClubType.Gaming:
			if (StudentManager.Students[36].Grudge)
			{
				LeaderGrudge = true;
			}
			break;
		}
	}

	public void ActivateClubBenefit()
	{
		if (ClubGlobals.Club == ClubType.Cooking)
		{
			if (!Refrigerator.CookingEvent.EventActive)
			{
				Refrigerator.enabled = true;
				Refrigerator.Prompt.enabled = true;
			}
		}
		else if (ClubGlobals.Club == ClubType.Drama)
		{
			for (ID = 1; ID < Masks.Length; ID++)
			{
				Masks[ID].enabled = true;
				Masks[ID].Prompt.enabled = true;
			}
			Gloves.enabled = true;
			Gloves.Prompt.enabled = true;
		}
		else if (ClubGlobals.Club == ClubType.Occult)
		{
			StudentManager.UpdatePerception();
			Yandere.Numbness -= 0.5f;
		}
		else if (ClubGlobals.Club == ClubType.Art)
		{
			StudentManager.UpdateBooths();
		}
		else if (ClubGlobals.Club == ClubType.LightMusic)
		{
			Container.enabled = true;
			Container.Prompt.enabled = true;
		}
		else if (ClubGlobals.Club == ClubType.MartialArts)
		{
			StudentManager.UpdateBooths();
		}
		else
		{
			if (ClubGlobals.Club == ClubType.Photography)
			{
				return;
			}
			if (ClubGlobals.Club == ClubType.Science)
			{
				BloodCleaner.Prompt.enabled = true;
				StudentManager.UpdateBooths();
			}
			else if (ClubGlobals.Club == ClubType.Sports)
			{
				Yandere.RunSpeed += 1f;
				if (Yandere.Armed)
				{
					Yandere.EquippedWeapon.SuspicionCheck();
				}
			}
			else if (ClubGlobals.Club == ClubType.Gardening)
			{
				ShedDoor.Prompt.Label[0].text = "     Open";
				Padlock.SetActive(false);
				ShedDoor.Locked = false;
				if (Yandere.Armed)
				{
					Yandere.EquippedWeapon.SuspicionCheck();
				}
			}
			else if (ClubGlobals.Club == ClubType.Gaming)
			{
				ComputerGames.EnableGames();
			}
		}
	}

	public void DeactivateClubBenefit()
	{
		if (ClubGlobals.Club == ClubType.Cooking)
		{
			Refrigerator.enabled = false;
			Refrigerator.Prompt.Hide();
			Refrigerator.Prompt.enabled = false;
		}
		else if (ClubGlobals.Club == ClubType.Drama)
		{
			for (ID = 1; ID < Masks.Length; ID++)
			{
				if (Masks[ID] != null)
				{
					Masks[ID].enabled = false;
					Masks[ID].Prompt.Hide();
					Masks[ID].Prompt.enabled = false;
				}
			}
			Gloves.enabled = false;
			Gloves.Prompt.Hide();
			Gloves.Prompt.enabled = false;
		}
		else if (ClubGlobals.Club == ClubType.Occult)
		{
			ClubGlobals.Club = ClubType.None;
			StudentManager.UpdatePerception();
			Yandere.Numbness += 0.5f;
		}
		else
		{
			if (ClubGlobals.Club == ClubType.Art)
			{
				return;
			}
			if (ClubGlobals.Club == ClubType.LightMusic)
			{
				Container.enabled = false;
				Container.Prompt.Hide();
				Container.Prompt.enabled = false;
			}
			else
			{
				if (ClubGlobals.Club == ClubType.MartialArts || ClubGlobals.Club == ClubType.Photography)
				{
					return;
				}
				if (ClubGlobals.Club == ClubType.Science)
				{
					BloodCleaner.enabled = false;
					BloodCleaner.Prompt.Hide();
					BloodCleaner.Prompt.enabled = false;
				}
				else if (ClubGlobals.Club == ClubType.Sports)
				{
					Yandere.RunSpeed -= 1f;
					if (Yandere.Armed)
					{
						ClubGlobals.Club = ClubType.None;
						Yandere.EquippedWeapon.SuspicionCheck();
					}
				}
				else if (ClubGlobals.Club == ClubType.Gardening)
				{
					if (!Yandere.Inventory.ShedKey)
					{
						ShedDoor.Prompt.Label[0].text = "     Locked";
						Padlock.SetActive(true);
						ShedDoor.Locked = true;
						ShedDoor.CloseDoor();
					}
					if (Yandere.Armed)
					{
						ClubGlobals.Club = ClubType.None;
						Yandere.EquippedWeapon.SuspicionCheck();
					}
				}
				else if (ClubGlobals.Club == ClubType.Gaming)
				{
					ComputerGames.DeactivateAllBenefits();
					ComputerGames.DisableGames();
				}
			}
		}
	}

	public void UpdateMasks()
	{
		bool flag = Yandere.Mask != null;
		for (ID = 1; ID < Masks.Length; ID++)
		{
			Masks[ID].Prompt.HideButton[0] = flag;
		}
	}
}
