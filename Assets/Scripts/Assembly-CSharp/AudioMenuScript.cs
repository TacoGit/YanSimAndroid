using UnityEngine;

public class AudioMenuScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public PromptBarScript PromptBar;

	public JukeboxScript Jukebox;

	public UILabel CurrentTrackLabel;

	public UILabel MusicVolumeLabel;

	public UILabel MusicOnOffLabel;

	public int SelectionLimit = 5;

	public int Selected = 1;

	public Transform Highlight;

	public GameObject CustomMusicMenu;

	private void Start()
	{
		UpdateText();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			CustomMusicMenu.SetActive(true);
			base.gameObject.SetActive(false);
		}
		if (InputManager.TappedUp)
		{
			Selected--;
			UpdateHighlight();
		}
		else if (InputManager.TappedDown)
		{
			Selected++;
			UpdateHighlight();
		}
		if (Selected == 1)
		{
			if (InputManager.TappedRight)
			{
				if (Jukebox.Volume < 1f)
				{
					Jukebox.Volume += 0.05f;
				}
				UpdateText();
			}
			else if (InputManager.TappedLeft)
			{
				if (Jukebox.Volume > 0f)
				{
					Jukebox.Volume -= 0.05f;
				}
				UpdateText();
			}
		}
		else if (Selected == 2 && (InputManager.TappedRight || InputManager.TappedLeft))
		{
			Jukebox.StartStopMusic();
			UpdateText();
		}
		if (Input.GetButtonDown("B"))
		{
			PromptBar.ClearButtons();
			PromptBar.Label[0].text = "Accept";
			PromptBar.Label[1].text = "Exit";
			PromptBar.Label[4].text = "Choose";
			PromptBar.UpdateButtons();
			PauseScreen.ScreenBlur.enabled = true;
			PauseScreen.MainMenu.SetActive(true);
			PauseScreen.Sideways = false;
			PauseScreen.PressedB = true;
			base.gameObject.SetActive(false);
		}
	}

	public void UpdateText()
	{
		if (Jukebox != null)
		{
			CurrentTrackLabel.text = "Current Track: " + Jukebox.BGM;
			MusicVolumeLabel.text = string.Empty + (Jukebox.Volume * 10f).ToString("F1");
			if (Jukebox.Volume == 0f)
			{
				MusicOnOffLabel.text = "Off";
			}
			else
			{
				MusicOnOffLabel.text = "On";
			}
		}
	}

	private void UpdateHighlight()
	{
		if (Selected == 0)
		{
			Selected = SelectionLimit;
		}
		else if (Selected > SelectionLimit)
		{
			Selected = 1;
		}
		Highlight.localPosition = new Vector3(Highlight.localPosition.x, 440f - 60f * (float)Selected, Highlight.localPosition.z);
	}
}
