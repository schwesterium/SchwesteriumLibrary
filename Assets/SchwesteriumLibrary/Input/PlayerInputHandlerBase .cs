/*
Author : schwesterium
Date   : 2026/08/01
*/

using UnityEngine.InputSystem;

namespace SchwesteriumLibrary.Input
{
    /// <summary>
    /// シングルプレイ用 入力ハンドラ基底クラス
    /// </summary>
    public abstract class PlayerInputHandlerBase
    {
        private InputActionMap _map = null;

        public bool IsActive { get; private set; } = false;

        protected abstract void SetUpAction();
        protected abstract void ReleaseAction();

        public void Init()
        {
            //ActionMapの作成
            _map = new InputActionMap("Player");
            SetUpAction();

            Enable();
        }

        public void Enable()
        {
            IsActive = true;
            _map?.Enable();
        }
        public void Disable()
        {
            IsActive = false;
            _map?.Disable();
        }

        public void Dispose()
        {
            ReleaseInternal();

            ReleaseAction();
        }

        protected void ReleaseInternal()
        {
            if (_map != null)
            {
                _map.Disable();
                _map.Dispose();
                _map = null;
            }

            IsActive = false;
        }
    }
}