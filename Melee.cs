using System;
using System.Threading;
using UnityEngine;

public class Melee : MonoBehaviour
{
	public float Range = 3f;

	private bool CanAttack = true;

	private Player player;

	private Ray ray;

	private RaycastHit hit;

	private AudioSource source;

	private Animation anim;

	public AudioClip HitSound;

	public AudioClip AttackSound;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
		this.anim = base.GetComponent<Animation>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot) && this.player.CanShoot && this.CanAttack && this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentDurability > 0f)
		{
			new Thread(new ThreadStart(this.Wait)).Start();
			this.source.clip = this.AttackSound;
			this.source.Play();
			this.player.Toolbet.Show();
			this.anim.Play("Attack2");
			this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
			if (Physics.Raycast(this.ray, out this.hit, this.Range))
			{
				this.source.clip = this.HitSound;
				this.source.Play();
				this.player.SubstractWeaponDurability(1);
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

	private void Wait()
	{
		this.CanAttack = false;
		Thread.Sleep(800);
		this.CanAttack = true;
	}
}
