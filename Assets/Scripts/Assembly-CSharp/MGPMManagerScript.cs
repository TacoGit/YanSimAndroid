using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MGPMManagerScript : MonoBehaviour
{
	public MGPMSpawnerScript[] EnemySpawner;

	public MGPMMiyukiScript Miyuki;

	public GameObject StageClearGraphic;

	public GameObject GameOverGraphic;

	public GameObject StartGraphic;

	public Renderer[] WaterRenderer;

	public Renderer RightArtwork;

	public Renderer LeftArtwork;

	public Texture RightBloody;

	public Texture LeftBloody;

	public AudioSource Jukebox;

	public AudioClip HardModeVoice;

	public AudioClip GameOverMusic;

	public AudioClip VictoryMusic;

	public AudioClip FinalBoss;

	public AudioClip BGM;

	public Renderer Black;

	public Text ScoreLabel;

	public bool StageClear;

	public bool GameOver;

	public bool FadeOut;

	public bool FadeIn;

	public bool Intro;

	public float GameOverTimer;

	public float Timer;

	public int Score;

	public int ID;

	private void Start()
	{
		if (GameGlobals.HardMode)
		{
			Jukebox.clip = HardModeVoice;
			WaterRenderer[0].material.color = Color.red;
			WaterRenderer[1].material.color = Color.red;
			RightArtwork.material.mainTexture = RightBloody;
			LeftArtwork.material.mainTexture = LeftBloody;
		}
		Miyuki.transform.localPosition = new Vector3(0f, -300f, 0f);
		Black.material.color = new Color(0f, 0f, 0f, 1f);
		StartGraphic.SetActive(false);
		Miyuki.Gameplay = false;
		for (ID = 1; ID < EnemySpawner.Length; ID++)
		{
			EnemySpawner[ID].enabled = false;
		}
		Time.timeScale = 1f;
	}

	private void Update()
	{
		ScoreLabel.text = "Score: " + Score * Miyuki.Health;
		if (StageClear)
		{
			GameOverTimer += Time.deltaTime;
			if (GameOverTimer > 1f)
			{
				Miyuki.transform.localPosition = new Vector3(Miyuki.transform.localPosition.x, Miyuki.transform.localPosition.y + Time.deltaTime * 10f, Miyuki.transform.localPosition.z);
				if (!StageClearGraphic.activeInHierarchy)
				{
					StageClearGraphic.SetActive(true);
					Jukebox.clip = VictoryMusic;
					Jukebox.loop = false;
					Jukebox.volume = 1f;
					Jukebox.Play();
				}
				if (GameOverTimer > 9f)
				{
					FadeOut = true;
				}
			}
			if (FadeOut)
			{
				Black.material.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Black.material.color.a, 1f, Time.deltaTime));
				Jukebox.volume = 1f - Black.material.color.a;
				if (Black.material.color.a == 1f)
				{
					SceneManager.LoadScene("MiyukiThanksScene");
				}
			}
			return;
		}
		if (!GameOver)
		{
			if (!Intro)
			{
				return;
			}
			if (FadeIn)
			{
				Black.material.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Black.material.color.a, 0f, Time.deltaTime));
				if (Black.material.color.a == 0f)
				{
					Jukebox.Play();
					FadeIn = false;
				}
			}
			else
			{
				Miyuki.transform.localPosition = new Vector3(0f, Mathf.MoveTowards(Miyuki.transform.localPosition.y, -120f, Time.deltaTime * 60f), 0f);
				if (Miyuki.transform.localPosition.y == -120f)
				{
					if (!Jukebox.isPlaying)
					{
						Jukebox.loop = true;
						Jukebox.clip = BGM;
						Jukebox.Play();
						if (GameGlobals.HardMode)
						{
							Jukebox.pitch = 0.2f;
						}
					}
					StartGraphic.SetActive(true);
					Timer += Time.deltaTime;
					if ((double)Timer > 3.5)
					{
						StartGraphic.SetActive(false);
						for (ID = 1; ID < EnemySpawner.Length; ID++)
						{
							EnemySpawner[ID].enabled = true;
						}
						Miyuki.Gameplay = true;
						Intro = false;
					}
				}
			}
			if (Input.GetKeyDown("space"))
			{
				StartGraphic.SetActive(false);
				for (ID = 1; ID < EnemySpawner.Length; ID++)
				{
					EnemySpawner[ID].enabled = true;
				}
				Black.material.color = new Color(0f, 0f, 0f, 0f);
				Miyuki.Gameplay = true;
				Intro = false;
				Jukebox.loop = true;
				Jukebox.clip = BGM;
				Jukebox.Play();
				if (GameGlobals.HardMode)
				{
					Jukebox.pitch = 0.2f;
				}
			}
			return;
		}
		GameOverTimer += Time.deltaTime;
		if (GameOverTimer > 3f)
		{
			if (!GameOverGraphic.activeInHierarchy)
			{
				GameOverGraphic.SetActive(true);
				Jukebox.clip = GameOverMusic;
				Jukebox.loop = false;
				Jukebox.Play();
			}
			else if (Input.anyKeyDown)
			{
				FadeOut = true;
			}
		}
		if (FadeOut)
		{
			Black.material.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Black.material.color.a, 1f, Time.deltaTime));
			Jukebox.volume = 1f - Black.material.color.a;
			if (Black.material.color.a == 1f)
			{
				SceneManager.LoadScene("MiyukiTitleScene");
			}
		}
	}

	public void BeginGameOver()
	{
		Jukebox.Stop();
		GameOver = true;
		Miyuki.enabled = false;
	}
}
