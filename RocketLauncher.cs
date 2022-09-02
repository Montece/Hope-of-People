using System;
using System.Threading;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
	private bool CanShoot = true;

	public int RocketSpeed;

	public GameObject Flash;

	public GameObject SpawnPoint;

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
				Weapon weapon = this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon;
				this.Flash.GetComponent<DisableTimer>().Disable();
				this.source.clip = this.ShootSound;
				this.source.Play();
				this.player.SubstractWeaponDurability(1);
				this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize--;
				this.player.Toolbet.Show();
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((Database.Get(weapon.Ammo.Id) as Ammo).Prefab, this.SpawnPoint.transform.position, this.SpawnPoint.transform.rotation);
				gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * (float)this.RocketSpeed);
			}
		}
		else if (Input.GetKeyDown(this.player.Reload))
		{
			Slot slot = this.player.Toolbet.InventoryArray[this.player.EquipedToolbet];
			Weapon weapon2 = this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon;
			int id = 0;
			Inventory owner = null;
			if (slot.CurrentClipSize < weapon2.ClipSize && this.player.HasAmmo(weapon2, ref id, ref owner))
			{
				this.Reload(slot, weapon2, id, owner);
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
		this.source.clip = this.ReloadSound;
		this.source.Play();
		new Thread(new ThreadStart(this.WaitReload)).Start();
		thisws.CurrentClipSize = thiswi.ClipSize;
		this.player.Toolbet.Show();
		owner.Show();
		this.CanShoot = true;
	}
}
