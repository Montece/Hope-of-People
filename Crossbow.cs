using System;
using System.Threading;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
	public float Range = 500f;

	private bool CanShoot = true;

	private Player player;

	private Ray ray;

	private RaycastHit hit;

	private AudioSource source;

	private Animation anim;

	public AudioClip ShootSound;

	public AudioClip ReloadSound;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
		this.anim = base.GetComponent<Animation>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot))
		{
			if (this.player.CanShoot && this.CanShoot && this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize > 0)
			{
				new Thread(new ThreadStart(this.Wait)).Start();
				this.source.clip = this.ShootSound;
				this.source.Play();
				this.player.SubstractWeaponDurability(1);
				this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize--;
				this.player.Toolbet.Show();
				this.anim.Play("CrossbowFire");
				this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
				if (Physics.Raycast(this.ray, out this.hit, this.Range))
				{
					RobotAI component = this.hit.collider.GetComponent<RobotAI>();
					if (component != null)
					{
						component.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon).Damage * this.player.WeaponDamageK);
					}
					AnimalAI component2 = this.hit.collider.GetComponent<AnimalAI>();
					if (component2 != null)
					{
						component2.GetDamage(this.player.gameObject, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon).Damage * this.player.WeaponDamageK);
					}
					Construction component3 = this.hit.collider.GetComponent<Construction>();
					if (component3 != null)
					{
						component3.GetDamage((float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon).Damage * this.player.WeaponDamageK);
					}
					BodyPart component4 = this.hit.collider.GetComponent<BodyPart>();
					if (component4 != null)
					{
						this.hit.collider.GetComponentInParent<PlayerStats>().GetDamage(component4.Type, (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon).Damage * this.player.WeaponDamageK);
					}
				}
			}
		}
		else if (Input.GetKeyDown(this.player.Reload))
		{
			Slot slot = this.player.Toolbet.InventoryArray[this.player.EquipedToolbet];
			Weapon weapon = this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon;
			int id = -1;
			Inventory owner = null;
			if (slot.CurrentClipSize < weapon.ClipSize && this.player.HasAmmo(weapon, ref id, ref owner))
			{
				this.Reload(slot, weapon, id, owner);
			}
		}
	}

	private void Wait()
	{
		this.CanShoot = false;
		Thread.Sleep(1000);
		this.CanShoot = true;
	}

	private void WaitReload()
	{
		this.CanShoot = false;
		Thread.Sleep(1200);
		this.CanShoot = true;
	}

	private void Reload(Slot thisws, Weapon thiswi, int id, Inventory owner)
	{
		this.CanShoot = false;
		this.source.clip = this.ReloadSound;
		this.source.Play();
		new Thread(new ThreadStart(this.WaitReload)).Start();
		thisws.CurrentClipSize = thiswi.ClipSize;
		this.player.Toolbet.Show();
		owner.Show();
		owner.Destroy(1, id);
		this.CanShoot = true;
	}
}
