using System;
using UnityEngine;

public class RaditaionZone : MonoBehaviour
{
	private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.GetComponent<AnimalAI>())
		{
			collision.gameObject.GetComponent<AnimalAI>().GetDamage(null, 0.03f);
		}
		if (collision.gameObject.GetComponent<RobotAI>())
		{
			collision.gameObject.GetComponent<RobotAI>().GetDamage(null, 0.03f);
		}
		if (collision.gameObject.GetComponent<PlayerStats>())
		{
			collision.gameObject.GetComponent<PlayerStats>().AddRadiation(0.03f);
		}
	}
}
