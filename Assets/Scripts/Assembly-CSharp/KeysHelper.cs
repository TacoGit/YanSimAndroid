using System;
using System.Collections.Generic;
using UnityEngine;

public static class KeysHelper
{
	private const string KeyListPrefix = "Keys";

	private const char KeyListSeparator = '|';

	public const char PairSeparator = '^';

	public static int[] GetIntegerKeys(string key)
	{
		string keyList = GetKeyList(GetKeyListKey(key));
		string[] array = SplitList(keyList);
		return Array.ConvertAll(array, int.Parse);
	}

	public static string[] GetStringKeys(string key)
	{
		string keyList = GetKeyList(GetKeyListKey(key));
		return SplitList(keyList);
	}

	public static T[] GetEnumKeys<T>(string key) where T : struct, IConvertible
	{
		string keyList = GetKeyList(GetKeyListKey(key));
		string[] array = SplitList(keyList);
		return Array.ConvertAll(array, (string str) => (T)Enum.Parse(typeof(T), str));
	}

	public static KeyValuePair<T, U>[] GetKeys<T, U>(string key) where T : struct where U : struct
	{
		string keyList = GetKeyList(GetKeyListKey(key));
		string[] array = SplitList(keyList);
		KeyValuePair<T, U>[] array2 = new KeyValuePair<T, U>[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('^');
			array2[i] = new KeyValuePair<T, U>((T)(object)int.Parse(array3[0]), (U)(object)int.Parse(array3[1]));
		}
		return array2;
	}

	public static void AddIfMissing(string key, string id)
	{
		string keyListKey = GetKeyListKey(key);
		string keyList = GetKeyList(keyListKey);
		string[] keyListStrings = SplitList(keyList);
		if (!HasKey(keyListStrings, id))
		{
			AppendKey(keyListKey, keyList, id);
		}
	}

	public static void Delete(string key)
	{
		string keyListKey = GetKeyListKey(key);
		Globals.Delete(keyListKey);
	}

	private static string GetKeyListKey(string key)
	{
		return key + "Keys";
	}

	private static string GetKeyList(string keyListKey)
	{
		return PlayerPrefs.GetString(keyListKey);
	}

	private static string[] SplitList(string keyList)
	{
		return (keyList.Length <= 0) ? new string[0] : keyList.Split('|');
	}

	private static int FindKey(string[] keyListStrings, string key)
	{
		return Array.IndexOf(keyListStrings, key);
	}

	private static bool HasKey(string[] keyListStrings, string key)
	{
		return FindKey(keyListStrings, key) > -1;
	}

	private static void AppendKey(string keyListKey, string keyList, string key)
	{
		string value = ((keyList.Length != 0) ? (keyList + '|' + key) : (keyList + key));
		PlayerPrefs.SetString(keyListKey, value);
	}
}
