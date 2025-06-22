using UnityEngine;

public class ShooterGun : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab của đạn
    public Transform firePoint; // Điểm spawn đạn (con của súng)
    public float fireRate = 0.25f; // Tốc độ bắn (giây giữa các phát bắn)
    public Transform playerTransform; // Tham chiếu đến player để lấy facingRight
    private float nextFireTime; // Thời điểm có thể bắn phát tiếp theo
    public bool isShooting=false;// Biến kiểm tra trạng thái bắn
    private ShooterMovement _shooterMovement;

    // Hàm kiểm tra xem con trỏ chuột đang ở bên trái hay phải player
    bool IsMouseLeftOfPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition.x < playerTransform.position.x;
    }

    void Update()
    {
        if (_shooterMovement == null)
        {
            _shooterMovement = playerTransform.GetComponent<ShooterMovement>();
        }

        // Kiểm tra nhấn chuột trái và thời gian bắn
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            nextFireTime = Time.time + fireRate + 0.1f;
        }

        if (Time.time <= nextFireTime)
        {
            isShooting = true; // Đặt trạng thái bắn
        }
        else
        {
            isShooting = false;
        }
    }

    void Shoot()
    {
        // Xác định vị trí chuột
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Xác định hướng nhân vật dựa trên vị trí chuột
        bool facingRight = mousePosition.x > playerTransform.position.x;

        // Lật nhân vật nếu cần
        if (IsMouseLeftOfPlayer() && _shooterMovement.FacingRight)
        {
            _shooterMovement.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(!IsMouseLeftOfPlayer() && !_shooterMovement.FacingRight){
            _shooterMovement.transform.localScale = new Vector3(1, 1, 1);

        }

        // Tính hướng bắn từ firePoint đến chuột
        Vector2 direction = playerTransform.localScale.x > 0 ? firePoint.right : -firePoint.right;

        // Spawn đạn tại firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ShooterBullet bulletScript = bullet.GetComponent<ShooterBullet>();
        if (!facingRight)
            bullet.GetComponent<SpriteRenderer>().flipX = true;

        // Đặt vận tốc cho đạn
        bulletScript.SetVelocity(direction.normalized);

        // Debug hướng đạn
        Debug.DrawRay(firePoint.position, direction.normalized * 2f, Color.red, 1f);
    }
}
