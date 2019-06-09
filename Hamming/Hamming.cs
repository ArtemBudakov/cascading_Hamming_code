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

        public List<List<List<int>>> encoding (List<List<int>> Bitmask)//кодирует сообщение
        {
            List<List<List<int>>> matrixencodefull = new List<List<List<int>>>();//все матрицы сообщения
            foreach (List<int> encode_mes in Bitmask)
            {
                List<List<int>> matrixencode = new List<List<int>>(); // порождающая матрица
                int j = 0;
                int conBit = 0; //количество проверочных битов
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
                matrixencodefull.Add(matrixencode);
             }
            return matrixencodefull;
        }
        public string GetEncBitMaskToStr(List<List<int>> Bitmaskmes) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<List<int>>> Bitmask = encoding(Bitmaskmes);
            int cBit = Bitmask[0].Count()-1;
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

        public void decoding()
            {
                List<int> mes = new List<int> { 1, 0, 1, 1, 0, 1, 0 };

                int i = 0, stepen = 0;
                while (i == 0)
                {
                    int a = mes.Count;
                    int test_con_bit = a - Convert.ToInt32(Math.Pow(2, stepen));
                    if (test_con_bit < 0)
                    {
                        stepen--;
                        break;
                    }
                    stepen++;
                }
                while (stepen >= 0)
                {
                    mes.RemoveAt(Convert.ToInt32(Math.Pow(2, stepen)) - 1);
                    stepen--;
                }
            }

            public void error_correction()
            {
                List<int> mes = new List<int> { 1, 0, 1, 1, 0, 1, 0 };
                List<int> check_bits_start = new List<int>();
                List<int> check_bits_error = new List<int>();
                int check_bit;

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

                // вызываем метод decoder который вернёт нам исходную строку и делаю ошибку
                List<int> error = new List<int> { 1, 0, 1, 0 };
                Random rnd = new Random();
                int random_index = rnd.Next(0, error.Count);
                MessageBox.Show(Convert.ToString(random_index));
                int element = error[random_index];
                if (element == 0)
                {
                    error.Insert(random_index, 1);
                    error.RemoveAt(random_index + 1);
                }
                else
                {
                    error.Insert(random_index, 0);
                    error.RemoveAt(random_index + 1);
                }
                // в листе ERROR лежит исходный бинарный набор с ОДНОЙ ошибкой!
                // нужно вызвать метод ENCODE, чтобы расставить для листа ERROR проверочные биты


                ///////////// расставление проверочных бит для сообщения с ошибкой

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

        }

    }

 
