using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance;

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
    public int maxHealth = 100;
    public int currentHealth;

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

        this.anim = GetComponent<Animator>();
        this.attackPoint = this.transform.GetChild(1);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            PlayerMovement.instance.DazedTime();
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
            if(enemy.gameObject.CompareTag("MobA") || enemy.gameObject.CompareTag("MobB"))
            {
                enemy.GetComponent<MobController>().MobTakeDamage(attackDamage);
            }
            else if (enemy.gameObject.CompareTag("Boss"))
            {
                enemy.GetComponent<PandoraController>().TakeDamage(attackDamage);
            }
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

    public void TakeDamage(int damage)
    {
        PlayerMovement.instance.DazedTime();
        currentHealth -= damage;
        anim.SetTrigger("hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetBool("isDead", true);
    }
}
