using System;
using UnityEngine;

[Serializable]
public class TOD_LightParameters
{
	[TOD_Min(0f), Tooltip("Refresh interval of the light source position in seconds.")]
	public float UpdateInterval;

	[TOD_Range(-1f, 1f), Tooltip("Controls how low the light source is allowed to go.")]
	public float MinimumHeight;
}
