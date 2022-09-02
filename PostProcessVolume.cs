using System;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(SphereCollider))]
public class PostProcessVolume : MonoBehaviour
{
	private SphereCollider _sphereCollider;

	private BoxCollider _boxCollider;

	[Header("VolumeMode")]
	public VolumeShape ShapeOfVolume;

	[Header("SphereVolumeSetting")]
	public float OuterSphereRadius = 1f;

	public float InnerSphereRadius;

	[Header("BoxVolumeSetting")]
	public Vector3 OuterBoxSize = Vector3.one;

	private Vector3 _outerBoxSize = Vector3.one;

	public float OuterBoxSizeMultiplier = 1f;

	private float _outerBoxSizeMultiplier = 1f;

	public Vector3 InnerBoxSize = Vector3.zero;

	private Vector3 _innerBoxSize = Vector3.zero;

	public float InnerBoxSizeMultiplier = 1f;

	private float _innerBoxSizeMultiplier = 1f;

	[Header("PostProcessing Effects")]
	public AntialiasingModel antialiasing = new AntialiasingModel();

	public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

	public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

	public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

	public MotionBlurModel motionBlur = new MotionBlurModel();

	public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

	public BloomModel bloom = new BloomModel();

	public ColorGradingModel colorGrading = new ColorGradingModel();

	public UserLutModel userLut = new UserLutModel();

	public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

	public GrainModel grain = new GrainModel();

	public VignetteModel vignette = new VignetteModel();

	public DitheringModel dithering = new DitheringModel();

	[Header("ResetAllValues")]
	public PostProcessingProfile _ResetProfile;

	private bool _hasJustStarted = true;

	private void Start()
	{
		this._sphereCollider = base.GetComponent<SphereCollider>();
		this._sphereCollider.isTrigger = true;
		this._boxCollider = base.GetComponent<BoxCollider>();
		this._boxCollider.isTrigger = true;
		base.transform.localScale = Vector3.one;
		if (this.ShapeOfVolume == VolumeShape.BOX)
		{
			this._boxCollider.enabled = true;
			this._sphereCollider.enabled = false;
		}
		else if (this.ShapeOfVolume == VolumeShape.SPHERE)
		{
			this._boxCollider.enabled = false;
			this._sphereCollider.enabled = true;
		}
	}

	private void OnValidate()
	{
		this.CheckColliderShape();
	}

	private void Update()
	{
	}

