using System;
using UnityEngine;

public class CustomChest : MonoBehaviour
{
	public PickupObject[] ItemsIds;

	private void Start()
	{
		Inventory component = base.GetComponent<Inventory>();
		PickupObject[] itemsIds = this.ItemsIds;
		for (int i = 0; i < itemsIds.Length; i++)
		{
			PickupObject pickupObject = itemsIds[i];
			component.InventoryArray.Add(new Slot(null));
			component.Add(pickupObject.Item, pickupObject.MinCount);
		}
	}
}
