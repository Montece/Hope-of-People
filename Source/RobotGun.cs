using System;
using System.Threading;
using UnityEngine;

public class RobotGun : MonoBehaviour
{
	public GameObject Target;

	public float ShootingRange = 100f;

	public float Damage = 10f;

	public int ConstructionDamageK = 3;

	public int BulletsPerClip = 5;

	private Ray ray;

	private RaycastHit hit;

	private bool CanShoot = true;

	private void Update()
	{
		if (this.Target != null)
		{
			Vector3 forward = Vector3.RotateTowards(base.transform.position, this.Target.transform.position - base.transform.position, 0.5f * Time.deltaTime, 0f);
			base.transform.rotation = Quaternion.LookRotation(forward);
		}
	}

	public void Shoot()
	{
		if (this.CanShoot)
		{
			for (int i = 0; i < this.BulletsPerClip; i++)
			{
				Vector3 direction = base.transform.TransformDirection(Vector3.forward);
				if (Physics.Raycast(base.transform.position, direction, out this.hit, this.ShootingRange))
				{
					BodyPart component = this.hit.collider.GetComponent<BodyPart>();
					Construction component2 = this.hit.collider.GetComponent<Construction>();
					if (component != null)
					{
						this.hit.collider.GetComponentInParent<PlayerStats>().GetDamage(component.Type, this.Damage);
					}
					if (component2 != null)
					{
						this.hit.collider.GetComponent<Construction>().GetDamage(this.Damage * (float)this.ConstructionDamageK);
					}
				}
			}
		}
	}

	public void Reload()
	{
		this.CanShoot = false;
		Thread.Sleep(100);
		this.CanShoot = true;
	}

	public void Delay()
	{
		this.CanShoot = false;
		Thread.Sleep(10000);
		this.CanShoot = true;
	}
}
