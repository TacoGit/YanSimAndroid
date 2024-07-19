using System;
using UnityEngine;

public class HomeClockScript : MonoBehaviour
{
	public UILabel HourLabel;

	public UILabel DayLabel;

	private void Start()
	{
		DayLabel.text = GetWeekdayText(DateGlobals.Weekday);
		if (HomeGlobals.Night)
		{
			HourLabel.text = "8:00 PM";
		}
		else
		{
			HourLabel.text = ((!HomeGlobals.LateForSchool) ? "6:30 AM" : "7:30 AM");
		}
	}

	private string GetWeekdayText(DayOfWeek weekday)
	{
		switch (weekday)
		{
		case DayOfWeek.Sunday:
			return "SUNDAY";
		case DayOfWeek.Monday:
			return "MONDAY";
		case DayOfWeek.Tuesday:
			return "TUESDAY";
		case DayOfWeek.Wednesday:
			return "WEDNESDAY";
		case DayOfWeek.Thursday:
			return "THURSDAY";
		case DayOfWeek.Friday:
			return "FRIDAY";
		default:
			return "SATURDAY";
		}
	}
}
