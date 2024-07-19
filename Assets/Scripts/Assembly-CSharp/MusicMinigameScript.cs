using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicMinigameScript : MonoBehaviour
{
	public GameObject[] NoteIcons;

	public Transform[] Scales;

	public Renderer[] Stars;

	public InputManagerScript InputManager;

	public Renderer HealthBarRenderer;

	public Renderer Black;

	public Transform ReputationMarker;

	public Transform ReputationBar;

	public Transform HealthBar;

	public Transform SadMiyuji;

	public Transform SadAyano;

	public GameObject GameOverScreen;

	public AudioSource MyAudio;

	public UILabel CurrentRep;

	public UILabel RepBonus;

	public Texture EmptyStar;

	public Texture GoldStar;

	public float JumpStrength;

	public float CringeTimer;

	public float StartRep;

	public float Health;

	public float Alpha;

	public float Power;

	public float Speed;

	public float Timer;

	public float[] Phase1Times;

	public int[] Phase1Notes;

	public float[] Phase2Times;

	public int[] Phase2Notes;

	public float[] Times;

	public int[] Notes;

	public int CurrentNote;

	public int Excitement;

	public int Phase;

	public int Note;

	public int ID;

	public bool SettingNotes;

	public bool LockHealth;

	public bool GameOver;

	public bool KeyDown;

	public bool Won;

	public Texture[] ChibiCelebrate;

	public Texture[] ChibiPerform;

	public Texture[] ChibiPerformB;

	public Texture[] ChibiCringe;

	public Texture[] ChibiIdle;

	public ParticleSystem[] MusicNotes;

	public AudioClip[] Celebrations;

	public Renderer[] ChibiRenderer;

	public Transform[] Instruments;

	public float[] AnimTimer;

	public float[] PingPong;

	public float[] Rotation;

	public float[] Jump;

	public bool[] ChibiSway;

	public bool[] FrameB;

	public bool[] Ping;

	private void Start()
	{
		Application.targetFrameRate = 60;
		Time.timeScale = 1f;
		Black.gameObject.SetActive(true);
		GameOverScreen.SetActive(false);
		Scales[0].localPosition = new Vector3(-1f, 0f, 0f);
		Scales[1].localPosition = new Vector3(0f, 0f, 0f);
		Scales[2].localPosition = new Vector3(1f, 0f, 0f);
		Scales[3].localPosition = new Vector3(2f, 0f, 0f);
		for (ID = 0; ID < Phase1Times.Length; ID++)
		{
			Times[ID] = Phase1Times[ID];
			Notes[ID] = Phase1Notes[ID];
		}
		for (ID = 0; ID < Phase2Times.Length; ID++)
		{
			Times[ID + 216] = Phase2Times[ID];
			Notes[ID + 216] = Phase2Notes[ID];
		}
		for (ID = 0; ID < Times.Length; ID++)
		{
			Times[ID] += 3f;
		}
		UpdateHealthBar();
		ReputationBar.localScale = new Vector3(0f, 0f, 0f);
		Black.material.color = new Color(0f, 0f, 0f, 1f);
	}

	private void Update()
	{
		for (ID = 0; ID < Scales.Length; ID++)
		{
			Scales[ID].localPosition -= new Vector3(Time.deltaTime * Speed, 0f, 0f);
			if (Scales[ID].localPosition.x < -2f)
			{
				Scales[ID].localPosition += new Vector3(4f, 0f, 0f);
			}
		}
		if (Input.GetKeyDown("escape"))
		{
			GameOver = true;
			Timer = 9f;
		}
		if (Input.GetKeyDown("l"))
		{
			LockHealth = !LockHealth;
		}
		if (GameOver)
		{
			MyAudio.pitch = Mathf.MoveTowards(MyAudio.pitch, 0f, Time.deltaTime * 0.33333f);
			Timer += Time.deltaTime;
			if (!(Timer > 4f))
			{
				return;
			}
			if (!GameOverScreen.activeInHierarchy)
			{
				SadMiyuji.localPosition = new Vector3(-0.51f, -0.1f, -0.2f);
				SadAyano.localPosition = new Vector3(0.495f, -0.1f, -0.2f);
				GameOverScreen.SetActive(true);
			}
			SadMiyuji.localPosition = Vector3.Lerp(SadMiyuji.localPosition, new Vector3(-0.455f, -0.1f, -0.2f), Time.deltaTime);
			SadAyano.localPosition = Vector3.Lerp(SadAyano.localPosition, new Vector3(0.44f, -0.1f, -0.2f), Time.deltaTime);
			if (Timer > 9f)
			{
				Alpha = Mathf.MoveTowards(Alpha, 1f, Time.deltaTime);
				Black.material.color = new Color(0f, 0f, 0f, Alpha);
				if (Alpha == 1f)
				{
					Quit();
				}
			}
			return;
		}
		if (!Won)
		{
			Alpha = Mathf.MoveTowards(Alpha, 0f, Time.deltaTime);
			Black.material.color = new Color(0f, 0f, 0f, Alpha);
			Timer += Time.deltaTime;
			if (!MyAudio.isPlaying)
			{
				if (Timer > 3f || Input.GetKeyDown("space"))
				{
					if (Timer < MyAudio.clip.length)
					{
						MyAudio.Play();
					}
					else
					{
						ChibiRenderer[1].material.mainTexture = ChibiCelebrate[1];
						ChibiRenderer[2].material.mainTexture = ChibiCelebrate[2];
						ChibiRenderer[3].material.mainTexture = ChibiCelebrate[3];
						ChibiRenderer[4].material.mainTexture = ChibiCelebrate[4];
						ChibiRenderer[5].material.mainTexture = ChibiCelebrate[5];
						ChibiRenderer[6].material.mainTexture = ChibiCelebrate[6];
						Jump[1] = JumpStrength;
						Jump[2] = JumpStrength * 0.9f;
						Jump[3] = JumpStrength * 0.8f;
						Jump[4] = JumpStrength * 0.7f;
						Jump[5] = JumpStrength * 0.6f;
						Jump[6] = JumpStrength * 0.5f;
						if (Health == 200f)
						{
							Excitement = 3;
						}
						else if (Health > 0f)
						{
							Excitement = 2;
						}
						else
						{
							Excitement = 1;
						}
						MyAudio.clip = Celebrations[Excitement];
						MyAudio.loop = false;
						MyAudio.Play();
						Won = true;
						Timer = 0f;
					}
				}
			}
			else
			{
				if (Input.GetKeyDown("space"))
				{
					MyAudio.time += 10f;
					Timer = MyAudio.time + 3f;
				}
				if (Input.GetKeyDown("z"))
				{
					MyAudio.time = MyAudio.clip.length - Time.deltaTime;
				}
				if (MyAudio.time > 131f)
				{
					ChibiSway[2] = false;
					ChibiSway[6] = false;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if ((double)MyAudio.time > 88.2833333)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = true;
					ChibiSway[5] = true;
					ChibiSway[4] = true;
				}
				else if ((double)MyAudio.time > 74.25)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = true;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if (MyAudio.time > 60f)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if ((double)MyAudio.time > 45.933333)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = true;
					ChibiSway[5] = true;
					ChibiSway[4] = true;
				}
				else if ((double)MyAudio.time > 45.08)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = false;
					ChibiSway[5] = true;
					ChibiSway[4] = true;
				}
				else if ((double)MyAudio.time > 35.33333)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = false;
					ChibiSway[3] = false;
					ChibiSway[5] = true;
					ChibiSway[4] = true;
				}
				else if ((double)MyAudio.time > 31.833333)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = false;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if ((double)MyAudio.time > 30.33333)
				{
					ChibiSway[2] = false;
					ChibiSway[6] = false;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if ((double)MyAudio.time > 28.2833333)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if ((double)MyAudio.time > 7.1166666)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = true;
					ChibiSway[5] = true;
					ChibiSway[4] = false;
				}
				else if ((double)MyAudio.time > 3.5833333)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = true;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				else if (MyAudio.time > 0f)
				{
					ChibiSway[2] = true;
					ChibiSway[6] = false;
					ChibiSway[3] = false;
					ChibiSway[5] = false;
					ChibiSway[4] = false;
				}
				if (MyAudio.time > 33f && MyAudio.time < 36.833332f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 39.5f && MyAudio.time < 43.25f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 46.833332f && MyAudio.time < 49.75f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 50.383335f && MyAudio.time < 53f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 53.916668f && MyAudio.time < 59f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 59.5f && MyAudio.time < 74.33333f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 77f && MyAudio.time < 80.33333f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 84.05f && MyAudio.time < 88.166664f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 91f && MyAudio.time < 98.5f)
				{
					ChibiSway[1] = true;
				}
				else if (MyAudio.time > 101.833336f && MyAudio.time < 130.58333f)
				{
					ChibiSway[1] = true;
				}
				else
				{
					ChibiSway[1] = false;
				}
				if (CringeTimer == 0f)
				{
					MyAudio.volume = 1f;
				}
				for (ID = 1; ID < ChibiSway.Length; ID++)
				{
					if (CringeTimer > 0f)
					{
						ChibiRenderer[ID].transform.localPosition = new Vector3(Random.Range(-0.01f, 0.01f), 0.15f + Random.Range(-0.01f, 0.01f), 0f);
						CringeTimer = Mathf.MoveTowards(CringeTimer, 0f, Time.deltaTime);
						if (CringeTimer == 0f)
						{
							ChibiRenderer[ID].transform.localPosition = new Vector3(0f, 0.15f, 0f);
						}
					}
					else if (ChibiSway[ID])
					{
						if (!MusicNotes[ID].isPlaying)
						{
							MusicNotes[ID].Play();
						}
						AnimTimer[ID] += Time.deltaTime;
						if (AnimTimer[ID] > 0.2f)
						{
							FrameB[ID] = !FrameB[ID];
							AnimTimer[ID] = 0f;
						}
						if (FrameB[ID])
						{
							ChibiRenderer[ID].material.mainTexture = ChibiPerform[ID];
						}
						else
						{
							ChibiRenderer[ID].material.mainTexture = ChibiPerformB[ID];
						}
						if (ID < 6)
						{
							if (Ping[ID])
							{
								PingPong[ID] += Time.deltaTime * 5f;
								if (PingPong[ID] > 1f)
								{
									Ping[ID] = false;
								}
							}
							else
							{
								PingPong[ID] -= Time.deltaTime * 5f;
								if (PingPong[ID] < -1f)
								{
									Ping[ID] = true;
								}
							}
							Rotation[ID] += PingPong[ID] * Time.deltaTime * 10f;
							if (Rotation[ID] > 7.5f)
							{
								Rotation[ID] = 7.5f;
							}
							else if (Rotation[ID] < -7.5f)
							{
								Rotation[ID] = -7.5f;
							}
						}
					}
					else
					{
						if (ID < 6)
						{
							Rotation[ID] = Mathf.MoveTowards(Rotation[ID], 0f, Time.deltaTime * 100f);
						}
						if (ChibiRenderer[ID].material.mainTexture != ChibiIdle[ID])
						{
							ChibiRenderer[ID].material.mainTexture = ChibiIdle[ID];
							MusicNotes[ID].Stop();
							PingPong[ID] = -1f;
							Ping[ID] = false;
						}
					}
					Instruments[ID].localEulerAngles = new Vector3(0f, 0f, Rotation[ID]);
				}
			}
			if (SettingNotes)
			{
				if (Input.GetKeyDown("up"))
				{
					if (Phase == 1)
					{
						Phase1Times[Note] = MyAudio.time;
						Phase1Notes[Note] = 1;
					}
					else
					{
						Phase2Times[Note] = MyAudio.time;
						Phase2Notes[Note] = 1;
					}
					Note++;
				}
				else if (Input.GetKeyDown("right"))
				{
					if (Phase == 1)
					{
						Phase1Times[Note] = MyAudio.time;
						Phase1Notes[Note] = 2;
					}
					else
					{
						Phase2Times[Note] = MyAudio.time;
						Phase2Notes[Note] = 2;
					}
					Note++;
				}
				else if (Input.GetKeyDown("left"))
				{
					if (Phase == 1)
					{
						Phase1Times[Note] = MyAudio.time;
						Phase1Notes[Note] = 3;
					}
					else
					{
						Phase2Times[Note] = MyAudio.time;
						Phase2Notes[Note] = 3;
					}
					Note++;
				}
				else if (Input.GetKeyDown("down"))
				{
					if (Phase == 1)
					{
						Phase1Times[Note] = MyAudio.time;
						Phase1Notes[Note] = 4;
					}
					else
					{
						Phase2Times[Note] = MyAudio.time;
						Phase2Notes[Note] = 4;
					}
					Note++;
				}
				return;
			}
			if (Input.GetKeyUp("up") || Input.GetKeyUp("right") || Input.GetKeyUp("down") || Input.GetKeyUp("left"))
			{
				KeyDown = false;
			}
			if (!InputManager.TappedUp && !InputManager.TappedDown && !InputManager.TappedLeft && !InputManager.TappedRight)
			{
				KeyDown = false;
			}
			if (Note < Notes.Length && Notes[Note] > 0 && Timer + 2f > Times[Note])
			{
				GameObject gameObject = Object.Instantiate(NoteIcons[Notes[Note]], base.transform.position, Quaternion.identity);
				gameObject.GetComponent<MusicNoteScript>().InputManager = InputManager;
				gameObject.GetComponent<MusicNoteScript>().MusicMinigame = this;
				gameObject.GetComponent<MusicNoteScript>().ID = Note;
				gameObject.transform.parent = Scales[0].parent;
				if (Notes[Note] == 1)
				{
					gameObject.transform.localPosition = new Vector3(1.5f, 0.15f, -0.0001f);
				}
				else if (Notes[Note] == 2)
				{
					gameObject.transform.localPosition = new Vector3(1.5f, 0.05f, -0.0001f);
				}
				else if (Notes[Note] == 3)
				{
					gameObject.transform.localPosition = new Vector3(1.5f, -0.05f, -0.0001f);
				}
				else if (Notes[Note] == 4)
				{
					gameObject.transform.localPosition = new Vector3(1.5f, -0.15f, -0.0001f);
				}
				gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				Note++;
			}
			return;
		}
		for (ID = 1; ID < Instruments.Length; ID++)
		{
			if (ID != 2 && ID != 6)
			{
				ChibiRenderer[ID].transform.localPosition += new Vector3(0f, Jump[ID], 0f);
				Jump[ID] -= Time.deltaTime * 0.01f;
				if (ChibiRenderer[ID].transform.localPosition.y < 0.15f)
				{
					ChibiRenderer[ID].transform.localPosition = new Vector3(0f, 0.15f, 0f);
					Jump[ID] = JumpStrength;
				}
			}
		}
		if (MyAudio.isPlaying)
		{
			return;
		}
		if (Timer == 0f)
		{
			StartRep = PlayerPrefs.GetFloat("TempReputation");
			CurrentRep.text = string.Empty + StartRep;
			if (Health > 100f)
			{
				RepBonus.text = "+" + (Health - 100f);
			}
			ReputationMarker.localPosition = new Vector3(StartRep * 0.01f, 0f, 0f);
		}
		ReputationBar.localScale = Vector3.Lerp(ReputationBar.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		Timer += Time.deltaTime;
		if (Timer > 1f && Health > 100f)
		{
			float num = StartRep + (Health - 100f);
			if (num > 100f)
			{
				num = 100f;
			}
			CurrentRep.text = string.Empty + num;
			Power += Time.deltaTime;
			ReputationMarker.localPosition = Vector3.Lerp(ReputationMarker.localPosition, new Vector3(num * 0.01f, 0f, -0.0002f), Power);
		}
		if (Timer > 5f)
		{
			Alpha = Mathf.MoveTowards(Alpha, 1f, Time.deltaTime);
			Black.material.color = new Color(0f, 0f, 0f, Alpha);
			if (Alpha == 1f)
			{
				Quit();
			}
		}
	}

	public void UpdateHealthBar()
	{
		if (Health > 200f)
		{
			Health = 200f;
		}
		if (Health <= 0f)
		{
			MyAudio.volume = 1f;
			GameOver = true;
			Health = 0f;
			Timer = 0f;
		}
		else
		{
			HealthBar.localScale = new Vector3(1f, Health / 200f, 1f);
			HealthBarRenderer.material.color = new Color(1f - Health / 200f, Health / 200f, 0f, 1f);
		}
		if (Health > 100f)
		{
			Stars[1].material.mainTexture = GoldStar;
		}
		else
		{
			Stars[1].material.mainTexture = EmptyStar;
		}
		if (Health > 125f)
		{
			Stars[2].material.mainTexture = GoldStar;
		}
		else
		{
			Stars[2].material.mainTexture = EmptyStar;
		}
		if (Health > 150f)
		{
			Stars[3].material.mainTexture = GoldStar;
		}
		else
		{
			Stars[3].material.mainTexture = EmptyStar;
		}
		if (Health > 175f)
		{
			Stars[4].material.mainTexture = GoldStar;
		}
		else
		{
			Stars[4].material.mainTexture = EmptyStar;
		}
		if (Health == 200f)
		{
			Stars[5].material.mainTexture = GoldStar;
		}
		else
		{
			Stars[5].material.mainTexture = EmptyStar;
		}
	}

	public void Cringe()
	{
		for (ID = 1; ID < ChibiRenderer.Length; ID++)
		{
			ChibiRenderer[ID].material.mainTexture = ChibiCringe[ID];
			MusicNotes[ID].Stop();
			Rotation[ID] = 0f;
		}
		MyAudio.volume = 0f;
		CringeTimer = 1f;
	}

	public void Quit()
	{
		if (Health > 100f)
		{
			PlayerPrefs.SetFloat("TempReputation", StartRep + (Health - 100f));
		}
		else
		{
			PlayerPrefs.SetFloat("TempReputation", 0f);
		}
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		foreach (GameObject gameObject in rootGameObjects)
		{
			gameObject.SetActive(true);
		}
		SceneManager.UnloadSceneAsync(19);
	}
}
