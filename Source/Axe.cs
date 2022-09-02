using System;
using System.Threading;
using UnityEngine;

public class Axe : MonoBehaviour
{
	public float Range = 1f;

	private bool CanChop = true;

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
				TreeMechs component = this.hit.collider.GetComponent<TreeMechs>();
				Construction component2 = this.hit.collider.GetComponent<Construction>();
				AnimalAI component3 = this.hit.collider.GetComponent<AnimalAI>();
				RobotAI component4 = this.hit.collider.GetComponent<RobotAI>();
				if (this.CanChop)
				{
					if (component != null)
					{
						this.source.pitch = this.player.ToolsSpeed;
						this.source.Play();
						this.anim["Chop"].speed = this.player.ToolsSpeed;
						this.anim.Play("Chop");
						new Thread(new ThreadStart(this.Wait)).Start();
						component.Chop(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool, this.player);
						this.player.SubstractToolDurability(1);
					}
				}
				else if (component2 != null)
				{
					this.source.pitch = this.player.ToolsSpeed;
					this.source.Play();
					this.anim["Chop"].speed = this.player.ToolsSpeed;
					this.anim.Play("Chop");
					new Thread(new ThreadStart(this.Wait)).Start();
					component2.GetDamage((float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
				else if (component3 != null)
				{
					this.source.pitch = this.player.ToolsSpeed;
					this.source.Play();
					this.anim["Chop"].speed = this.player.ToolsSpeed;
					this.anim.Play("Chop");
					new Thread(new ThreadStart(this.Wait)).Start();
					component3.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
				else if (component4 != null)
				{
					this.source.pitch = this.player.ToolsSpeed;
					this.source.Play();
					this.anim["Chop"].speed = this.player.ToolsSpeed;
					this.anim.Play("Chop");
					new Thread(new ThreadStart(this.Wait)).Start();
					component4.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Tool).Efficiency);
					this.player.SubstractToolDurability(2);
				}
			}
		}
	}

	private void Wait()
	{
		this.CanChop = false;
		Thread.Sleep((int)(1000f / this.player.ToolsSpeed));
		this.CanChop = true;
	}
}
