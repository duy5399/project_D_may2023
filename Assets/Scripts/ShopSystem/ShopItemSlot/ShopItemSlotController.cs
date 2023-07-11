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
    [SerializeField] private Image itemCurrencyImg;
    [SerializeField] private ItemSO currency;
    [SerializeField] private int price;

    protected override void Awake()
    {
        LoadComponents();
    }

    public override void LoadComponents()
    {
        base.LoadComponents();
        itemNameTxt = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        itemPriceTxt = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        itemCurrencyImg = transform.GetChild(5).GetComponent<Image>();
    }

    public void AddItemShop(ItemSO _item, ItemSO _currency, int _price, int _quantity)
    {
        item = _item;
        currency = _currency;
        quantity = _quantity;
        price = _price;
        itemIcon.sprite = _item.itemIcon_;
        quantityTxt.text = _quantity.ToString();
        itemNameTxt.text = _item.itemName_;
        itemPriceTxt.text = string.Format("{0:0,0}", _price);
        itemCurrencyImg.sprite = _currency.itemIcon_;
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
            if(InventoryManager.instance.GetMaterial(currency.itemID_) != null && InventoryManager.instance.GetMaterial(currency.itemID_).quantity_ >= price)
            {
                InventoryManager.instance.RemoveItem(currency, price);

                // tạo bản sao ScriptableObjects để truyền đi - tránh sửa đổi file ScriptableObjects gốc
                if (item.itemType_ == ItemSO.ItemType.Equipment)
                {
                    EquipmentSO equipment = ScriptableObject.Instantiate((EquipmentSO)item);
                    equipment.SetTier(0);
                    equipment.RandomStats(equipment.itemTier_);
                    equipment.SetItemID(equipment.itemID_ + "_" + equipment.itemTier_ + "_" + equipment.GetStatsInfo() + "_" + equipment.GetRandomItemID());
                    InventoryManager.instance.AddItem(equipment, 1);
                }
                else if (item.itemType_ == ItemSO.ItemType.Material)
                {
                    MaterialSO material = ScriptableObject.Instantiate((MaterialSO)item);
                    InventoryManager.instance.AddItem(material, 1);
                }
                AudioManager.instance.BuyitemSuccessSFX();
                QuestionDialogUI.instance.DisplayPurchaseSuccesful("Giao dịch thành công, vật phẩm sẽ được chuyển vào túi đồ!", () => { }, () => { });
                ShopSystemManager.instance.LoadCurrency();
            }
            else
            {
                QuestionDialogUI.instance.DisplayPurchaseFailed("Giao dịch thất bại, không đủ vật phẩm yêu cầu để mua hàng!", () => { }, () => { });
            }
        }
    }

    //hiển thị popup thông báo xác nhận giao dịch
    public void CheckOut()
    {
        AudioManager.instance.ClickSuccessSFX();
        QuestionDialogUI.instance.DisplayConfirmPurchase("Bạn có chắc muốn mua vật phẩm này?", item.itemIcon_, item.itemName_, quantity, price, currency.itemIcon_, () => { BuyItem(); },() => { } );
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
}
