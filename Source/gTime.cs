using gWeatherFunctions;
using System;
using UnityEngine;

public class gTime : MonoBehaviour
{
	public float fFrameTime;

	public float fNewFrameTime;

	public float fTimeSaveTimer;

	public bool bTimeProgress = true;

	public int nPersistentTime = 1;

	public int nMPD = 30;

	public int nTimeInitialized;

	public float fTimeSave = 30f;

	public float fDayLengthMinutes = 30f;

	public float fDayLengthSeconds;

	public int nYearCurrent = 1;

	public string sYearCurrent = string.Empty;

	public int nMonthCurrent = 1;

	public string sMonthCurrent = string.Empty;

	public int nDayCurrent = 1;

	public string sDayCurrent = string.Empty;

	public int nDayOfWeek = 1;

	public string sDayOfWeek = string.Empty;

	public int nDayStart = 6;

	public int nDayEnd = 18;

	public int nHourCurrent;

	public float fHourCurrent;

	public string sHourCurrent = string.Empty;

	public int nMinuteCurrent;

	public string sMinuteCurrent = string.Empty;

	public float fSecondCurrent;

	public string sTimeCurrent = string.Empty;

	public string sMonth1 = "January ";

	public string sMonth2 = "February ";

	public string sMonth3 = "March ";

	public string sMonth4 = "April ";

	public string sMonth5 = "May ";

	public string sMonth6 = "June ";

	public string sMonth7 = "July ";

	public string sMonth8 = "August ";

	public string sMonth9 = "September ";

	public string sMonth10 = "October ";

	public string sMonth11 = "November ";

	public string sMonth12 = "December ";

	public string sDay1 = "Sunday ";

	public string sDay2 = "Monday ";

	public string sDay3 = "Tuesday ";

	public string sDay4 = "Wednesday ";

	public string sDay5 = "Thursday ";

	public string sDay6 = "Friday ";

	public string sDay7 = "Saturday ";

	private void Update()
	{
		this.fTimeSaveTimer += Time.deltaTime;
		this.fFrameTime = Time.deltaTime;
		if (this.nMPD < 1)
		{
			this.nMPD = 1;
		}
		if (this.nPersistentTime == 1 && this.nTimeInitialized == 0)
		{
			this.nTimeInitialized = 1;
		}
		if (this.nPersistentTime == 1 && this.nTimeInitialized == 1 && this.fTimeSaveTimer >= this.fTimeSave)
		{
			this.fTimeSaveTimer = 0f;
		}
		this.fDayLengthSeconds = (float)this.nMPD * 60f;
		if (this.bTimeProgress)
		{
			this.fNewFrameTime = this.GetNewFrameTime(this.fFrameTime);
			this.fSecondCurrent += this.fNewFrameTime;
			this.fHourCurrent = this.GetHoursToDecimal(this.nHourCurrent, this.nMinuteCurrent, this.fSecondCurrent);
		}
		if (this.fSecondCurrent >= 60f)
		{
			this.nMinuteCurrent++;
			this.fSecondCurrent -= 60f;
		}
		if (this.nMinuteCurrent > 59)
		{
			this.nHourCurrent++;
			this.nMinuteCurrent = 0;
		}
		if (this.nHourCurrent > 23)
		{
			this.nDayCurrent++;
			this.nHourCurrent = 0;
		}
		if (this.nDayCurrent > 28)
		{
			this.nMonthCurrent++;
			this.nDayCurrent = 1;
		}
		if (this.nMonthCurrent > 12)
		{
			this.nYearCurrent++;
			this.nMonthCurrent = 1;
		}
		this.sMinuteCurrent = this.GetMinuteString();
		this.sHourCurrent = this.GetHourString();
		this.nDayOfWeek = this.GetCalendarDayOfWeek();
		this.sDayOfWeek = this.GetDayOfWeekString();
		this.sDayCurrent = this.GetDayString();
		this.sMonthCurrent = this.GetDayOfMonthString();
		this.sYearCurrent = this.GetCalendarYearString();
		string text = "st ";
		string text2 = "nd ";
		string text3 = "rd ";
		string text4 = "th ";
		string text5;
		if (this.nDayCurrent == 1 || this.nDayCurrent == 21)
		{
			text5 = text;
		}
		else if (this.nDayCurrent == 2 || this.nDayCurrent == 22)
		{
			text5 = text2;
		}
		else if (this.nDayCurrent == 3 || this.nDayCurrent == 23)
		{
			text5 = text3;
		}
		else
		{
			text5 = text4;
		}
		string text6 = "Day Of ";
		this.sTimeCurrent = string.Concat(new string[]
		{
			this.sDayOfWeek,
			"the ",
			this.sDayCurrent,
			text5,
			text6,
			this.sMonthCurrent,
			"of the Year ",
			this.sYearCurrent
		});
	}

	public int GetCalendarYear()
	{
		return (int)base.GetType().GetField("nYearCurrent").GetValue(this);
	}

	public string GetCalendarYearString()
	{
		int calendarYear = this.GetCalendarYear();
		return OrwConversions.IntToString(calendarYear);
	}

	public int GetYearToMonths(int nVar)
	{
		return nVar * 12;
	}

	public int GetCalendarMonth()
	{
		return (int)base.GetType().GetField("nMonthCurrent").GetValue(this);
	}

	public string GetMonthString()
	{
		int calendarMonth = this.GetCalendarMonth();
		return OrwConversions.IntToString(calendarMonth);
	}

