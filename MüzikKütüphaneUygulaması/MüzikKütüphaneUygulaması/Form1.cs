using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MüzikKütüphaneUygulaması
{
    public partial class Form1 : Form
    {
        private readonly string _connStr = ConfigurationManager.AppSettings["DbConnectionString"]
            ?? "Server=localhost;Database=muzik_kutuphanesi;Uid=root;Pwd=123456789;Charset=utf8mb4;";

        public Form1()
        {
            InitializeComponent();
            GirisTemasiniUygula();
        }

        private void GirisTemasiniUygula()
        {
            Text = "Müzik Kütüphanesi | Giriş";
            BackColor = Color.FromArgb(16, 24, 40);
            ForeColor = Color.FromArgb(236, 242, 255);
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            label1.Text = "Kullanıcı Adı:";
            label2.Text = "Şifre:";
            label3.Text = "Demo: yonetici / Admin123!  |  kullanici / Kullanici123!";
            label3.ForeColor = Color.FromArgb(182, 197, 226);

            txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.UseSystemPasswordChar = true;

            btnGiris.FlatStyle = FlatStyle.Flat;
            btnGiris.FlatAppearance.BorderSize = 0;
            btnGiris.BackColor = Color.FromArgb(29, 78, 216);
            btnGiris.ForeColor = Color.White;
            btnGiris.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);

            AcceptButton = btnGiris;
            MaximizeBox = false;
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            var kullaniciAdi = txtKullaniciAdi.Text.Trim();
            var sifre = txtSifre.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre zorunludur.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new MySqlConnection(_connStr))
            {
                try
                {
                    conn.Open();

                    const string sql = @"
SELECT rol
FROM kullanicilar
WHERE isim = @isim AND sifre = @sifre
LIMIT 1;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@isim", kullaniciAdi);
                        cmd.Parameters.AddWithValue("@sifre", sifre);

                        var result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var rol = result.ToString()?.Trim().ToLowerInvariant() ?? "kullanici";
                        Form hedefForm = rol == "yonetici" ? (Form)new Form3() : new Form2();

                        Hide();
                        hedefForm.FormClosed += (_, __) => Close();
                        hedefForm.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanına bağlanırken hata oluştu:\n" + ex.Message, "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
