using gWeatherFunctions;
using System;
using UnityEngine;

public class gWeatherMaster : MonoBehaviour
{
	public int nDebug;

	private gWeatherGameData gWeatherGameData;

	private gWeatherLightning gWeatherLightning;

	private gWeatherThunder gWeatherThunder;

	private gWeatherRain gWeatherRain;

	private gWeatherSnow gWeatherSnow;

	private gWeatherWind gWeatherWind;

	private gWeatherGroundFog gWeatherGroundFog;

	private TOD_Sky Sky;

	private Light Light;

	public GameObject WindZone;

	private gTime gTime;

	public float fWeatherUpdateRate = 10f;

	public float fWeatherCycleTimer = 8f;

	[Tooltip("(Automatic) Weather Cold Front. 1 is currently in a Cold Front. Database Stored")]
	public int nInColdFront;

	[Tooltip("(Automatic) Weather Warm Front. 1 is currently in a Warm Front. Database Stored")]
	public int nInWarmFront;

	[Tooltip("(Automatic) Weather Front Duration. This is a Countdown Each Cycle of the current Weather Front. Database Stored")]
	public int nFrontDuration;

	[Tooltip("(Automatic) Weather Front Duration Manual Adjust Duration. In Manual Mode Duration may be manually set.1 = Manual Mode. Return to 0 to continue from Duration Set.")]
	public int nFrontDurationManualAdjust;

	[Tooltip("Minimum Front Duration when a Front is Initialized.")]
	public int nFrontDurationMin = 10;

	[Tooltip("Maximum Front Duration when a Front is Initialized.")]
	public int nFrontDurationMax = 100;

	[Tooltip("(Automatic) Current Weather Season of a Scene. Dependent upon North or South Hemisphere.")]
	public int nCurrentSeason = 1;

	[Tooltip("The number of this WeatherZone. WeatherMaster must exist and is the Master Weather Controller for the Scene.")]
	public int nZoneNumber = 1;

	[Tooltip("Temperature Zone (All Degrees in Celsius) is the Climate Zone of a Scene. 1 - Tropical   12 Months Warm 20 to 50 2 - Subtropical 8 Months Warm 20 to 35, 4 at 15 to 25 3 - Temperate 8 Months at 15 to 30, 4 at 2 to -10 4 - Cool 8 Months at 10 to 20, 4 at 10 to -5 5 - Cold 4 Months 5 to 10, 8 at 5 to -30 6 - Polar 12 Months -5 to -50 ")]
	public int nTemperatureZone = 3;

	[Tooltip("Precipitation Class is the Precipitaion Zone of a Scene.0 - Arid - No Rain (even if Humidity is set) 1 - Semi Arid - Slight Summer Rain 2 - Arid Oceanic - Slight Winter Rain 3 - Oceanic - Winter Rain, Dry Summer 4 - Tropical - Summer Rain, Dry Winter 5 - Temperatate - Rain in all Seasons ")]
	public int nPrecipitationZone = 5;

	[Tooltip("Area Water Content (in Percent). Determines Fog and Clouds and helps tweak Precipitation. Range 0 to 100")]
	public int nWaterContent = 20;

	[Tooltip("Minimum Temperature for the Area. Range -50.0 to 50.0 (capped by Temperature Zone).")]
	public float fTemperatureMin = 15f;

	[Tooltip("Minimum Temperature for the Area. Range -50.0 to 50.0 (capped by Temperature Zone).")]
	public float fTemperatureMax = 30f;

	[Tooltip("Planet Hemisphere of the Scene. For North = N or South = S of the Equator.")]
	public string sHemisphere = "N";

	[Tooltip("/// Humidity Percent (Air Saturation) of the Scene Minimum Required for Precipitation to occur. Less than this amount will initialize a Warm Front.")]
	public float fHumidityRequiredToPrecipitate = 20f;

	[Tooltip("(Automatic) Humidity Percent (Air Saturation) of the Scene.>= 50 :: Light Rain/Snow>= 60 :: Medium Rain/Snow>= 70 :: Heavy Rain/Snow, Occasional Thunder, Rare Lightning>= 90 :: Very Heavy Rain/Snow, Common Thunder, Common Lightning")]
	public float fHumidityPercent = 1f;

	public float fHunmidityPrevious;

	[Tooltip("(Automatic) Current Temperature in Celsius of the Scene.")]
	public float fTemperatureC = 25f;

	public float fTemperatureCPrevious;

	[Tooltip("(Automatic) Current Temperature in Fahrenheit of the Scene.")]
	public float fTemperatureF;

