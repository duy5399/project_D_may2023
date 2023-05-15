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

    [Header("Mechanic")]
    public float intervalNextAction = 5f;
    public int phase = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if(phase == 1)
        {
            StartCoroutine(PandoraMechanics(intervalNextAction));
            phase++;
        }
    }

    public IEnumerator PandoraMechanics(float interval)
    {
        SummonMobs();
        yield return new WaitForSeconds(interval);
        Attack00();
        yield return new WaitForSeconds(interval);
        Attack01();
        yield return new WaitForSeconds(interval);
        Attack02();
        yield return new WaitForSeconds(interval);
        Buff_Atk();
    }

    public void SummonMobs()
    {
        PandoraSpawnerMob.instance.SummonMobs();
        anim.SetTrigger("summonMobs");
    }

    public void Attack00()
    {
        anim.SetTrigger("attack_00");
    }

    public void Attack01()
    {
        anim.SetTrigger("attack_01");
    }

    public void Attack02()
    {
        anim.SetTrigger("attack_02");
    }

    public void Buff_Atk()
    {
        anim.SetTrigger("buff");
        attackDamage = attackDamage + (attackDamage / 10);
    }

    public void Buff_Def()
    {
        anim.SetTrigger("buff");
    }

}
