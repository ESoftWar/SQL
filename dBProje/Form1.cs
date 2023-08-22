using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace dBProje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Bağlantımızın yolu
        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=CODE418;Integrated Security=True");
        //Köprümüzü tanımladık
        public SqlDataAdapter da;
        //Komutlarımız için nesnemiz
        public SqlCommand cmd;
        //Verileri tutmak için bir nesne ürettik.
        public DataSet ds;
        //Verileri okumak için
        public SqlDataReader dr;


        private void dB_Getir()//DataBase den verileri çekmek.
        {
            
            con.Open();
            da = new SqlDataAdapter("SELECT * From k_Bilgileri", con);
            ds = new DataSet();
            da.Fill(ds, "k_Bilgileri");
            dataGridView1.DataSource = ds.Tables["k_Bilgileri"];
            con.Close();
        }
        private void dB_Sırala()
        {
            con.Open();
            da = new SqlDataAdapter("Select k_Adi, skor From k_Bilgileri ORDER BY skor DESC",con);
            ds = new DataSet();
            da.Fill(ds, "k_Bilgileri");
            dataGridView2.DataSource = ds.Tables["k_Bilgileri"];
            con.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dB_Getir();
            dB_Sırala();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox4.PasswordChar = char.Parse("\0");
                textBox5.PasswordChar = char.Parse("\0");
            }
            else
            {
                textBox4.PasswordChar = char.Parse("*");
                textBox5.PasswordChar = char.Parse("*");
            }
        }

        private void kayit_Click(object sender, EventArgs e)//Ekleme işlemleri
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    k_Kontrol();
                    if (durum_Kayit == true)
                    {
                        MessageBox.Show("Farklı bir kullanıcı adı giriniz.");
                    }
                    else if (durum_Kayit == false)
                    {
                        con.Open();

                        string k_Ekle_Sorgu = "INSERT INTO k_Bilgileri (ad_Soyad,e_Posta,k_Adi,sifre,para,skor)values(@ad_Soyad,@e_Posta,@k_Adi,@sifre,@para,@skor)";
                        cmd = new SqlCommand(k_Ekle_Sorgu, con);
                        cmd.Parameters.AddWithValue("@ad_Soyad", textBox1.Text);
                        cmd.Parameters.AddWithValue("@e_Posta", textBox2.Text);
                        cmd.Parameters.AddWithValue("@k_Adi", textBox3.Text);
                        cmd.Parameters.AddWithValue("@sifre", textBox4.Text);
                        cmd.Parameters.AddWithValue("@para", Convert.ToInt32(textBox6.Text));
                        cmd.Parameters.AddWithValue("@skor", Convert.ToInt32(textBox7.Text));
                        cmd.ExecuteNonQuery();

                        con.Close();

                        dB_Getir();
                        dB_Sırala();
                        MessageBox.Show("Kayıt oluşturuldu.");
                    }

                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }

        }

        private void sil_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                   
                    con.Open();
                    string k_Sil_Sorgu = "DELETE From k_Bilgileri WHERE k_Adi = @k_Adi";
                    cmd = new SqlCommand(k_Sil_Sorgu, con);
                    cmd.Parameters.AddWithValue("@k_Adi", textBox3.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    dB_Getir();
                    dB_Sırala();
                    MessageBox.Show("Silme başarılı.");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }

        private void güncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    string k_Ekle_Sorgu = "UPDATE k_Bilgileri SET ad_Soyad = @ad_Soyad, e_Posta = @e_Posta, k_Adi = @k_Adi, sifre = @sifre WHERE k_Adi = @k_Adi";
                    cmd = new SqlCommand(k_Ekle_Sorgu, con);
                    cmd.Parameters.AddWithValue("@ad_Soyad", textBox1.Text);
                    cmd.Parameters.AddWithValue("@e_Posta", textBox2.Text);
                    cmd.Parameters.AddWithValue("@k_Adi", textBox3.Text);
                    cmd.Parameters.AddWithValue("@sifre", textBox4.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    dB_Getir();
                    dB_Sırala();
                    MessageBox.Show("Güncelleme başarılı.");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    if (textBox3.Text != "" && textBox4.Text != "")
                    {
                        con.Open();
                        cmd = new SqlCommand("SELECT * From k_Bilgileri Where k_Adi='" + textBox3.Text + "' and sifre = '" + textBox4.Text + "'", con);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Kayıt var.");
                        }
                        else
                        {
                            MessageBox.Show("Kayit yok");
                        }
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Boş geçme");
                    }
                }
                
                

            }
            catch (Exception)
            {
                
            }
        }

        public bool durum_Kayit;
        private void k_Kontrol()
        {
            con.Open();
            cmd = new SqlCommand("SELECT * From k_Bilgileri Where k_Adi='" + textBox3.Text + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                durum_Kayit = true;
            }
            else
            {
                
                durum_Kayit = false;
            }
            con.Close();
        }
        private void verileri_Yazdir()
        {
            con.Open();
            cmd = new SqlCommand("SELECT * From k_Bilgileri Where k_Adi='" + textBox3.Text + "'", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                label8.Text = dr[4].ToString();
                label9.Text = dr[5].ToString();
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            verileri_Yazdir();
        }
    }
}
