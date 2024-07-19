using System.IO;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
	public PauseScreenScript PauseScreen;

	public PromptBarScript PromptBar;

	public UISprite[] Subject1Bars;

	public UISprite[] Subject2Bars;

	public UISprite[] Subject3Bars;

	public UISprite[] Subject4Bars;

	public UISprite[] Subject5Bars;

	public UISprite[] Subject6Bars;

	public UISprite[] Subject7Bars;

	public UISprite[] Subject8Bars;

	public UILabel[] Ranks;

	public UILabel ClubLabel;

	public int Grade;

	public int BarID;

	public UITexture Portrait;

	private ClubTypeAndStringDictionary ClubLabels;

	private void Awake()
	{
		ClubLabels = new ClubTypeAndStringDictionary
		{
			{
				ClubType.None,
				"None"
			},
			{
				ClubType.Cooking,
				"Cooking"
			},
			{
				ClubType.Drama,
				"Drama"
			},
			{
				ClubType.Occult,
				"Occult"
			},
			{
				ClubType.Art,
				"Art"
			},
			{
				ClubType.LightMusic,
				"Light Music"
			},
			{
				ClubType.MartialArts,
				"Martial Arts"
			},
			{
				ClubType.Photography,
				"Photography"
			},
			{
				ClubType.Science,
				"Science"
			},
			{
				ClubType.Sports,
				"Sports"
			},
			{
				ClubType.Gardening,
				"Gardening"
			},
			{
				ClubType.Gaming,
				"Gaming"
			}
		};
	}

	private void Start()
	{
		if (File.Exists(Application.streamingAssetsPath + "/CustomPortrait.txt"))
		{
			string text = File.ReadAllText(Application.streamingAssetsPath + "/CustomPortrait.txt");
			if (text == "1")
			{
				string url = "file:///" + Application.streamingAssetsPath + "/CustomPortrait.png";
				WWW wWW = new WWW(url);
				Portrait.mainTexture = wWW.texture;
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backslash))
		{
			ClassGlobals.BiologyGrade = 1;
			ClassGlobals.ChemistryGrade = 5;
			ClassGlobals.LanguageGrade = 2;
			ClassGlobals.PhysicalGrade = 4;
			ClassGlobals.PsychologyGrade = 3;
			PlayerGlobals.Seduction = 4;
			PlayerGlobals.Numbness = 2;
			PlayerGlobals.Enlightenment = 5;
			UpdateStats();
		}
		if (Input.GetButtonDown("B"))
		{
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Accept";
			PromptBar.Label[1].text = "Exit";
			PromptBar.Label[4].text = "Choose";
			PromptBar.UpdateButtons();
			PauseScreen.MainMenu.SetActive(true);
			PauseScreen.Sideways = false;
			PauseScreen.PressedB = true;
			base.gameObject.SetActive(false);
		}
	}

	public void UpdateStats()
	{
		Grade = ClassGlobals.BiologyGrade;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite = Subject1Bars[BarID];
			if (Grade > 0)
			{
				uISprite.color = new Color(1f, 1f, 1f, 1f);
				Grade--;
			}
			else
			{
				uISprite.color = new Color(1f, 1f, 1f, 0.5f);
			}
		}
		if (ClassGlobals.BiologyGrade < 5)
		{
			Subject1Bars[ClassGlobals.BiologyGrade + 1].color = ((ClassGlobals.BiologyBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = ClassGlobals.ChemistryGrade;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite2 = Subject2Bars[BarID];
			if (Grade > 0)
			{
				uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite2.color = new Color(uISprite2.color.r, uISprite2.color.g, uISprite2.color.b, 0.5f);
			}
		}
		if (ClassGlobals.ChemistryGrade < 5)
		{
			Subject2Bars[ClassGlobals.ChemistryGrade + 1].color = ((ClassGlobals.ChemistryBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = ClassGlobals.LanguageGrade;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite3 = Subject3Bars[BarID];
			if (Grade > 0)
			{
				uISprite3.color = new Color(uISprite3.color.r, uISprite3.color.g, uISprite3.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite3.color = new Color(uISprite3.color.r, uISprite3.color.g, uISprite3.color.b, 0.5f);
			}
		}
		if (ClassGlobals.LanguageGrade < 5)
		{
			Subject3Bars[ClassGlobals.LanguageGrade + 1].color = ((ClassGlobals.LanguageBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = ClassGlobals.PhysicalGrade;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite4 = Subject4Bars[BarID];
			if (Grade > 0)
			{
				uISprite4.color = new Color(uISprite4.color.r, uISprite4.color.g, uISprite4.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite4.color = new Color(uISprite4.color.r, uISprite4.color.g, uISprite4.color.b, 0.5f);
			}
		}
		if (ClassGlobals.PhysicalGrade < 5)
		{
			Subject4Bars[ClassGlobals.PhysicalGrade + 1].color = ((ClassGlobals.PhysicalBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = ClassGlobals.PsychologyGrade;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite5 = Subject5Bars[BarID];
			if (Grade > 0)
			{
				uISprite5.color = new Color(uISprite5.color.r, uISprite5.color.g, uISprite5.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite5.color = new Color(uISprite5.color.r, uISprite5.color.g, uISprite5.color.b, 0.5f);
			}
		}
		if (ClassGlobals.PsychologyGrade < 5)
		{
			Subject5Bars[ClassGlobals.PsychologyGrade + 1].color = ((ClassGlobals.PsychologyBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = PlayerGlobals.Seduction;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite6 = Subject6Bars[BarID];
			if (Grade > 0)
			{
				uISprite6.color = new Color(uISprite6.color.r, uISprite6.color.g, uISprite6.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite6.color = new Color(uISprite6.color.r, uISprite6.color.g, uISprite6.color.b, 0.5f);
			}
		}
		if (PlayerGlobals.Seduction < 5)
		{
			Subject6Bars[PlayerGlobals.Seduction + 1].color = ((PlayerGlobals.SeductionBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = PlayerGlobals.Numbness;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite7 = Subject7Bars[BarID];
			if (Grade > 0)
			{
				uISprite7.color = new Color(uISprite7.color.r, uISprite7.color.g, uISprite7.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite7.color = new Color(uISprite7.color.r, uISprite7.color.g, uISprite7.color.b, 0.5f);
			}
		}
		if (PlayerGlobals.Numbness < 5)
		{
			Subject7Bars[PlayerGlobals.Numbness + 1].color = ((PlayerGlobals.NumbnessBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Grade = PlayerGlobals.Enlightenment;
		for (BarID = 1; BarID < 6; BarID++)
		{
			UISprite uISprite8 = Subject8Bars[BarID];
			if (Grade > 0)
			{
				uISprite8.color = new Color(uISprite8.color.r, uISprite8.color.g, uISprite8.color.b, 1f);
				Grade--;
			}
			else
			{
				uISprite8.color = new Color(uISprite8.color.r, uISprite8.color.g, uISprite8.color.b, 0.5f);
			}
		}
		if (PlayerGlobals.Enlightenment < 5)
		{
			Subject8Bars[PlayerGlobals.Enlightenment + 1].color = ((PlayerGlobals.EnlightenmentBonus <= 0) ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 1f));
		}
		Ranks[1].text = "Rank: " + ClassGlobals.BiologyGrade;
		Ranks[2].text = "Rank: " + ClassGlobals.ChemistryGrade;
		Ranks[3].text = "Rank: " + ClassGlobals.LanguageGrade;
		Ranks[4].text = "Rank: " + ClassGlobals.PhysicalGrade;
		Ranks[5].text = "Rank: " + ClassGlobals.PsychologyGrade;
		Ranks[6].text = "Rank: " + PlayerGlobals.Seduction;
		Ranks[7].text = "Rank: " + PlayerGlobals.Numbness;
		Ranks[8].text = "Rank: " + PlayerGlobals.Enlightenment;
		ClubType club = ClubGlobals.Club;
		string value;
		bool flag = ClubLabels.TryGetValue(club, out value);
		ClubLabel.text = "Club: " + value;
	}
}
