using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class UpgradeEquipmentManager : MonoBehaviour
{
    #region Singleton
    public static UpgradeEquipmentManager instance { get; private set; }
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
        upgradeSystem.gameObject.SetActive(false);
    }
    #endregion

    [SerializeField] private UpgradeEquipmentSO upgradeEquipmentSO;
    [SerializeField] private Transform upgradeSystem; //Canvas - UpgradeSystem
    [SerializeField] private Transform equipmentUpgradeSlot; //UpgradeSystem - Interface - Upgrade - EquipmentUpgrade
    [SerializeField] private List<Transform> strengthStoneSlot; //UpgradeSystem - Interface - Upgrade - StrengthStone
    [SerializeField] private Transform godCharmSlot; //UpgradeSystem - Interface - Upgrade - StrengthStone
    [SerializeField] private TextMeshProUGUI successRateTxt;
    [SerializeField] private Transform upgradeStrengthBtn;
    [SerializeField] private Transform alertResult;
    [SerializeField] private Transform closeBtn;

    public UpgradeEquipmentSO upgradeEquipmentSO_ => upgradeEquipmentSO;

    #region Equipment
    //add item to upgradeSO List
    public void AddEquipment(ItemSO _item)
    {
        upgradeEquipmentSO.AddEquipmentUpgrade(_item, 1);
    }

    //remove item to upgradeSO List
    public void RemoveEquipment(ItemSO _item)
    {
        upgradeEquipmentSO.RemoveEquipmentUpgrade();
    }
    #endregion

    #region Strength Stone
    //add Strength Stone to Material List
    public void AddStrengthStone(ItemSO _item)
    {
        upgradeEquipmentSO.AddStrengthStoneUpgrade(_item, 1);
    }

    //remove Strength Stone to Material List
    public void RemoveStrengthStone(ItemSO _item)
    {
        upgradeEquipmentSO.RemoveStrengthStoneUpgrade(_item, 1);
    }
    #endregion

    #region GodCharm
    //add god charm to upgradeSO List
    public void AddGodCharm(ItemSO _item)
    {
        upgradeEquipmentSO.AddGodCharmUpgrade(_item, 1);
    }

    //remove item to upgradeSO List
    public void RemoveGodCharm(ItemSO _item)
    {
        upgradeEquipmentSO.RemoveGodCharmUpgrade();
    }
    #endregion

    public void AddGearToUpgradeSlot(ItemSO _equipment)
    {
        if (_equipment.itemType_ == ItemSO.ItemType.Equipment)
        {
            equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().AddGearToUpgradeSlot(_equipment);
        }
    }

    public bool AddStrengthStoneToUpgradeSlot(ItemSO _marterial)
    {
        if (_marterial.itemType_ == ItemSO.ItemType.Material)
        {
            for (int i = 0; i < strengthStoneSlot.Count; i++)
            {
                if (strengthStoneSlot[i].GetComponent<UpgradeSlotController>().AddMaterialToUpgradeSlot(_marterial) == true)
                {
                    //Debug.Log("ô này " + strengthStoneSlot[i].name + " thêm được đá");
                    //AddStrengthStone(_marterial);                   
                    return true;
                }
                //Debug.Log("ô này " + strengthStoneSlot[i].name + " không thêm được đá");
            }
        }
        return false;
    }

    public bool AddGodCharmToUpgradeSlot(ItemSO _marterial)
    {
        if (_marterial.itemType_ == ItemSO.ItemType.Material && _marterial.itemID_.Contains("godCharm"))
        {
            if (godCharmSlot.GetComponent<UpgradeSlotController>().AddMaterialToUpgradeSlot(_marterial))
            {
                AddGodCharm(_marterial);
                return true;
            }
        }
        return false;
    }

    public void UpdateSuccessRate()
    {
        successRateTxt.text = upgradeEquipmentSO_.UpdateSuccessRate() > 100 ? "100%" : upgradeEquipmentSO_.UpdateSuccessRate().ToString() + "%";
    }

    public void ResetUppgradeSystem()
    {
        if(equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().slotInUse_ == true)
        {
            //Debug.Log("RemoveGearFromUpgradeSlot: " + equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().equipment_);
            equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().RemoveGearFromUpgradeSlot();
        }
        for(int i = 0; i < strengthStoneSlot.Count; i++)
        {
            if (strengthStoneSlot[i].GetComponent<UpgradeSlotController>().slotInUse_ == true)
            {
                //Debug.Log("RemoveGearFromUpgradeSlot: " + equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().equipment_);
                strengthStoneSlot[i].GetComponent<UpgradeSlotController>().RemoveMaterialFromUpgradeSlot();
            }
        }
        if (godCharmSlot.GetComponent<UpgradeSlotController>().slotInUse_ == true)
        {
            //Debug.Log("RemoveGearFromUpgradeSlot: " + equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().equipment_);
            godCharmSlot.GetComponent<UpgradeSlotController>().RemoveMaterialFromUpgradeSlot();
        }
    }

    public void OnClickUpgrade()
    {
        if (upgradeEquipmentSO_.UpdateSuccessRate() != 0f)  //if success rate > 0 => can upgrade
        {
            bool resultUpgarde_ = upgradeEquipmentSO_.GetResultUpgrade();   //get result upgrade
            if (resultUpgarde_)                                             //upgrade successful
            {
                //Debug.Log("upgradeEquipmentSO_.GetResultUpgrade(): " + resultUpgarde_);
                upgradeEquipmentSO_.UpgradeSuccessful(resultUpgarde_);
                equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().SetIcon(upgradeEquipmentSO_.equipmentSO_.itemSO_);
                AudioManager.instance.UpgradeSuccessSFX();
                StartCoroutine(DisplayedResultUpgrade(resultUpgarde_));
            }
            else                                                            //upgrade failed
            {
                //Debug.Log("upgradeEquipmentSO_.GetResultUpgrade(): " + resultUpgarde_);
                upgradeEquipmentSO_.UpgradeFailed(resultUpgarde_);
                equipmentUpgradeSlot.GetComponent<UpgradeSlotController>().SetIcon(upgradeEquipmentSO_.equipmentSO_.itemSO_);
                AudioManager.instance.UpgradeFailureSFX();
                StartCoroutine(DisplayedResultUpgrade(resultUpgarde_));              
            }
        }
    }

    public IEnumerator DisplayedResultUpgrade(bool _resultUpgrade)
    {
        RemoveStrengthStoneUsed();
        UpdateSuccessRate();
        closeBtn.GetComponent<Button>().interactable = false;
        upgradeStrengthBtn.GetComponent<Button>().interactable = false;
        alertResult.gameObject.SetActive(true);
        alertResult.GetComponent<Animator>().SetTrigger(_resultUpgrade.ToString());
        yield return new WaitForSeconds(1.5f);
        alertResult.GetComponent<Animator>().ResetTrigger(_resultUpgrade.ToString());
        alertResult.GetComponent<Animator>().SetTrigger("Default");
        yield return new WaitForSeconds(0.5f);
        alertResult.gameObject.SetActive(false);
        upgradeStrengthBtn.GetComponent<Button>().interactable = true;
        closeBtn.GetComponent<Button>().interactable = true;
    }

    public void RemoveStrengthStoneUsed()
    {
        upgradeEquipmentSO_.materialsList_.ClearStrengthStoneList();
        for (int i = 0; i < strengthStoneSlot.Count; i++)
        {
            strengthStoneSlot[i].GetComponent<UpgradeSlotController>().ResetDisplayUpgradeSlot();
        }
        upgradeEquipmentSO_.godCharm_.ResetGodCharmUpgrade();
        godCharmSlot.GetComponent<UpgradeSlotController>().ResetDisplayUpgradeSlot();
    }
}
