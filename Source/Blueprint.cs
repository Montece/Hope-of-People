using System;

public class Blueprint : Item
{
	public CraftInfo Info;

	public Blueprint()
	{
		this.ItemType = ItemType.Blueprint;
	}
}
