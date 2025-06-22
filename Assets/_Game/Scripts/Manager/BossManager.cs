
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace  BossMap
{
    public class BossManager : ComponentBehavior
    {
        private GameObject bossPrefab;
        private Vector3 spawnPos = Vector3.zero;
        private GameObject bossGO;

        public async UniTask Init(GameObject bossPrefabValue, Vector3 spawnPosvalue)
        {
            bossPrefab = bossPrefabValue;
            spawnPos = spawnPosvalue;
            await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
        }
    
       

        public async UniTask SpawnBoss()
        {
            if (bossPrefab != null)
            {
                bossGO = PoolingManager.Spawn(bossPrefab, spawnPos, default, transform);
                await UniTask.Delay(300, DelayType.UnscaledDeltaTime);
            }
        
        }

        public async UniTask DespawnBoss()
        {
            if (bossGO != null)
            {
                PoolingManager.Despawn(bossGO);
                await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
            }
        }

    }

}
