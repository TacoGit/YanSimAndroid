using System;

[Serializable]
public class DateSaveData
{
	public int week;

	public DayOfWeek weekday;

	public static DateSaveData ReadFromGlobals()
	{
		DateSaveData dateSaveData = new DateSaveData();
		dateSaveData.week = DateGlobals.Week;
		dateSaveData.weekday = DateGlobals.Weekday;
		return dateSaveData;
	}

	public static void WriteToGlobals(DateSaveData data)
	{
		DateGlobals.Week = data.week;
		DateGlobals.Weekday = data.weekday;
	}
}
