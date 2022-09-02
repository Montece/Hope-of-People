using System;
using UnityEngine;

public class Door : MonoBehaviour
{
	private bool isOpen;

	public float closeAfter = 5f;

	public float Speed = 1f;

	private float timer;

	private GameObject player;

	public string AnimationOpen = "DoorOpen";

	public string AnimationClose = "DoorClose";

	private Animation anim;

	private AudioSource source;

	private void Start()
	{
		this.anim = base.GetComponent<Animation>();
		this.source = base.GetComponent<AudioSource>();
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.timer = 0f;
	}

	private void Update()
	{
		if (this.isOpen && this.timer < Time.time)
		{
			this.anim[this.AnimationClose].speed = this.Speed;
			this.anim.Play(this.AnimationClose);
			this.source.Play();
			this.isOpen = false;
			if (!this.anim.IsPlaying(this.AnimationClose))
			{
			}
		}
	}

	public void Open()
	{
		if (!this.isOpen)
		{
			this.anim[this.AnimationOpen].speed = this.Speed;
			this.anim.Play(this.AnimationOpen);
			this.timer = Time.time + this.closeAfter;
			this.isOpen = true;
			this.source.Play();
		}
	}

	public void Close(Collider target)
	{
	}
}
