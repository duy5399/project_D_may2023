using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemSO;

[CreateAssetMenu(fileName = "New CombineEquipmentAndMarterial", menuName = "ScriptableObjects/UpgradeSystem/Combine")]
public class CombineItemSO : UpgradeSystemSO
{
    [SerializeField] private List<ItemSO> listItems = new List<ItemSO>();
    [SerializeField] private ItemSO finishedProduct;
    [SerializeField] private CombineConditionManager combineCondition;
    [SerializeField] private float successRate;

    public List<ItemSO> listItems_ => listItems;
    public ItemSO finishedProduct_ => finishedProduct;
    public CombineConditionManager combineCondition_ => combineCondition;
    public float successRate_ => successRate;

    public void RemoveFinishedProduct()
    {
        finishedProduct = null;
    }

    #region add, remove, clear equipment or material
    //Add equipment or material to the list of data
    public void AddItemCombine(ItemSO _itemSO, int _quantity)
    {
        if (_itemSO != null && _itemSO.itemTier_ != ItemSO.RarityTier.mythic)
        {
            listItems.Add(_itemSO);
        }
    }

    //Remove equipment or material from list data.
    public void RemoveItemCombine(ItemSO _itemSO, int _quantity)
    {
        if (_itemSO != null && _itemSO.itemTier_ != ItemSO.RarityTier.mythic)
        {
            if(_itemSO.itemType_ == ItemType.Equipment)
            {
                EquipmentSO equipmentSO_ = (EquipmentSO)_itemSO;
                listItems.Remove(equipmentSO_);
            }
            else
            {
                MaterialSO materialSO_ = (MaterialSO)_itemSO;
                listItems.Remove(materialSO_);
            }
        }
    }

    //Clear item in combine list
    public void ClearCombineList()
    {
        listItems.Clear();
    }
    #endregion

    //Check combine item, required: must have 4 equipment or material
    public int CanCombine()
    {
        if(listItems.Count == 4 && listItems[0].itemTier_ != ItemSO.RarityTier.mythic)
        {
            if (listItems[0].itemType_ == ItemSO.ItemType.Equipment)
            {
                EquipmentSO equip_ = (EquipmentSO)listItems[0];
                if(equip_ != null && equip_.itemTier_ != ItemSO.RarityTier.mythic)
                {
                    for (int i = 1; i < listItems.Count; i++)
                    {
                        string[] itemID_0_ = equip_.itemID_.Split(new char[] { '_' });
                        string[] itemID_i_ = equip_.itemID_.Split(new char[] { '_' });
                        //Debug.Log(itemID_0_[0] + itemID_0_[1] + " vs " + itemID_i_[0] + itemID_i_[1] + " ------------------- " + listItems[0].itemTier_ + " vs " + listItems[i].itemTier_);
                        if ((itemID_0_[0] + itemID_0_[1]) != (itemID_i_[0] + itemID_i_[1]) || listItems[0].itemTier_ != listItems[i].itemTier_)
                        {
                            return 0;
                        }
                    }
                    return 1;
                }
            }

            if(listItems[0].itemType_ == ItemSO.ItemType.Material)
            {
                MaterialSO material_ = (MaterialSO)listItems[0];
                if(material_ != null && material_.canCombine_)
                {
                    for (int i = 1; i < listItems.Count; i++)
                    {
                        if (listItems[0].itemID_ != listItems[i].itemID_ || listItems[0].itemTier_ != listItems[i].itemTier_)
                        {
                            return 0;
                        }
                    }
                    return 2;
                }
            }
        }
        return 0;
    }

    //Get success rate when combine
    public float CombineSuccessRate()
    {      
        int canCombine_ = CanCombine();
        Debug.Log("camcombine: " + canCombine_);
        if (canCombine_ == 1 || canCombine_ == 2)
        {
            return 100f;
        }
        else
        {
            return 0f;
        }
    }

    public ItemSO GetItemFinishedProduct(ItemSO _item)
    {
        if(_item.itemType_ == ItemType.Material)
        {
            return combineCondition.GetMaterialNextLevel(_item);
        }
        else if(_item.itemType_ == ItemType.Equipment)
        {
            EquipmentSO equipment_ = (EquipmentSO)_item;
            RarityTier newRarityTier = (RarityTier)((int)equipment_.itemTier_ + 1);
            return EquipmentSO.Init(equipment_.itemID_, equipment_.itemType_, equipment_.itemIcon_, equipment_.itemName_, newRarityTier, equipment_.itemDescription_, equipment_.maxStackSize_, equipment_.itemSlots_, equipment_.itemShow_, equipment_.itemStats_, equipment_.canUpgrade_, equipment_.itemStrength_, equipment_.itemStrengthImg_);
        }
        return null;
    }

    //Get result combine when click combine button
    public bool GetResultCombine()
    {
        float result = Random.Range(1f, 100f);
        //Debug.Log(result);
        if (result <= CombineSuccessRate())
        {
            return true;    //combine equipment successful
        }
        else
            return false;   //combine equipment failed
    }

    //If combine equipment successful
    public void CombineSuccessful(bool _result, ItemSO _item)
    {
        if(_result)
        {
            if(listItems[0].itemType_ == ItemType.Equipment)
            {
                EquipmentSO equipment_ = (EquipmentSO)_item;
                RarityTier newRarityTier = (RarityTier)((int)equipment_.itemTier_ + 1);
                EquipmentSO newEquipment_ = EquipmentSO.Init(equipment_.itemID_, equipment_.itemType_, equipment_.itemIcon_, equipment_.itemName_, newRarityTier, equipment_.itemDescription_, equipment_.maxStackSize_, equipment_.itemSlots_, equipment_.itemShow_, equipment_.itemStats_, equipment_.canUpgrade_, equipment_.itemStrength_, equipment_.itemStrengthImg_);
                newEquipment_.RandomStats(newEquipment_.itemTier_);
                newEquipment_.SetItemID(newEquipment_.itemID_ + "_" + newEquipment_.itemTier_ + "_" + newEquipment_.GetStatsInfo() + "_" + newEquipment_.GetRandomItemID());
                finishedProduct = newEquipment_;
            }
            else if(listItems[0].itemType_ == ItemType.Material)
            {
                finishedProduct = combineCondition.GetMaterialNextLevel(listItems[0]);
            }
        }
    }

    //Set valuse bonus of equipment (armlet or ring) when combine success
    public void SetValueBonusCombineForEquipment()
    {
        //for (int i = 0; i < equipment.itemSO_.itemStats_.Length; i++)
        //{
        //    equipment.itemSO_.itemStats_[i].SetValueBonusStrength(upgradeCondition.GetValueBonusStrength(equipment.itemSO_.itemStrength_));
        //}
    }

    internal void CombineFailed(bool resultCombine_)
    {
        throw new System.NotImplementedException();
    }
}
