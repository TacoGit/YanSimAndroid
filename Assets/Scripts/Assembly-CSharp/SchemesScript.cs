using UnityEngine;

public class SchemesScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public PromptBarScript PromptBar;

	public GameObject FavorMenu;

	public Transform Highlight;

	public Transform Arrow;

	public UILabel SchemeInstructions;

	public UITexture SchemeIcon;

	public UILabel PantyCount;

	public UILabel SchemeDesc;

	public UILabel[] SchemeDeadlineLabels;

	public UILabel[] SchemeCostLabels;

	public UILabel[] SchemeNameLabels;

	public UISprite[] Exclamations;

	public Texture[] SchemeIcons;

	public int[] SchemeCosts;

	public string[] SchemeDeadlines;

	public string[] SchemeSkills;

	public string[] SchemeDescs;

	public string[] SchemeNames;

	public string[] SchemeSteps;

	public int ID = 1;

	public string[] Steps;

	public AudioClip InfoPurchase;

	public AudioClip InfoAfford;

	public GameObject HUDIcon;

	public UILabel HUDInstructions;

	private void Start()
	{
		for (int i = 1; i < SchemeNames.Length; i++)
		{
			if (!SchemeGlobals.GetSchemeStatus(i))
			{
				SchemeDeadlineLabels[i].text = SchemeDeadlines[i];
				SchemeNameLabels[i].text = SchemeNames[i];
			}
		}
	}

	private void Update()
	{
		if (InputManager.TappedUp)
		{
			ID--;
			if (ID < 1)
			{
				ID = SchemeNames.Length - 1;
			}
			UpdateSchemeInfo();
		}
		if (InputManager.TappedDown)
		{
			ID++;
			if (ID > SchemeNames.Length - 1)
			{
				ID = 1;
			}
			UpdateSchemeInfo();
		}
		if (Input.GetButtonDown("A"))
		{
			AudioSource component = GetComponent<AudioSource>();
			if (PromptBar.Label[0].text != string.Empty)
			{
				if (!SchemeGlobals.GetSchemeUnlocked(ID))
				{
					if (PlayerGlobals.PantyShots >= SchemeCosts[ID])
					{
						PlayerGlobals.PantyShots -= SchemeCosts[ID];
						SchemeGlobals.SetSchemeUnlocked(ID, true);
						SchemeGlobals.CurrentScheme = ID;
						if (SchemeGlobals.GetSchemeStage(ID) == 0)
						{
							SchemeGlobals.SetSchemeStage(ID, 1);
						}
						UpdateInstructions();
						UpdateSchemeList();
						UpdateSchemeInfo();
						component.clip = InfoPurchase;
						component.Play();
					}
				}
				else
				{
					if (SchemeGlobals.CurrentScheme == ID)
					{
						SchemeGlobals.CurrentScheme = 0;
					}
					else
					{
						SchemeGlobals.CurrentScheme = ID;
					}
					UpdateSchemeInfo();
					UpdateInstructions();
				}
			}
			else if (SchemeGlobals.GetSchemeStage(ID) != 100 && PlayerGlobals.PantyShots < SchemeCosts[ID])
			{
				component.clip = InfoAfford;
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

	public void UpdateSchemeList()
	{
		for (int i = 1; i < SchemeNames.Length; i++)
		{
			if (SchemeGlobals.GetSchemeStage(i) == 100)
			{
				UILabel uILabel = SchemeNameLabels[i];
				uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
				Exclamations[i].enabled = false;
				SchemeCostLabels[i].text = string.Empty;
				continue;
			}
			SchemeCostLabels[i].text = (SchemeGlobals.GetSchemeUnlocked(i) ? string.Empty : SchemeCosts[i].ToString());
			if (SchemeGlobals.GetSchemeStage(i) > SchemeGlobals.GetSchemePreviousStage(i))
			{
				SchemeGlobals.SetSchemePreviousStage(i, SchemeGlobals.GetSchemeStage(i));
				Exclamations[i].enabled = true;
			}
			else
			{
				Exclamations[i].enabled = false;
			}
		}
	}

	public void UpdateSchemeInfo()
	{
		if (SchemeGlobals.GetSchemeStage(ID) != 100)
		{
			if (!SchemeGlobals.GetSchemeUnlocked(ID))
			{
				Arrow.gameObject.SetActive(false);
				PromptBar.Label[0].text = ((PlayerGlobals.PantyShots < SchemeCosts[ID]) ? string.Empty : "Purchase");
				PromptBar.UpdateButtons();
			}
			else if (SchemeGlobals.CurrentScheme == ID)
			{
				Arrow.gameObject.SetActive(true);
				Arrow.localPosition = new Vector3(Arrow.localPosition.x, -17f - 28f * (float)SchemeGlobals.GetSchemeStage(ID), Arrow.localPosition.z);
				PromptBar.Label[0].text = "Stop Tracking";
				PromptBar.UpdateButtons();
			}
			else
			{
				Arrow.gameObject.SetActive(false);
				PromptBar.Label[0].text = "Start Tracking";
				PromptBar.UpdateButtons();
			}
		}
		else
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		Highlight.localPosition = new Vector3(Highlight.localPosition.x, 200f - 25f * (float)ID, Highlight.localPosition.z);
		SchemeIcon.mainTexture = SchemeIcons[ID];
		SchemeDesc.text = SchemeDescs[ID];
		if (SchemeGlobals.GetSchemeStage(ID) == 100)
		{
			SchemeInstructions.text = "This scheme is no longer available.";
		}
		else
		{
			SchemeInstructions.text = (SchemeGlobals.GetSchemeUnlocked(ID) ? SchemeSteps[ID] : ("Skills Required:\n" + SchemeSkills[ID]));
		}
		UpdatePantyCount();
	}

	public void UpdatePantyCount()
	{
		PantyCount.text = PlayerGlobals.PantyShots.ToString();
	}

	public void UpdateInstructions()
	{
		Steps = SchemeSteps[SchemeGlobals.CurrentScheme].Split('\n');
		if (SchemeGlobals.CurrentScheme > 0)
		{
			if (SchemeGlobals.GetSchemeStage(SchemeGlobals.CurrentScheme) < 100)
			{
				HUDIcon.SetActive(true);
				HUDInstructions.text = Steps[SchemeGlobals.GetSchemeStage(SchemeGlobals.CurrentScheme) - 1].ToString();
				return;
			}
			Arrow.gameObject.SetActive(false);
			HUDIcon.gameObject.SetActive(false);
			HUDInstructions.text = string.Empty;
			SchemeGlobals.CurrentScheme = 0;
		}
		else
		{
			HUDIcon.SetActive(false);
			HUDInstructions.text = string.Empty;
		}
	}
}
