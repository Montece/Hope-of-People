using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ShootingSystem : MonoBehaviour
{
	[Header("WEAPON"), Space]
	public ShootingType Type;

	public float Speed = 5000f;

	public float BulletRecoil = 10f;

	public float CameraRecoil = 10f;

	public float AimSpeed = 40f;

	public AudioClip ShootSound;

	public AudioClip ReloadSound;

	public bool HasSniperScope;

	private bool CanShoot = true;

	private Vector3 FirstP = new Vector3(0.436f, -0.239f, 0.581f);

	private Vector3 SecondP = new Vector3(0f, -0.239f, 0.581f);

	private Quaternion FirstR;

	private Quaternion SecondR;

	private Player player;

	private AudioSource source;

	private GameObject Bullet;

	private Light Flash;

	private Transform SpawnZone;

	private Camera PlayerCamera;

	private Camera Scope;

	private GameObject ScopeUI;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
		this.source = base.GetComponent<AudioSource>();
		this.Bullet = Resources.Load<GameObject>("Bullet");
		this.Flash = base.GetComponentInChildren<Light>();
		this.SpawnZone = base.transform.GetChild(0);
		this.PlayerCamera = Camera.main;
		this.Scope = GameObject.FindGameObjectWithTag("Scope").GetComponent<Camera>();
		this.ScopeUI = GameObject.Find("UI").transform.Find("Scope").gameObject;
		this.ScopeUI.SetActive(false);
		this.Flash.enabled = false;
		this.Scope.enabled = false;
		if (!this.HasSniperScope)
		{
			base.transform.localPosition = this.FirstP;
		}
		if (!this.HasSniperScope)
		{
			base.transform.localRotation = this.FirstR;
		}
	}

	private void Update()
	{
		if (this.player.CanShoot && this.CanShoot)
		{
			ShootingType type = this.Type;
			if (type != ShootingType.Single)
			{
				if (type == ShootingType.Auto)
				{
					if (Input.GetKey(this.player.Shoot))
					{
						this.Shoot();
					}
				}
			}
			else if (Input.GetKeyDown(this.player.Shoot))
			{
				this.Shoot();
			}
		}
		if (Input.GetKeyDown(this.player.Reload))
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
		if (!this.HasSniperScope)
		{
			if (Input.GetKey(this.player.Aim))
			{
				base.transform.localPosition = Vector3.Slerp(base.transform.localPosition, this.SecondP, this.AimSpeed * Time.deltaTime);
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.SecondR, this.AimSpeed * Time.deltaTime);
			}
			if (Input.GetKeyUp(this.player.Aim))
			{
				base.transform.localPosition = this.FirstP;
				base.transform.localRotation = this.FirstR;
			}
		}
		if (this.HasSniperScope)
		{
			if (Input.GetKey(KeyCode.Mouse1))
			{
				this.Scope.enabled = true;
				this.ScopeUI.SetActive(true);
			}
			else
			{
				this.Scope.enabled = false;
				this.ScopeUI.SetActive(false);
			}
		}
	}

	private void Shoot()
	{
		if (this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize > 0)
		{
			base.StartCoroutine("ShowFlash");
			new Thread(new ThreadStart(this.Delay)).Start();
			this.source.clip = this.ShootSound;
			this.source.Play();
			this.player.SubstractWeaponDurability(1);
			this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].CurrentClipSize--;
			this.player.Toolbet.Show();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Bullet);
			gameObject.GetComponent<Bullet>().player = this.player;
			gameObject.GetComponent<Bullet>().Damage = (float)(this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item as Weapon).Damage * this.player.WeaponDamageK;
			gameObject.transform.position = this.SpawnZone.position;
			float num = UnityEngine.Random.Range(-this.BulletRecoil, this.BulletRecoil);
			float num2 = UnityEngine.Random.Range(-this.BulletRecoil, this.BulletRecoil);
			Vector3 a = new Vector3(base.transform.forward.x + num, base.transform.forward.y + num2, base.transform.forward.z);
			gameObject.GetComponent<Rigidbody>().AddForce(a * this.Speed);
		}
	}

	private void Delay()
	{
		this.CanShoot = false;
		Thread.Sleep(100);
		this.CanShoot = true;
	}

	private void WaitReload()
	{
		this.CanShoot = false;
		Thread.Sleep(700);
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
	}

	private void CameraMove()
	{
		if (this.PlayerCamera != null)
		{
			float num = UnityEngine.Random.Range(-this.CameraRecoil, this.CameraRecoil);
			this.player.GetComponent<FirstPersonController>().enabled = false;
			this.PlayerCamera.transform.rotation = Quaternion.Euler(this.PlayerCamera.transform.rotation.x, this.PlayerCamera.transform.rotation.y + num, this.PlayerCamera.transform.rotation.z + this.CameraRecoil);
			this.player.GetComponent<FirstPersonController>().enabled = true;
		}
	}

	[DebuggerHidden]
	private IEnumerator ShowFlash()
	{
		ShootingSystem.<ShowFlash>c__Iterator0 <ShowFlash>c__Iterator = new ShootingSystem.<ShowFlash>c__Iterator0();
		<ShowFlash>c__Iterator.$this = this;
		return <ShowFlash>c__Iterator;
	}
}
