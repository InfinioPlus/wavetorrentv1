using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WaveTorrentV1.src.Util
{
    class Listener
    {
        private bool listening = true;
        private int port = 6888;
        private TcpListener listener;

        public Listener()
        {
            listener = new TcpListener(port);
        }

        public void startToListen()
        {
            listener.Start();
            while (listening)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
            }
            listener.Stop();
        }
    }
}
