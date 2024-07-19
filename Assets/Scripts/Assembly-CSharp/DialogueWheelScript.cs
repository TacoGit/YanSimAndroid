using UnityEngine;

public class DialogueWheelScript : MonoBehaviour
{
	public AppearanceWindowScript AppearanceWindow;

	public PracticeWindowScript PracticeWindow;

	public ClubManagerScript ClubManager;

	public LoveManagerScript LoveManager;

	public PauseScreenScript PauseScreen;

	public TaskManagerScript TaskManager;

	public ClubWindowScript ClubWindow;

	public NoteLockerScript NoteLocker;

	public ReputationScript Reputation;

	public TaskWindowScript TaskWindow;

	public PromptBarScript PromptBar;

	public JukeboxScript Jukebox;

	public YandereScript Yandere;

	public ClockScript Clock;

	public UIPanel Panel;

	public GameObject SwitchTopicsWindow;

	public GameObject TaskDialogueWindow;

	public GameObject ClubLeaderWindow;

	public GameObject DatingMinigame;

	public GameObject LockerWindow;

	public Transform Interaction;

	public Transform Favors;

	public Transform Club;

	public Transform Love;

	public UISprite TaskIcon;

	public UISprite Impatience;

	public UILabel CenterLabel;

	public UISprite[] Segment;

	public UISprite[] Shadow;

	public string[] Text;

	public UISprite[] FavorSegment;

	public UISprite[] FavorShadow;

	public UISprite[] ClubSegment;

	public UISprite[] ClubShadow;

	public UISprite[] LoveSegment;

	public UISprite[] LoveShadow;

	public string[] FavorText;

	public string[] ClubText;

	public string[] LoveText;

	public int Selected;

	public int Victim;

	public bool AskingFavor;

	public bool Matchmaking;

	public bool ClubLeader;

	public bool Pestered;

	public bool Show;

	public Vector3 PreviousPosition;

	public Vector2 MouseDelta;

	private void Start()
	{
		Interaction.localScale = new Vector3(1f, 1f, 1f);
		Favors.localScale = Vector3.zero;
		Club.localScale = Vector3.zero;
		Love.localScale = Vector3.zero;
		base.transform.localScale = Vector3.zero;
	}

