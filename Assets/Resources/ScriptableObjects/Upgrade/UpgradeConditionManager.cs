using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New UpgradeCondition", menuName = "ScriptableObjects/UpgradeSystem/UpgradeCondition")]
public class UpgradeConditionManager : ScriptableObject
{
    [SerializeField] private List<Sprite> strengthImg = new List<Sprite>();
    [SerializeField] private List<MaterialSO> strengthStone = new List<MaterialSO>();

    public Sprite GetStrengthImg(int strength)
    {
        return strengthImg[strength];
    }

    public int GetValueBonusStrength(int strength)
    {
        switch (strength)
        {
            case 1:
                return 10;
            case 2:
                return 30;
            case 3:
                return 50;
            case 4:
                return 100;
            case 5:
                return 200;
            case 6:
                return 300;
            case 7:
                return 400;
            case 8:
                return 500;
            case 9:
                return 600;
            case 10:
                return 700;
            case 11:
                return 800;
            case 12:
                return 1000;
            case 13:
                return 1500;
            case 14:
                return 2000;
            case 15:
                return 3000;
        }
        return 0;
    }

    public float UpgradeCondition(int _itemStrength, MaterialSO _strengthStone)
    {
        float _successRate = 0f;
        if(_strengthStone.itemUses_ == ItemUses.Upgrade)
        {
            switch (_itemStrength)
            {
                case 0:
                    _successRate = 100f;
                    break;
                case 1:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 80f;
                            break;
                        case "strengthStone_02":
                            _successRate = 100f;
                            break;
                        case "strengthStone_03":
                            _successRate = 100f;
                            break;
                        case "strengthStone_04":
                            _successRate = 100f;
                            break;
                        case "strengthStone_05":
                            _successRate = 100f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 2:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 50f;
                            break;
                        case "strengthStone_02":
                            _successRate = 80f;
                            break;
                        case "strengthStone_03":
                            _successRate = 100f;
                            break;
                        case "strengthStone_04":
                            _successRate = 100f;
                            break;
                        case "strengthStone_05":
                            _successRate = 100f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 3:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 20f;
                            break;
                        case "strengthStone_02":
                            _successRate = 40f;
                            break;
                        case "strengthStone_03":
                            _successRate = 60f;
                            break;
                        case "strengthStone_04":
                            _successRate = 80f;
                            break;
                        case "strengthStone_05":
                            _successRate = 100f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 4:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 20f;
                            break;
                        case "strengthStone_02":
                            _successRate = 30f;
                            break;
                        case "strengthStone_03":
                            _successRate = 40f;
                            break;
                        case "strengthStone_04":
                            _successRate = 60f;
                            break;
                        case "strengthStone_05":
                            _successRate = 100f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 5:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 5f;
                            break;
                        case "strengthStone_02":
                            _successRate = 10f;
                            break;
                        case "strengthStone_03":
                            _successRate = 20f;
                            break;
                        case "strengthStone_04":
                            _successRate = 30f;
                            break;
                        case "strengthStone_05":
                            _successRate = 50f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 6:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.11f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.41f;
                            break;
                        case "strengthStone_03":
                            _successRate = 1.66f;
                            break;
                        case "strengthStone_04":
                            _successRate = 6.66f;
                            break;
                        case "strengthStone_05":
                            _successRate = 26.66f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 7:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.09f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.36f;
                            break;
                        case "strengthStone_03":
                            _successRate = 1.45f;
                            break;
                        case "strengthStone_04":
                            _successRate = 5.83f;
                            break;
                        case "strengthStone_05":
                            _successRate = 23.33f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 8:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.07f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.32f;
                            break;
                        case "strengthStone_03":
                            _successRate = 1.25f;
                            break;
                        case "strengthStone_04":
                            _successRate = 5f;
                            break;
                        case "strengthStone_05":
                            _successRate = 20f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 9:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.06f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.26f;
                            break;
                        case "strengthStone_03":
                            _successRate = 1.04f;
                            break;
                        case "strengthStone_04":
                            _successRate = 4.16f;
                            break;
                        case "strengthStone_05":
                            _successRate = 16.66f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 10:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.05f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.2f;
                            break;
                        case "strengthStone_03":
                            _successRate = 0.83f;
                            break;
                        case "strengthStone_04":
                            _successRate = 3.33f;
                            break;
                        case "strengthStone_05":
                            _successRate = 13.33f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 11:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.03f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.16f;
                            break;
                        case "strengthStone_03":
                            _successRate = 0.63f;
                            break;
                        case "strengthStone_04":
                            _successRate = 2.5f;
                            break;
                        case "strengthStone_05":
                            _successRate = 10f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 12:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.02f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.11f;
                            break;
                        case "strengthStone_03":
                            _successRate = 0.44f;
                            break;
                        case "strengthStone_04":
                            _successRate = 1.75f;
                            break;
                        case "strengthStone_05":
                            _successRate = 7f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 13:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.02f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.08f;
                            break;
                        case "strengthStone_03":
                            _successRate = 0.32f;
                            break;
                        case "strengthStone_04":
                            _successRate = 1.25f;
                            break;
                        case "strengthStone_05":
                            _successRate = 5f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
                case 14:
                    switch (_strengthStone.itemID_)
                    {
                        case "strengthStone_01":
                            _successRate = 0.01f;
                            break;
                        case "strengthStone_02":
                            _successRate = 0.05f;
                            break;
                        case "strengthStone_03":
                            _successRate = 0.19f;
                            break;
                        case "strengthStone_04":
                            _successRate = 0.75f;
                            break;
                        case "strengthStone_05":
                            _successRate = 3f;
                            break;
                        default:
                            _successRate = 0f;
                            break;
                    }
                    break;
            }
        }       
        return _successRate;
    }
}
