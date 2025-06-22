
using System;
using Game.Define;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BossMap
{
    public class TABossMapUI : UIScreen
    {
        [SerializeField] private Button pauseBtn;
        [SerializeField] private UIHealthCtrl playerHealth;
        [SerializeField] private UIHealthCtrl bossHealth;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (playerHealth == null) playerHealth = transform.Find("Player Infor/HealthMask/HealthBar").gameObject.GetOrAdd<UIHealthCtrl>();

            if (bossHealth == null) bossHealth = transform.Find("BossInfor/GameObject/HealthFill").gameObject.GetOrAdd<UIHealthCtrl>();

            if (pauseBtn == null) pauseBtn = transform.Find("PauseBtn").GetComponent<Button>();
            AddUIView<PauseUI>();
            AddUIView<SettingUI>();
            AddUIView<LoseUI>();
        }

        private void OnEnable()
        {
            pauseBtn.OrNull()?.onClick.AddListener(OnPauseBtnClick);
            ObserverManager<GameEventType>.Attach(GameEventType.Lose, OnLose);
        }

        private void OnDisable()
        {
            pauseBtn.OrNull()?.onClick.RemoveAllListeners();
            ObserverManager<GameEventType>.Detach(GameEventType.Lose, OnLose);
        }

        private async void OnPauseBtnClick() => await ShowUI<PauseUI>();

        public void OnPlayerHPChange(float value) => playerHealth.OnHPChange(value);
        public void OnBossHPChange(float value) => bossHealth.OnHPChange(value);
        private async void OnLose(object param) => await ShowUI<LoseUI>();
       
    }

}
