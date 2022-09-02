using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightAtNight : MonoBehaviour
{
	public TOD_Sky sky;

	public float fadeTime = 1f;

	private float lerpTime;

	private Light lightComponent;

	private float lightIntensity;

	protected void Start()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.lightComponent = base.GetComponent<Light>();
		this.lightIntensity = this.lightComponent.intensity;
	}

	protected void Update()
	{
		int num = (!this.sky.IsNight) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.lightComponent.intensity = Mathf.Lerp(0f, this.lightIntensity, this.lerpTime);
		this.lightComponent.enabled = (this.lightComponent.intensity > 0f);
	}
}
