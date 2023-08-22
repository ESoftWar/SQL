using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Önce EntityFramework ü oluşturuyoruz.
        DataSetOrnekEntities db = new DataSetOrnekEntities();
        private void dataList()
        {
            ////Bütün Verileri Listeleme
            //var kullaniciList = db.Kullanici.ToList();
            //dataGridView1.DataSource = kullaniciList;

            ////Seçili Verileri Listeleme VE Farklı Tablodan İsteniilen Veriyi Çekme (BAĞLI OLMAK ŞARTIYLA)
            dataGridView1.DataSource = (from x in db.Kullanici
                                        select new
                                        {
                                            x.Sıra,
                                            x.Ad,
                                            x.Soyad,
                                            x.IsYeri,
                                            x.Maas,
                                            x.Sektor.SAD //Burasi ComboBox işlemleri için önemli.
                                        }).ToList();
        }
        private void linqTech()
        {
            //LİNQ dediğimiz işlemler db.Kullanici. ile çıkan metotların adıdır.
            //LİNQ Metotlardan istatistiksel veri çekme
            lblTS.Text = db.Sektor.Count().ToString();
            lblTC.Text = db.Kullanici.Count().ToString();

            //SUM(x=> X ÖYLEKİ x.Maas MAAŞ SÜTÜNÜNDAKİ DEĞERLER) TOPLA //Extension Metod
            lblTM.Text = db.Kullanici.Sum(x => x.Maas).ToString();

            // SQL Sorgusu tipinde bir sorgu yazıyoruz LİNQ sorgular. //LİNQ Sorgu
            lblEYM.Text = (from x in db.Kullanici orderby x.Maas descending select x.Ad).FirstOrDefault();

            //Seçili sektörde kaç çalışan var.
            lblTSSC.Text = db.Kullanici.Count(x => x.Sektor.SAD == comboBox1.Text).ToString();

            //Tekrarsız olarak sıralama Distinct
            lblTIY.Text = (from x in db.Kullanici select x.IsYeri).Distinct().Count().ToString();

            //Extension Metod ile gruplandırma yapıp sonuc isimli değişkene aktarıyoruz.
            var sonuc = db.Kullanici.GroupBy(x => x.IsYeri);
            //Bu gruplandırmayı LİNQ sorgu ile sıralayıp değeri sırala değişkenine aktarıyoruz.
            var sırala = (from x in sonuc orderby x.Count() descending select x.Key).FirstOrDefault();
            //Sıralama işleminden çıkan sonucu yansıtıyoruz.
            lblMIY.Text = sırala.ToString();

            ////İkinci yolu da tamamı LİNQ sorgularla yapılan işlem.
            ////Daha çok kullanıcıya değerleri göstermek için kullanılır yani tek tek gruplanan elemanı almak için kullanılır.
            ////Önce gruplandırmamızı yapıyoruz ve değerlerimizi alıyoruz burdaki isimler opsiyoneldir. 
            //var sonuc = (from x in db.Kullanici
            //             group x by x.IsYeri into Grup
            //             select new
            //             {
            //                 sıra = Grup.Count(),
            //                 key = Grup.Key
            //             });
            ////Burda da yazdırma işlemini yapıyoruz sıralayarak. En yukardaki elemanı veren kod.
            //lblMIY.Text = (from x in sonuc orderby x.sıra descending select x.key).FirstOrDefault();
        }
        private void cmbDataAdd()
        {
            ////ComboBox a veri ekleme
            ////1.YOL
            comboBox1.DisplayMember = "SAD"; //Görünmesini istediğimiz alan
            comboBox1.ValueMember = "SID"; //Arka planda değeri alınacak alan
            comboBox1.DataSource = db.Sektor.ToList();

            ////2.YOL
            //var kullanici = (from k in db.Sektor
            //                 select new
            //                 { k.SID, k.SAD }).ToList();
            //comboBox1.ValueMember = "SID";
            //comboBox1.DisplayMember = "SAD";
            //comboBox1.DataSource = kullanici;
        }
        private void dtGridClick(DataGridViewCellEventArgs e)
        {
            //Tıklanan değeri textboxlara aktarma
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            //Seçili sektörde kaç çalışan var.
            lblTSSC.Text = db.Kullanici.Count(x => x.Sektor.SAD == comboBox1.Text).ToString();
        }
        private void add()
        {
            //Ekleme
            Kullanici k = new Kullanici
            {
                Ad = textBox2.Text,
                Soyad = textBox3.Text,
                IsYeri = textBox4.Text,
                Maas = decimal.Parse(textBox5.Text),
                SektorID = int.Parse(comboBox1.SelectedValue.ToString())
            };
            db.Kullanici.Add(k);
            //Kaydetme
            db.SaveChanges();
        }
        private void remove()
        {
            //Silme
            int x = int.Parse(textBox1.Text);
            var kulSil = db.Kullanici.Find(x);
            db.Kullanici.Remove(kulSil);
            db.SaveChanges();
        }
        private void update()
        {
            //Güncelleme
            int x = int.Parse(textBox1.Text);
            var kulGün = db.Kullanici.Find(x);
            kulGün.Ad = textBox2.Text;
            kulGün.Soyad = textBox3.Text;
            kulGün.IsYeri = textBox4.Text;
            kulGün.Maas = decimal.Parse(textBox5.Text);
            kulGün.SektorID = int.Parse(comboBox1.SelectedValue.ToString());
            db.SaveChanges();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataList();
            linqTech();
            cmbDataAdd(); 
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dtGridClick(e);
        }
        private void btnListele_Click(object sender, EventArgs e)
        {
            dataList();
            linqTech();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            add();
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            remove();
        }
        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            update();
        }
    }
}