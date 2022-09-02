using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
	private Quest CurrentQuest;

	[HideInInspector]
	public List<Quest> FlatQuests = new List<Quest>();

	[HideInInspector]
	public List<Quest> IslandQuests = new List<Quest>();

	[HideInInspector]
	public List<Quest> RobotsQuests = new List<Quest>();

	[HideInInspector]
	public List<Quest> WorldQuests = new List<Quest>();

	private string n = Environment.NewLine;

	private SceneLinks links;

	public void OnStart(SceneLinks links)
	{
		this.links = links;
		links.QuestCompletedPanel.canvasRenderer.SetAlpha(0f);
		links.QuestCompletedPanel.transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0f);
		this.InitQuests();
		base.InvokeRepeating("CustomUpdate", 0f, 1f);
	}

	private void InitQuests()
	{
		this.FlatQuests.Add(this.CreateLocationQuest("Home, Sweet Home!", false, 0f, new Vector3(10f, 0f, 2f)));
		this.FlatQuests.Add(this.CreateLocationQuest("WTF??!!", false, 0f, new Vector3(0f, float.NegativeInfinity, 0f)));
		this.IslandQuests.Add(this.CreateInventoryDetectQuest("Where am I?", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("woodenstick"), 40),
			new ItemInfo(Database.GetItemByID("stone"), 45)
		}));
		this.IslandQuests.Add(this.CreateCraftQuest("Stone Age", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("stoneaxe"), 1)
		}));
		this.IslandQuests.Add(this.CreateInventoryDetectQuest("Woodcutter", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("woodlog"), 1000)
		}));
		this.IslandQuests.Add(this.CreateInventoryDetectQuest("Major Pain", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("scifispike"), 2)
		}));
		this.IslandQuests.Add(this.CreateInventoryDetectQuest("Tungsten!", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("tungstenfilings"), 2)
		}));
		this.IslandQuests.Add(this.CreateInventoryDetectQuest("Nerves of steel", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("steelpipe"), 2)
		}));
		this.IslandQuests.Add(this.CreateInventoryDetectQuest("E = mc^2", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("powersupply"), 1)
		}));
		this.IslandQuests.Add(this.CreateCraftQuest("Where are these voices from?", false, 0f, new ItemInfo[]
		{
			new ItemInfo(Database.GetItemByID("activationkey"), 1)
		}));
		this.RobotsQuests.Add(this.CreateLocationQuest("!523%*&*$_&!&$45ffuhjfa@$;", false, 0f, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
		this.WorldQuests.Add(this.CreateLocationQuest("Town will save the world!", false, 0f, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
	}

	private void CustomUpdate()
	{
		this.UpdateText();
		string name = SceneManager.GetActiveScene().name;
		if (name != null)
		{
			if (!(name == "Menu2"))
			{
				if (!(name == "Island"))
				{
					if (!(name == "Flat"))
					{
						if (!(name == "World"))
						{
							if (name == "Robots")
							{
								if (this.CurrentQuest == null)
								{
									this.CurrentQuest = this.RobotsQuests[0];
								}
								this.Check(this.RobotsQuests);
							}
						}
						else
						{
							if (this.CurrentQuest == null)
							{
								this.CurrentQuest = this.WorldQuests[0];
							}
							this.Check(this.WorldQuests);
						}
					}
					else
					{
						if (this.CurrentQuest == null)
						{
							this.CurrentQuest = this.FlatQuests[0];
						}
						this.Check(this.FlatQuests);
					}
				}
				else
				{
					if (this.CurrentQuest == null)
					{
						this.CurrentQuest = this.IslandQuests[0];
					}
					this.Check(this.IslandQuests);
				}
			}
		}
	}

	private void Check(List<Quest> quests)
	{
		bool flag = true;
		foreach (Quest current in quests)
		{
			if (current.QuestComplete)
			{
				base.StartCoroutine("CompleteQuest");
				quests.RemoveAt(0);
				this.CurrentQuest = null;
				break;
			}
			if (current.GetType() == typeof(KillQuest) && !current.QuestComplete)
			{
				foreach (EnemyInfo current2 in (current as KillQuest).Enemies)
				{
					if (current2.Count > 0)
					{
						flag = false;
						break;
					}
					if (current2.Count < 0)
					{
						current2.Count = 0;
					}
				}
				if (flag)
				{
					current.QuestComplete = true;
				}
			}
			if (current.GetType() == typeof(CraftQuest) && !current.QuestComplete)
			{
				foreach (ItemInfo current3 in (current as CraftQuest).Items)
				{
					if (current3.Count > 0)
					{
						flag = false;
						break;
					}
					if (current3.Count < 0)
					{
						current3.Count = 0;
					}
				}
				if (flag)
				{
					current.QuestComplete = true;
				}
			}
			if (current.GetType() == typeof(InventoryDetectQuest) && !current.QuestComplete)
			{
				foreach (ItemInfo current4 in (current as InventoryDetectQuest).Items)
				{
					if (current4.Count > 0)
					{
						flag = false;
						break;
					}
					if (current4.Count < 0)
					{
						current4.Count = 0;
					}
				}
				if (flag)
				{
					current.QuestComplete = true;
				}
			}
			if (current.GetType() == typeof(LocationQuest) && !current.QuestComplete && (int)(base.transform.position.x / 10f) == (int)((current as LocationQuest).Position.x / 10f) && (int)base.transform.position.z / 10 == (int)(current as LocationQuest).Position.z / 10 && (int)base.transform.position.y / 10 == (int)(current as LocationQuest).Position.y / 10)
			{
				current.QuestComplete = true;
			}
		}
	}

	private void UpdateText()
	{
		if (this.CurrentQuest == null)
		{
			this.links.QuestTitle.text = string.Empty;
			this.links.QuestDescription.text = string.Empty;
		}
		else
		{
			this.links.QuestTitle.text = this.CurrentQuest.Title + this.n;
			if (this.CurrentQuest.HasTime)
			{
				this.CurrentQuest.CurrentTime -= Time.deltaTime;
				int num = (int)(this.CurrentQuest.CurrentTime % 60f);
				int num2 = (int)(this.CurrentQuest.CurrentTime / 60f);
				string text = num2 + ":" + num;
			}
			if (this.CurrentQuest.GetType() == typeof(KillQuest))
			{
				foreach (EnemyInfo current in (this.CurrentQuest as KillQuest).Enemies)
				{
					Text expr_10D = this.links.QuestDescription;
					string text2 = expr_10D.text;
					expr_10D.text = string.Concat(new object[]
					{
						text2,
						current.EnemyID,
						" left: ",
						current.Count,
						this.n
					});
				}
			}
			if (this.CurrentQuest.GetType() == typeof(CraftQuest))
			{
				this.links.QuestDescription.text = "Need to craft: " + this.n;
				foreach (ItemInfo current2 in (this.CurrentQuest as CraftQuest).Items)
				{
					Text expr_1DB = this.links.QuestDescription;
					string text2 = expr_1DB.text;
					expr_1DB.text = string.Concat(new object[]
					{
						text2,
						current2.Item.Title,
						": ",
						current2.Count,
						this.n
					});
				}
			}
			if (this.CurrentQuest.GetType() == typeof(InventoryDetectQuest))
			{
				this.links.QuestDescription.text = "Need to find: " + this.n;
				foreach (ItemInfo current3 in (this.CurrentQuest as InventoryDetectQuest).Items)
				{
					Text expr_2B0 = this.links.QuestDescription;
					string text2 = expr_2B0.text;
					expr_2B0.text = string.Concat(new object[]
					{
						text2,
						current3.Item.Title,
						": ",
						current3.Count,
						this.n
					});
				}
			}
			if (this.CurrentQuest.GetType() == typeof(LocationQuest))
			{
				this.links.QuestDescription.text = "Go to location: " + (this.CurrentQuest as LocationQuest).Position + this.n;
			}
		}
	}

	private void StartQuest(Quest quest)
	{
		this.CurrentQuest = quest;
		if (this.CurrentQuest.HasTime)
		{
			this.CurrentQuest.CurrentTime = this.CurrentQuest.MaximumTime;
		}
	}

	private KillQuest CreateKillQuest(string title, bool hasTime, float time, params EnemyInfo[] enemies)
	{
		return new KillQuest
		{
			Enemies = enemies.ToList<EnemyInfo>(),
			Title = title,
			HasTime = hasTime,
			MaximumTime = time
		};
	}

	private CraftQuest CreateCraftQuest(string title, bool hasTime, float time, params ItemInfo[] items)
	{
		return new CraftQuest
		{
			Items = items.ToList<ItemInfo>(),
			Title = title,
			HasTime = hasTime,
			MaximumTime = time
		};
	}

	private InventoryDetectQuest CreateInventoryDetectQuest(string title, bool hasTime, float time, params ItemInfo[] items)
	{
		return new InventoryDetectQuest
		{
			Items = items.ToList<ItemInfo>(),
			Title = title,
			HasTime = hasTime,
			MaximumTime = time
		};
	}

	private LocationQuest CreateLocationQuest(string title, bool hasTime, float time, Vector3 position)
	{
		return new LocationQuest
		{
			Position = position,
			Title = title,
			HasTime = hasTime,
			MaximumTime = time
		};
	}

	public void DoKill(string enemyID, byte count = 1)
	{
		string name = SceneManager.GetActiveScene().name;
		if (name != null)
		{
			if (!(name == "Menu2"))
			{
				if (!(name == "Island"))
				{
					if (!(name == "Flat"))
					{
						if (!(name == "World"))
						{
							if (name == "Robots")
							{
								foreach (Quest current in this.RobotsQuests)
								{
									if (current.GetType() == typeof(KillQuest))
									{
										foreach (EnemyInfo current2 in (current as KillQuest).Enemies)
										{
											if (current2.EnemyID == enemyID)
											{
												current2.Count -= (int)count;
												break;
											}
										}
									}
								}
							}
						}
						else
						{
							foreach (Quest current3 in this.WorldQuests)
							{
								if (current3.GetType() == typeof(KillQuest))
								{
									foreach (EnemyInfo current4 in (current3 as KillQuest).Enemies)
									{
										if (current4.EnemyID == enemyID)
										{
											current4.Count -= (int)count;
											break;
										}
									}
								}
							}
						}
					}
					else
					{
						foreach (Quest current5 in this.FlatQuests)
						{
							if (current5.GetType() == typeof(KillQuest))
							{
								foreach (EnemyInfo current6 in (current5 as KillQuest).Enemies)
								{
									if (current6.EnemyID == enemyID)
									{
										current6.Count -= (int)count;
										break;
									}
								}
							}
						}
					}
				}
				else
				{
					foreach (Quest current7 in this.IslandQuests)
					{
						if (current7.GetType() == typeof(KillQuest))
						{
							foreach (EnemyInfo current8 in (current7 as KillQuest).Enemies)
							{
								if (current8.EnemyID == enemyID)
								{
									current8.Count -= (int)count;
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	public void DoCraft(Item itemID, int count = 1)
	{
		string name = SceneManager.GetActiveScene().name;
		if (name != null)
		{
			if (!(name == "Menu2"))
			{
				if (!(name == "Island"))
				{
					if (!(name == "Flat"))
					{
						if (!(name == "World"))
						{
							if (name == "Robots")
							{
								foreach (Quest current in this.RobotsQuests)
								{
									if (current.GetType() == typeof(CraftQuest))
									{
										foreach (ItemInfo current2 in (current as CraftQuest).Items)
										{
											if (current2.Item == itemID)
											{
												current2.Count -= count;
												break;
											}
										}
									}
								}
							}
						}
						else
						{
							foreach (Quest current3 in this.WorldQuests)
							{
								if (current3.GetType() == typeof(CraftQuest))
								{
									foreach (ItemInfo current4 in (current3 as CraftQuest).Items)
									{
										if (current4.Item == itemID)
										{
											current4.Count -= count;
											break;
										}
									}
								}
							}
						}
					}
					else
					{
						foreach (Quest current5 in this.FlatQuests)
						{
							if (current5.GetType() == typeof(CraftQuest))
							{
								foreach (ItemInfo current6 in (current5 as CraftQuest).Items)
								{
									if (current6.Item == itemID)
									{
										current6.Count -= count;
										break;
									}
								}
							}
						}
					}
				}
				else
				{
					foreach (Quest current7 in this.IslandQuests)
					{
						if (current7.GetType() == typeof(CraftQuest))
						{
							foreach (ItemInfo current8 in (current7 as CraftQuest).Items)
							{
								if (current8.Item == itemID)
								{
									current8.Count -= count;
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	public void AddToInventory(Item item, int count = 1)
	{
		string name = SceneManager.GetActiveScene().name;
		if (name != null)
		{
			if (!(name == "Menu2"))
			{
				if (!(name == "Island"))
				{
					if (!(name == "Flat"))
					{
						if (!(name == "World"))
						{
							if (name == "Robots")
							{
								foreach (Quest current in this.RobotsQuests)
								{
									if (item.GetType() == typeof(InventoryDetectQuest))
									{
										foreach (ItemInfo current2 in (current as InventoryDetectQuest).Items)
										{
											if (current2.Item == item)
											{
												current2.Count -= count;
												break;
											}
										}
									}
								}
							}
						}
						else
						{
							foreach (Quest current3 in this.WorldQuests)
							{
								if (item.GetType() == typeof(InventoryDetectQuest))
								{
									foreach (ItemInfo current4 in (current3 as InventoryDetectQuest).Items)
									{
										if (current4.Item == item)
										{
											current4.Count -= count;
											break;
										}
									}
								}
							}
						}
					}
					else
					{
						foreach (Quest current5 in this.FlatQuests)
						{
							if (item.GetType() == typeof(InventoryDetectQuest))
							{
								foreach (ItemInfo current6 in (current5 as InventoryDetectQuest).Items)
								{
									if (current6.Item == item)
									{
										current6.Count -= count;
										break;
									}
								}
							}
						}
					}
				}
				else
				{
					foreach (Quest current7 in this.IslandQuests)
					{
						if (item.GetType() == typeof(InventoryDetectQuest))
						{
							foreach (ItemInfo current8 in (current7 as InventoryDetectQuest).Items)
							{
								if (current8.Item == item)
								{
									current8.Count -= count;
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator CompleteQuest()
	{
		QuestSystem.<CompleteQuest>c__Iterator0 <CompleteQuest>c__Iterator = new QuestSystem.<CompleteQuest>c__Iterator0();
		<CompleteQuest>c__Iterator.$this = this;
		return <CompleteQuest>c__Iterator;
	}
}
