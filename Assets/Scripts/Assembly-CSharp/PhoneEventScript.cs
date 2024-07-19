using System;
using UnityEngine;

public class PhoneEventScript : MonoBehaviour
{
	public OsanaClubEventScript OsanaClubEvent;

	public StudentManagerScript StudentManager;

	public BucketPourScript DumpPoint;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript EventStudent;

	public StudentScript EventFriend;

	public UILabel EventSubtitle;

	public Transform EventLocation;

	public Transform SpyLocation;

	public AudioClip[] EventClip;

	public string[] EventSpeech;

	public float[] SpeechTimes;

	public string[] EventAnim;

	public GameObject VoiceClip;

	public bool EventActive;

	public bool EventCheck;

	public bool EventOver;

	public int EventStudentID = 7;

	public int EventFriendID = 34;

	public float EventTime = 7.5f;

	public int EventPhase = 1;

	public DayOfWeek EventDay = DayOfWeek.Monday;

	public float CurrentClipLength;

	public float FailSafe;

	public float Timer;

	private void Start()
	{
		EventSubtitle.transform.localScale = Vector3.zero;
		if (DateGlobals.Weekday == EventDay)
		{
			EventCheck = true;
		}
		if (HomeGlobals.LateForSchool || StudentManager.YandereLate)
		{
			base.enabled = false;
		}
		if (EventStudentID == 11)
		{
			base.enabled = false;
		}
	}

