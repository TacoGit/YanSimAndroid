using UnityEngine;

public class NewMissionWindowScript : MonoBehaviour
{
	public MissionModeMenuScript MissionModeMenu;

	public InputManagerScript InputManager;

	public JsonScript JSON;

	public GameObject[] DeathSkulls;

	public GameObject[] Button;

	public UILabel[] MethodLabel;

	public UILabel[] NameLabel;

	public UITexture[] Portrait;

	public int[] UnsafeNumbers;

	public int[] Target;

	public int[] Method;

	public string[] MethodNames;

	public int Selected;

	public int Column;

	public int Row;

	public Transform Highlight;

	public Texture BlankPortrait;

	private void Start()
	{
		UpdateHighlight();
		for (int i = 1; i < 11; i++)
		{
			Portrait[i].mainTexture = BlankPortrait;
			NameLabel[i].text = "Kill: (Nobody)";
			MethodLabel[i].text = "By: Attacking";
			DeathSkulls[i].SetActive(false);
		}
	}

	private void Update()
	{
		if (InputManager.TappedDown)
		{
			Row++;
			UpdateHighlight();
		}
		if (InputManager.TappedUp)
		{
			Row--;
			UpdateHighlight();
		}
		if (InputManager.TappedRight)
		{
			Column++;
			UpdateHighlight();
		}
		if (InputManager.TappedLeft)
		{
			Column--;
			UpdateHighlight();
		}
		int num = 0;
		if (Input.GetButtonDown("A"))
		{
			int num2 = 0;
			for (int i = 0; i < 11; i++)
			{
				if (Target[i] > 0)
				{
					num2++;
				}
			}
			if (Row == 5)
			{
				if (Column == 1)
				{
					if (num2 > 0)
					{
						Globals.DeleteAll();
						SaveInfo();
						MissionModeMenu.GetComponent<AudioSource>().PlayOneShot(MissionModeMenu.InfoLines[6]);
						SchoolGlobals.SchoolAtmosphere = 1f - (float)num2 * 0.1f;
						SchoolGlobals.SchoolAtmosphereSet = true;
						MissionModeGlobals.MissionMode = true;
						MissionModeGlobals.MultiMission = true;
						MissionModeGlobals.MissionDifficulty = num2;
						ClassGlobals.BiologyGrade = 1;
						ClassGlobals.ChemistryGrade = 1;
						ClassGlobals.LanguageGrade = 1;
						ClassGlobals.PhysicalGrade = 1;
						ClassGlobals.PsychologyGrade = 1;
						MissionModeMenu.PromptBar.Show = false;
						MissionModeMenu.Speed = 0f;
						MissionModeMenu.Phase = 4;
						base.enabled = false;
					}
				}
				else if (Column == 2)
				{
					Randomize();
				}
			}
		}
		if (Input.GetButtonDown("B"))
		{
			MissionModeMenu.PromptBar.ClearButtons();
			MissionModeMenu.PromptBar.Label[0].text = "Accept";
			MissionModeMenu.PromptBar.Label[4].text = "Choose";
			MissionModeMenu.PromptBar.UpdateButtons();
			MissionModeMenu.PromptBar.Show = true;
			MissionModeMenu.TargetID = 0;
			MissionModeMenu.Phase = 2;
		}
		if (Input.GetButtonDown("X"))
		{
			if (Row == 1)
			{
				for (int j = 1; j < 11; j++)
				{
					UnsafeNumbers[j] = Target[j];
				}
				Increment(0);
				if (Target[Column] != 0)
				{
					while (Target[Column] == UnsafeNumbers[1] || Target[Column] == UnsafeNumbers[2] || Target[Column] == UnsafeNumbers[3] || Target[Column] == UnsafeNumbers[4] || Target[Column] == UnsafeNumbers[5] || Target[Column] == UnsafeNumbers[6] || Target[Column] == UnsafeNumbers[7] || Target[Column] == UnsafeNumbers[8] || Target[Column] == UnsafeNumbers[9] || Target[Column] == UnsafeNumbers[10])
					{
						Increment(0);
					}
				}
			}
			else if (Row == 2)
			{
				Method[Column]++;
				if (Method[Column] == MethodNames.Length)
				{
					Method[Column] = 0;
				}
				MethodLabel[Column].text = "By: " + MethodNames[Method[Column]];
			}
			else if (Row == 3)
			{
				for (int k = 1; k < 11; k++)
				{
					UnsafeNumbers[k] = Target[k];
				}
				Increment(5);
				if (Target[Column] != 0)
				{
					while (Target[Column + 5] == UnsafeNumbers[1] || Target[Column + 5] == UnsafeNumbers[2] || Target[Column + 5] == UnsafeNumbers[3] || Target[Column + 5] == UnsafeNumbers[4] || Target[Column + 5] == UnsafeNumbers[5] || Target[Column + 5] == UnsafeNumbers[6] || Target[Column + 5] == UnsafeNumbers[7] || Target[Column + 5] == UnsafeNumbers[8] || Target[Column + 5] == UnsafeNumbers[9] || Target[Column + 5] == UnsafeNumbers[10])
					{
						Increment(5);
					}
				}
			}
			else if (Row == 4)
			{
				Method[Column + 5]++;
				if (Method[Column + 5] == MethodNames.Length)
				{
					Method[Column + 5] = 0;
				}
				MethodLabel[Column + 5].text = "By: " + MethodNames[Method[Column + 5]];
			}
		}
		if (Input.GetButtonDown("Y"))
		{
			if (Row == 1)
			{
				for (int l = 1; l < 11; l++)
				{
					UnsafeNumbers[l] = Target[l];
				}
				Decrement(0);
				if (Target[Column] != 0)
				{
					while (Target[Column] == UnsafeNumbers[1] || Target[Column] == UnsafeNumbers[2] || Target[Column] == UnsafeNumbers[3] || Target[Column] == UnsafeNumbers[4] || Target[Column] == UnsafeNumbers[5] || Target[Column] == UnsafeNumbers[6] || Target[Column] == UnsafeNumbers[7] || Target[Column] == UnsafeNumbers[8] || Target[Column] == UnsafeNumbers[9] || Target[Column] == UnsafeNumbers[10])
					{
						Decrement(0);
					}
				}
			}
			else if (Row == 2)
			{
				Method[Column]--;
				if (Method[Column] < 0)
				{
					Method[Column] = MethodNames.Length - 1;
				}
				MethodLabel[Column].text = "By: " + MethodNames[Method[Column]];
			}
			else if (Row == 3)
			{
				for (int m = 1; m < 11; m++)
				{
					UnsafeNumbers[m] = Target[m];
				}
				Decrement(5);
				if (Target[Column] != 0)
				{
					while (Target[Column + 5] == UnsafeNumbers[1] || Target[Column + 5] == UnsafeNumbers[2] || Target[Column + 5] == UnsafeNumbers[3] || Target[Column + 5] == UnsafeNumbers[4] || Target[Column + 5] == UnsafeNumbers[5] || Target[Column + 5] == UnsafeNumbers[6] || Target[Column + 5] == UnsafeNumbers[7] || Target[Column + 5] == UnsafeNumbers[8] || Target[Column + 5] == UnsafeNumbers[9] || Target[Column + 5] == UnsafeNumbers[10])
					{
						Decrement(5);
					}
				}
			}
			else if (Row == 4)
			{
				Method[Column + 5]--;
				if (Method[Column + 5] < 0)
				{
					Method[Column + 5] = MethodNames.Length - 1;
				}
				MethodLabel[Column + 5].text = "By: " + MethodNames[Method[Column + 5]];
			}
		}
		if (Input.GetKeyDown("space"))
		{
			FillOutInfo();
		}
	}

