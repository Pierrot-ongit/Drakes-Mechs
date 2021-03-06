using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 60f;
    public float airSpeed = 10f;
    public float jumpForce = 60f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Vector3 moveDir;
    private bool isJumpButtonDown = false;
    private bool isOnGround = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.Q)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }

        moveDir = new Vector3(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpVelocity = 100f;
            rb.velocity = Vector2.up* jumpVelocity;
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //    isJumpButtonDown = false;
            isOnGround = false;
        }
    }

    private void FixedUpdate()
    {

         rb.velocity = moveDir * speed;
        if (isOnGround)
        {

        }
        else { 
           // rb.velocity = moveDir * airSpeed;
        }
       


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
