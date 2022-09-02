using System;
using UnityEngine;

public class HeliRotorController : MonoBehaviour
{
	public enum Axis
	{
		X,
		Y,
		Z
	}

	public HeliRotorController.Axis RotateAxis;

	private float _rotarSpeed;

	private float rotateDegree;

	private Vector3 OriginalRotate;

	public float RotarSpeed
	{
		get
		{
			return this._rotarSpeed;
		}
		set
		{
			this._rotarSpeed = Mathf.Clamp(value, 0f, 3000f);
		}
	}

	private void Start()
	{
		this.OriginalRotate = base.transform.localEulerAngles;
	}

	private void Update()
	{
		this.rotateDegree += this.RotarSpeed * Time.deltaTime;
		this.rotateDegree %= 360f;
		HeliRotorController.Axis rotateAxis = this.RotateAxis;
		if (rotateAxis != HeliRotorController.Axis.Y)
		{
			if (rotateAxis != HeliRotorController.Axis.Z)
			{
				base.transform.localRotation = Quaternion.Euler(this.rotateDegree, this.OriginalRotate.y, this.OriginalRotate.z);
			}
			else
			{
				base.transform.localRotation = Quaternion.Euler(this.OriginalRotate.x, this.OriginalRotate.y, this.rotateDegree);
			}
		}
		else
		{
			base.transform.localRotation = Quaternion.Euler(this.OriginalRotate.x, this.rotateDegree, this.OriginalRotate.z);
		}
	}
}