	private void OnAwake()
	{
		if (EventStudentID == 11)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (!Clock.StopTime && EventCheck)
		{
			if (Clock.HourTime > EventTime + 0.5f)
			{
				base.enabled = false;
			}
			else if (Clock.HourTime > EventTime)
			{
				EventStudent = StudentManager.Students[EventStudentID];
				if (EventStudent != null)
				{
					if (EventStudentID == 11 && EventFriend != null)
					{
						EventFriend = StudentManager.Students[EventFriendID];
						EventFriend.Pathfinding.canSearch = false;
						EventFriend.Pathfinding.canMove = false;
						EventFriend.TargetDistance = 0.5f;
						EventFriend.SpeechLines.Stop();
						EventFriend.PhoneEvent = this;
						EventFriend.CanTalk = false;
						EventFriend.Routine = false;
						EventFriend.InEvent = true;
						EventFriend.Private = true;
						EventFriend.Prompt.Hide();
					}
					if (EventStudent.Routine && !EventStudent.Distracted && !EventStudent.Talking && !EventStudent.Meeting && EventStudent.Indoors)
					{
						if (!EventStudent.WitnessedMurder)
						{
							EventStudent.CurrentDestination = EventStudent.Destinations[EventStudent.Phase];
							EventStudent.Pathfinding.target = EventStudent.Destinations[EventStudent.Phase];
							EventStudent.Obstacle.checkTime = 99f;
							EventStudent.SpeechLines.Stop();
							EventStudent.PhoneEvent = this;
							EventStudent.CanTalk = false;
							EventStudent.InEvent = true;
							EventStudent.Private = true;
							EventStudent.Prompt.Hide();
							EventCheck = false;
							EventActive = true;
							if (EventStudent.Following)
							{
								EventStudent.Pathfinding.canMove = true;
								EventStudent.Pathfinding.speed = 1f;
								EventStudent.Following = false;
								EventStudent.Routine = true;
								Yandere.Followers--;
								EventStudent.Subtitle.UpdateLabel(SubtitleType.StopFollowApology, 0, 3f);
								EventStudent.Prompt.Label[0].text = "     Talk";
							}
						}
						else
						{
							base.enabled = false;
						}
					}
				}
			}
		}
		if (!EventActive)
		{
			return;
		}
		if (EventStudent.DistanceToDestination < 0.5f)
		{
			EventStudent.Pathfinding.canSearch = false;
			EventStudent.Pathfinding.canMove = false;
		}
		if (Clock.HourTime > EventTime + 0.5f || EventStudent.WitnessedMurder || EventStudent.Splashed || EventStudent.Alarmed || EventStudent.Dying || !EventStudent.Alive)
		{
			EndEvent();
			return;
		}
		if (!EventStudent.Pathfinding.canMove)
		{
			if (EventPhase == 1)
			{
				Timer += Time.deltaTime;
				EventStudent.Character.GetComponent<Animation>().CrossFade(EventAnim[0]);
				AudioClipPlayer.Play(EventClip[0], EventStudent.transform.position, 5f, 10f, out VoiceClip, out CurrentClipLength);
				EventPhase++;
			}
			else if (EventPhase == 2)
			{
				Timer += Time.deltaTime;
				if (Timer > 1.5f)
				{
					EventStudent.SmartPhone.SetActive(true);
				}
				if (Timer > 3f)
				{
					AudioClipPlayer.Play(EventClip[1], EventStudent.transform.position, 5f, 10f, out VoiceClip, out CurrentClipLength);
					EventSubtitle.text = EventSpeech[1];
					Timer = 0f;
					EventPhase++;
				}
			}
			else if (EventPhase == 3)
			{
				Timer += Time.deltaTime;
				if (Timer > CurrentClipLength)
				{
					EventStudent.Character.GetComponent<Animation>().CrossFade(EventStudent.RunAnim);
					EventStudent.CurrentDestination = EventLocation;
					EventStudent.Pathfinding.target = EventLocation;
					EventStudent.Pathfinding.canSearch = true;
					EventStudent.Pathfinding.canMove = true;
					EventStudent.Pathfinding.speed = 4f;
					EventSubtitle.text = string.Empty;
					Timer = 0f;
					EventPhase++;
				}
			}
			else if (EventPhase == 4)
			{
				DumpPoint.enabled = true;
				EventStudent.Character.GetComponent<Animation>().CrossFade(EventAnim[2]);
				AudioClipPlayer.Play(EventClip[2], EventStudent.transform.position, 5f, 10f, out VoiceClip, out CurrentClipLength);
				EventPhase++;
			}
			else if (EventPhase < 13)
			{
				if (VoiceClip != null)
				{
					VoiceClip.GetComponent<AudioSource>().pitch = Time.timeScale;
					EventStudent.Character.GetComponent<Animation>()[EventAnim[2]].time = VoiceClip.GetComponent<AudioSource>().time;
					if (VoiceClip.GetComponent<AudioSource>().time > SpeechTimes[EventPhase - 3])
					{
						EventSubtitle.text = EventSpeech[EventPhase - 3];
						EventPhase++;
					}
				}
			}
			else
			{
				if (EventStudent.Character.GetComponent<Animation>()[EventAnim[2]].time >= EventStudent.Character.GetComponent<Animation>()[EventAnim[2]].length * 90.33333f)
				{
					EventStudent.SmartPhone.SetActive(true);
				}
				if (EventStudent.Character.GetComponent<Animation>()[EventAnim[2]].time >= EventStudent.Character.GetComponent<Animation>()[EventAnim[2]].length)
				{
					EndEvent();
				}
			}
			float num = Vector3.Distance(Yandere.transform.position, EventStudent.transform.position);
			if (num < 10f)
			{
				float num2 = Mathf.Abs((num - 10f) * 0.2f);
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				EventSubtitle.transform.localScale = new Vector3(num2, num2, num2);
			}
			else
			{
				EventSubtitle.transform.localScale = Vector3.zero;
			}
			if (base.enabled && EventPhase > 4)
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
			if (EventPhase == 11 && num < 5f && !EventGlobals.Event2)
			{
				EventGlobals.Event2 = true;
				Yandere.NotificationManager.DisplayNotification(NotificationType.Info);
				ConversationGlobals.SetTopicDiscovered(25, true);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Topic);
				Yandere.NotificationManager.DisplayNotification(NotificationType.Opinion);
				ConversationGlobals.SetTopicLearnedByStudent(25, EventStudentID, true);
			}
		}
		if ((!EventStudent.Pathfinding.canMove && EventPhase <= 3) || !(EventFriend != null) || EventPhase <= 3)
		{
			return;
		}
		if (EventFriend.CurrentDestination != SpyLocation)
		{
			Timer += Time.deltaTime;
			if (Timer > 2f)
			{
				EventFriend.Character.GetComponent<Animation>().CrossFade(EventStudent.RunAnim);
				EventFriend.CurrentDestination = SpyLocation;
				EventFriend.Pathfinding.target = SpyLocation;
				EventFriend.Pathfinding.canSearch = true;
				EventFriend.Pathfinding.canMove = true;
				EventFriend.Pathfinding.speed = 4f;
				EventFriend.Routine = true;
				Timer = 0f;
			}
			else
			{
				EventFriend.targetRotation = Quaternion.LookRotation(StudentManager.Students[EventStudentID].transform.position - EventFriend.transform.position);
				EventFriend.transform.rotation = Quaternion.Slerp(EventFriend.transform.rotation, EventFriend.targetRotation, 10f * Time.deltaTime);
			}
		}
		else
		{
			Debug.Log("Friend is heading to destination.");
			if (EventFriend.DistanceToDestination < 0.5f)
			{
				EventFriend.CharacterAnimation.CrossFade("f02_cornerPeek_00");
				EventFriend.Pathfinding.canSearch = false;
				EventFriend.Pathfinding.canMove = false;
				SettleFriend();
			}
		}
	}

	private void SettleFriend()
	{
		EventFriend.MoveTowardsTarget(SpyLocation.position);
		float num = Quaternion.Angle(EventFriend.transform.rotation, SpyLocation.rotation);
		if (num > 1f)
		{
			EventFriend.transform.rotation = Quaternion.Slerp(EventFriend.transform.rotation, SpyLocation.rotation, 10f * Time.deltaTime);
		}
	}

	private void EndEvent()
	{
		Debug.Log("A phone event ended.");
		if (!EventOver)
		{
			if (VoiceClip != null)
			{
				UnityEngine.Object.Destroy(VoiceClip);
			}
			if (EventFriend != null)
			{
				Debug.Log("Osana's friend is exiting the phone event.");
				EventFriend.CurrentDestination = EventFriend.Destinations[EventFriend.Phase];
				EventFriend.Pathfinding.target = EventFriend.Destinations[EventFriend.Phase];
				EventFriend.Obstacle.checkTime = 1f;
				EventFriend.Pathfinding.speed = 1f;
				EventFriend.TargetDistance = 1f;
				EventFriend.InEvent = false;
				EventFriend.Private = false;
				EventFriend.Routine = true;
				EventFriend.CanTalk = true;
				OsanaClubEvent.enabled = true;
			}
			EventStudent.CurrentDestination = EventStudent.Destinations[EventStudent.Phase];
			EventStudent.Pathfinding.target = EventStudent.Destinations[EventStudent.Phase];
			EventStudent.Obstacle.checkTime = 1f;
			if (!EventStudent.Dying)
			{
				EventStudent.Prompt.enabled = true;
			}
			if (!EventStudent.WitnessedMurder)
			{
				EventStudent.SmartPhone.SetActive(false);
			}
			EventStudent.Pathfinding.speed = 1f;
			EventStudent.TargetDistance = 1f;
			EventStudent.PhoneEvent = null;
			EventStudent.InEvent = false;
			EventStudent.Private = false;
			EventStudent.CanTalk = true;
			EventSubtitle.text = string.Empty;
			StudentManager.UpdateStudents();
		}
		Yandere.Eavesdropping = false;
		EventActive = false;
		EventCheck = false;
		base.enabled = false;
	}
}
