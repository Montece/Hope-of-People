using gWeatherFunctions;
using System;
using UnityEngine;

public class gWeatherLightning : MonoBehaviour
{
	public bool bTestLaF;

	public bool bTestL;

	public bool bTestF;

	public int nTestType = 1;

	private gWeatherGameData gWeatherGameData;

	private gWeatherMaster gWeatherMaster;

	private TOD_Sky Sky;

	public ParticleSystem[] Lightning;

	public GameObject LightningFlash;

	public bool bProcess;

	public float fWeatherLightningFlashLength = 0.5f;

	public float fWeatherLightningFlashTimer;

	public float fLightningFlashIntensityMin = 0.3f;

	public float fLightningFlashIntensityMax = 0.75f;

	public float fLightningFlashDelay = 0.35f;

	public float fLightningFlashDelayTimer;

	public float fLightningFlashLengthMin = 0.3f;

	public float fLightningFlashLengthMax = 0.85f;

	public int nLF;

	public int nFlash;

	private int nLFHold;

	public float fLFI = 0.5f;

	public int nLFC5 = 80;

	public int nLFC6 = 60;

	public float fWeatherLightningLength = 1.25f;

	public float fWeatherLightningTimer;

	public int nLS;

	public int nStrike;

	public int nStrikeRandom;

	public int nLSC5 = 90;

	public int nLSC6 = 70;

	private void Start()
	{
		this.gWeatherGameData = GameObject.Find("_GameData").GetComponent<gWeatherGameData>();
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
		this.Sky = GameObject.Find("Sky Dome").GetComponent<TOD_Sky>();
	}

	private void Update()
	{
		if (this.bProcess)
		{
			this.nFlash = OrwRandom.GetRandomInteger(1, 100, 1, 0);
			this.nStrike = OrwRandom.GetRandomInteger(1, 100, 1, 0);
			this.bProcess = false;
		}
		if (this.gWeatherMaster.nLevel == 5 && this.nLS == 0 && this.nStrike > this.nLSC5)
		{
			this.nLS = OrwRandom.GetRandomInteger(1, 3, 1, 0);
			this.nLF = this.nLS;
		}
		if (this.gWeatherMaster.nLevel == 6 && this.nLS == 0 && this.nStrike > this.nLSC6)
		{
			this.nLS = OrwRandom.GetRandomInteger(1, 4, 1, 0);
			this.nLF = this.nLS;
		}
		this.nFlash = 0;
		this.nStrike = 0;
		if (this.nLF > 0)
		{
			this.fLightningFlashDelayTimer += Time.deltaTime;
			if (this.fLightningFlashDelayTimer >= this.fLightningFlashDelay)
			{
				this.fWeatherLightningFlashTimer += Time.deltaTime;
				if (this.nLFHold == 0)
				{
					this.nLFHold = 1;
					this.fLFI = OrwRandom.GetRandomFloat(this.fLightningFlashIntensityMin, this.fLightningFlashIntensityMax, 1, 0);
					this.fWeatherLightningFlashLength = OrwRandom.GetRandomFloat(this.fLightningFlashLengthMin, this.fLightningFlashLengthMax, 1, 0);
					this.LightningFlash.GetComponent<Light>().intensity = this.fLFI;
					this.LightningFlash.GetComponent<Light>().enabled = true;
					this.Sky.Clouds.Brightness = this.fLFI;
				}
				if (this.fWeatherLightningFlashTimer >= this.fWeatherLightningFlashLength)
				{
					this.nLFHold = 0;
					this.nLF--;
					if (this.nLF < 0)
					{
						this.nLF = 0;
					}
					this.fLightningFlashDelayTimer = 0f;
					this.fWeatherLightningFlashTimer = 0f;
					this.LightningFlash.GetComponent<Light>().enabled = false;
					this.Sky.Clouds.Brightness = this.gWeatherMaster.fCloudBrightness;
				}
			}
		}
		if (this.nLS > 0)
		{
			this.fWeatherLightningTimer += Time.deltaTime;
			if (this.fWeatherLightningTimer >= this.fWeatherLightningLength)
			{
				this.nLS--;
				if (this.nLS < 0)
				{
					this.nLS = 0;
				}
				this.fWeatherLightningTimer = 0f;
				this.Lightning[0].enableEmission = false;
				this.Lightning[1].enableEmission = false;
				this.Lightning[2].enableEmission = false;
			}
		}
		if (this.nLS > 0)
		{
			this.Lightning[this.nLS - 1].enableEmission = true;
		}
		if (this.bTestLaF)
		{
			this.nLS = this.nTestType;
			this.nLF = 1;
			this.bTestLaF = false;
		}
		if (this.bTestF)
		{
			this.nLF = 1;
			this.bTestF = false;
		}
		if (this.bTestL)
		{
			this.nLS = this.nTestType;
			this.bTestL = false;
		}
	}
}
