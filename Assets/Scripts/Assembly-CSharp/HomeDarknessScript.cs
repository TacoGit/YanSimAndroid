using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeDarknessScript : MonoBehaviour
{
	public HomeVideoGamesScript HomeVideoGames;

	public HomeYandereScript HomeYandere;

	public HomeCameraScript HomeCamera;

	public HomeExitScript HomeExit;

	public UILabel BasementLabel;

	public UISprite Sprite;

	public bool FadeSlow;

	public bool FadeOut;

	private void Start()
	{
		Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 1f);
	}

	private void Update()
	{
		if (FadeOut)
		{
			Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, Sprite.color.a + Time.deltaTime * ((!FadeSlow) ? 1f : 0.2f));
			if (!(Sprite.color.a >= 1f))
			{
				return;
			}
			if (HomeCamera.ID != 2)
			{
				if (HomeCamera.ID == 5)
				{
					if (HomeVideoGames.ID == 1)
					{
						SceneManager.LoadScene("YanvaniaTitleScene");
					}
					else
					{
						SceneManager.LoadScene("MiyukiTitleScene");
					}
				}
				else if (HomeCamera.ID == 9)
				{
					SceneManager.LoadScene("CalendarScene");
				}
				else if (HomeCamera.ID == 10)
				{
					StudentGlobals.SetStudentKidnapped(SchoolGlobals.KidnapVictim, false);
					StudentGlobals.SetStudentSlave(SchoolGlobals.KidnapVictim);
					SceneManager.LoadScene("LoadingScene");
				}
				else if (HomeCamera.ID == 11)
				{
					EventGlobals.KidnapConversation = true;
					SceneManager.LoadScene("PhoneScene");
				}
				else if (HomeCamera.ID == 12)
				{
					SceneManager.LoadScene("LifeNoteScene");
				}
				else if (HomeExit.ID == 1)
				{
					SceneManager.LoadScene("LoadingScene");
				}
				else
				{
					if (HomeExit.ID == 2 || HomeExit.ID != 3)
					{
						return;
					}
					if (HomeYandere.transform.position.y > -5f)
					{
						HomeYandere.transform.position = new Vector3(-2f, -10f, -2f);
						HomeYandere.transform.eulerAngles = new Vector3(0f, 90f, 0f);
						HomeYandere.CanMove = true;
						FadeOut = false;
						HomeCamera.Destinations[0].position = new Vector3(2.425f, -8f, 0f);
						HomeCamera.Destination = HomeCamera.Destinations[0];
						HomeCamera.transform.position = HomeCamera.Destination.position;
						HomeCamera.Target = HomeCamera.Targets[0];
						HomeCamera.Focus.position = HomeCamera.Target.position;
						BasementLabel.text = "Upstairs";
						HomeCamera.DayLight.SetActive(true);
						Physics.SyncTransforms();
						return;
					}
					HomeYandere.transform.position = new Vector3(-1.6f, 0f, -1.6f);
					HomeYandere.transform.eulerAngles = new Vector3(0f, 45f, 0f);
					HomeYandere.CanMove = true;
					FadeOut = false;
					HomeCamera.Destinations[0].position = new Vector3(-2.0615f, 2f, 2.418f);
					HomeCamera.Destination = HomeCamera.Destinations[0];
					HomeCamera.transform.position = HomeCamera.Destination.position;
					HomeCamera.Target = HomeCamera.Targets[0];
					HomeCamera.Focus.position = HomeCamera.Target.position;
					BasementLabel.text = "Basement";
					if (HomeGlobals.Night)
					{
						HomeCamera.DayLight.SetActive(false);
					}
					Physics.SyncTransforms();
				}
			}
			else
			{
				SceneManager.LoadScene("CalendarScene");
			}
		}
		else
		{
			Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, Sprite.color.a - Time.deltaTime);
			if (Sprite.color.a < 0f)
			{
				Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 0f);
			}
		}
	}
}
