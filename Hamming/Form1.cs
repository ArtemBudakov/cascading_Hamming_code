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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Hamming hamm = new Hamming();
                conversion bitMask = new conversion();
                richTextBox1.Text = "Исходное сообщение: " + textBox1.Text + "\n" + bitMask.GetbitmaskToStr(textBox1.Text) + "\n";//начальное сообщение
                richTextBox1.Text = richTextBox1.Text + hamm.GetEncBitMaskToStr(bitMask.GetBitMask(textBox1.Text));//закодированное сообщение
            }
            else MessageBox.Show("введите сообшение");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= ' ' && e.KeyChar <= 'z') || e.KeyChar == 8)) { e.Handled = true; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                Hamming hamm = new Hamming();
                conversion bitMask = new conversion();
                richTextBox2.Text = "Исходное сообщение: " + "\n" + bitMask.Get_correction_mask(textBox2.Text) + "\n";//начальное сообщение
                richTextBox2.Text = richTextBox2.Text + hamm.GetDecBitMaskToStr(bitMask.Mask_decoding(textBox2.Text),bitMask.Mask_decoding(textBox2.Text));
            }
            else MessageBox.Show("введите сообшение");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!(e.KeyChar == 48 || e.KeyChar == 49 || e.KeyChar == 8)) { e.Handled = true; }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = RS_multi.alfa(2, 1);
            MessageBox.Show(i.ToString());
        }
    }
}

