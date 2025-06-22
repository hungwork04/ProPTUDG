
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BossMap
{
    public class UIHealthCtrl : ComponentBehavior
    {
        [SerializeField] private Image healthImg;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (healthImg == null) healthImg = GetComponent<Image>();
        }

        private void OnEnable()
        {
            healthImg.fillAmount = 1;
        }

        public async void OnHPChange(float value)
        {
            healthImg.DOKill();
            
            await healthImg.DOFillAmount(value, .5f).SetEase(Ease.OutCubic).AsyncWaitForCompletion();
        }
    }
}

