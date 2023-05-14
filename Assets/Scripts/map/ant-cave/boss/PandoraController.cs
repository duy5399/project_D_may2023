using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraController : MonoBehaviour
{
    [SerializeField]
    //public Rigidbody2D rb2d;
    public Animator anim;

    [Header("Movement")]
    public GameObject target;
    public float moveSpeed = 1f;

    [Header("Flip")]
    public bool facingLeft = true;

    [Header("Attack")]
    public bool isAttack;
    public float attackRange = 0.22f;
    public float attackTime = 0.3f;
    public Transform attackPoint;
    public int attackDamage = 40;
    public LayerMask playerLayer;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public float dazedTime;

    [Header("Health")]
    public int phase = 1;
    public int check = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if(currentHealth == 100 && phase == 1)
        {
            PandoraSpawnerMob.instance.SummonMobs(phase);
            phase++;
        }
        else if(currentHealth < 50 && phase == 2)
        {
            PandoraSpawnerMob.instance.SummonMobs(phase);
            phase++;
        }
    }

    
}
