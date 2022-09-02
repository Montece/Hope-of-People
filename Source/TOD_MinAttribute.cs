using System;
using UnityEngine;

public class TOD_MinAttribute : PropertyAttribute
{
	public float min;

	public TOD_MinAttribute(float min)
	{
		this.min = min;
	}
}
