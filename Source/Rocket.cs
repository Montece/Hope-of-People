using System;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	public GameObject Explode;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag != "Player")
		{
			UnityEngine.Object.Instantiate<GameObject>(this.Explode, base.transform.position, Quaternion.identity);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
