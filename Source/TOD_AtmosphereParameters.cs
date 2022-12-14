using System;
using UnityEngine;

[Serializable]
public class TOD_AtmosphereParameters
{
	[TOD_Min(0f), Tooltip("Intensity of the atmospheric Rayleigh scattering.")]
	public float RayleighMultiplier = 1f;

	[TOD_Min(0f), Tooltip("Intensity of the atmospheric Mie scattering.")]
	public float MieMultiplier = 1f;

	[TOD_Min(0f), Tooltip("Overall brightness of the atmosphere.")]
	public float Brightness = 1.5f;

	[TOD_Min(0f), Tooltip("Overall contrast of the atmosphere.")]
	public float Contrast = 1.5f;

	[TOD_Range(0f, 1f), Tooltip("Directionality factor that determines the size and sharpness of the glow around the light source.")]
	public float Directionality = 0.7f;

	[TOD_Range(0f, 1f), Tooltip("Density of the fog covering the sky.")]
	public float Fogginess;
}
