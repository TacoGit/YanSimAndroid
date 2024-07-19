using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	public static class ArrayPool<T>
	{
		private const int MaximumExactArrayLength = 256;

		private static readonly Stack<T[]>[] pool = new Stack<T[]>[31];

		private static readonly Stack<T[]>[] exactPool = new Stack<T[]>[257];

		private static readonly HashSet<T[]> inPool = new HashSet<T[]>();

		public static T[] Claim(int minimumLength)
		{
			if (minimumLength <= 0)
			{
				return ClaimWithExactLength(0);
			}
			int i;
			for (i = 0; 1 << i < minimumLength && i < 30; i++)
			{
			}
			if (i == 30)
			{
				throw new ArgumentException("Too high minimum length");
			}
			lock (pool)
			{
				if (pool[i] == null)
				{
					pool[i] = new Stack<T[]>();
				}
				if (pool[i].Count > 0)
				{
					T[] array = pool[i].Pop();
					inPool.Remove(array);
					return array;
				}
			}
			return new T[1 << i];
		}

		public static T[] ClaimWithExactLength(int length)
		{
			if (length != 0 && (length & (length - 1)) == 0)
			{
				return Claim(length);
			}
			if (length <= 256)
			{
				lock (pool)
				{
					Stack<T[]> stack = exactPool[length];
					if (stack != null && stack.Count > 0)
					{
						return stack.Pop();
					}
				}
			}
			return new T[length];
		}

		public static void Release(ref T[] array, bool allowNonPowerOfTwo = false)
		{
			if (array == null)
			{
				return;
			}
			if (array.GetType() != typeof(T[]))
			{
				throw new ArgumentException("Expected array type " + typeof(T[]).Name + " but found " + array.GetType().Name + "\nAre you using the correct generic class?\n");
			}
			bool flag = array.Length != 0 && (array.Length & (array.Length - 1)) == 0;
			if (!flag && !allowNonPowerOfTwo && array.Length != 0)
			{
				throw new ArgumentException("Length is not a power of 2");
			}
			lock (pool)
			{
				if (flag)
				{
					int i;
					for (i = 0; 1 << i < array.Length && i < 30; i++)
					{
					}
					if (pool[i] == null)
					{
						pool[i] = new Stack<T[]>();
					}
					pool[i].Push(array);
				}
				else if (array.Length <= 256)
				{
					Stack<T[]> stack = exactPool[array.Length];
					if (stack == null)
					{
						stack = (exactPool[array.Length] = new Stack<T[]>());
					}
					stack.Push(array);
				}
			}
			array = null;
		}
	}
}
