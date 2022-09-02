using System;
using UnityEngine;

public class Plant : MonoBehaviour
{
	public float GrowTimeInSeconds = 1f;

	public float CurrentGrowTime;

	private Vector3 MaxScale;

	private Vector3 CurrentScale;

	private void Start()
	{
		this.MaxScale = base.transform.localScale;
		this.CurrentGrowTime = 0f;
		this.CurrentScale = Vector3.zero;
	}

	private void Update()
	{
		this.CurrentGrowTime += Time.deltaTime;
		if (this.CurrentGrowTime > this.GrowTimeInSeconds)
		{
			this.CurrentGrowTime = this.GrowTimeInSeconds;
		}
		if (this.CurrentGrowTime < 0f)
		{
			this.CurrentGrowTime = 0f;
		}
		this.CurrentScale = this.MaxScale * this.CurrentGrowTime / this.GrowTimeInSeconds;
		base.transform.localScale = this.CurrentScale;
	}
}
