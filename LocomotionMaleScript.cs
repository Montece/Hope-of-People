using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class LocomotionMaleScript : MonoBehaviour
{
	private Animator anim;

	private void Start()
	{
		this.anim = base.transform.GetComponent<Animator>();
	}

	private void OnGUI()
	{
		GUILayout.Label("CONTROLS", new GUILayoutOption[0]);
		GUILayout.Label("Movement: W A S D", new GUILayoutOption[0]);
		GUILayout.Label("Turn: Q E", new GUILayoutOption[0]);
		GUILayout.Label("Jump: Spacebar", new GUILayoutOption[0]);
	}

	private void Update()
	{
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		this.anim.SetFloat("Speed", axis2, 0.15f, Time.deltaTime);
		this.anim.SetFloat("Direction", axis, 0.15f, Time.deltaTime);
		if (axis2 > 0.05f)
		{
			if (axis > 0.05f)
			{
				base.transform.Rotate(Vector3.up * (Time.deltaTime + 2f), Space.World);
			}
			if (axis < -0.05f)
			{
				base.transform.Rotate(Vector3.up * (Time.deltaTime + -2f), Space.World);
			}
		}
		else if (axis2 < -0.05f)
		{
			if (axis > 0.05f)
			{
				base.transform.Rotate(Vector3.up * (Time.deltaTime + -2f), Space.World);
			}
			if (axis < -0.05f)
			{
				base.transform.Rotate(Vector3.up * (Time.deltaTime + 2f), Space.World);
			}
		}
		if (Input.GetKey(KeyCode.Q))
		{
			this.anim.SetFloat("Turn", -1f, 0.1f, Time.deltaTime);
			base.transform.Rotate(Vector3.up * (Time.deltaTime + -2f), Space.World);
		}
		else if (Input.GetKey(KeyCode.E))
		{
			this.anim.SetFloat("Turn", 1f, 0.1f, Time.deltaTime);
			base.transform.Rotate(Vector3.up * (Time.deltaTime + 2f), Space.World);
		}
		else
		{
			this.anim.SetFloat("Turn", 0f, 0.1f, Time.deltaTime);
		}
		if (Input.GetButton("Jump"))
		{
			base.StartCoroutine(this.TriggerAnimatorBool("Jump"));
		}
	}

	[DebuggerHidden]
	private IEnumerator TriggerAnimatorBool(string name)
	{
		LocomotionMaleScript.<TriggerAnimatorBool>c__Iterator0 <TriggerAnimatorBool>c__Iterator = new LocomotionMaleScript.<TriggerAnimatorBool>c__Iterator0();
		<TriggerAnimatorBool>c__Iterator.name = name;
		<TriggerAnimatorBool>c__Iterator.$this = this;
		return <TriggerAnimatorBool>c__Iterator;
	}
}
