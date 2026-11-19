/*
Author : schwesterium
Date   : 2026/08/03
*/

using System;
using SchwesteriumLibrary.LibSystem;

namespace SchwesteriumLibrary.NFC.NDEF
{
    [Serializable]
    public class NDEFRecord
    {
        private NDEFRecordHeader _header;

        private byte[] _payload = Array.Empty<byte>();

        public NDEFRecordHeader Header { get => _header; }
        public byte[] Payload { get => _payload; }
        public int PayloadLength { get => _payload.Length; }//NDEFRecordHeaderと同じ

        public NDEFRecord() { }

        public NDEFRecord(byte[] data)
        {
            _header = new(data);

            if (data.Length < _header.HeaderLength) { throw new NDEFParseException("Payload : data length is not long enough"); }
            _payload = data[_header.HeaderLength.._header.RecordLength];
        }
    }
}