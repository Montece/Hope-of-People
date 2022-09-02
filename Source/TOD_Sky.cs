using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, RequireComponent(typeof(TOD_Resources)), RequireComponent(typeof(TOD_Components))]
public class TOD_Sky : MonoBehaviour
{
	private const float pi = 3.14159274f;

	private const float tau = 6.28318548f;

	private static List<TOD_Sky> instances = new List<TOD_Sky>();

	private int probeRenderID = -1;

	public TOD_ColorSpaceType ColorSpace;

	public TOD_ColorRangeType ColorRange;

	public TOD_SkyQualityType SkyQuality;

	public TOD_CloudQualityType CloudQuality = TOD_CloudQualityType.Bumped;

	public TOD_MeshQualityType MeshQuality = TOD_MeshQualityType.High;

	public TOD_CycleParameters Cycle;

	public TOD_WorldParameters World;

	public TOD_AtmosphereParameters Atmosphere;

	public TOD_DayParameters Day;

	public TOD_NightParameters Night;

	public TOD_SunParameters Sun;

	public TOD_MoonParameters Moon;

	public TOD_StarParameters Stars;

	public TOD_CloudParameters Clouds;

	public TOD_LightParameters Light;

	public TOD_FogParameters Fog;

	public TOD_AmbientParameters Ambient;

	public TOD_ReflectionParameters Reflection;

	private float timeSinceLightUpdate = 3.40282347E+38f;

	private float timeSinceAmbientUpdate = 3.40282347E+38f;

	private float timeSinceReflectionUpdate = 3.40282347E+38f;

	private const int TOD_SAMPLES = 2;

	private Vector3 kBetaMie;

	private Vector4 kSun;

	private Vector4 k4PI;

	private Vector4 kRadius;

	private Vector4 kScale;

	public static List<TOD_Sky> Instances
	{
		get
		{
			return TOD_Sky.instances;
		}
	}

	public static TOD_Sky Instance
	{
		get
		{
			return (TOD_Sky.instances.Count != 0) ? TOD_Sky.instances[TOD_Sky.instances.Count - 1] : null;
		}
	}

	internal bool Initialized
	{
		get;
		private set;
	}

	internal bool Headless
	{
		get
		{
			return Camera.allCamerasCount == 0;
		}
	}

	internal TOD_Components Components
	{
		get;
		private set;
	}

	internal TOD_Resources Resources
	{
		get;
		private set;
	}

	internal bool IsDay
	{
		get;
		private set;
	}

	internal bool IsNight
	{
		get;
		private set;
	}

	internal float Radius
	{
		get
		{
			return this.Components.DomeTransform.lossyScale.y;
		}
	}

	internal float Diameter
	{
		get
		{
			return this.Components.DomeTransform.lossyScale.y * 2f;
		}
	}

	internal float LerpValue
	{
		get;
		private set;
	}

	internal float SunZenith
	{
		get;
		private set;
	}

	internal float MoonZenith
	{
		get;
		private set;
	}

	internal float LightZenith
	{
		get
		{
			return Mathf.Min(this.SunZenith, this.MoonZenith);
		}
	}

	internal float LightIntensity
	{
		get
		{
			return this.Components.LightSource.intensity;
		}
	}

	internal Vector3 SunDirection
	{
		get;
		private set;
	}

	internal Vector3 MoonDirection
	{
		get;
		private set;
	}

	internal Vector3 LightDirection
	{
		get;
		private set;
	}

	internal Vector3 LocalSunDirection
	{
		get;
		private set;
	}

	internal Vector3 LocalMoonDirection
	{
		get;
		private set;
	}

	internal Vector3 LocalLightDirection
	{
		get;
		private set;
	}

	internal Color SunLightColor
	{
		get;
		private set;
	}

	internal Color MoonLightColor
	{
		get;
		private set;
	}

	internal Color LightColor
	{
		get
		{
			return this.Components.LightSource.color;
		}
	}

	internal Color SunRayColor
	{
		get;
		private set;
	}

	internal Color MoonRayColor
	{
		get;
		private set;
	}

	internal Color RayColor
	{
		get;
		private set;
	}

	internal Color SunSkyColor
	{
		get;
		private set;
	}

	internal Color MoonSkyColor
	{
		get;
		private set;
	}

	internal Color SunMeshColor
	{
		get;
		private set;
	}

	internal Color MoonMeshColor
	{
		get;
		private set;
	}

	internal Color CloudColor
	{
		get;
		private set;
	}

	internal Color AmbientColor
	{
		get;
		private set;
	}

	internal Color MoonHaloColor
	{
		get;
		private set;
	}

	internal ReflectionProbe Probe
	{
		get;
		private set;
	}

	private void UpdateScattering()
	{
		float num = -this.Atmosphere.Directionality;
		float num2 = num * num;
		this.kBetaMie.x = 1.5f * ((1f - num2) / (2f + num2));
		this.kBetaMie.y = 1f + num2;
		this.kBetaMie.z = 2f * num;
		float num3 = 0.002f * this.Atmosphere.MieMultiplier;
		float num4 = 0.002f * this.Atmosphere.RayleighMultiplier;
		float x = num4 * 40f * 5.27016449f;
		float y = num4 * 40f * 9.473285f;
		float z = num4 * 40f * 19.6438026f;
		float w = num3 * 40f;
		this.kSun.x = x;
		this.kSun.y = y;
		this.kSun.z = z;
		this.kSun.w = w;
		float x2 = num4 * 4f * 3.14159274f * 5.27016449f;
		float y2 = num4 * 4f * 3.14159274f * 9.473285f;
		float z2 = num4 * 4f * 3.14159274f * 19.6438026f;
		float w2 = num3 * 4f * 3.14159274f;
		this.k4PI.x = x2;
		this.k4PI.y = y2;
		this.k4PI.z = z2;
		this.k4PI.w = w2;
		this.kRadius.x = 1f;
		this.kRadius.y = 1f;
		this.kRadius.z = 1.025f;
		this.kRadius.w = 1.050625f;
		this.kScale.x = 40.00004f;
		this.kScale.y = 0.25f;
		this.kScale.z = 160.000153f;
		this.kScale.w = 0.0001f;
	}

