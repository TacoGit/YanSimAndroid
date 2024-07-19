using System;
using UnityEngine;

public class OsanaMondayBeforeClassEventScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public UILabel EventSubtitle;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript Rival;

	public Transform Destination;

	public AudioClip SpeechClip;

	public string[] SpeechText;

	public float[] SpeechTime;

	public GameObject AlarmDisc;

	public GameObject VoiceClip;

	public GameObject[] Bentos;

	public bool EventActive;

	public float Distance;

	public float Scale;

	public float Timer;

	public int SpeechPhase = 1;

	public int RivalID = 11;

	public int Phase;

	public int Frame;

	private void Start()
	{
		EventSubtitle.transform.localScale = Vector3.zero;
		Bentos[1].SetActive(false);
		Bentos[2].SetActive(false);
		if (DateGlobals.Weekday != DayOfWeek.Monday)
		{
			base.enabled = false;
		}
	}
}
