using UnityEngine;

public class PeekScript : MonoBehaviour
{
	public InfoChanWindowScript InfoChanWindow;

	public PromptBarScript PromptBar;

	public SubtitleScript Subtitle;

	public JukeboxScript Jukebox;

	public PromptScript Prompt;

	public GameObject PeekCamera;

	public bool Spoke;

	public float Timer;

	private void Start()
	{
		Prompt.Door = true;
	}

	private void Update()
	{
		float num = Vector3.Distance(base.transform.position, Prompt.Yandere.transform.position);
		if (num < 2f)
		{
			Prompt.Yandere.StudentManager.TutorialWindow.ShowInfoMessage = true;
		}
		if (InfoChanWindow.Drop)
		{
			Prompt.Circle[0].fillAmount = 1f;
		}
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Prompt.Circle[0].fillAmount = 1f;
			if (!Prompt.Yandere.Chased && Prompt.Yandere.Chasers == 0)
			{
				Prompt.Yandere.CanMove = false;
				PeekCamera.SetActive(true);
				Jukebox.Dip = 0.5f;
				PromptBar.ClearButtons();
				PromptBar.Label[1].text = "Stop";
				PromptBar.UpdateButtons();
				PromptBar.Show = true;
			}
		}
		if (PeekCamera.activeInHierarchy)
		{
			Timer += Time.deltaTime;
			if (Timer > 5f && !Spoke)
			{
				Subtitle.UpdateLabel(SubtitleType.InfoNotice, 0, 6.5f);
				Spoke = true;
				GetComponent<AudioSource>().Play();
			}
			if (Input.GetButtonDown("B"))
			{
				Prompt.Yandere.CanMove = true;
				PeekCamera.SetActive(false);
				Jukebox.Dip = 1f;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
				Timer = 0f;
			}
		}
	}
}
