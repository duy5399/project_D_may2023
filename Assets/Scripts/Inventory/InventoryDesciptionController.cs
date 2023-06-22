using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemSO;

public class InventoryDesciptionController : MonoBehaviour
{
    #region Singleton
    public static InventoryDesciptionController instance { get; private set; }

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
        LoadDescription();
        transform.gameObject.SetActive(false);
    }
    #endregion

    [SerializeField] private Transform displayContent;
    [SerializeField] private Image itemStrengthImg;
    [SerializeField] private Image itemIconImg;
    [SerializeField] private Text itemNameTxt;
    [SerializeField] private Text itemTierTxt;
    [SerializeField] private Text itemTypeTxt;
    [SerializeField] private Text itemStatsTxt;
    [SerializeField] private Text itemDescriptionTxt;

    [SerializeField] private Button upgradeBtn;

    public void LoadDescription()
    {
        displayContent = transform.GetChild(0).GetChild(0).transform;

        itemStrengthImg = displayContent.GetChild(0).GetComponent<Image>();
        itemIconImg = displayContent.GetChild(1).GetComponent<Image>();
        itemNameTxt = displayContent.GetChild(2).GetComponent<Text>();
        itemTypeTxt = displayContent.GetChild(3).GetChild(0).GetComponent<Text>();
        itemTierTxt = displayContent.GetChild(4).GetChild(0).GetComponent<Text>();
        itemStatsTxt = displayContent.GetChild(5).GetComponent<Text>();
        itemDescriptionTxt = displayContent.GetChild(6).GetComponent<Text>();
        upgradeBtn = displayContent.GetChild(6).GetComponent<Button>();
    }

    public void HiddenDescription()
    {
        ResetDescription();
        transform.gameObject.SetActive(false);
    }

    public void ResetDescription()
    {
        //itemUpgrade.sprite = null;
        itemIconImg.sprite = null;
        itemNameTxt.text = null;
        itemTierTxt.text = null;
        itemTypeTxt.text = null;
        itemDescriptionTxt.text = null;
    }

    public void SetDescription(ItemSO _item)
    {
        if (_item.itemType_ == ItemType.Equipment)
        {
            EquipmentSO equipmentSO = (EquipmentSO)_item;
            if (equipmentSO.itemStrength_ < 1)
            {
                itemStrengthImg.enabled = false;
            }
            else
            {
                itemStrengthImg.sprite = equipmentSO.itemStrengthImg_;
                itemStrengthImg.enabled = true;
            }

            //upgradeBtn.gameObject.SetActive(false);
            itemIconImg.sprite = equipmentSO.itemIcon_;
            itemNameTxt.text = equipmentSO.itemStrength_ > 0 ? equipmentSO.itemName_ + " +" + equipmentSO.itemStrength_ : equipmentSO.itemName_;
            itemTypeTxt.text = "Trang bị";
            itemStatsTxt.text = equipmentSO.GetStats();
            itemDescriptionTxt.text = equipmentSO.itemDescription_ + "\n";
            Debug.Log("Show Description of equipment");
        }
        else
        {
            MaterialSO material = (MaterialSO) _item;
            itemStrengthImg.enabled = false;

            //upgradeBtn.gameObject.SetActive(false);
            itemIconImg.sprite = material.itemIcon_;
            itemNameTxt.text = material.itemName_;
            itemTypeTxt.text = "Tài nguyên";           
            itemStatsTxt.text = "";
            itemDescriptionTxt.text = material.itemDescription_;
            Debug.Log("Show Description of material");
        }
        switch (_item.itemTier_)
        {
            case RarityTier.common:
                itemTierTxt.text = "Thường";
                itemNameTxt.color = new Color32(209, 213, 216, 255);
                itemTierTxt.color = new Color32(209, 213, 216, 255);
                itemStatsTxt.color = new Color32(209, 213, 216, 255);
                break;
            case RarityTier.uncommmon:
                itemTierTxt.text = "Cao cấp";
                itemNameTxt.color = new Color32(65, 168, 95, 255);
                itemTierTxt.color = new Color32(65, 168, 95, 255);
                itemStatsTxt.color = new Color32(65, 168, 95, 255);
                break;
            case RarityTier.rare:
                itemTierTxt.text = "Hiếm";
                itemNameTxt.color = new Color32(44, 130, 201, 255);
                itemTierTxt.color = new Color32(44, 130, 201, 255);
                itemStatsTxt.color = new Color32(44, 130, 201, 255);
                break;
            case RarityTier.epic:
                itemTierTxt.text = "Sử thi";
                itemNameTxt.color = new Color32(147, 101, 184, 255);
                itemTierTxt.color = new Color32(147, 101, 184, 255);
                itemStatsTxt.color = new Color32(147, 101, 184, 255);
                break;
            case RarityTier.legendary:
                itemTierTxt.text = "Huyền thoại";
                itemNameTxt.color = new Color32(250, 197, 28, 255);
                itemTierTxt.color = new Color32(250, 197, 28, 255);
                itemStatsTxt.color = new Color32(250, 197, 28, 255);
                break;
            case RarityTier.mythic:
                itemTierTxt.text = "Thần thoại";
                itemNameTxt.color = new Color32(226, 80, 65, 255);
                itemTierTxt.color = new Color32(226, 80, 65, 255);
                itemStatsTxt.color = new Color32(226, 80, 65, 255);
                break;
            default:
                Debug.Log("Not found rarity tier of item: " + _item.itemName_);
                break;
        }
        transform.gameObject.SetActive(true);
    }
}
