using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class CombineItemManager : MonoBehaviour
{
    #region Singleton
    public static CombineItemManager instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        SetPreviewFinishedProduct();
    }
    #endregion

    [SerializeField] private CombineItemSO combineItemSO;
    [SerializeField] private Transform upgradeSystem; //Canvas - UpgradeSystem
    [SerializeField] private List<Transform> itemCombineSlot; //UpgradeSystem - Interface - Upgrade - StrengthStone
    [SerializeField] private Transform finishedProductSlot; //UpgradeSystem - Interface - Upgrade - EquipmentUpgrade
    [SerializeField] private Transform previewProduct;
    [SerializeField] private Transform combineBtn;
    [SerializeField] private Transform alertResult;
    [SerializeField] private Transform closeBtn;

    public ItemSO finishedProductPreview;

    public CombineItemSO combineItemSO_ => combineItemSO;

    public void AddItem(ItemSO _item)
    {
        combineItemSO.AddItemCombine(_item, 1);
    }

    public void RemoveItem(ItemSO _item)
    {

        combineItemSO.RemoveItemCombine(_item, 1);
    }

    public bool AddItemToCombineSlot(ItemSO _itemSO)
    {
        if(_itemSO.itemType_ == ItemSO.ItemType.Material)
        {
            MaterialSO material_ = (MaterialSO)_itemSO;
            for (int i = 0; i < itemCombineSlot.Count; i++)
            {
                if (itemCombineSlot[i].GetComponent<UpgradeSlotController>().AddItemToCombineSlot(material_, material_.itemType_) == true)
                {
                    ////Debug.Log("ô này " + itemCombineSlot[i].name + " thêm được vật phẩm dung luyện");
                    //AddItem(material_);
                    return true;
                }
                //Debug.Log("ô này " + itemCombineSlot[i].name + " không thêm được vật phẩm dung luyện");
            }
        }
        else if(_itemSO.itemType_ == ItemSO.ItemType.Equipment)
        {
            EquipmentSO equipment_ = (EquipmentSO)_itemSO;
            for (int i = 0; i < itemCombineSlot.Count; i++)
            {
                if (itemCombineSlot[i].GetComponent<UpgradeSlotController>().AddItemToCombineSlot(equipment_, equipment_.itemType_) == true)
                {
                    ////Debug.Log("ô này " + itemCombineSlot[i].name + " thêm được vật phẩm dung luyện");
                    //AddItem(equipment_);
                    return true;
                }
                //Debug.Log("ô này " + itemCombineSlot[i].name + " không thêm được vật phẩm dung luyện");
            }
        }
        return false;
    }

    //lấy thông tin vật phẩm dung luyện thành công
    public void SetPreviewFinishedProduct()
    {
        if(combineItemSO.listItems_.Count == 0){
            previewProduct.GetChild(0).GetComponent<Image>().sprite = null;
            previewProduct.GetChild(0).GetComponent<Image>().enabled = false;
            previewProduct.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            for (int i = 0; i < combineItemSO.listItems_.Count; i++)
            {
                string[] itemID_0_ = combineItemSO.listItems_[0].itemID_.Split(new char[] { '_' });
                string[] itemID_i_ = combineItemSO.listItems_[i].itemID_.Split(new char[] { '_' });
                Debug.Log(itemID_0_[0] + itemID_0_[1] + " vs " + itemID_i_[0] + itemID_i_[1] + " ------------------- " + combineItemSO.listItems_[0].itemTier_ + " vs " + combineItemSO.listItems_[i].itemTier_);
                if (itemID_0_[0] + itemID_0_[1] != itemID_i_[0] + itemID_i_[1] || combineItemSO.listItems_[0].itemTier_ != combineItemSO.listItems_[i].itemTier_)
                {
                    previewProduct.GetChild(0).GetComponent<Image>().sprite = null;
                    previewProduct.GetChild(0).GetComponent<Image>().enabled = false;
                    previewProduct.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    return;
                }
            }
            finishedProductPreview = combineItemSO.GetItemFinishedProduct(combineItemSO.listItems_[0]);
            previewProduct.GetChild(0).GetComponent<Image>().sprite = finishedProductPreview.itemIcon_;
            previewProduct.GetChild(0).GetComponent<Image>().enabled = true;
            previewProduct.GetChild(1).GetComponent<TextMeshProUGUI>().text = finishedProductPreview.itemName_;
        }
    }

    //cập nhật tỉ lệ thành công khi bỏ vật phẩm cần dung luyện vào ô
    public void SetSuccessRateCombine()
    {
        previewProduct.GetChild(2).GetComponent<TextMeshProUGUI>().text = combineItemSO.CombineSuccessRate() > 100 ? "100%" : combineItemSO.CombineSuccessRate().ToString() + "%";
    }

    //khi nhấn nút Dung luyện
    public void OnClickCombine()
    {
        if (combineItemSO.CombineSuccessRate() != 0f)  //nếu tỉ lệ thành công > 0 thì tiến hành Dung luyện vật phẩm
        {
            bool resultCombine_ = combineItemSO.GetResultCombine();   //lấy kết quả thành công hoặc thạt bại (hiện tại đang set mức 100% => chưa có tỉ lệ thất bại - có thể phát triển thêm sau)
            if (resultCombine_)                                             //Dung luyện vật phẩm
            {
                Debug.Log("combineItemSO.GetResultCombine(): " + resultCombine_);
                combineItemSO.CombineSuccessful(resultCombine_, combineItemSO.listItems_[0]);
                finishedProductSlot.GetComponent<FinishedProductCombine>().ShowFinishProductCombine(combineItemSO.finishedProduct_);
                combineItemSO.RemoveFinishedProduct();
                StartCoroutine(DisplayedResultCombine(resultCombine_));
            }
            else                                                            //Dung luyện vật phẩm thất bại
            {
                Debug.Log("combineItemSO.GetResultCombine(): " + resultCombine_);
                combineItemSO.CombineFailed(resultCombine_);
                StartCoroutine(DisplayedResultCombine(resultCombine_));
            }
        }
    }

    public IEnumerator DisplayedResultCombine(bool _resultUpgrade)
    {
        RemoveStrengthStoneUsed();
        //UpdateSuccessRate();
        closeBtn.GetComponent<Button>().interactable = false;
        combineBtn.GetComponent<Button>().interactable = false;
        alertResult.gameObject.SetActive(true);
        alertResult.GetComponent<Animator>().SetTrigger(_resultUpgrade.ToString());
        yield return new WaitForSeconds(1.5f);
        alertResult.GetComponent<Animator>().ResetTrigger(_resultUpgrade.ToString());
        alertResult.GetComponent<Animator>().SetTrigger("Default");
        yield return new WaitForSeconds(0.5f);
        alertResult.gameObject.SetActive(false);
        combineBtn.GetComponent<Button>().interactable = true;
        closeBtn.GetComponent<Button>().interactable = true;
    }

    public void RemoveStrengthStoneUsed()
    {
        combineItemSO.ClearCombineList();
        for (int i = 0; i < itemCombineSlot.Count; i++)
        {
            itemCombineSlot[i].GetComponent<UpgradeSlotController>().ResetDisplayUpgradeSlot();
        }
    }

    public void OnApplicationQuit()
    {
        combineItemSO.ClearCombineList();
    }

    //trả các vật phẩm còn trong ô Dung luyện về kho đồ nếu có
    public void ResetCombineSystem()
    {
        for (int i = 0; i < itemCombineSlot.Count; i++)
        {
            if (itemCombineSlot[i].GetComponent<UpgradeSlotController>().slotInUse_ == true)
            {
                if (itemCombineSlot[i].GetComponent<UpgradeSlotController>().equipment_ != null)
                {
                    itemCombineSlot[i].GetComponent<UpgradeSlotController>().RemoveGearFromUpgradeSlot();
                }
                else
                {
                    itemCombineSlot[i].GetComponent<UpgradeSlotController>().RemoveMaterialFromUpgradeSlot();
                }
            }
        }
        if(finishedProductSlot.GetComponent<FinishedProductCombine>().slotInUse_ == true)
        {
            finishedProductSlot.GetComponent<FinishedProductCombine>().AddFinishesProductToInventory();
        }
    }
}
