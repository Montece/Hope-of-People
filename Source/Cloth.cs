using System;

public class Cloth : Item
{
	public byte LegsDamageReduction;

	public byte ArmsDamageReduction;

	public byte HeadsDamageReduction;

	public byte BodysDamageReduction;

	public byte AllsDamageReduction;

	public int MaximumDurability;

	public ArmorSlot Slot;

	public Cloth()
	{
		this.ItemType = ItemType.Cloth;
		this.CanStack = false;
	}
}
