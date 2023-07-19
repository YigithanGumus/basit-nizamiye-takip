using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Kütüphanelerimizi ekliyoruz
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.IO;

namespace nizamiye_takip
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        // VT bağlantısını sağlıyoruz
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=nizamiyetakip.accdb");

        // Kullanıcılar
        private void kullanicigoster()
        {
            try
            {
                baglanti.Open();
                OleDbDataAdapter kullanicilistele = new OleDbDataAdapter("select k_tcno AS[TC Kimlik Numarası]," +
                    " k_ad AS[ADI],k_soyad AS[SOYAD]," +
                    "k_yetki AS[YETKİ],k_kullaniciadi AS[KULLANICI ADI], k_parola AS[PAROLA],k_rutbe AS[RÜTBESİ] from kullanicilar Order by k_ad ASC",baglanti);
                DataSet dshafiza = new DataSet();
                kullanicilistele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglanti.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message,"Nizamiye Takip Programı",MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                baglanti.Close();
                throw;
            }
        }
        // Personel
        private void yetkiligoster()
        {
            try
            {
                baglanti.Open();
                OleDbDataAdapter yetkililerisirala = new OleDbDataAdapter("select y_tc AS[TC Kimlik Numarası], y_ad AS[ADI],y_sad AS[SOYAD],y_cins AS[CİNSİYET]," +
                    "y_mez AS[MEZUNİYETİ],y_dgmtrh AS[DOĞUM TARİHİ],y_grv AS[GÖREVİ],y_rutbe AS[RÜTBE],y_grvyeri AS[GÖREV YERİ], y_maas AS[MAAŞ] from yetkililer Order By y_ad ASC", baglanti);
                DataSet dshafiza = new DataSet();
                yetkililerisirala.Fill(dshafiza);
                dataGridView2.DataSource = dshafiza.Tables[0];
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
        private void Form2_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            pictureBox1.Width = 150;
            pictureBox1.Height = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            try
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kresimler\\" + Form1.k_tcno + ".jpg");
            }
            catch
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kresimler\\ppyok.jpg");              
            }
            this.Text = "Yetkili İşlemleri";
            lblaktif.ForeColor = Color.Black;
            lblaktif.Text = Form1.k_ad + " " + Form1.k_soyad;
            txttc.MaxLength = 11;
            txtkull.MaxLength = 8;
            toolTip1.SetToolTip(this.txttc, "TC Kimlik Numarası 11 karakterli olmalıdır!");
            rdbyet.Checked = true;
            txtad.CharacterCasing = CharacterCasing.Upper;
            txtsoyad.CharacterCasing = CharacterCasing.Upper;
            txtsfr.MaxLength = 15;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            kullanicigoster();


            /* Kullanıcı işlemleri için */
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Width = 100;
            pictureBox2.Height = 100;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;

            txttc2.Mask = "00000000000";
            txtad2.Mask = "LL????????????????????";
            txtsoyad2.Mask= "LL????????????????????";
            txtmaas.Mask = "0000";
            txtmaas.Text = "0";
            txtad2.Text.ToUpper();
            txtsoyad2.Text.ToUpper();

            cbomezuniyet.Items.Add("İlköğretim");
            cbomezuniyet.Items.Add("Ortaöğretim");
            cbomezuniyet.Items.Add("Lise");
            cbomezuniyet.Items.Add("Üniversite");
            cbomezuniyet.Items.Add("Askeri Üniversite");

            cbogorev.Items.Add("Nöbetçi");
            cbogorev.Items.Add("Nizamiye Askeri");
            cbogorev.Items.Add("Nizamiye Şoförü");
            cbogorev.Items.Add("Nizamiye İşçi");
            cbogorevyeri.Items.Add("Kışla içi");
            cbogorevyeri.Items.Add("Kışla dışı");
            cbogorevyeri.Items.Add("Nöbet Başı");
            cbogorevyeri.Items.Add("Denetim/Kontrol");

            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));
            dateTimePicker1.MinDate = new DateTime(1900, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(yil-22,ay,gun);
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            rdberkek.Checked=true;
            yetkiligoster();
        }

        private void txttc_TextChanged(object sender, EventArgs e)
        {
            if (txttc.Text.Length<11)
            {
                errorProvider1.SetError(txttc, "TC kimlik 11 karakterli olmalıdır!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void txttc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar>=48 && (int)e.KeyChar<=57) || (int)e.KeyChar==8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar)==true||char.IsControl(e.KeyChar)==true||char.IsSeparator(e.KeyChar)==true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtsoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtkull_TextChanged(object sender, EventArgs e)
        {
            if (txtkull.Text.Length!=8)
            {
                errorProvider1.SetError(txtkull, "Kullanıcı adı 8 karakter olmalıdır!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void txtkull_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar)==true || char.IsControl(e.KeyChar)==true || char.IsDigit(e.KeyChar)==true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        int pskor = 0;
        private void txtsfr_TextChanged(object sender, EventArgs e)
        {
            string pseviye = "";
            int khskor = 0, bhskor = 0, rskor = 0, sskor = 0;
            string sifre = txtsfr.Text;
            string duzsfr = "";
            duzsfr = sifre;
            duzsfr = duzsfr.Replace('İ', 'I');
            duzsfr = duzsfr.Replace('ı', 'i');
            duzsfr = duzsfr.Replace('Ç', 'C');
            duzsfr = duzsfr.Replace('ç', 'c');
            duzsfr = duzsfr.Replace('Ş', 'S');
            duzsfr = duzsfr.Replace('ş', 's');
            duzsfr = duzsfr.Replace('Ğ', 'G');
            duzsfr = duzsfr.Replace('ğ', 'g');
            duzsfr = duzsfr.Replace('Ü', 'U');
            duzsfr = duzsfr.Replace('ü', 'u');
            duzsfr = duzsfr.Replace('Ö', 'O');
            duzsfr = duzsfr.Replace('ö', 'o');
            if (sifre!=duzsfr)
            {
                sifre = duzsfr;
                txtsfr.Text = sifre;
                MessageBox.Show("Paroladaki Türkçe karakterler İngilizce karakterlere dönüştürülmüştür!","NİZAMİYE TAKİP");
            }
            int aksayisi = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;
            khskor = Math.Min(2, aksayisi) * 10;

            int AKsayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;
            bhskor = Math.Min(2, AKsayisi) * 10;

            int rsayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length;
            rskor = Math.Min(2, rsayisi) * 10;

            int ssayisi = sifre.Length - aksayisi - AKsayisi - rsayisi;
            sskor = Math.Min(2, ssayisi) * 10;

            pskor = khskor + bhskor + rskor + sskor;

            if (sifre.Length == 9)
            {
                pskor += 10;
            }
            else if(sifre.Length==10)
            {
                pskor += 20;
            }
            if (khskor==0 || bhskor==0 || rskor==0 || sskor==0)
            {
                lbluyar.Text = "Büyük harf, küçük harf, rakam ve sembol mutlaka kullanmalısın!";
            }
            if (khskor!=0 && bhskor!=0 && rskor!=0 && sskor!=0)
            {
                lbluyar.Text = "";
            }
            if (pskor<70)
            {
                pseviye = "Kabul edilemez!";
            }
            else if (pskor==70 || pskor==80)
            {
                pseviye = "Güçlü!";
            }
            else if (pskor==90 || pskor==100)
            {
                pseviye = "Çok güçlü!";
            }
            lblskor.Text = "%" + Convert.ToString(pskor);
            lblseviye.Text = pseviye;
            progressBar1.Value = pskor;
        }

        private void txtsfrtkr_TextChanged(object sender, EventArgs e)
        {
            if (txtsfrtkr.Text!=txtsfr.Text)
            {
                errorProvider1.SetError(txtsfrtkr, "Parola eşleşmiyor!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void txtsfr_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void topPage1_tem()
        {
            txttc.Clear();
            txtad.Clear();
            txtsoyad.Clear();
            txtkull.Clear();
            txtsfr.Clear();
            txtsfrtkr.Clear();
        }
        private void topPage2_tem()
        {
            pictureBox2.Image = null;
            txttc2.Clear();
            txtad2.Clear();
            txtsoyad2.Clear();
            txtmaas.Clear();
            cbomezuniyet.SelectedIndex = -1;
            cbogorev.SelectedIndex = -1;
            cbogorevyeri.SelectedIndex = -1;
        }
        private void btnkaydet_Click(object sender, EventArgs e)
        {
            string yetki = "";
            bool kkontrol = false;
            baglanti.Open();
            OleDbCommand ssorgu = new OleDbCommand("select * from kullanicilar where k_tcno='" + txttc.Text + "'", baglanti);
            OleDbDataReader kokuma = ssorgu.ExecuteReader();
            while(kokuma.Read())
            {
                kkontrol = true;
                break;
            }
            baglanti.Close();
            if (kkontrol==false)
            {
                if (txttc.Text.Length<11 || txttc.Text=="")
                {
                    label2.ForeColor = Color.Red;
                }
                else
                {
                    label2.ForeColor = Color.Black;
                }

                if (txtad.Text.Length < 2 || txtad.Text == "")
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }

                if (txtsoyad.Text.Length < 2 || txtsoyad.Text == "")
                {
                    label4.ForeColor = Color.Red;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }

                if (txtkull.Text.Length !=8 || txttc.Text == "")
                {
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    label6.ForeColor = Color.Black;
                }

                if (txtsfr.Text=="" || pskor<70)
                {
                    label7.ForeColor = Color.Red;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                }

                if (txtrutbe.Text == "")
                {
                    label7.ForeColor = Color.Red;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                }

                if (txtsfrtkr.Text == "" || txtsfr.Text!=txtsfrtkr.Text)
                {
                    label8.ForeColor = Color.Red;
                }
                else
                {
                    label8.ForeColor = Color.Black;
                }

                if (txttc.Text.Length==11 && txttc.Text!="" &&
                    txtad.Text!="" && txtad.Text.Length>1 && txtsoyad.Text!="" && 
                    txtsoyad.Text.Length>1 && txtkull.Text!="" && txtsfr.Text!="" && txtsfrtkr.Text!="" 
                    && txtsfr.Text==txtsfrtkr.Text && pskor>=70)
                {

                    if (rdbyet.Checked==true)
                    {
                        yetki = "Yönetici";
                    }
                    else if (rdbkull.Checked==true)
                    {
                        yetki = "Kullanıcı";
                    }

                    try
                    {
                        baglanti.Open();
                        OleDbCommand ekomut = new OleDbCommand("insert into kullanicilar values ('" + txttc.Text + "','" + txtad.Text + "','" + txtsoyad.Text + "','" + yetki + "','"
                            + txtkull.Text + "','" + txtsfr.Text + "','"+txtrutbe.Text+"')", baglanti);
                        ekomut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Yeni bir kullanıcı kaydı oluşturulmuştur!","Nizamiye Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        topPage1_tem();
                        kullanicigoster();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message);
                        baglanti.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Kırmızı olan alanları yeniden doldurunuz!","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Girilen TC kimlik numarası daha önce kayıtlıdır!","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);

            }
        }

        private void btnara_Click(object sender, EventArgs e)
        {
            bool kadurum = false;
            if (txttc.Text.Length==11)
            {
                baglanti.Open();
                OleDbCommand s_sorgu = new OleDbCommand("Select * from kullanicilar WHERE k_tcno='" + txttc.Text + "'", baglanti);
                OleDbDataReader k_okuma = s_sorgu.ExecuteReader();
                while (k_okuma.Read())
                {
                    kadurum = true;
                    txtad.Text = k_okuma.GetValue(1).ToString();
                    txtsoyad.Text = k_okuma.GetValue(2).ToString();
                    if (k_okuma.GetValue(3).ToString()=="Yönetici")
                    {
                        rdbyet.Checked=true;
                    }
                    else
                    {
                        rdbkull.Checked = true;
                    }
                    txtkull.Text = k_okuma.GetValue(4).ToString();
                    txtsfr.Text = k_okuma.GetValue(5).ToString();
                    txtsfrtkr.Text = k_okuma.GetValue(5).ToString();
                    break;
                }
                if (kadurum == false)
                {
                    MessageBox.Show("Aradığınız kayıt bulunamadı Efendim!", "Nizamiyet Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 11 numaralı TC kimlik giriniz!", "Nizamiyet Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                topPage1_tem();
            }
        }

        private void btnguncelle_Click(object sender, EventArgs e)
        {

            string yetki = "";
           
            
                if (txttc.Text.Length < 11 || txttc.Text == "")
                {
                    label2.ForeColor = Color.Red;
                }
                else
                {
                    label2.ForeColor = Color.Black;
                }

                if (txtad.Text.Length < 2 || txtad.Text == "")
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }

                if (txtsoyad.Text.Length < 2 || txtsoyad.Text == "")
                {
                    label4.ForeColor = Color.Red;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }

                if (txtkull.Text.Length != 8 || txttc.Text == "")
                {
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    label6.ForeColor = Color.Black;
                }

                if (txtsfr.Text == "" || pskor < 70)
                {
                    label7.ForeColor = Color.Red;
                }
                else
                {
                    label7.ForeColor = Color.Black;
                }

                if (txtrutbe.Text=="")
                {
                    label8.ForeColor = Color.Red;
                }
                else
                {
                    label8.ForeColor = Color.Black;
                }

            if (txtsfrtkr.Text == "" || txtsfr.Text != txtsfrtkr.Text)
                {
                    label8.ForeColor = Color.Red;
                }
                else
                {
                    label8.ForeColor = Color.Black;
                }

                if (txttc.Text.Length == 11 && txttc.Text != "" &&
                    txtad.Text != "" && txtad.Text.Length > 1 && txtsoyad.Text != "" &&
                    txtsoyad.Text.Length > 1 && txtkull.Text != "" && txtsfr.Text != "" && txtsfrtkr.Text != ""
                    && txtsfr.Text == txtsfrtkr.Text && pskor >= 70)
                {

                    if (rdbyet.Checked == true)
                    {
                        yetki = "Yönetici";
                    }
                    else if (rdbkull.Checked == true)
                    {
                        yetki = "Kullanıcı";
                    }

                    try
                    {
                        baglanti.Open();
                        OleDbCommand gkomut = new OleDbCommand("UPDATE kullanicilar set k_ad='"+txtad.Text+"',k_soyad='"+txtsoyad.Text+"',k_yetki='"+yetki+"',k_kullaniciadi='"+txtkull.Text+"',k_parola='"+txtsfr.Text+"',k_rutbe='"+txtrutbe.Text+"' WHERE k_tcno='"+txttc.Text+"'", baglanti);
                        gkomut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show("Kullanıcı kaydı güncellenmiştir", "Nizamiye Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kullanicigoster();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message,"NİZAMİYE TAKİP PROGRAMI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        baglanti.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Kırmızı olan alanları yeniden doldurunuz!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            
           
        }

        private void btncikis2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            if (txttc.Text.Length==11)
            {
                bool kadurum = false;
                baglanti.Open();
                OleDbCommand ssorgu = new OleDbCommand("SELECT * from kullanicilar WHERE k_tcno='" + txttc.Text + "'", baglanti);
                OleDbDataReader k_oku = ssorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    kadurum = true;
                    OleDbCommand dsorgu = new OleDbCommand("DELETE FROM kullanicilar WHERE k_tcno='" + txttc.Text + "'", baglanti);
                    dsorgu.ExecuteNonQuery();
                    MessageBox.Show("Kişinin kaydı silinmiştir!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    baglanti.Close();
                    kullanicigoster();
                    topPage1_tem();
                    break;
                }
                if (kadurum == false)
                {
                    MessageBox.Show("Silinecek bir kullanıcı bulunamadı!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
                baglanti.Close();
                topPage1_tem();
            }
            else
            {
                MessageBox.Show("Lütfen 11 karakterden oluşan bir TC kimlik numarası giriniz!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
          
        }

        private void btnformtemizle_Click(object sender, EventArgs e)
        {
            topPage1_tem();
        }

        private void btngozat_Click(object sender, EventArgs e)
        {
            OpenFileDialog rsec = new OpenFileDialog();
            rsec.Title = "Yetkili resmi seçiniz.";
            rsec.Filter = "JPG Dosyalar (*.jpg) | *.jpg";
            if (rsec.ShowDialog()==DialogResult.OK)
            {
                this.pictureBox2.Image = new Bitmap(rsec.OpenFile());
            }
        }

        private void btnkaydet2_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            bool k_kontrol = false;
            baglanti.Open();
            OleDbCommand s_sorgu = new OleDbCommand("SELECT * from yetkililer WHERE y_tc='" + txttc2.Text + "'", baglanti);
            OleDbDataReader k_oku = s_sorgu.ExecuteReader();
            while (k_oku.Read())
            {
                k_kontrol = true;
                break;
            }
            baglanti.Close();
            if (k_kontrol==false)
            {
                if (pictureBox2.Image==null)
                {
                    btngozat.ForeColor = Color.Red;
                }
                else
                {
                    btngozat.ForeColor = Color.Black;
                }

                if (txttc2.MaskCompleted==false)
                {
                    label9.ForeColor = Color.Red;
                }
                else
                {
                    label9.ForeColor = Color.Black;
                }

                if (txtad2.MaskCompleted == false)
                {
                    label10.ForeColor = Color.Red;
                }
                else
                {
                    label10.ForeColor = Color.Black;
                }

                if (txtsoyad2.MaskCompleted == false)
                {
                    label11.ForeColor = Color.Red;
                }
                else
                {
                    label11.ForeColor = Color.Black;
                }

                if (cbomezuniyet.Text=="")
                {
                    label9.ForeColor = Color.Red;
                }
                else
                {
                    label9.ForeColor = Color.Black;
                }

                if (cbogorev.Text == "")
                {
                    label15.ForeColor = Color.Red;
                }
                else
                {
                    label15.ForeColor = Color.Black;
                }

                if (cbogorevyeri.Text=="")
                {
                    label16.ForeColor = Color.Red;
                }
                else
                {
                    label16.ForeColor = Color.Black;
                }

                if (txtrutbe2.Text == "")
                {
                    label20.ForeColor = Color.Red;
                }
                else
                {
                    label20.ForeColor = Color.Black;
                }

                if (txtmaas.MaskCompleted == false)
                {
                    label17.ForeColor = Color.Red;
                }
                else
                {
                    label17.ForeColor = Color.Black;
                }

                if (int.Parse(txtmaas.Text)<1500)
                {
                    label17.ForeColor = Color.Red;
                }
                else
                {
                    label17.ForeColor = Color.Black;
                }
                
                if (pictureBox2.Image!=null && txttc2.MaskCompleted!=false && txtad2.MaskCompleted!=false && cbomezuniyet.Text!="" && cbogorev.Text!="" && cbogorevyeri.Text!="" && txtmaas.MaskCompleted!=false)
                {
                    if (rdberkek.Checked==true)
                    {
                        cinsiyet = "Erkek";
                    }
                    else if (rdbkadin.Checked==true)
                    {
                        cinsiyet = "Kadın";
                    }
                    try
                    {
                        baglanti.Open();
                        OleDbCommand e_komut = new OleDbCommand("insert into yetkililer values('"+txttc2.Text+"','"+txtad2.Text+"','"+txtsoyad2.Text+"','"+cinsiyet+"','"+cbomezuniyet.Text+"','"+dateTimePicker1.Text+"','"+cbogorev.Text+"','"+cbogorevyeri.Text+"','"+txtmaas.Text+"','"+txtrutbe2.Text+"')",baglanti);
                        e_komut.ExecuteNonQuery();
                        baglanti.Close();
                        if (!Directory.Exists(Application.StartupPath + "\\yresimler"))
                        {
                            Directory.CreateDirectory(Application.StartupPath+"\\yresimler");
                        }
                        else
                        {
                            pictureBox2.Image.Save(Application.StartupPath + "\\yresimler\\"+ txttc2.Text+".jpg");
                        }
                        MessageBox.Show("Kişi kaydı girişi tamamlanmıştır!","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                        yetkiligoster();
                        topPage2_tem();
                        txtmaas.Text = "0";

                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message,"Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                        baglanti.Close();
                    }
                 }
                else
                {
                    MessageBox.Show("Kırmızı olan alanları yeniden gözden geçiriniz!","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);  
                }
              
            }
            else
            {
                MessageBox.Show("Girilen TC kimlik numarası daha önceden kayıtlıdır!", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            } 
        }

        private void btnara2_Click(object sender, EventArgs e)
        {
            bool ka_durum = false;
            if (txttc2.Text.Length==11)
            {
                baglanti.Open();
                OleDbCommand s_sorgu = new OleDbCommand("SELECT * FROM yetkililer WHERE y_tc='"+txttc2.Text+"'",baglanti);
                OleDbDataReader k_oku = s_sorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    ka_durum = true;
                    try
                    {
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\yresimler\\" + k_oku.GetValue(0).ToString() + ".jpg");
                    }
                    catch
                    {
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\yresimler\\ppyok.jpg");

                    }
                    txtad2.Text=k_oku.GetValue(1).ToString();   
                    txtsoyad2.Text=k_oku.GetValue(2).ToString();
                    if (k_oku.GetValue(3).ToString()=="Erkek")
                    {
                        rdberkek.Checked = true;
                    }
                    else
                    {
                        rdbkadin.Checked = true;
                    }
                    cbomezuniyet.Text=k_oku.GetValue(4).ToString();
                    dateTimePicker1.Text=k_oku.GetValue(5).ToString();
                    cbogorev.Text=k_oku.GetValue(6).ToString();
                    cbogorevyeri.Text=k_oku.GetValue(7).ToString();
                    txtmaas.Text=k_oku.GetValue(8).ToString();
                    txtrutbe2.Text=k_oku.GetValue(9).ToString();
                    break;
                }
                if (ka_durum==false)
                {
                    MessageBox.Show("Aranan kişinin kaydı bulunamamıştır!","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("11 Numaralı TC kimlik numarası girmelisiniz!", "Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                topPage2_tem();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";



            if (pictureBox2.Image == null)
            {
                btngozat.ForeColor = Color.Red;
            }
            else
            {
                btngozat.ForeColor = Color.Black;
            }

            if (txttc2.MaskCompleted == false)
            {
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.ForeColor = Color.Black;
            }

            if (txtad2.MaskCompleted == false)
            {
                label10.ForeColor = Color.Red;
            }
            else
            {
                label10.ForeColor = Color.Black;
            }

            if (txtsoyad2.MaskCompleted == false)
            {
                label11.ForeColor = Color.Red;
            }
            else
            {
                label11.ForeColor = Color.Black;
            }

            if (cbomezuniyet.Text == "")
            {
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.ForeColor = Color.Black;
            }

            if (cbogorev.Text == "")
            {
                label15.ForeColor = Color.Red;
            }
            else
            {
                label15.ForeColor = Color.Black;
            }

            if (cbogorevyeri.Text == "")
            {
                label16.ForeColor = Color.Red;
            }
            else
            {
                label16.ForeColor = Color.Black;
            }

            if (txtrutbe2.Text == "")
            {
                label20.ForeColor = Color.Red;
            }
            else
            {
                label20.ForeColor = Color.Black;
            }

            if (txtmaas.MaskCompleted == false)
            {
                label17.ForeColor = Color.Red;
            }
            else
            {
                label17.ForeColor = Color.Black;
            }

            if (int.Parse(txtmaas.Text) < 1500)
            {
                label17.ForeColor = Color.Red;
            }
            else
            {
                label17.ForeColor = Color.Black;
            }

            if (pictureBox2.Image != null && txttc2.MaskCompleted != false && txtad2.MaskCompleted != false && cbomezuniyet.Text != "" && cbogorev.Text != "" && cbogorevyeri.Text != "" && txtmaas.MaskCompleted != false)
            {
                if (rdberkek.Checked == true)
                {
                    cinsiyet = "Erkek";
                }
                else if (rdbkadin.Checked == true)
                {
                    cinsiyet = "Kadın";
                }
                try
                {
                    baglanti.Open();
                    OleDbCommand g_komut = new OleDbCommand("UPDATE yetkililer SET y_ad='" + txtad2.Text + "',y_sad='" + txtsoyad2.Text + "',y_cins='" + cinsiyet + "',y_mez='" + cbomezuniyet.Text + "',y_dgmtrh='" + dateTimePicker1.Text + "',y_grv='" + cbogorev.Text + "',y_grvyeri='" + cbogorevyeri.Text + "',y_maas='" + txtmaas.Text + "',y_rutbe='" + txtrutbe2.Text + "' WHERE y_tc='" + txttc2.Text + "'", baglanti);
                    g_komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Seçtiğiniz yetkilinin bilgileri güncellenmiştir.", "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    yetkiligoster();
                    topPage2_tem();
                    txtmaas.Text = "0";

                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message, "Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    baglanti.Close();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txttc2.MaskCompleted==true)
            {
                bool ka_durum = false;
                baglanti.Open();
                OleDbCommand a_sorgu = new OleDbCommand("SELECT * from yetkililer WHERE y_tc='"+txttc2.Text+"'",baglanti);
                OleDbDataReader k_oku = a_sorgu.ExecuteReader();
                while (k_oku.Read())
                {
                    ka_durum = true;
                    OleDbCommand d_sorgu = new OleDbCommand("DELETE * from yetkililer WHERE y_tc='"+txttc2.Text+"'",baglanti);
                    d_sorgu.ExecuteNonQuery();
                    break;
                }
                if (ka_durum==false)
                {
                    MessageBox.Show("Silinecek bir yetkili bulunamadı!","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                }
                baglanti.Close();
                MessageBox.Show("Kişi silinmiştir","Nizamiye Takip Programı", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                yetkiligoster();
                topPage2_tem();
                txtmaas.Text = "0";
                
            }
            else
            {
                MessageBox.Show("Lütfen 11 numaradan oluşan bir TC kimlik giriniz.","Nizamiye Takip Programı",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                topPage2_tem();
                txtmaas.Text = "0";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString()=="Silah Stok Takip")
            {
                this.Hide();
                Form4 frm4 = new Form4();
                frm4.Show();
            }
            else if (comboBox1.SelectedItem.ToString() == "Araç Giriş/Çıkış") {
                this.Hide();
                Form5 frm5 = new Form5();
                frm5.Show();
            }
        }

        private void btngirisedon_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form frm1 = new Form1();
            frm1.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblaktif_Click(object sender, EventArgs e)
        {

        }
    }
}

