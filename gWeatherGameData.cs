using System;
using UnityEngine;

public class gWeatherGameData : MonoBehaviour
{
	public int nDebug;

	private gWeatherMaster gWeatherMaster;

	private gTime gTime;

	public int nWeatherInitialized;

	[Tooltip("Weather is Dynamic or Static. Dynamic = 1, Static = 0")]
	public int nWeatherType = 1;

	[Tooltip("Weather Persistent Database use. ON = 1, OFF = 0  (This may be set at any time)")]
	public int nWeatherPersistence = 1;

	[Tooltip("Weather Persistence Save Time. How often to store weather info to database? Only applies if Weather Persistent is ON.")]
	public float fWeatherPersistenceSaveInterval = 30f;

	public float fWeatherPersistentCycleTimer;

	[Tooltip("Weather Database Suffix for this Scene. Scene Name is recommended. Must be Unique in Project. DB File will be => Weather_Suffix")]
	public string sWeatherSceneDatabase = "String";

	public string sWDB = string.Empty;

	private void Start()
	{
		this.gTime = GameObject.Find("_GameData").GetComponent<gTime>();
		this.gWeatherMaster = GameObject.Find("_WeatherMaster").GetComponent<gWeatherMaster>();
	}

	private void Update()
	{
		this.fWeatherPersistentCycleTimer += Time.deltaTime;
		if (!RenderSettings.fog)
		{
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.Linear;
		}
		this.sWDB = "Weather" + this.sWeatherSceneDatabase;
		if (this.nWeatherInitialized == 0)
		{
			if (this.nWeatherPersistence == 1)
			{
			}
			this.nWeatherInitialized = 1;
		}
		if (this.nWeatherInitialized == 1 && this.fWeatherPersistentCycleTimer >= this.fWeatherPersistenceSaveInterval)
		{
			if (this.nWeatherPersistence == 1)
			{
			}
			this.fWeatherPersistentCycleTimer -= this.fWeatherPersistenceSaveInterval;
		}
	}
}
