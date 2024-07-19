using UnityEngine;

public class TaskWindowScript : MonoBehaviour
{
	public DialogueWheelScript DialogueWheel;

	public SewingMachineScript SewingMachine;

	public TaskManagerScript TaskManager;

	public PromptBarScript PromptBar;

	public UILabel TaskDescLabel;

	public YandereScript Yandere;

	public UITexture Portrait;

	public UITexture Icon;

	public GameObject[] TaskCompleteLetters;

	public string[] Descriptions;

	public Texture[] Portraits;

	public Texture[] Icons;

	public bool TaskComplete;

	public GameObject Window;

	public int StudentID;

	public int ID;

	public float TrueTimer;

	public float Timer;

	private void Start()
	{
		Window.SetActive(false);
		UpdateTaskObjects(30);
	}

	public void UpdateWindow(int ID)
	{
		PromptBar.ClearButtons();
		PromptBar.Label[0].text = "Accept";
		PromptBar.Label[1].text = "Refuse";
		PromptBar.UpdateButtons();
		PromptBar.Show = true;
		TaskDescLabel.transform.parent.gameObject.SetActive(true);
		TaskDescLabel.text = Descriptions[ID];
		Icon.mainTexture = Icons[ID];
		Window.SetActive(true);
		GetPortrait(ID);
		StudentID = ID;
	}

	private void Update()
	{
		if (Window.activeInHierarchy)
		{
			if (Input.GetButtonDown("A"))
			{
				TaskGlobals.SetTaskStatus(StudentID, 1);
				Yandere.TargetStudent.TalkTimer = 100f;
				Yandere.TargetStudent.Interaction = StudentInteractionType.GivingTask;
				Yandere.TargetStudent.TaskPhase = 4;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
				Window.SetActive(false);
				UpdateTaskObjects(StudentID);
			}
			else if (Input.GetButtonDown("B"))
			{
				Yandere.TargetStudent.TalkTimer = 100f;
				Yandere.TargetStudent.Interaction = StudentInteractionType.GivingTask;
				Yandere.TargetStudent.TaskPhase = 0;
				PromptBar.ClearButtons();
				PromptBar.Show = false;
				Window.SetActive(false);
			}
		}
		if (!TaskComplete)
		{
			return;
		}
		if (TrueTimer == 0f)
		{
			GetComponent<AudioSource>().Play();
		}
		TrueTimer += Time.deltaTime;
		Timer += Time.deltaTime;
		if (ID < TaskCompleteLetters.Length && Timer > 0.05f)
		{
			TaskCompleteLetters[ID].SetActive(true);
			Timer = 0f;
			ID++;
		}
		if (TaskCompleteLetters[12].transform.localPosition.y < -725f)
		{
			for (ID = 0; ID < TaskCompleteLetters.Length; ID++)
			{
				TaskCompleteLetters[ID].GetComponent<GrowShrinkScript>().Return();
			}
			TaskCheck();
			DialogueWheel.End();
			TaskComplete = false;
			TrueTimer = 0f;
			Timer = 0f;
			ID = 0;
		}
	}

	private void TaskCheck()
	{
		if (Yandere.TargetStudent.StudentID == 37)
		{
			DialogueWheel.Yandere.TargetStudent.Cosmetic.MaleAccessories[1].SetActive(true);
		}
	}

	private void GetPortrait(int ID)
	{
		string url = "file:///" + Application.streamingAssetsPath + "/Portraits/Student_" + ID + ".png";
		WWW wWW = new WWW(url);
		Portrait.mainTexture = wWW.texture;
	}

	private void UpdateTaskObjects(int StudentID)
	{
		if (this.StudentID == 30)
		{
			SewingMachine.Check = true;
		}
	}
}
