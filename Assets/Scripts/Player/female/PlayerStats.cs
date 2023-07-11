using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    #region Singleton
    public static PlayerStats instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        string resourcePath = "ScriptableObjects/Character/PlayerStat";
        //Debug.Log(resourcePath);
        playerStat = Resources.Load<StatsSO >(resourcePath);
        attack = playerStat.attack_;
        defense = playerStat.defense_;
        hitPoint = playerStat.hitPoint_;
        luck = playerStat.luck_;
    }
    #endregion

    [SerializeField] private StatsSO playerStat;
    public StatsSO playerStat_ => playerStat;

    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int hitPoint;
    [SerializeField] private int luck;

    public int attack_ => attack;
    public int defense_ => defense;
    public int hitPoint_ => hitPoint;
    public int luck_ => luck;

    public void SetAttack(int _attack)
    {
        playerStat.SetAttack(_attack);
    }
    public void SetDefense(int _defense) 
    {  
        playerStat.SetDefense(_defense);
    }
    public void SetHitPoint(int _hitPoint) 
    {  
        playerStat.SetHitPoint(_hitPoint);
    }
    public void SetLuck(int _luck)
    {
        playerStat.SetLuck(_luck);
    }
}
