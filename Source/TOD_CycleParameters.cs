using System;
using UnityEngine;

[Serializable]
public class TOD_CycleParameters
{
	[Tooltip("Current hour of the day.")]
	public float Hour = 12f;

	[Tooltip("Current day of the month.")]
	public int Day = 15;

	[Tooltip("Current month of the year.")]
	public int Month = 6;

	[TOD_Range(1f, 9999f), Tooltip("Current year.")]
	public int Year = 2000;

	public DateTime DateTime
	{
		get
		{
			DateTime dateTime = new DateTime(0L, DateTimeKind.Utc);
			return dateTime.AddYears(this.Year - 1).AddMonths(this.Month - 1).AddDays((double)(this.Day - 1)).AddHours((double)this.Hour);
		}
		set
		{
			this.Year = value.Year;
			this.Month = value.Month;
			this.Day = value.Day;
			this.Hour = (float)value.Hour + (float)value.Minute / 60f + (float)value.Second / 3600f + (float)value.Millisecond / 3600000f;
		}
	}

	public long Ticks
	{
		get
		{
			return this.DateTime.Ticks;
		}
		set
		{
			this.DateTime = new DateTime(value, DateTimeKind.Utc);
		}
	}
}
