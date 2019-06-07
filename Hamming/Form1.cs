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
        encode enc;
        conversion con ;

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //encode  = new encode();
            //conversion hello = new conversion(textBox1.Text);
            //this.con = new conversion_end(textBox1.Text);
            //this.con.conversion_end(textBox1.Text);
            //this.enc.En_code(con);
            conversion con = new conversion();
            con.conversion_end(textBox1.Text);
            encode enc = new encode();
            enc.En_code(con);

        }
    }
}

