using UnityEngine;

public class AppearanceWindowScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public InputManagerScript InputManager;

	public PromptBarScript PromptBar;

	public YandereScript Yandere;

	public Transform Highlight;

	public Transform Window;

	public GameObject[] Checks;

	public int Selected;

	public bool Ready;

	public bool Show;

	private void Start()
	{
		Window.localScale = Vector3.zero;
		for (int i = 1; i < 10; i++)
		{
			Checks[i].SetActive(DatingGlobals.GetSuitorCheck(i));
		}
	}

	private void Update()
	{
		if (!Show)
		{
			if (Window.gameObject.activeInHierarchy)
			{
				if (Window.localScale.x > 0.1f)
				{
					Window.localScale = Vector3.Lerp(Window.localScale, Vector3.zero, Time.deltaTime * 10f);
					return;
				}
				Window.localScale = Vector3.zero;
				Window.gameObject.SetActive(false);
			}
			return;
		}
		Window.localScale = Vector3.Lerp(Window.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		if (Ready)
		{
			if (InputManager.TappedUp)
			{
				Selected--;
				if (Selected == 10)
				{
					Selected = 9;
				}
				UpdateHighlight();
			}
			if (InputManager.TappedDown)
			{
				Selected++;
				if (Selected == 10)
				{
					Selected = 11;
				}
				UpdateHighlight();
			}
			if (Input.GetButtonDown("A"))
			{
				if (Selected == 1)
				{
					if (!Checks[1].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorHair = 22;
						DatingGlobals.SetSuitorCheck(1, true);
						DatingGlobals.SetSuitorCheck(2, false);
						Checks[1].SetActive(true);
						Checks[2].SetActive(false);
					}
					else
					{
						StudentGlobals.CustomSuitorHair = 0;
						DatingGlobals.SetSuitorCheck(1, false);
						Checks[1].SetActive(false);
					}
				}
				else if (Selected == 2)
				{
					if (!Checks[2].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorHair = 21;
						DatingGlobals.SetSuitorCheck(1, false);
						DatingGlobals.SetSuitorCheck(2, true);
						Checks[1].SetActive(false);
						Checks[2].SetActive(true);
					}
					else
					{
						StudentGlobals.CustomSuitorHair = 0;
						DatingGlobals.SetSuitorCheck(2, false);
						Checks[2].SetActive(false);
					}
				}
				else if (Selected == 3)
				{
					if (!Checks[3].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorAccessory = 3;
						DatingGlobals.SetSuitorCheck(3, true);
						DatingGlobals.SetSuitorCheck(4, false);
						Checks[3].SetActive(true);
						Checks[4].SetActive(false);
					}
					else
					{
						StudentGlobals.CustomSuitorAccessory = 0;
						DatingGlobals.SetSuitorCheck(3, false);
						Checks[3].SetActive(false);
					}
				}
				else if (Selected == 4)
				{
					if (!Checks[4].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorAccessory = 1;
						DatingGlobals.SetSuitorCheck(3, false);
						DatingGlobals.SetSuitorCheck(4, true);
						Checks[3].SetActive(false);
						Checks[4].SetActive(true);
					}
					else
					{
						StudentGlobals.CustomSuitorAccessory = 0;
						DatingGlobals.SetSuitorCheck(4, false);
						Checks[4].SetActive(false);
					}
				}
				else if (Selected == 5)
				{
					if (!Checks[5].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorBlonde = 1;
						DatingGlobals.SetSuitorCheck(5, true);
						Checks[5].SetActive(true);
					}
					else
					{
						StudentGlobals.CustomSuitorBlonde = 0;
						DatingGlobals.SetSuitorCheck(5, false);
						Checks[5].SetActive(false);
					}
				}
				else if (Selected == 6)
				{
					if (!Checks[6].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorEyewear = 6;
						DatingGlobals.SetSuitorCheck(6, true);
						DatingGlobals.SetSuitorCheck(8, false);
						Checks[6].SetActive(true);
						Checks[8].SetActive(false);
					}
					else
					{
						StudentGlobals.CustomSuitorEyewear = 0;
						DatingGlobals.SetSuitorCheck(6, false);
						Checks[6].SetActive(false);
					}
				}
				else if (Selected == 7)
				{
					if (!Checks[7].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorJewelry = 1;
						DatingGlobals.SetSuitorCheck(7, true);
						Checks[7].SetActive(true);
					}
					else
					{
						StudentGlobals.CustomSuitorJewelry = 0;
						DatingGlobals.SetSuitorCheck(7, false);
						Checks[7].SetActive(false);
					}
				}
				else if (Selected == 8)
				{
					if (!Checks[8].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorEyewear = 7;
						DatingGlobals.SetSuitorCheck(6, false);
						DatingGlobals.SetSuitorCheck(8, true);
						Checks[6].SetActive(false);
						Checks[8].SetActive(true);
					}
					else
					{
						StudentGlobals.CustomSuitorEyewear = 0;
						DatingGlobals.SetSuitorCheck(8, false);
						Checks[8].SetActive(false);
					}
				}
				else if (Selected == 9)
				{
					if (!Checks[9].activeInHierarchy)
					{
						StudentGlobals.CustomSuitorTan = true;
						DatingGlobals.SetSuitorCheck(9, true);
						Checks[9].SetActive(true);
					}
					else
					{
						StudentGlobals.CustomSuitorTan = false;
						DatingGlobals.SetSuitorCheck(9, false);
						Checks[9].SetActive(false);
					}
				}
				else if (Selected == 11)
				{
					StudentGlobals.CustomSuitor = true;
					PromptBar.ClearButtons();
					PromptBar.UpdateButtons();
					PromptBar.Show = false;
					Yandere.Interaction = YandereInteractionType.ChangingAppearance;
					Yandere.TalkTimer = 3f;
					Ready = false;
					Show = false;
				}
			}
		}
		if (Input.GetButtonUp("A"))
		{
			Ready = true;
		}
	}

	private void UpdateHighlight()
	{
		if (Selected < 1)
		{
			Selected = 11;
		}
		else if (Selected > 11)
		{
			Selected = 1;
		}
		Highlight.transform.localPosition = new Vector3(Highlight.transform.localPosition.x, 300f - 50f * (float)Selected, Highlight.transform.localPosition.z);
	}

	private void Exit()
	{
		Selected = 1;
		UpdateHighlight();
		PromptBar.ClearButtons();
		PromptBar.Show = false;
		Show = false;
	}
}
