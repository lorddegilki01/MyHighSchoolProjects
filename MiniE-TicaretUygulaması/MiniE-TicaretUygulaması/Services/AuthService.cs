using MiniE_TicaretUygulaması.Data;
using MiniE_TicaretUygulaması.Models;
using MiniE_TicaretUygulaması.Security;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Services;

public sealed class AuthService
{
    public AppUser Login(string kullaniciAdi, string password)
    {
        const string sql = @"
            SELECT kullanici_id, isim, email, sifre, rol, kayit_tarihi
            FROM kullanicilar
            WHERE isim = @kullanici_adi
            LIMIT 1;";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@kullanici_adi", kullaniciAdi.Trim());

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var hash = reader.GetString(reader.GetOrdinal("sifre"));
        if (!PasswordHasher.VerifyPassword(password, hash))
        {
            return null;
        }

        return new AppUser
        {
            KullaniciId = reader.GetInt32(reader.GetOrdinal("kullanici_id")),
            Isim = reader.GetString(reader.GetOrdinal("isim")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            Rol = reader.GetString(reader.GetOrdinal("rol")),
            KayitTarihi = reader.GetDateTime(reader.GetOrdinal("kayit_tarihi"))
        };
    }
}

