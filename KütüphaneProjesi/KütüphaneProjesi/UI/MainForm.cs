using System.Data;
using KütüphaneProjesi.Data;
using MySql.Data.MySqlClient;

namespace KütüphaneProjesi.UI
{

public partial class MainForm : Form
{
    private readonly string _kullaniciAdi;
    private readonly string _rol;
    private readonly bool _yoneticiMi;
    private SplitContainer _anaAyrim;
    private Label _aktifKullaniciEtiketi;
    private FlowLayoutPanel _yonetimAkisPaneli;
    private Button _btnOduncKaydiGuncelle;
    private Button _btnOduncKaydiSil;
    private int _seciliOduncIslemId;

    private const string TemelKitapSorgusu = @"
SELECT k.id, k.kitap_adi, y.yazar_adi, t.tur_adi, k.yayinevi, k.basim_yili, k.stok, k.eklenme_tarihi
FROM kitaplar k
JOIN yazarlar y ON y.id = k.yazar_id
JOIN turler t ON t.id = k.tur_id";

    public MainForm() : this("Demo Kullanıcı", "Yonetici")
    {
    }

    public MainForm(string kullaniciAdi, string rol)
    {
        InitializeComponent();

        _kullaniciAdi = kullaniciAdi;
        _rol = rol;
        _yoneticiMi = string.Equals(rol, "Yonetici", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(rol, "Yönetici", StringComparison.OrdinalIgnoreCase);

        Text = _yoneticiMi
            ? $"Kütüphane Yönetim Paneli - {_kullaniciAdi} ({RolMetniGetir(_rol)})"
            : $"Kütüphane Kullanıcı Paneli - {_kullaniciAdi} ({RolMetniGetir(_rol)})";
        BtnÖdüncKitapKaydet.Click += BtnÖdüncKitapKaydet_Click;
        dataGridView2.CellClick += dataGridView2_CellClick;
        ArayuzuHazirla();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        dataGridView1.ReadOnly = true;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.MultiSelect = false;

        dataGridView2.ReadOnly = true;
        dataGridView2.AllowUserToAddRows = false;
        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        dataGridView2.ScrollBars = ScrollBars.Both;
        dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView2.MultiSelect = false;

        UygulaYetkiKurallari();
        KitaplariYukle();
        if (_yoneticiMi)
        {
            ListeleOduncIslemleri();
        }
        AnaYerlesimiGuncelle();
    }

    private void UygulaYetkiKurallari()
    {
        if (_yoneticiMi)
        {
            return;
        }

        groupBox3.Visible = false;
        groupBox4.Visible = false;
        groupBox5.Visible = false;
        btnÖdüncKitapListele.Visible = false;
        S.Visible = false;
        groupBox6.Visible = false;

        if (_anaAyrim != null)
        {
            _anaAyrim.Panel2Collapsed = true;
        }

        if (S.Parent is TableLayoutPanel altYerlesim)
        {
            altYerlesim.Visible = false;
            if (altYerlesim.Parent is TableLayoutPanel anaSolYerlesim && anaSolYerlesim.RowStyles.Count >= 2)
            {
                anaSolYerlesim.RowStyles[0].SizeType = SizeType.Percent;
                anaSolYerlesim.RowStyles[0].Height = 100F;
                anaSolYerlesim.RowStyles[1].SizeType = SizeType.Absolute;
                anaSolYerlesim.RowStyles[1].Height = 0F;
            }
        }
    }

    private bool YoneticiKontrolu()
    {
        if (_yoneticiMi)
        {
            return true;
        }

        MessageBox.Show("Bu işlem için yönetici yetkisi gerekir.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void KitaplariYukle()
    {
        var sorgu = TemelKitapSorgusu + " ORDER BY k.kitap_adi;";
        Listele(sorgu);
    }

    private void ListeleOduncIslemleri()
    {
        const string sorgu = @"
SELECT o.id AS odunc_id,
       o.kullanici_id AS kullanici_id_raw,
       o.kitap_id AS kitap_id_raw,
       o.durum AS durum_raw,
       ku.isim AS kullanici,
       k.kitap_adi AS kitap,
       DATE_FORMAT(o.odunc_tarihi, '%d.%m.%Y') AS odunc_tarihi,
       COALESCE(DATE_FORMAT(o.iade_tarihi, '%d.%m.%Y'), '-') AS iade_tarihi,
       CASE
         WHEN o.durum IN ('Oduncte', 'Ödünçte') THEN 'Ödünçte'
         WHEN o.durum IN ('IadeEdildi', 'İade Edildi') THEN 'İade Edildi'
         ELSE o.durum
       END AS durum
FROM odunc_islemleri o
JOIN kullanicilar ku ON ku.id = o.kullanici_id
JOIN kitaplar k ON k.id = o.kitap_id
ORDER BY o.odunc_tarihi DESC, o.id DESC;";

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand(sorgu, baglanti);
        using var da = new MySqlDataAdapter(komut);

        var dt = new DataTable();
        da.Fill(dt);
        dataGridView2.DataSource = dt;
        _seciliOduncIslemId = 0;
        OduncTablosuDuzeniUygula();
    }

    private void OduncTablosuDuzeniUygula()
    {
        if (dataGridView2.Columns.Count == 0)
        {
            return;
        }

        if (dataGridView2.Columns.Contains("kullanici_id_raw"))
        {
            dataGridView2.Columns["kullanici_id_raw"].Visible = false;
        }

        if (dataGridView2.Columns.Contains("kitap_id_raw"))
        {
            dataGridView2.Columns["kitap_id_raw"].Visible = false;
        }

        if (dataGridView2.Columns.Contains("durum_raw"))
        {
            dataGridView2.Columns["durum_raw"].Visible = false;
        }

        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView2.ScrollBars = ScrollBars.Vertical;

        if (dataGridView2.Columns.Contains("odunc_id"))
        {
            var kolon = dataGridView2.Columns["odunc_id"];
            kolon.HeaderText = "İşlem ID";
            kolon.FillWeight = 56;
            kolon.MinimumWidth = 58;
        }

        if (dataGridView2.Columns.Contains("kullanici"))
        {
            var kolon = dataGridView2.Columns["kullanici"];
            kolon.HeaderText = "Kullanıcı";
            kolon.FillWeight = 108;
            kolon.MinimumWidth = 88;
        }

        if (dataGridView2.Columns.Contains("kitap"))
        {
            var kolon = dataGridView2.Columns["kitap"];
            kolon.HeaderText = "Kitap";
            kolon.FillWeight = 134;
            kolon.MinimumWidth = 96;
        }

        if (dataGridView2.Columns.Contains("odunc_tarihi"))
        {
            var kolon = dataGridView2.Columns["odunc_tarihi"];
            kolon.HeaderText = "Ödünç Tarihi";
            kolon.FillWeight = 94;
            kolon.MinimumWidth = 86;
        }

        if (dataGridView2.Columns.Contains("iade_tarihi"))
        {
            var kolon = dataGridView2.Columns["iade_tarihi"];
            kolon.HeaderText = "İade Tarihi";
            kolon.FillWeight = 88;
            kolon.MinimumWidth = 82;
        }

        if (dataGridView2.Columns.Contains("durum"))
        {
            var kolon = dataGridView2.Columns["durum"];
            kolon.HeaderText = "Durum";
            kolon.FillWeight = 84;
            kolon.MinimumWidth = 80;
        }
    }

    private void Listele(string sorgu, string paramName = null, string paramValue = null)
    {
        MySqlParameter[] parametreler = null;
        if (!string.IsNullOrWhiteSpace(paramName))
        {
            parametreler = new[] { new MySqlParameter(paramName, paramValue ?? string.Empty) };
        }

        Listele(sorgu, parametreler);
    }

    private void Listele(string sorgu, MySqlParameter[] parametreler)
    {
        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand(sorgu, baglanti);

        if (parametreler is { Length: > 0 })
        {
            komut.Parameters.AddRange(parametreler);
        }

        using var da = new MySqlDataAdapter(komut);
        var dt = new DataTable();
        da.Fill(dt);

        dataGridView1.DataSource = dt;
    }

    private void btnIsmeGoreAra_Click_1(object sender, EventArgs e)
    {
        var kitapAdi = txtKitapAdi.Text.Trim();
        var sorgu = TemelKitapSorgusu + " WHERE k.kitap_adi LIKE @kitapAdi ORDER BY k.kitap_adi;";
        Listele(sorgu, "@kitapAdi", $"%{kitapAdi}%");
    }

    private void btnYazaraGoreAra_Click(object sender, EventArgs e)
    {
        var yazar = txtYazar.Text.Trim();
        var sorgu = TemelKitapSorgusu + " WHERE y.yazar_adi LIKE @yazar ORDER BY k.kitap_adi;";
        Listele(sorgu, "@yazar", $"%{yazar}%");
    }

    private void btnTureGoreAra_Click(object sender, EventArgs e)
    {
        var tur = txtTur.Text.Trim();
        var sorgu = TemelKitapSorgusu + " WHERE t.tur_adi LIKE @tur ORDER BY k.kitap_adi;";
        Listele(sorgu, "@tur", $"%{tur}%");
    }

    private void btnTumKitaplariListele_Click(object sender, EventArgs e)
    {
        KitaplariYukle();
    }

    private void btnYazaraGoreListele_Click(object sender, EventArgs e)
    {
        var filtre = textBox5.Text.Trim();
        var sorgu = string.IsNullOrWhiteSpace(filtre)
            ? "SELECT id, yazar_adi FROM yazarlar ORDER BY yazar_adi;"
            : "SELECT id, yazar_adi FROM yazarlar WHERE yazar_adi LIKE @yazar ORDER BY yazar_adi;";

        if (string.IsNullOrWhiteSpace(filtre))
        {
            Listele(sorgu);
            return;
        }

        Listele(sorgu, "@yazar", $"%{filtre}%");
    }

    private void btnTureGoreListele_Click(object sender, EventArgs e)
    {
        var filtre = textBox5.Text.Trim();
        var sorgu = string.IsNullOrWhiteSpace(filtre)
            ? "SELECT id, tur_adi FROM turler ORDER BY tur_adi;"
            : "SELECT id, tur_adi FROM turler WHERE tur_adi LIKE @tur ORDER BY tur_adi;";

        if (string.IsNullOrWhiteSpace(filtre))
        {
            Listele(sorgu);
            return;
        }

        Listele(sorgu, "@tur", $"%{filtre}%");
    }

    private void btnKullaniciListele_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        const string sorgu = "SELECT id, isim, rol, kayit_tarihi FROM kullanicilar ORDER BY id;";
        Listele(sorgu);
    }

    private void btnYetkiDuzenle_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (!int.TryParse(txtKullaniciID.Text.Trim(), out var kullaniciId))
        {
            MessageBox.Show("Lütfen geçerli bir kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var rol = radioAdmin.Checked ? "Yonetici" : "Kullanici";

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("UPDATE kullanicilar SET rol=@rol WHERE id=@id;", baglanti);
        komut.Parameters.AddWithValue("@rol", rol);
        komut.Parameters.AddWithValue("@id", kullaniciId);

        var sonuc = komut.ExecuteNonQuery();
        MessageBox.Show(sonuc > 0 ? "Yetki güncellendi." : "Kullanıcı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        btnKullaniciListele_Click(sender, e);
    }

    private void btnYazarEkle_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        var yazarAdi = txtYazarAdi.Text.Trim();
        if (string.IsNullOrWhiteSpace(yazarAdi))
        {
            MessageBox.Show("Lütfen yazar adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("INSERT IGNORE INTO yazarlar (yazar_adi) VALUES (@ad);", baglanti);
        komut.Parameters.AddWithValue("@ad", yazarAdi);
        komut.ExecuteNonQuery();

        txtYazarAdi.Clear();
        button1_Click(sender, e);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        const string sorgu = "SELECT id, yazar_adi FROM yazarlar ORDER BY yazar_adi;";
        Listele(sorgu);
    }

    private void btnTurEkle_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        var turAdi = txtTurAdi.Text.Trim();
        if (string.IsNullOrWhiteSpace(turAdi))
        {
            MessageBox.Show("Lütfen tür adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("INSERT IGNORE INTO turler (tur_adi) VALUES (@ad);", baglanti);
        komut.Parameters.AddWithValue("@ad", turAdi);
        komut.ExecuteNonQuery();

        txtTurAdi.Clear();
        button2_Click(sender, e);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        const string sorgu = "SELECT id, tur_adi FROM turler ORDER BY tur_adi;";
        Listele(sorgu);
    }

    private void btnKitapEkle_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (!KitapBilgisiGecerli(out var basimYili))
        {
            return;
        }

        var kitapAdi = txtKitapKullaniciAdi.Text.Trim();
        var yazarId = YazarIdGetirVeyaOlustur(txtKitapYazar.Text.Trim());
        var turId = TurIdGetirVeyaOlustur(txtKitapTur.Text.Trim());

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand(
            @"INSERT INTO kitaplar (kitap_adi, yazar_id, tur_id, yayinevi, basim_yili, stok)
              VALUES (@kitapAdi, @yazarId, @turId, @yayinevi, @basimYili, @stok);",
            baglanti);

        komut.Parameters.AddWithValue("@kitapAdi", kitapAdi);
        komut.Parameters.AddWithValue("@yazarId", yazarId);
        komut.Parameters.AddWithValue("@turId", turId);
        komut.Parameters.AddWithValue("@yayinevi", txtKitapYayinEvi.Text.Trim());
        komut.Parameters.AddWithValue("@basimYili", basimYili);
        komut.Parameters.AddWithValue("@stok", 5);
        komut.ExecuteNonQuery();

        MessageBox.Show("Kitap eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        KitapAlanlariniTemizle();
        KitaplariYukle();
    }

    private void btnKitapGuncelle_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (!int.TryParse(txtKitapID.Text.Trim(), out var kitapId))
        {
            MessageBox.Show("Güncellemek için geçerli bir kitap seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!KitapBilgisiGecerli(out var basimYili))
        {
            return;
        }

        var yazarId = YazarIdGetirVeyaOlustur(txtKitapYazar.Text.Trim());
        var turId = TurIdGetirVeyaOlustur(txtKitapTur.Text.Trim());

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand(
            @"UPDATE kitaplar
              SET kitap_adi = @kitapAdi,
                  yazar_id = @yazarId,
                  tur_id = @turId,
                  yayinevi = @yayinevi,
                  basim_yili = @basimYili
              WHERE id = @id;",
            baglanti);

        komut.Parameters.AddWithValue("@id", kitapId);
        komut.Parameters.AddWithValue("@kitapAdi", txtKitapKullaniciAdi.Text.Trim());
        komut.Parameters.AddWithValue("@yazarId", yazarId);
        komut.Parameters.AddWithValue("@turId", turId);
        komut.Parameters.AddWithValue("@yayinevi", txtKitapYayinEvi.Text.Trim());
        komut.Parameters.AddWithValue("@basimYili", basimYili);

        var sonuc = komut.ExecuteNonQuery();
        MessageBox.Show(sonuc > 0 ? "Kitap güncellendi." : "Kitap bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        KitaplariYukle();
    }

    private void btnKitapSil_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (!int.TryParse(txtKitapID.Text.Trim(), out var kitapId))
        {
            MessageBox.Show("Silmek için geçerli bir kitap seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var onay = MessageBox.Show("Seçili kitap silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (onay != DialogResult.Yes)
        {
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("DELETE FROM kitaplar WHERE id=@id;", baglanti);
        komut.Parameters.AddWithValue("@id", kitapId);

        var sonuc = komut.ExecuteNonQuery();
        MessageBox.Show(sonuc > 0 ? "Kitap silindi." : "Kitap bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        KitapAlanlariniTemizle();
        KitaplariYukle();
    }

    private void Sil_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (!int.TryParse(txtKullaniciID.Text.Trim(), out var kullaniciId))
        {
            MessageBox.Show("Silmek için geçerli bir kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("DELETE FROM kullanicilar WHERE id=@id AND rol <> 'Yonetici';", baglanti);
        komut.Parameters.AddWithValue("@id", kullaniciId);

        var sonuc = komut.ExecuteNonQuery();
        MessageBox.Show(sonuc > 0 ? "Kullanıcı silindi." : "Yönetici hesabı silinemez veya kullanıcı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        btnKullaniciListele_Click(sender, e);
    }

    private void btnYazarSil_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        var yazarAdi = txtYazarAdi.Text.Trim();
        if (string.IsNullOrWhiteSpace(yazarAdi))
        {
            MessageBox.Show("Lütfen silinecek yazarı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("DELETE FROM yazarlar WHERE yazar_adi=@ad;", baglanti);
        komut.Parameters.AddWithValue("@ad", yazarAdi);
        komut.ExecuteNonQuery();

        button1_Click(sender, e);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        var turAdi = txtTurAdi.Text.Trim();
        if (string.IsNullOrWhiteSpace(turAdi))
        {
            MessageBox.Show("Lütfen silinecek türü girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand("DELETE FROM turler WHERE tur_adi=@ad;", baglanti);
        komut.Parameters.AddWithValue("@ad", turAdi);
        komut.ExecuteNonQuery();

        button2_Click(sender, e);
    }

    private void BtnÖdüncKitapKaydet_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (!OduncGirdisiGecerli(out var kullaniciId, out var kitapId, out var oduncTarihi))
        {
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();
        using var tx = baglanti.BeginTransaction();

        try
        {
            using var kullaniciKontrol = new MySqlCommand("SELECT COUNT(*) FROM kullanicilar WHERE id=@id;", baglanti, tx);
            kullaniciKontrol.Parameters.AddWithValue("@id", kullaniciId);
            var kullaniciVar = Convert.ToInt32(kullaniciKontrol.ExecuteScalar()) > 0;
            if (!kullaniciVar)
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }

            using var stokSorgu = new MySqlCommand("SELECT stok FROM kitaplar WHERE id=@id FOR UPDATE;", baglanti, tx);
            stokSorgu.Parameters.AddWithValue("@id", kitapId);
            var stokObj = stokSorgu.ExecuteScalar();
            if (stokObj is null || stokObj == DBNull.Value)
            {
                throw new InvalidOperationException("Kitap bulunamadı.");
            }

            var stok = Convert.ToInt32(stokObj);
            if (stok <= 0)
            {
                throw new InvalidOperationException("Seçili kitapta stok kalmadı.");
            }

            using var oduncEkle = new MySqlCommand(
                @"INSERT INTO odunc_islemleri (kullanici_id, kitap_id, odunc_tarihi, durum)
                  VALUES (@kullaniciId, @kitapId, @oduncTarihi, 'Ödünçte');",
                baglanti,
                tx);

            oduncEkle.Parameters.AddWithValue("@kullaniciId", kullaniciId);
            oduncEkle.Parameters.AddWithValue("@kitapId", kitapId);
            oduncEkle.Parameters.AddWithValue("@oduncTarihi", oduncTarihi);
            oduncEkle.ExecuteNonQuery();

            using var stokDus = new MySqlCommand("UPDATE kitaplar SET stok = stok - 1 WHERE id=@id;", baglanti, tx);
            stokDus.Parameters.AddWithValue("@id", kitapId);
            stokDus.ExecuteNonQuery();

            tx.Commit();

            MessageBox.Show("Ödünç kaydı oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OduncAlanlariniTemizle();
            ListeleOduncIslemleri();
            KitaplariYukle();
        }
        catch (Exception ex)
        {
            tx.Rollback();
            MessageBox.Show("Ödünç kaydı oluşturulamadı:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnÖdüncKaydiGuncelle_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (_seciliOduncIslemId <= 0)
        {
            MessageBox.Show("Lütfen güncellenecek ödünç kaydını tablodan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!OduncGirdisiGecerli(out var kullaniciId, out var kitapId, out var oduncTarihi))
        {
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();
        using var tx = baglanti.BeginTransaction();

        try
        {
            var eskiKitapId = 0;
            var eskiDurum = string.Empty;

            using (var mevcutKayit = new MySqlCommand("SELECT kitap_id, durum FROM odunc_islemleri WHERE id=@id FOR UPDATE;", baglanti, tx))
            {
                mevcutKayit.Parameters.AddWithValue("@id", _seciliOduncIslemId);
                using var okuyucu = mevcutKayit.ExecuteReader();
                if (!okuyucu.Read())
                {
                    throw new InvalidOperationException("Güncellenecek ödünç kaydı bulunamadı.");
                }

                eskiKitapId = Convert.ToInt32(okuyucu["kitap_id"]);
                eskiDurum = Convert.ToString(okuyucu["durum"]) ?? string.Empty;
            }

            using (var kullaniciKontrol = new MySqlCommand("SELECT COUNT(*) FROM kullanicilar WHERE id=@id;", baglanti, tx))
            {
                kullaniciKontrol.Parameters.AddWithValue("@id", kullaniciId);
                if (Convert.ToInt32(kullaniciKontrol.ExecuteScalar()) <= 0)
                {
                    throw new InvalidOperationException("Kullanıcı bulunamadı.");
                }
            }

            var yeniKitapStok = 0;
            using (var kitapKontrol = new MySqlCommand("SELECT stok FROM kitaplar WHERE id=@id FOR UPDATE;", baglanti, tx))
            {
                kitapKontrol.Parameters.AddWithValue("@id", kitapId);
                var stokObj = kitapKontrol.ExecuteScalar();
                if (stokObj is null || stokObj == DBNull.Value)
                {
                    throw new InvalidOperationException("Kitap bulunamadı.");
                }

                yeniKitapStok = Convert.ToInt32(stokObj);
            }

            if (eskiKitapId != kitapId && DurumStoktaAzaltmaGerektirir(eskiDurum))
            {
                if (yeniKitapStok <= 0)
                {
                    throw new InvalidOperationException("Yeni seçilen kitapta stok kalmadı.");
                }

                using var eskiStokArtir = new MySqlCommand("UPDATE kitaplar SET stok = stok + 1 WHERE id=@id;", baglanti, tx);
                eskiStokArtir.Parameters.AddWithValue("@id", eskiKitapId);
                eskiStokArtir.ExecuteNonQuery();

                using var yeniStokDus = new MySqlCommand("UPDATE kitaplar SET stok = stok - 1 WHERE id=@id;", baglanti, tx);
                yeniStokDus.Parameters.AddWithValue("@id", kitapId);
                yeniStokDus.ExecuteNonQuery();
            }

            using var guncelle = new MySqlCommand(
                @"UPDATE odunc_islemleri
                  SET kullanici_id = @kullaniciId,
                      kitap_id = @kitapId,
                      odunc_tarihi = @oduncTarihi
                  WHERE id = @id;",
                baglanti,
                tx);

            guncelle.Parameters.AddWithValue("@kullaniciId", kullaniciId);
            guncelle.Parameters.AddWithValue("@kitapId", kitapId);
            guncelle.Parameters.AddWithValue("@oduncTarihi", oduncTarihi);
            guncelle.Parameters.AddWithValue("@id", _seciliOduncIslemId);
            guncelle.ExecuteNonQuery();

            tx.Commit();

            MessageBox.Show("Ödünç kaydı güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OduncAlanlariniTemizle();
            ListeleOduncIslemleri();
            KitaplariYukle();
        }
        catch (Exception ex)
        {
            tx.Rollback();
            MessageBox.Show("Ödünç kaydı güncellenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnÖdüncKaydiSil_Click(object sender, EventArgs e)
    {
        if (!YoneticiKontrolu())
        {
            return;
        }

        if (_seciliOduncIslemId <= 0)
        {
            MessageBox.Show("Lütfen silinecek ödünç kaydını tablodan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var onay = MessageBox.Show("Seçili ödünç kaydı silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (onay != DialogResult.Yes)
        {
            return;
        }

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();
        using var tx = baglanti.BeginTransaction();

        try
        {
            var kitapId = 0;
            var durum = string.Empty;

            using (var sec = new MySqlCommand("SELECT kitap_id, durum FROM odunc_islemleri WHERE id=@id FOR UPDATE;", baglanti, tx))
            {
                sec.Parameters.AddWithValue("@id", _seciliOduncIslemId);
                using var okuyucu = sec.ExecuteReader();
                if (!okuyucu.Read())
                {
                    throw new InvalidOperationException("Silinecek ödünç kaydı bulunamadı.");
                }

                kitapId = Convert.ToInt32(okuyucu["kitap_id"]);
                durum = Convert.ToString(okuyucu["durum"]) ?? string.Empty;
            }

            using (var sil = new MySqlCommand("DELETE FROM odunc_islemleri WHERE id=@id;", baglanti, tx))
            {
                sil.Parameters.AddWithValue("@id", _seciliOduncIslemId);
                var etkilenen = sil.ExecuteNonQuery();
                if (etkilenen <= 0)
                {
                    throw new InvalidOperationException("Ödünç kaydı silinemedi.");
                }
            }

            if (DurumStoktaAzaltmaGerektirir(durum))
            {
                using var stokArtir = new MySqlCommand("UPDATE kitaplar SET stok = stok + 1 WHERE id=@id;", baglanti, tx);
                stokArtir.Parameters.AddWithValue("@id", kitapId);
                stokArtir.ExecuteNonQuery();
            }

            tx.Commit();

            MessageBox.Show("Ödünç kaydı silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OduncAlanlariniTemizle();
            ListeleOduncIslemleri();
            KitaplariYukle();
        }
        catch (Exception ex)
        {
            tx.Rollback();
            MessageBox.Show("Ödünç kaydı silinemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnÖdüncKitapListele_Click(object sender, EventArgs e)
    {
        if (!_yoneticiMi)
        {
            return;
        }

        OduncAlanlariniTemizle();
        ListeleOduncIslemleri();
        AnaYerlesimiGuncelle();
    }

    private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        var row = dataGridView2.Rows[e.RowIndex];
        if (!int.TryParse(HucreDegeriGuvenli(row, "odunc_id"), out _seciliOduncIslemId))
        {
            _seciliOduncIslemId = 0;
            return;
        }

        txtÖdüncKullanıcıİd.Text = HucreDegeriGuvenli(row, "kullanici_id_raw");
        txtÖdüncKitapİd.Text = HucreDegeriGuvenli(row, "kitap_id_raw");
        txtÖdüncTarih.Text = HucreDegeriGuvenli(row, "odunc_tarihi");
    }

    private bool OduncGirdisiGecerli(out int kullaniciId, out int kitapId, out DateTime oduncTarihi)
    {
        kullaniciId = 0;
        kitapId = 0;
        oduncTarihi = DateTime.Today;

        if (!int.TryParse(txtÖdüncKullanıcıİd.Text.Trim(), out kullaniciId) ||
            !int.TryParse(txtÖdüncKitapİd.Text.Trim(), out kitapId))
        {
            MessageBox.Show("Ödünç kaydı için geçerli kullanıcı ve kitap ID girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(txtÖdüncTarih.Text) && !DateTime.TryParse(txtÖdüncTarih.Text.Trim(), out oduncTarihi))
        {
            MessageBox.Show("Ödünç tarihi geçersiz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private void OduncAlanlariniTemizle()
    {
        _seciliOduncIslemId = 0;
        txtÖdüncKullanıcıİd.Clear();
        txtÖdüncKitapİd.Clear();
        txtÖdüncTarih.Clear();
        dataGridView2.ClearSelection();
    }

    private static bool DurumStoktaAzaltmaGerektirir(string durum)
    {
        return string.Equals(durum, "Oduncte", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(durum, "Ödünçte", StringComparison.OrdinalIgnoreCase);
    }

    private static string HucreDegeriGuvenli(DataGridViewRow row, string alan)
    {
        if (row.DataGridView?.Columns.Contains(alan) != true)
        {
            return string.Empty;
        }

        return Convert.ToString(row.Cells[alan].Value) ?? string.Empty;
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        var row = dataGridView1.Rows[e.RowIndex];

        if (dataGridView1.Columns.Contains("rol"))
        {
            txtKullaniciID.Text = HucreDegeri(row, "id");
            txtKullaniciAdi.Text = HucreDegeri(row, "isim");
            var rol = HucreDegeri(row, "rol");
            radioAdmin.Checked = string.Equals(rol, "Yonetici", StringComparison.OrdinalIgnoreCase);
            radioKullanici.Checked = !radioAdmin.Checked;
            return;
        }

        if (dataGridView1.Columns.Contains("yazar_adi") && !dataGridView1.Columns.Contains("kitap_adi"))
        {
            txtYazarAdi.Text = HucreDegeri(row, "yazar_adi");
            return;
        }

        if (dataGridView1.Columns.Contains("tur_adi") && !dataGridView1.Columns.Contains("kitap_adi"))
        {
            txtTurAdi.Text = HucreDegeri(row, "tur_adi");
            return;
        }

        if (dataGridView1.Columns.Contains("kitap_adi"))
        {
            txtKitapID.Text = HucreDegeri(row, "id");
            txtKitapKullaniciAdi.Text = HucreDegeri(row, "kitap_adi");
            txtKitapYazar.Text = HucreDegeri(row, "yazar_adi");
            txtKitapTur.Text = HucreDegeri(row, "tur_adi");
            txtKitapYayinEvi.Text = HucreDegeri(row, "yayinevi");
            txtKitapBasimYili.Text = HucreDegeri(row, "basim_yili");
        }
    }

    private static string HucreDegeri(DataGridViewRow row, string alan)
    {
        return Convert.ToString(row.Cells[alan].Value) ?? string.Empty;
    }

    private bool KitapBilgisiGecerli(out int basimYili)
    {
        basimYili = 0;

        if (string.IsNullOrWhiteSpace(txtKitapKullaniciAdi.Text) ||
            string.IsNullOrWhiteSpace(txtKitapYazar.Text) ||
            string.IsNullOrWhiteSpace(txtKitapTur.Text) ||
            string.IsNullOrWhiteSpace(txtKitapYayinEvi.Text) ||
            string.IsNullOrWhiteSpace(txtKitapBasimYili.Text))
        {
            MessageBox.Show("Lütfen tüm kitap bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(txtKitapBasimYili.Text.Trim(), out basimYili))
        {
            MessageBox.Show("Basım yılı sayısal olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private int YazarIdGetirVeyaOlustur(string yazarAdi)
    {
        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var sec = new MySqlCommand("SELECT id FROM yazarlar WHERE yazar_adi=@ad LIMIT 1;", baglanti);
        sec.Parameters.AddWithValue("@ad", yazarAdi);
        var mevcut = sec.ExecuteScalar();

        if (mevcut is not null && mevcut != DBNull.Value)
        {
            return Convert.ToInt32(mevcut);
        }

        using var ekle = new MySqlCommand("INSERT INTO yazarlar (yazar_adi) VALUES (@ad); SELECT LAST_INSERT_ID();", baglanti);
        ekle.Parameters.AddWithValue("@ad", yazarAdi);
        return Convert.ToInt32(ekle.ExecuteScalar());
    }

    private int TurIdGetirVeyaOlustur(string turAdi)
    {
        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var sec = new MySqlCommand("SELECT id FROM turler WHERE tur_adi=@ad LIMIT 1;", baglanti);
        sec.Parameters.AddWithValue("@ad", turAdi);
        var mevcut = sec.ExecuteScalar();

        if (mevcut is not null && mevcut != DBNull.Value)
        {
            return Convert.ToInt32(mevcut);
        }

        using var ekle = new MySqlCommand("INSERT INTO turler (tur_adi) VALUES (@ad); SELECT LAST_INSERT_ID();", baglanti);
        ekle.Parameters.AddWithValue("@ad", turAdi);
        return Convert.ToInt32(ekle.ExecuteScalar());
    }

    private void KitapAlanlariniTemizle()
    {
        txtKitapID.Clear();
        txtKitapKullaniciAdi.Clear();
        txtKitapYazar.Clear();
        txtKitapTur.Clear();
        txtKitapYayinEvi.Clear();
        txtKitapBasimYili.Clear();
    }

    private void txtKitapID_Click(object sender, EventArgs e)
    {
        txtKitapID.Clear();
    }

    private void txtKitapKullaniciAdi_Click(object sender, EventArgs e)
    {
        txtKitapKullaniciAdi.Clear();
    }

    private void txtKitapYazar_Click(object sender, EventArgs e)
    {
        txtKitapYazar.Clear();
    }

    private void txtKitapTur_Click(object sender, EventArgs e)
    {
        txtKitapTur.Clear();
    }

    private void txtKitapYayinEvi_Click(object sender, EventArgs e)
    {
        txtKitapYayinEvi.Clear();
    }

    private void txtKitapBasimYili_Click(object sender, EventArgs e)
    {
        txtKitapBasimYili.Clear();
    }

    private void txtTurAdi_Click(object sender, EventArgs e)
    {
        txtTurAdi.Clear();
    }

    private void txtYazarAdi_Click(object sender, EventArgs e)
    {
        txtYazarAdi.Clear();
    }
}
}
















