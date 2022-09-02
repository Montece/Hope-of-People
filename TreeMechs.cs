using System;
using UnityEngine;

public class TreeMechs : MonoBehaviour
{
	public int maximumDurability = 1000;

	public int currentDurability;

	private Player player;

	private Plant plant;

	private void Start()
	{
		this.plant = base.GetComponent<Plant>();
		this.currentDurability = this.maximumDurability;
	}

	private void Update()
	{
		if (this.currentDurability <= 0)
		{
			this.DestroyTree();
		}
	}

	public void Chop(Tool item, Player p)
	{
		bool flag = false;
		if (this.plant != null)
		{
			if (this.plant.CurrentGrowTime == this.plant.GrowTimeInSeconds)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			this.player = p;
			this.currentDurability -= item.Efficiency;
			p.MainInventory.Add("woodlog", item.Efficiency);
			p.MainInventory.Add("rubber", UnityEngine.Random.Range(1, 3));
		}
	}

	private void DestroyTree()
	{
		this.player.MainInventory.Add("apple", 10);
		this.player.MainInventory.Add("treesapling", 3);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
