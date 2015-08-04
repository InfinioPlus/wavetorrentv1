using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class HandshakeException : Exception
    {
        public HandshakeException(string message) : base(message)
        {
        }
    }
}
