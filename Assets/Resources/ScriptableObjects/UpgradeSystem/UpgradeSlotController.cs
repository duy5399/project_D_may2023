using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemSO;

public class UpgradeSlotController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image border;
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
        border = transform.GetChild(0).GetComponent<Image>();
        border.enabled = false;
        icon = transform.GetChild(1).GetComponent<Image>();
        icon.enabled = false;
        quantity = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void SetIcon(EquipmentSO _equipment)
    {
        icon.sprite = _equipment.itemIcon_;
    }

    #region Display img item and quantity

    public void SetDisplayUpgradeSlot(ItemSO _item)
    {
        switch (_item.itemTier_)
        {
            case RarityTier.common:
                border.color = new Color32(209, 213, 216, 255);
                break;
            case RarityTier.uncommmon:
                border.color = new Color32(65, 168, 95, 255);
                break;
            case RarityTier.rare:
                border.color = new Color32(44, 130, 201, 255);
                break;
            case RarityTier.epic:
                border.color = new Color32(147, 101, 184, 255);
                break;
            case RarityTier.legendary:
                border.color = new Color32(250, 197, 28, 255);
                break;
            case RarityTier.mythic:
                border.color = new Color32(226, 80, 65, 255);
                break;
            default:
                //Debug.Log("Not found rarity tier of item: " + equipment.itemName_);
                break;
        }
        border.enabled = true;
        //update img to equipped slot
        icon.sprite = _item.itemIcon_;
        icon.enabled = true;
        //push new gear to equipment list
        slotInUse = true;
    }

    public void ResetDisplayUpgradeSlot()
    {
        border.enabled = false;
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
        InventoryManager.instance.AddItem(equipment, 1);
        if (transform.root.GetChild(7).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(7).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
        {
            UpgradeEquipmentManager.instance.RemoveEquipment(equipment);
            UpgradeEquipmentManager.instance.UpdateSuccessRate();
        }

        if (transform.root.GetChild(7).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(7).GetChild(0).GetChild(2).gameObject.activeInHierarchy)
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
        InventoryManager.instance.AddItem(material, 1);
        if (transform.root.GetChild(7).GetChild(0).GetChild(2).gameObject.activeInHierarchy && !transform.root.GetChild(7).GetChild(0).GetChild(3).gameObject.activeInHierarchy)
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

        if (transform.root.GetChild(7).GetChild(0).GetChild(3).gameObject.activeInHierarchy && !transform.root.GetChild(7).GetChild(0).GetChild(2).gameObject.activeInHierarchy)
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
