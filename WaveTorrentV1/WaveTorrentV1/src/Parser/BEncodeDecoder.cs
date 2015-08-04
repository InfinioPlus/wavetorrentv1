using System;
using System.Collections.Generic;
using System.Text;
using WaveTorrentV1.src.BEncode;
using WaveTorrentV1.src.Exceptions;
using System.IO;

namespace WaveTorrentV1.src.Parser
{
    class BEncodeDecoder
    {
        BinaryReader reader;
        Stack<BEncodeString> keys = new Stack<BEncodeString>();
        int innerDictionaries = 0;

        public BEncodeDictionary parse(Stream file)
        {
            file.Position = 0;
            reader = new BinaryReader(file);
            return decodeDictionary();
        }

        private BEncodeDictionary decodeDictionary()
        {
            BEncodeDictionary value = new BEncodeDictionary();
            char start = (char)reader.ReadByte();
            bool hasFinal = false;
            if (start == 'd')
            {
                while (true)
                {
                    char actual = (char)reader.PeekChar();

                    if (actual == 'e')
                    {
                        reader.ReadByte();
                        hasFinal = true;
                        break;
                    }
                    else if (actual == 'd')
                    {
                        if ((innerDictionaries == 0) && (keys.Count % 2 == 1))
                        {
                            innerDictionaries += 1;
                            BEncodeDictionary dic = decodeDictionary();
                            value.Add(keys.Pop(), dic);
                            innerDictionaries -= 1;
                        }
                        else if ((innerDictionaries == 0) && (keys.Count % 2 == 0))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 0))
                        {
                            innerDictionaries += 1;
                            BEncodeDictionary dic = decodeDictionary();
                            value.Add(keys.Pop(), dic);
                            innerDictionaries -= 1;
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 1))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 1))
                        {
                            innerDictionaries += 1;
                            BEncodeDictionary dic = decodeDictionary();
                            value.Add(keys.Pop(), dic);
                            innerDictionaries -= 1;
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 0))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                    }
                    else if (actual == 'l')
                    {
                        if ((innerDictionaries == 0) && (keys.Count % 2 == 1))
                        {
                            value.Add(keys.Pop(), decodeList());
                        }
                        else if ((innerDictionaries == 0) && (keys.Count % 2 == 0))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 0))
                        {
                            value.Add(keys.Pop(), decodeList());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 1))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 1))
                        {
                            value.Add(keys.Pop(), decodeList());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 0))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                    }
                    else if (actual == 'i')
                    {
                        if ((innerDictionaries == 0) && (keys.Count % 2 == 1))
                        {
                            value.Add(keys.Pop(), decodeInteger());
                        }
                        else if ((innerDictionaries == 0) && (keys.Count % 2 == 0))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 0))
                        {
                            value.Add(keys.Pop(), decodeInteger());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 1))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 1))
                        {
                            value.Add(keys.Pop(), decodeInteger());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 0))
                        {
                            throw new BEncodeException("key must be a string, not dictionary");
                        }
                    }
                    else if (char.IsDigit(actual))
                    {
                        if ((innerDictionaries == 0) && (keys.Count % 2 == 1))
                        {
                            value.Add(keys.Pop(), decodeString());
                        }
                        else if ((innerDictionaries == 0) && (keys.Count % 2 == 0))
                        {
                            keys.Push(decodeString());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 0))
                        {
                            value.Add(keys.Pop(), decodeString());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 1) && (keys.Count % 2 == 1))
                        {
                            keys.Push(decodeString());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 1))
                        {
                            value.Add(keys.Pop(), decodeString());
                        }
                        else if ((innerDictionaries > 0) && (innerDictionaries % 2 == 0) && (keys.Count % 2 == 0))
                        {
                            keys.Push(decodeString());
                        }
                        else
                        {
                            throw new BEncodeException("cant recognize type in dictionary");
                        }
                    }
                }
            }
            else
            {
                throw new BEncodeException("dictionary must start with d");
            }

            if (!hasFinal)
            {
                throw new BEncodeException("dictionary must finish with e");
            }
            return value;
        }

        private BEncodeList decodeList()
        {
            BEncodeList value = new BEncodeList();
            char start = (char)reader.ReadByte();
            bool hasFinal = false;
            if (start == 'l')
            {
                while (true)
                {
                    char actual = (char)reader.PeekChar();
                    if (actual == 'e')
                    {
                        hasFinal = true;
                        reader.ReadByte();
                        break;
                    }
                    if (actual == 'd')
                    {
                        value.Add(decodeDictionary());
                    }
                    else if (actual == 'l')
                    {
                        value.Add(decodeList());
                    }
                    else if (actual == 'i')
                    {
                        value.Add(decodeInteger());
                    }
                    else if (char.IsDigit(actual))
                    {
                        value.Add(decodeString());
                    }
                    else
                    {
                        throw new BEncodeException("cant recognize type in list");
                    }
                }
            }
            else
            {
                throw new BEncodeException("list must start with l");
            }

            if (!hasFinal)
            {
                throw new BEncodeException("list must finish with e");
            }
            return value;
        }

        private BEncodeString decodeString()
        {
            char numeric = (char)reader.ReadByte();
            BEncodeString value;
            string totalNumbers = "";
            int counter = 0;

            if (char.IsDigit(numeric))
            {
                totalNumbers += numeric;

                while ((reader.PeekChar() != ':'))
                {
                    char num = (char)reader.ReadByte();

                    if (char.IsDigit(num))
                    {
                        totalNumbers += num;
                    }
                    else
                    {
                        throw new BEncodeException("char encountered in string legth");
                    }
                }
            }
            else
            {
                throw new BEncodeException("char encountered in string legth");
            }
            reader.ReadByte();
            int numberOfChars = Convert.ToInt32(totalNumbers);
            byte[] dats = new byte[numberOfChars];
            while (counter < numberOfChars)
            {
                dats[counter] = reader.ReadByte();
                counter += 1;
            }
            value = new BEncodeString(dats);
            return value;
        }

        private BEncodeInteger decodeInteger()
        {
            char start = (char)reader.ReadByte();
            BEncodeInteger value;
            string nums = "";
            bool hasFinal = false;

            if (start == 'i')
            {
                while (true)
                {
                    char actual = (char)reader.ReadByte();
                    if (actual == 'e')
                    {
                        hasFinal = true;
                        break;
                    }
                    else
                    {
                        if (char.IsDigit(actual))
                        {
                            nums += actual;
                        }
                        else
                        {
                            throw new BEncodeException("integer cant contain numbers");
                        }
                    }
                }
            }
            else
            {
                throw new BEncodeException("int must start with an i");
            }

            if (!hasFinal)
            {
                throw new BEncodeException("integer need to finish with e");
            }
            value = new BEncodeInteger(Convert.ToInt64(nums));
            return value;
        }
    }
}
