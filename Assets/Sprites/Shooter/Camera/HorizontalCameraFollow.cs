using UnityEngine;

public class HorizontalCameraFollow : MonoBehaviour
{
    public Transform player;               // Tham chiếu đến Player
    public float leftLimitX = -5f;         // Giới hạn trái tính từ tâm camera
    public float rightLimitX = 5f;         // Giới hạn phải tính từ tâm camera
    public float followSpeed = 3f;         // Tốc độ camera theo dõi

    private float cameraZ;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player chưa được gán trong CameraFollowX!");
            enabled = false;
            return;
        }

        cameraZ = transform.position.z;
    }

    void LateUpdate()
    {
        Vector3 camPos = transform.position;
        if( player==null||camPos==null) return;
        float deltaX = player.position.x - camPos.x;

        // Nếu player vượt giới hạn trái hoặc phải
        if (deltaX < leftLimitX || deltaX > rightLimitX)
        {
            // Tính vị trí mới để camera hướng về player (giữa màn hình)
            float targetX = player.position.x;
            camPos.x = Mathf.Lerp(camPos.x, targetX, followSpeed * Time.deltaTime);
            transform.position = new Vector3(camPos.x, camPos.y, cameraZ);
        }
    }
}
