using KütüphaneProjesi.Data;
using KütüphaneProjesi.Security;
using MySql.Data.MySqlClient;

namespace KütüphaneProjesi.UI
{
    public partial class RegisterForm : Form
    {
        public string YeniKullaniciAdi { get; private set; } = string.Empty;
        public string YeniSifre { get; private set; } = string.Empty;

        public RegisterForm()
        {
            InitializeComponent();
            txtSifre.UseSystemPasswordChar = !checkBoxSifreGoster.Checked;
            radioButton1.Checked = true;
            radioButton2.Enabled = false;
        }

        private void btnKaydol_Click(object sender, EventArgs e)
        {
            var kullaniciAdi = txtKullaniciAdi.Text.Trim();
            var sifre = txtSifre.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sifre.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter olmalıdır.", "Geçersiz Şifre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var baglanti = Veritabani.BaglantiOlustur();
                baglanti.Open();

                using var komut = new MySqlCommand(
                    @"INSERT INTO kullanicilar (isim, sifre_hash, rol)
                      VALUES (@isim, @hash, 'Kullanici');",
                    baglanti);

                komut.Parameters.AddWithValue("@isim", kullaniciAdi);
                komut.Parameters.AddWithValue("@hash", SifreYoneticisi.Hashle(sifre));
                komut.ExecuteNonQuery();

                YeniKullaniciAdi = kullaniciAdi;
                YeniSifre = sifre;

                MessageBox.Show("Kayıt başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında hata oluştu:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = !checkBoxSifreGoster.Checked;
        }
    }
}



