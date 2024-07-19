using UnityEngine;

public static class FLogicMethods
{
	public static float FLerp(float a, float b, float t, float factor = 0.01f)
	{
		float num = b;
		b = ((!(num > a)) ? (b - factor) : (b + factor));
		float num2 = Mathf.Lerp(a, b, t);
		if (num > a)
		{
			if (num2 >= num)
			{
				return num;
			}
		}
		else if (num2 <= num)
		{
			return num;
		}
		return num2;
	}

	public static float FAbs(float value)
	{
		if (value < 0f)
		{
			value = 0f - value;
		}
		return value;
	}

	public static float TopDownDistanceManhattan(Vector3 a, Vector3 b)
	{
		float num = 0f;
		num += FAbs(a.x - b.x);
		return num + FAbs(a.z - b.z);
	}

	public static float DistanceManhattan(Vector3 a, Vector3 b)
	{
		float num = 0f;
		num += FAbs(a.x - b.x);
		num += FAbs(a.y - b.y);
		return num + FAbs(a.z - b.z);
	}

	public static float WrapAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f)
		{
			return angle - 360f;
		}
		return angle;
	}

	public static Vector3 WrapVector(Vector3 angles)
	{
		return new Vector3(WrapAngle(angles.x), WrapAngle(angles.y), WrapAngle(angles.z));
	}

	public static float UnwrapAngle(float angle)
	{
		if (angle >= 0f)
		{
			return angle;
		}
		angle = (0f - angle) % 360f;
		return 360f - angle;
	}

	public static Vector3 UnwrapVector(Vector3 angles)
	{
		return new Vector3(UnwrapAngle(angles.x), UnwrapAngle(angles.y), UnwrapAngle(angles.z));
	}

	public static bool IsAlmostEqual(float val, float to, int afterComma = 2, float addRange = 0f)
	{
		float num = 1f / Mathf.Pow(10f, afterComma) + addRange;
		if ((val > to - num && val < to + num) || val == to)
		{
			return true;
		}
		return false;
	}

	public static Quaternion TopDownAngle(Vector3 from, Vector3 to)
	{
		from.y = 0f;
		to.y = 0f;
		return Quaternion.LookRotation(to - from);
	}

	public static Quaternion TopDownAnglePosition2D(Vector2 from, Vector2 to, float offset = 0f)
	{
		Vector2 vector = to - from;
		float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f + offset;
		return Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
