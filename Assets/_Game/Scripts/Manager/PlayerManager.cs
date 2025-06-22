
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BossMap
{
    public class PlayerManager : ComponentBehavior
    {
        private GameObject playerPrefab;
        private Vector3 spawnPos = Vector3.zero;
        public GameObject playerGO;

        public async UniTask Init(GameObject playerPrefabValue, Vector3 spawnPosValue)
        {
            playerPrefab = playerPrefabValue;
            spawnPos = spawnPosValue;
            await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
        }

        public async UniTask SpawnPlayer()
        {
            if (playerPrefab != null)
            {
                playerGO = PoolingManager.Spawn(playerPrefab, spawnPos, default, transform);
                await UniTask.Delay(300, DelayType.UnscaledDeltaTime);
            }
        }

        public async UniTask DespawnPlayer()
        {
            if (playerGO != null)
            {
                PoolingManager.Despawn(playerGO);
                await UniTask.Delay(100, DelayType.UnscaledDeltaTime);

            }
        }
    
    }

}
