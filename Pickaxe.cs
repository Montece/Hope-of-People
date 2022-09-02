using System;
using System.Threading;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
	public float Range = 1f;

	private bool CanHarvest = true;

	private Player player;

	private Ray ray;

	private RaycastHit hit;

	private AudioSource source;

	private Animation anim;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
		this.anim = base.GetComponent<Animation>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot) && this.player.CanUseTools)
		{
			this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
			if (Physics.Raycast(this.ray, out this.hit, this.Range))
			{
				RockMechs component = this.hit.collider.GetComponent<RockMechs>();
				Construction component2 = this.hit.collider.GetComponent<Construction>();
				AnimalAI component3 = this.hit.collider.GetComponent<AnimalAI>();
				RobotAI component4 = this.hit.collider.GetComponent<RobotAI>();
				if (component != null && this.CanHarvest)
				{
					this.source.Play();
					this.source.pitch = this.player.ToolsSpeed;
					this.anim["Harvest"].speed = this.player.ToolsSpeed;
					this.anim.Play("Harvest");
					new Thread(new ThreadStart(this.Wait)).Start();
					component.Harvest(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool, this.player);
					this.player.SubstractToolDurability(1);
				}
				else if (component2 != null && this.CanHarvest)
				{
					this.source.Play();
					this.source.pitch = this.player.ToolsSpeed;
					this.anim["Harvest"].speed = this.player.ToolsSpeed;
					this.anim.Play("Harvest");
					new Thread(new ThreadStart(this.Wait)).Start();
					component2.GetDamage((float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
				else if (component3 != null)
				{
					this.source.Play();
					this.source.pitch = this.player.ToolsSpeed;
					this.anim["Harvest"].speed = this.player.ToolsSpeed;
					this.anim.Play("Harvest");
					new Thread(new ThreadStart(this.Wait)).Start();
					component3.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
				else if (component4 != null)
				{
					this.source.Play();
					this.source.pitch = this.player.ToolsSpeed;
					this.anim["Harvest"].speed = this.player.ToolsSpeed;
					this.anim.Play("Harvest");
					new Thread(new ThreadStart(this.Wait)).Start();
					component4.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
			}
		}
	}

	private void Wait()
	{
		this.CanHarvest = false;
		Thread.Sleep((int)(1050f / this.player.ToolsSpeed));
		this.CanHarvest = true;
	}
}
