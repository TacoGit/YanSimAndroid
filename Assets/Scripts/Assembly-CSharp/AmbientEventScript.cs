using System;
using UnityEngine;

public class AmbientEventScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

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

	public bool EventOn;

	public bool Spoken;

	public int EventPhase;

	public float Timer;

	public float Scale;

	public int[] StudentID;

	public DayOfWeek EventDay;

	private void Start()
	{
		if (DateGlobals.Weekday != EventDay)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (!EventOn)
		{
			for (int i = 1; i < 3; i++)
			{
				if (EventStudent[i] == null)
				{
					EventStudent[i] = StudentManager.Students[StudentID[i]];
				}
				else if (!EventStudent[i].Alive || EventStudent[i].Slave)
				{
					base.enabled = false;
				}
			}
			if (Clock.HourTime > 13.001f && EventStudent[1] != null && EventStudent[2] != null && EventStudent[1].Pathfinding.canMove && EventStudent[2].Pathfinding.canMove)
			{
				EventStudent[1].CharacterAnimation.CrossFade(EventStudent[1].WalkAnim);
				EventStudent[1].CurrentDestination = EventLocation[1];
				EventStudent[1].Pathfinding.target = EventLocation[1];
				EventStudent[1].InEvent = true;
				EventStudent[2].CharacterAnimation.CrossFade(EventStudent[2].WalkAnim);
				EventStudent[2].CurrentDestination = EventLocation[2];
				EventStudent[2].Pathfinding.target = EventLocation[2];
				EventStudent[2].InEvent = true;
				EventOn = true;
			}
			return;
		}
		float num = Vector3.Distance(Yandere.transform.position, EventLocation[1].parent.position);
		if (Clock.HourTime > 13.5f || EventStudent[1].WitnessedCorpse || EventStudent[2].WitnessedCorpse || EventStudent[1].Alarmed || EventStudent[2].Alarmed || EventStudent[1].Dying || EventStudent[2].Dying)
		{
			EndEvent();
			return;
		}
		for (int j = 1; j < 3; j++)
		{
			if (!EventStudent[j].Pathfinding.canMove && !EventStudent[j].Private)
			{
				EventStudent[j].Character.GetComponent<Animation>().CrossFade(EventStudent[j].IdleAnim);
				EventStudent[j].Private = true;
				StudentManager.UpdateStudents();
			}
		}
		if (EventStudent[1].Pathfinding.canMove || EventStudent[2].Pathfinding.canMove)
		{
			return;
		}
		if (!Spoken)
		{
			EventStudent[EventSpeaker[1]].CharacterAnimation.CrossFade(EventStudent[1].IdleAnim);
			EventStudent[EventSpeaker[2]].CharacterAnimation.CrossFade(EventStudent[2].IdleAnim);
			EventStudent[EventSpeaker[EventPhase]].PickRandomAnim();
			EventStudent[EventSpeaker[EventPhase]].CharacterAnimation.CrossFade(EventStudent[EventSpeaker[EventPhase]].RandomAnim);
			if (DateGlobals.Weekday == DayOfWeek.Monday && EventPhase == 13)
			{
				EventStudent[EventSpeaker[EventPhase]].CharacterAnimation.CrossFade("jojoPose_00");
			}
			AudioClipPlayer.Play(EventClip[EventPhase], EventStudent[EventSpeaker[EventPhase]].transform.position + Vector3.up * 1.5f, 5f, 10f, out VoiceClip, Yandere.transform.position.y);
			Spoken = true;
			return;
		}
		int num2 = EventSpeaker[EventPhase];
		if (EventStudent[num2].CharacterAnimation[EventStudent[num2].RandomAnim].time >= EventStudent[num2].CharacterAnimation[EventStudent[num2].RandomAnim].length)
		{
			EventStudent[num2].PickRandomAnim();
			EventStudent[num2].CharacterAnimation.CrossFade(EventStudent[num2].RandomAnim);
		}
		Timer += Time.deltaTime;
		if (Yandere.transform.position.y > EventLocation[1].parent.position.y - 1f && Yandere.transform.position.y < EventLocation[1].parent.position.y + 1f)
		{
			if (VoiceClip != null)
			{
				VoiceClip.GetComponent<AudioSource>().volume = 1f;
			}
			if (num < 10f)
			{
				if (Timer > EventClip[EventPhase].length)
				{
					EventSubtitle.text = string.Empty;
				}
				else
				{
					EventSubtitle.text = EventSpeech[EventPhase];
				}
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
				EventSubtitle.text = string.Empty;
			}
		}
		else if (VoiceClip != null)
		{
			VoiceClip.GetComponent<AudioSource>().volume = 0f;
		}
		if (Timer > EventClip[EventPhase].length + 0.5f)
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

	public void EndEvent()
	{
		if (VoiceClip != null)
		{
			UnityEngine.Object.Destroy(VoiceClip);
		}
		for (int i = 1; i < 3; i++)
		{
			EventStudent[i].CurrentDestination = EventStudent[i].Destinations[EventStudent[i].Phase];
			EventStudent[i].Pathfinding.target = EventStudent[i].Destinations[EventStudent[i].Phase];
			EventStudent[i].InEvent = false;
			EventStudent[i].Private = false;
		}
		if (!StudentManager.Stop)
		{
			StudentManager.UpdateStudents();
		}
		EventSubtitle.text = string.Empty;
		base.enabled = false;
	}
}
