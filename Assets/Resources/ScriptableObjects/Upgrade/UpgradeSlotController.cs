using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlotController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity;
    [SerializeField] private EquipmentSO equipment;
    [SerializeField] private MaterialSO material;
    [SerializeField] private bool slotInUse;

    public EquipmentSO equipment_ => equipment;
    public MaterialSO material_ => material;
    public bool slotInUse_ => slotInUse;

    void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        icon.enabled = false;
        quantity = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    #region Display img item and quantity

    public void SetDisplayUpgradeSlot(ItemSO _item)
    {
        //update img to equipped slot
        icon.sprite = _item.itemIcon_;
        icon.enabled = true;
        //push new gear to equipment list
        slotInUse = true;
    }

    public void ResetDisplayUpgradeSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        slotInUse = false;
        equipment = null;
        material = null;
    }
    #endregion
    #region Equipment
    //equip gear to upgrade slot character
    public void AddGearToUpgradeSlot(ItemSO _item)
    {
        //if this slot is equipped, unequip and sent it to inventory before equip new gear
        if (slotInUse)
        {
            RemoveGearFromUpgradeSlot();
        }
        equipment = (EquipmentSO)_item;
        SetDisplayUpgradeSlot(equipment);
        InventoryManager.instance.UpdateUIInventory();
    }

    //gỡ item còn gắn trong các ô nâng cấp và trả về túi đồ
    public void RemoveGearFromUpgradeSlot()
    {
        InventoryManager.instance.AddItem(equipment);
        if (transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
        {
            UpgradeEquipmentManager.instance.RemoveEquipment(equipment);
            UpgradeEquipmentManager.instance.UpdateSuccessRate();
        }

        if (transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy)
        {
            CombineItemManager.instance.RemoveItem(equipment);
            CombineItemManager.instance.SetPreviewFinishedProduct();
        }
        InventoryManager.instance.UpdateUIInventory();
        ResetDisplayUpgradeSlot();
    }
    #endregion
    #region Material
    public bool AddMaterialToUpgradeSlot(ItemSO _item)
    {
        MaterialSO _material = (MaterialSO)_item;
        //Debug.Log(transform.name + _material);
        if (_material.itemUses_.ToString() == "Upgrade" && !slotInUse)
        {
            material = (MaterialSO)_item;
            SetDisplayUpgradeSlot(material);
            //Debug.Log("AddStrengthStoneToUpgradeSlot" + transform.name + " true");
            return true;
        }
        else
        {
            //Debug.Log("AddStrengthStoneToUpgradeSlot" + transform.name + " false");
            return false;
        }
    }

    public void RemoveMaterialFromUpgradeSlot()
    {
        InventoryManager.instance.AddItem(material);
        if (transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
        {
            if (material.itemID_.Contains("strengthStone"))
            {
                UpgradeEquipmentManager.instance.RemoveStrengthStone(material);
                UpgradeEquipmentManager.instance.UpdateSuccessRate();
            }
            else if (material.itemID_.Contains("godCharm"))
            {
                UpgradeEquipmentManager.instance.RemoveGodCharm(material);
            }
        }

        if (transform.root.GetChild(6).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(6).GetChild(0).GetChild(2).gameObject.activeInHierarchy)
        {
            CombineItemManager.instance.RemoveItem(material);
            CombineItemManager.instance.SetPreviewFinishedProduct();
        }
        InventoryManager.instance.UpdateUIInventory();
        ResetDisplayUpgradeSlot();
    }
    #endregion

    public bool AddItemToCombineSlot(ItemSO _item, ItemSO.ItemType _itemType)
    {
        if (!slotInUse)
        {
            if(_itemType == ItemSO.ItemType.Equipment)
            {
                equipment = (EquipmentSO)_item;
            }
            else
            {
                material = (MaterialSO)_item;
            }
            SetDisplayUpgradeSlot(_item);        
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisplayItemDescription()
    {
        if(equipment != null)
        {
            InventoryDesciptionController.instance.SetDescription(equipment);
        }
        else
        {
            InventoryDesciptionController.instance.SetDescription(material);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
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

    public void OnLeftClick()
    {
        if (slotInUse)
        {
            DisplayItemDescription();
        }
    }

    public void OnRightClick()
    {
        if (slotInUse)
        {
            if (equipment != null)
            {
                RemoveGearFromUpgradeSlot();
            }
            else
            {
                RemoveMaterialFromUpgradeSlot();
            }
        }
    }
}
