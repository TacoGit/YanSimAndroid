using UnityEngine;

public class ClassScript : MonoBehaviour
{
	public CutsceneManagerScript CutsceneManager;

	public InputManagerScript InputManager;

	public PromptBarScript PromptBar;

	public SchemesScript Schemes;

	public PortalScript Portal;

	public GameObject Poison;

	public UILabel StudyPointsLabel;

	public UILabel[] SubjectLabels;

	public UILabel GradeUpDesc;

	public UILabel GradeUpName;

	public UILabel DescLabel;

	public UISprite Darkness;

	public Transform[] Subject1Bars;

	public Transform[] Subject2Bars;

	public Transform[] Subject3Bars;

	public Transform[] Subject4Bars;

	public Transform[] Subject5Bars;

	public string[] Subject1GradeText;

	public string[] Subject2GradeText;

	public string[] Subject3GradeText;

	public string[] Subject4GradeText;

	public string[] Subject5GradeText;

	public Transform GradeUpWindow;

	public Transform Highlight;

	public int[] SubjectTemp;

	public int[] Subject;

	public string[] Desc;

	public int GradeUpSubject;

	public int StudyPoints;

	public int Selected;

	public int Grade;

	public bool GradeUp;

	public bool Show;

	private void Start()
	{
		GradeUpWindow.localScale = Vector3.zero;
		Subject[1] = ClassGlobals.Biology;
		Subject[2] = ClassGlobals.Chemistry;
		Subject[3] = ClassGlobals.Language;
		Subject[4] = ClassGlobals.Physical;
		Subject[5] = ClassGlobals.Psychology;
		DescLabel.text = Desc[Selected];
		UpdateSubjectLabels();
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 1f);
		UpdateBars();
	}

	private void Update()
	{
		if (Show)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a - Time.deltaTime);
			if (!(Darkness.color.a <= 0f))
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Backslash))
			{
				GivePoints();
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				MaxPhysical();
			}
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 0f);
			if (InputManager.TappedDown)
			{
				Selected++;
				if (Selected > 5)
				{
					Selected = 1;
				}
				Highlight.localPosition = new Vector3(Highlight.localPosition.x, 375f - 125f * (float)Selected, Highlight.localPosition.z);
				DescLabel.text = Desc[Selected];
				UpdateSubjectLabels();
			}
			if (InputManager.TappedUp)
			{
				Selected--;
				if (Selected < 1)
				{
					Selected = 5;
				}
				Highlight.localPosition = new Vector3(Highlight.localPosition.x, 375f - 125f * (float)Selected, Highlight.localPosition.z);
				DescLabel.text = Desc[Selected];
				UpdateSubjectLabels();
			}
			if (InputManager.TappedRight && StudyPoints > 0 && Subject[Selected] + SubjectTemp[Selected] < 100)
			{
				SubjectTemp[Selected]++;
				StudyPoints--;
				UpdateLabel();
				UpdateBars();
			}
			if (InputManager.TappedLeft && SubjectTemp[Selected] > 0)
			{
				SubjectTemp[Selected]--;
				StudyPoints++;
				UpdateLabel();
				UpdateBars();
			}
			if (Input.GetButtonDown("A") && StudyPoints == 0)
			{
				Show = false;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
				ClassGlobals.Biology = Subject[1] + SubjectTemp[1];
				ClassGlobals.Chemistry = Subject[2] + SubjectTemp[2];
				ClassGlobals.Language = Subject[3] + SubjectTemp[3];
				ClassGlobals.Physical = Subject[4] + SubjectTemp[4];
				ClassGlobals.Psychology = Subject[5] + SubjectTemp[5];
				for (int i = 0; i < 6; i++)
				{
					Subject[i] += SubjectTemp[i];
					SubjectTemp[i] = 0;
				}
				CheckForGradeUp();
			}
			return;
		}
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a + Time.deltaTime);
		if (!(Darkness.color.a >= 1f))
		{
			return;
		}
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 1f);
		if (!GradeUp)
		{
			if (GradeUpWindow.localScale.x > 0.1f)
			{
				GradeUpWindow.localScale = Vector3.Lerp(GradeUpWindow.localScale, Vector3.zero, Time.deltaTime * 10f);
			}
			else
			{
				GradeUpWindow.localScale = Vector3.zero;
			}
			if (!(GradeUpWindow.localScale.x < 0.01f))
			{
				return;
			}
			GradeUpWindow.localScale = Vector3.zero;
			CheckForGradeUp();
			if (!GradeUp)
			{
				if (ClassGlobals.ChemistryGrade > 0 && Poison != null)
				{
					Poison.SetActive(true);
				}
				if (SchemeGlobals.GetSchemeStage(5) == 7)
				{
					SchemeGlobals.SetSchemeStage(5, 100);
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Continue";
					PromptBar.UpdateButtons();
					CutsceneManager.gameObject.SetActive(true);
					Schemes.UpdateInstructions();
					base.gameObject.SetActive(false);
				}
				else if (!Portal.FadeOut)
				{
					PromptBar.Show = false;
					Portal.Proceed = true;
					base.gameObject.SetActive(false);
				}
			}
			return;
		}
		if (GradeUpWindow.localScale.x == 0f)
		{
			if (GradeUpSubject == 1)
			{
				GradeUpName.text = "BIOLOGY RANK UP";
				GradeUpDesc.text = Subject1GradeText[Grade];
			}
			else if (GradeUpSubject == 2)
			{
				GradeUpName.text = "CHEMISTRY RANK UP";
				GradeUpDesc.text = Subject2GradeText[Grade];
			}
			else if (GradeUpSubject == 3)
			{
				GradeUpName.text = "LANGUAGE RANK UP";
				GradeUpDesc.text = Subject3GradeText[Grade];
			}
			else if (GradeUpSubject == 4)
			{
				GradeUpName.text = "PHYSICAL RANK UP";
				GradeUpDesc.text = Subject4GradeText[Grade];
			}
			else if (GradeUpSubject == 5)
			{
				GradeUpName.text = "PSYCHOLOGY RANK UP";
				GradeUpDesc.text = Subject5GradeText[Grade];
			}
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Continue";
			PromptBar.UpdateButtons();
			PromptBar.Show = true;
		}
		else if (GradeUpWindow.localScale.x > 0.99f && Input.GetButtonDown("A"))
		{
			PromptBar.ClearButtons();
			GradeUp = false;
		}
		GradeUpWindow.localScale = Vector3.Lerp(GradeUpWindow.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
	}

	private void UpdateSubjectLabels()
	{
		for (int i = 1; i < 6; i++)
		{
			SubjectLabels[i].color = new Color(0f, 0f, 0f, 1f);
		}
		SubjectLabels[Selected].color = new Color(1f, 1f, 1f, 1f);
	}

	public void UpdateLabel()
	{
		StudyPointsLabel.text = "STUDY POINTS: " + StudyPoints;
		if (StudyPoints == 0)
		{
			PromptBar.Label[0].text = "Confirm";
			PromptBar.UpdateButtons();
		}
		else
		{
			PromptBar.Label[0].text = string.Empty;
			PromptBar.UpdateButtons();
		}
	}

	private void UpdateBars()
	{
		for (int i = 1; i < 6; i++)
		{
			Transform transform = Subject1Bars[i];
			if (Subject[1] + SubjectTemp[1] > (i - 1) * 20)
			{
				transform.localScale = new Vector3(0f - (float)((i - 1) * 20 - (Subject[1] + SubjectTemp[1])) / 20f, transform.localScale.y, transform.localScale.z);
				if (transform.localScale.x > 1f)
				{
					transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
				}
			}
			else
			{
				transform.localScale = new Vector3(0f, transform.localScale.y, transform.localScale.z);
			}
		}
		for (int j = 1; j < 6; j++)
		{
			Transform transform2 = Subject2Bars[j];
			if (Subject[2] + SubjectTemp[2] > (j - 1) * 20)
			{
				transform2.localScale = new Vector3(0f - (float)((j - 1) * 20 - (Subject[2] + SubjectTemp[2])) / 20f, transform2.localScale.y, transform2.localScale.z);
				if (transform2.localScale.x > 1f)
				{
					transform2.localScale = new Vector3(1f, transform2.localScale.y, transform2.localScale.z);
				}
			}
			else
			{
				transform2.localScale = new Vector3(0f, transform2.localScale.y, transform2.localScale.z);
			}
		}
		for (int k = 1; k < 6; k++)
		{
			Transform transform3 = Subject3Bars[k];
			if (Subject[3] + SubjectTemp[3] > (k - 1) * 20)
			{
				transform3.localScale = new Vector3(0f - (float)((k - 1) * 20 - (Subject[3] + SubjectTemp[3])) / 20f, transform3.localScale.y, transform3.localScale.z);
				if (transform3.localScale.x > 1f)
				{
					transform3.localScale = new Vector3(1f, transform3.localScale.y, transform3.localScale.z);
				}
			}
			else
			{
				transform3.localScale = new Vector3(0f, transform3.localScale.y, transform3.localScale.z);
			}
		}
		for (int l = 1; l < 6; l++)
		{
			Transform transform4 = Subject4Bars[l];
			if (Subject[4] + SubjectTemp[4] > (l - 1) * 20)
			{
				transform4.localScale = new Vector3(0f - (float)((l - 1) * 20 - (Subject[4] + SubjectTemp[4])) / 20f, transform4.localScale.y, transform4.localScale.z);
				if (transform4.localScale.x > 1f)
				{
					transform4.localScale = new Vector3(1f, transform4.localScale.y, transform4.localScale.z);
				}
			}
			else
			{
				transform4.localScale = new Vector3(0f, transform4.localScale.y, transform4.localScale.z);
			}
		}
		for (int m = 1; m < 6; m++)
		{
			Transform transform5 = Subject5Bars[m];
			if (Subject[5] + SubjectTemp[5] > (m - 1) * 20)
			{
				transform5.localScale = new Vector3(0f - (float)((m - 1) * 20 - (Subject[5] + SubjectTemp[5])) / 20f, transform5.localScale.y, transform5.localScale.z);
				if (transform5.localScale.x > 1f)
				{
					transform5.localScale = new Vector3(1f, transform5.localScale.y, transform5.localScale.z);
				}
			}
			else
			{
				transform5.localScale = new Vector3(0f, transform5.localScale.y, transform5.localScale.z);
			}
		}
	}

	private void CheckForGradeUp()
	{
		if (ClassGlobals.Biology >= 20 && ClassGlobals.BiologyGrade < 1)
		{
			ClassGlobals.BiologyGrade = 1;
			GradeUpSubject = 1;
			GradeUp = true;
			Grade = 1;
		}
		else if (ClassGlobals.Chemistry >= 20 && ClassGlobals.ChemistryGrade < 1)
		{
			ClassGlobals.ChemistryGrade = 1;
			GradeUpSubject = 2;
			GradeUp = true;
			Grade = 1;
		}
		else if (ClassGlobals.Language >= 20 && ClassGlobals.LanguageGrade < 1)
		{
			ClassGlobals.LanguageGrade = 1;
			GradeUpSubject = 3;
			GradeUp = true;
			Grade = 1;
		}
		else if (ClassGlobals.Physical >= 20 && ClassGlobals.PhysicalGrade < 1)
		{
			ClassGlobals.PhysicalGrade = 1;
			GradeUpSubject = 4;
			GradeUp = true;
			Grade = 1;
		}
		else if (ClassGlobals.Physical >= 40 && ClassGlobals.PhysicalGrade < 2)
		{
			ClassGlobals.PhysicalGrade = 2;
			GradeUpSubject = 4;
			GradeUp = true;
			Grade = 2;
		}
		else if (ClassGlobals.Physical >= 60 && ClassGlobals.PhysicalGrade < 3)
		{
			ClassGlobals.PhysicalGrade = 3;
			GradeUpSubject = 4;
			GradeUp = true;
			Grade = 3;
		}
		else if (ClassGlobals.Physical >= 80 && ClassGlobals.PhysicalGrade < 4)
		{
			ClassGlobals.PhysicalGrade = 4;
			GradeUpSubject = 4;
			GradeUp = true;
			Grade = 4;
		}
		else if (ClassGlobals.Physical == 100 && ClassGlobals.PhysicalGrade < 5)
		{
			ClassGlobals.PhysicalGrade = 5;
			GradeUpSubject = 4;
			GradeUp = true;
			Grade = 5;
		}
		else if (ClassGlobals.Psychology >= 20 && ClassGlobals.PsychologyGrade < 1)
		{
			ClassGlobals.PsychologyGrade = 1;
			GradeUpSubject = 5;
			GradeUp = true;
			Grade = 1;
		}
	}

	private void GivePoints()
	{
		ClassGlobals.BiologyGrade = 0;
		ClassGlobals.ChemistryGrade = 0;
		ClassGlobals.LanguageGrade = 0;
		ClassGlobals.PhysicalGrade = 0;
		ClassGlobals.PsychologyGrade = 0;
		ClassGlobals.Biology = 19;
		ClassGlobals.Chemistry = 19;
		ClassGlobals.Language = 19;
		ClassGlobals.Physical = 19;
		ClassGlobals.Psychology = 19;
		Subject[1] = ClassGlobals.Biology;
		Subject[2] = ClassGlobals.Chemistry;
		Subject[3] = ClassGlobals.Language;
		Subject[4] = ClassGlobals.Physical;
		Subject[5] = ClassGlobals.Psychology;
		UpdateBars();
	}

	private void MaxPhysical()
	{
		ClassGlobals.PhysicalGrade = 0;
		ClassGlobals.Physical = 99;
		Subject[4] = ClassGlobals.Physical;
		UpdateBars();
	}
}
