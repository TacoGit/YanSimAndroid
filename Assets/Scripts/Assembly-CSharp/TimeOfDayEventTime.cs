using System;
using UnityEngine;

[Serializable]
public class TimeOfDayEventTime : IScheduledEventTime
{
	[SerializeField]
	private int week;

	[SerializeField]
	private DayOfWeek weekday;

	[SerializeField]
	private TimeOfDay timeOfDay;

	public ScheduledEventTimeType ScheduleType
	{
		get
		{
			return ScheduledEventTimeType.TimeOfDay;
		}
	}

	public TimeOfDayEventTime(int week, DayOfWeek weekday, TimeOfDay timeOfDay)
	{
		this.week = week;
		this.weekday = weekday;
		this.timeOfDay = timeOfDay;
	}

	public bool OccurringNow(DateAndTime currentTime)
	{
		bool flag = currentTime.Week == week;
		bool flag2 = currentTime.Weekday == weekday;
		bool flag3 = currentTime.Clock.TimeOfDay == timeOfDay;
		return flag && flag2 && flag3;
	}

	public bool OccursInTheFuture(DateAndTime currentTime)
	{
		if (currentTime.Week == week)
		{
			if (currentTime.Weekday == weekday)
			{
				return currentTime.Clock.TimeOfDay < timeOfDay;
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
				return currentTime.Clock.TimeOfDay > timeOfDay;
			}
			return currentTime.Weekday > weekday;
		}
		return currentTime.Week > week;
	}
}
