using UnityEngine;

public class HomeInternetScript : MonoBehaviour
{
	public StudentInfoMenuScript StudentInfoMenu;

	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public PromptBarScript PromptBar;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeWindowScript HomeWindow;

	public UILabel YanderePostLabel;

	public UILabel AcceptLabel;

	public GameObject InternetPrompts;

	public GameObject NewPostText;

	public GameObject ChangeLabel;

	public GameObject ChangeIcon;

	public GameObject WriteLabel;

	public GameObject WriteIcon;

	public GameObject PostLabel;

	public GameObject PostIcon;

	public GameObject BG;

	public Transform MenuHighlight;

	public Transform StudentPost1;

	public Transform StudentPost2;

	public Transform YandereReply;

	public Transform YanderePost;

	public Transform LameReply;

	public Transform NewPost;

	public Transform Menu;

	public Transform[] StudentReplies;

	public UISprite[] Highlights;

	public UILabel[] PostLabels;

	public UILabel[] MenuLabels;

	public string[] Locations;

	public string[] Actions;

	public bool PostSequence;

	public bool WritingPost;

	public bool ShowMenu;

	public bool FadeOut;

	public bool Success;

	public bool Posted;

	public int MenuSelected = 1;

	public int Selected = 1;

	public int ID = 1;

	public int Location;

	public int Student;

	public int Action;

	public float Timer;

	public UITexture StudentPost1Portrait;

	public UITexture StudentPost2Portrait;

	public UITexture LamePostPortrait;

	public Texture CurrentPortrait;

	public UITexture[] Portraits;

