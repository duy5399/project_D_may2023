﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;

    [Header("Attack")]
    [SerializeField] private int countAttacks;
    [SerializeField] private bool attacking;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.35f;
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Defense")]
    [SerializeField] private int armor;

    [Header("Health")]
    [SerializeField] private int maxHealth;
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
        audioSource = GetComponent<AudioSource>();
        attackPoint = transform.GetChild(1);
        attackDamage = PlayerStats.instance.attack_ > 0 ? PlayerStats.instance.attack_ : 1;
        maxHealth = PlayerStats.instance.hitPoint_ > 0 ? PlayerStats.instance.hitPoint_ : 1;
        armor = PlayerStats.instance.defense_ > 0 ? PlayerStats.instance.defense_ : 1;
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
        AudioManager.instance.PlayerAttackSFX(audioSource, countAttacks);
        anim.SetTrigger("attack_" + countAttacks);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.gameObject.layer == LayerMask.NameToLayer("Mob"))
            {
                enemy.GetComponent<MobController>().TakeDamage(attackDamage);
            }
            else if (enemy.gameObject.layer == LayerMask.NameToLayer("Totem"))
            {
                enemy.GetComponent<Totem>().TakeDamage(attackDamage);
            }
            else if (enemy.gameObject.layer == LayerMask.NameToLayer("Boss") || enemy.gameObject.layer == LayerMask.NameToLayer("MiniBoss"))
            {
                enemy.GetComponent<BossController>().TakeDamage(attackDamage);
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

    public void TakeDamage(int _damage, float _interval)
    {
        AudioManager.instance.PlayerTakeDamageSFX(audioSource);
        int damage_ = _damage * 100 / (100 + armor);
        StartCoroutine(PlayerMovement.instance.SlowEffect(0f, _interval));
        currentHealth -= damage_;
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
