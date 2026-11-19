/*
Author : schwesterium
Date   : 2026/08/04
*/

using SchwesteriumLibrary.LibSystem;
using System;
using UnityEngine;

namespace SchwesteriumLibrary.NFC.NDEF
{
    public static class NDEFHelper
    {
        //NDEFヘッダーのフラグビット

        public const byte MB_FLAG = 0b10000000;
        public const byte ME_FLAG = 0b01000000;
        public const byte CF_FLAG = 0b00100000;
        public const byte SR_FLAG = 0b00010000;
        public const byte IL_FLAG = 0b00001000;
        public const byte TNF_FLAG = 0b00000111;

        /// <summary>
        /// 0~2ビット目に判定をとる
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static TypeNameFormat GetTNF(byte flags)
        {
            switch (flags & TNF_FLAG)
            {
                case 0x00: return TypeNameFormat.Empty;
                case 0x01: return TypeNameFormat.WellKnown;
                case 0x02: return TypeNameFormat.MediaType;
                case 0x03: return TypeNameFormat.AbsoluteURI;
                case 0x04: return TypeNameFormat.ExternalType;
                case 0x05: return TypeNameFormat.Unknown;
                case 0x06: return TypeNameFormat.Unchanged;
                case 0x07: return TypeNameFormat.Reserved;

                default:
                    Debug.LogWarning($"{flags & TNF_FLAG} はTNFにありません");
                    return TypeNameFormat.Empty;
            }
        }

        public static void SetTNF(ref byte flags, TypeNameFormat tnf)
        {
            flags |= (byte)tnf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">NFC Forum Type2</param>
        /// <param name="startIndex">読みたいTLV領域のTの位置</param>
        /// <returns></returns>
        public static NDEFRecord GetNDEFRecord(byte[] data, int startIndex)
        {
            try
            {
                int index = startIndex;

                //Type
                if (data[index++] != 0x03)
                {
                    throw new NDEFReadException("Start index is not 0x03");
                }

                //Length
                int length = data[index++];

                //拡張フォーマット
                //0xFFの後に2バイトのLengthがあり、これはビッグエンディアンで読み取る
                if (length == 0xFF)
                {
                    length = data[index] << 8 | data[index + 1];
                    index += 2;
                }

                //Value
                return new NDEFRecord(data[index..(index + length)]);
            }
            catch (NDEFReadException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new NDEFReadException("data is not long enough", e);
            }
        }
    }
}