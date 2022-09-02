using System;
using UnityEngine;

public class CheckMusicVolume : MonoBehaviour
{
	public void Start()
	{
		base.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
	}

	public void UpdateVolume()
	{
		base.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
	}
}
