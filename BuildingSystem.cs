using System;
using System.Collections;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
	public Material BuildingMaterial;

	private GameObject Prefab;

	private GameObject PrefabCopy;

	private Material[] PrefabMaterial = new Material[1024];

	public bool CanBuild;

	public float BuildingRange = 5f;

	private bool IsPlant;

	private Player player;

	private Quaternion PreviousQuaternion = Quaternion.identity;

	private RaycastHit hit;

	private Ray ray;

	private void Update()
	{
		if (this.player == null)
		{
			this.player = base.GetComponent<Player>();
		}
		if (this.Prefab != null)
		{
			this.CanBuild = true;
		}
		if (this.CanBuild)
		{
			this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
			if (Physics.Raycast(this.ray, out this.hit, this.BuildingRange))
			{
				if (this.PrefabCopy == null)
				{
					if (this.Prefab == null)
					{
						this.Cancel();
						return;
					}
					this.PrefabCopy = UnityEngine.Object.Instantiate<GameObject>(this.Prefab);
					this.ToggleScripts(false);
					this.SetMaterial(MaterialType.Building);
					if (this.PreviousQuaternion == Quaternion.identity)
					{
						this.PreviousQuaternion = this.PrefabCopy.transform.rotation;
					}
				}
				else
				{
					this.PrefabCopy.transform.position = this.hit.point;
					this.PrefabCopy.transform.rotation = this.PreviousQuaternion;
					if (Input.GetKey(KeyCode.R))
					{
						this.PrefabCopy.transform.Rotate(new Vector3(0f, 0f, 1f));
					}
					if (Input.GetKey(KeyCode.T))
					{
						this.PrefabCopy.transform.Rotate(new Vector3(0f, 0f, -1f));
					}
					if (Input.GetKey(KeyCode.F))
					{
						this.PrefabCopy.transform.Rotate(new Vector3(0f, 1f, 0f));
					}
					if (Input.GetKey(KeyCode.G))
					{
						this.PrefabCopy.transform.Rotate(new Vector3(0f, -1f, 0f));
					}
					if (Input.GetKey(KeyCode.V))
					{
						this.PrefabCopy.transform.Rotate(new Vector3(1f, 0f, 0f));
					}
					if (Input.GetKey(KeyCode.B))
					{
						this.PrefabCopy.transform.Rotate(new Vector3(-1f, 0f, 0f));
					}
					this.PreviousQuaternion = this.PrefabCopy.transform.rotation;
					if (Input.GetKeyDown(this.player.Shoot) && this.hit.collider.tag != "Border")
					{
						if (!this.IsPlant)
						{
							this.ToggleScripts(true);
							this.SetMaterial(MaterialType.Prefab);
							this.player.Toolbet.Destroy(1, -1);
							GameObject prefab = this.Prefab;
							this.Stop();
							if (this.player.EquipedToolbet >= 0 && this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item != null)
							{
								this.StartBuild(prefab);
							}
						}
						else if (this.hit.collider.tag == "Terrain")
						{
							this.ToggleScripts(true);
							this.SetMaterial(MaterialType.Prefab);
							this.player.Toolbet.Destroy(1, -1);
							GameObject prefab2 = this.Prefab;
							this.Stop();
							if (this.player.EquipedToolbet >= 0 && this.player.Toolbet.InventoryArray[this.player.EquipedToolbet].Item != null)
							{
								this.StartBuild(prefab2);
							}
						}
					}
				}
			}
		}
	}

	public void Stop()
	{
		this.Prefab = null;
		this.PrefabCopy = null;
		this.CanBuild = false;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			this.CanBuild = false;
		}
		else
		{
			this.CanBuild = true;
		}
	}

	public void StartBuild(GameObject obj)
	{
		this.Stop();
		this.Prefab = obj;
		this.CanBuild = true;
		if (this.Prefab.GetComponent<Plant>())
		{
			this.IsPlant = true;
		}
		else
		{
			this.IsPlant = false;
		}
	}

	private void ToggleScripts(bool active)
	{
		if (this.PrefabCopy.GetComponent<Rigidbody>())
		{
			this.PrefabCopy.GetComponent<Rigidbody>().isKinematic = !active;
		}
		if (this.PrefabCopy.GetComponent<BoxCollider>())
		{
			this.PrefabCopy.GetComponent<BoxCollider>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<SphereCollider>())
		{
			this.PrefabCopy.GetComponent<SphereCollider>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<MeshCollider>())
		{
			this.PrefabCopy.GetComponent<MeshCollider>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<CapsuleCollider>())
		{
			this.PrefabCopy.GetComponent<CapsuleCollider>().isTrigger = active;
		}
		if (this.PrefabCopy.GetComponent<Forge>())
		{
			this.PrefabCopy.GetComponent<Forge>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<Chest>())
		{
			this.PrefabCopy.GetComponent<Chest>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<Campfire>())
		{
			this.PrefabCopy.GetComponent<Campfire>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<Light>())
		{
			this.PrefabCopy.GetComponent<Light>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<Construction>())
		{
			this.PrefabCopy.GetComponent<Construction>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<C4>())
		{
			this.PrefabCopy.GetComponent<C4>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<Car>())
		{
			this.PrefabCopy.GetComponent<Car>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<CarControllerV2>())
		{
			this.PrefabCopy.GetComponent<CarControllerV2>().enabled = active;
		}
		if (this.PrefabCopy.GetComponent<Plant>())
		{
			this.PrefabCopy.GetComponent<Plant>().enabled = active;
		}
		IEnumerator enumerator = this.PrefabCopy.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (!this.PrefabCopy.GetComponent<Forge>() && transform.GetComponent<Rigidbody>())
				{
					transform.GetComponent<Rigidbody>().isKinematic = !active;
				}
				if (transform.GetComponent<BoxCollider>())
				{
					transform.GetComponent<BoxCollider>().enabled = active;
				}
				if (transform.GetComponent<SphereCollider>())
				{
					transform.GetComponent<SphereCollider>().enabled = active;
				}
				if (transform.GetComponent<MeshCollider>())
				{
					transform.GetComponent<MeshCollider>().enabled = active;
				}
				if (transform.GetComponent<CapsuleCollider>())
				{
					transform.GetComponent<CapsuleCollider>().enabled = active;
				}
				if (transform.GetComponent<Forge>())
				{
					transform.GetComponent<Forge>().enabled = active;
				}
				if (transform.GetComponent<Campfire>())
				{
					transform.GetComponent<Campfire>().enabled = active;
				}
				if (transform.GetComponent<Chest>())
				{
					transform.GetComponent<Chest>().enabled = active;
				}
				if (transform.GetComponent<Light>())
				{
					transform.GetComponent<Light>().enabled = active;
				}
				if (transform.GetComponent<Construction>())
				{
					transform.GetComponent<Construction>().enabled = active;
				}
				if (transform.GetComponent<C4>())
				{
					transform.GetComponent<C4>().enabled = active;
				}
				if (transform.GetComponent<Car>())
				{
					transform.GetComponent<Car>().enabled = active;
				}
				if (transform.GetComponent<CarControllerV2>())
				{
					transform.GetComponent<CarControllerV2>().enabled = active;
				}
				if (transform.GetComponent<Plant>())
				{
					transform.GetComponent<Plant>().enabled = active;
				}
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

	private void SetMaterial(MaterialType type)
	{
		if (type == MaterialType.Building)
		{
			if (this.PrefabCopy.GetComponent<MeshRenderer>())
			{
				this.PrefabMaterial[0] = this.PrefabCopy.GetComponent<MeshRenderer>().sharedMaterial;
				this.PrefabCopy.GetComponent<MeshRenderer>().sharedMaterial = this.BuildingMaterial;
			}
			for (int i = 0; i < this.PrefabCopy.transform.childCount; i++)
			{
				Transform child = this.PrefabCopy.transform.GetChild(i);
				if (child.GetComponent<MeshRenderer>())
				{
					this.PrefabMaterial[i + 1] = child.GetComponent<MeshRenderer>().sharedMaterial;
					child.GetComponent<MeshRenderer>().sharedMaterial = this.BuildingMaterial;
				}
				else
				{
					this.PrefabMaterial[i + 1] = null;
				}
			}
		}
		else if (type == MaterialType.Prefab)
		{
			if (this.PrefabCopy.GetComponent<MeshRenderer>())
			{
				this.PrefabCopy.GetComponent<MeshRenderer>().sharedMaterial = this.PrefabMaterial[0];
			}
			for (int j = 0; j < this.PrefabCopy.transform.childCount; j++)
			{
				Transform child2 = this.PrefabCopy.transform.GetChild(j);
				if (child2.GetComponent<MeshRenderer>())
				{
					child2.GetComponent<MeshRenderer>().sharedMaterial = this.PrefabMaterial[j + 1];
				}
			}
		}
	}

	public void Cancel()
	{
		this.CanBuild = false;
		this.Prefab = null;
		if (this.PrefabCopy != null)
		{
			UnityEngine.Object.Destroy(this.PrefabCopy);
		}
	}
}