	private void Update()
	{
		if (!Show)
		{
			if (base.transform.localScale.x > 0.1f)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			else if (Panel.enabled)
			{
				base.transform.localScale = Vector3.zero;
				Panel.enabled = false;
			}
		}
		else
		{
			if (ClubLeader)
			{
				Interaction.localScale = Vector3.Lerp(Interaction.localScale, Vector3.zero, Time.deltaTime * 10f);
				Favors.localScale = Vector3.Lerp(Favors.localScale, Vector3.zero, Time.deltaTime * 10f);
				Club.localScale = Vector3.Lerp(Club.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				Love.localScale = Vector3.Lerp(Love.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			else if (AskingFavor)
			{
				Interaction.localScale = Vector3.Lerp(Interaction.localScale, Vector3.zero, Time.deltaTime * 10f);
				Favors.localScale = Vector3.Lerp(Favors.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				Club.localScale = Vector3.Lerp(Club.localScale, Vector3.zero, Time.deltaTime * 10f);
				Love.localScale = Vector3.Lerp(Love.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			else if (Matchmaking)
			{
				Interaction.localScale = Vector3.Lerp(Interaction.localScale, Vector3.zero, Time.deltaTime * 10f);
				Favors.localScale = Vector3.Lerp(Favors.localScale, Vector3.zero, Time.deltaTime * 10f);
				Club.localScale = Vector3.Lerp(Club.localScale, Vector3.zero, Time.deltaTime * 10f);
				Love.localScale = Vector3.Lerp(Love.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			}
			else
			{
				Interaction.localScale = Vector3.Lerp(Interaction.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				Favors.localScale = Vector3.Lerp(Favors.localScale, Vector3.zero, Time.deltaTime * 10f);
				Club.localScale = Vector3.Lerp(Club.localScale, Vector3.zero, Time.deltaTime * 10f);
				Love.localScale = Vector3.Lerp(Love.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			MouseDelta.x += Input.GetAxis("Mouse X");
			MouseDelta.y += Input.GetAxis("Mouse Y");
			if (MouseDelta.x > 11f)
			{
				MouseDelta.x = 11f;
			}
			else if (MouseDelta.x < -11f)
			{
				MouseDelta.x = -11f;
			}
			if (MouseDelta.y > 11f)
			{
				MouseDelta.y = 11f;
			}
			else if (MouseDelta.y < -11f)
			{
				MouseDelta.y = -11f;
			}
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			if (!AskingFavor && !Matchmaking)
			{
				if (Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f && Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f)
				{
					Selected = 0;
				}
				if ((Input.GetAxis("Vertical") > 0.5f && Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f) || (MouseDelta.y > 10f && MouseDelta.x < 10f && MouseDelta.x > -10f))
				{
					Selected = 1;
				}
				if ((Input.GetAxis("Vertical") > 0f && Input.GetAxis("Horizontal") > 0.5f) || (MouseDelta.y > 0f && MouseDelta.x > 10f))
				{
					Selected = 2;
				}
				if ((Input.GetAxis("Vertical") < 0f && Input.GetAxis("Horizontal") > 0.5f) || (MouseDelta.y < 0f && MouseDelta.x > 10f))
				{
					Selected = 3;
				}
				if ((Input.GetAxis("Vertical") < -0.5f && Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f) || (MouseDelta.y < -10f && MouseDelta.x < 10f && MouseDelta.x > -10f))
				{
					Selected = 4;
				}
				if ((Input.GetAxis("Vertical") < 0f && Input.GetAxis("Horizontal") < -0.5f) || (MouseDelta.y < 0f && MouseDelta.x < -10f))
				{
					Selected = 5;
				}
				if ((Input.GetAxis("Vertical") > 0f && Input.GetAxis("Horizontal") < -0.5f) || (MouseDelta.y > 0f && MouseDelta.x < -10f))
				{
					Selected = 6;
				}
				if (!ClubLeader)
				{
					if (Selected == 5)
					{
						CenterLabel.text = (PlayerGlobals.GetStudentFriend(Yandere.TargetStudent.StudentID) ? "Love" : Text[Selected]);
					}
					else
					{
						CenterLabel.text = Text[Selected];
					}
				}
				else
				{
					CenterLabel.text = ClubText[Selected];
				}
			}
			else
			{
				if (Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f && Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f)
				{
					Selected = 0;
				}
				if ((Input.GetAxis("Vertical") > 0.5f && Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f) || (MouseDelta.y > 10f && MouseDelta.x < 10f && MouseDelta.x > -10f))
				{
					Selected = 1;
				}
				if ((Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f && Input.GetAxis("Horizontal") > 0.5f) || (MouseDelta.y < 10f && MouseDelta.y > -10f && MouseDelta.x > 10f))
				{
					Selected = 2;
				}
				if ((Input.GetAxis("Vertical") < -0.5f && Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f) || (MouseDelta.y < -10f && MouseDelta.x < 10f && MouseDelta.x > -10f))
				{
					Selected = 3;
				}
				if ((Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f && Input.GetAxis("Horizontal") < -0.5f) || (MouseDelta.y < 10f && MouseDelta.y > -10f && MouseDelta.x < -10f))
				{
					Selected = 4;
				}
				if (Selected < FavorText.Length)
				{
					CenterLabel.text = ((!AskingFavor) ? LoveText[Selected] : FavorText[Selected]);
				}
			}
			if (!ClubLeader)
			{
				for (int i = 1; i < 7; i++)
				{
					Transform transform = Segment[i].transform;
					transform.localScale = Vector3.Lerp(transform.localScale, (Selected != i) ? new Vector3(1f, 1f, 1f) : new Vector3(1.3f, 1.3f, 1f), Time.deltaTime * 10f);
				}
			}
			else
			{
				for (int j = 1; j < 7; j++)
				{
					Transform transform2 = ClubSegment[j].transform;
					transform2.localScale = Vector3.Lerp(transform2.localScale, (Selected != j) ? new Vector3(1f, 1f, 1f) : new Vector3(1.3f, 1.3f, 1f), Time.deltaTime * 10f);
				}
			}
			if (!Matchmaking)
			{
				for (int k = 1; k < 5; k++)
				{
					Transform transform3 = FavorSegment[k].transform;
					transform3.localScale = Vector3.Lerp(transform3.localScale, (Selected != k) ? new Vector3(1f, 1f, 1f) : new Vector3(1.3f, 1.3f, 1f), Time.deltaTime * 10f);
				}
			}
			else
			{
				for (int l = 1; l < 5; l++)
				{
					Transform transform4 = LoveSegment[l].transform;
					transform4.localScale = Vector3.Lerp(transform4.localScale, (Selected != l) ? new Vector3(1f, 1f, 1f) : new Vector3(1.3f, 1.3f, 1f), Time.deltaTime * 10f);
				}
			}
			if (Input.GetButtonDown("A"))
			{
				if (ClubLeader)
				{
					if (Selected != 0 && ClubShadow[Selected].color.a == 0f)
					{
						if (Selected == 1)
						{
							Impatience.fillAmount = 0f;
							Yandere.TargetStudent.Interaction = StudentInteractionType.ClubInfo;
							Yandere.TargetStudent.TalkTimer = 100f;
							Yandere.TargetStudent.ClubPhase = 1;
							Show = false;
						}
						else if (Selected == 2)
						{
							Impatience.fillAmount = 0f;
							Yandere.TargetStudent.Interaction = StudentInteractionType.ClubJoin;
							Yandere.TargetStudent.TalkTimer = 100f;
							Show = false;
							ClubManager.CheckGrudge(Yandere.TargetStudent.Club);
							if (ClubGlobals.GetQuitClub(Yandere.TargetStudent.Club))
							{
								Yandere.TargetStudent.ClubPhase = 4;
							}
							else if (ClubGlobals.Club != 0)
							{
								Yandere.TargetStudent.ClubPhase = 5;
							}
							else if (ClubManager.ClubGrudge)
							{
								Yandere.TargetStudent.ClubPhase = 6;
							}
							else
							{
								Yandere.TargetStudent.ClubPhase = 1;
							}
						}
						else if (Selected == 3)
						{
							Impatience.fillAmount = 0f;
							Yandere.TargetStudent.Interaction = StudentInteractionType.ClubQuit;
							Yandere.TargetStudent.TalkTimer = 100f;
							Yandere.TargetStudent.ClubPhase = 1;
							Show = false;
						}
						int num = 0;
						if (Yandere.TargetStudent.Sleuthing)
						{
							num = 5;
						}
						else if (Selected == 4)
						{
							Impatience.fillAmount = 0f;
							Yandere.TargetStudent.Interaction = StudentInteractionType.ClubBye;
							Yandere.TargetStudent.TalkTimer = Yandere.Subtitle.ClubFarewellClips[(int)(Yandere.TargetStudent.Club + num)].length;
							Show = false;
							Debug.Log("This club leader exchange is over.");
						}
						else if (Selected == 5)
						{
							Impatience.fillAmount = 0f;
							Yandere.TargetStudent.Interaction = StudentInteractionType.ClubActivity;
							Yandere.TargetStudent.TalkTimer = 100f;
							if (Clock.HourTime < 17f)
							{
								Yandere.TargetStudent.ClubPhase = 4;
							}
							else if (Clock.HourTime > 17.5f)
							{
								Yandere.TargetStudent.ClubPhase = 5;
							}
							else
							{
								Yandere.TargetStudent.ClubPhase = 1;
							}
							Show = false;
						}
						else if (Selected == 6)
						{
							Impatience.fillAmount = 0f;
							Yandere.TargetStudent.Interaction = StudentInteractionType.ClubPractice;
							Yandere.TargetStudent.TalkTimer = 100f;
							Yandere.TargetStudent.ClubPhase = 1;
							Show = false;
						}
					}
				}
				else if (AskingFavor)
				{
					if (Selected != 0)
					{
						if (Selected < FavorShadow.Length && FavorShadow[Selected] != null && FavorShadow[Selected].color.a == 0f)
						{
							if (Selected == 1)
							{
								Impatience.fillAmount = 0f;
								Yandere.Interaction = YandereInteractionType.FollowMe;
								Yandere.TalkTimer = 3f;
								Show = false;
							}
							else if (Selected == 2)
							{
								Impatience.fillAmount = 0f;
								Yandere.Interaction = YandereInteractionType.GoAway;
								Yandere.TalkTimer = 3f;
								Show = false;
							}
							else if (Selected == 4)
							{
								PauseScreen.StudentInfoMenu.Distracting = true;
								PauseScreen.StudentInfoMenu.gameObject.SetActive(true);
								PauseScreen.StudentInfoMenu.Column = 0;
								PauseScreen.StudentInfoMenu.Row = 0;
								PauseScreen.StudentInfoMenu.UpdateHighlight();
								StartCoroutine(PauseScreen.StudentInfoMenu.UpdatePortraits());
								PauseScreen.MainMenu.SetActive(false);
								PauseScreen.Panel.enabled = true;
								PauseScreen.Sideways = true;
								PauseScreen.Show = true;
								Time.timeScale = 0.0001f;
								PromptBar.ClearButtons();
								PromptBar.Label[1].text = "Cancel";
								PromptBar.UpdateButtons();
								PromptBar.Show = true;
								Impatience.fillAmount = 0f;
								Yandere.Interaction = YandereInteractionType.DistractThem;
								Yandere.TalkTimer = 3f;
								Show = false;
							}
						}
						if (Selected == 3)
						{
							AskingFavor = false;
						}
					}
				}
				else if (Matchmaking)
				{
					if (Selected != 0)
					{
						if (Selected < LoveShadow.Length && LoveShadow[Selected] != null && LoveShadow[Selected].color.a == 0f)
						{
							if (Selected == 1)
							{
								PromptBar.ClearButtons();
								PromptBar.Label[0].text = "Select";
								PromptBar.Label[4].text = "Change";
								PromptBar.UpdateButtons();
								PromptBar.Show = true;
								AppearanceWindow.gameObject.SetActive(true);
								AppearanceWindow.Show = true;
								Show = false;
							}
							else if (Selected == 2)
							{
								Impatience.fillAmount = 0f;
								Yandere.Interaction = YandereInteractionType.Court;
								Yandere.TalkTimer = 5f;
								Show = false;
							}
							else if (Selected == 4)
							{
								Impatience.fillAmount = 0f;
								Yandere.Interaction = YandereInteractionType.Confess;
								Yandere.TalkTimer = 5f;
								Show = false;
							}
						}
						if (Selected == 3)
						{
							Matchmaking = false;
						}
					}
				}
				else if (Selected != 0 && Shadow[Selected].color.a == 0f)
				{
					if (Selected == 1)
					{
						Impatience.fillAmount = 0f;
						Yandere.Interaction = YandereInteractionType.Apologizing;
						Yandere.TalkTimer = 3f;
						Show = false;
					}
					else if (Selected == 2)
					{
						Impatience.fillAmount = 0f;
						Yandere.Interaction = YandereInteractionType.Compliment;
						Yandere.TalkTimer = 3f;
						Show = false;
					}
					else if (Selected == 3)
					{
						PauseScreen.StudentInfoMenu.Gossiping = true;
						PauseScreen.StudentInfoMenu.gameObject.SetActive(true);
						PauseScreen.StudentInfoMenu.Column = 0;
						PauseScreen.StudentInfoMenu.Row = 0;
						PauseScreen.StudentInfoMenu.UpdateHighlight();
						StartCoroutine(PauseScreen.StudentInfoMenu.UpdatePortraits());
						PauseScreen.MainMenu.SetActive(false);
						PauseScreen.Panel.enabled = true;
						PauseScreen.Sideways = true;
						PauseScreen.Show = true;
						Time.timeScale = 0.0001f;
						PromptBar.ClearButtons();
						PromptBar.Label[0].text = string.Empty;
						PromptBar.Label[1].text = "Cancel";
						PromptBar.UpdateButtons();
						PromptBar.Show = true;
						Impatience.fillAmount = 0f;
						Yandere.Interaction = YandereInteractionType.Gossip;
						Yandere.TalkTimer = 3f;
						Show = false;
					}
					else if (Selected == 4)
					{
						Impatience.fillAmount = 0f;
						Yandere.Interaction = YandereInteractionType.Bye;
						Yandere.TalkTimer = 2f;
						Show = false;
						Debug.Log("This exchange is over.");
					}
					else if (Selected == 5)
					{
						if (!PlayerGlobals.GetStudentFriend(Yandere.TargetStudent.StudentID))
						{
							CheckTaskCompletion();
							if (Yandere.TargetStudent.TaskPhase == 0)
							{
								Impatience.fillAmount = 0f;
								Yandere.TargetStudent.Interaction = StudentInteractionType.GivingTask;
								Yandere.TargetStudent.TalkTimer = 100f;
								Yandere.TargetStudent.TaskPhase = 1;
							}
							else
							{
								Impatience.fillAmount = 0f;
								Yandere.TargetStudent.Interaction = StudentInteractionType.GivingTask;
								Yandere.TargetStudent.TalkTimer = 100f;
							}
							Show = false;
						}
						else if (Yandere.LoveManager.SuitorProgress == 0)
						{
							PauseScreen.StudentInfoMenu.MatchMaking = true;
							PauseScreen.StudentInfoMenu.gameObject.SetActive(true);
							PauseScreen.StudentInfoMenu.Column = 0;
							PauseScreen.StudentInfoMenu.Row = 0;
							PauseScreen.StudentInfoMenu.UpdateHighlight();
							StartCoroutine(PauseScreen.StudentInfoMenu.UpdatePortraits());
							PauseScreen.MainMenu.SetActive(false);
							PauseScreen.Panel.enabled = true;
							PauseScreen.Sideways = true;
							PauseScreen.Show = true;
							Time.timeScale = 0.0001f;
							PromptBar.ClearButtons();
							PromptBar.Label[0].text = "View Info";
							PromptBar.Label[1].text = "Cancel";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
							Impatience.fillAmount = 0f;
							Yandere.Interaction = YandereInteractionType.NamingCrush;
							Yandere.TalkTimer = 3f;
							Show = false;
						}
						else
						{
							Matchmaking = true;
						}
					}
					else if (Selected == 6)
					{
						AskingFavor = true;
					}
				}
			}
			else if (Input.GetButtonDown("X"))
			{
				if (TaskDialogueWindow.activeInHierarchy)
				{
					Impatience.fillAmount = 0f;
					Yandere.Interaction = YandereInteractionType.TaskInquiry;
					Yandere.TalkTimer = 3f;
					Show = false;
				}
				else if (SwitchTopicsWindow.activeInHierarchy)
				{
					ClubLeader = !ClubLeader;
					HideShadows();
				}
			}
			else if (Input.GetButtonDown("B") && LockerWindow.activeInHierarchy)
			{
				Impatience.fillAmount = 0f;
				Yandere.Interaction = YandereInteractionType.SendingToLocker;
				Yandere.TalkTimer = 5f;
				Show = false;
			}
		}
		PreviousPosition = Input.mousePosition;
	}

	public void HideShadows()
	{
		Jukebox.Dip = 0.5f;
		TaskDialogueWindow.SetActive(false);
		ClubLeaderWindow.SetActive(false);
		LockerWindow.SetActive(false);
		if (ClubLeader && !Yandere.TargetStudent.Talk.Fake)
		{
			SwitchTopicsWindow.SetActive(true);
		}
		else
		{
			SwitchTopicsWindow.SetActive(false);
		}
		if (Yandere.TargetStudent.Armband.activeInHierarchy && !ClubLeader && Yandere.TargetStudent.Club != ClubType.Council)
		{
			ClubLeaderWindow.SetActive(true);
		}
		if (NoteLocker.NoteLeft && NoteLocker.Student == Yandere.TargetStudent)
		{
			LockerWindow.SetActive(true);
		}
		if (Yandere.TargetStudent.Club == ClubType.Bully && TaskGlobals.GetTaskStatus(36) == 1)
		{
			TaskDialogueWindow.SetActive(true);
		}
		TaskIcon.spriteName = ((!PlayerGlobals.GetStudentFriend(Yandere.TargetStudent.StudentID)) ? "Task" : "Heart");
		Impatience.fillAmount = 0f;
		for (int i = 1; i < 7; i++)
		{
			UISprite uISprite = Shadow[i];
			uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, 0f);
		}
		for (int j = 1; j < 5; j++)
		{
			UISprite uISprite2 = FavorShadow[j];
			uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0f);
		}
		for (int k = 1; k < 7; k++)
		{
			UISprite uISprite3 = ClubShadow[k];
			uISprite3.color = new Color(uISprite3.color.r, uISprite3.color.g, uISprite3.color.b, 0f);
		}
		for (int l = 1; l < 5; l++)
		{
			UISprite uISprite4 = LoveShadow[l];
			uISprite4.color = new Color(uISprite4.color.r, uISprite4.color.g, uISprite4.color.b, 0f);
		}
		if (!Yandere.TargetStudent.Witness || Yandere.TargetStudent.Forgave || Yandere.TargetStudent.Club == ClubType.Council)
		{
			UISprite uISprite5 = Shadow[1];
			uISprite5.color = new Color(uISprite5.color.r, uISprite5.color.g, uISprite5.color.b, 0.75f);
		}
		if (Yandere.TargetStudent.Complimented || Yandere.TargetStudent.Club == ClubType.Council)
		{
			UISprite uISprite6 = Shadow[2];
			uISprite6.color = new Color(uISprite6.color.r, uISprite6.color.g, uISprite6.color.b, 0.75f);
		}
		if (Yandere.TargetStudent.Gossiped || Yandere.TargetStudent.Club == ClubType.Council)
		{
			UISprite uISprite7 = Shadow[3];
			uISprite7.color = new Color(uISprite7.color.r, uISprite7.color.g, uISprite7.color.b, 0.75f);
		}
		if (Yandere.Bloodiness > 0f || Yandere.Sanity < 33.33333f || Yandere.TargetStudent.Club == ClubType.Council)
		{
			UISprite uISprite8 = Shadow[3];
			uISprite8.color = new Color(uISprite8.color.r, uISprite8.color.g, uISprite8.color.b, 0.75f);
			UISprite uISprite9 = Shadow[5];
			uISprite9.color = new Color(uISprite9.color.r, uISprite9.color.g, uISprite9.color.b, 0.75f);
			UISprite uISprite10 = Shadow[6];
			uISprite10.color = new Color(uISprite10.color.r, uISprite10.color.g, uISprite10.color.b, 0.75f);
		}
		else if (Reputation.Reputation < -33.33333f)
		{
			UISprite uISprite11 = Shadow[3];
			uISprite11.color = new Color(uISprite11.color.r, uISprite11.color.g, uISprite11.color.b, 0.75f);
		}
		if (!Yandere.TargetStudent.Indoors || Yandere.TargetStudent.Club == ClubType.Council)
		{
			UISprite uISprite12 = Shadow[5];
			uISprite12.color = new Color(uISprite12.color.r, uISprite12.color.g, uISprite12.color.b, 0.75f);
		}
		else if (!PlayerGlobals.GetStudentFriend(Yandere.TargetStudent.StudentID))
		{
			if (Yandere.TargetStudent.StudentID != 8 && Yandere.TargetStudent.StudentID != 11 && Yandere.TargetStudent.StudentID != 25 && Yandere.TargetStudent.StudentID != 28 && Yandere.TargetStudent.StudentID != 30 && Yandere.TargetStudent.StudentID != 36 && Yandere.TargetStudent.StudentID != 37 && Yandere.TargetStudent.StudentID != 38 && Yandere.TargetStudent.StudentID != 52 && Yandere.TargetStudent.StudentID != 81)
			{
				UISprite uISprite13 = Shadow[5];
				uISprite13.color = new Color(uISprite13.color.r, uISprite13.color.g, uISprite13.color.b, 0.75f);
			}
			else
			{
				if ((Yandere.TargetStudent.TaskPhase > 0 && Yandere.TargetStudent.TaskPhase < 5) || Yandere.TargetStudent.TaskPhase == 100)
				{
					UISprite uISprite14 = Shadow[5];
					uISprite14.color = new Color(uISprite14.color.r, uISprite14.color.g, uISprite14.color.b, 0.75f);
				}
				if (Yandere.TargetStudent.StudentID == 36 && TaskGlobals.GetTaskStatus(36) == 0 && (StudentGlobals.GetStudentDead(81) || StudentGlobals.GetStudentDead(82) || StudentGlobals.GetStudentDead(83) || StudentGlobals.GetStudentDead(84) || StudentGlobals.GetStudentDead(85)))
				{
					UISprite uISprite15 = Shadow[5];
					uISprite15.color = new Color(uISprite15.color.r, uISprite15.color.g, uISprite15.color.b, 0.75f);
				}
				if (Yandere.TargetStudent.StudentID == 81)
				{
					if (Yandere.TargetStudent.Actions[Yandere.TargetStudent.Phase] != StudentActionType.Wait || Yandere.TargetStudent.DistanceToDestination > 1f)
					{
						UISprite uISprite16 = Shadow[5];
						uISprite16.color = new Color(uISprite16.color.r, uISprite16.color.g, uISprite16.color.b, 0.75f);
					}
					else if (TaskGlobals.GetTaskStatus(81) == 1 && Yandere.Inventory.Cigs)
					{
						UISprite uISprite17 = Shadow[5];
						uISprite17.color = new Color(uISprite17.color.r, uISprite17.color.g, uISprite17.color.b, 0f);
					}
				}
			}
		}
		else if (Yandere.TargetStudent.StudentID != 28 && Yandere.TargetStudent.StudentID != 30)
		{
			UISprite uISprite18 = Shadow[5];
			uISprite18.color = new Color(uISprite18.color.r, uISprite18.color.g, uISprite18.color.b, 0.75f);
		}
		else if (!Yandere.TargetStudent.Male && LoveManager.SuitorProgress == 0)
		{
			UISprite uISprite19 = Shadow[5];
			uISprite19.color = new Color(uISprite19.color.r, uISprite19.color.g, uISprite19.color.b, 0.75f);
		}
		if (!Yandere.TargetStudent.Indoors || Yandere.TargetStudent.Club == ClubType.Council)
		{
			UISprite uISprite20 = Shadow[6];
			uISprite20.color = new Color(uISprite20.color.r, uISprite20.color.g, uISprite20.color.b, 0.75f);
		}
		else
		{
			if (!PlayerGlobals.GetStudentFriend(Yandere.TargetStudent.StudentID))
			{
				UISprite uISprite21 = Shadow[6];
				uISprite21.color = new Color(uISprite21.color.r, uISprite21.color.g, uISprite21.color.b, 0.75f);
			}
			if ((Yandere.TargetStudent.Male && PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 3) || PlayerGlobals.Seduction + PlayerGlobals.SeductionBonus > 4)
			{
				UISprite uISprite22 = Shadow[6];
				uISprite22.color = new Color(uISprite22.color.r, uISprite22.color.g, uISprite22.color.b, 0f);
			}
		}
		if (ClubGlobals.Club == Yandere.TargetStudent.Club)
		{
			UISprite uISprite23 = ClubShadow[1];
			uISprite23.color = new Color(uISprite23.color.r, uISprite23.color.g, uISprite23.color.b, 0.75f);
			UISprite uISprite24 = ClubShadow[2];
			uISprite24.color = new Color(uISprite24.color.r, uISprite24.color.g, uISprite24.color.b, 0.75f);
		}
		if (Yandere.ClubAttire || Yandere.Mask != null || Yandere.Gloves != null || Yandere.Container != null)
		{
			UISprite uISprite25 = ClubShadow[3];
			uISprite25.color = new Color(uISprite25.color.r, uISprite25.color.g, uISprite25.color.b, 0.75f);
		}
		if (ClubGlobals.Club != Yandere.TargetStudent.Club)
		{
			UISprite uISprite26 = ClubShadow[2];
			uISprite26.color = new Color(uISprite26.color.r, uISprite26.color.g, uISprite26.color.b, 0f);
			UISprite uISprite27 = ClubShadow[3];
			uISprite27.color = new Color(uISprite27.color.r, uISprite27.color.g, uISprite27.color.b, 0.75f);
			UISprite uISprite28 = ClubShadow[5];
			uISprite28.color = new Color(uISprite28.color.r, uISprite28.color.g, uISprite28.color.b, 0.75f);
		}
		if (Yandere.StudentManager.MurderTakingPlace)
		{
			UISprite uISprite29 = ClubShadow[5];
			uISprite29.color = new Color(uISprite29.color.r, uISprite29.color.g, uISprite29.color.b, 0.75f);
		}
		if ((Yandere.TargetStudent.StudentID != 46 && Yandere.TargetStudent.StudentID != 51) || Yandere.Police.Show)
		{
			UISprite uISprite30 = ClubShadow[6];
			uISprite30.color = new Color(uISprite30.color.r, uISprite30.color.g, uISprite30.color.b, 0.75f);
		}
		if (Yandere.TargetStudent.StudentID == 51)
		{
			int num = 4;
			if (ClubGlobals.Club != ClubType.LightMusic || PracticeWindow.PlayedRhythmMinigame)
			{
				num = 0;
			}
			for (int m = 52; m < 56; m++)
			{
				if (Yandere.StudentManager.Students[m] == null)
				{
					num--;
				}
				else if (!Yandere.StudentManager.Students[m].gameObject.activeInHierarchy || Yandere.StudentManager.Students[m].Investigating || Yandere.StudentManager.Students[m].Distracting || Yandere.StudentManager.Students[m].Distracted || Yandere.StudentManager.Students[m].SentHome || Yandere.StudentManager.Students[m].Tranquil || Yandere.StudentManager.Students[m].GoAway || !Yandere.StudentManager.Students[m].Routine || !Yandere.StudentManager.Students[m].Alive)
				{
					num--;
				}
			}
			if (num < 4)
			{
				UISprite uISprite31 = ClubShadow[6];
				uISprite31.color = new Color(uISprite31.color.r, uISprite31.color.g, uISprite31.color.b, 0.75f);
			}
		}
		if (Yandere.Followers > 0)
		{
			UISprite uISprite32 = FavorShadow[1];
			uISprite32.color = new Color(uISprite32.color.r, uISprite32.color.g, uISprite32.color.b, 0.75f);
		}
		if (Yandere.TargetStudent.DistanceToDestination > 0.5f)
		{
			UISprite uISprite33 = FavorShadow[2];
			uISprite33.color = new Color(uISprite33.color.r, uISprite33.color.g, uISprite33.color.b, 0.75f);
		}
		if (!Yandere.TargetStudent.Male)
		{
			UISprite uISprite34 = LoveShadow[1];
			uISprite34.color = new Color(uISprite34.color.r, uISprite34.color.g, uISprite34.color.b, 0.75f);
		}
		if (DatingMinigame == null || !Yandere.Inventory.Headset || (Yandere.TargetStudent.Male && !LoveManager.RivalWaiting) || LoveManager.Courted)
		{
			UISprite uISprite35 = LoveShadow[2];
			uISprite35.color = new Color(uISprite35.color.r, uISprite35.color.g, uISprite35.color.b, 0.75f);
		}
		if (!Yandere.TargetStudent.Male || !Yandere.Inventory.Rose || Yandere.TargetStudent.Rose)
		{
			UISprite uISprite36 = LoveShadow[4];
			uISprite36.color = new Color(uISprite36.color.r, uISprite36.color.g, uISprite36.color.b, 0.75f);
		}
	}

	private void CheckTaskCompletion()
	{
		if (TaskGlobals.GetTaskStatus(Yandere.TargetStudent.StudentID) == 2 && Yandere.TargetStudent.StudentID == 81)
		{
			Yandere.Inventory.Cigs = false;
		}
	}

	public void End()
	{
		if (Yandere.TargetStudent != null)
		{
			if (Yandere.TargetStudent.Pestered >= 10)
			{
				Yandere.TargetStudent.Ignoring = true;
			}
			if (!Pestered)
			{
				Yandere.Subtitle.Label.text = string.Empty;
			}
			Yandere.TargetStudent.Interaction = StudentInteractionType.Idle;
			Yandere.TargetStudent.WaitTimer = 1f;
			if (Yandere.TargetStudent.enabled)
			{
				Debug.Log(Yandere.TargetStudent.Name + " has been told to travel to the destination of their current phase.");
				Yandere.TargetStudent.CurrentDestination = Yandere.TargetStudent.Destinations[Yandere.TargetStudent.Phase];
				Yandere.TargetStudent.Pathfinding.target = Yandere.TargetStudent.Destinations[Yandere.TargetStudent.Phase];
				if (Yandere.TargetStudent.Actions[Yandere.TargetStudent.Phase] == StudentActionType.Clean)
				{
					Yandere.TargetStudent.EquipCleaningItems();
				}
				if (Yandere.TargetStudent.Actions[Yandere.TargetStudent.Phase] == StudentActionType.Patrol)
				{
					Yandere.TargetStudent.CurrentDestination = Yandere.TargetStudent.StudentManager.Patrols.List[Yandere.TargetStudent.StudentID].GetChild(Yandere.TargetStudent.PatrolID);
					Yandere.TargetStudent.Pathfinding.target = Yandere.TargetStudent.CurrentDestination;
				}
				if (Yandere.TargetStudent.Actions[Yandere.TargetStudent.Phase] == StudentActionType.Sleuth)
				{
					Yandere.TargetStudent.CurrentDestination = Yandere.TargetStudent.SleuthTarget;
					Yandere.TargetStudent.Pathfinding.target = Yandere.TargetStudent.SleuthTarget;
				}
				if (Yandere.TargetStudent.Actions[Yandere.TargetStudent.Phase] == StudentActionType.Sunbathe && Yandere.TargetStudent.SunbathePhase > 1)
				{
					Yandere.TargetStudent.CurrentDestination = Yandere.StudentManager.SunbatheSpots[Yandere.TargetStudent.StudentID - 80];
					Yandere.TargetStudent.Pathfinding.target = Yandere.StudentManager.SunbatheSpots[Yandere.TargetStudent.StudentID - 80];
				}
			}
			if (Yandere.TargetStudent.Persona == PersonaType.PhoneAddict)
			{
				bool flag = false;
				if (Yandere.TargetStudent.CurrentAction == StudentActionType.Sunbathe && Yandere.TargetStudent.SunbathePhase > 2)
				{
					flag = true;
				}
				if (!Yandere.TargetStudent.Scrubber.activeInHierarchy && !flag)
				{
					Yandere.TargetStudent.SmartPhone.SetActive(true);
					Yandere.TargetStudent.WalkAnim = Yandere.TargetStudent.PhoneAnims[1];
				}
				else
				{
					Yandere.TargetStudent.SmartPhone.SetActive(false);
				}
			}
			if (Yandere.TargetStudent.LostTeacherTrust)
			{
				Yandere.TargetStudent.WalkAnim = Yandere.TargetStudent.BulliedWalkAnim;
				Yandere.TargetStudent.SmartPhone.SetActive(false);
			}
			if (Yandere.TargetStudent.EatingSnack)
			{
				Yandere.TargetStudent.Scrubber.SetActive(false);
				Yandere.TargetStudent.Eraser.SetActive(false);
			}
			if (Yandere.TargetStudent.SentToLocker)
			{
				Yandere.TargetStudent.CurrentDestination = Yandere.TargetStudent.MyLocker;
				Yandere.TargetStudent.Pathfinding.target = Yandere.TargetStudent.MyLocker;
			}
			Yandere.TargetStudent.Talk.NegativeResponse = false;
			Yandere.ShoulderCamera.OverShoulder = false;
			Yandere.TargetStudent.Waiting = true;
			Yandere.TargetStudent = null;
		}
		Yandere.StudentManager.VolumeUp();
		Jukebox.Dip = 1f;
		AskingFavor = false;
		Matchmaking = false;
		ClubLeader = false;
		Pestered = false;
		Show = false;
	}
}
