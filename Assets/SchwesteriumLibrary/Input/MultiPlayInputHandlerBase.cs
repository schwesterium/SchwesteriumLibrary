/*
Author : schwesterium
Date   : 2026/08/01
*/

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace SchwesteriumLibrary.Input
{
    /// <summary>
    /// ローカルマルチ用 入力ハンドラ基底クラス
    /// </summary>
    public abstract class MultiPlayInputHandlerBase : IDisposable
    {
        private int _playerId = -1;

        private InputUser _inputUser;
        private InputDevice _device = null;
        private InputActionMap _map = null;

        private bool _isJoined = false;

        public int PlayerId { get { return _playerId; } }
        public InputDevice ParentDevice { get { return _device; } }
        public bool IsJoined { get { return _isJoined; } }
        public bool IsActive { get; private set; } = false;

        protected abstract void SetUpAction();
        protected abstract void ReleaseAction();

        /// <summary>
        /// デバイスにハンドラーを割り当てる
        /// </summary>
        /// <param name="device"></param>
        public void Join(InputDevice device, int playerId)
        {
            if (_isJoined)
            {
                Debug.LogWarning($"{{id {playerId}, device {device.name}}} はすでにJoinしています！");
                return;
            }
            _isJoined = true;

            _playerId = playerId;
            _device = device;

            //InputUserを作成してデバイスを割り当て
            _inputUser = InputUser.CreateUserWithoutPairedDevices();
            InputUser.PerformPairingWithDevice(_device, user: _inputUser);

            //ActionMapの作成
            _map = new InputActionMap($"Player{_playerId}");
            SetUpAction();
            
            //ユーザーに割り当て
            _inputUser.AssociateActionsWithUser(_map);

            Enable();
        }

        public void Leave(InputDevice device)
        {
            if (!_isJoined)
            {
                Debug.LogWarning($"{{device {device.name}}} はすでにLeaveしています！");
                return;
            }

            if(device != _device)
            {
                Debug.LogWarning("無効なdevice");
                return;
            }

            _isJoined = false;

            _playerId = -1;

            ReleaseInternal();
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

            _isJoined = false;
        }

        protected void ReleaseInternal()
        {
            if (_map != null)
            {
                _map.Disable();
                _map.Dispose();
                _map = null;
            }

            if (_inputUser.valid) { _inputUser.UnpairDevicesAndRemoveUser(); }
            _device = null;

            IsActive = false;
        }
    }
}