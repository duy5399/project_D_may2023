using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "ScriptableObjects/Player Stat")]
public class StatsSO : ScriptableObject
{
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int hitPoint;
    [SerializeField] private int luck;

    public int attack_ => attack;
    public int defense_ => defense;
    public int hitPoint_ => hitPoint;
    public int luck_ => luck;

    public void ResetStat()
    {
        attack = 0;
        defense = 0;
        hitPoint = 0;
        luck = 0;
    }

    public void SetAttack(int attack)
    {
        this.attack = attack;
    }
    public void SetDefense(int defense)
    {
        this.defense = defense;
    }
    public void SetHitPoint(int hitPoint)
    {
        this.hitPoint = hitPoint;
    }
    public void SetLuck(int luck)
    {
        this.luck = luck;
    }
}
