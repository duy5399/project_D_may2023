using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemSO;

public class InventorySlotEquipmentController : InventorySlotController, IPointerClickHandler
{
    [SerializeField] private EquipmentSO equipment;
    public EquipmentSO equipment_ => equipment;

    protected override void Awake()
    {
        base.Awake();
    }

    //thêm thông tin vật phẩm cho object InventorySlot này
    public void AddItemInfo(InventoryEquipmentSlot _item, int _quantity)
    {
        equipment = EquipmentSO.Init(_item.idItem_, _item.type_, _item.icon_, _item.name_, _item.tier_, _item.description_, _item.maxStack_, _item.slot_, _item.show_, _item.stats_, _item.canUpgrade_, _item.itemStrength_, _item.itemStrengthImg_);
        quantity = _quantity;
        itemIcon.sprite = equipment.itemIcon_;
        quantityTxt.text = quantity.ToString();
        switch (equipment.itemTier_)
        {
            case RarityTier.common:
                itemBorder.color = new Color32(209, 213, 216, 255);
                break;
            case RarityTier.uncommmon:
                itemBorder.color = new Color32(65, 168, 95, 255);
                break;
            case RarityTier.rare:
                itemBorder.color = new Color32(44, 130, 201, 255);
                break;
            case RarityTier.epic:
                itemBorder.color = new Color32(147, 101, 184, 255);
                break;
            case RarityTier.legendary:
                itemBorder.color = new Color32(250, 197, 28, 255);
                break;
            case RarityTier.mythic:
                itemBorder.color = new Color32(226, 80, 65, 255);
                break;
            default:
                Debug.Log("Not found rarity tier of item: " + equipment.itemName_);
                break;
        }
    }

    //equip item for player
    public void EquipGear()
    {
        CharacterEquipmentManager.instance.EquipGear(equipment);
        CharacterEquipmentManager.instance.AddItem(equipment);
        InventoryManager.instance.RemoveItem(equipment);
        Destroy(gameObject);
    }

    //Add gear to upgrade slot
    public void AddGearToUpgradeSlot()
    {
        UpgradeEquipmentManager.instance.AddGearToUpgradeSlot(equipment);
        UpgradeEquipmentManager.instance.AddEquipment(equipment);
        InventoryManager.instance.RemoveItem(equipment);
        UpgradeEquipmentManager.instance.UpdateSuccessRate();
        Destroy(gameObject);
    }

    //Add gear to combine slot
    public void AddGearToCombineSlot()
    {
        if(equipment.itemTier_ != ItemSO.RarityTier.mythic)
        {
            if (CombineItemManager.instance.AddItemToCombineSlot(equipment))
            {
                CombineItemManager.instance.AddItem(equipment);
                InventoryManager.instance.RemoveItem(equipment);
                CombineItemManager.instance.SetPreviewFinishedProduct();
                CombineItemManager.instance.SetSuccessRateCombine();
                Destroy(gameObject);
            }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public override void DisplayItemDescription()
    {
        InventoryDesciptionController.instance.SetDescription(equipment);
    }

    public override void OnRightClick()
    {
        if (transform.root.GetChild(5).gameObject.activeInHierarchy && !transform.root.GetChild(6).gameObject.activeInHierarchy)
        {
            if (transform.parent.name == "InventoryEquipment" && equipment.itemType_ == ItemSO.ItemType.Equipment)
            {
                EquipGear();
            }
        }
        else if (transform.root.GetChild(6).gameObject.activeInHierarchy && !transform.root.GetChild(5).gameObject.activeInHierarchy)
        {
            if(transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
            {
                if (transform.parent.name == "InventoryEquipment" && equipment.itemType_ == ItemSO.ItemType.Equipment)
                {
                    AddGearToUpgradeSlot();
                }
            }
            else if(transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy){
                if (transform.parent.name == "InventoryEquipment" && equipment.itemType_ == ItemSO.ItemType.Equipment)
                {
                    AddGearToCombineSlot();
                }
            }
        }
    }
}

