using UnityEngine;

public class NoteLockerScript : MonoBehaviour
{
	public FindStudentLockerScript FindStudentLocker;

	public StudentManagerScript StudentManager;

	public NoteWindowScript NoteWindow;

	public PromptBarScript PromptBar;

	public StudentScript Student;

	public YandereScript Yandere;

	public ListScript MeetSpots;

	public PromptScript Prompt;

	public GameObject NewBall;

	public GameObject NewNote;

	public GameObject Locker;

	public GameObject Ball;

	public GameObject Note;

	public AudioClip NoteSuccess;

	public AudioClip NoteFail;

	public AudioClip NoteFind;

	public bool CheckingNote;

	public bool CanLeaveNote = true;

	public bool SpawnedNote;

	public bool NoteLeft;

	public bool Success;

	public float MeetTime;

	public float Timer;

	public int LockerOwner;

	public int MeetID;

	public int Phase = 1;

	private void Start()
	{
		if (StudentGlobals.GetStudentDead(LockerOwner))
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (Student != null)
		{
			Vector3 b = new Vector3(base.transform.position.x, Student.transform.position.y, base.transform.position.z);
			if (Prompt.enabled)
			{
				if (Vector3.Distance(Student.transform.position, b) < 1f || Yandere.Armed)
				{
					Prompt.Hide();
					Prompt.enabled = false;
				}
			}
			else if (CanLeaveNote && Vector3.Distance(Student.transform.position, b) > 1f && !Yandere.Armed)
			{
				Prompt.enabled = true;
			}
		}
		else
		{
			Student = StudentManager.Students[LockerOwner];
		}
		if (Prompt != null && Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			NoteWindow.NoteLocker = this;
			Yandere.Blur.enabled = true;
			NoteWindow.gameObject.SetActive(true);
			Yandere.CanMove = false;
			NoteWindow.Show = true;
			Yandere.HUD.alpha = 0f;
			PromptBar.Show = true;
			Time.timeScale = 0.0001f;
			PromptBar.Label[0].text = "Confirm";
			PromptBar.Label[1].text = "Cancel";
			PromptBar.Label[4].text = "Select";
			PromptBar.UpdateButtons();
		}
		if (!NoteLeft)
		{
			return;
		}
		if (Student != null && ((Student.Routine && Student.Phase == 2) || (Student.Routine && Student.Phase == 8) || Student.SentToLocker) && Vector3.Distance(base.transform.position, Student.transform.position) < 2f && !Student.InEvent)
		{
			Student.CharacterAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
			if (!Success)
			{
				Student.CharacterAnimation.CrossFade(Student.TossNoteAnim);
			}
			else
			{
				Student.CharacterAnimation.CrossFade(Student.KeepNoteAnim);
			}
			Student.Pathfinding.canSearch = false;
			Student.Pathfinding.canMove = false;
			Student.CheckingNote = true;
			Student.Routine = false;
			Student.InEvent = true;
			CheckingNote = true;
		}
		if (!CheckingNote)
		{
			return;
		}
		Timer += Time.deltaTime;
		Student.MoveTowardsTarget(Student.MyLocker.position);
		Student.transform.rotation = Quaternion.Slerp(Student.transform.rotation, Student.MyLocker.rotation, 10f * Time.deltaTime);
		if (Student != null)
		{
			if (Student.CharacterAnimation[Student.TossNoteAnim].time >= Student.CharacterAnimation[Student.TossNoteAnim].length)
			{
				Finish();
			}
			if (Student.CharacterAnimation[Student.KeepNoteAnim].time >= Student.CharacterAnimation[Student.KeepNoteAnim].length)
			{
				DetermineSchedule();
				Finish();
			}
		}
		if (Timer > 3.5f && !SpawnedNote)
		{
			NewNote = Object.Instantiate(Note, base.transform.position, Quaternion.identity);
			NewNote.transform.parent = Student.LeftHand;
			NewNote.transform.localPosition = new Vector3(-0.06f, -0.01f, 0f);
			NewNote.transform.localEulerAngles = new Vector3(-75f, -90f, 180f);
			NewNote.transform.localScale = new Vector3(0.1f, 0.2f, 1f);
			SpawnedNote = true;
		}
		if (!Success)
		{
			if (Timer > 10f && NewNote != null)
			{
				if (NewNote.transform.localScale.z > 0.1f)
				{
					NewNote.transform.localScale = Vector3.MoveTowards(NewNote.transform.localScale, Vector3.zero, Time.deltaTime * 2f);
				}
				else
				{
					Object.Destroy(NewNote);
				}
			}
			if (Timer > 12.25f && NewBall == null)
			{
				NewBall = Object.Instantiate(Ball, Student.LeftHand.position, Quaternion.identity);
				Rigidbody component = NewBall.GetComponent<Rigidbody>();
				component.AddRelativeForce(Student.transform.forward * -100f);
				component.AddRelativeForce(Vector3.up * 100f);
				Phase++;
			}
		}
		else if (Timer > 12.5f && NewNote != null)
		{
			if (NewNote.transform.localScale.z > 0.1f)
			{
				NewNote.transform.localScale = Vector3.MoveTowards(NewNote.transform.localScale, Vector3.zero, Time.deltaTime * 2f);
			}
			else
			{
				Object.Destroy(NewNote);
			}
		}
		if (Phase == 1)
		{
			if (Timer > 2.3333333f)
			{
				if (!Student.Male)
				{
					Yandere.Subtitle.Speaker = Student;
					Yandere.Subtitle.UpdateLabel(SubtitleType.NoteReaction, 1, 3f);
				}
				else
				{
					Yandere.Subtitle.Speaker = Student;
					Yandere.Subtitle.UpdateLabel(SubtitleType.NoteReactionMale, 1, 3f);
				}
				Phase++;
			}
		}
		else
		{
			if (Phase != 2)
			{
				return;
			}
			if (!Success)
			{
				if (Timer > 9.666667f)
				{
					if (!Student.Male)
					{
						Yandere.Subtitle.Speaker = Student;
						Yandere.Subtitle.UpdateLabel(SubtitleType.NoteReaction, 2, 3f);
					}
					else
					{
						Yandere.Subtitle.Speaker = Student;
						Yandere.Subtitle.UpdateLabel(SubtitleType.NoteReactionMale, 2, 3f);
					}
					Phase++;
				}
			}
			else if (Timer > 10.166667f)
			{
				if (!Student.Male)
				{
					Yandere.Subtitle.Speaker = Student;
					Yandere.Subtitle.UpdateLabel(SubtitleType.NoteReaction, 3, 3f);
				}
				else
				{
					Yandere.Subtitle.Speaker = Student;
					Yandere.Subtitle.UpdateLabel(SubtitleType.NoteReactionMale, 3, 3f);
				}
				Phase++;
			}
		}
	}

	private void Finish()
	{
		if (Success)
		{
			if (Student.Clock.HourTime > Student.MeetTime)
			{
				Student.CurrentDestination = Student.MeetSpot;
				Student.Pathfinding.target = Student.MeetSpot;
				Student.Meeting = true;
				Student.MeetTime = 0f;
			}
			else
			{
				Student.CurrentDestination = Student.Destinations[Student.Phase];
				Student.Pathfinding.target = Student.Destinations[Student.Phase];
			}
			Student.Pathfinding.canSearch = true;
			Student.Pathfinding.canMove = true;
			Student.Pathfinding.speed = 1f;
		}
		Animation component = Student.Character.GetComponent<Animation>();
		component.cullingType = AnimationCullingType.BasedOnRenderers;
		component.CrossFade(Student.IdleAnim);
		Student.DistanceToDestination = 100f;
		Student.CheckingNote = false;
		Student.InEvent = false;
		Student.Routine = true;
		CheckingNote = false;
		NoteLeft = false;
		Phase++;
	}

	private void DetermineSchedule()
	{
		Student.MeetSpot = MeetSpots.List[MeetID];
		Student.MeetTime = MeetTime;
	}
}
