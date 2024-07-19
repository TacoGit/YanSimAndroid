using System;
using UnityEngine;

namespace Pathfinding.Util
{
	public class Draw
	{
		public static readonly Draw Debug = new Draw
		{
			gizmos = false
		};

		public static readonly Draw Gizmos = new Draw
		{
			gizmos = true
		};

		private bool gizmos;

		private Matrix4x4 matrix = Matrix4x4.identity;

		private void SetColor(Color color)
		{
			if (gizmos && UnityEngine.Gizmos.color != color)
			{
				UnityEngine.Gizmos.color = color;
			}
		}

		public void Line(Vector3 a, Vector3 b, Color color)
		{
			SetColor(color);
			if (gizmos)
			{
				UnityEngine.Gizmos.DrawLine(matrix.MultiplyPoint3x4(a), matrix.MultiplyPoint3x4(b));
			}
			else
			{
				UnityEngine.Debug.DrawLine(matrix.MultiplyPoint3x4(a), matrix.MultiplyPoint3x4(b), color);
			}
		}

		public void CircleXZ(Vector3 center, float radius, Color color, float startAngle = 0f, float endAngle = (float)Math.PI * 2f)
		{
			int num = 40;
			while (startAngle > endAngle)
			{
				startAngle -= (float)Math.PI * 2f;
			}
			Vector3 vector = new Vector3(Mathf.Cos(startAngle) * radius, 0f, Mathf.Sin(startAngle) * radius);
			for (int i = 0; i <= num; i++)
			{
				Vector3 vector2 = new Vector3(Mathf.Cos(Mathf.Lerp(startAngle, endAngle, (float)i / (float)num)) * radius, 0f, Mathf.Sin(Mathf.Lerp(startAngle, endAngle, (float)i / (float)num)) * radius);
				Line(center + vector, center + vector2, color);
				vector = vector2;
			}
		}

		public void Cylinder(Vector3 position, Vector3 up, float height, float radius, Color color)
		{
			Vector3 normalized = Vector3.Cross(up, Vector3.one).normalized;
			matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(normalized, up), new Vector3(radius, height, radius));
			CircleXZ(Vector3.zero, 1f, color);
			if (height > 0f)
			{
				CircleXZ(Vector3.up, 1f, color);
				Line(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), color);
				Line(new Vector3(-1f, 0f, 0f), new Vector3(-1f, 1f, 0f), color);
				Line(new Vector3(0f, 0f, 1f), new Vector3(0f, 1f, 1f), color);
				Line(new Vector3(0f, 0f, -1f), new Vector3(0f, 1f, -1f), color);
			}
			matrix = Matrix4x4.identity;
		}

		public void CrossXZ(Vector3 position, Color color, float size = 1f)
		{
			size *= 0.5f;
			Line(position - Vector3.right * size, position + Vector3.right * size, color);
			Line(position - Vector3.forward * size, position + Vector3.forward * size, color);
		}

		public void Bezier(Vector3 a, Vector3 b, Color color)
		{
			Vector3 vector = b - a;
			if (!(vector == Vector3.zero))
			{
				Vector3 rhs = Vector3.Cross(Vector3.up, vector);
				Vector3 normalized = Vector3.Cross(vector, rhs).normalized;
				normalized *= vector.magnitude * 0.1f;
				Vector3 p = a + normalized;
				Vector3 p2 = b + normalized;
				Vector3 a2 = a;
				for (int i = 1; i <= 20; i++)
				{
					float t = (float)i / 20f;
					Vector3 vector2 = AstarSplines.CubicBezier(a, p, p2, b, t);
					Line(a2, vector2, color);
					a2 = vector2;
				}
			}
		}
	}
}
