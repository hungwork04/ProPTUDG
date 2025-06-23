using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Game.Define;
using Game.UI;
using UnityEngine;

namespace BossMap
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Player")]
        public List<GameObject> playerPrefabs = new List<GameObject>();

        public Vector3 playerSpawnPos;
        [Header("Boss")] 
        public GameObject bossPrefab;

        public Vector3 bossSpawnPos;
        [Header("Camera")] public CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private GameObject NextLevel;
        
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private BossManager bossManager;
        public bool isPauseGame = false;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (playerManager == null) playerManager = transform.GetComponentInChildren<PlayerManager>();
            if (bossManager == null) bossManager = transform.GetComponentInChildren<BossManager>();
        }

        private async void Start()
        {
            if (UIScreen.Instance != null) await UIScreen.Instance.ShowPanel(0);
            await GameInit();
            PlayGame();
        }

        private void OnEnable()
        {
            ObserverManager<GameEventType>.Attach(GameEventType.Win, OnWin);
        }
        

        private void OnWin(object param)
        {
            if(NextLevel != null) NextLevel.gameObject.SetActive(true);
        }

        private async UniTask GameInit()
        {
            GameObject playerPrefab;
            if (GameController.Instance == null) playerPrefab = playerPrefabs[0];
            else
            {
                int playerIndex = GameController.Instance.playerIndex;
                if (playerIndex < 0 || playerIndex >= playerPrefabs.Count) playerIndex = 0;
                playerPrefab = playerPrefabs[playerIndex];
            }
            await playerManager.Init(playerPrefab, playerSpawnPos);
            await bossManager.Init(bossPrefab, bossSpawnPos);
        }

        public async void PlayGame()
        {
            isPauseGame = true;
            await playerManager.SpawnPlayer();
            if (cinemachineVirtualCamera != null)
            {
                Time.timeScale = 1;
                if (playerManager.playerGO != null)
                {
                    Transform playerGO = playerManager.playerGO.transform;
                    cinemachineVirtualCamera.transform.position = playerGO.position.With(z:-10);
                    cinemachineVirtualCamera.Follow = playerGO;
                    cinemachineVirtualCamera.LookAt = playerGO;

                }

                await UniTask.Delay(500, DelayType.UnscaledDeltaTime);
                Time.timeScale = 0;
            }
            await bossManager.SpawnBoss();

            if (UIScreen.Instance != null) await UIScreen.Instance.HidePanel(1f);
            MusicManager.Instance.PlayMusic(MusicType.TABossMap);
            isPauseGame = false;
        }
        

        public async void ReplayGame()
        {
            isPauseGame = true;
            if (UIScreen.Instance != null) await UIScreen.Instance.ShowPanel(0);
            if(MusicManager.Instance != null) MusicManager.Instance.StopMusic();
            await bossManager.DespawnBoss();

            if (EnemyManager.Instance != null) await EnemyManager.Instance.DespawnAllEnemy();
            await playerManager.DespawnPlayer();
            PlayGame();
        }

        private void OnDisable()
        {
            if(MusicManager.Instance != null) MusicManager.Instance.StopMusic();
            ObserverManager<GameEventType>.Detach(GameEventType.Win, OnWin);
            
        }
    }

}
