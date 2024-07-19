using System;

[Serializable]
public class ApplicationSaveData
{
	public float versionNumber;

	public static ApplicationSaveData ReadFromGlobals()
	{
		ApplicationSaveData applicationSaveData = new ApplicationSaveData();
		applicationSaveData.versionNumber = ApplicationGlobals.VersionNumber;
		return applicationSaveData;
	}

	public static void WriteToGlobals(ApplicationSaveData data)
	{
		ApplicationGlobals.VersionNumber = data.versionNumber;
	}
}
