using System;
using System.Collections.Generic;
using System.Text;
using WaveTorrentV1.src.Exceptions;

namespace WaveTorrentV1.src.Client.Messages
{
    class Handshake
    {
        private const int protocol = 20;
        private const int reserved = 8;
        private const int hash = 20;
        private const int peer_ids = 20;
        private const int total = protocol + reserved + hash + peer_ids;

        private byte[] complete_message = new byte[total];

        public Handshake(byte[] sha1, string peer_id)
        {
            int counter = 0;
            complete_message[counter] = 19;
            counter++;

            foreach (char chr in "BitTorrent protocol")
            {
                complete_message[counter] = (byte)chr;
                counter++;
            }

            counter += 8;

            if (sha1.Length != 20)
            {
                throw new HandshakeException("length of the info_hash different of 20 bytes");
            }

            foreach (byte byt in sha1)
            {
                complete_message[counter] = byt;
                counter++;
            }

            if (peer_id.Length != 20)
            {
                throw new HandshakeException("length of peer_id different of 20 chars");
            }

            foreach (char chrs in peer_id)
            {
                complete_message[counter] = (byte)chrs;
                counter++;
            }
        }

        public byte[] getMessageBytes()
        {
            return complete_message;
        }
    }
}
