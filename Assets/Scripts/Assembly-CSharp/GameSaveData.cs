using System;

[Serializable]
public class GameSaveData
{
	public bool loveSick;

	public bool masksBanned;

	public bool paranormal;

	public static GameSaveData ReadFromGlobals()
	{
		GameSaveData gameSaveData = new GameSaveData();
		gameSaveData.loveSick = GameGlobals.LoveSick;
		gameSaveData.masksBanned = GameGlobals.MasksBanned;
		gameSaveData.paranormal = GameGlobals.Paranormal;
		return gameSaveData;
	}

	public static void WriteToGlobals(GameSaveData data)
	{
		GameGlobals.LoveSick = data.loveSick;
		GameGlobals.MasksBanned = data.masksBanned;
		GameGlobals.Paranormal = data.paranormal;
	}
}
