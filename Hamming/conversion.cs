using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Hamming
{

    public class conversion
    {
        static readonly Dictionary<char, int> HexMap = new Dictionary<char, int>
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['A'] = 10,
            ['B'] = 11,
            ['C'] = 12,
            ['D'] = 13,
            ['E'] = 14,
            ['F'] = 15,
        };
        public conversion()
        {

        }
        public List<List<int>> GetBitMask(int[] message) //пребразует сообщение в биты и возвращает в виде List<List<int>> 
        {
            string Bitmessage = "";
            List<List<int>> Bitmask = new List<List<int>>();
            for (int i = 0; i < message.Length; i++)
            {
                Bitmessage += Convert.ToString(message[i], 2);

                while (Bitmessage.Length < 4)
                    Bitmessage = String.Concat('0', Bitmessage);

                List<int> bit4 = new List<int>();

                    for (int j = 0; j < 4; j++)
                        bit4.Add((int)Char.GetNumericValue(Bitmessage[j]));

                    Bitmask.Add(bit4);
                Bitmessage = "";


            }
            return Bitmask;
        }
        public string GetbitmaskToStr(int[] message) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<int>> Bitmask = GetBitMask(message);
            string strBitMask = "          ";
            foreach (List<int> j in Bitmask)
                {
                foreach (int i in j)
                {
                    strBitMask = strBitMask + i;
                }
                strBitMask =strBitMask + "   ##   ";
            }
            return strBitMask;
        }

        public List<List<int>> Mask_decoding(string message)
        {

            List<List<int>> Bitmask = new List<List<int>>();
            int for_out = 0;
            try
            {
                for (int i = 0; i < message.Length; i++)
                {

                    while (message.Length > 1)
                    {
                        List<int> bit8 = new List<int>();

                        for (int j = 0; j < 8; j++)
                            bit8.Add((int)Char.GetNumericValue(message[j]));
                        Bitmask.Add(bit8);
                        message = message.Substring(8);
                    }
                }
            }
            catch
            {
                if (for_out == 0)
                {
                    MessageBox.Show(Convert.ToString("Введённое значение недопустимо"));
                    List<List<int>> catches = new List<List<int>> ();
                    return catches;
                }

            }

               
            return Bitmask;
        }

        public string Get_correction_mask(string matrix)
        {
            for(int i = 8; i < matrix.Length; i+=10)
            {
                matrix = matrix.Insert(i, "##");
            }
            matrix = matrix + "##";
            return matrix;
        }

        public string from2to10(List<List<int>> mes)
        {
            int[] complite = new int[15];
            string progress = "";
            int for_convert, p=0;
            foreach (List<int> i in mes)
            {
                int pb, st = 0, ind = 1;
                progress = "";
                foreach (int j in i)
                {
                    pb = (int)Math.Pow(2, st);
                    if (ind == pb)
                    {
                        st++;
                        ind++;
                        continue;
                    }
                    progress += Convert.ToString(j);
                    ind++;
                }
                try
                {
                    for_convert = Convert.ToInt32(progress, 2);
                    complite[p] = for_convert;
                    p++;
                }
                catch
                {
                    for_convert = 0;
                    complite[p] = for_convert;
                    p++;
                }

            }
            string solomon_string = "";
            p = 0;
            while (p < complite.Length)
            {
                solomon_string += Convert.ToString(complite[p], 16);
                p++;
            }
            return solomon_string;
        }

    }
    
}
