using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.BEncode
{
    class BEncodeInteger : BEncodeValue
    {
        private Int64 data;

        public BEncodeInteger(long value)
        {
            data = value;
        }

        public override string ToString()
        {
            return data.ToString();
        }

        public string ToBencodeInteger()
        {
            return "i" + data.ToString() + "e";
        }

        public Int64 getValue()
        {
            return data;
        }
    }
}
