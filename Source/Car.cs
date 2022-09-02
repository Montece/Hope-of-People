using System;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
	[Header("CAR"), Space]
	public KeyCode Exit = KeyCode.Escape;

	public bool CarOn;

	public int FuelPerSecond = 1;

	public bool NeedFuel = true;

	public float NowFuel;

	private string n;

	[Space]
	public Text InfoText;

	private GameObject Player;

	private GameObject cam;

	private CarControllerV2 car;

	private Inventory inv;

	private Construction c;

	private void Start()
	{
		this.n = Environment.NewLine;
		this.cam = base.transform.GetChild(0).gameObject;
		this.car = base.GetComponent<CarControllerV2>();
		this.inv = base.GetComponent<Inventory>();
		this.c = base.GetComponent<Construction>();
		this.car.enabled = false;
		base.InvokeRepeating("EatFuel", 0f, 1f);
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.Exit))
		{
			this.Off();
		}
		if (this.InfoText != null && this.CarOn)
		{
			this.InfoText.text = string.Concat(new object[]
			{
				"Speed: ",
				(int)this.car.Speed,
				"/",
				this.car.maxSpeed,
				"km/h",
				this.n,
				"Fuel: ",
				this.NowFuel,
				" L",
				this.n,
				"Durability: ",
				this.c.CurrentHealth,
				"/",
				this.c.MaximumHealth
			});
		}
	}

	public void On(GameObject player)
	{
		this.InfoText = GameObject.Find("UI/CarInfo").GetComponent<Text>();
		if (player != null)
		{
			this.CarOn = true;
			this.Player = player;
			this.Player.SetActive(false);
			this.car.enabled = true;
			if (this.car.crashAudio != null)
			{
				this.car.crashAudio.SetActive(true);
			}
			if (this.car.skidAudio != null)
			{
				this.car.skidAudio.SetActive(true);
			}
			if (this.car.engineAudio != null)
			{
				this.car.engineAudio.SetActive(true);
			}
			this.cam.SetActive(true);
		}
	}

	public void Off()
	{
		if (this.Player != null)
		{
			this.CarOn = false;
			this.Player.SetActive(true);
			if (this.car.crashAudio != null)
			{
				this.car.crashAudio.SetActive(false);
			}
			if (this.car.skidAudio != null)
			{
				this.car.skidAudio.SetActive(false);
			}
			if (this.car.engineAudio != null)
			{
				this.car.engineAudio.SetActive(false);
			}
			this.car.enabled = false;
			this.cam.SetActive(false);
			this.Player.transform.position = base.transform.position;
			this.Player.transform.position += new Vector3(0f, 3f, 0f);
			this.Player = null;
			this.InfoText.text = string.Empty;
		}
	}

	public void EatFuel()
	{
		if (this.CarOn)
		{
			bool flag = false;
			for (int i = 0; i < this.inv.InventoryArray.Count; i++)
			{
				if (this.inv.InventoryArray[i].Item != null && this.inv.InventoryArray[i].Item.Id == "biofuel" && this.inv.InventoryArray[i].Count > 0)
				{
					this.inv.InventoryArray[i].Count -= this.FuelPerSecond;
					this.NowFuel = (float)this.inv.InventoryArray[i].Count;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.Off();
			}
		}
	}
}
