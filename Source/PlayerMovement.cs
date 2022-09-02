using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMovement : MonoBehaviour
{
	private Player player;

	private Inventory inv;

	private FirstPersonController control;

	private CharacterController cc;

	private PlayerStats stats;

	public AudioClip[] FootsepSounds;

	public AudioClip JumpSound;

	public AudioClip LandSound;

	private bool IsOnLadder;

	private float CrouchWalkSpeed;

	private float CrouchRunSpeed;

	private float CrouchH;

	private float WalkSpeed;

	private float RunSpeed;

	private float H;

	public float minSurviveFall = 1.1f;

	public float damageForSeconds = 10f;

	public float airTime;

	private void Start()
	{
		this.control = base.GetComponent<FirstPersonController>();
		this.cc = base.GetComponent<CharacterController>();
		this.player = base.GetComponent<Player>();
		this.WalkSpeed = this.control.m_WalkSpeed;
		this.RunSpeed = this.control.m_RunSpeed;
		this.CrouchWalkSpeed = this.WalkSpeed * 0.5f;
		this.CrouchRunSpeed = this.RunSpeed * 0.5f;
		this.H = this.cc.height;
		this.CrouchH = this.H * 0.3f;
	}

	private void Update()
	{
		if (this.stats == null)
		{
			this.stats = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStats>();
		}
		this.Crouch();
		this.Slowing();
		this.FallDamage();
		this.Climb();
	}

	private void Climb()
	{
		if (this.IsOnLadder)
		{
			if (Input.GetKey(this.player.ClimbUp))
			{
				if (this.control.m_IsWalking)
				{
					base.transform.Translate(Vector3.up * Time.deltaTime * this.WalkSpeed);
				}
				else
				{
					base.transform.Translate(new Vector3(0f, -1f, 0f) * Time.deltaTime * this.RunSpeed);
				}
			}
			if (Input.GetKey(this.player.ClimbDown))
			{
				if (this.control.m_IsWalking)
				{
					base.transform.Translate(new Vector3(0f, -1f, 0f) * Time.deltaTime * this.WalkSpeed);
				}
				else
				{
					base.transform.Translate(new Vector3(0f, -1f, 0f) * Time.deltaTime * this.RunSpeed);
				}
			}
		}
	}

	private void Crouch()
	{
		if (Input.GetKey(this.player.Crouch))
		{
			this.control.m_WalkSpeed = this.CrouchWalkSpeed;
			this.control.m_RunSpeed = this.CrouchRunSpeed;
			this.cc.height = Mathf.Lerp(this.CrouchH, this.H, 3f * Time.deltaTime);
		}
		if (Input.GetKeyUp(this.player.Crouch))
		{
			this.control.m_WalkSpeed = this.WalkSpeed;
			this.control.m_RunSpeed = this.RunSpeed;
			this.cc.height = this.H;
			this.cc.height = Mathf.Lerp(this.H, this.CrouchH, 3f * Time.deltaTime);
		}
	}

	private void Slowing()
	{
		if (Input.GetKey(this.player.Slowing))
		{
			this.control.m_JumpSound = null;
			this.control.m_LandSound = null;
			for (int i = 0; i < this.control.m_FootstepSounds.Length; i++)
			{
				this.control.m_FootstepSounds[i] = null;
			}
			this.control.m_WalkSpeed = this.CrouchWalkSpeed;
			this.control.m_RunSpeed = this.CrouchRunSpeed;
		}
		if (Input.GetKeyUp(this.player.Slowing))
		{
			this.control.m_JumpSound = this.JumpSound;
			this.control.m_LandSound = this.LandSound;
			for (int j = 0; j < this.control.m_FootstepSounds.Length; j++)
			{
				this.control.m_FootstepSounds[j] = this.FootsepSounds[j];
			}
			this.control.m_WalkSpeed = this.WalkSpeed;
			this.control.m_RunSpeed = this.RunSpeed;
		}
	}

	private void FallDamage()
	{
		if (!this.cc.isGrounded && base.GetComponentInParent<CharacterController>().enabled)
		{
			this.airTime += Time.deltaTime;
		}
		else
		{
			if (this.airTime > this.minSurviveFall)
			{
				this.stats.GetDamage(DamageType.Legs, this.damageForSeconds * this.airTime);
			}
			this.airTime = 0f;
		}
	}

	private void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.GetComponent<LadderZone>())
		{
			this.IsOnLadder = true;
		}
	}

	private void OnCollisionExit(Collision coll)
	{
		if (coll.gameObject.GetComponent<LadderZone>())
		{
			this.IsOnLadder = false;
		}
	}
}
