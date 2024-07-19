using System;
using UnityEngine;

[Serializable]
public class DateAndTime
{
	[SerializeField]
	private int week;

	[SerializeField]
	private DayOfWeek weekday;

	[SerializeField]
	private Clock clock;

	public int Week
	{
		get
		{
			return week;
		}
	}

	public DayOfWeek Weekday
	{
		get
		{
			return weekday;
		}
	}

	public Clock Clock
	{
		get
		{
			return clock;
		}
	}

	public int TotalSeconds
	{
		get
		{
			int num = week * 604800;
			int num2 = (int)weekday * 86400;
			int totalSeconds = clock.TotalSeconds;
			return num + num2 + totalSeconds;
		}
	}

	public DateAndTime(int week, DayOfWeek weekday, Clock clock)
	{
		this.week = week;
		this.weekday = weekday;
		this.clock = clock;
	}

	public void IncrementWeek()
	{
		week++;
	}

	public void IncrementWeekday()
	{
		int num = (int)weekday;
		num++;
		if (num == 7)
		{
			IncrementWeek();
			num = 0;
		}
		weekday = (DayOfWeek)num;
	}

	public void Tick(float dt)
	{
		int hours = clock.Hours24;
		clock.Tick(dt);
		int hours2 = clock.Hours24;
		if (hours2 < hours)
		{
			IncrementWeekday();
		}
	}
}
