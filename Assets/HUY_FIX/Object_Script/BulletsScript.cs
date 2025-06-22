using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction;
    public float lifeTime = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}
