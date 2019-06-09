using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamming
{
    class Hamming
    {

        int starshaya_stepen;
       // List<List<string>> matrixencode;// порождающая матрица
        public Hamming()
        {

        }

        public List<List<int>> encoding (List<List<int>> Bitmask)//кодирует сообщение
        {
            int mi = 0; //индекс порождающей матрицы
            //matrixencode = new List<List<string>>();
            foreach (List<int> encode_mes in Bitmask)
            {
             
                int j = 0;
                int conBit = -1;
                for (int i = 1; i < encode_mes.Count; j++, i = (int)Math.Pow(2, j))
                {

                    encode_mes.Insert(i - 1, 0);
                    //matrixencode[mi].Insert(i - 1, "X");
                    conBit++;
                }

                int cbit = 0;

                while (conBit >= 0)
                {
                    int pb = (int)Math.Pow(2, conBit);
                    for (int i = pb; i <= encode_mes.Count; i += pb)
                        for (int p = 1; p <= pb; p++, i++)
                        {
                            if (i > encode_mes.Count) break;
                            cbit += encode_mes[i - 1];
                        }
                    conBit--;
                    encode_mes[pb - 1] = cbit % 2;
                    cbit = 0;
                }
                mi++; 
            }
            return Bitmask;
        }
        public string GetEncBitMaskToStr(List<List<int>> Bitmaskmes) //пребразует сообщение в биты и возвращает в виде строки
        {
            List<List<int>> Bitmask = encoding(Bitmaskmes);
            string strBitMask = "";
            foreach (List<int> j in Bitmask)
            {
                foreach (int i in j)
                {
                    strBitMask = strBitMask + i;
                }
                strBitMask = strBitMask + "##";
            }
            return strBitMask;
        }

        public List<int> decoding(List<int> Bitmask)
        {
             List <int> mes =  new List<int> (Bitmask);
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
            return mes;
        }

        public List<int> error_correction(List<int> Bitmask)
        {
            List<int> mes = new List<int>(Bitmask);
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
            //вызываем метод decoder который вернёт нам исходную строку
            Hamming ham_dec_for_error = new Hamming();
            List<int> error = ham_dec_for_error.decoding(mes);

            //  делаю ошибку
            Random rnd = new Random();
            int random_index = rnd.Next(0, error.Count);
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
            MessageBox.Show(Convert.ToString("ошибка по индексу = " + random_index));

            ///////// здесь можно вызывать метод преобразования Листа в строку для вывода проверочной матрицы
            //Hamming error_message = new Hamming();
            //string error_mes = error_message.GetEncBitMaskToStr(error);


            // в листе ERROR лежит исходный бинарный набор с ОДНОЙ ошибкой!

            
            ///////////// расставление проверочных бит для сообщения с ошибкой
            ////encode for one list
            int j = 0, conBit = -1;

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

            Hamming ham_dec_for_end = new Hamming();
            error = ham_dec_for_end.decoding(mes);
            return error;
        }

    }

}

 
