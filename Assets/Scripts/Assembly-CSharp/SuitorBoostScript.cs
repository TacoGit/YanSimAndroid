using UnityEngine;

public class SuitorBoostScript : MonoBehaviour
{
	public PromptBarScript PromptBar;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public UISprite Darkness;

	public Transform YandereSitSpot;

	public Transform SuitorSitSpot;

	public Transform YandereChair;

	public Transform SuitorChair;

	public Transform YandereSpot;

	public Transform SuitorSpot;

	public Transform LookTarget;

	public Transform TextBox;

	public bool Boosting;

	public bool FadeOut;

	public float Timer;

	public int Phase = 1;

	private void Update()
	{
		if (Yandere.Followers > 0)
		{
			if (Yandere.Follower.StudentID == 28 && Yandere.Follower.DistanceToPlayer < 2f)
			{
				Prompt.enabled = true;
			}
			else if (Prompt.enabled)
			{
				Prompt.Hide();
				Prompt.enabled = false;
			}
		}
		else if (Prompt.enabled)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (!Yandere.Chased && Yandere.Chasers == 0)
			{
				Yandere.Character.GetComponent<Animation>().CrossFade(Yandere.IdleAnim);
				Yandere.Follower.CharacterAnimation.CrossFade(Yandere.Follower.IdleAnim);
				Yandere.Follower.Pathfinding.canSearch = false;
				Yandere.Follower.Pathfinding.canMove = false;
				Yandere.Follower.enabled = false;
				Yandere.RPGCamera.enabled = false;
				Darkness.enabled = true;
				Yandere.CanMove = false;
				Boosting = true;
				FadeOut = true;
			}
		}
		if (!Boosting)
		{
			return;
		}
		if (FadeOut)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			if (Darkness.color.a != 1f)
			{
				return;
			}
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				if (Phase == 1)
				{
					Camera.main.transform.position = new Vector3(-26f, 5.3f, 17.5f);
					Camera.main.transform.eulerAngles = new Vector3(15f, 180f, 0f);
					Yandere.Follower.Character.transform.localScale = new Vector3(1f, 1f, 1f);
					YandereChair.transform.localPosition = new Vector3(YandereChair.transform.localPosition.x, YandereChair.transform.localPosition.y, -0.6f);
					SuitorChair.transform.localPosition = new Vector3(SuitorChair.transform.localPosition.x, SuitorChair.transform.localPosition.y, -0.6f);
					Yandere.Character.GetComponent<Animation>().Play("f02_sit_01");
					Yandere.Follower.Character.GetComponent<Animation>().Play("sit_01");
					Yandere.transform.eulerAngles = Vector3.zero;
					Yandere.Follower.transform.eulerAngles = Vector3.zero;
					Yandere.transform.position = YandereSitSpot.position;
					Yandere.Follower.transform.position = SuitorSitSpot.position;
				}
				else
				{
					Yandere.FixCamera();
					Yandere.Follower.Character.transform.localScale = new Vector3(0.94f, 0.94f, 0.94f);
					YandereChair.transform.localPosition = new Vector3(YandereChair.transform.localPosition.x, YandereChair.transform.localPosition.y, -1f / 3f);
					SuitorChair.transform.localPosition = new Vector3(SuitorChair.transform.localPosition.x, SuitorChair.transform.localPosition.y, -1f / 3f);
					Yandere.Character.GetComponent<Animation>().Play(Yandere.IdleAnim);
					Yandere.Follower.Character.GetComponent<Animation>().Play(Yandere.Follower.IdleAnim);
					Yandere.transform.position = YandereSpot.position;
					Yandere.Follower.transform.position = SuitorSpot.position;
				}
				PromptBar.ClearButtons();
				FadeOut = false;
				Phase++;
			}
			return;
		}
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
		if (Darkness.color.a != 0f)
		{
			return;
		}
		if (Phase == 2)
		{
			TextBox.gameObject.SetActive(true);
			TextBox.localScale = Vector3.Lerp(TextBox.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			if (TextBox.localScale.x > 0.9f)
			{
				if (!PromptBar.Show)
				{
					PromptBar.ClearButtons();
					PromptBar.Label[0].text = "Continue";
					PromptBar.UpdateButtons();
					PromptBar.Show = true;
				}
				if (Input.GetButtonDown("A"))
				{
					PromptBar.Show = false;
					Phase++;
				}
			}
		}
		else if (Phase == 3)
		{
			if (TextBox.localScale.x > 0.1f)
			{
				TextBox.localScale = Vector3.Lerp(TextBox.localScale, Vector3.zero, Time.deltaTime * 10f);
				return;
			}
			TextBox.gameObject.SetActive(false);
			FadeOut = true;
			Phase++;
		}
		else if (Phase == 5)
		{
			DatingGlobals.SetSuitorTrait(2, DatingGlobals.GetSuitorTrait(2) + 1);
			Yandere.RPGCamera.enabled = true;
			Darkness.enabled = false;
			Yandere.CanMove = true;
			Boosting = false;
			Yandere.Follower.Pathfinding.canSearch = true;
			Yandere.Follower.Pathfinding.canMove = true;
			Yandere.Follower.enabled = true;
			Prompt.Hide();
			Prompt.enabled = false;
			base.enabled = false;
		}
	}

	private void LateUpdate()
	{
		if (Boosting && Phase > 1 && Phase < 5)
		{
			Yandere.Head.LookAt(LookTarget);
			Yandere.Follower.Head.LookAt(LookTarget);
		}
	}
}
