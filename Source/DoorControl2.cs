using System;
using UnityEngine;

public class DoorControl2 : MonoBehaviour
{
	private bool isOpen;

	public float closeAfter = 5f;

	private float timer;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.isOpen && this.timer < Time.time)
		{
			base.GetComponent<Animation>().Play("DoorClose2");
			this.isOpen = false;
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerEnter(Collider target)
	{
		if (!this.isOpen)
		{
			base.GetComponent<Animation>().Play("DoorOpen2");
			this.timer = Time.time + this.closeAfter;
			this.isOpen = true;
			base.GetComponent<AudioSource>().Play();
		}
	}
}
