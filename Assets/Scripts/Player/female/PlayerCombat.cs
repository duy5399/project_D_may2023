using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Animator anim;

    [Header("Attack")]
    [SerializeField] private int countAttacks;
    [SerializeField] private bool attacking;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.35f;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private HealthBarController healthBar;
    [SerializeField] private bool isDead;

    public int currentHealth_ => currentHealth;
    public bool isDead_ => isDead;

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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attackPoint = transform.GetChild(1);
        currentHealth = maxHealth;
        healthBar = this.transform.GetChild(2).GetComponent<HealthBarController>();
        healthBar.SetHealth(currentHealth, maxHealth);
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking && !isDead)
        {
            UpdateAttack();
        }
    }

    protected void UpdateAttack()
    {
        attacking = true;
        anim.SetTrigger("attack_" + countAttacks);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
// HEAD
            //enemy.GetComponent<IDamageable>().TakeDamage(attackDamage);
            if(enemy.gameObject.CompareTag("MobA") || enemy.gameObject.CompareTag("MobB"))
            {
                enemy.GetComponent<MobController>().TakeDamage(attackDamage);
            }
            else if (enemy.gameObject.CompareTag("Boss"))
            {
                enemy.GetComponent<PandoraController>().TakeDamage(attackDamage);
            }
//2089b7c3def69a9e30388c370ca5fbf78cb1c7ab
        }
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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage, float interval)
    {
        StartCoroutine(PlayerMovement.instance.SlowEffect(0f,interval));
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, maxHealth);
        anim.SetTrigger("hurt");
        if (currentHealth <= 0)
        {
            anim.SetTrigger("isDead");
        }
    }

    public void SlowEffect(float interval)
    {
        StartCoroutine(PlayerMovement.instance.SlowEffect(2f, interval));
        anim.SetTrigger("hurt");
        if (currentHealth <= 0)
        {
            anim.SetTrigger("isDead");
        }
    }

    private void Die()
    {
        isDead = true;
        anim.enabled = false;
    }
}
