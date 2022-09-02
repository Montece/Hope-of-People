using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	[Header("Inventory"), HideInInspector]
	public List<Slot> InventoryArray = new List<Slot>();

	public string ChestID;

	[HideInInspector]
	private GameObject InventoryUI;

	private GameObject SlotPrefab;

	public InventoryType InvType = InventoryType.Chest;

	[HideInInspector]
	public Player player;

	private ChestConfig config = new ChestConfig();

	public bool FoodRot = true;

	public bool CanDestroy;

	[Range(1f, 100f), Space]
	public int capacity;

	[HideInInspector]
	public int ChoosenSlot;

	[Space]
	public bool HasRandomLoot;

	[Range(1f, 100f)]
	public int MinSlots = 3;

	[Range(1f, 100f)]
	public int MaxSlots = 5;

	public bool InitOnStart = true;

	public ChestLootType ChestLoot = ChestLootType.So;

	private Buffs buffs;

	private QuestSystem quests;

	private SceneLinks links;

	private void Start()
	{
		if (this.InitOnStart)
		{
			this.OnStart();
		}
	}

	public void OnStart()
	{
		this.InitUI();
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		this.SlotPrefab = Resources.Load<GameObject>("Slot");
		this.links = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneLinks>();
		this.buffs = this.links.gameObject.GetComponent<Buffs>();
		this.quests = this.links.gameObject.GetComponent<QuestSystem>();
		base.CancelInvoke();
		base.InvokeRepeating("EverySecond", 1f, 1f);
		if (this.HasRandomLoot && Database.Items.Count > 0)
		{
			this.AddLoot();
		}
		else if (this.capacity > 0)
		{
			if (this.InventoryArray.Count == 0)
			{
				for (int i = 0; i < this.capacity; i++)
				{
					this.InventoryArray.Add(new Slot(null));
				}
			}
		}
		else
		{
			Debug.LogError("Capacity error!");
		}
	}

	private void InitUI()
	{
		GameObject gameObject = GameObject.Find("UI");
		switch (this.InvType)
		{
		case InventoryType.Forge1:
			this.InventoryUI = gameObject.transform.Find("Inventory/Forge/FiledResources").gameObject;
			break;
		case InventoryType.Forge2:
			this.InventoryUI = gameObject.transform.Find("Inventory/Forge/FiledFuel").gameObject;
			break;
		case InventoryType.Forge3:
			this.InventoryUI = gameObject.transform.Find("Inventory/Forge/FiledOutput").gameObject;
			break;
		case InventoryType.Campfire1:
			this.InventoryUI = gameObject.transform.Find("Inventory/Campfire/FiledResources").gameObject;
			break;
		case InventoryType.Campfire2:
			this.InventoryUI = gameObject.transform.Find("Inventory/Campfire/FiledFuel").gameObject;
			break;
		case InventoryType.Campfire3:
			this.InventoryUI = gameObject.transform.Find("Inventory/Campfire/FiledOutput").gameObject;
			break;
		case InventoryType.MainInventory:
			this.InventoryUI = gameObject.transform.Find("Inventory/Inventory/Field").gameObject;
			break;
		case InventoryType.Toolbet:
			this.InventoryUI = gameObject.transform.Find("ToolbetField").gameObject;
			break;
		case InventoryType.Chest:
			this.InventoryUI = gameObject.transform.Find("Inventory/Chest/FieldChest").gameObject;
			break;
		case InventoryType.Crusher1:
			this.InventoryUI = gameObject.transform.Find("Inventory/Crusher/FieldInput").gameObject;
			break;
		case InventoryType.Crusher2:
			this.InventoryUI = gameObject.transform.Find("Inventory/Crusher/FieldOutput").gameObject;
			break;
		case InventoryType.Cloth:
			this.InventoryUI = gameObject.transform.Find("ClothsField").gameObject;
			break;
		}
	}

	private void CheckForDestroy()
	{
		if (this.CanDestroy)
		{
			bool flag = true;
			for (int i = 0; i < this.InventoryArray.Count; i++)
			{
				if (this.InventoryArray[i].Item != null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (this.player != null)
				{
					this.player.HidePanels();
				}
				if (base.gameObject != null)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	}

	public void Show()
	{
		this.Hide();
		for (int i = 0; i < this.InventoryArray.Count; i++)
		{
			if (this.InventoryArray[i].Count <= 0)
			{
				this.InventoryArray[i].Item = null;
			}
			if (this.InventoryArray[i].Item != null && (Inventory.IsTool(this.InventoryArray[i].Item) || Inventory.IsWeapon(this.InventoryArray[i].Item) || Inventory.IsCloth(this.InventoryArray[i].Item)) && this.InventoryArray[i].CurrentDurability <= 0f)
			{
				if (this.InvType == InventoryType.Toolbet)
				{
					this.player.UnequipAllToolbet();
				}
				this.InventoryArray[i].Item = null;
			}
			if (this.InventoryArray[i].Item != null && Inventory.IsFood(this.InventoryArray[i].Item) && this.InventoryArray[i].CurrentDurability <= 0f)
			{
				this.InventoryArray[i].Item = Database.GetItemByID("rottenfood");
			}
		}
		for (int j = 0; j < this.InventoryArray.Count; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SlotPrefab, this.InventoryUI.transform);
			if (this.InventoryArray[j].Item == null)
			{
				gameObject.GetComponentInChildren<Text>().text = string.Empty;
				gameObject.transform.GetChild(0).GetComponentInChildren<Image>().enabled = false;
			}
			else if (Inventory.IsWeapon(this.InventoryArray[j].Item))
			{
				gameObject.GetComponentInChildren<Text>().text = string.Concat(new object[]
				{
					this.InventoryArray[j].CurrentDurability,
					" (",
					this.InventoryArray[j].CurrentClipSize,
					")"
				});
				gameObject.transform.GetChild(0).GetComponent<Image>().sprite = this.InventoryArray[j].Item.Icon;
				gameObject.transform.GetChild(0).GetComponentInChildren<Image>().enabled = true;
			}
			else if (Inventory.IsTool(this.InventoryArray[j].Item))
			{
				gameObject.GetComponentInChildren<Text>().text = "(" + this.InventoryArray[j].CurrentDurability + ")";
				gameObject.transform.GetChild(0).GetComponent<Image>().sprite = this.InventoryArray[j].Item.Icon;
				gameObject.transform.GetChild(0).GetComponentInChildren<Image>().enabled = true;
			}
			else if (Inventory.IsFood(this.InventoryArray[j].Item))
			{
				if ((this.InventoryArray[j].Item as Food).CanRot)
				{
					gameObject.GetComponentInChildren<Text>().text = string.Concat(new object[]
					{
						"x",
						this.InventoryArray[j].Count,
						" (",
						(int)this.InventoryArray[j].CurrentDurability,
						")"
					});
					gameObject.transform.GetChild(0).GetComponent<Image>().sprite = this.InventoryArray[j].Item.Icon;
					gameObject.transform.GetChild(0).GetComponentInChildren<Image>().enabled = true;
				}
			}
			else if (Inventory.IsCloth(this.InventoryArray[j].Item))
			{
				gameObject.GetComponentInChildren<Text>().text = "(" + this.InventoryArray[j].CurrentDurability + ")";
				gameObject.transform.GetChild(0).GetComponent<Image>().sprite = this.InventoryArray[j].Item.Icon;
				gameObject.transform.GetChild(0).GetComponentInChildren<Image>().enabled = true;
			}
			else
			{
				gameObject.GetComponentInChildren<Text>().text = "x" + this.InventoryArray[j].Count;
				gameObject.transform.GetChild(0).GetComponent<Image>().sprite = this.InventoryArray[j].Item.Icon;
				gameObject.transform.GetChild(0).GetComponentInChildren<Image>().enabled = true;
			}
			if (this.InvType == InventoryType.Cloth)
			{
				switch (j)
				{
				case 0:
					gameObject.GetComponent<SlotObject>().type = ArmorSlot.Head;
					break;
				case 1:
					gameObject.GetComponent<SlotObject>().type = ArmorSlot.Body;
					break;
				case 2:
					gameObject.GetComponent<SlotObject>().type = ArmorSlot.Legs;
					break;
				case 3:
					gameObject.GetComponent<SlotObject>().type = ArmorSlot.Feet;
					break;
				default:
					gameObject.GetComponent<SlotObject>().type = ArmorSlot.None;
					break;
				}
			}
			gameObject.GetComponent<SlotObject>().SlotID = j;
			gameObject.GetComponent<SlotObject>().owner = this;
		}
	}

	public void Hide()
	{
		IEnumerator enumerator = this.InventoryUI.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
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
		this.CheckForDestroy();
	}

	public void Drop(GameObject DropZone, int count = 0)
	{
		if (this.ChoosenSlot >= 0 && this.ChoosenSlot < this.InventoryArray.Count)
		{
			Item item = this.InventoryArray[this.ChoosenSlot].Item;
			if (count == 0 || this.InventoryArray[this.ChoosenSlot].Count <= count)
			{
				this.InventoryArray[this.ChoosenSlot].Item = null;
				count = this.InventoryArray[this.ChoosenSlot].Count;
				this.InventoryArray[this.ChoosenSlot].Count = 0;
			}
			else
			{
				this.InventoryArray[this.ChoosenSlot].Count -= count;
			}
			GameObject gameObject;
			if (item.Drop == null)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"));
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(item.Drop);
			}
			gameObject.transform.position = DropZone.transform.position;
			gameObject.GetComponent<Pickup>().slot.Count = count;
			gameObject.GetComponent<Pickup>().slot.Item = item;
			gameObject.GetComponent<Pickup>().slot.CurrentClipSize = this.InventoryArray[this.ChoosenSlot].CurrentClipSize;
			gameObject.GetComponent<Pickup>().slot.CurrentDurability = this.InventoryArray[this.ChoosenSlot].CurrentDurability;
			this.Show();
		}
	}

	public void Destroy(int count = 0, int slotId = -1)
	{
		if (slotId == -1)
		{
			slotId = this.ChoosenSlot;
		}
		if (slotId >= 0 && slotId < this.InventoryArray.Count)
		{
			if (count == 0 || this.InventoryArray[slotId].Count <= count)
			{
				this.InventoryArray[slotId].Item = null;
				this.InventoryArray[slotId].Count = 0;
			}
			else
			{
				this.InventoryArray[slotId].Count -= count;
			}
			this.Show();
		}
	}

	public void Use(PlayerStats stats, Craft craft)
	{
		if (this.ChoosenSlot >= 0 && this.ChoosenSlot < this.InventoryArray.Count)
		{
			Slot slot = this.InventoryArray[this.ChoosenSlot];
			if (slot.Item != null)
			{
				if (Inventory.IsHealing(slot.Item))
				{
					string id = slot.Item.Id;
					switch (id)
					{
					case "bandage":
						if (stats.AddHealth(DamageType.Arms, (float)(slot.Item as Healing).Power))
						{
							this.Destroy(1, -1);
						}
						break;
					case "medbag":
						if (stats.AddHealth(DamageType.Body, (float)(slot.Item as Healing).Power))
						{
							this.Destroy(1, -1);
						}
						break;
					case "boostspeed":
						this.buffs.AddBuff(24, 30f);
						this.Destroy(1, -1);
						break;
					case "boostregen":
						this.buffs.AddBuff(23, 30f);
						this.Destroy(1, -1);
						break;
					case "chloroform":
						base.GetComponent<PlayerStats>().CurrentHead = 0f;
						this.Destroy(1, -1);
						break;
					case "boostjump":
						this.buffs.AddBuff(22, 30f);
						this.Destroy(1, -1);
						break;
					case "feethealing":
						if (stats.AddHealth(DamageType.Legs, (float)(slot.Item as Healing).Power))
						{
							this.Destroy(1, -1);
						}
						break;
					case "painkillers":
						if (stats.AddHealth(DamageType.Head, (float)(slot.Item as Healing).Power))
						{
							this.Destroy(1, -1);
						}
						break;
					}
				}
				else if (Inventory.IsBlueprint(slot.Item))
				{
					if (craft.AddBlueprint((slot.Item as Blueprint).Info))
					{
						this.Destroy(1, -1);
					}
				}
				else if (Inventory.IsFood(slot.Item))
				{
					if (slot.Item.Id == "lootbox")
					{
						if (this.Add(new Slot(Database.GetRandomItem()), UnityEngine.Random.Range(1, 11)))
						{
							this.Destroy(1, -1);
						}
					}
					else
					{
						bool flag = false || stats.AddFood((float)(slot.Item as Food).FoodPower * slot.CurrentDurability / (slot.Item as Food).FoodDurability) || stats.AddWater((float)(slot.Item as Food).WaterPower * slot.CurrentDurability / (slot.Item as Food).FoodDurability) || stats.AddStamina((float)(slot.Item as Food).StaminaPower * slot.CurrentDurability / (slot.Item as Food).FoodDurability);
						if (flag)
						{
							if (!(slot.Item as Food).HealthyFood)
							{
								stats.FoodReductionK += 0.05f;
							}
							this.Destroy(1, -1);
						}
					}
				}
			}
			if (this.player.DraggingSlot != null)
			{
				this.player.DraggingSlot.Count = slot.Count;
			}
			this.player.links.DragIcon.transform.GetChild(1).GetComponent<Text>().text = "x" + slot.Count;
			if (slot.Count <= 0)
			{
				this.player.StopDrag();
			}
			this.Show();
		}
	}

	public void Move(Inventory target)
	{
		Slot slot = new Slot(null)
		{
			Count = this.InventoryArray[this.ChoosenSlot].Count,
			Item = this.InventoryArray[this.ChoosenSlot].Item,
			CurrentClipSize = this.InventoryArray[this.ChoosenSlot].CurrentClipSize,
			CurrentDurability = this.InventoryArray[this.ChoosenSlot].CurrentDurability
		};
		if (target.Add(slot, slot.Count))
		{
			this.InventoryArray[this.ChoosenSlot].Item = null;
			this.Show();
			target.Show();
		}
	}

	public bool Add(Slot slot, int count)
	{
		bool flag = false;
		int num = 1;
		if (slot.Item != null)
		{
			if (!Inventory.IsCloth(slot.Item) && this.InvType == InventoryType.Cloth)
			{
				return false;
			}
			if (Inventory.IsWeapon(slot.Item) || Inventory.IsTool(slot.Item) || Inventory.IsCloth(slot.Item))
			{
				num = count;
			}
			for (int i = 0; i < num; i++)
			{
				bool flag2 = false;
				for (int j = 0; j < this.InventoryArray.Count; j++)
				{
					if (this.InventoryArray[j].Item == slot.Item && slot.Item.CanStack && this.InventoryArray[j].Count < Database.StackSize)
					{
						this.InventoryArray[j].Count += count;
						flag = true;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					for (int k = 0; k < this.InventoryArray.Count; k++)
					{
						if (this.InventoryArray[k].Item == null)
						{
							this.InventoryArray[k].CurrentClipSize = slot.CurrentClipSize;
							this.InventoryArray[k].CurrentDurability = slot.CurrentDurability;
							if (Inventory.IsWeapon(slot.Item) || Inventory.IsTool(slot.Item))
							{
								this.InventoryArray[k].Count = 1;
							}
							else
							{
								this.InventoryArray[k].Count = count;
							}
							this.InventoryArray[k].Item = slot.Item;
							flag = true;
							break;
						}
					}
				}
			}
		}
		if (flag && (this.InvType == InventoryType.MainInventory || this.InvType == InventoryType.Toolbet))
		{
			this.quests.AddToInventory(slot.Item, count);
		}
		return flag;
	}

	public bool Add(string id, int count)
	{
		foreach (Item current in Database.Items)
		{
			if (current.Id == id)
			{
				return this.Add(new Slot(current), count);
			}
		}
		return false;
	}

	private void AddLoot()
	{
		this.config = Database.GetChestConfig(this.ChestID);
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
		this.capacity = UnityEngine.Random.Range(this.MinSlots, this.MaxSlots + 1);
		for (int j = 0; j < this.capacity; j++)
		{
			this.InventoryArray.Add(new Slot(null));
			Slot slot2 = list[UnityEngine.Random.Range(0, list.Count)];
			this.Add(slot2, slot2.Count);
		}
	}

	private void EverySecond()
	{
		if (this.FoodRot)
		{
			for (int i = 0; i < this.InventoryArray.Count; i++)
			{
				if (Inventory.IsFood(this.InventoryArray[i].Item) && (this.InventoryArray[i].Item as Food).CanRot)
				{
					this.InventoryArray[i].CurrentDurability -= Database.RottingPerSecond;
				}
			}
		}
	}

	public static bool IsHealing(Item item)
	{
		return item != null && item.ItemType == ItemType.Healing;
	}

	public static bool IsCloth(Item item)
	{
		return item != null && item.ItemType == ItemType.Cloth;
	}

	public static bool IsFood(Item item)
	{
		return item != null && item.ItemType == ItemType.Food;
	}

	public static bool IsResource(Item item)
	{
		return item != null && item.ItemType == ItemType.Resource;
	}

	public static bool IsTool(Item item)
	{
		return item != null && item.ItemType == ItemType.Tool;
	}

	public static bool IsWeapon(Item item)
	{
		return item != null && item.ItemType == ItemType.Weapon;
	}

	public static bool IsBlueprint(Item item)
	{
		return item != null && item.ItemType == ItemType.Blueprint;
	}

	public static bool IsBuilding(Item item)
	{
		return item != null && item.ItemType == ItemType.Building;
	}

	public static bool IsAmmo(Item item)
	{
		return item != null && item.ItemType == ItemType.Ammo;
	}
}
