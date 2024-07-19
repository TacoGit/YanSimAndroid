using System;
using UnityEngine;

[Serializable]
public class SpecificEventTime : IScheduledEventTime
{
	[SerializeField]
	private int week;

	[SerializeField]
	private DayOfWeek weekday;

	[SerializeField]
	private Clock startClock;

	[SerializeField]
	private Clock endClock;

	public ScheduledEventTimeType ScheduleType
	{
		get
		{
			return ScheduledEventTimeType.Specific;
		}
	}

	public SpecificEventTime(int week, DayOfWeek weekday, Clock startClock, Clock endClock)
	{
		this.week = week;
		this.weekday = weekday;
		this.startClock = startClock;
		this.endClock = endClock;
	}

	public bool OccurringNow(DateAndTime currentTime)
	{
		bool flag = currentTime.Week == week;
		bool flag2 = currentTime.Weekday == weekday;
		Clock clock = currentTime.Clock;
		bool flag3 = clock.TotalSeconds >= startClock.TotalSeconds && clock.TotalSeconds < endClock.TotalSeconds;
		return flag && flag2 && flag3;
	}

	public bool OccursInTheFuture(DateAndTime currentTime)
	{
		if (currentTime.Week == week)
		{
			if (currentTime.Weekday == weekday)
			{
				return currentTime.Clock.TotalSeconds < startClock.TotalSeconds;
			}
			return currentTime.Weekday < weekday;
		}
		return currentTime.Week < week;
	}

	public bool OccurredInThePast(DateAndTime currentTime)
	{
		if (currentTime.Week == week)
		{
			if (currentTime.Weekday == weekday)
			{
				return currentTime.Clock.TotalSeconds >= endClock.TotalSeconds;
			}
			return currentTime.Weekday > weekday;
		}
		return currentTime.Week > week;
	}
}
