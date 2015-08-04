using System;
using System.Collections.Generic;
using System.Text;
using WaveTorrentV1.src.Exceptions;

namespace WaveTorrentV1.src.BEncode
{
    class BEncodeList : BEncodeValue
    {
        private List<BEncodeValue> data;

        public BEncodeList()
        {
            data = new List<BEncodeValue>();
        }

        public void Add(BEncodeString value)
        {
            data.Add(value);
        }

        public void Add(BEncodeInteger value)
        {
            data.Add(value);
        }

        public void Add(BEncodeList value)
        {
            data.Add(value);
        }

        public void Add(BEncodeDictionary value)
        {
            data.Add(value);
        }

        public string ToBencodeList()
        {
            string str = "";
            str += "l";

            foreach (BEncodeValue item in data)
            {
                if (item is BEncodeString)
                {
                    BEncodeString value = (BEncodeString)item;
                    str += value.ToBencodeString();
                }
                else if (item is BEncodeInteger)
                {
                    BEncodeInteger value = (BEncodeInteger)item;
                    str += value.ToBencodeInteger();
                }
                else if (item is BEncodeList)
                {
                    BEncodeList value = (BEncodeList)item;
                    str += value.ToBencodeList();
                }
                else if (item is BEncodeDictionary)
                {
                    BEncodeDictionary value = (BEncodeDictionary)item;
                    str += value.ToBencodeDictionary();
                }
                else
                {
                    throw new BEncodeException("cant recognize type in list");
                }
            }
            str += "e";
            return str;
        }

        public List<BEncodeValue> getItems()
        {
            return data;
        }
    }
}
