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
    public partial class Form1 : Form
    {
       
        Random r = new Random();
        List<int> v = new List<int>();
        List<float> p = new List<float>();
        Random_Ponderat WRandom;
        int CQid;
        private void NDist(int n) {
            List<float> brut = new List<float>();
            for (int i = 0; i < n; i++)
                brut.Add(n - i);
            float SumBrut = brut.Sum();
            foreach(float a in brut)
                p.Add(a/SumBrut*100);

        }

        private void RInit() {
            this.intrebariTableAdapter.Fill(this.flashLearnDBDataSet.Intrebari);
            this.intrebariTableAdapter.FillBylvl(this.flashLearnDBDataSet.Intrebari);
            DataTable dt = flashLearnDBDataSet.Intrebari;
            int n = dt.Rows.Count;
            for (int i = 0; i < n; i++)
            {
                string lvl = dt.Rows[i]["lvl"].ToString();
                v.Add(int.Parse(lvl));
            }
            NDist(n);
        }
        private void selectQ() {
            this.intrebariTableAdapter.Fill(this.flashLearnDBDataSet.Intrebari);
            RInit();
            int lvl = -1;
            while (lvl == -1)
                lvl = WRandom.getValue();
            this.intrebariTableAdapter.FillBySLvl(this.flashLearnDBDataSet.Intrebari, lvl);
            DataTable qu = flashLearnDBDataSet.Intrebari;
            if (qu.Rows.Count != 0)
            {
                int idx = r.Next(qu.Rows.Count);
                label1.Text = qu.Rows[idx]["Intrebare"].ToString();
                label2.Text = qu.Rows[idx]["Raspuns"].ToString();
                string idq = qu.Rows[idx]["id"].ToString();
                CQid = int.Parse(idq);
            }
            else RInit();
            
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            flashLearnDBDataSet.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'flashLearnDBDataSet.Utilizatori' table. You can move, or remove it, as needed.
            this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);
            this.intrebariTableAdapter.Fill(this.flashLearnDBDataSet.Intrebari);
            RInit();
            WRandom = new Random_Ponderat(v, p);

        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectQ();
            tabControl1.SelectedIndex = 2;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(2, CQid);
            selectQ();
            tabControl1.SelectedIndex = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(1, CQid);
            selectQ();
            tabControl1.SelectedIndex = 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(1, CQid);
            selectQ();
            tabControl1.SelectedIndex = 2;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool ok = false;
            DataTable user = flashLearnDBDataSet.Utilizatori;
            utilizatoriTableAdapter.CheckUser(flashLearnDBDataSet.Utilizatori, textBox1.Text.ToString());
            label7.Text = "";
            if (user.Rows.Count != 0)
            {
                label7.Text = user.Rows[0]["password"] + " " + textBox2.Text.ToString() + " " + (user.Rows[0]["password"].ToString().Equals(textBox2.Text.ToString()));
                if (user.Rows[0]["password"].ToString().Equals(textBox2.Text.ToString()))
                    tabControl1.SelectedIndex = 1;
                else
                {
                    System.Console.WriteLine(user.Rows[0]["password"]);
                    System.Console.WriteLine(textBox2.Text);
                    label4.Text = "Parola gresita!";
                }
            }
            else
                label4.Text = "Utilizator inexistent";
        }

        private void button6_Click(object sender, EventArgs e)
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
                    utilizatoriTableAdapter.InsertQuery(idm + 1, textBox1.Text.ToString(), textBox2.Text.ToString());
                }
                else
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "Parolele nu coincid";
                }
            }
            }
        }
        }
      
