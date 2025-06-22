using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    public float speed = 8.0f;
    public float jumpForce = 8.0f;

    private Rigidbody2D rb;
    private Animator anim;

    private float horizontalInput;


    [Header("Health")]
    [SerializeField] private float startingHealth = 10;
    public float currentHealth ;
    private bool dead;

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
            anim.SetTrigger("hurt");
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