	private void UpdateCelestials()
	{
		float f = 0.0174532924f * this.World.Latitude;
		float num = Mathf.Sin(f);
		float num2 = Mathf.Cos(f);
		float longitude = this.World.Longitude;
		float num3 = 1.57079637f;
		int year = this.Cycle.Year;
		int month = this.Cycle.Month;
		int day = this.Cycle.Day;
		float num4 = this.Cycle.Hour - this.World.UTC;
		float num5 = (float)(367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530) + num4 / 24f;
		float num6 = 23.4393f - 3.563E-07f * num5;
		float f2 = 0.0174532924f * num6;
		float num7 = Mathf.Sin(f2);
		float num8 = Mathf.Cos(f2);
		float num9 = 282.9404f + 4.70935E-05f * num5;
		float num10 = 0.016709f - 1.151E-09f * num5;
		float num11 = 356.047f + 0.985600233f * num5;
		float num12 = 0.0174532924f * num11;
		float num13 = Mathf.Sin(num12);
		float num14 = Mathf.Cos(num12);
		float f3 = num12 + num10 * num13 * (1f + num10 * num14);
		float num15 = Mathf.Sin(f3);
		float num16 = Mathf.Cos(f3);
		float num17 = num16 - num10;
		float num18 = Mathf.Sqrt(1f - num10 * num10) * num15;
		float num19 = 57.29578f * Mathf.Atan2(num18, num17);
		float num20 = Mathf.Sqrt(num17 * num17 + num18 * num18);
		float num21 = num19 + num9;
		float f4 = 0.0174532924f * num21;
		float num22 = Mathf.Sin(f4);
		float num23 = Mathf.Cos(f4);
		float num24 = num20 * num23;
		float num25 = num20 * num22;
		float num26 = num24;
		float num27 = num25 * num8;
		float y = num25 * num7;
		float num28 = Mathf.Atan2(num27, num26);
		float f5 = Mathf.Atan2(y, Mathf.Sqrt(num26 * num26 + num27 * num27));
		float num29 = Mathf.Sin(f5);
		float num30 = Mathf.Cos(f5);
		float num31 = num19 + num9;
		float num32 = num31 + 180f;
		float num33 = num32 + 15f * num4;
		float num34 = 0.0174532924f * (num33 + longitude);
		float f6 = num34 - num28;
		float num35 = Mathf.Sin(f6);
		float num36 = Mathf.Cos(f6);
		float num37 = num36 * num30;
		float num38 = num35 * num30;
		float num39 = num29;
		float num40 = num37 * num - num39 * num2;
		float num41 = num38;
		float y2 = num37 * num2 + num39 * num;
		float num42 = Mathf.Atan2(num41, num40) + 3.14159274f;
		float num43 = Mathf.Atan2(y2, Mathf.Sqrt(num40 * num40 + num41 * num41));
		float num44 = num3 - num43;
		float num45 = num42;
		float num88;
		float phi;
		if (this.Moon.Position == TOD_MoonPositionType.Realistic)
		{
			float num46 = 125.1228f - 0.05295381f * num5;
			float num47 = 5.1454f;
			float num48 = 318.0634f + 0.164357319f * num5;
			float num49 = 60.2666f;
			float num50 = 0.0549f;
			float num51 = 115.3654f + 13.0649929f * num5;
			float f7 = 0.0174532924f * num46;
			float num52 = Mathf.Sin(f7);
			float num53 = Mathf.Cos(f7);
			float f8 = 0.0174532924f * num47;
			float num54 = Mathf.Sin(f8);
			float num55 = Mathf.Cos(f8);
			float num56 = 0.0174532924f * num51;
			float num57 = Mathf.Sin(num56);
			float num58 = Mathf.Cos(num56);
			float f9 = num56 + num50 * num57 * (1f + num50 * num58);
			float num59 = Mathf.Sin(f9);
			float num60 = Mathf.Cos(f9);
			float num61 = num49 * (num60 - num50);
			float num62 = num49 * (Mathf.Sqrt(1f - num50 * num50) * num59);
			float num63 = 57.29578f * Mathf.Atan2(num62, num61);
			float num64 = Mathf.Sqrt(num61 * num61 + num62 * num62);
			float num65 = num63 + num48;
			float f10 = 0.0174532924f * num65;
			float num66 = Mathf.Sin(f10);
			float num67 = Mathf.Cos(f10);
			float num68 = num64 * (num53 * num67 - num52 * num66 * num55);
			float num69 = num64 * (num52 * num67 + num53 * num66 * num55);
			float num70 = num64 * (num66 * num54);
			float num71 = num68;
			float num72 = num69;
			float num73 = num70;
			float num74 = num71;
			float num75 = num72 * num8 - num73 * num7;
			float y3 = num72 * num7 + num73 * num8;
			float num76 = Mathf.Atan2(num75, num74);
			float f11 = Mathf.Atan2(y3, Mathf.Sqrt(num74 * num74 + num75 * num75));
			float num77 = Mathf.Sin(f11);
			float num78 = Mathf.Cos(f11);
			float f12 = num34 - num76;
			float num79 = Mathf.Sin(f12);
			float num80 = Mathf.Cos(f12);
			float num81 = num80 * num78;
			float num82 = num79 * num78;
			float num83 = num77;
			float num84 = num81 * num - num83 * num2;
			float num85 = num82;
			float y4 = num81 * num2 + num83 * num;
			float num86 = Mathf.Atan2(num85, num84) + 3.14159274f;
			float num87 = Mathf.Atan2(y4, Mathf.Sqrt(num84 * num84 + num85 * num85));
			num88 = num3 - num87;
			phi = num86;
		}
		else
		{
			num88 = num44 - 3.14159274f;
			phi = num45;
		}
		this.SunZenith = 57.29578f * num44;
		this.MoonZenith = 57.29578f * num88;
		Quaternion quaternion = Quaternion.Euler(90f - this.World.Latitude, 0f, 0f) * Quaternion.Euler(0f, this.World.Longitude, 0f) * Quaternion.Euler(0f, num34 * 57.29578f, 0f);
		if (this.Stars.Position == TOD_StarsPositionType.Rotating)
		{
			this.Components.SpaceTransform.localRotation = quaternion;
		}
		else
		{
			this.Components.SpaceTransform.localRotation = Quaternion.identity;
		}
		Vector3 localPosition = this.OrbitalToLocal(num44, num45);
		this.Components.SunTransform.localPosition = localPosition;
		this.Components.SunTransform.LookAt(this.Components.DomeTransform.position, this.Components.SunTransform.up);
		Vector3 localPosition2 = this.OrbitalToLocal(num88, phi);
		Vector3 worldUp = quaternion * -Vector3.right;
		this.Components.MoonTransform.localPosition = localPosition2;
		this.Components.MoonTransform.LookAt(this.Components.DomeTransform.position, worldUp);
		float num89 = 2f * Mathf.Tan(0.06981317f * this.Sun.MeshSize);
		Vector3 localScale = new Vector3(num89, num89, num89);
		this.Components.SunTransform.localScale = localScale;
		float num90 = 2f * Mathf.Tan(0.0174532924f * this.Moon.MeshSize);
		Vector3 localScale2 = new Vector3(num90, num90, num90);
		this.Components.MoonTransform.localScale = localScale2;
		bool enabled = this.Components.SunTransform.localPosition.y > -num89;
		this.Components.SunRenderer.enabled = enabled;
		bool enabled2 = this.Components.MoonTransform.localPosition.y > -num90;
		this.Components.MoonRenderer.enabled = enabled2;
		bool enabled3 = this.Clouds.Density > 0f;
		this.Components.CloudRenderer.enabled = enabled3;
		bool enabled4 = this.Components.ShadowMaterial != null && this.Clouds.ShadowStrength != 0f;
		this.Components.ShadowProjector.enabled = enabled4;
		bool enabled5 = true;
		this.Components.SpaceRenderer.enabled = enabled5;
		bool enabled6 = true;
		this.Components.AtmosphereRenderer.enabled = enabled6;
		bool enabled7 = this.Components.Rays != null;
		this.Components.ClearRenderer.enabled = enabled7;
		this.LerpValue = Mathf.InverseLerp(110f, 80f, this.SunZenith);
		float time = 1f - this.LerpValue;
		float colorMultiplier = this.Day.ColorMultiplier;
		float num91 = this.Night.ColorMultiplier * 0.25f;
		float num92 = 1f - this.Atmosphere.Fogginess;
		float num93 = Mathf.Clamp01((90f - num88 * 57.29578f) / 5f);
		float num94 = Mathf.Clamp01(num92 * (this.LerpValue - 0.1f) / 0.9f);
		float num95 = Mathf.Clamp01(num92 * num93 * (0.1f - this.LerpValue) / 0.1f);
		float multiplier = colorMultiplier * num94;
		this.SunLightColor = TOD_Util.MulRGB(this.Day.LightColor.Evaluate(time), multiplier);
		float multiplier2 = num91 * num95;
		this.MoonLightColor = TOD_Util.MulRGB(this.Night.LightColor.Evaluate(time), multiplier2);
		float multiplier3 = colorMultiplier * num94;
		this.SunRayColor = TOD_Util.MulRGB(this.Day.RayColor.Evaluate(time), multiplier3);
		float multiplier4 = num91 * num95;
		this.MoonRayColor = TOD_Util.MulRGB(this.Night.RayColor.Evaluate(time), multiplier4);
		float multiplier5 = colorMultiplier;
		this.SunSkyColor = TOD_Util.MulRGB(this.Day.SkyColor.Evaluate(time), multiplier5);
		float multiplier6 = num91;
		this.MoonSkyColor = TOD_Util.MulRGB(this.Night.SkyColor.Evaluate(time), multiplier6);
		float multiplier7 = colorMultiplier;
		this.SunMeshColor = TOD_Util.MulRGB(this.Sun.MeshColor.Evaluate(time), multiplier7);
		float multiplier8 = num91;
		this.MoonMeshColor = TOD_Util.MulRGB(this.Moon.MeshColor.Evaluate(time), multiplier8);
		float multiplier9 = colorMultiplier * colorMultiplier * this.Clouds.Brightness;
		Color b = TOD_Util.MulRGB(this.Day.CloudColor.Evaluate(time), multiplier9);
		float multiplier10 = num91 * num91 * this.Clouds.Brightness;
		Color a = TOD_Util.MulRGB(this.Night.CloudColor.Evaluate(time), multiplier10);
		this.CloudColor = Color.Lerp(a, b, this.LerpValue);
		float multiplier11 = colorMultiplier * this.Day.AmbientMultiplier;
		Color b2 = TOD_Util.MulRGB(this.Day.AmbientColor.Evaluate(time), multiplier11);
		float multiplier12 = num91 * this.Night.AmbientMultiplier;
		Color a2 = TOD_Util.MulRGB(this.Night.AmbientColor.Evaluate(time), multiplier12);
		this.AmbientColor = Color.Lerp(a2, b2, this.LerpValue);
		float multiplier13 = num91 * num93;
		this.MoonHaloColor = TOD_Util.MulRGB(this.Moon.HaloColor.Evaluate(time), multiplier13);
		float shadowStrength;
		float intensity;
		Color color;
		if (this.LerpValue > 0.1f)
		{
			this.IsDay = true;
			this.IsNight = false;
			shadowStrength = this.Day.ShadowStrength;
			intensity = Mathf.Lerp(0f, this.Day.LightIntensity, num94);
			color = this.SunLightColor;
			this.RayColor = this.SunRayColor;
		}
		else
		{
			this.IsDay = false;
			this.IsNight = true;
			shadowStrength = this.Night.ShadowStrength;
			intensity = Mathf.Lerp(0f, this.Night.LightIntensity, num95);
			color = this.MoonLightColor;
			this.RayColor = this.MoonRayColor;
		}
		this.Components.LightSource.color = color;
		this.Components.LightSource.intensity = intensity;
		this.Components.LightSource.shadowStrength = shadowStrength;
		if (!Application.isPlaying || this.timeSinceLightUpdate >= this.Light.UpdateInterval)
		{
			this.timeSinceLightUpdate = 0f;
			Vector3 localPosition3 = (!this.IsNight) ? this.OrbitalToLocal(Mathf.Min(num44, (1f - this.Light.MinimumHeight) * 3.14159274f / 2f), num45) : this.OrbitalToLocal(Mathf.Min(num88, (1f - this.Light.MinimumHeight) * 3.14159274f / 2f), phi);
			this.Components.LightTransform.localPosition = localPosition3;
			this.Components.LightTransform.LookAt(this.Components.DomeTransform.position);
		}
		else
		{
			this.timeSinceLightUpdate += Time.deltaTime;
		}
		this.SunDirection = -this.Components.SunTransform.forward;
		this.LocalSunDirection = this.Components.DomeTransform.InverseTransformDirection(this.SunDirection);
		this.MoonDirection = -this.Components.MoonTransform.forward;
		this.LocalMoonDirection = this.Components.DomeTransform.InverseTransformDirection(this.MoonDirection);
		this.LightDirection = Vector3.Lerp(this.MoonDirection, this.SunDirection, this.LerpValue * this.LerpValue);
		this.LocalLightDirection = this.Components.DomeTransform.InverseTransformDirection(this.LightDirection);
	}

