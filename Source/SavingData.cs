using System;
using System.Collections.Generic;
using UnityEngine;

public class SavingData
{
	public float CurrentHead;

	public float CurrentBody;

	public float CurrentRightLeg;

	public float CurrentLeftLeg;

	public float CurrentRightArm;

	public float CurrentLeftArm;

	public float CurrentFood;

	public float CurrentStamina;

	public float CurrentWater;

	public float CurrentRadiation;

	public float CurrentPiss;

	public float FoodReductionK;

	public int Year;

	public int Month;

	public int Day;

	public float Hour;

	public List<BuffSlot> CurrentBuffs = new List<BuffSlot>();

	public List<string> CurrentCrafts = new List<string>();

	public List<SavingSlot> Toolbet = new List<SavingSlot>();

	public List<SavingSlot> Cloths = new List<SavingSlot>();

	public List<SavingSlot> Inventory = new List<SavingSlot>();

	public List<SavingConstructions> Constructions = new List<SavingConstructions>();

	public Vector3 PlayerPosition;

	public string SceneTitle;
}
