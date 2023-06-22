using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinishedProductCombine : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemSO finishProductCombine;
    [SerializeField] private Image icon;
    [SerializeField] private bool slotInUse;

    public bool slotInUse_ => slotInUse;

    void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        icon.enabled = false;
    }

    public void ShowFinishProductCombine(ItemSO _item)
    {
        finishProductCombine = _item;
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
                InventoryManager.instance.AddItem((EquipmentSO)finishProductCombine);

            }
            else
            {
                InventoryManager.instance.AddItem((MaterialSO)finishProductCombine);
            }
            icon.sprite = null;
            icon.enabled = false;
            finishProductCombine = null;
            slotInUse = false;
            CombineItemManager.instance.SetPreviewFinishedProduct();
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
        InventoryManager.instance.UpdateUIInventory();
    }
}
