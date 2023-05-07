using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator anim;
    [SerializeField] private int countAttacks;
    [SerializeField] private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        this.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            UpdateAttack();
        }
    }

    protected void UpdateAttack()
    {
        attacking = true;
        anim.SetTrigger("attack_" + countAttacks);    
    }

    protected void StartAttack()
    {
        attacking = false;
        if (countAttacks < 3)
        {
            countAttacks++;
        }
    }

    protected void EndAttack()
    {
        attacking = false;
        countAttacks = 0;
    }
}
