using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerOnStart : MonoBehaviour
{
    public Transform playerSpawnPos;
    GameController gameCtr;
    void Awake()
    {
        gameCtr=FindAnyObjectByType<GameController>();
    }
    void Start()
    {
        if(playerSpawnPos==null||gameCtr==null){
            Debug.Log("null");
            return;
        }
        gameCtr.SpawnPlayer(playerSpawnPos);
    }
}
