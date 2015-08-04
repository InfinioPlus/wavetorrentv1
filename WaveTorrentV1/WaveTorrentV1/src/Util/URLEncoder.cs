using System;
using System.Collections.Generic;
using System.Text;

namespace WaveTorrentV1.src.Util
{
    class URLEncoder
    {
        public static string URLEncodeHash(string value)
        {
            char[] chars = value.ToCharArray();
            string encValue = "";
            int counter = 0;

            foreach (char chrs in chars)
            {
                if (counter % 2 == 0)
                {
                    encValue += "%" + chrs;
                }
                else
                {
                    encValue += chrs;
                }
                counter += 1;
            }
            return encValue;
        }
    }
}
