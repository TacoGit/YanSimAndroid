public static class CollectibleGlobals
{
	private const string Str_HeadmasterTapeCollected = "HeadmasterTapeCollected_";

	private const string Str_HeadmasterTapeListened = "HeadmasterTapeListened_";

	private const string Str_BasementTapeCollected = "BasementTapeCollected_";

	private const string Str_BasementTapeListened = "BasementTapeListened_";

	private const string Str_MangaCollected = "MangaCollected_";

	private const string Str_TapeCollected = "TapeCollected_";

	private const string Str_TapeListened = "TapeListened_";

	public static bool GetHeadmasterTapeCollected(int tapeID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_HeadmasterTapeCollected_" + tapeID.ToString());
	}

	public static void SetHeadmasterTapeCollected(int tapeID, bool value)
	{
		string text = tapeID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_HeadmasterTapeCollected_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_HeadmasterTapeCollected_" + text, value);
	}

	public static bool GetHeadmasterTapeListened(int tapeID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_HeadmasterTapeListened_" + tapeID.ToString());
	}

	public static void SetHeadmasterTapeListened(int tapeID, bool value)
	{
		string text = tapeID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_HeadmasterTapeListened_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_HeadmasterTapeListened_" + text, value);
	}

	public static bool GetBasementTapeCollected(int tapeID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_BasementTapeCollected_" + tapeID.ToString());
	}

	public static void SetBasementTapeCollected(int tapeID, bool value)
	{
		string text = tapeID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_BasementTapeCollected_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_BasementTapeCollected_" + text, value);
	}

	public static int[] KeysOfBasementTapeCollected()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_BasementTapeCollected_");
	}

	public static bool GetBasementTapeListened(int tapeID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_BasementTapeListened_" + tapeID.ToString());
	}

	public static void SetBasementTapeListened(int tapeID, bool value)
	{
		string text = tapeID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_BasementTapeListened_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_BasementTapeListened_" + text, value);
	}

	public static int[] KeysOfBasementTapeListened()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_BasementTapeListened_");
	}

	public static bool GetMangaCollected(int mangaID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_MangaCollected_" + mangaID.ToString());
	}

	public static void SetMangaCollected(int mangaID, bool value)
	{
		string text = mangaID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_MangaCollected_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_MangaCollected_" + text, value);
	}

	public static int[] KeysOfMangaCollected()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_MangaCollected_");
	}

	public static bool GetTapeCollected(int tapeID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_TapeCollected_" + tapeID.ToString());
	}

	public static void SetTapeCollected(int tapeID, bool value)
	{
		string text = tapeID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_TapeCollected_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_TapeCollected_" + text, value);
	}

	public static int[] KeysOfTapeCollected()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_TapeCollected_");
	}

	public static bool GetTapeListened(int tapeID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_TapeListened_" + tapeID.ToString());
	}

	public static void SetTapeListened(int tapeID, bool value)
	{
		string text = tapeID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_TapeListened_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_TapeListened_" + text, value);
	}

	public static int[] KeysOfTapeListened()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_TapeListened_");
	}

	public static void DeleteAll()
	{
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_BasementTapeCollected_", KeysOfBasementTapeCollected());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_BasementTapeListened_", KeysOfBasementTapeListened());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_MangaCollected_", KeysOfMangaCollected());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_TapeCollected_", KeysOfTapeCollected());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_TapeListened_", KeysOfTapeListened());
	}
}
