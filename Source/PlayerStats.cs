using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStats : MonoBehaviour
{
	[Header("HEALTH")]
	public float CurrentHead = 25f;

	public float MaximumHead = 25f;

	public float CurrentBody = 100f;

	public float MaximumBody = 100f;

	public float CurrentRightLeg = 50f;

	public float MaximumRightLeg = 50f;

	public float CurrentLeftLeg = 50f;

	public float MaximumLeftLeg = 50f;

	public float CurrentRightArm = 50f;

	public float MaximumRightArm = 50f;

	public float CurrentLeftArm = 50f;

	public float MaximumLeftArm = 50f;

	[Header("FOOD")]
	public float CurrentFood = 100f;

	public float MaximumFood = 100f;

	public float FoodReduction = 0.1f;

	public float FoodReductionK = 1f;

	[Header("STAMINA")]
	public float CurrentStamina = 100f;

	public float MaximumStamina = 100f;

	public float RunStamina = 0.1f;

	[Header("WATER")]
	public float CurrentWater = 100f;

	public float MaximumWater = 100f;

	public float WaterReduction = 0.15f;

	[Header("RADIATION")]
	public float CurrentRadiation = 100f;

	public float MaximumRadiation = 100f;

	[Header("PISS")]
	public float CurrentPiss = 100f;

	public float MaximumPiss = 100f;

	public float PissAddition = 0.1f;

	public float WaterPerPiss = 0.5f;

	[Header("SOUNDS")]
	public AudioClip SickSound;

	public AudioClip RadiationSound;

	private bool IsDead;

	private int LegsDamageReductionK;

	private int ArmsDamageReductionK;

	private int HeadsDamageReductionK;

	private int BodysDamageReductionK;

	private Buffs buffs;

	private Player player;

	private AudioSource source;

	private GameManager manager;

	private SceneLinks links;

	private FirstPersonController fpc;

	public void OnStart(SceneLinks links)
	{
		this.links = links;
		this.buffs = base.GetComponent<Buffs>();
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		this.fpc = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
		this.source = this.player.gameObject.GetComponents<AudioSource>()[1];
		this.manager = base.GetComponent<GameManager>();
		base.CancelInvoke();
		base.InvokeRepeating("SubstractFood", 10f, 10f);
		base.InvokeRepeating("AddPiss", 10f, 10f);
	}

	private void SubstractFood()
	{
		if (this.CurrentFood <= 0f)
		{
			this.CurrentBody -= 1f;
			return;
		}
		this.CurrentFood -= this.FoodReduction * this.FoodReductionK;
	}

	private void SubstractWater()
	{
		if (this.CurrentWater <= 0f)
		{
			this.CurrentBody -= 2f;
			return;
		}
		this.CurrentWater -= this.WaterReduction;
	}

	private void AddPiss()
	{
		if (this.CurrentPiss == this.MaximumPiss)
		{
			this.GetDamage(DamageType.Body, 0.05f);
			this.SubstractWater();
			return;
		}
		this.CurrentPiss += this.PissAddition;
	}

	private void SubstractStamina(float value)
	{
		this.CurrentStamina -= value;
	}

	public void GetDamage(DamageType type, float value)
	{
		switch (type)
		{
		case DamageType.Legs:
			this.CurrentLeftLeg -= value * (float)(1 - this.LegsDamageReductionK);
			this.CurrentRightLeg -= value * (float)(1 - this.LegsDamageReductionK);
			return;
		case DamageType.Arms:
			this.CurrentLeftArm -= value * (float)(1 - this.ArmsDamageReductionK);
			this.CurrentRightArm -= value * (float)(1 - this.ArmsDamageReductionK);
			return;
		case DamageType.Head:
			this.CurrentHead -= value * (float)(1 - this.HeadsDamageReductionK);
			return;
		case DamageType.Body:
			this.CurrentBody -= value * (float)(1 - this.BodysDamageReductionK);
			return;
		case DamageType.Pure:
			this.CurrentBody -= value;
			this.CurrentHead -= value;
			this.CurrentLeftArm -= value;
			this.CurrentRightArm -= value;
			this.CurrentLeftLeg -= value;
			this.CurrentRightLeg -= value;
			return;
		default:
			return;
		}
	}

	private void Update()
	{
		if (this.player == null)
		{
			return;
		}
		if (this.fpc == null)
		{
			return;
		}
		if (this.CurrentStamina < 0f)
		{
			this.CurrentStamina = 0f;
		}
		if (this.CurrentStamina > this.MaximumStamina)
		{
			this.CurrentStamina = this.MaximumStamina;
		}
		if (this.CurrentFood > this.MaximumFood)
		{
			this.CurrentFood = this.MaximumFood;
		}
		if (this.CurrentPiss > this.MaximumPiss)
		{
			this.CurrentPiss = this.MaximumPiss;
		}
		if (this.CurrentWater > this.MaximumWater)
		{
			this.CurrentWater = this.MaximumWater;
		}
		if (this.CurrentRadiation > this.MaximumRadiation)
		{
			this.CurrentRadiation = this.MaximumRadiation;
		}
		this.LegsDamageReductionK = 0;
		this.ArmsDamageReductionK = 0;
		this.HeadsDamageReductionK = 0;
		this.BodysDamageReductionK = 0;
		for (int i = 0; i < this.player.Cloths.InventoryArray.Count; i++)
		{
			if (Inventory.IsCloth(this.player.Cloths.InventoryArray[i].Item))
			{
				this.LegsDamageReductionK += (int)(this.player.Cloths.InventoryArray[i].Item as Cloth).LegsDamageReduction;
				this.ArmsDamageReductionK += (int)(this.player.Cloths.InventoryArray[i].Item as Cloth).ArmsDamageReduction;
				this.HeadsDamageReductionK += (int)(this.player.Cloths.InventoryArray[i].Item as Cloth).HeadsDamageReduction;
				this.BodysDamageReductionK += (int)(this.player.Cloths.InventoryArray[i].Item as Cloth).BodysDamageReduction;
			}
		}
		if (Mathf.Ceil(this.CurrentStamina) <= 5f && !this.fpc.m_IsWalking)
		{
			this.CurrentStamina = 0f;
			this.fpc.CanRun = false;
		}
		if (Mathf.Ceil(this.CurrentStamina) > 5f)
		{
			this.fpc.CanRun = true;
		}
		if (!this.fpc.m_IsWalking)
		{
			this.SubstractStamina(this.RunStamina);
		}
		else
		{
			this.AddStamina(this.RunStamina);
		}
		if (this.CurrentRadiation == this.MaximumRadiation)
		{
			this.GetDamage(DamageType.Head, 0.03f);
		}
		this.links.StaminaSprite.fillAmount = this.CurrentStamina / this.MaximumStamina;
		this.links.FoodSprite.fillAmount = this.CurrentFood / this.MaximumFood;
		this.links.PissSprite.fillAmount = this.CurrentPiss / this.MaximumPiss;
		this.links.WaterSprite.fillAmount = this.CurrentWater / this.MaximumWater;
		this.links.RadiationSprite.fillAmount = this.CurrentRadiation / this.MaximumRadiation;
		if (this.CurrentBody > this.MaximumBody)
		{
			this.CurrentBody = this.MaximumBody;
		}
		if (this.CurrentHead > this.MaximumHead)
		{
			this.CurrentHead = this.MaximumHead;
		}
		if (this.CurrentLeftArm > this.MaximumLeftArm)
		{
			this.CurrentLeftArm = this.MaximumLeftArm;
		}
		if (this.CurrentRightArm > this.MaximumRightArm)
		{
			this.CurrentRightArm = this.MaximumRightArm;
		}
		if (this.CurrentLeftLeg > this.MaximumLeftLeg)
		{
			this.CurrentLeftLeg = this.MaximumLeftLeg;
		}
		if (this.CurrentRightLeg > this.MaximumRightLeg)
		{
			this.CurrentRightLeg = this.MaximumRightLeg;
		}
		if (this.CurrentBody <= 0f || this.CurrentHead <= 0f)
		{
			this.Death();
		}
		if (this.CurrentLeftArm < 0f)
		{
			this.CurrentLeftArm = 0f;
		}
		if (this.CurrentRightArm < 0f)
		{
			this.CurrentRightArm = 0f;
		}
		if (this.CurrentLeftLeg < 0f)
		{
			this.CurrentLeftLeg = 0f;
		}
		if (this.CurrentRightLeg < 0f)
		{
			this.CurrentRightLeg = 0f;
		}
		if (this.CurrentBody + this.CurrentHead + this.CurrentLeftArm + this.CurrentRightArm + this.CurrentLeftLeg + this.CurrentRightLeg < (this.MaximumBody + this.MaximumHead + this.MaximumLeftArm + this.MaximumRightArm + this.MaximumLeftLeg + this.MaximumRightLeg) / 4f)
		{
			this.Death();
		}
		if (this.CurrentHead / this.MaximumHead * 100f == 100f)
		{
			this.links.HeadSprite.color = Color.black;
			this.buffs.RemoveBuff(16);
			this.buffs.RemoveBuff(17);
			this.buffs.RemoveBuff(18);
		}
		if (this.CurrentHead / this.MaximumHead * 100f < 100f && this.CurrentHead / this.MaximumHead * 100f >= 60f)
		{
			this.links.HeadSprite.color = Color.yellow;
			this.buffs.AddBuff(16, 1f);
			this.buffs.RemoveBuff(17);
			this.buffs.RemoveBuff(18);
		}
		if (this.CurrentHead / this.MaximumHead * 100f < 60f && this.CurrentHead / this.MaximumHead * 100f >= 20f)
		{
			this.links.HeadSprite.color = new Color(1f, 0.4f, 0f);
			this.buffs.RemoveBuff(16);
			this.buffs.AddBuff(17, 1f);
			this.buffs.RemoveBuff(18);
		}
		if (this.CurrentHead / this.MaximumHead * 100f < 20f && this.CurrentHead / this.MaximumHead * 100f > 0f)
		{
			this.links.HeadSprite.color = Color.red;
			this.buffs.RemoveBuff(16);
			this.buffs.RemoveBuff(17);
			this.buffs.AddBuff(18, 1f);
		}
		if (this.CurrentHead / this.MaximumHead * 100f == 0f)
		{
			this.links.HeadSprite.color = Color.white;
			this.Death();
		}
		if (this.CurrentBody / this.MaximumBody * 100f == 100f)
		{
			this.links.BodySprite.color = Color.black;
			this.buffs.RemoveBuff(19);
			this.buffs.RemoveBuff(20);
			this.buffs.RemoveBuff(21);
		}
		if (this.CurrentBody / this.MaximumBody * 100f < 100f && this.CurrentBody / this.MaximumBody * 100f >= 60f)
		{
			this.links.BodySprite.color = Color.yellow;
			this.buffs.AddBuff(19, 1f);
			this.buffs.RemoveBuff(20);
			this.buffs.RemoveBuff(21);
		}
		if (this.CurrentBody / this.MaximumBody * 100f < 60f && this.CurrentBody / this.MaximumBody * 100f >= 20f)
		{
			this.links.BodySprite.color = new Color(1f, 0.4f, 0f);
			this.buffs.RemoveBuff(19);
			this.buffs.AddBuff(20, 1f);
			this.buffs.RemoveBuff(21);
		}
		if (this.CurrentBody / this.MaximumBody * 100f < 20f && this.CurrentBody / this.MaximumBody * 100f > 0f)
		{
			this.links.BodySprite.color = Color.red;
			this.buffs.RemoveBuff(19);
			this.buffs.RemoveBuff(20);
			this.buffs.AddBuff(21, 1f);
		}
		if (this.CurrentBody / this.MaximumBody * 100f == 0f)
		{
			this.links.BodySprite.color = Color.white;
			this.Death();
		}
		if (this.CurrentLeftArm / this.MaximumLeftArm * 100f == 100f)
		{
			this.links.LeftArmSprite.color = Color.black;
			this.buffs.RemoveBuff(8);
			this.buffs.RemoveBuff(9);
			this.buffs.RemoveBuff(10);
			this.buffs.RemoveBuff(11);
		}
		if (this.CurrentLeftArm / this.MaximumLeftArm * 100f < 100f && this.CurrentLeftArm / this.MaximumLeftArm * 100f >= 60f)
		{
			this.links.LeftArmSprite.color = Color.yellow;
			this.buffs.AddBuff(8, 1f);
			this.buffs.RemoveBuff(9);
			this.buffs.RemoveBuff(10);
			this.buffs.RemoveBuff(11);
		}
		if (this.CurrentLeftArm / this.MaximumLeftArm * 100f < 60f && this.CurrentLeftArm / this.MaximumLeftArm * 100f >= 20f)
		{
			this.links.LeftArmSprite.color = new Color(1f, 0.4f, 0f);
			this.buffs.RemoveBuff(8);
			this.buffs.AddBuff(9, 1f);
			this.buffs.RemoveBuff(10);
			this.buffs.RemoveBuff(11);
		}
		if (this.CurrentLeftArm / this.MaximumLeftArm * 100f < 20f && this.CurrentLeftArm / this.MaximumLeftArm * 100f > 0f)
		{
			this.links.LeftArmSprite.color = Color.red;
			this.buffs.RemoveBuff(8);
			this.buffs.RemoveBuff(9);
			this.buffs.AddBuff(10, 1f);
			this.buffs.RemoveBuff(11);
		}
		if (this.CurrentLeftArm / this.MaximumLeftArm * 100f == 0f)
		{
			this.links.LeftArmSprite.color = Color.white;
			this.buffs.RemoveBuff(8);
			this.buffs.RemoveBuff(9);
			this.buffs.RemoveBuff(10);
			this.buffs.AddBuff(11, 1f);
		}
		if (this.CurrentRightArm / this.MaximumRightArm * 100f == 100f)
		{
			this.links.RightArmSprite.color = Color.black;
			this.buffs.RemoveBuff(12);
			this.buffs.RemoveBuff(13);
			this.buffs.RemoveBuff(14);
			this.buffs.RemoveBuff(15);
		}
		if (this.CurrentRightArm / this.MaximumRightArm * 100f < 100f && this.CurrentRightArm / this.MaximumRightArm * 100f >= 60f)
		{
			this.links.RightArmSprite.color = Color.yellow;
			this.buffs.AddBuff(12, 1f);
			this.buffs.RemoveBuff(13);
			this.buffs.RemoveBuff(14);
			this.buffs.RemoveBuff(15);
		}
		if (this.CurrentRightArm / this.MaximumRightArm * 100f < 60f && this.CurrentRightArm / this.MaximumRightArm * 100f >= 20f)
		{
			this.links.RightArmSprite.color = new Color(1f, 0.4f, 0f);
			this.buffs.RemoveBuff(12);
			this.buffs.AddBuff(13, 1f);
			this.buffs.RemoveBuff(14);
			this.buffs.RemoveBuff(15);
		}
		if (this.CurrentRightArm / this.MaximumRightArm * 100f < 20f && this.CurrentRightArm / this.MaximumRightArm * 100f > 0f)
		{
			this.links.RightArmSprite.color = Color.red;
			this.buffs.RemoveBuff(12);
			this.buffs.RemoveBuff(13);
			this.buffs.AddBuff(14, 1f);
			this.buffs.RemoveBuff(15);
		}
		if (this.CurrentRightArm / this.MaximumRightArm * 100f == 0f)
		{
			this.links.RightArmSprite.color = Color.white;
			this.buffs.RemoveBuff(12);
			this.buffs.RemoveBuff(13);
			this.buffs.RemoveBuff(14);
			this.buffs.AddBuff(15, 1f);
		}
		if (this.CurrentLeftLeg / this.MaximumLeftLeg * 100f == 100f)
		{
			this.links.LeftLegSprite.color = Color.black;
			this.buffs.RemoveBuff(0);
			this.buffs.RemoveBuff(1);
			this.buffs.RemoveBuff(2);
			this.buffs.RemoveBuff(3);
		}
		if (this.CurrentLeftLeg / this.MaximumLeftLeg * 100f < 100f && this.CurrentLeftLeg / this.MaximumLeftLeg * 100f >= 60f)
		{
			this.links.LeftLegSprite.color = Color.yellow;
			this.buffs.AddBuff(0, 1f);
			this.buffs.RemoveBuff(1);
			this.buffs.RemoveBuff(2);
			this.buffs.RemoveBuff(3);
		}
		if (this.CurrentLeftLeg / this.MaximumLeftLeg * 100f < 60f && this.CurrentLeftLeg / this.MaximumLeftLeg * 100f >= 20f)
		{
			this.links.LeftLegSprite.color = new Color(1f, 0.4f, 0f);
			this.buffs.RemoveBuff(0);
			this.buffs.AddBuff(1, 1f);
			this.buffs.RemoveBuff(2);
			this.buffs.RemoveBuff(3);
		}
		if (this.CurrentLeftLeg / this.MaximumLeftLeg * 100f < 20f && this.CurrentLeftLeg / this.MaximumLeftLeg * 100f > 0f)
		{
			this.links.LeftLegSprite.color = Color.red;
			this.buffs.RemoveBuff(0);
			this.buffs.RemoveBuff(1);
			this.buffs.AddBuff(2, 1f);
			this.buffs.RemoveBuff(3);
		}
		if (this.CurrentLeftLeg / this.MaximumLeftLeg * 100f == 0f)
		{
			this.links.LeftLegSprite.color = Color.white;
			this.buffs.RemoveBuff(0);
			this.buffs.RemoveBuff(1);
			this.buffs.RemoveBuff(2);
			this.buffs.AddBuff(3, 1f);
		}
		if (this.CurrentRightLeg / this.MaximumRightLeg * 100f == 100f)
		{
			this.links.RightLegSprite.color = Color.black;
			this.buffs.RemoveBuff(4);
			this.buffs.RemoveBuff(5);
			this.buffs.RemoveBuff(6);
			this.buffs.RemoveBuff(7);
		}
		if (this.CurrentRightLeg / this.MaximumRightLeg * 100f < 100f && this.CurrentRightLeg / this.MaximumRightLeg * 100f >= 60f)
		{
			this.links.RightLegSprite.color = Color.yellow;
			this.buffs.AddBuff(4, 1f);
			this.buffs.RemoveBuff(5);
			this.buffs.RemoveBuff(6);
			this.buffs.RemoveBuff(7);
		}
		if (this.CurrentRightLeg / this.MaximumRightLeg * 100f < 60f && this.CurrentRightLeg / this.MaximumRightLeg * 100f >= 20f)
		{
			this.links.RightLegSprite.color = new Color(1f, 0.4f, 0f);
			this.buffs.RemoveBuff(4);
			this.buffs.AddBuff(5, 1f);
			this.buffs.RemoveBuff(6);
			this.buffs.RemoveBuff(7);
		}
		if (this.CurrentRightLeg / this.MaximumRightLeg * 100f < 20f && this.CurrentRightLeg / this.MaximumRightLeg * 100f > 0f)
		{
			this.links.RightLegSprite.color = Color.red;
			this.buffs.RemoveBuff(4);
			this.buffs.RemoveBuff(5);
			this.buffs.AddBuff(6, 1f);
			this.buffs.RemoveBuff(7);
		}
		if (this.CurrentRightLeg / this.MaximumRightLeg * 100f == 0f)
		{
			this.links.RightLegSprite.color = Color.white;
			this.buffs.RemoveBuff(4);
			this.buffs.RemoveBuff(5);
			this.buffs.RemoveBuff(6);
			this.buffs.AddBuff(7, 1f);
		}
	}

	private void Death()
	{
		if (!this.IsDead)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.IsDead = true;
			this.player.manager.InitDeath(base.gameObject);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.manager.DeadPlayer, base.transform.position, Quaternion.identity);
			gameObject.transform.position += new Vector3(0f, 2f, 0f);
			gameObject.GetComponent<Inventory>().InventoryArray = new List<Slot>();
			foreach (Slot current in this.player.MainInventory.InventoryArray)
			{
				if (current.Item != null)
				{
					Slot item = new Slot(Database.Get(current.Item.Id))
					{
						CurrentClipSize = current.CurrentClipSize,
						CurrentDurability = current.CurrentDurability,
						Count = current.Count
					};
					gameObject.GetComponent<Inventory>().InventoryArray.Add(item);
				}
			}
			using (List<Slot>.Enumerator enumerator = this.player.MainInventory.InventoryArray.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Item = null;
				}
			}
			base.gameObject.SetActive(false);
		}
	}

	public bool AddHealth(DamageType type, float power)
	{
		switch (type)
		{
		case DamageType.Legs:
			if (this.CurrentLeftLeg >= this.MaximumLeftLeg && this.CurrentRightLeg >= this.MaximumRightLeg)
			{
				return false;
			}
			this.CurrentLeftLeg += power;
			this.CurrentRightLeg += power;
			break;
		case DamageType.Arms:
			if (this.CurrentLeftArm >= this.MaximumLeftArm && this.CurrentRightArm >= this.MaximumRightArm)
			{
				return false;
			}
			this.CurrentLeftArm += power;
			this.CurrentRightArm += power;
			break;
		case DamageType.Head:
			if (this.CurrentHead >= this.MaximumHead)
			{
				return false;
			}
			this.CurrentHead += power;
			break;
		case DamageType.Body:
			if (this.CurrentBody >= this.MaximumBody)
			{
				return false;
			}
			this.CurrentBody += power;
			break;
		case DamageType.Pure:
			if (this.CurrentBody >= this.MaximumBody && this.CurrentHead >= this.MaximumHead && this.CurrentLeftArm >= this.MaximumLeftArm && this.CurrentRightArm >= this.MaximumRightArm && this.CurrentLeftLeg >= this.MaximumLeftLeg && this.CurrentRightLeg >= this.MaximumRightLeg)
			{
				return false;
			}
			this.CurrentBody += power;
			this.CurrentHead += power;
			this.CurrentLeftArm += power;
			this.CurrentRightArm += power;
			this.CurrentLeftLeg += power;
			this.CurrentRightLeg += power;
			break;
		}
		return true;
	}

	public bool AddFood(float power)
	{
		if (this.CurrentFood == this.MaximumFood)
		{
			return false;
		}
		this.CurrentFood += power;
		return true;
	}

	public bool AddWater(float power)
	{
		if (this.CurrentWater == this.MaximumWater)
		{
			return false;
		}
		this.CurrentWater += power;
		return true;
	}

	public bool AddStamina(float power)
	{
		if (this.CurrentStamina == this.MaximumStamina)
		{
			return false;
		}
		this.CurrentStamina += power;
		return true;
	}

	public void AddRadiation(float power)
	{
		if (this.CurrentRadiation < this.MaximumRadiation)
		{
			this.CurrentRadiation += power;
			this.source.loop = true;
			this.source.clip = this.RadiationSound;
			this.source.Play();
		}
	}

	public void Piss()
	{
		if (this.CurrentPiss > 2f)
		{
			this.CurrentWater -= this.CurrentPiss * this.WaterPerPiss;
			if (!this.player.MainInventory.Add("piss", (int)Mathf.Ceil(this.CurrentPiss) * 2))
			{
				GameObject gameObject;
				if (Database.Get("piss").Drop != null)
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(Database.Get("piss").Drop, this.player.DropZone.transform.position, Quaternion.identity);
				}
				else
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("ItemDrops/Unknown"), this.player.DropZone.transform.position, Quaternion.identity);
				}
				gameObject.GetComponent<Pickup>().slot = new Slot(Database.Get("piss"))
				{
					Count = (int)Mathf.Ceil(this.CurrentPiss) * 2
				};
			}
			else
			{
				this.player.MainInventory.Show();
			}
			this.CurrentPiss = 0f;
			this.source.clip = this.SickSound;
			this.source.loop = false;
			this.source.Play();
		}
	}

	public void Restore()
	{
		this.IsDead = false;
		this.CurrentStamina = this.MaximumStamina;
		this.CurrentFood = this.MaximumFood;
		this.CurrentPiss = 0f;
		this.CurrentWater = this.MaximumWater;
		this.CurrentRadiation = 0f;
		this.CurrentBody = this.MaximumBody;
		this.CurrentHead = this.MaximumHead;
		this.CurrentLeftArm = this.MaximumLeftArm;
		this.CurrentRightArm = this.MaximumRightArm;
		this.CurrentLeftLeg = this.MaximumLeftLeg;
		this.CurrentRightLeg = this.MaximumRightLeg;
	}
}
