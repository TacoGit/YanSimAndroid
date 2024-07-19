using System;
using UnityEngine;

public class OsanaFridayBeforeClassEvent2Script : MonoBehaviour
{
	public OsanaFridayBeforeClassEvent1Script OtherEvent;

	public StudentManagerScript StudentManager;

	public AudioSoftwareScript AudioSoftware;

	public JukeboxScript Jukebox;

	public UILabel EventSubtitle;

	public YandereScript Yandere;

	public ClockScript Clock;

	public SpyScript Spy;

	public StudentScript Ganguro;

	public StudentScript Rival;

	public Transform[] Location;

	public AudioClip[] SpeechClip;

	public string[] SpeechText;

	public float[] SpeechTime;

	public string[] EventAnim;

	public GameObject AlarmDisc;

	public GameObject VoiceClip;

	public Quaternion targetRotation;

	public float Distance;

	public float Scale;

	public float Timer;

	public DayOfWeek EventDay;

	public int SpeechPhase = 1;

	public int GanguroID = 81;

	public int RivalID = 11;

	public int Phase;

	public int Frame;

	public Vector3 OriginalPosition;

	public Vector3 OriginalRotation;
}
