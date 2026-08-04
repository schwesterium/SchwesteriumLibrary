/*
Author : schwesterium
Date   : 2026/08/04
*/

using System;

namespace SchwesteriumLibrary.LibSystem
{
    public class NDEFParseException : Exception
    {
        public NDEFParseException() { }

        public NDEFParseException(string message) : base(message) { }

        public NDEFParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}