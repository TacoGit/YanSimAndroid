using UnityEngine;

public class ConfessionManagerScript : MonoBehaviour
{
	public ShoulderCameraScript ShoulderCamera;

	public StudentManagerScript StudentManager;

	public HeartbrokenScript Heartbroken;

	public JukeboxScript OriginalJukebox;

	public CosmeticScript OsanaCosmetic;

	public AudioClip ConfessionAccepted;

	public AudioClip ConfessionRejected;

	public AudioClip ConfessionGiggle;

	public AudioClip[] ConfessionMusic;

	public GameObject OriginalBlossoms;

	public GameObject HeartBeatCamera;

	public GameObject MainCamera;

	public Transform ConfessionCamera;

	public Transform OriginalPOV;

	public Transform ReactionPOV;

	public Transform SenpaiNeck;

	public Transform SenpaiPOV;

	public string[] ConfessSubs;

	public string[] AcceptSubs;

	public string[] RejectSubs;

	public float[] ConfessTimes;

	public float[] AcceptTimes;

	public float[] RejectTimes;

	public UISprite TimelessDarkness;

	public UILabel SubtitleLabel;

	public UISprite Darkness;

	public UIPanel Panel;

	public AudioSource MyAudio;

	public AudioSource Jukebox;

	public Animation Yandere;

	public Animation Senpai;

	public Animation Osana;

	public Renderer Tears;

	public float RotateSpeed;

	public float TearSpeed;

	public float TearTimer;

	public float Timer;

	public bool ReverseTears;

	public bool FadeOut;

	public bool Reject;

	public int TearPhase;

	public int Phase;

	public int MusicID;

	public int SubID;

	private void Start()
	{
		ConfessionCamera.gameObject.SetActive(false);
		TimelessDarkness.color = new Color(0f, 0f, 0f, 0f);
		Darkness.color = new Color(0f, 0f, 0f, 1f);
		SubtitleLabel.text = string.Empty;
	}

