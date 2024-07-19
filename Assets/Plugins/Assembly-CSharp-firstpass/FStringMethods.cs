public static class FStringMethods
{
	public static string IntToString(int value, int signs)
	{
		string text = value.ToString();
		int num = signs - text.Length;
		if (num > 0)
		{
			string text2 = "0";
			for (int i = 1; i < num; i++)
			{
				text2 += 0;
			}
			text = text2 + text;
		}
		return text;
	}

	public static string CapitalizeOnlyFirstLetter(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text[0].ToString().ToUpper() + ((text.Length <= 1) ? string.Empty : text.Substring(1));
	}

	public static string CapitalizeFirstLetter(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		return text[0].ToString().ToUpper() + text.Substring(1);
	}

	public static string ReplaceSpacesWithUnderline(string text)
	{
		if (text.Contains(" "))
		{
			text = text.Replace(" ", "_");
		}
		return text;
	}

	public static string GetEndOfStringFromSeparator(string source, char[] separators, int which = 1, bool fromEnd = false)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (num3 = source.Length - 1; num3 >= 0; num3--)
		{
			num2++;
			for (int i = 0; i < separators.Length; i++)
			{
				if (source[num3] == separators[i])
				{
					num++;
					if (num == which)
					{
						num3++;
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			if (!fromEnd)
			{
				return source.Substring(0, source.Length - num2);
			}
			return source.Substring(num3, source.Length - num3);
		}
		return string.Empty;
	}

	public static string GetEndOfStringFromStringSeparator(string source, string[] separators, int which = 1, bool rest = false)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (num3 = 0; num3 < source.Length; num3++)
		{
			num2++;
			for (int i = 0; i < separators.Length && num3 + separators[i].Length <= source.Length; i++)
			{
				if (source.Substring(num3, separators[i].Length) == separators[i])
				{
					num++;
					if (num == which)
					{
						num3++;
						num3 += separators[i].Length - 1;
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			if (rest)
			{
				return source.Substring(0, source.Length - num2);
			}
			return source.Substring(num3, source.Length - num3);
		}
		return string.Empty;
	}
}
