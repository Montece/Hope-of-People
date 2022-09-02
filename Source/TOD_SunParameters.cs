using System;
using UnityEngine;

[Serializable]
public class TOD_SunParameters
{
	[Tooltip("Color of the sun spot.\nInterpolates from left (day) to right (night).")]
	public Gradient MeshColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(253, 171, 50, 255), 0f),
			new GradientColorKey(new Color32(253, 171, 50, 255), 1f)
		}
	};

	[TOD_Min(0f), Tooltip("Size of the sun spot in degrees.")]
	public float MeshSize = 1f;

	[TOD_Min(0f), Tooltip("Brightness of the sun spot.")]
	public float MeshBrightness = 1f;

	[TOD_Min(0f), Tooltip("Contrast of the sun spot.")]
	public float MeshContrast = 1f;
}
