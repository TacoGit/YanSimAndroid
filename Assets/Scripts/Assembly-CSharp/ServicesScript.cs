using UnityEngine;

public class ServicesScript : MonoBehaviour
{
	public TextMessageManagerScript TextMessageManager;

	public StudentManagerScript StudentManager;

	public InputManagerScript InputManager;

	public ReputationScript Reputation;

	public PromptBarScript PromptBar;

	public SchemesScript Schemes;

	public YandereScript Yandere;

	public GameObject FavorMenu;

	public Transform Highlight;

	public PoliceScript Police;

	public UITexture ServiceIcon;

	public UILabel ServiceLimit;

	public UILabel ServiceDesc;

	public UILabel PantyCount;

	public UILabel[] CostLabels;

	public UILabel[] NameLabels;

	public Texture[] ServiceIcons;

	public string[] ServiceLimits;

	public string[] ServiceDescs;

	public string[] ServiceNames;

	public bool[] ServiceAvailable;

	public bool[] ServicePurchased;

	public int[] ServiceCosts;

	public int Selected = 1;

	public int ID = 1;

	public AudioClip InfoUnavailable;

	public AudioClip InfoPurchase;

	public AudioClip InfoAfford;

	private void Start()
	{
		for (int i = 1; i < ServiceNames.Length; i++)
		{
			SchemeGlobals.SetServicePurchased(i, false);
			NameLabels[i].text = ServiceNames[i];
		}
	}

	private void Update()
	{
		if (InputManager.TappedUp)
		{
			Selected--;
			if (Selected < 1)
			{
				Selected = ServiceNames.Length - 1;
			}
			UpdateDesc();
		}
		if (InputManager.TappedDown)
		{
			Selected++;
			if (Selected > ServiceNames.Length - 1)
			{
				Selected = 1;
			}
			UpdateDesc();
		}
		AudioSource component = GetComponent<AudioSource>();
		if (Input.GetButtonDown("A"))
		{
			if (!SchemeGlobals.GetServicePurchased(Selected) && (double)NameLabels[Selected].color.a == 1.0)
			{
				if (PromptBar.Label[0].text != string.Empty)
				{
					if (PlayerGlobals.PantyShots >= ServiceCosts[Selected])
					{
						if (Selected == 1)
						{
							Yandere.PauseScreen.StudentInfoMenu.GettingInfo = true;
							Yandere.PauseScreen.StudentInfoMenu.gameObject.SetActive(true);
							StartCoroutine(Yandere.PauseScreen.StudentInfoMenu.UpdatePortraits());
							Yandere.PauseScreen.StudentInfoMenu.Column = 0;
							Yandere.PauseScreen.StudentInfoMenu.Row = 0;
							Yandere.PauseScreen.StudentInfoMenu.UpdateHighlight();
							Yandere.PauseScreen.Sideways = true;
							Yandere.PromptBar.ClearButtons();
							Yandere.PromptBar.Label[1].text = "Cancel";
							Yandere.PromptBar.UpdateButtons();
							Yandere.PromptBar.Show = true;
							base.gameObject.SetActive(false);
						}
						if (Selected == 2)
						{
							Reputation.PendingRep += 5f;
							Purchase();
						}
						else if (Selected == 3)
						{
							StudentGlobals.SetStudentReputation(StudentManager.RivalID, StudentGlobals.GetStudentReputation(StudentManager.RivalID) - 5);
							Purchase();
						}
						else if (Selected == 4)
						{
							SchemeGlobals.SetServicePurchased(Selected, true);
							SchemeGlobals.SetSchemeStage(1, 2);
							SchemeGlobals.DarkSecret = true;
							Schemes.UpdateInstructions();
							Purchase();
						}
						else if (Selected == 5)
						{
							Yandere.PauseScreen.StudentInfoMenu.SendingHome = true;
							Yandere.PauseScreen.StudentInfoMenu.gameObject.SetActive(true);
							StartCoroutine(Yandere.PauseScreen.StudentInfoMenu.UpdatePortraits());
							Yandere.PauseScreen.StudentInfoMenu.Column = 0;
							Yandere.PauseScreen.StudentInfoMenu.Row = 0;
							Yandere.PauseScreen.StudentInfoMenu.UpdateHighlight();
							Yandere.PauseScreen.Sideways = true;
							Yandere.PromptBar.ClearButtons();
							Yandere.PromptBar.Label[1].text = "Cancel";
							Yandere.PromptBar.UpdateButtons();
							Yandere.PromptBar.Show = true;
							base.gameObject.SetActive(false);
						}
						else if (Selected == 6)
						{
							Police.Timer += 300f;
							Police.Delayed = true;
							Purchase();
						}
						else if (Selected == 7)
						{
							PlayerPrefs.SetInt("CounselorTape", 1);
							Purchase();
						}
					}
				}
				else if (PlayerGlobals.PantyShots < ServiceCosts[Selected])
				{
					component.clip = InfoAfford;
					component.Play();
				}
				else
				{
					component.clip = InfoUnavailable;
					component.Play();
				}
			}
			else
			{
				component.clip = InfoUnavailable;
				component.Play();
			}
		}
		if (Input.GetButtonDown("B"))
		{
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Accept";
			PromptBar.Label[1].text = "Exit";
			PromptBar.Label[5].text = "Choose";
			PromptBar.UpdateButtons();
			FavorMenu.SetActive(true);
			base.gameObject.SetActive(false);
		}
	}

