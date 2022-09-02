using System;

public class Resource : Item
{
	public bool IsFuel;

	public Resource()
	{
		this.ItemType = ItemType.Resource;
	}
}
