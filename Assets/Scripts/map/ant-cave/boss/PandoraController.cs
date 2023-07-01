using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraController : MonoBehaviour, IDamageable
{
    [SerializeField]
    //public Rigidbody2D rb2d;
    private Animator anim;
    [SerializeField]
    private BoxCollider2D boxCollider2D;

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
    private int attackDamage = 40;

    [Header("Health")]
    [SerializeField]
    private int maxHealth = 1000;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int armor = 50;
    [SerializeField]
    private HealthBarController healthBar01;
    [SerializeField]
    private bool isDead;
    private int def = 10;
    private float dazedTime;

    [Header("Mechanic")]
    [SerializeField]
    private float intervalNextAction = 2f;
    [SerializeField]
    private int turn = 1;
    [SerializeField] private IEnumerator bossMechanics;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider2D = GetComponent<BoxCollider2D>();
        attackPoint = transform.GetChild(0).transform;
        isDead = false;
        currentHealth = maxHealth;
        boxCollider2D.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("CantAttack");
        UpdateHealth();
        bossMechanics = PandoraMechanics(intervalNextAction);
        StartCoroutine(bossMechanics);
    }

    void FixedUpdate()
    {
        //UpdateHealth();
        //Movement();
    }

    public IEnumerator PandoraMechanics(float interval)
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(interval);
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
            if (turn < 5)
            {
                if (turn == 2)
                {
                    StartCoroutine(Movement());
                    gameObject.layer = LayerMask.NameToLayer("Boss");
                }
                //StartCoroutine(PandoraMechanics(intervalNextAction));
            }
            Debug.Log("turn: " +turn);
        }
        yield return null;
    }

    public void UpdateHealth()
    {
        healthBar01.SetHealth(currentHealth, maxHealth);
        UIController.instance.SetHealth(currentHealth, maxHealth);
    }

    public void SummonMobs()
    {
        PandoraSummon.instance.Summon();
        anim.SetTrigger("beatA");
    }

    public IEnumerator Attack00(float speed, Transform atkPoint, Transform tg)
    {
        anim.SetTrigger("beatB");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.NormalArrow(speed, atkPoint, tg.position);
    }

    public IEnumerator Attack01()
    {
        anim.SetTrigger("beatC");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.LightningArrow();
    }

    public IEnumerator Attack02(float speed, Transform atkPoint, Transform tg)
    {
        anim.SetTrigger("beatD");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.IceArrow(speed, atkPoint, tg.position);
    }

    public void Buff_Atk()
    {
        anim.SetTrigger("beatE");
        attackDamage = attackDamage + (attackDamage / 10);
    }

    public void Buff_Def()
    {
        anim.SetTrigger("beatE");
    }

    public IEnumerator Movement()
    {
        UIController.instance.OnAlertWarning("Pandora chuẩn bị ra khỏi tổ, hãy tấn công!!!");
        GameObject teleport = transform.GetChild(2).gameObject;
        teleport.SetActive(true);
        yield return new WaitForSeconds(2f);
        Vector2 newPosition = new Vector2(7f, -1.8f);
        this.transform.position = newPosition;
        teleport.SetActive(false);
        UIController.instance.OffAlertWarning();
    }

    public void TakeDamage(int damage)
    {
        int dmg = damage * 600 / (600 + armor);
        currentHealth -= dmg;
        UpdateHealth();
        anim.SetTrigger("cry");
        if (currentHealth <= 0)
        {
            isDead = true;
            StopMechanics();
            anim.SetBool("isDead", isDead);
        }
    }

    public void StopMechanics()
    {
        if (bossMechanics != null)
        {
            StopCoroutine(bossMechanics);
            bossMechanics = null;
        }
    }

    private void HideBoss()
    {
        if (isDead)
        {
            gameObject.SetActive(false);
        }
    }
}