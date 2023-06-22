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
    }
    #endregion

    public int attack;
    public int defense;
    public int hitPoint;
    public int luck;

    public Text attackTxt; //Bag - PlayerStats - ATK - Stat
    public Text defenseTxt; //Bag - PlayerStats - DEF - Stat
    public Text hitPointTxt; //Bag - PlayerStats - HP - Stat
    public Text luckTxt; //Bag - PlayerStats - LUCK - Stat

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdatePlayerStats()
    {
        attackTxt.text = attack.ToString();
        defenseTxt.text = defense.ToString();
        hitPointTxt.text = hitPoint.ToString();
        luckTxt.text = luck.ToString();
    }
}