	internal Vector3 OrbitalToUnity(float radius, float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float num2 = Mathf.Cos(theta);
		float num3 = Mathf.Sin(phi);
		float num4 = Mathf.Cos(phi);
		Vector3 result;
		result.z = radius * num * num4;
		result.y = radius * num2;
		result.x = radius * num * num3;
		return result;
	}

	internal Vector3 OrbitalToLocal(float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float y = Mathf.Cos(theta);
		float num2 = Mathf.Sin(phi);
		float num3 = Mathf.Cos(phi);
		Vector3 result;
		result.z = num * num3;
		result.y = y;
		result.x = num * num2;
		return result;
	}

	internal Color SampleAtmosphere(Vector3 direction, bool directLight = true)
	{
		Vector3 dir = this.Components.DomeTransform.InverseTransformDirection(direction);
		Color color = this.ShaderScatteringColor(dir, directLight);
		color = this.TOD_HDR2LDR(color);
		return this.TOD_LINEAR2GAMMA(color);
	}

	internal SphericalHarmonicsL2 RenderToSphericalHarmonics()
	{
		SphericalHarmonicsL2 result = default(SphericalHarmonicsL2);
		bool directLight = false;
		Color linear = this.AmbientColor.linear;
		Vector3 vector = new Vector3(0.612372458f, 0.5f, 0.612372458f);
		Vector3 up = Vector3.up;
		Color linear2 = this.SampleAtmosphere(up, directLight).linear;
		result.AddDirectionalLight(up, linear2, 0.428571433f);
		Vector3 direction = new Vector3(-vector.x, vector.y, -vector.z);
		Color linear3 = this.SampleAtmosphere(direction, directLight).linear;
		result.AddDirectionalLight(direction, linear3, 0.2857143f);
		Vector3 direction2 = new Vector3(vector.x, vector.y, -vector.z);
		Color linear4 = this.SampleAtmosphere(direction2, directLight).linear;
		result.AddDirectionalLight(direction2, linear4, 0.2857143f);
		Vector3 direction3 = new Vector3(-vector.x, vector.y, vector.z);
		Color linear5 = this.SampleAtmosphere(direction3, directLight).linear;
		result.AddDirectionalLight(direction3, linear5, 0.2857143f);
		Vector3 direction4 = new Vector3(vector.x, vector.y, vector.z);
		Color linear6 = this.SampleAtmosphere(direction4, directLight).linear;
		result.AddDirectionalLight(direction4, linear6, 0.2857143f);
		Vector3 left = Vector3.left;
		Color linear7 = this.SampleAtmosphere(left, directLight).linear;
		result.AddDirectionalLight(left, linear7, 0.142857149f);
		Vector3 right = Vector3.right;
		Color linear8 = this.SampleAtmosphere(right, directLight).linear;
		result.AddDirectionalLight(right, linear8, 0.142857149f);
		Vector3 back = Vector3.back;
		Color linear9 = this.SampleAtmosphere(back, directLight).linear;
		result.AddDirectionalLight(back, linear9, 0.142857149f);
		Vector3 forward = Vector3.forward;
		Color linear10 = this.SampleAtmosphere(forward, directLight).linear;
		result.AddDirectionalLight(forward, linear10, 0.142857149f);
		Vector3 direction5 = new Vector3(-vector.x, -vector.y, -vector.z);
		result.AddDirectionalLight(direction5, linear, 0.2857143f);
		Vector3 direction6 = new Vector3(vector.x, -vector.y, -vector.z);
		result.AddDirectionalLight(direction6, linear, 0.2857143f);
		Vector3 direction7 = new Vector3(-vector.x, -vector.y, vector.z);
		result.AddDirectionalLight(direction7, linear, 0.2857143f);
		Vector3 direction8 = new Vector3(vector.x, -vector.y, vector.z);
		result.AddDirectionalLight(direction8, linear, 0.2857143f);
		Vector3 down = Vector3.down;
		result.AddDirectionalLight(down, linear, 0.428571433f);
		return result;
	}

