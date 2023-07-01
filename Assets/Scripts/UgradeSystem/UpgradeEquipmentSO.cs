using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeEquipment", menuName = "ScriptableObjects/UpgradeSystem/Upgrade")]
public class UpgradeEquipmentSO : UpgradeSystemSO
{
    [SerializeField] private EquipmentUpgrade equipment;
    [SerializeField] private StrengthStoneUpgrade materialsList;
    [SerializeField] private GodCharmUpgrade godCharm;
    [SerializeField] private UpgradeConditionManager upgradeCondition;
    [SerializeField] private float successRate;

    public EquipmentUpgrade equipmentSO_ => equipment;
    public StrengthStoneUpgrade materialsList_ => materialsList;
    public GodCharmUpgrade godCharm_ => godCharm;

    #region Equipment
    //Add equipment to the list of data.
    public void AddEquipmentUpgrade(ItemSO _itemSO, int _quantity)
    {
        if (_itemSO != null)
        {
            equipment = new EquipmentUpgrade(_itemSO, 1);
        }
    }

    //Remove equipment from list data.
    public void RemoveEquipmentUpgrade()
    {
        equipment.ResetEquipmentUpgrade();
    }
    #endregion

    public void ResetSuccessRate()
    {
        successRate = 0;
    }

    #region Strength Stone List
    //Add strength stone to the list of data
    public void AddStrengthStoneUpgrade(ItemSO _itemSO, int _quantity)
    {
        MaterialSO materialSO_ = _itemSO as MaterialSO;
        if (materialSO_ != null && materialSO_.itemUses_ == ItemUses.Upgrade)
        {
            materialsList.strengthStoneList_.Add(materialSO_);
        }
    }

    //Remove equipment from list data.
    public void RemoveStrengthStoneUpgrade(ItemSO _itemSO, int _quantity)
    {
        MaterialSO materialSO_ = _itemSO as MaterialSO;
        if (materialSO_ != null && materialSO_.itemUses_ == ItemUses.Upgrade)
        {
            materialsList.strengthStoneList_.Remove(materialSO_);
        }
    }
    #endregion

    #region God Charm
    //Add strength stone to the list of data
    public void AddGodCharmUpgrade(ItemSO _itemSO, int _quantity)
    {
        MaterialSO materialSO_ = _itemSO as MaterialSO;
        if (materialSO_ != null && materialSO_.itemUses_ == ItemUses.Upgrade)
        {
            godCharm = new GodCharmUpgrade(_itemSO, 1);
        }
    }

    public ItemSO GetGodCharmUpgrade()
    {
        return godCharm.itemSO_;
    }

    //Remove equipment from list data.
    public void RemoveGodCharmUpgrade()
    {
        godCharm.ResetGodCharmUpgrade();
    }
    #endregion


    //Check upgrade item, required: have equipment (equipment's strength < 15 and a strength stone).
    public bool CanUpgrade()
    {
        if (equipment.itemSO_ != null && materialsList.strengthStoneList_.Count > 0)
        {
            if (equipment.itemSO_.itemStrength_ >= 0 && equipment.itemSO_.itemStrength_ < 15)
            {
                return true;
            }
        }
        return false;
    }

    //Get success rate when upgrade
    public float UpdateSuccessRate()
    {
        if (CanUpgrade())
        {
            ResetSuccessRate();
            for (int i = 0; i < materialsList.strengthStoneList_.Count; i++)
            {
                successRate += upgradeCondition.UpgradeCondition(equipment.itemSO_.itemStrength_, materialsList.strengthStoneList_[i]);
            }
            return successRate;
        }
        else
            return 0f;
    }

    //Get result upgrade when click upgrade button
    public bool GetResultUpgrade()
    {
        float result = Random.Range(1f, 100f);
        Debug.Log(result);
        if (result <= UpdateSuccessRate())
        {
            return true;    //upgrade equipment successful
        }
        else
            return false;   //upgrade equipment failed
    }

    //If upgrade equipment successful
    public void UpgradeSuccessful(bool _result)
    {
        if (_result)
        {
            equipment.itemSO_.SetItemStrength(equipment.itemSO_.itemStrength_ + 1);
            equipment.itemSO_.SetItemStrengthImg(upgradeCondition.GetStrengthImg(equipment.itemSO_.itemStrength_));
            SetValueBonusStrengthForEquipment();
        }
    }

    //If upgrade equipment failed
    public void UpgradeFailed(bool _result)
    {
        if (_result == false && equipment.itemSO_.itemStrength_ > 6)
        {
            if(godCharm.itemSO_ == null && ReduceEquipmentStrength() == true)
            {
                Debug.Log("Reduce Strength: " + equipment.itemSO_.itemStrength_.ToString() + "=> " + (equipment.itemSO_.itemStrength_ - 1).ToString());
                equipment.itemSO_.SetItemStrength(equipment.itemSO_.itemStrength_ - 1);
                equipment.itemSO_.SetItemStrengthImg(upgradeCondition.GetStrengthImg(equipment.itemSO_.itemStrength_));
                SetValueBonusStrengthForEquipment();
            }
        }
    }

    //If upgrading equipment fails, equipment can be reduced in strength.
    public bool ReduceEquipmentStrength()
    {
        int result = Random.Range(1, 11);
        Debug.Log(result);
        if (result <= 5)
        {
            return true;    //reduce strength level
        }
        else
            return false;   //not reduce strength level
    }

    //Set valuse bonus of equipment when upgrade success or reduce strength
    public void SetValueBonusStrengthForEquipment()
    {
        for(int i = 0; i < equipment.itemSO_.itemStats_.Length; i++)
        {
            equipment.itemSO_.itemStats_[i].SetValueBonusStrength(upgradeCondition.GetValueBonusStrength(equipment.itemSO_.itemStrength_));
        }
    }
}

[System.Serializable]
public class EquipmentUpgrade
{
    [SerializeField] private EquipmentSO itemSO;
    [SerializeField] private int quantity;

    public EquipmentSO itemSO_ => itemSO;
    public int quantity_ => quantity;

    public EquipmentUpgrade(ItemSO _itemSO, int _quantity)
    {
        EquipmentSO _equipment = _itemSO as EquipmentSO;
        itemSO = _equipment;
        quantity = _quantity;
    }

    public void ResetEquipmentUpgrade()
    {
        itemSO = null;
        quantity = 0;
    }
}

[System.Serializable]
public class StrengthStoneUpgrade
{
    [SerializeField] private List<MaterialSO> strengthStoneList = new List<MaterialSO>();
    public List<MaterialSO> strengthStoneList_ => strengthStoneList;

    public void ClearStrengthStoneList()
    {
        strengthStoneList.Clear();
    }
}

[System.Serializable]
public class GodCharmUpgrade
{
    [SerializeField] private MaterialSO itemSO;
    [SerializeField] private int quantity;

    public MaterialSO itemSO_ => itemSO;
    public int quantity_ => quantity;

    public GodCharmUpgrade(ItemSO _itemSO, int _quantity)
    {
        MaterialSO _material = _itemSO as MaterialSO;
        itemSO = _material;
        quantity = _quantity;
    }

    public void ResetGodCharmUpgrade()
    {
        itemSO = null;
        quantity = 0;
    }
}
