using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class gWeatherRain : MonoBehaviour
{
	public bool bTestAudio;

	public int nTestType;

	private gWeatherMaster gWeatherMaster;

	public ParticleSystem Rain;

	public AudioClip[] acRain;

	public AudioSource asRain;

	public float fRainVol;

	public float fRain1Vol = 0.35f;

	public float fRain2Vol = 0.65f;

	public int nRainVolStage = 1;

	private float fRainVolS1;

	private float fRainVolS2;

	private float fRainVolS3;

	public int nRT;

	public int nRainHold;

	public int nRainAudio;

	public float fWeatherRainLength = 9.69f;

	public float fWeatherRainTimer;

	public bool bProcess;

	public int nICF;

	public int nWL;

	public float fT;

	public float fH;

	public int nRainEmissionRate;

	private void Start()
	{
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
		this.Rain.enableEmission = false;
		this.asRain.enabled = false;
	}

	private void Update()
	{
		if (this.bProcess)
		{
			this.nICF = this.gWeatherMaster.nInColdFront;
			this.nWL = this.gWeatherMaster.nLevel;
			this.fT = this.gWeatherMaster.fTemperatureC;
			this.fH = this.gWeatherMaster.fHumidityPercent;
			this.nRainEmissionRate = this.gWeatherMaster.nRainEmissionRate;
			if (this.fT > 0f && this.nICF == 1)
			{
				this.Rain.enableEmission = true;
				this.Rain.emissionRate = (float)this.nRainEmissionRate;
				this.asRain.enabled = true;
			}
			if (this.fT > 0f && this.nICF == 0)
			{
				this.Rain.enableEmission = false;
				this.Rain.emissionRate = 0f;
				this.nRainVolStage = 1;
				this.asRain.enabled = false;
			}
			if (this.fT < 0f)
			{
				this.Rain.enableEmission = false;
				this.Rain.emissionRate = 0f;
				this.nRainVolStage = 1;
				this.asRain.enabled = false;
			}
			this.bProcess = false;
		}
		if (this.bTestAudio && this.nRainHold == 0)
		{
			this.nRT = this.nTestType;
			this.nRainHold = 1;
			this.bTestAudio = false;
		}
		if (this.Rain.enableEmission && this.nRainHold == 0)
		{
			this.nRainHold = 1;
		}
		if (this.nRT > 0 && this.nRainAudio == 0)
		{
			this.nRainAudio = 1;
			this.asRain.clip = this.acRain[this.nRT - 1];
			this.asRain.enabled = true;
			base.GetComponent<AudioSource>().PlayOneShot(this.asRain.clip, this.fRainVol);
		}
		if (this.gWeatherMaster.nLevel <= 3)
		{
			this.nRT = 1;
			this.fRainVolS1 = this.fRain1Vol * 0.25f;
			this.fRainVolS2 = this.fRain1Vol * 0.5f;
			this.fRainVolS3 = this.fRain1Vol * 0.75f;
			if (this.nRainVolStage == 1)
			{
				this.fRainVol = this.fRainVolS1;
			}
			if (this.nRainVolStage == 2)
			{
				this.fRainVol = this.fRainVolS2;
			}
			if (this.nRainVolStage == 3)
			{
				this.fRainVol = this.fRainVolS3;
			}
			if (this.nRainVolStage == 4)
			{
				this.fRainVol = this.fRain1Vol;
			}
		}
		if (this.gWeatherMaster.nLevel >= 4)
		{
			this.nRT = 2;
			this.fRainVolS1 = this.fRain2Vol * 0.25f;
			this.fRainVolS2 = this.fRain2Vol * 0.5f;
			this.fRainVolS3 = this.fRain2Vol * 0.75f;
			if (this.nRainVolStage == 1)
			{
				this.fRainVol = this.fRainVolS1;
			}
			if (this.nRainVolStage == 2)
			{
				this.fRainVol = this.fRainVolS2;
			}
			if (this.nRainVolStage == 3)
			{
				this.fRainVol = this.fRainVolS3;
			}
			if (this.nRainVolStage == 4)
			{
				this.fRainVol = this.fRain2Vol;
			}
		}
		if (this.nRainHold > 0)
		{
			this.fWeatherRainTimer += Time.deltaTime;
			if (this.fWeatherRainTimer >= this.fWeatherRainLength)
			{
				this.nRT = 0;
				this.nRainHold = 0;
				this.nRainAudio = 0;
				this.fWeatherRainTimer = 0f;
				if (this.nRainVolStage < 5)
				{
					this.nRainVolStage++;
				}
			}
		}
	}
}
