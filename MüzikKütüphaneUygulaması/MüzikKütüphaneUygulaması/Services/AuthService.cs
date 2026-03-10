using MüzikKütüphaneUygulaması.Data;
using MüzikKütüphaneUygulaması.Models;
using MüzikKütüphaneUygulaması.Security;
using MySqlConnector;

namespace MüzikKütüphaneUygulaması.Services;

public sealed class AuthService
{
    public AppUser Login(string email, string password)
    {
        const string sql = @"
SELECT kullanici_id, isim, email, sifre, rol, kayit_tarihi
FROM kullanicilar
WHERE email = @email
LIMIT 1;";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@email", email.Trim());

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var hash = reader.GetString("sifre");
        if (!PasswordHasher.VerifyPassword(password, hash))
        {
            return null;
        }

        return new AppUser
        {
            KullaniciId = reader.GetInt32("kullanici_id"),
            Isim = reader.GetString("isim"),
            Email = reader.GetString("email"),
            Rol = reader.GetString("rol"),
            KayitTarihi = reader.GetDateTime("kayit_tarihi")
        };
    }
}