	[Tooltip("(Automatic) Current Weather Level in a Scene. Based upon Humidity.0-30.0% :: Level 1 Clear to Mild clouds.31-40.0% :: Level 2 Mild to Partly Cloudy.41-50.0% :: Level 3 Partly to Mostly Cloudy.51-60.0% :: Level 4 Full Clouds with Light Darkening.61-80.0% :: Level 5 Full Clouds with Medium Darkening.81+% :: Level 6 Full Clouds with Heavy Darkening.")]
	public int nLevel = 1;

	[Tooltip("(Automatic) Current Light Intensity. This follows Cloud Density. VDIP is the amount of Drop in Light Intensity during Partly and Mostly Cloudy periods.")]
	public float fLightIntensityDay = 0.9f;

	public float fLightIntensityDayMin = 0.1f;

	public float fLightIntensityDayMax = 0.9f;

	public float fLightIntensityNight = 0.15f;

	public float fLightIntensityNightMin = 0.05f;

	public float fLightIntensityNightMax = 0.15f;

	[Tooltip("(Automatic) Time of Day uses 'Fogginess' to grey the sky and dim light. This combined with Light Intensity lends a nice effect for higher humidity and cloudy atmospheres. Follows Humidity.")]
	public float fLightFogginess = 0.1f;

	public float fLightFogginessMin = 0.25f;

	public float fLightFogginessMax = 0.85f;

	public float fLightFogginessHumidityMin = 35f;

	public float fLightFogginessHumidityMax = 90f;

	[Tooltip("(Automatic) VDIP is the amount of Drop in Light Intensity during Partly and Mostly Cloudy periods.")]
	public float fLightIntensityVDIP = 0.15f;

	public float fLightIntensityPrevious;

	public bool bLightIntensityVDIP_MODE;

	public bool bLightIntensityVDIP_TOG;

	public float fLightIntensityVDIPCycle = 10f;

	public float fLightIntensityVDIPCycleMin = 10f;

	public float fLightIntensityVDIPCycleMax = 30f;

	public float fLightIntensityVDIPCyclePrev;

	[Tooltip("(Automatic) Current Cloud Density. This follows Humidity. With Time of Day this only applies a light density to the clouds.")]
	public float fCloudDensity;

	public float fCloudDensityMin;

	public float fCloudDensityMax = 1f;

	public float fCloudDensityHumidityMin = 25f;

	public float fCloudDensityHumidityMax = 50f;

	[Tooltip("(Automatic) Current Cloud Sharpness. This follows Humidity. With Time of Day this make more haze in the clouds and combines with Density to 'thicken' clouds.")]
	public float fCloudSharpness;

	public float fCloudSharpnessMin;

	public float fCloudSharpnessMax = 0.5f;

	public float fCloudSharpnessHumidityMin = 30f;

	public float fCloudSharpnessHumidityMax = 50f;

	[Tooltip("(Automatic) Current Cloud Brightness. This follows Cloud Density. With Time of Day this dims the clouds making them look darker and thicker.")]
	public float fCloudBrightness = 0.95f;

	public float fCloudBrightnessMin = 0.95f;

	public float fCloudBrightnessMax = 1.5f;

	public float fCloudBrightnessHumidityMin = 10f;

	public float fCloudBrightnessHumidityMax = 90f;

	[Tooltip("Fog Static Override. fFoginess will not vary and may be manipulated manually. Set to 1 to Enable")]
	public int nFogStaticOverride;

	[Tooltip("Persistent Ground Fog Override. Ground Fog will not burn off. Good for Moors or any moist area. Set to 1 to Enable")]
	public int nFogGroundOverride;

	[Tooltip("Persistent Ground Fog Density Override. Set this to the Density of the Ground Fog. Only functions if nWEATHER_FOG_GO is Enabled.")]
	public int nFogDensityOverride = 500;

	[Tooltip("Ground Fog Configuration. Ground Fog settles in at night and burns off in the morning. Based upon Temperature and Humidity.")]
	public int nFogDensity;

	public float fFogMin = 10f;

	public float fFogMax = 1000f;

	public float fFogHumidityMin = 25f;

	public float fFogHumidityMax = 80f;

	public float fFogTemperatureMin = 1.5f;

	public float fFogTemperatureMax = 25f;

	[Tooltip("Current Scene Global Fog Fogginess. This follows Humidity. Sent to => RenderSettings.fogStartDistance & RenderSettings.fogEndDistance")]
	public float fFogginess;

	public float fFogginessMin = 100f;

	public float fFogginessMax = 300f;

	public float fFogginessHumidityMin = 20f;

	public float fFogginessHumidityMax = 80f;

	[Tooltip("(Automatic) Weather Heating time of the day. This affects how much moisture is evaporated into the air.")]
	public int nHeatingDay;

	[Tooltip("(Automatic) Rain Emission Rate based upon current Humidity. Calculated each cycle.")]
	public int nRainEmissionRate;

	[Tooltip("(Automatic) Snow Emission Rate based upon current Humidity. Calculated each cycle.")]
	public int nSnowEmissionRate;

