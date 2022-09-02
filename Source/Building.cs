using System;
using UnityEngine;

public class Building : Item
{
	public GameObject Prefab;

	public bool IsPlant;

	public Building()
	{
		this.ItemType = ItemType.Building;
	}
}
