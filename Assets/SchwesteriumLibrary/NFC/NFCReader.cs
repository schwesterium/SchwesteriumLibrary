/*
Author : schwesterium
Date   : 2026/08/02
*/

using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace SchwesteriumLibrary.NFC
{
    public class NFCReader
    {
        private IntPtr _hContext = IntPtr.Zero;
        IntPtr _hCard = IntPtr.Zero;
        //リーダーは一つだけ
        private NFCAPI.SCARD_READERSTATE[] _readStates = new NFCAPI.SCARD_READERSTATE[1];

        public async UniTask Run(CancellationToken token)
        {
            try
            {
                if (!TryEstablishContext()) { throw new Exception("Establich Context Failed"); }

                if (!TrySelectReader()) { throw new Exception("Select Failed"); }

                await UniTask.WaitForEndOfFrame(token);

                if (!IsReaderPresent()) { throw new Exception("No Card"); }

                if (!TryReadCard()) { throw new Exception("Read Failed"); }

            }
            catch (OperationCanceledException e)
            {
                Debug.LogException(e);
                Release();

                return;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Release();

                return;
            }

            Release();

            return;
        }

        private bool TryEstablishContext()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            //リソースマネージャーコンテキストの確立
            r = NFCAPI.SCardEstablishContext(NFCAPI.SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, out _hContext);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardEstablishContext Failed {r}");
                return false;
            }

            return true;
        }

        private bool TrySelectReader()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            //使用できる全リーダーの文字数を取得
            uint readerCount = 0;
            r = NFCAPI.SCardListReaders(_hContext, null, null, ref readerCount);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardListReaders Failed {r}");
                return false;
            }

            //リーダーの文字列を取得
            byte[] readers = new byte[readerCount * 2]; //マルチバイトなため*2
            r = NFCAPI.SCardListReaders(_hContext, null, readers, ref readerCount);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardListReaders Failed {r}");
                return false;
            }

            List<string> readerNames = Encoding.Unicode.GetString(readers).Split('\0').ToList();

            _readStates[0].szReader = readerNames[0];
            _readStates[0].dwEventState = NFCAPI.SCARD_STATE_UNAWARE;

            //リーダーが機能するか
            r = NFCAPI.SCardGetStatusChange(_hContext, 100, _readStates, (uint)_readStates.Length);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"Check SCardGetStatusChange Failed {r}");
                return false;
            }

            return true;
        }

        private bool IsReaderPresent()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            //3秒間 カードがあるかの判別を行う
            r = NFCAPI.SCardGetStatusChange(_hContext, 3000, _readStates, (uint)_readStates.Length);
            if (r == NFCAPI.SCARD_S_SUCCESS)
            {
                if ((_readStates[0].dwEventState & NFCAPI.SCARD_STATE_PRESENT) == NFCAPI.SCARD_STATE_PRESENT)
                {
                    Debug.Log($"カードあるね");
                }
                else
                {
                    Debug.Log($"カードないね");
                }
            }
            else
            {
                Debug.LogWarning($"Read SCardGetStatusChange Failed {r}");
                return false;
            }

            return true;
        }

        private bool TryReadCard()
        {
            //SCardConnect
            //SCardControl << なくてもよい
            //SCardTransmit
            //SCardDisconnect

            uint pdwActiveProtocol = 0;
            var r = NFCAPI.SCARD_S_SUCCESS;

            r = NFCAPI.SCardConnect(_hContext, _readStates[0].szReader, NFCAPI.SCARD_SHARE_EXCLUSIVE, NFCAPI.SCARD_PROTOCOL_T0 | NFCAPI.SCARD_PROTOCOL_T1, out _hCard, out pdwActiveProtocol);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardConnect Failed {r}");
                return false;
            }

            NFCAPI.SCARD_IO_REQUEST request = new NFCAPI.SCARD_IO_REQUEST();
            request.dwProtocol = pdwActiveProtocol;
            request.cbPciLength = Marshal.SizeOf<NFCAPI.SCARD_IO_REQUEST>();

            byte[] reciveBuffer = new byte[256 * 2];
            uint reciveLength = (uint)reciveBuffer.Length;

            //IDmを取得するAPDUコマンド
            //https://learn.microsoft.com/ja-jp/windows-hardware/drivers/nfc/storage-card-requirements
            byte[] apduCommand = { 0xFF, 0xCA, 0x00, 0x00, 0x00 };

            try
            {
                r = NFCAPI.SCardTransmit(_hCard, ref request, apduCommand, (uint)apduCommand.Length, (IntPtr)null, reciveBuffer, ref reciveLength);
            }
            finally
            {
                Debug.Log("Disconnect");
                Disconnect();
            }

            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardTransmit Failed {r}");
                return false;
            }

            if (reciveLength < 2) { return false; }
            byte sw1 = reciveBuffer[reciveLength - 2];
            byte sw2 = reciveBuffer[reciveLength - 1];
            if (sw1 != 0x90 || sw2 != 0x00)
            {
                Debug.LogWarning($"APDU Error: {sw1:X2}{sw2:X2}");
                return false;
            }

            //受信データからIDmを抽出 
            Debug.Log(BitConverter.ToString(reciveBuffer, 0, (int)reciveLength));

            return true;
        }

        private void Disconnect()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            r = NFCAPI.SCardDisconnect(_hCard, NFCAPI.SCARD_LEAVE_CARD);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardDisconnect Failed {r}");
            }
        }

        private bool Release()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            r = NFCAPI.SCardReleaseContext(_hContext);

            return r == NFCAPI.SCARD_S_SUCCESS;
        }
    }
}