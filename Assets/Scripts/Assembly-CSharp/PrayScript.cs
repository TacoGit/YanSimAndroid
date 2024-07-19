using UnityEngine;

public class PrayScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public WeaponManagerScript WeaponManager;

	public InputManagerScript InputManager;

	public PromptBarScript PromptBar;

	public StudentScript Student;

	public YandereScript Yandere;

	public PoliceScript Police;

	public UILabel SanityLabel;

	public UILabel VictimLabel;

	public PromptScript GenderPrompt;

	public PromptScript Prompt;

	public Transform PrayWindow;

	public Transform SummonSpot;

	public Transform Highlight;

	public Transform[] WeaponSpot;

	public GameObject[] Weapon;

	public GameObject FemaleTurtle;

	public int StudentNumber;

	public int StudentID;

	public int Selected;

	public int Victims;

	public int Uses;

	public bool FemaleVictimChecked;

	public bool MaleVictimChecked;

	public bool JustSummoned;

	public bool SpawnMale;

	public bool Show;

	private void Start()
	{
		if (StudentGlobals.GetStudentDead(39))
		{
			VictimLabel.color = new Color(VictimLabel.color.r, VictimLabel.color.g, VictimLabel.color.b, 0.5f);
		}
		PrayWindow.localScale = Vector3.zero;
		if (MissionModeGlobals.MissionMode)
		{
			Disable();
		}
		if (GameGlobals.LoveSick)
		{
			Disable();
		}
	}

	private void Disable()
	{
		GenderPrompt.gameObject.SetActive(false);
		base.enabled = false;
		Prompt.enabled = false;
		Prompt.Hide();
	}

	private void Update()
	{
		if (!FemaleVictimChecked)
		{
			if (StudentManager.Students[39] != null && !StudentManager.Students[39].Alive)
			{
				FemaleVictimChecked = true;
				Victims++;
			}
		}
		else if (StudentManager.Students[39] == null)
		{
			FemaleVictimChecked = false;
			Victims--;
		}
		if (!MaleVictimChecked)
		{
			if (StudentManager.Students[37] != null && !StudentManager.Students[37].Alive)
			{
				MaleVictimChecked = true;
				Victims++;
			}
		}
		else if (StudentManager.Students[37] == null)
		{
			MaleVictimChecked = false;
			Victims--;
		}
		if (JustSummoned)
		{
			StudentManager.UpdateMe(StudentID);
			JustSummoned = false;
		}
		if (GenderPrompt.Circle[0].fillAmount == 0f)
		{
			GenderPrompt.Circle[0].fillAmount = 1f;
			if (!SpawnMale)
			{
				VictimLabel.color = new Color(VictimLabel.color.r, VictimLabel.color.g, VictimLabel.color.b, (!StudentGlobals.GetStudentDead(37)) ? 1f : 0.5f);
				GenderPrompt.Label[0].text = "     Male Victim";
				SpawnMale = true;
			}
			else
			{
				VictimLabel.color = new Color(VictimLabel.color.r, VictimLabel.color.g, VictimLabel.color.b, (!StudentGlobals.GetStudentDead(39)) ? 1f : 0.5f);
				GenderPrompt.Label[0].text = "     Female Victim";
				SpawnMale = false;
			}
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (!Yandere.Chased && Yandere.Chasers == 0)
			{
				Yandere.TargetStudent = Student;
				StudentManager.DisablePrompts();
				PrayWindow.gameObject.SetActive(true);
				Show = true;
				Yandere.ShoulderCamera.OverShoulder = true;
				Yandere.WeaponMenu.KeyboardShow = false;
				Yandere.Obscurance.enabled = false;
				Yandere.WeaponMenu.Show = false;
				Yandere.YandereVision = false;
				Yandere.CanMove = false;
				Yandere.Talking = true;
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Accept";
				PromptBar.Label[4].text = "Choose";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
				StudentNumber = ((!SpawnMale) ? 39 : 37);
				if (StudentManager.Students[StudentNumber] != null)
				{
					if (!StudentManager.Students[StudentNumber].gameObject.activeInHierarchy)
					{
						VictimLabel.color = new Color(VictimLabel.color.r, VictimLabel.color.g, VictimLabel.color.b, 0.5f);
					}
					else
					{
						VictimLabel.color = new Color(VictimLabel.color.r, VictimLabel.color.g, VictimLabel.color.b, 1f);
					}
				}
			}
		}
		if (!Show)
		{
			if (PrayWindow.gameObject.activeInHierarchy)
			{
				if (PrayWindow.localScale.x > 0.1f)
				{
					PrayWindow.localScale = Vector3.Lerp(PrayWindow.localScale, Vector3.zero, Time.deltaTime * 10f);
					return;
				}
				PrayWindow.localScale = Vector3.zero;
				PrayWindow.gameObject.SetActive(false);
			}
			return;
		}
		PrayWindow.localScale = Vector3.Lerp(PrayWindow.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		if (InputManager.TappedUp)
		{
			Selected--;
			if (Selected == 7)
			{
				Selected = 6;
			}
			UpdateHighlight();
		}
		if (InputManager.TappedDown)
		{
			Selected++;
			if (Selected == 7)
			{
				Selected = 8;
			}
			UpdateHighlight();
		}
		if (!Input.GetButtonDown("A"))
		{
			return;
		}
		if (Selected == 1)
		{
			if (!Yandere.SanityBased)
			{
				SanityLabel.text = "Disable Sanity Anims";
				Yandere.SanityBased = true;
			}
			else
			{
				SanityLabel.text = "Enable Sanity Anims";
				Yandere.SanityBased = false;
			}
			Exit();
		}
		else if (Selected == 2)
		{
			Yandere.Sanity -= 50f;
			Exit();
		}
		else if (Selected == 3)
		{
			if (VictimLabel.color.a == 1f && StudentManager.NPCsSpawned >= StudentManager.NPCsTotal)
			{
				if (SpawnMale)
				{
					MaleVictimChecked = false;
					StudentID = 37;
				}
				else
				{
					FemaleVictimChecked = false;
					StudentID = 39;
				}
				if (StudentManager.Students[StudentID] != null)
				{
					Object.Destroy(StudentManager.Students[StudentID].gameObject);
				}
				StudentManager.Students[StudentID] = null;
				StudentManager.ForceSpawn = true;
				StudentManager.SpawnPositions[StudentID] = SummonSpot;
				StudentManager.SpawnID = StudentID;
				StudentManager.SpawnStudent(StudentManager.SpawnID);
				StudentManager.SpawnID = 0;
				Police.Corpses -= Victims;
				Victims = 0;
				JustSummoned = true;
				Exit();
			}
		}
		else if (Selected == 4)
		{
			SpawnWeapons();
			Exit();
		}
		else if (Selected == 5)
		{
			if (Yandere.Gloved)
			{
				Yandere.Gloves.Blood.enabled = false;
			}
			Police.BloodyClothing = 0;
			Yandere.Bloodiness = 0f;
			Yandere.Sanity = 100f;
			Exit();
		}
		else if (Selected == 6)
		{
			WeaponManager.CleanWeapons();
			Exit();
		}
		else if (Selected == 8)
		{
			Exit();
		}
	}

	private void UpdateHighlight()
	{
		if (Selected < 1)
		{
			Selected = 8;
		}
		else if (Selected > 8)
		{
			Selected = 1;
		}
		Highlight.transform.localPosition = new Vector3(Highlight.transform.localPosition.x, 225f - 50f * (float)Selected, Highlight.transform.localPosition.z);
	}

	private void Exit()
	{
		Selected = 1;
		UpdateHighlight();
		Yandere.ShoulderCamera.OverShoulder = false;
		StudentManager.EnablePrompts();
		Yandere.TargetStudent = null;
		PromptBar.ClearButtons();
		PromptBar.Show = false;
		Show = false;
		Uses++;
		if (Uses > 9)
		{
			FemaleTurtle.SetActive(true);
		}
	}

	public void SpawnWeapons()
	{
		for (int i = 1; i < 6; i++)
		{
			if (Weapon[i] != null)
			{
				Weapon[i].transform.position = WeaponSpot[i].position;
			}
		}
	}
}
