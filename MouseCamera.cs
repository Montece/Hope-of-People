using System;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{
	public float XSensitivity = 2f;

	public float YSensitivity = 2f;

	public bool clampVerticalRotation = true;

	public float MinimumX = -90f;

	public float MaximumX = 90f;

	public bool smooth;

	public float smoothTime = 5f;

	public bool lockCursor = true;

	private Quaternion m_CharacterTargetRot;

	private Quaternion m_CameraTargetRot;

	private bool m_cursorIsLocked = true;

	public void Init(Transform character, Transform camera)
	{
		this.m_CharacterTargetRot = character.localRotation;
		this.m_CameraTargetRot = camera.localRotation;
	}

	public void LookRotation(Transform character, Transform camera)
	{
		float y = Input.GetAxis("Mouse X") * this.XSensitivity;
		float num = Input.GetAxis("Mouse Y") * this.YSensitivity;
		this.m_CharacterTargetRot *= Quaternion.Euler(0f, y, 0f);
		this.m_CameraTargetRot *= Quaternion.Euler(-num, 0f, 0f);
		if (this.clampVerticalRotation)
		{
			this.m_CameraTargetRot = this.ClampRotationAroundXAxis(this.m_CameraTargetRot);
		}
		if (this.smooth)
		{
			character.localRotation = Quaternion.Slerp(character.localRotation, this.m_CharacterTargetRot, this.smoothTime * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp(camera.localRotation, this.m_CameraTargetRot, this.smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = this.m_CharacterTargetRot;
			camera.localRotation = this.m_CameraTargetRot;
		}
		this.UpdateCursorLock();
	}

	public void SetCursorLock(bool value)
	{
		this.lockCursor = value;
		if (!this.lockCursor)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void UpdateCursorLock()
	{
		if (this.lockCursor)
		{
			this.InternalLockUpdate();
		}
	}

	private void InternalLockUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			this.m_cursorIsLocked = false;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.m_cursorIsLocked = true;
		}
		if (this.m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (!this.m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	private Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float num = 114.59156f * Mathf.Atan(q.x);
		num = Mathf.Clamp(num, this.MinimumX, this.MaximumX);
		q.x = Mathf.Tan(0.008726646f * num);
		return q;
	}
}
