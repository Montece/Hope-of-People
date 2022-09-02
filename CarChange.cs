using System;
using UnityEngine;

public class CarChange : MonoBehaviour
{
	private GameObject[] objects;

	private int activeObjectIdx;

	public float cameraDistance = 15f;

	private GameObject activeObject;

	private float size;

	private bool selectScreen = true;

	public Vector3 cameraOffset = new Vector3(0f, 1f, 0f);

	private void Start()
	{
		this.objects = GameObject.FindGameObjectsWithTag("Player");
		Array.Sort<GameObject>(this.objects, (GameObject go1, GameObject go2) => go1.transform.position.x.CompareTo(go2.transform.position.x));
		this.SetActiveObject(this.objects[this.activeObjectIdx]);
	}

	private void Update()
	{
		if (this.selectScreen)
		{
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, this.objects[this.activeObjectIdx].transform.position + -Camera.main.transform.forward * this.size + this.cameraOffset, Time.deltaTime * 5f);
		}
	}

	private void OnGUI()
	{
		if (this.selectScreen)
		{
			GUIStyle style = GUI.skin.GetStyle("Button");
			style.alignment = TextAnchor.MiddleCenter;
			if (GUI.Button(new Rect((float)(Screen.width / 2 + 65), (float)(Screen.height - 100), 120f, 50f), "Next"))
			{
				this.activeObjectIdx++;
				if (this.activeObjectIdx >= this.objects.Length)
				{
					this.activeObjectIdx = 0;
				}
				this.SetActiveObject(this.objects[this.activeObjectIdx]);
			}
			if (GUI.Button(new Rect((float)(Screen.width / 2 - 185), (float)(Screen.height - 100), 120f, 50f), "Previous"))
			{
				this.activeObjectIdx--;
				if (this.activeObjectIdx < 0)
				{
					this.activeObjectIdx = this.objects.Length - 1;
				}
				this.SetActiveObject(this.objects[this.activeObjectIdx]);
			}
			if (GUI.Button(new Rect((float)(Screen.width / 2 - 60), (float)(Screen.height - 100), 120f, 50f), "Select"))
			{
				this.selectScreen = false;
				this.objects[this.activeObjectIdx].GetComponent<CarControllerV2>().canControl = true;
				base.GetComponent<CamManager>().enabled = true;
				base.GetComponent<CamManager>().target = this.objects[this.activeObjectIdx];
				base.GetComponent<CamManager>().dist = this.size;
			}
		}
		else if (GUI.Button(new Rect((float)(Screen.width - 270), 350f, 240f, 50f), "Select Screen"))
		{
			this.selectScreen = true;
			this.objects[this.activeObjectIdx].GetComponent<CarControllerV2>().canControl = false;
			base.GetComponent<CamManager>().enabled = false;
			Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.x, 330f, Camera.main.transform.rotation.z);
		}
	}

	private void SetActiveObject(GameObject go)
	{
		this.size = this.cameraDistance;
		Renderer[] componentsInChildren = this.objects[this.activeObjectIdx].GetComponentsInChildren<Renderer>();
		if (componentsInChildren.Length > 0)
		{
			Array.Sort<Renderer>(componentsInChildren, (Renderer r1, Renderer r2) => r2.bounds.size.magnitude.CompareTo(r1.bounds.size.magnitude));
			this.size = componentsInChildren[0].bounds.size.magnitude;
		}
	}
}
