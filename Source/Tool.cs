using System;

public class Tool : Item
{
	public int Efficiency;

	public int MaximumDurability;

	public Tool()
	{
		this.ItemType = ItemType.Tool;
		this.CanStack = false;
	}
}
