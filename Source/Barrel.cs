using System;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	[Header("Barrel"), HideInInspector]
	public List<Slot> InventoryArray = new List<Slot>();

	public string BarrelID;

	public int MinCount = 3;

	public int MaxCount = 5;

	private ChestConfig config = new ChestConfig();

	private void Start()
	{
		this.AddLoot();
	}

	private void AddLoot()
	{
		this.config = Database.GetChestConfig(this.BarrelID);
		List<Slot> list = new List<Slot>();
		for (int i = 0; i < this.config.items.Count; i++)
		{
			Slot slot = new Slot(Database.Get(this.config.items[i].ItemID))
			{
				Count = UnityEngine.Random.Range(this.config.items[i].MinAmount, this.config.items[i].MaxAmount)
			};
			switch (slot.Item.ItemType)
			{
			case ItemType.Healing:
				slot.Count *= Database.HealingRate;
				break;
			case ItemType.Food:
				slot.Count *= Database.FoodRate;
				break;
			case ItemType.Resource:
				slot.Count *= Database.ResourceRate;
				break;
			case ItemType.Building:
				slot.Count *= Database.BuildingRate;
				break;
			}
			list.Add(slot);
		}
		int num = UnityEngine.Random.Range(this.MinCount, this.MaxCount + 1);
		for (int j = 0; j < num; j++)
		{
			Slot item = list[UnityEngine.Random.Range(0, list.Count)];
			this.InventoryArray.Add(item);
		}
	}

	public void DestroyBarrel()
	{
		for (int i = 0; i < this.InventoryArray.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"));
			gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
			gameObject.GetComponent<Pickup>().slot = this.InventoryArray[i];
		}
		if (base.transform.parent != null)
		{
			UnityEngine.Object.Destroy(base.transform.parent);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
