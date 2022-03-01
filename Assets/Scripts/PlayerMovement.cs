using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 60f;
    [SerializeField] private float midAirControl = 2f;
    [SerializeField] private float jumpForce = 60f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask groundLayerMask;
    private Animator playerAnimator;

    // private Vector3 moveDir;
    //  private bool isOnGround = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            float jumpVelocity = 100f;
            rb.velocity = Vector2.up * jumpForce;
        }

        HandleMovement();
    }


    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d =  Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, groundLayerMask);
        return raycastHit2d != null;
    }

    private void HandleMovement()
    {
        // Move left
        if (Input.GetKey(KeyCode.Q))
        {
            if (IsGrounded())
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
                if (IsGrounded())
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
                if (IsGrounded())
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    playerAnimator.SetFloat("MoveSpeed", 0);
                }
            }
        }
    }
}
