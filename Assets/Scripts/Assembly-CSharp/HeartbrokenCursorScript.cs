using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartbrokenCursorScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public InputManagerScript InputManager;

	public HeartbrokenScript Heartbroken;

	public UISprite Darkness;

	public UILabel Continue;

	public UILabel MyLabel;

	public bool LoveSick;

	public bool FadeOut;

	public bool Nudge;

	public int Selected = 1;

	public int Options = 4;

	public AudioClip SelectSound;

	public AudioClip MoveSound;

	private void Start()
	{
		Darkness.transform.localPosition = new Vector3(Darkness.transform.localPosition.x, Darkness.transform.localPosition.y, -989f);
		Continue.color = new Color(Continue.color.r, Continue.color.g, Continue.color.b, 0f);
	}

	private void Update()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(base.transform.localPosition.y, 255f - (float)Selected * 50f, Time.deltaTime * 10f), base.transform.localPosition.z);
		if (!FadeOut)
		{
			AudioSource component = GetComponent<AudioSource>();
			if (MyLabel.color.a >= 1f)
			{
				if (InputManager.TappedDown)
				{
					Selected++;
					if (Selected > Options)
					{
						Selected = 1;
					}
					component.clip = MoveSound;
					component.Play();
				}
				if (InputManager.TappedUp)
				{
					Selected--;
					if (Selected < 1)
					{
						Selected = Options;
					}
					component.clip = MoveSound;
					component.Play();
				}
				Continue.color = new Color(Continue.color.r, Continue.color.g, Continue.color.b, (Selected == 4) ? 0f : 1f);
				if (Input.GetButtonDown("A"))
				{
					component.clip = SelectSound;
					component.Play();
					Nudge = true;
					if (Selected != 4)
					{
						FadeOut = true;
					}
				}
			}
		}
		else
		{
			Heartbroken.GetComponent<AudioSource>().volume -= Time.deltaTime;
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a + Time.deltaTime);
			if (Darkness.color.a >= 1f)
			{
				if (Selected == 1)
				{
					for (int i = 0; i < StudentManager.NPCsTotal; i++)
					{
						if (StudentGlobals.GetStudentDying(i))
						{
							StudentGlobals.SetStudentDying(i, false);
						}
					}
					SceneManager.LoadScene("LoadingScene");
				}
				else if (Selected == 2)
				{
					LoveSick = GameGlobals.LoveSick;
					Globals.DeleteAll();
					GameGlobals.LoveSick = LoveSick;
					SceneManager.LoadScene("CalendarScene");
				}
				else if (Selected == 3)
				{
					SceneManager.LoadScene("TitleScene");
				}
			}
		}
		if (Nudge)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x + Time.deltaTime * 250f, base.transform.localPosition.y, base.transform.localPosition.z);
			if (base.transform.localPosition.x > -225f)
			{
				base.transform.localPosition = new Vector3(-225f, base.transform.localPosition.y, base.transform.localPosition.z);
				Nudge = false;
			}
		}
		else
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x - Time.deltaTime * 250f, base.transform.localPosition.y, base.transform.localPosition.z);
			if (base.transform.localPosition.x < -250f)
			{
				base.transform.localPosition = new Vector3(-250f, base.transform.localPosition.y, base.transform.localPosition.z);
			}
		}
	}
}
