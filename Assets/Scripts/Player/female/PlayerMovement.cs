using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator anim;

    [Header("Movement")]
    [SerializeField] private Vector2 movement;
    [SerializeField] private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform pointGroundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool doubleJump;

    [SerializeField] private Vector2 velocity;

    void Awake()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {      
        moveSpeed = 6f;
        jumpForce = 25f;
        //isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("moveSpeed", Mathf.Abs(movement.x));
        //movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GroundCheck())
            {
                UpdateJump();
                doubleJump = true;
            }
            else if (doubleJump)
            {
                this.rb2d.velocity = Vector2.up * jumpForce * 0.7f;
                doubleJump = false;
            }
        }
        
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateAnimation();
        velocity = this.rb2d.velocity;
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
        return Physics2D.OverlapCapsule(pointGroundCheck.position, new Vector2(0.15f, 0.03f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    protected void Flip(int scaleX)
    {
        this.transform.localScale = new Vector3(scaleX, this.transform.localScale.y, 0);
    }

    protected void UpdateAnimation()
    {
        if (movement.x != 0)
        {
            Flip((int)movement.x);
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
}
