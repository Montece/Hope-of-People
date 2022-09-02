using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
	[Header("ZOMIBE")]
	public string EnemyID = string.Empty;

	public AudioClip DeathSound;

	public float CurrentHealth = 100f;

	public GameObject dead;

	public float damage = 100f;

	public float speed = 3.5f;

	public float seeDistance;

	public float AttackDistance = 3f;

	private GameObject player;

	private NavMeshAgent nav;

	private Animator anim;

	private AudioSource source;

	private bool CanAttack = true;

	private RaycastHit hit;

	private GameObject manager;

	private GameManager Gmanager;

	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.anim = base.GetComponent<Animator>();
		this.nav = base.GetComponent<NavMeshAgent>();
		this.source = base.GetComponent<AudioSource>();
		this.manager = GameObject.FindGameObjectWithTag("SceneManager");
		this.Gmanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		this.nav.enabled = true;
		this.nav.speed = this.speed;
	}

	private void Update()
	{
		if (!this.nav.isOnNavMesh)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.player == null)
		{
			this.player = GameObject.FindGameObjectWithTag("Player");
		}
		else if (Vector3.Distance(base.transform.position, this.player.transform.position) <= this.seeDistance)
		{
			if (Vector3.Distance(base.transform.position, this.player.transform.position) > this.AttackDistance || Mathf.Abs(base.transform.position.y - this.player.transform.position.y) > 3.5f)
			{
				this.anim.Play("walk01");
				if (this.player != null)
				{
					this.nav.SetDestination(this.player.transform.position);
				}
			}
			else
			{
				this.Attack();
			}
		}
		else
		{
			this.anim.Play("idle");
			this.nav.enabled = false;
		}
	}

	private void Attack()
	{
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

	private void Death()
	{
		if (this.dead != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.dead, base.transform.position, base.transform.rotation);
		}
		this.source.clip = this.DeathSound;
		this.source.Play();
		this.Gmanager.GetComponent<QuestSystem>().DoKill(this.EnemyID, 1);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.player != null && other.tag == "RobotSpawnpoint" && this.player.tag != "Player")
		{
			this.player = null;
		}
	}
}
