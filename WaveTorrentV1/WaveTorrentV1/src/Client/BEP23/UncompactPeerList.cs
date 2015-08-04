using System;
using System.Collections.Generic;
using System.Text;
using WaveTorrentV1.src.BEncode;
using WaveTorrentV1.src.Exceptions;
using WaveTorrentV1.src.Client;
using WaveTorrentV1.src.Util;
using System.Net;

namespace WaveTorrentV1.src.Client.BEP23
{
    class UncompactPeerList
    {
        public static List<Peer> uncompact(BEncodeString list)
        {
            List<Peer> result = new List<Peer>();
            byte[] bytes = list.getBytes();
            if (bytes.Length % 6 == 0)
            {
                int counter = 0;
                int index = 0;
                byte[] actualPeer = new byte[6];
                bool final = false;

                while (true)
                {
                    if (counter < bytes.Length)
                    {
                        if ((counter % 6 == 0 && counter != 0) || final)
                        {
                            index = 0;
                            int i = 3;
                            byte[] byteIp = new byte[4];
                            byte[] bytePort = new byte[2];

                            byteIp[0] = actualPeer[3];
                            byteIp[1] = actualPeer[2];
                            byteIp[2] = actualPeer[1];
                            byteIp[3] = actualPeer[0];

                            bytePort[0] = actualPeer[5];
                            bytePort[1] = actualPeer[4];

                            Peer peer = new Peer(IPConverter.Int32ToIP(BitConverter.ToUInt32(byteIp, 0)), BitConverter.ToUInt16(bytePort, 0));
                            result.Add(peer);
                            actualPeer = new byte[6];
                            actualPeer[index] = bytes[counter];
                            counter++;
                            index++;
                        }
                        else
                        {
                            actualPeer[index] = bytes[counter];
                            if (counter == bytes.Length - 1)
                            {
                                final = true;
                            }
                            else
                            {
                                counter++;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new TrackerResponseException("peer length in bytes different of 6 multiplier");
            }
            return result;
        }
    }
}
