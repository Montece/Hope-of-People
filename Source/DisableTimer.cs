using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
	public float Seconds;

	public void Disable()
	{
		base.StartCoroutine(this.Wait());
	}

	[DebuggerHidden]
	private IEnumerator Wait()
	{
		DisableTimer.<Wait>c__Iterator0 <Wait>c__Iterator = new DisableTimer.<Wait>c__Iterator0();
		<Wait>c__Iterator.$this = this;
		return <Wait>c__Iterator;
	}
}
