using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    [SerializeField] protected BossSO bossSO;

    [Header("Components")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected Transform summonPoint;

    [Header("Target")]
    [SerializeField] protected Transform target;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed; //************************

    [Header("Flip")]
    [SerializeField] protected bool facingLeft; //************************

    [Header("Attack")]
    [SerializeField] protected int attackDamage; //************************
    [SerializeField] protected List<GameObject> prefabBullet; //************************

    [Header("Health")]
    [SerializeField] protected int maxHealth; //************************
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int armor; //************************
    [SerializeField] protected HealthBarController healthBar01;
    [SerializeField] protected bool isDead;

    [Header("SummonMob")]
    [SerializeField] protected List<GameObject> prefabMob; //************************
    [SerializeField] protected int numberOfMob; //************************

    [Header("Mechanic")]
    [SerializeField] protected IEnumerator bossMechanics;
    [SerializeField] protected float intervalNextAction; //************************
    [SerializeField] protected int turnAction;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        attackPoint = transform.GetChild(0).transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.layer = LayerMask.NameToLayer("CantAttack");
        healthBar01 = transform.GetChild(1).GetComponent<HealthBarController>();
        isDead = false;
        currentHealth = maxHealth;
        turnAction = 0;
        UpdateHealth();
        bossMechanics = BossMechanics(intervalNextAction);
        StartCoroutine(bossMechanics);
    }

    protected virtual void FixedUpdate()
    {
        if(!isDead)
        {
            Flip();
        }
    }

    public void SetDataForBoss(BossSO _bossSO)
    {
        this.bossSO = _bossSO;
        this.moveSpeed = _bossSO.moveSpeed_;
        this.facingLeft = _bossSO.facingLeft_;
        this.attackDamage = _bossSO.attackDamage_;
        this.prefabBullet = _bossSO.prefabBullet_;
        this.maxHealth = _bossSO.maxHealth_;
        this.armor = _bossSO.armor_;
        this.prefabMob = _bossSO.prefabMob_;
        this.numberOfMob = _bossSO.numberOfMob_;
        this.intervalNextAction = _bossSO.intervalNextAction_;
    }

    protected virtual IEnumerator BossMechanics(float _interval)
    {
        yield return null;
    }

    protected void UpdateHealth()
    {
        healthBar01.SetHealth(currentHealth, maxHealth);
        UIController.instance.SetHealth(currentHealth, maxHealth);
    }

    protected virtual void Attack()
    {

    }

    protected virtual IEnumerator SummonMobs()
    {
        yield return null;
    }


    protected virtual void BuffATK()
    {
       
    }

    protected virtual void BuffDEF()
    {
        
    }

    protected void Flip()
    {
        if (this.transform.position.x > target.transform.position.x && !facingLeft)
        {
            this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
            facingLeft = true;
        }
        else if (this.transform.position.x < target.transform.position.x && facingLeft)
        {
            this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
            facingLeft = false;
        }
    }

    public void TakeDamage(int _damage)
    {
        int dmg = _damage * 600 / (600 + armor);
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

    protected void HideBoss()
    {
        if (isDead)
        {
            gameObject.SetActive(false);
        }
    }
}