	public void UpdateList()
	{
		for (ID = 1; ID < ServiceNames.Length; ID++)
		{
			CostLabels[ID].text = ServiceCosts[ID].ToString();
			bool servicePurchased = SchemeGlobals.GetServicePurchased(ID);
			ServiceAvailable[ID] = false;
			if (ID == 1 || ID == 2 || ID == 3)
			{
				ServiceAvailable[ID] = true;
			}
			else if (ID == 4)
			{
				if (!SchemeGlobals.DarkSecret)
				{
					ServiceAvailable[ID] = true;
				}
			}
			else if (ID == 5)
			{
				if (!ServicePurchased[ID])
				{
					ServiceAvailable[ID] = true;
				}
			}
			else if (ID == 6)
			{
				if (Police.Show && !Police.Delayed)
				{
					ServiceAvailable[ID] = true;
				}
			}
			else if (ID == 7 && PlayerPrefs.GetInt("CounselorTape") == 0)
			{
				ServiceAvailable[ID] = true;
			}
			UILabel uILabel = NameLabels[ID];
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, (!ServiceAvailable[ID] || servicePurchased) ? 0.5f : 1f);
		}
	}

	public void UpdateDesc()
	{
		if (ServiceAvailable[Selected] && !SchemeGlobals.GetServicePurchased(Selected))
		{
			PromptBar.Label[0].text = ((PlayerGlobals.PantyShots < ServiceCosts[Selected]) ? string.Empty : "Purchase");
			PromptBar.UpdateButtons();
		}
		else
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		Highlight.localPosition = new Vector3(Highlight.localPosition.x, 200f - 25f * (float)Selected, Highlight.localPosition.z);
		ServiceIcon.mainTexture = ServiceIcons[Selected];
		ServiceLimit.text = ServiceLimits[Selected];
		ServiceDesc.text = ServiceDescs[Selected];
		if (Selected == 5)
		{
			ServiceDesc.text = ServiceDescs[Selected] + "\n\nIf student portraits don't appear, back out of the menu, load the Student Info menu, then return to this screen.";
		}
		UpdatePantyCount();
	}

	public void UpdatePantyCount()
	{
		PantyCount.text = PlayerGlobals.PantyShots.ToString();
	}

	public void Purchase()
	{
		ServicePurchased[Selected] = true;
		TextMessageManager.SpawnMessage(Selected);
		PlayerGlobals.PantyShots -= ServiceCosts[Selected];
		AudioSource.PlayClipAtPoint(InfoPurchase, base.transform.position);
		UpdateList();
		UpdateDesc();
		PromptBar.Label[0].text = string.Empty;
		PromptBar.Label[1].text = "Back";
		PromptBar.UpdateButtons();
	}
}
