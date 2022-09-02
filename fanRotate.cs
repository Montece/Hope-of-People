using System;
using UnityEngine;

public class fanRotate : MonoBehaviour
{
	public Vector3 rotationSpeed = new Vector3(0f, 0f, 0f);

	private void Update()
	{
		base.transform.Rotate(this.rotationSpeed * Time.deltaTime);
	}
}
