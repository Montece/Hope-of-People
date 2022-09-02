using System;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
	public bool IsWork = true;

	public Transform SecondTeleport;

	public float OffsetY = 1f;

	public float MaximumCooldown = 5f;

	public float CurrentCooldown;

	private Transform subject;

	private AudioSource teleportSound;

	private AudioSource teleportPadSound;

	private void Start()
	{
		this.CurrentCooldown = 0f;
		this.teleportPadSound = base.GetComponents<AudioSource>()[0];
		this.teleportSound = base.GetComponents<AudioSource>()[1];
	}

	private void Update()
	{
		if (this.IsWork)
		{
			if (this.CurrentCooldown == 0f)
			{
				this.Teleport();
			}
			else
			{
				this.CurrentCooldown -= Time.deltaTime;
			}
		}
		if (this.CurrentCooldown < 0f)
		{
			this.CurrentCooldown = 0f;
		}
	}

	private void Teleport()
	{
		if (this.subject != null && this.subject.tag == "Player")
		{
			this.subject.transform.position = this.SecondTeleport.transform.position + new Vector3(0f, this.OffsetY, 0f);
			this.CurrentCooldown = this.MaximumCooldown;
			this.SecondTeleport.GetComponent<Teleporter>().CurrentCooldown = this.MaximumCooldown;
			this.teleportSound.Play();
		}
	}

	private void OnTriggerEnter(Collider trig)
	{
		this.subject = trig.transform;
	}

	private void OnTriggerExit(Collider trig)
	{
		this.subject = null;
	}
}
