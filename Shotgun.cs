using System;
using System.Threading;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
	public float Range = 10f;

	private bool CanShoot = true;

	public GameObject Flash;

	private Player player;

	private Ray ray;

	private RaycastHit hit;

	private AudioSource source;

	private Animation anim;

	public AudioClip ShootSound;

	public AudioClip ReloadSound1;

	public AudioClip ReloadSound2;

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
				this.Flash.GetComponent<DisableTimer>().Disable();
				this.source.clip = this.ShootSound;
				this.source.Play();
				this.player.SubstractWeaponDurability(1);
				this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize--;
				this.player.Toolbet.Show();
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
			int id = 0;
			Inventory owner = null;
			if (slot.CurrentClipSize < weapon.ClipSize && this.player.HasAmmo(weapon, ref id, ref owner))
			{
				this.Reload(slot, weapon, id, owner);
			}
		}
	}

	private void WaitReload()
	{
		Thread.Sleep(500);
	}

	private void Reload(Slot thisws, Weapon thiswi, int id, Inventory owner)
	{
		this.CanShoot = false;
		owner.Destroy(1, id);
		for (int i = 0; i < thiswi.ClipSize; i++)
		{
			this.source.clip = this.ReloadSound1;
			this.source.Play();
			new Thread(new ThreadStart(this.WaitReload)).Start();
			thisws.CurrentClipSize++;
			this.player.Toolbet.Show();
			owner.Show();
		}
		this.source.clip = this.ReloadSound2;
		this.source.Play();
		this.CanShoot = true;
	}
}
