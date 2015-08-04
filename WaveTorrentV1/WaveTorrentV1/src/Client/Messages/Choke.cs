using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Client.Messages
{
    class Choke
    {
        private byte[] data;

        public Choke()
        {
            data = new byte[5];
            data[3] = 1;
            data[4] = 0;
        }

        public byte[] getMessageBytes()
        {
            return data;
        }
    }
}
