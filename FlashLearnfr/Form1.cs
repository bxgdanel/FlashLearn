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
            
            this.intrebariTableAdapter.Fill(this.flashLearnDBDataSet.Intrebari);
            flashLearnDBDataSet.EnforceConstraints = false;
            RInit();
            WRandom = new Random_Ponderat(v, p);

        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectQ();
            tabControl1.SelectedIndex = 1;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(2, CQid);
            selectQ();
            tabControl1.SelectedIndex = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(1, CQid);
            selectQ();
            tabControl1.SelectedIndex = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(1, CQid);
            selectQ();
            tabControl1.SelectedIndex = 1;
        }
      
    }
}
