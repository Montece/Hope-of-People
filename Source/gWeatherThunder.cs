using gWeatherFunctions;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class gWeatherThunder : MonoBehaviour
{
	public bool bTestAudio;

	public int nTestType = 1;

	private gWeatherMaster gWeatherMaster;

	public AudioClip[] acThunder;

	public AudioSource asThunder;

	public float fWeatherThunderLength = 11f;

	public float fWeatherThunderTimer;

	public int nTT;

	public int nThunder;

	public int nThunderHold;

	public int nThunderAudio;

	public int nTHC5 = 80;

	public int nTHC6 = 60;

	public bool bProcess;

	public int nICF;

	public int nWL;

	public float fT;

	public float fH;

	private void Start()
	{
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
		this.asThunder.enabled = false;
	}

	private void Update()
	{
		if (this.bProcess)
		{
			this.nICF = this.gWeatherMaster.nInColdFront;
			this.nWL = this.gWeatherMaster.nLevel;
			this.fT = this.gWeatherMaster.fTemperatureC;
			this.fH = this.gWeatherMaster.fHumidityPercent;
			if (this.fT > 0f && this.nICF == 1)
			{
				this.nThunder = OrwRandom.GetRandomInteger(1, 100, 1, 0);
			}
			this.bProcess = false;
		}
		if (this.bTestAudio && this.nThunderHold == 0)
		{
			this.nTT = this.nTestType;
			this.nThunderHold = 1;
			this.bTestAudio = false;
		}
		if (this.gWeatherMaster.nLevel == 5 && this.nThunderHold == 0 && this.nThunderHold == 0 && this.nThunder > this.nTHC5)
		{
			this.nThunderHold = 1;
			this.nTT = OrwRandom.GetRandomInteger(1, 9, 1, 0);
		}
		if (this.gWeatherMaster.nLevel == 6 && this.nThunderHold == 0 && this.nThunderHold == 0 && this.nThunder > this.nTHC6)
		{
			this.nThunderHold = 1;
			this.nTT = OrwRandom.GetRandomInteger(1, 9, 1, 0);
		}
		this.nThunder = 0;
		if (this.nTT > 0 && this.nThunderAudio == 0)
		{
			this.nThunderAudio = 1;
			this.asThunder.clip = this.acThunder[this.nTT - 1];
			this.asThunder.enabled = true;
			base.GetComponent<AudioSource>().PlayOneShot(this.asThunder.clip);
		}
		if (this.nThunderHold > 0)
		{
			this.fWeatherThunderTimer += Time.deltaTime;
			if (this.fWeatherThunderTimer >= this.fWeatherThunderLength)
			{
				this.nTT = 0;
				this.nThunderHold = 0;
				this.nThunderAudio = 0;
				this.fWeatherThunderTimer = 0f;
				this.asThunder.enabled = false;
			}
		}
	}
}
