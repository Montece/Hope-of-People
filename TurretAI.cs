using System;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
	public GameObject Tower;

	public float ReloadTime = 10f;

	public float Damage = 30f;

	public float BulletSpeed = 20f;

	public float RotationSpeed = 10f;

	public float ShootingDistance = 500f;

	public float EyeDistance = 700f;

	public GameObject Bullet;

	public Transform SpawnZone;

	private Transform Target;

	private float CurrentReloadTime;

	private void Start()
	{
		if (this.Target == null)
		{
			this.Target = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}

	private void Update()
	{
		if (this.CurrentReloadTime < 0f)
		{
			this.CurrentReloadTime = 0f;
		}
		if (this.CurrentReloadTime > 0f)
		{
			this.CurrentReloadTime -= Time.deltaTime;
		}
		if (this.CurrentReloadTime > this.ReloadTime)
		{
			this.CurrentReloadTime = this.ReloadTime;
		}
		if (Vector3.Distance(this.Target.position, this.Tower.transform.position) <= this.EyeDistance)
		{
			this.LookOnTarget();
		}
		if (Vector3.Distance(this.Target.position, this.Tower.transform.position) <= this.ShootingDistance)
		{
			this.Shoot();
		}
	}

	private void Shoot()
	{
		if (this.CurrentReloadTime == 0f)
		{
			this.CurrentReloadTime = this.ReloadTime;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Bullet);
			gameObject.GetComponent<TurretBullet>().Damage = this.Damage;
			gameObject.transform.position = this.SpawnZone.position;
			gameObject.GetComponent<Rigidbody>().AddForce(this.Tower.transform.forward * this.BulletSpeed);
		}
	}

	private void LookOnTarget()
	{
		Vector3 forward = this.Target.position - base.transform.position;
		Quaternion b = Quaternion.LookRotation(forward);
		Vector3 eulerAngles = Quaternion.Lerp(this.Tower.transform.rotation, b, Time.deltaTime * this.RotationSpeed).eulerAngles;
		this.Tower.transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
	}
}
