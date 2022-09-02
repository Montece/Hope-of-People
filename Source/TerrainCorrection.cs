using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCorrection : MonoBehaviour
{
	public GameObject[] Trees;

	public GameObject[] Terrains;

	private List<TerrainData> TerrainsDatas = new List<TerrainData>();

	private List<TreeInstance[]> backup = new List<TreeInstance[]>();

	public bool ChangeTrees;

	private LootManager lmanager;

	private void Start()
	{
		if (!this.ChangeTrees)
		{
			return;
		}
		this.lmanager = base.GetComponent<LootManager>();
		if (this.Terrains.Length == 0)
		{
			this.TerrainsDatas.Add(Terrain.activeTerrain.terrainData);
		}
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.TerrainsDatas.Add(this.Terrains[i].GetComponent<Terrain>().terrainData);
		}
		for (int j = 0; j < this.Terrains.Length; j++)
		{
			this.backup.Add(this.TerrainsDatas[j].treeInstances);
			for (int k = 0; k < this.TerrainsDatas[j].treeInstances.Length; k++)
			{
				Vector3 position = Vector3.Scale(this.TerrainsDatas[j].treeInstances[k].position, this.TerrainsDatas[j].size) + this.Terrains[j].transform.position;
				GameObject original = this.Trees[UnityEngine.Random.Range(0, this.Trees.Length)];
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, position, Quaternion.identity);
				gameObject.transform.parent = this.Terrains[j].transform;
				this.lmanager.AllTrees.Add(gameObject);
			}
			TreeInstance[] treeInstances = new TreeInstance[0];
			this.TerrainsDatas[j].treeInstances = treeInstances;
		}
	}

	private void OnApplicationQuit()
	{
		if (!this.ChangeTrees)
		{
			return;
		}
		Debug.Log("Terrain hsa been restored.");
		if (this.backup != null)
		{
			for (int i = 0; i < this.TerrainsDatas.Count; i++)
			{
				this.Terrains[i].GetComponent<Terrain>().terrainData.treeInstances = this.backup[i];
			}
		}
	}
}
