using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Client.Messages
{
    class NotInterested
    {
        private byte[] data;

        public NotInterested()
        {
            data = new byte[5];
            data[3] = 1;
            data[4] = 3;
        }

        public byte[] getMessageBytes()
        {
            return data;
        }
    }
}
