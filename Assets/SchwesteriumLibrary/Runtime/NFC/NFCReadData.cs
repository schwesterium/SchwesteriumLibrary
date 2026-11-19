/*
Author : schwesterium
Date   : 2026/08/10
*/

using System;

namespace SchwesteriumLibrary.NFC
{
    [Serializable]
    public class NFCReadData
    {
        public byte[] Data = null;
        public uint Length = 0;

        public NFCReadData() { }

        public NFCReadData(byte[] data, uint length)
        {
            Data = data;
            Length = length;
        }
    }
}