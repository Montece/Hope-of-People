using System;
using UnityEngine;

public class ShowPosition : MonoBehaviour
{
	public Transform WhosePosition;

	private SceneLinks links;

	private void Start()
	{
		if (this.WhosePosition == null)
		{
			this.WhosePosition = base.transform;
		}
		base.InvokeRepeating("ShowPos", 0f, 0.5f);
	}

	private void ShowPos()
	{
		if (this.links == null)
		{
			this.links = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneLinks>();
		}
		string text = string.Format("X: {1}{0}Y: {2}{0}Z: {3}{0}", new object[]
		{
			Environment.NewLine,
			(int)this.WhosePosition.position.x,
			(int)this.WhosePosition.position.y,
			(int)this.WhosePosition.position.z
		});
		this.links.PosField.text = text;
	}
}
