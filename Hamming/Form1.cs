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
            var source = Input.Text.Trim();
            if (source.Length != 7)
            {
                Disappoint("Введите строку длиной 7 символов!");
                return;
            }

            try
            {
                var hex = new HexArray(source);
                var encoded = RSCoder.Encode(hex.Array);
                var encodedHex = new HexArray(encoded);
                string res = encodedHex.String;
                hex = new HexArray(res);
                Hamming hamm = new Hamming();
                conversion bitMask = new conversion();
                richTextBox1.Text = "Исходное сообщение: " + Input.Text + "\n"+
                   "код Рида-Соломона: " + encodedHex.String + "\n" +
                   bitMask.GetbitmaskToStr(hex.Array) + "\n";//начальное сообщение
                richTextBox1.Text = richTextBox1.Text + hamm.GetEncBitMaskToStr(bitMask.GetBitMask(hex.Array));//закодированное сообщение
            }
            catch (ArgumentException argumentException)
            {
                Disappoint(argumentException.Message);
            }

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
        void Disappoint(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}

