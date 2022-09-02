using System;
using UnityEngine;

public class HugeDoor : MonoBehaviour
{
	public int type;

	public GameObject left;

	public GameObject right;

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
				this.left.GetComponent<Animation>().Play("HDoorClose");
				this.right.GetComponent<Animation>().Play("HRDoorClose");
			}
			else if (this.type == 1)
			{
				this.left.GetComponent<Animation>().Play("ELDoorClose");
				this.right.GetComponent<Animation>().Play("ERDoorClose");
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
				this.left.GetComponent<Animation>().Play("HDoorOpen");
				this.right.GetComponent<Animation>().Play("HRDoorOpen");
			}
			else if (this.type == 1)
			{
				this.left.GetComponent<Animation>().Play("ELDoorOpen");
				this.right.GetComponent<Animation>().Play("ERDoorOpen");
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
