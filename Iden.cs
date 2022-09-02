using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Iden : MonoBehaviour
{
	public GameObject IdenCamera;

	private bool working;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.IdenCamera == null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("Player/FirstPersonCharacter"));
			gameObject.name = "IdenCamera";
			gameObject.AddComponent<FreeCamera>();
			this.IdenCamera = gameObject;
			this.IdenCamera.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			this.working = !this.working;
			base.GetComponentInChildren<FirstPersonController>().enabled = !this.working;
			base.GetComponentInChildren<CharacterController>().enabled = !this.working;
			this.IdenCamera.SetActive(this.working);
			this.IdenCamera.transform.position = base.transform.position;
			this.IdenCamera.transform.rotation = base.transform.GetChild(0).transform.rotation;
			GameObject.Find("UI").GetComponent<Canvas>().enabled = !this.working;
			this.IdenCamera.SetActive(this.working);
			if (this.working)
			{
				Cursor.lockState = CursorLockMode.None;
				return;
			}
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (this.working)
		{
			Cursor.visible = false;
		}
	}
}
