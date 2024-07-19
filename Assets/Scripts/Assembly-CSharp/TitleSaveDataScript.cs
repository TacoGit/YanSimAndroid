using UnityEngine;

public class TitleSaveDataScript : MonoBehaviour
{
	public GameObject EmptyFile;

	public GameObject Data;

	public Texture[] Bloods;

	public UITexture Blood;

	public UILabel Kills;

	public UILabel Mood;

	public UILabel Alerts;

	public UILabel Week;

	public UILabel Day;

	public UILabel Rival;

	public UILabel Rep;

	public UILabel Club;

	public UILabel Friends;

	public int ID;

	public void Start()
	{
		if (PlayerPrefs.GetInt("ProfileCreated_" + ID) == 1)
		{
			GameGlobals.Profile = ID;
			EmptyFile.SetActive(false);
			Data.SetActive(true);
			Kills.text = "Kills: " + PlayerGlobals.Kills;
			Mood.text = "Mood: " + Mathf.RoundToInt(SchoolGlobals.SchoolAtmosphere * 100f);
			Alerts.text = "Alerts: " + PlayerGlobals.Alerts;
			Week.text = "Week: " + 1;
			Day.text = "Day: " + DateGlobals.Weekday;
			Rival.text = "Rival: Osana";
			Rep.text = "Rep: " + PlayerGlobals.Reputation;
			Club.text = "Club: " + ClubGlobals.Club;
			Friends.text = "Friends: " + PlayerGlobals.Friends;
			if (PlayerGlobals.Kills == 0)
			{
				Blood.mainTexture = null;
			}
			else if (PlayerGlobals.Kills > 0)
			{
				Blood.mainTexture = Bloods[1];
			}
			else if (PlayerGlobals.Kills > 5)
			{
				Blood.mainTexture = Bloods[2];
			}
			else if (PlayerGlobals.Kills > 10)
			{
				Blood.mainTexture = Bloods[3];
			}
			else if (PlayerGlobals.Kills > 15)
			{
				Blood.mainTexture = Bloods[4];
			}
			else if (PlayerGlobals.Kills > 20)
			{
				Blood.mainTexture = Bloods[5];
			}
		}
		else
		{
			EmptyFile.SetActive(true);
			Data.SetActive(false);
			Blood.enabled = false;
		}
	}
}
