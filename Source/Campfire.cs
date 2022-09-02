using System;
using UnityEngine;
using UnityEngine.UI;

public class Campfire : MonoBehaviour
{
	[HideInInspector]
	public Inventory InputResourcesSlots;

	[HideInInspector]
	public Inventory InputFuelSlots;

	[HideInInspector]
	public Inventory OutputSlots;

	public float RepeatTime = 3f;

	public int FuelForOne = 1;

	private int CurrentFuelCount;

	private bool IsWorking;

	public Player openedPlayer;

	private void Awake()
	{
		this.InputResourcesSlots = base.GetComponents<Inventory>()[0];
		this.InputFuelSlots = base.GetComponents<Inventory>()[1];
		this.OutputSlots = base.GetComponents<Inventory>()[2];
		base.transform.GetChild(0).gameObject.SetActive(false);
	}

	private void On()
	{
		base.InvokeRepeating("Frying", this.RepeatTime, this.RepeatTime);
	}

	private void Off()
	{
		base.CancelInvoke();
	}

	private void Frying()
	{
		bool flag = false;
		int index = 0;
		for (int i = 0; i < this.InputFuelSlots.InventoryArray.Count; i++)
		{
			if (this.InputFuelSlots.InventoryArray[i].Item != null && this.InputFuelSlots.InventoryArray[i].Count > 0 && Inventory.IsResource(this.InputFuelSlots.InventoryArray[i].Item) && (this.InputFuelSlots.InventoryArray[i].Item as Resource).IsFuel)
			{
				flag = true;
				index = i;
				break;
			}
		}
		if (flag)
		{
			this.CurrentFuelCount++;
			if (this.CurrentFuelCount == this.FuelForOne)
			{
				this.InputFuelSlots.InventoryArray[index].Count--;
				this.CurrentFuelCount = 0;
			}
			for (int j = 0; j < this.InputResourcesSlots.InventoryArray.Count; j++)
			{
				if (this.InputResourcesSlots.InventoryArray[j].Item != null)
				{
					Item fryResult = this.InputResourcesSlots.InventoryArray[j].Item.FryResult;
					if (fryResult != null && this.InputResourcesSlots.InventoryArray[j].Item.CanCampfire)
					{
						Slot slot = new Slot(fryResult);
						if (this.OutputSlots.Add(slot, 1))
						{
							this.InputResourcesSlots.InventoryArray[j].Count--;
						}
						else
						{
							this.IsWorking = false;
						}
					}
				}
			}
			this.InputResourcesSlots.Show();
			this.InputFuelSlots.Show();
			this.OutputSlots.Show();
		}
		else
		{
			this.ToggleCampfire();
		}
	}

	public bool ToggleCampfire()
	{
		this.IsWorking = !this.IsWorking;
		if (this.IsWorking)
		{
			this.On();
			this.openedPlayer.links.ToggleImage2.color = Color.green;
			this.openedPlayer.links.ToggleButtonText2.GetComponentInChildren<Text>().color = Color.green;
			base.transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			this.Off();
			this.openedPlayer.links.ToggleImage2.color = Color.red;
			this.openedPlayer.links.ToggleButtonText2.GetComponentInChildren<Text>().color = Color.green;
			base.transform.GetChild(0).gameObject.SetActive(false);
		}
		return this.IsWorking;
	}

	public void Show()
	{
		this.InputResourcesSlots.Show();
		this.InputResourcesSlots.player = this.openedPlayer;
		this.InputFuelSlots.Show();
		this.InputFuelSlots.player = this.openedPlayer;
		this.OutputSlots.Show();
		this.OutputSlots.player = this.openedPlayer;
	}
}
