using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hamming
{
    class Hamming
    {

        public Hamming()
        {

        }
        //encoding
        public List<List<List<int>>> encoding(List<List<int>> Bitmask)//кодирует сообщение
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
                while (ps <= conBit - 1)
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
            int cBit = Bitmask[0].Count() - 2;//игнорируем строку с счётным битом
            string strBitMask = "";
            for (int d = 0; d <= cBit; d++) //вывод порождающей матрицы 
            {
                if (d == 0) strBitMask = "Порождающая матрица\n" + "bit(" + cBit + ")" + strBitMask;
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
        public List<List<int>> decoding(List<List<int>> Decodemes, List<List<int>> EncodeMess)
        {

            foreach (List<int> bit8 in Decodemes)
            {
                int cbit = 0;  //значение проверочного бита
                int ps = 0; //начальный проверочный бит
                int chbit = 0; //чётный бит
                bit8.RemoveAt(0);
                int conBit = 0; //количество проверочных битов
                for (int i = 1; i < bit8.Count; conBit++, i = (int)Math.Pow(2, conBit))//заполнение контрольных битов нулями
                    bit8[i - 1] = 0;

                while (ps <= conBit - 1)
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
                foreach (int j in bit8)//вычисление чётного бита
                    if (j == 1) chbit++;

                bit8.Insert(0, chbit % 2);//вставка чётного бита
            }
            Decodemes = ErrorCorrection(Decodemes, EncodeMess);//получение 
            return Decodemes;
        }
        public List<List<int>> ErrorCorrection(List<List<int>> Decodemes, List<List<int>> EncodeMess)
        {

            for (int i = 0; i < Decodemes.Count(); i++)
            {
                int checkChbit = 0; //общая проверка
                List<int> decode = Decodemes[i];
                List<int> encode = EncodeMess[i];
                if (decode[0] != encode[0]) checkChbit++;//общая проверка если 1 то ошибка есть
                decode.RemoveAt(0);
                encode.RemoveAt(0);
                int conBit = 0; //количество проверочных битов
                int erBit = 0;
                for (int j = 1; j < decode.Count(); conBit++, j = (int)Math.Pow(2, conBit))//нахождение места одиночной ошибки
                {
                    if (decode[j-1] != encode[j-1]) erBit += j;
                }

                if (((checkChbit == 0) && (erBit == 0)) || ((checkChbit != 0) && (erBit != 0)))//исправление одиночной ошибки
                {
                    if (erBit == 0) continue;
                    if (decode[erBit - 1] == 1) decode[erBit - 1] = 0;
                    else decode[erBit - 1] = 1;
                }
                else
                {
                    //Decodemes.RemoveAt(i);//стирание при двойной ошибке
                    for (int h = 0; h < decode.Count(); h++) decode[h] = 8;

                }

            }

            return Decodemes;
        }
        public string GetDecBitMaskToStr(List<List<int>> Decodemes, List<List<int>> EncodeMess) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<int>> Bitmask = decoding(Decodemes, EncodeMess);
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
                int pb, ind = 1;
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
            strBitMask = strBitMask.Replace("8", "X");


            return strBitMask;
        }
    }

}


 
