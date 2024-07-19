using UnityEngine;

public class PickpocketScript : MonoBehaviour
{
	public PickpocketMinigameScript PickpocketMinigame;

	public StudentScript Student;

	public PromptScript Prompt;

	public UIPanel PickpocketPanel;

	public UISprite TimeBar;

	public Transform PickpocketSpot;

	public GameObject AlarmDisc;

	public GameObject Key;

	public float Timer;

	public int ID = 1;

	public bool NotNurse;

	public bool Test;

	private void Start()
	{
		if (Student.StudentID != 71)
		{
			Prompt.transform.parent.gameObject.SetActive(false);
			base.enabled = false;
			return;
		}
		PickpocketMinigame = Student.StudentManager.PickpocketMinigame;
		if (Student.StudentID == Student.StudentManager.NurseID)
		{
			ID = 2;
		}
		else if (ClubGlobals.GetClubClosed(Student.OriginalClub))
		{
			Prompt.transform.parent.gameObject.SetActive(false);
			base.enabled = false;
		}
		else
		{
			Prompt.Label[3].text = "     Steal Shed Key";
			NotNurse = true;
		}
	}

	private void Update()
	{
		if (Prompt.transform.parent != null)
		{
			if (Student.Routine)
			{
				if (Student.DistanceToDestination > 0.5f)
				{
					if (Prompt.enabled)
					{
						Prompt.Hide();
						Prompt.enabled = false;
						PickpocketPanel.enabled = false;
					}
					if (Student.Yandere.Pickpocketing && PickpocketMinigame.ID == ID)
					{
						Prompt.Yandere.Caught = true;
						PickpocketMinigame.End();
						Punish();
					}
				}
				else
				{
					PickpocketPanel.enabled = true;
					if (Student.Yandere.PickUp == null && Student.Yandere.Pursuer == null)
					{
						Prompt.enabled = true;
					}
					else
					{
						Prompt.enabled = false;
						Prompt.Hide();
					}
					Timer += Time.deltaTime * Student.CharacterAnimation[Student.PatrolAnim].speed;
					TimeBar.fillAmount = 1f - Timer / Student.CharacterAnimation[Student.PatrolAnim].length;
					if (Timer > Student.CharacterAnimation[Student.PatrolAnim].length)
					{
						if (Student.Yandere.Pickpocketing && PickpocketMinigame.ID == ID)
						{
							Prompt.Yandere.Caught = true;
							PickpocketMinigame.End();
							Punish();
						}
						Timer = 0f;
					}
				}
			}
			else if (Prompt.enabled)
			{
				Prompt.Hide();
				Prompt.enabled = false;
				PickpocketPanel.enabled = false;
				if (Student.Yandere.Pickpocketing && PickpocketMinigame.ID == ID)
				{
					Prompt.Yandere.Caught = true;
					PickpocketMinigame.End();
					Punish();
				}
			}
			if (Prompt.Circle[3].fillAmount == 0f)
			{
				Prompt.Circle[3].fillAmount = 1f;
				if (!Prompt.Yandere.Chased && Prompt.Yandere.Chasers == 0)
				{
					PickpocketMinigame.PickpocketSpot = PickpocketSpot;
					PickpocketMinigame.NotNurse = NotNurse;
					PickpocketMinigame.Show = true;
					PickpocketMinigame.ID = ID;
					Student.Yandere.CharacterAnimation.CrossFade("f02_pickpocketing_00");
					Student.Yandere.Pickpocketing = true;
					Student.Yandere.EmptyHands();
					Student.Yandere.CanMove = false;
				}
			}
			if (PickpocketMinigame != null && PickpocketMinigame.ID == ID)
			{
				if (PickpocketMinigame.Success)
				{
					PickpocketMinigame.Success = false;
					PickpocketMinigame.ID = 0;
					Succeed();
					PickpocketPanel.enabled = false;
					Prompt.enabled = false;
					Prompt.Hide();
					Key.SetActive(false);
					base.enabled = false;
				}
				if (PickpocketMinigame.Failure)
				{
					PickpocketMinigame.Failure = false;
					PickpocketMinigame.ID = 0;
					Punish();
				}
			}
			if (!Student.Alive)
			{
				base.transform.position = new Vector3(Student.transform.position.x, Student.transform.position.y + 1f, Student.transform.position.z);
				Prompt.gameObject.GetComponent<BoxCollider>().isTrigger = false;
				Prompt.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				Prompt.gameObject.GetComponent<Rigidbody>().useGravity = true;
				Prompt.enabled = true;
				base.transform.parent = null;
			}
		}
		else if (Prompt.Circle[3].fillAmount == 0f)
		{
			Succeed();
			Prompt.Hide();
			PickpocketPanel.enabled = false;
			Prompt.enabled = false;
			Prompt.Hide();
			Key.SetActive(false);
			base.enabled = false;
		}
	}

	private void Punish()
	{
		Debug.Log("Punishing Yandere-chan for pickpocketing.");
		GameObject gameObject = Object.Instantiate(AlarmDisc, Student.Yandere.transform.position + Vector3.up, Quaternion.identity);
		gameObject.GetComponent<AlarmDiscScript>().NoScream = true;
		if (!NotNurse && !Prompt.Yandere.Egg)
		{
			Debug.Log("A faculty member saw pickpocketing.");
			Student.Witnessed = StudentWitnessType.Theft;
			Student.SenpaiNoticed();
			Student.CameraEffects.MurderWitnessed();
			Student.Concern = 5;
		}
		else
		{
			Student.Witnessed = StudentWitnessType.Pickpocketing;
			Student.CameraEffects.Alarm();
			Student.Alarm += 200f;
		}
		Timer = 0f;
		Prompt.Hide();
		Prompt.enabled = false;
		PickpocketPanel.enabled = false;
		Student.CharacterAnimation[Student.PatrolAnim].time = 0f;
		Student.PatrolTimer = 0f;
	}

	private void Succeed()
	{
		if (ID == 1)
		{
			Student.StudentManager.ShedDoor.Prompt.Label[0].text = "     Open";
			Student.StudentManager.ShedDoor.Locked = false;
			Student.ClubManager.Padlock.SetActive(false);
			Student.Yandere.Inventory.ShedKey = true;
		}
		else
		{
			Student.StudentManager.CabinetDoor.Prompt.Label[0].text = "     Open";
			Student.StudentManager.CabinetDoor.Locked = false;
			Student.Yandere.Inventory.CabinetKey = true;
		}
	}
}
