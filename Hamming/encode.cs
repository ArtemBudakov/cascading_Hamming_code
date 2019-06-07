using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamming
{
    class encode
    {
        int conBit = -1;
        public encode()
        {

            List<int> mes = new List<int> { 1, 0, 1, 0 };
            List<int> encode_mes = new List<int> { 1,2,3,4,5,6,7};

           /* int j = 0;

            encode_mes = mes;

            for (int i = 1; i < mes.Capacity; j++, i = (int)Math.Pow(2, j))
            {

                encode_mes.Insert(i-1,0);
                conBit++;
            }
            */
            int cbit = 0;
            conBit = 2;
            
            while(conBit >= 0)
            {
                int pb = (int)Math.Pow(2, conBit);
                for (int i = pb; i <= encode_mes.Capacity; i += pb)
                    for (int p = 1; p <= pb; p++, i++)
                    {
                        cbit += encode_mes[i-1];
                    }
                conBit--;
                cbit = 0;
            }



        }

    }
}


