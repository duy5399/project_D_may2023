using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "ScriptableObjects/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items;
}
