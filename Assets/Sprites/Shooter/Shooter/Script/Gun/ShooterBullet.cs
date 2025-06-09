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

    void OnTriggerEnter2D(Collider2D collision)
    {
                // Xử lý va chạm (ví dụ: hủy đạn khi chạm vào Ground hoặc Enemy)
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}