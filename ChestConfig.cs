using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChestConfig
{
	[SerializeField]
	public List<ChestItem> items = new List<ChestItem>();
}
