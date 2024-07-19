using UnityEngine;

public class StudentInfoScript : MonoBehaviour
{
	public StudentInfoMenuScript StudentInfoMenu;

	public StudentManagerScript StudentManager;

	public DialogueWheelScript DialogueWheel;

	public HomeInternetScript HomeInternet;

	public TopicManagerScript TopicManager;

	public NoteLockerScript NoteLocker;

	public PromptBarScript PromptBar;

	public ShutterScript Shutter;

	public YandereScript Yandere;

	public JsonScript JSON;

	public Texture GuidanceCounselor;

	public Texture DefaultPortrait;

	public Texture BlankPortrait;

	public Texture Headmaster;

	public Texture InfoChan;

	public Transform ReputationBar;

	public GameObject Static;

	public GameObject Topics;

	public UILabel OccupationLabel;

	public UILabel ReputationLabel;

	public UILabel StrengthLabel;

	public UILabel PersonaLabel;

	public UILabel ClassLabel;

	public UILabel CrushLabel;

	public UILabel ClubLabel;

	public UILabel InfoLabel;

	public UILabel NameLabel;

	public UITexture Portrait;

	public string[] OpinionSpriteNames;

	public string[] Strings;

	public int CurrentStudent;

	public bool Back;

	public UISprite[] TopicIcons;

	public UISprite[] TopicOpinionIcons;

	private static readonly IntAndStringDictionary StrengthStrings = new IntAndStringDictionary
	{
		{ 0, "Incapable" },
		{ 1, "Very Weak" },
		{ 2, "Weak" },
		{ 3, "Strong" },
		{ 4, "Very Strong" },
		{ 5, "Peak Physical Strength" },
		{ 6, "Extensive Training" },
		{ 7, "Carries Pepper Spray" },
		{ 8, "Armed" },
		{ 9, "Invincible" },
		{ 99, "?????" }
	};

	private void Start()
	{
		Topics.SetActive(false);
	}

	public void UpdateInfo(int ID)
	{
		StudentJson studentJson = JSON.Students[ID];
		NameLabel.text = studentJson.Name;
		string text = string.Empty + studentJson.Class;
		text = text.Insert(1, "-");
		ClassLabel.text = "Class " + text;
		if (studentJson.Name == "Unknown" || studentJson.Name == "Reserved" || ID == 90 || ID > 96)
		{
			ClassLabel.text = string.Empty;
		}
		if (StudentGlobals.GetStudentReputation(ID) < 0)
		{
			ReputationLabel.text = StudentGlobals.GetStudentReputation(ID).ToString();
		}
		else if (StudentGlobals.GetStudentReputation(ID) > 0)
		{
			ReputationLabel.text = "+" + StudentGlobals.GetStudentReputation(ID);
		}
		else
		{
			ReputationLabel.text = "0";
		}
		ReputationBar.localPosition = new Vector3((float)StudentGlobals.GetStudentReputation(ID) * 0.96f, ReputationBar.localPosition.y, ReputationBar.localPosition.z);
		if (ReputationBar.localPosition.x > 96f)
		{
			ReputationBar.localPosition = new Vector3(96f, ReputationBar.localPosition.y, ReputationBar.localPosition.z);
		}
		if (ReputationBar.localPosition.x < -96f)
		{
			ReputationBar.localPosition = new Vector3(-96f, ReputationBar.localPosition.y, ReputationBar.localPosition.z);
		}
		PersonaLabel.text = Persona.PersonaNames[studentJson.Persona];
		if (studentJson.Persona == PersonaType.Strict && studentJson.Club == ClubType.GymTeacher && !StudentGlobals.GetStudentReplaced(ID))
		{
			PersonaLabel.text = "Friendly but Strict";
		}
		if (studentJson.Crush == 0)
		{
			CrushLabel.text = "None";
		}
		else if (studentJson.Crush == 99)
		{
			CrushLabel.text = "?????";
		}
		else
		{
			CrushLabel.text = JSON.Students[studentJson.Crush].Name;
		}
		if (studentJson.Club < ClubType.Teacher)
		{
			OccupationLabel.text = "Club";
		}
		else
		{
			OccupationLabel.text = "Occupation";
		}
		if (studentJson.Club < ClubType.Teacher)
		{
			ClubLabel.text = Club.ClubNames[studentJson.Club];
		}
		else
		{
			ClubLabel.text = Club.TeacherClubNames[studentJson.Class];
		}
		if (ClubGlobals.GetClubClosed(studentJson.Club))
		{
			ClubLabel.text = "No Club";
		}
		StrengthLabel.text = StrengthStrings[studentJson.Strength];
		AudioSource component = GetComponent<AudioSource>();
		component.enabled = false;
		Static.SetActive(false);
		component.volume = 0f;
		component.Stop();
		if (ID < 98)
		{
			string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + ID + ".png";
			WWW wWW = new WWW(url);
			if (!StudentGlobals.GetStudentReplaced(ID))
			{
				Portrait.mainTexture = wWW.texture;
			}
			else
			{
				Portrait.mainTexture = BlankPortrait;
			}
		}
		else
		{
			switch (ID)
			{
			case 98:
				Portrait.mainTexture = GuidanceCounselor;
				break;
			case 99:
				Portrait.mainTexture = Headmaster;
				break;
			case 100:
				Portrait.mainTexture = InfoChan;
				Static.SetActive(true);
				if (!StudentInfoMenu.Gossiping && !StudentInfoMenu.Distracting && !StudentInfoMenu.CyberBullying)
				{
					component.enabled = true;
					component.volume = 1f;
					component.Play();
				}
				break;
			}
		}
		UpdateAdditionalInfo(ID);
		CurrentStudent = ID;
	}

