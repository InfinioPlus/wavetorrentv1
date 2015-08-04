using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WaveTorrentV1.src.Parser;
using WaveTorrentV1.src.BEncode;
using System.Security.Cryptography;

namespace WaveTorrentV1.src.Util
{
    class TorrentReader
    {
        private BEncodeDictionary decodedTorrent;
        private string announceURL;
        private string comment;
        private BEncodeList announceList;
        private string encoding;
        private string info_hash;
        private byte[] info_hash_byte;
        private Int64 content_length;
        private string filename;
        private string created_by;

        public TorrentReader(string archivo)
        {
            FileStream f = new FileStream(archivo, FileMode.Open, FileAccess.Read);
            BEncodeDecoder parser = new BEncodeDecoder();
            decodedTorrent = parser.parse(f);
            comment = ((BEncodeString)decodedTorrent.getValue(new BEncodeString("comment"))).ToString();
            announceURL = ((BEncodeString)decodedTorrent.getValue(new BEncodeString("announce"))).ToString();
            announceList = (BEncodeList)decodedTorrent.getValue(new BEncodeString("announce-list"));
            try
            {
                encoding = ((BEncodeString)decodedTorrent.getValue(new BEncodeString("encoding"))).ToString();
            }
            catch (Exception e)
            {
            }
            string test = ((BEncodeDictionary)decodedTorrent.getValue(new BEncodeString("info"))).ToBencodeDictionary();
            char[] tester = test.ToCharArray();
            byte[] total = new byte[tester.Length];
            int counter = 0;
            foreach(char chars in tester)
            {
                total[counter] = (byte)chars;
                counter += 1;
            }
            info_hash_byte = SHA1CryptoServiceProvider.Create().ComputeHash(total);
            info_hash = BitConverter.ToString(info_hash_byte).Replace("-", "");
            content_length = ((BEncodeInteger)((BEncodeDictionary)decodedTorrent.getValue(new BEncodeString("info"))).getValue(new BEncodeString("length"))).getValue();
            filename = ((BEncodeString)((BEncodeDictionary)decodedTorrent.getValue(new BEncodeString("info"))).getValue(new BEncodeString("name"))).ToString();
            created_by = ((BEncodeString)decodedTorrent.getValue(new BEncodeString("created by"))).ToString();
        }

        public string getInfoHash()
        {
            return info_hash;
        }

        public string getEncoding()
        {
            return encoding;
        }

        public string getComment()
        {
            return comment;
        }

        public string getContentLength()
        {
            return content_length.ToString();
        }

        public string getFileName()
        {
            return filename;
        }

        public string getCreatedBy()
        {
            return created_by;
        }

        public string getAnnounceURL()
        {
            return announceURL;
        }

        public string[] getAnnounceList()
        {
            List<string> values = new List<string>();

            foreach (BEncodeValue value in announceList.getItems())
            {
                if (value is BEncodeList)
                {
                    BEncodeList data = (BEncodeList)value;
                    BEncodeString valuein = (BEncodeString)data.getItems()[0];
                    values.Add(valuein.ToString());
                }
            }

            return values.ToArray();
        }

        public byte[] getInfoHashBytes()
        {
            char[] chars = info_hash.ToCharArray();
            byte[] bytes = new byte[chars.Length];
            int counter = 0;

            foreach (char charss in chars)
            {
                bytes[counter] = (byte)chars[counter];
                counter++;
            }

            return bytes;
        }

        public byte[] getRawInfoHash()
        {
            return info_hash_byte;
        }
    }
}
