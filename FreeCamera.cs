using System;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
	private float mainSpeed = 10f;

	private float shiftAdd = 20f;

	private float maxShift = 25f;

	private float camSens = 0.2f;

	private Vector3 lastMouse = new Vector3(255f, 255f, 255f);

	private float totalRun = 1f;

	private void Update()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = false;
		this.lastMouse = Input.mousePosition - this.lastMouse;
		this.lastMouse = new Vector3(-this.lastMouse.y * this.camSens, this.lastMouse.x * this.camSens, 0f);
		this.lastMouse = new Vector3(base.transform.eulerAngles.x + this.lastMouse.x, base.transform.eulerAngles.y + this.lastMouse.y, 0f);
		base.transform.eulerAngles = this.lastMouse;
		this.lastMouse = Input.mousePosition;
		Vector3 vector = this.GetBaseInput();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.totalRun += Time.deltaTime;
			vector = vector * this.totalRun * this.shiftAdd;
			vector.x = Mathf.Clamp(vector.x, -this.maxShift, this.maxShift);
			vector.y = Mathf.Clamp(vector.y, -this.maxShift, this.maxShift);
			vector.z = Mathf.Clamp(vector.z, -this.maxShift, this.maxShift);
		}
		else
		{
			this.totalRun = Mathf.Clamp(this.totalRun * 0.5f, 1f, 1000f);
			vector *= this.mainSpeed;
		}
		vector *= Time.deltaTime;
		Vector3 position = base.transform.position;
		if (Input.GetKey(KeyCode.Space))
		{
			base.transform.Translate(vector);
			position.x = base.transform.position.x;
			position.z = base.transform.position.z;
			base.transform.position = position;
			return;
		}
		base.transform.Translate(vector);
	}

	private Vector3 GetBaseInput()
	{
		Vector3 vector = default(Vector3);
		if (Input.GetKey(KeyCode.W))
		{
			vector += new Vector3(0f, 0f, 1f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			vector += new Vector3(0f, 0f, -1f);
		}
		if (Input.GetKey(KeyCode.A))
		{
			vector += new Vector3(-1f, 0f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			vector += new Vector3(1f, 0f, 0f);
		}
		return vector;
	}
}
