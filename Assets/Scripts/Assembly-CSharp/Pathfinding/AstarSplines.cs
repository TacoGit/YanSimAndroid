using System;
using UnityEngine;

namespace Pathfinding
{
	internal static class AstarSplines
	{
		public static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
		{
			float num = elapsedTime * elapsedTime;
			float num2 = num * elapsedTime;
			return previous * (-0.5f * num2 + num - 0.5f * elapsedTime) + start * (1.5f * num2 + -2.5f * num + 1f) + end * (-1.5f * num2 + 2f * num + 0.5f * elapsedTime) + next * (0.5f * num2 - 0.5f * num);
		}

		[Obsolete("Use CatmullRom")]
		public static Vector3 CatmullRomOLD(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
		{
			return CatmullRom(previous, start, end, next, elapsedTime);
		}

		public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
		}

		public static Vector3 CubicBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return 3f * num * num * (p1 - p0) + 6f * num * t * (p2 - p1) + 3f * t * t * (p3 - p2);
		}

		public static Vector3 CubicBezierSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return 6f * num * (p2 - 2f * p1 + p0) + 6f * t * (p3 - 2f * p2 + p1);
		}
	}
}
