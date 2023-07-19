using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace nizamiye_takip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=nizamiyetakip.accdb");

       

        private void btncikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }              
        public static string k_tcno, k_ad, k_soyad, k_yetki;
        private void btngiris_Click(object sender, EventArgs e)
        {
            if(hak!=0)
            {
                baglanti.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar", baglanti);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while(kayitokuma.Read())
                {
                    if (rdbyet.Checked == true)
                    {
                        if(kayitokuma["k_kullaniciadi"].ToString()==txtkull.Text && 
                            kayitokuma["k_parola"].ToString()==txtsfr.Text && kayitokuma["k_yetki"].ToString()=="Yönetici")
                        {
                            durum = true;
                            k_tcno = kayitokuma.GetValue(0).ToString();
                            k_ad = kayitokuma.GetValue(1).ToString();
                            k_soyad = kayitokuma.GetValue(2).ToString();
                            k_yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide();
                            Form2 frm2 = new Form2();
                            frm2.Show();
                            break;
                        }     
                    }
                    if (rdbkull.Checked == true)
                    {
                        if (kayitokuma["k_kullaniciadi"].ToString() == txtkull.Text &&
                            kayitokuma["k_parola"].ToString() == txtsfr.Text && kayitokuma["k_yetki"].ToString() == "Kullanıcı")
                        {
                            durum = true;
                            k_tcno = kayitokuma.GetValue(0).ToString();
                            k_ad = kayitokuma.GetValue(1).ToString();
                            k_soyad = kayitokuma.GetValue(2).ToString();
                            k_yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide();
                            Form3 frm3 = new Form3();
                            frm3.Show();
                            break;
                        }
                    }
                }
            
            if(durum==false)
                hak--;
            baglanti.Close();
            }
            txthak.Text = Convert.ToString(hak);
            if(hak==0)
            {
                btngiris.Enabled = false;
                MessageBox.Show("Giriş hakkınız kalmadı", "Nizamiye Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        int hak = 3;
        bool durum = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text =  "Nizamiye Takip v1.0";
            this.AcceptButton = btngiris;
            this.CancelButton = btncikis;
            rdbkull.Checked = true;
            txthak.Text = Convert.ToString(hak);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

        }
    }
}
