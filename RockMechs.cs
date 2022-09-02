using System;
using UnityEngine;

public class RockMechs : MonoBehaviour
{
	public int maximumDurability = 1000;

	public int currentDurability;

	private void Start()
	{
		this.currentDurability = this.maximumDurability;
	}

	private void Update()
	{
		if (this.currentDurability <= 0)
		{
			this.DestroyRock();
		}
	}

	private void DestroyRock()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Harvest(Tool item, Player player)
	{
		this.currentDurability -= item.Efficiency;
		int num = UnityEngine.Random.Range(1, 101);
		if (num > 0 && num < 40)
		{
			player.MainInventory.Add("stone", item.Efficiency);
		}
		if (num >= 40 && num < 60)
		{
			player.MainInventory.Add("ironore", item.Efficiency);
		}
		if (num >= 60 && num < 64)
		{
			player.MainInventory.Add("coal", item.Efficiency);
		}
		if (num >= 64 && num < 68)
		{
			player.MainInventory.Add("ironore", item.Efficiency);
		}
		if (num >= 68 && num < 82)
		{
			player.MainInventory.Add("leadore", item.Efficiency);
		}
		if (num >= 82 && num < 86)
		{
			player.MainInventory.Add("tinore", item.Efficiency);
		}
		if (num >= 90 && num < 94)
		{
			player.MainInventory.Add("copperore", item.Efficiency);
		}
		if (num >= 94 && num <= 98)
		{
			player.MainInventory.Add("sulphur", item.Efficiency);
		}
		if (num >= 99 && num <= 100)
		{
			player.MainInventory.Add("diamond", 1);
		}
	}
}
