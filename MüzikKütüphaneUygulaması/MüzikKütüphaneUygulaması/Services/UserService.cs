using MüzikKütüphaneUygulaması.Data;
using MüzikKütüphaneUygulaması.Models;
using MySqlConnector;

namespace MüzikKütüphaneUygulaması.Services;

public sealed class UserService
{
    public IReadOnlyList<UserSummary> GetAllUsers()
    {
        const string sql = @"
SELECT kullanici_id, isim, email, rol, kayit_tarihi
FROM kullanicilar
ORDER BY kullanici_id DESC;";

        var list = new List<UserSummary>();
        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new UserSummary
            {
                KullaniciId = reader.GetInt32("kullanici_id"),
                Isim = reader.GetString("isim"),
                Email = reader.GetString("email"),
                Rol = reader.GetString("rol"),
                KayitTarihi = reader.GetDateTime("kayit_tarihi")
            });
        }

        return list;
    }

    public void DeleteUser(int userId)
    {
        const string sql = "DELETE FROM kullanicilar WHERE kullanici_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.ExecuteNonQuery();
    }
}
