using System;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
	private Buffs buffs;

	private void Start()
	{
		this.buffs = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Buffs>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			this.buffs.AddBuff(26, 15f);
		}
	}
}
