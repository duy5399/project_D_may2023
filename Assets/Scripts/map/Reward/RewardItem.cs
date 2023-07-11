using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemSO;

public class RewardItem : InventorySlotController
{
    protected override void Awake()
    {
        base.Awake();
    }

    //thêm thông tin vật phẩm cho object RewardItem này
    public void AddItemInfoToSlot(ItemSO _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
        itemIcon.sprite = item.itemIcon_;
        quantityTxt.text = quantity.ToString();

        switch (item.itemTier_)
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
                //Debug.Log("Not found rarity tier of item: " + _item.itemName_);
                break;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            base.OnLeftClick();
        }
    }

}
