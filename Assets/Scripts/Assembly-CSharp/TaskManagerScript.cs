using UnityEngine;

public class TaskManagerScript : MonoBehaviour
{
	public StudentManagerScript StudentManager;

	public YandereScript Yandere;

	public GameObject[] TaskObjects;

	public PromptScript[] Prompts;

	public bool[] GirlsQuestioned;

	private void Start()
	{
		UpdateTaskStatus();
	}

	public void CheckTaskPickups()
	{
		Debug.Log("Checking Tasks that are completed by picking something up!");
		if (TaskGlobals.GetTaskStatus(11) == 1 && Prompts[11].Circle[3] != null && Prompts[11].Circle[3].fillAmount == 0f)
		{
			if (StudentManager.Students[11] != null)
			{
				StudentManager.Students[11].TaskPhase = 5;
			}
			TaskGlobals.SetTaskStatus(11, 2);
			Object.Destroy(TaskObjects[11]);
		}
		if (TaskGlobals.GetTaskStatus(25) == 1 && Prompts[25].Circle[3].fillAmount == 0f)
		{
			if (StudentManager.Students[25] != null)
			{
				StudentManager.Students[25].TaskPhase = 5;
			}
			TaskGlobals.SetTaskStatus(25, 2);
			Object.Destroy(TaskObjects[25]);
		}
		if (TaskGlobals.GetTaskStatus(37) == 1 && Prompts[37].Circle[3] != null && Prompts[37].Circle[3].fillAmount == 0f)
		{
			if (StudentManager.Students[37] != null)
			{
				StudentManager.Students[37].TaskPhase = 5;
			}
			TaskGlobals.SetTaskStatus(37, 2);
			Object.Destroy(TaskObjects[37]);
		}
		if (Yandere.Talking)
		{
			return;
		}
		Debug.Log("Checking Musume's Task.");
		if (TaskGlobals.GetTaskStatus(81) == 1)
		{
			if (Yandere.Inventory.Cigs)
			{
				if (StudentManager.Students[81] != null)
				{
					StudentManager.Students[81].TaskPhase = 5;
				}
				TaskGlobals.SetTaskStatus(81, 2);
			}
		}
		else if (TaskGlobals.GetTaskStatus(81) == 2 && !Yandere.Inventory.Cigs)
		{
			if (StudentManager.Students[81] != null)
			{
				StudentManager.Students[81].TaskPhase = 4;
			}
			TaskGlobals.SetTaskStatus(81, 1);
		}
	}

	public void UpdateTaskStatus()
	{
		if (TaskGlobals.GetTaskStatus(8) == 1 && StudentManager.Students[8] != null)
		{
			if (StudentManager.Students[8].TaskPhase == 0)
			{
				StudentManager.Students[8].TaskPhase = 4;
			}
			if (Yandere.Inventory.Soda)
			{
				StudentManager.Students[8].TaskPhase = 5;
			}
		}
		if (TaskGlobals.GetTaskStatus(25) == 1)
		{
			if (StudentManager.Students[25] != null)
			{
				if (StudentManager.Students[25].TaskPhase == 0)
				{
					StudentManager.Students[25].TaskPhase = 4;
				}
				TaskObjects[25].SetActive(true);
			}
		}
		else if (TaskObjects[25] != null)
		{
			TaskObjects[25].SetActive(false);
		}
		if (TaskGlobals.GetTaskStatus(28) == 1 && StudentManager.Students[28] != null)
		{
			if (StudentManager.Students[28].TaskPhase == 0)
			{
				StudentManager.Students[28].TaskPhase = 4;
			}
			for (int i = 1; i < 26; i++)
			{
				if (TaskGlobals.GetKittenPhoto(i))
				{
					StudentManager.Students[28].TaskPhase = 5;
				}
			}
		}
		if (TaskGlobals.GetTaskStatus(30) == 1 && StudentManager.Students[30] != null && StudentManager.Students[30].TaskPhase == 0)
		{
			StudentManager.Students[30].TaskPhase = 4;
		}
		if (TaskGlobals.GetTaskStatus(36) == 1 && StudentManager.Students[36] != null)
		{
			if (StudentManager.Students[36].TaskPhase == 0)
			{
				StudentManager.Students[36].TaskPhase = 4;
			}
			if (GirlsQuestioned[1] && GirlsQuestioned[2] && GirlsQuestioned[3] && GirlsQuestioned[4] && GirlsQuestioned[5])
			{
				Debug.Log("Gema's task should be ready to turn in!");
				StudentManager.Students[36].TaskPhase = 5;
			}
		}
		if (TaskGlobals.GetTaskStatus(37) == 1)
		{
			if (StudentManager.Students[37] != null)
			{
				if (StudentManager.Students[37].TaskPhase == 0)
				{
					StudentManager.Students[37].TaskPhase = 4;
				}
				TaskObjects[37].SetActive(true);
			}
		}
		else if (TaskObjects[37] != null)
		{
			TaskObjects[37].SetActive(false);
		}
		if (TaskGlobals.GetTaskStatus(38) == 1)
		{
			if (StudentManager.Students[38] != null && StudentManager.Students[38].TaskPhase == 0)
			{
				StudentManager.Students[38].TaskPhase = 4;
			}
		}
		else if (TaskGlobals.GetTaskStatus(38) == 2 && StudentManager.Students[38] != null)
		{
			StudentManager.Students[38].TaskPhase = 5;
		}
		if (ClubGlobals.GetClubClosed(ClubType.LightMusic) || StudentManager.Students[51] == null)
		{
			if (StudentManager.Students[52] != null)
			{
				StudentManager.Students[52].TaskPhase = 100;
			}
			TaskGlobals.SetTaskStatus(52, 100);
		}
		else if (TaskGlobals.GetTaskStatus(52) == 1 && StudentManager.Students[52] != null)
		{
			StudentManager.Students[52].TaskPhase = 4;
			for (int j = 1; j < 52; j++)
			{
				if (TaskGlobals.GetGuitarPhoto(j))
				{
					StudentManager.Students[52].TaskPhase = 5;
				}
			}
		}
		if (TaskGlobals.GetTaskStatus(81) == 3)
		{
		}
		if (TaskGlobals.GetTaskStatus(11) == 1)
		{
			if (StudentManager.Students[11] != null)
			{
				if (StudentManager.Students[11].TaskPhase == 0)
				{
					StudentManager.Students[11].TaskPhase = 4;
				}
				TaskObjects[11].SetActive(true);
			}
		}
		else if (TaskObjects[11] != null)
		{
			TaskObjects[11].SetActive(false);
		}
	}
}
