using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; 
using System.IO; 

namespace FlashLearnfr
{
    public partial class Form1 : Form
    {
       
        Random r = new Random();
        List<int> v = new List<int>();
        List<float> p = new List<float>();
        List<int> ids = new List<int>();
        Random_Ponderat WRandom;
        int CQid,CUid=-1,CQlvl;
        private void NDist(int n) {
            List<float> brut = new List<float>();
            for (int i = 0; i < n; i++)
                brut.Add(n - i);
            float SumBrut = brut.Sum();
            foreach(float a in brut)
                p.Add(a/SumBrut*100);
        }
        
        private void RInit() {
            this.intrebariTableAdapter.FillU(this.flashLearnDBDataSet.Intrebari, CUid);
            if ( this.intrebariTableAdapter.FillU(this.flashLearnDBDataSet.Intrebari,CUid) !=0)
            {
                this.intrebariTableAdapter.FillBylvl(this.flashLearnDBDataSet.Intrebari, CUid);
                DataTable dt = flashLearnDBDataSet.Intrebari;
                int n = dt.Rows.Count;
                for (int i = 0; i < n; i++)
                {
                    string lvl = dt.Rows[i]["lvl"].ToString();
                    v.Add(int.Parse(lvl));
                }
                NDist(n);
            }
        }

        private void selectQ() {
            this.intrebariTableAdapter.FillU(this.flashLearnDBDataSet.Intrebari, CUid);
            if (this.intrebariTableAdapter.FillU(this.flashLearnDBDataSet.Intrebari, CUid) != 0)
            {
                button2.Visible = true;
                RInit();
                int lvl = -1;
                while (lvl == -1)
                {
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
                    CQlvl = int.Parse(qu.Rows[idx]["lvl"].ToString());
                    this.intrebariTableAdapter.Aparitii(CQid);
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
            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
                InitializeComponent();
                this.BackgroundImageLayout = ImageLayout.Stretch;
                menuStrip1.Renderer = new TranslucentToolStripRenderer(this);
                menuStrip1.BackColor = Color.Transparent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FlashLearnDB.mdf");
                string connectionString = string.Format(@"
                Data Source=(LocalDB)\MSSQLLocalDB;
                AttachDbFilename='{0}';
                Integrated Security=True;
                Connect Timeout=30;",
                dbPath);
                flashLearnDBDataSet.EnforceConstraints = false;
                this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);
                this.intrebariTableAdapter.Fill(this.flashLearnDBDataSet.Intrebari);
                RInit();
                WRandom = new Random_Ponderat(v, p);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectQ();
            tabControl1.SelectedIndex = 2;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.intrebariTableAdapter.UpdateLvl(3, CQid);
            this.intrebariTableAdapter.MediuCnt(CQid);
            selectQ();
            tabControl1.SelectedIndex = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(CQlvl==4)
                this.intrebariTableAdapter.UpdateLvl(5, CQid);
            else
                this.intrebariTableAdapter.UpdateLvl(4, CQid);
            this.intrebariTableAdapter.UsorCnt(CQid);
            selectQ();
            tabControl1.SelectedIndex = 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CQlvl == 2)
                this.intrebariTableAdapter.UpdateLvl(1, CQid);
            else
                this.intrebariTableAdapter.UpdateLvl(2, CQid);
            this.intrebariTableAdapter.GreuCnt(CQid);
            selectQ();
            tabControl1.SelectedIndex = 2;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.utilizatoriTableAdapter.Fill(this.flashLearnDBDataSet.Utilizatori);
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
                if (textBox1.Text != "")
                {
                    if (user.Rows.Count == 0)
                    {
                        if (textBox2.Text == textBox3.Text)
                        {
                            if (textBox2.Text != "")
                            {
                                label3.Visible = false;
                                textBox3.Visible = false;
                                label34.Visible = false;
                                label4.ForeColor = Color.Green;
                                label4.Text = "Succes";
                                utilizatoriTableAdapter.InsertQuery(textBox1.Text.ToString(), textBox2.Text.ToString());
                            }
                            else
                            {
                                label4.ForeColor = Color.Red;
                                label4.Text = "Parola nu poate fi nula";
                            }
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
                else
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "Username-ul nu poate fi null";
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string Intrebare, Raspuns;
            int lvl=1;
            Intrebare = textBox4.Text.ToString();
            Raspuns = textBox5.Text.ToString();
            List<RadioButton> levels = new List<RadioButton>();
            levels.Add(radioButton1);
            levels.Add(radioButton2);
            levels.Add(radioButton3);
            levels.Add(radioButton4);
            levels.Add(radioButton5);
            for (int i = 0; i < levels.Count; i++)
                if (levels[i].Checked)
                    lvl = 5-i;
            if (Raspuns != "" && Intrebare != "")
            {
                label15.ForeColor = Color.Green;
                label15.Text = "Succes!";
                this.intrebariTableAdapter.InsertQ(Intrebare, Raspuns, lvl, CUid);
            }
            else
            {
                label15.ForeColor = Color.Red;
                label15.Text = "Datele nu sunt complete";
            }
        }

        private void acasaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CUid != -1)
                tabControl1.SelectedIndex = 1;
        }

        private void creazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CUid != -1)
                tabControl1.SelectedIndex = 4;
        }

