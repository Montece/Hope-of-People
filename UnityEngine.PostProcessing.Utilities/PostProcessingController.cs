using System;

namespace UnityEngine.PostProcessing.Utilities
{
	[RequireComponent(typeof(PostProcessingBehaviour))]
	public class PostProcessingController : MonoBehaviour
	{
		public bool controlAntialiasing;

		public bool enableAntialiasing;

		public AntialiasingModel.Settings antialiasing;

		public bool controlAmbientOcclusion;

		public bool enableAmbientOcclusion;

		public AmbientOcclusionModel.Settings ambientOcclusion;

		public bool controlScreenSpaceReflection;

		public bool enableScreenSpaceReflection;

		public ScreenSpaceReflectionModel.Settings screenSpaceReflection;

		public bool controlDepthOfField = true;

		public bool enableDepthOfField;

		public DepthOfFieldModel.Settings depthOfField;

		public bool controlMotionBlur;

		public bool enableMotionBlur;

		public MotionBlurModel.Settings motionBlur;

		public bool controlEyeAdaptation;

		public bool enableEyeAdaptation;

		public EyeAdaptationModel.Settings eyeAdaptation;

		public bool controlBloom;

		public bool enableBloom;

		public BloomModel.Settings bloom;

		public bool controlColorGrading;

		public bool enableColorGrading;

		public ColorGradingModel.Settings colorGrading;

		public bool controlUserLut;

		public bool enableUserLut;

		public UserLutModel.Settings userLut;

		public bool controlChromaticAberration;

		public bool enableChromaticAberration;

		public ChromaticAberrationModel.Settings chromaticAberration;

		public bool controlGrain;

		public bool enableGrain;

		public GrainModel.Settings grain;

		public bool controlVignette;

		public bool enableVignette;

		public VignetteModel.Settings vignette;

		private PostProcessingProfile _profile;

		private void Start()
		{
			PostProcessingBehaviour component = base.GetComponent<PostProcessingBehaviour>();
			this._profile = UnityEngine.Object.Instantiate<PostProcessingProfile>(component.profile);
			component.profile = this._profile;
			this.enableAntialiasing = this._profile.antialiasing.enabled;
			this.antialiasing = this._profile.antialiasing.settings;
			this.enableAmbientOcclusion = this._profile.ambientOcclusion.enabled;
			this.ambientOcclusion = this._profile.ambientOcclusion.settings;
			this.enableScreenSpaceReflection = this._profile.screenSpaceReflection.enabled;
			this.screenSpaceReflection = this._profile.screenSpaceReflection.settings;
			this.enableDepthOfField = this._profile.depthOfField.enabled;
			this.depthOfField = this._profile.depthOfField.settings;
			this.enableMotionBlur = this._profile.motionBlur.enabled;
			this.motionBlur = this._profile.motionBlur.settings;
			this.enableEyeAdaptation = this._profile.eyeAdaptation.enabled;
			this.eyeAdaptation = this._profile.eyeAdaptation.settings;
			this.enableBloom = this._profile.bloom.enabled;
			this.bloom = this._profile.bloom.settings;
			this.enableColorGrading = this._profile.colorGrading.enabled;
			this.colorGrading = this._profile.colorGrading.settings;
			this.enableUserLut = this._profile.userLut.enabled;
			this.userLut = this._profile.userLut.settings;
			this.enableChromaticAberration = this._profile.chromaticAberration.enabled;
			this.chromaticAberration = this._profile.chromaticAberration.settings;
			this.enableGrain = this._profile.grain.enabled;
			this.grain = this._profile.grain.settings;
			this.enableVignette = this._profile.vignette.enabled;
			this.vignette = this._profile.vignette.settings;
		}

		private void Update()
		{
			if (this.controlAntialiasing)
			{
				if (this.enableAntialiasing != this._profile.antialiasing.enabled)
				{
					this._profile.antialiasing.enabled = this.enableAntialiasing;
				}
				if (this.enableAntialiasing)
				{
					this._profile.antialiasing.settings = this.antialiasing;
				}
			}
			if (this.controlAmbientOcclusion)
			{
				if (this.enableAmbientOcclusion != this._profile.ambientOcclusion.enabled)
				{
					this._profile.ambientOcclusion.enabled = this.enableAmbientOcclusion;
				}
				if (this.enableAmbientOcclusion)
				{
					this._profile.ambientOcclusion.settings = this.ambientOcclusion;
				}
			}
			if (this.controlScreenSpaceReflection)
			{
				if (this.enableScreenSpaceReflection != this._profile.screenSpaceReflection.enabled)
				{
					this._profile.screenSpaceReflection.enabled = this.enableScreenSpaceReflection;
				}
				if (this.enableScreenSpaceReflection)
				{
					this._profile.screenSpaceReflection.settings = this.screenSpaceReflection;
				}
			}
			if (this.controlDepthOfField)
			{
				if (this.enableDepthOfField != this._profile.depthOfField.enabled)
				{
					this._profile.depthOfField.enabled = this.enableDepthOfField;
				}
				if (this.enableDepthOfField)
				{
					this._profile.depthOfField.settings = this.depthOfField;
				}
			}
			if (this.controlMotionBlur)
			{
				if (this.enableMotionBlur != this._profile.motionBlur.enabled)
				{
					this._profile.motionBlur.enabled = this.enableMotionBlur;
				}
				if (this.enableMotionBlur)
				{
					this._profile.motionBlur.settings = this.motionBlur;
				}
			}
			if (this.controlEyeAdaptation)
			{
				if (this.enableEyeAdaptation != this._profile.eyeAdaptation.enabled)
				{
					this._profile.eyeAdaptation.enabled = this.enableEyeAdaptation;
				}
				if (this.enableEyeAdaptation)
				{
					this._profile.eyeAdaptation.settings = this.eyeAdaptation;
				}
			}
			if (this.controlBloom)
			{
				if (this.enableBloom != this._profile.bloom.enabled)
				{
					this._profile.bloom.enabled = this.enableBloom;
				}
				if (this.enableBloom)
				{
					this._profile.bloom.settings = this.bloom;
				}
			}
			if (this.controlColorGrading)
			{
				if (this.enableColorGrading != this._profile.colorGrading.enabled)
				{
					this._profile.colorGrading.enabled = this.enableColorGrading;
				}
				if (this.enableColorGrading)
				{
					this._profile.colorGrading.settings = this.colorGrading;
				}
			}
			if (this.controlUserLut)
			{
				if (this.enableUserLut != this._profile.userLut.enabled)
				{
					this._profile.userLut.enabled = this.enableUserLut;
				}
				if (this.enableUserLut)
				{
					this._profile.userLut.settings = this.userLut;
				}
			}
			if (this.controlChromaticAberration)
			{
				if (this.enableChromaticAberration != this._profile.chromaticAberration.enabled)
				{
					this._profile.chromaticAberration.enabled = this.enableChromaticAberration;
				}
				if (this.enableChromaticAberration)
				{
					this._profile.chromaticAberration.settings = this.chromaticAberration;
				}
			}
			if (this.controlGrain)
			{
				if (this.enableGrain != this._profile.grain.enabled)
				{
					this._profile.grain.enabled = this.enableGrain;
				}
				if (this.enableGrain)
				{
					this._profile.grain.settings = this.grain;
				}
			}
			if (this.controlVignette)
			{
				if (this.enableVignette != this._profile.vignette.enabled)
				{
					this._profile.vignette.enabled = this.enableVignette;
				}
				if (this.enableVignette)
				{
					this._profile.vignette.settings = this.vignette;
				}
			}
		}
	}
}
