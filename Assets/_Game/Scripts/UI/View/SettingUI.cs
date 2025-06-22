
using System;
using AudioSystem;
using Game.Defines;
using Game.UI;

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace BossMap
{
    public class SettingUI : UIView
    {
        [SerializeField] private ButtonEffectBase exitGame;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Button exitBtn;
        public override void LoadComponent()
        {
            base.LoadComponent();
            ShowAnimation = ViewAnimationType.DipToBlack;
            HideAnimation = ViewAnimationType.DipToBlack;
            if (exitBtn == null) exitBtn = transform.Find("Exit")?.GetComponent<Button>();
            if (exitGame == null) exitGame = transform.Find("ExitGame")?.GetComponent<ButtonEffectBase>();
            Transform SaMHolder = transform.Find("SoundAndMusic");
            if (soundSlider == null) soundSlider = SaMHolder.Find("Sound/Slider").GetComponent<Slider>();
            if (musicSlider == null) musicSlider = SaMHolder.Find("Music/Slider").GetComponent<Slider>();

            exitGame.OrNull()?.Init(ExitGame);
        }

        private void Start()
        {
            soundSlider.onValueChanged.AddListener(OnSoundValueChange);
            musicSlider.onValueChanged.AddListener(OnMusicValueChange);
        }

        private void OnEnable()
        {
            exitBtn.onClick.AddListener(OnExitBtnClick);
            
        }

        private void OnDisable()
        {
            exitBtn.onClick.RemoveAllListeners();
        }

        public override void Show()
        {
            ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
            if (SoundManager.Instance != null) soundSlider.value = SoundManager.Instance.SoundRate;
            if (MusicManager.Instance != null) musicSlider.value = MusicManager.Instance.Volume;
            base.Show();
        }

      

        public override void Hide()
        {
            base.Hide();
            ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
        }
        private async void OnExitBtnClick() => await UIScreen.HideUI<SettingUI>();

        private void ExitGame()
        {
            #if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
        }

        private void OnMusicValueChange(float value)
        {
            if (MusicManager.Instance != null) MusicManager.Instance.Volume = value;
        }

        private void OnSoundValueChange(float value)
        {
            if (SoundManager.Instance != null) SoundManager.Instance.SoundRate = value;
        }
    }

}
