using System;
using UnityEngine;

public class CheckOptions : MonoBehaviour
{
	public void Start()
	{
		if (PlayerPrefs.GetInt("Shadows") == 0)
		{
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 0f;
		}
		else if (PlayerPrefs.GetInt("Shadows") == 1)
		{
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 75f;
		}
		else if (PlayerPrefs.GetInt("Shadows") == 2)
		{
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowDistance = 500f;
		}
		if (QualitySettings.vSyncCount == 0)
		{
			QualitySettings.vSyncCount = 0;
		}
		else if (QualitySettings.vSyncCount == 1)
		{
			QualitySettings.vSyncCount = 1;
		}
		if (PlayerPrefs.GetInt("Inverted") != 0)
		{
			if (PlayerPrefs.GetInt("Inverted") == 1)
			{
			}
		}
		if (PlayerPrefs.GetInt("MotionBlur") != 0)
		{
			if (PlayerPrefs.GetInt("MotionBlur") == 1)
			{
			}
		}
		if (PlayerPrefs.GetInt("AmbientOcclusion") != 0)
		{
			if (PlayerPrefs.GetInt("AmbientOcclusion") == 1)
			{
			}
		}
		if (PlayerPrefs.GetInt("Textures") == 0)
		{
			QualitySettings.masterTextureLimit = 2;
		}
		else if (PlayerPrefs.GetInt("Textures") == 1)
		{
			QualitySettings.masterTextureLimit = 1;
		}
		else if (PlayerPrefs.GetInt("Textures") == 2)
		{
			QualitySettings.masterTextureLimit = 0;
		}
	}
}
