using UnityEngine;

public class DropsScript : MonoBehaviour
{
	public InfoChanWindowScript InfoChanWindow;

	public InputManagerScript InputManager;

	public PromptBarScript PromptBar;

	public SchemesScript Schemes;

	public GameObject FavorMenu;

	public Transform Highlight;

	public UILabel PantyCount;

	public UITexture DropIcon;

	public UILabel DropDesc;

	public UILabel[] CostLabels;

	public UILabel[] NameLabels;

	public bool[] Purchased;

	public Texture[] DropIcons;

	public int[] DropCosts;

	public string[] DropDescs;

	public string[] DropNames;

	public int Selected = 1;

	public int ID = 1;

	public AudioClip InfoUnavailable;

	public AudioClip InfoPurchase;

	public AudioClip InfoAfford;

	private void Start()
	{
		for (ID = 1; ID < DropNames.Length; ID++)
		{
			NameLabels[ID].text = DropNames[ID];
		}
	}

	private void Update()
	{
		if (InputManager.TappedUp)
		{
			Selected--;
			if (Selected < 1)
			{
				Selected = DropNames.Length - 1;
			}
			UpdateDesc();
		}
		if (InputManager.TappedDown)
		{
			Selected++;
			if (Selected > DropNames.Length - 1)
			{
				Selected = 1;
			}
			UpdateDesc();
		}
		if (Input.GetButtonDown("A"))
		{
			AudioSource component = GetComponent<AudioSource>();
			if (!Purchased[Selected])
			{
				if (PromptBar.Label[0].text != string.Empty)
				{
					if (PlayerGlobals.PantyShots >= DropCosts[Selected])
					{
						PlayerGlobals.PantyShots -= DropCosts[Selected];
						Purchased[Selected] = true;
						InfoChanWindow.Orders++;
						InfoChanWindow.ItemsToDrop[InfoChanWindow.Orders] = Selected;
						InfoChanWindow.DropObject();
						UpdateList();
						UpdateDesc();
						component.clip = InfoPurchase;
						component.Play();
						if (Selected == 2)
						{
							SchemeGlobals.SetSchemeStage(3, 2);
							Schemes.UpdateInstructions();
						}
					}
				}
				else if (PlayerGlobals.PantyShots < DropCosts[Selected])
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
		for (ID = 1; ID < DropNames.Length; ID++)
		{
			UILabel uILabel = NameLabels[ID];
			if (!Purchased[ID])
			{
				CostLabels[ID].text = DropCosts[ID].ToString();
				uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 1f);
			}
			else
			{
				CostLabels[ID].text = string.Empty;
				uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
			}
		}
	}

	public void UpdateDesc()
	{
		if (!Purchased[Selected])
		{
			if (PlayerGlobals.PantyShots >= DropCosts[Selected])
			{
				PromptBar.Label[0].text = "Purchase";
				PromptBar.UpdateButtons();
			}
			else
			{
				PromptBar.Label[0].text = string.Empty;
				PromptBar.UpdateButtons();
			}
		}
		else
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
		Highlight.localPosition = new Vector3(Highlight.localPosition.x, 200f - 25f * (float)Selected, Highlight.localPosition.z);
		DropIcon.mainTexture = DropIcons[Selected];
		DropDesc.text = DropDescs[Selected];
		UpdatePantyCount();
	}

	public void UpdatePantyCount()
	{
		PantyCount.text = PlayerGlobals.PantyShots.ToString();
	}
}