	private void OnTriggerStay(Collider other)
	{
		PostProcessVolumeReceiver component = other.gameObject.GetComponent<PostProcessVolumeReceiver>();
		if (component != null)
		{
			PostProcessVolume component2 = base.GetComponent<PostProcessVolume>();
			component.SetValues(ref component2, this.GradientPercentage(other.transform.position));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		PostProcessVolumeReceiver component = other.gameObject.GetComponent<PostProcessVolumeReceiver>();
		if (component != null)
		{
			component.ResetValues();
		}
	}

	private float GradientPercentage(Vector3 position)
	{
		Vector3 point = base.transform.position - position;
		VolumeShape shapeOfVolume = this.ShapeOfVolume;
		if (shapeOfVolume == VolumeShape.BOX)
		{
			point = Quaternion.Inverse(base.transform.rotation) * point;
			float num = Mathf.Clamp01((Mathf.Abs(point.x) - this.InnerBoxSize.x * 0.5f) / ((this._boxCollider.size.x - this.InnerBoxSize.x) * 0.5f));
			float num2 = Mathf.Clamp01((Mathf.Abs(point.y) - this.InnerBoxSize.y * 0.5f) / ((this._boxCollider.size.y - this.InnerBoxSize.y) * 0.5f));
			float num3 = Mathf.Clamp01((Mathf.Abs(point.z) - this.InnerBoxSize.z * 0.5f) / ((this._boxCollider.size.z - this.InnerBoxSize.z) * 0.5f));
			float num4 = Mathf.Max(new float[]
			{
				num,
				num2,
				num3
			});
			return Mathf.Clamp01(1f - num4);
		}
		if (shapeOfVolume != VolumeShape.SPHERE)
		{
			return 0f;
		}
		float num5 = (point.magnitude - this.InnerSphereRadius) / (this._sphereCollider.radius - this.InnerSphereRadius);
		return Mathf.Clamp01(1f - num5);
	}

	public void ResetValues()
	{
		if (this._ResetProfile == null)
		{
			Debug.LogError("No ProfileEntered");
			return;
		}
		this.antialiasing.enabled = this._ResetProfile.antialiasing.enabled;
		this.ambientOcclusion.enabled = this._ResetProfile.ambientOcclusion.enabled;
		this.screenSpaceReflection.enabled = this._ResetProfile.screenSpaceReflection.enabled;
		this.depthOfField.enabled = this._ResetProfile.depthOfField.enabled;
		this.motionBlur.enabled = this._ResetProfile.motionBlur.enabled;
		this.eyeAdaptation.enabled = this._ResetProfile.eyeAdaptation.enabled;
		this.bloom.enabled = this._ResetProfile.bloom.enabled;
		this.colorGrading.enabled = this._ResetProfile.colorGrading.enabled;
		this.userLut.enabled = this._ResetProfile.userLut.enabled;
		this.chromaticAberration.enabled = this._ResetProfile.chromaticAberration.enabled;
		this.grain.enabled = this._ResetProfile.grain.enabled;
		this.vignette.enabled = this._ResetProfile.vignette.enabled;
		this.dithering.enabled = this._ResetProfile.dithering.enabled;
		this.antialiasing.settings = this._ResetProfile.antialiasing.settings;
		this.ambientOcclusion.settings = this._ResetProfile.ambientOcclusion.settings;
		this.screenSpaceReflection.settings = this._ResetProfile.screenSpaceReflection.settings;
		this.depthOfField.settings = this._ResetProfile.depthOfField.settings;
		this.motionBlur.settings = this._ResetProfile.motionBlur.settings;
		this.eyeAdaptation.settings = this._ResetProfile.eyeAdaptation.settings;
		this.bloom.settings = this._ResetProfile.bloom.settings;
		this.colorGrading.settings = this._ResetProfile.colorGrading.settings;
		this.userLut.settings = this._ResetProfile.userLut.settings;
		this.chromaticAberration.settings = this._ResetProfile.chromaticAberration.settings;
		this.grain.settings = this._ResetProfile.grain.settings;
		this.vignette.settings = this._ResetProfile.vignette.settings;
		this.dithering.settings = this._ResetProfile.dithering.settings;
	}

	public void CheckColliderShape()
	{
		if (this._hasJustStarted)
		{
			this._hasJustStarted = false;
			this._outerBoxSize = this.OuterBoxSize / this.OuterBoxSizeMultiplier;
			this._innerBoxSize = this.InnerBoxSize / this.InnerBoxSizeMultiplier;
		}
		base.transform.localScale = Vector3.one;
		if (this._boxCollider == null || this._sphereCollider == null)
		{
			this._sphereCollider = base.GetComponent<SphereCollider>();
			this._sphereCollider.isTrigger = true;
			this._boxCollider = base.GetComponent<BoxCollider>();
			this._boxCollider.isTrigger = true;
		}
		this.OuterSphereRadius = Mathf.Clamp(this.OuterSphereRadius, 0f, float.PositiveInfinity);
		this._sphereCollider.radius = this.OuterSphereRadius;
		this.InnerSphereRadius = Mathf.Clamp(this.InnerSphereRadius, 0f, this._sphereCollider.radius);
		if (this._outerBoxSizeMultiplier != this.OuterBoxSizeMultiplier)
		{
			this.OuterBoxSizeMultiplier = Mathf.Clamp(this.OuterBoxSizeMultiplier, 0.01f, float.PositiveInfinity);
			this.OuterBoxSize = this._outerBoxSize * this.OuterBoxSizeMultiplier;
			this._outerBoxSizeMultiplier = this.OuterBoxSizeMultiplier;
		}
		this.OuterBoxSize.x = Mathf.Clamp(this.OuterBoxSize.x, 0f, float.PositiveInfinity);
		this.OuterBoxSize.y = Mathf.Clamp(this.OuterBoxSize.y, 0f, float.PositiveInfinity);
		this.OuterBoxSize.z = Mathf.Clamp(this.OuterBoxSize.z, 0f, float.PositiveInfinity);
		this._outerBoxSize = this.OuterBoxSize / this._outerBoxSizeMultiplier;
		this._boxCollider.size = this.OuterBoxSize;
		if (this._innerBoxSizeMultiplier != this.InnerBoxSizeMultiplier)
		{
			this.InnerBoxSizeMultiplier = Mathf.Clamp(this.InnerBoxSizeMultiplier, 0.01f, float.PositiveInfinity);
			this.InnerBoxSize = this._innerBoxSize * this.InnerBoxSizeMultiplier;
			this._innerBoxSizeMultiplier = this.InnerBoxSizeMultiplier;
		}
		this._innerBoxSize = this.InnerBoxSize / this._innerBoxSizeMultiplier;
		this.InnerBoxSize.x = Mathf.Clamp(this.InnerBoxSize.x, 0f, this._boxCollider.size.x);
		this.InnerBoxSize.y = Mathf.Clamp(this.InnerBoxSize.y, 0f, this._boxCollider.size.y);
		this.InnerBoxSize.z = Mathf.Clamp(this.InnerBoxSize.z, 0f, this._boxCollider.size.z);
		if (this.ShapeOfVolume == VolumeShape.BOX)
		{
			this._boxCollider.enabled = true;
			this._sphereCollider.enabled = false;
		}
		else if (this.ShapeOfVolume == VolumeShape.SPHERE)
		{
			this._boxCollider.enabled = false;
			this._sphereCollider.enabled = true;
		}
	}

	private void OnDrawGizmos()
	{
		this.CheckColliderShape();
		Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
		Gizmos.matrix = matrix;
		VolumeShape shapeOfVolume = this.ShapeOfVolume;
		if (shapeOfVolume != VolumeShape.BOX)
		{
			if (shapeOfVolume == VolumeShape.SPHERE)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(Vector3.zero, this._sphereCollider.radius);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(Vector3.zero, this.InnerSphereRadius);
			}
		}
		else
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(Vector3.zero, this._boxCollider.size);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(Vector3.zero, this.InnerBoxSize);
		}
		Gizmos.matrix = Matrix4x4.identity;
	}
}
