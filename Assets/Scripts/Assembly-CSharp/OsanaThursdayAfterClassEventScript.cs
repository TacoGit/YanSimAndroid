using System;
using UnityEngine;

public class OsanaThursdayAfterClassEventScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public PhoneMinigameScript PhoneMinigame;

	public UILabel EventSubtitle;

	public YandereScript Yandere;

	public ClockScript Clock;

	public StudentScript Rival;

	public Transform Location;

	public AudioClip[] SpeechClip;

	public string[] SpeechText;

	public string[] EventAnim;

	public GameObject AlarmDisc;

	public GameObject VoiceClip;

	public float Distance;

	public float Scale;

	public float Timer;

	public DayOfWeek EventDay;

	public int RivalID = 11;

	public int Phase;

	public int Frame;

	public bool Sabotaged;

	public Vector3 OriginalPosition;

	public Vector3 OriginalRotation;
}
