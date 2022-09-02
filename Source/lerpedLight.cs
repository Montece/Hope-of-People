using System;
using UnityEngine;

public class lerpedLight : MonoBehaviour
{
	public Color distortColor = Color.white;

	private Color baseColor = Color.white;

	public float blinkFrequency = 1f;

	private float lerper;

	private Light myLight;

	private int direction = 1;

	private void Start()
	{
		this.myLight = base.gameObject.GetComponent<Light>();
		this.baseColor = this.myLight.color;
	}

	private void Update()
	{
		if ((float)this.direction > 0f)
		{
			this.lerper += this.blinkFrequency * Time.deltaTime;
		}
		else if ((float)this.direction < 0f)
		{
			this.lerper -= this.blinkFrequency * Time.deltaTime;
		}
		this.myLight.color = Color.Lerp(this.baseColor, this.distortColor, this.lerper);
		if (this.myLight.color == this.distortColor)
		{
			this.direction = -1;
		}
		if (this.myLight.color == this.baseColor)
		{
			this.direction = 1;
		}
	}
}
