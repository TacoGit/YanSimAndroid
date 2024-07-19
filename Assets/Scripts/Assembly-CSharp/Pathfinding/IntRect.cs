using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	public struct IntRect
	{
		public int xmin;

		public int ymin;

		public int xmax;

		public int ymax;

		private static readonly int[] Rotations = new int[16]
		{
			1, 0, 0, 1, 0, 1, -1, 0, -1, 0,
			0, -1, 0, -1, 1, 0
		};

		public int Width
		{
			get
			{
				return xmax - xmin + 1;
			}
		}

		public int Height
		{
			get
			{
				return ymax - ymin + 1;
			}
		}

		public IntRect(int xmin, int ymin, int xmax, int ymax)
		{
			this.xmin = xmin;
			this.xmax = xmax;
			this.ymin = ymin;
			this.ymax = ymax;
		}

		public bool Contains(int x, int y)
		{
			return x >= xmin && y >= ymin && x <= xmax && y <= ymax;
		}

		public bool IsValid()
		{
			return xmin <= xmax && ymin <= ymax;
		}

		public static bool operator ==(IntRect a, IntRect b)
		{
			return a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin && a.ymax == b.ymax;
		}

		public static bool operator !=(IntRect a, IntRect b)
		{
			return a.xmin != b.xmin || a.xmax != b.xmax || a.ymin != b.ymin || a.ymax != b.ymax;
		}

		public override bool Equals(object obj)
		{
			IntRect intRect = (IntRect)obj;
			return xmin == intRect.xmin && xmax == intRect.xmax && ymin == intRect.ymin && ymax == intRect.ymax;
		}

		public override int GetHashCode()
		{
			return (xmin * 131071) ^ (xmax * 3571) ^ (ymin * 3109) ^ (ymax * 7);
		}

		public static IntRect Intersection(IntRect a, IntRect b)
		{
			return new IntRect(Math.Max(a.xmin, b.xmin), Math.Max(a.ymin, b.ymin), Math.Min(a.xmax, b.xmax), Math.Min(a.ymax, b.ymax));
		}

		public static bool Intersects(IntRect a, IntRect b)
		{
			return a.xmin <= b.xmax && a.ymin <= b.ymax && a.xmax >= b.xmin && a.ymax >= b.ymin;
		}

		public static IntRect Union(IntRect a, IntRect b)
		{
			return new IntRect(Math.Min(a.xmin, b.xmin), Math.Min(a.ymin, b.ymin), Math.Max(a.xmax, b.xmax), Math.Max(a.ymax, b.ymax));
		}

		public IntRect ExpandToContain(int x, int y)
		{
			return new IntRect(Math.Min(xmin, x), Math.Min(ymin, y), Math.Max(xmax, x), Math.Max(ymax, y));
		}

		public IntRect Expand(int range)
		{
			return new IntRect(xmin - range, ymin - range, xmax + range, ymax + range);
		}

		public IntRect Rotate(int r)
		{
			int num = Rotations[r * 4];
			int num2 = Rotations[r * 4 + 1];
			int num3 = Rotations[r * 4 + 2];
			int num4 = Rotations[r * 4 + 3];
			int val = num * xmin + num2 * ymin;
			int val2 = num3 * xmin + num4 * ymin;
			int val3 = num * xmax + num2 * ymax;
			int val4 = num3 * xmax + num4 * ymax;
			return new IntRect(Math.Min(val, val3), Math.Min(val2, val4), Math.Max(val, val3), Math.Max(val2, val4));
		}

		public IntRect Offset(Int2 offset)
		{
			return new IntRect(xmin + offset.x, ymin + offset.y, xmax + offset.x, ymax + offset.y);
		}

		public IntRect Offset(int x, int y)
		{
			return new IntRect(xmin + x, ymin + y, xmax + x, ymax + y);
		}

		public override string ToString()
		{
			return "[x: " + xmin + "..." + xmax + ", y: " + ymin + "..." + ymax + "]";
		}

		public void DebugDraw(GraphTransform transform, Color color)
		{
			Vector3 vector = transform.Transform(new Vector3(xmin, 0f, ymin));
			Vector3 vector2 = transform.Transform(new Vector3(xmin, 0f, ymax));
			Vector3 vector3 = transform.Transform(new Vector3(xmax, 0f, ymax));
			Vector3 vector4 = transform.Transform(new Vector3(xmax, 0f, ymin));
			Debug.DrawLine(vector, vector2, color);
			Debug.DrawLine(vector2, vector3, color);
			Debug.DrawLine(vector3, vector4, color);
			Debug.DrawLine(vector4, vector, color);
		}
	}
}
