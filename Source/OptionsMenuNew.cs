using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuNew : MonoBehaviour
{
	public Text FullscreenText;

	public Text cameraeffectstext;

	public Text InvertMouseText;

	public Text VSyncText;

	public Text MotionBlurText;

	public Text AmbientOcclusionText;

	public Text ShadowOffText;

	public GameObject ShadowOffTextLine;

	public Text ShadowLowText;

	public GameObject ShadowLowTextLine;

	public Text ShadowHighText;

	public GameObject ShadowHighTextLine;

	public Text DifficultyNormalText;

	public GameObject DifficultyNormalTextLine;

	public Text DifficultyHardcoreText;

	public GameObject DifficultyHardcoreTextLine;

	public Text TextureLowText;

	public GameObject TextureLowTextLine;

	public Text TextureMediumText;

	public GameObject TextureMediumTextLine;

	public Text TextureHighText;

	public GameObject TextureHighTextLine;

	public Text AAOffText;

	public GameObject AAOffTextLine;

	public Text AA2xText;

	public GameObject AA2xTextLine;

	public Text AA4xText;

	public GameObject AA4xTextLine;

	public Text AA8xText;

	public GameObject AA8xTextLine;

	public Slider MusicSlider;

	public Slider SFXSlider;

	public Slider SensitivityXSlider;

	public Slider SensitivityYSlider;

	public Slider MouseSmoothSlider;

	private float SliderValue;

	private float SliderValueSFX;

	private float SliderValueXSensitivity;

	private float SliderValueYSensitivity;

	private float SliderValueSmoothing;

	public void Start()
	{
		if (PlayerPrefs.GetInt("NormalDifficulty") == 1)
		{
			this.DifficultyNormalTextLine.gameObject.SetActive(true);
			this.DifficultyHardcoreTextLine.gameObject.SetActive(false);
		}
		else
		{
			this.DifficultyNormalTextLine.gameObject.SetActive(false);
			this.DifficultyHardcoreTextLine.gameObject.SetActive(true);
		}
		this.MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		this.SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
		this.SensitivityXSlider.value = PlayerPrefs.GetFloat("XSensitivity");
		this.SensitivityYSlider.value = PlayerPrefs.GetFloat("YSensitivity");
		this.MouseSmoothSlider.value = PlayerPrefs.GetFloat("MouseSmoothing");
		if (Screen.fullScreen)
		{
			this.FullscreenText.text = "on";
		}
		else
		{
			this.FullscreenText.text = "off";
		}
		if (PlayerPrefs.GetInt("Shadows") == 0)
		{
			QualitySettings.shadowCascades = 0;
			QualitySettings.shadowDistance = 0f;
			this.ShadowOffText.text = "OFF";
			this.ShadowLowText.text = "low";
			this.ShadowHighText.text = "high";
			this.ShadowOffTextLine.gameObject.SetActive(true);
			this.ShadowLowTextLine.gameObject.SetActive(false);
			this.ShadowHighTextLine.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("Shadows") == 1)
		{
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowDistance = 125f;
			this.ShadowOffText.text = "off";
			this.ShadowLowText.text = "LOW";
			this.ShadowHighText.text = "high";
			this.ShadowOffTextLine.gameObject.SetActive(false);
			this.ShadowLowTextLine.gameObject.SetActive(true);
			this.ShadowHighTextLine.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("Shadows") == 2)
		{
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowDistance = 500f;
			this.ShadowOffText.text = "off";
			this.ShadowLowText.text = "low";
			this.ShadowHighText.text = "HIGH";
			this.ShadowOffTextLine.gameObject.SetActive(false);
			this.ShadowLowTextLine.gameObject.SetActive(false);
			this.ShadowHighTextLine.gameObject.SetActive(true);
		}
		if (QualitySettings.vSyncCount == 0)
		{
			this.VSyncText.text = "off";
		}
		else if (QualitySettings.vSyncCount == 1)
		{
			this.VSyncText.text = "on";
		}
		if (PlayerPrefs.GetInt("Inverted") == 0)
		{
			this.InvertMouseText.text = "off";
		}
		else if (PlayerPrefs.GetInt("Inverted") == 1)
		{
			this.InvertMouseText.text = "on";
		}
		if (PlayerPrefs.GetInt("MotionBlur") == 0)
		{
			this.MotionBlurText.text = "off";
		}
		else if (PlayerPrefs.GetInt("MotionBlur") == 1)
		{
			this.MotionBlurText.text = "on";
		}
		if (PlayerPrefs.GetInt("AmbientOcclusion") == 0)
		{
			this.AmbientOcclusionText.text = "off";
		}
		else if (PlayerPrefs.GetInt("AmbientOcclusion") == 1)
		{
			this.AmbientOcclusionText.text = "on";
		}
		if (PlayerPrefs.GetInt("Textures") == 0)
		{
			QualitySettings.masterTextureLimit = 2;
			this.TextureLowText.text = "LOW";
			this.TextureMediumText.text = "med";
			this.TextureHighText.text = "high";
			this.TextureLowTextLine.gameObject.SetActive(true);
			this.TextureLowTextLine.gameObject.SetActive(false);
			this.TextureLowTextLine.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("Textures") == 1)
		{
			QualitySettings.masterTextureLimit = 1;
			this.TextureLowText.text = "low";
			this.TextureMediumText.text = "MED";
			this.TextureHighText.text = "high";
			this.TextureLowTextLine.gameObject.SetActive(false);
			this.TextureLowTextLine.gameObject.SetActive(true);
			this.TextureLowTextLine.gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("Textures") == 2)
		{
			QualitySettings.masterTextureLimit = 0;
			this.TextureLowText.text = "low";
			this.TextureMediumText.text = "med";
			this.TextureHighText.text = "HIGH";
			this.TextureLowTextLine.gameObject.SetActive(false);
			this.TextureLowTextLine.gameObject.SetActive(false);
			this.TextureLowTextLine.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		this.SliderValue = this.MusicSlider.value;
		this.SliderValueSFX = this.SFXSlider.value;
		this.SliderValueXSensitivity = this.SensitivityXSlider.value;
		this.SliderValueYSensitivity = this.SensitivityYSlider.value;
		this.SliderValueSmoothing = this.MouseSmoothSlider.value;
	}

	public void FullScreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
		if (Screen.fullScreen)
		{
			this.FullscreenText.text = "on";
		}
		else
		{
			this.FullscreenText.text = "off";
		}
	}

	public void ChangeMusicSlider()
	{
		PlayerPrefs.SetFloat("MusicVolume", this.SliderValue);
	}

	public void ChangeSFXSlider()
	{
		PlayerPrefs.SetFloat("SFXVolume", this.SliderValueSFX);
	}

	public void ChangeSensitivityXSlider()
	{
		PlayerPrefs.SetFloat("XSensitivity", this.SliderValueXSensitivity);
	}

	public void ChangeSensitivityYSlider()
	{
		PlayerPrefs.SetFloat("YSensitivity", this.SliderValueYSensitivity);
	}

	public void ChangeSensitivitySmoothing()
	{
		PlayerPrefs.SetFloat("MouseSmoothing", this.SliderValueSmoothing);
	}

	public void NormalDifficulty()
	{
		this.DifficultyHardcoreTextLine.gameObject.SetActive(false);
		this.DifficultyNormalTextLine.gameObject.SetActive(true);
		PlayerPrefs.SetInt("NormalDifficulty", 1);
		PlayerPrefs.SetInt("HardCoreDifficulty", 0);
	}

	public void HardcoreDifficulty()
	{
		this.DifficultyHardcoreTextLine.gameObject.SetActive(true);
		this.DifficultyNormalTextLine.gameObject.SetActive(false);
		PlayerPrefs.SetInt("NormalDifficulty", 0);
		PlayerPrefs.SetInt("HardCoreDifficulty", 1);
	}

	public void ShadowsOff()
	{
		PlayerPrefs.SetInt("Shadows", 0);
		QualitySettings.shadowCascades = 0;
		QualitySettings.shadowDistance = 0f;
		this.ShadowOffText.text = "OFF";
		this.ShadowLowText.text = "low";
		this.ShadowHighText.text = "high";
		this.ShadowOffTextLine.gameObject.SetActive(true);
		this.ShadowLowTextLine.gameObject.SetActive(false);
		this.ShadowHighTextLine.gameObject.SetActive(false);
	}

	public void ShadowsLow()
	{
		PlayerPrefs.SetInt("Shadows", 1);
		QualitySettings.shadowCascades = 2;
		QualitySettings.shadowDistance = 75f;
		this.ShadowOffText.text = "off";
		this.ShadowLowText.text = "LOW";
		this.ShadowHighText.text = "high";
		this.ShadowOffTextLine.gameObject.SetActive(false);
		this.ShadowLowTextLine.gameObject.SetActive(true);
		this.ShadowHighTextLine.gameObject.SetActive(false);
	}

	public void ShadowsHigh()
	{
		PlayerPrefs.SetInt("Shadows", 2);
		QualitySettings.shadowCascades = 4;
		QualitySettings.shadowDistance = 500f;
		this.ShadowOffText.text = "off";
		this.ShadowLowText.text = "low";
		this.ShadowHighText.text = "HIGH";
		this.ShadowOffTextLine.gameObject.SetActive(false);
		this.ShadowLowTextLine.gameObject.SetActive(false);
		this.ShadowHighTextLine.gameObject.SetActive(true);
	}

	public void VSync()
	{
		if (QualitySettings.vSyncCount == 0)
		{
			QualitySettings.vSyncCount = 1;
			this.VSyncText.text = "on";
		}
		else if (QualitySettings.vSyncCount == 1)
		{
			QualitySettings.vSyncCount = 0;
			this.VSyncText.text = "off";
		}
	}

	public void InvertMouse()
	{
		if (PlayerPrefs.GetInt("Inverted") == 0)
		{
			PlayerPrefs.SetInt("Inverted", 1);
			this.InvertMouseText.text = "on";
		}
		else if (PlayerPrefs.GetInt("Inverted") == 1)
		{
			PlayerPrefs.SetInt("Inverted", 0);
			this.InvertMouseText.text = "off";
		}
	}

	public void MotionBlur()
	{
		if (PlayerPrefs.GetInt("MotionBlur") == 0)
		{
			PlayerPrefs.SetInt("MotionBlur", 1);
			this.MotionBlurText.text = "on";
		}
		else if (PlayerPrefs.GetInt("MotionBlur") == 1)
		{
			PlayerPrefs.SetInt("MotionBlur", 0);
			this.MotionBlurText.text = "off";
		}
	}

	public void AmbientOcclusion()
	{
		if (PlayerPrefs.GetInt("AmbientOcclusion") == 0)
		{
			PlayerPrefs.SetInt("AmbientOcclusion", 1);
			this.AmbientOcclusionText.text = "on";
		}
		else if (PlayerPrefs.GetInt("AmbientOcclusion") == 1)
		{
			PlayerPrefs.SetInt("AmbientOcclusion", 0);
			this.AmbientOcclusionText.text = "off";
		}
	}

	public void CameraEffects()
	{
		if (PlayerPrefs.GetInt("CameraEffects") == 0)
		{
			PlayerPrefs.SetInt("CameraEffects", 1);
			this.cameraeffectstext.text = "on";
		}
		else if (PlayerPrefs.GetInt("CameraEffects") == 1)
		{
			PlayerPrefs.SetInt("CameraEffects", 0);
			this.cameraeffectstext.text = "off";
		}
	}

	public void TexturesLow()
	{
		PlayerPrefs.SetInt("Textures", 0);
		QualitySettings.masterTextureLimit = 2;
		this.TextureLowText.text = "LOW";
		this.TextureMediumText.text = "med";
		this.TextureHighText.text = "high";
		this.TextureLowTextLine.gameObject.SetActive(true);
		this.TextureMediumTextLine.gameObject.SetActive(false);
		this.TextureHighTextLine.gameObject.SetActive(false);
	}

	public void TexturesMed()
	{
		PlayerPrefs.SetInt("Textures", 1);
		QualitySettings.masterTextureLimit = 1;
		this.TextureLowText.text = "low";
		this.TextureMediumText.text = "MED";
		this.TextureHighText.text = "high";
		this.TextureLowTextLine.gameObject.SetActive(false);
		this.TextureMediumTextLine.gameObject.SetActive(true);
		this.TextureHighTextLine.gameObject.SetActive(false);
	}

	public void TexturesHigh()
	{
		PlayerPrefs.SetInt("Textures", 2);
		QualitySettings.masterTextureLimit = 0;
		this.TextureLowText.text = "low";
		this.TextureMediumText.text = "med";
		this.TextureHighText.text = "HIGH";
		this.TextureLowTextLine.gameObject.SetActive(false);
		this.TextureMediumTextLine.gameObject.SetActive(false);
		this.TextureHighTextLine.gameObject.SetActive(true);
	}
}
