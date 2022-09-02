using System;
using UnityEngine;

public class gWeatherSnow : MonoBehaviour
{
	private gWeatherMaster gWeatherMaster;

	public ParticleSystem SnowFlake;

	public ParticleSystem SnowIce;

	public bool bProcess;

	public int nICF;

	public int nWL;

	public float fT;

	public float fH;

	public int nSnowEmissionRate;

	private void Start()
	{
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
		this.SnowFlake.enableEmission = false;
		this.SnowIce.enableEmission = false;
	}

	private void Update()
	{
		if (this.bProcess)
		{
			this.nICF = this.gWeatherMaster.nInColdFront;
			this.nWL = this.gWeatherMaster.nLevel;
			this.fT = this.gWeatherMaster.fTemperatureC;
			this.fH = this.gWeatherMaster.fHumidityPercent;
			this.nSnowEmissionRate = this.gWeatherMaster.nSnowEmissionRate;
			if (this.fT < 1.5f && this.nICF == 1)
			{
				this.SnowFlake.enableEmission = true;
				this.SnowFlake.emissionRate = (float)this.nSnowEmissionRate;
				this.SnowIce.enableEmission = true;
				this.SnowIce.emissionRate = (float)this.nSnowEmissionRate;
			}
			if (this.fT < 1.5f && this.nICF == 0)
			{
				this.SnowFlake.enableEmission = false;
				this.SnowFlake.emissionRate = 0f;
				this.SnowIce.enableEmission = false;
				this.SnowIce.emissionRate = 0f;
			}
			if (this.fT > 1.5f)
			{
				this.SnowFlake.enableEmission = false;
				this.SnowFlake.emissionRate = 0f;
				this.SnowIce.enableEmission = false;
				this.SnowIce.emissionRate = 0f;
			}
			this.bProcess = false;
		}
	}
}
