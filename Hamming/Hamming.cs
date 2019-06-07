using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamming
{
    class Hamming
    {
        public Hamming()
        {

           

        }
        public void encoding()
        {
            List<int> mes = new List<int> { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 };
            List<int> encode_mes = new List<int>();

            int j = 0;
            int conBit = -1;
            encode_mes = mes;

            for (int i = 1; i < mes.Count; j++, i = (int)Math.Pow(2, j))
            {

                encode_mes.Insert(i - 1, 0);
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
        }
        public void decoding()
        {
            List<int> mes = new List<int> { 1, 0, 1, 1, 0, 1, 0 };

            int a = mes.Capacity;

        }

    }
}


