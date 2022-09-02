using System;
using UnityEngine;

[Serializable]
public class TOD_StarParameters
{
	[TOD_Min(0f), Tooltip("Texture tiling of the stars texture.")]
	public float Tiling = 6f;

	[TOD_Min(0f), Tooltip("Brightness of the stars.")]
	public float Brightness = 3f;

	[Tooltip("Type of the stars position calculation.")]
	public TOD_StarsPositionType Position = TOD_StarsPositionType.Rotating;
}
