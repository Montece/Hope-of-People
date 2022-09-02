using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsSpawn : MonoBehaviour
{
	public int ConstantCount;

	public GameObject[] Animals;

	private Transform Points;

	private List<Transform> points = new List<Transform>();

	private void Start()
	{
		this.Points = GameObject.FindGameObjectWithTag("Points").transform;
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
		for (int i = 0; i < this.ConstantCount; i++)
		{
			this.Spawn();
		}
	}

	private void Spawn()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.Animals[UnityEngine.Random.Range(0, this.Animals.Length)], this.points[UnityEngine.Random.Range(0, this.points.Count)].position, Quaternion.identity);
	}
}
