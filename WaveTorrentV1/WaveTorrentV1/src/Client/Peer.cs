using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Client
{
    class Peer
    {
        ushort port;
        string ip;
        string peer_id;

        public Peer(string address, ushort por)
        {
            ip = address;
            port = por;
        }

        public Peer(string address, ushort pors, string id)
        {
            ip = address;
            port = pors;
            peer_id = id;
        }

        public string getIPAddress()
        {
            return ip;
        }

        public ushort getPort()
        {
            return port;
        }
    }
}
