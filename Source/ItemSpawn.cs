using System;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
	public string id;

	public int count = 1;

	private void Start()
	{
		Item item = Database.Get(this.id);
		GameObject drop = item.Drop;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(drop);
		gameObject.GetComponent<Pickup>().slot.Count = this.count;
		gameObject.GetComponent<Pickup>().slot.Item = item;
		gameObject.transform.position = base.transform.position;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
