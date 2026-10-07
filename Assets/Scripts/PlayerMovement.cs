using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private BoxCollider2D playerCollider;
    private Animator animator;
    private ParticleSystem smokeFX;
    bool isFacingRight = true;

    [Header("Moving")]
    public float moveSpeed = 5.0f;
    float horizontalMovement;
    public Vector2 startPos {  get; private set; }

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJump = 2;
    int jumpRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    
    bool isGrounded;
    bool isOnPlatform;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.6f, 0.05f);
    public LayerMask wallLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 10f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;
    //wallJump
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        smokeFX = GetComponentInChildren<ParticleSystem>();

        startPos = transform.position;
        GameController.OnLevelReset += GoToStartPos;
    }

    void Update()
    {
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        rb2D.velocity = new Vector2(horizontalMovement * moveSpeed, rb2D.velocity.y);
        Flip();

        animator.SetFloat("yVelocity", rb2D.velocity.y);
        animator.SetFloat("magnitude", rb2D.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isGrounded", isGrounded);
    }
    private void OnDestroy()
    {
        GameController.OnLevelReset -= GoToStartPos;
    }
    private void ProcessGravity()
    {
        if(rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = baseGravity * fallSpeedMultiplier;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Max(rb2D.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb2D.gravityScale = baseGravity;
        }
    }
    public void Move(InputAction.CallbackContext context) 
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 0)
        {
            if (context.performed)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
                jumpRemaining--;
                JumpFX();
            }
            else if (context.canceled && rb2D.velocity.y > 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f);
                jumpRemaining--;
                JumpFX();
            }
        }

        //wallJump
        if (context.performed && wallJumpTimer > 0)
        {
            isWallJumping = true;
            rb2D.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            JumpFX();

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }
    public void DropDown(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DisableCollider(0.25f));
        }
    }
    private IEnumerator DisableCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }
    private void JumpFX()
    {
        animator.SetTrigger("jump");
        smokeFX.Play();
    }
    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            if(rb2D.velocity.y == 0)
            {
                smokeFX.Play();
            }
        }
    }
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer))
        {
            jumpRemaining = maxJump;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer);
    }
    private void ProcessWallSlide()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Max(rb2D.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    private void GoToStartPos()
    {
        transform.position = startPos;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

}
