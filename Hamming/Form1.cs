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
           // encode enc = new encode();
            conversion hello = new conversion(textBox1.Text);
        }
    }
}

