using System;

public class Slot
{
	public Item Item;

	public int Count;

	public float CurrentDurability;

	public int CurrentClipSize;

	public Slot(Item item)
	{
		if (item != null)
		{
			this.Item = item;
			if (item.GetType() == typeof(Tool))
			{
				this.CurrentDurability = (float)(item as Tool).MaximumDurability;
			}
			else if (item.GetType() == typeof(Weapon))
			{
				this.CurrentDurability = (float)(item as Weapon).MaximumDurability;
				this.CurrentClipSize = (item as Weapon).ClipSize;
			}
			else if (item.GetType() == typeof(Food))
			{
				this.CurrentDurability = (item as Food).FoodDurability;
			}
			else if (item.GetType() == typeof(Cloth))
			{
				this.CurrentDurability = (float)(item as Cloth).MaximumDurability;
			}
		}
	}
}
