using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce = 7;

    public int defaultJumps = 1;
    int addiJumps;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool onGround = false;
    public Transform isGroundChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float rememberGroundFor;
    float lastTimeGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        BetterJump();
        CheckIfGrounded();
    }
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * moveSpeed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && (onGround || Time.time - lastTimeGrounded <= rememberGroundFor))
        {
            if(addiJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                addiJumps--;
            }
            
        }
    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundChecker.position, checkGroundRadius, groundLayer);

        if(collider != null)
        {
            onGround = true;
            addiJumps = defaultJumps;
        } else
        {
            if (onGround)
            {
                lastTimeGrounded = Time.time;
            }
            onGround = false;
        }
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        } else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

}
