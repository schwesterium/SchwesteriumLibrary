/*
Author : schwesterium
Date   : 2026/08/04
*/

using SchwesteriumLibrary.LibSystem;
using System;

namespace SchwesteriumLibrary.NFC.NDEF
{
    [Serializable]
    public class NDEFRecordHeader
    {
        //MB ME CF SR IL TNF(3byte)
        private byte _flags = 0x00;

        private byte _typeLength = 0x00;

        private byte[] _payloadLengthByte = Array.Empty<byte>();

        private byte _idLength = 0x00;

        private byte[] _type = Array.Empty<byte>();

        private byte[] _id = Array.Empty<byte>();

        public byte Flags { get => _flags; }
        public int TypeLength { get => _typeLength; }
        /// <summary>
        /// Headerを除くPayloadの長さ
        /// </summary>
        public int PayloadLength { get; private set; } = 0;//NDEFRecordと同じ
        public int IdLength{ get => _idLength; }
        public byte[] Type { get => _type; }
        public byte[] Id { get => _id; }

        public int HeaderLength { get; private set; } = 0;
        public int RecordLength { get { return PayloadLength + HeaderLength; } }


        public NDEFRecordHeader() { }

        public NDEFRecordHeader(byte[] data)
        {
            if (data == null || data.Length < 2) { throw new NDEFParseException("data length is too short"); }

            var index = 0;

            _flags = data[index++];//0
            _typeLength = data[index++];//1

            var payloadLengthIndex = ((_flags & NDEFHelper.SR_FLAG) != 0) ? 1 : 4;
            if (data.Length < index + payloadLengthIndex) { throw new NDEFParseException("Payload Length : data length is not long enough"); }

            _payloadLengthByte = data[index..(index + payloadLengthIndex)];

            index += _payloadLengthByte.Length;

            if ((_flags & NDEFHelper.IL_FLAG) != 0)
            {
                if (data.Length < index + 1) { throw new NDEFParseException("Id Length : data length is not long enough"); }
                _idLength = data[index++];//3 or 6
            }

            if (data.Length < index + _typeLength) { throw new NDEFParseException("Type : data length is not long enough"); }
            _type = data[index..(index + _typeLength)];//(3 or 6) ~ (3 or 6) + _typeLength - 1

            index += _type.Length;


            if ((_flags & NDEFHelper.IL_FLAG) != 0)
            {
                if (data.Length < index + _idLength) { throw new NDEFParseException("Id : data length is not long enough"); }
                _id = data[index..(index + _idLength)];//3 or 6

                index += _idLength;
            }

            //Payload Lengthはビッグエンディアンとして解釈する
            PayloadLength = ((_flags & NDEFHelper.SR_FLAG) != 0)
                ? _payloadLengthByte[0]
                : (int)((uint)(_payloadLengthByte[0] << 24 | _payloadLengthByte[1] << 16 | _payloadLengthByte[2] << 8 | _payloadLengthByte[3]));

            HeaderLength = index;
        }
    }
}