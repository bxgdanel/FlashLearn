using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlashLearnfr
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ok = false;
            DataTable user = flashLearnDBDataSet.Utilizatori;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);
            int idm = utilizatoriTableAdapter.FillByMaxID(this.flashLearnDBDataSet.Utilizatori);
            this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);
            if (label3.Visible == false)
            {
                label3.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                if (textBox2.Text == textBox3.Text)
                {
                    label3.Visible = false;
                    textBox3.Visible = false;
                    label4.ForeColor = Color.Green;
                    label4.Text = "Succes";
                    utilizatoriTableAdapter.InsertQuery(textBox1.Text.ToString(), textBox2.Text.ToString());
                }
                else
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "Parolele nu coincid";
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            flashLearnDBDataSet.EnforceConstraints = false;
            this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);   
        }
    }
}
