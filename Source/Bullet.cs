using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Player player;

	public float Damage;

	private void OnCollisionEnter(Collision collision)
	{
		base.GetComponent<Rigidbody>().isKinematic = true;
		RobotAI component = collision.collider.GetComponent<RobotAI>();
		if (component != null)
		{
			component.GetDamage(this.player.gameObject, this.Damage);
		}
		AnimalAI component2 = collision.collider.GetComponent<AnimalAI>();
		if (component2 != null)
		{
			component2.GetDamage(this.player.gameObject, this.Damage);
		}
		Construction component3 = collision.collider.GetComponent<Construction>();
		if (component3 != null)
		{
			component3.GetDamage(this.Damage);
		}
		BodyPart component4 = collision.collider.GetComponent<BodyPart>();
		if (component4 != null)
		{
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStats>().GetDamage(component4.Type, this.Damage);
		}
		ZombieAI component5 = collision.collider.GetComponent<ZombieAI>();
		if (component5 != null)
		{
			component5.GetDamage(this.player.gameObject, this.Damage);
		}
		Rigidbody component6 = collision.collider.GetComponent<Rigidbody>();
		if (component6 != null)
		{
			component6.AddForceAtPosition(Vector3.one * 20f, collision.contacts[0].point);
		}
		base.GetComponent<Collider>().isTrigger = true;
	}
}
