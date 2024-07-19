using System;

[Serializable]
public class OptionSaveData
{
	public bool disableBloom;

	public int disableFarAnimations = 5;

	public bool disableOutlines;

	public bool disablePostAliasing;

	public bool disableShadows;

	public int drawDistance;

	public int drawDistanceLimit;

	public bool fog;

	public int fpsIndex;

	public bool highPopulation;

	public int lowDetailStudents;

	public int particleCount;

	public static OptionSaveData ReadFromGlobals()
	{
		OptionSaveData optionSaveData = new OptionSaveData();
		optionSaveData.disableBloom = OptionGlobals.DisableBloom;
		optionSaveData.disableFarAnimations = OptionGlobals.DisableFarAnimations;
		optionSaveData.disableOutlines = OptionGlobals.DisableOutlines;
		optionSaveData.disablePostAliasing = OptionGlobals.DisablePostAliasing;
		optionSaveData.disableShadows = OptionGlobals.DisableShadows;
		optionSaveData.drawDistance = OptionGlobals.DrawDistance;
		optionSaveData.drawDistanceLimit = OptionGlobals.DrawDistanceLimit;
		optionSaveData.fog = OptionGlobals.Fog;
		optionSaveData.fpsIndex = OptionGlobals.FPSIndex;
		optionSaveData.highPopulation = OptionGlobals.HighPopulation;
		optionSaveData.lowDetailStudents = OptionGlobals.LowDetailStudents;
		optionSaveData.particleCount = OptionGlobals.ParticleCount;
		return optionSaveData;
	}

	public static void WriteToGlobals(OptionSaveData data)
	{
		OptionGlobals.DisableBloom = data.disableBloom;
		OptionGlobals.DisableFarAnimations = data.disableFarAnimations;
		OptionGlobals.DisableOutlines = data.disableOutlines;
		OptionGlobals.DisablePostAliasing = data.disablePostAliasing;
		OptionGlobals.DisableShadows = data.disableShadows;
		OptionGlobals.DrawDistance = data.drawDistance;
		OptionGlobals.DrawDistanceLimit = data.drawDistanceLimit;
		OptionGlobals.Fog = data.fog;
		OptionGlobals.FPSIndex = data.fpsIndex;
		OptionGlobals.HighPopulation = data.highPopulation;
		OptionGlobals.LowDetailStudents = data.lowDetailStudents;
		OptionGlobals.ParticleCount = data.particleCount;
	}
}
