using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunScript : MonoBehaviour
{
	public TypewriterEffect Typewriter;

	public GameObject Controls;

	public GameObject Skip;

	public Texture[] Portraits;

	public string[] Lines;

	public UITexture Girl;

	public UILabel Label;

	public float OutroTimer;

	public float Timer;

	public int DebugNumber;

	public int ID;

	public bool VeryFun;

	public float R = 1f;

	public float G = 1f;

	public float B = 1f;

	public string Text;

	private void Start()
	{
		if (PlayerPrefs.GetInt("DebugNumber") > 0)
		{
			if (PlayerPrefs.GetInt("DebugNumber") > 10)
			{
				PlayerPrefs.SetInt("DebugNumber", 0);
			}
			DebugNumber = PlayerPrefs.GetInt("DebugNumber");
		}
		if (VeryFun)
		{
			if (DebugNumber != -1)
			{
				Text = string.Empty + DebugNumber;
			}
			else
			{
				Text = File.ReadAllText(Application.streamingAssetsPath + "/Fun.txt");
			}
			if (Text == "0")
			{
				ID = 0;
			}
			else if (Text == "1")
			{
				ID = 1;
			}
			else if (Text == "2")
			{
				ID = 2;
			}
			else if (Text == "3")
			{
				ID = 3;
			}
			else if (Text == "4")
			{
				ID = 4;
			}
			else if (Text == "5")
			{
				ID = 5;
			}
			else if (Text == "6")
			{
				ID = 6;
			}
			else if (Text == "7")
			{
				ID = 7;
			}
			else if (Text == "8")
			{
				ID = 8;
			}
			else if (Text == "9")
			{
				ID = 9;
			}
			else if (Text == "10")
			{
				ID = 10;
			}
			else if (Text == "69")
			{
				Label.text = "( \u0361° \u035cʖ \u0361°) ";
				ID = 8;
			}
			else if (Text == "666")
			{
				Label.text = "Sometimes, I lie. It's just too fun. You eat up everything I say. I wonder what else I can trick you into believing? ";
				Girl.color = new Color(1f, 0f, 0f, 0f);
				Label.color = new Color(1f, 0f, 0f, 1f);
				ID = 5;
			}
			else
			{
				Application.LoadLevel("WelcomeScene");
			}
		}
		if (Text != "666" && Text != "69")
		{
			Label.text = Lines[ID];
		}
		if (SceneManager.GetActiveScene().name == "MoreFunScene" || Text == "666")
		{
			G = 0f;
			B = 0f;
			Label.color = new Color(R, G, B, 1f);
			Skip.SetActive(false);
		}
		if (SceneManager.GetActiveScene().name == "VeryFunScene")
		{
			Skip.SetActive(false);
		}
		Controls.SetActive(false);
		Label.gameObject.SetActive(false);
		Girl.color = new Color(R, G, B, 0f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(",") && PlayerPrefs.GetInt("DebugNumber") > 0)
		{
			PlayerPrefs.SetInt("DebugNumber", PlayerPrefs.GetInt("DebugNumber") - 1);
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKeyDown(".") && PlayerPrefs.GetInt("DebugNumber") < 10)
		{
			PlayerPrefs.SetInt("DebugNumber", PlayerPrefs.GetInt("DebugNumber") + 1);
			Application.LoadLevel(Application.loadedLevel);
		}
		Timer += Time.deltaTime;
		if (Timer > 3f)
		{
			if (!Typewriter.mActive)
			{
				Controls.SetActive(true);
			}
		}
		else if (Timer > 2f)
		{
			Girl.mainTexture = Portraits[ID];
			Label.gameObject.SetActive(true);
		}
		else if (Timer > 1f)
		{
			Girl.color = new Color(R, G, B, Mathf.MoveTowards(Girl.color.a, 1f, Time.deltaTime));
		}
		if (!Controls.activeInHierarchy)
		{
			return;
		}
		if (Input.GetButtonDown("B"))
		{
			if (Skip.activeInHierarchy)
			{
				ID = 19;
				Skip.SetActive(false);
				Girl.mainTexture = Portraits[ID];
				Typewriter.ResetToBeginning();
				Typewriter.mFullText = Lines[ID];
			}
		}
		else
		{
			if (!Input.GetButtonDown("A"))
			{
				return;
			}
			if (ID < Lines.Length - 1 && !VeryFun)
			{
				if (Typewriter.mCurrentOffset < Typewriter.mFullText.Length)
				{
					Typewriter.Finish();
					return;
				}
				ID++;
				if (ID == 19)
				{
					Skip.SetActive(false);
				}
				Girl.mainTexture = Portraits[ID];
				Typewriter.ResetToBeginning();
				Typewriter.mFullText = Lines[ID];
			}
			else
			{
				Application.Quit();
			}
		}
	}
}
