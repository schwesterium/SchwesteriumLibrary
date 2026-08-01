/*
Author : schwesterium
Date   : 2026/08/01
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SchwesteriumLibrary.Input
{
    public class LocalMultiplayManager<T> : IDisposable where T : MultiPlayInputHandlerBase
    {
        private int _maxPlayers = -1;
        private GameObject[] _playerPrefabs = null;

        private bool _useKeyboard = true;
        private bool _initResister = true;

        private readonly List<T> _activePlayers = new();
        private readonly HashSet<InputDevice> _pairedDevices = new();

        private bool _isReceiving = false;
        public int ActivePlayerCount => _activePlayers.Count;

        public LocalMultiplayManager(int maxPlayers, GameObject[] playerObjs, bool useKeyboard, bool initResister)
        {
            if (maxPlayers != playerObjs.Length) { Debug.LogWarning($"最大プレイヤー数が異なります！ playerObjs {playerObjs.Length}, maxPlayers {maxPlayers}"); }

            _playerPrefabs = playerObjs;
            _maxPlayers = maxPlayers;
            _useKeyboard = useKeyboard;
            _initResister = initResister;
        }

        public void Init()
        {
            //デバイス切断の監視)
            InputSystem.onDeviceChange += OnDeviceChange;

            if (_initResister)
            {
                RegisterAllDevices();
            }
        }

        public void Dispose()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        /// <summary>
        /// 未割当のプレイヤーオブジェクトを非表示にする
        /// </summary>
        /// <param name="v"></param>
        public void SetActiveUnassignedPlayer(bool v)
        {
            for (int i = 0; i < _maxPlayers - _activePlayers.Count; ++i)
            {
                _playerPrefabs[_playerPrefabs.Length - i - 1].SetActive(v);
            }
        }

        public void StartRegisterDevice() => _isReceiving = true;
        public void EndRegisterDevice() => _isReceiving = false;

        /// <summary>
        /// 接続済みのデバイスをすべてプレイヤーに登録する
        /// </summary>
        public void RegisterAllDevices()
        {
            var devices = InputSystem.devices;

            foreach (var device in devices)
            {
                //プレイヤー数の上限チェック
                if (_activePlayers.Count >= _maxPlayers) { return; }

                //既に参加済みのデバイスは無視
                if (_pairedDevices.Contains(device)) { continue; }

                if (IsAcceptableDevice(device))
                {
                    JoinPlayer(device);
                }
            }
        }

        //接続されたデバイスを登録する
        private void RegisterDevice(InputDevice device)
        {
            if (!_isReceiving) { return; }

            //プレイヤー数の上限チェック
            if (_activePlayers.Count >= _maxPlayers) { return; }

            //既に参加済みのデバイスは無視
            if (_pairedDevices.Contains(device)) { return; }

            if (IsAcceptableDevice(device))
            {
                JoinPlayer(device);
            }
        }


        //デバイスとInputHandlerを紐づける
        private void JoinPlayer(InputDevice device)
        {
            var playerId = _activePlayers.Count;

            if (!_playerPrefabs[playerId].TryGetComponent<IInputHandlerOwner<T>>(out var owner))
            {
                Debug.LogError("ownerが見つかりません");
                return;
            }

            //デバイスの紐付け
            var handler = owner.GetInputHandler();

            handler.Join(device, playerId);

            //プレイヤーとデバイスの記録
            _activePlayers.Add(handler);
            _pairedDevices.Add(device);

#if UNITY_EDITOR
            Debug.Log($"Player{++playerId} が参加しました{device.displayName}");
#endif
        }

        private bool IsAcceptableDevice(InputDevice device) => _useKeyboard ? (device is Gamepad || device is Keyboard) : device is Gamepad;

        /// <summary>
        /// デバイスの接続状態が変化したとき（切断対応）
        /// </summary>
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    RegisterDevice(device);
                    break;
                case InputDeviceChange.Reconnected:
                    RegisterDevice(device);
                    break;

                case InputDeviceChange.Disconnected:
                    if (!_pairedDevices.Contains(device)) { return; }

                    //切断されたデバイスのプレイヤーを探す
                    var disconnected = _activePlayers.Find(p => p.ParentDevice == device);

                    if (disconnected == null)
                    {
                        Debug.LogWarning("対応するプレイヤーが見つかりません");
                        return;
                    }

                    disconnected.Leave(device);

                    _pairedDevices.Remove(device);
                    _activePlayers.Remove(disconnected);

#if UNITY_EDITOR
                    Debug.LogWarning($"{device.displayName} が切断されました");
#endif
                    break;
                default:
                    break;
            }
        }
    }
}