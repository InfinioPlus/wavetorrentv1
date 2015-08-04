using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class TorrentException : Exception
    {
        public TorrentException(string message) : base(message)
        {
        }
    }
}
