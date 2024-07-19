using System;
using UnityEngine;

public class EventManagerScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public NoteLockerScript NoteLocker;

	public UILabel EventSubtitle;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript[] EventStudent;

	public Transform[] EventLocation;

	public AudioClip[] EventClip;

	public string[] EventSpeech;

	public string[] EventAnim;

	public int[] EventSpeaker;

	public GameObject VoiceClip;

	public bool EventCheck;

	public bool EventOn;

	public bool Spoken;

	public int EventPhase;

	public float Timer;

	public float Scale;

	private void Start()
	{
		EventSubtitle.transform.localScale = Vector3.zero;
		if (DateGlobals.Weekday == DayOfWeek.Monday)
		{
			EventCheck = true;
		}
		NoteLocker.Prompt.enabled = true;
		NoteLocker.CanLeaveNote = true;
	}

	private void Update()
	{
		if (!Clock.StopTime && EventCheck && Clock.HourTime > 13.01f)
		{
			if (EventStudent[1] == null)
			{
				EventStudent[1] = StudentManager.Students[25];
			}
			else if (!EventStudent[1].Alive)
			{
				EventCheck = false;
				base.enabled = false;
			}
			if (EventStudent[2] == null)
			{
				EventStudent[2] = StudentManager.Students[30];
			}
			else if (!EventStudent[2].Alive)
			{
				EventCheck = false;
				base.enabled = false;
			}
			if (EventStudent[1] != null && EventStudent[2] != null && !EventStudent[1].Slave && !EventStudent[2].Slave && EventStudent[1].Pathfinding.canMove && EventStudent[2].Pathfinding.canMove)
			{
				EventStudent[1].CurrentDestination = EventLocation[1];
				EventStudent[1].Pathfinding.target = EventLocation[1];
				EventStudent[1].EventManager = this;
				EventStudent[1].InEvent = true;
				EventStudent[2].CurrentDestination = EventLocation[2];
				EventStudent[2].Pathfinding.target = EventLocation[2];
				EventStudent[2].EventManager = this;
				EventStudent[2].InEvent = true;
				EventCheck = false;
				EventOn = true;
			}
		}
		if (!EventOn)
		{
			return;
		}
		float num = Vector3.Distance(Yandere.transform.position, EventStudent[EventSpeaker[EventPhase]].transform.position);
		if (Clock.HourTime > 13.5f || EventStudent[1].WitnessedCorpse || EventStudent[2].WitnessedCorpse || EventStudent[1].Dying || EventStudent[2].Dying || EventStudent[1].Splashed || EventStudent[2].Splashed || EventStudent[1].Alarmed || EventStudent[2].Alarmed)
		{
			EndEvent();
			return;
		}
		if (!EventStudent[1].Pathfinding.canMove && !EventStudent[1].Private)
		{
			EventStudent[1].Character.GetComponent<Animation>().CrossFade(EventStudent[1].IdleAnim);
			EventStudent[1].Private = true;
			StudentManager.UpdateStudents();
		}
		if (!EventStudent[2].Pathfinding.canMove && !EventStudent[2].Private)
		{
			EventStudent[2].Character.GetComponent<Animation>().CrossFade(EventStudent[2].IdleAnim);
			EventStudent[2].Private = true;
			StudentManager.UpdateStudents();
		}
		if (EventStudent[1].Pathfinding.canMove || EventStudent[2].Pathfinding.canMove)
		{
			return;
		}
		if (!Spoken)
		{
			EventStudent[EventSpeaker[EventPhase]].Character.GetComponent<Animation>().CrossFade(EventAnim[EventPhase]);
			if (num < 10f)
			{
				EventSubtitle.text = EventSpeech[EventPhase];
			}
			AudioClipPlayer.Play(EventClip[EventPhase], EventStudent[EventSpeaker[EventPhase]].transform.position + Vector3.up * 1.5f, 5f, 10f, out VoiceClip, Yandere.transform.position.y);
			Spoken = true;
		}
		else
		{
			if (Yandere.transform.position.z > 0f)
			{
				Timer += Time.deltaTime;
				if (Timer > EventClip[EventPhase].length)
				{
					EventSubtitle.text = string.Empty;
				}
				if (Yandere.transform.position.y < EventStudent[1].transform.position.y - 1f)
				{
					EventSubtitle.transform.localScale = Vector3.zero;
				}
				else if (num < 10f)
				{
					Scale = Mathf.Abs((num - 10f) * 0.2f);
					if (Scale < 0f)
					{
						Scale = 0f;
					}
					if (Scale > 1f)
					{
						Scale = 1f;
					}
					EventSubtitle.transform.localScale = new Vector3(Scale, Scale, Scale);
				}
				else
				{
					EventSubtitle.transform.localScale = Vector3.zero;
				}
				Animation component = EventStudent[EventSpeaker[EventPhase]].Character.GetComponent<Animation>();
				if (component[EventAnim[EventPhase]].time >= component[EventAnim[EventPhase]].length)
				{
					component.CrossFade(EventStudent[EventSpeaker[EventPhase]].IdleAnim);
				}
				if (Timer > EventClip[EventPhase].length + 1f)
				{
					Spoken = false;
					EventPhase++;
					Timer = 0f;
					if (EventPhase == EventSpeech.Length)
					{
						EndEvent();
					}
				}
			}
			if (Yandere.transform.position.y > EventStudent[1].transform.position.y - 1f && EventPhase == 7 && num < 5f && !EventGlobals.Event1)
			{
				Yandere.NotificationManager.DisplayNotification(NotificationType.Info);
				EventGlobals.Event1 = true;
			}
		}
		if (base.enabled)
		{
			if (num < 5f)
			{
				Yandere.Eavesdropping = true;
			}
			else
			{
				Yandere.Eavesdropping = false;
			}
		}
	}

	public void EndEvent()
	{
		if (VoiceClip != null)
		{
			UnityEngine.Object.Destroy(VoiceClip);
		}
		EventStudent[1].CurrentDestination = EventStudent[1].Destinations[EventStudent[1].Phase];
		EventStudent[1].Pathfinding.target = EventStudent[1].Destinations[EventStudent[1].Phase];
		EventStudent[1].EventManager = null;
		EventStudent[1].InEvent = false;
		EventStudent[1].Private = false;
		EventStudent[2].CurrentDestination = EventStudent[2].Destinations[EventStudent[2].Phase];
		EventStudent[2].Pathfinding.target = EventStudent[2].Destinations[EventStudent[2].Phase];
		EventStudent[2].EventManager = null;
		EventStudent[2].InEvent = false;
		EventStudent[2].Private = false;
		if (!StudentManager.Stop)
		{
			StudentManager.UpdateStudents();
		}
		Yandere.Eavesdropping = false;
		EventSubtitle.text = string.Empty;
		EventCheck = false;
		EventOn = false;
		base.enabled = false;
	}
}
