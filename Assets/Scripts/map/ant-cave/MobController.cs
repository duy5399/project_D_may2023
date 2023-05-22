using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    [Header("Components")]
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
    public HealthBarController healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    void Awake()
    {
        target = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar = this.transform.GetChild(1).GetComponent<HealthBarController>();
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
        anim.SetBool("attack", isAttack);
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
        anim.SetTrigger("hurt");
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
