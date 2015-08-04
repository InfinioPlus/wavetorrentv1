using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class BEncodeException : Exception
    {
        public BEncodeException(string message) : base(message)
        {
        }
    }
}
