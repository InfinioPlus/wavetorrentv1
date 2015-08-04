using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WaveTorrentV1.src.Util;
using System.Net;
using System.IO;
using WaveTorrentV1.src.Parser;
using WaveTorrentV1.src.BEncode;
using WaveTorrentV1.src.Client;
using WaveTorrentV1.src.Client.BEP23;
using System.Net.Sockets;
using WaveTorrentV1.src.Client.Messages;

namespace WaveTorrentV1
{
    public partial class tester : Form
    {
        public tester()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TorrentReader file = new TorrentReader("C:\\test2.torrent");
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 7646);
            Stream st = client.GetStream();
            KeepAlive message2 = new KeepAlive();
            Handshake message1 = new Handshake(file.getRawInfoHash(), "-WT0010-135729995916");
            st.Write(message1.getMessageBytes(), 0, message1.getMessageBytes().Length);
            st.Flush();
            byte[] readed = new byte[1024];
            st.Read(readed, 0, 3);

        }
    }
}