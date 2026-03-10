using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MüzikKütüphaneUygulaması
{
    public partial class Form2 : Form
    {
        private readonly string _connStr = ConfigurationManager.AppSettings["DbConnectionString"]
            ?? "Server=localhost;Database=muzik_kutuphanesi;Uid=root;Pwd=123456789;Charset=utf8mb4;";

        public Form2()
        {
            InitializeComponent();
            KullaniciTemasiniUygula();
        }

        private void KullaniciTemasiniUygula()
        {
            Text = "Müzik Kütüphanesi | Kullanıcı Paneli";
            BackColor = Color.FromArgb(10, 25, 47);
            ForeColor = Color.FromArgb(233, 243, 255);
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            groupBox1.ForeColor = ForeColor;
            groupBox2.ForeColor = ForeColor;

            btnAra.Text = "Ara";
            btnTumSarkilar.Text = "Tüm Şarkıları Listele";
            btnTureGoreListele.Text = "Sanatçıları Listele";
            btnSanatcilariListele.Text = "Türleri Listele";
            btnTemizle.Text = "Filtreleri Temizle";

            StilButon(btnAra, true);
            StilButon(btnTumSarkilar, false);
            StilButon(btnTureGoreListele, false);
            StilButon(btnSanatcilariListele, false);
            StilButon(btnTemizle, false);

            StilGirdi(txtSarkiAdi);
            StilGirdi(txtSanatciAdi);
            StilSecim(cmbTur);

            dgv.BackgroundColor = Color.FromArgb(13, 33, 63);
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(36, 83, 158);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(228, 238, 253);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(23, 35, 58);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(57, 107, 187);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(214, 228, 250);
        }

        private static void StilButon(Button buton, bool birincil)
        {
            buton.FlatStyle = FlatStyle.Flat;
            buton.FlatAppearance.BorderSize = 0;
            buton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            buton.ForeColor = Color.White;
            buton.BackColor = birincil ? Color.FromArgb(29, 78, 216) : Color.FromArgb(30, 58, 95);
        }

        private static void StilGirdi(TextBox kutu)
        {
            kutu.BorderStyle = BorderStyle.FixedSingle;
            kutu.BackColor = Color.White;
            kutu.ForeColor = Color.FromArgb(22, 34, 55);
        }

        private static void StilSecim(ComboBox kutu)
        {
            kutu.FlatStyle = FlatStyle.Flat;
            kutu.BackColor = Color.White;
            kutu.ForeColor = Color.FromArgb(22, 34, 55);
            kutu.DropDownStyle = ComboBoxStyle.DropDownList;
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

        private void BindGrid(DataTable dt)
        {
            dgv.AutoGenerateColumns = true;
            dgv.DataSource = dt;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                var dtTur = GetTable("SELECT tur_id, tur_adi FROM turler ORDER BY tur_adi;");
                cmbTur.DisplayMember = "tur_adi";
                cmbTur.ValueMember = "tur_id";
                cmbTur.DataSource = dtTur;
                cmbTur.SelectedIndex = -1;

                ListeleTumSarkilar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListeleTumSarkilar()
        {
            const string sql = @"
SELECT
  s.sarki_id,
  s.isim AS sarki_adi,
  sa.sanatci_adi,
  t.tur_adi,
  s.album,
  s.yil
FROM sarkilar s
JOIN sanatcilar sa ON sa.sanatci_id = s.sanatci_id
JOIN turler t ON t.tur_id = s.tur_id
ORDER BY s.sarki_id DESC;";

            BindGrid(GetTable(sql));
        }

        private void btnTureGoreListele_Click(object sender, EventArgs e)
        {
            const string sql = "SELECT sanatci_id, sanatci_adi FROM sanatcilar ORDER BY sanatci_adi;";
            BindGrid(GetTable(sql));
        }

        private void btnSanatcilariListele_Click(object sender, EventArgs e)
        {
            const string sql = "SELECT tur_id, tur_adi FROM turler ORDER BY tur_adi;";
            BindGrid(GetTable(sql));
        }

        private void btnTumSarkilar_Click(object sender, EventArgs e)
        {
            ListeleTumSarkilar();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            var sarkiAdi = txtSarkiAdi.Text.Trim();
            var sanatciAdi = txtSanatciAdi.Text.Trim();
            int? turId = null;

            if (cmbTur.SelectedIndex >= 0 && cmbTur.SelectedValue != null && int.TryParse(cmbTur.SelectedValue.ToString(), out var parsedTurId))
            {
                turId = parsedTurId;
            }

            var sql = @"
SELECT
  s.sarki_id,
  s.isim AS sarki_adi,
  sa.sanatci_adi,
  t.tur_adi,
  s.album,
  s.yil
FROM sarkilar s
JOIN sanatcilar sa ON sa.sanatci_id = s.sanatci_id
JOIN turler t ON t.tur_id = s.tur_id
WHERE 1=1";

            var prms = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(sarkiAdi))
            {
                sql += " AND s.isim LIKE @sarkiAdi";
                prms.Add(new MySqlParameter("@sarkiAdi", "%" + sarkiAdi + "%"));
            }

            if (!string.IsNullOrWhiteSpace(sanatciAdi))
            {
                sql += " AND sa.sanatci_adi LIKE @sanatciAdi";
                prms.Add(new MySqlParameter("@sanatciAdi", "%" + sanatciAdi + "%"));
            }

            if (turId.HasValue)
            {
                sql += " AND s.tur_id = @turId";
                prms.Add(new MySqlParameter("@turId", turId.Value));
            }

            sql += " ORDER BY s.sarki_id DESC;";
            BindGrid(GetTable(sql, prms.ToArray()));
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            txtSarkiAdi.Clear();
            txtSanatciAdi.Clear();
            cmbTur.SelectedIndex = -1;
            ListeleTumSarkilar();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
