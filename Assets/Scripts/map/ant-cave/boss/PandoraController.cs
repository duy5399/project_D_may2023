using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraController : MonoBehaviour
{
    [SerializeField]
    //public Rigidbody2D rb2d;
    private Animator anim;
    [SerializeField]
    private BoxCollider2D boxCollider;

    [Header("Movement")]
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float moveSpeed = 1f;

    [Header("Flip")]
    [SerializeField]
    private bool facingLeft = true;

    [Header("Attack")]
    private bool isAttack;
    private float attackRange = 0.22f;
    private float attackTime = 0.3f;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private Transform earthquakePoint;
    [SerializeField]
    private GameObject earthquakeEffect;
    [SerializeField]
    private int attackDamage = 40;
    private LayerMask playerLayer;

    [Header("Health")]
    [SerializeField]
    private HealthBarController healthBar01;
    [SerializeField]
    private HealthbarBossController healthBar02;
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int def = 10;
    private float dazedTime;

    [Header("Mechanic")]
    [SerializeField]
    private float intervalNextAction = 2f;
    [SerializeField]
    private int turn = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider = GetComponent<BoxCollider2D>();
        attackPoint = transform.GetChild(0).transform;
        currentHealth = maxHealth;
        UpdateHealth();
        StartCoroutine(PandoraMechanics(intervalNextAction, turn));
    }

    void FixedUpdate()
    {
        //UpdateHealth();
        //Movement();
    }

    public IEnumerator PandoraMechanics(float interval, int turn)
    {
        if (turn == 2)
        {
            Movement();
            yield return new WaitForSeconds(interval);
        }
        //SummonMobs();
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack02(5, attackPoint, target));
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack00(5, attackPoint, target));
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack00(5, attackPoint, target));
        yield return new WaitForSeconds(interval);
        StartCoroutine(Attack02(5, attackPoint, target));
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

    public IEnumerator Attack00(float speed, Transform atkPoint, Transform tg)
    {
        anim.SetTrigger("attack_00");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.NormalArrow(speed, atkPoint, tg.position);
    }

    public IEnumerator Attack01()
    {
        anim.SetTrigger("attack_01");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.LightningArrow();
    }

    public IEnumerator Attack02(float speed, Transform atkPoint, Transform tg)
    {
        anim.SetTrigger("attack_02");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.IceArrow(speed, atkPoint, tg.position);
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
        Vector2 newPosition = new Vector2(7f, -1.8f);
        this.transform.position = newPosition;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage/def;
        UpdateHealth();
        //healthBar.SetHealth(currentHealth, maxHealth);
        anim.SetTrigger("hurt");
        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
        }
    }
}
