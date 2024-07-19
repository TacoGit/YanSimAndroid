using System;
using System.Collections.Generic;

[Serializable]
public class TaskSaveData
{
	public IntHashSet guitarPhoto = new IntHashSet();

	public IntHashSet kittenPhoto = new IntHashSet();

	public IntAndIntDictionary taskStatus = new IntAndIntDictionary();

	public static TaskSaveData ReadFromGlobals()
	{
		TaskSaveData taskSaveData = new TaskSaveData();
		int[] array = TaskGlobals.KeysOfGuitarPhoto();
		foreach (int num in array)
		{
			if (TaskGlobals.GetGuitarPhoto(num))
			{
				taskSaveData.guitarPhoto.Add(num);
			}
		}
		int[] array2 = TaskGlobals.KeysOfKittenPhoto();
		foreach (int num2 in array2)
		{
			if (TaskGlobals.GetKittenPhoto(num2))
			{
				taskSaveData.kittenPhoto.Add(num2);
			}
		}
		int[] array3 = TaskGlobals.KeysOfTaskStatus();
		foreach (int num3 in array3)
		{
			taskSaveData.taskStatus.Add(num3, TaskGlobals.GetTaskStatus(num3));
		}
		return taskSaveData;
	}

	public static void WriteToGlobals(TaskSaveData data)
	{
		foreach (int item in data.kittenPhoto)
		{
			TaskGlobals.SetKittenPhoto(item, true);
		}
		foreach (int item2 in data.guitarPhoto)
		{
			TaskGlobals.SetGuitarPhoto(item2, true);
		}
		foreach (KeyValuePair<int, int> item3 in data.taskStatus)
		{
			TaskGlobals.SetTaskStatus(item3.Key, item3.Value);
		}
	}
}
