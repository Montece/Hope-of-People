using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootManager : MonoBehaviour
{
	public GameObject LootPoints;

	public GameObject[] Chests;

	public float RepeatTime = 1800f;

	public GameObject[] EnviromentLoot;

	public GameObject[] Terrains;

	public GameObject EnviromentLootHolder;

	public int EnviromentLootCount = 5;

	private float Width;

	private float Length;

	private float terrainPosX;

	private float terrainPosZ;

	private GameObject loot;

	[HideInInspector]
	public List<GameObject> AllChests = new List<GameObject>();

	[HideInInspector]
	public List<GameObject> AllTrees = new List<GameObject>();

	[HideInInspector]
	public List<GameObject> AllRocks = new List<GameObject>();

	private void Start()
	{
		this.loot = new GameObject
		{
			name = "Loot"
		};
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.Width = (float)((int)this.Terrains[i].GetComponent<Terrain>().terrainData.size.x);
			this.Length = (float)((int)this.Terrains[i].GetComponent<Terrain>().terrainData.size.z);
			this.terrainPosX = (float)((int)this.Terrains[i].transform.position.x);
			this.terrainPosZ = (float)((int)this.Terrains[i].GetComponent<Terrain>().transform.position.z);
			if (this.RepeatTime > 0f)
			{
				base.InvokeRepeating("Spawn", 0f, this.RepeatTime);
			}
			for (int j = 0; j < this.EnviromentLootCount; j++)
			{
				this.SpawnRandomEnviroment(i);
			}
		}
	}

	public void Spawn()
	{
		List<GameObject> list = new List<GameObject>();
		LootPoint[] array = UnityEngine.Object.FindObjectsOfType<LootPoint>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.SetParent(this.LootPoints.transform);
		}
		IEnumerator enumerator = this.LootPoints.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.gameObject.SetActive(false);
				if (transform.GetComponent<LootPoint>() && transform.GetComponent<LootPoint>().Chest == null)
				{
					ChestLootType lootType = transform.GetComponent<LootPoint>().LootType;
					list.Clear();
					GameObject[] chests = this.Chests;
					for (int j = 0; j < chests.Length; j++)
					{
						GameObject gameObject = chests[j];
						if (lootType == gameObject.GetComponent<Inventory>().ChestLoot)
						{
							list.Add(gameObject);
						}
					}
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(list[UnityEngine.Random.Range(0, list.Count)]);
					Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
					gameObject2.transform.position = position;
					transform.GetComponent<LootPoint>().Chest = gameObject2;
					gameObject2.GetComponent<Construction>().NeedSave = false;
					gameObject2.transform.SetParent(this.loot.transform);
					this.AllChests.Add(gameObject2);
				}
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

	public void SpawnRandomEnviroment(int j)
	{
		if (this.EnviromentLootHolder != null)
		{
			float x = UnityEngine.Random.Range(this.terrainPosX, this.terrainPosX + this.Width);
			float z = UnityEngine.Random.Range(this.terrainPosZ, this.terrainPosZ + this.Length);
			float y = this.Terrains[j].GetComponent<Terrain>().SampleHeight(new Vector3(x, 0f, z));
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EnviromentLoot[UnityEngine.Random.Range(0, this.EnviromentLoot.Length)], new Vector3(x, y, z), Quaternion.identity);
			gameObject.transform.SetParent(this.EnviromentLootHolder.transform);
			if (gameObject.GetComponent<TreeMechs>())
			{
				this.AllTrees.Add(gameObject);
			}
			if (gameObject.GetComponent<RockMechs>())
			{
				this.AllRocks.Add(gameObject);
			}
		}
	}

	public Vector3 GetRandomPoint()
	{
		float x = UnityEngine.Random.Range(this.terrainPosX, this.terrainPosX + this.Width);
		float z = UnityEngine.Random.Range(this.terrainPosZ, this.terrainPosZ + this.Length);
		float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0f, z));
		return new Vector3(x, y, z);
	}

	public GameObject GetNearestChest(Vector3 pos)
	{
		return (from x in this.AllChests
		orderby Vector3.Distance(pos, x.transform.position)
		select x).FirstOrDefault<GameObject>();
	}

	public GameObject GetNearestTree(Vector3 pos)
	{
		return (from x in this.AllTrees
		orderby Vector3.Distance(pos, x.transform.position)
		select x).FirstOrDefault<GameObject>();
	}

	public GameObject GetNearestRock(Vector3 pos)
	{
		return (from x in this.AllRocks
		orderby Vector3.Distance(pos, x.transform.position)
		select x).FirstOrDefault<GameObject>();
	}
}
