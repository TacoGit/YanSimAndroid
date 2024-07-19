using System;

[Serializable]
public class SenpaiSaveData
{
	public bool customSenpai;

	public string senpaiEyeColor = string.Empty;

	public int senpaiEyeWear;

	public int senpaiFacialHair;

	public string senpaiHairColor = string.Empty;

	public int senpaiHairStyle;

	public int senpaiSkinColor;

	public static SenpaiSaveData ReadFromGlobals()
	{
		SenpaiSaveData senpaiSaveData = new SenpaiSaveData();
		senpaiSaveData.customSenpai = SenpaiGlobals.CustomSenpai;
		senpaiSaveData.senpaiEyeColor = SenpaiGlobals.SenpaiEyeColor;
		senpaiSaveData.senpaiEyeWear = SenpaiGlobals.SenpaiEyeWear;
		senpaiSaveData.senpaiFacialHair = SenpaiGlobals.SenpaiFacialHair;
		senpaiSaveData.senpaiHairColor = SenpaiGlobals.SenpaiHairColor;
		senpaiSaveData.senpaiHairStyle = SenpaiGlobals.SenpaiHairStyle;
		senpaiSaveData.senpaiSkinColor = SenpaiGlobals.SenpaiSkinColor;
		return senpaiSaveData;
	}

	public static void WriteToGlobals(SenpaiSaveData data)
	{
		SenpaiGlobals.CustomSenpai = data.customSenpai;
		SenpaiGlobals.SenpaiEyeColor = data.senpaiEyeColor;
		SenpaiGlobals.SenpaiEyeWear = data.senpaiEyeWear;
		SenpaiGlobals.SenpaiFacialHair = data.senpaiFacialHair;
		SenpaiGlobals.SenpaiHairColor = data.senpaiHairColor;
		SenpaiGlobals.SenpaiHairStyle = data.senpaiHairStyle;
		SenpaiGlobals.SenpaiSkinColor = data.senpaiSkinColor;
	}
}
