using System;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
	public AudioSource HelicopterSound;

	public ControlPanel ControlPanel;

	public Rigidbody HelicopterModel;

	public HeliRotorController MainRotorController;

	public HeliRotorController SubRotorController;

	[Space]
	public GameObject Camera;

	public bool IsWorking;

	private GameObject player;

	public float TurnForce = 3f;

	public float ForwardForce = 10f;

	public float ForwardTiltForce = 20f;

	public float TurnTiltForce = 30f;

	public float EffectiveHeight = 100f;

	public float turnTiltForcePercent = 1.5f;

	public float turnForcePercent = 1.3f;

	private float _engineForce;

	private Vector2 hMove = Vector2.zero;

	private Vector2 hTilt = Vector2.zero;

	private float hTurn;

	public bool IsOnGround = true;

	public float EngineForce
	{
		get
		{
			return this._engineForce;
		}
		set
		{
			this.MainRotorController.RotarSpeed = value * 80f;
			this.SubRotorController.RotarSpeed = value * 40f;
			this.HelicopterSound.pitch = Mathf.Clamp(value / 40f, 0f, 1.2f);
			if (UIGameController.runtime.EngineForceView != null)
			{
				UIGameController.runtime.EngineForceView.text = string.Format("Engine value [ {0} ] ", (int)value);
			}
			this._engineForce = value;
		}
	}

	private void Start()
	{
		ControlPanel expr_06 = this.ControlPanel;
		expr_06.KeyPressed = (Action<PressedKeyCode[]>)Delegate.Combine(expr_06.KeyPressed, new Action<PressedKeyCode[]>(this.OnKeyPressed));
	}

	private void FixedUpdate()
	{
		if (this.IsWorking)
		{
			this.LiftProcess();
			this.MoveProcess();
			this.TiltProcess();
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.Off();
			}
		}
	}

	private void MoveProcess()
	{
		float b = this.TurnForce * Mathf.Lerp(this.hMove.x, this.hMove.x * (this.turnTiltForcePercent - Mathf.Abs(this.hMove.y)), Mathf.Max(0f, this.hMove.y));
		this.hTurn = Mathf.Lerp(this.hTurn, b, Time.fixedDeltaTime * this.TurnForce);
		this.HelicopterModel.AddRelativeTorque(0f, this.hTurn * this.HelicopterModel.mass, 0f);
		this.HelicopterModel.AddRelativeForce(Vector3.forward * Mathf.Max(0f, this.hMove.y * this.ForwardForce * this.HelicopterModel.mass));
	}

	private void LiftProcess()
	{
		float num = 1f - Mathf.Clamp(this.HelicopterModel.transform.position.y / this.EffectiveHeight, 0f, 1f);
		num = Mathf.Lerp(0f, this.EngineForce, num) * this.HelicopterModel.mass;
		this.HelicopterModel.AddRelativeForce(Vector3.up * num);
	}

	private void TiltProcess()
	{
		this.hTilt.x = Mathf.Lerp(this.hTilt.x, this.hMove.x * this.TurnTiltForce, Time.deltaTime);
		this.hTilt.y = Mathf.Lerp(this.hTilt.y, this.hMove.y * this.ForwardTiltForce, Time.deltaTime);
		this.HelicopterModel.transform.localRotation = Quaternion.Euler(this.hTilt.y, this.HelicopterModel.transform.localEulerAngles.y, -this.hTilt.x);
	}

	private void OnKeyPressed(PressedKeyCode[] obj)
	{
		if (!this.IsWorking)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		if (this.hMove.y > 0f)
		{
			num = -Time.fixedDeltaTime;
		}
		else if (this.hMove.y < 0f)
		{
			num = Time.fixedDeltaTime;
		}
		if (this.hMove.x > 0f)
		{
			num2 = -Time.fixedDeltaTime;
		}
		else if (this.hMove.x < 0f)
		{
			num2 = Time.fixedDeltaTime;
		}
		for (int i = 0; i < obj.Length; i++)
		{
			switch (obj[i])
			{
			case PressedKeyCode.SpeedUpPressed:
				this.EngineForce += 0.1f;
				break;
			case PressedKeyCode.SpeedDownPressed:
				this.EngineForce -= 0.12f;
				if (this.EngineForce < 0f)
				{
					this.EngineForce = 0f;
				}
				break;
			case PressedKeyCode.ForwardPressed:
				if (!this.IsOnGround)
				{
					num = Time.fixedDeltaTime;
				}
				break;
			case PressedKeyCode.BackPressed:
				if (!this.IsOnGround)
				{
					num = -Time.fixedDeltaTime;
				}
				break;
			case PressedKeyCode.LeftPressed:
				if (!this.IsOnGround)
				{
					num2 = -Time.fixedDeltaTime;
				}
				break;
			case PressedKeyCode.RightPressed:
				if (!this.IsOnGround)
				{
					num2 = Time.fixedDeltaTime;
				}
				break;
			case PressedKeyCode.TurnLeftPressed:
				if (!this.IsOnGround)
				{
					float y = -(this.turnForcePercent - Mathf.Abs(this.hMove.y)) * this.HelicopterModel.mass;
					this.HelicopterModel.AddRelativeTorque(0f, y, 0f);
				}
				break;
			case PressedKeyCode.TurnRightPressed:
				if (!this.IsOnGround)
				{
					float y2 = (this.turnForcePercent - Mathf.Abs(this.hMove.y)) * this.HelicopterModel.mass;
					this.HelicopterModel.AddRelativeTorque(0f, y2, 0f);
				}
				break;
			}
		}
		this.hMove.x = this.hMove.x + num2;
		this.hMove.x = Mathf.Clamp(this.hMove.x, -1f, 1f);
		this.hMove.y = this.hMove.y + num;
		this.hMove.y = Mathf.Clamp(this.hMove.y, -1f, 1f);
	}

	private void OnCollisionEnter()
	{
		this.IsOnGround = true;
	}

	private void OnCollisionExit()
	{
		this.IsOnGround = false;
	}

	public void On(GameObject g)
	{
		this.player = g;
		this.player.SetActive(false);
		this.Camera.SetActive(true);
		this.IsWorking = true;
	}

	public void Off()
	{
		this.player.SetActive(true);
		this.Camera.SetActive(false);
		this.IsWorking = false;
		this.player.transform.position = base.transform.position + new Vector3(3f, 0f, 0f);
	}
}
