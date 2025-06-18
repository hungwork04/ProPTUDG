using UnityEngine;

public class HorizontalCameraFollow : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;        // Tốc độ di chuyển camera
    public float leftLimit = -20f;
    public float rightLimit = 100f;
    [Range(0.1f, 0.45f)]
    public float zonePercent = 0.3f;    // Tỷ lệ vùng trái/phải tính theo chiều rộng màn hình

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float camX = transform.position.x;

        // Xác định các biên chia vùng trái - giữa - phải
        float leftZoneEdge = camX - camHalfWidth + camHalfWidth * 2f * zonePercent;
        float rightZoneEdge = camX + camHalfWidth - camHalfWidth * 2f * zonePercent;

        float targetX = camX;

        if (target.position.x < leftZoneEdge)
        {
            // Di chuyển về phía nhân vật nếu nằm trong vùng trái
            targetX = target.position.x;
        }
        else if (target.position.x > rightZoneEdge)
        {
            // Di chuyển về phía nhân vật nếu nằm trong vùng phải
            targetX = target.position.x;
        }

        // Giữ camera trong giới hạn bản đồ
        float camHalf = camHalfWidth;
        targetX = Mathf.Clamp(targetX, leftLimit + camHalf, rightLimit - camHalf);

        // Di chuyển dần dần theo tốc độ moveSpeed
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, fixedY, transform.position.z);
    }
}
