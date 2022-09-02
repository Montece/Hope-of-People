using System;
using UnityEngine;

public class HangarDoors : MonoBehaviour
{
	public int type;

	public GameObject door;

	private bool isOpen;

	public float closeAfter = 5f;

	private float timer;

	public GameObject targ;

	private void Start()
	{
		this.timer = 0f;
	}

	private void Update()
	{
		if (this.isOpen && this.timer < Time.time && this.targ == null)
		{
			if (this.type == 0)
			{
				this.door.GetComponent<Animation>().Play("HangarDoorClose");
			}
			else if (this.type == 1)
			{
				this.door.GetComponent<Animation>().Play("HangarDoorClose2");
			}
			this.isOpen = false;
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerEnter(Collider target)
	{
		if (!this.isOpen && target.tag == "Player")
		{
			if (this.type == 0)
			{
				this.door.GetComponent<Animation>().Play("HangarDoorOpen");
			}
			else if (this.type == 1)
			{
				this.door.GetComponent<Animation>().Play("HangarDoorOpen2");
			}
			this.targ = target.gameObject;
			this.timer = Time.time + this.closeAfter;
			this.isOpen = true;
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerExit(Collider target)
	{
		if (this.isOpen && target.tag == "Player")
		{
			this.targ = null;
		}
	}
}
