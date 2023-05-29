using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraController : MonoBehaviour
{
    [SerializeField]
    //public Rigidbody2D rb2d;
    public Animator anim;
    public BoxCollider2D boxCollider;

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
    public HealthBarController healthBar01;
    public HealthbarBossController healthBar02;
    public int maxHealth = 100;
    public int currentHealth;
    public float dazedTime;

    [Header("Mechanic")]

    public float intervalNextAction = 2f;
    public int turn = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        attackPoint = transform.GetChild(1).transform;
        currentHealth = maxHealth;
        UpdateHealth();
        StartCoroutine(PandoraMechanics(intervalNextAction, turn));
    }

    void FixedUpdate()
    {
        UpdateHealth();
    }

    public IEnumerator PandoraMechanics(float interval, int turn)
    {
        //SummonMobs();
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack02());
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack00());
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack01());
        yield return new WaitForSeconds(interval);
        Buff_Atk();
        yield return new WaitForSeconds(interval);
        turn++;
        if(turn < 5)
        {
            StartCoroutine(PandoraMechanics(intervalNextAction, turn));
        }
    }

    public void UpdateHealth()
    {
        healthBar01.SetHealth(currentHealth, maxHealth);
        healthBar02.SetHealth(currentHealth, maxHealth);
    }

    public void SummonMobs()
    {
        PandoraSummon.instance.Summon();
        anim.SetTrigger("summonMobs");
    }

    public IEnumerator Attack00()
    {
        anim.SetTrigger("attack_00");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.NormalArrow();
    }

    public IEnumerator Attack01()
    {
        anim.SetTrigger("attack_01");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.LightningArrow();
    }

    public IEnumerator Attack02()
    {
        anim.SetTrigger("attack_02");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.IceArrow();
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

    public void Movement()
    {
        Vector2 tart
    }

}
