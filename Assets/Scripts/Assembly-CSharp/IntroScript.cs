using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
	public UISprite FadeOutDarkness;

	public UITexture LoveSickLogo;

	public UIPanel SkipPanel;

	public UISprite Darkness;

	public UISprite Circle;

	public UITexture Logo;

	public UILabel Label;

	public AudioSource Narration;

	public string[] Lines;

	public float[] Cue;

	public bool Narrating;

	public bool Musicing;

	public bool FadeOut;

	public float SkipTimer;

	public float Timer;

	public int ID;

	private void Start()
	{
		LoveSickCheck();
		Circle.fillAmount = 0f;
		Darkness.color = new Color(0f, 0f, 0f, 1f);
		Label.text = string.Empty;
		SkipTimer = 15f;
	}

	private void Update()
	{
		if (Input.GetButton("X"))
		{
			Circle.fillAmount = Mathf.MoveTowards(Circle.fillAmount, 1f, Time.deltaTime);
			SkipTimer = 15f;
			if (Circle.fillAmount == 1f)
			{
				FadeOut = true;
			}
			SkipPanel.alpha = 1f;
		}
		else
		{
			Circle.fillAmount = Mathf.MoveTowards(Circle.fillAmount, 0f, Time.deltaTime);
			SkipTimer -= Time.deltaTime;
			SkipPanel.alpha = SkipTimer / 10f;
		}
		Timer += Time.deltaTime;
		if (Timer > 1f && !Narrating)
		{
			Narration.Play();
			Narrating = true;
		}
		if (ID < Cue.Length && Narration.time > Cue[ID])
		{
			Label.text = Lines[ID];
			ID++;
		}
		if (FadeOut)
		{
			FadeOutDarkness.color = new Color(FadeOutDarkness.color.r, FadeOutDarkness.color.g, FadeOutDarkness.color.b, Mathf.MoveTowards(FadeOutDarkness.color.a, 1f, Time.deltaTime));
			Circle.fillAmount = 1f;
			Narration.volume = FadeOutDarkness.color.a;
			if (FadeOutDarkness.color.a == 1f)
			{
				SceneManager.LoadScene("PhoneScene");
			}
		}
		if (Narration.time > 39.75f && Narration.time < 73f)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime * 0.5f));
		}
		if (Narration.time > 73f && Narration.time < 105.5f)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
		}
		if (Narration.time > 105.5f && Narration.time < 134f)
		{
			Darkness.color = new Color(1f, 0f, 0f, 1f);
		}
		if (Narration.time > 134f && Narration.time < 147f)
		{
			Darkness.color = new Color(0f, 0f, 0f, 1f);
		}
		if (Narration.time > 147f && Narration.time < 152f)
		{
			Logo.transform.localScale = new Vector3(Logo.transform.localScale.x + Time.deltaTime * 0.1f, Logo.transform.localScale.y + Time.deltaTime * 0.1f, Logo.transform.localScale.z + Time.deltaTime * 0.1f);
			LoveSickLogo.transform.localScale = new Vector3(LoveSickLogo.transform.localScale.x + Time.deltaTime * 0.05f, LoveSickLogo.transform.localScale.y + Time.deltaTime * 0.05f, LoveSickLogo.transform.localScale.z + Time.deltaTime * 0.05f);
			Logo.color = new Color(Logo.color.r, Logo.color.g, Logo.color.b, 1f);
			LoveSickLogo.color = new Color(LoveSickLogo.color.r, LoveSickLogo.color.g, LoveSickLogo.color.b, 1f);
		}
		if (Narration.time > 152f)
		{
			Logo.color = new Color(Logo.color.r, Logo.color.g, Logo.color.b, 0f);
			LoveSickLogo.color = new Color(LoveSickLogo.color.r, LoveSickLogo.color.g, LoveSickLogo.color.b, 0f);
		}
		if (Narration.time > 156f)
		{
			SceneManager.LoadScene("PhoneScene");
		}
	}

	private void LoveSickCheck()
	{
		if (GameGlobals.LoveSick)
		{
			Camera.main.backgroundColor = new Color(0f, 0f, 0f, 1f);
			GameObject[] array = Object.FindObjectsOfType<GameObject>();
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				UISprite component = gameObject.GetComponent<UISprite>();
				if (component != null)
				{
					component.color = new Color(1f, 0f, 0f, component.color.a);
				}
				UITexture component2 = gameObject.GetComponent<UITexture>();
				if (component2 != null)
				{
					component2.color = new Color(1f, 0f, 0f, component2.color.a);
				}
				UILabel component3 = gameObject.GetComponent<UILabel>();
				if (component3 != null && component3.color != Color.black)
				{
					component3.color = new Color(1f, 0f, 0f, component3.color.a);
				}
			}
			FadeOutDarkness.color = new Color(0f, 0f, 0f, 0f);
			LoveSickLogo.enabled = true;
			Logo.enabled = false;
		}
		else
		{
			LoveSickLogo.enabled = false;
		}
	}
}
