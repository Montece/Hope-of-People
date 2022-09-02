using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class Player : MonoBehaviour
{
	private class AnotherSlot
	{
		public string Item;

		public int Count;
	}

	[CompilerGenerated]
	[Serializable]
	private sealed class <>c
	{
		public static readonly Player.<>c <>9 = new Player.<>c();

		public static UnityAction <>9__24_0;

		public static UnityAction <>9__24_1;

		public static UnityAction <>9__24_2;

		public static UnityAction <>9__24_3;

		public static UnityAction <>9__24_4;

		public static UnityAction <>9__24_5;

		public static UnityAction<string> <>9__24_6;

		public static UnityAction <>9__24_7;

		internal void <DoInjection>b__24_0()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().Drop2();
		}

		internal void <DoInjection>b__24_1()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().player.Use();
		}

		internal void <DoInjection>b__24_2()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(1);
		}

		internal void <DoInjection>b__24_3()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(10);
		}

		internal void <DoInjection>b__24_4()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(-1);
		}

		internal void <DoInjection>b__24_5()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(-10);
		}

		internal void <DoInjection>b__24_6(string a)
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.SetCount();
		}

		internal void <DoInjection>b__24_7()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.TryCraft();
		}
	}

	[CompilerGenerated]
	[Serializable]
	private sealed class <>c__0
	{
		public static readonly Player.<>c__0 <>9 = new Player.<>c__0();

		public static UnityAction <>9__0_0;

		public static UnityAction <>9__0_1;

		public static UnityAction <>9__0_2;

		public static UnityAction <>9__0_3;

		public static UnityAction <>9__0_4;

		public static UnityAction <>9__0_5;

		public static UnityAction<string> <>9__0_6;

		public static UnityAction <>9__0_7;

		internal void <DoInjection>b__0_0()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().Drop2();
		}

		internal void <DoInjection>b__0_1()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().player.Use();
		}

		internal void <DoInjection>b__0_2()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(1);
		}

		internal void <DoInjection>b__0_3()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(10);
		}

		internal void <DoInjection>b__0_4()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(-1);
		}

		internal void <DoInjection>b__0_5()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.AddCount(-10);
		}

		internal void <DoInjection>b__0_6(string a)
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.SetCount();
		}

		internal void <DoInjection>b__0_7()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().craft.TryCraft();
		}
	}

	[HideInInspector]
	public Inventory Toolbet;

	[HideInInspector]
	public Inventory MainInventory;

	[HideInInspector]
	public Inventory Cloths;

	public Transform ToolsHolder;

	public GameObject DropZone;

	private Ray ray;

	private RaycastHit hit;

	public bool CanUseTools = true;

	public bool CanShoot = true;

	[HideInInspector]
	public int EquipedToolbet = -1;

	public int ArmRange = 3;

	public int BuildingRange = 8;

	public float ToolsSpeed = 1f;

	public float WeaponDamageK = 1f;

	[Space]
	public KeyCode InventoryButton = KeyCode.Tab;

	public KeyCode PickupItemButton = KeyCode.F;

	public KeyCode Shoot = KeyCode.Mouse0;

	public KeyCode Aim = KeyCode.Mouse1;

	public KeyCode Reload = KeyCode.R;

	public KeyCode Crouch = KeyCode.C;

	public KeyCode Slowing = KeyCode.LeftControl;

	public KeyCode Piss = KeyCode.P;

	public KeyCode Save = KeyCode.F5;

	public KeyCode Load = KeyCode.F9;

	public KeyCode GetIntoCar = KeyCode.T;

	public KeyCode ClimbUp = KeyCode.W;

	public KeyCode ClimbDown = KeyCode.S;

	public AudioClip PickupSound;

	[HideInInspector]
	public bool IsDragging;

	public Slot DraggingSlot;

	[HideInInspector]
	public Inventory DraggingInventory;

	[HideInInspector]
	public GameObject MechObject;

	public TOD_Sky sky;

	public gWeatherMaster weather;

	[HideInInspector]
	public Craft craft;

	private BuildingSystem build;

	private PlayerStats stats;

	private AudioSource source;

	[HideInInspector]
	public GameManager manager;

	public SceneLinks links;

	private const string fmt1 = "0#";

	private const string fmt2 = "000#";

	private bool doneInjection;

	public void OnStart(SceneLinks links)
	{
		this.manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		this.links = links;
		this.stats = this.manager.GetComponent<PlayerStats>();
		this.craft = this.manager.GetComponent<Craft>();
		this.build = base.GetComponent<BuildingSystem>();
		this.MainInventory = this.manager.GetComponents<Inventory>()[0];
		this.Toolbet = this.manager.GetComponents<Inventory>()[1];
		this.Cloths = this.manager.GetComponents<Inventory>()[2];
		this.source = base.GetComponents<AudioSource>()[0];
		this.EquipedToolbet = -1;
		this.StopDrag();
		this.MainInventory.player = this;
		this.Toolbet.player = this;
		if (links.InventoryPanel != null)
		{
			links.InventoryPanel.SetActive(false);
			base.GetComponentInChildren<Blur>().enabled = false;
		}
		this.craft.currentCrafts = Database.CurrentCrafts;
		this.craft.RefreshList(0);
		base.gameObject.AddComponent<Iden>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.InventoryButton))
		{
			this.ShowHideInventory();
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) && !this.links.InventoryPanel.activeSelf)
		{
			this.UseToolbet(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && !this.links.InventoryPanel.activeSelf)
		{
			this.UseToolbet(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && !this.links.InventoryPanel.activeSelf)
		{
			this.UseToolbet(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && !this.links.InventoryPanel.activeSelf)
		{
			this.UseToolbet(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && !this.links.InventoryPanel.activeSelf)
		{
			this.UseToolbet(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6) && !this.links.InventoryPanel.activeSelf)
		{
			this.UseToolbet(5);
		}
		if (Input.GetKeyDown(this.Piss))
		{
			this.stats.Piss();
		}
		if (Input.GetKeyDown(this.Save))
		{
			this.manager.Save();
		}
		Input.GetKeyDown(this.Load);
		if (this.IsDragging)
		{
			this.links.DragIcon.transform.position = Input.mousePosition - new Vector3(this.links.DragIcon.GetComponent<RectTransform>().sizeDelta.x / 2f + 5f, this.links.DragIcon.GetComponent<RectTransform>().sizeDelta.y / 2f + 5f);
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
			{
				this.StopDrag();
			}
		}
		if (this.sky != null)
		{
			this.links.DateTime.text = string.Concat(new object[]
			{
				this.sky.Cycle.Year.ToString("000#"),
				".",
				this.sky.Cycle.Month.ToString("0#"),
				".",
				this.sky.Cycle.Day.ToString("0#"),
				"       ",
				((int)this.sky.Cycle.Hour).ToString("0#"),
				" hours       ",
				this.weather.fTemperatureC,
				" C"
			});
		}
		this.links.Uitext.text = string.Empty;
		this.links.UitextObject.SetActive(false);
		this.links.DurabilityText.text = string.Empty;
		this.links.DurabilityTextObject.SetActive(false);
		this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
		if (Physics.Raycast(this.ray, out this.hit, (float)this.ArmRange))
		{
			if (this.hit.collider.GetComponent<Pickup>())
			{
				Pickup component = this.hit.collider.GetComponent<Pickup>();
				this.links.Uitext.text = string.Concat(new object[]
				{
					component.slot.Count,
					" ",
					component.slot.Item.Title,
					" (",
					this.PickupItemButton,
					")"
				});
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild && this.MainInventory.Add(component.slot, component.slot.Count))
				{
					this.source.clip = this.PickupSound;
					this.source.Play();
					this.MainInventory.Show();
					UnityEngine.Object.Destroy(this.hit.collider.gameObject);
				}
			}
			else if (this.hit.collider.GetComponent<Chest>() && this.hit.collider.GetComponent<Chest>().enabled && this.hit.collider.tag != "Player")
			{
				this.links.Uitext.text = "Open (" + this.PickupItemButton + ")";
				this.ShowChestPanel();
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					this.ShowHideInventory();
					this.hit.collider.GetComponent<Inventory>().Show();
					this.hit.collider.GetComponent<Inventory>().player = this;
					this.MechObject = this.hit.collider.gameObject;
				}
			}
			else if (this.hit.collider.GetComponent<Forge>())
			{
				this.links.Uitext.text = "Open forge (" + this.PickupItemButton + ")";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					this.ShowHideInventory();
					this.MechObject = this.hit.collider.gameObject;
					this.hit.collider.GetComponent<Forge>().Show(this);
					this.ShowForgePanel();
				}
			}
			else if (this.hit.collider.GetComponent<Campfire>())
			{
				this.links.Uitext.text = "Open campfire (" + this.PickupItemButton + ")";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					this.MechObject = this.hit.collider.gameObject;
					this.hit.collider.GetComponent<Campfire>().openedPlayer = this;
					this.hit.collider.GetComponent<Campfire>().Show();
					this.ShowCampfirePanel();
					this.ShowHideInventory();
				}
			}
			else if (this.hit.collider.GetComponent<Crusher>())
			{
				this.links.Uitext.text = "Open crusher (" + this.PickupItemButton + ")";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					this.ShowHideInventory();
					this.MechObject = this.hit.collider.gameObject;
					this.hit.collider.GetComponent<Crusher>().Show(this);
					this.ShowCrusherPanel();
				}
			}
			else if (this.hit.collider.GetComponent<HoloScreen>())
			{
				this.links.Uitext.text = "Open Control Panel (" + this.PickupItemButton + ")";
				this.ShowChestPanel();
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					this.hit.collider.GetComponent<Inventory>().Show();
					this.hit.collider.GetComponent<Inventory>().player = this;
					this.MechObject = this.hit.collider.gameObject;
					this.ShowHideInventory();
				}
			}
			else if (this.hit.collider.GetComponent<PickupableObject>())
			{
				this.links.Uitext.text = "Pickup (" + this.PickupItemButton + ")";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					bool flag = false;
					if (this.hit.collider.GetComponent<Plant>() != null)
					{
						if (this.hit.collider.GetComponent<Plant>().CurrentGrowTime == this.hit.collider.GetComponent<Plant>().GrowTimeInSeconds)
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
						PickupObject[] itemsIds = this.hit.collider.GetComponent<PickupableObject>().ItemsIds;
						for (int i = 0; i < itemsIds.Length; i++)
						{
							PickupObject pickupObject = itemsIds[i];
							this.MainInventory.Add(new Slot(Database.Get(pickupObject.Item)), UnityEngine.Random.Range(pickupObject.MinCount, pickupObject.MaxCount + 1));
						}
						this.source.clip = this.PickupSound;
						this.source.Play();
						UnityEngine.Object.Destroy(this.hit.collider.gameObject);
					}
				}
			}
			else if (this.hit.collider.GetComponent<Door>())
			{
				this.links.Uitext.text = "Open door (" + this.PickupItemButton + ")";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton))
				{
					this.hit.collider.GetComponent<Door>().Open();
				}
			}
			else if (this.hit.collider.GetComponent<Plant>())
			{
				this.links.Uitext.text = "Progress: " + (int)(this.hit.collider.GetComponent<Plant>().CurrentGrowTime / this.hit.collider.GetComponent<Plant>().GrowTimeInSeconds * 100f) + "%";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton))
				{
					this.hit.collider.GetComponent<Door>().Open();
				}
			}
			else if (this.hit.collider.GetComponent<Bed>())
			{
				this.links.Uitext.text = "Sleep (" + this.PickupItemButton + ")...";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					SceneManager.LoadScene("World");
				}
			}
			else if (this.hit.collider.GetComponent<Keyboard>())
			{
				this.links.Uitext.text = "Use (" + this.PickupItemButton + ")";
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.PickupItemButton) && !this.build.CanBuild)
				{
					this.hit.collider.GetComponent<Keyboard>().Use();
				}
			}
			if (this.hit.collider.GetComponent<Construction>())
			{
				this.links.DurabilityText.text = this.hit.collider.GetComponent<Construction>().CurrentHealth.ToString() + "/" + this.hit.collider.GetComponent<Construction>().MaximumHealth.ToString();
				this.links.DurabilityTextObject.SetActive(true);
			}
			if (this.hit.collider.GetComponent<Car>())
			{
				Text expr_C58 = this.links.Uitext;
				string text = expr_C58.text;
				expr_C58.text = string.Concat(new object[]
				{
					text,
					" Drive (",
					this.GetIntoCar,
					")"
				});
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.GetIntoCar))
				{
					this.links.Uitext.text = string.Empty;
					this.links.DurabilityText.text = string.Empty;
					this.links.DurabilityTextObject.SetActive(false);
					this.links.UitextObject.SetActive(false);
					this.hit.collider.GetComponent<Car>().On(base.gameObject);
				}
			}
			if (this.hit.collider.GetComponent<HelicopterController>())
			{
				Text expr_D3D = this.links.Uitext;
				string text2 = expr_D3D.text;
				expr_D3D.text = string.Concat(new object[]
				{
					text2,
					" Fly (",
					this.GetIntoCar,
					")"
				});
				this.links.UitextObject.SetActive(true);
				if (Input.GetKeyDown(this.GetIntoCar))
				{
					this.links.Uitext.text = string.Empty;
					this.links.DurabilityText.text = string.Empty;
					this.links.DurabilityTextObject.SetActive(false);
					this.links.UitextObject.SetActive(false);
					this.hit.collider.GetComponent<HelicopterController>().On(base.gameObject);
				}
			}
		}
	}

	public bool HasAmmo(Weapon weapon, ref int ammoSlotId, ref Inventory owner)
	{
		bool result = false;
		ammoSlotId = -1;
		if (weapon != null)
		{
			for (int i = 0; i < this.MainInventory.InventoryArray.Count; i++)
			{
				if (Inventory.IsAmmo(this.MainInventory.InventoryArray[i].Item) && this.MainInventory.InventoryArray[i].Item as Ammo == weapon.Ammo)
				{
					result = true;
					ammoSlotId = i;
					owner = this.MainInventory;
					break;
				}
			}
			for (int j = 0; j < this.Toolbet.InventoryArray.Count; j++)
			{
				if (Inventory.IsAmmo(this.Toolbet.InventoryArray[j].Item) && this.Toolbet.InventoryArray[j].Item as Ammo == weapon.Ammo)
				{
					result = true;
					ammoSlotId = j;
					owner = this.Toolbet;
					break;
				}
			}
		}
		return result;
	}

	public bool TryCraft(CraftInfo info, int count)
	{
		List<Player.AnotherSlot> list = new List<Player.AnotherSlot>();
		for (int i = 0; i < this.MainInventory.InventoryArray.Count; i++)
		{
			if (this.MainInventory.InventoryArray[i].Item != null)
			{
				list.Add(new Player.AnotherSlot
				{
					Item = this.MainInventory.InventoryArray[i].Item.Id,
					Count = this.MainInventory.InventoryArray[i].Count
				});
			}
			else
			{
				list.Add(new Player.AnotherSlot
				{
					Item = string.Empty,
					Count = 0
				});
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			for (int k = 0; k < list.Count; k++)
			{
				if (list[j].Item != string.Empty && list[j].Item == list[k].Item && j != k)
				{
					list[j].Count += list[k].Count;
					list[k].Count = 0;
					list[k].Item = string.Empty;
				}
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			for (int m = 0; m < info.NeedResources.Length; m++)
			{
				if (list[l].Item != string.Empty && info.NeedResources[m].Item.Id == list[l].Item)
				{
					list[l].Count -= (int)Mathf.Ceil((float)(info.NeedResources[m].Count * count) / (float)info.Result.Count);
				}
			}
		}
		bool flag = true;
		foreach (Player.AnotherSlot current in list)
		{
			if (current.Item != string.Empty && current.Count < 0)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			List<int> list2 = new List<int>();
			for (int n = 0; n < info.NeedResources.Length; n++)
			{
				list2.Add((int)Mathf.Ceil((float)(info.NeedResources[n].Count * count) / (float)info.Result.Count));
			}
			for (int num = 0; num < this.MainInventory.InventoryArray.Count; num++)
			{
				for (int num2 = 0; num2 < list2.Count; num2++)
				{
					if (this.MainInventory.InventoryArray[num].Item != null && info.NeedResources[num2].Item.Id == this.MainInventory.InventoryArray[num].Item.Id)
					{
						if (this.MainInventory.InventoryArray[num].Count >= list2[num2])
						{
							this.MainInventory.InventoryArray[num].Count -= list2[num2];
							list2[num2] = 0;
						}
						else
						{
							List<int> list3;
							int index;
							(list3 = list2)[index = num2] = list3[index] - this.MainInventory.InventoryArray[num].Count;
							this.MainInventory.InventoryArray[num].Count = 0;
						}
					}
				}
			}
			flag = true;
			using (List<int>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current != 0)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag && this.MainInventory.Add(new Slot(info.Result.Item), count))
			{
				this.MainInventory.Show();
				return true;
			}
		}
		return false;
	}

	public void SubstractToolDurability(int count)
	{
		this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].CurrentDurability -= (float)count;
		this.Toolbet.Show();
	}

	public void SubstractWeaponDurability(int count)
	{
		this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].CurrentDurability -= (float)count;
		this.Toolbet.Show();
	}

	public void UnequipAllToolbet()
	{
		IEnumerator enumerator = this.ToolsHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				((Transform)enumerator.Current).gameObject.SetActive(false);
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
	}

	public void ShowHideInventory()
	{
		if (this.links.InventoryPanel != null)
		{
			this.links.InventoryPanel.SetActive(!this.links.InventoryPanel.activeSelf);
		}
		if (this.links.InventoryPanel.activeSelf)
		{
			this.MainInventory.Show();
			this.Toolbet.Show();
			this.Cloths.Show();
			this.ShowCraftPanel();
			base.GetComponentInChildren<Blur>().enabled = true;
			base.GetComponentInParent<FirstPersonController>().enabled = false;
			base.GetComponentInParent<CharacterController>().enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.CanUseTools = false;
			this.CanShoot = false;
			this.StopDrag();
			if (!this.doneInjection)
			{
				this.DoInjection();
				this.doneInjection = true;
			}
			return;
		}
		this.Cloths.Hide();
		base.GetComponentInChildren<Blur>().enabled = false;
		base.GetComponentInParent<FirstPersonController>().enabled = true;
		base.GetComponentInParent<CharacterController>().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.CanUseTools = true;
		this.CanShoot = true;
		if (this.MechObject != null)
		{
			if (this.MechObject.GetComponents<Inventory>() != null)
			{
				Inventory[] components = this.MechObject.GetComponents<Inventory>();
				for (int i = 0; i < components.Length; i++)
				{
					Inventory inventory = components[i];
					if (inventory != null)
					{
						inventory.Hide();
					}
				}
			}
			if (this.MechObject.GetComponent<Campfire>() != null)
			{
				this.MechObject.GetComponent<Campfire>().openedPlayer = null;
			}
		}
		this.MechObject = null;
	}

	public void SlotClicked(Slot slot, Inventory inv)
	{
		if (slot.Item != null)
		{
			this.links.ItemsDescrition.gameObject.SetActive(true);
			this.links.ButtonDrop.SetActive(true);
			this.links.ItemsDescrition.GetChild(1).GetComponent<Text>().text = slot.Item.Title;
			this.links.ItemsDescrition.GetChild(2).GetComponent<Image>().sprite = slot.Item.Icon;
			string newLine = Environment.NewLine;
			string text = string.Concat(new object[]
			{
				"Can stack: ",
				slot.Item.CanStack,
				newLine,
				"Item type: ",
				slot.Item.ItemType
			});
			this.links.ItemsDescrition.GetChild(3).GetComponent<Text>().text = text;
			this.DraggingInventory = inv;
			this.StartDrag(slot);
		}
	}

	private void StartDrag(Slot slot)
	{
		if (slot == this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot])
		{
			this.UnequipAllToolbet();
		}
		this.links.DragIcon.SetActive(true);
		this.links.DragIcon.transform.GetChild(0).GetComponent<Image>().sprite = slot.Item.Icon;
		this.links.DragIcon.transform.GetChild(1).GetComponent<Text>().text = "x" + slot.Count;
		this.IsDragging = true;
		this.DraggingSlot = slot;
		this.links.DropCountField.text = this.DraggingSlot.Count.ToString();
	}

	public void StopDrag()
	{
		this.links.DragIcon.SetActive(false);
		this.links.DragIcon.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemIcons/None");
		this.IsDragging = false;
		this.DraggingSlot = null;
		this.links.ItemsDescrition.gameObject.SetActive(false);
		this.links.ButtonDrop.SetActive(false);
		this.links.ItemsDescrition.GetChild(1).GetComponent<Text>().text = string.Empty;
		this.links.ItemsDescrition.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemIcons/None");
		this.links.ItemsDescrition.GetChild(3).GetComponent<Text>().text = string.Empty;
	}

	private void UseToolbet(int id)
	{
		this.Toolbet.ChoosenSlot = id;
		if (id >= 0 && this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item != null)
		{
			if (Inventory.IsBuilding(this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item))
			{
				if (this.EquipedToolbet == id)
				{
					this.build.Cancel();
					this.EquipedToolbet = -1;
					return;
				}
				this.UnequipAllToolbet();
				this.build.Cancel();
				this.build.StartBuild((this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item as Building).Prefab);
				this.EquipedToolbet = id;
				return;
			}
			else
			{
				if (Inventory.IsBlueprint(this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item))
				{
					this.Toolbet.Use(this.stats, this.craft);
					return;
				}
				if (Inventory.IsFood(this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item))
				{
					this.Toolbet.Use(this.stats, this.craft);
					return;
				}
				if (Inventory.IsHealing(this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item))
				{
					this.Toolbet.Use(this.stats, this.craft);
					return;
				}
				if (Inventory.IsWeapon(this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item) || Inventory.IsTool(this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item))
				{
					if (this.EquipedToolbet == id)
					{
						IEnumerator enumerator = this.ToolsHolder.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								Transform transform = (Transform)enumerator.Current;
								if (transform.GetComponent<ToolObject>().id == this.Toolbet.InventoryArray[this.Toolbet.ChoosenSlot].Item.Id)
								{
									transform.gameObject.SetActive(!transform.gameObject.activeSelf);
								}
							}
							return;
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
					this.EquipToolbet(id);
				}
			}
		}
	}

	public void GetDamage(DamageType type, float amount)
	{
		this.manager.stats.GetDamage(type, amount);
	}

	private void EquipToolbet(int id)
	{
		this.EquipedToolbet = id;
		this.UnequipAllToolbet();
		IEnumerator enumerator = this.ToolsHolder.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.name == this.Toolbet.InventoryArray[id].Item.Id.ToString())
				{
					transform.gameObject.SetActive(true);
				}
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
	}

	public void Drop(int count)
	{
		if (this.DraggingSlot != null)
		{
			this.DraggingInventory.Drop(this.DropZone, count);
			this.StopDrag();
			this.DraggingInventory.Show();
		}
	}

	public void Use()
	{
		if (this.DraggingSlot != null)
		{
			this.DraggingInventory.Use(this.stats, this.craft);
			this.DraggingInventory.Show();
		}
	}

	public void Toggle(int type)
	{
		if (this.MechObject != null)
		{
			if (type != 1)
			{
				if (type == 0)
				{
					if (this.MechObject.GetComponent<Campfire>().ToggleCampfire())
					{
						this.links.ToggleButtonText2.GetComponentInChildren<Text>().color = Color.green;
						return;
					}
					this.links.ToggleButtonText2.GetComponentInChildren<Text>().color = Color.red;
					return;
				}
			}
			else
			{
				if (this.MechObject.GetComponent<Forge>().ToggleForge())
				{
					this.links.ToggleButtonText1.GetComponentInChildren<Text>().color = Color.green;
					return;
				}
				this.links.ToggleButtonText1.GetComponentInChildren<Text>().color = Color.red;
			}
		}
	}

	private void ShowForgePanel()
	{
		this.links.CraftPanel.SetActive(false);
		this.links.ForgePanel.SetActive(true);
		this.links.ChestPanel.SetActive(false);
		this.links.CampfirePanel.SetActive(false);
		this.links.CrusherPanel.SetActive(false);
	}

	private void ShowCraftPanel()
	{
		this.links.CraftPanel.SetActive(true);
		this.links.ForgePanel.SetActive(false);
		this.links.ChestPanel.SetActive(false);
		this.links.CampfirePanel.SetActive(false);
		this.links.CrusherPanel.SetActive(false);
	}

	public void HidePanels()
	{
		this.links.CraftPanel.SetActive(false);
		this.links.ForgePanel.SetActive(false);
		this.links.ChestPanel.SetActive(false);
		this.links.CampfirePanel.SetActive(false);
		this.links.CrusherPanel.SetActive(false);
	}

	private void ShowCampfirePanel()
	{
		this.links.CraftPanel.SetActive(false);
		this.links.ForgePanel.SetActive(false);
		this.links.ChestPanel.SetActive(false);
		this.links.CampfirePanel.SetActive(true);
		this.links.CrusherPanel.SetActive(false);
	}

	private void ShowCrusherPanel()
	{
		this.links.CraftPanel.SetActive(false);
		this.links.ForgePanel.SetActive(false);
		this.links.ChestPanel.SetActive(false);
		this.links.CrusherPanel.SetActive(true);
		this.links.CampfirePanel.SetActive(false);
	}

	private void ShowChestPanel()
	{
		this.links.CraftPanel.SetActive(false);
		this.links.ForgePanel.SetActive(false);
		this.links.ChestPanel.SetActive(true);
		this.links.CampfirePanel.SetActive(false);
		this.links.CrusherPanel.SetActive(false);
	}

	private void DoInjection()
	{
		UnityEvent arg_33_0 = GameObject.Find("UI/Inventory/ItemDescription/Drop").GetComponent<Button>().onClick;
		UnityAction arg_33_1;
		if ((arg_33_1 = Player.<>c__0.<>9__0_0) == null)
		{
			arg_33_1 = (Player.<>c__0.<>9__0_0 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_0));
		}
		arg_33_0.AddListener(arg_33_1);
		UnityEvent arg_6B_0 = GameObject.Find("UI/Inventory/ItemDescription/Use").GetComponent<Button>().onClick;
		UnityAction arg_6B_1;
		if ((arg_6B_1 = Player.<>c__0.<>9__0_1) == null)
		{
			arg_6B_1 = (Player.<>c__0.<>9__0_1 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_1));
		}
		arg_6B_0.AddListener(arg_6B_1);
		GameObject.Find("UI/Inventory/ItemDescription/Count").GetComponent<InputField>().interactable = true;
		UnityEvent arg_B8_0 = GameObject.Find("UI/Inventory/CraftDescription/Add1").GetComponent<Button>().onClick;
		UnityAction arg_B8_1;
		if ((arg_B8_1 = Player.<>c__0.<>9__0_2) == null)
		{
			arg_B8_1 = (Player.<>c__0.<>9__0_2 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_2));
		}
		arg_B8_0.AddListener(arg_B8_1);
		UnityEvent arg_F0_0 = GameObject.Find("UI/Inventory/CraftDescription/Add2").GetComponent<Button>().onClick;
		UnityAction arg_F0_1;
		if ((arg_F0_1 = Player.<>c__0.<>9__0_3) == null)
		{
			arg_F0_1 = (Player.<>c__0.<>9__0_3 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_3));
		}
		arg_F0_0.AddListener(arg_F0_1);
		UnityEvent arg_128_0 = GameObject.Find("UI/Inventory/CraftDescription/Substract1").GetComponent<Button>().onClick;
		UnityAction arg_128_1;
		if ((arg_128_1 = Player.<>c__0.<>9__0_4) == null)
		{
			arg_128_1 = (Player.<>c__0.<>9__0_4 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_4));
		}
		arg_128_0.AddListener(arg_128_1);
		UnityEvent arg_160_0 = GameObject.Find("UI/Inventory/CraftDescription/Substract2").GetComponent<Button>().onClick;
		UnityAction arg_160_1;
		if ((arg_160_1 = Player.<>c__0.<>9__0_5) == null)
		{
			arg_160_1 = (Player.<>c__0.<>9__0_5 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_5));
		}
		arg_160_0.AddListener(arg_160_1);
		UnityEvent<string> arg_198_0 = GameObject.Find("UI/Inventory/CraftDescription/Count").GetComponent<InputField>().onEndEdit;
		UnityAction<string> arg_198_1;
		if ((arg_198_1 = Player.<>c__0.<>9__0_6) == null)
		{
			arg_198_1 = (Player.<>c__0.<>9__0_6 = new UnityAction<string>(Player.<>c__0.<>9.<DoInjection>b__0_6));
		}
		arg_198_0.AddListener(arg_198_1);
		UnityEvent arg_1D0_0 = GameObject.Find("UI/Inventory/CraftDescription/Craft").GetComponent<Button>().onClick;
		UnityAction arg_1D0_1;
		if ((arg_1D0_1 = Player.<>c__0.<>9__0_7) == null)
		{
			arg_1D0_1 = (Player.<>c__0.<>9__0_7 = new UnityAction(Player.<>c__0.<>9.<DoInjection>b__0_7));
		}
		arg_1D0_0.AddListener(arg_1D0_1);
	}

	private void GetSceneStructure(Transform entry, ref string all, int depth)
	{
		all = all + "\n" + new string(' ', depth) + entry.gameObject.name;
		foreach (Transform entry2 in entry)
		{
			this.GetSceneStructure(entry2, ref all, depth + 1);
		}
	}
}
