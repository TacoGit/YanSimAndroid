using System;
using UnityEngine;

public static class FEasing
{
	public enum EFease
	{
		EaseInCubic = 0,
		EaseOutCubic = 1,
		EaseInOutCubic = 2,
		EaseInOutElastic = 3,
		EaseInElastic = 4,
		EaseOutElastic = 5,
		EaseInExpo = 6,
		EaseOutExpo = 7,
		EaseInOutExpo = 8,
		Linear = 9
	}

	public delegate float Function(float s, float e, float v, float extraParameter = 1f);

	public static float EaseInCubic(float start, float end, float value, float ignore = 1f)
	{
		end -= start;
		return end * value * value * value + start;
	}

	public static float EaseOutCubic(float start, float end, float value, float ignore = 1f)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	public static float EaseInOutCubic(float start, float end, float value, float ignore = 1f)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	public static float EaseOutElastic(float start, float end, float value, float rangeMul = 1f)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f * rangeMul;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f * rangeMul;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value * rangeMul) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2) + end + start;
	}

	public static float EaseInElastic(float start, float end, float value, float rangeMul = 1f)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f * rangeMul;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f * rangeMul;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		return 0f - num3 * Mathf.Pow(2f, 10f * rangeMul * (value -= 1f)) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2) + start;
	}

	public static float EaseInOutElastic(float start, float end, float value, float rangeMul = 1f)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f * rangeMul;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f * rangeMul;
		}
		else
		{
			num4 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * rangeMul * (value -= 1f)) * Mathf.Sin((value * num - num4) * ((float)Math.PI * 2f) / num2) * 0.5f + end + start;
	}

	public static float EaseInExpo(float start, float end, float value, float ignore = 1f)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	public static float EaseOutExpo(float start, float end, float value, float ignore = 1f)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	public static float EaseInOutExpo(float start, float end, float value, float ignore = 1f)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	public static float Linear(float start, float end, float value, float ignore = 1f)
	{
		return Mathf.Lerp(start, end, value);
	}

	public static Function GetEasingFunction(EFease easingFunction)
	{
		switch (easingFunction)
		{
		case EFease.EaseInCubic:
			return EaseInCubic;
		case EFease.EaseOutCubic:
			return EaseOutCubic;
		case EFease.EaseInOutCubic:
			return EaseInOutCubic;
		case EFease.EaseInElastic:
			return EaseInElastic;
		case EFease.EaseOutElastic:
			return EaseOutElastic;
		case EFease.EaseInOutElastic:
			return EaseInOutElastic;
		case EFease.EaseInExpo:
			return EaseInExpo;
		case EFease.EaseOutExpo:
			return EaseOutExpo;
		case EFease.EaseInOutExpo:
			return EaseInOutExpo;
		case EFease.Linear:
			return Linear;
		default:
			return null;
		}
	}
}
