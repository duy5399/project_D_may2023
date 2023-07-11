using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemSO;

public class FinishedProductCombine : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemSO finishProductCombine;
    [SerializeField] private Image border;
    [SerializeField] private Image icon;
    [SerializeField] private bool slotInUse;

    public bool slotInUse_ => slotInUse;

    void Awake()
    {
        border = transform.GetChild(0).GetComponent<Image>();
        border.enabled = false;
        icon = transform.GetChild(1).GetComponent<Image>();
        icon.enabled = false;
    }

    public void DisplayFinishProductCombine(ItemSO _item)
    {
        finishProductCombine = _item;
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
        icon.sprite = _item.itemIcon_;
        icon.enabled = true;
        slotInUse = true;
    }

    public void AddFinishesProductToInventory()
    {
        if (finishProductCombine != null)
        {
            if (finishProductCombine.itemType_ == ItemSO.ItemType.Equipment)
            {
                InventoryManager.instance.AddItem((EquipmentSO)finishProductCombine, 1);

            }
            else
            {
                InventoryManager.instance.AddItem((MaterialSO)finishProductCombine, 1);
            }
            border.enabled = false;
            icon.sprite = null;
            icon.enabled = false;
            finishProductCombine = null;
            slotInUse = false;
            CombineItemManager.instance.SetPreviewFinishedProduct();
            InventoryManager.instance.UpdateUIInventory();
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
        if (finishProductCombine.itemType_ == ItemSO.ItemType.Equipment)
        {
            InventoryDesciptionController.instance.SetDescription((EquipmentSO)finishProductCombine);
        }
        else
        {
            InventoryDesciptionController.instance.SetDescription((MaterialSO)finishProductCombine);
        }
    }

    public void OnRightClick()
    {
        AddFinishesProductToInventory();
    }
}
