using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 60f;
    [SerializeField] private float midAirControl = 2f;


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask groundLayerMask;
    private Animator playerAnimator;
    private GameManager gameManager;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float jumpMultiplier = 3f;
    [SerializeField] private float fallMultiplier = 3f;
    private Vector2 vGravityY;
    private Vector2 fallForce;
    private Vector2 riseForce;
    private bool onGround;
    private float jumpTimeCounter = 0f;
    private bool isJumping;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
        vGravityY = Vector2.up * Physics2D.gravity.y;
        riseForce = -vGravityY * (jumpMultiplier - 1);
        fallForce = vGravityY * (fallMultiplier - 1);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.isGameActive)
        {
            return;
        }

        onGround = IsGrounded();
        Debug.Log(onGround);
       
        HandleJump();
        HandleMovement();
    }


    private bool IsGrounded()
    {
        return  Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.5f, groundLayerMask);
    }

    private void HandleMovement()
    {
        // Move left
        if (Input.GetKey(KeyCode.Q))
        {
            if (onGround)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                playerAnimator.SetFloat("MoveSpeed", moveSpeed);
            } else
            {
                rb.velocity += new Vector2(-moveSpeed * midAirControl * Time.deltaTime, 0);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, +moveSpeed), rb.velocity.y);
            }
        }
        else
        {
            // Move Right
            if (Input.GetKey(KeyCode.D))
            {
                if (onGround)
                {
                    rb.velocity = new Vector2(+moveSpeed, rb.velocity.y);
                    playerAnimator.SetFloat("MoveSpeed", moveSpeed);
                }
                else
                {
                    rb.velocity += new Vector2(+moveSpeed * midAirControl * Time.deltaTime, 0);
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, +moveSpeed), rb.velocity.y);
                }
            }
            else
            {
                // No Keys pressed
                if (onGround)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    playerAnimator.SetFloat("MoveSpeed", 0);
                }
            }
        }
    }

    private void HandleJump()
    {

        if (onGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = Vector2.up * jumpForce;
            playerAnimator.SetTrigger("Jumping");
        }
        // Button still behind hold down.
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        // Button release.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //Debug.Log(" Button release.");
            isJumping = false;
        }
        // Better handling of gravity.
        if (rb.velocity.y < 0)
        {
            //player jump falling
          rb.velocity += fallForce * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && isJumping == true)
        {
            // player jump rising
            rb.velocity += riseForce * Time.deltaTime;
        }
    }

}
