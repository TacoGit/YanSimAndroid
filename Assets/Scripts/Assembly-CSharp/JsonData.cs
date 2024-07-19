using System.Collections.Generic;
using System.IO;
using JsonFx.Json;
using UnityEngine;

public abstract class JsonData
{
	protected static string FolderPath
	{
		get
		{
			return Path.Combine(Application.streamingAssetsPath, "JSON");
		}
	}

	protected static Dictionary<string, object>[] Deserialize(string filename)
	{
		string value = File.ReadAllText(filename);
		return JsonReader.Deserialize<Dictionary<string, object>[]>(value);
	}
}
