using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Client.Messages
{
    class Unchoke
    {
        private byte[] data;

        public Unchoke()
        {
            data = new byte[5];
            data[3] = 1;
            data[4] = 1;
        }

        public byte[] getMessageBytes()
        {
            return data;
        }
    }
}
