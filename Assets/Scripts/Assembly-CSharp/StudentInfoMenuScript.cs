using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudentInfoMenuScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public StudentInfoScript StudentInfo;

	public NoteWindowScript NoteWindow;

	public PromptBarScript PromptBar;

	public JsonScript JSON;

	public GameObject StudentPortrait;

	public Texture UnknownPortrait;

	public Texture BlankPortrait;

	public Texture Headmaster;

	public Texture Counselor;

	public Texture InfoChan;

	public Transform PortraitGrid;

	public Transform Highlight;

	public Transform Scrollbar;

	public StudentPortraitScript[] StudentPortraits;

	public bool[] PortraitLoaded;

	public UISprite[] DeathShadows;

	public UISprite[] Friends;

	public UISprite[] Panties;

	public UITexture[] PrisonBars;

	public UITexture[] Portraits;

	public UILabel NameLabel;

	public bool CyberBullying;

	public bool FindingLocker;

	public bool UsingLifeNote;

	public bool GettingInfo;

	public bool MatchMaking;

	public bool Distracting;

	public bool SendingHome;

	public bool Gossiping;

	public bool Targeting;

	public bool Dead;

	public int[] SetSizes;

	public int StudentID;

	public int Column;

	public int Row;

	public int Set;

	public int Columns;

	public int Rows;

	public bool GrabbedPortraits;

	public bool Debugging;

	private void Start()
	{
		for (int i = 1; i < 101; i++)
		{
			GameObject gameObject = Object.Instantiate(StudentPortrait, base.transform.position, Quaternion.identity);
			gameObject.transform.parent = PortraitGrid;
			gameObject.transform.localPosition = new Vector3(-300f + (float)Column * 150f, 80f - (float)Row * 160f, 0f);
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			StudentPortraits[i] = gameObject.GetComponent<StudentPortraitScript>();
			Column++;
			if (Column > 4)
			{
				Column = 0;
				Row++;
			}
		}
		Column = 0;
		Row = 0;
	}

	private void Update()
	{
		if (!GrabbedPortraits)
		{
			StartCoroutine(UpdatePortraits());
			GrabbedPortraits = true;
		}
		if (Input.GetButtonDown("A") && PromptBar.Label[0].text != string.Empty)
		{
			if (StudentGlobals.GetStudentPhotographed(StudentID) || StudentID > 97)
			{
				if (UsingLifeNote)
				{
					PauseScreen.MainMenu.SetActive(true);
					PauseScreen.Sideways = false;
					PauseScreen.Show = false;
					base.gameObject.SetActive(false);
					NoteWindow.TargetStudent = StudentID;
					NoteWindow.gameObject.SetActive(true);
					NoteWindow.SlotLabels[1].text = StudentManager.Students[StudentID].Name;
					NoteWindow.SlotsFilled[1] = true;
					UsingLifeNote = false;
					PromptBar.Label[0].text = "Confirm";
					PromptBar.UpdateButtons();
					NoteWindow.CheckForCompletion();
				}
				else
				{
					StudentInfo.gameObject.SetActive(true);
					StudentInfo.UpdateInfo(StudentID);
					StudentInfo.Topics.SetActive(false);
					base.gameObject.SetActive(false);
					PromptBar.ClearButtons();
					if (Gossiping)
					{
						PromptBar.Label[0].text = "Gossip";
					}
					if (Distracting)
					{
						PromptBar.Label[0].text = "Distract";
					}
					if (CyberBullying)
					{
						PromptBar.Label[0].text = "Accept";
					}
					if (FindingLocker)
					{
						PromptBar.Label[0].text = "Find Locker";
					}
					if (MatchMaking)
					{
						PromptBar.Label[0].text = "Match";
					}
					if (Targeting || UsingLifeNote)
					{
						PromptBar.Label[0].text = "Kill";
					}
					if (SendingHome)
					{
						PromptBar.Label[0].text = "Send Home";
					}
					if (StudentManager.Students[StudentID] != null)
					{
						if (StudentManager.Students[StudentID].gameObject.activeInHierarchy)
						{
							if (StudentManager.Tag.Target == StudentManager.Students[StudentID].Head)
							{
								PromptBar.Label[2].text = "Untag";
							}
							else
							{
								PromptBar.Label[2].text = "Tag";
							}
						}
						else
						{
							PromptBar.Label[2].text = string.Empty;
						}
					}
					else
					{
						PromptBar.Label[2].text = string.Empty;
					}
					PromptBar.Label[1].text = "Back";
					PromptBar.Label[3].text = "Interests";
					PromptBar.UpdateButtons();
				}
			}
			else
			{
				StudentGlobals.SetStudentPhotographed(StudentID, true);
				PauseScreen.ServiceMenu.gameObject.SetActive(true);
				PauseScreen.ServiceMenu.UpdateList();
				PauseScreen.ServiceMenu.UpdateDesc();
				PauseScreen.ServiceMenu.Purchase();
				GettingInfo = false;
				base.gameObject.SetActive(false);
			}
		}
		if (Input.GetButtonDown("B"))
		{
			if (Gossiping || Distracting || MatchMaking || Targeting)
			{
				if (Targeting)
				{
					PauseScreen.Yandere.RPGCamera.enabled = true;
				}
				PauseScreen.Yandere.Interaction = YandereInteractionType.Bye;
				PauseScreen.Yandere.TalkTimer = 2f;
				PauseScreen.MainMenu.SetActive(true);
				PauseScreen.Sideways = false;
				PauseScreen.Show = false;
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				Distracting = false;
				MatchMaking = false;
				Gossiping = false;
				Targeting = false;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (CyberBullying || FindingLocker)
			{
				PauseScreen.MainMenu.SetActive(true);
				PauseScreen.Sideways = false;
				PauseScreen.Show = false;
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				if (FindingLocker)
				{
					PauseScreen.Yandere.RPGCamera.enabled = true;
				}
				FindingLocker = false;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
			}
			else if (SendingHome || GettingInfo)
			{
				PauseScreen.ServiceMenu.gameObject.SetActive(true);
				PauseScreen.ServiceMenu.UpdateList();
				PauseScreen.ServiceMenu.UpdateDesc();
				base.gameObject.SetActive(false);
				SendingHome = false;
				GettingInfo = false;
			}
			else if (UsingLifeNote)
			{
				PauseScreen.MainMenu.SetActive(true);
				PauseScreen.Sideways = false;
				PauseScreen.Show = false;
				base.gameObject.SetActive(false);
				NoteWindow.gameObject.SetActive(true);
				UsingLifeNote = false;
			}
			else
			{
				PauseScreen.MainMenu.SetActive(true);
				PauseScreen.Sideways = false;
				PauseScreen.PressedB = true;
				base.gameObject.SetActive(false);
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Accept";
				PromptBar.Label[1].text = "Exit";
				PromptBar.Label[4].text = "Choose";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
			}
		}
		float t = Time.unscaledDeltaTime * 10f;
		float num = ((Row % 2 != 0) ? ((Row - 1) / 2) : (Row / 2));
		float b = 320f * num;
		PortraitGrid.localPosition = new Vector3(PortraitGrid.localPosition.x, Mathf.Lerp(PortraitGrid.localPosition.y, b, t), PortraitGrid.localPosition.z);
		Scrollbar.localPosition = new Vector3(Scrollbar.localPosition.x, Mathf.Lerp(Scrollbar.localPosition.y, 175f - 350f * (PortraitGrid.localPosition.y / 2880f), t), Scrollbar.localPosition.z);
		if (InputManager.TappedUp)
		{
			Row--;
			if (Row < 0)
			{
				Row = Rows - 1;
			}
			UpdateHighlight();
		}
		if (InputManager.TappedDown)
		{
			Row++;
			if (Row > Rows - 1)
			{
				Row = 0;
			}
			UpdateHighlight();
		}
		if (InputManager.TappedRight)
		{
			Column++;
			if (Column > Columns - 1)
			{
				Column = 0;
			}
			UpdateHighlight();
		}
		if (InputManager.TappedLeft)
		{
			Column--;
			if (Column < 0)
			{
				Column = Columns - 1;
			}
			UpdateHighlight();
		}
	}

	public void UpdateHighlight()
	{
		StudentID = 1 + (Column + Row * Columns);
		if (StudentGlobals.GetStudentPhotographed(StudentID) || StudentID > 97)
		{
			PromptBar.Label[0].text = "View Info";
			PromptBar.UpdateButtons();
		}
		else
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		if (Gossiping && (StudentID == 1 || StudentID == PauseScreen.Yandere.TargetStudent.StudentID || JSON.Students[StudentID].Club == ClubType.Sports || StudentGlobals.GetStudentDead(StudentID) || StudentID > 97))
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		if (CyberBullying && (JSON.Students[StudentID].Gender == 1 || StudentGlobals.GetStudentDead(StudentID) || StudentID > 97))
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		if (FindingLocker && (StudentID == 1 || StudentID > 85 || StudentGlobals.GetStudentDead(StudentID)))
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		if (Distracting)
		{
			Dead = false;
			if (StudentManager.Students[StudentID] == null)
			{
				Dead = true;
			}
			if (Dead)
			{
				PromptBar.Label[0].text = string.Empty;
				PromptBar.UpdateButtons();
			}
			else if (StudentID == 1 || !StudentManager.Students[StudentID].Alive || StudentID == PauseScreen.Yandere.TargetStudent.StudentID || StudentGlobals.GetStudentKidnapped(StudentID) || StudentManager.Students[StudentID].Tranquil || StudentManager.Students[StudentID].Slave || StudentGlobals.GetStudentDead(StudentID) || StudentID > 97)
			{
				PromptBar.Label[0].text = string.Empty;
				PromptBar.UpdateButtons();
			}
		}
		if (MatchMaking && (StudentID == PauseScreen.Yandere.TargetStudent.StudentID || StudentGlobals.GetStudentDead(StudentID) || StudentID > 97))
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		if (Targeting && (StudentID == 1 || StudentID > 97 || StudentGlobals.GetStudentDead(StudentID) || !StudentManager.Students[StudentID].gameObject.activeInHierarchy || StudentManager.Students[StudentID].InEvent || StudentManager.Students[StudentID].Tranquil))
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		if (SendingHome)
		{
			Debug.Log("Highlighting student number " + StudentID);
			if (StudentManager.Students[StudentID] != null)
			{
				StudentScript studentScript = StudentManager.Students[StudentID];
				if (StudentID == 1 || StudentGlobals.GetStudentDead(StudentID) || (StudentID < 98 && studentScript.SentHome) || StudentID > 97 || StudentGlobals.GetStudentSlave() == StudentID || (studentScript.Club == ClubType.MartialArts && studentScript.ClubAttire) || (studentScript.Club == ClubType.Sports && studentScript.ClubAttire))
				{
					PromptBar.Label[0].text = string.Empty;
					PromptBar.UpdateButtons();
				}
			}
		}
		if (GettingInfo)
		{
			if (StudentGlobals.GetStudentPhotographed(StudentID) || StudentID > 97)
			{
				PromptBar.Label[0].text = string.Empty;
				PromptBar.UpdateButtons();
			}
			else
			{
				PromptBar.Label[0].text = "Get Info";
				PromptBar.UpdateButtons();
			}
		}
		if (UsingLifeNote)
		{
			if (StudentID == 1 || StudentID > 97 || (StudentID > 10 && StudentID < 21) || StudentPortraits[StudentID].DeathShadow.activeInHierarchy || (StudentManager.Students[StudentID] != null && !StudentManager.Students[StudentID].enabled))
			{
				PromptBar.Label[0].text = string.Empty;
			}
			else
			{
				PromptBar.Label[0].text = "Kill";
			}
			PromptBar.UpdateButtons();
		}
		Highlight.localPosition = new Vector3(-300f + (float)Column * 150f, 80f - (float)Row * 160f, Highlight.localPosition.z);
		UpdateNameLabel();
	}

	private void UpdateNameLabel()
	{
		if (StudentID > 97 || StudentGlobals.GetStudentPhotographed(StudentID) || GettingInfo)
		{
			NameLabel.text = JSON.Students[StudentID].Name;
		}
		else
		{
			NameLabel.text = "Unknown";
		}
	}

	public IEnumerator UpdatePortraits()
	{
		if (Debugging)
		{
			Debug.Log("The Student Info Menu was instructed to get photos.");
		}
		for (int ID = 1; ID < 101; ID++)
		{
			if (Debugging)
			{
				Debug.Log("1 - We entered the loop.");
			}
			if (ID == 0)
			{
				StudentPortraits[ID].Portrait.mainTexture = InfoChan;
			}
			else
			{
				if (Debugging)
				{
					Debug.Log("2 - ID is not zero.");
				}
				if (!PortraitLoaded[ID])
				{
					if (Debugging)
					{
						Debug.Log("3 - PortraitLoaded is false.");
					}
					if (ID < 98)
					{
						if (Debugging)
						{
							Debug.Log("4 - ID is less than 98.");
						}
						if (StudentGlobals.GetStudentPhotographed(ID))
						{
							if (Debugging)
							{
								Debug.Log("5 - GetStudentPhotographed is true.");
							}
							string path = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + ID + ".png";
							if (Debugging)
							{
								Debug.Log("Path is: " + path);
							}
							WWW www = new WWW(path);
							if (Debugging)
							{
								Debug.Log("Waiting for www to return.");
							}
							yield return www;
							if (Debugging)
							{
								Debug.Log("www has returned.");
							}
							if (www.error == null)
							{
								if (!StudentGlobals.GetStudentReplaced(ID))
								{
									StudentPortraits[ID].Portrait.mainTexture = www.texture;
								}
								else
								{
									StudentPortraits[ID].Portrait.mainTexture = BlankPortrait;
								}
							}
							else
							{
								StudentPortraits[ID].Portrait.mainTexture = UnknownPortrait;
							}
							PortraitLoaded[ID] = true;
						}
						else
						{
							StudentPortraits[ID].Portrait.mainTexture = UnknownPortrait;
						}
					}
					else
					{
						switch (ID)
						{
						case 98:
							StudentPortraits[ID].Portrait.mainTexture = Counselor;
							break;
						case 99:
							StudentPortraits[ID].Portrait.mainTexture = Headmaster;
							break;
						case 100:
							StudentPortraits[ID].Portrait.mainTexture = InfoChan;
							break;
						}
					}
				}
			}
			if (PlayerGlobals.GetStudentPantyShot(JSON.Students[ID].Name))
			{
				StudentPortraits[ID].Panties.SetActive(true);
			}
			StudentPortraits[ID].Friend.SetActive(PlayerGlobals.GetStudentFriend(ID));
			if (StudentGlobals.GetStudentDying(ID) || StudentGlobals.GetStudentDead(ID))
			{
				StudentPortraits[ID].DeathShadow.SetActive(true);
			}
			if (SceneManager.GetActiveScene().name == "SchoolScene" && StudentManager.Students[ID] != null && StudentManager.Students[ID].Tranquil)
			{
				StudentPortraits[ID].DeathShadow.SetActive(true);
			}
			if (StudentGlobals.GetStudentArrested(ID))
			{
				StudentPortraits[ID].PrisonBars.SetActive(true);
				StudentPortraits[ID].DeathShadow.SetActive(true);
			}
		}
	}
}
