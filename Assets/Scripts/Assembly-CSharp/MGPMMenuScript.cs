using UnityEngine;
using UnityEngine.SceneManagement;

public class MGPMMenuScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public AudioSource Jukebox;

	public AudioClip HardModeClip;

	public bool WindowDisplaying;

	public Transform Highlight;

	public Transform Background;

	public GameObject Controls;

	public GameObject Credits;

	public GameObject DifficultySelect;

	public GameObject MainMenu;

	public Renderer Black;

	public Renderer Logo;

	public Renderer BG;

	public Texture BloodyLogo;

	public AudioClip BGM;

	public float Rotation;

	public float Vibrate;

	public bool HardMode;

	public bool FadeOut;

	public bool FadeIn;

	public int ID;

	private void Start()
	{
		Black.material.color = new Color(0f, 0f, 0f, 1f);
		Time.timeScale = 1f;
	}

	private void Update()
	{
		Rotation -= Time.deltaTime * 3f;
		Background.localEulerAngles = new Vector3(0f, 0f, Rotation);
		if (FadeIn)
		{
			Black.material.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Black.material.color.a, 0f, Time.deltaTime));
			if (Black.material.color.a == 0f)
			{
				Jukebox.Play();
				FadeIn = false;
			}
		}
		if (FadeOut)
		{
			Black.material.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(Black.material.color.a, 1f, Time.deltaTime));
			Jukebox.volume = 1f - Black.material.color.a;
			if (Black.material.color.a == 1f)
			{
				if (ID == 4)
				{
					SceneManager.LoadScene("HomeScene");
				}
				else
				{
					GameGlobals.HardMode = HardMode;
					SceneManager.LoadScene("MiyukiGameplayScene");
				}
			}
		}
		if (FadeOut || FadeIn)
		{
			return;
		}
		if (!HardMode && Input.GetKeyDown("h"))
		{
			AudioSource.PlayClipAtPoint(HardModeClip, base.transform.position);
			Logo.material.mainTexture = BloodyLogo;
			HardMode = true;
			Vibrate = 0.1f;
		}
		if (HardMode)
		{
			Jukebox.pitch = Mathf.MoveTowards(Jukebox.pitch, 0.1f, Time.deltaTime);
			BG.material.color = new Color(Mathf.MoveTowards(BG.material.color.r, 0.5f, Time.deltaTime * 0.5f), Mathf.MoveTowards(BG.material.color.g, 0f, Time.deltaTime), Mathf.MoveTowards(BG.material.color.b, 0f, Time.deltaTime), 1f);
			Logo.transform.localPosition = new Vector3(0f, 0.5f, 2f) + new Vector3(Random.Range(Vibrate * -1f, Vibrate), Random.Range(Vibrate * -1f, Vibrate), 0f);
			Vibrate = Mathf.MoveTowards(Vibrate, 0f, Time.deltaTime * 0.1f);
		}
		if (Jukebox.clip != BGM && !Jukebox.isPlaying)
		{
			Jukebox.loop = true;
			Jukebox.clip = BGM;
			Jukebox.Play();
		}
		if (!WindowDisplaying)
		{
			if (InputManager.TappedDown)
			{
				ID++;
				UpdateHighlight();
			}
			if (InputManager.TappedUp)
			{
				ID--;
				UpdateHighlight();
			}
			if (!Input.GetButtonDown("A") && !Input.GetKeyDown("z") && !(Input.GetKeyDown("return") | Input.GetKeyDown("space")))
			{
				return;
			}
			if (MainMenu.activeInHierarchy)
			{
				if (ID == 1)
				{
					DifficultySelect.SetActive(true);
					MainMenu.SetActive(false);
					ID = 2;
					UpdateHighlight();
				}
				else if (ID == 2)
				{
					Highlight.gameObject.SetActive(false);
					Controls.SetActive(true);
					WindowDisplaying = true;
				}
				else if (ID == 3)
				{
					Highlight.gameObject.SetActive(false);
					Credits.SetActive(true);
					WindowDisplaying = true;
				}
				else if (ID == 4)
				{
					FadeOut = true;
				}
			}
			else
			{
				if (ID == 2)
				{
					GameGlobals.EasyMode = false;
				}
				else
				{
					GameGlobals.EasyMode = true;
				}
				FadeOut = true;
			}
		}
		else if (Input.GetButtonDown("B"))
		{
			Highlight.gameObject.SetActive(true);
			Controls.SetActive(false);
			Credits.SetActive(false);
			WindowDisplaying = false;
		}
	}

	private void UpdateHighlight()
	{
		if (MainMenu.activeInHierarchy)
		{
			if (ID == 0)
			{
				ID = 4;
			}
			else if (ID == 5)
			{
				ID = 1;
			}
		}
		else if (ID == 1)
		{
			ID = 3;
		}
		else if (ID == 4)
		{
			ID = 2;
		}
		Highlight.transform.position = new Vector3(0f, -0.2f * (float)ID, Highlight.transform.position.z);
	}
}
