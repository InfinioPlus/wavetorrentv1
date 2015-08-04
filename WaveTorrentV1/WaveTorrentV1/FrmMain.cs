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
    public partial class FrmMain : Form
    {
        TorrentReader file;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openDialog.Filter = "torrents (*.torrent)|*.torrent";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                file = new TorrentReader(openDialog.FileName);
                this.txtPath.Text = openDialog.FileName; 
            } 
            if (file != null)
            {
                this.txtName.Text = file.getFileName();
                this.txtComment.Text = file.getComment();
                this.txtHash.Text = file.getInfoHash();
                this.txtEncoding.Text = file.getEncoding();
                this.txtLength.Text = file.getContentLength();
                this.txtCreatedBy.Text = file.getCreatedBy();
                this.txtTracker.Text = file.getAnnounceURL();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Choke testing = new Choke();

            string starturl = file.getAnnounceURL();
            starturl += "?info_hash=" + URLEncoder.URLEncodeHash(file.getInfoHash()) + "&peer_id=-WT0010-135729995916&compact=1&port=6888&event=started";
            HttpWebResponse response = null;
            bool correct = false;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(starturl);
                HttpWebResponse response2 = (HttpWebResponse)request.GetResponse();
                response = response2;
                correct = true;
            }
            catch (Exception exc)
            {
                string[] announceList = file.getAnnounceList();

                foreach (string url in announceList)
                {
                    try
                    {
                        string completeURL = url + "?info_hash=" + URLEncoder.URLEncodeHash(file.getInfoHash()) + "&peer_id=-WT0010-135729995916&port=6888&compact=1&event=started";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(completeURL);
                        HttpWebResponse response2 = (HttpWebResponse)request.GetResponse();
                        response = response2;
                        correct = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            if (correct)
            {
                Stream result = response.GetResponseStream();
                List<byte> bytes = new List<byte>();
                while (true)
                {
                    byte[] readBytes = new byte[255];
                    int readed = result.Read(readBytes, 0, 255);
                    if (readed == 0)
                    {
                        break;
                    }

                    foreach (byte byt in readBytes)
                    {
                        string actual = byt.ToString();
                        if (actual == "0")
                        {
                            break;
                        }
                        bytes.Add(byt);
                    }
                }
                byte[] byt2 = bytes.ToArray();

                string fileName = Application.StartupPath + "\\tmp\\" + file.getInfoHash() + ".txt";

                FileStream newFile = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                newFile.Write(byt2, 0, byt2.Length);
                newFile.Close();
                newFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BEncodeDecoder parser = new BEncodeDecoder();
                BEncodeDictionary dic = parser.parse(newFile);
                BEncodeString value = (BEncodeString)dic.getValue(new BEncodeString("peers"));
                List<Peer> peers = UncompactPeerList.uncompact(value);

                Handshake message1 = new Handshake(file.getRawInfoHash(), "-WT0010-135729995916");
                TcpClient client = new TcpClient();
                client.Connect("195.191.165.4", 40890);
                NetworkStream st = client.GetStream();
                st.Write(message1.getMessageBytes(), 0, message1.getMessageBytes().Length);
                byte[] responses = new byte[1024];
                st.Read(responses, 0, 1024);
                /*byte[] number = BitConverter.GetBytes(19);
                st.Write(number, 0, number.Length); 
                BEncodeString tstc = new BEncodeString("BitTorrent protocol");
                st.Write(tstc.getBytes(), 0, tstc.getBytes().Length);
                byte[] reservedbytes = new byte[8];
                st.Write(reservedbytes, 0, reservedbytes.Length);
                st.Write(file.getInfoHashBytes(), 0, file.getInfoHashBytes().Length);
                tstc = new BEncodeString("-WT0010-135729995916");
                st.Write(tstc.getBytes(), 0, tstc.getBytes().Length);
                byte[] responseb = new byte[1024];
                st.Read(responseb, 0, 1024);*/

                
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            worker.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Application.StartupPath + "\\tmp");
        }



        private static byte[] ReverseBytes(byte[] bytearr)
        {
            byte bttemp;
            int inthighCtr = bytearr.Length - 1;

            for (int i = 0; i < bytearr.Length / 2; i++)
            {
                bttemp = bytearr[i];
                bytearr[i] = bytearr[inthighCtr];
                bytearr[inthighCtr] = bttemp;
                inthighCtr -= 1;
            }
            return bytearr;
        }
    }
}