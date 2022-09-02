using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsSpawn : MonoBehaviour
{
	public GameObject RobotExplode;

	public GameObject RobotShooting;

	public GameObject Drone;

	[Header("WAVES")]
	public Transform Points;

	private List<Transform> points = new List<Transform>();

	public float EveryNSeconds;

	public int RobotsCount;

	public int DronsCount;

	private int j;

	[Header("MAP"), Space]
	public int MapCount;

	public Transform RespawnPointsObject;

	private List<Transform> RespawnPoints = new List<Transform>();

	private GameObject TownObject;

	private void Start()
	{
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
		if (this.RespawnPointsObject != null)
		{
			IEnumerator enumerator2 = this.RespawnPointsObject.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Transform transform = (Transform)enumerator2.Current;
					this.RespawnPoints.Add(transform);
					transform.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}
		this.TownObject = new GameObject
		{
			name = "Town Robots"
		};
		for (int i = 0; i < this.MapCount; i++)
		{
			this.CreateRobot(RobotType.Explode);
		}
		if (this.EveryNSeconds > 0f)
		{
			base.InvokeRepeating("Spawn", 0f, this.EveryNSeconds);
		}
	}

	private void Spawn()
	{
		Vector3 position = this.points[UnityEngine.Random.Range(0, this.points.Count)].position;
		GameObject gameObject = new GameObject();
		this.j++;
		gameObject.name = "Robot's squad #" + this.j;
		for (int i = 0; i < this.RobotsCount; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.RobotShooting, position, Quaternion.identity).transform;
			transform.GetComponent<RobotAI>().seeDistance = 10000f;
			transform.SetParent(gameObject.transform);
		}
		for (int j = 0; j < this.DronsCount; j++)
		{
			Transform transform2 = UnityEngine.Object.Instantiate<GameObject>(this.Drone, position, Quaternion.identity).transform;
			transform2.GetComponent<DroneAI>().seeDistance = 10000f;
			transform2.transform.position += new Vector3(0f, 50f, 0f);
			transform2.SetParent(gameObject.transform);
		}
	}

	public Transform GetRandomPoint()
	{
		return this.RespawnPoints[UnityEngine.Random.Range(0, this.RespawnPoints.Count)];
	}

	public void CreateRobot(RobotType type)
	{
		Vector3 position = this.GetRandomPoint().position;
		Transform transform = null;
		if (type != RobotType.Explode)
		{
			if (type == RobotType.Shooting)
			{
				transform = UnityEngine.Object.Instantiate<GameObject>(this.RobotShooting, position, Quaternion.identity).transform;
			}
		}
		else
		{
			transform = UnityEngine.Object.Instantiate<GameObject>(this.RobotExplode, position, Quaternion.identity).transform;
		}
		transform.SetParent(this.TownObject.transform);
		transform.GetComponent<RobotAI>().IsTown = true;
	}
}
