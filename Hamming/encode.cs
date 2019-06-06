using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamming
{
    class encode
    {
        string str = "";
        public encode()
        {

            List<int> mes = new List<int> { 1, 0, 1, 0 };
            List<int> encode_mes = new List<int>();
           // int i = 0;
            int j = 2;
            //str = str.Insert(0, "X").Insert(1, "X");
            /*
                        foreach (int mesbit in mes)
                        {
                            str = str.Insert(i+2, mesbit.ToString());
                            //encode_mes.Insert(i, mesbit);
                            i++;
                            if (i == (Math.Pow(2, j))) str = str.Insert(i - 1, "X");
                        }
                        */
            encode_mes = mes;
            for (int i = 4; i <= mes.Capacity; j++)
            {
                if (i == (Math.Pow(2, j)))
                {
                    encode_mes.Add(0);
                    j++;
                }
                    encode_mes.Add(mes[i]);

            }



        }

    }
}