	private void Increment(int Number)
	{
		Target[Column + Number]++;
		if (Target[Column + Number] == 1)
		{
			Target[Column + Number] = 2;
		}
		else if (Target[Column + Number] == 10)
		{
			Target[Column + Number] = 21;
		}
		else if (Target[Column + Number] > 89)
		{
			Target[Column + Number] = 0;
		}
		if (Target[Column + Number] == 0)
		{
			NameLabel[Column + Number].text = "Kill: Nobody";
		}
		else
		{
			NameLabel[Column + Number].text = "Kill: " + JSON.Students[Target[Column + Number]].Name;
		}
		string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + Target[Column + Number] + ".png";
		WWW wWW = new WWW(url);
		Portrait[Column + Number].mainTexture = wWW.texture;
	}

	private void Decrement(int Number)
	{
		Target[Column + Number]--;
		if (Target[Column + Number] == 1)
		{
			Target[Column + Number] = 0;
		}
		else if (Target[Column + Number] == 20)
		{
			Target[Column + Number] = 9;
		}
		else if (Target[Column + Number] == -1)
		{
			Target[Column + Number] = 89;
		}
		if (Target[Column + Number] == 0)
		{
			NameLabel[Column + Number].text = "Kill: Nobody";
		}
		else
		{
			NameLabel[Column + Number].text = "Kill: " + JSON.Students[Target[Column + Number]].Name;
		}
		string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + Target[Column + Number] + ".png";
		WWW wWW = new WWW(url);
		Portrait[Column + Number].mainTexture = wWW.texture;
	}

