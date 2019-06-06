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
        public string messege;
        public string word;

        public conversion (string messege)
        {
            for(int i=0; i<messege.Length; i++)
            {
                word += Convert.ToString(messege[i], 2);
                //MessageBox.Show(word.ToString());
            }
        }
    }
    
}
