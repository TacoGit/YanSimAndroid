using System;

[Serializable]
public class YanvaniaSaveData
{
	public bool draculaDefeated;

	public bool midoriEasterEgg;

	public static YanvaniaSaveData ReadFromGlobals()
	{
		YanvaniaSaveData yanvaniaSaveData = new YanvaniaSaveData();
		yanvaniaSaveData.draculaDefeated = YanvaniaGlobals.DraculaDefeated;
		yanvaniaSaveData.midoriEasterEgg = YanvaniaGlobals.MidoriEasterEgg;
		return yanvaniaSaveData;
	}

	public static void WriteToGlobals(YanvaniaSaveData data)
	{
		YanvaniaGlobals.DraculaDefeated = data.draculaDefeated;
		YanvaniaGlobals.MidoriEasterEgg = data.midoriEasterEgg;
	}
}
