using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool canDash = true;
    public static bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] TrailRenderer trail;

    public static bool knockback = false;
    public static bool IsPaused = false;

    private float coyoteTime = 0.2f;
    public float coyoteCounter; 

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool doubleJump;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    private void Update()
    {
        if (knockback)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }

        if (IsPaused)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (horizontal != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
            animator.SetBool("IsGrounded", false);
        }

        if (coyoteCounter > 0f && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            if (coyoteCounter > 0f || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
                jumpBufferCounter = 0f;
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (knockback)
        {
            return;
        }

        if (IsPaused)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        animator.SetBool("IsDashing", true);
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("IsDashing", false);
        Time.timeScale = 1;
        yield return new WaitForSeconds(dashingCooldown); 
        canDash = true;
        Time.timeScale = 1;
    }

    public IEnumerator Knockback(Vector2 diff, int kbforce)
    {
        GetComponent<AudioSource>().Play();
        Vector2 force = diff * kbforce;
        knockback = true;
        rb.AddForce(force, ForceMode2D.Impulse);
        //animator.SetBool("IsGrounded", true);
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);
        knockback = false;
        animator.SetBool("IsGrounded", true);
        PlayerMove.IsPaused = false;
        Time.timeScale = 1;
    }
}