using KütüphaneProjesi.Services;

namespace KütüphaneProjesi.UI
{
    public partial class LoginForm : Form
    {
        private readonly KimlikDogrulamaServisi _kimlikDogrulamaServisi = new();

        public LoginForm()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = !checkBoxSifreGoster.Checked;
            label1.Cursor = Cursors.Hand;
            SizeChanged += (_, _) => KartiOrtala();
            Shown += (_, _) => KartiOrtala();
            KartiOrtala();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var kullaniciAdi = textBox1.Text.Trim();
            var sifre = textBox2.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre zorunludur.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var oturum = _kimlikDogrulamaServisi.GirisYap(kullaniciAdi, sifre);
                if (oturum is null)
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var panel = new MainForm(oturum.Isim, oturum.Rol);
                panel.FormClosed += (_, _) => Close();
                Hide();
                panel.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giriş sırasında hata oluştu:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBoxSifreGoster.Checked;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            using var kayitFormu = new RegisterForm();
            if (kayitFormu.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = kayitFormu.YeniKullaniciAdi;
                textBox2.Clear();
                textBox2.Focus();
            }
        }

        private void checkBoxSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBoxSifreGoster.Checked;
        }

        private void KartiOrtala()
        {
            if (panelKart == null)
            {
                return;
            }

            var x = Math.Max(20, (ClientSize.Width - panelKart.Width) / 2);
            var y = Math.Max(20, (ClientSize.Height - panelKart.Height) / 2);
            panelKart.Location = new Point(x, y);
        }
    }
}

