using UnityEngine;

public class GiggleScript : MonoBehaviour
{
	public GameObject EmptyGameObject;

	public GameObject Giggle;

	public StudentScript Student;

	public bool StudentIsBusy;

	public bool Distracted;

	public int Frame;

	private void Start()
	{
		float num = 500f * (2f - SchoolGlobals.SchoolAtmosphere);
		base.transform.localScale = new Vector3(num, base.transform.localScale.y, num);
	}

	private void Update()
	{
		if (Frame > 0)
		{
			Object.Destroy(base.gameObject);
		}
		Frame++;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 9 || Distracted)
		{
			return;
		}
		Student = other.gameObject.GetComponent<StudentScript>();
		if (!(Student != null) || !(Student.Giggle == null))
		{
			return;
		}
		if (Student.Clock.Period == 3 && Student.BusyAtLunch)
		{
			StudentIsBusy = true;
		}
		if ((Student.StudentID == 47 || Student.StudentID == 49) && Student.StudentManager.ConvoManager.Confirmed)
		{
			StudentIsBusy = true;
		}
		if (Student.YandereVisible || Student.Alarmed || Student.Distracted || Student.Wet || Student.Slave || Student.WitnessedMurder || Student.WitnessedCorpse || Student.Investigating || Student.InEvent || Student.Following || Student.Confessing || Student.Meeting || Student.TurnOffRadio || Student.Fleeing || Student.Distracting || Student.GoAway || Student.FocusOnYandere || StudentIsBusy || Student.Actions[Student.Phase] == StudentActionType.Teaching || Student.Actions[Student.Phase] == StudentActionType.SitAndTakeNotes || Student.Actions[Student.Phase] == StudentActionType.Graffiti || Student.Actions[Student.Phase] == StudentActionType.Bully || !Student.Routine || Student.Persona == PersonaType.Protective || Student.MyBento.Tampered)
		{
			return;
		}
		Student.Character.GetComponent<Animation>().CrossFade(Student.IdleAnim);
		Giggle = Object.Instantiate(EmptyGameObject, new Vector3(base.transform.position.x, Student.transform.position.y, base.transform.position.z), Quaternion.identity);
		Student.Giggle = Giggle;
		if (Student.Pathfinding != null && !Student.Nemesis)
		{
			if (Student.Drownable)
			{
				Student.Drownable = false;
				Student.StudentManager.UpdateMe(Student.StudentID);
			}
			if (Student.ChalkDust.isPlaying)
			{
				Student.ChalkDust.Stop();
				Student.Pushable = false;
				Student.StudentManager.UpdateMe(Student.StudentID);
			}
			Student.Pathfinding.canSearch = false;
			Student.Pathfinding.canMove = false;
			Student.InvestigationPhase = 0;
			Student.InvestigationTimer = 0f;
			Student.Investigating = true;
			Student.EatingSnack = false;
			Student.SpeechLines.Stop();
			Student.ChalkDust.Stop();
			Student.DiscCheck = true;
			Student.Routine = false;
			Student.CleanTimer = 0f;
			Student.ReadPhase = 0;
			Student.StopPairing();
			if (Student.SunbathePhase > 2)
			{
				Student.SunbathePhase = 2;
			}
			if (Student.Persona != PersonaType.PhoneAddict && !Student.Sleuthing)
			{
				Student.SmartPhone.SetActive(false);
			}
			else
			{
				Student.SmartPhone.SetActive(true);
			}
			Student.OccultBook.SetActive(false);
			Student.Pen.SetActive(false);
			if (!Student.Male)
			{
				Student.Cigarette.SetActive(false);
				Student.Lighter.SetActive(false);
			}
			bool flag = false;
			if (Student.Bento.activeInHierarchy && Student.StudentID > 1)
			{
				flag = true;
			}
			Student.EmptyHands();
			if (flag)
			{
				GenericBentoScript component = Student.Bento.GetComponent<GenericBentoScript>();
				component.enabled = true;
				component.Prompt.enabled = true;
				Student.Bento.SetActive(true);
				Student.Bento.transform.parent = Student.transform;
				if (Student.Male)
				{
					Student.Bento.transform.localPosition = new Vector3(0f, 0.4266666f, -0.075f);
				}
				else
				{
					Student.Bento.transform.localPosition = new Vector3(0f, 0.461f, -0.075f);
				}
				Student.Bento.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				Student.Bento.transform.parent = null;
			}
		}
		Distracted = true;
	}
}
