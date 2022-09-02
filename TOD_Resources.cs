using System;
using UnityEngine;

public class TOD_Resources : MonoBehaviour
{
	public Mesh Quad;

	public Mesh SphereHigh;

	public Mesh SphereMedium;

	public Mesh SphereLow;

	public Mesh IcosphereHigh;

	public Mesh IcosphereMedium;

	public Mesh IcosphereLow;

	public Mesh HalfIcosphereHigh;

	public Mesh HalfIcosphereMedium;

	public Mesh HalfIcosphereLow;

	public Material CloudMaterial;

	public Material ShadowMaterial;

	public Material BillboardMaterial;

	public Material SpaceMaterial;

	public Material AtmosphereMaterial;

	public Material SunMaterial;

	public Material MoonMaterial;

	public Material ClearMaterial;

	public Material SkyboxMaterial;

	internal int ID_SunSkyColor;

	internal int ID_MoonSkyColor;

	internal int ID_SunCloudColor;

	internal int ID_MoonCloudColor;

	internal int ID_SunMeshColor;

	internal int ID_MoonMeshColor;

	internal int ID_CloudColor;

	internal int ID_AmbientColor;

	internal int ID_MoonHaloColor;

	internal int ID_SunDirection;

	internal int ID_MoonDirection;

	internal int ID_LightDirection;

	internal int ID_LocalSunDirection;

	internal int ID_LocalMoonDirection;

	internal int ID_LocalLightDirection;

	internal int ID_Contrast;

	internal int ID_Brightness;

	internal int ID_Fogginess;

	internal int ID_Directionality;

	internal int ID_MoonHaloPower;

	internal int ID_CloudDensity;

	internal int ID_CloudSharpness;

	internal int ID_CloudShadow;

	internal int ID_CloudScale;

	internal int ID_CloudUV;

	internal int ID_SpaceTiling;

	internal int ID_SpaceBrightness;

	internal int ID_SunMeshContrast;

	internal int ID_SunMeshBrightness;

	internal int ID_MoonMeshContrast;

	internal int ID_MoonMeshBrightness;

	internal int ID_kBetaMie;

	internal int ID_kSun;

	internal int ID_k4PI;

	internal int ID_kRadius;

	internal int ID_kScale;

	internal int ID_World2Sky;

	internal int ID_Sky2World;

	public void Initialize()
	{
		this.ID_SunSkyColor = Shader.PropertyToID("TOD_SunSkyColor");
		this.ID_MoonSkyColor = Shader.PropertyToID("TOD_MoonSkyColor");
		this.ID_SunCloudColor = Shader.PropertyToID("TOD_SunCloudColor");
		this.ID_MoonCloudColor = Shader.PropertyToID("TOD_MoonCloudColor");
		this.ID_SunMeshColor = Shader.PropertyToID("TOD_SunMeshColor");
		this.ID_MoonMeshColor = Shader.PropertyToID("TOD_MoonMeshColor");
		this.ID_CloudColor = Shader.PropertyToID("TOD_CloudColor");
		this.ID_AmbientColor = Shader.PropertyToID("TOD_AmbientColor");
		this.ID_MoonHaloColor = Shader.PropertyToID("TOD_MoonHaloColor");
		this.ID_SunDirection = Shader.PropertyToID("TOD_SunDirection");
		this.ID_MoonDirection = Shader.PropertyToID("TOD_MoonDirection");
		this.ID_LightDirection = Shader.PropertyToID("TOD_LightDirection");
		this.ID_LocalSunDirection = Shader.PropertyToID("TOD_LocalSunDirection");
		this.ID_LocalMoonDirection = Shader.PropertyToID("TOD_LocalMoonDirection");
		this.ID_LocalLightDirection = Shader.PropertyToID("TOD_LocalLightDirection");
		this.ID_Contrast = Shader.PropertyToID("TOD_Contrast");
		this.ID_Brightness = Shader.PropertyToID("TOD_Brightness");
		this.ID_Fogginess = Shader.PropertyToID("TOD_Fogginess");
		this.ID_Directionality = Shader.PropertyToID("TOD_Directionality");
		this.ID_MoonHaloPower = Shader.PropertyToID("TOD_MoonHaloPower");
		this.ID_CloudDensity = Shader.PropertyToID("TOD_CloudDensity");
		this.ID_CloudSharpness = Shader.PropertyToID("TOD_CloudSharpness");
		this.ID_CloudShadow = Shader.PropertyToID("TOD_CloudShadow");
		this.ID_CloudScale = Shader.PropertyToID("TOD_CloudScale");
		this.ID_CloudUV = Shader.PropertyToID("TOD_CloudUV");
		this.ID_SpaceTiling = Shader.PropertyToID("TOD_SpaceTiling");
		this.ID_SpaceBrightness = Shader.PropertyToID("TOD_SpaceBrightness");
		this.ID_SunMeshContrast = Shader.PropertyToID("TOD_SunMeshContrast");
		this.ID_SunMeshBrightness = Shader.PropertyToID("TOD_SunMeshBrightness");
		this.ID_MoonMeshContrast = Shader.PropertyToID("TOD_MoonMeshContrast");
		this.ID_MoonMeshBrightness = Shader.PropertyToID("TOD_MoonMeshBrightness");
		this.ID_kBetaMie = Shader.PropertyToID("TOD_kBetaMie");
		this.ID_kSun = Shader.PropertyToID("TOD_kSun");
		this.ID_k4PI = Shader.PropertyToID("TOD_k4PI");
		this.ID_kRadius = Shader.PropertyToID("TOD_kRadius");
		this.ID_kScale = Shader.PropertyToID("TOD_kScale");
		this.ID_World2Sky = Shader.PropertyToID("TOD_World2Sky");
		this.ID_Sky2World = Shader.PropertyToID("TOD_Sky2World");
	}

	public static Mesh CreateQuad(Vector2 minUV, Vector2 maxUV)
	{
		return new Mesh
		{
			name = string.Concat(new object[]
			{
				"Quad ",
				minUV,
				" ",
				maxUV
			}),
			vertices = new Vector3[]
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f, 1f, 0f),
				new Vector3(1f, 1f, 0f),
				new Vector3(1f, -1f, 0f)
			},
			uv = new Vector2[]
			{
				new Vector2(minUV.x, minUV.y),
				new Vector2(minUV.x, maxUV.y),
				new Vector2(maxUV.x, maxUV.y),
				new Vector2(maxUV.x, minUV.y)
			},
			triangles = new int[]
			{
				0,
				3,
				2,
				0,
				2,
				1
			},
			normals = new Vector3[]
			{
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f)
			},
			tangents = new Vector4[]
			{
				new Vector4(1f, 0f, 0f, 1f),
				new Vector4(1f, 0f, 0f, 1f),
				new Vector4(1f, 0f, 0f, 1f),
				new Vector4(1f, 0f, 0f, 1f)
			}
		};
	}
}
