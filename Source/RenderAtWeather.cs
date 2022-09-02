using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderAtWeather : MonoBehaviour
{
	public TOD_Sky sky;

	public TOD_WeatherType type;

	private Renderer rendererComponent;

	protected void Start()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.rendererComponent = base.GetComponent<Renderer>();
	}

	protected void Update()
	{
		this.rendererComponent.enabled = (this.sky.Components.Weather.Weather == this.type);
	}
}
