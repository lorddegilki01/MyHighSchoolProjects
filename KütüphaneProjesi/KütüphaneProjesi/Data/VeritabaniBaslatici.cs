using KütüphaneProjesi.Security;
using MySql.Data.MySqlClient;

namespace KütüphaneProjesi.Data
{

public static class VeritabaniBaslatici
{
    private const int HedefKayit = 200;

    public static void Baslat()
    {
        VeritabaniniOlustur();

        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        TablolariOlustur(baglanti);
        VerileriHazirla(baglanti);
    }

    private static void VeritabaniniOlustur()
    {
        using var baglanti = new MySqlConnection(Veritabani.SunucuBaglantiCumlesi);
        baglanti.Open();

        using var komut = new MySqlCommand(
            "CREATE DATABASE IF NOT EXISTS kutuphane CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;",
            baglanti);
        komut.ExecuteNonQuery();
    }

    private static void TablolariOlustur(MySqlConnection baglanti)
    {
        var scriptler = new[]
        {
            @"CREATE TABLE IF NOT EXISTS kullanicilar (
                id INT AUTO_INCREMENT PRIMARY KEY,
                isim VARCHAR(120) NOT NULL UNIQUE,
                sifre_hash VARCHAR(500) NOT NULL,
                rol ENUM('Kullanici','Yonetici') NOT NULL DEFAULT 'Kullanici',
                kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            );",
            @"CREATE TABLE IF NOT EXISTS yazarlar (
                id INT AUTO_INCREMENT PRIMARY KEY,
                yazar_adi VARCHAR(180) NOT NULL UNIQUE
            );",
            @"CREATE TABLE IF NOT EXISTS turler (
                id INT AUTO_INCREMENT PRIMARY KEY,
                tur_adi VARCHAR(180) NOT NULL UNIQUE
            );",
            @"CREATE TABLE IF NOT EXISTS kitaplar (
                id INT AUTO_INCREMENT PRIMARY KEY,
                kitap_adi VARCHAR(220) NOT NULL,
                yazar_id INT NOT NULL,
                tur_id INT NOT NULL,
                yayinevi VARCHAR(180),
                basim_yili INT,
                stok INT NOT NULL DEFAULT 5,
                eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UNIQUE KEY uq_kitap_yazar (kitap_adi, yazar_id),
                FOREIGN KEY (yazar_id) REFERENCES yazarlar(id) ON UPDATE CASCADE ON DELETE RESTRICT,
                FOREIGN KEY (tur_id) REFERENCES turler(id) ON UPDATE CASCADE ON DELETE RESTRICT
            );",
            @"CREATE TABLE IF NOT EXISTS odunc_islemleri (
                id INT AUTO_INCREMENT PRIMARY KEY,
                kullanici_id INT NOT NULL,
                kitap_id INT NOT NULL,
                odunc_tarihi DATE NOT NULL,
                iade_tarihi DATE NULL,
                durum ENUM('Oduncte','IadeEdildi') NOT NULL DEFAULT 'Oduncte',
                FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(id) ON UPDATE CASCADE ON DELETE CASCADE,
                FOREIGN KEY (kitap_id) REFERENCES kitaplar(id) ON UPDATE CASCADE ON DELETE CASCADE
            );"
        };

        foreach (var script in scriptler)
        {
            using var komut = new MySqlCommand(script, baglanti);
            komut.ExecuteNonQuery();
        }
    }

    private static void VerileriHazirla(MySqlConnection baglanti)
    {
        HesapOlusturVeyaGuncelle(baglanti, "admin", "Admin123!", "Yonetici");
        HesapOlusturVeyaGuncelle(baglanti, "kullanici", "Kullanici123!", "Kullanici");

        KullanicilariTamamla(baglanti, HedefKayit);
        YazarlariTamamla(baglanti, HedefKayit);
        TurleriTamamla(baglanti, HedefKayit);
        KitaplariTamamla(baglanti, HedefKayit);
        OduncKayitlariniTamamla(baglanti, HedefKayit);
    }

    private static void HesapOlusturVeyaGuncelle(MySqlConnection baglanti, string isim, string sifre, string rol)
    {
        using var komut = new MySqlCommand(
            @"INSERT INTO kullanicilar (isim, sifre_hash, rol)
              VALUES (@isim, @hash, @rol)
              ON DUPLICATE KEY UPDATE
                sifre_hash = VALUES(sifre_hash),
                rol = VALUES(rol);",
            baglanti);

        komut.Parameters.AddWithValue("@isim", isim);
        komut.Parameters.AddWithValue("@hash", SifreYoneticisi.Hashle(sifre));
        komut.Parameters.AddWithValue("@rol", rol);
        komut.ExecuteNonQuery();
    }

    private static void KullanicilariTamamla(MySqlConnection baglanti, int hedef)
    {
        var sayi = KayitSayisi(baglanti, "kullanicilar");
        var i = 1;

        while (sayi < hedef)
        {
            using var komut = new MySqlCommand(
                @"INSERT IGNORE INTO kullanicilar (isim, sifre_hash, rol, kayit_tarihi)
                  VALUES (@isim, @hash, @rol, @kayit);",
                baglanti);

            komut.Parameters.AddWithValue("@isim", $"uye{i:000}");
            komut.Parameters.AddWithValue("@hash", SifreYoneticisi.Hashle("Kullanici123!"));
            komut.Parameters.AddWithValue("@rol", i % 25 == 0 ? "Yonetici" : "Kullanici");
            komut.Parameters.AddWithValue("@kayit", DateTime.Now.AddDays(-i));

            if (komut.ExecuteNonQuery() > 0)
            {
                sayi++;
            }

            i++;
        }
    }

    private static void YazarlariTamamla(MySqlConnection baglanti, int hedef)
    {
        var sayi = KayitSayisi(baglanti, "yazarlar");
        var i = 1;

        while (sayi < hedef)
        {
            using var komut = new MySqlCommand("INSERT IGNORE INTO yazarlar (yazar_adi) VALUES (@ad);", baglanti);
            komut.Parameters.AddWithValue("@ad", $"Yazar {i:000}");

            if (komut.ExecuteNonQuery() > 0)
            {
                sayi++;
            }

            i++;
        }
    }

    private static void TurleriTamamla(MySqlConnection baglanti, int hedef)
    {
        var sayi = KayitSayisi(baglanti, "turler");
        var i = 1;

        while (sayi < hedef)
        {
            using var komut = new MySqlCommand("INSERT IGNORE INTO turler (tur_adi) VALUES (@ad);", baglanti);
            komut.Parameters.AddWithValue("@ad", $"Tür {i:000}");

            if (komut.ExecuteNonQuery() > 0)
            {
                sayi++;
            }

            i++;
        }
    }

    private static void KitaplariTamamla(MySqlConnection baglanti, int hedef)
    {
        var sayi = KayitSayisi(baglanti, "kitaplar");
        if (sayi >= hedef)
        {
            return;
        }

        var yazarIdleri = IdleriGetir(baglanti, "yazarlar", "id");
        var turIdleri = IdleriGetir(baglanti, "turler", "id");

        if (yazarIdleri.Count == 0 || turIdleri.Count == 0)
        {
            return;
        }

        var i = sayi + 1;
        while (sayi < hedef)
        {
            using var komut = new MySqlCommand(
                @"INSERT IGNORE INTO kitaplar (kitap_adi, yazar_id, tur_id, yayinevi, basim_yili, stok, eklenme_tarihi)
                  VALUES (@kitapAdi, @yazarId, @turId, @yayinevi, @basimYili, @stok, @eklenmeTarihi);",
                baglanti);

            komut.Parameters.AddWithValue("@kitapAdi", $"Kitap {i:000}");
            komut.Parameters.AddWithValue("@yazarId", yazarIdleri[(i - 1) % yazarIdleri.Count]);
            komut.Parameters.AddWithValue("@turId", turIdleri[(i - 1) % turIdleri.Count]);
            komut.Parameters.AddWithValue("@yayinevi", $"Yayınevi {((i - 1) % 30) + 1:00}");
            komut.Parameters.AddWithValue("@basimYili", 1980 + (i % 46));
            komut.Parameters.AddWithValue("@stok", 4 + (i % 12));
            komut.Parameters.AddWithValue("@eklenmeTarihi", DateTime.Now.AddHours(-i));

            if (komut.ExecuteNonQuery() > 0)
            {
                sayi++;
            }

            i++;
        }
    }

    private static void OduncKayitlariniTamamla(MySqlConnection baglanti, int hedef)
    {
        var sayi = KayitSayisi(baglanti, "odunc_islemleri");
        if (sayi >= hedef)
        {
            return;
        }

        var kullaniciIdleri = IdleriGetir(baglanti, "kullanicilar", "id");
        var kitapIdleri = IdleriGetir(baglanti, "kitaplar", "id");

        if (kullaniciIdleri.Count == 0 || kitapIdleri.Count == 0)
        {
            return;
        }

        var i = sayi + 1;
        while (sayi < hedef)
        {
            var oduncTarihi = DateTime.Today.AddDays(-(i % 120));
            var iadeEdildiMi = i % 4 == 0;
            var iadeTarihi = iadeEdildiMi ? oduncTarihi.AddDays((i % 20) + 1) : (DateTime?)null;

            using var komut = new MySqlCommand(
                @"INSERT INTO odunc_islemleri (kullanici_id, kitap_id, odunc_tarihi, iade_tarihi, durum)
                  VALUES (@kullaniciId, @kitapId, @oduncTarihi, @iadeTarihi, @durum);",
                baglanti);

            komut.Parameters.AddWithValue("@kullaniciId", kullaniciIdleri[(i - 1) % kullaniciIdleri.Count]);
            komut.Parameters.AddWithValue("@kitapId", kitapIdleri[(i - 1) % kitapIdleri.Count]);
            komut.Parameters.AddWithValue("@oduncTarihi", oduncTarihi);
            komut.Parameters.AddWithValue("@iadeTarihi", iadeTarihi.HasValue ? iadeTarihi.Value : DBNull.Value);
            komut.Parameters.AddWithValue("@durum", iadeEdildiMi ? "IadeEdildi" : "Oduncte");
            komut.ExecuteNonQuery();

            sayi++;
            i++;
        }
    }

    private static int KayitSayisi(MySqlConnection baglanti, string tablo)
    {
        using var komut = new MySqlCommand($"SELECT COUNT(*) FROM {tablo};", baglanti);
        return Convert.ToInt32(komut.ExecuteScalar());
    }

    private static List<int> IdleriGetir(MySqlConnection baglanti, string tablo, string idAlan)
    {
        var idler = new List<int>();

        using var komut = new MySqlCommand($"SELECT {idAlan} FROM {tablo} ORDER BY {idAlan};", baglanti);
        using var okuyucu = komut.ExecuteReader();

        while (okuyucu.Read())
        {
            idler.Add(okuyucu.GetInt32(0));
        }

        return idler;
    }
}
}


