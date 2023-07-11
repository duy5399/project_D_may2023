using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using static ItemSO;

public class InventorySlotMaterialController : InventorySlotController, IPointerClickHandler
{
    [SerializeField] private MaterialSO material;
    public MaterialSO material_ => material;

    protected override void Awake()
    {
        base.Awake();
    }

    //add new item
    public override void AddItem(ItemSO _item, int _quantity)
    {
        base.AddItem(_item, _quantity);
    }

    public void AddItemInfo(InventoryMaterialSlot _item, int _quantity)
    {
        Sprite iconItem_ = Resources.Load<Sprite>(_item.iconPath_);

        material = MaterialSO.Init(_item.idItem_, _item.type_, iconItem_, _item.name_, _item.tier_, _item.description_, _item.maxStack_, _item.itemUses_, _item.canCombine_);
        quantity = _quantity;
        itemIcon.sprite = material.itemIcon_;
        quantityTxt.text = quantity.ToString();
        switch (material.itemTier_)
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
                //Debug.Log("Not found rarity tier of item: " + material.itemName_);
                break;
        }
    }

    //Add strength stone to strength stone slot
    public void AddMaterialToUpgradeSlot()
    {
        if (material.itemID_.Contains("strengthStone"))
        {
            if (UpgradeEquipmentManager.instance.AddStrengthStoneToUpgradeSlot(material))
            {
                quantity -= 1;
                quantityTxt.text = quantity.ToString();
                UpgradeEquipmentManager.instance.AddStrengthStone(material);
                InventoryManager.instance.RemoveItem(material, 1);
                UpgradeEquipmentManager.instance.UpdateSuccessRate();
                if (quantity == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (material.itemID_.Contains("godCharm"))
        {
            if (UpgradeEquipmentManager.instance.AddGodCharmToUpgradeSlot(material))
            {
                quantity -= 1;
                quantityTxt.text = quantity.ToString();
                InventoryManager.instance.RemoveItem(material, 1);
                UpgradeEquipmentManager.instance.UpdateSuccessRate();
                if (quantity == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        
    }

    //Add strength stone to strength stone slot
    public void AddMaterialToCombineSlot()
    {
        if (material.canCombine_)
        {
            if (CombineItemManager.instance.AddItemToCombineSlot(material))
            {
                quantity -= 1;
                quantityTxt.text = quantity.ToString();
                CombineItemManager.instance.AddItem(material);
                InventoryManager.instance.RemoveItem(material, 1);
                CombineItemManager.instance.SetPreviewFinishedProduct();
                CombineItemManager.instance.SetSuccessRateCombine();
                if (quantity == 0)
                {
                    Destroy(gameObject);
                }
            }           
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
            AudioManager.instance.ClickSuccessSFX();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public override void DisplayItemDescription()
    {
        InventoryDesciptionController.instance.SetDescription(material);
    }

    public override void OnRightClick()
    {
        if (transform.root.GetChild(7).gameObject.activeInHierarchy && !transform.root.GetChild(6).gameObject.activeInHierarchy)
        {
            if(transform.root.GetChild(7).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(7).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
            {
                if (transform.parent.name == "InventoryMaterial" && material.itemType_ == ItemSO.ItemType.Material)
                {
                    AddMaterialToUpgradeSlot();
                }
            }
            else if (transform.root.GetChild(7).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(7).GetChild(0).GetChild(2).gameObject.activeInHierarchy)
            {
                if (transform.parent.name == "InventoryMaterial" && material.itemType_ == ItemSO.ItemType.Material)
                {
                    AddMaterialToCombineSlot();
                }
            }
        }
    }
}


