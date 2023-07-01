using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using static ItemSO;


[CreateAssetMenu(fileName = "New Inventory", menuName = "ScriptableObjects/Inventory - Equipment")]
public class InventorySO : ScriptableObject
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private InventoryEquipment listEquipmentSO;    //danh sách vật phẩm trang bị
    [SerializeField] private InventoryMaterial listMaterialSO;      //ldanh sách vật phẩm đạo cụ

    public ItemDatabaseSO itemDatabase_ => itemDatabase;
    public InventoryEquipment listEquipmentSO_ => listEquipmentSO;
    public InventoryMaterial listMaterialSO_ => listMaterialSO;

    public void AddItem(ItemSO _item, int _quantity)
    {
        if(_item.itemType_ == ItemSO.ItemType.Equipment)
        {
            AddEquipment((EquipmentSO)_item, _quantity);
        }
        else
        {
            AddMaterial((MaterialSO)_item, _quantity);
        }
    }

    public void RemoveItem(ItemSO _item, int _quantity)
    {
        if (_item.itemType_ == ItemSO.ItemType.Equipment)
        {
            RemoveEquipment((EquipmentSO)_item, _quantity);
        }
        else
        {
            RemoveMaterial((MaterialSO)_item, _quantity);
        }
    }

    #region thêm, xóa danh sách trang bị
    //thêm trang bị mới nhặt được vào kho đồ
    public void AddEquipment(EquipmentSO _item, int _quantity)
    {
        if (_item.maxStackSize_ > 1)
        {
            for (int i = 0; i < listEquipmentSO.equipments.Count; i++)
            {
                if (listEquipmentSO.equipments[i].idItem_ == _item.itemID_ && listEquipmentSO.equipments[i].quantity_ < listEquipmentSO.equipments[i].maxStack_)
                {
                    listEquipmentSO.equipments[i].AddQuantiy(_quantity);
                    return;
                }
            }
        }
        listEquipmentSO.equipments.Add(new InventoryEquipmentSlot(_item.itemID_, _item.itemType_, _item.itemIcon_, _item.itemName_, _item.itemTier_, _item.itemDescription_, _item.maxStackSize_, _item.itemSlots_, _item.itemShow_, _item.itemStats_, _item.canUpgrade_, _item.itemStrength_, _item.itemStrengthImg_, 1));
    }

    //gỡ trang bị ra khỏi kho đồ khi cần sử dụng (VD: khi trang bị, cường hóa, dung luyện,...)
    public void RemoveEquipment(EquipmentSO _item, int _quantity)
    {
        for (int i = 0; i < listEquipmentSO.equipments.Count; i++)
        {
            if (listEquipmentSO.equipments[i].idItem_ == _item.itemID_)
            {
                if (listEquipmentSO.equipments[i].quantity_ > 1)
                {
                    listEquipmentSO.equipments[i].RemoveQuantiy(_quantity);
                    return;
                }
                else
                {
                    listEquipmentSO.equipments.RemoveAt(i);
                }
            }
        }
    }
    #endregion
    #region thêm, xóa danh sách đạo cụ
    //thêm đạo cụ mới nhặt được vào kho đồ
    public void AddMaterial(MaterialSO _item, int _quantity)
    {
        if (_item.maxStackSize_ > 1)
        {
            for (int i = 0; i < listMaterialSO.materials.Count; i++)
            {
                if (listMaterialSO.materials[i].idItem_ == _item.itemID_ && listMaterialSO.materials[i].quantity_ < listMaterialSO.materials[i].maxStack_)
                {
                    listMaterialSO.materials[i].AddQuantiy(_quantity);
                    return;
                }
            }
        }
        listMaterialSO.materials.Add(new InventoryMaterialSlot(_item.itemID_, _item.itemType_, _item.itemIcon_, _item.itemName_, _item.itemTier_, _item.itemDescription_, _item.maxStackSize_, _item.itemUses_, _item.canCombine_, 1));
    }

    //gỡ đạo cụ ra khỏi kho đồ khi cần sử dụng (VD: khi trang bị, cường hóa, dung luyện,...)
    public void RemoveMaterial(MaterialSO _item, int _quantity)
    {
        for (int i = 0; i < listMaterialSO.materials.Count; i++)
        {
            if (listMaterialSO.materials[i].idItem_ == _item.itemID_)
            {
                if (listMaterialSO.materials[i].quantity_ > 1)
                {
                    listMaterialSO.materials[i].RemoveQuantiy(_quantity);
                    return;
                }
                else
                {
                    listMaterialSO.materials.RemoveAt(i);
                    return;
                }
            }
        }
    }
    #endregion

    public void SaveEquipment(string savePath)
    {
        string saveData = JsonUtility.ToJson(listEquipmentSO, true); //chuyển đổi dữ liệu về các trang bị thành đình dạng JSON
        Debug.Log(saveData);
        BinaryFormatter binaryFormatter = new BinaryFormatter(); //binary formatter
        FileStream fileStream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create);
        binaryFormatter.Serialize(fileStream, saveData); //convert saveData to binary format and write data
        fileStream.Close();
        Debug.Log("Save equipment succesful: " + string.Concat(Application.persistentDataPath, savePath));

        //IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //formatter.Serialize(stream, listItemSO);
        //stream.Close();
    }

    public void LoadEquipment(string savePath)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fileStream).ToString(), listEquipmentSO);
            fileStream.Close();
            Debug.Log("Load equipment succesful");

            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            //listItemSO = (Inventory)formatter.Deserialize(stream);
            //stream.Close();
        }
        else
        {
            Debug.LogError("Save path not found" + string.Concat(Application.persistentDataPath, savePath));
        }
    }

    public void SaveMaterial(string savePath)
    {
        string saveData = JsonUtility.ToJson(listMaterialSO, true); //chuyển đổi dữ liệu về các đạo cụ thành đình dạng JSON
        Debug.Log(saveData);
        BinaryFormatter binaryFormatter = new BinaryFormatter(); //binary formatter
        FileStream fileStream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create);
        binaryFormatter.Serialize(fileStream, saveData); //convert saveData to binary format and write data
        fileStream.Close();
        Debug.Log("Save material succesful: " + string.Concat(Application.persistentDataPath, savePath));

        //IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //formatter.Serialize(stream, listItemSO);
        //stream.Close();
    }

    public void LoadMaterial(string savePath)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fileStream).ToString(), listMaterialSO);
            fileStream.Close();
            Debug.Log("Load material succesful");

            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            //listItemSO = (Inventory)formatter.Deserialize(stream);
            //stream.Close();
        }
        else
        {
            Debug.LogError("Save path not found" + string.Concat(Application.persistentDataPath, savePath));
        }
    }

    public void Clear()
    {
        //listItemSO = new Inventory();
    }
}

