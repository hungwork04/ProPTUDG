using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton Instance
    public static GameController Instance { get; private set; }

    public int playerIndex = -1;
    public List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        // Đảm bảo chỉ có 1 instance duy nhất
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Xóa bản dư thừa
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại khi load scene mới (nếu cần)
    }

    public void SpawnPlayer(Transform pos)
    {
        if (playerIndex < 0 || playerIndex >= players.Count)
            playerIndex = 0;

        Instantiate(players[playerIndex], pos.position, pos.rotation);
    }
}
