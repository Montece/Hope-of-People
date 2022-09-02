using System;
using UnityEngine;

public class HoloScreen : MonoBehaviour
{
	public GameObject Portal;

	private Inventory inv;

	private void Start()
	{
		this.inv = base.GetComponent<Inventory>();
	}

	private void Update()
	{
		bool flag = false;
		foreach (Slot current in this.inv.InventoryArray)
		{
			if (current.Item != null && current.Item.Id == "activationkey")
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.OnPortal();
		}
		else
		{
			this.OffPortal();
		}
	}

	private void OnPortal()
	{
		this.Portal.transform.GetChild(0).gameObject.SetActive(true);
	}

	private void OffPortal()
	{
		this.Portal.transform.GetChild(0).gameObject.SetActive(false);
	}
}
