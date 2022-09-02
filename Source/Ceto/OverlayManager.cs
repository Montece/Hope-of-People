using Ceto.Common.Unity.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ceto
{
	public class OverlayManager
	{
		private static readonly Vector2 TEXTURE_FOAM = new Vector2(1f, 0f);

		private static readonly Vector2 DONT_TEXTURE_FOAM = new Vector2(0f, 1f);

		private Material m_overlayMat;

		private LinkedList<WaveOverlay> m_waveOverlays;

		private List<WaveOverlay> m_queryableOverlays;

		private List<WaveOverlay> m_removeOverlays;

		private List<QueryableOverlayResult> m_containingOverlays;

		private Texture2D m_blankNormal;

		private bool m_beenCleared;

		private List<WaveOverlay> m_heightOverlays;

		private List<WaveOverlay> m_normalOverlays;

		private List<WaveOverlay> m_foamOverlays;

		private List<WaveOverlay> m_clipOverlays;

		private Color m_clearColor;

		public bool HasHeightOverlay
		{
			get;
			private set;
		}

		public bool HasNormalOverlay
		{
			get;
			private set;
		}

		public bool HasFoamOverlay
		{
			get;
			private set;
		}

		public bool HasClipOverlay
		{
			get;
			private set;
		}

		public float MaxDisplacement
		{
			get;
			private set;
		}

		public OverlayManager(Material mat)
		{
			this.MaxDisplacement = 1f;
			this.m_overlayMat = mat;
			this.m_waveOverlays = new LinkedList<WaveOverlay>();
			this.m_queryableOverlays = new List<WaveOverlay>(32);
			this.m_removeOverlays = new List<WaveOverlay>(32);
			this.m_containingOverlays = new List<QueryableOverlayResult>(32);
			this.m_heightOverlays = new List<WaveOverlay>(32);
			this.m_normalOverlays = new List<WaveOverlay>(32);
			this.m_foamOverlays = new List<WaveOverlay>(32);
			this.m_clipOverlays = new List<WaveOverlay>(32);
			this.m_blankNormal = new Texture2D(1, 1, TextureFormat.ARGB32, false, true);
			this.m_blankNormal.SetPixel(0, 0, new Color(0.5f, 0.5f, 1f, 0.5f));
			this.m_blankNormal.hideFlags = HideFlags.HideAndDontSave;
			this.m_blankNormal.name = "Ceto Blank Normal Texture";
			this.m_blankNormal.Apply();
			this.m_clearColor = new Color(0f, 0f, 0f, 0f);
		}

		public void Release()
		{
			UnityEngine.Object.DestroyImmediate(this.m_blankNormal);
		}

		public void Update()
		{
			this.HasHeightOverlay = false;
			this.HasNormalOverlay = false;
			this.HasFoamOverlay = false;
			this.HasClipOverlay = false;
			this.MaxDisplacement = 1f;
			this.m_queryableOverlays.Clear();
			this.m_removeOverlays.Clear();
			LinkedList<WaveOverlay>.Enumerator enumerator = this.m_waveOverlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				if (current.Kill)
				{
					this.m_removeOverlays.Add(current);
				}
				else if (!current.Hide)
				{
					bool flag = false;
					if (current.HeightTex.IsDrawable)
					{
						this.HasHeightOverlay = true;
						flag = true;
						if (current.HeightTex.alpha > this.MaxDisplacement)
						{
							this.MaxDisplacement = current.HeightTex.alpha;
						}
					}
					if (current.NormalTex.IsDrawable)
					{
						this.HasNormalOverlay = true;
					}
					if (current.FoamTex.IsDrawable)
					{
						this.HasFoamOverlay = true;
					}
					if (current.ClipTex.IsDrawable)
					{
						this.HasClipOverlay = true;
						flag = true;
					}
					if (flag)
					{
						this.m_queryableOverlays.Add(current);
					}
				}
			}
			this.MaxDisplacement = Mathf.Min(this.MaxDisplacement, 20f);
			List<WaveOverlay>.Enumerator enumerator2 = this.m_removeOverlays.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				this.m_waveOverlays.Remove(enumerator2.Current);
			}
		}

		public void Add(WaveOverlay overlay)
		{
			if (overlay.Kill)
			{
				return;
			}
			this.m_waveOverlays.AddLast(overlay);
		}

		public void Remove(WaveOverlay overlay)
		{
			this.m_waveOverlays.Remove(overlay);
		}

		public void Clear()
		{
			this.m_waveOverlays.Clear();
		}

		public void ClearBuffers(WaveOverlayData data)
		{
			RTUtility.ClearColor(data.height, this.m_clearColor);
			RTUtility.ClearColor(data.normal, this.m_clearColor);
			RTUtility.ClearColor(data.foam, this.m_clearColor);
			RTUtility.ClearColor(data.clip, this.m_clearColor);
		}

		public void DestroyBuffers(WaveOverlayData data)
		{
			RTUtility.ReleaseAndDestroy(data.normal);
			RTUtility.ReleaseAndDestroy(data.height);
			RTUtility.ReleaseAndDestroy(data.foam);
			RTUtility.ReleaseAndDestroy(data.clip);
			data.normal = null;
			data.height = null;
			data.foam = null;
			data.clip = null;
		}

		public bool QueryableContains(float x, float z, bool overrideIqnoreQuerys)
		{
			List<WaveOverlay>.Enumerator enumerator = this.m_queryableOverlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				if (!current.Hide)
				{
					bool flag = (overrideIqnoreQuerys || !current.HeightTex.ignoreQuerys) && current.HeightTex.IsDrawable;
					bool flag2 = (overrideIqnoreQuerys || !current.ClipTex.ignoreQuerys) && current.ClipTex.IsDrawable;
					if (flag || flag2)
					{
						if (current.Contains(x, z))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public void GetQueryableContaining(float x, float z, bool overrideIqnoreQuerys, bool clipOnly)
		{
			this.m_containingOverlays.Clear();
			List<WaveOverlay>.Enumerator enumerator = this.m_queryableOverlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				if (!current.Hide)
				{
					bool flag = !clipOnly && (overrideIqnoreQuerys || !current.HeightTex.ignoreQuerys) && current.HeightTex.IsDrawable;
					bool flag2 = (overrideIqnoreQuerys || !current.ClipTex.ignoreQuerys) && current.ClipTex.IsDrawable;
					if (flag || flag2)
					{
						float u;
						float v;
						if (current.Contains(x, z, out u, out v))
						{
							QueryableOverlayResult item;
							item.overlay = current;
							item.u = u;
							item.v = v;
							this.m_containingOverlays.Add(item);
						}
					}
				}
			}
		}

		public void QueryWaves(WaveQuery query)
		{
			if (this.m_queryableOverlays.Count == 0)
			{
				return;
			}
			if (!query.sampleOverlay)
			{
				return;
			}
			bool flag = query.mode == QUERY_MODE.CLIP_TEST;
			float posX = query.posX;
			float posZ = query.posZ;
			this.GetQueryableContaining(posX, posZ, query.overrideIgnoreQuerys, flag);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			List<QueryableOverlayResult>.Enumerator enumerator = this.m_containingOverlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				QueryableOverlayResult current = enumerator.Current;
				try
				{
					OverlayClipTexture clipTex = current.overlay.ClipTex;
					OverlayHeightTexture heightTex = current.overlay.HeightTex;
					if (clipTex.IsDrawable && clipTex.tex is Texture2D)
					{
						float a = (clipTex.tex as Texture2D).GetPixelBilinear(current.u, current.v).a;
						num += a * Mathf.Max(0f, clipTex.alpha);
					}
					if (!flag && heightTex.IsDrawable)
					{
						float alpha = heightTex.alpha;
						float num4 = Mathf.Max(0f, heightTex.maskAlpha);
						float num5 = 0f;
						float num6 = 0f;
						if (heightTex.tex != null && heightTex.tex is Texture2D)
						{
							num5 = (heightTex.tex as Texture2D).GetPixelBilinear(current.u, current.v).a;
						}
						if (heightTex.mask != null && heightTex.mask is Texture2D)
						{
							num6 = (heightTex.mask as Texture2D).GetPixelBilinear(current.u, current.v).a;
							num6 = Mathf.Clamp01(num6 * num4);
						}
						if (heightTex.maskMode == OVERLAY_MASK_MODE.WAVES)
						{
							num5 *= alpha;
						}
						else if (heightTex.maskMode == OVERLAY_MASK_MODE.OVERLAY)
						{
							num5 *= alpha * num6;
							num6 = 0f;
						}
						else if (heightTex.maskMode == OVERLAY_MASK_MODE.WAVES_AND_OVERLAY)
						{
							num5 *= alpha * (1f - num6);
						}
						else if (heightTex.maskMode == OVERLAY_MASK_MODE.WAVES_AND_OVERLAY_BLEND)
						{
							num5 *= alpha * num6;
						}
						num2 += num5;
						num3 += num6;
					}
				}
				catch
				{
				}
			}
			num = Mathf.Clamp01(num);
			if (0.5f - num < 0f)
			{
				query.result.isClipped = true;
			}
			num3 = 1f - Mathf.Clamp01(num3);
			query.result.height = query.result.height * num3;
			query.result.displacementX = query.result.displacementX * num3;
			query.result.displacementZ = query.result.displacementZ * num3;
			query.result.height = query.result.height + num2;
			query.result.overlayHeight = num2;
		}

		public void RenderWaveOverlays(Camera cam, WaveOverlayData data)
		{
			if (!this.m_beenCleared)
			{
				this.ClearBuffers(data);
				this.m_beenCleared = true;
			}
			if (this.m_waveOverlays.Count == 0)
			{
				return;
			}
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.current);
			this.m_heightOverlays.Clear();
			this.m_normalOverlays.Clear();
			this.m_foamOverlays.Clear();
			this.m_clipOverlays.Clear();
			LinkedList<WaveOverlay>.Enumerator enumerator = this.m_waveOverlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				if (!current.Hide && GeometryUtility.TestPlanesAABB(planes, current.BoundingBox))
				{
					if (current.HeightTex.IsDrawable)
					{
						this.m_heightOverlays.Add(current);
					}
					if (current.NormalTex.IsDrawable)
					{
						this.m_normalOverlays.Add(current);
					}
					if (current.FoamTex.IsDrawable)
					{
						this.m_foamOverlays.Add(current);
					}
					if (current.ClipTex.IsDrawable)
					{
						this.m_clipOverlays.Add(current);
					}
				}
			}
			this.RenderHeightOverlays(this.m_heightOverlays, data.height);
			this.RenderNormalOverlays(this.m_normalOverlays, data.normal);
			this.RenderFoamOverlays(this.m_foamOverlays, data.foam);
			this.RenderClipOverlays(this.m_clipOverlays, data.clip);
			if (data.normal != null)
			{
				Shader.SetGlobalTexture("Ceto_Overlay_NormalMap", data.normal);
			}
			else
			{
				Shader.SetGlobalTexture("Ceto_Overlay_NormalMap", Texture2D.blackTexture);
			}
			if (data.height != null)
			{
				Shader.SetGlobalTexture("Ceto_Overlay_HeightMap", data.height);
			}
			else
			{
				Shader.SetGlobalTexture("Ceto_Overlay_HeightMap", Texture2D.blackTexture);
			}
			if (data.foam != null)
			{
				Shader.SetGlobalTexture("Ceto_Overlay_FoamMap", data.foam);
			}
			else
			{
				Shader.SetGlobalTexture("Ceto_Overlay_FoamMap", Texture2D.blackTexture);
			}
			if (data.clip != null)
			{
				Shader.SetGlobalTexture("Ceto_Overlay_ClipMap", data.clip);
			}
			else
			{
				Shader.SetGlobalTexture("Ceto_Overlay_ClipMap", Texture2D.blackTexture);
			}
			this.m_beenCleared = false;
		}

		private void RenderHeightOverlays(IEnumerable<WaveOverlay> overlays, RenderTexture target)
		{
			if (target == null)
			{
				return;
			}
			IEnumerator<WaveOverlay> enumerator = overlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				this.m_overlayMat.SetFloat("Ceto_Overlay_Alpha", current.HeightTex.alpha);
				this.m_overlayMat.SetFloat("Ceto_Overlay_MaskAlpha", Mathf.Max(0f, current.HeightTex.maskAlpha));
				this.m_overlayMat.SetTexture("Ceto_Overlay_Height", (!(current.HeightTex.tex != null)) ? Texture2D.blackTexture : current.HeightTex.tex);
				this.m_overlayMat.SetTexture("Ceto_Overlay_HeightMask", (!(current.HeightTex.mask != null)) ? Texture2D.blackTexture : current.HeightTex.mask);
				this.m_overlayMat.SetFloat("Ceto_Overlay_MaskMode", (float)current.HeightTex.maskMode);
				this.Blit(current.Corners, current.HeightTex.scaleUV, current.HeightTex.offsetUV, target, this.m_overlayMat, 0);
			}
		}

		private void RenderNormalOverlays(IEnumerable<WaveOverlay> overlays, RenderTexture target)
		{
			if (target == null)
			{
				return;
			}
			IEnumerator<WaveOverlay> enumerator = overlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				this.m_overlayMat.SetFloat("Ceto_Overlay_Alpha", Mathf.Max(0f, current.NormalTex.alpha));
				this.m_overlayMat.SetFloat("Ceto_Overlay_MaskAlpha", Mathf.Max(0f, current.NormalTex.maskAlpha));
				this.m_overlayMat.SetTexture("Ceto_Overlay_Normal", (!(current.NormalTex.tex != null)) ? this.m_blankNormal : current.NormalTex.tex);
				this.m_overlayMat.SetTexture("Ceto_Overlay_NormalMask", (!(current.NormalTex.mask != null)) ? Texture2D.blackTexture : current.NormalTex.mask);
				this.m_overlayMat.SetFloat("Ceto_Overlay_MaskMode", (float)current.NormalTex.maskMode);
				this.Blit(current.Corners, current.NormalTex.scaleUV, current.NormalTex.offsetUV, target, this.m_overlayMat, 1);
			}
		}

		private void RenderFoamOverlays(IEnumerable<WaveOverlay> overlays, RenderTexture target)
		{
			if (target == null)
			{
				return;
			}
			IEnumerator<WaveOverlay> enumerator = overlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				this.m_overlayMat.SetFloat("Ceto_Overlay_Alpha", Mathf.Max(0f, current.FoamTex.alpha));
				this.m_overlayMat.SetFloat("Ceto_Overlay_MaskAlpha", Mathf.Max(0f, current.FoamTex.maskAlpha));
				this.m_overlayMat.SetTexture("Ceto_Overlay_Foam", (!(current.FoamTex.tex != null)) ? Texture2D.blackTexture : current.FoamTex.tex);
				this.m_overlayMat.SetTexture("Ceto_Overlay_FoamMask", (!(current.FoamTex.mask != null)) ? Texture2D.blackTexture : current.FoamTex.mask);
				this.m_overlayMat.SetFloat("Ceto_Overlay_MaskMode", (float)current.FoamTex.maskMode);
				this.m_overlayMat.SetVector("Ceto_TextureFoam", (!current.FoamTex.textureFoam) ? OverlayManager.DONT_TEXTURE_FOAM : OverlayManager.TEXTURE_FOAM);
				this.Blit(current.Corners, current.FoamTex.scaleUV, current.FoamTex.offsetUV, target, this.m_overlayMat, 2);
			}
		}

		private void RenderClipOverlays(IEnumerable<WaveOverlay> overlays, RenderTexture target)
		{
			if (target == null)
			{
				return;
			}
			IEnumerator<WaveOverlay> enumerator = overlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				this.m_overlayMat.SetFloat("Ceto_Overlay_Alpha", Mathf.Max(0f, current.ClipTex.alpha));
				this.m_overlayMat.SetTexture("Ceto_Overlay_Clip", (!(current.ClipTex.tex != null)) ? Texture2D.blackTexture : current.ClipTex.tex);
				this.Blit(current.Corners, current.ClipTex.scaleUV, current.ClipTex.offsetUV, target, this.m_overlayMat, 3);
			}
		}

		private void Blit(Vector4[] corners, Vector2 scale, Vector2 offset, RenderTexture des, Material mat, int pass)
		{
			Graphics.SetRenderTarget(des);
			GL.PushMatrix();
			GL.LoadOrtho();
			mat.SetPass(pass);
			GL.Begin(7);
			GL.MultiTexCoord2(0, offset.x, offset.y);
			GL.MultiTexCoord2(1, 0f, 0f);
			GL.Vertex(corners[0]);
			GL.MultiTexCoord2(0, offset.x + 1f * scale.x, offset.y);
			GL.MultiTexCoord2(1, 1f, 0f);
			GL.Vertex(corners[1]);
			GL.MultiTexCoord2(0, offset.x + 1f * scale.x, offset.y + 1f * scale.y);
			GL.MultiTexCoord2(1, 1f, 1f);
			GL.Vertex(corners[2]);
			GL.MultiTexCoord2(0, offset.x, offset.y + 1f * scale.y);
			GL.MultiTexCoord2(1, 0f, 1f);
			GL.Vertex(corners[3]);
			GL.End();
			GL.PopMatrix();
		}

		public void CreateOverlays(Camera cam, WaveOverlayData overlay, OVERLAY_MAP_SIZE normalOverlaySize, OVERLAY_MAP_SIZE heightOverlaySize, OVERLAY_MAP_SIZE foamOverlaySize, OVERLAY_MAP_SIZE clipOverlaySize)
		{
			if (this.HasNormalOverlay)
			{
				RenderTextureFormat format = RenderTextureFormat.ARGBHalf;
				if (!SystemInfo.SupportsRenderTextureFormat(format))
				{
					format = RenderTextureFormat.ARGB32;
				}
				this.CreateBuffer("Normal", cam, normalOverlaySize, format, true, ref overlay.normal);
			}
			if (this.HasHeightOverlay)
			{
				RenderTextureFormat format2 = RenderTextureFormat.RGHalf;
				if (!SystemInfo.SupportsRenderTextureFormat(format2))
				{
					format2 = RenderTextureFormat.ARGB32;
				}
				this.CreateBuffer("Height", cam, heightOverlaySize, format2, true, ref overlay.height);
			}
			if (this.HasFoamOverlay)
			{
				RenderTextureFormat format3 = RenderTextureFormat.ARGB32;
				this.CreateBuffer("Foam", cam, foamOverlaySize, format3, false, ref overlay.foam);
			}
			if (this.HasClipOverlay)
			{
				RenderTextureFormat format4 = RenderTextureFormat.R8;
				if (!SystemInfo.SupportsRenderTextureFormat(format4))
				{
					format4 = RenderTextureFormat.RHalf;
				}
				if (!SystemInfo.SupportsRenderTextureFormat(format4))
				{
					format4 = RenderTextureFormat.ARGB32;
				}
				this.CreateBuffer("Clip", cam, clipOverlaySize, format4, true, ref overlay.clip);
			}
		}

		public void CreateBuffer(string name, Camera cam, OVERLAY_MAP_SIZE size, RenderTextureFormat format, bool isLinear, ref RenderTexture map)
		{
			float num = this.SizeToValue(size);
			int num2 = Mathf.Min(4096, (int)((float)cam.pixelWidth * num));
			int num3 = Mathf.Min(4096, (int)((float)cam.pixelHeight * num));
			if (map == null || map.width != num2 || map.height != num3)
			{
				if (map != null)
				{
					RTUtility.ReleaseAndDestroy(map);
				}
				RenderTextureReadWrite readWrite = (!isLinear) ? RenderTextureReadWrite.Default : RenderTextureReadWrite.Linear;
				map = new RenderTexture(num2, num3, 0, format, readWrite);
				map.useMipMap = false;
				map.filterMode = FilterMode.Bilinear;
				map.hideFlags = HideFlags.DontSave;
				map.name = "Ceto Overlay " + name + " Buffer: " + cam.name;
			}
		}

		private float SizeToValue(OVERLAY_MAP_SIZE size)
		{
			switch (size)
			{
			case OVERLAY_MAP_SIZE.DOUBLE:
				return 2f;
			case OVERLAY_MAP_SIZE.FULL_HALF:
				return 1.5f;
			case OVERLAY_MAP_SIZE.FULL:
				return 1f;
			case OVERLAY_MAP_SIZE.HALF:
				return 0.5f;
			case OVERLAY_MAP_SIZE.QUARTER:
				return 0.25f;
			default:
				return 1f;
			}
		}
	}
}
