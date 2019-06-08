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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public conversion conv;

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Hamming hamm = new Hamming();
                conversion bitMask = new conversion();
                 bitMask.GetBitMask(textBox1.Text); //сделать отправку в encoding
                hamm.encoding();
                hamm.decoding();
                hamm.error_correction();

            }
            else MessageBox.Show("введите сообшение");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= ' ' && e.KeyChar <= 'z') || e.KeyChar == 8)) { e.Handled = true; }
        }
    }
}

