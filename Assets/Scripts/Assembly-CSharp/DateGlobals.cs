using System;
using UnityEngine;

public static class DateGlobals
{
	private const string Str_Week = "Week";

	private const string Str_Weekday = "Weekday";

	private const string Str_PassDays = "PassDays";

	public static int Week
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_Week");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_Week", value);
		}
	}

	public static DayOfWeek Weekday
	{
		get
		{
			return GlobalsHelper.GetEnum<DayOfWeek>("Profile_" + GameGlobals.Profile + "_Weekday");
		}
		set
		{
			GlobalsHelper.SetEnum("Profile_" + GameGlobals.Profile + "_Weekday", value);
		}
	}

	public static int PassDays
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_PassDays");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_PassDays", value);
		}
	}

	public static void DeleteAll()
	{
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Week");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Weekday");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_PassDays");
	}
}
