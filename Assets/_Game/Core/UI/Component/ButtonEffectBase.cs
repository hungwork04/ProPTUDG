using System;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class ButtonEffectBase : ComponentBehavior
    {
        [SerializeField] private Button btn;
        private Action onClick = delegate{};
        private Action beforeBtnClick = delegate {  };

        public bool Interacable
        {
            set => btn.interactable = value;
        }
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (btn == null) btn = GetComponent<Button>();
        }

        public void Init(Action onClickAction, Action beforeClick = null)
        {
            beforeBtnClick = beforeClick;
            onClick = onClickAction;
        }

        private async void OnButtonClick()
        {
            beforeBtnClick?.Invoke();
            await RunEffect();
            onClick?.Invoke();
        }
        

        protected abstract UniTask RunEffect();
        private void OnEnable()
        {
            btn.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}
