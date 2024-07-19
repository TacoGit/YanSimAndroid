public static class ClubGlobals
{
	private const string Str_Club = "Club";

	private const string Str_ClubClosed = "ClubClosed_";

	private const string Str_ClubKicked = "ClubKicked_";

	private const string Str_QuitClub = "QuitClub_";

	public static ClubType Club
	{
		get
		{
			return GlobalsHelper.GetEnum<ClubType>("Profile_" + GameGlobals.Profile + "_Club");
		}
		set
		{
			GlobalsHelper.SetEnum("Profile_" + GameGlobals.Profile + "_Club", value);
		}
	}

	public static bool GetClubClosed(ClubType clubID)
	{
		object[] obj = new object[4]
		{
			"Profile_",
			GameGlobals.Profile,
			"_ClubClosed_",
			null
		};
		int num = (int)clubID;
		obj[3] = num.ToString();
		return GlobalsHelper.GetBool(string.Concat(obj));
	}

	public static void SetClubClosed(ClubType clubID, bool value)
	{
		int num = (int)clubID;
		string text = num.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_ClubClosed_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_ClubClosed_" + text, value);
	}

	public static ClubType[] KeysOfClubClosed()
	{
		return KeysHelper.GetEnumKeys<ClubType>("Profile_" + GameGlobals.Profile + "_ClubClosed_");
	}

	public static bool GetClubKicked(ClubType clubID)
	{
		object[] obj = new object[4]
		{
			"Profile_",
			GameGlobals.Profile,
			"_ClubKicked_",
			null
		};
		int num = (int)clubID;
		obj[3] = num.ToString();
		return GlobalsHelper.GetBool(string.Concat(obj));
	}

	public static void SetClubKicked(ClubType clubID, bool value)
	{
		int num = (int)clubID;
		string text = num.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_ClubKicked_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_ClubKicked_" + text, value);
	}

	public static ClubType[] KeysOfClubKicked()
	{
		return KeysHelper.GetEnumKeys<ClubType>("Profile_" + GameGlobals.Profile + "_ClubKicked_");
	}

	public static bool GetQuitClub(ClubType clubID)
	{
		object[] obj = new object[4]
		{
			"Profile_",
			GameGlobals.Profile,
			"_QuitClub_",
			null
		};
		int num = (int)clubID;
		obj[3] = num.ToString();
		return GlobalsHelper.GetBool(string.Concat(obj));
	}

	public static void SetQuitClub(ClubType clubID, bool value)
	{
		int num = (int)clubID;
		string text = num.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_QuitClub_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_QuitClub_" + text, value);
	}

	public static ClubType[] KeysOfQuitClub()
	{
		return KeysHelper.GetEnumKeys<ClubType>("Profile_" + GameGlobals.Profile + "_QuitClub_");
	}

	public static void DeleteAll()
	{
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Club");
		ClubType[] array = KeysOfClubClosed();
		foreach (ClubType clubType in array)
		{
			object[] obj = new object[4]
			{
				"Profile_",
				GameGlobals.Profile,
				"_ClubClosed_",
				null
			};
			int num = (int)clubType;
			obj[3] = num.ToString();
			Globals.Delete(string.Concat(obj));
		}
		ClubType[] array2 = KeysOfClubKicked();
		foreach (ClubType clubType2 in array2)
		{
			object[] obj2 = new object[4]
			{
				"Profile_",
				GameGlobals.Profile,
				"_ClubKicked_",
				null
			};
			int num2 = (int)clubType2;
			obj2[3] = num2.ToString();
			Globals.Delete(string.Concat(obj2));
		}
		ClubType[] array3 = KeysOfQuitClub();
		foreach (ClubType clubType3 in array3)
		{
			object[] obj3 = new object[4]
			{
				"Profile_",
				GameGlobals.Profile,
				"_QuitClub_",
				null
			};
			int num3 = (int)clubType3;
			obj3[3] = num3.ToString();
			Globals.Delete(string.Concat(obj3));
		}
		KeysHelper.Delete("Profile_" + GameGlobals.Profile + "_ClubClosed_");
		KeysHelper.Delete("Profile_" + GameGlobals.Profile + "_ClubKicked_");
		KeysHelper.Delete("Profile_" + GameGlobals.Profile + "_QuitClub_");
	}
}
