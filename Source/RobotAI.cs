using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
	[Header("ROBOT V.2.87.05")]
	public string EnemyID = string.Empty;

	public RobotType RobotType;

	public bool IsTown;

	public AudioClip DeathSound;

	public float CurrentHealth = 100f;

	public GameObject dead;

	public GameObject Explode;

	public float damage = 100f;

	public float speed = 3.5f;

	public float seeDistance;

	public float AttackDistance = 3f;

	public float ShootingRange = 100f;

	public float BulletDamage = 10f;

	public int ConstructionDamageK = 3;

	public int BulletsPerClip = 5;

	private GameObject player;

	private NavMeshAgent nav;

	private Animator anim;

	private AudioSource source;

	private bool CanShoot = true;

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
			if (this.IsTown)
			{
				this.player = this.manager.gameObject.GetComponent<RobotsSpawn>().GetRandomPoint().gameObject;
			}
			else
			{
				this.player = GameObject.FindGameObjectWithTag("Player");
			}
		}
		else if (Vector3.Distance(base.transform.position, this.player.transform.position) <= this.seeDistance)
		{
			RobotType robotType = this.RobotType;
			if (robotType != RobotType.Explode)
			{
				if (robotType == RobotType.Shooting)
				{
					if (Vector3.Distance(base.transform.position, this.player.transform.position) > this.ShootingRange || Mathf.Abs(base.transform.position.y - this.player.transform.position.y) > 3.5f)
					{
						this.anim.Play("Walk_F");
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
			}
			else if (Vector3.Distance(base.transform.position, this.player.transform.position) > this.AttackDistance || Mathf.Abs(base.transform.position.y - this.player.transform.position.y) > 3.5f)
			{
				this.anim.Play("Walk_F");
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
			this.anim.Play("Idle");
			this.nav.enabled = false;
		}
	}

	private void Attack()
	{
		if (this.player.tag != "RobotSpawnpoint")
		{
			RobotType robotType = this.RobotType;
			if (robotType != RobotType.Explode)
			{
				if (robotType == RobotType.Shooting)
				{
					base.StartCoroutine(this.Shoot());
				}
			}
			else
			{
				this.Suicide();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator Shoot()
	{
		RobotAI.<Shoot>c__Iterator0 <Shoot>c__Iterator = new RobotAI.<Shoot>c__Iterator0();
		<Shoot>c__Iterator.$this = this;
		return <Shoot>c__Iterator;
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
		if (this.IsTown)
		{
			this.manager.gameObject.GetComponent<RobotsSpawn>().CreateRobot(this.RobotType);
		}
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
		if (this.player != null && other.tag == "RobotSpawnpoint" && this.IsTown && this.player.tag != "Player")
		{
			this.player = null;
		}
	}
}
