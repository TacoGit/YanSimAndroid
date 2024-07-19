public static class EventGlobals
{
	private const string Str_BefriendConversation = "BefriendConversation";

	private const string Str_Event1 = "Event1";

	private const string Str_Event2 = "Event2";

	private const string Str_KidnapConversation = "KidnapConversation";

	private const string Str_LivingRoom = "LivingRoom";

	public static bool BefriendConversation
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_BefriendConversation");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_BefriendConversation", value);
		}
	}

	public static bool Event1
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_Event1");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_Event1", value);
		}
	}

	public static bool Event2
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_Event2");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_Event2", value);
		}
	}

	public static bool KidnapConversation
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_KidnapConversation");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_KidnapConversation", value);
		}
	}

	public static bool LivingRoom
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_LivingRoom");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_LivingRoom", value);
		}
	}

	public static void DeleteAll()
	{
		Globals.Delete("Profile_" + GameGlobals.Profile + "_BefriendConversation");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Event1");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Event2");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_KidnapConversation");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_LivingRoom");
	}
}
