using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;

    [Header("Movement")]
    [SerializeField] private bool canMove;
    [SerializeField] private Vector2 movement;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dazedTime;

    [Header("Jump")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool doubleJump;

    [Header("Dash")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private bool canDash;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        this.groundCheckPoint = this.transform.GetChild(0);
        tr = GetComponent<TrailRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        moveSpeed = 6f;
        jumpForce = 25f;

        canDash = true;
        isDashing = false;
        dashPower = 10f;
        dashTime = 0.4f;
        dashCooldown = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            anim.SetFloat("moveSpeed", Mathf.Abs(movement.x));
        }
        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            //Debug.Log("Input.GetKeyDown(KeyCode.Space)");
            if (GroundCheck())
            {
                //Debug.Log("GroundCheck = true");
                UpdateJump();
                doubleJump = true;
                AudioManager.instance.PlayerJumpSFX(audioSource);
            }
            else if (doubleJump)
            {
                //Debug.Log("GroundCheck = false");
                this.rb2d.velocity = Vector2.up * jumpForce * 0.7f;
                doubleJump = false;
                AudioManager.instance.PlayerJumpSFX(audioSource);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            AudioManager.instance.PlayerDashSFX(audioSource);
        }
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        UpdateMovement();
        UpdateAnimation();
    }

    protected virtual void UpdateMovement()
    {
        this.rb2d.velocity = new Vector2(movement.x * moveSpeed, rb2d.velocity.y);
        //this.rb2d.MovePosition(this.rb2d.position + this.movement * moveSpeed * Time.fixedDeltaTime);
    }

    protected void UpdateJump()
    {
        this.rb2d.velocity = Vector2.up * jumpForce;
        //this.rb2d.AddForce(Vector2.up * jumpForce);
    }

    protected bool GroundCheck()
    {
        return Physics2D.OverlapCapsule(groundCheckPoint.position, new Vector2(0.15f, 0.03f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    protected void Flip(int scaleX)
    {
        this.transform.localScale = new Vector3(scaleX, this.transform.localScale.y, 0);
        transform.GetChild(2).GetChild(0).localScale = this.transform.localScale;
    }

    protected void UpdateAnimation()
    {
        if (movement.x != 0)
        {
            Flip((int)movement.x);
            //bool x = movement.x > 0 ? transform.GetComponent<SpriteRenderer>().flipX = false : transform.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (GroundCheck())
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
        anim.SetFloat("velocity.y", this.rb2d.velocity.y);
    }

    public IEnumerator SlowEffect(float _moveSpeed,float _interval)
    {
        moveSpeed = _moveSpeed;
        yield return new WaitForSeconds(_interval);
        moveSpeed = 6f;
    }

    public void SetMoveSpeed(float _moveSpeed)
    {
        this.moveSpeed = _moveSpeed;
    }

    public void SetCanDash(bool _canDash)
    {
        this.canDash = _canDash;
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetTrigger("dash");
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        this.rb2d.velocity = new Vector2(transform.localScale.x * dashPower, 0);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void SetLayer(string _layer)
    {
        gameObject.layer = LayerMask.NameToLayer(_layer);
    }
}
