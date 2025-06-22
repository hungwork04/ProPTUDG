using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoss : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float fixedY = 5f;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public float minX = 0;
    public float maxX = 50f;

    [Header("Zoom Settings")]
    public Camera cam;
    public float zoomLimiter = 5f; // Càng nhỏ thì zoom càng mạnh khi 2 player xa nhau
    public float minZoom = 5f;
    public float maxZoom = 10f;

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        Vector2 pos1 = player1.position;
        Vector2 pos2 = player2.position;

        // Tính điểm giữa
        float midX = (pos1.x + pos2.x) / 2f;
        midX = Mathf.Clamp(midX, minX, maxX);

        // Giữ nguyên Y, Z
        Vector3 targetPosition = new Vector3(midX, fixedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Zoom camera theo khoảng cách 2 người chơi
        float distanceX = Mathf.Abs(pos1.x - pos2.x);
        float desiredZoom = Mathf.Lerp(minZoom, maxZoom, distanceX / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredZoom, Time.deltaTime * 3f); // zoom mượt
    }
}
