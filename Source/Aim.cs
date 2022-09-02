using System;
using UnityEngine;

public class Aim : MonoBehaviour
{
	private Player player;

	public Vector3 FirstP;

	public Vector3 SecondP;

	public Quaternion FirstR;

	public Quaternion SecondR;

	public float AimSpeed = 40f;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		base.transform.localPosition = this.FirstP;
		base.transform.localRotation = this.FirstR;
	}

	private void Update()
	{
		if (Input.GetKey(this.player.Aim))
		{
			base.transform.localPosition = Vector3.Slerp(base.transform.localPosition, this.SecondP, this.AimSpeed * Time.deltaTime);
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.SecondR, this.AimSpeed * Time.deltaTime);
		}
		if (Input.GetKeyUp(this.player.Aim))
		{
			base.transform.localPosition = this.FirstP;
			base.transform.localRotation = this.FirstR;
		}
	}
}
