using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Util
{
    class IPConverter
    {
        public static string Int32ToIP(uint num)
        {
            uint first = 0;
            uint second = 0;
            uint third = 0;
            uint fourth = 0;

            first = num / 16777216;
            uint residual = num % 16777216;
            second = residual / 65536;
            residual = residual % 65536;
            third = residual / 256;
            residual = residual % 256;
            fourth = residual;

            return first.ToString() + "." + second.ToString() + "." + third.ToString() + "." + fourth.ToString();
        }
    }
}
