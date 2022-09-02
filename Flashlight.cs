using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
	private Player player;

	private AudioSource source;

	private Light lightsrc;

	public bool IsOn;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
		this.lightsrc = base.GetComponent<Light>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot) && this.player.CanUseTools)
		{
			base.StartCoroutine(this.Switch());
		}
	}

	[DebuggerHidden]
	private IEnumerator Switch()
	{
		Flashlight.<Switch>c__Iterator0 <Switch>c__Iterator = new Flashlight.<Switch>c__Iterator0();
		<Switch>c__Iterator.$this = this;
		return <Switch>c__Iterator;
	}
}
