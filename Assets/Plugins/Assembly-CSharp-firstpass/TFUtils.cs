using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TFUtils
{
	public static DateTime EPOCH = new DateTime(1970, 1, 1).ToUniversalTime();

	public static string ApplicationDataPath;

	public static string ApplicationPersistentDataPath;

	public static string DeviceID;

	public static string DeviceName;

	public static void Init()
	{
		ApplicationDataPath = Application.dataPath;
		ApplicationPersistentDataPath = Application.persistentDataPath;
		DeviceID = SystemInfo.deviceUniqueIdentifier;
		DeviceName = SystemInfo.deviceName;
		DebugLog("This device is:" + DeviceID);
	}

	public static int EpochTime()
	{
		return EpochTime(DateTime.UtcNow);
	}

	public static int EpochTime(DateTime dt)
	{
		return (int)(dt - EPOCH).TotalSeconds;
	}

	public static DateTime EpochToDateTime(int seconds)
	{
		return DateTime.SpecifyKind(EPOCH.AddSeconds(seconds), DateTimeKind.Utc);
	}

	public static string DurationToString(int duration)
	{
		if (duration < 60)
		{
			return string.Format("{0}s", duration);
		}
		int num = duration % 60;
		duration -= num;
		int num2 = duration / 60;
		if (num2 < 60)
		{
			if (num == 0)
			{
				return string.Format("{0}m", num2);
			}
			return string.Format("{0}m {1}s", num2, num);
		}
		int num3 = num2 / 60;
		num2 %= 60;
		if (num3 < 24)
		{
			if (num2 == 0)
			{
				return string.Format("{0}h", num3);
			}
			return string.Format("{0}h {1}m", num3, num2);
		}
		int num4 = num3 / 24;
		num3 %= 24;
		if (num3 == 0)
		{
			return string.Format("{0}d", num4);
		}
		return string.Format("{0}d {1}h", num4, num3);
	}

	public static Dictionary<KeyType, ValueType> CloneDictionary<KeyType, ValueType>(Dictionary<KeyType, ValueType> source)
	{
		Dictionary<KeyType, ValueType> dictionary = new Dictionary<KeyType, ValueType>();
		foreach (KeyType key in source.Keys)
		{
			dictionary[key] = source[key];
		}
		return dictionary;
	}

	public static void CloneDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> source, Dictionary<KeyType, ValueType> dest)
	{
		dest.Clear();
		foreach (KeyValuePair<KeyType, ValueType> item in source)
		{
			dest.Add(item.Key, item.Value);
		}
	}

	public static Dictionary<KeyType, ValueType> ConcatenateDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> dest, Dictionary<KeyType, ValueType> source)
	{
		foreach (KeyType key in source.Keys)
		{
			if (dest.ContainsKey(key))
			{
				throw new ArgumentException("Destination dictionary already contains key " + key.ToString());
			}
			dest[key] = source[key];
		}
		return dest;
	}

	public static List<To> CloneAndCastList<From, To>(List<From> list) where From : To
	{
		List<To> list2 = new List<To>(list.Count);
		foreach (From item in list)
		{
			list2.Add((To)(object)item);
		}
		return list2;
	}

	private static T AssertCast<T>(Dictionary<string, object> dict, string key)
	{
		AssertKeyExists(dict, key);
		Assert(dict[key] is T, string.Format("Could not cast the key({0}) with value({1}) to type({2}) in dictionary{3}", key, dict[key], typeof(T).ToString(), DebugDictToString(dict)));
		return (T)dict[key];
	}

	public static void AssertKeyExists(Dictionary<string, object> dict, string key)
	{
		Assert(dict != null, string.Format("Can't search for the key '{0}' in a null dictionary", key));
		Assert(dict.ContainsKey(key), string.Format("Could not find the key '{0}' in the given dictionary:\n{1}", key, DebugDictToString(dict)));
	}

	public static bool? LoadNullableBool(Dictionary<string, object> d, string key)
	{
		AssertKeyExists(d, key);
		object obj = d[key];
		if (obj == null)
		{
			return (bool?)obj;
		}
		return (bool?)d[key];
	}

	public static List<T> TryLoadList<T>(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return LoadList<T>(data, key);
	}

	public static List<T> LoadList<T>(Dictionary<string, object> data, string key)
	{
		AssertKeyExists(data, key);
		if (data[key] is List<T>)
		{
			return (List<T>)data[key];
		}
		List<object> list = (List<object>)data[key];
		List<T> retval = new List<T>(data.Count);
		list.ForEach(delegate(object obj)
		{
			retval.Add((T)Convert.ChangeType(obj, typeof(T)));
		});
		return retval;
	}

	public static Dictionary<string, object> LoadDict(Dictionary<string, object> data, string key)
	{
		AssertKeyExists(data, key);
		return (Dictionary<string, object>)data[key];
	}

	public static Dictionary<string, object> TryLoadDict(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return (Dictionary<string, object>)data[key];
	}

	public static string LoadString(Dictionary<string, object> data, string key)
	{
		AssertKeyExists(data, key);
		AssertCast<string>(data, key);
		return (string)data[key];
	}

	public static string TryLoadString(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return AssertCast<string>(data, key);
		}
		return null;
	}

	public static string LoadNullableString(Dictionary<string, object> data, string key)
	{
		AssertKeyExists(data, key);
		return (string)data[key];
	}

	public static int? LoadNullableInt(Dictionary<string, object> d, string key)
	{
		AssertKeyExists(d, key);
		object obj = d[key];
		if (obj == null)
		{
			return (int?)obj;
		}
		return LoadInt(d, key);
	}

	public static uint? LoadNullableUInt(Dictionary<string, object> d, string key)
	{
		AssertKeyExists(d, key);
		object obj = d[key];
		if (obj == null)
		{
			return (uint?)obj;
		}
		return LoadUint(d, key);
	}

	public static uint? TryLoadNullableUInt(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return LoadNullableUInt(d, key);
		}
		return null;
	}

	public static object NullableToObject<T>(T? nullable) where T : struct
	{
		return (!nullable.HasValue) ? null : ((object)nullable.Value);
	}

	public static int? TryLoadInt(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return LoadIntHelper(data, key);
		}
		return null;
	}

	public static bool LoadBoolAsInt(Dictionary<string, object> d, string key)
	{
		return (LoadInt(d, key) != 0) ? true : false;
	}

	public static int LoadInt(Dictionary<string, object> d, string key)
	{
		AssertKeyExists(d, key);
		return LoadIntHelper(d, key);
	}

	private static int LoadIntHelper(Dictionary<string, object> d, string key)
	{
		return Convert.ToInt32(d[key]);
	}

	public static uint LoadUint(Dictionary<string, object> data, string key)
	{
		AssertKeyExists(data, key);
		return LoadUintHelper(data, key);
	}

	public static uint? TryLoadUint(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return LoadUintHelper(data, key);
	}

	private static uint LoadUintHelper(Dictionary<string, object> data, string key)
	{
		return Convert.ToUInt32(data[key]);
	}

	public static float? TryLoadFloat(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return (float)AssertCast<double>(data, key);
		}
		return null;
	}

	public static float LoadFloat(Dictionary<string, object> d, string key)
	{
		AssertKeyExists(d, key);
		return Convert.ToSingle(d[key]);
	}

	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d, float defaultValue)
	{
		v3.x = ((!d.ContainsKey("x")) ? defaultValue : LoadFloat(d, "x"));
		v3.y = ((!d.ContainsKey("y")) ? defaultValue : LoadFloat(d, "y"));
		v3.z = ((!d.ContainsKey("z")) ? defaultValue : LoadFloat(d, "z"));
	}

	public static void SaveVector3(Vector3 v3, string name, Dictionary<string, object> d)
	{
		d[name] = new Dictionary<string, object>
		{
			{ "x", v3.x },
			{ "y", v3.y },
			{ "z", v3.z }
		};
	}

	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d, float defaultValue)
	{
		Assert(!d.ContainsKey("z"), "Don't call LoadVector2 on something that has a z value! (do you want to use LoadVector3?)");
		v2.x = ((!d.ContainsKey("x")) ? defaultValue : LoadFloat(d, "x"));
		v2.y = ((!d.ContainsKey("y")) ? defaultValue : LoadFloat(d, "y"));
	}

	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d)
	{
		LoadVector3(out v3, d, 0f);
	}

	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d)
	{
		LoadVector2(out v2, d, 0f);
	}

	public static Vector3 ExpandVector(Vector2 vector)
	{
		return ExpandVector(vector, 0f);
	}

	public static Vector3 ExpandVector(Vector2 vector, float z)
	{
		return new Vector3(vector.x, vector.y, z);
	}

	public static Vector2 TruncateVector(Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}

	public static void TruncateFile(string filePath)
	{
		DeleteFile(filePath);
		using (FileStream fileStream = File.Create(filePath))
		{
			fileStream.Close();
		}
	}

	public static void DeleteFile(string filePath)
	{
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}

	public static string GetPersistentAssetsPath()
	{
		return Path.Combine(ApplicationPersistentDataPath, "Contents");
	}

	public static string GetStreamingAssetsPath()
	{
		return Path.Combine(ApplicationDataPath, "StreamingAssets");
	}

	public static string GetStreamingAssetsSubfolder(string path)
	{
		return GetStreamingAssetsPath() + Path.DirectorySeparatorChar + path;
	}

	public static string GetStreamingAssetsFileInDirectory(string path, string filename)
	{
		return GetStreamingAssetsFile(path + Path.DirectorySeparatorChar + filename);
	}

	public static string GetStreamingAssetsFile(string fileName)
	{
		string text = GetPersistentAssetsPath() + Path.DirectorySeparatorChar + fileName;
		if (File.Exists(text))
		{
			return text;
		}
		return GetStreamingAssetsPath() + Path.DirectorySeparatorChar + fileName;
	}

	public static string[] GetFilesInPath(string path, string searchPattern)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string path2 = GetPersistentAssetsPath() + Path.DirectorySeparatorChar + path;
		if (Directory.Exists(path2))
		{
		}
		string[] array = new string[dictionary.Count];
		dictionary.Values.CopyTo(array, 0);
		return array;
	}

	public static void DebugDict(Dictionary<string, object> d)
	{
		Debug.Log(DebugDictToString(d));
	}

	public static string DebugDictToString(Dictionary<string, object> d)
	{
		return "[Dictionary Debug View]\n" + PrintDict(d, string.Empty);
	}

	public static string DebugListToString(List<object> l)
	{
		return "[List Debug View]\n" + PrintList(l, string.Empty);
	}

	public static string DebugListToString(List<Vector3> list)
	{
		return DebugListToString(list.ConvertAll((Converter<Vector3, object>)((Vector3 v) => "\t(" + v.x + ",\t" + v.y + ",\t" + v.z + ")")));
	}

	public static string DebugListToString(List<Vector2> list)
	{
		return DebugListToString(list.ConvertAll<Vector3>(ExpandVector));
	}

	private static string PrintDict(Dictionary<string, object> d, string lead)
	{
		if (d == null)
		{
			return "null";
		}
		string text = "{\n";
		foreach (string key in d.Keys)
		{
			if (d[key] != null)
			{
				string text2 = text;
				text = text2 + lead + key + ":" + PrintGenericValue(d[key], lead + " ") + ",\n";
			}
			else
			{
				string text2 = text;
				text = string.Concat(text2, lead, key, ":", d[key], ",\n");
			}
		}
		return text + lead + "}";
	}

	private static string PrintList(List<object> l, string lead)
	{
		if (l == null)
		{
			return "null";
		}
		string text = "[\n";
		for (int i = 0; i < l.Count; i++)
		{
			string text2 = text;
			text = text2 + lead + i + ":" + PrintGenericValue(l[i], lead + " ") + ",\n";
		}
		return text + lead + "]";
	}

	private static string PrintGenericValue(object v, string lead)
	{
		if (v is Dictionary<string, object>)
		{
			return PrintDict(v as Dictionary<string, object>, lead + " ");
		}
		if (v is List<object>)
		{
			return PrintList(v as List<object>, lead + " ");
		}
		if (v == null)
		{
			return "null\n";
		}
		return v.ToString();
	}

	public static void DebugLog(object message)
	{
		Debug.Log(message);
	}

	public static void ErrorLog(object message)
	{
		Debug.LogError(message);
	}

	public static void LogFormat(string format, params object[] args)
	{
		Debug.Log(string.Format(format, args));
	}

	public static void UnexpectedEntry()
	{
		throw new Exception("Unexpected path of code execution! You should not be here!");
	}

	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			throw new Exception(message);
		}
	}

	public static GameObject FindGameObjectInHierarchy(GameObject root, string name)
	{
		if (root.name.Equals(name))
		{
			return root;
		}
		GameObject gameObject = null;
		int childCount = root.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			gameObject = FindGameObjectInHierarchy(root.transform.GetChild(i).gameObject, name);
			if (gameObject != null)
			{
				break;
			}
		}
		return gameObject;
	}

	public static GameObject FindParentGameObjectInHierarchy(GameObject root, string name)
	{
		Transform transform = root.transform;
		while (transform.parent != null)
		{
			if (transform.gameObject.name.Equals(name))
			{
				return transform.gameObject;
			}
			transform = transform.parent;
		}
		return null;
	}

	public static void PlayMovie(string movie)
	{
	}

	public static int BoolToInt(bool myBool)
	{
		if (myBool)
		{
			return 1;
		}
		return 0;
	}

	public static int KontagentCurrencyLevelIndex(int kRange)
	{
		if (kRange > 0 && kRange < 10)
		{
			return 1;
		}
		if (kRange > 10 && kRange < 100)
		{
			return 2;
		}
		if (kRange > 100 && kRange < 1000)
		{
			return 3;
		}
		if (kRange > 1000 && kRange < 10000)
		{
			return 4;
		}
		if (kRange > 10000 && kRange < 100000)
		{
			return 5;
		}
		if (kRange > 100000)
		{
			return 6;
		}
		return 0;
	}

	public static string GetiOSDeviceTypeString()
	{
		return "Unknown";
	}
}
