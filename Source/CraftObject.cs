using System;
using UnityEngine;

public class CraftObject : MonoBehaviour
{
	public CraftInfo info;

	private Player player;

	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	public void Clicked()
	{
		this.player.craft.CurrentInfo = this.info;
		this.player.craft.ShowPanel();
		this.player.links.ItemsDescrition.gameObject.SetActive(false);
		this.player.StopDrag();
	}
}
