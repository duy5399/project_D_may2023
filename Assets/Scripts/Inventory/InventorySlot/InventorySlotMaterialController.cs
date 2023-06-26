using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class InventorySlotMaterialController : InventorySlotController, IPointerClickHandler
{
    [SerializeField] private MaterialSO material;
    public MaterialSO material_ => material;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetQuantity(int _quantity)
    {
        this.quantity = _quantity;
    }

    //add new item
    public override void AddItem(ItemSO _item, int _quantity)
    {
        base.AddItem(_item, _quantity);
    }

    public void AddItemInfo(InventoryMaterialSlot _item, int _quantity)
    {
        material = MaterialSO.Init(_item.idItem_, _item.type_, _item.icon_, _item.name_, _item.tier_, _item.description_, _item.maxStack_, _item.itemUses_, _item.canCombine_);
        quantity = _quantity;
        iconImg.sprite = material.itemIcon_;
        quantityTxt.text = quantity.ToString();
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
                InventoryManager.instance.RemoveItem(material);
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
                InventoryManager.instance.RemoveItem(material);
                UpgradeEquipmentManager.instance.UpdateSuccessRate();
                Destroy(gameObject);
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
                InventoryManager.instance.RemoveItem(material);
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
        if (transform.root.GetChild(6).gameObject.activeInHierarchy && !transform.root.GetChild(5).gameObject.activeInHierarchy)
        {
            if(transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
            {
                if (transform.parent.name == "InventoryMaterial" && material.itemType_ == ItemSO.ItemType.Material)
                {
                    AddMaterialToUpgradeSlot();
                }
            }
            else if (transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy)
            {
                if (transform.parent.name == "InventoryMaterial" && material.itemType_ == ItemSO.ItemType.Material)
                {
                    AddMaterialToCombineSlot();
                }
            }
        }
    }
}


