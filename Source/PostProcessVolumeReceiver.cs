using System;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour)), RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(Rigidbody))]
public class PostProcessVolumeReceiver : MonoBehaviour
{
	private PostProcessingProfile _profileOriginal;

	private PostProcessingProfile _profileCopy;

	private AntialiasingModel.Settings antialiasingSettings;

	private AmbientOcclusionModel.Settings ambientOcclusionSettings;

	private ScreenSpaceReflectionModel.Settings screenSpaceReflectionSettings;

	private DepthOfFieldModel.Settings depthOfFieldSettings;

	private MotionBlurModel.Settings motionBlurSettings;

	private EyeAdaptationModel.Settings eyeAdaptationSettings;

	private BloomModel.Settings bloomSettings;

	private ColorGradingModel.Settings colorGradingSettings;

	private UserLutModel.Settings userLutSettings;

	private ChromaticAberrationModel.Settings chromaticAberrationSettings;

	private GrainModel.Settings grainSettings;

	private VignetteModel.Settings vignetteSettings;

	private DitheringModel.Settings ditheringSettings;

	private void Start()
	{
		this._profileOriginal = base.GetComponent<PostProcessingBehaviour>().profile;
		if (this._profileOriginal == null)
		{
			Debug.LogError("No Post ProcessingProfile added to camera!");
			return;
		}
		this._profileCopy = UnityEngine.Object.Instantiate<PostProcessingProfile>(this._profileOriginal);
		base.GetComponent<PostProcessingBehaviour>().profile = this._profileCopy;
		this.colorGradingSettings = this._profileCopy.colorGrading.settings;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.GetComponent<Rigidbody>().useGravity = false;
		base.GetComponent<Collider>().isTrigger = true;
	}

