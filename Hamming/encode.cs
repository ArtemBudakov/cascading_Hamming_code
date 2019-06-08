using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using System.Windows.Forms;
>>>>>>> encode

namespace Hamming
{
    class encode
    {
<<<<<<< HEAD
        int conBit = -1;
        public encode()
        {

            List<int> mes = new List<int> { 1, 0, 1, 0, 1, 0, 1, 0,1,0,1,0,1,0,1,0};
=======
        int conBit = -1, starshaya_stepen;
        public encode()
        {

            List<int> mes = new List<int> { 1, 0, 1, 0 };
>>>>>>> encode
            List<int> encode_mes = new List<int>();

            int j = 0;

            encode_mes = mes;

<<<<<<< HEAD
            for (int i = 1; i < mes.Count; j++, i = (int)Math.Pow(2, j))
=======
            for (int i = 1; i < mes.Capacity; j++, i = (int)Math.Pow(2, j))
>>>>>>> encode
            {

                encode_mes.Insert(i-1,0);
                conBit++;
            }
            
            int cbit = 0;
            
            while(conBit >= 0)
            {
                int pb = (int)Math.Pow(2, conBit);
<<<<<<< HEAD
                for (int i = pb; i <= encode_mes.Count; i += pb)
                    for (int p = 1; p <= pb; p++, i++)
                    {
                        if (i > encode_mes.Count) break;
=======
                for (int i = pb; i <= encode_mes.Capacity; i += pb)
                    for (int p = 1; p <= pb; p++, i++)
                    {
>>>>>>> encode
                        cbit += encode_mes[i-1];
                    }
                conBit--;
                encode_mes[pb - 1] = cbit % 2;
                cbit = 0;
            }

<<<<<<< HEAD
            int p3b = (int)Math.Pow(2, conBit);


=======

            
>>>>>>> encode
        }

        public void decoding ()
        {
            List<int> mes = new List<int> { 1, 0, 1, 1, 0, 1, 0 };

<<<<<<< HEAD
            int a = mes.Capacity;
=======
            int i=0, stepen=0;
            while (i==0)
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
            while (stepen >=0)
            {
                mes.RemoveAt(Convert.ToInt32(Math.Pow(2,stepen))-1);
                stepen--;
            }
        }

        public void error_correction()
        {
            List<int> mes = new List<int> { 1, 0, 1, 1, 0, 1, 0 };
            List<int> check_bits_start = new List<int> ();
            List<int> check_bits_error = new List<int> ();
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
            ///////////// encode
            int j = 0;

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
            /////////////////encode
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
            int control_sum=0;
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
            check_bit = error[control_sum-1];
            if (check_bit == 0)
            {
                error.Insert(control_sum - 1, 1);
                error.RemoveAt(control_sum );
            }
            else
            {
                error.Insert(control_sum - 1, 0);
                error.RemoveAt(control_sum );
            }
>>>>>>> encode

        }

    }
}


