using System;
using UnityEngine;

public class TOD_Weather : MonoBehaviour
{
	[Tooltip("Time to fade from one weather type to the other.")]
	public float FadeTime = 10f;

	[Tooltip("Currently selected cloud type.")]
	public TOD_CloudType Clouds;

	[Tooltip("Currently selected weather type.")]
	public TOD_WeatherType Weather;

	private float cloudBrightnessDefault;

	private float cloudDensityDefault;

	private float atmosphereFogDefault;

	private float cloudBrightness;

	private float cloudDensity;

	private float atmosphereFog;

	private float cloudSharpness;

	private TOD_Sky sky;

	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
		this.cloudBrightness = (this.cloudBrightnessDefault = this.sky.Clouds.Brightness);
		this.cloudDensity = (this.cloudDensityDefault = this.sky.Clouds.Density);
		this.atmosphereFog = (this.atmosphereFogDefault = this.sky.Atmosphere.Fogginess);
		this.cloudSharpness = this.sky.Clouds.Sharpness;
	}

	protected void Update()
	{
		if (this.Clouds == TOD_CloudType.Custom && this.Weather == TOD_WeatherType.Custom)
		{
			return;
		}
		switch (this.Clouds)
		{
		case TOD_CloudType.Custom:
			this.cloudDensity = this.sky.Clouds.Density;
			this.cloudSharpness = this.sky.Clouds.Sharpness;
			break;
		case TOD_CloudType.None:
			this.cloudDensity = 0f;
			this.cloudSharpness = 1f;
			break;
		case TOD_CloudType.Few:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 5f;
			break;
		case TOD_CloudType.Scattered:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 3f;
			break;
		case TOD_CloudType.Broken:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 1f;
			break;
		case TOD_CloudType.Overcast:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 0.1f;
			break;
		}
		switch (this.Weather)
		{
		case TOD_WeatherType.Custom:
			this.cloudBrightness = this.sky.Clouds.Brightness;
			this.atmosphereFog = this.sky.Atmosphere.Fogginess;
			break;
		case TOD_WeatherType.Clear:
			this.cloudBrightness = this.cloudBrightnessDefault;
			this.atmosphereFog = this.atmosphereFogDefault;
			break;
		case TOD_WeatherType.Storm:
			this.cloudBrightness = 0.3f;
			this.atmosphereFog = 1f;
			break;
		case TOD_WeatherType.Dust:
			this.cloudBrightness = this.cloudBrightnessDefault;
			this.atmosphereFog = 0.5f;
			break;
		case TOD_WeatherType.Fog:
			this.cloudBrightness = this.cloudBrightnessDefault;
			this.atmosphereFog = 1f;
			break;
		}
		float t = Time.deltaTime / this.FadeTime;
		this.sky.Clouds.Brightness = Mathf.Lerp(this.sky.Clouds.Brightness, this.cloudBrightness, t);
		this.sky.Clouds.Density = Mathf.Lerp(this.sky.Clouds.Density, this.cloudDensity, t);
		this.sky.Clouds.Sharpness = Mathf.Lerp(this.sky.Clouds.Sharpness, this.cloudSharpness, t);
		this.sky.Atmosphere.Fogginess = Mathf.Lerp(this.sky.Atmosphere.Fogginess, this.atmosphereFog, t);
	}
}