	private void Start()
	{
		this.gTime = GameObject.Find("_GameData").GetComponent<gTime>();
		this.gWeatherGameData = GameObject.Find("_GameData").GetComponent<gWeatherGameData>();
		this.gWeatherLightning = GameObject.Find("_Lightning").GetComponent<gWeatherLightning>();
		this.gWeatherThunder = GameObject.Find("_Thunder").GetComponent<gWeatherThunder>();
		this.gWeatherRain = GameObject.Find("_Rain").GetComponent<gWeatherRain>();
		this.gWeatherSnow = GameObject.Find("_Snow").GetComponent<gWeatherSnow>();
		this.gWeatherWind = GameObject.Find("_Wind").GetComponent<gWeatherWind>();
		this.gWeatherGroundFog = GameObject.Find("_GroundFog").GetComponent<gWeatherGroundFog>();
		this.Sky = GameObject.Find("Sky Dome").GetComponent<TOD_Sky>();
	}

	private void Update()
	{
		int nWeatherInitialized = this.gWeatherGameData.nWeatherInitialized;
		if (nWeatherInitialized == 1)
		{
			this.fWeatherCycleTimer += Time.deltaTime;
			if (this.fWeatherCycleTimer >= this.fWeatherUpdateRate)
			{
				int num = 0;
				int nHourCurrent = this.gTime.nHourCurrent;
				if (nHourCurrent >= 0 && nHourCurrent <= 7)
				{
					num = 0;
				}
				if (nHourCurrent > 7 && nHourCurrent <= 16)
				{
					num = 1;
				}
				if (nHourCurrent > 16 && nHourCurrent <= 23)
				{
					num = 0;
				}
				this.nHeatingDay = num;
				this.nLevel = this.WeatherSetLevel(this.fHumidityPercent, this.nInColdFront, this.nPrecipitationZone);
				if (num == 1)
				{
					this.fTemperatureC = this.WeatherWarmAirTemp(this.gTime.nMonthCurrent, this.fTemperatureC, this.nLevel, this.nTemperatureZone, this.nInColdFront, this.nInWarmFront, this.sHemisphere);
				}
				if (num == 0)
				{
					this.fTemperatureC = this.WeatherCoolAirTemp(this.gTime.nMonthCurrent, this.fTemperatureC, this.nLevel, this.nTemperatureZone, this.nInColdFront, this.nInWarmFront, this.sHemisphere);
				}
				if (this.nFrontDuration > 0)
				{
					this.nFrontDuration--;
				}
				if (this.nFrontDuration < 1)
				{
					int randomInteger = OrwRandom.GetRandomInteger(1, 100, 1, 0);
					if (randomInteger >= 50 && this.fHumidityPercent >= 40f)
					{
						this.nInColdFront = 1;
						this.nInWarmFront = 0;
						this.nFrontDuration = OrwRandom.GetRandomInteger(this.nFrontDurationMin, this.nFrontDurationMax, 1, 0);
					}
					else
					{
						this.nInColdFront = 0;
						this.nInWarmFront = 1;
						this.nFrontDuration = OrwRandom.GetRandomInteger(this.nFrontDurationMin, this.nFrontDurationMax, 1, 0);
					}
				}
				if (this.nInWarmFront == 1 && this.nPrecipitationZone > 0 && this.fHumidityPercent >= 100f)
				{
					this.nInColdFront = 1;
					this.nInWarmFront = 0;
					this.nFrontDuration = OrwRandom.GetRandomInteger(this.nFrontDurationMin, this.nFrontDurationMax, 1, 0);
				}
				if (this.fHumidityPercent < this.fHumidityRequiredToPrecipitate && this.nFrontDuration < 10)
				{
					this.nInColdFront = 0;
					this.nInWarmFront = 1;
					this.nFrontDuration = OrwRandom.GetRandomInteger(this.nFrontDurationMin, this.nFrontDurationMax, 1, 0);
				}
				if ((this.nInColdFront == 1 && this.nInWarmFront == 1) || (this.nInColdFront == 0 && this.nInWarmFront == 0))
				{
					this.nInColdFront = 0;
					this.nInWarmFront = 1;
					this.nFrontDuration = OrwRandom.GetRandomInteger(this.nFrontDurationMin, this.nFrontDurationMax, 1, 0);
				}
				this.fTemperatureC = this.WeatherSetTemperatureC(this.fTemperatureC);
				this.fTemperatureF = this.WeatherSetTemperatureF(this.fTemperatureC);
				this.fTemperatureCPrevious = this.fTemperatureC;
				this.fHumidityPercent = this.WeatherSetHumidity(this.fHumidityPercent, this.nPrecipitationZone, this.nInColdFront, this.nInWarmFront, this.nHeatingDay);
				this.fHunmidityPrevious = this.fHumidityPercent;
				this.nRainEmissionRate = this.WeatherRainEmissionRate(this.fHumidityPercent);
				this.nSnowEmissionRate = this.WeatherSnowEmissionRate(this.fHumidityPercent);
				this.nCurrentSeason = this.WeatherSetSeason(this.sHemisphere);
				this.fCloudDensity = OrwConversions.ScaleValueFloat(this.fHumidityPercent, this.fCloudDensityHumidityMin, this.fCloudDensityHumidityMax, this.fCloudDensityMin, this.fCloudDensityMax);
				this.fCloudBrightness = OrwConversions.ScaleValueFloat(this.fHumidityPercent, this.fCloudBrightnessHumidityMin, this.fCloudBrightnessHumidityMax, this.fCloudBrightnessMin, this.fCloudBrightnessMax);
				this.fLightFogginess = OrwConversions.ScaleValueFloat(this.fHumidityPercent, this.fLightFogginessHumidityMin, this.fLightFogginessHumidityMax, this.fLightFogginessMin, this.fLightFogginessMax);
				this.fCloudSharpness = OrwConversions.ScaleValueFloat(this.fHumidityPercent, this.fCloudSharpnessHumidityMin, this.fCloudSharpnessHumidityMax, this.fCloudSharpnessMin, this.fCloudSharpnessMax);
				if (this.nFogStaticOverride < 1)
				{
					this.fFogginess = OrwConversions.ScaleValueFloat(this.fHumidityPercent, this.fFogginessHumidityMin, this.fFogginessHumidityMax, this.fFogginessMin, this.fFogginessMax);
				}
				this.nFogDensity = gWeatherMaster.WeatherSetFogGround(this.fHumidityPercent, this.fTemperatureC, this.nInColdFront, this.gTime.nHourCurrent, this.nFogGroundOverride, this.nFogDensityOverride, this.fTemperatureMin, this.fTemperatureMax, this.fFogginessHumidityMin, this.fFogginessHumidityMax, this.fFogginessMin, this.fFogginessMax);
				this.gWeatherLightning.bProcess = true;
				this.gWeatherThunder.bProcess = true;
				this.gWeatherRain.bProcess = true;
				this.gWeatherSnow.bProcess = true;
				this.gWeatherWind.bProcess = true;
				this.gWeatherGroundFog.bProcess = true;
				this.fWeatherCycleTimer -= this.fWeatherUpdateRate;
			}
			this.fLightIntensityVDIPCyclePrev += Time.deltaTime;
			this.WeatherSetLighting();
			this.Sky.Clouds.Sharpness = this.fCloudSharpness;
			this.Sky.Clouds.Brightness = this.fCloudBrightness;
			this.Sky.Atmosphere.Fogginess = this.fLightFogginess;
			this.Sky.Day.LightIntensity = this.fLightIntensityDay;
			this.Sky.Night.LightIntensity = this.fLightIntensityNight;
		}
	}

