using System;
using UnityEngine;

public class Explode : MonoBehaviour
{
	private void Start()
	{
		float radius = 4f;
		Collider[] array = Physics.OverlapSphere(base.transform.position, radius);
		Collider[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Collider collider = array2[i];
			if (collider.GetComponent<AnimalAI>())
			{
				collider.GetComponent<AnimalAI>().GetDamage(null, 1000f);
			}
			if (collider.GetComponent<RobotAI>())
			{
				collider.GetComponent<RobotAI>().GetDamage(null, 1000f);
			}
			if (collider.GetComponent<Construction>())
			{
				collider.GetComponent<Construction>().GetDamage(1000f);
			}
			if (collider.GetComponent<Player>())
			{
				collider.GetComponent<Player>().GetDamage(DamageType.Body, 70f);
			}
		}
	}
}
