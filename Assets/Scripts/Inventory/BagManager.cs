using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    [SerializeField] protected InventorySO inventorySO;
    public InventorySO inventorySO_ => inventorySO;
    //[SerializeField] protected ItemDatabaseSO itemDatabaseSO;

    //add item to inventory
    public virtual bool AddItem(ItemSO _item, int _quantity)
    {
        inventorySO.AddItem(_item, _quantity);
        return true;
    }

    //remove item to inventory
    public virtual bool RemoveItem(ItemSO _item, int _quantity)
    {
        inventorySO.RemoveItem(_item, _quantity);
        return true;
    }

    public virtual InventoryMaterialSlot GetMaterial(string _idItem)
    {
        return inventorySO.listMaterialSO_.materials.Find(x => x.idItem_ == _idItem);
    }

    public InventorySO GetInventorySO()
    {
        return inventorySO;
    }
}
