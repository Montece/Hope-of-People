using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAtDay : MonoBehaviour
{
	public TOD_Sky sky;

	public float fadeTime = 1f;

	private float lerpTime;

	private ParticleSystem particleComponent;

	private float particleEmission;

	protected void Start()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.particleComponent = base.GetComponent<ParticleSystem>();
		this.particleEmission = this.particleComponent.emissionRate;
	}

	protected void Update()
	{
		int num = (!this.sky.IsDay) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.particleComponent.emissionRate = Mathf.Lerp(0f, this.particleEmission, this.lerpTime);
	}
}
