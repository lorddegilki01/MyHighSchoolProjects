using MiniE_TicaretUygulaması.Data;
using MiniE_TicaretUygulaması.Models;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Services;

public sealed class UserService
{
    public IReadOnlyList<AppUser> GetAllUsers()
    {
        const string sql = @"
            SELECT kullanici_id, isim, email, rol, kayit_tarihi
            FROM kullanicilar
            ORDER BY kayit_tarihi DESC;";

        var users = new List<AppUser>();
        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new AppUser
            {
                KullaniciId = reader.GetInt32(reader.GetOrdinal("kullanici_id")),
                Isim = reader.GetString(reader.GetOrdinal("isim")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Rol = reader.GetString(reader.GetOrdinal("rol")),
                KayitTarihi = reader.GetDateTime(reader.GetOrdinal("kayit_tarihi"))
            });
        }

        return users;
    }

    public void DeleteUser(int userId, int activeUserId)
    {
        if (userId == activeUserId)
        {
            throw new InvalidOperationException("Aktif oturumdaki yönetici hesabı silinemez.");
        }

        using var connection = Database.CreateConnection();
        connection.Open();

        string role;
        using (var getRoleCmd = new MySqlCommand("SELECT rol FROM kullanicilar WHERE kullanici_id = @id;", connection))
        {
            getRoleCmd.Parameters.AddWithValue("@id", userId);
            var roleObj = getRoleCmd.ExecuteScalar();
            if (roleObj is null)
            {
                throw new InvalidOperationException("Silinecek kullanıcı bulunamadı.");
            }

            role = roleObj.ToString() ?? "kullanici";
        }

        if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            using var countAdminCmd = new MySqlCommand("SELECT COUNT(*) FROM kullanicilar WHERE rol = 'admin';", connection);
            var adminCount = Convert.ToInt32(countAdminCmd.ExecuteScalar());
            if (adminCount <= 1)
            {
                throw new InvalidOperationException("Son admin hesabı silinemez.");
            }
        }

        using var deleteCmd = new MySqlCommand("DELETE FROM kullanicilar WHERE kullanici_id = @id;", connection);
        deleteCmd.Parameters.AddWithValue("@id", userId);

        if (deleteCmd.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Silme işlemi tamamlanamadı.");
        }
    }
}

