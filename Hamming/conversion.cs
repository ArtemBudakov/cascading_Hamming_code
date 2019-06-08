using System;
using System.Collections.Generic;

namespace Hamming
{

    public class conversion
    {

        public conversion()
        {

        }
        public List<List<int>> GetBitMask(string message) //пребразует сообщение в биты и возвращает в виде List<List<int>> 
        {
            string Bitmessage = "";
            List<List<int>> Bitmask = new List<List<int>>();
            for (int i = 0; i < message.Length; i++)
            {
                Bitmessage += Convert.ToString(message[i], 2);

                while (Bitmessage.Length < 8)
                    Bitmessage = String.Concat('0', Bitmessage);

                while (Bitmessage.Length > 1)
                {
                    List<int> bit4 = new List<int>();

                    for (int j = 0; j < 4; j++)
                        bit4.Add((int)Char.GetNumericValue(Bitmessage[j]));

                    Bitmask.Add(bit4);
                    Bitmessage = Bitmessage.Substring(4);
                }
            }
            return Bitmask;
        }
        public string GetbitmaskToStr(string message) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<int>> Bitmask = GetBitMask(message);
            string strBitMask = "";
            foreach (List<int> j in Bitmask)
                {
                foreach (int i in j)
                {
                    strBitMask = strBitMask + i;
                }
                strBitMask = strBitMask + "|";
            }
            return strBitMask;
        }
    }
    
}