	internal void RenderToCubemap(RenderTexture targetTexture = null)
	{
		if (!this.Probe)
		{
			this.Probe = new GameObject().AddComponent<ReflectionProbe>();
			this.Probe.name = base.gameObject.name + " Reflection Probe";
			this.Probe.mode = ReflectionProbeMode.Realtime;
		}
		if (this.probeRenderID < 0 || this.Probe.IsFinishedRendering(this.probeRenderID))
		{
			float num = 3.40282347E+38f;
			this.Probe.transform.position = this.Components.DomeTransform.position;
			this.Probe.size = new Vector3(num, num, num);
			this.Probe.intensity = RenderSettings.reflectionIntensity;
			this.Probe.clearFlags = this.Reflection.ClearFlags;
			this.Probe.cullingMask = this.Reflection.CullingMask;
			this.Probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
			this.Probe.timeSlicingMode = this.Reflection.TimeSlicing;
			this.probeRenderID = this.Probe.RenderProbe(targetTexture);
		}
	}

	internal Color SampleFogColor(bool directLight = true)
	{
		Vector3 vector = Vector3.forward;
		if (this.Components.Camera != null)
		{
			vector = Quaternion.Euler(0f, this.Components.Camera.transform.rotation.eulerAngles.y, 0f) * vector;
		}
		Color color = this.SampleAtmosphere(Vector3.Lerp(vector, Vector3.up, this.Fog.HeightBias).normalized, directLight);
		return new Color(color.r, color.g, color.b, 1f);
	}

	internal Color SampleSkyColor()
	{
		Vector3 sunDirection = this.SunDirection;
		sunDirection.y = Mathf.Abs(sunDirection.y);
		Color color = this.SampleAtmosphere(sunDirection.normalized, false);
		return new Color(color.r, color.g, color.b, 1f);
	}

	internal Color SampleEquatorColor()
	{
		Vector3 sunDirection = this.SunDirection;
		sunDirection.y = 0f;
		Color color = this.SampleAtmosphere(sunDirection.normalized, false);
		return new Color(color.r, color.g, color.b, 1f);
	}

	internal void UpdateFog()
	{
		TOD_FogType mode = this.Fog.Mode;
		if (mode != TOD_FogType.None)
		{
			if (mode != TOD_FogType.Color)
			{
				if (mode == TOD_FogType.Directional)
				{
					Color fogColor = this.SampleFogColor(true);
					RenderSettings.fogColor = fogColor;
				}
			}
			else
			{
				Color fogColor2 = this.SampleFogColor(false);
				RenderSettings.fogColor = fogColor2;
			}
		}
	}