	public string GetDayOfMonthString()
	{
		string empty = string.Empty;
		switch (this.GetCalendarMonth())
		{
		case 1:
			empty = this.sMonth1;
			break;
		case 2:
			empty = this.sMonth2;
			break;
		case 3:
			empty = this.sMonth3;
			break;
		case 4:
			empty = this.sMonth4;
			break;
		case 5:
			empty = this.sMonth5;
			break;
		case 6:
			empty = this.sMonth6;
			break;
		case 7:
			empty = this.sMonth7;
			break;
		case 8:
			empty = this.sMonth8;
			break;
		case 9:
			empty = this.sMonth9;
			break;
		case 10:
			empty = this.sMonth10;
			break;
		case 11:
			empty = this.sMonth11;
			break;
		case 12:
			empty = this.sMonth12;
			break;
		}
		return empty;
	}

	public int GetMonthToDays(int nVar)
	{
		return nVar * 28;
	}

	public int GetCalendarDay()
	{
		return (int)base.GetType().GetField("nDayCurrent").GetValue(this);
	}

	public string GetDayString()
	{
		int calendarDay = this.GetCalendarDay();
		return OrwConversions.IntToString(calendarDay);
	}

	public int GetCalendarDayOfWeek()
	{
		int calendarDay = this.GetCalendarDay();
		int result = 1;
		if (calendarDay <= 7)
		{
			result = calendarDay;
		}
		else if (calendarDay > 7 && calendarDay <= 14)
		{
			result = calendarDay - 7;
		}
		else if (calendarDay > 14 && calendarDay <= 21)
		{
			result = calendarDay - 14;
		}
		else if (calendarDay > 21 && calendarDay <= 28)
		{
			result = calendarDay - 21;
		}
		return result;
	}

	public string GetDayOfWeekString()
	{
		string empty = string.Empty;
		switch (this.GetCalendarDayOfWeek())
		{
		case 1:
			empty = this.sDay1;
			break;
		case 2:
			empty = this.sDay2;
			break;
		case 3:
			empty = this.sDay3;
			break;
		case 4:
			empty = this.sDay4;
			break;
		case 5:
			empty = this.sDay5;
			break;
		case 6:
			empty = this.sDay6;
			break;
		case 7:
			empty = this.sDay7;
			break;
		}
		return empty;
	}

	public int GetDaysToZero()
	{
		int nVar = this.GetCalendarYear() - 1;
		int num = this.GetCalendarMonth() - 1;
		int calendarDay = this.GetCalendarDay();
		int nVar2 = this.GetYearToMonths(nVar) + num;
		return this.GetMonthToDays(nVar2) + calendarDay;
	}

	public int GetDaysInAdvance(int nVar)
	{
		int daysToZero = this.GetDaysToZero();
		return daysToZero + nVar;
	}

	public int GetDayToHours(int nVar)
	{
		return nVar * 24;
	}

	public int GetTimeHour()
	{
		return (int)base.GetType().GetField("nHourCurrent").GetValue(this);
	}

	public string GetHourString()
	{
		int timeHour = this.GetTimeHour();
		return OrwConversions.IntToString(timeHour);
	}

	public int GetHoursToZero()
	{
		int calendarYear = this.GetCalendarYear();
		int num = this.GetCalendarMonth() - 1;
		int num2 = this.GetCalendarDay() - 1;
		int timeHour = this.GetTimeHour();
		int nVar = this.GetYearToMonths(calendarYear) + num;
		int nVar2 = this.GetMonthToDays(nVar) + num2;
		return this.GetDayToHours(nVar2) + timeHour;
	}

	public int GetHoursToSeconds(int nVar)
	{
		return nVar * 60 * 60;
	}

	public int GetTimeMinute()
	{
		return (int)base.GetType().GetField("nMinuteCurrent").GetValue(this);
	}

	public string GetMinuteString()
	{
		int timeMinute = this.GetTimeMinute();
		return OrwConversions.IntToString(timeMinute);
	}

	public int GetMinutesToZero()
	{
		int calendarYear = this.GetCalendarYear();
		int num = this.GetCalendarMonth() - 1;
		int num2 = this.GetCalendarDay() - 1;
		int timeHour = this.GetTimeHour();
		int timeMinute = this.GetTimeMinute();
		int nVar = this.GetYearToMonths(calendarYear) + num;
		int nVar2 = this.GetMonthToDays(nVar) + num2;
		int num3 = this.GetDayToHours(nVar2) + timeHour;
		return num3 * 60 + timeMinute;
	}

	public int GetHoursToMinutes(int nVar)
	{
		return nVar * 60;
	}

	public int GetMinutesToSeconds(int nVar)
	{
		return nVar * 60;
	}

	public float GetNewFrameTime(float fFrameTime)
	{
		int num = 86400;
		int num2 = this.nMPD * 60;
		float num3 = (float)num / (float)num2;
		return num3 * fFrameTime;
	}

	public float GetHoursToDecimal(int nHour, int nMinute, float fSeconds)
	{
		float num = (float)nHour;
		float num2 = (float)nMinute / 60f;
		float num3 = fSeconds / 3600f;
		return num + num2 + num3;
	}

	public bool GetIsDay()
	{
		bool result = false;
		if (this.nHourCurrent >= this.nDayStart && this.nHourCurrent < this.nDayEnd)
		{
			result = true;
		}
		return result;
	}

	public bool GetIsNight()
	{
		bool result = false;
		if (this.nHourCurrent >= this.nDayEnd && this.nHourCurrent < this.nDayStart)
		{
			result = true;
		}
		return result;
	}
}
