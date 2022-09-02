using System;
using System.Threading;
using UnityEngine;

public class Shovel : MonoBehaviour
{
	public float Range = 1f;

	private bool CanDig = true;

	private Player player;

	private Ray ray;

	private RaycastHit hit;

	private AudioSource source;

	private Animation anim;

	private TerrainData mTerrainData;

	private int alphamapWidth;

	private int alphamapHeight;

	private float[,,] mSplatmapData;

	private int mNumTextures;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
		this.anim = base.GetComponent<Animation>();
		this.mTerrainData = Terrain.activeTerrain.terrainData;
		this.alphamapWidth = this.mTerrainData.alphamapWidth;
		this.alphamapHeight = this.mTerrainData.alphamapHeight;
		this.mSplatmapData = this.mTerrainData.GetAlphamaps(0, 0, this.alphamapWidth, this.alphamapHeight);
		this.mNumTextures = this.mSplatmapData.Length / (this.alphamapWidth * this.alphamapHeight);
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot) && this.player.CanUseTools)
		{
			this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
			if (Physics.Raycast(this.ray, out this.hit, this.Range))
			{
				Terrain component = this.hit.collider.GetComponent<Terrain>();
				Construction component2 = this.hit.collider.GetComponent<Construction>();
				AnimalAI component3 = this.hit.collider.GetComponent<AnimalAI>();
				RobotAI component4 = this.hit.collider.GetComponent<RobotAI>();
				if (this.CanDig)
				{
					if (component != null)
					{
						this.player.MainInventory.Add("sand", (this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
						int activeTerrainTextureIdx = this.GetActiveTerrainTextureIdx(this.hit.point);
						if (activeTerrainTextureIdx == 6)
						{
							this.source.pitch = this.player.ToolsSpeed;
							this.source.Play();
							this.anim["Dig"].speed = this.player.ToolsSpeed;
							this.anim.Play("Dig");
							new Thread(new ThreadStart(this.Wait)).Start();
							this.player.MainInventory.Add("flint", (this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
							this.player.SubstractToolDurability(1);
						}
						else if (activeTerrainTextureIdx == 1)
						{
							this.source.pitch = this.player.ToolsSpeed;
							this.source.Play();
							this.anim["Dig"].speed = this.player.ToolsSpeed;
							this.anim.Play("Dig");
							new Thread(new ThreadStart(this.Wait)).Start();
							this.player.MainInventory.Add("grass", 1);
							this.player.SubstractToolDurability(1);
						}
					}
				}
				else if (component2 != null)
				{
					this.source.pitch = this.player.ToolsSpeed;
					this.source.Play();
					this.anim["Dig"].speed = this.player.ToolsSpeed;
					this.anim.Play("Dig");
					new Thread(new ThreadStart(this.Wait)).Start();
					component2.GetDamage((float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
				else if (component3 != null)
				{
					this.source.pitch = this.player.ToolsSpeed;
					this.source.Play();
					this.anim["Dig"].speed = this.player.ToolsSpeed;
					this.anim.Play("Dig");
					new Thread(new ThreadStart(this.Wait)).Start();
					component3.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
				else if (component4 != null)
				{
					this.source.pitch = this.player.ToolsSpeed;
					this.source.Play();
					this.anim["Dig"].speed = this.player.ToolsSpeed;
					this.anim.Play("Dig");
					new Thread(new ThreadStart(this.Wait)).Start();
					component4.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
			}
		}
	}

	private void Wait()
	{
		this.CanDig = false;
		Thread.Sleep((int)(1000f / this.player.ToolsSpeed));
		this.CanDig = true;
	}

	private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos)
	{
		Vector3 result = default(Vector3);
		Terrain activeTerrain = Terrain.activeTerrain;
		Vector3 position = activeTerrain.transform.position;
		result.x = (playerPos.x - position.x) / activeTerrain.terrainData.size.x * (float)activeTerrain.terrainData.alphamapWidth;
		result.z = (playerPos.z - position.z) / activeTerrain.terrainData.size.z * (float)activeTerrain.terrainData.alphamapHeight;
		return result;
	}

	private int GetActiveTerrainTextureIdx(Vector3 pos)
	{
		Vector3 vector = this.ConvertToSplatMapCoordinate(pos);
		int result = 0;
		float num = 0f;
		for (int i = 0; i < this.mNumTextures; i++)
		{
			if (num < this.mSplatmapData[(int)vector.z, (int)vector.x, i])
			{
				result = i;
			}
		}
		return result;
	}
}
