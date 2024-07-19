using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeVideoGamesScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public HomeDarknessScript HomeDarkness;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeWindowScript HomeWindow;

	public PromptBarScript PromptBar;

	public Texture[] TitleScreens;

	public UITexture TitleScreen;

	public GameObject Controller;

	public Transform Highlight;

	public UILabel[] GameTitles;

	public Transform TV;

	public int ID = 1;

	private void Start()
	{
		if (TaskGlobals.GetTaskStatus(38) == 0)
		{
			TitleScreens[1] = TitleScreens[5];
			UILabel uILabel = GameTitles[1];
			uILabel.text = GameTitles[5].text;
			uILabel.color = new Color(uILabel.color.r, uILabel.color.g, uILabel.color.b, 0.5f);
		}
		TitleScreen.mainTexture = TitleScreens[1];
	}

	private void Update()
	{
		if (HomeCamera.Destination == HomeCamera.Destinations[5])
		{
			if (Input.GetKeyDown("y"))
			{
				TaskGlobals.SetTaskStatus(38, 1);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
			TV.localScale = Vector3.Lerp(TV.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			if (HomeYandere.CanMove)
			{
				return;
			}
			if (!HomeDarkness.FadeOut)
			{
				if (InputManager.TappedDown)
				{
					ID++;
					if (ID > 5)
					{
						ID = 1;
					}
					TitleScreen.mainTexture = TitleScreens[ID];
					Highlight.localPosition = new Vector3(Highlight.localPosition.x, 150f - (float)ID * 50f, Highlight.localPosition.z);
				}
				if (InputManager.TappedUp)
				{
					ID--;
					if (ID < 1)
					{
						ID = 5;
					}
					TitleScreen.mainTexture = TitleScreens[ID];
					Highlight.localPosition = new Vector3(Highlight.localPosition.x, 150f - (float)ID * 50f, Highlight.localPosition.z);
				}
				if (Input.GetButtonDown("A") && GameTitles[ID].color.a == 1f)
				{
					Transform transform = HomeCamera.Targets[5];
					transform.localPosition = new Vector3(transform.localPosition.x, 1.153333f, transform.localPosition.z);
					HomeDarkness.Sprite.color = new Color(HomeDarkness.Sprite.color.r, HomeDarkness.Sprite.color.g, HomeDarkness.Sprite.color.b, -1f);
					HomeDarkness.FadeOut = true;
					HomeWindow.Show = false;
					PromptBar.Show = false;
					HomeCamera.ID = 5;
				}
				if (Input.GetButtonDown("B"))
				{
					Quit();
				}
			}
			else
			{
				Transform transform2 = HomeCamera.Destinations[5];
				Transform transform3 = HomeCamera.Targets[5];
				transform2.position = new Vector3(Mathf.Lerp(transform2.position.x, transform3.position.x, Time.deltaTime * 0.75f), Mathf.Lerp(transform2.position.y, transform3.position.y, Time.deltaTime * 10f), Mathf.Lerp(transform2.position.z, transform3.position.z, Time.deltaTime * 10f));
			}
		}
		else
		{
			TV.localScale = Vector3.Lerp(TV.localScale, Vector3.zero, Time.deltaTime * 10f);
		}
	}

	public void Quit()
	{
		Controller.transform.localPosition = new Vector3(0.20385f, 0.0595f, 0.0215f);
		Controller.transform.localEulerAngles = new Vector3(-90f, -90f, 0f);
		HomeCamera.Destination = HomeCamera.Destinations[0];
		HomeCamera.Target = HomeCamera.Targets[0];
		HomeYandere.CanMove = true;
		HomeYandere.enabled = true;
		HomeWindow.Show = false;
		HomeCamera.PlayMusic();
		PromptBar.ClearButtons();
		PromptBar.Show = false;
	}
}
