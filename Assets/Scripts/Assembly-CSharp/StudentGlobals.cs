using UnityEngine;

public static class StudentGlobals
{
	private const string Str_CustomSuitor = "CustomSuitor";

	private const string Str_CustomSuitorAccessory = "CustomSuitorAccessory";

	private const string Str_CustomSuitorBlonde = "CustomSuitorBlonde";

	private const string Str_CustomSuitorEyewear = "CustomSuitorEyewear";

	private const string Str_CustomSuitorHair = "CustomSuitorHair";

	private const string Str_CustomSuitorJewelry = "CustomSuitorJewelry";

	private const string Str_CustomSuitorTan = "CustomSuitorTan";

	private const string Str_ExpelProgress = "ExpelProgress";

	private const string Str_FemaleUniform = "FemaleUniform";

	private const string Str_MaleUniform = "MaleUniform";

	private const string Str_StudentAccessory = "StudentAccessory_";

	private const string Str_StudentArrested = "StudentArrested_";

	private const string Str_StudentBroken = "StudentBroken_";

	private const string Str_StudentBustSize = "StudentBustSize_";

	private const string Str_StudentColor = "StudentColor_";

	private const string Str_StudentDead = "StudentDead_";

	private const string Str_StudentDying = "StudentDying_";

	private const string Str_StudentExpelled = "StudentExpelled_";

	private const string Str_StudentExposed = "StudentExposed_";

	private const string Str_StudentEyeColor = "StudentEyeColor_";

	private const string Str_StudentGrudge = "StudentGrudge_";

	private const string Str_StudentHairstyle = "StudentHairstyle_";

	private const string Str_StudentKidnapped = "StudentKidnapped_";

	private const string Str_StudentMissing = "StudentMissing_";

	private const string Str_StudentName = "StudentName_";

	private const string Str_StudentPhotographed = "StudentPhotographed_";

	private const string Str_StudentReplaced = "StudentReplaced_";

	private const string Str_StudentReputation = "StudentReputation_";

	private const string Str_StudentSanity = "StudentSanity_";

	private const string Str_StudentSlave = "StudentSlave";

	private const string Str_StudentFragileSlave = "StudentFragileSlave";

	private const string Str_FragileTarget = "FragileTarget";

	private const string Str_MemorialStudents = "MemorialStudents";

	private const string Str_MemorialStudent1 = "MemorialStudent1";

	private const string Str_MemorialStudent2 = "MemorialStudent2";

	private const string Str_MemorialStudent3 = "MemorialStudent3";

	private const string Str_MemorialStudent4 = "MemorialStudent4";

	private const string Str_MemorialStudent5 = "MemorialStudent5";

	private const string Str_MemorialStudent6 = "MemorialStudent6";

	private const string Str_MemorialStudent7 = "MemorialStudent7";

	private const string Str_MemorialStudent8 = "MemorialStudent8";

	private const string Str_MemorialStudent9 = "MemorialStudent9";

