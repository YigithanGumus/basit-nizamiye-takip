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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=mustafa.accdb");
        private void arac_goster()
        {
            try
            {
                baglanti.Open();
                OleDbDataAdapter aracsirala = new OleDbDataAdapter("select arac_id AS[ARAÇ ID],arac_isim AS[ARAÇ İSMİ], arac_turu AS[ARAÇ TÜRÜ], arac_g_c AS[ARAÇ G/Ç],arac_tarih AS[ARAÇ GİRİŞ/ÇIKIŞ TARİHİ],arac_sorumlu AS[ARAÇ SORUMLUSU] from aractakip Order By arac_id ASC", baglanti);
                DataSet dshafiza = new DataSet();
                aracsirala.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Nizamiye Takip Programı", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                baglanti.Close();
            }
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            txtaracisim.CharacterCasing = CharacterCasing.Upper;
            txtaracsorumlusu.CharacterCasing = CharacterCasing.Upper;
            txtaracturu.CharacterCasing = CharacterCasing.Upper;
            this.Text = "Araç GİRİŞ/ÇIKIŞ Takibi";
            arac_goster();
        }
        
        private void btngeridon_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void btnara_Click(object sender, EventArgs e)
        {
            bool ka_durum = false;
            if (txtarac.Text != "")
            {
                baglanti.Open();
                OleDbCommand s_sorgu = new OleDbCommand("SELECT * FROM aractakip WHERE arac_id='" + txtarac.Text + "'", baglanti);
                OleDbDataReader k_oku = s_sorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    ka_durum = true;
                    txtarac.Text = k_oku.GetValue(0).ToString();
                    txtaracisim.Text = k_oku.GetValue(1).ToString();
                    txtaracturu.Text = k_oku.GetValue(2).ToString();
                    comboBox1.Text = k_oku.GetValue(3).ToString();
                    dateTimePicker1.Text = k_oku.GetValue(4).ToString();
                    txtaracsorumlusu.Text = k_oku.GetValue(5).ToString();
                    break;
                }
                if (ka_durum == false)
                {
                    MessageBox.Show("Aranan aracın kaydı bulunamamıştır!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Lütfen boş bırakmayınız!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }
        private void btnekle_Click(object sender, EventArgs e)
        {
            bool kkontrol = false;
            baglanti.Open();
            OleDbCommand ssorgu = new OleDbCommand("select * from aractakip where arac_id='" + txtarac.Text + "'", baglanti);
            OleDbDataReader kokuma = ssorgu.ExecuteReader();
            while (kokuma.Read())
            {
                kkontrol = true;
                break;
            }
            baglanti.Close();
            if (kkontrol == false)
            {
                if (txtarac.Text == "")
                {
                    label1.ForeColor = Color.Red;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                }

                if (txtaracisim.Text == "")
                {
                    label2.ForeColor = Color.Red;
                }
                else
                {
                    label2.ForeColor = Color.Black;
                }

                if (txtaracsorumlusu.Text == "")
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }

                if (txtaracturu.Text == "")
                {
                    label4.ForeColor = Color.Red;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }

                if (dateTimePicker1.Text == "")
                {
                    label5.ForeColor = Color.Red;
                }
                else
                {
                    label5.ForeColor = Color.Black;
                }

                if (comboBox1.Text == "")
                {
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    label6.ForeColor = Color.Black;
                }

                if (txtarac.Text != "" && txtaracisim.Text != "" && txtaracsorumlusu.Text != "" && txtaracturu.Text != "" && dateTimePicker1.Text != "")
                {
                    try
                    {
                        baglanti.Open();
                        OleDbCommand ekomut = new OleDbCommand("insert into aractakip values ('" + txtarac.Text + "','" + txtaracisim.Text + "','"+txtaracturu.Text+ "','" + comboBox1.Text + "','" + dateTimePicker1.Text + "','" + txtaracsorumlusu.Text + "')", baglanti);
                        ekomut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Yeni bir araç giriş kaydı oluşturulmuştur!", "Nizamiye Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        topPage1_tem();
                        arac_goster();
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
                MessageBox.Show("Girilen araç ismi daha önce kayıtlıdır!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            }
        }
        private void topPage1_tem()
        {
            txtarac.Clear();
            txtaracisim.Clear();
            txtaracsorumlusu.Clear();
            txtaracturu.Clear();
            comboBox1.Text = "0";
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnguncelle_Click(object sender, EventArgs e)
        {
           if (txtarac.Text == "")
            {
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.ForeColor = Color.Black;
            }

            if (txtaracisim.Text == "")
            {
                label2.ForeColor = Color.Red;
            }
            else
            {
                label2.ForeColor = Color.Black;
            }

            if (txtaracturu.Text == "")
            {
                label3.ForeColor = Color.Red;
            }
            else
            {
                label3.ForeColor = Color.Black;
            }

            if (comboBox1.Text == "")
            {
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.ForeColor = Color.Black;
            }

            if (txtaracsorumlusu.Text == "")
            {
                label6.ForeColor = Color.Red;
            }
            else
            {
                label6.ForeColor = Color.Black;
            }

            if (txtarac.Text != "" && txtaracisim.Text != "" && txtaracturu.Text != "" && comboBox1.Text != "")
            {

                try
                {
                    baglanti.Open();
                    OleDbCommand gkomut = new OleDbCommand("UPDATE aractakip set arac_isim='" + txtaracisim.Text + "',arac_turu='" + txtaracturu.Text + "',arac_g_c='" + comboBox1.Text +"',arac_tarih='"+dateTimePicker1.Text+ "' WHERE arac_id='" + txtarac.Text + "'", baglanti);
                    gkomut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Araç kaydı güncellenmiştir", "Nizamiye Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    arac_goster();
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

        private void btnkaldir_Click(object sender, EventArgs e)
        {
            if (txtarac.Text != "")
            {
                bool kadurum = false;
                baglanti.Open();
                OleDbCommand ssorgu = new OleDbCommand("SELECT * from aractakip WHERE arac_id='" + txtarac.Text + "'", baglanti);
                OleDbDataReader k_oku = ssorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    kadurum = true;
                    OleDbCommand dsorgu = new OleDbCommand("DELETE FROM aractakip WHERE arac_id='" + txtarac.Text + "'", baglanti);
                    dsorgu.ExecuteNonQuery();
                    MessageBox.Show("Araç kaydı silinmiştir!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    baglanti.Close();
                    arac_goster();
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
                MessageBox.Show("Lütfen listede gösterilen gibi düzgün bir araç ID'si giriniz!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
    }
}
