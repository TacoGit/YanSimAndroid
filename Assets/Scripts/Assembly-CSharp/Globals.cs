using UnityEngine;

public static class Globals
{
	public static bool KeyExists(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public static void Delete(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	public static void DeleteCollection(string key, int[] usedKeys)
	{
		for (int i = 0; i < usedKeys.Length; i++)
		{
			int num = usedKeys[i];
			PlayerPrefs.DeleteKey(key + num);
		}
		KeysHelper.Delete(key);
	}

	public static void DeleteCollection(string key, string[] usedKeys)
	{
		foreach (string text in usedKeys)
		{
			PlayerPrefs.DeleteKey(key + text);
		}
		KeysHelper.Delete(key);
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}
}
