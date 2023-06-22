using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CombineCondition", menuName = "ScriptableObjects/UpgradeSystem/CombineCondition")]
public class CombineConditionManager : ScriptableObject
{
    [SerializeField] private List<CombineMaterialsDatabase> combineMaterialsDatabases = new List<CombineMaterialsDatabase>(10);
    public List<CombineMaterialsDatabase> combineMaterialsDatabases_ => combineMaterialsDatabases;

    public ItemSO GetMaterialNextLevel(ItemSO _materialCurrent)
    {
        for(int i = 0; i < combineMaterialsDatabases.Count; i++)
        {
            if (combineMaterialsDatabases[i].materialCurrent_.itemName_ == _materialCurrent.itemName_)
            {
                return combineMaterialsDatabases[i].GetMaterialNextLevel();
            }
        }
        return null;
    }
}

[System.Serializable]
public class CombineMaterialsDatabase
{
    [SerializeField] private ItemSO materialCurrent;
    [SerializeField] private ItemSO materialNextLevel;

    public ItemSO materialCurrent_ => materialCurrent;
    public ItemSO materialNextLevel_ => materialNextLevel;

    public CombineMaterialsDatabase(ItemSO _materialCurrent, ItemSO _materialNextLevel)
    {
        this.materialCurrent = _materialCurrent;
        this.materialNextLevel = _materialNextLevel;
    }

    public ItemSO GetMaterialCurrent()
    {
        return this.materialCurrent;
    }

    public ItemSO GetMaterialNextLevel()
    {
        return this.materialNextLevel;
    }
}
