using System;
using System.Windows.Forms;
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
                        List<int> bit7 = new List<int>();

                        for (int j = 0; j < 7; j++)
                            bit7.Add((int)Char.GetNumericValue(message[j]));
                        Bitmask.Add(bit7);
                        message = message.Substring(7);
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
            for(int i = 7; i < matrix.Length; i+=9)
            {
                matrix = matrix.Insert(i, "##");
            }
            matrix = matrix + "##";
            return matrix;
        }

    }
    
}
