using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using WaveTorrentV1.src.Exceptions;

namespace WaveTorrentV1.src.Client.Messages
{
    class Piece
    {
        byte[] data;
        byte[] block;

        public Piece(int piece_index, int offset, int block_length)
        {
            data = new byte[4 + 1 + 4 + 4 + block_length];
            block = new byte[block_length];

            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(9 + block_length));

            int counter = 0;

            if (header.Length != 4)
            {
                throw new PieceException("cant convert header to 4 byte array");
            }

            foreach (byte byt in header)
            {
                data[counter] = byt;
                counter++;
            }

            data[counter] = 7;
            counter++;

            byte[] index = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(piece_index));

            if (index.Length != 4)
            {
                throw new PieceException("cant convert piece_index to 4 byte array");
            }

            foreach (byte byt in index)
            {
                data[counter] = byt;
                counter++;
            }

            byte[] off = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(offset));

            if (off.Length != 4)
            {
                throw new PieceException("cant convert offset to 4 byte array");
            }

            foreach (byte byt in off)
            {
                data[counter] = byt;
                counter++;
            }

            foreach (byte byt in block)
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
