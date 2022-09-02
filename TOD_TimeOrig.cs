using System;
using System.Threading;
using UnityEngine;

public class TOD_TimeOrig : MonoBehaviour
{
	[Tooltip("Length of one day in minutes.")]
	public float DayLengthInMinutes = 30f;

	[Tooltip("Set the time to the current device time on start.")]
	public bool UseDeviceTime;

	[Tooltip("Apply the time curve when progressing time.")]
	public bool UseTimeCurve;

	[Tooltip("Time progression curve.")]
	public AnimationCurve TimeCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);

	private TOD_Sky sky;

	private AnimationCurve timeCurve;

	private AnimationCurve timeCurveInverse;

	internal event Action OnMinute
	{
		add
		{
			Action action = this.OnMinute;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnMinute, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnMinute;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnMinute, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	internal event Action OnHour
	{
		add
		{
			Action action = this.OnHour;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnHour, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnHour;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnHour, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	internal event Action OnDay
	{
		add
		{
			Action action = this.OnDay;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnDay, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnDay;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnDay, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	internal event Action OnMonth
	{
		add
		{
			Action action = this.OnMonth;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnMonth, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnMonth;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnMonth, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	internal event Action OnYear
	{
		add
		{
			Action action = this.OnYear;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnYear, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.OnYear;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnYear, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	internal void RefreshTimeCurve()
	{
		this.TimeCurve.preWrapMode = WrapMode.Once;
		this.TimeCurve.postWrapMode = WrapMode.Once;
		this.ApproximateCurve(this.TimeCurve, out this.timeCurve, out this.timeCurveInverse);
		this.timeCurve.preWrapMode = WrapMode.Loop;
		this.timeCurve.postWrapMode = WrapMode.Loop;
		this.timeCurveInverse.preWrapMode = WrapMode.Loop;
		this.timeCurveInverse.postWrapMode = WrapMode.Loop;
	}

	internal float ApplyTimeCurve(float deltaTime)
	{
		float num = this.timeCurveInverse.Evaluate(this.sky.Cycle.Hour) + deltaTime;
		deltaTime = this.timeCurve.Evaluate(num) - this.sky.Cycle.Hour;
		if (num >= 24f)
		{
			deltaTime += (float)((int)num / 24 * 24);
		}
		else if (num < 0f)
		{
			deltaTime += (float)(((int)num / 24 - 1) * 24);
		}
		return deltaTime;
	}

	internal void AddHours(float hours, bool adjust = true)
	{
		if (this.UseTimeCurve && adjust)
		{
			hours = this.ApplyTimeCurve(hours);
		}
		DateTime dateTime = this.sky.Cycle.DateTime;
		DateTime dateTime2 = dateTime.AddHours((double)hours);
		if (dateTime2.Year > dateTime.Year)
		{
			if (this.OnYear != null)
			{
				this.OnYear();
			}
			if (this.OnMonth != null)
			{
				this.OnMonth();
			}
			if (this.OnDay != null)
			{
				this.OnDay();
			}
			if (this.OnHour != null)
			{
				this.OnHour();
			}
			if (this.OnMinute != null)
			{
				this.OnMinute();
			}
		}
		else if (dateTime2.Month > dateTime.Month)
		{
			if (this.OnMonth != null)
			{
				this.OnMonth();
			}
			if (this.OnDay != null)
			{
				this.OnDay();
			}
			if (this.OnHour != null)
			{
				this.OnHour();
			}
			if (this.OnMinute != null)
			{
				this.OnMinute();
			}
		}
		else if (dateTime2.Day > dateTime.Day)
		{
			if (this.OnDay != null)
			{
				this.OnDay();
			}
			if (this.OnHour != null)
			{
				this.OnHour();
			}
			if (this.OnMinute != null)
			{
				this.OnMinute();
			}
		}
		else if (dateTime2.Hour > dateTime.Hour)
		{
			if (this.OnHour != null)
			{
				this.OnHour();
			}
			if (this.OnMinute != null)
			{
				this.OnMinute();
			}
		}
		else if (dateTime2.Minute > dateTime.Minute && this.OnMinute != null)
		{
			this.OnMinute();
		}
		this.sky.Cycle.DateTime = dateTime2;
	}

	internal void AddSeconds(float seconds, bool adjust = true)
	{
		this.AddHours(seconds / 3600f, true);
	}

	private void CalculateLinearTangents(Keyframe[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe keyframe = keys[i];
			if (i > 0)
			{
				Keyframe keyframe2 = keys[i - 1];
				keyframe.inTangent = (keyframe.value - keyframe2.value) / (keyframe.time - keyframe2.time);
			}
			if (i < keys.Length - 1)
			{
				Keyframe keyframe3 = keys[i + 1];
				keyframe.outTangent = (keyframe3.value - keyframe.value) / (keyframe3.time - keyframe.time);
			}
			keys[i] = keyframe;
		}
	}

	private void ApproximateCurve(AnimationCurve source, out AnimationCurve approxCurve, out AnimationCurve approxInverse)
	{
		Keyframe[] array = new Keyframe[25];
		Keyframe[] array2 = new Keyframe[25];
		float num = -0.01f;
		for (int i = 0; i < 25; i++)
		{
			num = Mathf.Max(num + 0.01f, source.Evaluate((float)i));
			array[i] = new Keyframe((float)i, num);
			array2[i] = new Keyframe(num, (float)i);
		}
		this.CalculateLinearTangents(array);
		this.CalculateLinearTangents(array2);
		approxCurve = new AnimationCurve(array);
		approxInverse = new AnimationCurve(array2);
	}

	protected void Awake()
	{
		this.sky = base.GetComponent<TOD_Sky>();
		if (this.UseDeviceTime)
		{
			this.sky.Cycle.DateTime = DateTime.Now;
		}
		this.RefreshTimeCurve();
	}

	protected void FixedUpdate()
	{
		float num = 1440f / this.DayLengthInMinutes;
		this.AddSeconds(Time.deltaTime * num, true);
	}
}