	private void Awake()
	{
		StudentPost1.localPosition = new Vector3(StudentPost1.localPosition.x, -180f, StudentPost1.localPosition.z);
		StudentPost2.localPosition = new Vector3(StudentPost2.localPosition.x, -365f, StudentPost2.localPosition.z);
		YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, -88f, YandereReply.localPosition.z);
		YanderePost.localPosition = new Vector3(YanderePost.localPosition.x, -2f, YanderePost.localPosition.z);
		for (int i = 1; i < 6; i++)
		{
			Transform transform = StudentReplies[i];
			transform.localPosition = new Vector3(transform.localPosition.x, -40f, transform.localPosition.z);
		}
		LameReply.localPosition = new Vector3(LameReply.localPosition.x, -40f, LameReply.localPosition.z);
		Highlights[1].enabled = false;
		Highlights[2].enabled = false;
		Highlights[3].enabled = false;
		YanderePost.gameObject.SetActive(false);
		ChangeLabel.SetActive(false);
		ChangeIcon.SetActive(false);
		PostLabel.SetActive(false);
		PostIcon.SetActive(false);
		NewPostText.SetActive(false);
		BG.SetActive(false);
		if (!EventGlobals.Event2 || StudentGlobals.GetStudentExposed(30))
		{
			WriteLabel.SetActive(false);
			WriteIcon.SetActive(false);
		}
		GetPortrait(2);
		StudentPost1Portrait.mainTexture = CurrentPortrait;
		GetPortrait(39);
		StudentPost2Portrait.mainTexture = CurrentPortrait;
		GetPortrait(25);
		LamePostPortrait.mainTexture = CurrentPortrait;
		for (ID = 1; ID < 6; ID++)
		{
			GetPortrait(86 - ID);
			Portraits[ID].mainTexture = CurrentPortrait;
		}
	}

	private void Update()
	{
		if (!HomeYandere.CanMove && !PauseScreen.Show)
		{
			Menu.localScale = Vector3.Lerp(Menu.localScale, (!ShowMenu) ? Vector3.zero : new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			if (WritingPost)
			{
				NewPost.transform.localPosition = Vector3.Lerp(NewPost.transform.localPosition, Vector3.zero, Time.deltaTime * 10f);
				NewPost.transform.localScale = Vector3.Lerp(NewPost.transform.localScale, new Vector3(2.43f, 2.43f, 2.43f), Time.deltaTime * 10f);
				for (int i = 1; i < Highlights.Length; i++)
				{
					UISprite uISprite = Highlights[i];
					uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, Mathf.MoveTowards(uISprite.color.a, (!FadeOut) ? 1f : 0f, Time.deltaTime));
				}
				if (Highlights[Selected].color.a == 1f)
				{
					FadeOut = true;
				}
				else if (Highlights[Selected].color.a == 0f)
				{
					FadeOut = false;
				}
				if (!ShowMenu)
				{
					if (InputManager.TappedRight)
					{
						Selected++;
						UpdateHighlight();
					}
					if (InputManager.TappedLeft)
					{
						Selected--;
						UpdateHighlight();
					}
				}
				else
				{
					if (InputManager.TappedDown)
					{
						MenuSelected++;
						UpdateMenuHighlight();
					}
					if (InputManager.TappedUp)
					{
						MenuSelected--;
						UpdateMenuHighlight();
					}
				}
			}
			else
			{
				NewPost.transform.localPosition = Vector3.Lerp(NewPost.transform.localPosition, new Vector3(175f, -10f, 0f), Time.deltaTime * 10f);
				NewPost.transform.localScale = Vector3.Lerp(NewPost.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			}
			if (!PostSequence)
			{
				if (Input.GetButtonDown("A") && WriteIcon.activeInHierarchy && !Posted)
				{
					if (!ShowMenu)
					{
						if (!WritingPost)
						{
							AcceptLabel.text = "Select";
							ChangeLabel.SetActive(true);
							ChangeIcon.SetActive(true);
							NewPostText.SetActive(true);
							BG.SetActive(true);
							WritingPost = true;
							Selected = 1;
							UpdateHighlight();
						}
						else if (Selected == 1)
						{
							PauseScreen.MainMenu.SetActive(false);
							PauseScreen.Panel.enabled = true;
							PauseScreen.Sideways = true;
							PauseScreen.Show = true;
							StudentInfoMenu.gameObject.SetActive(true);
							StudentInfoMenu.CyberBullying = true;
							StartCoroutine(StudentInfoMenu.UpdatePortraits());
							PromptBar.ClearButtons();
							PromptBar.Label[0].text = "View Info";
							PromptBar.Label[1].text = "Back";
							PromptBar.UpdateButtons();
							PromptBar.Show = true;
						}
						else if (Selected == 2)
						{
							MenuSelected = 1;
							UpdateMenuHighlight();
							ShowMenu = true;
							for (int j = 1; j < MenuLabels.Length; j++)
							{
								MenuLabels[j].text = Locations[j];
							}
						}
						else if (Selected == 3)
						{
							MenuSelected = 1;
							UpdateMenuHighlight();
							ShowMenu = true;
							for (int k = 1; k < MenuLabels.Length; k++)
							{
								MenuLabels[k].text = Actions[k];
							}
						}
					}
					else
					{
						if (Selected == 2)
						{
							Location = MenuSelected;
							PostLabels[2].text = Locations[MenuSelected];
							ShowMenu = false;
						}
						else if (Selected == 3)
						{
							Action = MenuSelected;
							PostLabels[3].text = Actions[MenuSelected];
							ShowMenu = false;
						}
						CheckForCompletion();
					}
				}
				if (Input.GetButtonDown("B"))
				{
					if (!ShowMenu)
					{
						if (!WritingPost)
						{
							HomeCamera.Destination = HomeCamera.Destinations[0];
							HomeCamera.Target = HomeCamera.Targets[0];
							HomeYandere.CanMove = true;
							HomeWindow.Show = false;
							base.enabled = false;
						}
						else
						{
							AcceptLabel.text = "Write";
							ChangeLabel.SetActive(false);
							ChangeIcon.SetActive(false);
							PostLabel.SetActive(false);
							PostIcon.SetActive(false);
							ExitPost();
						}
					}
					else
					{
						ShowMenu = false;
					}
				}
				if (Input.GetButtonDown("X") && PostIcon.activeInHierarchy)
				{
					YanderePostLabel.text = "Today, I saw " + PostLabels[1].text + " in " + PostLabels[2].text + ". She was " + PostLabels[3].text + ".";
					ExitPost();
					InternetPrompts.SetActive(false);
					ChangeLabel.SetActive(false);
					ChangeIcon.SetActive(false);
					WriteLabel.SetActive(false);
					WriteIcon.SetActive(false);
					PostLabel.SetActive(false);
					PostIcon.SetActive(false);
					PostSequence = true;
					Posted = true;
					if (Student == 30 && Location == 7 && Action == 9)
					{
						Success = true;
					}
				}
				if (Input.GetKeyDown("space"))
				{
					WriteLabel.SetActive(true);
					WriteIcon.SetActive(true);
				}
			}
			if (PostSequence)
			{
				if (Input.GetButtonDown("A"))
				{
					Timer += 2f;
				}
				Timer += Time.deltaTime;
				if (Timer > 1f && Timer < 3f)
				{
					YanderePost.gameObject.SetActive(true);
					YanderePost.transform.localPosition = new Vector3(YanderePost.transform.localPosition.x, Mathf.Lerp(YanderePost.transform.localPosition.y, -155f, Time.deltaTime * 10f), YanderePost.transform.localPosition.z);
					StudentPost1.transform.localPosition = new Vector3(StudentPost1.transform.localPosition.x, Mathf.Lerp(StudentPost1.transform.localPosition.y, -365f, Time.deltaTime * 10f), StudentPost1.transform.localPosition.z);
					StudentPost2.transform.localPosition = new Vector3(StudentPost2.transform.localPosition.x, Mathf.Lerp(StudentPost2.transform.localPosition.y, -550f, Time.deltaTime * 10f), StudentPost2.transform.localPosition.z);
				}
				if (!Success)
				{
					if (Timer > 3f && Timer < 5f)
					{
						LameReply.localPosition = new Vector3(LameReply.localPosition.x, Mathf.Lerp(LameReply.transform.localPosition.y, -88f, Time.deltaTime * 10f), LameReply.localPosition.z);
						YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, Mathf.Lerp(YandereReply.transform.localPosition.y, -137f, Time.deltaTime * 10f), YandereReply.localPosition.z);
						StudentPost1.localPosition = new Vector3(StudentPost1.localPosition.x, Mathf.Lerp(StudentPost1.transform.localPosition.y, -415f, Time.deltaTime * 10f), StudentPost1.localPosition.z);
					}
					if (Timer > 5f)
					{
						PlayerGlobals.Reputation -= 5f;
						InternetPrompts.SetActive(true);
						PostSequence = false;
					}
				}
				else
				{
					if (Timer > 3f && Timer < 5f)
					{
						Transform transform = StudentReplies[1];
						transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.transform.localPosition.y, -88f, Time.deltaTime * 10f), transform.localPosition.z);
						YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, Mathf.Lerp(YandereReply.transform.localPosition.y, -137f, Time.deltaTime * 10f), YandereReply.localPosition.z);
						StudentPost1.localPosition = new Vector3(StudentPost1.localPosition.x, Mathf.Lerp(StudentPost1.transform.localPosition.y, -415f, Time.deltaTime * 10f), StudentPost1.localPosition.z);
					}
					if (Timer > 5f && Timer < 7f)
					{
						Transform transform2 = StudentReplies[2];
						transform2.localPosition = new Vector3(transform2.localPosition.x, Mathf.Lerp(transform2.transform.localPosition.y, -88f, Time.deltaTime * 10f), transform2.localPosition.z);
						Transform transform3 = StudentReplies[1];
						transform3.localPosition = new Vector3(transform3.localPosition.x, Mathf.Lerp(transform3.transform.localPosition.y, -136f, Time.deltaTime * 10f), transform3.localPosition.z);
						YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, Mathf.Lerp(YandereReply.transform.localPosition.y, -185f, Time.deltaTime * 10f), YandereReply.localPosition.z);
						StudentPost1.localPosition = new Vector3(StudentPost1.localPosition.x, Mathf.Lerp(StudentPost1.transform.localPosition.y, -465f, Time.deltaTime * 10f), StudentPost1.localPosition.z);
					}
					if (Timer > 7f && Timer < 9f)
					{
						Transform transform4 = StudentReplies[3];
						transform4.localPosition = new Vector3(transform4.localPosition.x, Mathf.Lerp(transform4.transform.localPosition.y, -88f, Time.deltaTime * 10f), transform4.localPosition.z);
						Transform transform5 = StudentReplies[2];
						transform5.localPosition = new Vector3(transform5.localPosition.x, Mathf.Lerp(transform5.transform.localPosition.y, -136f, Time.deltaTime * 10f), transform5.localPosition.z);
						Transform transform6 = StudentReplies[1];
						transform6.localPosition = new Vector3(transform6.localPosition.x, Mathf.Lerp(transform6.transform.localPosition.y, -184f, Time.deltaTime * 10f), transform6.localPosition.z);
						YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, Mathf.Lerp(YandereReply.transform.localPosition.y, -233f, Time.deltaTime * 10f), YandereReply.localPosition.z);
						StudentPost1.localPosition = new Vector3(StudentPost1.localPosition.x, Mathf.Lerp(StudentPost1.transform.localPosition.y, -510f, Time.deltaTime * 10f), StudentPost1.localPosition.z);
					}
					if (Timer > 9f && Timer < 11f)
					{
						Transform transform7 = StudentReplies[4];
						transform7.localPosition = new Vector3(transform7.localPosition.x, Mathf.Lerp(transform7.transform.localPosition.y, -88f, Time.deltaTime * 10f), transform7.localPosition.z);
						Transform transform8 = StudentReplies[3];
						transform8.localPosition = new Vector3(transform8.localPosition.x, Mathf.Lerp(transform8.transform.localPosition.y, -136f, Time.deltaTime * 10f), transform8.localPosition.z);
						Transform transform9 = StudentReplies[2];
						transform9.localPosition = new Vector3(transform9.localPosition.x, Mathf.Lerp(transform9.transform.localPosition.y, -184f, Time.deltaTime * 10f), transform9.localPosition.z);
						Transform transform10 = StudentReplies[1];
						transform10.localPosition = new Vector3(transform10.localPosition.x, Mathf.Lerp(transform10.transform.localPosition.y, -232f, Time.deltaTime * 10f), transform10.localPosition.z);
						YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, Mathf.Lerp(YandereReply.transform.localPosition.y, -281f, Time.deltaTime * 10f), YandereReply.localPosition.z);
						StudentPost1.localPosition = new Vector3(StudentPost1.localPosition.x, Mathf.Lerp(StudentPost1.transform.localPosition.y, -560f, Time.deltaTime * 10f), StudentPost1.localPosition.z);
					}
					if (Timer > 11f && Timer < 13f)
					{
						Transform transform11 = StudentReplies[5];
						transform11.localPosition = new Vector3(transform11.localPosition.x, Mathf.Lerp(transform11.transform.localPosition.y, -88f, Time.deltaTime * 10f), transform11.localPosition.z);
						Transform transform12 = StudentReplies[4];
						transform12.localPosition = new Vector3(transform12.localPosition.x, Mathf.Lerp(transform12.transform.localPosition.y, -136f, Time.deltaTime * 10f), transform12.localPosition.z);
						Transform transform13 = StudentReplies[3];
						transform13.localPosition = new Vector3(transform13.localPosition.x, Mathf.Lerp(transform13.transform.localPosition.y, -184f, Time.deltaTime * 10f), transform13.localPosition.z);
						Transform transform14 = StudentReplies[2];
						transform14.localPosition = new Vector3(transform14.localPosition.x, Mathf.Lerp(transform14.transform.localPosition.y, -232f, Time.deltaTime * 10f), transform14.localPosition.z);
						Transform transform15 = StudentReplies[1];
						transform15.localPosition = new Vector3(transform15.localPosition.x, Mathf.Lerp(transform15.transform.localPosition.y, -280f, Time.deltaTime * 10f), transform15.localPosition.z);
						YandereReply.localPosition = new Vector3(YandereReply.localPosition.x, Mathf.Lerp(YandereReply.transform.localPosition.y, -329f, Time.deltaTime * 10f), YandereReply.localPosition.z);
					}
					if (Timer > 13f)
					{
						StudentGlobals.SetStudentExposed(30, true);
						StudentGlobals.SetStudentReputation(30, StudentGlobals.GetStudentReputation(30) - 50);
						InternetPrompts.SetActive(true);
						PostSequence = false;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StudentGlobals.SetStudentExposed(7, false);
		}
	}

	private void ExitPost()
	{
		Highlights[1].enabled = false;
		Highlights[2].enabled = false;
		Highlights[3].enabled = false;
		NewPostText.SetActive(false);
		BG.SetActive(false);
		PostLabels[1].text = string.Empty;
		PostLabels[2].text = string.Empty;
		PostLabels[3].text = string.Empty;
		WritingPost = false;
	}

	private void UpdateHighlight()
	{
		if (Selected > 3)
		{
			Selected = 1;
		}
		if (Selected < 1)
		{
			Selected = 3;
		}
		Highlights[1].enabled = false;
		Highlights[2].enabled = false;
		Highlights[3].enabled = false;
		Highlights[Selected].enabled = true;
	}

	private void UpdateMenuHighlight()
	{
		if (MenuSelected > 10)
		{
			MenuSelected = 1;
		}
		if (MenuSelected < 1)
		{
			MenuSelected = 10;
		}
		MenuHighlight.transform.localPosition = new Vector3(MenuHighlight.transform.localPosition.x, 220f - 40f * (float)MenuSelected, MenuHighlight.transform.localPosition.z);
	}

	private void CheckForCompletion()
	{
		if (PostLabels[1].text != string.Empty && PostLabels[2].text != string.Empty && PostLabels[3].text != string.Empty)
		{
			PostLabel.SetActive(true);
			PostIcon.SetActive(true);
		}
	}

	private void GetPortrait(int ID)
	{
		string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + ID + ".png";
		WWW wWW = new WWW(url);
		CurrentPortrait = wWW.texture;
	}
}
