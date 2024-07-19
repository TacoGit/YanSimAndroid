using System.Collections.Generic;
using System.IO;
using JsonFx.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorManagerScript : MonoBehaviour
{
	[SerializeField]
	private UIPanel mainPanel;

	[SerializeField]
	private UIPanel[] editorPanels;

	[SerializeField]
	private UILabel cursorLabel;

	[SerializeField]
	private PromptBarScript promptBar;

	private int buttonIndex;

	private const int ButtonCount = 3;

	private InputManagerScript inputManager;

	private void Awake()
	{
		buttonIndex = 0;
		inputManager = Object.FindObjectOfType<InputManagerScript>();
	}

	private void Start()
	{
		promptBar.Label[0].text = "Select";
		promptBar.Label[1].text = "Exit";
		promptBar.Label[4].text = "Choose";
		promptBar.UpdateButtons();
	}

	private void OnEnable()
	{
		promptBar.Label[0].text = "Select";
		promptBar.Label[1].text = "Exit";
		promptBar.Label[4].text = "Choose";
		promptBar.UpdateButtons();
	}

	public static Dictionary<string, object>[] DeserializeJson(string filename)
	{
		string path = Path.Combine(Application.streamingAssetsPath, Path.Combine("JSON", filename));
		string value = File.ReadAllText(path);
		return JsonReader.Deserialize<Dictionary<string, object>[]>(value);
	}

	private void HandleInput()
	{
		if (Input.GetButtonDown("B"))
		{
			SceneManager.LoadScene("TitleScene");
		}
		bool tappedUp = inputManager.TappedUp;
		bool tappedDown = inputManager.TappedDown;
		if (tappedUp)
		{
			buttonIndex = ((buttonIndex <= 0) ? 2 : (buttonIndex - 1));
		}
		else if (tappedDown)
		{
			buttonIndex = ((buttonIndex < 2) ? (buttonIndex + 1) : 0);
		}
		if (tappedUp || tappedDown)
		{
			Transform transform = cursorLabel.transform;
			transform.localPosition = new Vector3(transform.localPosition.x, 100f - (float)buttonIndex * 100f, transform.localPosition.z);
		}
		if (Input.GetButtonDown("A"))
		{
			editorPanels[buttonIndex].gameObject.SetActive(true);
			mainPanel.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		HandleInput();
	}
}
