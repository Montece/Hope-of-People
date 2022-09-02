using Ceto.Common.Unity.Utility;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ceto
{
	[AddComponentMenu("Ceto/Components/UnderWater"), DisallowMultipleComponent, RequireComponent(typeof(Ocean))]
	public class UnderWater : UnderWaterBase
	{
		[Serializable]
		public struct AbsorptionModifier
		{
			[Range(0f, 50f)]
			public float scale;

			[Range(0f, 10f)]
			public float intensity;

			public Color tint;

			public AbsorptionModifier(float scale, float intensity, Color tint)
			{
				this.scale = scale;
				this.intensity = intensity;
				this.tint = tint;
			}
		}

		public enum INSCATTER_MODE
		{
			LINEAR,
			EXP,
			EXP2
		}

		[Serializable]
		public struct InscatterModifier
		{
			[Range(0f, 5000f)]
			public float scale;

			[Range(0f, 2f)]
			public float intensity;

			public UnderWater.INSCATTER_MODE mode;

			public Color color;

			public InscatterModifier(float scale, float intensity, Color color, UnderWater.INSCATTER_MODE mode)
			{
				this.scale = scale;
				this.intensity = intensity;
				this.color = color;
				this.mode = mode;
			}
		}

		public const float MAX_REFRACTION_INTENSITY = 2f;

		public const float MAX_REFRACTION_DISORTION = 4f;

		public const float MAX_ABSORPTION_INTENSITY = 10f;

		public const float MAX_INSCATTER_INTENSITY = 2f;

		public const CameraEvent FORWARD_EVENT = CameraEvent.AfterDepthTexture;

		public const CameraEvent DEFERRED_EVENT = CameraEvent.AfterLighting;

		public const CameraEvent LEGACY_DEFERRED_EVENT = CameraEvent.AfterLighting;

		private readonly float OCEAN_BOTTOM_DEPTH = 1000f;

		private readonly float MAX_DEPTH_DIST = 500f;

		public UNDERWATER_MODE underwaterMode;

		public DEPTH_MODE depthMode;

		public LayerMask oceanDepthsMask = 1;

		private REFRACTION_RESOLUTION refractionResolution;

		[Range(0f, 1f)]
		public float depthBlend = 0.2f;

		[Range(0f, 1f)]
		public float edgeFade = 0.2f;

		[Range(0f, 1f)]
		public float absorptionR = 0.45f;

		[Range(0f, 1f)]
		public float absorptionG = 0.029f;

		[Range(0f, 1f)]
		public float absorptionB = 0.018f;

		public UnderWater.AbsorptionModifier aboveAbsorptionModifier = new UnderWater.AbsorptionModifier(2f, 1f, Color.white);

		public UnderWater.AbsorptionModifier belowAbsorptionModifier = new UnderWater.AbsorptionModifier(0.1f, 1f, Color.white);

		public UnderWater.AbsorptionModifier subSurfaceScatterModifier = new UnderWater.AbsorptionModifier(10f, 1.5f, new Color32(220, 250, 180, 255));

		public UnderWater.InscatterModifier aboveInscatterModifier = new UnderWater.InscatterModifier(300f, 1f, new Color32(0, 19, 30, 255), UnderWater.INSCATTER_MODE.EXP2);

		public UnderWater.InscatterModifier belowInscatterModifier = new UnderWater.InscatterModifier(60f, 1f, new Color32(1, 37, 58, 255), UnderWater.INSCATTER_MODE.EXP);

		[Range(0f, 2f)]
		public float refractionIntensity = 0.5f;

		[Range(0f, 4f)]
		public float refractionDistortion = 0.5f;

		private IRefractionCommand m_refractionCommand;

		private GameObject m_bottomMask;

		[HideInInspector]
		public Shader oceanBottomSdr;

		[HideInInspector]
		public Shader oceanDepthSdr;

		[HideInInspector]
		public Shader copyDepthSdr;

		[HideInInspector]
		public Shader oceanMaskSdr;

		public override UNDERWATER_MODE Mode
		{
			get
			{
				return this.underwaterMode;
			}
		}

		public override DEPTH_MODE DepthMode
		{
			get
			{
				return this.depthMode;
			}
		}

		private void Start()
		{
			try
			{
				this.m_refractionCommand = new RefractionCommand(this.copyDepthSdr);
				Mesh mesh = this.CreateBottomMesh(32, 512);
				this.m_bottomMask = new GameObject("Ceto Bottom Mask Gameobject");
				MeshFilter meshFilter = this.m_bottomMask.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = this.m_bottomMask.AddComponent<MeshRenderer>();
				NotifyOnWillRender notifyOnWillRender = this.m_bottomMask.AddComponent<NotifyOnWillRender>();
				meshFilter.sharedMesh = mesh;
				meshRenderer.receiveShadows = false;
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
				meshRenderer.material = new Material(this.oceanBottomSdr);
				notifyOnWillRender.AddAction(new Action<GameObject>(this.m_ocean.RenderWaveOverlays));
				notifyOnWillRender.AddAction(new Action<GameObject>(this.m_ocean.RenderOceanMask));
				notifyOnWillRender.AddAction(new Action<GameObject>(this.m_ocean.RenderOceanDepth));
				this.m_bottomMask.layer = LayerMask.NameToLayer(Ocean.OCEAN_LAYER);
				this.m_bottomMask.hideFlags = HideFlags.HideAndDontSave;
				this.UpdateBottomBounds();
				UnityEngine.Object.Destroy(mesh);
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			try
			{
				Shader.EnableKeyword("CETO_UNDERWATER_ON");
				this.SetBottomActive(this.m_bottomMask, true);
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			try
			{
				Shader.DisableKeyword("CETO_UNDERWATER_ON");
				this.SetBottomActive(this.m_bottomMask, false);
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			try
			{
				if (this.m_bottomMask != null)
				{
					Mesh mesh = this.m_bottomMask.GetComponent<MeshFilter>().mesh;
					UnityEngine.Object.Destroy(this.m_bottomMask);
					UnityEngine.Object.Destroy(mesh);
				}
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		private void Update()
		{
			try
			{
				Vector4 vector = new Vector4(this.absorptionR, this.absorptionG, this.absorptionB, 1f);
				Vector4 value = vector;
				Vector4 value2 = vector;
				vector.w = Mathf.Max(0f, this.aboveAbsorptionModifier.scale);
				value.w = Mathf.Max(0f, this.subSurfaceScatterModifier.scale);
				value2.w = Mathf.Max(0f, this.belowAbsorptionModifier.scale);
				Color c = this.aboveAbsorptionModifier.tint * Mathf.Max(0f, this.aboveAbsorptionModifier.intensity);
				Color c2 = this.subSurfaceScatterModifier.tint * Mathf.Max(0f, this.subSurfaceScatterModifier.intensity);
				Color c3 = this.belowAbsorptionModifier.tint * Mathf.Max(0f, this.belowAbsorptionModifier.intensity);
				Shader.SetGlobalVector("Ceto_AbsCof", vector);
				Shader.SetGlobalVector("Ceto_AbsTint", c);
				Shader.SetGlobalVector("Ceto_SSSCof", value);
				Shader.SetGlobalVector("Ceto_SSSTint", c2);
				Shader.SetGlobalVector("Ceto_BelowCof", value2);
				Shader.SetGlobalVector("Ceto_BelowTint", c3);
				Color color = this.aboveInscatterModifier.color;
				color.a = Mathf.Clamp01(this.aboveInscatterModifier.intensity);
				Shader.SetGlobalFloat("Ceto_AboveInscatterScale", Mathf.Max(0.1f, this.aboveInscatterModifier.scale));
				Shader.SetGlobalVector("Ceto_AboveInscatterMode", this.InscatterModeToMask(this.aboveInscatterModifier.mode));
				Shader.SetGlobalVector("Ceto_AboveInscatterColor", color);
				Color color2 = this.belowInscatterModifier.color;
				color2.a = Mathf.Clamp01(this.belowInscatterModifier.intensity);
				Shader.SetGlobalFloat("Ceto_BelowInscatterScale", Mathf.Max(0.1f, this.belowInscatterModifier.scale));
				Shader.SetGlobalVector("Ceto_BelowInscatterMode", this.InscatterModeToMask(this.belowInscatterModifier.mode));
				Shader.SetGlobalVector("Ceto_BelowInscatterColor", color2);
				Shader.SetGlobalFloat("Ceto_RefractionIntensity", Mathf.Max(0f, this.refractionIntensity));
				Shader.SetGlobalFloat("Ceto_RefractionDistortion", this.refractionDistortion * 0.05f);
				Shader.SetGlobalFloat("Ceto_MaxDepthDist", Mathf.Max(0f, this.MAX_DEPTH_DIST));
				Shader.SetGlobalFloat("Ceto_DepthBlend", Mathf.Clamp01(this.depthBlend));
				Shader.SetGlobalFloat("Ceto_EdgeFade", Mathf.Lerp(20f, 2f, Mathf.Clamp01(this.edgeFade)));
				if (this.depthMode == DEPTH_MODE.USE_OCEAN_DEPTH_PASS)
				{
					Shader.EnableKeyword("CETO_USE_OCEAN_DEPTHS_BUFFER");
					if (this.underwaterMode == UNDERWATER_MODE.ABOVE_ONLY)
					{
						this.SetBottomActive(this.m_bottomMask, false);
					}
					else
					{
						this.SetBottomActive(this.m_bottomMask, true);
						this.UpdateBottomBounds();
					}
				}
				else
				{
					Shader.DisableKeyword("CETO_USE_OCEAN_DEPTHS_BUFFER");
					if (this.underwaterMode == UNDERWATER_MODE.ABOVE_ONLY)
					{
						this.SetBottomActive(this.m_bottomMask, false);
					}
					else
					{
						this.SetBottomActive(this.m_bottomMask, true);
						this.UpdateBottomBounds();
					}
				}
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		private void SetBottomActive(GameObject bottom, bool active)
		{
			if (bottom != null)
			{
				bottom.SetActive(active);
			}
		}

		private Vector3 InscatterModeToMask(UnderWater.INSCATTER_MODE mode)
		{
			switch (mode)
			{
			case UnderWater.INSCATTER_MODE.LINEAR:
				return new Vector3(1f, 0f, 0f);
			case UnderWater.INSCATTER_MODE.EXP:
				return new Vector3(0f, 1f, 0f);
			case UnderWater.INSCATTER_MODE.EXP2:
				return new Vector3(0f, 0f, 1f);
			default:
				return new Vector3(0f, 0f, 1f);
			}
		}

		private void FitBottomToCamera()
		{
			if (!base.enabled || this.m_bottomMask == null)
			{
				return;
			}
			Camera current = Camera.current;
			Vector3 position = current.transform.position;
			float num = current.farClipPlane * 0.85f;
			this.m_bottomMask.transform.localScale = new Vector3(num, this.OCEAN_BOTTOM_DEPTH, num);
			float num2 = 0f;
			this.m_bottomMask.transform.localPosition = new Vector3(position.x, -this.OCEAN_BOTTOM_DEPTH + this.m_ocean.level - num2, position.z);
		}

		private void UpdateBottomBounds()
		{
			float num = 1E+08f;
			float level = this.m_ocean.level;
			if (this.m_bottomMask != null && this.m_bottomMask.activeSelf)
			{
				Bounds bounds = new Bounds(new Vector3(0f, level, 0f), new Vector3(num, this.OCEAN_BOTTOM_DEPTH, num));
				this.m_bottomMask.GetComponent<MeshFilter>().mesh.bounds = bounds;
			}
		}

		private LayerMask GetOceanDepthsLayermask(OceanCameraSettings settings)
		{
			return (!(settings != null)) ? this.oceanDepthsMask : settings.oceanDepthsMask;
		}

		private bool GetDisableUnderwater(OceanCameraSettings settings)
		{
			return settings != null && settings.disableUnderwater;
		}

		public override void RenderOceanMask(GameObject go)
		{
			try
			{
				if (base.enabled)
				{
					if (!(this.oceanMaskSdr == null))
					{
						if (this.underwaterMode != UNDERWATER_MODE.ABOVE_ONLY)
						{
							Camera current = Camera.current;
							if (!(current == null))
							{
								CameraData cameraData = this.m_ocean.FindCameraData(current);
								if (cameraData.mask == null)
								{
									cameraData.mask = new MaskData();
								}
								if (!cameraData.mask.updated)
								{
									if (current.name == "SceneCamera" || current.GetComponent<UnderWaterPostEffect>() == null || SystemInfo.graphicsShaderLevel < 30 || this.GetDisableUnderwater(cameraData.settings))
									{
										Shader.SetGlobalTexture("Ceto_OceanMask", Texture2D.blackTexture);
										cameraData.mask.updated = true;
									}
									else
									{
										this.CreateMaskCameraFor(current, cameraData.mask);
										this.FitBottomToCamera();
										NotifyOnEvent.Disable = true;
										cameraData.mask.cam.RenderWithShader(this.oceanMaskSdr, "OceanMask");
										NotifyOnEvent.Disable = false;
										Shader.SetGlobalTexture("Ceto_OceanMask", cameraData.mask.cam.targetTexture);
										cameraData.mask.updated = true;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		public override void RenderOceanDepth(GameObject go)
		{
			try
			{
				if (base.enabled)
				{
					Camera current = Camera.current;
					if (!(current == null))
					{
						CameraData cameraData = this.m_ocean.FindCameraData(current);
						if (cameraData.depth == null)
						{
							cameraData.depth = new DepthData();
						}
						if (!cameraData.depth.updated)
						{
							Shader.SetGlobalTexture(Ocean.REFRACTION_GRAB_TEXTURE_NAME, Texture2D.blackTexture);
							Shader.SetGlobalTexture(Ocean.DEPTH_GRAB_TEXTURE_NAME, Texture2D.whiteTexture);
							Shader.SetGlobalTexture("Ceto_OceanDepth", Texture2D.whiteTexture);
							if (this.GetDisableUnderwater(cameraData.settings))
							{
								Shader.DisableKeyword("CETO_UNDERWATER_ON");
								cameraData.depth.updated = true;
							}
							else
							{
								Shader.EnableKeyword("CETO_UNDERWATER_ON");
								if (current.name == "SceneCamera" || SystemInfo.graphicsShaderLevel < 30)
								{
									Shader.SetGlobalMatrix("Ceto_Camera_IVP", (current.projectionMatrix * current.worldToCameraMatrix).inverse);
									cameraData.depth.updated = true;
								}
								else if (this.depthMode == DEPTH_MODE.USE_DEPTH_BUFFER)
								{
									current.depthTextureMode |= DepthTextureMode.Depth;
									Shader.SetGlobalMatrix("Ceto_Camera_IVP", (current.projectionMatrix * current.worldToCameraMatrix).inverse);
									this.CreateRefractionGrab(current, cameraData.depth);
									cameraData.depth.updated = true;
								}
								else if (this.depthMode == DEPTH_MODE.USE_OCEAN_DEPTH_PASS)
								{
									this.CreateDepthCameraFor(current, cameraData.depth);
									this.CreateRefractionGrab(current, cameraData.depth);
									cameraData.depth.cam.cullingMask = this.GetOceanDepthsLayermask(cameraData.settings);
									cameraData.depth.cam.cullingMask = OceanUtility.HideLayer(cameraData.depth.cam.cullingMask, Ocean.OCEAN_LAYER);
									NotifyOnEvent.Disable = true;
									cameraData.depth.cam.RenderWithShader(this.oceanDepthSdr, "RenderType");
									NotifyOnEvent.Disable = false;
									Shader.SetGlobalTexture("Ceto_OceanDepth", cameraData.depth.cam.targetTexture);
									cameraData.depth.updated = true;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Ocean.LogError(ex.ToString());
				base.WasError = true;
				base.enabled = false;
			}
		}

		private void CreateMaskCameraFor(Camera cam, MaskData data)
		{
			if (data.cam == null)
			{
				GameObject gameObject = new GameObject("Ceto Mask Camera: " + cam.name);
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				gameObject.AddComponent<IgnoreOceanEvents>();
				gameObject.AddComponent<DisableFog>();
				gameObject.AddComponent<DisableShadows>();
				data.cam = gameObject.AddComponent<Camera>();
				data.cam.clearFlags = CameraClearFlags.Color;
				data.cam.backgroundColor = Color.black;
				data.cam.cullingMask = 1 << LayerMask.NameToLayer(Ocean.OCEAN_LAYER);
				data.cam.enabled = false;
				data.cam.renderingPath = RenderingPath.Forward;
				data.cam.targetTexture = null;
				data.cam.useOcclusionCulling = false;
				data.cam.RemoveAllCommandBuffers();
				data.cam.targetTexture = null;
			}
			data.cam.fieldOfView = cam.fieldOfView;
			data.cam.nearClipPlane = cam.nearClipPlane;
			data.cam.farClipPlane = cam.farClipPlane;
			data.cam.transform.position = cam.transform.position;
			data.cam.transform.rotation = cam.transform.rotation;
			data.cam.worldToCameraMatrix = cam.worldToCameraMatrix;
			data.cam.projectionMatrix = cam.projectionMatrix;
			data.cam.orthographic = cam.orthographic;
			data.cam.aspect = cam.aspect;
			data.cam.orthographicSize = cam.orthographicSize;
			data.cam.rect = new Rect(0f, 0f, 1f, 1f);
			if (data.cam.farClipPlane < this.OCEAN_BOTTOM_DEPTH * 2f)
			{
				data.cam.farClipPlane = this.OCEAN_BOTTOM_DEPTH * 2f;
				data.cam.ResetProjectionMatrix();
			}
			RenderTexture targetTexture = data.cam.targetTexture;
			if (targetTexture == null || targetTexture.width != cam.pixelWidth || targetTexture.height != cam.pixelHeight)
			{
				if (targetTexture != null)
				{
					RTUtility.ReleaseAndDestroy(targetTexture);
				}
				int pixelWidth = cam.pixelWidth;
				int pixelHeight = cam.pixelHeight;
				int depth = 32;
				data.cam.targetTexture = new RenderTexture(pixelWidth, pixelHeight, depth, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear);
				data.cam.targetTexture.filterMode = FilterMode.Point;
				data.cam.targetTexture.hideFlags = HideFlags.DontSave;
				data.cam.targetTexture.name = "Ceto Mask Render Target: " + cam.name;
			}
		}

		private void CreateDepthCameraFor(Camera cam, DepthData data)
		{
			if (data.cam == null)
			{
				GameObject gameObject = new GameObject("Ceto Depth Camera: " + cam.name);
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				gameObject.AddComponent<IgnoreOceanEvents>();
				gameObject.AddComponent<DisableFog>();
				gameObject.AddComponent<DisableShadows>();
				data.cam = gameObject.AddComponent<Camera>();
				data.cam.clearFlags = CameraClearFlags.Color;
				data.cam.backgroundColor = Color.white;
				data.cam.enabled = false;
				data.cam.renderingPath = RenderingPath.Forward;
				data.cam.targetTexture = null;
				data.cam.useOcclusionCulling = false;
				data.cam.RemoveAllCommandBuffers();
				data.cam.targetTexture = null;
			}
			data.cam.fieldOfView = cam.fieldOfView;
			data.cam.nearClipPlane = cam.nearClipPlane;
			data.cam.farClipPlane = cam.farClipPlane;
			data.cam.transform.position = cam.transform.position;
			data.cam.transform.rotation = cam.transform.rotation;
			data.cam.worldToCameraMatrix = cam.worldToCameraMatrix;
			data.cam.projectionMatrix = cam.projectionMatrix;
			data.cam.orthographic = cam.orthographic;
			data.cam.aspect = cam.aspect;
			data.cam.orthographicSize = cam.orthographicSize;
			data.cam.rect = new Rect(0f, 0f, 1f, 1f);
			data.cam.layerCullDistances = cam.layerCullDistances;
			data.cam.layerCullSpherical = cam.layerCullSpherical;
			RenderTexture targetTexture = data.cam.targetTexture;
			if (targetTexture == null || targetTexture.width != cam.pixelWidth || targetTexture.height != cam.pixelHeight)
			{
				if (targetTexture != null)
				{
					RTUtility.ReleaseAndDestroy(targetTexture);
				}
				int pixelWidth = cam.pixelWidth;
				int pixelHeight = cam.pixelHeight;
				int depth = 24;
				RenderTextureFormat format;
				if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					format = RenderTextureFormat.RGFloat;
				}
				else
				{
					format = RenderTextureFormat.RGHalf;
				}
				data.cam.targetTexture = new RenderTexture(pixelWidth, pixelHeight, depth, format, RenderTextureReadWrite.Linear);
				data.cam.targetTexture.filterMode = FilterMode.Bilinear;
				data.cam.targetTexture.hideFlags = HideFlags.DontSave;
				data.cam.targetTexture.name = "Ceto Ocean Depths Render Target: " + cam.name;
			}
		}

		private void CreateRefractionGrab(Camera cam, DepthData data)
		{
			IRefractionCommand refractionCommand = this.GetRefractionCommand();
			CameraEvent commandEvent = this.GetCommandEvent(cam);
			if (data.grabCmd != null)
			{
				if (this.depthMode == DEPTH_MODE.USE_DEPTH_BUFFER && this.refractionResolution == refractionCommand.Resolution && commandEvent == data.cmdEvent && refractionCommand.Matches(cam))
				{
					return;
				}
				refractionCommand.Remove(cam);
				data.grabCmd = null;
			}
			if (this.depthMode == DEPTH_MODE.USE_DEPTH_BUFFER)
			{
				refractionCommand.Resolution = this.refractionResolution;
				refractionCommand.Event = commandEvent;
				data.grabCmd = refractionCommand.Create(cam);
				data.cmdEvent = commandEvent;
			}
		}

		private CameraEvent GetCommandEvent(Camera cam)
		{
			RenderingPath actualRenderingPath = cam.actualRenderingPath;
			if (actualRenderingPath == RenderingPath.DeferredShading)
			{
				return CameraEvent.AfterLighting;
			}
			if (actualRenderingPath == RenderingPath.DeferredLighting)
			{
				return CameraEvent.AfterLighting;
			}
			return CameraEvent.AfterDepthTexture;
		}

		private IRefractionCommand GetRefractionCommand()
		{
			IRefractionCommand result;
			if (base.CustomRefractionCommand != null)
			{
				result = base.CustomRefractionCommand;
				this.m_refractionCommand.RemoveAll();
			}
			else
			{
				result = this.m_refractionCommand;
			}
			return result;
		}

		private Mesh CreateBottomMesh(int segementsX, int segementsY)
		{
			Vector3[] array = new Vector3[segementsX * segementsY];
			Vector2[] uv = new Vector2[segementsX * segementsY];
			float num = 6.28318548f;
			for (int i = 0; i < segementsX; i++)
			{
				for (int j = 0; j < segementsY; j++)
				{
					float num2 = (float)i / (float)(segementsX - 1);
					array[i + j * segementsX].x = num2 * Mathf.Cos(num * (float)j / (float)(segementsY - 1));
					array[i + j * segementsX].y = 0f;
					array[i + j * segementsX].z = num2 * Mathf.Sin(num * (float)j / (float)(segementsY - 1));
					if (i == segementsX - 1)
					{
						array[i + j * segementsX].y = 1f;
					}
				}
			}
			int[] array2 = new int[segementsX * segementsY * 6];
			int num3 = 0;
			for (int k = 0; k < segementsX - 1; k++)
			{
				for (int l = 0; l < segementsY - 1; l++)
				{
					array2[num3++] = k + l * segementsX;
					array2[num3++] = k + (l + 1) * segementsX;
					array2[num3++] = k + 1 + l * segementsX;
					array2[num3++] = k + (l + 1) * segementsX;
					array2[num3++] = k + 1 + (l + 1) * segementsX;
					array2[num3++] = k + 1 + l * segementsX;
				}
			}
			return new Mesh
			{
				vertices = array,
				uv = uv,
				triangles = array2,
				name = "Ceto Bottom Mesh",
				hideFlags = HideFlags.HideAndDontSave
			};
		}
	}
}