	private float WeatherSetHumidity(float fH, int nPZ, int nICF, int nIWF, int nHeating)
	{
		float num = fH * 10f;
		int num2 = (int)num;
		fH = (float)num2 * 0.1f;
		int randomInteger = OrwRandom.GetRandomInteger(1, 100, 1, 0);
		if (nHeating == 1 && nIWF == 1 && nPZ > 0 && randomInteger <= this.nWaterContent)
		{
			fH += 0.1f;
		}
		if (nICF == 1 && this.nLevel > 2)
		{
			fH -= 0.1f;
		}
		if (nHeating == 1 && nICF == 1 && nPZ > 0 && randomInteger >= 50)
		{
			fH -= 0.1f;
		}
		if (nHeating == 0 && nIWF == 1 && nPZ > 0 && randomInteger <= this.nWaterContent)
		{
			fH += 0.1f;
		}
		if (nHeating == 0 && nICF == 1 && nPZ > 0 && randomInteger >= 40)
		{
			fH -= 0.1f;
		}
		if (fH < 10f)
		{
			fH = 10f;
		}
		if (fH > 100f)
		{
			fH = 100f;
		}
		return fH;
	}

	private float SeasonMonthlyTemperatureMin(int nTZ, int nMonth, string sHemisphere)
	{
		float result = 0f;
		string b = "N";
		string b2 = "S";
		if (sHemisphere == b)
		{
			if (nTZ == 1)
			{
				switch (nMonth)
				{
				case 1:
					result = 20.5f;
					break;
				case 2:
					result = 24.5f;
					break;
				case 3:
					result = 28.5f;
					break;
				case 4:
					result = 32.5f;
					break;
				case 5:
					result = 36.5f;
					break;
				case 6:
					result = 40.5f;
					break;
				case 7:
					result = 44.5f;
					break;
				case 8:
					result = 40.5f;
					break;
				case 9:
					result = 36.5f;
					break;
				case 10:
					result = 32.5f;
					break;
				case 11:
					result = 28.5f;
					break;
				case 12:
					result = 24.5f;
					break;
				}
			}
			if (nTZ == 2)
			{
				switch (nMonth)
				{
				case 1:
					result = 15.5f;
					break;
				case 2:
					result = 18.3f;
					break;
				case 3:
					result = 21f;
					break;
				case 4:
					result = 23.8f;
					break;
				case 5:
					result = 26.5f;
					break;
				case 6:
					result = 29.3f;
					break;
				case 7:
					result = 32f;
					break;
				case 8:
					result = 29.3f;
					break;
				case 9:
					result = 26.5f;
					break;
				case 10:
					result = 23.8f;
					break;
				case 11:
					result = 21f;
					break;
				case 12:
					result = 18.3f;
					break;
				}
			}
			if (nTZ == 3)
			{
				switch (nMonth)
				{
				case 1:
					result = -9.5f;
					break;
				case 2:
					result = -3.8f;
					break;
				case 3:
					result = 2f;
					break;
				case 4:
					result = 7.8f;
					break;
				case 5:
					result = 13.5f;
					break;
				case 6:
					result = 19.3f;
					break;
				case 7:
					result = 25f;
					break;
				case 8:
					result = 19.3f;
					break;
				case 9:
					result = 13.5f;
					break;
				case 10:
					result = 7.8f;
					break;
				case 11:
					result = 2f;
					break;
				case 12:
					result = -3.8f;
					break;
				}
			}
			if (nTZ == 4)
			{
				switch (nMonth)
				{
				case 1:
					result = -4.5f;
					break;
				case 2:
					result = -1.3f;
					break;
				case 3:
					result = 2f;
					break;
				case 4:
					result = 5.3f;
					break;
				case 5:
					result = 8.5f;
					break;
				case 6:
					result = 11.8f;
					break;
				case 7:
					result = 15f;
					break;
				case 8:
					result = 11.8f;
					break;
				case 9:
					result = 8.5f;
					break;
				case 10:
					result = 5.3f;
					break;
				case 11:
					result = 2f;
					break;
				case 12:
					result = -1.3f;
					break;
				}
			}
			if (nTZ == 5)
			{
				switch (nMonth)
				{
				case 1:
					result = -29.5f;
					break;
				case 2:
					result = -23.8f;
					break;
				case 3:
					result = -18f;
					break;
				case 4:
					result = -12.3f;
					break;
				case 5:
					result = -6.5f;
					break;
				case 6:
					result = -0.8f;
					break;
				case 7:
					result = 5f;
					break;
				case 8:
					result = -0.8f;
					break;
				case 9:
					result = -6.5f;
					break;
				case 10:
					result = -12.3f;
					break;
				case 11:
					result = -18f;
					break;
				case 12:
					result = -23.8f;
					break;
				}
			}
			if (nTZ == 6)
			{
				switch (nMonth)
				{
				case 1:
					result = -49.5f;
					break;
				case 2:
					result = -42.9f;
					break;
				case 3:
					result = -36.3f;
					break;
				case 4:
					result = -29.7f;
					break;
				case 5:
					result = -23.1f;
					break;
				case 6:
					result = -16.5f;
					break;
				case 7:
					result = -9.9f;
					break;
				case 8:
					result = -16.5f;
					break;
				case 9:
					result = -23.1f;
					break;
				case 10:
					result = -29.7f;
					break;
				case 11:
					result = -36.3f;
					break;
				case 12:
					result = -42.9f;
					break;
				}
			}
		}
		if (sHemisphere == b2)
		{
			if (nTZ == 1)
			{
				switch (nMonth)
				{
				case 1:
					result = 44.5f;
					break;
				case 2:
					result = 40.5f;
					break;
				case 3:
					result = 36.5f;
					break;
				case 4:
					result = 32.5f;
					break;
				case 5:
					result = 28.5f;
					break;
				case 6:
					result = 24.5f;
					break;
				case 7:
					result = 20.5f;
					break;
				case 8:
					result = 24.5f;
					break;
				case 9:
					result = 28.5f;
					break;
				case 10:
					result = 32.5f;
					break;
				case 11:
					result = 36.5f;
					break;
				case 12:
					result = 40.5f;
					break;
				}
			}
			if (nTZ == 2)
			{
				switch (nMonth)
				{
				case 1:
					result = 32f;
					break;
				case 2:
					result = 29.3f;
					break;
				case 3:
					result = 26.5f;
					break;
				case 4:
					result = 23.8f;
					break;
				case 5:
					result = 21f;
					break;
				case 6:
					result = 18.3f;
					break;
				case 7:
					result = 15.5f;
					break;
				case 8:
					result = 18.3f;
					break;
				case 9:
					result = 21f;
					break;
				case 10:
					result = 23.8f;
					break;
				case 11:
					result = 26.5f;
					break;
				case 12:
					result = 29.3f;
					break;
				}
			}
			if (nTZ == 3)
			{
				switch (nMonth)
				{
				case 1:
					result = 25f;
					break;
				case 2:
					result = 19.3f;
					break;
				case 3:
					result = 13.5f;
					break;
				case 4:
					result = 7.8f;
					break;
				case 5:
					result = 2f;
					break;
				case 6:
					result = -3.8f;
					break;
				case 7:
					result = -9.5f;
					break;
				case 8:
					result = -3.8f;
					break;
				case 9:
					result = 2f;
					break;
				case 10:
					result = 7.8f;
					break;
				case 11:
					result = 13.5f;
					break;
				case 12:
					result = 19.3f;
					break;
				}
			}
			if (nTZ == 4)
			{
				switch (nMonth)
				{
				case 1:
					result = 15f;
					break;
				case 2:
					result = 11.8f;
					break;
				case 3:
					result = 8.5f;
					break;
				case 4:
					result = 5.3f;
					break;
				case 5:
					result = 2f;
					break;
				case 6:
					result = -1.3f;
					break;
				case 7:
					result = -4.5f;
					break;
				case 8:
					result = -1.3f;
					break;
				case 9:
					result = 2f;
					break;
				case 10:
					result = 5.3f;
					break;
				case 11:
					result = 8.5f;
					break;
				case 12:
					result = 11.8f;
					break;
				}
			}
			if (nTZ == 5)
			{
				switch (nMonth)
				{
				case 1:
					result = 5f;
					break;
				case 2:
					result = -0.8f;
					break;
				case 3:
					result = -6.5f;
					break;
				case 4:
					result = -12.3f;
					break;
				case 5:
					result = -18f;
					break;
				case 6:
					result = -23.8f;
					break;
				case 7:
					result = -29.5f;
					break;
				case 8:
					result = -23.8f;
					break;
				case 9:
					result = -18f;
					break;
				case 10:
					result = -12.3f;
					break;
				case 11:
					result = -6.5f;
					break;
				case 12:
					result = -0.8f;
					break;
				}
			}
			if (nTZ == 6)
			{
				switch (nMonth)
				{
				case 1:
					result = -9.9f;
					break;
				case 2:
					result = -16.5f;
					break;
				case 3:
					result = -23.1f;
					break;
				case 4:
					result = -29.7f;
					break;
				case 5:
					result = -36.3f;
					break;
				case 6:
					result = -42.9f;
					break;
				case 7:
					result = -49.5f;
					break;
				case 8:
					result = -42.9f;
					break;
				case 9:
					result = -36.3f;
					break;
				case 10:
					result = -29.7f;
					break;
				case 11:
					result = -23.1f;
					break;
				case 12:
					result = -16.5f;
					break;
				}
			}
		}
		return result;
	}

