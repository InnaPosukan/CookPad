using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookPad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mainform f = new Mainform();
            this.Hide();
            f.Show();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                button1_Click(button1, null);
            }
            if (e.KeyValue == (char)Keys.Enter)
            {
                button2_Click(button2, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void Form1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            using (var helpForm = new Form3())
            {
                helpForm.ShowDialog();
            }
        }
    }
}