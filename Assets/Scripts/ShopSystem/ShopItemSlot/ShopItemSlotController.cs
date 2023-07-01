using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemSlotController : InventorySlotController
{
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI itemPriceTxt;
    [SerializeField] private int price;

    protected override void Awake()
    {
        LoadComponents();
    }

    public override void LoadComponents()
    {
        base.LoadComponents();
        itemNameTxt = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemPriceTxt = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    public void AddItemShop(ItemSO _item, int _quantity, int _price)
    {
        item = _item;
        quantity = _quantity;
        price = _price;
        itemIcon.sprite = item.itemIcon_;
        quantityTxt.text = quantity.ToString();
        itemNameTxt.text = _item.itemName_;
        itemPriceTxt.text = _price.ToString();
    }

    //mặc thử và xem trước vật phẩm
    public void TryOnEquipment()
    {
        EquipmentSO equipment = (EquipmentSO)item;
        ShopSystemManager.instance.TryOnEquiment(equipment);
    }

    //mua vật phẩm - tạo bản sao (trang bị thì set tier = common và random stats) => truyền vật phẩm vào inventory
    public void BuyItem()
    {
        if (item == null)
        {
            return;
        }
        else
        {
            // tạo bản sao ScriptableObjects để truyền đi - tránh sửa đổi file ScriptableObjects gốc
            if (item.itemType_ == ItemSO.ItemType.Equipment)
            {
                EquipmentSO equipment = ScriptableObject.Instantiate((EquipmentSO)item);
                equipment.SetTier(0);
                equipment.RandomStats(equipment.itemTier_);
                equipment.SetItemID(equipment.itemID_ + "_" + equipment.itemTier_ + "_" + equipment.GetStatsInfo() + "_" + equipment.GetRandomItemID());
                InventoryManager.instance.AddItem(equipment);
            }
            else if(item.itemType_ == ItemSO.ItemType.Material)
            {
                MaterialSO material = ScriptableObject.Instantiate((MaterialSO)item);
                InventoryManager.instance.AddItem(material);
            }
        }
    }

    //hiển thị popup thông báo xác nhận giao dịch
    public void CheckOut()
    {
        QuestionDialogUI.instance.DisplayQuestion("Bạn có chắc muốn mua vật phẩm này?", item.itemIcon_, item.itemName_, quantity, price, () => { BuyItem(); },() => { } );
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
}
