using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class Buffs : MonoBehaviour
{
	public List<BuffSlot> CurrentBuffs = new List<BuffSlot>();

	public GameObject BuffsSlot;

	private PlayerStats stats;

	private FirstPersonController fpc;

	private VignetteAndChromaticAberration effect;

	private Player player;

	private SceneLinks links;

	private float NormalWalkSpeed;

	private float NormalRunSpeed;

	private float NormalJumpSpeed;

	private float NormalFoodRedutcion;

	private float NormalCamEffectK;

	private float NormalDamageK = 1f;

	private float NormalToolsSpeed = 1f;

	public void OnStart()
	{
		this.stats = base.GetComponent<PlayerStats>();
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		this.fpc = gameObject.GetComponent<FirstPersonController>();
		this.effect = gameObject.GetComponentInChildren<VignetteAndChromaticAberration>();
		this.player = gameObject.GetComponent<Player>();
		this.links = base.GetComponent<SceneLinks>();
		this.NormalWalkSpeed = this.fpc.m_WalkSpeed;
		this.NormalRunSpeed = this.fpc.m_RunSpeed;
		this.NormalJumpSpeed = this.fpc.m_JumpSpeed;
		this.NormalFoodRedutcion = this.stats.FoodReduction;
		this.NormalDamageK = this.player.WeaponDamageK;
		this.NormalToolsSpeed = this.player.ToolsSpeed;
	}

	private void Update()
	{
		if (this.fpc == null)
		{
			return;
		}
		this.fpc.m_WalkSpeed = this.NormalWalkSpeed;
		this.fpc.m_RunSpeed = this.NormalRunSpeed;
		this.fpc.m_JumpSpeed = this.NormalJumpSpeed;
		this.stats.FoodReduction = this.NormalFoodRedutcion;
		this.effect.intensity = this.NormalCamEffectK;
		this.player.WeaponDamageK = this.NormalDamageK;
		this.player.ToolsSpeed = this.NormalToolsSpeed;
		for (int i = 0; i < this.CurrentBuffs.Count; i++)
		{
			if (!this.CurrentBuffs[i].Buff.IsInfinity && this.CurrentBuffs[i].CurrentTime <= 0f)
			{
				this.CurrentBuffs.RemoveAt(i);
				i--;
			}
			else
			{
				switch (this.CurrentBuffs[i].Buff.Id)
				{
				case 0:
					this.fpc.m_WalkSpeed = this.NormalWalkSpeed * 0.7f;
					this.fpc.m_RunSpeed = this.NormalRunSpeed * 0.7f;
					break;
				case 1:
					this.fpc.m_WalkSpeed = this.NormalWalkSpeed * 0.4f;
					this.fpc.m_RunSpeed = this.NormalRunSpeed * 0.4f;
					break;
				case 2:
					this.fpc.m_WalkSpeed = this.NormalWalkSpeed * 0.2f;
					this.fpc.m_RunSpeed = this.NormalRunSpeed * 0.2f;
					break;
				case 3:
					this.fpc.m_WalkSpeed = 0f;
					this.fpc.m_RunSpeed = 0f;
					break;
				case 4:
					this.fpc.m_JumpSpeed = this.NormalJumpSpeed * 0.8f;
					break;
				case 5:
					this.fpc.m_JumpSpeed = this.NormalJumpSpeed * 0.6f;
					break;
				case 6:
					this.fpc.m_JumpSpeed = this.NormalJumpSpeed * 0.4f;
					break;
				case 7:
					this.fpc.m_JumpSpeed = 0f;
					break;
				case 8:
					this.player.ToolsSpeed = 0.8f;
					break;
				case 9:
					this.player.ToolsSpeed = 0.6f;
					break;
				case 10:
					this.player.ToolsSpeed = 0.4f;
					break;
				case 11:
					this.player.CanUseTools = false;
					break;
				case 12:
					this.player.WeaponDamageK = 0.8f;
					break;
				case 13:
					this.player.WeaponDamageK = 0.6f;
					break;
				case 14:
					this.player.WeaponDamageK = 0.4f;
					break;
				case 15:
					this.player.CanShoot = false;
					break;
				case 16:
					this.effect.intensity = 0.5f;
					break;
				case 17:
					this.effect.intensity = 0.7f;
					break;
				case 18:
					this.effect.intensity = 0.9f;
					break;
				case 19:
					this.stats.FoodReduction = this.NormalFoodRedutcion * 2f;
					break;
				case 20:
					this.stats.FoodReduction = this.NormalFoodRedutcion * 3f;
					break;
				case 21:
					this.stats.FoodReduction = this.NormalFoodRedutcion * 4f;
					break;
				case 22:
					this.fpc.m_JumpSpeed *= 3f;
					break;
				case 23:
					this.stats.AddHealth(DamageType.Pure, 0.01f);
					break;
				case 24:
					this.fpc.m_RunSpeed *= 3f;
					this.fpc.m_WalkSpeed *= 3f;
					break;
				case 25:
					this.stats.GetDamage(DamageType.Legs, 0.01f);
					break;
				case 26:
					this.stats.GetDamage(DamageType.Legs, 0.01f);
					break;
				}
				if (!this.CurrentBuffs[i].Buff.IsInfinity)
				{
					this.CurrentBuffs[i].CurrentTime -= Time.deltaTime;
				}
			}
		}
		this.RefreshBuffs();
	}

	public void AddBuff(int id, float time = 1f)
	{
		Buff buffByID = Database.GetBuffByID(id);
		bool flag = false;
		int index = 0;
		if (buffByID != null && time > 0f)
		{
			for (int i = 0; i < this.CurrentBuffs.Count; i++)
			{
				if (this.CurrentBuffs[i].Buff.Id == id)
				{
					flag = true;
					index = i;
					break;
				}
			}
			if (!flag)
			{
				this.CurrentBuffs.Add(new BuffSlot
				{
					Buff = buffByID,
					CurrentTime = time
				});
			}
			else
			{
				this.CurrentBuffs[index].CurrentTime = time;
			}
		}
	}

	public bool RemoveBuff(int id)
	{
		bool result = false;
		for (int i = 0; i < this.CurrentBuffs.Count; i++)
		{
			if (this.CurrentBuffs[i].Buff.Id == id)
			{
				this.CurrentBuffs.RemoveAt(i);
				result = true;
				break;
			}
		}
		return result;
	}

	public void RefreshBuffs()
	{
		IEnumerator enumerator = this.links.BuffsField.transform.GetEnumerator();
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
		foreach (BuffSlot current in this.CurrentBuffs)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BuffsSlot, this.links.BuffsField.transform);
			gameObject.transform.GetChild(0).GetComponent<Text>().text = current.Buff.Title;
			gameObject.transform.GetChild(1).GetComponent<Image>().sprite = current.Buff.Icon;
			gameObject.transform.GetChild(2).GetComponent<Text>().text = ((int)current.CurrentTime).ToString();
		}
	}
}
