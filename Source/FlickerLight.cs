using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
	public float lampSpeed = 0.1f;

	public float intens_Speed = 9f;

	public bool timung;

	public float minIntens = 0.8f;

	public float maxIntens = 3.5f;

	public bool loopEnd;

	public float range_Speed = 12f;

	public float minRange = 2.8f;

	public float maxRange = 13.5f;

	public Color col_Main = Color.white;

	public float col_Speed = 1.5f;

	public Color col_Blend1 = Color.yellow;

	public Color col_Blend2 = Color.red;

	private Color refCol;

	private float intens;

	private float randomIntens;

	private float range;

	private float randomRange;

	private GameObject lamp;

	private void Start()
	{
		this.lamp = base.gameObject;
		this.intens = this.lamp.GetComponent<Light>().intensity;
		this.range = this.lamp.GetComponent<Light>().range;
		this.lamp.GetComponent<Light>().color = this.col_Main;
		base.StartCoroutine(this.Timer());
	}

	private void LateUpdate()
	{
		if (this.loopEnd)
		{
			base.StartCoroutine(this.Timer());
		}
		this.intens = Mathf.SmoothStep(this.intens, this.randomIntens, Time.deltaTime * this.intens_Speed);
		this.range = Mathf.SmoothStep(this.range, this.randomRange, Time.deltaTime * this.range_Speed);
		this.lamp.GetComponent<Light>().intensity = this.intens;
		this.lamp.GetComponent<Light>().range = this.range;
		this.col_Main = Color.Lerp(this.col_Main, this.refCol, Time.deltaTime * this.col_Speed);
		this.lamp.GetComponent<Light>().color = this.col_Main;
	}

	[DebuggerHidden]
	private IEnumerator Timer()
	{
		FlickerLight.<Timer>c__Iterator0 <Timer>c__Iterator = new FlickerLight.<Timer>c__Iterator0();
		<Timer>c__Iterator.$this = this;
		return <Timer>c__Iterator;
	}
}
