using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
	public GameObject canvasGameOver;
	public float speed = 8.0f;
    public float jumpForce = 8.0f;

    private Rigidbody2D rb;
    private Animator anim;

    private float horizontalInput;


    [Header("Health")]
    [SerializeField] private float startingHealth = 10;
    public float currentHealth ;
    private bool dead;
    HorizontalCameraFollow horizontalCamera;
    CamBoss camBoss;
    BossController bossController;
    public int maxJumps = 2; // Số lần nhảy tối đa (2 cho double jump)
    private int jumpCount; // Đếm số lần nhảy hiện tại
    private void Awake()
    {
        horizontalCamera=FindAnyObjectByType<HorizontalCameraFollow>();
        if(horizontalCamera!=null){
            horizontalCamera.player=this.transform;
        }
        camBoss=FindAnyObjectByType<CamBoss>();
        if(camBoss!=null){
            camBoss.player1=this.transform;
        }
        bossController=FindAnyObjectByType<BossController>();
        if(bossController!=null){
            bossController.playerPos=this.transform;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }
        private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0; // Reset số lần nhảy
        }
    }
    void Start()
    {
        currentHealth = startingHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        UpdateAnimation();
        Flip();
        Jump();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
    private void UpdateAnimation()
    {
        if (anim != null)
        {
            bool isMoving = horizontalInput != 0;
            anim.SetBool("move", isMoving);
        }
    }

    private void Flip()
    {
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void TakeDamage(float _damage)
    {
        if (dead) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                dead = true;
                anim.SetTrigger("die");
                Debug.Log("Player đã chết!");
                GetComponent<PlayerCharacterMovement>().enabled = false;
                GetComponent<PlayerCharacterAimAndShoot>().enabled = false;
				FindObjectOfType<GameManagerScript>().gameOver();
			}
        }

        if (UIHealthBar.instance != null)
            UIHealthBar.instance.SetValue(currentHealth / startingHealth);
    }

    public void AddHealth(float _amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _amount, 0, startingHealth);
        if (UIHealthBar.instance != null)
            UIHealthBar.instance.SetValue(currentHealth / startingHealth);
    }
}
