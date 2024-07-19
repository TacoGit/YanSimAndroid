using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScript : MonoBehaviour
{
	[SerializeField]
	private JsonScript JSON;

	[SerializeField]
	private GameObject WelcomePanel;

	[SerializeField]
	private UILabel[] FlashingLabels;

	[SerializeField]
	private UILabel AltBeginLabel;

	[SerializeField]
	private UILabel BeginLabel;

	[SerializeField]
	private UISprite Darkness;

	[SerializeField]
	private bool Continue;

	[SerializeField]
	private bool FlashRed;

	[SerializeField]
	private float VersionNumber;

	[SerializeField]
	private float Timer;

	private string Text;

	private int ID;

	private void Start()
	{
		Time.timeScale = 1f;
		BeginLabel.color = new Color(BeginLabel.color.r, BeginLabel.color.g, BeginLabel.color.b, 0f);
		AltBeginLabel.color = BeginLabel.color;
		Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 2f);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (ApplicationGlobals.VersionNumber != VersionNumber)
		{
			ApplicationGlobals.VersionNumber = VersionNumber;
		}
		if (File.Exists(Application.streamingAssetsPath + "/Fun.txt"))
		{
			Text = File.ReadAllText(Application.streamingAssetsPath + "/Fun.txt");
		}
		if (Text == "0" || Text == "1" || Text == "2" || Text == "3" || Text == "4" || Text == "5" || Text == "6" || Text == "7" || Text == "8" || Text == "9" || Text == "10" || Text == "69" || Text == "666")
		{
			SceneManager.LoadScene("VeryFunScene");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
		}
		if (!Continue)
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a - Time.deltaTime);
			if (Darkness.color.a <= 0f)
			{
				if (Input.GetKeyDown(KeyCode.W))
				{
				}
				if (Input.anyKeyDown)
				{
					Timer = 5f;
				}
				Timer += Time.deltaTime;
				if (Timer > 5f)
				{
					BeginLabel.color = new Color(BeginLabel.color.r, BeginLabel.color.g, BeginLabel.color.b, BeginLabel.color.a + Time.deltaTime);
					AltBeginLabel.color = BeginLabel.color;
					if (BeginLabel.color.a >= 1f && Input.anyKeyDown)
					{
						Darkness.color = new Color(1f, 1f, 1f, 0f);
						Continue = true;
					}
				}
			}
		}
		else
		{
			Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Darkness.color.a + Time.deltaTime);
			if (Darkness.color.a >= 1f)
			{
				SceneManager.LoadScene("SponsorScene");
			}
		}
		if (!FlashRed)
		{
			ID = 0;
			while (ID < 3)
			{
				ID++;
				FlashingLabels[ID].color = new Color(FlashingLabels[ID].color.r + Time.deltaTime * 10f, FlashingLabels[ID].color.g, FlashingLabels[ID].color.b, FlashingLabels[ID].color.a);
				if (FlashingLabels[ID].color.r > 1f)
				{
					FlashRed = true;
				}
			}
			return;
		}
		ID = 0;
		while (ID < 3)
		{
			ID++;
			FlashingLabels[ID].color = new Color(FlashingLabels[ID].color.r - Time.deltaTime * 10f, FlashingLabels[ID].color.g, FlashingLabels[ID].color.b, FlashingLabels[ID].color.a);
			if (FlashingLabels[ID].color.r < 0f)
			{
				FlashRed = false;
			}
		}
	}
}
