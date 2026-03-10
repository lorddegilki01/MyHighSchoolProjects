using System.Security.Cryptography;

namespace KütüphaneProjesi.Security
{
    public static class SifreYoneticisi
    {
        private const int Iterasyon = 120000;
        private const int TuzBoyutu = 16;
        private const int AnahtarBoyutu = 32;

        public static string Hashle(string sifre)
        {
            if (string.IsNullOrWhiteSpace(sifre))
            {
                throw new ArgumentException("Şifre boş olamaz.", nameof(sifre));
            }

            var tuz = RandomNumberGenerator.GetBytes(TuzBoyutu);
            using var pbkdf2 = new Rfc2898DeriveBytes(sifre, tuz, Iterasyon, HashAlgorithmName.SHA256);
            var anahtar = pbkdf2.GetBytes(AnahtarBoyutu);

            return $"{Iterasyon}.{Convert.ToBase64String(tuz)}.{Convert.ToBase64String(anahtar)}";
        }

        public static bool Dogrula(string sifre, string hash)
        {
            if (string.IsNullOrWhiteSpace(sifre) || string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            var parcalar = hash.Split('.');
            if (parcalar.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parcalar[0], out var iterasyon) || iterasyon <= 0)
            {
                return false;
            }

            var tuz = Convert.FromBase64String(parcalar[1]);
            var beklenen = Convert.FromBase64String(parcalar[2]);

            using var pbkdf2 = new Rfc2898DeriveBytes(sifre, tuz, iterasyon, HashAlgorithmName.SHA256);
            var gelen = pbkdf2.GetBytes(beklenen.Length);

            return CryptographicOperations.FixedTimeEquals(gelen, beklenen);
        }
    }
}


