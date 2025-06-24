using System.Collections;
using System.Collections.Generic;
using AudioSystem;
using DG.Tweening;
using Game.Defines;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossMap
{
    public class LoseUI : UIView
    {
        [SerializeField] private ScaleBtn exitBtn;
        [SerializeField] private ScaleBtn replayBtn;
        public override void LoadComponent()
        {
            base.LoadComponent();
            ShowAnimation = ViewAnimationType.DipToBlack;
            HideAnimation = ViewAnimationType.DipToBlack;
            Transform buttonHolder = transform.Find("Buttons");
            if (exitBtn == null) exitBtn = buttonHolder.Find("Exit").gameObject.GetOrAdd<ScaleBtn>();
            if (replayBtn == null) replayBtn = buttonHolder.Find("Replay").gameObject.GetOrAdd<ScaleBtn>();
            
            replayBtn.OrNull()?.Init(OnReplayBtnClick);
            exitBtn.OrNull()?.Init(OnExitBtnClick);
        }

        public override void Show()
        {
            ObserverManager<SoundActionType>.Notify(SoundActionType.StopAll);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
        }

        private async void OnReplayBtnClick()
        {
            await UIScreen.HideUI<LoseUI>(true, () =>
            {
                if (GameManager.Instance != null) GameManager.Instance.ReplayGame();
            });
        }

        private void OnExitBtnClick()
        {
            Time.timeScale = 1;
            DOTween.KillAll();
            SceneManager.LoadScene("MainMenu");
        }
    }
}

