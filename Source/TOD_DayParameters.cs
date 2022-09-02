using System;
using UnityEngine;

[Serializable]
public class TOD_DayParameters
{
	[Tooltip("Color of the light that hits the atmosphere.\nInterpolates from left (day) to right (night).")]
	public Gradient SkyColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(255, 243, 234, 255), 0f),
			new GradientColorKey(new Color32(255, 243, 234, 255), 1f)
		}
	};

	[Tooltip("Color of the light that hits the ground.\nInterpolates from left (day) to right (night).")]
	public Gradient LightColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(255, 243, 234, 255), 0f),
			new GradientColorKey(new Color32(255, 107, 0, 255), 1f)
		}
	};

	[Tooltip("Color of the god rays.\nInterpolates from left (day) to right (night).")]
	public Gradient RayColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(255, 243, 234, 255), 0f),
			new GradientColorKey(new Color32(255, 107, 0, 255), 1f)
		}
	};

	[Tooltip("Color of the clouds.\nInterpolates from left (day) to right (night).")]
	public Gradient CloudColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(255, 255, 255, 255), 0f),
			new GradientColorKey(new Color32(255, 200, 100, 255), 1f)
		}
	};

	[Tooltip("Color of the ambient light.\nInterpolates from left (day) to right (night).")]
	public Gradient AmbientColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(new Color32(94, 89, 87, 255), 0f),
			new GradientColorKey(new Color32(94, 89, 87, 255), 1f)
		}
	};

	[TOD_Min(0f), Tooltip("Intensity of the light source.")]
	public float LightIntensity = 1f;

	[TOD_Range(0f, 1f), Tooltip("Opacity of the shadows dropped by the light source.")]
	public float ShadowStrength = 1f;

	[Range(0f, 1f), Tooltip("Brightness of colors.")]
	public float ColorMultiplier = 1f;

	[Range(0f, 1f), Tooltip("Brightness of ambient light.")]
	public float AmbientMultiplier = 1f;

	[Range(0f, 1f), Tooltip("Brightness of reflected light.")]
	public float ReflectionMultiplier = 1f;
}
