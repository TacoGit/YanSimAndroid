using UnityEngine;

public static class FVectorMethods
{
	public static Vector3 RandomVector(float rangeA, float rangeB)
	{
		return new Vector3(Random.Range(rangeA, rangeB), Random.Range(rangeA, rangeB), Random.Range(rangeA, rangeB));
	}

	public static float VectorSum(Vector3 vector)
	{
		return vector.x + vector.y + vector.z;
	}

	public static Vector3 RandomVectorNoY(float rangeA, float rangeB)
	{
		return new Vector3(Random.Range(rangeA, rangeB), 0f, Random.Range(rangeA, rangeB));
	}

	public static Vector3 RandomVectorMinMax(float min, float max)
	{
		float num = 1f;
		if (Random.Range(0, 2) == 1)
		{
			num = -1f;
		}
		float num2 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			num2 = -1f;
		}
		float num3 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			num3 = -1f;
		}
		return new Vector3(Random.Range(min, max) * num, Random.Range(min, max) * num2, Random.Range(min, max) * num3);
	}

	public static Vector3 RandomVectorNoYMinMax(float min, float max)
	{
		float num = 1f;
		if (Random.Range(0, 2) == 1)
		{
			num = -1f;
		}
		float num2 = 1f;
		if (Random.Range(0, 2) == 1)
		{
			num2 = -1f;
		}
		return new Vector3(Random.Range(min, max) * num, 0f, Random.Range(min, max) * num2);
	}

	public static Vector3 GetUIPositionFromWorldPosition(Vector3 position, Camera camera, RectTransform canvas)
	{
		Vector3 result = camera.WorldToViewportPoint(position);
		result.x *= canvas.sizeDelta.x;
		result.y *= canvas.sizeDelta.y;
		result.x -= canvas.sizeDelta.x * canvas.pivot.x;
		result.y -= canvas.sizeDelta.y * canvas.pivot.y;
		return result;
	}

	public static Vector3 PosFromMatrix(Matrix4x4 m)
	{
		return m.GetColumn(3);
	}

	public static Quaternion RotFromMatrix(Matrix4x4 m)
	{
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}

	public static Vector3 ScaleFromMatrix(Matrix4x4 m)
	{
		return new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
	}
}
