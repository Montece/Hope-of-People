using System;
using UnityEngine;

public class SlotObject : MonoBehaviour
{
	public int SlotID;

	public Inventory owner;

	public ArmorSlot type;

	public void Clicked()
	{
		Slot slot = this.owner.InventoryArray[this.SlotID];
		this.owner.ChoosenSlot = this.SlotID;
		this.owner.player.craft.HidePanel();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			InventoryType invType = this.owner.InvType;
			switch (invType)
			{
			case InventoryType.MainInventory:
				if (this.owner.player.Toolbet.Add(slot, slot.Count))
				{
					this.owner.player.UnequipAllToolbet();
					this.owner.InventoryArray[this.SlotID].Item = null;
					this.owner.player.Toolbet.Show();
					this.owner.Show();
				}
				goto IL_2F5;
			case InventoryType.Toolbet:
				if (this.owner.player.MainInventory.Add(slot, slot.Count))
				{
					this.owner.player.UnequipAllToolbet();
					this.owner.InventoryArray[this.SlotID].Item = null;
					this.owner.player.MainInventory.Show();
					this.owner.Show();
				}
				goto IL_2F5;
			case InventoryType.Chest:
				if (this.owner.player.MainInventory.Add(slot, slot.Count))
				{
					this.owner.InventoryArray[this.SlotID].Item = null;
					this.owner.player.MainInventory.Show();
					this.owner.Show();
				}
				goto IL_2F5;
			case InventoryType.Campfire:
				if (this.owner.player.MainInventory.Add(slot, slot.Count))
				{
					this.owner.InventoryArray[this.SlotID].Item = null;
					this.owner.player.MainInventory.Show();
					this.owner.Show();
				}
				goto IL_2F5;
			case InventoryType.Crusher1:
			case InventoryType.Crusher2:
				IL_7E:
				if (invType != InventoryType.Forge1)
				{
					goto IL_2F5;
				}
				if (this.owner.player.MainInventory.Add(slot, slot.Count))
				{
					this.owner.InventoryArray[this.SlotID].Item = null;
					this.owner.player.MainInventory.Show();
					this.owner.Show();
				}
				goto IL_2F5;
			case InventoryType.Cloth:
				if (this.owner.player.MainInventory.Add(slot, slot.Count))
				{
					this.owner.InventoryArray[this.SlotID].Item = null;
					this.owner.player.MainInventory.Show();
					this.owner.Show();
				}
				goto IL_2F5;
			}
			goto IL_7E;
			IL_2F5:;
		}
		else if (this.owner.player.IsDragging)
		{
			if (slot.Item == null)
			{
				if (this.owner.InvType == InventoryType.Cloth && !Inventory.IsCloth(this.owner.player.DraggingSlot.Item))
				{
					return;
				}
				if (Inventory.IsCloth(this.owner.player.DraggingSlot.Item) && this.owner.InvType == InventoryType.Cloth && this.type != (this.owner.player.DraggingSlot.Item as Cloth).Slot)
				{
					return;
				}
				slot.Count = this.owner.player.DraggingSlot.Count;
				slot.Item = this.owner.player.DraggingSlot.Item;
				slot.CurrentDurability = this.owner.player.DraggingSlot.CurrentDurability;
				slot.CurrentClipSize = this.owner.player.DraggingSlot.CurrentClipSize;
				this.owner.player.DraggingSlot.Item = null;
				this.owner.Show();
				this.owner.player.DraggingInventory.Show();
			}
			else if (slot.Item.Id == this.owner.player.DraggingSlot.Item.Id && slot != this.owner.player.DraggingSlot)
			{
				if (this.owner.InvType == InventoryType.Cloth && !Inventory.IsCloth(this.owner.player.DraggingSlot.Item))
				{
					return;
				}
				if (Inventory.IsCloth(this.owner.player.DraggingSlot.Item) && this.owner.InvType == InventoryType.Cloth && this.type != (this.owner.player.DraggingSlot.Item as Cloth).Slot)
				{
					return;
				}
				if (slot.Count + this.owner.player.DraggingSlot.Count <= Database.StackSize)
				{
					slot.Count += this.owner.player.DraggingSlot.Count;
				}
			}
			else
			{
				if (this.owner.InvType == InventoryType.Cloth && !Inventory.IsCloth(this.owner.player.DraggingSlot.Item))
				{
					return;
				}
				if (Inventory.IsCloth(this.owner.player.DraggingSlot.Item) && this.owner.InvType == InventoryType.Cloth && this.type != (this.owner.player.DraggingSlot.Item as Cloth).Slot)
				{
					return;
				}
				Slot slot2 = new Slot(this.owner.player.DraggingSlot.Item);
				if (this.owner.player.Toolbet.InventoryArray.Contains(slot))
				{
					this.owner.player.UnequipAllToolbet();
				}
				slot2.Count = this.owner.player.DraggingSlot.Count;
				slot2.Item = this.owner.player.DraggingSlot.Item;
				slot2.CurrentDurability = this.owner.player.DraggingSlot.CurrentDurability;
				slot2.CurrentClipSize = this.owner.player.DraggingSlot.CurrentClipSize;
				this.owner.player.DraggingSlot.Count = slot.Count;
				this.owner.player.DraggingSlot.Item = slot.Item;
				this.owner.player.DraggingSlot.CurrentDurability = slot.CurrentDurability;
				this.owner.player.DraggingSlot.CurrentClipSize = slot.CurrentClipSize;
				slot.Count = slot2.Count;
				slot.Item = slot2.Item;
				slot.CurrentDurability = slot2.CurrentDurability;
				slot.CurrentClipSize = slot2.CurrentClipSize;
			}
			this.owner.player.StopDrag();
			this.owner.player.DraggingInventory.Show();
			this.owner.player.Toolbet.Show();
			this.owner.Show();
		}
		else
		{
			this.owner.player.SlotClicked(slot, this.owner);
		}
	}
}
