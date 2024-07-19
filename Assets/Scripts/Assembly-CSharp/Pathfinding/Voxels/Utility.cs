using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Voxels
{
	public class Utility
	{
		public static float Min(float a, float b, float c)
		{
			a = ((!(a < b)) ? b : a);
			return (!(a < c)) ? c : a;
		}

		public static float Max(float a, float b, float c)
		{
			a = ((!(a > b)) ? b : a);
			return (!(a > c)) ? c : a;
		}

		public static int Max(int a, int b, int c, int d)
		{
			a = ((a <= b) ? b : a);
			a = ((a <= c) ? c : a);
			return (a <= d) ? d : a;
		}

		public static int Min(int a, int b, int c, int d)
		{
			a = ((a >= b) ? b : a);
			a = ((a >= c) ? c : a);
			return (a >= d) ? d : a;
		}

		public static float Max(float a, float b, float c, float d)
		{
			a = ((!(a > b)) ? b : a);
			a = ((!(a > c)) ? c : a);
			return (!(a > d)) ? d : a;
		}

		public static float Min(float a, float b, float c, float d)
		{
			a = ((!(a < b)) ? b : a);
			a = ((!(a < c)) ? c : a);
			return (!(a < d)) ? d : a;
		}

		public static void CopyVector(float[] a, int i, Vector3 v)
		{
			a[i] = v.x;
			a[i + 1] = v.y;
			a[i + 2] = v.z;
		}

		public static Int3[] RemoveDuplicateVertices(Int3[] vertices, int[] triangles)
		{
			Dictionary<Int3, int> obj = ObjectPoolSimple<Dictionary<Int3, int>>.Claim();
			obj.Clear();
			int[] array = new int[vertices.Length];
			int num = 0;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (!obj.ContainsKey(vertices[i]))
				{
					obj.Add(vertices[i], num);
					array[i] = num;
					vertices[num] = vertices[i];
					num++;
				}
				else
				{
					array[i] = obj[vertices[i]];
				}
			}
			obj.Clear();
			ObjectPoolSimple<Dictionary<Int3, int>>.Release(ref obj);
			for (int j = 0; j < triangles.Length; j++)
			{
				triangles[j] = array[triangles[j]];
			}
			Int3[] array2 = new Int3[num];
			for (int k = 0; k < num; k++)
			{
				array2[k] = vertices[k];
			}
			return array2;
		}
	}
}
