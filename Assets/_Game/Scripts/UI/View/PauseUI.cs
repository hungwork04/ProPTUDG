using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioSystem;
using Cysharp.Threading.Tasks;
using Game.Defines;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BossMap
{
    public class PauseUI : UIView
    {
        [SerializeField] private ButtonEffectBase continueBtn;
        [SerializeField] private ButtonEffectBase replayBtn;
        [SerializeField] private ButtonEffectBase settingBtn;
        [SerializeField] private Button exitBtn;
        public override void LoadComponent()
        {
            base.LoadComponent();
            ShowAnimation = ViewAnimationType.PopZoom;
            HideAnimation = ViewAnimationType.PopZoom;

            Transform buttonsHolder = transform.Find("Buttons");
            if (continueBtn == null) continueBtn = buttonsHolder.Find("Continue").gameObject.GetOrAdd<ButtonEffectBase>();
            if (replayBtn == null) replayBtn = buttonsHolder.Find("Replay").gameObject.GetOrAdd<ButtonEffectBase>();
            if (settingBtn == null) settingBtn = buttonsHolder.Find("Setting").gameObject.GetOrAdd<ButtonEffectBase>();

            if (exitBtn == null) exitBtn = transform.Find("Exit").GetComponent<Button>();
            
            continueBtn.OrNull()?.Init(OnExitBtnClick);
            settingBtn.OrNull()?.Init(OnSettingBtnClick);
            replayBtn.OrNull()?.Init(OnReplayBtnClick);
        }

        private void OnEnable()
        {
            exitBtn.onClick.AddListener(OnExitBtnClick);
        }

        private void OnDisable()
        {
            exitBtn.onClick.RemoveAllListeners();
        }

        private async void OnExitBtnClick() => await UIScreen.HideUI<PauseUI>();
        private async void OnSettingBtnClick()
        {
            await UIScreen.HideUI<PauseUI>(true);
            await UIScreen.ShowUI<SettingUI>();
            
        }

        private async void OnReplayBtnClick()
        {
            await UIScreen.HideUI<PauseUI>(true, () =>
            {
                if(GameManager.Instance != null) GameManager.Instance.ReplayGame();
            });
        }
        public override void Show()
        {
            ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
        }
    }

}
