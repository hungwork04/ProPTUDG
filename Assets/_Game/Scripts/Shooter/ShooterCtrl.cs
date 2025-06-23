
using AudioSystem;
using DG.Tweening;
using Game.Define;
using Game.UI;
using UnityEngine;

namespace BossMap
{
    public class ShooterCtrl : ComponentBehavior
    {
        [SerializeField] private SoundData hurtSoundData;
        [SerializeField] private HealthSystem healthSystem;
        
        public float maxHP;
        public float curHP;
        public SpriteRenderer head;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (healthSystem == null) healthSystem = gameObject.GetOrAdd<HealthSystem>();
           
        }

        private void OnEnable()
        {
            maxHP = 500;
            curHP = 500;
            if (healthSystem != null)
            {
                healthSystem.Init(maxHP);
                healthSystem.OnHPChange = OnHPChange;
                healthSystem.OnDead = OnDead;
                TABossMapUI taBossMapUI = UIScreen.Instance as TABossMapUI;
                if (taBossMapUI != null) taBossMapUI.OnPlayerHPChange(healthSystem.curHP * 1f / healthSystem.maxHP);
                
            }
        }

        private async void OnHPChange()
        {
            if (!gameObject.activeInHierarchy) return;
            if(hurtSoundData != null && SoundManager.Instance != null) SoundManager.Instance.CreateSound().WithSoundData(hurtSoundData).WithPosition(transform.position).WithRandomPitch().Play();
            curHP = healthSystem.curHP;
         
            TABossMapUI taBossMapUI = UIScreen.Instance as TABossMapUI;
            if (taBossMapUI != null) taBossMapUI.OnPlayerHPChange(healthSystem.curHP * 1f / healthSystem.maxHP);
            head.DOKill();
            Sequence seq = DOTween.Sequence();

            seq.Append(head.DOColor(Color.red, .3f))
                .Append(head.DOColor(Color.white, .3f));
            await seq.AsyncWaitForCompletion();
        }

        private void OnDead()
        {
            ObserverManager<GameEventType>.Notify(GameEventType.Lose);
            transform.DOKill();
            head.DOKill();
            PoolingManager.Despawn(gameObject);
        }
    }

}