	private float SeasonMonthlyTemperatureMax(int nTZ, int nMonth, string sHemisphere)
	{
		return this.SeasonMonthlyTemperatureMin(nTZ, nMonth, sHemisphere) + 5f;
	}

	private float WeatherCoolAirTemp(int nMonth, float fT, int nCWL, int nTZ, int nICF, int nIWF, string sHemisphere)
	{
		float num = this.SeasonMonthlyTemperatureMin(nTZ, nMonth, sHemisphere);
		float num2 = this.SeasonMonthlyTemperatureMax(nTZ, nMonth, sHemisphere);
		float num3 = num - 1.5f;
		float num4 = num2 + 1.5f;
		int randomInteger = OrwRandom.GetRandomInteger(1, 3, 1, 0);
		float num5 = 0f;
		bool flag = false;
		if (fT > num4)
		{
			flag = true;
		}
		if (nICF == 1 && randomInteger <= 2)
		{
			flag = true;
			num5 += 0.1f;
			if (nCWL >= 3)
			{
				num5 += 0.1f;
			}
		}
		if (randomInteger <= 2)
		{
			flag = true;
			num5 += 0.1f;
		}
		if (fT < num3)
		{
			flag = false;
		}
		if (flag)
		{
			fT -= num5;
		}
		return fT;
	}

