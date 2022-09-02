using System;
using UnityEngine;

public class Crusher : MonoBehaviour
{
	[HideInInspector]
	public Inventory InputSlots;

	[HideInInspector]
	public Inventory OutputSlots;

	public float TickTime = 5f;

	public int ResourcesPerTickTime = 2;

	private Player openedPlayer;

	private void Awake()
	{
		this.InputSlots = base.GetComponents<Inventory>()[0];
		this.OutputSlots = base.GetComponents<Inventory>()[1];
	}

	private void Start()
	{
		base.InvokeRepeating("Crushing", this.TickTime, this.TickTime);
	}

	private void Stop()
	{
		base.CancelInvoke();
	}

	private void Crushing()
	{
		for (int i = 0; i < this.InputSlots.InventoryArray.Count; i++)
		{
			if (Inventory.IsResource(this.InputSlots.InventoryArray[i].Item))
			{
				IINFO[] waste = (this.InputSlots.InventoryArray[i].Item as Resource).Waste;
				bool flag = false;
				for (int j = 0; j < waste.Length; j++)
				{
					if (this.OutputSlots.Add(new Slot(waste[j].Item), waste[j].Count) && waste.Length > 0)
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.InputSlots.InventoryArray[i].Count--;
				}
			}
		}
		this.InputSlots.Show();
		this.OutputSlots.Show();
	}

	public void Show(Player sender)
	{
		this.openedPlayer = sender;
		this.InputSlots.Show();
		this.InputSlots.player = sender;
		this.OutputSlots.Show();
		this.OutputSlots.player = sender;
	}
}
