using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class MobController : MonoBehaviour, IDamageable
{
    [Header("ScriptableObjects")]
    [SerializeField] protected MobSO mobSO;
    public MobSO MobSO => mobSO;

    [Header("Components")]
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D boxCollider2D;

    [Header("Movement")]
    public GameObject target;
    public float moveSpeed;

    [Header("Flip")]
    public bool facingLeft = true;

    [Header("Attack")]
    public bool isAttack;
    public float attackRange;
    public Transform attackPoint;
    public int attackDamage;
    public LayerMask playerLayer;

    [Header("Health")]
    public HealthBarController healthBar;
    public int armor;
    public int maxHealth;
    public int currentHealth;
    public bool isDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        attackPoint = transform.GetChild(0);
        healthBar = this.transform.GetChild(1).GetComponent<HealthBarController>();
        LoadMobController();
    }
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public void LoadMobController()
    {
        string resourcePath = "ScriptableObjects/Mob/" + transform.tag;
        Debug.Log(resourcePath);
        mobSO = Resources.Load<MobSO>(resourcePath);
        isDead = false;
        moveSpeed = MobSO.moveSpeed;
        attackDamage = MobSO.attackDamage;
        attackRange = MobSO.attackRange;
        armor = MobSO.armor;
        maxHealth = MobSO.maxHealth;
        currentHealth = maxHealth;     
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            UpdateMovement();
            ModFlip();
            MobAttack();
        }
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
            player.GetComponent<PlayerCombat>().TakeDamage(attackDamage, 0.5f);
        }
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth > 0)
        {
            int dmg = damage * 600 / (600 + armor);
            currentHealth -= dmg;
            healthBar.SetHealth(currentHealth, maxHealth);
            anim.SetTrigger("cry");
        }
        else
        {
            boxCollider2D.enabled = false;
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
        moveSpeed = MobSO.moveSpeed;
    }
}
