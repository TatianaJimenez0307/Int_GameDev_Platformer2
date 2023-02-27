using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed = 2.0f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool movingRight = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      //  groundCheck = transform.Find("ColliderCheck");
    }

    void FixedUpdate()
    {
        Move();
        CheckForGroundCollision();
    }

    void Move()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            
        }
    }

    void CheckForGroundCollision()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (groundCollider != null)
        {
            movingRight = !movingRight;
        }
    }
}
