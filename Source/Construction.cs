using System;
using UnityEngine;

public class Construction : MonoBehaviour
{
	public float MaximumHealth = 100f;

	public float CurrentHealth = 100f;

	public bool NeedSave;

	private void Start()
	{
		this.CurrentHealth = this.MaximumHealth;
	}

	private void Update()
	{
		if (this.CurrentHealth <= 0f)
		{
			this.DestroyConstructio();
		}
	}

	public void GetDamage(float value)
	{
		this.CurrentHealth -= value;
		if (this.CurrentHealth <= 0f)
		{
			this.DestroyConstructio();
		}
	}

	public void Remove()
	{
		this.CurrentHealth = 0f;
	}

	private void DestroyConstructio()
	{
		if (base.GetComponent<Barrel>())
		{
			base.GetComponent<Barrel>().DestroyBarrel();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
