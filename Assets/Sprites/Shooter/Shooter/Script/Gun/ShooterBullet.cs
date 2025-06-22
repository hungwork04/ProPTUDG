using UnityEngine;

public class ShooterBullet : MonoBehaviour
{
    public float speed = 15f; // Tốc độ di chuyển của đạn
    public float lifetime = 2f; // Thời gian tồn tại của đạn (giây)
    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Hủy đạn sau thời gian lifetime để tránh rò rỉ bộ nhớ
        Destroy(gameObject, lifetime);
    }

    // Gọi từ ShooterGun để set vận tốc cho đạn
    public void SetVelocity(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy")||collision.gameObject.CompareTag("Boss"))
        {
            if(collision.gameObject.CompareTag("Enemy")){
                collision.GetComponent<Health>().TakeDamage(1);
            }
            if(collision.gameObject.CompareTag("Boss")){
                collision.GetComponent<BossMovement>().makeHitted();
                collision.GetComponent<BossHealth>().takeDame(1);
                Debug.Log("bs");

            }
            Destroy(gameObject);
        }
    }
}