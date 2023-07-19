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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=nizamiyetakip.accdb");
        private void yetkiligoster()
        {
            try
            {
                baglanti.Open();
                OleDbDataAdapter yetkililistele = new OleDbDataAdapter("SELECT y_tc AS[TC KİMLİK NUMARASI],y_ad AS[İSİM],y_sad AS[SOYADI],y_cins AS[CİNSİYET],y_mez AS[MEZUNİYETİ],y_dgmtrh AS[DOĞUM TARİHİ],y_grv AS[GÖREVİ],y_grvyeri AS[GÖREV YERİ],y_maas AS[MAAŞI], y_rutbe AS[RÜTBESİ] from yetkililer Order By y_ad ASC", baglanti);
                DataSet dshafiza = new DataSet();
                yetkililistele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {

                MessageBox.Show(hatamsj.Message,"Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                baglanti.Close();
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            yetkiligoster();
            lblaktifkull.Text = Form1.k_ad + " " + Form1.k_soyad;
            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.Height = 150;
            pictureBox2.Width = 150;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            try
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kresimler\\" + Form1.k_tcno + ".jpg");
            }
            catch
            {

                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kresimler\\ppyok.jpg");
            }
            txttc.Mask = "00000000000";
        }

        private void btnara_Click(object sender, EventArgs e)
        {
            bool ka_durum = false;
            if (txttc.Text.Length==11)
            {
                baglanti.Open();
                OleDbCommand s_sorgu = new OleDbCommand("SELECT * from yetkililer WHERE y_tc='" + txttc.Text + "'", baglanti);
                OleDbDataReader k_oku = s_sorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    ka_durum = true;
                    try
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath+"\\yresimler\\"+k_oku.GetValue(0)+".jpg");
                    }
                    catch (Exception)
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\yresimler\\ppyok.jpg");
                    }
                    lblad.Text = k_oku.GetValue(1).ToString();
                    lblsoyad.Text = k_oku.GetValue(2).ToString();
                    if (k_oku.GetValue(3).ToString()=="Erkek")
                    {
                        lblcinsiyet.Text = "Erkek";
                    }
                    else
                    {
                        lblcinsiyet.Text = "Kadın";
                    }
                    lblmezuniyet.Text= k_oku.GetValue(4).ToString();
                    lblrtb.Text= k_oku.GetValue(5).ToString();
                    lbldgmtrh.Text= k_oku.GetValue(6).ToString();
                    lblgorev.Text= k_oku.GetValue(7).ToString();
                    lblgrvyri.Text= k_oku.GetValue(8).ToString();
                    lblmaas.Text= k_oku.GetValue(9).ToString();
                    break;
                }
                if (ka_durum==false)
                {
                    MessageBox.Show("Aranan kişi bulunamamıştır.","Nizamiye Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Error);     
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("11 Karakterli bir TC kimlik numarası girmelisiniz!","Nizamiye Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
