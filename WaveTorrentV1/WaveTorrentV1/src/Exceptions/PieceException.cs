using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Exceptions
{
    class PieceException : Exception
    {
        public PieceException(string message) : base(message)
        {
        }
    }
}
