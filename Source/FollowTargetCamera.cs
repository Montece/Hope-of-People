using System;
using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
	public Transform Target;

	public float PositionFolowForce = 5f;

	public float RotationFolowForce = 5f;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		Vector3 forward = Vector3.forward;
		Vector3 a = this.Target.rotation * Vector3.forward;
		a.y = 0f;
		if (a.magnitude > 0f)
		{
			forward = a / a.magnitude;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, this.Target.position, this.PositionFolowForce * Time.deltaTime);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(forward), this.RotationFolowForce * Time.deltaTime);
	}
}
