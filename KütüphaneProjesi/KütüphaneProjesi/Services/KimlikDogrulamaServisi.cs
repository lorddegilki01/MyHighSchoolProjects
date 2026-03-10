using KütüphaneProjesi.Data;
using KütüphaneProjesi.Models;
using KütüphaneProjesi.Security;
using MySql.Data.MySqlClient;

namespace KütüphaneProjesi.Services;

public sealed class KimlikDogrulamaServisi
{
    public KullaniciOturumu GirisYap(string kullaniciAdi, string sifre)
    {
        using var baglanti = Veritabani.BaglantiOlustur();
        baglanti.Open();

        using var komut = new MySqlCommand(
            @"SELECT id, isim, sifre_hash, rol
              FROM kullanicilar
              WHERE isim = @isim
              LIMIT 1;",
            baglanti);

        komut.Parameters.AddWithValue("@isim", kullaniciAdi);

        using var okuyucu = komut.ExecuteReader();
        if (!okuyucu.Read())
        {
            return null;
        }

        var hash = okuyucu.GetString("sifre_hash");
        if (!SifreYoneticisi.Dogrula(sifre, hash))
        {
            return null;
        }

        return new KullaniciOturumu
        {
            Id = okuyucu.GetInt32("id"),
            Isim = okuyucu.GetString("isim"),
            Rol = okuyucu.GetString("rol")
        };
    }
}

