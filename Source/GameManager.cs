using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	private sealed class <>c
	{
		public static readonly GameManager.<>c <>9 = new GameManager.<>c();

		public static UnityAction <>9__9_0;

		internal void <InitDeath>b__9_0()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().Respawn();
		}
	}

	[CompilerGenerated]
	[Serializable]
	private sealed class <>c__0
	{
		public static readonly GameManager.<>c__0 <>9 = new GameManager.<>c__0();

		public static UnityAction <>9__0_0;

		internal void <InitDeath>b__0_0()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().Respawn();
		}
	}

	[CompilerGenerated]
	[Serializable]
	private sealed class <>c__1
	{
		public static readonly GameManager.<>c__1 <>9 = new GameManager.<>c__1();

		public static UnityAction <>9__0_0;

		internal void <InitDeath>b__0_0()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().Respawn();
		}
	}

	public GameObject DeadPlayer;

	public GameObject UI;

	private string XMLPath = string.Empty;

	private static string JSONPath;

	private static string FilePath;

	private SceneLinks links;

	[HideInInspector]
	public PlayerStats stats;

	[HideInInspector]
	public Buffs buffs;

	[HideInInspector]
	public Craft craft;

	[HideInInspector]
	public Player player;

	private QuestSystem system;

	private SavingData data;

	private bool Loaded;

	private bool doneInjection;

	private void Awake()
	{
		this.XMLPath = Application.dataPath + "/Saves/";
		GameManager.JSONPath = Application.dataPath + "/Configs/";
		GameManager.FilePath = Application.dataPath + "/Saves/SavedData.xml";
	}

	private void Start()
	{
		this.UI.SetActive(false);
		this.stats = base.GetComponent<PlayerStats>();
		this.buffs = base.GetComponent<Buffs>();
		this.craft = base.GetComponent<Craft>();
		this.system = base.GetComponent<QuestSystem>();
		this.links = base.GetComponent<SceneLinks>();
		SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.SceneManager_sceneLoaded);
		SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.SceneManager_sceneLoaded);
	}

	public void Save()
	{
		if (SceneManager.GetActiveScene().name == "Menu2")
		{
			return;
		}
		if (this.stats == null || this.craft == null)
		{
			return;
		}
		if (this.player == null)
		{
			return;
		}
		SavingData savingData = new SavingData
		{
			CurrentHead = this.stats.CurrentHead,
			CurrentBody = this.stats.CurrentBody,
			CurrentLeftArm = this.stats.CurrentLeftArm,
			CurrentRightArm = this.stats.CurrentRightArm,
			CurrentLeftLeg = this.stats.CurrentLeftLeg,
			CurrentRightLeg = this.stats.CurrentRightLeg,
			CurrentFood = this.stats.CurrentFood,
			CurrentWater = this.stats.CurrentWater,
			CurrentRadiation = this.stats.CurrentRadiation,
			CurrentPiss = this.stats.CurrentPiss,
			CurrentStamina = this.stats.CurrentStamina,
			CurrentBuffs = this.buffs.CurrentBuffs,
			FoodReductionK = this.stats.FoodReductionK,
			SceneTitle = SceneManager.GetActiveScene().name
		};
		if (this.player.sky != null)
		{
			savingData.Year = this.player.sky.Cycle.Year;
			savingData.Month = this.player.sky.Cycle.Month;
			savingData.Day = this.player.sky.Cycle.Day;
			savingData.Hour = this.player.sky.Cycle.Hour;
		}
		foreach (CraftInfo current in this.craft.currentCrafts)
		{
			if (!current.Startup)
			{
				savingData.CurrentCrafts.Add(current.Result.Item.Id);
			}
		}
		foreach (Slot current2 in this.player.Toolbet.InventoryArray)
		{
			SavingSlot savingSlot = new SavingSlot();
			if (current2.Item == null)
			{
				savingSlot.Item = string.Empty;
			}
			else
			{
				savingSlot.Item = current2.Item.Id;
			}
			savingSlot.Count = current2.Count;
			savingSlot.CurrentClipSize = current2.CurrentClipSize;
			savingSlot.CurrentDurability = current2.CurrentDurability;
			savingData.Toolbet.Add(savingSlot);
		}
		foreach (Slot current3 in this.player.Cloths.InventoryArray)
		{
			SavingSlot savingSlot2 = new SavingSlot();
			if (current3.Item == null)
			{
				savingSlot2.Item = string.Empty;
			}
			else
			{
				savingSlot2.Item = current3.Item.Id;
			}
			savingSlot2.Count = current3.Count;
			savingSlot2.CurrentClipSize = current3.CurrentClipSize;
			savingSlot2.CurrentDurability = current3.CurrentDurability;
			savingData.Cloths.Add(savingSlot2);
		}
		foreach (Slot current4 in this.player.MainInventory.InventoryArray)
		{
			SavingSlot savingSlot3 = new SavingSlot();
			if (current4.Item == null)
			{
				savingSlot3.Item = string.Empty;
			}
			else
			{
				savingSlot3.Item = current4.Item.Id;
			}
			savingSlot3.Count = current4.Count;
			savingSlot3.CurrentClipSize = current4.CurrentClipSize;
			savingSlot3.CurrentDurability = current4.CurrentDurability;
			savingData.Inventory.Add(savingSlot3);
		}
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			Construction component = gameObject.GetComponent<Construction>();
			if (component && component.NeedSave)
			{
				SavingConstructions savingConstructions = new SavingConstructions();
				if (component.name.Contains("Clone"))
				{
					savingConstructions.ID = component.name.Substring(0, component.name.IndexOf("("));
				}
				else
				{
					savingConstructions.ID = component.name;
				}
				savingConstructions.Position = component.transform.position;
				savingConstructions.Rotation = component.transform.rotation;
				Inventory[] components = gameObject.GetComponents<Inventory>();
				for (int j = 0; j < components.Length; j++)
				{
					Inventory arg_4D3_0 = components[j];
					List<SavingSlot> list = new List<SavingSlot>();
					foreach (Slot current5 in arg_4D3_0.InventoryArray)
					{
						SavingSlot savingSlot4 = new SavingSlot();
						if (current5.Item == null)
						{
							savingSlot4.Item = string.Empty;
						}
						else
						{
							savingSlot4.Item = current5.Item.Id;
						}
						savingSlot4.Count = current5.Count;
						savingSlot4.CurrentClipSize = current5.CurrentClipSize;
						savingSlot4.CurrentDurability = current5.CurrentDurability;
						list.Add(savingSlot4);
					}
					savingConstructions.Inventories.Add(list);
				}
				savingData.Constructions.Add(savingConstructions);
			}
		}
		savingData.PlayerPosition = this.player.transform.position;
		this.SaveXml(savingData);
	}

	public void Load()
	{
		this.data = this.LoadXml();
		if (this.data != null)
		{
			SceneManager.LoadScene(this.data.SceneTitle);
		}
	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
	{
		string name = scene.name;
		if (name != null)
		{
			if (!(name == "Menu2"))
			{
				if (name == "Island")
				{
					this.UI.SetActive(true);
					this.OnStart();
					return;
				}
				if (name == "Flat")
				{
					this.UI.SetActive(false);
					return;
				}
				if (name == "World")
				{
					this.UI.SetActive(true);
					this.OnStart();
					return;
				}
				if (name == "Robots")
				{
					this.UI.SetActive(true);
					this.OnStart();
					return;
				}
			}
			else
			{
				this.UI.SetActive(false);
			}
		}
	}

	public void LoadData()
	{
		if (this.Loaded)
		{
			return;
		}
		if (this.data == null)
		{
			return;
		}
		this.stats.CurrentHead = this.data.CurrentHead;
		this.stats.CurrentBody = this.data.CurrentBody;
		this.stats.CurrentLeftArm = this.data.CurrentLeftArm;
		this.stats.CurrentRightArm = this.data.CurrentRightArm;
		this.stats.CurrentLeftLeg = this.data.CurrentLeftLeg;
		this.stats.CurrentRightLeg = this.data.CurrentRightLeg;
		this.stats.CurrentFood = this.data.CurrentFood;
		this.stats.CurrentWater = this.data.CurrentWater;
		this.stats.CurrentRadiation = this.data.CurrentRadiation;
		this.stats.CurrentPiss = this.data.CurrentPiss;
		this.stats.CurrentStamina = this.data.CurrentStamina;
		this.stats.FoodReductionK = this.data.FoodReductionK;
		if (this.player.sky != null)
		{
			this.player.sky.Cycle.Year = this.data.Year;
			this.player.sky.Cycle.Month = this.data.Month;
			this.player.sky.Cycle.Day = this.data.Day;
			this.player.sky.Cycle.Hour = this.data.Hour;
		}
		for (int i = 0; i < this.data.CurrentBuffs.Count; i++)
		{
			this.buffs.AddBuff(this.data.CurrentBuffs[i].Buff.Id, this.data.CurrentBuffs[i].CurrentTime);
		}
		foreach (string current in this.data.CurrentCrafts)
		{
			this.craft.currentCrafts.Add(Database.GetCraftInfoByID(current));
		}
		using (List<Slot>.Enumerator enumerator2 = this.player.Toolbet.InventoryArray.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				enumerator2.Current.Item = null;
			}
		}
		foreach (SavingSlot current2 in this.data.Toolbet)
		{
			Slot slot;
			if (current2.Item == string.Empty)
			{
				slot = new Slot(null);
			}
			else
			{
				slot = new Slot(Database.Get(current2.Item));
			}
			slot.Count = current2.Count;
			slot.CurrentClipSize = current2.CurrentClipSize;
			slot.CurrentDurability = current2.CurrentDurability;
			slot.Count = current2.Count;
			this.player.Toolbet.Add(slot, slot.Count);
		}
		this.player.Toolbet.Show();
		using (List<Slot>.Enumerator enumerator2 = this.player.MainInventory.InventoryArray.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				enumerator2.Current.Item = null;
			}
		}
		foreach (SavingSlot current3 in this.data.Inventory)
		{
			Slot slot2;
			if (current3.Item == string.Empty)
			{
				slot2 = new Slot(null);
			}
			else
			{
				slot2 = new Slot(Database.Get(current3.Item));
			}
			slot2.Count = current3.Count;
			slot2.CurrentClipSize = current3.CurrentClipSize;
			slot2.CurrentDurability = current3.CurrentDurability;
			this.player.MainInventory.Add(slot2, slot2.Count);
		}
		this.player.MainInventory.Show();
		using (List<Slot>.Enumerator enumerator2 = this.player.Cloths.InventoryArray.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				enumerator2.Current.Item = null;
			}
		}
		foreach (SavingSlot current4 in this.data.Cloths)
		{
			Slot slot3;
			if (current4.Item == string.Empty)
			{
				slot3 = new Slot(null);
			}
			else
			{
				slot3 = new Slot(Database.Get(current4.Item));
			}
			slot3.Count = current4.Count;
			slot3.CurrentClipSize = current4.CurrentClipSize;
			slot3.CurrentDurability = current4.CurrentDurability;
			this.player.Cloths.Add(slot3, slot3.Count);
		}
		this.player.Cloths.Show();
		foreach (SavingConstructions current5 in this.data.Constructions)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemBuildings/" + current5.ID), current5.Position, current5.Rotation);
			for (int j = 0; j < current5.Inventories.Count; j++)
			{
				for (int k = 0; k < current5.Inventories[j].Count; k++)
				{
					Slot item = new Slot(Database.Get(current5.Inventories[j][k].Item))
					{
						CurrentClipSize = current5.Inventories[j][k].CurrentClipSize,
						Count = current5.Inventories[j][k].Count,
						CurrentDurability = current5.Inventories[j][k].CurrentDurability
					};
					gameObject.GetComponents<Inventory>()[j].InventoryArray.Add(item);
				}
			}
		}
		this.player.transform.position = this.data.PlayerPosition;
		this.Loaded = true;
		base.CancelInvoke();
		base.InvokeRepeating("Save", 120f, 120f);
	}

	public void OnStart()
	{
		if (GameObject.FindGameObjectWithTag("SceneManager") != null)
		{
			this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			if (this.links.PlayerSpawnPointsObject != null)
			{
				IEnumerator enumerator = this.links.PlayerSpawnPointsObject.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform item = (Transform)enumerator.Current;
						this.links.PlayerSpawnPoints.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			this.stats.OnStart(this.links);
			this.craft.OnStart(this.links);
			this.buffs.OnStart();
			this.system.OnStart(this.links);
			this.player.OnStart(this.links);
			base.GetComponents<Inventory>()[0].OnStart();
			base.GetComponents<Inventory>()[1].OnStart();
			base.GetComponents<Inventory>()[2].OnStart();
		}
		this.LoadData();
	}

	public void InitDeath(GameObject player)
	{
		this.links.DeathUI.SetActive(true);
		this.DeadPlayer = player;
		player.SetActive(false);
		if (!this.doneInjection)
		{
			this.doneInjection = true;
			GameObject.Find("UI/DeathScreen/Exit/Text").GetComponent<Text>().text = "Exit from this game";
			GameObject.Find("UI/DeathScreen/Respawn").GetComponent<Button>().onClick.AddListener(delegate
			{
				this.Respawn();
			});
		}
	}

	public void Drop()
	{
		this.player.GetComponent<Player>().Drop(int.Parse(this.links.DropCountField.text));
	}

	public void Drop2()
	{
		this.player.GetComponent<Player>().Drop(0);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void Respawn()
	{
		this.DeadPlayer.SetActive(true);
		Vector3 position = new Vector3(0f, 20f, 0f);
		if (this.links.PlayerSpawnPoints.Count > 0)
		{
			position = this.links.PlayerSpawnPoints[UnityEngine.Random.Range(0, this.links.PlayerSpawnPoints.Count)].position;
		}
		this.DeadPlayer.transform.position = position;
		this.links.DeathUI.SetActive(false);
		this.DeadPlayer.GetComponent<PlayerStats>().Restore();
	}

	public void CallAirDrop(Vector3 position)
	{
		this.links.AirDropShip.transform.position = new Vector3(1500f, 500f, 1500f);
		position.y = this.links.AirDropShip.transform.position.y;
		this.links.AirDropShip.transform.LookAt(position);
		this.links.AirDropShip.GetComponent<AirdropShip>().Target = position;
		this.links.AirDropShip.GetComponent<AirdropShip>().IsMoving = true;
		this.links.AirDropShip.GetComponent<AirdropShip>().DoAirDrop = true;
	}

	public static void ClearSaves()
	{
		if (File.Exists(GameManager.FilePath))
		{
			File.Delete(GameManager.FilePath);
		}
	}

	private void SaveXml(SavingData data)
	{
		if (!Directory.Exists(this.XMLPath))
		{
			Directory.CreateDirectory(this.XMLPath);
		}
		XmlSerializer arg_4E_0 = new XmlSerializer(typeof(SavingData));
		if (File.Exists(GameManager.FilePath))
		{
			File.Delete(GameManager.FilePath);
		}
		FileStream fileStream = new FileStream(GameManager.FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
		arg_4E_0.Serialize(fileStream, data);
		fileStream.Close();
	}

	private SavingData LoadXml()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SavingData));
		if (!Directory.Exists(this.XMLPath))
		{
			return null;
		}
		if (!File.Exists(GameManager.FilePath))
		{
			return null;
		}
		FileStream fileStream = new FileStream(GameManager.FilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
		if (fileStream.Length == 0L)
		{
			fileStream.Close();
			return null;
		}
		SavingData arg_5D_0 = (SavingData)xmlSerializer.Deserialize(fileStream);
		fileStream.Close();
		return arg_5D_0;
	}

	public static ChestConfig LoadJson(string name)
	{
		TextAsset textAsset = Resources.Load<TextAsset>("Configs/" + name);
		return JsonUtility.FromJson<ChestConfig>(Encoding.ASCII.GetString(textAsset.bytes));
	}

	public static void SaveJson(string name, ChestConfig config)
	{
		if (!Directory.Exists(GameManager.JSONPath))
		{
			Directory.CreateDirectory(GameManager.JSONPath);
		}
		FileStream arg_3F_0 = new FileStream(GameManager.JSONPath + name + ".json", FileMode.Create, FileAccess.Write);
		byte[] bytes = Encoding.ASCII.GetBytes(JsonUtility.ToJson(config));
		arg_3F_0.Write(bytes, 0, bytes.Length);
		arg_3F_0.Close();
	}

	private void OnApplicationPause(bool pause)
	{
	}

	private void OnApplicationQuit()
	{
		this.Save();
	}

	static GameManager()
	{
		GameManager.JSONPath = string.Empty;
		GameManager.FilePath = string.Empty;
	}
}
