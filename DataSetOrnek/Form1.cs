using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSetOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Tanımlama oluşturalım (DataTable1 Tablosu)
        DataSet1TableAdapters.KullaniciTableAdapter kdta = new DataSet1TableAdapters.KullaniciTableAdapter();
        private void Form1_Load(object sender, EventArgs e)
        {
            //Listeliyoruz.
            dataGridView1.DataSource = kdta.allTableListele();
            //ComboBox ımıza değer atıyoruz.
            cmbSektor.DisplayMember = "SAD"; //Görünmesini istediğimiz ad.
            cmbSektor.ValueMember = "SID"; //Arka planda dönecek ad.
            cmbSektor.DataSource = kdta.SektorListele();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Tabloya tıklandığında değerleri textlere yazdırma işlemi.

            txtSıra.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtIs.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtMaas.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            cmbSektor.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

        }
        private void btnListele_Click(object sender, EventArgs e)
        {
            //Listeliyoruz.
            dataGridView1.DataSource = kdta.allTableListele();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            //Ekleme
            kdta.ınsQKullanici
                (
                    txtAd.Text,
                    txtSoyad.Text,
                    txtIs.Text,
                    decimal.Parse(txtMaas.Text),
                    (int?)cmbSektor.SelectedValue
                );
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            //Silme
            kdta.dltQKullanici(int.Parse(txtSıra.Text));
        }
        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            //Güncelleme
            kdta.updtQKullanici
                (
                            txtAd.Text,
                            txtSoyad.Text,
                            txtIs.Text,
                            decimal.Parse(txtMaas.Text),
                            (int?)cmbSektor.SelectedValue,
                            int.Parse(txtSıra.Text)
                ) ;
        }
    }
}