using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderAtNight : MonoBehaviour
{
	public TOD_Sky sky;

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
		this.rendererComponent.enabled = this.sky.IsNight;
	}
}
