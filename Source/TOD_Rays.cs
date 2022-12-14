using System;
using UnityEngine;

[AddComponentMenu("Time of Day/Camera God Rays"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class TOD_Rays : TOD_ImageEffect
{
	public enum ResolutionType
	{
		Low,
		Normal,
		High
	}

	public enum BlendModeType
	{
		Screen,
		Add
	}

	public Shader GodRayShader;

	public Shader ScreenClearShader;

	public TOD_Rays.ResolutionType Resolution = TOD_Rays.ResolutionType.Normal;

	public TOD_Rays.BlendModeType BlendMode;

	[TOD_Range(0f, 4f)]
	public int BlurIterations = 2;

	[TOD_Min(0f)]
	public float BlurRadius = 2f;

	[TOD_Min(0f)]
	public float Intensity = 1f;

	[TOD_Min(0f)]
	public float MaxRadius = 0.5f;

	public bool UseDepthTexture = true;

	private Material godRayMaterial;

	private Material screenClearMaterial;

	private const int PASS_DEPTH = 2;

	private const int PASS_NODEPTH = 3;

	private const int PASS_RADIAL = 1;

	private const int PASS_SCREEN = 0;

	private const int PASS_ADD = 4;

	protected void OnEnable()
	{
		this.godRayMaterial = base.CreateMaterial(this.GodRayShader);
		this.screenClearMaterial = base.CreateMaterial(this.ScreenClearShader);
	}

	protected void OnDisable()
	{
		if (this.godRayMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.godRayMaterial);
		}
		if (this.screenClearMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.screenClearMaterial);
		}
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!base.CheckSupport(this.UseDepthTexture, false))
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.sky.Components.Rays = this;
		int width;
		int height;
		int depthBuffer;
		if (this.Resolution == TOD_Rays.ResolutionType.High)
		{
			width = source.width;
			height = source.height;
			depthBuffer = 0;
		}
		else if (this.Resolution == TOD_Rays.ResolutionType.Normal)
		{
			width = source.width / 2;
			height = source.height / 2;
			depthBuffer = 0;
		}
		else
		{
			width = source.width / 4;
			height = source.height / 4;
			depthBuffer = 0;
		}
		Vector3 vector = this.cam.WorldToViewportPoint(this.sky.Components.LightTransform.position);
		this.godRayMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.BlurRadius);
		this.godRayMaterial.SetVector("_LightPosition", new Vector4(vector.x, vector.y, vector.z, this.MaxRadius));
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer);
		if (this.UseDepthTexture)
		{
			Graphics.Blit(source, temporary, this.godRayMaterial, 2);
		}
		else
		{
			Graphics.Blit(source, temporary, this.godRayMaterial, 3);
		}
		base.DrawBorder(temporary, this.screenClearMaterial);
		float num = this.BlurRadius * 0.00130208337f;
		this.godRayMaterial.SetVector("_BlurRadius4", new Vector4(num, num, 0f, 0f));
		this.godRayMaterial.SetVector("_LightPosition", new Vector4(vector.x, vector.y, vector.z, this.MaxRadius));
		for (int i = 0; i < this.BlurIterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, depthBuffer);
			Graphics.Blit(temporary, temporary2, this.godRayMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary);
			num = this.BlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
			this.godRayMaterial.SetVector("_BlurRadius4", new Vector4(num, num, 0f, 0f));
			temporary = RenderTexture.GetTemporary(width, height, depthBuffer);
			Graphics.Blit(temporary2, temporary, this.godRayMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary2);
			num = this.BlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
			this.godRayMaterial.SetVector("_BlurRadius4", new Vector4(num, num, 0f, 0f));
		}
		Vector4 value = ((double)vector.z < 0.0) ? Vector4.zero : (this.Intensity * this.sky.RayColor);
		this.godRayMaterial.SetVector("_LightColor", value);
		this.godRayMaterial.SetTexture("_ColorBuffer", temporary);
		if (this.BlendMode == TOD_Rays.BlendModeType.Screen)
		{
			Graphics.Blit(source, destination, this.godRayMaterial, 0);
		}
		else
		{
			Graphics.Blit(source, destination, this.godRayMaterial, 4);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}
}
