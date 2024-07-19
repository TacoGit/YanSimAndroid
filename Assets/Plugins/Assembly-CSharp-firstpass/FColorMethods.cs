using System.Globalization;
using UnityEngine;

public static class FColorMethods
{
	public static Color ChangeColorAlpha(Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static Color ChangeColorsValue(Color color, float brightenOrDarken = 0f)
	{
		return new Color(color.r + brightenOrDarken, color.g + brightenOrDarken, color.b + brightenOrDarken, color.a);
	}

	public static Color32 HexToColor(string hex)
	{
		if (string.IsNullOrEmpty(hex))
		{
			FDebug.LogRed("Trying convert from hex to color empty string!");
			return Color.white;
		}
		uint result = 255u;
		hex = hex.Replace("#", string.Empty);
		hex = hex.Replace("0x", string.Empty);
		if (!uint.TryParse(hex, NumberStyles.HexNumber, null, out result))
		{
			Debug.Log("Error during converting hex string.");
			return Color.white;
		}
		return new Color32((byte)((result & -16777216) >> 24), (byte)((result & 0xFF0000) >> 16), (byte)((result & 0xFF00) >> 8), (byte)(result & 0xFFu));
	}

	public static string ColorToHex(Color32 color, bool addHash = true)
	{
		string text = string.Empty;
		if (addHash)
		{
			text = "#";
		}
		return text + string.Format("{0}{1}{2}{3}", (color.r.ToString("X").Length != 1) ? color.r.ToString("X") : string.Format("0{0}", color.r.ToString("X")), (color.g.ToString("X").Length != 1) ? color.g.ToString("X") : string.Format("0{0}", color.g.ToString("X")), (color.b.ToString("X").Length != 1) ? color.b.ToString("X") : string.Format("0{0}", color.b.ToString("X")), (color.a.ToString("X").Length != 1) ? color.a.ToString("X") : string.Format("0{0}", color.a.ToString("X")));
	}

	public static string ColorToHex(Color color, bool addHash = true)
	{
		Color32 color2 = new Color32((byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), (byte)(color.a * 255f));
		return ColorToHex(color2, addHash);
	}

	public static void LerpMaterialColor(Material mat, string property, Color targetColor, float deltaMultiplier = 8f)
	{
		if (!(mat == null))
		{
			if (!mat.HasProperty(property))
			{
				Debug.LogError("Material " + mat.name + " don't have property '" + property + "'  in shader " + mat.shader.name);
			}
			else
			{
				Color color = mat.GetColor(property);
				mat.SetColor(property, Color.Lerp(color, targetColor, Time.deltaTime * deltaMultiplier));
			}
		}
	}
}
