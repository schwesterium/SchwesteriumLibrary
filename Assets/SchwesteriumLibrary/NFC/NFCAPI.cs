/*
Author : schwesterium
Date   : 2026/08/02
*/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SchwesteriumLibrary.NFC
{
    //参考 https://tomosoft.jp/design/?p=5543
    //https://learn.microsoft.com/ja-jp/windows/win32/api/winscard/
    //https://learn.microsoft.com/ja-jp/windows/win32/winprog/windows-data-types#long
    //https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
    //https://www.softech.co.jp/mm_240605_tr.htm
    //https://qiita.com/shulmj_/items/e890db029e5dbf984cbd

    public static class NFCAPI
    {
        [DllImport("winscard.dll")]
        public static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, ref IntPtr phContext);

        [DllImport("winscard.dll", EntryPoint = "SCardGetStatusChangeW", CharSet = CharSet.Unicode)]
        public static extern int SCardGetStatusChange(IntPtr hContext, uint dwTimeout, ref IntPtr rgReaderStates, uint cReaders);

        [DllImport("winscard.dll", EntryPoint = "SCardListReadersW", CharSet = CharSet.Unicode)]
        public static extern int SCardListReaders(IntPtr hContext, byte[] mszGroups, ref byte[] mszReaders, ref UIntPtr pcchReaders);

        [DllImport("winscard.dll")]
        public static extern int SCardReleaseContext(IntPtr hContext);

    }
}