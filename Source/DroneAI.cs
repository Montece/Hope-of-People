using System;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
	[Header("DRONE V.1.17.12")]
	public string EnemyID = string.Empty;

	public AudioClip DeathSound;

	public float CurrentHealth = 100f;

	public GameObject dead;

	public GameObject Explode;

	public float damage = 100f;

	public float speed = 3.5f;

	public float seeDistance;

	public float AttackDistance = 3f;

	public int ConstructionDamageK = 3;

	private GameObject player;

	private AudioSource source;

	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.source = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (this.player != null)
		{
			if ((this.player.transform.position - base.transform.position).sqrMagnitude > this.AttackDistance * this.AttackDistance)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.player.transform.position, this.speed * Time.deltaTime);
				base.transform.LookAt(this.player.transform);
			}
			else
			{
				this.Suicide();
			}
		}
	}

	public void GetDamage(GameObject sender, float value)
	{
		this.CurrentHealth -= value;
		if (this.CurrentHealth <= 0f)
		{
			this.Death();
		}
		else if (sender != null)
		{
			this.player = sender;
		}
	}

	private void Suicide()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.Explode, base.transform.position + new Vector3(0f, 3f, 0f), base.transform.rotation);
		this.Death();
	}

	private void Death()
	{
		if (this.dead != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.dead, base.transform.position, base.transform.rotation);
		}
		this.source.clip = this.DeathSound;
		this.source.Play();
		this.player.GetComponent<QuestSystem>().DoKill(this.EnemyID, 1);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Construction>())
		{
			this.Suicide();
		}
	}
}