	private float WeatherWarmAirTemp(int nMonth, float fT, int nCWL, int nTZ, int nICF, int nIWF, string sHemisphere)
	{
		float num = this.SeasonMonthlyTemperatureMin(nTZ, nMonth, sHemisphere);
		float num2 = this.SeasonMonthlyTemperatureMax(nTZ, nMonth, sHemisphere);
		float num3 = num - 1.5f;
		float num4 = num2 + 1.5f;
		int randomInteger = OrwRandom.GetRandomInteger(1, 3, 1, 0);
		float num5 = 0f;
		bool flag = false;
		if (fT < num3)
		{
			flag = true;
		}
		if (nIWF == 1 && randomInteger <= 2)
		{
			flag = true;
			num5 += 0.1f;
			if (nCWL < 3)
			{
				num5 += 0.1f;
			}
		}
		if (randomInteger < 1)
		{
			flag = true;
			num5 += 0.1f;
		}
		if (fT > num4)
		{
		}
		if (flag)
		{
			fT += num5;
		}
		return fT;
	}

	private float WeatherSetTemperatureC(float fT)
	{
		fT = OrwConversions.FloatToRounded(fT, 1);
		return fT;
	}

	private float WeatherSetTemperatureF(float fT)
	{
		float fValue = fT * 1.8f + 32f;
		return OrwConversions.FloatToRounded(fValue, 1);
	}

