using System;
using UnityEngine;

[ExecuteInEditMode]
public class TOD_Components : MonoBehaviour
{
	public GameObject Sun;

	public GameObject Moon;

	public GameObject Atmosphere;

	public GameObject Clear;

	public GameObject Clouds;

	public GameObject Space;

	public GameObject Light;

	public GameObject Projector;

	public GameObject Billboards;

	internal Transform DomeTransform;

	internal Transform SunTransform;

	internal Transform MoonTransform;

	internal Transform LightTransform;

	internal Transform SpaceTransform;

	internal Renderer SpaceRenderer;

	internal Renderer AtmosphereRenderer;

	internal Renderer ClearRenderer;

	internal Renderer CloudRenderer;

	internal Renderer SunRenderer;

	internal Renderer MoonRenderer;

	internal MeshFilter SpaceMeshFilter;

	internal MeshFilter AtmosphereMeshFilter;

	internal MeshFilter ClearMeshFilter;

	internal MeshFilter CloudMeshFilter;

	internal MeshFilter SunMeshFilter;

	internal MeshFilter MoonMeshFilter;

	internal Material SpaceMaterial;

	internal Material AtmosphereMaterial;

	internal Material ClearMaterial;

	internal Material CloudMaterial;

	internal Material SunMaterial;

	internal Material MoonMaterial;

	internal Material ShadowMaterial;

	internal Light LightSource;

	internal Projector ShadowProjector;

	internal TOD_Sky Sky;

	internal TOD_Animation Animation;

	internal TOD_TimeOrig Time;

	internal TOD_Weather Weather;

	internal TOD_Camera Camera;

	internal TOD_Rays Rays;

	internal TOD_Scattering Scattering;

	public void Initialize()
	{
		this.DomeTransform = base.GetComponent<Transform>();
		this.Sky = base.GetComponent<TOD_Sky>();
		this.Animation = base.GetComponent<TOD_Animation>();
		this.Time = base.GetComponent<TOD_TimeOrig>();
		this.Weather = base.GetComponent<TOD_Weather>();
		if (this.Space)
		{
			this.SpaceTransform = this.Space.GetComponent<Transform>();
			this.SpaceRenderer = this.Space.GetComponent<Renderer>();
			this.SpaceMaterial = this.SpaceRenderer.sharedMaterial;
			this.SpaceMeshFilter = this.Space.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Space reference not set.");
		}
		if (this.Atmosphere)
		{
			this.AtmosphereRenderer = this.Atmosphere.GetComponent<Renderer>();
			this.AtmosphereMaterial = this.AtmosphereRenderer.sharedMaterial;
			this.AtmosphereMeshFilter = this.Atmosphere.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Atmosphere reference not set.");
		}
		if (this.Clear)
		{
			this.ClearRenderer = this.Clear.GetComponent<Renderer>();
			this.ClearMaterial = this.ClearRenderer.sharedMaterial;
			this.ClearMeshFilter = this.Clear.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Clear reference not set.");
		}
		if (this.Clouds)
		{
			this.CloudRenderer = this.Clouds.GetComponent<Renderer>();
			this.CloudMaterial = this.CloudRenderer.sharedMaterial;
			this.CloudMeshFilter = this.Clouds.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Clouds reference not set.");
		}
		if (this.Projector)
		{
			this.ShadowProjector = this.Projector.GetComponent<Projector>();
			this.ShadowMaterial = this.ShadowProjector.material;
		}
		else
		{
			Debug.LogError("Projector reference not set.");
		}
		if (this.Light)
		{
			this.LightTransform = this.Light.GetComponent<Transform>();
			this.LightSource = this.Light.GetComponent<Light>();
		}
		else
		{
			Debug.LogError("Light reference not set.");
		}
		if (this.Sun)
		{
			this.SunTransform = this.Sun.GetComponent<Transform>();
			this.SunRenderer = this.Sun.GetComponent<Renderer>();
			this.SunMaterial = this.SunRenderer.sharedMaterial;
			this.SunMeshFilter = this.Sun.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Sun reference not set.");
		}
		if (this.Moon)
		{
			this.MoonTransform = this.Moon.GetComponent<Transform>();
			this.MoonRenderer = this.Moon.GetComponent<Renderer>();
			this.MoonMaterial = this.MoonRenderer.sharedMaterial;
			this.MoonMeshFilter = this.Moon.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Moon reference not set.");
		}
		if (!this.Billboards)
		{
			Debug.LogError("Billboards reference not set.");
		}
	}
}
