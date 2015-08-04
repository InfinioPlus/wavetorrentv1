using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class RequestException : Exception
    {
        public RequestException(string message) : base(message)
        {
        }
    }
}
