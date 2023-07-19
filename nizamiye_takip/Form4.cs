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
using System.Text.RegularExpressions;
using System.IO;

namespace nizamiye_takip
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=nizamiyetakip.accdb");
        private void Form4_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            txtmodel.CharacterCasing = CharacterCasing.Upper;
            txtsilahisim.CharacterCasing = CharacterCasing.Upper;
            txtsilahisim2.CharacterCasing = CharacterCasing.Upper;
            txtmodel2.CharacterCasing = CharacterCasing.Upper;
            numericUpDown2.Maximum = 5000;
            numericUpDown4.Maximum = 15800;
            numericUpDown2.Minimum = 0;
            numericUpDown4.Minimum = 0;
            silahgoster();
        }
        private void silahgoster()
        {
            try
            {
                baglanti.Open();
                OleDbDataAdapter yetkililerisirala = new OleDbDataAdapter("select silah_isim AS[SİLAH İSMİ], silah_modeli AS[SİLAH MODELİ],silah_sayisi AS[SİLAH SAYISI], kap_sayisi AS[KAP SAYISI] from silahtakip Order By silah_isim ASC", baglanti);
                DataSet dshafiza = new DataSet();
                yetkililerisirala.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Nizamiye Takip Programı", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                baglanti.Close();
                throw;
            }
        }
        private void btnara_Click(object sender, EventArgs e)
        {
            bool ka_durum = false;
            if (txtsilahisim.Text!="")
            {
                baglanti.Open();
                OleDbCommand s_sorgu = new OleDbCommand("SELECT * FROM silahtakip WHERE silah_isim='" + txtsilahisim.Text + "'", baglanti);
                OleDbDataReader k_oku = s_sorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    ka_durum = true;
                    txtsilahisim .Text = k_oku.GetValue(0).ToString();
                    txtmodel.Text = k_oku.GetValue(1).ToString();
                    numericUpDown1.Text = k_oku.GetValue(2).ToString();
                    numericUpDown3.Text = k_oku.GetValue(3).ToString();
                    txtsilahisim2.Text = k_oku.GetValue(0).ToString();
                    txtmodel2.Text = k_oku.GetValue(1).ToString();
                    numericUpDown2.Text = k_oku.GetValue(2).ToString();
                    numericUpDown4.Text = k_oku.GetValue(3).ToString();
                    break;
                }
                if (ka_durum == false)
                {
                    MessageBox.Show("Aranan kişinin kaydı bulunamamıştır!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                baglanti.Close();
            } 
            else
            {
                MessageBox.Show("Lütfen boş bırakmayınız!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            }
        private void topPage1_tem()
        {
            txtsilahisim2.Clear();
            txtmodel2.Clear();
            numericUpDown2.Text = "0";
            numericUpDown4.Text="0";
        }
        private void btnekle_Click(object sender, EventArgs e)
        {
            bool kkontrol = false;
            baglanti.Open();
            OleDbCommand ssorgu = new OleDbCommand("select * from silahtakip where silah_isim='" + txtsilahisim2.Text + "'", baglanti);
            OleDbDataReader kokuma = ssorgu.ExecuteReader();
            while (kokuma.Read())
            {
                kkontrol = true;
                break;
            }
            baglanti.Close();
            if (kkontrol == false)
            {
                if (txtsilahisim2.Text=="")
                {
                    label7.ForeColor = Color.Red;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                }

                if (txtmodel2.Text == "")
                {
                    label10.ForeColor = Color.Red;
                }
                else
                {
                    label10.ForeColor = Color.Black;
                }

                if (numericUpDown2.Text == "")
                {
                    label8.ForeColor = Color.Red;
                }
                else
                {
                    label8.ForeColor = Color.Black;
                }

                if (numericUpDown4.Text == "")
                {
                    label9.ForeColor = Color.Red;
                }
                else
                {
                    label9.ForeColor = Color.Black;
                }

                if (txtsilahisim2.Text!="" && txtmodel2.Text!="" && numericUpDown2.Text!="" && numericUpDown4.Text!="")
                {
                    try
                    {
                        baglanti.Open();
                        OleDbCommand ekomut = new OleDbCommand("insert into silahtakip values ('" + txtsilahisim2.Text + "','" + txtmodel2.Text + "','" + numericUpDown2.Text + "','" + numericUpDown4.Text + "')", baglanti);
                        ekomut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Yeni bir silah kaydı oluşturulmuştur!", "Nizamiye Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        topPage1_tem();
                        silahgoster();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message);
                        baglanti.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Kırmızı olan alanları yeniden doldurunuz!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Girilen silah ismi daha önce kayıtlıdır!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            }
        }

        private void btngeridon_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void btnguncelle_Click(object sender, EventArgs e)
        {
            if (txtsilahisim2.Text == "")
            {
                label7.ForeColor = Color.Red;
            }
            else
            {
                label7.ForeColor = Color.Black;
            }

            if (numericUpDown2.Text == "")
            {
                label8.ForeColor = Color.Red;
            }
            else
            {
                label8.ForeColor = Color.Black;
            }
            
            if (numericUpDown4.Text == "")
            {
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.ForeColor = Color.Black;
            }

            if (txtmodel2.Text == "")
            {
                label10.ForeColor = Color.Red;
            }
            else
            {
                label10.ForeColor = Color.Black;
            }

            if (txtsilahisim2.Text!="" && txtmodel2.Text!="" && numericUpDown2.Text!="" && numericUpDown4.Text!="")
            {

                try
                {
                    baglanti.Open();
                    OleDbCommand gkomut = new OleDbCommand("UPDATE silahtakip set silah_modeli='" + txtmodel2.Text + "',silah_sayisi='" + numericUpDown2.Text + "',kap_sayisi='" + numericUpDown4.Text + "' WHERE silah_isim='"+txtsilahisim2.Text+"'", baglanti);
                    gkomut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Silah kaydı güncellenmiştir", "Nizamiye Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    silahgoster();
                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message, "NİZAMİYE TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglanti.Close();
                }

            }
            else
            {
                MessageBox.Show("Kırmızı olan alanları yeniden doldurunuz!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void btntemizle_Click(object sender, EventArgs e)
        {
            topPage1_tem();
        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            if (txtsilahisim2.Text!="")
            {
                bool kadurum = false;
                baglanti.Open();
                OleDbCommand ssorgu = new OleDbCommand("SELECT * from silahtakip WHERE silah_isim='" + txtsilahisim2.Text + "'", baglanti);
                OleDbDataReader k_oku = ssorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    kadurum = true;
                    OleDbCommand dsorgu = new OleDbCommand("DELETE FROM silahtakip WHERE silah_isim='" + txtsilahisim2.Text + "'", baglanti);
                    dsorgu.ExecuteNonQuery();
                    MessageBox.Show("Silah kaydı silinmiştir!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    baglanti.Close();
                    silahgoster();
                    topPage1_tem();
                    break;
                }
                if (kadurum == false)
                {
                    MessageBox.Show("Silinecek bir kayıt bulunamadı!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
                baglanti.Close();
                topPage1_tem();
            }
            else
            {
                MessageBox.Show("Lütfen listede gösterilen gibi düzgün bir silah ismi giriniz!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
    }
    }