	private void Update()
	{
		Timer += Time.deltaTime;
		if (Phase == -1)
		{
			TimelessDarkness.color = new Color(TimelessDarkness.color.r, TimelessDarkness.color.g, TimelessDarkness.color.b, Mathf.MoveTowards(TimelessDarkness.color.a, 1f, Time.deltaTime));
			Panel.alpha = Mathf.MoveTowards(Panel.alpha, 0f, Time.deltaTime);
			OriginalJukebox.Volume = Mathf.MoveTowards(OriginalJukebox.Volume, 0f, Time.deltaTime);
			if (TimelessDarkness.color.a == 1f && Timer > 2f)
			{
				TimelessDarkness.color = new Color(0f, 0f, 0f, 0f);
				Darkness.color = new Color(0f, 0f, 0f, 1f);
				ConfessionCamera.gameObject.SetActive(true);
				MainCamera.SetActive(false);
				OsanaCosmetic = StudentManager.Students[StudentManager.RivalID].Cosmetic;
				Osana = StudentManager.Students[StudentManager.RivalID].CharacterAnimation;
				Tears = StudentManager.Students[StudentManager.RivalID].Tears;
				Senpai = StudentManager.Students[1].CharacterAnimation;
				SenpaiNeck = StudentManager.Students[1].Neck;
				Osana[OsanaCosmetic.Student.ShyAnim].weight = 0f;
				Senpai["SenpaiConfession"].speed = 0.9f;
				OriginalBlossoms.SetActive(false);
				Tears.gameObject.SetActive(true);
				Osana.transform.position = new Vector3(0f, 6f, 98.5f);
				Senpai.transform.position = new Vector3(0f, 6f, 98.5f);
				Osana.transform.eulerAngles = new Vector3(0f, 180f, 0f);
				Senpai.transform.eulerAngles = new Vector3(0f, 180f, 0f);
				OsanaCosmetic.MyRenderer.materials[OsanaCosmetic.FaceID].SetFloat("_BlendAmount", 1f);
				Senpai.Play("SenpaiConfession");
				Osana.Play("OsanaConfession");
				OriginalBlossoms.SetActive(false);
				HeartBeatCamera.SetActive(false);
				GetComponent<AudioSource>().Play();
				Jukebox.Play();
				Timer = 0f;
				Phase++;
				Yandere.transform.parent.position = new Vector3(5f, 5.73f, 98f);
				Yandere.transform.parent.eulerAngles = new Vector3(0f, -90f, 0f);
			}
		}
		else if (Phase == 0)
		{
			if (Timer > 11f)
			{
				FadeOut = true;
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 1)
		{
			if (Timer > 2f)
			{
				ConfessionCamera.eulerAngles = SenpaiPOV.eulerAngles;
				ConfessionCamera.position = SenpaiPOV.position;
				Senpai.gameObject.SetActive(false);
				Osana["OsanaConfession"].time = 11f;
				MyAudio.volume = 1f;
				MyAudio.time = 8f;
				FadeOut = false;
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (SubID < ConfessTimes.Length && Osana["OsanaConfession"].time > ConfessTimes[SubID] + 3f)
			{
				SubtitleLabel.text = string.Empty + ConfessSubs[SubID];
				SubID++;
			}
			RotateSpeed += Time.deltaTime * 0.2f;
			ConfessionCamera.eulerAngles = Vector3.Lerp(ConfessionCamera.eulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * RotateSpeed);
			ConfessionCamera.position = Vector3.Lerp(ConfessionCamera.position, new Vector3(0f, 7.25f, 97f), Time.deltaTime * RotateSpeed);
			if (Osana["OsanaConfession"].time >= Osana["OsanaConfession"].length)
			{
				if (DatingGlobals.RivalSabotaged > 4)
				{
					Reject = true;
				}
				if (!Reject)
				{
					Osana.CrossFade("OsanaConfessionAccepted");
					MyAudio.clip = ConfessionAccepted;
				}
				else
				{
					Osana.CrossFade("OsanaConfessionRejected");
					MyAudio.clip = ConfessionRejected;
				}
				MyAudio.time = 0f;
				MyAudio.Play();
				Jukebox.Stop();
				SubtitleLabel.text = string.Empty;
				RotateSpeed = 0f;
				SubID = 0;
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 3)
		{
			if (!Reject)
			{
				if (SubID < AcceptTimes.Length && Osana["OsanaConfessionAccepted"].time > AcceptTimes[SubID])
				{
					SubtitleLabel.text = string.Empty + AcceptSubs[SubID];
					SubID++;
				}
				if (TearPhase == 0)
				{
					if (Timer > 26f)
					{
						ReverseTears = true;
						TearSpeed = 5f;
						TearPhase++;
					}
				}
				else if (TearPhase == 1)
				{
					if ((double)Timer > 33.33333)
					{
						ReverseTears = true;
						TearSpeed = 5f;
						TearPhase++;
					}
				}
				else if (TearPhase == 2)
				{
					if (Timer > 39f)
					{
						ReverseTears = true;
						TearSpeed = 5f;
						TearPhase++;
					}
				}
				else if (TearPhase == 3 && Timer > 40f)
				{
					TearPhase++;
				}
				if (Timer > 10f)
				{
					if (!Jukebox.isPlaying)
					{
						Jukebox.clip = ConfessionMusic[4];
						Jukebox.loop = true;
						Jukebox.volume = 0f;
						Jukebox.Play();
					}
					Jukebox.volume = Mathf.MoveTowards(Jukebox.volume, 0.05f, Time.deltaTime * 0.01f);
					if (!ReverseTears)
					{
						TearTimer = Mathf.MoveTowards(TearTimer, 1f, Time.deltaTime * TearSpeed);
					}
					else
					{
						TearTimer = Mathf.MoveTowards(TearTimer, 0f, Time.deltaTime * TearSpeed);
						if (TearTimer == 0f)
						{
							ReverseTears = false;
							TearSpeed = 0.2f;
						}
					}
					if (TearPhase < 4)
					{
						Tears.materials[0].SetFloat("_TearReveal", TearTimer);
					}
					Tears.materials[1].SetFloat("_TearReveal", TearTimer);
				}
				if (Input.GetKeyDown("space"))
				{
					Jukebox.clip = ConfessionMusic[4];
					Jukebox.loop = true;
					Jukebox.volume = 0.05f;
					Jukebox.Play();
					Osana["OsanaConfessionAccepted"].time = 43f;
					MyAudio.Stop();
					Timer = 43f;
				}
				if (Timer > 43f)
				{
					TearSpeed = 0.1f;
					FadeOut = true;
					Timer = 0f;
					Phase++;
				}
			}
			else
			{
				if (SubID < RejectTimes.Length && Osana["OsanaConfessionRejected"].time > RejectTimes[SubID])
				{
					SubtitleLabel.text = string.Empty + RejectSubs[SubID];
					SubID++;
				}
				if (Input.GetKeyDown("space"))
				{
					Osana["OsanaConfessionRejected"].time = 41f;
					MyAudio.time = 41f;
					Timer = 41f;
				}
				if (Timer > 41f)
				{
					TearTimer = Mathf.MoveTowards(TearTimer, 1f, Time.deltaTime * TearSpeed);
					Tears.materials[0].SetFloat("_TearReveal", TearTimer);
					Tears.materials[1].SetFloat("_TearReveal", TearTimer);
				}
				if (Timer > 47f)
				{
					RotateSpeed += Time.deltaTime * 0.01f;
					ConfessionCamera.eulerAngles = new Vector3(ConfessionCamera.eulerAngles.x, ConfessionCamera.eulerAngles.y - RotateSpeed * 2f, ConfessionCamera.eulerAngles.z);
					ConfessionCamera.position = new Vector3(ConfessionCamera.position.x, ConfessionCamera.position.y, ConfessionCamera.position.z - RotateSpeed * 0.05f);
				}
				if (Timer > 51f)
				{
					FadeOut = true;
					Timer = 0f;
					Phase++;
				}
			}
		}
		else if (Phase == 4)
		{
			if (Reject)
			{
				RotateSpeed += Time.deltaTime * 0.01f;
				ConfessionCamera.eulerAngles = new Vector3(ConfessionCamera.eulerAngles.x, ConfessionCamera.eulerAngles.y - RotateSpeed * 2f, ConfessionCamera.eulerAngles.z);
				ConfessionCamera.position = new Vector3(ConfessionCamera.position.x, ConfessionCamera.position.y, ConfessionCamera.position.z - RotateSpeed * 0.05f);
			}
			if (Timer > 2f)
			{
				ConfessionCamera.eulerAngles = OriginalPOV.eulerAngles;
				ConfessionCamera.position = OriginalPOV.position;
				Senpai.gameObject.SetActive(true);
				if (!Reject)
				{
					Senpai.Play("SenpaiConfessionAccepted");
					Senpai["SenpaiConfessionAccepted"].time = Osana["OsanaConfessionAccepted"].time;
					Senpai.Play("SenpaiConfessionAccepted");
					Yandere.Play("YandereConfessionAccepted");
				}
				else
				{
					Senpai.Play("SenpaiConfessionRejected");
					Senpai["SenpaiConfessionRejected"].time += 2f;
				}
				SubtitleLabel.text = string.Empty;
				FadeOut = false;
				RotateSpeed = 0f;
				Timer = 0f;
				Phase++;
			}
		}
		else if (Phase == 5)
		{
			if (Timer > 5f)
			{
				if (Reject)
				{
					Yandere.Play("YandereConfessionRejected");
				}
				Jukebox.pitch = Mathf.MoveTowards(Jukebox.pitch, 0f, Time.deltaTime * 0.1f);
				RotateSpeed += Time.deltaTime * 0.5f;
				ConfessionCamera.position = Vector3.Lerp(ConfessionCamera.position, new Vector3(7f, 7f, 97.5f), Time.deltaTime * RotateSpeed);
				if (Timer > 10f)
				{
					if (Reject)
					{
						AudioSource.PlayClipAtPoint(ConfessionGiggle, Yandere.transform.position);
					}
					ConfessionCamera.eulerAngles = ReactionPOV.eulerAngles;
					ConfessionCamera.position = ReactionPOV.position;
					RotateSpeed = 0f;
					Timer = 0f;
					Phase++;
				}
			}
		}
		else if (Phase == 6)
		{
			Jukebox.pitch = Mathf.MoveTowards(Jukebox.pitch, 0f, Time.deltaTime * 0.1f);
			if (!Reject)
			{
				if (!Heartbroken.Confessed)
				{
					MainCamera.transform.eulerAngles = ConfessionCamera.eulerAngles;
					MainCamera.transform.position = ConfessionCamera.position;
					Heartbroken.Confessed = true;
					MainCamera.SetActive(true);
					Camera.main.enabled = false;
					ShoulderCamera.enabled = true;
					ShoulderCamera.Noticed = true;
					ShoulderCamera.Skip = true;
				}
				ConfessionCamera.position = MainCamera.transform.position;
			}
			else
			{
				RotateSpeed += Time.deltaTime * 0.5f;
				ConfessionCamera.position = Vector3.Lerp(ConfessionCamera.position, new Vector3(4f, 7f, 98f), Time.deltaTime * RotateSpeed);
				if (Timer > 5f)
				{
					FadeOut = true;
				}
			}
		}
		if (FadeOut)
		{
			Darkness.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime * 0.5f));
		}
		else
		{
			Darkness.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime * 0.5f));
		}
		if (Input.GetKeyDown("-"))
		{
			Time.timeScale -= 1f;
			MyAudio.pitch -= 1f;
			Jukebox.pitch -= 1f;
		}
		if (Input.GetKeyDown("="))
		{
			Time.timeScale += 1f;
			MyAudio.pitch += 1f;
			Jukebox.pitch += 1f;
		}
	}

	private void LateUpdate()
	{
		if (Phase > 4 && Reject)
		{
			SenpaiNeck.eulerAngles = new Vector3(SenpaiNeck.eulerAngles.x + 15f, SenpaiNeck.eulerAngles.y, SenpaiNeck.eulerAngles.z);
		}
	}
}
