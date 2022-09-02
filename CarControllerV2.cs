using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarControllerV2 : MonoBehaviour
{
	public enum WheelType
	{
		FWD,
		RWD
	}

	public bool useAccelerometerForSteer;

	public bool steeringWheelControl;

	private Vector3 defBrakePedalPosition;

	public bool demoGUI;

	public bool dashBoard;

	private bool andHandBrake;

	public float gyroTiltMultiplier = 2f;

	public WheelCollider Wheel_FL;

	public WheelCollider Wheel_FR;

	public WheelCollider Wheel_RL;

	public WheelCollider Wheel_RR;

	public Transform FrontLeftWheelT;

	public Transform FrontRightWheelT;

	public Transform RearLeftWheelT;

	public Transform RearRightWheelT;

	public WheelCollider[] ExtraRearWheels;

	public Transform[] ExtraRearWheelsT;

	public Transform SteeringWheel;

	public CarControllerV2.WheelType _wheelTypeChoise;

	private bool rwd;

	private bool fwd = true;

	public Transform COM;

	public int steeringAssistanceDivider = 5;

	private float driftAngle;

	private float stabilizerAssistance = 500f;

	public bool canControl = true;

	public bool canBurnout = true;

	public bool driftMode;

	public float gearShiftRate = 10f;

	public int CurrentGear;

	public AnimationCurve EngineTorqueCurve;

	private float[] GearRatio;

	public float EngineTorque = 600f;

	public float MaxEngineRPM = 6000f;

	public float MinEngineRPM = 1000f;

	public float SteerAngle = 20f;

	private float LowestSpeedSteerAngleAtSpeed = 40f;

	public float HighSpeedSteerAngle = 10f;

	public float HighSpeedSteerAngleAtSpeed = 80f;

	[HideInInspector]
	public float Speed;

	public float Brake = 200f;

	public float handbrakeStiffness = 0.1f;

	public float maxSpeed = 180f;

	public bool useDifferantial = true;

	private float differantialRatioRight;

	private float differantialRatioLeft;

	private float differantialDifference;

	private float RotationValueFL;

	private float RotationValueFR;

	private float RotationValueRL;

	private float RotationValueRR;

	private float[] RotationValueExtra;

	private float defSteerAngle;

	private float StiffnessRear;

	private float StiffnessFront;

	private bool reversing;

	private bool centerSteer;

	private bool headLightsOn;

	private float acceleration;

	private float lastVelocity;

	private float gearTimeMultiplier;

	public GameObject skidAudio;

	public AudioClip skidClip;

	public GameObject crashAudio;

	public AudioClip[] crashClips;

	public GameObject engineAudio;

	public AudioClip engineClip;

	private int collisionForceLimit = 5;

	private float EngineRPM;

	[HideInInspector]
	public float motorInput;

	[HideInInspector]
	public float steerInput;

	public Texture2D speedOMeter;

	public Texture2D speedOMeterNeedle;

	public Texture2D kiloMeter;

	public Texture2D kiloMeterNeedle;

	private float needleRotation;

	private float kMHneedleRotation;

	private float smoothedNeedleRotation;

	public Font dashBoardFont;

	public float guiWidth;

	public float guiHeight;

	private WheelFrictionCurve RearLeftFriction;

	private WheelFrictionCurve RearRightFriction;

	private WheelFrictionCurve FrontLeftFriction;

	private WheelFrictionCurve FrontRightFriction;

	public GameObject chassis;

	public float chassisVerticalLean = 3f;

	public float chassisHorizontalLean = 3f;

	private float horizontalLean;

	private float verticalLean;

	public Light[] HeadLights;

	public Light[] BrakeLights;

	public Light[] ReverseLights;

	public float steeringWheelMaximumSteerAngle = 180f;

	public float steeringWheelGuiScale = 256f;

	public float steeringWheelXOffset = 30f;

	public float steeringWheelYOffset = 30f;

	public Vector2 steeringWheelPivotPos = Vector2.zero;

	public float steeringWheelResetPosSpeed = 200f;

	public Texture2D steeringWheelTexture;

	private float steeringWheelsteerAngle;

	private bool steeringWheelIsTouching;

	private Rect steeringWheelTextureRect;

	private Vector2 steeringWheelWheelCenter;

	private float steeringWheelOldAngle;

	private int touchId = -1;

	private Vector2 touchPos;

	private void Start()
	{
		this.SetWheelFrictions();
		this.SoundsInitialize();
		this.WheelTypeInit();
		this.GearInit();
		this.SteeringWheelInit();
		Time.fixedDeltaTime = 0.03f;
		base.GetComponent<Rigidbody>().centerOfMass = new Vector3(this.COM.localPosition.x * base.transform.localScale.x, this.COM.localPosition.y * base.transform.localScale.y, this.COM.localPosition.z * base.transform.localScale.z);
		base.GetComponent<Rigidbody>().maxAngularVelocity = 5f;
	}

	private void SetWheelFrictions()
	{
		this.RearLeftFriction = this.Wheel_RL.sidewaysFriction;
		this.RearRightFriction = this.Wheel_RR.sidewaysFriction;
		this.FrontLeftFriction = this.Wheel_FL.sidewaysFriction;
		this.FrontRightFriction = this.Wheel_FR.sidewaysFriction;
		this.RotationValueExtra = new float[this.ExtraRearWheels.Length];
		this.StiffnessRear = this.Wheel_RL.sidewaysFriction.stiffness;
		this.StiffnessFront = this.Wheel_FL.sidewaysFriction.stiffness;
		this.defSteerAngle = this.SteerAngle;
	}

	private void SoundsInitialize()
	{
		this.engineAudio = new GameObject("EngineSound");
		this.engineAudio.transform.position = base.transform.position;
		this.engineAudio.transform.rotation = base.transform.rotation;
		this.engineAudio.transform.parent = base.transform;
		this.engineAudio.AddComponent<AudioSource>();
		this.engineAudio.GetComponent<AudioSource>().minDistance = 5f;
		this.engineAudio.GetComponent<AudioSource>().volume = 0f;
		this.engineAudio.GetComponent<AudioSource>().clip = this.engineClip;
		this.engineAudio.GetComponent<AudioSource>().loop = true;
		this.engineAudio.GetComponent<AudioSource>().Play();
		this.skidAudio = new GameObject("SkidSound");
		this.skidAudio.transform.position = base.transform.position;
		this.skidAudio.transform.rotation = base.transform.rotation;
		this.skidAudio.transform.parent = base.transform;
		this.skidAudio.AddComponent<AudioSource>();
		this.skidAudio.GetComponent<AudioSource>().minDistance = 10f;
		this.skidAudio.GetComponent<AudioSource>().volume = 0f;
		this.skidAudio.GetComponent<AudioSource>().clip = this.skidClip;
		this.skidAudio.GetComponent<AudioSource>().loop = true;
		this.skidAudio.GetComponent<AudioSource>().Play();
		this.crashAudio = new GameObject("CrashSound");
		this.crashAudio.transform.position = base.transform.position;
		this.crashAudio.transform.rotation = base.transform.rotation;
		this.crashAudio.transform.parent = base.transform;
		this.crashAudio.AddComponent<AudioSource>();
		this.crashAudio.GetComponent<AudioSource>().minDistance = 10f;
	}

	private void WheelTypeInit()
	{
		CarControllerV2.WheelType wheelTypeChoise = this._wheelTypeChoise;
		if (wheelTypeChoise != CarControllerV2.WheelType.FWD)
		{
			if (wheelTypeChoise == CarControllerV2.WheelType.RWD)
			{
				this.fwd = false;
				this.rwd = true;
			}
		}
		else
		{
			this.fwd = true;
			this.rwd = false;
		}
	}

	private void GearInit()
	{
		this.GearRatio = new float[this.EngineTorqueCurve.length];
		for (int i = 0; i < this.EngineTorqueCurve.length; i++)
		{
			this.GearRatio[i] = this.EngineTorqueCurve.keys[i].value;
		}
	}

	private void Differantial()
	{
		if (this.useDifferantial)
		{
			this.differantialDifference = Mathf.Clamp(Mathf.Abs(this.Wheel_RR.rpm) - Mathf.Abs(this.Wheel_RL.rpm), -100f, 100f);
			this.differantialRatioRight = Mathf.Lerp(0f, 1f, (this.Wheel_RR.rpm + this.Wheel_RL.rpm + 5f + this.differantialDifference) / (this.Wheel_RR.rpm + this.Wheel_RL.rpm));
			this.differantialRatioLeft = Mathf.Lerp(0f, 1f, (this.Wheel_RR.rpm + this.Wheel_RL.rpm + 5f - this.differantialDifference) / (this.Wheel_RR.rpm + this.Wheel_RL.rpm));
		}
		else
		{
			this.differantialRatioRight = 1f;
			this.differantialRatioLeft = 1f;
		}
	}

	private void SteeringWheelInit()
	{
		this.steeringWheelGuiScale = (float)Screen.width * 1f / 2.7f;
		this.steeringWheelIsTouching = false;
		this.steeringWheelTextureRect = new Rect(this.steeringWheelXOffset + this.steeringWheelGuiScale / (float)Screen.width, -this.steeringWheelYOffset + ((float)Screen.height - this.steeringWheelGuiScale), this.steeringWheelGuiScale, this.steeringWheelGuiScale);
		this.steeringWheelWheelCenter = new Vector2(this.steeringWheelTextureRect.x + this.steeringWheelTextureRect.width * 0.5f, (float)Screen.height - this.steeringWheelTextureRect.y - this.steeringWheelTextureRect.height * 0.5f);
		this.steeringWheelsteerAngle = 0f;
	}

	private void Update()
	{
		this.WheelAlign();
		if (this.canControl)
		{
			this.Lights();
			if (this.chassis)
			{
				this.Chassis();
			}
		}
	}

	private void FixedUpdate()
	{
		this.ShiftGears();
		this.SkidAudio();
		this.Braking();
		this.Differantial();
		if (this.canControl)
		{
			this.Engine();
			this.KeyboardControlling();
		}
	}

	private void Engine()
	{
		if (this.EngineTorqueCurve.keys.Length >= 2)
		{
			if (this.CurrentGear == this.EngineTorqueCurve.length - 2)
			{
				this.gearTimeMultiplier = -this.EngineTorqueCurve[this.CurrentGear].time / this.gearShiftRate / (this.maxSpeed * 3f) + 1f;
			}
			else
			{
				this.gearTimeMultiplier = -this.EngineTorqueCurve[this.CurrentGear].time / (this.maxSpeed * 3f) + 1f;
			}
		}
		else
		{
			this.gearTimeMultiplier = 1f;
			Debug.Log("You DID NOT CREATE any engine torque curve keys!, Please create 1 key at least...");
		}
		this.Speed = base.GetComponent<Rigidbody>().velocity.magnitude * 3f;
		base.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * (this.motorInput * this.steerInput * (this.stabilizerAssistance * 10f)));
		this.acceleration = 0f;
		this.acceleration = (base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z - this.lastVelocity) / Time.fixedDeltaTime;
		this.lastVelocity = base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity).z;
		if (this.Speed < 100f)
		{
			base.GetComponent<Rigidbody>().drag = Mathf.Clamp(this.acceleration / 30f, 0f, 1f);
		}
		else
		{
			base.GetComponent<Rigidbody>().drag = 0.04f;
		}
		if (this.Speed > this.HighSpeedSteerAngleAtSpeed)
		{
			this.SteerAngle = Mathf.Lerp(this.SteerAngle, this.HighSpeedSteerAngle, Time.deltaTime * 5f);
		}
		else if (this.Speed < this.LowestSpeedSteerAngleAtSpeed)
		{
			this.SteerAngle = Mathf.Lerp(this.SteerAngle, this.defSteerAngle * 1.5f, Time.deltaTime * 5f);
		}
		else
		{
			this.SteerAngle = Mathf.Lerp(this.SteerAngle, this.defSteerAngle, Time.deltaTime * 5f);
		}
		if (this.EngineTorqueCurve.keys.Length >= 2)
		{
			this.EngineRPM = Mathf.Abs(this.Wheel_FR.rpm * this.gearShiftRate * Mathf.Clamp01(this.motorInput) + this.Wheel_FL.rpm * this.gearShiftRate * this.motorInput) / 2f * this.GearRatio[this.CurrentGear] * this.gearTimeMultiplier + this.MinEngineRPM;
		}
		else
		{
			this.EngineRPM = Mathf.Abs(this.Wheel_FR.rpm * this.gearShiftRate * Mathf.Clamp01(this.motorInput) + this.Wheel_FL.rpm * this.gearShiftRate * this.motorInput) / 2f * this.gearTimeMultiplier + this.MinEngineRPM;
		}
		if (this.motorInput < 0f && this.Wheel_FL.rpm < 50f)
		{
			this.reversing = true;
		}
		else
		{
			this.reversing = false;
		}
		this.engineAudio.GetComponent<AudioSource>().volume = Mathf.Lerp(this.engineAudio.GetComponent<AudioSource>().volume, Mathf.Clamp(this.motorInput, 0.25f, 1f), Time.deltaTime * 5f);
		if (this.Speed < 40f && !this.reversing && this.canBurnout)
		{
			this.engineAudio.GetComponent<AudioSource>().pitch = Mathf.Lerp(this.engineAudio.GetComponent<AudioSource>().pitch, Mathf.Clamp(this.motorInput * 2f, 1f, 2f), Time.deltaTime * 5f);
			this.skidAudio.GetComponent<AudioSource>().volume = Mathf.Lerp(this.skidAudio.GetComponent<AudioSource>().volume, Mathf.Clamp(this.motorInput, 0f, 1f), Time.deltaTime * 5f);
		}
		else if (this.Speed > 5f)
		{
			this.engineAudio.GetComponent<AudioSource>().pitch = Mathf.Lerp(this.engineAudio.GetComponent<AudioSource>().pitch, Mathf.Lerp(1f, 2f, (this.EngineRPM - this.MinEngineRPM / 1.5f) / (this.MaxEngineRPM + this.MinEngineRPM)), Time.deltaTime * 5f);
		}
		else
		{
			this.engineAudio.GetComponent<AudioSource>().pitch = Mathf.Lerp(this.engineAudio.GetComponent<AudioSource>().pitch, Mathf.Clamp(this.motorInput * 2f, 1f, 2f), Time.deltaTime * 5f);
		}
		if (this.rwd)
		{
			if (this.Speed > this.maxSpeed)
			{
				this.Wheel_RL.motorTorque = 0f;
				this.Wheel_RR.motorTorque = 0f;
			}
			else if (!this.reversing)
			{
				this.Wheel_RL.motorTorque = this.EngineTorque * Mathf.Clamp(this.motorInput * this.differantialRatioLeft, 0f, 1f) * this.EngineTorqueCurve.Evaluate(this.Speed);
				this.Wheel_RR.motorTorque = this.EngineTorque * Mathf.Clamp(this.motorInput * this.differantialRatioRight, 0f, 1f) * this.EngineTorqueCurve.Evaluate(this.Speed);
			}
			if (this.reversing)
			{
				if (this.Speed < 30f)
				{
					this.Wheel_RL.motorTorque = this.EngineTorque * this.motorInput / 3f;
					this.Wheel_RR.motorTorque = this.EngineTorque * this.motorInput / 3f;
				}
				else
				{
					this.Wheel_RL.motorTorque = 0f;
					this.Wheel_RR.motorTorque = 0f;
				}
			}
		}
		if (this.fwd)
		{
			if (this.Speed > this.maxSpeed)
			{
				this.Wheel_FL.motorTorque = 0f;
				this.Wheel_FR.motorTorque = 0f;
			}
			else if (!this.reversing)
			{
				this.Wheel_FL.motorTorque = this.EngineTorque * Mathf.Clamp(this.motorInput * this.differantialRatioLeft, 0f, 1f) * this.EngineTorqueCurve.Evaluate(this.Speed);
				this.Wheel_FR.motorTorque = this.EngineTorque * Mathf.Clamp(this.motorInput * this.differantialRatioRight, 0f, 1f) * this.EngineTorqueCurve.Evaluate(this.Speed);
			}
			if (this.reversing)
			{
				if (this.Speed < 30f)
				{
					this.Wheel_FL.motorTorque = this.EngineTorque * this.motorInput / 3f;
					this.Wheel_FR.motorTorque = this.EngineTorque * this.motorInput / 3f;
				}
				else
				{
					this.Wheel_FL.motorTorque = 0f;
					this.Wheel_FR.motorTorque = 0f;
				}
			}
		}
	}

	private void MobileSteeringInputs()
	{
		if (this.useAccelerometerForSteer)
		{
			this.steerInput = Input.acceleration.x * this.gyroTiltMultiplier;
			if (!this.driftMode)
			{
				this.Wheel_FL.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle);
				this.Wheel_FR.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle);
			}
			else
			{
				this.Wheel_FL.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle) + this.driftAngle / (float)this.steeringAssistanceDivider;
				this.Wheel_FR.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle) + this.driftAngle / (float)this.steeringAssistanceDivider;
			}
		}
		else if (!this.steeringWheelControl)
		{
			if (!this.driftMode)
			{
				this.Wheel_FL.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle);
				this.Wheel_FR.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle);
			}
			else
			{
				this.Wheel_FL.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle) + this.driftAngle / (float)this.steeringAssistanceDivider;
				this.Wheel_FR.steerAngle = Mathf.Clamp(this.SteerAngle * this.steerInput, -this.SteerAngle, this.SteerAngle) + this.driftAngle / (float)this.steeringAssistanceDivider;
			}
		}
		else if (!this.driftMode)
		{
			this.Wheel_FL.steerAngle = this.SteerAngle * (-this.steeringWheelsteerAngle / this.steeringWheelMaximumSteerAngle);
			this.Wheel_FR.steerAngle = this.SteerAngle * (-this.steeringWheelsteerAngle / this.steeringWheelMaximumSteerAngle);
		}
		else
		{
			this.Wheel_FL.steerAngle = this.SteerAngle * (-this.steeringWheelsteerAngle / this.steeringWheelMaximumSteerAngle) + this.driftAngle / (float)this.steeringAssistanceDivider;
			this.Wheel_FR.steerAngle = this.SteerAngle * (-this.steeringWheelsteerAngle / this.steeringWheelMaximumSteerAngle) + this.driftAngle / (float)this.steeringAssistanceDivider;
		}
	}

	private void SteeringWheelControlling()
	{
		if (this.steeringWheelIsTouching)
		{
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.fingerId == this.touchId)
				{
					this.touchPos = touch.position;
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						this.steeringWheelIsTouching = false;
					}
				}
			}
			float num = Vector2.Angle(Vector2.up, this.touchPos - this.steeringWheelWheelCenter);
			if (Vector2.Distance(this.touchPos, this.steeringWheelWheelCenter) > 20f)
			{
				if (this.touchPos.x > this.steeringWheelWheelCenter.x)
				{
					this.steeringWheelsteerAngle -= num - this.steeringWheelOldAngle;
				}
				else
				{
					this.steeringWheelsteerAngle += num - this.steeringWheelOldAngle;
				}
			}
			if (this.steeringWheelsteerAngle > this.steeringWheelMaximumSteerAngle)
			{
				this.steeringWheelsteerAngle = this.steeringWheelMaximumSteerAngle;
			}
			else if (this.steeringWheelsteerAngle < -this.steeringWheelMaximumSteerAngle)
			{
				this.steeringWheelsteerAngle = -this.steeringWheelMaximumSteerAngle;
			}
			this.steeringWheelOldAngle = num;
		}
		else
		{
			Touch[] touches2 = Input.touches;
			for (int j = 0; j < touches2.Length; j++)
			{
				Touch touch2 = touches2[j];
				if (touch2.phase == TouchPhase.Began && this.steeringWheelTextureRect.Contains(new Vector2(touch2.position.x, (float)Screen.height - touch2.position.y)))
				{
					this.steeringWheelIsTouching = true;
					this.steeringWheelOldAngle = Vector2.Angle(Vector2.up, touch2.position - this.steeringWheelWheelCenter);
					this.touchId = touch2.fingerId;
				}
			}
			if (!Mathf.Approximately(0f, this.steeringWheelsteerAngle))
			{
				float num2 = this.steeringWheelResetPosSpeed * Time.deltaTime;
				if (Mathf.Abs(num2) > Mathf.Abs(this.steeringWheelsteerAngle))
				{
					this.steeringWheelsteerAngle = 0f;
					return;
				}
				if (this.steeringWheelsteerAngle > 0f)
				{
					this.steeringWheelsteerAngle -= num2;
				}
				else
				{
					this.steeringWheelsteerAngle += num2;
				}
			}
		}
	}

	private void KeyboardControlling()
	{
		this.motorInput = Input.GetAxis("Vertical");
		this.steerInput = Input.GetAxis("Horizontal");
		this.Wheel_FL.steerAngle = this.SteerAngle * this.steerInput + this.driftAngle / (float)this.steeringAssistanceDivider;
		this.Wheel_FR.steerAngle = this.SteerAngle * this.steerInput + this.driftAngle / (float)this.steeringAssistanceDivider;
	}

	private void ShiftGears()
	{
		for (int i = 0; i < this.EngineTorqueCurve.length; i++)
		{
			if (this.EngineTorqueCurve.Evaluate(this.Speed) < this.EngineTorqueCurve.keys[i].value)
			{
				this.CurrentGear = i;
			}
		}
	}

	private void WheelAlign()
	{
		Vector3 vector = this.Wheel_FL.transform.TransformPoint(this.Wheel_FL.center);
		WheelHit wheelHit;
		this.Wheel_FL.GetGroundHit(out wheelHit);
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, -this.Wheel_FL.transform.up, out raycastHit, (this.Wheel_FL.suspensionDistance + this.Wheel_FL.radius) * base.transform.localScale.y))
		{
			if (raycastHit.transform.gameObject.layer != LayerMask.NameToLayer("CarCollider"))
			{
				this.FrontLeftWheelT.transform.position = raycastHit.point + this.Wheel_FL.transform.up * this.Wheel_FL.radius * base.transform.localScale.y;
				float num = (-this.Wheel_FL.transform.InverseTransformPoint(wheelHit.point).y - this.Wheel_FL.radius) / this.Wheel_FL.suspensionDistance;
				Debug.DrawLine(wheelHit.point, wheelHit.point + this.Wheel_FL.transform.up * (wheelHit.force / 8000f), ((double)num > 0.0) ? Color.white : Color.magenta);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_FL.transform.forward * wheelHit.forwardSlip, Color.green);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_FL.transform.right * wheelHit.sidewaysSlip, Color.red);
			}
		}
		else
		{
			this.FrontLeftWheelT.transform.position = vector - this.Wheel_FL.transform.up * this.Wheel_FL.suspensionDistance * base.transform.localScale.y;
		}
		if (this.fwd && this.Speed < 20f && this.motorInput > 0f && this.canBurnout)
		{
			this.RotationValueFL += this.Wheel_FL.rpm * 12f * Time.deltaTime;
		}
		else
		{
			this.RotationValueFL += this.Wheel_FL.rpm * 6f * Time.deltaTime;
		}
		this.FrontLeftWheelT.transform.rotation = this.Wheel_FL.transform.rotation * Quaternion.Euler(this.RotationValueFL, this.Wheel_FL.steerAngle + this.driftAngle / (float)this.steeringAssistanceDivider, this.Wheel_FL.transform.rotation.z);
		Vector3 vector2 = this.Wheel_FR.transform.TransformPoint(this.Wheel_FR.center);
		this.Wheel_FR.GetGroundHit(out wheelHit);
		if (Physics.Raycast(vector2, -this.Wheel_FR.transform.up, out raycastHit, (this.Wheel_FR.suspensionDistance + this.Wheel_FR.radius) * base.transform.localScale.y))
		{
			if (raycastHit.transform.gameObject.layer != LayerMask.NameToLayer("CarCollider"))
			{
				this.FrontRightWheelT.transform.position = raycastHit.point + this.Wheel_FR.transform.up * this.Wheel_FR.radius * base.transform.localScale.y;
				float num2 = (-this.Wheel_FR.transform.InverseTransformPoint(wheelHit.point).y - this.Wheel_FR.radius) / this.Wheel_FR.suspensionDistance;
				Debug.DrawLine(wheelHit.point, wheelHit.point + this.Wheel_FR.transform.up * (wheelHit.force / 8000f), ((double)num2 > 0.0) ? Color.white : Color.magenta);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_FR.transform.forward * wheelHit.forwardSlip, Color.green);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_FR.transform.right * wheelHit.sidewaysSlip, Color.red);
			}
		}
		else
		{
			this.FrontRightWheelT.transform.position = vector2 - this.Wheel_FR.transform.up * this.Wheel_FR.suspensionDistance * base.transform.localScale.y;
		}
		if (this.fwd && this.Speed < 20f && this.motorInput > 0f && this.canBurnout)
		{
			this.RotationValueFR += this.Wheel_FR.rpm * 12f * Time.deltaTime;
		}
		else
		{
			this.RotationValueFR += this.Wheel_FR.rpm * 6f * Time.deltaTime;
		}
		this.FrontRightWheelT.transform.rotation = this.Wheel_FR.transform.rotation * Quaternion.Euler(this.RotationValueFR, this.Wheel_FR.steerAngle + this.driftAngle / (float)this.steeringAssistanceDivider, this.Wheel_FR.transform.rotation.z);
		Vector3 vector3 = this.Wheel_RL.transform.TransformPoint(this.Wheel_RL.center);
		this.Wheel_RL.GetGroundHit(out wheelHit);
		if (Physics.Raycast(vector3, -this.Wheel_RL.transform.up, out raycastHit, (this.Wheel_RL.suspensionDistance + this.Wheel_RL.radius) * base.transform.localScale.y))
		{
			if (raycastHit.transform.gameObject.layer != LayerMask.NameToLayer("CarCollider"))
			{
				this.RearLeftWheelT.transform.position = raycastHit.point + this.Wheel_RL.transform.up * this.Wheel_RL.radius * base.transform.localScale.y;
				float num3 = (-this.Wheel_RL.transform.InverseTransformPoint(wheelHit.point).y - this.Wheel_RL.radius) / this.Wheel_RL.suspensionDistance;
				Debug.DrawLine(wheelHit.point, wheelHit.point + this.Wheel_RL.transform.up * (wheelHit.force / 8000f), ((double)num3 > 0.0) ? Color.white : Color.magenta);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_RL.transform.forward * wheelHit.forwardSlip, Color.green);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_RL.transform.right * wheelHit.sidewaysSlip, Color.red);
			}
		}
		else
		{
			this.RearLeftWheelT.transform.position = vector3 - this.Wheel_RL.transform.up * this.Wheel_RL.suspensionDistance * base.transform.localScale.y;
		}
		this.RearLeftWheelT.transform.rotation = this.Wheel_RL.transform.rotation * Quaternion.Euler(this.RotationValueRL, 0f, this.Wheel_RL.transform.rotation.z);
		if (this.rwd && this.Speed < 20f && this.motorInput > 0f && this.canBurnout)
		{
			this.RotationValueRL += this.Wheel_RL.rpm * 12f * Time.deltaTime;
		}
		else
		{
			this.RotationValueRL += this.Wheel_RL.rpm * 6f * Time.deltaTime;
		}
		Vector3 vector4 = this.Wheel_RR.transform.TransformPoint(this.Wheel_RR.center);
		this.Wheel_RR.GetGroundHit(out wheelHit);
		if (Physics.Raycast(vector4, -this.Wheel_RR.transform.up, out raycastHit, (this.Wheel_RR.suspensionDistance + this.Wheel_RR.radius) * base.transform.localScale.y))
		{
			if (raycastHit.transform.gameObject.layer != LayerMask.NameToLayer("CarCollider"))
			{
				this.RearRightWheelT.transform.position = raycastHit.point + this.Wheel_RR.transform.up * this.Wheel_RR.radius * base.transform.localScale.y;
				float num4 = (-this.Wheel_RR.transform.InverseTransformPoint(wheelHit.point).y - this.Wheel_RR.radius) / this.Wheel_RR.suspensionDistance;
				Debug.DrawLine(wheelHit.point, wheelHit.point + this.Wheel_RR.transform.up * (wheelHit.force / 8000f), ((double)num4 > 0.0) ? Color.white : Color.magenta);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_RR.transform.forward * wheelHit.forwardSlip, Color.green);
				Debug.DrawLine(wheelHit.point, wheelHit.point - this.Wheel_RR.transform.right * wheelHit.sidewaysSlip, Color.red);
			}
		}
		else
		{
			this.RearRightWheelT.transform.position = vector4 - this.Wheel_RR.transform.up * this.Wheel_RR.suspensionDistance * base.transform.localScale.y;
		}
		this.RearRightWheelT.transform.rotation = this.Wheel_RR.transform.rotation * Quaternion.Euler(this.RotationValueRR, 0f, this.Wheel_RR.transform.rotation.z);
		if (this.rwd && this.Speed < 20f && this.motorInput > 0f && this.canBurnout)
		{
			this.RotationValueRR += this.Wheel_RR.rpm * 12f * Time.deltaTime;
		}
		else
		{
			this.RotationValueRR += this.Wheel_RR.rpm * 6f * Time.deltaTime;
		}
		if (this.ExtraRearWheels.Length > 0)
		{
			for (int i = 0; i < this.ExtraRearWheels.Length; i++)
			{
				Vector3 vector5 = this.ExtraRearWheels[i].transform.TransformPoint(this.ExtraRearWheels[i].center);
				if (Physics.Raycast(vector5, -this.ExtraRearWheels[i].transform.up, out raycastHit, (this.ExtraRearWheels[i].suspensionDistance + this.ExtraRearWheels[i].radius) * base.transform.localScale.y))
				{
					this.ExtraRearWheelsT[i].transform.position = raycastHit.point + this.ExtraRearWheels[i].transform.up * this.ExtraRearWheels[i].radius * base.transform.localScale.y;
				}
				else
				{
					this.ExtraRearWheelsT[i].transform.position = vector5 - this.ExtraRearWheels[i].transform.up * this.ExtraRearWheels[i].suspensionDistance * base.transform.localScale.y;
					this.ExtraRearWheels[i].brakeTorque = this.Brake / 10f;
				}
				this.ExtraRearWheelsT[i].transform.rotation = this.ExtraRearWheels[i].transform.rotation * Quaternion.Euler(this.RotationValueExtra[i], 0f, this.ExtraRearWheels[i].transform.rotation.z);
				this.RotationValueExtra[i] += this.ExtraRearWheels[i].rpm * 6f * Time.deltaTime;
				this.ExtraRearWheels[i].GetGroundHit(out wheelHit);
			}
		}
		WheelHit wheelHit2;
		this.Wheel_RR.GetGroundHit(out wheelHit2);
		this.driftAngle = Mathf.Lerp(this.driftAngle, Mathf.Clamp(wheelHit2.sidewaysSlip, -35f, 35f), Time.deltaTime * 2f);
		if (this.SteeringWheel)
		{
			this.SteeringWheel.transform.rotation = base.transform.rotation * Quaternion.Euler(0f, 0f, (this.Wheel_FL.steerAngle + this.driftAngle / (float)this.steeringAssistanceDivider) * -6f);
		}
	}

	private void Braking()
	{
		if (Input.GetButton("Jump") || this.andHandBrake)
		{
			this.Wheel_RL.brakeTorque = this.Brake;
			this.Wheel_RR.brakeTorque = this.Brake;
			this.Wheel_FL.brakeTorque = this.Brake / 2f;
			this.Wheel_FR.brakeTorque = this.Brake / 2f;
			this.RearLeftFriction.stiffness = this.handbrakeStiffness;
			this.RearRightFriction.stiffness = this.handbrakeStiffness;
			this.FrontLeftFriction.stiffness = this.handbrakeStiffness;
			this.FrontRightFriction.stiffness = this.handbrakeStiffness;
			this.Wheel_FL.sidewaysFriction = this.FrontRightFriction;
			this.Wheel_FR.sidewaysFriction = this.FrontLeftFriction;
			this.Wheel_RL.sidewaysFriction = this.RearLeftFriction;
			this.Wheel_RR.sidewaysFriction = this.RearRightFriction;
		}
		else
		{
			if (this.motorInput == 0f)
			{
				this.Wheel_RL.brakeTorque = this.Brake / 10f;
				this.Wheel_RR.brakeTorque = this.Brake / 10f;
				this.Wheel_FL.brakeTorque = this.Brake / 10f;
				this.Wheel_FR.brakeTorque = this.Brake / 10f;
			}
			else if (this.motorInput < 0f && this.Wheel_FL.rpm > 0f)
			{
				this.Wheel_FL.brakeTorque = this.Brake * Mathf.Abs(this.motorInput);
				this.Wheel_FR.brakeTorque = this.Brake * Mathf.Abs(this.motorInput);
				this.Wheel_RL.brakeTorque = this.Brake * Mathf.Abs(this.motorInput);
				this.Wheel_RR.brakeTorque = this.Brake * Mathf.Abs(this.motorInput);
			}
			else
			{
				this.Wheel_RL.brakeTorque = 0f;
				this.Wheel_RR.brakeTorque = 0f;
				this.Wheel_FL.brakeTorque = 0f;
				this.Wheel_FR.brakeTorque = 0f;
			}
			if (!this.driftMode)
			{
				this.RearLeftFriction.stiffness = Mathf.Lerp(this.RearLeftFriction.stiffness, this.StiffnessRear, Time.deltaTime * 2f);
				this.RearRightFriction.stiffness = Mathf.Lerp(this.RearRightFriction.stiffness, this.StiffnessRear, Time.deltaTime * 2f);
				this.FrontLeftFriction.stiffness = Mathf.Lerp(this.FrontLeftFriction.stiffness, this.StiffnessFront, Time.deltaTime * 2f);
				this.FrontRightFriction.stiffness = Mathf.Lerp(this.FrontRightFriction.stiffness, this.StiffnessFront, Time.deltaTime * 2f);
				this.Wheel_FL.sidewaysFriction = this.FrontRightFriction;
				this.Wheel_FR.sidewaysFriction = this.FrontLeftFriction;
				this.Wheel_RL.sidewaysFriction = this.RearLeftFriction;
				this.Wheel_RR.sidewaysFriction = this.RearRightFriction;
			}
			else if (this.steerInput != 0f)
			{
				this.RearLeftFriction.stiffness = Mathf.Lerp(this.RearLeftFriction.stiffness, 0.1f, Time.deltaTime * 2f);
				this.RearRightFriction.stiffness = Mathf.Lerp(this.RearRightFriction.stiffness, 0.1f, Time.deltaTime * 2f);
				this.FrontLeftFriction.stiffness = Mathf.Lerp(this.FrontLeftFriction.stiffness, this.StiffnessFront, Time.deltaTime * 2f);
				this.FrontRightFriction.stiffness = Mathf.Lerp(this.FrontRightFriction.stiffness, this.StiffnessFront, Time.deltaTime * 2f);
				this.Wheel_FL.sidewaysFriction = this.FrontRightFriction;
				this.Wheel_FR.sidewaysFriction = this.FrontLeftFriction;
				this.Wheel_RL.sidewaysFriction = this.RearLeftFriction;
				this.Wheel_RR.sidewaysFriction = this.RearRightFriction;
			}
			else
			{
				this.RearLeftFriction.stiffness = Mathf.Lerp(this.RearLeftFriction.stiffness, this.StiffnessRear, Time.deltaTime);
				this.RearRightFriction.stiffness = Mathf.Lerp(this.RearRightFriction.stiffness, this.StiffnessRear, Time.deltaTime);
				this.FrontLeftFriction.stiffness = Mathf.Lerp(this.FrontLeftFriction.stiffness, this.StiffnessFront, Time.deltaTime);
				this.FrontRightFriction.stiffness = Mathf.Lerp(this.FrontRightFriction.stiffness, this.StiffnessFront, Time.deltaTime);
				this.Wheel_FL.sidewaysFriction = this.FrontRightFriction;
				this.Wheel_FR.sidewaysFriction = this.FrontLeftFriction;
				this.Wheel_RL.sidewaysFriction = this.RearLeftFriction;
				this.Wheel_RR.sidewaysFriction = this.RearRightFriction;
			}
		}
	}

	private void SkidAudio()
	{
		WheelHit wheelHit;
		this.Wheel_FR.GetGroundHit(out wheelHit);
		if (Mathf.Abs(wheelHit.sidewaysSlip) > 5f || Mathf.Abs(wheelHit.forwardSlip) > 7f)
		{
			this.skidAudio.GetComponent<AudioSource>().volume = Mathf.Abs(wheelHit.sidewaysSlip) / 10f + Mathf.Abs(wheelHit.forwardSlip) / 10f;
		}
		else
		{
			this.skidAudio.GetComponent<AudioSource>().volume -= Time.deltaTime;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.contacts.Length > 0 && collision.relativeVelocity.magnitude > (float)this.collisionForceLimit && this.crashClips.Length > 0 && collision.contacts[0].thisCollider.gameObject.layer != LayerMask.NameToLayer("Wheel") && collision.gameObject.layer != LayerMask.NameToLayer("Road"))
		{
			base.GetComponent<Construction>().GetDamage(1f);
			if (this.crashAudio != null)
			{
				this.crashAudio.GetComponent<AudioSource>().clip = this.crashClips[UnityEngine.Random.Range(0, this.crashClips.Length)];
				this.crashAudio.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(1f, 1.2f);
				this.crashAudio.GetComponent<AudioSource>().Play();
			}
		}
	}

	private void Chassis()
	{
		WheelHit wheelHit;
		this.Wheel_RR.GetGroundHit(out wheelHit);
		this.verticalLean = Mathf.Clamp(Mathf.Lerp(this.verticalLean, base.GetComponent<Rigidbody>().angularVelocity.x * this.chassisVerticalLean, Time.deltaTime * 5f), -3f, 3f);
		this.horizontalLean = Mathf.Clamp(Mathf.Lerp(this.horizontalLean, base.GetComponent<Rigidbody>().angularVelocity.y * this.chassisHorizontalLean, Time.deltaTime * 3f), -5f, 5f);
		Quaternion localRotation = Quaternion.Euler(this.verticalLean, this.chassis.transform.localRotation.y, this.horizontalLean);
		this.chassis.transform.localRotation = localRotation;
	}

	private void Lights()
	{
		float intensity = Mathf.Clamp(-this.motorInput * 2f, 0f, 1f);
		if (Input.GetKeyDown(KeyCode.L))
		{
			if (this.headLightsOn)
			{
				this.headLightsOn = false;
			}
			else
			{
				this.headLightsOn = true;
			}
		}
		for (int i = 0; i < this.BrakeLights.Length; i++)
		{
			if (this.Wheel_FL.rpm > 0f)
			{
				this.BrakeLights[i].intensity = intensity;
			}
			else
			{
				this.BrakeLights[i].intensity = Mathf.Lerp(this.BrakeLights[i].intensity, 0f, Time.deltaTime * 5f);
			}
		}
		for (int j = 0; j < this.HeadLights.Length; j++)
		{
			if (this.headLightsOn)
			{
				this.HeadLights[j].enabled = true;
			}
			else
			{
				this.HeadLights[j].enabled = false;
			}
		}
		for (int k = 0; k < this.ReverseLights.Length; k++)
		{
			if (this.Wheel_FL.rpm > 0f)
			{
				this.ReverseLights[k].intensity = Mathf.Lerp(this.ReverseLights[k].intensity, 0f, Time.deltaTime * 5f);
			}
			else
			{
				this.ReverseLights[k].intensity = intensity;
			}
		}
	}
}
