using System;
using UnityEngine;

[AddComponentMenu("Time of Day/Camera Scattering"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class TOD_Scattering : TOD_ImageEffect
{
	public Shader ScatteringShader;

	public Texture2D DitheringTexture;

	private Material scatteringMaterial;

	protected void OnEnable()
	{
		this.scatteringMaterial = base.CreateMaterial(this.ScatteringShader);
	}

	protected void OnDisable()
	{
		if (this.scatteringMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.scatteringMaterial);
		}
	}

	protected void OnPreCull()
	{
		if (this.sky && this.sky.Initialized)
		{
			this.sky.Components.AtmosphereRenderer.enabled = false;
		}
	}

	protected void OnPostRender()
	{
		if (this.sky && this.sky.Initialized)
		{
			this.sky.Components.AtmosphereRenderer.enabled = true;
		}
	}

	[ImageEffectOpaque]
	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!base.CheckSupport(true, true))
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.sky.Components.Scattering = this;
		float nearClipPlane = this.cam.nearClipPlane;
		float farClipPlane = this.cam.farClipPlane;
		float fieldOfView = this.cam.fieldOfView;
		float aspect = this.cam.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = fieldOfView * 0.5f;
		Vector3 b = this.cam.transform.right * nearClipPlane * Mathf.Tan(num * 0.0174532924f) * aspect;
		Vector3 b2 = this.cam.transform.up * nearClipPlane * Mathf.Tan(num * 0.0174532924f);
		Vector3 vector = this.cam.transform.forward * nearClipPlane - b + b2;
		float d = vector.magnitude * farClipPlane / nearClipPlane;
		vector.Normalize();
		vector *= d;
		Vector3 vector2 = this.cam.transform.forward * nearClipPlane + b + b2;
		vector2.Normalize();
		vector2 *= d;
		Vector3 vector3 = this.cam.transform.forward * nearClipPlane + b - b2;
		vector3.Normalize();
		vector3 *= d;
		Vector3 vector4 = this.cam.transform.forward * nearClipPlane - b - b2;
		vector4.Normalize();
		vector4 *= d;
		identity.SetRow(0, vector);
		identity.SetRow(1, vector2);
		identity.SetRow(2, vector3);
		identity.SetRow(3, vector4);
		this.scatteringMaterial.SetMatrix("_FrustumCornersWS", identity);
		this.scatteringMaterial.SetTexture("_DitheringTexture", this.DitheringTexture);
		base.CustomBlit(source, destination, this.scatteringMaterial, 0);
	}
}
