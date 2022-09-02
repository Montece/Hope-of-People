using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class gWeatherWind : MonoBehaviour
{
	public int nTestAudio;

	private gWeatherMaster gWeatherMaster;

	public bool bProcess;

	private void Start()
	{
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
	}

	private void Update()
	{
		if (this.bProcess)
		{
			this.bProcess = false;
		}
	}
}
