using System;
using System.Collections.Generic;
using System.Linq;

namespace Hamming
{
    class Hamming
    {

        public Hamming()
        {

        }
        //encoding
        public List<List<List<int>>> encoding (List<List<int>> Bitmask)//кодирует сообщение
        {
            List<List<List<int>>> matrixencodefull = new List<List<List<int>>>();//все матрицы сообщения
            foreach (List<int> encode_mes in Bitmask)
            {
                List<List<int>> matrixencode = new List<List<int>>(); // порождающая матрица
                int conBit = 0; //количество проверочных битов
                int chbit = 0; //чётный бит
                for (int i = 1; i < encode_mes.Count; conBit++, i = (int)Math.Pow(2, conBit))//заполнение контрольных битов нулями
                    encode_mes.Insert(i - 1, 0);

                List<int> matrix = new List<int>(encode_mes); //новая строка порождающей матрицы
                matrixencode.Add(matrix);
                int cbit = 0;  //значение проверочного бита
                int ps = 0; //начальный проверочный бит
                while (ps<=conBit-1)
                {
                    int pb = (int)Math.Pow(2, ps); //степень двойки
                    for (int i = pb; i <= encode_mes.Count; i += pb)
                        for (int p = 1; p <= pb; p++, i++)
                        {
                            if (i > encode_mes.Count) break;
                            cbit += encode_mes[i - 1];
                        }
                    ps++;
                    encode_mes[pb - 1] = cbit % 2;
                    matrix = new List<int>(encode_mes); //новая строка порождающей матрицы
                    matrixencode.Add(matrix);
                    cbit = 0;
                }

                matrix = new List<int>(matrixencode[matrixencode.Count - 1]); //новая строка порождающей матрицы
               
                foreach (int i in matrix)//вычисление чётного бита
                    if (i == 1) chbit++;

                matrix.Insert(0, chbit % 2);//вставка чётного бита
                matrixencode.Add(matrix);
                matrixencodefull.Add(matrixencode);
             }
            return matrixencodefull;
        }

        public string GetEncBitMaskToStr(List<List<int>> Bitmaskmes) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<List<int>>> Bitmask = encoding(Bitmaskmes);
            int cBit = Bitmask[0].Count()-2;//игнорируем строку с счётным битом
            string strBitMask = "";
            for (int d = 0; d <= cBit; d++) //вывод порождающей матрицы 
            {
                if(d==0)strBitMask = "Порождающая матрица\n" + "bit(" + cBit + ")" + strBitMask;
                else strBitMask = strBitMask + "\nbit(" + d + ")";
                foreach (List<List<int>> s in Bitmask)
            {

                    int db = 0; //номер необходимой строки
                    foreach (List<int> j in s)
                    {
                        if (d != db)
                        {
                            db++;
                            continue;
                        }
                        foreach (int i in j)
                        {
                            strBitMask = strBitMask + i;

                        }
                        strBitMask = strBitMask + "##";
                        db++;
                    }
                }
            }
            cBit++;//для вывода зашифрованного сообщения с чётным битом
            strBitMask = strBitMask + "\nЗашифрованное сообщение\n";
            foreach (List<List<int>> s in Bitmask) //вывод зашифрованного сообщения
            {
                int db = 0;//номер необходимой строки
                foreach (List<int> j in s)
                {
                    if (cBit != db)
                    {
                        db++;
                        continue;
                    }
                    foreach (int i in j)
                    {
                        strBitMask = strBitMask + i;

                    }
                    db++;
                }
            }
            return strBitMask;
        }

        //decoding
        public List<List<int>> decoding(List<List<int>> EncodeMess)
        {
            List<List<int>> Decodemes = new List<List<int>>(EncodeMess);
            foreach (List<int> bit8 in Decodemes)
            {
                int cbit = 0;  //значение проверочного бита
                int ps = 0; //начальный проверочный бит
                int chbit = bit8[0]; //чётный бит
                bit8.RemoveAt(0);
                int j = 0;
                int conBit = 0; //количество проверочных битов
                for (int i = 1; i < bit8.Count; conBit++, i = (int)Math.Pow(2, conBit))//заполнение контрольных битов нулями
                    bit8[i - 1]=0;

                while (ps <= conBit-1)
                {
                    int pb = (int)Math.Pow(2, ps); //степень двойки
                    for (int i = pb; i <= bit8.Count; i += pb)
                        for (int p = 1; p <= pb; p++, i++)
                        {
                            if (i > bit8.Count) break;
                            cbit += bit8[i - 1];
                        }
                    ps++;
                    bit8[pb - 1] = cbit % 2;//синдром
                    cbit = 0;
                }
                foreach (int i in bit8)//вычисление чётного бита
                    if (i == 1) chbit++;

                bit8.Insert(0, chbit % 2);//вставка чётного бита
            }

                     //////////////// сверяем проверочные биты для поиска ошибки (по индексу)
                   /*  int control_sum = 0;
                     stepen = starshaya_stepen;
                     while (stepen >= 0)
                     {
                         check_bit = mes[Convert.ToInt32(Math.Pow(2, stepen)) - 1];
                         if (check_bit == error[Convert.ToInt32(Math.Pow(2, stepen)) - 1])
                         {
                             stepen--;
                             continue;
                         }
                         else
                         {
                             control_sum = control_sum + Convert.ToInt32(Math.Pow(2, stepen));
                             stepen--;
                         }
                     }

                     //////////////////////// исправляем ошибку по найденному индексу

                     if (control_sum != 0)
                     {
                         check_bit = error[control_sum - 1];
                         if (check_bit == 0)
                         {
                             error.Insert(control_sum - 1, 1);
                             error.RemoveAt(control_sum);
                         }
                         else
                         {
                             error.Insert(control_sum - 1, 0);
                             error.RemoveAt(control_sum);
                         }

                     }
                 error_new.Add(error);*/

             return Decodemes;
        }

        public string GetDecBitMaskToStr(List<List<int>> Bitmaskmes) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<int>> Bitmask = decoding(Bitmaskmes);
            string strBitMask = "";
            strBitMask = strBitMask + "Исправление ошибок\n";
            foreach (List<int> i in Bitmask)
            {
                foreach (int j in i)
                    strBitMask = strBitMask + j;
                strBitMask = strBitMask + "##";
            }
            strBitMask = strBitMask + "\nРаcшифрованное сообщение\n";
            foreach (List<int> i in Bitmask)
            {
                int st = 0;
                int pb,ind = 1;
                foreach (int j in i)
                {
                    pb = (int)Math.Pow(2, st);
                    if (ind == pb)
                    {
                       st++;
                       ind++;
                        continue;
                    }
                    strBitMask = strBitMask + j;
                    ind++;
                }
                strBitMask = strBitMask + "##";
            }


            return strBitMask;
        }
    }

}

 
