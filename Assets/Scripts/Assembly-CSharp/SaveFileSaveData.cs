using System;

[Serializable]
public class SaveFileSaveData
{
	public int currentSaveFile;

	public static SaveFileSaveData ReadFromGlobals()
	{
		SaveFileSaveData saveFileSaveData = new SaveFileSaveData();
		saveFileSaveData.currentSaveFile = SaveFileGlobals.CurrentSaveFile;
		return saveFileSaveData;
	}

	public static void WriteToGlobals(SaveFileSaveData data)
	{
		SaveFileGlobals.CurrentSaveFile = data.currentSaveFile;
	}
}