	internal void UpdateAmbient()
	{
		float ambientIntensity = Mathf.Lerp(this.Night.AmbientMultiplier, this.Day.AmbientMultiplier, this.LerpValue);
		TOD_AmbientType mode = this.Ambient.Mode;
		if (mode != TOD_AmbientType.Color)
		{
			if (mode != TOD_AmbientType.Gradient)
			{
				if (mode == TOD_AmbientType.Spherical)
				{
					RenderSettings.ambientMode = AmbientMode.Skybox;
					RenderSettings.skybox = this.Resources.SkyboxMaterial;
					RenderSettings.ambientLight = this.AmbientColor;
					RenderSettings.ambientIntensity = ambientIntensity;
					RenderSettings.ambientProbe = this.RenderToSphericalHarmonics();
				}
			}
			else
			{
				Color ambientColor = this.AmbientColor;
				Color ambientEquatorColor = this.SampleEquatorColor();
				Color ambientSkyColor = this.SampleSkyColor();
				RenderSettings.ambientMode = AmbientMode.Trilight;
				RenderSettings.ambientSkyColor = ambientSkyColor;
				RenderSettings.ambientEquatorColor = ambientEquatorColor;
				RenderSettings.ambientGroundColor = ambientColor;
				RenderSettings.ambientIntensity = ambientIntensity;
			}
		}
		else
		{
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientLight = this.AmbientColor;
			RenderSettings.ambientIntensity = ambientIntensity;
		}
	}

	internal void UpdateReflection()
	{
		TOD_ReflectionType mode = this.Reflection.Mode;
		if (mode == TOD_ReflectionType.Cubemap)
		{
			float reflectionIntensity = Mathf.Lerp(this.Night.ReflectionMultiplier, this.Day.ReflectionMultiplier, this.LerpValue);
			RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
			RenderSettings.skybox = this.Resources.SkyboxMaterial;
			RenderSettings.reflectionIntensity = reflectionIntensity;
			if (Application.isPlaying)
			{
				this.RenderToCubemap(null);
			}
		}
	}

