using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using WaveTorrentV1.src.Exceptions;

namespace WaveTorrentV1.src.Client.Messages
{
    class Request
    {
        byte[] data;

        public Request(int piece_index, int offset, int length)
        {
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(13));
            data = new byte[4 + 1 + 4 + 4 + 4];

            int counter = 0;

            if (header.Length != 4)
            {
                throw new RequestException("message header different of 4 bytes length");
            }

            foreach (byte byt in header)
            {
                data[counter] = byt;
                counter++;
            }
            data[counter] = 6;
            counter++;

            byte[] index = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(piece_index));

            if(index.Length != 4)
            {
                throw new RequestException("cant convert piece index to 4 byte array");
            }

            foreach (byte byt in index)
            {
                data[counter] = byt;
                counter++;
            }

            byte[] off = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(offset));

            if (off.Length != 4)
            {
                throw new RequestException("cant convert offset to 4 byte array");
            }

            foreach (byte byt in off)
            {
                data[counter] = byt;
                counter++;
            }

            byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(length));

            if (len.Length != 4)
            {
                throw new RequestException("cant convert requested length to 4 byte array");
            }

            foreach (byte byt in len)
            {
                data[counter] = byt;
                counter++;
            }
        }

        public byte[] getMessageBytes()
        {
            return data;
        }
    }
}
