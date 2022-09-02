using System;
using UnityEngine;

[Serializable]
public class TOD_FogParameters
{
	[Tooltip("Fog color mode.")]
	public TOD_FogType Mode = TOD_FogType.Color;

	[TOD_Range(0f, 1f), Tooltip("Fog color sampling height.")]
	public float HeightBias;
}
