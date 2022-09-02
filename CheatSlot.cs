using System;
using UnityEngine;

public class CheatSlot : MonoBehaviour
{
	public string ItemID;

	[HideInInspector]
	public Item Item;

	[HideInInspector]
	public Inventory Inventory;

	public void Add()
	{
		this.Inventory.Add(new Slot(this.Item), 10);
	}
}
