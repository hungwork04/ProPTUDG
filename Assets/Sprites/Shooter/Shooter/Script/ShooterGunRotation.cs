using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterGunRotation : MonoBehaviour
{
 public float minAngle = -60f; // Góc tối thiểu
    public float maxAngle = 60f;  // Góc tối đa
    public Transform playerTransform;
    public Transform aimPos;

    void Update()
    {
        // Lấy vị trí chuột trong không gian thế giới
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Đảm bảo z bằng vị trí của súng

        // Tính hướng từ súng đến chuột
        Vector2 direction = mousePos - aimPos.position;
        // Xác định nhân vật quay phải hay trái
        bool facingRight = playerTransform.localScale.x > 0;

        // Tính góc từ hướng chuột
        //float angle = Mathf.Atan2(direction.y-1.25f, direction.x) * Mathf.Rad2Deg;

        if (facingRight)
        {
            // Quay phải: Giới hạn góc trong [minAngle, maxAngle]
            float angle = Mathf.Atan2(direction.y-1.25f, direction.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, minAngle, maxAngle);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            // Quay trái: Điều chỉnh hướng và góc
            // Đảo ngược hướng x của direction để tính góc trong hệ tọa độ lật
            direction.x = -direction.x;
            float angle = Mathf.Atan2(direction.y-1.25f, direction.x) * Mathf.Rad2Deg;
            // angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, minAngle, maxAngle);
            transform.rotation = Quaternion.Euler(0f, 0, -angle);

            
        }
    }
}