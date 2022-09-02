using System;

public class Weapon : Item
{
	public int Damage;

	public bool IsAuto;

	public int MaximumDurability;

	public int ClipSize;

	public bool Range;

	public Item Ammo;

	public Weapon()
	{
		this.ItemType = ItemType.Weapon;
		this.CanStack = false;
	}
}