	private void Randomize()
	{
		int i;
		for (i = 1; i < 11; i++)
		{
			Target[i] = Random.Range(2, 89);
			Method[i] = Random.Range(0, 7);
			MethodLabel[i].text = "By: " + MethodNames[Method[i]];
		}
		i = 1;
		Column = 0;
		for (; i < 11; i++)
		{
			for (int j = 1; j < 11; j++)
			{
				UnsafeNumbers[j] = Target[j];
			}
			while (Target[i] == UnsafeNumbers[1] || Target[i] == UnsafeNumbers[2] || Target[i] == UnsafeNumbers[3] || Target[i] == UnsafeNumbers[4] || Target[i] == UnsafeNumbers[5] || Target[i] == UnsafeNumbers[6] || Target[i] == UnsafeNumbers[7] || Target[i] == UnsafeNumbers[8] || Target[i] == UnsafeNumbers[9] || Target[i] == UnsafeNumbers[10] || Target[i] == 0 || (Target[i] > 9 && Target[i] < 21))
			{
				Increment(i);
			}
		}
		Column = 2;
	}

	public void UpdateHighlight()
	{
		MissionModeMenu.PromptBar.Label[0].text = string.Empty;
		if (Row < 1)
		{
			Row = 5;
		}
		else if (Row > 5)
		{
			Row = 1;
		}
		if (Row < 5)
		{
			if (Column < 1)
			{
				Column = 5;
			}
			else if (Column > 5)
			{
				Column = 1;
			}
			int num = 0;
			if (Row == 1)
			{
				num = 225;
			}
			else if (Row == 2)
			{
				num = 125;
			}
			else if (Row == 3)
			{
				num = -300;
			}
			else if (Row == 4)
			{
				num = -400;
			}
			Highlight.localPosition = new Vector3(-1200 + 400 * Column, num, 0f);
			return;
		}
		if (Column < 1)
		{
			Column = 3;
		}
		else if (Column > 3)
		{
			Column = 1;
		}
		Highlight.localPosition = new Vector3(-1200 + 600 * Column, -525f, 0f);
		if (Column == 1)
		{
			if (Target[1] + Target[2] + Target[3] + Target[4] + Target[5] + Target[6] + Target[7] + Target[8] + Target[9] + Target[10] == 0)
			{
				MissionModeMenu.PromptBar.Label[0].text = string.Empty;
			}
			else
			{
				MissionModeMenu.PromptBar.Label[0].text = "Confirm";
			}
		}
		else if (Column == 2)
		{
			MissionModeMenu.PromptBar.Label[0].text = "Confirm";
		}
		else
		{
			MissionModeMenu.PromptBar.Label[0].text = string.Empty;
		}
		MissionModeMenu.PromptBar.UpdateButtons();
	}

	private void SaveInfo()
	{
		for (int i = 1; i < 11; i++)
		{
			PlayerPrefs.SetInt("MissionModeTarget" + i, Target[i]);
			PlayerPrefs.SetInt("MissionModeMethod" + i, Method[i]);
		}
	}

	public void FillOutInfo()
	{
		for (int i = 1; i < 11; i++)
		{
			Target[i] = PlayerPrefs.GetInt("MissionModeTarget" + i);
			Method[i] = PlayerPrefs.GetInt("MissionModeMethod" + i);
			if (Target[i] == 0)
			{
				NameLabel[i].text = "Kill: Nobody";
			}
			else
			{
				NameLabel[i].text = "Kill: " + JSON.Students[Target[i]].Name;
			}
			string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + Target[i] + ".png";
			WWW wWW = new WWW(url);
			Portrait[i].mainTexture = wWW.texture;
			MethodLabel[i].text = "By: " + MethodNames[Method[i]];
			DeathSkulls[i].SetActive(false);
		}
	}

	public void HideButtons()
	{
		Button[0].SetActive(false);
		Button[1].SetActive(false);
		Button[2].SetActive(false);
		Button[3].SetActive(false);
	}
}
