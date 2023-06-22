using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeEquipment", menuName = "ScriptableObjects/UpgradeSystem")]
public class UpgradeSystemSO : ScriptableObject
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string upgradeDescription;
    [SerializeField] private UpgradeType upgradeType;

    public string upgradeName_ => upgradeName;
    public string upgradeDescription_ => upgradeDescription;
    public UpgradeType upgradeType_ => upgradeType;
}

public enum UpgradeType
{
    Upgrade,
    Enchant
}