[System.Serializable]
public class InventoryEquipment
{
    public List<InventoryEquipmentSlot> equipments = new List<InventoryEquipmentSlot>();
}

[System.Serializable]
public class InventoryEquipmentSlot
{
    [SerializeField] protected string idItem;
    [SerializeField] protected ItemType type;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string name;
    [SerializeField] protected RarityTier tier;
    [SerializeField] protected string description;
    [SerializeField] protected int maxStack;
    [SerializeField] private ItemSlots slot;
    [SerializeField] private Sprite show;
    [SerializeField] private Stats[] stats;
    [SerializeField] private bool canUpgrade;
    [SerializeField] private int itemStrength;
    [SerializeField] private Sprite itemStrengthImg;

    [SerializeField] protected EquipmentSO equipmentSO;
    [SerializeField] private int quantity;

    public string idItem_ => idItem;
    public ItemType type_ => type;
    public Sprite icon_ => icon;
    public string name_ => name;
    public RarityTier tier_ => tier;
    public string description_ => description;
    public int maxStack_ => maxStack;
    public ItemSlots slot_ => slot;
    public Sprite show_ => show;
    public Stats[] stats_ => stats;
    public bool canUpgrade_ => canUpgrade;
    public int itemStrength_ => itemStrength;
    public Sprite itemStrengthImg_ => itemStrengthImg;

