using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.Basics
{
	public class FBasic_NotRepetiveSelector<T>
	{
		private int backRange;

		private List<int> previousElements = new List<int>();

		private List<T> elements;

		private int added;

		public FBasic_NotRepetiveSelector(List<T> elements, int backRange)
		{
			this.elements = elements;
			this.backRange = backRange;
			if (backRange > 0 && elements.Count > 1)
			{
				if (backRange > elements.Count - 1)
				{
					backRange = Mathf.Max(1, elements.Count / 2);
				}
				for (int i = 0; i < backRange; i++)
				{
					previousElements.Add(-1);
				}
			}
			else
			{
				backRange = 0;
			}
			added = 0;
		}

		public static List<T> ArrayToList(T[] elements)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < elements.Length; i++)
			{
				list.Add(elements[i]);
			}
			return list;
		}

		public T GetElementNotRepetive()
		{
			if (backRange < 1)
			{
				return elements[Random.Range(0, elements.Count)];
			}
			int num = ChooseElementDontRepeat(elements, previousElements, backRange);
			T result = elements[num];
			previousElements[added] = num;
			added++;
			if (added > previousElements.Count - 1)
			{
				added = 0;
			}
			return result;
		}

		private int ChooseElementDontRepeat(List<T> elements, List<int> previousClips, int backCount)
		{
			int num = Random.Range(0, elements.Count);
			if (backCount > elements.Count - 1)
			{
				Debug.Log("Back Count too big for given array!");
				return num;
			}
			bool flag = false;
			for (int i = 0; i < backCount; i++)
			{
				if (previousClips[i] == num)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return ChooseElementDontRepeat(elements, previousClips, backCount);
			}
			return num;
		}
	}
}
