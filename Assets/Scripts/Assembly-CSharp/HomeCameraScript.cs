using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCameraScript : MonoBehaviour
{
	public HomeWindowScript[] HomeWindows;

	public HomeTriggerScript[] Triggers;

	public HomePantyChangerScript HomePantyChanger;

	public HomeSenpaiShrineScript HomeSenpaiShrine;

	public HomeVideoGamesScript HomeVideoGames;

	public HomeCorkboardScript HomeCorkboard;

	public HomeDarknessScript HomeDarkness;

	public HomeInternetScript HomeInternet;

	public HomePrisonerScript HomePrisoner;

	public HomeYandereScript HomeYandere;

	public HomeSleepScript HomeAnime;

	public HomeMangaScript HomeManga;

	public HomeSleepScript HomeSleep;

	public HomeExitScript HomeExit;

	public PromptBarScript PromptBar;

	public Vignetting Vignette;

	public UILabel PantiesMangaLabel;

	public UISprite Button;

	public GameObject ComputerScreen;

	public GameObject CorkboardLabel;

	public GameObject LoveSickCamera;

	public GameObject LoadingScreen;

	public GameObject CeilingLight;

	public GameObject SenpaiLight;

	public GameObject Controller;

	public GameObject NightLight;

	public GameObject RopeGroup;

	public GameObject DayLight;

	public GameObject Tripod;

	public GameObject Victim;

	public Transform Destination;

	public Transform Target;

	public Transform Focus;

	public Transform[] Destinations;

	public Transform[] Targets;

	public int ID;

	public AudioSource BasementJukebox;

	public AudioSource RoomJukebox;

	public AudioClip NightBasement;

	public AudioClip NightRoom;

	public AudioClip HomeLoveSick;

	public bool Torturing;

	public CosmeticScript SenpaiCosmetic;

	public Renderer HairLock;

	public Transform PromptBarPanel;

	public Transform PauseScreen;

	private void Start()
	{
		Button.color = new Color(Button.color.r, Button.color.g, Button.color.b, 0f);
		Focus.position = Target.position;
		base.transform.position = Destination.position;
		if (HomeGlobals.Night)
		{
			CeilingLight.SetActive(true);
			SenpaiLight.SetActive(true);
			NightLight.SetActive(true);
			DayLight.SetActive(false);
			Triggers[7].Disable();
			BasementJukebox.clip = NightBasement;
			RoomJukebox.clip = NightRoom;
			PlayMusic();
			PantiesMangaLabel.text = "Read Manga";
		}
		else
		{
			BasementJukebox.Play();
			RoomJukebox.Play();
			ComputerScreen.SetActive(false);
			Triggers[2].Disable();
			Triggers[3].Disable();
			Triggers[5].Disable();
			Triggers[9].Disable();
		}
		if (SchoolGlobals.KidnapVictim == 0)
		{
			RopeGroup.SetActive(false);
			Tripod.SetActive(false);
			Victim.SetActive(false);
			Triggers[10].Disable();
		}
		else
		{
			int kidnapVictim = SchoolGlobals.KidnapVictim;
			if (StudentGlobals.GetStudentArrested(kidnapVictim) || StudentGlobals.GetStudentDead(kidnapVictim))
			{
				RopeGroup.SetActive(false);
				Victim.SetActive(false);
				Triggers[10].Disable();
			}
		}
		if (GameGlobals.LoveSick)
		{
			LoveSickColorSwap();
		}
		Time.timeScale = 1f;
		HairLock.material.color = SenpaiCosmetic.ColorValue;
		if (SchoolGlobals.SchoolAtmosphere > 1f)
		{
			SchoolGlobals.SchoolAtmosphere = 1f;
		}
	}

	private void LateUpdate()
	{
		if (HomeYandere.transform.position.y > -5f)
		{
			Transform transform = Destinations[0];
			transform.position = new Vector3(0f - HomeYandere.transform.position.x, transform.position.y, transform.position.z);
		}
		Focus.position = Vector3.Lerp(Focus.position, Target.position, Time.deltaTime * 10f);
		base.transform.position = Vector3.Lerp(base.transform.position, Destination.position, Time.deltaTime * 10f);
		base.transform.LookAt(Focus.position);
		if (ID != 11 && Input.GetButtonDown("A") && HomeYandere.CanMove && ID != 0)
		{
			Destination = Destinations[ID];
			Target = Targets[ID];
			HomeWindows[ID].Show = true;
			HomeYandere.CanMove = false;
			if (ID == 1 || ID == 8)
			{
				HomeExit.enabled = true;
			}
			else if (ID == 2)
			{
				HomeSleep.enabled = true;
			}
			else if (ID == 3)
			{
				HomeInternet.enabled = true;
			}
			else if (ID == 4)
			{
				CorkboardLabel.SetActive(false);
				HomeCorkboard.enabled = true;
				LoadingScreen.SetActive(true);
				HomeYandere.gameObject.SetActive(false);
			}
			else if (ID == 5)
			{
				HomeYandere.enabled = false;
				Controller.transform.localPosition = new Vector3(0.1245f, 0.032f, 0f);
				HomeYandere.transform.position = new Vector3(1f, 0f, 0f);
				HomeYandere.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				HomeYandere.Character.GetComponent<Animation>().Play("f02_gaming_00");
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Play";
				PromptBar.Label[1].text = "Back";
				PromptBar.Label[4].text = "Select";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
			}
			else if (ID == 6)
			{
				HomeSenpaiShrine.enabled = true;
				HomeYandere.gameObject.SetActive(false);
			}
			else if (ID == 7)
			{
				HomePantyChanger.enabled = true;
			}
			else if (ID == 9)
			{
				HomeManga.enabled = true;
			}
			else if (ID == 10)
			{
				PromptBar.ClearButtons();
				PromptBar.Label[0].text = "Accept";
				PromptBar.Label[1].text = "Back";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
				HomePrisoner.UpdateDesc();
				HomeYandere.gameObject.SetActive(false);
			}
			else if (ID == 12)
			{
				HomeAnime.enabled = true;
			}
		}
		if (Destination == Destinations[0])
		{
			Vignette.intensity = ((!(HomeYandere.transform.position.y > -1f)) ? Mathf.MoveTowards(Vignette.intensity, 5f, Time.deltaTime * 5f) : Mathf.MoveTowards(Vignette.intensity, 1f, Time.deltaTime));
			Vignette.chromaticAberration = Mathf.MoveTowards(Vignette.chromaticAberration, 1f, Time.deltaTime);
			Vignette.blur = Mathf.MoveTowards(Vignette.blur, 1f, Time.deltaTime);
		}
		else
		{
			Vignette.intensity = ((!(HomeYandere.transform.position.y > -1f)) ? Mathf.MoveTowards(Vignette.intensity, 0f, Time.deltaTime * 5f) : Mathf.MoveTowards(Vignette.intensity, 0f, Time.deltaTime));
			Vignette.chromaticAberration = Mathf.MoveTowards(Vignette.chromaticAberration, 0f, Time.deltaTime);
			Vignette.blur = Mathf.MoveTowards(Vignette.blur, 0f, Time.deltaTime);
		}
		Button.color = new Color(Button.color.r, Button.color.g, Button.color.b, Mathf.MoveTowards(Button.color.a, (ID <= 0 || !HomeYandere.CanMove) ? 0f : 1f, Time.deltaTime * 10f));
		if (HomeDarkness.FadeOut)
		{
			BasementJukebox.volume = Mathf.MoveTowards(BasementJukebox.volume, 0f, Time.deltaTime);
			RoomJukebox.volume = Mathf.MoveTowards(RoomJukebox.volume, 0f, Time.deltaTime);
		}
		else if (HomeYandere.transform.position.y > -1f)
		{
			BasementJukebox.volume = Mathf.MoveTowards(BasementJukebox.volume, 0f, Time.deltaTime);
			RoomJukebox.volume = Mathf.MoveTowards(RoomJukebox.volume, 0.5f, Time.deltaTime);
		}
		else if (!Torturing)
		{
			BasementJukebox.volume = Mathf.MoveTowards(BasementJukebox.volume, 0.5f, Time.deltaTime);
			RoomJukebox.volume = Mathf.MoveTowards(RoomJukebox.volume, 0f, Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			TaskGlobals.SetTaskStatus(38, 1);
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			BasementJukebox.gameObject.SetActive(false);
			RoomJukebox.gameObject.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			HomeGlobals.Night = !HomeGlobals.Night;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if (Input.GetKeyDown(KeyCode.Equals))
		{
			Time.timeScale += 1f;
		}
		if (Input.GetKeyDown(KeyCode.Minus) && Time.timeScale > 1f)
		{
			Time.timeScale -= 1f;
		}
	}

	public void PlayMusic()
	{
		if (!YanvaniaGlobals.DraculaDefeated && !HomeGlobals.MiyukiDefeated)
		{
			if (!BasementJukebox.isPlaying)
			{
				BasementJukebox.Play();
			}
			if (!RoomJukebox.isPlaying)
			{
				RoomJukebox.Play();
			}
		}
	}

	private void LoveSickColorSwap()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.transform.parent != PauseScreen && gameObject.transform.parent != PromptBarPanel)
			{
				UISprite component = gameObject.GetComponent<UISprite>();
				if (component != null && component.color != Color.black)
				{
					component.color = new Color(1f, 0f, 0f, component.color.a);
				}
				UILabel component2 = gameObject.GetComponent<UILabel>();
				if (component2 != null && component2.color != Color.black)
				{
					component2.color = new Color(1f, 0f, 0f, component2.color.a);
				}
			}
		}
		DayLight.GetComponent<Light>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
		HomeDarkness.Sprite.color = Color.black;
		BasementJukebox.clip = HomeLoveSick;
		RoomJukebox.clip = HomeLoveSick;
		LoveSickCamera.SetActive(true);
		PlayMusic();
	}
}
