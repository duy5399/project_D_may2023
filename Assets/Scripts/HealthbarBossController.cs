using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBossController : MonoBehaviour
{
    public Slider healthbar;
    public Image img_icon;
    public Text txt_name;
    public Text txt_hp;

    void Awake()
    {
        if (transform.childCount > 0 && transform.GetChild(0) != null)
        {
            healthbar = gameObject.transform.GetChild(0).GetComponent<Slider>();
            img_icon = gameObject.transform.GetChild(0).GetChild(3).GetComponent<Image>();
            txt_name = gameObject.transform.GetChild(0).GetChild(4).GetComponent<Text>();
            txt_hp = gameObject.transform.GetChild(0).GetChild(5).GetComponent<Text>();
        }
        else
        {
            Debug.Log("No child");
        }
    }

    void Start()
    {
        txt_name.text = GameObject.FindGameObjectWithTag("Boss").transform.name;
        var iconBoss = Resources.Load<Sprite>("Sprites/IconBoss/"+txt_name.text+"/"+txt_name.text+"_icon");
        Debug.Log("Sprites/IconBoss/" + txt_name.text + "/" + txt_name.text + "_icon");
        img_icon.sprite = iconBoss;
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthbar.maxValue = maxHealth;
        healthbar.value = health;
        txt_hp.text = Math.Round((double)health / maxHealth * 100) + "%";
    }
}
