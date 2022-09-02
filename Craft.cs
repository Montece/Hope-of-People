using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
	public List<CraftInfo> currentCrafts = new List<CraftInfo>();

	[HideInInspector]
	public Player player;

	public CraftInfo CurrentInfo;

	public int Count = -1;

	private SceneLinks links;

	public void OnStart(SceneLinks links)
	{
		this.links = links;
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		this.HidePanel();
	}

	public void AddCount(int amount)
	{
		if (this.Count != -1)
		{
			this.Count += amount;
			this.links.CraftsInfoPanel.transform.GetChild(9).GetComponent<InputField>().text = this.Count.ToString();
		}
	}

	public void SetCount()
	{
		if (this.Count != -1)
		{
			this.Count = int.Parse(this.links.CraftsInfoPanel.transform.GetChild(9).GetComponent<InputField>().text);
		}
	}

	public void RefreshList(int id = 0)
	{
		IEnumerator enumerator = this.links.CraftsGrid.transform.GetEnumerator();
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
		if (id == 0)
		{
			for (int i = 0; i < this.currentCrafts.Count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.links.CraftSlot, this.links.CraftsGrid.transform);
				gameObject.GetComponent<CraftObject>().info = this.currentCrafts[i];
				gameObject.transform.GetChild(0).GetComponent<Text>().text = this.currentCrafts[i].Result.Item.Title;
				gameObject.transform.GetChild(1).GetComponent<Image>().sprite = this.currentCrafts[i].Result.Item.Icon;
				gameObject.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
				for (int j = 0; j < this.currentCrafts[i].NeedResources.Length; j++)
				{
					if (j == this.currentCrafts[i].NeedResources.Length - 1)
					{
						Text expr_15A = gameObject.transform.GetChild(2).GetComponent<Text>();
						string text = expr_15A.text;
						expr_15A.text = string.Concat(new object[]
						{
							text,
							this.currentCrafts[i].NeedResources[j].Item.Title,
							" (",
							this.currentCrafts[i].NeedResources[j].Count,
							")"
						});
					}
					else
					{
						Text expr_1E2 = gameObject.transform.GetChild(2).GetComponent<Text>();
						string text = expr_1E2.text;
						expr_1E2.text = string.Concat(new object[]
						{
							text,
							this.currentCrafts[i].NeedResources[j].Item.Title,
							" (",
							this.currentCrafts[i].NeedResources[j].Count,
							"), "
						});
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < this.currentCrafts.Count; k++)
			{
				if (this.currentCrafts[k].Result.Item.ItemType == (ItemType)id)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.links.CraftSlot, this.links.CraftsGrid.transform);
					gameObject2.GetComponent<CraftObject>().info = this.currentCrafts[k];
					gameObject2.transform.GetChild(0).GetComponent<Text>().text = this.currentCrafts[k].Result.Item.Title;
					gameObject2.transform.GetChild(1).GetComponent<Image>().sprite = this.currentCrafts[k].Result.Item.Icon;
					gameObject2.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
					for (int l = 0; l < this.currentCrafts[k].NeedResources.Length; l++)
					{
						if (l == this.currentCrafts[k].NeedResources.Length - 1)
						{
							Text expr_3AF = gameObject2.transform.GetChild(2).GetComponent<Text>();
							string text = expr_3AF.text;
							expr_3AF.text = string.Concat(new object[]
							{
								text,
								this.currentCrafts[k].NeedResources[l].Item.Title,
								" (",
								this.currentCrafts[k].NeedResources[l].Count,
								")"
							});
						}
						else
						{
							Text expr_437 = gameObject2.transform.GetChild(2).GetComponent<Text>();
							string text = expr_437.text;
							expr_437.text = string.Concat(new object[]
							{
								text,
								this.currentCrafts[k].NeedResources[l].Item.Title,
								" (",
								this.currentCrafts[k].NeedResources[l].Count,
								"), "
							});
						}
					}
				}
			}
		}
	}

	public bool AddBlueprint(CraftInfo info)
	{
		bool flag = true;
		if (info != null)
		{
			for (int i = 0; i < this.currentCrafts.Count; i++)
			{
				if (this.currentCrafts[i] == info)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.RefreshList(0);
				this.currentCrafts.Add(info);
			}
			return flag;
		}
		return false;
	}

	public void TryCraft()
	{
		if (this.Count > 0 && this.player.TryCraft(this.CurrentInfo, this.Count))
		{
			this.player.GetComponent<QuestSystem>().DoCraft(this.CurrentInfo.Result.Item, this.Count);
			this.HidePanel();
		}
	}

	public void ShowPanel()
	{
		this.links.CraftsInfoPanel.gameObject.SetActive(true);
		this.links.CraftsInfoPanel.transform.GetChild(1).GetComponent<Text>().text = this.CurrentInfo.Result.Item.Title;
		this.links.CraftsInfoPanel.transform.GetChild(2).GetComponent<Image>().sprite = this.CurrentInfo.Result.Item.Icon;
		this.links.CraftsInfoPanel.transform.GetChild(3).GetComponent<Text>().text = string.Empty;
		for (int i = 0; i < this.CurrentInfo.NeedResources.Length; i++)
		{
			if (i == this.CurrentInfo.NeedResources.Length - 1)
			{
				Text expr_DC = this.links.CraftsInfoPanel.transform.GetChild(3).GetComponent<Text>();
				string text = expr_DC.text;
				expr_DC.text = string.Concat(new object[]
				{
					text,
					this.CurrentInfo.NeedResources[i].Item.Title,
					" (",
					this.CurrentInfo.NeedResources[i].Count,
					")"
				});
			}
			else
			{
				Text expr_15B = this.links.CraftsInfoPanel.transform.GetChild(3).GetComponent<Text>();
				string text = expr_15B.text;
				expr_15B.text = string.Concat(new object[]
				{
					text,
					this.CurrentInfo.NeedResources[i].Item.Title,
					" (",
					this.CurrentInfo.NeedResources[i].Count,
					"), "
				});
			}
		}
		this.links.CraftsInfoPanel.transform.GetChild(9).GetComponent<InputField>().text = "1";
		this.Count = 1;
	}

	public void HidePanel()
	{
		this.links.CraftsInfoPanel.transform.GetChild(1).GetComponent<Text>().text = string.Empty;
		this.links.CraftsInfoPanel.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemIcons/None");
		this.links.CraftsInfoPanel.transform.GetChild(3).GetComponent<Text>().text = string.Empty;
		this.links.CraftsInfoPanel.transform.GetChild(9).GetComponent<InputField>().text = "1";
		this.Count = -1;
		this.links.CraftsInfoPanel.gameObject.SetActive(false);
	}
}
