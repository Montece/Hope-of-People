using System;
using UnityEngine;

public class CheckSFXVolume : MonoBehaviour
{
	public void Start()
	{
		base.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume");
	}

	public void UpdateVolume()
	{
		base.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume");
	}
}
