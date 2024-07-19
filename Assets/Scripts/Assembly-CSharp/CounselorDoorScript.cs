using UnityEngine;

public class CounselorDoorScript : MonoBehaviour
{
	public CounselorScript Counselor;

	public PromptScript Prompt;

	public UISprite Darkness;

	public bool FadeOut;

	public bool FadeIn;

	public bool Exit;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (!Prompt.Yandere.Chased && Prompt.Yandere.Chasers == 0 && !FadeIn && Prompt.Yandere.Bloodiness == 0f && Prompt.Yandere.Sanity > 66.66666f && !Prompt.Yandere.Carrying && !Prompt.Yandere.Dragging)
			{
				if (!Counselor.Busy)
				{
					Prompt.Yandere.CharacterAnimation.CrossFade(Prompt.Yandere.IdleAnim);
					Prompt.Yandere.Police.Darkness.enabled = true;
					Prompt.Yandere.CanMove = false;
					FadeOut = true;
				}
				else
				{
					Counselor.CounselorSubtitle.text = Counselor.CounselorBusyText;
					Counselor.MyAudio.clip = Counselor.CounselorBusyClip;
					Counselor.MyAudio.Play();
				}
			}
		}
		if (FadeOut)
		{
			float a = Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime);
			Darkness.color = new Color(0f, 0f, 0f, a);
			if (Darkness.color.a == 1f)
			{
				if (!Exit)
				{
					Prompt.Yandere.CharacterAnimation.Play("f02_sit_00");
					Prompt.Yandere.transform.position = new Vector3(-27.51f, 0f, 12f);
					Prompt.Yandere.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
					Counselor.Talk();
					FadeOut = false;
					FadeIn = true;
				}
				else
				{
					Darkness.color = new Color(0f, 0f, 0f, 2f);
					Counselor.Quit();
					FadeOut = false;
					FadeIn = true;
					Exit = false;
				}
			}
		}
		if (FadeIn)
		{
			float a2 = Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime);
			Darkness.color = new Color(0f, 0f, 0f, a2);
			if (Darkness.color.a == 0f)
			{
				FadeIn = false;
			}
		}
	}
}
