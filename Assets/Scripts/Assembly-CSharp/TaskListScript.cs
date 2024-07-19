using System.Collections;
using UnityEngine;

public class TaskListScript : MonoBehaviour
{
	public InputManagerScript InputManager;

	public PauseScreenScript PauseScreen;

	public GameObject MainMenu;

	public UITexture StudentIcon;

	public UITexture TaskIcon;

	public UILabel TaskDesc;

	public Texture QuestionMark;

	public Transform Highlight;

	public Texture Silhouette;

	public UILabel[] TaskNameLabels;

	public UISprite[] Checkmarks;

	public Texture[] TaskIcons;

	public string[] TaskDescs;

	public string[] TaskNames;

	public int ID = 1;

	private void Update()
	{
		if (InputManager.TappedUp)
		{
			ID--;
			if (ID < 1)
			{
				ID = 16;
			}
			StartCoroutine(UpdateTaskInfo());
		}
		if (InputManager.TappedDown)
		{
			ID++;
			if (ID > 16)
			{
				ID = 1;
			}
			StartCoroutine(UpdateTaskInfo());
		}
		if (Input.GetButtonDown("B"))
		{
			PauseScreen.PromptBar.ClearButtons();
			PauseScreen.PromptBar.Label[0].text = "Accept";
			PauseScreen.PromptBar.Label[1].text = "Back";
			PauseScreen.PromptBar.Label[4].text = "Choose";
			PauseScreen.PromptBar.Label[5].text = "Choose";
			PauseScreen.PromptBar.UpdateButtons();
			PauseScreen.Sideways = false;
			PauseScreen.PressedB = true;
			MainMenu.SetActive(true);
			base.gameObject.SetActive(false);
		}
	}

	public void UpdateTaskList()
	{
		for (int i = 1; i < TaskNames.Length; i++)
		{
			TaskNameLabels[i].text = ((TaskGlobals.GetTaskStatus(i) != 0) ? TaskNames[i] : "?????");
			Checkmarks[i].enabled = TaskGlobals.GetTaskStatus(i) == 3;
		}
	}

	public IEnumerator UpdateTaskInfo()
	{
		Highlight.localPosition = new Vector3(Highlight.localPosition.x, 200f - 25f * (float)ID, Highlight.localPosition.z);
		if (TaskGlobals.GetTaskStatus(ID) == 0)
		{
			StudentIcon.mainTexture = Silhouette;
			TaskIcon.mainTexture = QuestionMark;
			TaskDesc.text = "This task has not been discovered yet.";
			yield break;
		}
		string path = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + ID + ".png";
		WWW www = new WWW(path);
		yield return www;
		StudentIcon.mainTexture = www.texture;
		TaskIcon.mainTexture = TaskIcons[ID];
		TaskDesc.text = TaskDescs[ID];
	}
}
