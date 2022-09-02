using System;
using UnityEngine;

[AddComponentMenu("Time of Day/Camera Main Script"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class TOD_Camera : MonoBehaviour
{
	public TOD_Sky sky;

	public bool DomePosToCamera = true;

	public Vector3 DomePosOffset = Vector3.zero;

	public bool DomeScaleToFarClip = true;

	public float DomeScaleFactor = 0.95f;

	private Camera cameraComponent;

	private Transform cameraTransform;

	internal bool HDR
	{
		get
		{
			return this.cameraComponent && this.cameraComponent.allowHDR;
		}
	}

	protected void OnValidate()
	{
		this.DomeScaleFactor = Mathf.Clamp(this.DomeScaleFactor, 0.01f, 1f);
	}

	protected void OnEnable()
	{
		this.cameraComponent = base.GetComponent<Camera>();
		this.cameraTransform = base.GetComponent<Transform>();
		if (!this.sky)
		{
			this.sky = (UnityEngine.Object.FindObjectOfType(typeof(TOD_Sky)) as TOD_Sky);
		}
	}

	protected void Update()
	{
		this.sky.Components.Camera = this;
	}

	protected void OnPreCull()
	{
		if (this.DomeScaleToFarClip)
		{
			this.DoDomeScaleToFarClip();
		}
		if (this.DomePosToCamera)
		{
			this.DoDomePosToCamera();
		}
	}

	public void DoDomeScaleToFarClip()
	{
		if (!this.sky || !this.sky.Initialized)
		{
			return;
		}
		float num = this.DomeScaleFactor * this.cameraComponent.farClipPlane;
		Vector3 localScale = new Vector3(num, num, num);
		this.sky.Components.DomeTransform.localScale = localScale;
	}

	public void DoDomePosToCamera()
	{
		if (!this.sky || !this.sky.Initialized)
		{
			return;
		}
		Vector3 position = this.cameraTransform.position + this.cameraTransform.rotation * this.DomePosOffset;
		this.sky.Components.DomeTransform.position = position;
	}
}
