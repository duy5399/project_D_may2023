using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertResultBattle : MonoBehaviour
{
    [SerializeField] private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    //hiển thị thông báo khi thua(nhân vật hết máu, hết thời gian,...)
    public void OnAlertLose()
    {
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        anim.SetTrigger("loseBattle");
    }

    //hiển thị thông báo khi chiến thắng (đánh bại BOSS trước khi hết thời gian và HP > 0)
    public void OnAlertWin()
    {
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        anim.SetTrigger("winBattle");
    }

    public void OffAlertResultBattle()
    {
        gameObject.SetActive(false);
    }
}