	private void Update()
	{
		if (Input.GetButtonDown("A"))
		{
			if (StudentInfoMenu.Gossiping)
			{
				StudentInfoMenu.PauseScreen.MainMenu.SetActive(true);
				StudentInfoMenu.PauseScreen.Show = false;
				DialogueWheel.Victim = CurrentStudent;
				StudentInfoMenu.Gossiping = false;
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (StudentInfoMenu.Distracting)
			{
				StudentInfoMenu.PauseScreen.MainMenu.SetActive(true);
				StudentInfoMenu.PauseScreen.Show = false;
				DialogueWheel.Victim = CurrentStudent;
				StudentInfoMenu.Distracting = false;
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (StudentInfoMenu.CyberBullying)
			{
				HomeInternet.PostLabels[1].text = JSON.Students[CurrentStudent].Name;
				HomeInternet.Student = CurrentStudent;
				StudentInfoMenu.PauseScreen.MainMenu.SetActive(true);
				StudentInfoMenu.PauseScreen.Show = false;
				StudentInfoMenu.CyberBullying = false;
				base.gameObject.SetActive(false);
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (StudentInfoMenu.MatchMaking)
			{
				StudentInfoMenu.PauseScreen.MainMenu.SetActive(true);
				StudentInfoMenu.PauseScreen.Show = false;
				DialogueWheel.Victim = CurrentStudent;
				StudentInfoMenu.MatchMaking = false;
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (StudentInfoMenu.Targeting)
			{
				StudentInfoMenu.PauseScreen.MainMenu.SetActive(true);
				StudentInfoMenu.PauseScreen.Show = false;
				Yandere.TargetStudent.HuntTarget = StudentManager.Students[CurrentStudent];
				Yandere.TargetStudent.GoCommitMurder();
				Yandere.RPGCamera.enabled = true;
				Yandere.TargetStudent = null;
				StudentInfoMenu.Targeting = false;
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (StudentInfoMenu.SendingHome)
			{
				if (StudentManager.Students[CurrentStudent].Routine && !StudentManager.Students[CurrentStudent].InEvent && !StudentManager.Students[CurrentStudent].TargetedForDistraction && StudentManager.Students[CurrentStudent].ClubActivityPhase < 16 && !StudentManager.Students[CurrentStudent].MyBento.Tampered)
				{
					StudentManager.Students[CurrentStudent].Routine = false;
					StudentManager.Students[CurrentStudent].SentHome = true;
					StudentManager.Students[CurrentStudent].CameraReacting = false;
					StudentManager.Students[CurrentStudent].SpeechLines.Stop();
					StudentManager.Students[CurrentStudent].EmptyHands();
					StudentInfoMenu.PauseScreen.ServiceMenu.gameObject.SetActive(true);
					StudentInfoMenu.PauseScreen.ServiceMenu.UpdateList();
					StudentInfoMenu.PauseScreen.ServiceMenu.UpdateDesc();
					StudentInfoMenu.PauseScreen.ServiceMenu.Purchase();
					StudentInfoMenu.SendingHome = false;
					base.gameObject.SetActive(false);
					PromptBar.ClearButtons();
					PromptBar.Show = false;
				}
				else
				{
					StudentInfoMenu.PauseScreen.ServiceMenu.TextMessageManager.SpawnMessage(0);
					base.gameObject.SetActive(false);
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = string.Empty;
					PromptBar.Label[1].text = "Back";
					PromptBar.UpdateButtons();
				}
			}
			else if (StudentInfoMenu.FindingLocker)
			{
				NoteLocker.gameObject.SetActive(true);
				NoteLocker.transform.position = StudentManager.Students[StudentInfoMenu.StudentID].MyLocker.position;
				NoteLocker.transform.position += new Vector3(0f, 1.355f, 0f);
				NoteLocker.transform.position += StudentManager.Students[StudentInfoMenu.StudentID].MyLocker.forward * 0.33333f;
				NoteLocker.Prompt.Label[0].text = "     Leave note for " + StudentManager.Students[StudentInfoMenu.StudentID].Name;
				NoteLocker.Student = StudentManager.Students[StudentInfoMenu.StudentID];
				NoteLocker.LockerOwner = StudentInfoMenu.StudentID;
				StudentInfoMenu.PauseScreen.MainMenu.SetActive(true);
				StudentInfoMenu.PauseScreen.Show = false;
				StudentInfoMenu.FindingLocker = false;
				base.gameObject.SetActive(false);
				PromptBar.ClearButtons();
				PromptBar.Show = false;
				Yandere.RPGCamera.enabled = true;
				Time.timeScale = 1f;
			}
		}
		if (Input.GetButtonDown("B"))
		{
			Topics.SetActive(false);
			GetComponent<AudioSource>().Stop();
			if (Shutter != null)
			{
				if (!Shutter.PhotoIcons.activeInHierarchy)
				{
					Back = true;
				}
			}
			else
			{
				Back = true;
			}
			if (Back)
			{
				StudentInfoMenu.gameObject.SetActive(true);
				base.gameObject.SetActive(false);
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "View Info";
				if (!StudentInfoMenu.Gossiping)
				{
					PromptBar.Label[1].text = "Back";
				}
				PromptBar.UpdateButtons();
				Back = false;
			}
		}
		if (Input.GetButtonDown("X"))
		{
			if (StudentManager.Tag.Target != StudentManager.Students[CurrentStudent].Head)
			{
				StudentManager.Tag.Target = StudentManager.Students[CurrentStudent].Head;
				PromptBar.Label[2].text = "Untag";
			}
			else
			{
				StudentManager.Tag.Target = null;
				PromptBar.Label[2].text = "Tag";
			}
		}
		if (Input.GetButtonDown("Y") && PromptBar.Button[3].enabled)
		{
			if (!Topics.activeInHierarchy)
			{
				PromptBar.Label[3].text = "Basic Info";
				PromptBar.UpdateButtons();
				Topics.SetActive(true);
				UpdateTopics();
			}
			else
			{
				PromptBar.Label[3].text = "Interests";
				PromptBar.UpdateButtons();
				Topics.SetActive(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			StudentGlobals.SetStudentReputation(CurrentStudent, StudentGlobals.GetStudentReputation(CurrentStudent) + 10);
			UpdateInfo(CurrentStudent);
		}
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			StudentGlobals.SetStudentReputation(CurrentStudent, StudentGlobals.GetStudentReputation(CurrentStudent) - 10);
			UpdateInfo(CurrentStudent);
		}
	}

	private void UpdateAdditionalInfo(int ID)
	{
		switch (ID)
		{
		case 30:
			Strings[1] = ((!EventGlobals.Event1) ? "?????" : "May be a victim of domestic abuse.");
			Strings[2] = ((!EventGlobals.Event2) ? "?????" : "May be engaging in compensated dating in Shisuta Town.");
			InfoLabel.text = Strings[1] + "\n\n" + Strings[2];
			return;
		case 51:
			if (ClubGlobals.GetClubClosed(ClubType.LightMusic))
			{
				InfoLabel.text = "Disbanded the Light Music Club, dyed her hair back to its original color, removed her piercings, and stopped socializing with others.";
			}
			else
			{
				InfoLabel.text = JSON.Students[ID].Info;
			}
			return;
		}
		if (!StudentGlobals.GetStudentReplaced(ID))
		{
			if (JSON.Students[ID].Info == string.Empty)
			{
				InfoLabel.text = "No additional information is available at this time.";
			}
			else
			{
				InfoLabel.text = JSON.Students[ID].Info;
			}
		}
		else
		{
			InfoLabel.text = "No additional information is available at this time.";
		}
	}

	private void UpdateTopics()
	{
		for (int i = 1; i < TopicIcons.Length; i++)
		{
			TopicIcons[i].spriteName = (ConversationGlobals.GetTopicDiscovered(i) ? i : 0).ToString();
		}
		for (int j = 1; j <= 25; j++)
		{
			UISprite uISprite = TopicOpinionIcons[j];
			if (!ConversationGlobals.GetTopicLearnedByStudent(j, CurrentStudent))
			{
				uISprite.spriteName = "Unknown";
				continue;
			}
			int[] topics = JSON.Topics[CurrentStudent].Topics;
			uISprite.spriteName = OpinionSpriteNames[topics[j]];
		}
	}
}