	public static bool CustomSuitor
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_CustomSuitor");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_CustomSuitor", value);
		}
	}

	public static int CustomSuitorAccessory
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorAccessory");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorAccessory", value);
		}
	}

	public static int CustomSuitorBlonde
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorBlonde");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorBlonde", value);
		}
	}

	public static int CustomSuitorEyewear
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorEyewear");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorEyewear", value);
		}
	}

	public static int CustomSuitorHair
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorHair");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorHair", value);
		}
	}

	public static int CustomSuitorJewelry
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorJewelry");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_CustomSuitorJewelry", value);
		}
	}

	public static bool CustomSuitorTan
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_CustomSuitorTan");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_CustomSuitorTan", value);
		}
	}

	public static int ExpelProgress
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_ExpelProgress");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_ExpelProgress", value);
		}
	}

	public static int FemaleUniform
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_FemaleUniform");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_FemaleUniform", value);
		}
	}

	public static int MaleUniform
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MaleUniform");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MaleUniform", value);
		}
	}

	public static int MemorialStudents
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudents");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudents", value);
		}
	}

	public static int MemorialStudent1
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent1");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent1", value);
		}
	}

	public static int MemorialStudent2
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent2");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent2", value);
		}
	}

	public static int MemorialStudent3
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent3");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent3", value);
		}
	}

	public static int MemorialStudent4
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent4");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent4", value);
		}
	}

	public static int MemorialStudent5
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent5");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent5", value);
		}
	}

	public static int MemorialStudent6
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent6");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent6", value);
		}
	}

	public static int MemorialStudent7
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent7");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent7", value);
		}
	}

	public static int MemorialStudent8
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent8");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent8", value);
		}
	}

	public static int MemorialStudent9
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent9");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_MemorialStudent9", value);
		}
	}

	public static string GetStudentAccessory(int studentID)
	{
		return PlayerPrefs.GetString("Profile_" + GameGlobals.Profile + "_StudentAccessory_" + studentID.ToString());
	}

	public static void SetStudentAccessory(int studentID, string value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentAccessory_", text);
		PlayerPrefs.SetString("Profile_" + GameGlobals.Profile + "_StudentAccessory_" + text, value);
	}

	public static int[] KeysOfStudentAccessory()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentAccessory_");
	}

	public static bool GetStudentArrested(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentArrested_" + studentID.ToString());
	}

	public static void SetStudentArrested(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentArrested_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentArrested_" + text, value);
	}

	public static int[] KeysOfStudentArrested()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentArrested_");
	}

	public static bool GetStudentBroken(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentBroken_" + studentID.ToString());
	}

	public static void SetStudentBroken(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentBroken_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentBroken_" + text, value);
	}

	public static int[] KeysOfStudentBroken()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentBroken_");
	}

	public static float GetStudentBustSize(int studentID)
	{
		return PlayerPrefs.GetFloat("Profile_" + GameGlobals.Profile + "_StudentBustSize_" + studentID.ToString());
	}

	public static void SetStudentBustSize(int studentID, float value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentBustSize_", text);
		PlayerPrefs.SetFloat("Profile_" + GameGlobals.Profile + "_StudentBustSize_" + text, value);
	}

	public static int[] KeysOfStudentBustSize()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentBustSize_");
	}

	public static Color GetStudentColor(int studentID)
	{
		return GlobalsHelper.GetColor("Profile_" + GameGlobals.Profile + "_StudentColor_" + studentID.ToString());
	}

	public static void SetStudentColor(int studentID, Color value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentColor_", text);
		GlobalsHelper.SetColor("Profile_" + GameGlobals.Profile + "_StudentColor_" + text, value);
	}

	public static int[] KeysOfStudentColor()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentColor_");
	}

	public static bool GetStudentDead(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentDead_" + studentID.ToString());
	}

	public static void SetStudentDead(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentDead_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentDead_" + text, value);
	}

	public static int[] KeysOfStudentDead()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentDead_");
	}

	public static bool GetStudentDying(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentDying_" + studentID.ToString());
	}

	public static void SetStudentDying(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentDying_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentDying_" + text, value);
	}

	public static int[] KeysOfStudentDying()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentDying_");
	}

	public static bool GetStudentExpelled(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentExpelled_" + studentID.ToString());
	}

	public static void SetStudentExpelled(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentExpelled_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentExpelled_" + text, value);
	}

	public static int[] KeysOfStudentExpelled()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentExpelled_");
	}

	public static bool GetStudentExposed(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentExposed_" + studentID.ToString());
	}

	public static void SetStudentExposed(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentExposed_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentExposed_" + text, value);
	}

	public static int[] KeysOfStudentExposed()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentExposed_");
	}

	public static Color GetStudentEyeColor(int studentID)
	{
		return GlobalsHelper.GetColor("Profile_" + GameGlobals.Profile + "_StudentEyeColor_" + studentID.ToString());
	}

	public static void SetStudentEyeColor(int studentID, Color value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentEyeColor_", text);
		GlobalsHelper.SetColor("Profile_" + GameGlobals.Profile + "_StudentEyeColor_" + text, value);
	}

	public static int[] KeysOfStudentEyeColor()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentEyeColor_");
	}

	public static bool GetStudentGrudge(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentGrudge_" + studentID.ToString());
	}

	public static void SetStudentGrudge(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentGrudge_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentGrudge_" + text, value);
	}

	public static int[] KeysOfStudentGrudge()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentGrudge_");
	}

	public static string GetStudentHairstyle(int studentID)
	{
		return PlayerPrefs.GetString("Profile_" + GameGlobals.Profile + "_StudentHairstyle_" + studentID.ToString());
	}

	public static void SetStudentHairstyle(int studentID, string value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentHairstyle_", text);
		PlayerPrefs.SetString("Profile_" + GameGlobals.Profile + "_StudentHairstyle_" + text, value);
	}

	public static int[] KeysOfStudentHairstyle()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentHairstyle_");
	}

	public static bool GetStudentKidnapped(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentKidnapped_" + studentID.ToString());
	}

	public static void SetStudentKidnapped(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentKidnapped_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentKidnapped_" + text, value);
	}

	public static int[] KeysOfStudentKidnapped()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentKidnapped_");
	}

	public static bool GetStudentMissing(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentMissing_" + studentID.ToString());
	}

	public static void SetStudentMissing(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentMissing_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentMissing_" + text, value);
	}

	public static int[] KeysOfStudentMissing()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentMissing_");
	}

	public static string GetStudentName(int studentID)
	{
		return PlayerPrefs.GetString("Profile_" + GameGlobals.Profile + "_StudentName_" + studentID.ToString());
	}

	public static void SetStudentName(int studentID, string value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentName_", text);
		PlayerPrefs.SetString("Profile_" + GameGlobals.Profile + "_StudentName_" + text, value);
	}

	public static int[] KeysOfStudentName()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentName_");
	}

	public static bool GetStudentPhotographed(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentPhotographed_" + studentID.ToString());
	}

	public static void SetStudentPhotographed(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentPhotographed_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentPhotographed_" + text, value);
	}

	public static int[] KeysOfStudentPhotographed()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentPhotographed_");
	}

	public static bool GetStudentReplaced(int studentID)
	{
		return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_StudentReplaced_" + studentID.ToString());
	}

	public static void SetStudentReplaced(int studentID, bool value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentReplaced_", text);
		GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_StudentReplaced_" + text, value);
	}

	public static int[] KeysOfStudentReplaced()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentReplaced_");
	}

	public static int GetStudentReputation(int studentID)
	{
		return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_StudentReputation_" + studentID.ToString());
	}

	public static void SetStudentReputation(int studentID, int value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentReputation_", text);
		PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_StudentReputation_" + text, value);
	}

	public static int[] KeysOfStudentReputation()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentReputation_");
	}

	public static float GetStudentSanity(int studentID)
	{
		return PlayerPrefs.GetFloat("Profile_" + GameGlobals.Profile + "_StudentSanity_" + studentID.ToString());
	}

	public static void SetStudentSanity(int studentID, float value)
	{
		string text = studentID.ToString();
		KeysHelper.AddIfMissing("Profile_" + GameGlobals.Profile + "_StudentSanity_", text);
		PlayerPrefs.SetFloat("Profile_" + GameGlobals.Profile + "_StudentSanity_" + text, value);
	}

	public static int[] KeysOfStudentSanity()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentSanity_");
	}

	public static int GetStudentSlave()
	{
		return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_StudentSlave");
	}

	public static int GetStudentFragileSlave()
	{
		return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_StudentFragileSlave");
	}

	public static void SetStudentSlave(int studentID)
	{
		PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_StudentSlave", studentID);
	}

	public static void SetStudentFragileSlave(int studentID)
	{
		PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_StudentFragileSlave", studentID);
	}

	public static int[] KeysOfStudentSlave()
	{
		return KeysHelper.GetIntegerKeys("Profile_" + GameGlobals.Profile + "_StudentSlave");
	}

	public static int GetFragileTarget()
	{
		return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_FragileTarget");
	}

	public static void SetFragileTarget(int value)
	{
		PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_FragileTarget", value);
	}

	public static void DeleteAll()
	{
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitor");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitorAccessory");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitorBlonde");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitorEyewear");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitorHair");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitorJewelry");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_CustomSuitorTan");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_ExpelProgress");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_FemaleUniform");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MaleUniform");
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentAccessory_", KeysOfStudentAccessory());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentArrested_", KeysOfStudentArrested());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentBroken_", KeysOfStudentBroken());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentBustSize_", KeysOfStudentBustSize());
		GlobalsHelper.DeleteColorCollection("Profile_" + GameGlobals.Profile + "_StudentColor_", KeysOfStudentColor());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentDead_", KeysOfStudentDead());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentDying_", KeysOfStudentDying());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentExpelled_", KeysOfStudentExpelled());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentExposed_", KeysOfStudentExposed());
		GlobalsHelper.DeleteColorCollection("Profile_" + GameGlobals.Profile + "_StudentEyeColor_", KeysOfStudentEyeColor());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentGrudge_", KeysOfStudentGrudge());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentHairstyle_", KeysOfStudentHairstyle());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentKidnapped_", KeysOfStudentKidnapped());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentMissing_", KeysOfStudentMissing());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentName_", KeysOfStudentName());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentPhotographed_", KeysOfStudentPhotographed());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentReplaced_", KeysOfStudentReplaced());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentReputation_", KeysOfStudentReputation());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentSanity_", KeysOfStudentSanity());
		Globals.DeleteCollection("Profile_" + GameGlobals.Profile + "_StudentSlave", KeysOfStudentSlave());
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudents");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent1");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent2");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent3");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent4");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent5");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent6");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent7");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent8");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_MemorialStudent9");
	}
}
