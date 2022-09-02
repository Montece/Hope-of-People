using System;
using UnityEngine;

public class LookAtTargetController : MonoBehaviour
{
	public Transform Target;

	public bool smooth = true;

	public float damping = 6f;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (this.Target != null)
		{
			if (this.smooth)
			{
				Quaternion b = Quaternion.LookRotation(this.Target.position - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * this.damping);
			}
			else
			{
				base.transform.LookAt(this.Target);
			}
		}
	}
}
