using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hamming
{

    public class conversion
    {

        public conversion(string messege)
        {
            string Bitmessage = "";
            List<List<int>> Bitmask = new List<List<int>>();
            for (int i = 0; i < messege.Length; i++)
            {
                Bitmessage += Convert.ToString(messege[i], 2);
                Bitmessage = String.Concat('0', Bitmessage);

                while (Bitmessage.Length < 1)
                {
                    List<int> bit4 = new List<int>();

                    for (int j = 0; j < 4; j++)
                    {
                        bit4.Add((int)Char.GetNumericValue(Bitmessage[j]));
                    }
                    MessageBox.Show(Bitmessage);
                    MessageBox.Show(Bitmessage);

                    Bitmessage = "";
                }
            }
            //Transfer(Bitmessage);
        }

       /* public List<List<int>> Transfer(string Bitmessage)
        {            

            //return parsed;
        }*/
    }
    
}