	private int WeatherSetLevel(float fH, int nICF, int nPZ)
	{
		int result = 1;
		if (fH >= 31f)
		{
			result = 2;
		}
		if (fH >= 41f)
		{
			result = 3;
		}
		if (nICF == 1 && nPZ > 0)
		{
			if (fH >= 51f)
			{
				result = 4;
			}
			if (fH >= 61f)
			{
				result = 5;
			}
			if (fH >= 81f)
			{
				result = 6;
			}
		}
		return result;
	}

	private int WeatherSetSeason(string sHemisphere)
	{
		int result = 0;
		int nMonthCurrent = this.gTime.nMonthCurrent;
		if (sHemisphere == "N" || sHemisphere == string.Empty)
		{
			if (nMonthCurrent < 3 || nMonthCurrent == 12)
			{
				result = 1;
			}
			if (nMonthCurrent >= 3 && nMonthCurrent < 6)
			{
				result = 2;
			}
			if (nMonthCurrent >= 6 && nMonthCurrent < 9)
			{
				result = 3;
			}
			if (nMonthCurrent >= 9 && nMonthCurrent <= 11)
			{
				result = 4;
			}
		}
		if (sHemisphere == "S")
		{
			if (nMonthCurrent < 3 || nMonthCurrent == 12)
			{
				result = 4;
			}
			if (nMonthCurrent >= 3 && nMonthCurrent < 6)
			{
				result = 1;
			}
			if (nMonthCurrent >= 6 && nMonthCurrent < 9)
			{
				result = 2;
			}
			if (nMonthCurrent >= 9 && nMonthCurrent <= 11)
			{
				result = 3;
			}
		}
		return result;
	}

	private int WeatherRainEmissionRate(float fH)
	{
		int num = 500;
		float fScaleMin = (float)num;
		int num2 = 7500;
		float fScaleMax = (float)num2;
		float fInputMin = 50f;
		float fInputMax = 100f;
		float num3 = OrwConversions.ScaleValueFloat(fH, fInputMin, fInputMax, fScaleMin, fScaleMax);
		return (int)num3;
	}

	private int WeatherSnowEmissionRate(float fH)
	{
		int num = 100;
		float fScaleMin = (float)num;
		int num2 = 4000;
		float fScaleMax = (float)num2;
		float fInputMin = 50f;
		float fInputMax = 100f;
		float num3 = OrwConversions.ScaleValueFloat(fH, fInputMin, fInputMax, fScaleMin, fScaleMax);
		return (int)num3;
	}

