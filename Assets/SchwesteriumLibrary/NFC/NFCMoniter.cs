/*
Author : schwesterium
Date   : 2026/09/10
*/

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SchwesteriumLibrary.NFC
{
    public class NFCMonitor : IDisposable
    {
        private readonly NFCReader _reader = new NFCReader();
        private CancellationTokenSource _cts;

        public event Action<NFCReadData> OnCardRead;
        public event Action OnCardRemoved;
        public event Action<string> OnError;

        public bool Init()
        {
            if (!_reader.TryEstablishContext()) { return false; }
            if (!_reader.TrySelectReader()) { return false; }

            return true;
        }

        public void StartMonitoring()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            MonitorLoopAsync(_cts.Token).Forget();
        }

        public void StopMonitoring()
        {
            Debug.Log("StopMonitoring");
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTaskVoid MonitorLoopAsync(CancellationToken token)
        {
            bool wasPresent = false;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.SwitchToThreadPool();

                    //メインスレッドだと止まるのでスレッドプールで呼ぶ
                    var isChange = _reader.WaitForStatusChange();

                    if (!isChange)
                    {
                        await UniTask.Delay(16, cancellationToken: token);
                        continue;
                    }

                    bool isPresent = _reader.IsCardPresent();

                    if (isPresent && !wasPresent)
                    {
                        wasPresent = true;
                        NFCReadData data = null;

                        await UniTask.SwitchToMainThread(token);

                        if (_reader.TryReadCard(out var readData)) { data = readData; }

                        if (data != null && data.Length > 0) { OnCardRead?.Invoke(data); }
                        else { OnError?.Invoke("カード読み取りに失敗しました"); }

                    }
                    else if (!isPresent && wasPresent)
                    {
                        await UniTask.SwitchToMainThread(token);

                        wasPresent = false;
                        OnCardRemoved?.Invoke();
                    }

                    await UniTask.SwitchToMainThread(token);
                    await UniTask.WaitForEndOfFrame(token);
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
            }
            catch (Exception e)
            {
                OnError?.Invoke("カード読み取りに失敗しました");
                Debug.LogException(e);
            }
        }

        public void Dispose()
        {
            Debug.Log("Dispose");
            StopMonitoring();
            _reader.Disconnect();
            _reader.Release();
        }
    }
}