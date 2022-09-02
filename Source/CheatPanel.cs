using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheatPanel : MonoBehaviour
{
	public GameObject Cheatpanel;

	public Transform ItemsField;

	public GameObject Slot;

	public Inventory Inventory;

	private void Start()
	{
		this.Cheatpanel.transform.GetChild(1).GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
		this.Cheatpanel.SetActive(false);
	}

	public void Open()
	{
		IEnumerator enumerator = this.ItemsField.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
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
		foreach (Item current in Database.Items)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Slot, this.ItemsField);
			gameObject.GetComponent<Image>().sprite = current.Icon;
			gameObject.GetComponent<CheatSlot>().Item = current;
			gameObject.GetComponent<CheatSlot>().ItemID = current.Id;
			gameObject.GetComponent<CheatSlot>().Inventory = this.Inventory;
		}
		this.Cheatpanel.SetActive(true);
	}
}
