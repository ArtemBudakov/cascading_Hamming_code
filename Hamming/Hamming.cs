using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Hamming
{
    class Hamming
    {

        int starshaya_stepen;
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
                int j = 0;
                int conBit = 0; //количество проверочных битов
                int chbit = 0; //чётный бит
                for (int i = 1; i < encode_mes.Count; j++, i = (int)Math.Pow(2, j))//заполнение контрольных битов нулями
                {
             
                    encode_mes.Insert(i - 1, 0);
                    conBit++;
                }
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
            foreach (List<int> second in Decodemes)
            {
                List<int> mes = new List<int>();
                int lenth = 0;
                foreach (int first in second)
                {
                    mes.Insert(lenth, first);
                    lenth++;
                }

                List<int> check_bits_start = new List<int>();
                List<int> check_bits_error = new List<int>();
                int check_bit;
                starshaya_stepen = 0;
                try
                {
                    ///////////////// смотрю какая степень 2ки максимальная
                    int i = 0, stepen = 0;
                    while (i == 0)
                    {
                        int a = mes.Count;
                        int test_con_bit = a - Convert.ToInt32(Math.Pow(2, stepen));
                        if (test_con_bit < 0)
                        {
                            stepen--;
                            starshaya_stepen = stepen;
                            break;
                        }
                        stepen++;
                    }
                    //////////////////////// забираю все информационные биты
                    while (stepen >= 0)
                    {
                        check_bit = mes[Convert.ToInt32(Math.Pow(2, stepen)) - 1];
                        stepen--;
                        //MessageBox.Show(Convert.ToString(check_bit));
                        check_bits_start.Insert(0, check_bit);
                    }
                    //вызываем метод decoder который вернёт нам исходную строку
                    Hamming ham_dec_for_error = new Hamming();
                    List<int> error = ham_dec_for_error.decoding(mes);

                    ///////////// расставление проверочных бит для сообщения с ошибкой
                    int j = 0, conBit = -1;
                    //// encode for one list
                    for (int z = 1; z < error.Count; j++, z = (int)Math.Pow(2, j))
                    {

                        error.Insert(z - 1, 0);
                        conBit++;
                    }

                    int cbit = 0;

                    while (conBit >= 0)
                    {
                        int pb = (int)Math.Pow(2, conBit);
                        for (int k = pb; k <= error.Count; k += pb)
                            for (int p = 1; p <= pb; p++, k++)
                            {
                                cbit += error[k - 1];
                            }
                        conBit--;
                        error[pb - 1] = cbit % 2;
                        cbit = 0;
                    }
                    //// encode for one list
                    //////////////////// забираю проверочные биты сообщения с ошибкой
                    stepen = starshaya_stepen;
                    while (stepen >= 0)
                    {
                        check_bit = error[Convert.ToInt32(Math.Pow(2, stepen)) - 1];
                        stepen--;
                        //MessageBox.Show(Convert.ToString(check_bit));
                        check_bits_error.Insert(0, check_bit);
                    }
                    //////////////// сверяем проверочные биты для поиска ошибки (по индексу)
                    int control_sum = 0;
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
                error_new.Add(error);
                }
                catch
                {
                    List<int> XXX = new List<int>(Convert.ToInt32("88888888"));
                    error_new.Add(XXX);
                }
                

            }            
            return error_new;
        }

        public string GetDecBitMaskToStr(List<List<int>> Bitmaskmes) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<int>> Bitmask = error_correction(Bitmaskmes);
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

 
