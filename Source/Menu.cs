using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public Slider Sounds;

	public Slider Music;

	public GameObject LoadingLayer;

	public float SoundsProgress;

	public float MusicProgress;

	public bool NeedLoad;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += this.Scene_Loaded();
		this.LoadingLayer.SetActive(false);
	}

	private UnityAction<Scene, LoadSceneMode> Scene_Loaded()
	{
		this.LoadingLayer.SetActive(false);
		return null;
	}

	public void LoadGame_Clicked()
	{
		base.GetComponent<AudioSource>().volume = 0f;
		this.NeedLoad = true;
		this.LoadingLayer.SetActive(true);
		SceneManager.LoadScene("Flat");
	}

	public void NewGame_Clicked()
	{
		this.NeedLoad = false;
		this.LoadingLayer.SetActive(true);
		SceneManager.LoadScene("Flat");
	}

	public void Exit_Clicked()
	{
		Application.Quit();
	}

	public void Sounds_Changed()
	{
		this.SoundsProgress = this.Sounds.value;
	}

	public void Music_Changed()
	{
		this.MusicProgress = this.Music.value;
		base.GetComponent<AudioSource>().volume = this.MusicProgress;
	}
}
