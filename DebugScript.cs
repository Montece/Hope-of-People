using System;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			this.DoDebug();
		}
	}

	private void DoDebug()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		MonoBehaviour.print(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			MonoBehaviour.print(array[i].name);
		}
	}
}
