using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    [SerializeField] protected InventorySO inventorySO;
    //[SerializeField] protected ItemDatabaseSO itemDatabaseSO;

    //add item to inventory
    public virtual bool AddItem(ItemSO _item)
    {
        inventorySO.AddItem(_item, 1);
        return true;
    }

    //remove item to inventory
    public virtual bool RemoveItem(ItemSO _item)
    {
        inventorySO.RemoveItem(_item, 1);
        return true;
    }

    public InventorySO GetInventorySO()
    {
        return inventorySO;
    }
}
