using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MüzikKütüphaneUygulaması
{
    public partial class Form3 : Form
    {
        private readonly string _connStr = ConfigurationManager.AppSettings["DbConnectionString"]
            ?? "Server=localhost;Database=muzik_kutuphanesi;Uid=root;Pwd=123456789;Charset=utf8mb4;";

        private enum Mode
        {
            Sarkilar,
            Sanatcilar,
            Turler,
            Kullanicilar
        }

        private Mode _aktifMod = Mode.Sarkilar;

        public Form3()
        {
            InitializeComponent();
            YoneticiTemasiniUygula();

            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.MultiSelect = false;
            dgvMain.ReadOnly = true;
            dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cmbMode.SelectedIndexChanged += (_, __) => ModDegistir();
            btnListele.Click += (_, __) => Listele();
            btnAra.Click += (_, __) => Ara();
            btnEkle.Click += (_, __) => Ekle();
            btnGuncelle.Click += (_, __) => Guncelle();
            btnSil.Click += (_, __) => Sil();
        }

        private void YoneticiTemasiniUygula()
        {
            Text = "Müzik Kütüphanesi | Yönetici Paneli";
            BackColor = Color.FromArgb(7, 22, 44);
            ForeColor = Color.FromArgb(235, 245, 255);
            Font = new Font("Segoe UI", 9.3F, FontStyle.Regular, GraphicsUnit.Point);

            groupBox1.ForeColor = ForeColor;
            groupBox2.ForeColor = ForeColor;
            groupBox3.ForeColor = ForeColor;
            groupBox4.ForeColor = ForeColor;
            groupBox5.ForeColor = ForeColor;
            groupBox6.ForeColor = ForeColor;
            groupBox7.ForeColor = ForeColor;

            groupBox1.Text = "Kontrol";
            groupBox2.Text = "Kayıt Yönetimi";
            groupBox3.Text = "Şarkı Bilgileri";
            groupBox4.Text = "Sanatçı";
            groupBox5.Text = "Tür";
            groupBox6.Text = "Kullanıcı";
            groupBox7.Text = "İşlemler";

            label1.Text = "Şarkı:";
            label2.Text = "Albüm:";
            label3.Text = "Yıl:";
            label4.Text = "Tür:";
            label5.Text = "Sanatçı:";
            label6.Text = "Kullanıcı:";

            StilButon(btnListele, false);
            StilButon(btnAra, true);
            StilButon(btnEkle, true);
            StilButon(btnGuncelle, false);
            StilButon(btnSil, false);
            StilButon(btnTemizle, false);

            StilGirdi(txtAra);
            StilGirdi(txtSarkıIsim);
            StilGirdi(txtAlbum);
            StilGirdi(txtYil);
            StilGirdi(txtSanatciAdi);
            StilGirdi(txtTurAdi);
            StilGirdi(txtKullaniciIsim);
            StilGirdi(txtKullaniciSifre);
            StilGirdi(textBox1);

            StilSecim(cmbMode);
            StilSecim(cmbSanatci);
            StilSecim(cmbTur);
            StilSecim(cmbSarkiKullanici);
            StilSecim(cmbRol);

            dgvMain.BackgroundColor = Color.FromArgb(11, 34, 67);
            dgvMain.BorderStyle = BorderStyle.None;
            dgvMain.RowHeadersVisible = false;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 99, 161);
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold, GraphicsUnit.Point);
            dgvMain.DefaultCellStyle.BackColor = Color.FromArgb(225, 238, 252);
            dgvMain.DefaultCellStyle.ForeColor = Color.FromArgb(18, 30, 54);
            dgvMain.DefaultCellStyle.SelectionBackColor = Color.FromArgb(39, 125, 194);
            dgvMain.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(210, 227, 247);
        }

        private static void StilButon(Button buton, bool birincil)
        {
            buton.FlatStyle = FlatStyle.Flat;
            buton.FlatAppearance.BorderSize = 0;
            buton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            buton.ForeColor = Color.White;
            buton.BackColor = birincil ? Color.FromArgb(14, 116, 144) : Color.FromArgb(41, 61, 94);
        }

        private static void StilGirdi(TextBox kutu)
        {
            kutu.BorderStyle = BorderStyle.FixedSingle;
            kutu.BackColor = Color.White;
            kutu.ForeColor = Color.FromArgb(22, 33, 57);
        }

        private static void StilSecim(ComboBox kutu)
        {
            kutu.FlatStyle = FlatStyle.Flat;
            kutu.BackColor = Color.White;
            kutu.ForeColor = Color.FromArgb(22, 33, 57);
            kutu.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            cmbMode.SelectedIndex = 0;
            cmbRol.SelectedIndex = 0;
            ModDegistir();
        }

        private DataTable GetTable(string sql, params MySqlParameter[] prms)
        {
            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                if (prms != null && prms.Length > 0)
                {
                    cmd.Parameters.AddRange(prms);
                }

                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private int Exec(string sql, params MySqlParameter[] prms)
        {
            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (prms != null && prms.Length > 0)
                {
                    cmd.Parameters.AddRange(prms);
                }

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private static int? SeciliDegeriAl(ComboBox combo)
        {
            if (combo.SelectedValue == null)
            {
                return null;
            }

            return int.TryParse(combo.SelectedValue.ToString(), out var id) ? id : (int?)null;
        }

        private void ModDegistir()
        {
            _aktifMod = (Mode)Math.Max(0, cmbMode.SelectedIndex);

            groupBox3.Visible = _aktifMod == Mode.Sarkilar;
            groupBox4.Visible = _aktifMod == Mode.Sanatcilar;
            groupBox5.Visible = _aktifMod == Mode.Turler;
            groupBox6.Visible = _aktifMod == Mode.Kullanicilar;

            if (_aktifMod == Mode.Sarkilar)
            {
                DoldurSanatciCombo();
                DoldurTurCombo();
                DoldurKullaniciCombo();
            }

            Temizle();
            Listele();
        }

        private void DoldurSanatciCombo()
        {
            var dt = GetTable("SELECT sanatci_id, sanatci_adi FROM sanatcilar ORDER BY sanatci_adi;");
            cmbSanatci.DisplayMember = "sanatci_adi";
            cmbSanatci.ValueMember = "sanatci_id";
            cmbSanatci.DataSource = dt;
        }

        private void DoldurTurCombo()
        {
            var dt = GetTable("SELECT tur_id, tur_adi FROM turler ORDER BY tur_adi;");
            cmbTur.DisplayMember = "tur_adi";
            cmbTur.ValueMember = "tur_id";
            cmbTur.DataSource = dt;
        }

        private void DoldurKullaniciCombo()
        {
            var dt = GetTable("SELECT kullanici_id, isim FROM kullanicilar ORDER BY isim;");
            cmbSarkiKullanici.DisplayMember = "isim";
            cmbSarkiKullanici.ValueMember = "kullanici_id";
            cmbSarkiKullanici.DataSource = dt;
        }

        private void Listele()
        {
            if (_aktifMod == Mode.Sarkilar)
            {
                dgvMain.DataSource = GetTable(@"
SELECT
  s.sarki_id,
  s.isim AS sarki_adi,
  a.sanatci_adi,
  t.tur_adi,
  s.album,
  s.yil,
  s.sanatci_id,
  s.tur_id,
  s.kullanici_id
FROM sarkilar s
JOIN sanatcilar a ON a.sanatci_id = s.sanatci_id
JOIN turler t ON t.tur_id = s.tur_id
ORDER BY s.sarki_id DESC;");

                if (dgvMain.Columns.Contains("sanatci_id")) dgvMain.Columns["sanatci_id"].Visible = false;
                if (dgvMain.Columns.Contains("tur_id")) dgvMain.Columns["tur_id"].Visible = false;
                if (dgvMain.Columns.Contains("kullanici_id")) dgvMain.Columns["kullanici_id"].Visible = false;
                return;
            }

            if (_aktifMod == Mode.Sanatcilar)
            {
                dgvMain.DataSource = GetTable("SELECT sanatci_id, sanatci_adi FROM sanatcilar ORDER BY sanatci_id DESC;");
                return;
            }

            if (_aktifMod == Mode.Turler)
            {
                dgvMain.DataSource = GetTable("SELECT tur_id, tur_adi FROM turler ORDER BY tur_id DESC;");
                return;
            }

            dgvMain.DataSource = GetTable(@"
SELECT kullanici_id, isim, sifre, rol, created_at
FROM kullanicilar
ORDER BY kullanici_id DESC;");
        }

        private void Ara()
        {
            var aranan = txtAra.Text.Trim();
            if (string.IsNullOrWhiteSpace(aranan))
            {
                Listele();
                return;
            }

            var like = "%" + aranan + "%";

            if (_aktifMod == Mode.Sarkilar)
            {
                dgvMain.DataSource = GetTable(@"
SELECT
  s.sarki_id,
  s.isim AS sarki_adi,
  a.sanatci_adi,
  t.tur_adi,
  s.album,
  s.yil,
  s.sanatci_id,
  s.tur_id,
  s.kullanici_id
FROM sarkilar s
JOIN sanatcilar a ON a.sanatci_id = s.sanatci_id
JOIN turler t ON t.tur_id = s.tur_id
WHERE s.isim LIKE @q OR a.sanatci_adi LIKE @q OR t.tur_adi LIKE @q
ORDER BY s.sarki_id DESC;", new MySqlParameter("@q", like));

                if (dgvMain.Columns.Contains("sanatci_id")) dgvMain.Columns["sanatci_id"].Visible = false;
                if (dgvMain.Columns.Contains("tur_id")) dgvMain.Columns["tur_id"].Visible = false;
                if (dgvMain.Columns.Contains("kullanici_id")) dgvMain.Columns["kullanici_id"].Visible = false;
                return;
            }

            if (_aktifMod == Mode.Sanatcilar)
            {
                dgvMain.DataSource = GetTable(
                    "SELECT sanatci_id, sanatci_adi FROM sanatcilar WHERE sanatci_adi LIKE @q ORDER BY sanatci_id DESC;",
                    new MySqlParameter("@q", like));
                return;
            }

            if (_aktifMod == Mode.Turler)
            {
                dgvMain.DataSource = GetTable(
                    "SELECT tur_id, tur_adi FROM turler WHERE tur_adi LIKE @q ORDER BY tur_id DESC;",
                    new MySqlParameter("@q", like));
                return;
            }

            dgvMain.DataSource = GetTable(@"
SELECT kullanici_id, isim, sifre, rol, created_at
FROM kullanicilar
WHERE isim LIKE @q OR rol LIKE @q
ORDER BY kullanici_id DESC;", new MySqlParameter("@q", like));
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMain.CurrentRow == null)
            {
                return;
            }

            var r = dgvMain.CurrentRow;

            if (_aktifMod == Mode.Sarkilar)
            {
                textBox1.Text = r.Cells["sarki_id"].Value.ToString();
                txtSarkıIsim.Text = r.Cells["sarki_adi"].Value?.ToString() ?? string.Empty;
                txtAlbum.Text = r.Cells["album"].Value == DBNull.Value ? string.Empty : r.Cells["album"].Value.ToString();
                txtYil.Text = r.Cells["yil"].Value == DBNull.Value ? string.Empty : r.Cells["yil"].Value.ToString();

                if (r.Cells["sanatci_id"].Value != DBNull.Value)
                {
                    cmbSanatci.SelectedValue = r.Cells["sanatci_id"].Value;
                }

                if (r.Cells["tur_id"].Value != DBNull.Value)
                {
                    cmbTur.SelectedValue = r.Cells["tur_id"].Value;
                }

                if (r.Cells["kullanici_id"].Value != DBNull.Value)
                {
                    cmbSarkiKullanici.SelectedValue = r.Cells["kullanici_id"].Value;
                }

                return;
            }

            if (_aktifMod == Mode.Sanatcilar)
            {
                textBox1.Text = r.Cells["sanatci_id"].Value.ToString();
                txtSanatciAdi.Text = r.Cells["sanatci_adi"].Value?.ToString() ?? string.Empty;
                return;
            }

            if (_aktifMod == Mode.Turler)
            {
                textBox1.Text = r.Cells["tur_id"].Value.ToString();
                txtTurAdi.Text = r.Cells["tur_adi"].Value?.ToString() ?? string.Empty;
                return;
            }

            textBox1.Text = r.Cells["kullanici_id"].Value.ToString();
            txtKullaniciIsim.Text = r.Cells["isim"].Value?.ToString() ?? string.Empty;
            txtKullaniciSifre.Text = r.Cells["sifre"].Value?.ToString() ?? string.Empty;
            cmbRol.SelectedItem = r.Cells["rol"].Value?.ToString() ?? "kullanici";
        }

        private void Ekle()
        {
            try
            {
                if (_aktifMod == Mode.Sarkilar) EkleSarki();
                else if (_aktifMod == Mode.Sanatcilar) EkleSanatci();
                else if (_aktifMod == Mode.Turler) EkleTur();
                else EkleKullanici();

                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt eklenemedi:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guncelle()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Önce listeden bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_aktifMod == Mode.Sarkilar) GuncelleSarki();
                else if (_aktifMod == Mode.Sanatcilar) GuncelleSanatci();
                else if (_aktifMod == Mode.Turler) GuncelleTur();
                else GuncelleKullanici();

                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme başarısız:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Sil()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Önce listeden bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var onay = MessageBox.Show("Seçilen kayıt silinsin mi?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (_aktifMod == Mode.Sarkilar) SilSarki();
                else if (_aktifMod == Mode.Sanatcilar) SilSanatci();
                else if (_aktifMod == Mode.Turler) SilTur();
                else SilKullanici();

                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi başarısız:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EkleSarki()
        {
            var isim = txtSarkıIsim.Text.Trim();
            if (string.IsNullOrWhiteSpace(isim))
            {
                MessageBox.Show("Şarkı adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sanatciId = SeciliDegeriAl(cmbSanatci);
            var turId = SeciliDegeriAl(cmbTur);
            var kullaniciId = SeciliDegeriAl(cmbSarkiKullanici);

            if (!sanatciId.HasValue || !turId.HasValue || !kullaniciId.HasValue)
            {
                MessageBox.Show("Sanatçı, tür ve kullanıcı seçimi zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object album = string.IsNullOrWhiteSpace(txtAlbum.Text) ? (object)DBNull.Value : txtAlbum.Text.Trim();
            object yil = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(txtYil.Text))
            {
                if (!int.TryParse(txtYil.Text.Trim(), out var y))
                {
                    MessageBox.Show("Yıl alanı yalnızca sayı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                yil = y;
            }

            Exec(@"INSERT INTO sarkilar (isim, sanatci_id, tur_id, album, yil, kullanici_id)
VALUES (@isim,@sanatci,@tur,@album,@yil,@kid);",
                new MySqlParameter("@isim", isim),
                new MySqlParameter("@sanatci", sanatciId.Value),
                new MySqlParameter("@tur", turId.Value),
                new MySqlParameter("@album", album),
                new MySqlParameter("@yil", yil),
                new MySqlParameter("@kid", kullaniciId.Value));
        }

        private void GuncelleSarki()
        {
            var id = Convert.ToInt32(textBox1.Text);
            var isim = txtSarkıIsim.Text.Trim();
            if (string.IsNullOrWhiteSpace(isim))
            {
                MessageBox.Show("Şarkı adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sanatciId = SeciliDegeriAl(cmbSanatci);
            var turId = SeciliDegeriAl(cmbTur);
            var kullaniciId = SeciliDegeriAl(cmbSarkiKullanici);

            if (!sanatciId.HasValue || !turId.HasValue || !kullaniciId.HasValue)
            {
                MessageBox.Show("Sanatçı, tür ve kullanıcı seçimi zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object album = string.IsNullOrWhiteSpace(txtAlbum.Text) ? (object)DBNull.Value : txtAlbum.Text.Trim();
            object yil = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(txtYil.Text))
            {
                if (!int.TryParse(txtYil.Text.Trim(), out var y))
                {
                    MessageBox.Show("Yıl alanı yalnızca sayı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                yil = y;
            }

            Exec(@"UPDATE sarkilar
SET isim=@isim, sanatci_id=@sanatci, tur_id=@tur, album=@album, yil=@yil, kullanici_id=@kid
WHERE sarki_id=@id;",
                new MySqlParameter("@isim", isim),
                new MySqlParameter("@sanatci", sanatciId.Value),
                new MySqlParameter("@tur", turId.Value),
                new MySqlParameter("@album", album),
                new MySqlParameter("@yil", yil),
                new MySqlParameter("@kid", kullaniciId.Value),
                new MySqlParameter("@id", id));
        }

        private void SilSarki()
        {
            var id = Convert.ToInt32(textBox1.Text);
            Exec("DELETE FROM sarkilar WHERE sarki_id=@id;", new MySqlParameter("@id", id));
        }

        private void EkleSanatci()
        {
            var ad = txtSanatciAdi.Text.Trim();
            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Sanatçı adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exec("INSERT INTO sanatcilar (sanatci_adi) VALUES (@a);", new MySqlParameter("@a", ad));
        }

        private void GuncelleSanatci()
        {
            var id = Convert.ToInt32(textBox1.Text);
            var ad = txtSanatciAdi.Text.Trim();
            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Sanatçı adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exec("UPDATE sanatcilar SET sanatci_adi=@a WHERE sanatci_id=@id;",
                new MySqlParameter("@a", ad),
                new MySqlParameter("@id", id));
        }

        private void SilSanatci()
        {
            var id = Convert.ToInt32(textBox1.Text);
            Exec("DELETE FROM sanatcilar WHERE sanatci_id=@id;", new MySqlParameter("@id", id));
        }

        private void EkleTur()
        {
            var ad = txtTurAdi.Text.Trim();
            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Tür adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exec("INSERT INTO turler (tur_adi) VALUES (@t);", new MySqlParameter("@t", ad));
        }

        private void GuncelleTur()
        {
            var id = Convert.ToInt32(textBox1.Text);
            var ad = txtTurAdi.Text.Trim();
            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Tür adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exec("UPDATE turler SET tur_adi=@t WHERE tur_id=@id;",
                new MySqlParameter("@t", ad),
                new MySqlParameter("@id", id));
        }

        private void SilTur()
        {
            var id = Convert.ToInt32(textBox1.Text);
            Exec("DELETE FROM turler WHERE tur_id=@id;", new MySqlParameter("@id", id));
        }

        private void EkleKullanici()
        {
            var isim = txtKullaniciIsim.Text.Trim();
            var sifre = txtKullaniciSifre.Text;
            var rol = cmbRol.SelectedItem?.ToString() ?? "kullanici";

            if (string.IsNullOrWhiteSpace(isim))
            {
                MessageBox.Show("Kullanıcı adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Şifre zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exec("INSERT INTO kullanicilar (isim, sifre, rol) VALUES (@i,@s,@r);",
                new MySqlParameter("@i", isim),
                new MySqlParameter("@s", sifre),
                new MySqlParameter("@r", rol));
        }

        private void GuncelleKullanici()
        {
            var id = Convert.ToInt32(textBox1.Text);
            var isim = txtKullaniciIsim.Text.Trim();
            var sifre = txtKullaniciSifre.Text;
            var rol = cmbRol.SelectedItem?.ToString() ?? "kullanici";

            if (string.IsNullOrWhiteSpace(isim))
            {
                MessageBox.Show("Kullanıcı adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Şifre zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exec("UPDATE kullanicilar SET isim=@i, sifre=@s, rol=@r WHERE kullanici_id=@id;",
                new MySqlParameter("@i", isim),
                new MySqlParameter("@s", sifre),
                new MySqlParameter("@r", rol),
                new MySqlParameter("@id", id));
        }

        private void SilKullanici()
        {
            var id = Convert.ToInt32(textBox1.Text);
            Exec("DELETE FROM kullanicilar WHERE kullanici_id=@id;", new MySqlParameter("@id", id));
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            textBox1.Clear();
            txtAra.Clear();
            txtSarkıIsim.Clear();
            txtAlbum.Clear();
            txtYil.Clear();
            txtSanatciAdi.Clear();
            txtTurAdi.Clear();
            txtKullaniciIsim.Clear();
            txtKullaniciSifre.Clear();

            if (cmbRol.Items.Count > 0)
            {
                cmbRol.SelectedIndex = 0;
            }

            if (cmbSanatci.Items.Count > 0)
            {
                cmbSanatci.SelectedIndex = 0;
            }

            if (cmbTur.Items.Count > 0)
            {
                cmbTur.SelectedIndex = 0;
            }

            if (cmbSarkiKullanici.Items.Count > 0)
            {
                cmbSarkiKullanici.SelectedIndex = 0;
            }
        }
    }
}
