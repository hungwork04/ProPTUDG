using UnityEngine;

public class ShooterGun : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab của đạn
    public Transform firePoint; // Điểm spawn đạn (con của súng)
    public float fireRate = 0.25f; // Tốc độ bắn (giây giữa các phát bắn)
    public Transform playerTransform; // Tham chiếu đến player để lấy facingRight
    private float nextFireTime; // Thời điểm có thể bắn phát tiếp theo
    public bool isShooting=false;// Biến kiểm tra trạng thái bắn

    void Update()
    {
        // Kiểm tra nhấn chuột trái và thời gian bắn
        if (Input.GetMouseButtonDown(0) )
        {
            Shoot();
            nextFireTime = Time.time + fireRate+0.25f;

        }
        if(Time.time <= nextFireTime){
            isShooting = true; // Đặt trạng thái bắn
        }else isShooting=false;
    }

    void Shoot()
    {
        // Xác định hướng nhân vật
        bool facingRight = playerTransform.localScale.x > 0;

        // Tính hướng bắn dựa trên rotation của súng và hướng nhân vật
        Vector2 direction = facingRight ? firePoint.right : -firePoint.right;

        // Spawn đạn tại firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ShooterBullet bulletScript = bullet.GetComponent<ShooterBullet>();
        if (!facingRight)
            bullet.GetComponent<SpriteRenderer>().flipX = true;

        // Đặt vận tốc cho đạn
        bulletScript.SetVelocity(direction);

        // Debug hướng đạn
        Debug.DrawRay(firePoint.position, direction * 2f, Color.red, 1f);
    }
}