using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNew : MonoBehaviour
{
	public GameManager manager;

	public Animator CameraObject;

	public GameObject PanelControls;

	public GameObject PanelVideo;

	public GameObject PanelGame;

	public GameObject PanelKeyBindings;

	public GameObject PanelMovement;

	public GameObject PanelCombat;

	public GameObject PanelGeneral;

	public AudioSource hoverSound;

	public AudioSource sfxhoversound;

	public AudioSource clickSound;

	public GameObject areYouSure;

	public GameObject continueBtn;

	public GameObject newGameBtn;

	public GameObject lineGame;

	public GameObject lineVideo;

	public GameObject lineControls;

	public GameObject lineKeyBindings;

	public GameObject lineMovement;

	public GameObject lineCombat;

	public GameObject lineGeneral;

	public void PlayCampaign()
	{
		this.areYouSure.SetActive(false);
		this.continueBtn.SetActive(true);
		this.newGameBtn.SetActive(true);
	}

	public void DisablePlayCampaign()
	{
		this.continueBtn.SetActive(false);
		this.newGameBtn.SetActive(false);
	}

	public void Position2()
	{
		this.DisablePlayCampaign();
		this.CameraObject.SetFloat("Animate", 1f);
	}

	public void Position1()
	{
		this.CameraObject.SetFloat("Animate", 0f);
	}

	public void GamePanel()
	{
		this.PanelControls.SetActive(false);
		this.PanelVideo.SetActive(false);
		this.PanelGame.SetActive(true);
		this.PanelKeyBindings.SetActive(false);
		this.lineGame.gameObject.SetActive(true);
		this.lineControls.gameObject.SetActive(false);
		this.lineVideo.gameObject.SetActive(false);
		this.lineKeyBindings.gameObject.SetActive(false);
	}

	public void VideoPanel()
	{
		this.PanelControls.SetActive(false);
		this.PanelVideo.SetActive(true);
		this.PanelGame.SetActive(false);
		this.PanelKeyBindings.SetActive(false);
		this.lineGame.SetActive(false);
		this.lineControls.SetActive(false);
		this.lineVideo.SetActive(true);
		this.lineKeyBindings.SetActive(false);
	}

	public void ControlsPanel()
	{
		this.PanelControls.SetActive(true);
		this.PanelVideo.SetActive(false);
		this.PanelGame.SetActive(false);
		this.PanelKeyBindings.SetActive(false);
		this.lineGame.SetActive(false);
		this.lineControls.SetActive(true);
		this.lineVideo.SetActive(false);
		this.lineKeyBindings.SetActive(false);
	}

	public void KeyBindingsPanel()
	{
		this.PanelControls.SetActive(false);
		this.PanelVideo.SetActive(false);
		this.PanelGame.SetActive(false);
		this.PanelKeyBindings.SetActive(true);
		this.lineGame.SetActive(false);
		this.lineControls.SetActive(false);
		this.lineVideo.SetActive(false);
		this.lineKeyBindings.SetActive(true);
	}

	public void MovementPanel()
	{
		this.PanelMovement.SetActive(true);
		this.PanelCombat.SetActive(false);
		this.PanelGeneral.SetActive(false);
		this.lineMovement.SetActive(true);
		this.lineCombat.SetActive(false);
		this.lineGeneral.SetActive(false);
	}

	public void CombatPanel()
	{
		this.PanelMovement.SetActive(false);
		this.PanelCombat.SetActive(true);
		this.PanelGeneral.SetActive(false);
		this.lineMovement.SetActive(false);
		this.lineCombat.SetActive(true);
		this.lineGeneral.SetActive(false);
	}

	public void GeneralPanel()
	{
		this.PanelMovement.SetActive(false);
		this.PanelCombat.SetActive(false);
		this.PanelGeneral.SetActive(true);
		this.lineMovement.SetActive(false);
		this.lineCombat.SetActive(false);
		this.lineGeneral.SetActive(true);
	}

	public void PlayHover()
	{
		this.hoverSound.Play();
	}

	public void PlaySFXHover()
	{
		this.sfxhoversound.Play();
	}

	public void PlayClick()
	{
		this.clickSound.Play();
	}

	public void AreYouSure()
	{
		this.areYouSure.SetActive(true);
		this.DisablePlayCampaign();
	}

	public void No()
	{
		this.areYouSure.SetActive(false);
	}

	public void Yes()
	{
		Application.Quit();
	}

	public void NewGame()
	{
		SceneManager.LoadScene("Flat");
	}

	public void LoadGame()
	{
		this.manager.Load();
	}
}
