using UnityEngine;

public class OfferHelpScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public JukeboxScript Jukebox;

	public StudentScript Student;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public UILabel EventSubtitle;

	public Transform[] Locations;

	public AudioClip[] EventClip;

	public string[] EventSpeech;

	public string[] EventAnim;

	public int[] EventSpeaker;

	public bool Offering;

	public bool Spoken;

	public bool Unable;

	public int EventStudentID;

	public int EventPhase = 1;

	public float Timer;

	private void Start()
	{
		Prompt.enabled = true;
	}

	private void Update()
	{
		if (!Unable)
		{
			if (Prompt.Circle[0].fillAmount == 0f)
			{
				Prompt.Circle[0].fillAmount = 1f;
				if (!Yandere.Chased && Yandere.Chasers == 0)
				{
					Jukebox.Dip = 0.1f;
					Yandere.EmptyHands();
					Yandere.CanMove = false;
					Student = StudentManager.Students[EventStudentID];
					Student.Prompt.Label[0].text = "     Talk";
					Student.Pushable = false;
					Student.Meeting = false;
					Student.Routine = false;
					Student.MeetTimer = 0f;
					Offering = true;
				}
			}
			if (Offering)
			{
				Yandere.transform.rotation = Quaternion.Slerp(Yandere.transform.rotation, base.transform.rotation, Time.deltaTime * 10f);
				Yandere.MoveTowardsTarget(base.transform.position + Vector3.down);
				Quaternion b = Quaternion.LookRotation(Yandere.transform.position - Student.transform.position);
				Student.transform.rotation = Quaternion.Slerp(Student.transform.rotation, b, Time.deltaTime * 10f);
				Animation component = Yandere.Character.GetComponent<Animation>();
				Animation component2 = Student.Character.GetComponent<Animation>();
				if (!Spoken)
				{
					if (EventSpeaker[EventPhase] == 1)
					{
						component.CrossFade(EventAnim[EventPhase]);
						component2.CrossFade(Student.IdleAnim, 1f);
					}
					else
					{
						component2.CrossFade(EventAnim[EventPhase]);
						component.CrossFade(Yandere.IdleAnim, 1f);
					}
					EventSubtitle.transform.localScale = new Vector3(1f, 1f, 1f);
					EventSubtitle.text = EventSpeech[EventPhase];
					AudioSource component3 = GetComponent<AudioSource>();
					component3.clip = EventClip[EventPhase];
					component3.Play();
					Spoken = true;
					return;
				}
				if (!Yandere.PauseScreen.Show && Input.GetButtonDown("A"))
				{
					Timer += EventClip[EventPhase].length + 1f;
				}
				if (EventSpeaker[EventPhase] == 1)
				{
					if (component[EventAnim[EventPhase]].time >= component[EventAnim[EventPhase]].length)
					{
						component.CrossFade(Yandere.IdleAnim);
					}
				}
				else if (component2[EventAnim[EventPhase]].time >= component2[EventAnim[EventPhase]].length)
				{
					component2.CrossFade(Student.IdleAnim);
				}
				Timer += Time.deltaTime;
				if (Timer > EventClip[EventPhase].length)
				{
					Debug.Log("Emptying string.");
					EventSubtitle.text = string.Empty;
				}
				if (Timer > EventClip[EventPhase].length + 1f)
				{
					if (EventStudentID == 5 && EventPhase == 2)
					{
						Yandere.PauseScreen.StudentInfoMenu.Targeting = true;
						StartCoroutine(Yandere.PauseScreen.PhotoGallery.GetPhotos());
						Yandere.PauseScreen.PhotoGallery.gameObject.SetActive(true);
						Yandere.PauseScreen.PhotoGallery.NamingBully = true;
						Yandere.PauseScreen.MainMenu.SetActive(false);
						Yandere.PauseScreen.Panel.enabled = true;
						Yandere.PauseScreen.Sideways = true;
						Yandere.PauseScreen.Show = true;
						Time.timeScale = 0.0001f;
						Yandere.PauseScreen.PhotoGallery.UpdateButtonPrompts();
						Offering = false;
					}
					else
					{
						Continue();
					}
				}
			}
			else if (StudentManager.Students[EventStudentID].Pushed || !StudentManager.Students[EventStudentID].Alive)
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			Prompt.Circle[0].fillAmount = 1f;
		}
	}

	public void UpdateLocation()
	{
		Student = StudentManager.Students[EventStudentID];
		if (Student.CurrentDestination == StudentManager.MeetSpots.List[8])
		{
			base.transform.position = Locations[1].position;
			base.transform.eulerAngles = Locations[1].eulerAngles;
		}
		else if (Student.CurrentDestination == StudentManager.MeetSpots.List[9])
		{
			base.transform.position = Locations[2].position;
			base.transform.eulerAngles = Locations[2].eulerAngles;
		}
		else if (Student.CurrentDestination == StudentManager.MeetSpots.List[10])
		{
			base.transform.position = Locations[3].position;
			base.transform.eulerAngles = Locations[3].eulerAngles;
		}
		if (EventStudentID == 30 && !PlayerGlobals.GetStudentFriend(30))
		{
			Prompt.Label[0].text = "     Must Befriend Student First";
			Unable = true;
		}
	}

	public void Continue()
	{
		Debug.Log("Proceeding to next line.");
		Offering = true;
		Spoken = false;
		EventPhase++;
		Timer = 0f;
		if (EventStudentID == 30 && EventPhase == 14)
		{
			if (!ConversationGlobals.GetTopicDiscovered(23))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				ConversationGlobals.SetTopicDiscovered(23, true);
			}
			if (!ConversationGlobals.GetTopicLearnedByStudent(23, EventStudentID))
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
				ConversationGlobals.SetTopicLearnedByStudent(23, EventStudentID, true);
			}
		}
		if (EventPhase == EventSpeech.Length)
		{
			if (EventStudentID == 30)
			{
				SchemeGlobals.SetSchemeStage(6, 5);
			}
			Student.CurrentDestination = Student.Destinations[Student.Phase];
			Student.Pathfinding.target = Student.Destinations[Student.Phase];
			Student.Pathfinding.canSearch = true;
			Student.Pathfinding.canMove = true;
			Student.Routine = true;
			EventSubtitle.transform.localScale = Vector3.zero;
			Yandere.CanMove = true;
			Jukebox.Dip = 1f;
			Object.Destroy(base.gameObject);
		}
	}
}
