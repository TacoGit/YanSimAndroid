using System;
using UnityEngine;

public class OsanaFridayBeforeClassEvent1Script : MonoBehaviour
{
	public OsanaFridayBeforeClassEvent2Script OtherEvent;

	public StudentManagerScript StudentManager;

	public UILabel EventSubtitle;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript Rival;

	public Transform Location;

	public AudioClip[] SpeechClip;

	public string[] SpeechText;

	public string EventAnim;

	public GameObject AlarmDisc;

	public GameObject VoiceClip;

	public GameObject Yoogle;

	public float Distance;

	public float Scale;

	public float Timer;

	public DayOfWeek EventDay;

	public int RivalID = 11;

	public int Phase;

	public int Frame;

	public Vector3 OriginalPosition;

	public Vector3 OriginalRotation;

	private void Start()
	{
		EventSubtitle.transform.localScale = Vector3.zero;
		if (DateGlobals.Weekday != EventDay)
		{
			base.enabled = false;
		}
		Yoogle.SetActive(false);
	}
}
