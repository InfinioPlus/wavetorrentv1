using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class CancelException : Exception
    {
        public CancelException(string message) : base(message)
        {
        }
    }
}
