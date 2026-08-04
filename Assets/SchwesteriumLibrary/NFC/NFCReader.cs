/*
Author : schwesterium
Date   : 2026/08/02
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SchwesteriumLibrary.NFC
{
    [Serializable]
    public sealed class NFCReader
    {
        private IntPtr _hContext = IntPtr.Zero;
        private IntPtr _hCard = IntPtr.Zero;
        //リーダーは一つだけ
        private NFCAPI.SCARD_READERSTATE[] _readStates = new NFCAPI.SCARD_READERSTATE[1];

        public bool TryEstablishContext()
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

        public bool TrySelectReader()
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

        public bool IsReaderPresent()
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

        public bool TryReadCard(out byte[] data)
        {
            //SCardConnect
            //SCardControl << なくてもよい
            //SCardTransmit
            //SCardDisconnect

            data = Array.Empty<byte>();

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

            List<byte> pagedata = new List<byte>();


            for (int i = 4; i < 44; i += 4)
            {
                byte[] apduCommand = { 0xFF, 0xB0, 0x00, (byte)i, 0x10 };

                r = NFCAPI.SCardTransmit(_hCard, ref request, apduCommand, (uint)apduCommand.Length, (IntPtr)null, reciveBuffer, ref reciveLength);
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

                for (int j = 0; j < 16; j += 4)
                {
                    pagedata.Add(reciveBuffer[j]);
                    pagedata.Add(reciveBuffer[j + 1]);
                    pagedata.Add(reciveBuffer[j + 2]);
                    pagedata.Add(reciveBuffer[j + 3]);
                }


                //受信データからIDmを抽出 
                Debug.Log($"{i} page : {BitConverter.ToString(reciveBuffer, 0, (int)reciveLength - 2)}");
            }

            data = pagedata.ToArray();

            Disconnect();

            return true;
        }



        public void Disconnect()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            r = NFCAPI.SCardDisconnect(_hCard, NFCAPI.SCARD_LEAVE_CARD);
            if (r != NFCAPI.SCARD_S_SUCCESS)
            {
                Debug.LogWarning($"SCardDisconnect Failed {r}");
            }
        }

        public bool Release()
        {
            var r = NFCAPI.SCARD_S_SUCCESS;

            r = NFCAPI.SCardReleaseContext(_hContext);

            return r == NFCAPI.SCARD_S_SUCCESS;
        }
    }
}