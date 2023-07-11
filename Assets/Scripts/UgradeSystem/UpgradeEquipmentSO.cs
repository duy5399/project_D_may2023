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
    //thêm trang bị để nâng cấp
    public void AddEquipmentUpgrade(ItemSO _itemSO, int _quantity)
    {
        if (_itemSO != null)
        {
            equipment = new EquipmentUpgrade(_itemSO, 1);
        }
    }

    //gỡ trang bị nâng cấp
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
    //thêm đá cường hóa
    public void AddStrengthStoneUpgrade(ItemSO _itemSO, int _quantity)
    {
        MaterialSO materialSO_ = _itemSO as MaterialSO;
        if (materialSO_ != null && materialSO_.itemUses_ == ItemUses.Upgrade)
        {
            materialsList.strengthStoneList_.Add(materialSO_);
        }
    }

    //gỡ đá cường hóa
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


    //kiểm tra các vật phẩm đầu vào,yêu cầu: 1 trang bị có cấp độ cường hóa < 15 và ít nhất 1 đá cường hóa
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

    //tỉ lệ thành công khi nâng cấp
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

    //nhận kết quả khi nhấn nút "Cường hóa" =. nhận giá trị tru ( thành công) hoặc false (thất bại)
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

    //nâng cấp trang bi thành công
    public void UpgradeSuccessful(bool _result)
    {
        if (_result)
        {
            equipment.itemSO_.SetItemStrength(equipment.itemSO_.itemStrength_ + 1);
            equipment.itemSO_.SetItemStrengthImg(upgradeCondition.GetStrengthImg(equipment.itemSO_.itemStrength_));
            if (equipment.itemSO_.itemStrength_ == 6 || equipment.itemSO_.itemStrength_ == 7 || equipment.itemSO_.itemStrength_ == 9 || equipment.itemSO_.itemStrength_ == 10 || equipment.itemSO_.itemStrength_ == 12 || equipment.itemSO_.itemStrength_ == 13 || equipment.itemSO_.itemStrength_ == 15)
            {
                equipment.itemSO_.SetItemIcon(upgradeCondition.GetNewItemIcon(equipment.itemSO_));
            }
            SetValueBonusStrengthForEquipment();
        }
    }

    //nâng cấp trang bi thất bại
    public void UpgradeFailed(bool _result)
    {
        if (_result == false && equipment.itemSO_.itemStrength_ > 6)
        {
            if(godCharm.itemSO_ == null && ReduceEquipmentStrength() == true)
            {
                //Debug.Log("Reduce Strength: " + equipment.itemSO_.itemStrength_.ToString() + "=> " + (equipment.itemSO_.itemStrength_ - 1).ToString());
                equipment.itemSO_.SetItemStrength(equipment.itemSO_.itemStrength_ - 1);
                equipment.itemSO_.SetItemStrengthImg(upgradeCondition.GetStrengthImg(equipment.itemSO_.itemStrength_));
                if (equipment.itemSO_.itemStrength_ == 6 || equipment.itemSO_.itemStrength_ == 7 || equipment.itemSO_.itemStrength_ == 9 || equipment.itemSO_.itemStrength_ == 10 || equipment.itemSO_.itemStrength_ == 12 || equipment.itemSO_.itemStrength_ == 13 || equipment.itemSO_.itemStrength_ == 15)
                {
                    equipment.itemSO_.SetItemIcon(upgradeCondition.GetNewItemIcon(equipment.itemSO_));
                }
                SetValueBonusStrengthForEquipment();
            }
        }
    }

    //nếu nâng cấp thất bại có thể bị giảm cấp trang bị
    public bool ReduceEquipmentStrength()
    {
        int result = Random.Range(1, 11);
        //Debug.Log(result);
        if (result <= 5)
        {
            return true;    //giảm cấp
        }
        else
            return false;   //không thay đổi
    }

    //thêm hoặc bớt valuse bonus khi nâng cấp thành công hoặc thất bại
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
