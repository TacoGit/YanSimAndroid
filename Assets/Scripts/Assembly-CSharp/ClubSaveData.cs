using System;

[Serializable]
public class ClubSaveData
{
	public ClubType club;

	public ClubTypeHashSet clubClosed = new ClubTypeHashSet();

	public ClubTypeHashSet clubKicked = new ClubTypeHashSet();

	public ClubTypeHashSet quitClub = new ClubTypeHashSet();

	public static ClubSaveData ReadFromGlobals()
	{
		ClubSaveData clubSaveData = new ClubSaveData();
		clubSaveData.club = ClubGlobals.Club;
		ClubType[] array = ClubGlobals.KeysOfClubClosed();
		foreach (ClubType clubType in array)
		{
			if (ClubGlobals.GetClubClosed(clubType))
			{
				clubSaveData.clubClosed.Add(clubType);
			}
		}
		ClubType[] array2 = ClubGlobals.KeysOfClubKicked();
		foreach (ClubType clubType2 in array2)
		{
			if (ClubGlobals.GetClubKicked(clubType2))
			{
				clubSaveData.clubKicked.Add(clubType2);
			}
		}
		ClubType[] array3 = ClubGlobals.KeysOfQuitClub();
		foreach (ClubType clubType3 in array3)
		{
			if (ClubGlobals.GetQuitClub(clubType3))
			{
				clubSaveData.quitClub.Add(clubType3);
			}
		}
		return clubSaveData;
	}

	public static void WriteToGlobals(ClubSaveData data)
	{
		ClubGlobals.Club = data.club;
		foreach (ClubType item in data.clubClosed)
		{
			ClubGlobals.SetClubClosed(item, true);
		}
		foreach (ClubType item2 in data.clubKicked)
		{
			ClubGlobals.SetClubKicked(item2, true);
		}
		foreach (ClubType item3 in data.quitClub)
		{
			ClubGlobals.SetQuitClub(item3, true);
		}
	}
}
