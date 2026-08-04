/*
Author : schwesterium
Date   : 2026/08/04
*/

using System;

namespace SchwesteriumLibrary.LibSystem
{
    public class NDEFReadException : Exception
    {
        public NDEFReadException() { }

        public NDEFReadException(string message) : base(message) { }

        public NDEFReadException(string message, Exception innerException) : base(message, innerException) { }
    }
}