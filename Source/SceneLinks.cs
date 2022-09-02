using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLinks : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject AirDropShip;

	public Transform PlayerSpawnPointsObject;

	public List<Transform> PlayerSpawnPoints = new List<Transform>();

	public GameObject DeathUI;

	public GameObject BuffsField;

	public InputField DropCountField;

	public GameObject CraftsGrid;

	public Transform CraftsInfoPanel;

	public GameObject CraftSlot;

	public Image HeadSprite;

	public Image BodySprite;

	public Image LeftArmSprite;

	public Image RightArmSprite;

	public Image LeftLegSprite;

	public Image RightLegSprite;

	public Image FoodSprite;

	public Image StaminaSprite;

	public Image WaterSprite;

	public Image RadiationSprite;

	public Image PissSprite;

	public Text QuestTitle;

	public Text QuestDescription;

	public Image QuestCompletedPanel;

	public GameObject ChestPanel;

	public GameObject CraftPanel;

	public GameObject CampfirePanel;

	public GameObject ForgePanel;

	public GameObject CrusherPanel;

	public GameObject DropZone;

	public Text Uitext;

	public GameObject UitextObject;

	public Text DurabilityText;

	public GameObject DurabilityTextObject;

	public Transform ItemsDescrition;

	public GameObject DragIcon;

	public Text PosField;

	public Text ToggleButtonText1;

	public Text ToggleButtonText2;

	public Image ToggleImage1;

	public Image ToggleImage2;

	public GameObject ButtonDrop;

	public GameObject InventoryPanel;

	public Text DateTime;

	public void Start()
	{
		Transform transform = GameObject.Find("UI").transform;
		this.DeathUI = transform.Find("DeathScreen").gameObject;
		this.BuffsField = transform.Find("BuffsField").gameObject;
		this.DropCountField = transform.Find("Inventory/ItemDescription/Count").GetComponent<InputField>();
		this.CraftsGrid = transform.Find("Inventory/Craft/Zone/FiledCraft").gameObject;
		this.CraftsInfoPanel = transform.Find("Inventory/CraftDescription");
		this.FoodSprite = transform.Find("Stats/Food").GetComponent<Image>();
		this.StaminaSprite = transform.Find("Stats/Stamina").GetComponent<Image>();
		this.WaterSprite = transform.Find("Stats/Water").GetComponent<Image>();
		this.RadiationSprite = transform.Find("Stats/Radiation").GetComponent<Image>();
		this.PissSprite = transform.Find("Stats/Piss").GetComponent<Image>();
		this.HeadSprite = transform.Find("Body/Head").GetComponent<Image>();
		this.BodySprite = transform.Find("Body/Body").GetComponent<Image>();
		this.LeftArmSprite = transform.Find("Body/LeftArm").GetComponent<Image>();
		this.RightArmSprite = transform.Find("Body/RightArm").GetComponent<Image>();
		this.LeftLegSprite = transform.Find("Body/LeftLeg").GetComponent<Image>();
		this.RightLegSprite = transform.Find("Body/RightLeg").GetComponent<Image>();
		this.QuestTitle = transform.Find("QuestTitle").GetComponent<Text>();
		this.QuestDescription = transform.Find("QuestDescription").GetComponent<Text>();
		this.QuestCompletedPanel = transform.Find("QuestCompleted").GetComponent<Image>();
		this.ChestPanel = transform.Find("Inventory/Chest").gameObject;
		this.CraftPanel = transform.Find("Inventory/Craft").gameObject;
		this.CampfirePanel = transform.Find("Inventory/Campfire").gameObject;
		this.ForgePanel = transform.Find("Inventory/Forge").gameObject;
		this.CrusherPanel = transform.Find("Inventory/Crusher").gameObject;
		this.DropZone = transform.Find("Inventory/Chest").gameObject;
		this.Uitext = transform.Find("TextPanel/Text").GetComponent<Text>();
		this.UitextObject = transform.Find("TextPanel").gameObject;
		this.DurabilityText = transform.Find("DurabilityPanel/DurabilityText").GetComponent<Text>();
		this.DurabilityTextObject = transform.Find("DurabilityPanel").gameObject;
		this.ItemsDescrition = transform.Find("Inventory/ItemDescription");
		this.DragIcon = transform.Find("Inventory/Panel").gameObject;
		this.PosField = transform.Find("Position").GetComponent<Text>();
		this.ToggleButtonText1 = transform.Find("Inventory/Forge/OnOff/Text").GetComponent<Text>();
		this.ToggleButtonText2 = transform.Find("Inventory/Campfire/OnOff/Text").GetComponent<Text>();
		this.ToggleImage1 = transform.Find("Inventory/Forge/OnOff/Image").GetComponent<Image>();
		this.ToggleImage2 = transform.Find("Inventory/Campfire/OnOff/Image").GetComponent<Image>();
		this.ButtonDrop = transform.Find("Inventory/ItemDescription/Drop").gameObject;
		this.InventoryPanel = transform.Find("Inventory").gameObject;
		this.DateTime = transform.Find("DateTime/DateTime").GetComponent<Text>();
	}
}