	public void SetValues(ref PostProcessVolume volume, float percentage)
	{
		if (volume.antialiasing.enabled)
		{
			this._profileCopy.antialiasing.enabled = true;
			this._profileCopy.antialiasing.settings = volume.antialiasing.settings;
			this.antialiasingSettings.method = volume.antialiasing.settings.method;
			if (volume.antialiasing.settings.method == AntialiasingModel.Method.Taa)
			{
				this.antialiasingSettings.taaSettings.jitterSpread = Mathf.Lerp(this._profileOriginal.antialiasing.settings.taaSettings.jitterSpread, volume.antialiasing.settings.taaSettings.jitterSpread, percentage);
				this.antialiasingSettings.taaSettings.stationaryBlending = Mathf.Lerp(this._profileOriginal.antialiasing.settings.taaSettings.stationaryBlending, volume.antialiasing.settings.taaSettings.stationaryBlending, percentage);
				this.antialiasingSettings.taaSettings.motionBlending = Mathf.Lerp(this._profileOriginal.antialiasing.settings.taaSettings.motionBlending, volume.antialiasing.settings.taaSettings.motionBlending, percentage);
				this.antialiasingSettings.taaSettings.sharpen = Mathf.Lerp(this._profileOriginal.antialiasing.settings.taaSettings.sharpen, volume.antialiasing.settings.taaSettings.sharpen, percentage);
				this._profileCopy.antialiasing.settings = this.antialiasingSettings;
			}
			else if (volume.antialiasing.settings.method == AntialiasingModel.Method.Fxaa)
			{
				this.antialiasingSettings.fxaaSettings.preset = volume.antialiasing.settings.fxaaSettings.preset;
				this._profileCopy.antialiasing.settings = this.antialiasingSettings;
			}
		}
		if (volume.ambientOcclusion.enabled)
		{
			this._profileCopy.ambientOcclusion.enabled = true;
			this._profileCopy.ambientOcclusion.settings = volume.ambientOcclusion.settings;
			this.ambientOcclusionSettings.intensity = Mathf.Lerp(this._profileOriginal.ambientOcclusion.settings.intensity, volume.ambientOcclusion.settings.intensity, percentage);
			this.ambientOcclusionSettings.radius = Mathf.Lerp(this._profileOriginal.ambientOcclusion.settings.radius, volume.ambientOcclusion.settings.radius, percentage);
			this.ambientOcclusionSettings.sampleCount = volume.ambientOcclusion.settings.sampleCount;
			this.ambientOcclusionSettings.downsampling = volume.ambientOcclusion.settings.downsampling;
			this.ambientOcclusionSettings.forceForwardCompatibility = volume.ambientOcclusion.settings.forceForwardCompatibility;
			this.ambientOcclusionSettings.highPrecision = volume.ambientOcclusion.settings.highPrecision;
			this.ambientOcclusionSettings.ambientOnly = volume.ambientOcclusion.settings.ambientOnly;
			this._profileCopy.ambientOcclusion.settings = this.ambientOcclusionSettings;
		}
		else
		{
			this._profileCopy.ambientOcclusion.enabled = false;
		}
		if (volume.screenSpaceReflection.enabled)
		{
			this._profileCopy.screenSpaceReflection.enabled = true;
			this._profileCopy.screenSpaceReflection.settings = volume.screenSpaceReflection.settings;
			this.screenSpaceReflectionSettings.reflection.blendType = volume.screenSpaceReflection.settings.reflection.blendType;
			this.screenSpaceReflectionSettings.reflection.reflectionQuality = volume.screenSpaceReflection.settings.reflection.reflectionQuality;
			this.screenSpaceReflectionSettings.reflection.maxDistance = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.reflection.maxDistance, volume.screenSpaceReflection.settings.reflection.maxDistance, percentage);
			this.screenSpaceReflectionSettings.reflection.iterationCount = (int)Mathf.Lerp((float)this._profileOriginal.screenSpaceReflection.settings.reflection.iterationCount, (float)volume.screenSpaceReflection.settings.reflection.iterationCount, percentage);
			this.screenSpaceReflectionSettings.reflection.stepSize = (int)Mathf.Lerp((float)this._profileOriginal.screenSpaceReflection.settings.reflection.stepSize, (float)volume.screenSpaceReflection.settings.reflection.stepSize, percentage);
			this.screenSpaceReflectionSettings.reflection.widthModifier = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.reflection.widthModifier, volume.screenSpaceReflection.settings.reflection.widthModifier, percentage);
			this.screenSpaceReflectionSettings.reflection.reflectionBlur = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.reflection.reflectionBlur, volume.screenSpaceReflection.settings.reflection.reflectionBlur, percentage);
			this.screenSpaceReflectionSettings.reflection.reflectBackfaces = volume.screenSpaceReflection.settings.reflection.reflectBackfaces;
			this.screenSpaceReflectionSettings.intensity.reflectionMultiplier = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.intensity.reflectionMultiplier, volume.screenSpaceReflection.settings.intensity.reflectionMultiplier, percentage);
			this.screenSpaceReflectionSettings.intensity.fadeDistance = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.intensity.fadeDistance, volume.screenSpaceReflection.settings.intensity.fadeDistance, percentage);
			this.screenSpaceReflectionSettings.intensity.fresnelFade = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.intensity.fresnelFade, volume.screenSpaceReflection.settings.intensity.fresnelFade, percentage);
			this.screenSpaceReflectionSettings.intensity.fresnelFadePower = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.intensity.fresnelFadePower, volume.screenSpaceReflection.settings.intensity.fresnelFadePower, percentage);
			this.screenSpaceReflectionSettings.screenEdgeMask.intensity = Mathf.Lerp(this._profileOriginal.screenSpaceReflection.settings.screenEdgeMask.intensity, volume.screenSpaceReflection.settings.screenEdgeMask.intensity, percentage);
			this._profileCopy.screenSpaceReflection.settings = this.screenSpaceReflectionSettings;
		}
		else
		{
			this._profileCopy.screenSpaceReflection.enabled = false;
		}
		if (volume.depthOfField.enabled)
		{
			this._profileCopy.depthOfField.enabled = true;
			this._profileCopy.depthOfField.settings = volume.depthOfField.settings;
			this.depthOfFieldSettings.focusDistance = Mathf.Lerp(this._profileOriginal.depthOfField.settings.focusDistance, volume.depthOfField.settings.focusDistance, percentage);
			this.depthOfFieldSettings.aperture = Mathf.Lerp(this._profileOriginal.depthOfField.settings.aperture, volume.depthOfField.settings.aperture, percentage);
			this.depthOfFieldSettings.useCameraFov = volume.depthOfField.settings.useCameraFov;
			this.depthOfFieldSettings.focalLength = Mathf.Lerp(this._profileOriginal.depthOfField.settings.focalLength, volume.depthOfField.settings.focalLength, percentage);
			this.depthOfFieldSettings.kernelSize = volume.depthOfField.settings.kernelSize;
			this._profileCopy.depthOfField.settings = this.depthOfFieldSettings;
		}
		else
		{
			this._profileCopy.depthOfField.enabled = false;
		}
		if (volume.motionBlur.enabled)
		{
			this._profileCopy.motionBlur.enabled = true;
			this._profileCopy.motionBlur.settings = volume.motionBlur.settings;
			this.motionBlurSettings.shutterAngle = Mathf.Lerp(this._profileOriginal.motionBlur.settings.shutterAngle, volume.motionBlur.settings.shutterAngle, percentage);
			this.motionBlurSettings.sampleCount = (int)Mathf.Lerp((float)this._profileOriginal.motionBlur.settings.sampleCount, (float)volume.motionBlur.settings.sampleCount, percentage);
			this.motionBlurSettings.frameBlending = Mathf.Lerp(this._profileOriginal.motionBlur.settings.frameBlending, volume.motionBlur.settings.frameBlending, percentage);
			this._profileCopy.motionBlur.settings = this.motionBlurSettings;
		}
		else
		{
			this._profileCopy.motionBlur.enabled = false;
		}
		if (volume.eyeAdaptation.enabled)
		{
			this._profileCopy.eyeAdaptation.enabled = true;
			this._profileCopy.eyeAdaptation.settings = volume.eyeAdaptation.settings;
			this.eyeAdaptationSettings.logMin = (int)Mathf.Lerp((float)this._profileOriginal.eyeAdaptation.settings.logMin, (float)volume.eyeAdaptation.settings.logMin, percentage);
			this.eyeAdaptationSettings.logMax = (int)Mathf.Lerp((float)this._profileOriginal.eyeAdaptation.settings.logMax, (float)volume.eyeAdaptation.settings.logMax, percentage);
			this.eyeAdaptationSettings.lowPercent = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.lowPercent, volume.eyeAdaptation.settings.lowPercent, percentage);
			this.eyeAdaptationSettings.highPercent = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.highPercent, volume.eyeAdaptation.settings.highPercent, percentage);
			this.eyeAdaptationSettings.minLuminance = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.minLuminance, volume.eyeAdaptation.settings.minLuminance, percentage);
			this.eyeAdaptationSettings.maxLuminance = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.maxLuminance, volume.eyeAdaptation.settings.maxLuminance, percentage);
			this.eyeAdaptationSettings.dynamicKeyValue = volume.eyeAdaptation.settings.dynamicKeyValue;
			this.eyeAdaptationSettings.keyValue = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.keyValue, volume.eyeAdaptation.settings.keyValue, percentage);
			this.eyeAdaptationSettings.adaptationType = volume.eyeAdaptation.settings.adaptationType;
			this.eyeAdaptationSettings.speedUp = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.speedUp, volume.eyeAdaptation.settings.speedUp, percentage);
			this.eyeAdaptationSettings.speedDown = Mathf.Lerp(this._profileOriginal.eyeAdaptation.settings.speedDown, volume.eyeAdaptation.settings.speedDown, percentage);
			this._profileCopy.eyeAdaptation.settings = this.eyeAdaptationSettings;
		}
		else
		{
			this._profileCopy.eyeAdaptation.enabled = false;
		}
		if (volume.bloom.enabled)
		{
			this._profileCopy.bloom.enabled = true;
			this._profileCopy.bloom.settings = volume.bloom.settings;
			this.bloomSettings.bloom.intensity = Mathf.Lerp(this._profileOriginal.bloom.settings.bloom.intensity, volume.bloom.settings.bloom.intensity, percentage);
			this.bloomSettings.bloom.threshold = Mathf.Lerp(this._profileOriginal.bloom.settings.bloom.threshold, volume.bloom.settings.bloom.threshold, percentage);
			this.bloomSettings.bloom.thresholdLinear = Mathf.Lerp(this._profileOriginal.bloom.settings.bloom.thresholdLinear, volume.bloom.settings.bloom.thresholdLinear, percentage);
			this.bloomSettings.bloom.softKnee = Mathf.Lerp(this._profileOriginal.bloom.settings.bloom.softKnee, volume.bloom.settings.bloom.softKnee, percentage);
			this.bloomSettings.bloom.radius = Mathf.Lerp(this._profileOriginal.bloom.settings.bloom.radius, volume.bloom.settings.bloom.radius, percentage);
			this.bloomSettings.bloom.antiFlicker = volume.bloom.settings.bloom.antiFlicker;
			this.bloomSettings.lensDirt.texture = this._profileOriginal.bloom.settings.lensDirt.texture;
			this.bloomSettings.lensDirt.intensity = Mathf.Lerp(this._profileOriginal.bloom.settings.lensDirt.intensity, volume.bloom.settings.lensDirt.intensity, percentage);
			this._profileCopy.bloom.settings = this.bloomSettings;
		}
		else
		{
			this._profileCopy.bloom.enabled = false;
		}
		if (volume.colorGrading.enabled)
		{
			this._profileCopy.colorGrading.enabled = true;
			this._profileCopy.colorGrading.settings = volume.colorGrading.settings;
			this.colorGradingSettings.tonemapping.tonemapper = volume.colorGrading.settings.tonemapping.tonemapper;
			this.colorGradingSettings.tonemapping.neutralBlackIn = Mathf.Lerp(this._profileOriginal.colorGrading.settings.tonemapping.neutralBlackIn, volume.colorGrading.settings.tonemapping.neutralBlackIn, percentage);
			this.colorGradingSettings.tonemapping.neutralWhiteIn = Mathf.Lerp(this._profileOriginal.colorGrading.settings.tonemapping.neutralWhiteIn, volume.colorGrading.settings.tonemapping.neutralWhiteIn, percentage);
			this.colorGradingSettings.tonemapping.neutralBlackOut = Mathf.Lerp(this._profileOriginal.colorGrading.settings.tonemapping.neutralBlackOut, volume.colorGrading.settings.tonemapping.neutralBlackOut, percentage);
			this.colorGradingSettings.tonemapping.neutralWhiteOut = Mathf.Lerp(this._profileOriginal.colorGrading.settings.tonemapping.neutralWhiteOut, volume.colorGrading.settings.tonemapping.neutralWhiteOut, percentage);
			this.colorGradingSettings.tonemapping.neutralWhiteLevel = Mathf.Lerp(this._profileOriginal.colorGrading.settings.tonemapping.neutralWhiteLevel, volume.colorGrading.settings.tonemapping.neutralWhiteLevel, percentage);
			this.colorGradingSettings.tonemapping.neutralWhiteClip = Mathf.Lerp(this._profileOriginal.colorGrading.settings.tonemapping.neutralWhiteClip, volume.colorGrading.settings.tonemapping.neutralWhiteClip, percentage);
			this.colorGradingSettings.basic.temperature = Mathf.Lerp(this._profileOriginal.colorGrading.settings.basic.temperature, volume.colorGrading.settings.basic.temperature, percentage);
			this.colorGradingSettings.basic.hueShift = Mathf.Lerp(this._profileOriginal.colorGrading.settings.basic.hueShift, volume.colorGrading.settings.basic.hueShift, percentage);
			this.colorGradingSettings.basic.contrast = Mathf.Lerp(this._profileOriginal.colorGrading.settings.basic.contrast, volume.colorGrading.settings.basic.contrast, percentage);
			this.colorGradingSettings.basic.postExposure = Mathf.Lerp(this._profileOriginal.colorGrading.settings.basic.postExposure, volume.colorGrading.settings.basic.postExposure, percentage);
			this.colorGradingSettings.basic.saturation = Mathf.Lerp(this._profileOriginal.colorGrading.settings.basic.saturation, volume.colorGrading.settings.basic.saturation, percentage);
			this.colorGradingSettings.basic.tint = Mathf.Lerp(this._profileOriginal.colorGrading.settings.basic.tint, volume.colorGrading.settings.basic.tint, percentage);
			this.colorGradingSettings.channelMixer.currentEditingChannel = volume.colorGrading.settings.channelMixer.currentEditingChannel;
			this.colorGradingSettings.channelMixer.red = Vector3.Lerp(this._profileOriginal.colorGrading.settings.channelMixer.red, volume.colorGrading.settings.channelMixer.red, percentage);
			this.colorGradingSettings.channelMixer.green = Vector3.Lerp(this._profileOriginal.colorGrading.settings.channelMixer.green, volume.colorGrading.settings.channelMixer.green, percentage);
			this.colorGradingSettings.channelMixer.blue = Vector3.Lerp(this._profileOriginal.colorGrading.settings.channelMixer.blue, volume.colorGrading.settings.channelMixer.blue, percentage);
			this.colorGradingSettings.colorWheels.mode = volume.colorGrading.settings.colorWheels.mode;
			if (this.colorGradingSettings.colorWheels.mode == ColorGradingModel.ColorWheelMode.Linear)
			{
				this.colorGradingSettings.colorWheels.linear.gain = Color.Lerp(this._profileOriginal.colorGrading.settings.colorWheels.linear.gain, volume.colorGrading.settings.colorWheels.linear.gain, percentage);
				this.colorGradingSettings.colorWheels.linear.gamma = Color.Lerp(this._profileOriginal.colorGrading.settings.colorWheels.linear.gamma, volume.colorGrading.settings.colorWheels.linear.gamma, percentage);
				this.colorGradingSettings.colorWheels.linear.lift = Color.Lerp(this._profileOriginal.colorGrading.settings.colorWheels.linear.lift, volume.colorGrading.settings.colorWheels.linear.lift, percentage);
			}
			else if (this.colorGradingSettings.colorWheels.mode == ColorGradingModel.ColorWheelMode.Log)
			{
				this.colorGradingSettings.colorWheels.log.slope = Color.Lerp(this._profileOriginal.colorGrading.settings.colorWheels.log.slope, volume.colorGrading.settings.colorWheels.log.slope, percentage);
				this.colorGradingSettings.colorWheels.log.power = Color.Lerp(this._profileOriginal.colorGrading.settings.colorWheels.log.power, volume.colorGrading.settings.colorWheels.log.power, percentage);
				this.colorGradingSettings.colorWheels.log.offset = Color.Lerp(this._profileOriginal.colorGrading.settings.colorWheels.log.offset, volume.colorGrading.settings.colorWheels.log.offset, percentage);
			}
			this.colorGradingSettings.curves = this._profileOriginal.colorGrading.settings.curves;
			this._profileCopy.colorGrading.settings = this.colorGradingSettings;
		}
		else
		{
			this._profileCopy.colorGrading.enabled = false;
		}
		if (volume.userLut.enabled)
		{
			this._profileCopy.userLut.enabled = true;
			this._profileCopy.userLut.settings = volume.userLut.settings;
			this.userLutSettings.lut = this._profileOriginal.userLut.settings.lut;
			this.userLutSettings.contribution = Mathf.Lerp(this._profileOriginal.userLut.settings.contribution, volume.userLut.settings.contribution, percentage);
			this._profileCopy.userLut.settings = this.userLutSettings;
		}
		else
		{
			this._profileCopy.userLut.enabled = false;
		}
		if (volume.chromaticAberration.enabled)
		{
			this._profileCopy.chromaticAberration.enabled = true;
			this._profileCopy.chromaticAberration.settings = volume.chromaticAberration.settings;
			this.chromaticAberrationSettings.spectralTexture = volume.chromaticAberration.settings.spectralTexture;
			this.chromaticAberrationSettings.intensity = Mathf.Lerp(this._profileOriginal.chromaticAberration.settings.intensity, volume.chromaticAberration.settings.intensity, percentage);
			this._profileCopy.chromaticAberration.settings = this.chromaticAberrationSettings;
		}
		else
		{
			this._profileCopy.chromaticAberration.enabled = false;
		}
		if (volume.grain.enabled)
		{
			this._profileCopy.grain.enabled = true;
			this._profileCopy.grain.settings = volume.grain.settings;
			this.grainSettings.intensity = Mathf.Lerp(this._profileOriginal.grain.settings.intensity, volume.grain.settings.intensity, percentage);
			this.grainSettings.luminanceContribution = Mathf.Lerp(this._profileOriginal.grain.settings.luminanceContribution, volume.grain.settings.luminanceContribution, percentage);
			this.grainSettings.size = Mathf.Lerp(this._profileOriginal.grain.settings.size, volume.grain.settings.size, percentage);
			this.grainSettings.colored = volume.grain.settings.colored;
			this._profileCopy.grain.settings = this.grainSettings;
		}
		else
		{
			this._profileCopy.grain.enabled = false;
		}
		if (volume.vignette.enabled)
		{
			this._profileCopy.vignette.enabled = true;
			this._profileCopy.vignette.settings = volume.vignette.settings;
			this.vignetteSettings.mode = volume.vignette.settings.mode;
			this.vignetteSettings.color = Color.Lerp(this._profileOriginal.vignette.settings.color, volume.vignette.settings.color, percentage);
			if (this.vignetteSettings.mode == VignetteModel.Mode.Masked)
			{
				this.vignetteSettings.opacity = Mathf.Lerp(this._profileOriginal.vignette.settings.opacity, volume.vignette.settings.opacity, percentage);
			}
			else if (this.vignetteSettings.mode == VignetteModel.Mode.Classic)
			{
				this.vignetteSettings.center = Vector3.Lerp(this._profileOriginal.vignette.settings.center, volume.vignette.settings.center, percentage);
				this.vignetteSettings.intensity = Mathf.Lerp(this._profileOriginal.vignette.settings.intensity, volume.vignette.settings.intensity, percentage);
				this.vignetteSettings.smoothness = Mathf.Lerp(this._profileOriginal.vignette.settings.smoothness, volume.vignette.settings.smoothness, percentage);
				this.vignetteSettings.roundness = Mathf.Lerp(this._profileOriginal.vignette.settings.roundness, volume.vignette.settings.roundness, percentage);
				this.vignetteSettings.rounded = volume.vignette.settings.rounded;
			}
			this._profileCopy.vignette.settings = this.vignetteSettings;
		}
		else
		{
			this._profileCopy.vignette.enabled = false;
		}
		if (volume.dithering.enabled)
		{
			this._profileCopy.dithering.enabled = true;
		}
		else
		{
			this._profileCopy.dithering.enabled = false;
		}
	}

	public void ResetValues()
	{
		if (this._profileOriginal.antialiasing.enabled)
		{
			this._profileCopy.antialiasing.enabled = true;
			this._profileCopy.antialiasing.settings = this._profileOriginal.antialiasing.settings;
		}
		else
		{
			this._profileCopy.antialiasing.enabled = false;
		}
		if (this._profileOriginal.ambientOcclusion.enabled)
		{
			this._profileCopy.ambientOcclusion.enabled = true;
			this._profileCopy.ambientOcclusion.settings = this._profileOriginal.ambientOcclusion.settings;
		}
		else
		{
			this._profileCopy.ambientOcclusion.enabled = false;
		}
		if (this._profileOriginal.screenSpaceReflection.enabled)
		{
			this._profileCopy.screenSpaceReflection.enabled = true;
			this._profileCopy.screenSpaceReflection.settings = this._profileOriginal.screenSpaceReflection.settings;
		}
		else
		{
			this._profileCopy.screenSpaceReflection.enabled = false;
		}
		if (this._profileOriginal.depthOfField.enabled)
		{
			this._profileCopy.depthOfField.enabled = true;
			this._profileCopy.depthOfField.settings = this._profileOriginal.depthOfField.settings;
		}
		else
		{
			this._profileCopy.depthOfField.enabled = false;
		}
		if (this._profileOriginal.motionBlur.enabled)
		{
			this._profileCopy.motionBlur.enabled = true;
			this._profileCopy.motionBlur.settings = this._profileOriginal.motionBlur.settings;
		}
		else
		{
			this._profileCopy.motionBlur.enabled = false;
		}
		if (this._profileOriginal.eyeAdaptation.enabled)
		{
			this._profileCopy.eyeAdaptation.enabled = true;
			this._profileCopy.eyeAdaptation.settings = this._profileOriginal.eyeAdaptation.settings;
		}
		else
		{
			this._profileCopy.eyeAdaptation.enabled = false;
		}
		if (this._profileOriginal.bloom.enabled)
		{
			this._profileCopy.bloom.enabled = true;
			this._profileCopy.bloom.settings = this._profileOriginal.bloom.settings;
		}
		else
		{
			this._profileCopy.bloom.enabled = false;
		}
		if (this._profileOriginal.colorGrading.enabled)
		{
			this._profileCopy.colorGrading.enabled = true;
			this._profileCopy.colorGrading.settings = this._profileOriginal.colorGrading.settings;
		}
		else
		{
			this._profileCopy.colorGrading.enabled = false;
		}
		if (this._profileOriginal.userLut.enabled)
		{
			this._profileCopy.userLut.enabled = true;
			this._profileCopy.userLut.settings = this._profileOriginal.userLut.settings;
		}
		else
		{
			this._profileCopy.userLut.enabled = false;
		}
		if (this._profileOriginal.chromaticAberration.enabled)
		{
			this._profileCopy.chromaticAberration.enabled = true;
			this._profileCopy.chromaticAberration.settings = this._profileOriginal.chromaticAberration.settings;
		}
		else
		{
			this._profileCopy.chromaticAberration.enabled = false;
		}
		if (this._profileOriginal.grain.enabled)
		{
			this._profileCopy.grain.enabled = true;
			this._profileCopy.grain.settings = this._profileOriginal.grain.settings;
		}
		else
		{
			this._profileCopy.grain.enabled = false;
		}
		if (this._profileOriginal.vignette.enabled)
		{
			this._profileCopy.vignette.enabled = true;
			this._profileCopy.vignette.settings = this._profileOriginal.vignette.settings;
		}
		else
		{
			this._profileCopy.vignette.enabled = false;
		}
		if (this._profileOriginal.dithering.enabled)
		{
			this._profileCopy.dithering.enabled = true;
			this._profileCopy.dithering.settings = this._profileOriginal.dithering.settings;
		}
		else
		{
			this._profileCopy.dithering.enabled = false;
		}
	}
}
