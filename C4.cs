using System;
using UnityEngine;

public class C4 : MonoBehaviour
{
	public GameObject Explode;

	public float Radius = 1f;

	private void Update()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, this.Radius);
		Collider[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Collider collider = array2[i];
			if (collider.GetComponent<AnimalAI>())
			{
				UnityEngine.Object.Instantiate<GameObject>(this.Explode);
			}
			if (collider.GetComponent<RobotAI>())
			{
				UnityEngine.Object.Instantiate<GameObject>(this.Explode);
			}
			if (collider.GetComponent<Construction>())
			{
				UnityEngine.Object.Instantiate<GameObject>(this.Explode);
			}
			if (collider.GetComponent<Player>())
			{
				UnityEngine.Object.Instantiate<GameObject>(this.Explode);
			}
		}
	}
}
