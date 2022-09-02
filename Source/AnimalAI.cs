using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
	[Header("ANIMAL")]
	public string EnemyID = string.Empty;

	public AudioClip DeathSound;

	public float CurrentHealth = 100f;

	public float damage = 20f;

	public float speed = 7f;

	public float seeDistance = 10000f;

	public float attackDistance = 3f;

	public string WalkTitle = string.Empty;

	public int FatCount = 20;

	public int MeatCount = 5;

	public int FurCount = 20;

	private bool Peace = true;

	private GameObject target;

	private Vector3 point;

	private NavMeshAgent nav;

	private Animator anim;

	private AudioSource source;

	private Transform Points;

	private List<Transform> points = new List<Transform>();

	private GameObject player;

	private bool CanAttack = true;

	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.Points = GameObject.FindGameObjectWithTag("Points").transform;
		this.anim = base.GetComponent<Animator>();
		this.nav = base.GetComponent<NavMeshAgent>();
		this.source = base.GetComponent<AudioSource>();
		if (this.Points != null)
		{
			IEnumerator enumerator = this.Points.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform item = (Transform)enumerator.Current;
					this.points.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	private void Update()
	{
		if (!this.nav.isOnNavMesh)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.nav.speed = this.speed;
		if (this.Peace)
		{
			if (this.point == Vector3.zero)
			{
				this.point = this.points[UnityEngine.Random.Range(0, this.points.Count)].position;
				this.nav.SetDestination(this.point);
			}
			this.anim.Play(this.WalkTitle);
			if (base.transform.position.x == this.point.x && base.transform.position.z == this.point.z)
			{
				this.point = Vector3.zero;
			}
		}
		else if (Vector3.Distance(base.transform.position, this.target.transform.position) <= this.seeDistance)
		{
			this.nav.enabled = true;
			if (Vector3.Distance(base.transform.position, this.target.transform.position) > this.attackDistance || Mathf.Abs(base.transform.position.y - this.target.transform.position.y) > 3.5f)
			{
				this.anim.Play(this.WalkTitle);
				if (this.target != null)
				{
					this.nav.SetDestination(this.target.transform.position);
				}
			}
			else if (this.CanAttack)
			{
				this.Attack();
			}
		}
		else
		{
			this.Peace = true;
		}
	}

	private void Attack()
	{
		PlayerStats component = this.target.GetComponent<PlayerStats>();
		if (component != null)
		{
			component.GetDamage(DamageType.Legs, this.damage);
		}
		this.Wait();
	}

	private void Wait()
	{
		this.CanAttack = false;
		Thread.Sleep(1000);
		this.CanAttack = true;
	}

	public void GetDamage(GameObject sender, float value)
	{
		this.CurrentHealth -= value;
		if (this.CurrentHealth <= 0f)
		{
			this.source.clip = this.DeathSound;
			this.source.Play();
			Slot slot = new Slot(Database.Get("fat"));
			Slot slot2 = new Slot(Database.Get("rawmeat"));
			Slot slot3 = new Slot(Database.Get("fur"));
			slot.Count = UnityEngine.Random.Range(5, this.FatCount);
			slot2.Count = UnityEngine.Random.Range(1, this.MeatCount);
			slot3.Count = UnityEngine.Random.Range(1, this.FurCount);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"));
			gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
			gameObject.GetComponent<Pickup>().slot = slot;
			gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"));
			gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
			gameObject.GetComponent<Pickup>().slot = slot2;
			gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"));
			gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
			gameObject.GetComponent<Pickup>().slot = slot3;
			this.player.GetComponent<QuestSystem>().DoKill(this.EnemyID, 1);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
