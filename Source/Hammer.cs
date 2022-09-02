using System;
using UnityEngine;

public class Hammer : MonoBehaviour
{
	public float Range = 2f;

	private Player player;

	private Ray ray;

	private RaycastHit hit;

	private void Start()
	{
		this.player = base.GetComponentInParent<Player>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.player.Shoot) && this.player.CanUseTools)
		{
			this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
			if (Physics.Raycast(this.ray, out this.hit, this.Range))
			{
				Construction component = this.hit.collider.GetComponent<Construction>();
				if (component != null)
				{
					component.Remove();
					this.player.SubstractToolDurability(1);
				}
			}
		}
	}
}
