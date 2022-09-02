using System;
using UnityEngine;

[Serializable]
public class ChestItem
{
	[SerializeField]
	public string ItemID;

	[SerializeField]
	public int MinAmount;

	[SerializeField]
	public int MaxAmount;
}
