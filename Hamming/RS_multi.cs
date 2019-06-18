using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hamming
{
    static class RS_multi
    {
      private static List<int> alfam = new List<int>() { 1, 2, 4, 8, 3, 6, 12, 11, 5, 10, 7, 14, 15, 13, 9, 1 };

        public static int alfa(int a ,int b)
        {
            int res = alfam[a+b];
            return res;
        }
    }
}