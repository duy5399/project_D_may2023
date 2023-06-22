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
    private UIController uiController;
    [SerializeField]
    private bool isDead;

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
        boxCollider2D = GetComponent<BoxCollider2D>();
        attackPoint = transform.GetChild(0).transform;
        isDead = false;
        currentHealth = maxHealth;
        boxCollider2D.enabled = false;
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
        if (!isDead)
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
                    boxCollider2D.enabled = true;
                }
                StartCoroutine(PandoraMechanics(intervalNextAction, turn));
            }
        }
        yield return null;
    }

    public void UpdateHealth()
    {
        healthBar01.SetHealth(currentHealth, maxHealth);
        uiController.SetHealth(currentHealth, maxHealth);
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
        uiController.OnAlert("Pandora chuẩn bị ra khỏi tổ, hãy tấn công!!!");
        GameObject teleport = transform.GetChild(2).gameObject;
        teleport.SetActive(true);
        yield return new WaitForSeconds(2f);
        Vector2 newPosition = new Vector2(7f, -1.8f);
        this.transform.position = newPosition;
        teleport.SetActive(false);
        uiController.OffAlert();
    }

    public void TakeDamage(int damage)
    {
        int dmg = damage * 600 / (600 + armor);
        currentHealth -= dmg;
        Debug.Log(dmg);
        UpdateHealth();
        anim.SetTrigger("cry");
        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetBool("isDead", isDead);
        }
    }

}
