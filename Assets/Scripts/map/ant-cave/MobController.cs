using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class MobController : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;

    [Header("Movement")]
    public GameObject target;
    public float moveSpeed;

    [Header("Flip")]
    public bool facingLeft = true;

    [Header("Attack")]
    public bool isAttack;
    public float attackRange = 0.22f;
    public float attackTime = 0.3f;
    public Transform attackPoint;
    public int attackDamage;
    public LayerMask playerLayer;

    [Header("Health")]
    public HealthBarController healthBar;
    public int maxHealth;
    public int currentHealth;

    void Awake()
    {
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player");
        attackPoint = transform.GetChild(0);
        healthBar = this.transform.GetChild(1).GetComponent<HealthBarController>();
        if (transform.tag == "MobA")
        {
            moveSpeed = 2f;
            attackDamage = 50;
            maxHealth = 50;
        }
        else
        {
            moveSpeed = 1f;
            attackDamage = 20;
            maxHealth = 100;
        }
        LoadMobController();
    }

    public void LoadMobController()
    {      
        currentHealth = maxHealth;     
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        ModFlip();
        MobAttack();
    }

    public void UpdateMovement()
    {
        Vector2 targetPosition = new Vector2(target.transform.position.x, this.transform.position.y);
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    }

    public void ModFlip()
    {
        if(this.transform.position.x > target.transform.position.x && !facingLeft)
        {
            this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
            facingLeft = true;
        }
        else if(this.transform.position.x < target.transform.position.x && facingLeft)
        {
            this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
            facingLeft = false;
        }
    }

    public void MobAttack()
    {
        if (Vector2.Distance(this.transform.position, target.transform.position) <= attackRange)
        {
            isAttack = true;            
        }
        else
        {
            isAttack = false;          
        }
        anim.SetBool("beatA", isAttack);
    }

    public void MobDealDamage()
    {
        var hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(10);
        }
    }

    public void MobTakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, maxHealth);
        anim.SetTrigger("cry");
        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
        }
    }

    public void Die()
    {       
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void StartDazedTime()
    {
        moveSpeed = 0f;
    }

    public void EndDazedTime()
    {
        moveSpeed = 1f;
    }
}
