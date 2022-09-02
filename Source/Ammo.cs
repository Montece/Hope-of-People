using System;
using UnityEngine;

public class Ammo : Item
{
	public GameObject Prefab;

	public Ammo()
	{
		this.ItemType = ItemType.Ammo;
	}
}
