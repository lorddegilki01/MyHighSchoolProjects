using System.Configuration;
using MySql.Data.MySqlClient;

namespace KütüphaneProjesi.Data
{
    public static class Veritabani
    {
        private const string BaglantiAdi = "MySqlConnection";

        private static MySqlConnectionStringBuilder BaglantiAyarlariGetir()
        {
            var baglanti = ConfigurationManager.ConnectionStrings[BaglantiAdi];
            if (baglanti is null || string.IsNullOrWhiteSpace(baglanti.ConnectionString))
            {
                throw new InvalidOperationException(
                    "App.config içinde MySqlConnection bağlantı ayarı bulunamadı.");
            }

            var ayarlar = new MySqlConnectionStringBuilder(baglanti.ConnectionString);

            if (string.IsNullOrWhiteSpace(ayarlar.Database))
            {
                ayarlar.Database = "kutuphane";
            }

            if (string.IsNullOrWhiteSpace(ayarlar.CharacterSet))
            {
                ayarlar.CharacterSet = "utf8mb4";
            }

            ayarlar.AllowUserVariables = true;
            return ayarlar;
        }

        public static string BaglantiCumlesi => BaglantiAyarlariGetir().ConnectionString;

        public static MySqlConnection BaglantiOlustur()
        {
            return new MySqlConnection(BaglantiCumlesi);
        }

        public static string SunucuBaglantiCumlesi
        {
            get
            {
                var builder = BaglantiAyarlariGetir();
                builder.Database = string.Empty;
                return builder.ConnectionString;
            }
        }
    }
}
