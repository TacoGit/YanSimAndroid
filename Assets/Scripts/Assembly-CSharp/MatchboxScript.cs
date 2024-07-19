using UnityEngine;

public class MatchboxScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public PickUpScript PickUp;

	public GameObject Match;

	private void Start()
	{
		Yandere = GameObject.Find("YandereChan").GetComponent<YandereScript>();
	}

	private void Update()
	{
		if (Prompt.PauseScreen.Show)
		{
			return;
		}
		if (Yandere.PickUp == PickUp)
		{
			if (Prompt.HideButton[0])
			{
				Yandere.Arc.SetActive(true);
				Prompt.HideButton[0] = false;
				Prompt.HideButton[3] = true;
			}
			if (Prompt.Circle[0].fillAmount == 0f)
			{
				Prompt.Circle[0].fillAmount = 1f;
				if (!Yandere.Chased && Yandere.Chasers == 0 && Yandere.CanMove && !Yandere.Flicking)
				{
					GameObject gameObject = Object.Instantiate(Match, base.transform.position, Quaternion.identity);
					gameObject.transform.parent = Yandere.ItemParent;
					gameObject.transform.localPosition = new Vector3(0.0159f, 0.0043f, 0.0152f);
					gameObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					Yandere.Match = gameObject;
					Yandere.Character.GetComponent<Animation>().CrossFade("f02_flickingMatch_00");
					Yandere.YandereVision = false;
					Yandere.Arc.SetActive(false);
					Yandere.Flicking = true;
					Yandere.CanMove = false;
					Prompt.Hide();
					Prompt.enabled = false;
				}
			}
		}
		else if (!Prompt.HideButton[0])
		{
			Yandere.Arc.SetActive(false);
			Prompt.HideButton[0] = true;
			Prompt.HideButton[3] = false;
		}
	}
}
