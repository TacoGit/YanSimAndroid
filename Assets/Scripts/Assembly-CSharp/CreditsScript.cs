using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
	[SerializeField]
	private JsonScript JSON;

	[SerializeField]
	private Transform SpawnPoint;

	[SerializeField]
	private Transform Panel;

	[SerializeField]
	private GameObject SmallCreditsLabel;

	[SerializeField]
	private GameObject BigCreditsLabel;

	[SerializeField]
	private UISprite Darkness;

	[SerializeField]
	private int ID;

	[SerializeField]
	private float SpeedUpFactor;

	[SerializeField]
	private float TimerLimit;

	[SerializeField]
	private float FadeTimer;

	[SerializeField]
	private float Speed = 1f;

	[SerializeField]
	private float Timer;

	[SerializeField]
	private bool FadeOut;

	[SerializeField]
	private bool Begin;

	private const int SmallTextSize = 1;

	private const int BigTextSize = 2;

	private bool ShouldStopCredits
	{
		get
		{
			return ID == JSON.Credits.Length;
		}
	}

	private GameObject SpawnLabel(int size)
	{
		return Object.Instantiate((size != 1) ? BigCreditsLabel : SmallCreditsLabel, SpawnPoint.position, Quaternion.identity);
	}

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (!Begin)
		{
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				Begin = true;
				component.Play();
				Timer = 0f;
			}
		}
		else
		{
			if (!ShouldStopCredits)
			{
				if (Timer == 0f)
				{
					CreditJson creditJson = JSON.Credits[ID];
					GameObject gameObject = SpawnLabel(creditJson.Size);
					TimerLimit = (float)creditJson.Size * SpeedUpFactor;
					gameObject.transform.parent = Panel;
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					gameObject.GetComponent<UILabel>().text = creditJson.Name;
					ID++;
				}
				Timer += Time.deltaTime * Speed;
				if (Timer >= TimerLimit)
				{
					Timer = 0f;
				}
			}
			if (Input.GetButtonDown("B") || !component.isPlaying)
			{
				FadeOut = true;
			}
		}
		if (FadeOut)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			component.volume -= Time.deltaTime;
			if (Darkness.color.a == 1f)
			{
				SceneManager.LoadScene("TitleScene");
			}
		}
		bool keyDown = Input.GetKeyDown(KeyCode.Minus);
		bool keyDown2 = Input.GetKeyDown(KeyCode.Equals);
		if (keyDown)
		{
			Time.timeScale -= 1f;
		}
		else if (keyDown2)
		{
			Time.timeScale += 1f;
		}
		if (keyDown || keyDown2)
		{
			component.pitch = Time.timeScale;
		}
	}
}
