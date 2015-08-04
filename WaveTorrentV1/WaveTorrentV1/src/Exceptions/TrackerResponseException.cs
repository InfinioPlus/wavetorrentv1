using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class TrackerResponseException : Exception
    {
        public TrackerResponseException(string message) : base(message)
        {
        }
    }
}
