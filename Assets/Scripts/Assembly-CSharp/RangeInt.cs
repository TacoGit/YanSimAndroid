using System;
using UnityEngine;

[Serializable]
public class RangeInt
{
	[SerializeField]
	private int value;

	[SerializeField]
	private int min;

	[SerializeField]
	private int max;

	public int Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public int Min
	{
		get
		{
			return min;
		}
	}

	public int Max
	{
		get
		{
			return max;
		}
	}

	public int Next
	{
		get
		{
			return (value != max) ? (value + 1) : min;
		}
	}

	public int Previous
	{
		get
		{
			return (value != min) ? (value - 1) : max;
		}
	}

	public RangeInt(int value, int min, int max)
	{
		this.value = value;
		this.min = min;
		this.max = max;
	}

	public RangeInt(int min, int max)
		: this(min, min, max)
	{
	}
}