        private void statisticiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ids.Clear();
            comboBox1.Items.Clear();
            if (CUid != -1)
            {
                this.intrebariTableAdapter.FillU(flashLearnDBDataSet.Intrebari, CUid);
                DataTable intrebari = flashLearnDBDataSet.Intrebari;
                for (int i = 0; i < intrebari.Rows.Count; i++)
                {
                    comboBox1.Items.Add(intrebari.Rows[i]["intrebare"]);
                    ids.Add(int.Parse(intrebari.Rows[i]["id"].ToString()));
                }
                tabControl1.SelectedIndex = 5;
                this.intrebariTableAdapter.FillByMaxAp(flashLearnDBDataSet.Intrebari, CUid);
                DataTable apm = flashLearnDBDataSet.Intrebari;
                if(apm.Rows.Count!=0)
                label27.Text = apm.Rows[0]["Intrebare"] + " " + apm.Rows[0]["Aparitii"];
            }
            label29.Text = this.intrebariTableAdapter.CntGreu(CUid).ToString();
            label31.Text = this.intrebariTableAdapter.CntMediu(CUid).ToString();
            label33.Text = this.intrebariTableAdapter.CntUsor(CUid).ToString();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CUid != -1)
            {
                this.intrebariTableAdapter.Fill(flashLearnDBDataSet.Intrebari);
                CUid = -1;
                textBox1.Text = "";
                textBox2.Text = "";
                tabControl1.SelectedIndex = 0;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                System.Console.WriteLine(ids[comboBox1.SelectedIndex]);
                this.intrebariTableAdapter.FillByQid(flashLearnDBDataSet.Intrebari, ids[comboBox1.SelectedIndex]);
                DataTable stats = flashLearnDBDataSet.Intrebari;
                label21.Text = (5-int.Parse(stats.Rows[0]["lvl"].ToString())+1).ToString();
                label22.Text = stats.Rows[0]["Aparitii"].ToString();
                label23.Text = stats.Rows[0]["Raspunsuri Greu"].ToString();
                label24.Text = stats.Rows[0]["Raspunsuri Mediu"].ToString();
                label25.Text = stats.Rows[0]["Raspunsuri Usor"].ToString();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (label34.Visible == false)
            {
                if (textBox3.Text.Equals(textBox2.Text))
                    label34.Visible = true;
            }
            else
                label34.Visible = false;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (label34.Visible == false)
            {
                if (textBox3.Text.Equals(textBox2.Text))
                    label34.Visible = true;
            }
            else
                label34.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

    
   }
}
      
