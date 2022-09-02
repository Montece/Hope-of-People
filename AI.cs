using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
	[Header("AI BY YURA P"), Space]
	public BehaviorMatrix Behavior;

	public string TargetTag = "Player";

	public string EnemyID = string.Empty;

	public float FOVAngle = 60f;

	public float FOVRange = 10f;

	[Space]
	public List<AIAction> action = new List<AIAction>();

	public bool GodMode;

	public float CurrentHealth = 100f;

	public float MaximumHealth = 100f;

	public bool SeePlayer;

	[Space]
	public GameObject House;

	public byte TreesForHouse = 5;

	private Vector3 HousePosition;

	private bool HasHouse;

	private Transform Target;

	private RaycastHit hit;

	private Vector3 Direction;

	private LootManager lmanager;

	private NavMeshAgent agent;

	private Inventory inventory;

	private int Stage;

	private void Start()
	{
		this.lmanager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LootManager>();
		this.agent = base.GetComponent<NavMeshAgent>();
		this.inventory = base.GetComponent<Inventory>();
	}

	private void FindPlayer()
	{
		this.Target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		if (this.Target == null)
		{
			this.FindPlayer();
		}
		else
		{
			this.Direction = this.Target.position - base.transform.position;
			if (Vector3.Angle(this.Direction, base.transform.forward) <= this.FOVAngle && Physics.Raycast(base.transform.position, this.Direction, out this.hit, this.FOVRange) && this.hit.transform.tag == this.TargetTag)
			{
				this.SeePlayer = true;
			}
			BehaviorMatrix behavior = this.Behavior;
			if (behavior != BehaviorMatrix.Builder)
			{
				this.Death();
			}
			else
			{
				this.Builder();
			}
		}
	}

	private void Builder()
	{
		if (!this.HasHouse)
		{
			this.HousePosition = this.lmanager.GetRandomPoint();
			this.HasHouse = true;
			this.action.Add(AIAction.SearchingForResourcesForHouse);
		}
		if (this.SeePlayer)
		{
			this.action.Add(AIAction.RunAway);
		}
		switch (this.action[0])
		{
		case AIAction.SearchingForResourcesForHouse:
			this.BuildHouse();
			break;
		}
		this.action.RemoveAt(0);
	}

	private void BuildHouse()
	{
		for (int i = 0; i < (int)this.TreesForHouse; i++)
		{
			GameObject nearestTree = this.lmanager.GetNearestTree(base.transform.position);
			this.agent.Move(nearestTree.transform.position);
			while (!this.agent.isStopped)
			{
			}
			UnityEngine.Object.Destroy(nearestTree);
			this.inventory.Add(new Slot(Database.GetItemByID("woodlog")), 100);
		}
		this.agent.Move(this.HousePosition - new Vector3(10f, 0f, 10f));
		UnityEngine.Object.Instantiate<GameObject>(this.House);
		this.House.transform.position = this.HousePosition;
	}

	public void GetDamage(float amount)
	{
		if (!this.GodMode)
		{
			this.CurrentHealth -= amount;
			if (this.CurrentHealth <= 0f)
			{
				this.CurrentHealth = 0f;
				this.Death();
			}
		}
	}

	public void Heal(float amount)
	{
		this.CurrentHealth += amount;
		if (this.CurrentHealth > this.MaximumHealth)
		{
			this.CurrentHealth = this.MaximumHealth;
		}
	}

	private void Death()
	{
		foreach (Slot current in this.inventory.InventoryArray)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"));
			gameObject.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
			gameObject.GetComponent<Pickup>().slot = current;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
