/*
Author : schwesterium
Date   : 2026/08/04
*/

using System;
using UnityEngine;

namespace SchwesteriumLibrary.NFC.NDEF
{
    public enum TypeNameFormat : byte
    {
        Empty        = 0x00,
        WellKnown    = 0x01,
        MediaType    = 0x02,
        AbsoluteURI  = 0x03,
        ExternalType = 0x04,
        Unknown      = 0x05,
        Unchanged    = 0x06,
        Reserved     = 0x07,
    }
}