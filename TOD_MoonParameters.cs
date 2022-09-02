using System;
using UnityEngine;

[Serializable]
public class TOD_MoonParameters
{
	[Tooltip("Color of the moon mesh.\nInterpolates from left (day) to right (night).")]
	public Gradient MeshColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(255, 233, 200, 255), 0f),
			new GradientColorKey(new Color32(255, 233, 200, 255), 1f)
		}
	};

	[TOD_Min(0f), Tooltip("Size of the moon mesh in degrees.")]
	public float MeshSize = 1f;

	[TOD_Min(0f), Tooltip("Brightness of the moon mesh.")]
	public float MeshBrightness = 1f;

	[TOD_Min(0f), Tooltip("Contrast of the moon mesh.")]
	public float MeshContrast = 1f;

	[Tooltip("Color of the moon halo.\nInterpolates from left (day) to right (night).")]
	public Gradient HaloColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(25, 40, 65, 255), 0f),
			new GradientColorKey(new Color32(25, 40, 65, 255), 1f)
		}
	};

	[TOD_Min(0f), Tooltip("Size of the moon halo.")]
	public float HaloSize = 0.1f;

	[Tooltip("Type of the moon position calculation.")]
	public TOD_MoonPositionType Position = TOD_MoonPositionType.Realistic;
}
