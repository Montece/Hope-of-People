using System;
using UnityEngine;

public class gWeatherObject : MonoBehaviour
{
	public GameObject oPlayer;

	private void Start()
	{
		base.transform.parent = this.oPlayer.transform;
	}
}
