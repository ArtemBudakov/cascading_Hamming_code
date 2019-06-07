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
        public int parse_symbol { get; set; }

        public conversion(string messege)
        {
            //List<int> mes = new List<int>();
            for (int i = 0; i < messege.Length; i++)
            {
                word += Convert.ToString(messege[i], 2);
            }
            //MessageBox.Show(word.ToString());
        }

        public List<int> Transfer()
        {            
            MessageBox.Show(word);
            List<int> parsed = new List<int>();
            int word_number = 0, four_symbols_int;
            while (word.Length != 0)
            {
                string four_symbols = String.Empty;
                
                for(int i = 0; i < 4; i++)
                {
                    if (String.IsNullOrEmpty(word)) { break; }
                    int first_element = 0;
                    char symbol = word[first_element];
                    //MessageBox.Show(symbol.ToString());
                    string test = symbol.ToString();
                    four_symbols = four_symbols + test ;
                    word = word.Remove(0,1);
                    //char test = four_symbols[i];
                    //MessageBox.Show( test .ToString());
                    //four_symbols.PadRight(i, symbol); // в чаре хрен пойми какой символ. нужно преобразовать в строку и тестить
                    //four_symbols.PadLeft(i, symbol);

                }
                four_symbols_int = Convert.ToInt32(four_symbols);
                parsed.Add(word_number);
                parsed.Insert(word_number, four_symbols_int);
                //MessageBox.Show((word_number, "номер слова", four_symbols, "символы в строке", four_symbols_int, "символы в INT").ToString());
                word_number++;
            }
            MessageBox.Show(parsed.ToString());
            return parsed;
        }
    }
    
}
