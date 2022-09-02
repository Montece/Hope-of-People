using System;
using UnityEngine;

public class gWeatherGroundFog : MonoBehaviour
{
	public bool bTestFog;

	public int nTestEmissionAmount = 100;

	private gWeatherMaster gWeatherMaster;

	public ParticleSystem GroundFog;

	public int nEFog;

	public bool bProcess;

	private void Start()
	{
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
	}

	private void Update()
	{
		if (this.bProcess)
		{
			int nFogDensity = this.gWeatherMaster.nFogDensity;
			if (nFogDensity > 0 && !this.bTestFog)
			{
				this.GroundFog.enableEmission = true;
				this.GroundFog.emissionRate = (float)nFogDensity;
			}
			if (nFogDensity < 1 && !this.bTestFog)
			{
				this.GroundFog.enableEmission = false;
				this.GroundFog.emissionRate = 0f;
			}
			if (this.bTestFog)
			{
				this.GroundFog.enableEmission = true;
				this.GroundFog.emissionRate = (float)this.nTestEmissionAmount;
			}
			this.bProcess = false;
		}
	}
}
