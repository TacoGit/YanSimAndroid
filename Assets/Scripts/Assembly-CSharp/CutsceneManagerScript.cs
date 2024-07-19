using UnityEngine;

public class CutsceneManagerScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public CounselorScript Counselor;

	public PromptBarScript PromptBar;

	public EndOfDayScript EndOfDay;

	public PortalScript Portal;

	public UISprite Darkness;

	public UILabel Subtitle;

	public AudioClip[] Voice;

	public string[] Text;

	public int Phase = 1;

	public int Line = 1;

	private void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (Phase == 1)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 1f, Time.deltaTime));
			if (Darkness.color.a == 1f)
			{
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			Subtitle.text = Text[Line];
			component.clip = Voice[Line];
			component.Play();
			Phase++;
		}
		else if (Phase == 3)
		{
			if (!component.isPlaying || Input.GetButtonDown("A"))
			{
				if (Line < 2)
				{
					Phase--;
					Line++;
				}
				else
				{
					Subtitle.text = string.Empty;
					Phase++;
				}
			}
		}
		else if (Phase == 4)
		{
			EndOfDay.gameObject.SetActive(true);
			EndOfDay.Phase = 12;
			Counselor.LecturePhase = 5;
			Phase++;
		}
		else if (Phase == 6)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.MoveTowards(Darkness.color.a, 0f, Time.deltaTime));
			if (Darkness.color.a == 0f)
			{
				Phase++;
			}
		}
		else if (Phase == 7)
		{
			if (StudentManager.Students[7] != null)
			{
				Object.Destroy(StudentManager.Students[7].gameObject);
			}
			PromptBar.ClearButtons();
			PromptBar.Show = false;
			Portal.Proceed = true;
			base.gameObject.SetActive(false);
		}
	}
}
