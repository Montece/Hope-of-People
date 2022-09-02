using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	public AudioClip[] Trecks;

	private AudioSource source;

	public bool IsPlaying;

	[Range(0f, 1f)]
	public float volume = 1f;

	private void Start()
	{
		this.source = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (this.source.volume != this.volume)
		{
			this.source.volume = this.volume;
		}
		if (!this.source.isPlaying)
		{
			this.IsPlaying = false;
		}
		if (!this.IsPlaying)
		{
			AudioClip clip = this.Trecks[UnityEngine.Random.Range(0, this.Trecks.Length)];
			this.source.clip = clip;
			this.source.Play();
			this.IsPlaying = true;
		}
	}
}
