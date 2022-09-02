using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAtWeather : MonoBehaviour
{
	public TOD_Sky sky;

	public TOD_WeatherType type;

	public float fadeTime = 1f;

	private float lerpTime;

	private AudioSource audioComponent;

	private float audioVolume;

	protected void Start()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.audioComponent = base.GetComponent<AudioSource>();
		this.audioVolume = this.audioComponent.volume;
		if (this.sky.Components.Weather.Weather != this.type)
		{
			this.audioComponent.volume = 0f;
		}
	}

	protected void Update()
	{
		int num = (this.sky.Components.Weather.Weather != this.type) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.audioComponent.volume = Mathf.Lerp(0f, this.audioVolume, this.lerpTime);
	}
}
