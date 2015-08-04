using System;
using System.Collections.Generic;
using System.Text;
using WaveTorrentV1.src.Exceptions;

namespace WaveTorrentV1.src.BEncode
{
    class BEncodeDictionary : BEncodeValue
    {
        private Dictionary<BEncodeString, BEncodeValue> data;

        public BEncodeDictionary()
        {
            data = new Dictionary<BEncodeString, BEncodeValue>();
        }

        public void Add(BEncodeString key, BEncodeString value)
        {
            data.Add(key, value);
        }

        public void Add(BEncodeString key, BEncodeInteger value)
        {
            data.Add(key, value);
        }

        public void Add(BEncodeString key, BEncodeList value)
        {
            data.Add(key, value);
        }

        public void Add(BEncodeString key, BEncodeDictionary value)
        {
            data.Add(key, value);
        }

        public string ToBencodeDictionary()
        {
            string str = "";
            str += "d";

            foreach (KeyValuePair<BEncodeString, BEncodeValue> pair in data)
            {
                str += pair.Key.ToBencodeString();

                if (pair.Value is BEncodeString)
                {
                    BEncodeString value = (BEncodeString)pair.Value;
                    str += value.ToBencodeString();
                }
                else if (pair.Value is BEncodeInteger)
                {
                    BEncodeInteger value = (BEncodeInteger)pair.Value;
                    str += value.ToBencodeInteger();
                }
                else if (pair.Value is BEncodeList)
                {
                    BEncodeList value = (BEncodeList)pair.Value;
                    str += value.ToBencodeList();
                }
                else if (pair.Value is BEncodeDictionary)
                {
                    BEncodeDictionary value = (BEncodeDictionary)pair.Value;
                    str += value.ToBencodeDictionary();
                }
                else
                {
                    throw new BEncodeException("cant recognize value");
                }
            }
            str += "e";
            return str;
        }

        public BEncodeValue getValue(BEncodeString value)
        {
            BEncodeValue result = null;
            foreach (KeyValuePair<BEncodeString, BEncodeValue> pair in data)
            {
                if (pair.Key.ToString() == value.ToString())
                {
                    result = pair.Value;
                }
            }
            return result;
        }
    }
}
