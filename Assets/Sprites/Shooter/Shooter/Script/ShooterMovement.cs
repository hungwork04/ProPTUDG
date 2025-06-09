using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public float speed = 6f;
    public float jumpForce = 5f;
    public Animator ani;
    public bool FacingRight = true;
    public Transform gun; // Vẫn giữ để tham chiếu, nhưng không lật localScale
    public ShooterGun shooterGun;
    public int maxJumps = 2; // Số lần nhảy tối đa (2 cho double jump)
    private int jumpCount; // Đếm số lần nhảy hiện tại

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shooterGun = GetComponent<ShooterGun>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        ani.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x)); // Sử dụng Abs để animation đồng nhất

        FlipTrans();   

        // Xử lý nhảy và trạng thái
        Jump();
        ani.SetFloat("yVelocity", rb.velocity.y);
        ani.SetBool("IsJumping", isJumping);
        ani.SetBool("IsFalling", rb.velocity.y < 0);
    }

    public void FlipTrans()
    {
        if (shooterGun.isShooting) return;
        // Lật nhân vật
        if (moveInput.x > 0 && !FacingRight)
        {
            FacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0 && FacingRight)
        {
            FacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public bool isJumping;
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            isJumping = true;
            ani.SetBool("IsJumping", isJumping); // Cập nhật Animator
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0; // Reset số lần nhảy
            isJumping = false;
            ani.SetBool("IsJumping", isJumping);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
            ani.SetBool("IsJumping", isJumping);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * moveInput.x, rb.velocity.y);
    }
}