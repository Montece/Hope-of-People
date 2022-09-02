using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
	public Text EngineForceView;

	public static UIGameController runtime;

	private void Awake()
	{
		UIGameController.runtime = this;
	}

	private void Start()
	{
		this.ShowInfo();
	}

	private void ShowInfoPanel(bool isShow)
	{
		this.EngineForceView.gameObject.SetActive(!isShow);
	}

	public void ShowInfo()
	{
		this.ShowInfoPanel(true);
	}

	public void HideInfo()
	{
		this.ShowInfoPanel(false);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
