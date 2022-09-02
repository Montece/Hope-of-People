using System;

public class Food : Item
{
	public int FoodPower;

	public int WaterPower;

	public int StaminaPower;

	public float FoodDurability;

	public bool CanRot = true;

	public bool HealthyFood = true;

	public Food()
	{
		this.ItemType = ItemType.Food;
	}
}