	internal void LoadParameters(string xml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(TOD_Parameters));
		XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
		TOD_Parameters tOD_Parameters = xmlSerializer.Deserialize(xmlReader) as TOD_Parameters;
		tOD_Parameters.ToSky(this);
	}

	private void UpdateQualitySettings()
	{
		if (this.Headless)
		{
			return;
		}
		Mesh mesh = null;
		Mesh mesh2 = null;
		Mesh mesh3 = null;
		Mesh mesh4 = null;
		Mesh mesh5 = null;
		Mesh mesh6 = null;
		TOD_MeshQualityType meshQuality = this.MeshQuality;
		if (meshQuality != TOD_MeshQualityType.Low)
		{
			if (meshQuality != TOD_MeshQualityType.Medium)
			{
				if (meshQuality == TOD_MeshQualityType.High)
				{
					mesh = this.Resources.IcosphereHigh;
					mesh2 = this.Resources.IcosphereHigh;
					mesh3 = this.Resources.IcosphereLow;
					mesh4 = this.Resources.HalfIcosphereHigh;
					mesh5 = this.Resources.Quad;
					mesh6 = this.Resources.SphereHigh;
				}
			}
			else
			{
				mesh = this.Resources.IcosphereMedium;
				mesh2 = this.Resources.IcosphereMedium;
				mesh3 = this.Resources.IcosphereLow;
				mesh4 = this.Resources.HalfIcosphereMedium;
				mesh5 = this.Resources.Quad;
				mesh6 = this.Resources.SphereMedium;
			}
		}
		else
		{
			mesh = this.Resources.IcosphereLow;
			mesh2 = this.Resources.IcosphereLow;
			mesh3 = this.Resources.IcosphereLow;
			mesh4 = this.Resources.HalfIcosphereLow;
			mesh5 = this.Resources.Quad;
			mesh6 = this.Resources.SphereLow;
		}
		if (this.Components.SpaceRenderer && this.Components.SpaceMaterial != this.Resources.SpaceMaterial)
		{
			TOD_Components arg_187_0 = this.Components;
			Material material = this.Resources.SpaceMaterial;
			this.Components.SpaceRenderer.sharedMaterial = material;
			arg_187_0.SpaceMaterial = material;
		}
		if (this.Components.AtmosphereRenderer && this.Components.AtmosphereMaterial != this.Resources.AtmosphereMaterial)
		{
			TOD_Components arg_1E8_0 = this.Components;
			Material material = this.Resources.AtmosphereMaterial;
			this.Components.AtmosphereRenderer.sharedMaterial = material;
			arg_1E8_0.AtmosphereMaterial = material;
		}
		if (this.Components.ClearRenderer && this.Components.ClearMaterial != this.Resources.ClearMaterial)
		{
			TOD_Components arg_249_0 = this.Components;
			Material material = this.Resources.ClearMaterial;
			this.Components.ClearRenderer.sharedMaterial = material;
			arg_249_0.ClearMaterial = material;
		}
		if (this.Components.CloudRenderer && this.Components.CloudMaterial != this.Resources.CloudMaterial)
		{
			TOD_Components arg_2AA_0 = this.Components;
			Material material = this.Resources.CloudMaterial;
			this.Components.CloudRenderer.sharedMaterial = material;
			arg_2AA_0.CloudMaterial = material;
		}
		if (this.Components.ShadowProjector && this.Components.ShadowMaterial != this.Resources.ShadowMaterial)
		{
			TOD_Components arg_30B_0 = this.Components;
			Material material = this.Resources.ShadowMaterial;
			this.Components.ShadowProjector.material = material;
			arg_30B_0.ShadowMaterial = material;
		}
		if (this.Components.SunRenderer && this.Components.SunMaterial != this.Resources.SunMaterial)
		{
			TOD_Components arg_36C_0 = this.Components;
			Material material = this.Resources.SunMaterial;
			this.Components.SunRenderer.sharedMaterial = material;
			arg_36C_0.SunMaterial = material;
		}
		if (this.Components.MoonRenderer && this.Components.MoonMaterial != this.Resources.MoonMaterial)
		{
			TOD_Components arg_3CD_0 = this.Components;
			Material material = this.Resources.MoonMaterial;
			this.Components.MoonRenderer.sharedMaterial = material;
			arg_3CD_0.MoonMaterial = material;
		}
		if (this.Components.SpaceMeshFilter && this.Components.SpaceMeshFilter.sharedMesh != mesh)
		{
			this.Components.SpaceMeshFilter.mesh = mesh;
		}
		if (this.Components.AtmosphereMeshFilter && this.Components.AtmosphereMeshFilter.sharedMesh != mesh2)
		{
			this.Components.AtmosphereMeshFilter.mesh = mesh2;
		}
		if (this.Components.ClearMeshFilter && this.Components.ClearMeshFilter.sharedMesh != mesh3)
		{
			this.Components.ClearMeshFilter.mesh = mesh3;
		}
		if (this.Components.CloudMeshFilter && this.Components.CloudMeshFilter.sharedMesh != mesh4)
		{
			this.Components.CloudMeshFilter.mesh = mesh4;
		}
		if (this.Components.SunMeshFilter && this.Components.SunMeshFilter.sharedMesh != mesh5)
		{
			this.Components.SunMeshFilter.mesh = mesh5;
		}
		if (this.Components.MoonMeshFilter && this.Components.MoonMeshFilter.sharedMesh != mesh6)
		{
			this.Components.MoonMeshFilter.mesh = mesh6;
		}
	}

	private void UpdateRenderSettings()
	{
		if (this.Headless)
		{
			return;
		}
		this.UpdateFog();
		if (!Application.isPlaying || this.timeSinceAmbientUpdate >= this.Ambient.UpdateInterval)
		{
			this.timeSinceAmbientUpdate = 0f;
			this.UpdateAmbient();
		}
		else
		{
			this.timeSinceAmbientUpdate += Time.deltaTime;
		}
		if (!Application.isPlaying || this.timeSinceReflectionUpdate >= this.Reflection.UpdateInterval)
		{
			this.timeSinceReflectionUpdate = 0f;
			this.UpdateReflection();
		}
		else
		{
			this.timeSinceReflectionUpdate += Time.deltaTime;
		}
	}

	private void UpdateShaderKeywords()
	{
		if (this.Headless)
		{
			return;
		}
		if (this.Resources.CloudMaterial)
		{
			this.SetCloudQuality(this.Resources.CloudMaterial);
			this.SetColorSpace(this.Resources.CloudMaterial);
			this.SetColorRange(this.Resources.CloudMaterial);
		}
		if (this.Resources.BillboardMaterial)
		{
			this.SetCloudQuality(this.Resources.BillboardMaterial);
			this.SetColorSpace(this.Resources.BillboardMaterial);
			this.SetColorRange(this.Resources.BillboardMaterial);
		}
		if (this.Resources.ShadowMaterial)
		{
			this.SetCloudQuality(this.Resources.ShadowMaterial);
		}
		if (this.Resources.AtmosphereMaterial)
		{
			this.SetSkyQuality(this.Resources.AtmosphereMaterial);
			this.SetColorSpace(this.Resources.AtmosphereMaterial);
			this.SetColorRange(this.Resources.AtmosphereMaterial);
		}
		if (this.Resources.SkyboxMaterial)
		{
			this.SetColorSpace(this.Resources.SkyboxMaterial);
			this.SetColorRange(this.Resources.SkyboxMaterial);
		}
	}

	private void UpdateShaderProperties()
	{
		if (this.Headless)
		{
			return;
		}
		Vector4 value = this.Components.Animation.CloudUV + this.Components.Animation.OffsetUV;
		Vector4 value2 = new Vector4(this.Clouds.Scale1.x, this.Clouds.Scale1.y, this.Clouds.Scale2.x, this.Clouds.Scale2.y);
		float value3 = this.Clouds.ShadowStrength * Mathf.Clamp01(1f - this.LightZenith / 90f);
		Shader.SetGlobalColor(this.Resources.ID_SunSkyColor, this.SunSkyColor);
		Shader.SetGlobalColor(this.Resources.ID_MoonSkyColor, this.MoonSkyColor);
		Shader.SetGlobalColor(this.Resources.ID_SunCloudColor, this.CloudColor * this.SunLightColor);
		Shader.SetGlobalColor(this.Resources.ID_MoonCloudColor, this.CloudColor * this.MoonLightColor);
		Shader.SetGlobalColor(this.Resources.ID_SunMeshColor, this.SunMeshColor);
		Shader.SetGlobalColor(this.Resources.ID_MoonMeshColor, this.MoonMeshColor);
		Shader.SetGlobalColor(this.Resources.ID_CloudColor, this.CloudColor);
		Shader.SetGlobalColor(this.Resources.ID_AmbientColor, this.AmbientColor);
		Shader.SetGlobalColor(this.Resources.ID_MoonHaloColor, this.MoonHaloColor);
		Shader.SetGlobalVector(this.Resources.ID_SunDirection, this.SunDirection);
		Shader.SetGlobalVector(this.Resources.ID_MoonDirection, this.MoonDirection);
		Shader.SetGlobalVector(this.Resources.ID_LightDirection, this.LightDirection);
		Shader.SetGlobalVector(this.Resources.ID_LocalSunDirection, this.LocalSunDirection);
		Shader.SetGlobalVector(this.Resources.ID_LocalMoonDirection, this.LocalMoonDirection);
		Shader.SetGlobalVector(this.Resources.ID_LocalLightDirection, this.LocalLightDirection);
		Shader.SetGlobalFloat(this.Resources.ID_Contrast, this.Atmosphere.Contrast);
		Shader.SetGlobalFloat(this.Resources.ID_Brightness, this.Atmosphere.Brightness);
		Shader.SetGlobalFloat(this.Resources.ID_Fogginess, this.Atmosphere.Fogginess);
		Shader.SetGlobalFloat(this.Resources.ID_Directionality, this.Atmosphere.Directionality);
		Shader.SetGlobalFloat(this.Resources.ID_MoonHaloPower, 1f / this.Moon.HaloSize);
		Shader.SetGlobalFloat(this.Resources.ID_CloudDensity, this.Clouds.Density);
		Shader.SetGlobalFloat(this.Resources.ID_CloudSharpness, this.Clouds.Sharpness);
		Shader.SetGlobalFloat(this.Resources.ID_CloudShadow, value3);
		Shader.SetGlobalVector(this.Resources.ID_CloudScale, value2);
		Shader.SetGlobalVector(this.Resources.ID_CloudUV, value);
		Shader.SetGlobalFloat(this.Resources.ID_SpaceTiling, this.Stars.Tiling);
		Shader.SetGlobalFloat(this.Resources.ID_SpaceBrightness, this.Stars.Brightness * (1f - this.Atmosphere.Fogginess) * (1f - this.LerpValue));
		Shader.SetGlobalFloat(this.Resources.ID_SunMeshContrast, 2f / this.Sun.MeshContrast);
		Shader.SetGlobalFloat(this.Resources.ID_SunMeshBrightness, this.Sun.MeshBrightness * (1f - this.Atmosphere.Fogginess));
		Shader.SetGlobalFloat(this.Resources.ID_MoonMeshContrast, 1f / this.Moon.MeshContrast);
		Shader.SetGlobalFloat(this.Resources.ID_MoonMeshBrightness, this.Moon.MeshBrightness * (1f - this.Atmosphere.Fogginess));
		Shader.SetGlobalVector(this.Resources.ID_kBetaMie, this.kBetaMie);
		Shader.SetGlobalVector(this.Resources.ID_kSun, this.kSun);
		Shader.SetGlobalVector(this.Resources.ID_k4PI, this.k4PI);
		Shader.SetGlobalVector(this.Resources.ID_kRadius, this.kRadius);
		Shader.SetGlobalVector(this.Resources.ID_kScale, this.kScale);
		Shader.SetGlobalMatrix(this.Resources.ID_World2Sky, this.Components.DomeTransform.worldToLocalMatrix);
		Shader.SetGlobalMatrix(this.Resources.ID_Sky2World, this.Components.DomeTransform.localToWorldMatrix);
		if (this.Components.ShadowProjector)
		{
			float farClipPlane = this.Radius * 2f;
			float radius = this.Radius;
			this.Components.ShadowProjector.farClipPlane = farClipPlane;
			this.Components.ShadowProjector.orthographicSize = radius;
		}
	}

	private void SetColorSpace(Material material)
	{
		TOD_ColorSpaceType colorSpace = this.ColorSpace;
		if (colorSpace != TOD_ColorSpaceType.Auto)
		{
			if (colorSpace != TOD_ColorSpaceType.Linear)
			{
				if (colorSpace == TOD_ColorSpaceType.Gamma)
				{
					material.DisableKeyword("LINEAR");
					material.EnableKeyword("GAMMA");
				}
			}
			else
			{
				material.EnableKeyword("LINEAR");
				material.DisableKeyword("GAMMA");
			}
		}
		else if (QualitySettings.activeColorSpace == UnityEngine.ColorSpace.Linear)
		{
			material.EnableKeyword("LINEAR");
			material.DisableKeyword("GAMMA");
		}
		else
		{
			material.DisableKeyword("LINEAR");
			material.EnableKeyword("GAMMA");
		}
	}

	private void SetColorRange(Material material)
	{
		TOD_ColorRangeType colorRange = this.ColorRange;
		if (colorRange != TOD_ColorRangeType.Auto)
		{
			if (colorRange != TOD_ColorRangeType.HDR)
			{
				if (colorRange == TOD_ColorRangeType.LDR)
				{
					material.DisableKeyword("HDR");
					material.EnableKeyword("LDR");
				}
			}
			else
			{
				material.EnableKeyword("HDR");
				material.DisableKeyword("LDR");
			}
		}
		else if (this.Components.Camera && this.Components.Camera.HDR)
		{
			material.EnableKeyword("HDR");
			material.DisableKeyword("LDR");
		}
		else
		{
			material.DisableKeyword("HDR");
			material.EnableKeyword("LDR");
		}
	}

	private void SetSkyQuality(Material material)
	{
		TOD_SkyQualityType skyQuality = this.SkyQuality;
		if (skyQuality != TOD_SkyQualityType.PerVertex)
		{
			if (skyQuality == TOD_SkyQualityType.PerPixel)
			{
				material.DisableKeyword("PER_VERTEX");
				material.EnableKeyword("PER_PIXEL");
			}
		}
		else
		{
			material.EnableKeyword("PER_VERTEX");
			material.DisableKeyword("PER_PIXEL");
		}
	}

	private void SetCloudQuality(Material material)
	{
		TOD_CloudQualityType cloudQuality = this.CloudQuality;
		if (cloudQuality != TOD_CloudQualityType.Fastest)
		{
			if (cloudQuality != TOD_CloudQualityType.Density)
			{
				if (cloudQuality == TOD_CloudQualityType.Bumped)
				{
					material.DisableKeyword("FASTEST");
					material.DisableKeyword("DENSITY");
					material.EnableKeyword("BUMPED");
				}
			}
			else
			{
				material.DisableKeyword("FASTEST");
				material.EnableKeyword("DENSITY");
				material.DisableKeyword("BUMPED");
			}
		}
		else
		{
			material.EnableKeyword("FASTEST");
			material.DisableKeyword("DENSITY");
			material.DisableKeyword("BUMPED");
		}
	}

	private float ShaderScale(float inCos)
	{
		float num = 1f - inCos;
		return 0.25f * Mathf.Exp(-0.00287f + num * (0.459f + num * (3.83f + num * (-6.8f + num * 5.25f))));
	}

	private float ShaderMiePhase(float eyeCos, float eyeCos2)
	{
		return this.kBetaMie.x * (1f + eyeCos2) / Mathf.Pow(this.kBetaMie.y + this.kBetaMie.z * eyeCos, 1.5f);
	}

	private float ShaderRayleighPhase(float eyeCos2)
	{
		return 0.75f + 0.75f * eyeCos2;
	}

	private Color ShaderNightSkyColor(Vector3 dir)
	{
		return Color.Lerp(this.MoonSkyColor, Color.black, dir.y);
	}

	private Color ShaderMoonHaloColor(Vector3 dir)
	{
		return this.MoonHaloColor * Mathf.Pow(Mathf.Max(0f, Vector3.Dot(dir, this.LocalMoonDirection)), 1f / this.Moon.HaloSize);
	}

	private Color TOD_HDR2LDR(Color color)
	{
		return new Color(1f - Mathf.Pow(2f, -this.Atmosphere.Brightness * color.r), 1f - Mathf.Pow(2f, -this.Atmosphere.Brightness * color.g), 1f - Mathf.Pow(2f, -this.Atmosphere.Brightness * color.b), color.a);
	}

	private Color TOD_GAMMA2LINEAR(Color color)
	{
		return new Color(color.r * color.r, color.g * color.g, color.b * color.b, color.a);
	}

	private Color TOD_LINEAR2GAMMA(Color color)
	{
		return new Color(Mathf.Sqrt(color.r), Mathf.Sqrt(color.g), Mathf.Sqrt(color.b), color.a);
	}

	private Color ShaderScatteringColor(Vector3 dir, bool directLight = true)
	{
		dir.y = Mathf.Clamp01(dir.y);
		float x = this.kRadius.x;
		float y = this.kRadius.y;
		float w = this.kRadius.w;
		float x2 = this.kScale.x;
		float z = this.kScale.z;
		float w2 = this.kScale.w;
		float x3 = this.k4PI.x;
		float y2 = this.k4PI.y;
		float z2 = this.k4PI.z;
		float w3 = this.k4PI.w;
		float x4 = this.kSun.x;
		float y3 = this.kSun.y;
		float z3 = this.kSun.z;
		float w4 = this.kSun.w;
		Vector3 vector = new Vector3(0f, x + w2, 0f);
		float num = Mathf.Sqrt(w + y * dir.y * dir.y - y) - x * dir.y;
		float num2 = Mathf.Exp(z * -w2);
		float inCos = Vector3.Dot(dir, vector) / (x + w2);
		float num3 = num2 * this.ShaderScale(inCos);
		float num4 = num / 2f;
		float num5 = num4 * x2;
		Vector3 vector2 = dir * num4;
		Vector3 vector3 = vector + vector2 * 0.5f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		for (int i = 0; i < 2; i++)
		{
			float magnitude = vector3.magnitude;
			float num9 = 1f / magnitude;
			float num10 = Mathf.Exp(z * (x - magnitude));
			float num11 = num10 * num5;
			float inCos2 = Vector3.Dot(dir, vector3) * num9;
			float inCos3 = Vector3.Dot(this.LocalSunDirection, vector3) * num9;
			float num12 = num3 + num10 * (this.ShaderScale(inCos3) - this.ShaderScale(inCos2));
			float num13 = Mathf.Exp(-num12 * (x3 + w3));
			float num14 = Mathf.Exp(-num12 * (y2 + w3));
			float num15 = Mathf.Exp(-num12 * (z2 + w3));
			num6 += num13 * num11;
			num7 += num14 * num11;
			num8 += num15 * num11;
			vector3 += vector2;
		}
		float num16 = this.SunSkyColor.r * num6 * x4;
		float num17 = this.SunSkyColor.g * num7 * y3;
		float num18 = this.SunSkyColor.b * num8 * z3;
		float num19 = this.SunSkyColor.r * num6 * w4;
		float num20 = this.SunSkyColor.g * num7 * w4;
		float num21 = this.SunSkyColor.b * num8 * w4;
		float num22 = 0f;
		float num23 = 0f;
		float num24 = 0f;
		float t = Mathf.SmoothStep(0f, 1.25f, -dir.y);
		float num25 = Vector3.Dot(this.LocalSunDirection, dir);
		float eyeCos = num25 * num25;
		float num26 = this.ShaderRayleighPhase(eyeCos);
		num22 += num26 * num16;
		num23 += num26 * num17;
		num24 += num26 * num18;
		if (directLight)
		{
			float num27 = this.ShaderMiePhase(num25, eyeCos);
			num22 += num27 * num19;
			num23 += num27 * num20;
			num24 += num27 * num21;
		}
		Color color = this.ShaderNightSkyColor(dir);
		num22 += color.r;
		num23 += color.g;
		num24 += color.b;
		if (directLight)
		{
			Color color2 = this.ShaderMoonHaloColor(dir);
			num22 += color2.r;
			num23 += color2.g;
			num24 += color2.b;
		}
		num22 = Mathf.Lerp(num22, this.CloudColor.r, this.Atmosphere.Fogginess);
		num23 = Mathf.Lerp(num23, this.CloudColor.g, this.Atmosphere.Fogginess);
		num24 = Mathf.Lerp(num24, this.CloudColor.b, this.Atmosphere.Fogginess);
		num22 = Mathf.Lerp(num22, this.AmbientColor.r, t);
		num23 = Mathf.Lerp(num23, this.AmbientColor.g, t);
		num24 = Mathf.Lerp(num24, this.AmbientColor.b, t);
		num22 = Mathf.Pow(num22 * this.Atmosphere.Brightness, this.Atmosphere.Contrast);
		num23 = Mathf.Pow(num23 * this.Atmosphere.Brightness, this.Atmosphere.Contrast);
		num24 = Mathf.Pow(num24 * this.Atmosphere.Brightness, this.Atmosphere.Contrast);
		return new Color(num22, num23, num24, 1f);
	}

	protected void OnEnable()
	{
		this.Components = base.GetComponent<TOD_Components>();
		this.Components.Initialize();
		this.Resources = base.GetComponent<TOD_Resources>();
		this.Resources.Initialize();
		this.LateUpdate();
		TOD_Sky.instances.Add(this);
		this.Initialized = true;
	}

	protected void OnDisable()
	{
		TOD_Sky.instances.Remove(this);
		if (this.Probe)
		{
			UnityEngine.Object.Destroy(this.Probe.gameObject);
		}
	}

	protected void Start()
	{
		if (Application.isPlaying)
		{
			Vector2 mainTextureScale = this.Resources.BillboardMaterial.mainTextureScale;
			int num = Mathf.RoundToInt(1f / mainTextureScale.x);
			int num2 = Mathf.RoundToInt(1f / mainTextureScale.y);
			Mesh[] array = new Mesh[2 * num * num2];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					array[i * num + j] = TOD_Resources.CreateQuad(new Vector2((float)j, (float)i), new Vector2((float)(j + 1), (float)(i + 1)));
				}
			}
			for (int k = 0; k < num2; k++)
			{
				for (int l = 0; l < num; l++)
				{
					array[num * num2 + k * num + l] = TOD_Resources.CreateQuad(new Vector2((float)(l + 1), (float)k), new Vector2((float)l, (float)(k + 1)));
				}
			}
			for (int m = 0; m < this.Clouds.Billboards; m++)
			{
				GameObject gameObject = new GameObject("Cloud " + m);
				gameObject.transform.parent = this.Components.Billboards.transform;
				float num3 = UnityEngine.Random.Range(0.3f, 0.4f);
				gameObject.transform.localScale = new Vector3(num3, num3 * 0.5f, 1f);
				float f = 6.28318548f * ((float)m / (float)this.Clouds.Billboards);
				Transform arg_1B9_0 = gameObject.transform;
				float arg_1B4_0 = 0.95f;
				Vector3 vector = new Vector3(Mathf.Sin(f), UnityEngine.Random.Range(0.1f, 0.2f), Mathf.Cos(f));
				arg_1B9_0.localPosition = arg_1B4_0 * vector.normalized;
				gameObject.transform.LookAt(this.Components.DomeTransform.position);
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = array[UnityEngine.Random.Range(0, array.Length)];
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterial = this.Resources.BillboardMaterial;
			}
		}
	}

	protected void LateUpdate()
	{
		this.UpdateScattering();
		this.UpdateCelestials();
		this.UpdateQualitySettings();
		this.UpdateRenderSettings();
		this.UpdateShaderKeywords();
		this.UpdateShaderProperties();
	}

	protected void OnValidate()
	{
		this.Cycle.DateTime = this.Cycle.DateTime;
	}
}
