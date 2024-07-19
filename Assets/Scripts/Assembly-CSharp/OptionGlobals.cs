using UnityEngine;

public static class OptionGlobals
{
	private const string Str_DisableBloom = "DisableBloom";

	private const string Str_DisableFarAnimations = "DisableFarAnimations";

	private const string Str_DisableOutlines = "DisableOutlines";

	private const string Str_DisablePostAliasing = "DisablePostAliasing";

	private const string Str_DisableShadows = "DisableShadows";

	private const string Str_DrawDistance = "DrawDistance";

	private const string Str_DrawDistanceLimit = "DrawDistanceLimit";

	private const string Str_Fog = "Fog";

	private const string Str_FPSIndex = "FPSIndex";

	private const string Str_HighPopulation = "HighPopulation";

	private const string Str_LowDetailStudents = "LowDetailStudents";

	private const string Str_ParticleCount = "ParticleCount";

	private const string Str_RimLight = "RimLight";

	private const string Str_DepthOfField = "DepthOfField";

	private const string Str_Sensitivity = "Sensitivity";

	private const string Str_InvertAxis = "InvertAxis";

	public static bool DisableBloom
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_DisableBloom");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_DisableBloom", value);
		}
	}

	public static int DisableFarAnimations
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_DisableFarAnimations");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_DisableFarAnimations", value);
		}
	}

	public static bool DisableOutlines
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_DisableOutlines");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_DisableOutlines", value);
		}
	}

	public static bool DisablePostAliasing
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_DisablePostAliasing");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_DisablePostAliasing", value);
		}
	}

	public static bool DisableShadows
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_DisableShadows");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_DisableShadows", value);
		}
	}

	public static int DrawDistance
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_DrawDistance");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_DrawDistance", value);
		}
	}

	public static int DrawDistanceLimit
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_DrawDistanceLimit");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_DrawDistanceLimit", value);
		}
	}

	public static bool Fog
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_Fog");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_Fog", value);
		}
	}

	public static int FPSIndex
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_FPSIndex");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_FPSIndex", value);
		}
	}

	public static bool HighPopulation
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_HighPopulation");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_HighPopulation", value);
		}
	}

	public static int LowDetailStudents
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_LowDetailStudents");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_LowDetailStudents", value);
		}
	}

	public static int ParticleCount
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_ParticleCount");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_ParticleCount", value);
		}
	}

	public static bool RimLight
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_RimLight");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_RimLight", value);
		}
	}

	public static bool DepthOfField
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_DepthOfField");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_DepthOfField", value);
		}
	}

	public static int Sensitivity
	{
		get
		{
			return PlayerPrefs.GetInt("Profile_" + GameGlobals.Profile + "_Sensitivity");
		}
		set
		{
			PlayerPrefs.SetInt("Profile_" + GameGlobals.Profile + "_Sensitivity", value);
		}
	}

	public static bool InvertAxis
	{
		get
		{
			return GlobalsHelper.GetBool("Profile_" + GameGlobals.Profile + "_InvertAxis");
		}
		set
		{
			GlobalsHelper.SetBool("Profile_" + GameGlobals.Profile + "_InvertAxis", value);
		}
	}

	public static void DeleteAll()
	{
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DisableBloom");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DisableFarAnimations");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DisableOutlines");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DisablePostAliasing");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DisableShadows");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DrawDistance");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DrawDistanceLimit");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Fog");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_FPSIndex");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_HighPopulation");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_LowDetailStudents");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_ParticleCount");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_RimLight");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_DepthOfField");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_Sensitivity");
		Globals.Delete("Profile_" + GameGlobals.Profile + "_InvertAxis");
	}
}
