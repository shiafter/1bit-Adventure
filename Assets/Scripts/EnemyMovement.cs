using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody2D rb2D;
    public float enemySpeed;
    public int facingDirection { get; private set; } = 1;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.12f, 0.05f);
    public LayerMask groundLayer;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.05f, 0.52f);
    public LayerMask wallLayer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        CliffCheck();
        WallCheck();
    }
    void FixedUpdate()
    {
        rb2D.velocity = new Vector2(enemySpeed * facingDirection, rb2D.velocity.y);
    }
    public void CliffCheck()
    {
        if (!Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer))
        {
            Debug.Log("I found a cliff");
            Flip();
        }
    }
    public void WallCheck()
    {
        if(Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer))
        {
            Debug.Log("I hit the wall");
            Flip();
        }
    }
    private void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = facingDirection;
        transform.localScale = scale;

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (collider.GetComponentInParent<Rigidbody2D>().velocity.y <= 0f)
            {
                collider.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(rb2D.velocity.x, collider.GetComponentInParent<PlayerMovement>().jumpPower / 2);
                SoundEffectManager.Play("Stomp");
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit another enemt");
            Flip();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
