using System;
using UnityEngine;

[Serializable]
public class TOD_AmbientParameters
{
	[Tooltip("Ambient light mode.")]
	public TOD_AmbientType Mode = TOD_AmbientType.Color;

	[TOD_Min(0f), Tooltip("Refresh interval of the ambient light probe in seconds.")]
	public float UpdateInterval = 1f;
}
