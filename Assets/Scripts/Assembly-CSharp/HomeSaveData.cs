using System;

[Serializable]
public class HomeSaveData
{
	public bool lateForSchool;

	public bool night;

	public bool startInBasement;

	public static HomeSaveData ReadFromGlobals()
	{
		HomeSaveData homeSaveData = new HomeSaveData();
		homeSaveData.lateForSchool = HomeGlobals.LateForSchool;
		homeSaveData.night = HomeGlobals.Night;
		homeSaveData.startInBasement = HomeGlobals.StartInBasement;
		return homeSaveData;
	}

	public static void WriteToGlobals(HomeSaveData data)
	{
		HomeGlobals.LateForSchool = data.lateForSchool;
		HomeGlobals.Night = data.night;
		HomeGlobals.StartInBasement = data.startInBasement;
	}
}