    public EquipmentSO equipmentSO_ => equipmentSO;
    public int quantity_ => quantity;

    public InventoryEquipmentSlot(string idItem, ItemType type, Sprite icon, string name, RarityTier tier, string description, int maxStack, ItemSlots slot, Sprite show, Stats[] stats, bool canUpgrade, int itemStrength, Sprite itemStrengthImg, int quantity)
    {
        this.idItem = idItem;
        this.type = type;
        this.icon = icon;
        this.name = name;
        this.tier = tier;
        this.description = description;
        this.maxStack = maxStack;
        this.slot = slot;
        this.show = show;
        this.stats = stats;
        this.canUpgrade = canUpgrade;
        this.itemStrength = itemStrength;
        this.itemStrengthImg = itemStrengthImg;
        this.quantity = quantity;
        equipmentSO = EquipmentSO.Init(idItem, type, icon, name, tier, description, maxStack, slot, show, stats, canUpgrade, itemStrength, itemStrengthImg);
    }

    public void AddQuantiy(int _quantity)
    {
        quantity += _quantity;
    }

    public void RemoveQuantiy(int _quantity)
    {
        quantity -= _quantity;
    }

    public void SerializableItenInfo()
    {
        this.idItem = equipmentSO.itemID_;
        this.type = equipmentSO.itemType_;
        this.icon = equipmentSO.itemIcon_;
        this.name = equipmentSO.name;
        this.tier = equipmentSO.itemTier_;
        this.description = equipmentSO.itemDescription_;
        this.maxStack = equipmentSO.maxStackSize_;
        this.slot = equipmentSO.itemSlots_;
        this.show = equipmentSO.itemShow_;
        this.stats = equipmentSO.itemStats_;
        this.canUpgrade = equipmentSO.canUpgrade_;
        this.itemStrength = equipmentSO.itemStrength_;
        this.itemStrengthImg = equipmentSO.itemStrengthImg_;
    }
}

[System.Serializable]
public class InventoryMaterial
{
    public List<InventoryMaterialSlot> materials = new List<InventoryMaterialSlot>();
}

[System.Serializable]
public class InventoryMaterialSlot
{
    [SerializeField] protected string idItem;
    [SerializeField] protected ItemType type;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string name;
    [SerializeField] protected RarityTier tier;
    [SerializeField] protected string description;
    [SerializeField] protected int maxStack;
    [SerializeField] private ItemUses itemUses;
    [SerializeField] private bool canCombine;

    [SerializeField] private int quantity;

    public string idItem_ => idItem;
    public ItemType type_ => type;
    public Sprite icon_ => icon;
    public string name_ => name;
    public RarityTier tier_ => tier;
    public string description_ => description;
    public int maxStack_ => maxStack;
    public ItemUses itemUses_ => itemUses;
    public bool canCombine_ => canCombine;
    public int quantity_ => quantity;

    public InventoryMaterialSlot(string idItem, ItemType type, Sprite icon, string name, RarityTier tier, string description, int maxStack, ItemUses itemUses, bool canCombine, int quantity)
    {
        this.idItem = idItem;
        this.type = type;
        this.icon = icon;
        this.name = name;
        this.tier = tier;
        this.description = description;
        this.maxStack = maxStack;
        this.itemUses = itemUses;
        this.canCombine = canCombine;
        this.quantity = quantity;
    }

    public void AddQuantiy(int _quantity)
    {
        quantity += _quantity;
    }

    public void RemoveQuantiy(int _quantity)
    {
        quantity -= _quantity;
    }
}

[System.Serializable]
public class ItemSlot
{
    [SerializeField] private ItemSO item;
    [SerializeField] private int quantity;

    public ItemSO item_ => item;
    public int quantity_ => quantity;

    public ItemSlot(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}