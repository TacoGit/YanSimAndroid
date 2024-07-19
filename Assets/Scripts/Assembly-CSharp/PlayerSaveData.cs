using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
	public int alerts;

	public int enlightenment;

	public int enlightenmentBonus;

	public bool headset;

	public int kills;

	public int numbness;

	public int numbnessBonus;

	public int pantiesEquipped;

	public int pantyShots;

	public IntHashSet photo = new IntHashSet();

	public IntHashSet photoOnCorkboard = new IntHashSet();

	public IntAndVector2Dictionary photoPosition = new IntAndVector2Dictionary();

	public IntAndFloatDictionary photoRotation = new IntAndFloatDictionary();

	public float reputation;

	public int seduction;

	public int seductionBonus;

	public IntHashSet senpaiPhoto = new IntHashSet();

	public int senpaiShots;

	public int socialBonus;

	public int speedBonus;

	public int stealthBonus;

	public IntHashSet studentFriend = new IntHashSet();

	public StringHashSet studentPantyShot = new StringHashSet();

	public static PlayerSaveData ReadFromGlobals()
	{
		PlayerSaveData playerSaveData = new PlayerSaveData();
		playerSaveData.alerts = PlayerGlobals.Alerts;
		playerSaveData.enlightenment = PlayerGlobals.Enlightenment;
		playerSaveData.enlightenmentBonus = PlayerGlobals.EnlightenmentBonus;
		playerSaveData.headset = PlayerGlobals.Headset;
		playerSaveData.kills = PlayerGlobals.Kills;
		playerSaveData.numbness = PlayerGlobals.Numbness;
		playerSaveData.numbnessBonus = PlayerGlobals.NumbnessBonus;
		playerSaveData.pantiesEquipped = PlayerGlobals.PantiesEquipped;
		playerSaveData.pantyShots = PlayerGlobals.PantyShots;
		int[] array = PlayerGlobals.KeysOfPhoto();
		foreach (int num in array)
		{
			if (PlayerGlobals.GetPhoto(num))
			{
				playerSaveData.photo.Add(num);
			}
		}
		int[] array2 = PlayerGlobals.KeysOfPhotoOnCorkboard();
		foreach (int num2 in array2)
		{
			if (PlayerGlobals.GetPhotoOnCorkboard(num2))
			{
				playerSaveData.photoOnCorkboard.Add(num2);
			}
		}
		int[] array3 = PlayerGlobals.KeysOfPhotoPosition();
		foreach (int num3 in array3)
		{
			playerSaveData.photoPosition.Add(num3, PlayerGlobals.GetPhotoPosition(num3));
		}
		int[] array4 = PlayerGlobals.KeysOfPhotoRotation();
		foreach (int num4 in array4)
		{
			playerSaveData.photoRotation.Add(num4, PlayerGlobals.GetPhotoRotation(num4));
		}
		playerSaveData.reputation = PlayerGlobals.Reputation;
		playerSaveData.seduction = PlayerGlobals.Seduction;
		playerSaveData.seductionBonus = PlayerGlobals.SeductionBonus;
		int[] array5 = PlayerGlobals.KeysOfSenpaiPhoto();
		foreach (int num5 in array5)
		{
			if (PlayerGlobals.GetSenpaiPhoto(num5))
			{
				playerSaveData.senpaiPhoto.Add(num5);
			}
		}
		playerSaveData.senpaiShots = PlayerGlobals.SenpaiShots;
		playerSaveData.socialBonus = PlayerGlobals.SocialBonus;
		playerSaveData.speedBonus = PlayerGlobals.SpeedBonus;
		playerSaveData.stealthBonus = PlayerGlobals.StealthBonus;
		int[] array6 = PlayerGlobals.KeysOfStudentFriend();
		foreach (int num6 in array6)
		{
			if (PlayerGlobals.GetStudentFriend(num6))
			{
				playerSaveData.studentFriend.Add(num6);
			}
		}
		string[] array7 = PlayerGlobals.KeysOfStudentPantyShot();
		foreach (string text in array7)
		{
			if (PlayerGlobals.GetStudentPantyShot(text))
			{
				playerSaveData.studentPantyShot.Add(text);
			}
		}
		return playerSaveData;
	}

	public static void WriteToGlobals(PlayerSaveData data)
	{
		PlayerGlobals.Alerts = data.alerts;
		PlayerGlobals.Enlightenment = data.enlightenment;
		PlayerGlobals.EnlightenmentBonus = data.enlightenmentBonus;
		PlayerGlobals.Headset = data.headset;
		PlayerGlobals.Kills = data.kills;
		PlayerGlobals.Numbness = data.numbness;
		PlayerGlobals.NumbnessBonus = data.numbnessBonus;
		PlayerGlobals.PantiesEquipped = data.pantiesEquipped;
		PlayerGlobals.PantyShots = data.pantyShots;
		foreach (int item in data.photo)
		{
			PlayerGlobals.SetPhoto(item, true);
		}
		foreach (int item2 in data.photoOnCorkboard)
		{
			PlayerGlobals.SetPhotoOnCorkboard(item2, true);
		}
		foreach (KeyValuePair<int, Vector2> item3 in data.photoPosition)
		{
			PlayerGlobals.SetPhotoPosition(item3.Key, item3.Value);
		}
		foreach (KeyValuePair<int, float> item4 in data.photoRotation)
		{
			PlayerGlobals.SetPhotoRotation(item4.Key, item4.Value);
		}
		PlayerGlobals.Reputation = data.reputation;
		PlayerGlobals.Seduction = data.seduction;
		PlayerGlobals.SeductionBonus = data.seductionBonus;
		foreach (int item5 in data.senpaiPhoto)
		{
			PlayerGlobals.SetSenpaiPhoto(item5, true);
		}
		PlayerGlobals.SenpaiShots = data.senpaiShots;
		PlayerGlobals.SocialBonus = data.socialBonus;
		PlayerGlobals.SpeedBonus = data.speedBonus;
		PlayerGlobals.StealthBonus = data.stealthBonus;
		foreach (int item6 in data.studentFriend)
		{
			PlayerGlobals.SetStudentFriend(item6, true);
		}
		foreach (string item7 in data.studentPantyShot)
		{
			PlayerGlobals.SetStudentPantyShot(item7, true);
		}
	}
}
