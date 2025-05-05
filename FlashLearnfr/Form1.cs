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
        int CQid,CUid;
        private void NDist(int n) {
            List<float> brut = new List<float>();
            for (int i = 0; i < n; i++)
                brut.Add(n - i);
            float SumBrut = brut.Sum();
            foreach(float a in brut)
                p.Add(a/SumBrut*100);

        }
        private bool check() {
            return true;
        }
        private void RInit() {
            if ( this.intrebariTableAdapter.FillU(this.flashLearnDBDataSet.Intrebari,CUid) !=0)
            {
                this.intrebariTableAdapter.FillBylvl(this.flashLearnDBDataSet.Intrebari, CUid);
                DataTable dt = flashLearnDBDataSet.Intrebari;
                int n = dt.Rows.Count;
                System.Console.WriteLine("n:"+n);
                for (int i = 0; i < n; i++)
                {
                    string lvl = dt.Rows[i]["lvl"].ToString();
                    v.Add(int.Parse(lvl));
                }
                NDist(n);
            }
        }
        private void selectQ() {
           
            if (this.intrebariTableAdapter.FillU(this.flashLearnDBDataSet.Intrebari, CUid) != 0)
            {
                RInit();
                int lvl = -1;
                while (lvl == -1)
                {
                    System.Console.WriteLine("inebuynesc");
                    lvl = WRandom.getValue();
                }
                this.intrebariTableAdapter.FillBySLvl(this.flashLearnDBDataSet.Intrebari, lvl, CUid);
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
            else
            {
                label1.Text = "Nu exista intrebari pentru acest user";
                button2.Visible = false;
            }
            
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
            DataTable user = flashLearnDBDataSet.Utilizatori;
            utilizatoriTableAdapter.CheckUser(flashLearnDBDataSet.Utilizatori, textBox1.Text.ToString());
            System.Console.WriteLine(CUid);
            if (user.Rows.Count != 0)
            {
                if (user.Rows[0]["password"].ToString().Equals(textBox2.Text.ToString()))
                {
                    CUid = int.Parse(user.Rows[0]["id"].ToString());
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "Parola gresita!";
                }
            }
            else
                label4.Text = "Utilizator inexistent";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);
            DataTable user = flashLearnDBDataSet.Utilizatori;
            utilizatoriTableAdapter.CheckUser(flashLearnDBDataSet.Utilizatori, textBox1.Text.ToString());
            if (label3.Visible == false)
            {
                label3.Visible = true;
                textBox3.Visible = true;
            }
            else
            {
                if (user.Rows.Count == 0)
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
                else
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "Username existent";
                }
            }
            }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        }
        }
      
