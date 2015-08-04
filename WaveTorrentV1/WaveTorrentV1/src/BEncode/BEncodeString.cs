using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.BEncode
{
    class BEncodeString : BEncodeValue
    {
        private byte[] data;

        public BEncodeString(byte[] value)
        {
            data = value;
        }

        public BEncodeString(string value)
        {
            char[] chars = value.ToCharArray();
            data = new byte[chars.Length];
            int counter = 0;
            foreach (char charss in chars)
            {
                data[counter] = (byte)charss;
                counter += 1;
            }
        }

        public override string ToString()
        {
            string value = "";
            foreach (byte bytes in data)
            {
                value += (char)bytes;
            }
            return value;
        }

        public string ToBencodeString()
        {
            return "" + data.Length + ":" + ToString(); 
        }

        public byte[] getBytes()
        {
            return data;
        }
    }
}
