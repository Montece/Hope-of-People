using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AirdropShip : MonoBehaviour
{
	public float Speed = 2f;

	public bool IsMoving;

	public bool DoAirDrop;

	[HideInInspector]
	public Vector3 Target;

	public GameObject DropChest;

	private void Update()
	{
		if (this.IsMoving)
		{
			Vector3 b = base.transform.forward * this.Speed;
			base.transform.position += b;
		}
		if (this.DoAirDrop && (int)(base.transform.position.x / 10f) == (int)(this.Target.x / 10f) && (int)base.transform.position.z / 10 == (int)this.Target.z / 10)
		{
			base.StartCoroutine(this.Drop());
			this.DoAirDrop = false;
		}
	}

	[DebuggerHidden]
	private IEnumerator Drop()
	{
		AirdropShip.<Drop>c__Iterator0 <Drop>c__Iterator = new AirdropShip.<Drop>c__Iterator0();
		<Drop>c__Iterator.$this = this;
		return <Drop>c__Iterator;
	}
}
