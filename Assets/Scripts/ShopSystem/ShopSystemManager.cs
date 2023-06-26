using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemSO;

public class ShopSystemManager : MonoBehaviour
{
    public static ShopSystemManager instance { get; private set; }

    [SerializeField] private ShopDatabaseSO shopDatabase;
    [SerializeField] private GameObject shopItemSlot;
    [SerializeField] private List<Transform> shopItemSlotParent;
    [SerializeField] private List<Transform> subButton;
    [SerializeField] private ScrollRect shopItem;
    [SerializeField] private Sprite subBtnUp;
    [SerializeField] private Sprite subBtnDown;
    [SerializeField] private List<Transform> slotItem;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        LoadListItem();
        int indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnWeapon");
        int indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentWeapon");
        if (indexListContent_ > -1 && indexSubButton_ > -1)
        {
            SetActiveListContent(indexListContent_, indexSubButton_);
        }
    }

    //hiển thị danh sách vật phẩm theo từng loại (tùy button truyền vào khi nhấn)
    public void ShowListItem(Button _subBtn)
    {
        int indexSubButton_ = -1;
        int indexListContent_ = -1;
        Debug.Log(_subBtn.name + " " + _subBtn.gameObject);
        switch (_subBtn.name)
        {
            case "SubBtnWeapon":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnWeapon");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentWeapon");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnCloth":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnCloth");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentCloth");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnHead":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnHead");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentHead");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnHair":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnHair");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentHair");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnGlass":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnGlass");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentGlass");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnFace":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnFace");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentFace");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnWing":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnWing");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentWing");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnRingAndArmlet":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnRingAndArmlet");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentRingAndArmlet");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
            case "SubBtnMaterial":
                indexSubButton_ = subButton.FindIndex(x => x.name == "SubBtnMaterial");
                indexListContent_ = shopItemSlotParent.FindIndex(x => x.name == "ListContentMaterial");
                if(indexListContent_ > -1 && indexSubButton_ > -1)
                {
                    SetActiveListContent(indexListContent_, indexSubButton_);
                }
                break;
        }
    }

    //tải danh sách các vật theo từng loại khác nhau
    public void LoadListItem()
    {
        for(int i = 0; i < shopItemSlotParent.Count; i++)
        {
            switch (shopItemSlotParent[i].name)
            {
                case "ListContentWeapon":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Weapon);
                    break;
                case "ListContentCloth":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Cloth);
                    break;
                case "ListContentHead":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Head);
                    break;
                case "ListContentHair":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Hair);
                    break;
                case "ListContentGlass":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Glass);
                    break;
                case "ListContentFace":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Face);
                    break;
                case "ListContentWing":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Wing);
                    break;
                case "ListContentRingAndArmlet":
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Ring);
                    InstantiateEquipmentItem(shopItemSlotParent[i], ItemType.Equipment, ItemSlots.Armlet);
                    break;
                case "ListContentMaterial":
                    InstantiateMaterialItem(shopItemSlotParent[i], ItemType.Material);
                    break;
            }
        }        
    }

    //khởi tạo vật phẩm trang bị (vũ khí, áo, nón,...) để hiển thị item trong shop
    public void InstantiateEquipmentItem(Transform _shopItemSlotParent, ItemType _itemType, ItemSlots _itemSlot)
    {
        for (int j = 0; j < shopDatabase.shopItem_.Count; j++)
        {
            if (shopDatabase.shopItem_[j].item_.itemType_ == _itemType)
            {
                EquipmentSO equipment_ = (EquipmentSO)shopDatabase.shopItem_[j].item_;
                if (equipment_ != null && equipment_.itemSlots_ == _itemSlot)
                {
                    GameObject obj = Instantiate(shopItemSlot, _shopItemSlotParent);
                    if (obj != null && equipment_.itemSlots_ == ItemSlots.Weapon || equipment_.itemSlots_ == ItemSlots.Ring || equipment_.itemSlots_ == ItemSlots.Armlet)
                    {
                        obj.transform.GetChild(5).gameObject.SetActive(false);
                    }
                    obj.GetComponent<ShopItemSlotController>().AddItemShop(shopDatabase.shopItem_[j].item_, 1, shopDatabase.shopItem_[j].price_);
                    obj.GetComponent<ShopItemSlotController>().LoadComponents();
                }
            }
        }
    }

    //khởi tạo vật phẩm đạo cụ (đá cường hóa, bùa ma thuật,...) để hiển thị trong shop
    public void InstantiateMaterialItem(Transform _shopItemSlotParent, ItemType _itemType)
    {
        for (int j = 0; j < shopDatabase.shopItem_.Count; j++)
        {
            if (shopDatabase.shopItem_[j].item_.itemType_ == _itemType)
            {
                MaterialSO material_ = (MaterialSO)shopDatabase.shopItem_[j].item_;
                if (material_ != null)
                {
                    GameObject obj = Instantiate(shopItemSlot, _shopItemSlotParent);
                    obj.transform.GetChild(5).gameObject.SetActive(false);
                    obj.GetComponent<ShopItemSlotController>().AddItemShop(shopDatabase.shopItem_[j].item_, 1, shopDatabase.shopItem_[j].price_);
                    obj.GetComponent<ShopItemSlotController>().LoadComponents();
                }
            }
        }
    }

    //hiển thị 1 danh sách vật phẩm theo mục - ẩn các danh sách vật phẩm khác
    public void SetActiveListContent(int _indexListContent, int _indexSubButton)
    {
        Debug.Log("index: " + _indexListContent);
        for (int i = 0; i < shopItemSlotParent.Count; i++)
        {
            if (i == _indexListContent)
            {
                shopItemSlotParent[i].gameObject.SetActive(true);
                shopItem.content = shopItemSlotParent[i].GetComponent<RectTransform>();
                Debug.Log("SetActive true:" + shopItemSlotParent[i].gameObject);
            }
            else
            {
                shopItemSlotParent[i].gameObject.SetActive(false);
                Debug.Log("SetActive false:" + shopItemSlotParent[i].gameObject);
            }
        }
        for (int i = 0; i < subButton.Count; i++)
        {
            if (i == _indexSubButton)
            {
                subButton[i].GetComponent<Image>().sprite = subBtnUp;
            }
            else
            {
                subButton[i].GetComponent<Image>().sprite = subBtnDown;
            }
        }
    }

    //xem trước vật phẩm - phòng thử đồ
    public void TryOnEquiment(EquipmentSO _item)
    {
        switch (_item.itemSlots_)
        {
            case ItemSlots.Head:
                PreviewIconAndShow(_item, "HeadSlot");
                break;
            case ItemSlots.Hair:
                PreviewIconAndShow(_item, "HairSlot");
                break;
            case ItemSlots.Glass:
                PreviewIconAndShow(_item, "GlassSlot");
                break;
            case ItemSlots.Face:
                PreviewIconAndShow(_item, "FaceSlot");
                break;
            case ItemSlots.Cloth:
                PreviewIconAndShow(_item, "ClothSlot");
                break;
            case ItemSlots.Wing:
                PreviewIconAndShow(_item, "WingSlot");
                break;
        }
    }

    //xem trước vật phẩm - phòng thử đồ
    public void PreviewIconAndShow(EquipmentSO _item, string slotName)
    {
        int indexSlot_ = slotItem.FindIndex(x => x.name == slotName);
        if (indexSlot_ > -1)
        {
            slotItem[indexSlot_].GetComponent<GearSlotPreview>().PreviewIconAndShow(_item);
        }
    }
}
