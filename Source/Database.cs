using System;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
	public static float RottingPerSecond;

	public static int RarityCount;

	public static int StackSize;

	public static int FoodRate;

	public static int HealingRate;

	public static int ResourceRate;

	public static int AmmoRate;

	public static int BuildingRate;

	public static List<Item> Items;

	public static List<CraftInfo> Crafts;

	public static List<Buff> Buffs;

	public static List<CraftInfo> CurrentCrafts;

	public static Dictionary<string, ChestConfig> Configs;

	static Database()
	{
		Database.RottingPerSecond = 0.2f;
		Database.RarityCount = 5;
		Database.StackSize = 1000;
		Database.FoodRate = 1;
		Database.HealingRate = 1;
		Database.ResourceRate = 1;
		Database.AmmoRate = 1;
		Database.BuildingRate = 1;
		Database.Items = new List<Item>();
		Database.Crafts = new List<CraftInfo>();
		Database.Buffs = new List<Buff>();
		Database.CurrentCrafts = new List<CraftInfo>();
		Database.Configs = new Dictionary<string, ChestConfig>();
		Database.LoadItems();
		Database.LoadCrafts();
	}

	private static void LoadItems()
	{
		Database.CreateBuff("Low Slowing", 0, false, true);
		Database.CreateBuff("Medium Slowing", 1, false, true);
		Database.CreateBuff("Maximum Slowing", 2, false, true);
		Database.CreateBuff("Walk Anemia", 3, false, true);
		Database.CreateBuff("Low Jump Reduction", 4, false, true);
		Database.CreateBuff("Medium Jump Reduction", 5, false, true);
		Database.CreateBuff("Maximum Jump Reduction", 6, false, true);
		Database.CreateBuff("Jump Anemia", 7, false, true);
		Database.CreateBuff("Low De-Digging", 8, false, true);
		Database.CreateBuff("Medium De-Digging", 9, false, true);
		Database.CreateBuff("Maximum De-Digging", 10, false, true);
		Database.CreateBuff("Cannot Dig", 11, false, true);
		Database.CreateBuff("Low Weapon Damage Reduction", 12, false, true);
		Database.CreateBuff("Medium Weapon Damage Reduction", 13, false, true);
		Database.CreateBuff("Maximum Weapon Damage Reduction", 14, false, true);
		Database.CreateBuff("Cannot Shoot", 15, false, true);
		Database.CreateBuff("Low Disease", 16, false, true);
		Database.CreateBuff("Medium Disease", 17, false, true);
		Database.CreateBuff("Maximum Disease", 18, false, true);
		Database.CreateBuff("Low Food Decrease", 19, false, true);
		Database.CreateBuff("Medium Food Decrease", 20, false, true);
		Database.CreateBuff("Maximum Food Decrease", 21, false, true);
		Database.CreateBuff("Jump Boost", 22, true, false);
		Database.CreateBuff("Regeneration", 23, true, false);
		Database.CreateBuff("Speed Boost", 24, true, false);
		Database.CreateBuff("Fire", 25, true, false);
		Database.CreateBuff("Bleeding", 26, false, false);
		Database.CreateResource("Iron Bar", "ironbar", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Wood Log", "woodlog", true, string.Empty, false, false, true, new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 3)
		});
		Database.CreateResource("Wood Plank", "woodplank", true, string.Empty, false, false, true, new IINFO[]
		{
			new IINFO(Database.Get("woodenstick"), 4)
		});
		Database.CreateResource("Coin", "ironcoin", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateResource("Gold Bar", "goldbar", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateResource("Diamond", "diamond", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("glass"), 1)
		});
		Database.CreateResource("Small Battery", "smallbattery", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("sulfuricacid"), 2)
		});
		Database.CreateResource("Iron Plank", "ironplank", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 2)
		});
		Database.CreateResource("Stone", "stone", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Iron Ore", "ironore", true, "ironbar", true, false, false, new IINFO[0]);
		Database.CreateResource("Gold Ore", "goldore", true, "goldbar", true, false, false, new IINFO[]
		{
			new IINFO(Database.Get("ironore"), 1)
		});
		Database.CreateResource("Coal", "coal", true, string.Empty, false, false, true, new IINFO[]
		{
			new IINFO(Database.Get("stone"), 5)
		});
		Database.CreateResource("Wooden Stick", "woodenstick", true, string.Empty, false, false, true, new IINFO[0]);
		Database.CreateResource("Human Piss", "piss", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("rottenfood"), 1)
		});
		Database.CreateResource("Sulphur", "sulphur", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("stone"), 5)
		});
		Database.CreateResource("Gunpowder", "gunpowder", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("sulphur"), 3)
		});
		Database.CreateResource("Concrete", "concrete", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("stone"), 50)
		});
		Database.CreateResource("Grass Fiber", "grass", true, string.Empty, false, false, true, new IINFO[0]);
		Database.CreateResource("Rope", "rope", true, string.Empty, false, false, false, new IINFO[]
		{
			new IINFO(Database.Get("grass"), 3)
		});
		Database.CreateResource("Can Of Biofuel", "biofuel", true, string.Empty, false, false, true, new IINFO[0]);
		Database.CreateResource("Animal Fat", "fat", true, string.Empty, false, false, true, new IINFO[0]);
		Database.CreateResource("Sulfuric Acid", "sulfuricacid", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Nails", "nails", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Lead Bar", "leadbar", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Tin Bar", "tinbar", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Copper Bar", "copperbar", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Lead Ore", "leadore", true, "leadbar", true, false, false, new IINFO[0]);
		Database.CreateResource("Tin Ore", "tinore", true, "tinbar", true, false, false, new IINFO[0]);
		Database.CreateResource("Copper Ore", "copperore", true, "copperbar", true, false, false, new IINFO[0]);
		Database.CreateResource("Chip", "chip", false, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Flint", "flint", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Copper Wire", "copperwire", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Activation Key", "activationkey", false, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Rubber", "rubber", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Steel Bar", "steelbar", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Steel Ore", "steelore", true, "steelbar", true, false, false, new IINFO[0]);
		Database.CreateResource("Glass", "glass", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Sand", "sand", true, "glass", true, false, false, new IINFO[0]);
		Database.CreateResource("Iron Pipe", "pipe", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Tire", "tire", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Disk", "disk", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Big Battery", "bigbattery", false, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Window", "window", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Car Frame", "carframe", false, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Car Engine", "engine", false, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Tin Sheet", "tinsheet", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Spring", "spring", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Gear", "gear", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Tin Can", "tincan", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Glass Dishes", "glassdishes", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateWeapon("Smoke Grenade", "smokegrenade", 1, 1, 1, "stone", new IINFO[0]);
		Database.CreateWeapon("Grenade", "grenade", 1, 1, 1, "stone", new IINFO[0]);
		Database.CreateWeapon("Supply Grenade", "supplygrenade", 1, 1, 1, "stone", new IINFO[0]);
		Database.CreateResource("Rubber Duck", "rubberduck", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Capacitor", "capacitor", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Resistor", "resistor", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("LED", "led", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("CPU", "cpu", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Motherboard", "motherboard", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Power Supply", "powersupply", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("RAM", "ram", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("HDD", "hdd", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Screen", "screen", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Cases", "cases", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Sci-fi Spike", "scifispike", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Steel Pipe", "steelpipe", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Tungsten Filings", "tungstenfilings", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Wheel", "wheel", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Cloth", "cloth", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateResource("Fur", "fur", true, string.Empty, false, false, false, new IINFO[0]);
		Database.CreateTool("Stone Axe", "stoneaxe", 100, 50, new IINFO[0]);
		Database.CreateTool("Stone Pickaxe", "stonepickaxe", 100, 50, new IINFO[0]);
		Database.CreateTool("Iron Axe", "ironaxe", 300, 80, new IINFO[0]);
		Database.CreateTool("Flashlight", "flashlight", 100, 0, new IINFO[0]);
		Database.CreateTool("Iron Shovel", "ironshovel", 300, 20, new IINFO[0]);
		Database.CreateTool("Hammer", "hammer", 300, 30, new IINFO[0]);
		Database.CreateTool("Iron Pickaxe", "ironpickaxe", 300, 80, new IINFO[0]);
		Database.CreateTool("Sci-fi Axe", "scifiaxe", 1000, 120, new IINFO[0]);
		Database.CreateTool("Sci-fi Pickaxe", "scifipickaxe", 1000, 120, new IINFO[0]);
		Database.CreateTool("Torch", "torch", 100, 0, new IINFO[0]);
		Database.CreateFood("Apple", "apple", true, 1, 1, 0, string.Empty, false, false, 100, true, new IINFO[0]);
		Database.CreateFood("Whipped Cream", "cream", true, 5, 2, 0, string.Empty, false, false, 500, false, new IINFO[0]);
		Database.CreateFood("Tuna", "tuna", true, 5, 3, 0, string.Empty, false, false, 1000, true, new IINFO[0]);
		Database.CreateFood("Sushi", "sushi", true, 5, 0, 0, string.Empty, false, false, 100, false, new IINFO[0]);
		Database.CreateFood("Sushi Dinner", "sushidinner", false, 20, 5, 10, string.Empty, false, false, 100, false, new IINFO[0]);
		Database.CreateFood("Soup", "soup", true, 20, 20, 30, string.Empty, false, false, 100, true, new IINFO[0]);
		Database.CreateFood("Soda", "soda", true, 5, 10, 30, string.Empty, false, false, 1000, false, new IINFO[0]);
		Database.CreateFood("Soda Can", "sodacan", true, 3, 8, 30, string.Empty, false, false, 1000, false, new IINFO[0]);
		Database.CreateFood("Beer", "beer", true, 10, 10, 5, string.Empty, false, false, 1000, false, new IINFO[0]);
		Database.CreateFood("Crackers", "crackers", true, 5, 0, 0, string.Empty, false, false, 200, false, new IINFO[0]);
		Database.CreateFood("Salami", "salami", false, 15, 0, 0, string.Empty, false, false, 100, false, new IINFO[0]);
		Database.CreateFood("Chips", "chips", true, 5, -5, 5, string.Empty, false, false, 500, false, new IINFO[0]);
		Database.CreateFood("Noodles", "noodles", true, 5, 5, 0, string.Empty, false, false, 300, false, new IINFO[0]);
		Database.CreateFood("Cheese", "cheese", true, 5, 3, 3, string.Empty, false, false, 100, true, new IINFO[0]);
		Database.CreateFood("Milk", "milk", true, 5, 15, 15, string.Empty, false, false, 50, true, new IINFO[0]);
		Database.CreateFood("Apple Juice", "applejuice", true, 5, 20, 20, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Fries", "fries", true, 5, 1, 0, string.Empty, false, false, 200, false, new IINFO[0]);
		Database.CreateFood("Pirate Charms", "charms", true, 20, 20, 20, string.Empty, false, false, 300, false, new IINFO[0]);
		Database.CreateFood("Roast Meat", "roastmeat", true, 20, 15, 5, string.Empty, false, false, 400, true, new IINFO[0]);
		Database.CreateFood("Raw Meat", "rawmeat", true, 5, -10, -10, "roastmeat", false, true, 100, false, new IINFO[0]);
		Database.CreateFood("Lootbox", "lootbox", true, 0, 0, 0, string.Empty, false, false, 0, false, new IINFO[0]);
		Database.CreateFood("Raspberry", "raspberry", true, 2, 2, 1, string.Empty, false, false, 100, true, new IINFO[0]);
		Database.CreateFood("Amanita", "amanita", true, -10, -10, -50, string.Empty, false, false, 400, true, new IINFO[0]);
		Database.CreateFood("Porcini", "porcini", true, 3, 3, 3, string.Empty, false, false, 400, true, new IINFO[0]);
		Database.CreateFood("Chanterelles", "chanterelles", true, 2, 2, 2, string.Empty, false, false, 400, true, new IINFO[0]);
		Database.CreateFood("Russula", "russula", true, 1, 1, 1, string.Empty, false, false, 400, true, new IINFO[0]);
		Database.CreateFood("Rotten food", "rottenfood", true, -40, -40, -40, string.Empty, false, false, 0, false, new IINFO[0]);
		Database.CreateFood("Wattermelon", "watermelon", true, 25, 15, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Tomatoe", "tomatoe", true, 5, 5, 5, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Radishe", "radishe", true, 10, 10, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Pumpkin", "pumpkin", true, 20, 5, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Potatoe", "potatoe", true, 5, 5, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Peacher", "peacher", true, 5, 5, 5, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Pear", "pear", true, 5, 5, 5, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Lemon", "lemon", true, 10, 10, -10, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Grape", "grape", true, 10, 10, 5, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Cucumber", "cucumber", true, 10, 10, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Corn", "corn", true, 15, 5, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Coconut", "coconut", true, 5, 20, 10, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Carrot", "carrot", true, 5, 5, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Cabbage", "cabbage", true, 20, 5, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Bell Pepper", "bellpepper", true, 10, 0, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Bagette", "bagette", true, 15, 0, 0, string.Empty, false, false, 500, true, new IINFO[0]);
		Database.CreateFood("Bagel", "bagel", true, 5, 0, 10, string.Empty, false, false, 100, true, new IINFO[0]);
		Database.CreateHealing("Bandage", "bandage", true, 25, new IINFO[0]);
		Database.CreateHealing("Medical Bag", "medbag", true, 75, new IINFO[0]);
		Database.CreateHealing("Speed Boost", "boostspeed", true, 0, new IINFO[0]);
		Database.CreateHealing("Regeneration Boost", "boostregen", true, 0, new IINFO[0]);
		Database.CreateHealing("Chloroform", "chloroform", true, 0, new IINFO[0]);
		Database.CreateHealing("Jump Boost", "boostjump", true, 0, new IINFO[0]);
		Database.CreateHealing("Ointment For Feet", "feethealing", true, 15, new IINFO[0]);
		Database.CreateHealing("Painkillers", "painkillers", true, 10, new IINFO[0]);
		Database.CreateBuilding("Stone Forge", "stoneforge", false, false, new IINFO[0]);
		Database.CreateBuilding("C4", "c4", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Big Window", "c_big_window", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Medium Window", "c_medium_window", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Small Window", "c_small_window", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Block", "c_block", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Floor", "c_floor", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Pillar", "c_pillar", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Big Wall", "c_big_wall", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Medium Wall", "c_medium_wall", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Small Wall", "c_small_wall", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Medium Border", "c_medium_border", false, false, new IINFO[0]);
		Database.CreateBuilding("Concrete Small Border", "c_small_border", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Big Window", "w_big_window", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Medium Window", "w_medium_window", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Small Window", "w_small_window", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Block", "w_block", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Floor", "w_floor", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Pillar", "w_pillar", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Big Wall", "w_big_wall", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Medium Wall", "w_medium_wall", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Small Wall", "w_small_wall", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Medium Border", "w_medium_border", false, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Small Border", "w_small_border", false, false, new IINFO[0]);
		Database.CreateBuilding("Dildo", "dildo", true, false, new IINFO[0]);
		Database.CreateBuilding("Rust House", "rusthouse", true, false, new IINFO[0]);
		Database.CreateBuilding("Wooden Box", "woodenbox", true, false, new IINFO[0]);
		Database.CreateBuilding("Construction Lamp", "c_lamp", true, false, new IINFO[0]);
		Database.CreateBuilding("Iron Fence", "iron_fence", true, false, new IINFO[0]);
		Database.CreateBuilding("Sci-Fi Door", "scifidoor", true, false, new IINFO[0]);
		Database.CreateBuilding("Campfire", "campfire", false, false, new IINFO[0]);
		Database.CreateBuilding("Stone Stairs", "stonestairs", true, false, new IINFO[0]);
		Database.CreateBuilding("Dead Chest", "dead", true, false, new IINFO[0]);
		Database.CreateBuilding("Sci-fi Border", "scifiborder", true, false, new IINFO[0]);
		Database.CreateBuilding("Big Bricks Wall", "bricks_big_wall", true, false, new IINFO[0]);
		Database.CreateBuilding("Big Forge", "bigforge", false, false, new IINFO[0]);
		Database.CreateBuilding("SUV", "suv", false, false, new IINFO[0]);
		Database.CreateBuilding("Buggy", "buggy", false, false, new IINFO[0]);
		Database.CreateBuilding("VAN", "van", false, false, new IINFO[0]);
		Database.CreateBuilding("Sedan", "sedan", false, false, new IINFO[0]);
		Database.CreateBuilding("Flatbed", "flatbed", false, false, new IINFO[0]);
		Database.CreateBuilding("Bus", "bus", false, false, new IINFO[0]);
		Database.CreateBuilding("Crusher", "crusher", false, false, new IINFO[0]);
		Database.CreateBuilding("Glass Window", "glasswindow", true, false, new IINFO[0]);
		Database.CreateBuilding("Table Lamp", "tablelamp", true, false, new IINFO[0]);
		Database.CreateBuilding("Wall Lamp", "walllamp", true, false, new IINFO[0]);
		Database.CreateBuilding("TV Stand", "tvstand", true, false, new IINFO[0]);
		Database.CreateBuilding("Toilet", "toilet", true, false, new IINFO[0]);
		Database.CreateBuilding("Toilet Room", "toiletroom", true, false, new IINFO[0]);
		Database.CreateBuilding("Sofa", "sofa", true, false, new IINFO[0]);
		Database.CreateBuilding("Shower", "shower", true, false, new IINFO[0]);
		Database.CreateBuilding("Table", "table", true, false, new IINFO[0]);
		Database.CreateBuilding("Chair", "chair", true, false, new IINFO[0]);
		Database.CreateBuilding("Coffee Table", "coffeetable", true, false, new IINFO[0]);
		Database.CreateBuilding("Closet", "closet", true, false, new IINFO[0]);
		Database.CreateBuilding("Bookshelfs", "bookshelfs", true, false, new IINFO[0]);
		Database.CreateBuilding("Bath", "bath", true, false, new IINFO[0]);
		Database.CreateBuilding("Candle", "candle", true, false, new IINFO[0]);
		Database.CreateBuilding("Pillow", "pillow", true, false, new IINFO[0]);
		Database.CreateBuilding("Painting", "painting1", true, false, new IINFO[0]);
		Database.CreateBuilding("Painting", "painting2", true, false, new IINFO[0]);
		Database.CreateBuilding("Painting", "painting3", true, false, new IINFO[0]);
		Database.CreateBuilding("Fridge", "fridge", true, false, new IINFO[0]);
		Database.CreateBuilding("Oven", "oven", true, false, new IINFO[0]);
		Database.CreateBuilding("Big Box", "bigbox", true, false, new IINFO[0]);
		Database.CreateBuilding("Tree Sapling", "treesapling", true, true, new IINFO[0]);
		Database.CreateBuilding("Wattermelons Matter-Seeds", "watermelonsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Tomatoes Matter-Seeds", "tomatoesseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Radishes Matter-Seeds", "radishesseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Pumpkins Matter-Seeds", "pumpkinsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Potatoes Matter-Seeds", "potatoesseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Peacher Matter-Seeds", "peacherseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Pears Matter-Seeds", "pearsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Porcini Matter-Seeds", "porciniseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Lemons Matter-Seeds", "lemonsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Grapes Matter-Seeds", "grapesseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Cucumber Matter-Seeds", "cucumberseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Corn Matter-Seeds", "cornseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Coconuts Matter-Seeds", "coconutsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Carrots Matter-Seeds", "carrotsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Cabbages Matter-Seeds", "cabbagesseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Bell Peppers Matter-Seeds", "bellpeppersseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Bagettes Matter-Seeds", "bagettesseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Bagels Matter-Seeds", "bagelsseeds", true, true, new IINFO[0]);
		Database.CreateBuilding("Apples Matter-Seeds", "applesseeds", true, true, new IINFO[0]);
		Database.CreateAmmo("Sci-Fi Rifle Ammo", "scifirifleammo", true);
		Database.CreateAmmo("Sci-Fi Submachine Ammo", "scifisubammo", true);
		Database.CreateAmmo("Sci-Fi Shotgun Shells", "scifishotgunammo", true);
		Database.CreateAmmo("Sci-Fi Rocket", "scifirocket", false);
		Database.CreateAmmo("Sci-Fi Pistol Ammo", "scifipistolammo", true);
		Database.CreateAmmo("Bolt", "bolt", true);
		Database.CreateAmmo("M200 Ammo", "m200ammo", true);
		Database.CreateWeapon("Sci-Fi Rifle", "scifirifle", 300, 20, 100, "scifirifleammo", new IINFO[0]);
		Database.CreateWeapon("Sci-Fi Submachine", "scifisubmachine", 1000, 15, 200, "scifisubammo", new IINFO[0]);
		Database.CreateWeapon("Sci-Fi Shotgun", "scifishotgun", 100, 80, 6, "scifishotgunammo", new IINFO[0]);
		Database.CreateWeapon("Sci-Fi Rocket Launcher", "scifiricketlauncher", 10, 1000, 1, "scifirocket", new IINFO[0]);
		Database.CreateWeapon("Sci-Fi Standart Pistol", "scifipisol_normal", 100, 10, 13, "scifipistolammo", new IINFO[0]);
		Database.CreateWeapon("Sci-Fi Short Pistol", "scifipisol_short", 100, 20, 13, "scifipistolammo", new IINFO[0]);
		Database.CreateWeapon("Sci-Fi Long Pistol", "scifipisol_long", 100, 15, 13, "scifipistolammo", new IINFO[0]);
		Database.CreateWeapon("Iron Sword", "ironsword", 200, 12, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Copper Sword", "coppersword", 300, 14, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Gold Sword", "goldsword", 50, 20, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Cutlass", "cutlass", 400, 16, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Diamond Sword", "diamondsword", 600, 20, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Steel Sword", "steelsword", 700, 15, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Ancient Sword", "ancientsword", 2000, 30, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Silver Sword", "silversword", 200, 20, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Lead Sword", "leadsword", 500, 22, 999, "stone", new IINFO[0]);
		Database.CreateWeapon("Crossbow", "crossbow", 200, 50, 1, "bolt", new IINFO[0]);
		Database.CreateWeapon("Smoke Grenade", "smokegrenade", 1, 1, 1, "stone", new IINFO[0]);
		Database.CreateWeapon("Supply Grenade", "supplysrenade", 1, 1, 1, "stone", new IINFO[0]);
		Database.CreateWeapon("Grenade", "grenade", 1, 1, 1, "stone", new IINFO[0]);
		Database.CreateWeapon("M200", "m200", 500, 120, 5, "m200ammo", new IINFO[0]);
		Database.CreateCloth("Cap", "cap", 100, 10, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("Cowboy Hat", "cowboyhat", 100, 15, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("Cop Hat", "cophat", 100, 20, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("Safety Helmet", "safetyhelmet", 200, 30, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("Moto Helmet", "motohelmet", 300, 40, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("Military Helmet", "militaryhelmet", 500, 60, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("Sci-fi Helmet", "scifihelmet", 1000, 80, 0, 0, 0, 0, ArmorSlot.Head, new IINFO[0]);
		Database.CreateCloth("T-Short", "tshort", 100, 0, 10, 10, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Poncho", "poncho", 150, 0, 15, 15, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Bulletroof Vest", "bulletproofvest", 200, 0, 20, 20, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Jacket", "jacket", 100, 0, 20, 20, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Dragon Armor", "dragonarmor", 500, 0, 60, 60, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Military Vest", "militaryvest", 600, 40, 40, 0, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Sci-fi Armor", "scifiarmor", 1000, 0, 80, 80, 0, 0, ArmorSlot.Body, new IINFO[0]);
		Database.CreateCloth("Pants", "pants", 100, 0, 0, 0, 10, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Dress", "dress", 100, 0, 0, 0, 10, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Trousers", "trousers", 100, 0, 0, 0, 10, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Construction Pants", "constructionpants", 100, 0, 0, 0, 15, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Shorts", "shorts", 100, 0, 0, 0, 10, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Military Pants", "militarypants", 400, 0, 0, 0, 20, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Sci-fi Leg Armor", "scifilegarmor", 1000, 0, 0, 0, 50, 0, ArmorSlot.Legs, new IINFO[0]);
		Database.CreateCloth("Boots", "boots", 300, 0, 0, 0, 30, 0, ArmorSlot.Feet, new IINFO[0]);
		Database.CreateCloth("Sneakers", "sneakers", 100, 0, 0, 0, 20, 0, ArmorSlot.Feet, new IINFO[0]);
		Database.CreateCloth("Shoes", "shoes", 100, 0, 0, 0, 20, 0, ArmorSlot.Feet, new IINFO[0]);
		GC.Collect();
	}

	private static void LoadCrafts()
	{
		Database.CreateCraft(true, new IINFO(Database.Get("scifispike"), 1), new IINFO[]
		{
			new IINFO(Database.Get("stone"), 2000)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("activationkey"), 1), new IINFO[]
		{
			new IINFO(Database.Get("scifispike"), 2),
			new IINFO(Database.Get("tungstenfilings"), 2),
			new IINFO(Database.Get("steelpipe"), 2),
			new IINFO(Database.Get("powersupply"), 2)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("woodplank"), 2), new IINFO[]
		{
			new IINFO(Database.Get("woodlog"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("woodenstick"), 4), new IINFO[]
		{
			new IINFO(Database.Get("woodlog"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("rope"), 1), new IINFO[]
		{
			new IINFO(Database.Get("grass"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("stoneaxe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodenstick"), 40),
			new IINFO(Database.Get("stone"), 45)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("stonepickaxe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("stone"), 60),
			new IINFO(Database.Get("woodenstick"), 40)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("ironaxe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 45),
			new IINFO(Database.Get("woodenstick"), 40)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("ironshovel"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 50),
			new IINFO(Database.Get("woodenstick"), 50)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("ironpickaxe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 60),
			new IINFO(Database.Get("woodenstick"), 40)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifiaxe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("chip"), 1),
			new IINFO(Database.Get("woodenstick"), 40),
			new IINFO(Database.Get("diamond"), 2)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifipickaxe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("chip"), 2),
			new IINFO(Database.Get("woodenstick"), 40),
			new IINFO(Database.Get("diamond"), 3)
		});
		Database.CreateCraft(false, new IINFO(Database.Get("ironcoin"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 2)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("stoneforge"), 1), new IINFO[]
		{
			new IINFO(Database.Get("stone"), 200),
			new IINFO(Database.Get("woodenstick"), 50)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("gunpowder"), 50), new IINFO[]
		{
			new IINFO(Database.Get("coal"), 100),
			new IINFO(Database.Get("piss"), 100),
			new IINFO(Database.Get("sulphur"), 100)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c4"), 1), new IINFO[]
		{
			new IINFO(Database.Get("gunpowder"), 50),
			new IINFO(Database.Get("ironbar"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("concrete"), 400), new IINFO[]
		{
			new IINFO(Database.Get("soda"), 1),
			new IINFO(Database.Get("stone"), 600)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("concrete"), 100), new IINFO[]
		{
			new IINFO(Database.Get("sodacan"), 1),
			new IINFO(Database.Get("stone"), 300)
		});
		Database.CreateCraft(false, new IINFO(Database.Get("dildo"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodlog"), 1000)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("rusthouse"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodlog"), 300)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("woodenbox"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 400)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("biofuel"), 50), new IINFO[]
		{
			new IINFO(Database.Get("fat"), 50),
			new IINFO(Database.Get("sulfuricacid"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("nails"), 5), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(false, new IINFO(Database.Get("chip"), 1), new IINFO[]
		{
			new IINFO(Database.Get("copperbar"), 20),
			new IINFO(Database.Get("tinbar"), 20),
			new IINFO(Database.Get("flint"), 20)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifirifleammo"), 1), new IINFO[]
		{
			new IINFO(Database.Get("gunpowder"), 30),
			new IINFO(Database.Get("ironbar"), 3)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifisubammo"), 1), new IINFO[]
		{
			new IINFO(Database.Get("gunpowder"), 60),
			new IINFO(Database.Get("ironbar"), 6)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifishotgunammo"), 1), new IINFO[]
		{
			new IINFO(Database.Get("gunpowder"), 10),
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifirocket"), 1), new IINFO[]
		{
			new IINFO(Database.Get("gunpowder"), 200),
			new IINFO(Database.Get("ironbar"), 20)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("campfire"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodenstick"), 50),
			new IINFO(Database.Get("woodlog"), 3)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("hammer"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodenstick"), 50),
			new IINFO(Database.Get("stone"), 300)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("stonestairs"), 1), new IINFO[]
		{
			new IINFO(Database.Get("stone"), 500)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifipistolammo"), 1), new IINFO[]
		{
			new IINFO(Database.Get("gunpowder"), 20),
			new IINFO(Database.Get("ironbar"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("copperwire"), 2), new IINFO[]
		{
			new IINFO(Database.Get("copperbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifipisol_normal"), 1), new IINFO[]
		{
			new IINFO(Database.Get("chip"), 1),
			new IINFO(Database.Get("ironbar"), 10),
			new IINFO(Database.Get("copperwire"), 50)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifipisol_short"), 1), new IINFO[]
		{
			new IINFO(Database.Get("chip"), 1),
			new IINFO(Database.Get("ironbar"), 10),
			new IINFO(Database.Get("copperwire"), 50)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifipisol_long"), 1), new IINFO[]
		{
			new IINFO(Database.Get("chip"), 1),
			new IINFO(Database.Get("ironbar"), 10),
			new IINFO(Database.Get("copperwire"), 50)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("ironsword"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 20),
			new IINFO(Database.Get("woodenstick"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("goldsword"), 1), new IINFO[]
		{
			new IINFO(Database.Get("goldbar"), 20),
			new IINFO(Database.Get("woodenstick"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("coppersword"), 1), new IINFO[]
		{
			new IINFO(Database.Get("copperbar"), 20),
			new IINFO(Database.Get("woodenstick"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("diamondsword"), 1), new IINFO[]
		{
			new IINFO(Database.Get("diamond"), 5),
			new IINFO(Database.Get("woodenstick"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("leadsword"), 1), new IINFO[]
		{
			new IINFO(Database.Get("leadbar"), 20),
			new IINFO(Database.Get("woodenstick"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("crossbow"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 10),
			new IINFO(Database.Get("woodenstick"), 30),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("bolt"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1),
			new IINFO(Database.Get("grass"), 2)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("tire"), 1), new IINFO[]
		{
			new IINFO(Database.Get("rubber"), 30)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("disk"), 1), new IINFO[]
		{
			new IINFO(Database.Get("steelbar"), 5)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("wheel"), 1), new IINFO[]
		{
			new IINFO(Database.Get("tire"), 1),
			new IINFO(Database.Get("disk"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("bigbattery"), 1), new IINFO[]
		{
			new IINFO(Database.Get("sulfuricacid"), 20),
			new IINFO(Database.Get("leadbar"), 10),
			new IINFO(Database.Get("ironbar"), 10),
			new IINFO(Database.Get("smallbattery"), 5)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("glass"), 20)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("carframe"), 1), new IINFO[]
		{
			new IINFO(Database.Get("steelbar"), 50),
			new IINFO(Database.Get("ironbar"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("engine"), 1), new IINFO[]
		{
			new IINFO(Database.Get("pipe"), 12),
			new IINFO(Database.Get("steelbar"), 10),
			new IINFO(Database.Get("ironbar"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("glasswindow"), 1), new IINFO[]
		{
			new IINFO(Database.Get("glass"), 20),
			new IINFO(Database.Get("ironbar"), 5)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("tablelamp"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("walllamp"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("tvstand"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("toilet"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("sofa"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("shower"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("table"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("chair"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("coffeetable"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("closet"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("bookshelfs"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("bath"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("candle"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("pillow"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("painting1"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("painting2"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("painting3"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("fridge"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("oven"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 1)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("suv"), 1), new IINFO[]
		{
			new IINFO(Database.Get("engine"), 1),
			new IINFO(Database.Get("carframe"), 1),
			new IINFO(Database.Get("window"), 6),
			new IINFO(Database.Get("bigbattery"), 2),
			new IINFO(Database.Get("wheel"), 4)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("crusher"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 400),
			new IINFO(Database.Get("ironbar"), 50),
			new IINFO(Database.Get("copperbar"), 20)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_lamp"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 10),
			new IINFO(Database.Get("biofuel"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("iron_fence"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 300)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("scifidoor"), 1), new IINFO[]
		{
			new IINFO(Database.Get("ironbar"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("torch"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodenstick"), 10),
			new IINFO(Database.Get("biofuel"), 2)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_big_window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 200),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_medium_window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 150),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_small_window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 200),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_block"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 200),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_floor"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 200),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_pillar"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 200),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_big_wall"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 200),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_medium_wall"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 150),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_small_wall"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 100),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_medium_border"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 150),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("w_small_border"), 1), new IINFO[]
		{
			new IINFO(Database.Get("woodplank"), 100),
			new IINFO(Database.Get("rope"), 2),
			new IINFO(Database.Get("nails"), 10)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_big_window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_medium_window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 150)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_small_window"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_block"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_floor"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_pillar"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_big_wall"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 200)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_medium_wall"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 150)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_small_wall"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 100)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_medium_border"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 150)
		});
		Database.CreateCraft(true, new IINFO(Database.Get("c_small_border"), 1), new IINFO[]
		{
			new IINFO(Database.Get("concrete"), 100)
		});
		Database.CreateBlueprint(Database.GetCraftInfoByID("ironcoin"), "ironcoin_blueprint", new IINFO[0]);
		Database.CreateBlueprint(Database.GetCraftInfoByID("dildo"), "dildo_blueprint", new IINFO[0]);
		Database.CreateBlueprint(Database.GetCraftInfoByID("chip"), "chip_blueprint", new IINFO[0]);
	}

	public static Item GetRandomItem()
	{
		return Database.Items[UnityEngine.Random.Range(0, Database.Items.Count)];
	}

	public static Item Get(string id)
	{
		for (int i = 0; i < Database.Items.Count; i++)
		{
			if (Database.Items[i].Id.ToLower() == id.ToLower())
			{
				return Database.Items[i];
			}
		}
		return null;
	}

	public static Item GetItemByID(string id)
	{
		for (int i = 0; i < Database.Items.Count; i++)
		{
			if (Database.Items[i].Id.ToLower() == id.ToLower())
			{
				return Database.Items[i];
			}
		}
		return null;
	}

	public static CraftInfo GetCraftInfoByID(string id)
	{
		foreach (CraftInfo current in Database.Crafts)
		{
			if (current.Result.Item.Id == id)
			{
				return current;
			}
		}
		return null;
	}

	public static Buff GetBuffByID(int id)
	{
		foreach (Buff current in Database.Buffs)
		{
			if (current.Id == id)
			{
				return current;
			}
		}
		return null;
	}

	public static void CreateBuff(string title, int ID, bool IsPositive = true, bool IsInfinity = false)
	{
		Buff item = new Buff
		{
			Title = title,
			Id = ID,
			Icon = Resources.Load<Sprite>("BuffsIcons/" + ID),
			IsInfinity = IsInfinity,
			IsPositive = IsPositive
		};
		Database.Buffs.Add(item);
	}

	public static void CreateCraft(bool startup, IINFO result, params IINFO[] need)
	{
		CraftInfo item = new CraftInfo
		{
			Result = result,
			NeedResources = need,
			Startup = startup
		};
		Database.Crafts.Add(item);
		if (startup)
		{
			Database.CurrentCrafts.Add(item);
		}
	}

	public static void CreateHealing(string title, string ID, bool canStack, int power, params IINFO[] waste)
	{
		if (title != string.Empty && Database.CanCreateItem(ID))
		{
			Healing healing = new Healing
			{
				Title = title,
				CanStack = canStack,
				Id = ID,
				Power = power
			};
			healing.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (healing.Icon == null)
			{
				healing.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			healing.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (healing.Drop == null)
			{
				healing.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			healing.Drop.GetComponent<Pickup>().slot.Item = healing;
			Database.Items.Add(healing);
		}
	}

	public static void CreateFood(string title, string ID, bool canStack, int foodpower, int waterpower, int staminapower, string ForgeResultId, bool CanForge, bool CanCamp, int Durability, bool HealthyFood, params IINFO[] waste)
	{
		if (title != string.Empty && Database.CanCreateItem(ID))
		{
			Food food = new Food
			{
				Title = title,
				CanStack = canStack,
				Id = ID,
				FoodPower = foodpower,
				WaterPower = waterpower,
				StaminaPower = staminapower
			};
			food.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (food.Icon == null)
			{
				food.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			food.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (food.Drop == null)
			{
				food.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			food.Drop.GetComponent<Pickup>().slot.Item = food;
			food.FoodDurability = (float)Durability;
			if (Durability <= 0)
			{
				food.CanRot = false;
			}
			else
			{
				food.CanRot = true;
			}
			food.HealthyFood = HealthyFood;
			if (ForgeResultId != string.Empty)
			{
				food.FryResult = Database.Get(ForgeResultId);
				food.CanForge = CanForge;
				food.CanCampfire = CanCamp;
			}
			Database.Items.Add(food);
		}
	}

	public static void CreateResource(string title, string ID, bool canStack, string ForgeResultId = "", bool CanForge = false, bool CanCamp = false, bool IsFuel = false, params IINFO[] waste)
	{
		if (title != string.Empty && Database.CanCreateItem(ID))
		{
			Resource resource = new Resource
			{
				Title = title,
				CanStack = canStack,
				Id = ID
			};
			resource.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (resource.Icon == null)
			{
				resource.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			resource.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (resource.Drop == null)
			{
				resource.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			resource.Drop.GetComponent<Pickup>().slot.Item = resource;
			if (ForgeResultId != string.Empty)
			{
				resource.FryResult = Database.Get(ForgeResultId);
				resource.CanForge = CanForge;
				resource.CanCampfire = CanCamp;
			}
			if (IsFuel)
			{
				IsFuel = true;
			}
			Database.Items.Add(resource);
		}
	}

	public static void CreateTool(string title, string ID, int durablility, int efficiency, params IINFO[] waste)
	{
		if (title != string.Empty && durablility > 0 && Database.CanCreateItem(ID))
		{
			Tool tool = new Tool
			{
				Title = title,
				CanStack = false,
				Id = ID,
				MaximumDurability = durablility,
				Efficiency = efficiency
			};
			tool.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (tool.Icon == null)
			{
				tool.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			tool.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (tool.Drop == null)
			{
				tool.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			tool.Drop.GetComponent<Pickup>().slot.Item = tool;
			Database.Items.Add(tool);
		}
	}

	public static void CreateCloth(string title, string ID, int durablility, byte HeadsDamageReduction, byte BodysDamageReduction, byte ArmsDamageReduction, byte LegsDamageReduction, byte AllsDamageReduction, ArmorSlot slot, params IINFO[] waste)
	{
		if (title != string.Empty && durablility > 0 && Database.CanCreateItem(ID))
		{
			Cloth cloth = new Cloth
			{
				Title = title,
				CanStack = false,
				Id = ID,
				MaximumDurability = durablility,
				HeadsDamageReduction = HeadsDamageReduction,
				BodysDamageReduction = BodysDamageReduction,
				ArmsDamageReduction = ArmsDamageReduction,
				LegsDamageReduction = LegsDamageReduction,
				AllsDamageReduction = AllsDamageReduction
			};
			cloth.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (cloth.Icon == null)
			{
				cloth.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			cloth.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (cloth.Drop == null)
			{
				cloth.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			cloth.MaximumDurability = durablility;
			cloth.Drop.GetComponent<Pickup>().slot.Item = cloth;
			cloth.Slot = slot;
			Database.Items.Add(cloth);
		}
	}

	public static void CreateWeapon(string title, string ID, int durablility, int damage, int clipammo, string ammoid, params IINFO[] waste)
	{
		if (title != string.Empty && durablility > 0 && Database.CanCreateItem(ID))
		{
			Weapon weapon = new Weapon
			{
				Title = title,
				CanStack = false,
				Id = ID,
				MaximumDurability = durablility,
				Damage = damage,
				ClipSize = clipammo
			};
			weapon.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (weapon.Icon == null)
			{
				weapon.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			weapon.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (weapon.Drop == null)
			{
				weapon.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			weapon.Drop.GetComponent<Pickup>().slot.Item = weapon;
			weapon.Ammo = Database.Get(ammoid);
			Database.Items.Add(weapon);
		}
	}

	public static void CreateBlueprint(CraftInfo info, string ID, params IINFO[] waste)
	{
		if (ID != string.Empty && Database.CanCreateItem(ID))
		{
			Blueprint blueprint = new Blueprint
			{
				Title = info.Result.Item.Title + "'s blueprint",
				CanStack = false,
				Id = ID
			};
			blueprint.Info = info;
			blueprint.Icon = Resources.Load<Sprite>("ItemIcons/Blueprint");
			if (blueprint.Icon == null)
			{
				blueprint.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			blueprint.Drop = Resources.Load<GameObject>("ItemDrops/Blueprint");
			if (blueprint.Drop == null)
			{
				blueprint.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			blueprint.Drop.GetComponent<Pickup>().slot.Item = blueprint;
			Database.Items.Add(blueprint);
		}
	}

	public static void CreateBuilding(string title, string ID, bool canStack, bool IsPlant, params IINFO[] waste)
	{
		if (ID != string.Empty && title != string.Empty && Database.CanCreateItem(ID))
		{
			Building building = new Building
			{
				Title = title,
				CanStack = canStack,
				Id = ID
			};
			building.Prefab = Resources.Load<GameObject>("ItemBuildings/" + ID);
			if (!ID.Equals("dildo"))
			{
				building.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			}
			else
			{
				building.Icon = null;
			}
			if (building.Icon == null)
			{
				building.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			building.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (building.Drop == null)
			{
				building.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			building.Drop.GetComponent<Pickup>().slot.Item = building;
			building.IsPlant = IsPlant;
			Database.Items.Add(building);
		}
	}

	public static void CreateAmmo(string title, string ID, bool canStack)
	{
		if (title != string.Empty && Database.CanCreateItem(ID))
		{
			Ammo ammo = new Ammo
			{
				Title = title,
				CanStack = canStack,
				Id = ID
			};
			ammo.Icon = Resources.Load<Sprite>("ItemIcons/" + ID);
			if (ammo.Icon == null)
			{
				ammo.Icon = Resources.Load<Sprite>("ItemIcons/Unknown");
			}
			ammo.Drop = Resources.Load<GameObject>("ItemDrops/" + ID);
			if (ammo.Drop == null)
			{
				ammo.Drop = Resources.Load<GameObject>("ItemDrops/Unknown");
			}
			ammo.Drop.GetComponent<Pickup>().slot.Item = ammo;
			ammo.Prefab = Resources.Load<GameObject>("ItemAmmo/" + ID);
			Database.Items.Add(ammo);
		}
	}

	private static bool CanCreateItem(string id)
	{
		using (List<Item>.Enumerator enumerator = Database.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Id == id)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static ChestConfig GetChestConfig(string id)
	{
		ChestConfig chestConfig = null;
		if (!Database.Configs.TryGetValue(id, out chestConfig))
		{
			chestConfig = GameManager.LoadJson(id);
			Database.Configs.Add(id, chestConfig);
		}
		return chestConfig;
	}
}
