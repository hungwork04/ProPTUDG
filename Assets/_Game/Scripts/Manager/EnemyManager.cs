
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BossMap
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        public List<GameObject> EnemyHolders = new List<GameObject>();

        public void AddEnemy(GameObject enemy)
        {
            if (enemy != null)
            {
                EnemyHolders.Add(enemy);
                enemy.transform.SetParent(transform);
            }
        }

        public async UniTask DespawnAllEnemy()
        {
            List<UniTask> uniTasks = new List<UniTask>();
            for (int i = EnemyHolders.Count - 1; i >= 0; --i)
            {
                GameObject enemy = EnemyHolders[i];
                if(enemy == null) continue;
                uniTasks.Add(DespawnEnemy(enemy));
            }

            await UniTask.WhenAll(uniTasks);
        }
        public async UniTask DespawnEnemy(GameObject enemy)
        {
            if (enemy != null)
            {
                RemoveEnemy(enemy);
                PoolingManager.Despawn(enemy);
                await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
            }
        }

        public void RemoveEnemy(GameObject enemy) => EnemyHolders.Remove(enemy);
    }
}

