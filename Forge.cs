using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Forge : MonoBehaviour
{
	[HideInInspector]
	public Inventory InputResourcesSlots;

	[HideInInspector]
	public Inventory InputFuelSlots;

	[HideInInspector]
	public Inventory OutputSlots;

	public float TickTime = 5f;

	public int ResourcesForOneFuel = 2;

	private int CurrentFuelCount;

	private bool IsWorking;

	private Player openedPlayer;

	private void Awake()
	{
		this.InputResourcesSlots = base.GetComponents<Inventory>()[0];
		this.InputFuelSlots = base.GetComponents<Inventory>()[1];
		this.OutputSlots = base.GetComponents<Inventory>()[2];
	}

	private void On()
	{
		base.InvokeRepeating("Smelting", this.TickTime, this.TickTime);
		base.transform.GetChild(0).gameObject.SetActive(true);
	}

	private void Off()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
		base.CancelInvoke();
	}

	private void Smelting()
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
			if (this.CurrentFuelCount == this.ResourcesForOneFuel)
			{
				this.InputFuelSlots.InventoryArray[index].Count--;
				this.CurrentFuelCount = 0;
			}
			for (int j = 0; j < this.InputResourcesSlots.InventoryArray.Count; j++)
			{
				if (this.InputResourcesSlots.InventoryArray[j].Item != null)
				{
					Item fryResult = this.InputResourcesSlots.InventoryArray[j].Item.FryResult;
					if (fryResult != null && this.InputResourcesSlots.InventoryArray[j].Item.CanForge)
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
					else
					{
						this.Explode();
					}
				}
			}
			this.InputResourcesSlots.Show();
			this.InputFuelSlots.Show();
			this.OutputSlots.Show();
		}
		else
		{
			this.ToggleForge();
		}
	}

	private void Explode()
	{
		GameObject gameObject = new GameObject
		{
			name = "Bricks"
		};
		UnityEngine.Object.Destroy(base.transform.GetChild(0).gameObject);
		UnityEngine.Object.Destroy(base.transform.GetChild(1).gameObject);
		UnityEngine.Object.Destroy(base.transform.GetChild(0).gameObject);
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.GetComponent<Rigidbody>())
				{
					transform.GetComponent<Rigidbody>().isKinematic = false;
					transform.GetComponent<Rigidbody>().AddExplosionForce((float)UnityEngine.Random.Range(100, 10000), transform.transform.position, 30f);
				}
				transform.SetParent(gameObject.transform);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.InputResourcesSlots.Hide();
		this.InputFuelSlots.Hide();
		this.OutputSlots.Hide();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public bool ToggleForge()
	{
		this.IsWorking = !this.IsWorking;
		if (this.IsWorking)
		{
			this.On();
			this.openedPlayer.links.ToggleImage1.color = Color.green;
			this.openedPlayer.links.ToggleButtonText1.GetComponentInChildren<Text>().color = Color.green;
		}
		else
		{
			this.Off();
			this.openedPlayer.links.ToggleImage1.color = Color.red;
			this.openedPlayer.links.ToggleButtonText1.GetComponentInChildren<Text>().color = Color.red;
		}
		return this.IsWorking;
	}

	public void Show(Player sender)
	{
		this.openedPlayer = sender;
		this.InputResourcesSlots.Show();
		this.InputResourcesSlots.player = sender;
		this.InputFuelSlots.Show();
		this.InputFuelSlots.player = sender;
		this.OutputSlots.Show();
		this.OutputSlots.player = sender;
	}
}
