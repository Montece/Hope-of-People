using System;
using UnityEngine;

public class Item
{
	public string Title;

	public bool CanStack;

	public string Id;

	public Sprite Icon;

	public GameObject Drop;

	public ItemType ItemType;

	public Item FryResult;

	public bool CanForge;

	public bool CanCampfire;

	public IINFO[] Waste;
}
