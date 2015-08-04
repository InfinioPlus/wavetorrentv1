using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace WaveTorrentV1.src.Client.Messages
{
    class KeepAlive
    {
        private int length = 0;
        private byte[] data; 

        public KeepAlive()
        {
            data = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length));
        }

        public byte[] getMessageBytes()
        {
            return data;
        }
    }
}
