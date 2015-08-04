using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Client.Messages
{
    class Interested
    {
        private byte[] data;

        public Interested()
        {
            data = new byte[5];
            data[3] = 1;
            data[4] = 2;
        }

        public byte[] getMessageBytes()
        {
            return data;
        }
    }
}