	private void WeatherSetLighting()
	{
		int nHourCurrent = this.gTime.nHourCurrent;
		float fValue = this.fCloudDensity;
		int num = this.nLevel;
		float num2 = this.fLightIntensityDay;
		float num3 = this.fLightIntensityNight;
		float num4 = this.fLightIntensityPrevious;
		float fInputMin = this.fCloudDensityMin;
		float fInputMax = this.fCloudDensityMax;
		float num5 = this.fLightIntensityVDIP;
		bool flag = this.bLightIntensityVDIP_MODE;
		bool flag2 = this.bLightIntensityVDIP_TOG;
		bool flag3 = false;
		float fScaleMax = this.fLightIntensityDayMin;
		float fScaleMin = this.fLightIntensityDayMax;
		float num6 = OrwConversions.ScaleValueFloat(fValue, fInputMin, fInputMax, fScaleMin, fScaleMax);
		float fScaleMax2 = this.fLightIntensityNightMin;
		float fScaleMin2 = this.fLightIntensityNightMax;
		float num7 = OrwConversions.ScaleValueFloat(fValue, fInputMin, fInputMax, fScaleMin2, fScaleMax2);
		if (num < 2 && num > 3)
		{
			flag3 = false;
			this.fLightIntensityVDIPCyclePrev = 0f;
		}
		if (nHourCurrent >= 6 && nHourCurrent <= 18)
		{
			if (this.fLightIntensityVDIPCyclePrev > this.fLightIntensityVDIPCycle)
			{
				flag3 = true;
				this.fLightIntensityVDIPCycle = OrwRandom.GetRandomFloat(this.fLightIntensityVDIPCycleMin, this.fLightIntensityVDIPCycleMax, 1, 0);
			}
			if (flag3)
			{
				this.fLightIntensityVDIPCyclePrev = 0f;
			}
			if (num >= 2 && num <= 3 && !flag && flag3)
			{
				int randomInteger = OrwRandom.GetRandomInteger(1, 100, 1, 0);
				if (randomInteger > 90)
				{
					this.fLightIntensityPrevious = this.fLightIntensityDay;
					num4 = this.fLightIntensityPrevious;
					this.bLightIntensityVDIP_MODE = true;
				}
			}
			if (flag)
			{
				float num8 = num4 - num5;
				if ((double)num4 < 0.1)
				{
					this.fLightIntensityPrevious = this.fLightIntensityDay;
					num4 = this.fLightIntensityPrevious;
				}
				if (num2 <= num4 && !flag2)
				{
					num2 -= 0.005f;
					this.fLightIntensityDay = num2;
				}
				if (num2 <= num8 && !flag2)
				{
					this.bLightIntensityVDIP_TOG = true;
				}
				if (flag2)
				{
					num2 += 0.005f;
					this.fLightIntensityDay = num2;
				}
				if (num2 >= num4)
				{
					this.bLightIntensityVDIP_MODE = false;
					this.bLightIntensityVDIP_TOG = false;
					this.fLightIntensityDay = num6;
					this.fLightIntensityNight = num7;
				}
				this.fLightIntensityDay = num2;
				this.fLightIntensityNight = num7;
			}
			else
			{
				this.fLightIntensityDay = num6;
				this.fLightIntensityNight = num7;
			}
		}
		if ((nHourCurrent > 18 && nHourCurrent <= 23) || (nHourCurrent >= 0 && nHourCurrent < 6))
		{
			if (num >= 2 && num <= 3 && !flag)
			{
				int randomInteger2 = OrwRandom.GetRandomInteger(1, 100, 1, 0);
				if (randomInteger2 > 50)
				{
					this.fLightIntensityPrevious = this.fLightIntensityNight;
					num4 = this.fLightIntensityPrevious;
					this.bLightIntensityVDIP_MODE = true;
				}
			}
			if (flag)
			{
				float num9 = num4 - num5;
				if ((double)num4 < 0.1)
				{
					this.fLightIntensityPrevious = this.fLightIntensityNight;
					num4 = this.fLightIntensityPrevious;
				}
				if (num3 <= num4 && !flag2)
				{
					num3 -= 0.005f;
					this.fLightIntensityNight = num3;
				}
				if (num3 <= num9 && !flag2)
				{
					this.bLightIntensityVDIP_TOG = true;
				}
				if (flag2)
				{
					num3 += 0.005f;
					this.fLightIntensityNight = num3;
				}
				if (num3 >= num4)
				{
					this.bLightIntensityVDIP_MODE = false;
					this.bLightIntensityVDIP_TOG = false;
					this.fLightIntensityDay = num6;
					this.fLightIntensityNight = num7;
				}
				this.fLightIntensityDay = num6;
				this.fLightIntensityNight = num3;
			}
			else
			{
				this.fLightIntensityDay = num6;
				this.fLightIntensityNight = num7;
			}
		}
	}

	public static int WeatherSetFogGround(float fH, float fT, int nICF, int nHour, int nGO, int nDO, float fTMin, float fTMax, float fIMin, float fIMax, float fSMin, float fSMax)
	{
		float fVal = 0f;
		int num = OrwConversions.FloatToInt(fVal);
		if (nHour >= 0 && nHour <= 6 && fH >= 20f && nGO == 0 && nICF < 1 && fT >= fTMin && fT <= fTMax)
		{
			fVal = OrwConversions.ScaleValueFloat(fH, fIMin, fIMax, fSMin, fSMax);
		}
		if (((nHour >= 18 && nHour <= 23) || (nHour >= 0 && nHour < 6 && fH >= 20f && nGO == 0)) && nICF < 1 && fT >= fTMin && fT <= fTMax)
		{
			fVal = OrwConversions.ScaleValueFloat(fH, fIMin, fIMax, fSMin, fSMax);
		}
		if (nGO == 1)
		{
		}
		return OrwConversions.FloatToInt(fVal);
	}

	public static Color32 WeatherSetFogColor(float fH, float fT, int nTZ, int nPZ)
	{
		Color32 result = new Color32(204, 204, 204, 255);
		if (nTZ > 3)
		{
			if (fT < 0f)
			{
			}
			if (fT < 10f)
			{
			}
			if (fT < 10f)
			{
			}
		}
		if (nTZ < 2)
		{
			if (fT >= 1.5f)
			{
			}
			if (fT < 1.5f)
			{
			}
		}
		return result;
	}
}
