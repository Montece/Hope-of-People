using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public Action CheckpointActivated;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter()
	{
		Debug.Log("Activate");
		if (this.CheckpointActivated != null)
		{
			this.CheckpointActivated();
		}
	}
}
