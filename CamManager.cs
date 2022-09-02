using System;
using UnityEngine;
using UnityStandardAssets.Utility;

public class CamManager : MonoBehaviour
{
	private SmoothFollow cameraScript;

	public float dist = 10f;

	public int cameraChangeCount;

	public GameObject target;

	private void Start()
	{
		this.cameraScript = base.GetComponent<SmoothFollow>();
	}

	private void Update()
	{
		this.cameraScript.target = this.target.transform;
		this.cameraScript.distance = this.dist;
		this.cameraScript.height = this.dist / 3f;
		if (Input.GetKeyDown(KeyCode.C))
		{
			this.cameraChangeCount++;
			if (this.cameraChangeCount == 3)
			{
				this.cameraChangeCount = 0;
			}
		}
		int num = this.cameraChangeCount;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					this.cameraScript.enabled = false;
				}
			}
			else
			{
				this.cameraScript.enabled = false;
			}
		}
		else
		{
			this.cameraScript.enabled = true;
		}
	}
}
