using System;
using UnityEngine;

public class Fire : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.GetComponent<Buffs>().AddBuff(25, 5f);
		}
	}
}
