using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	private bool CanShoot = true;

	public GameObject SpawnPoint;

	public GameObject GrenadePrefab;

	private Player player;

	private AudioSource source;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot) && this.player.CanShoot && this.CanShoot && this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize > 0)
		{
			Weapon weapon = this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon;
			this.player.SubstractWeaponDurability(1);
			this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize--;
			this.player.Toolbet.Show();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.GrenadePrefab, this.SpawnPoint.transform.position, this.SpawnPoint.transform.rotation);
			this.GrenadePrefab.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 10f);
		}
	}
}
