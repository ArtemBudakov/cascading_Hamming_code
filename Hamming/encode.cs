using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamming
{
    class encode
    {
        int conBit = 0;
        public encode()
        {

            List<int> mes = new List<int> { 1, 0, 1, 0 };
            List<int> encode_mes = new List<int>();
            // int i = 0;

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
            int j = 0;

            encode_mes = mes;

            for (int i = 1; i < mes.Capacity; j++, i = (int)Math.Pow(2, j))
            {

                encode_mes.Insert(i-1,0);
                conBit++;
            }

            int cbit = 0;
            
            while(conBit != 0)
            {
                int pb = (int)Math.Pow(2, conBit);
                int p;
                for (int i = pb; i <= mes.Capacity; i++)

                    cbit += mes[i];
                    
            }
            /*
            for (int b = i; b <= mes.Capacity; b += i + 1)
            {

                cbit += mes[b];

            }
            /* while(conBit!=0)
             for (int i = 0; i <= mes.Capacity; i++)
             {
                 if ()
             }*/


        }

    }
}


