using MiniE_TicaretUygulaması.Data;
using MiniE_TicaretUygulaması.Models;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Services;

public sealed class CategoryService
{
    public IReadOnlyList<Category> GetAll()
    {
        const string sql = "SELECT kategori_id, kategori_adi FROM kategoriler ORDER BY kategori_adi;";

        var categories = new List<Category>();
        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            categories.Add(new Category
            {
                KategoriId = reader.GetInt32(reader.GetOrdinal("kategori_id")),
                KategoriAdi = reader.GetString(reader.GetOrdinal("kategori_adi"))
            });
        }

        return categories;
    }

    public void Add(string kategoriAdi)
    {
        const string sql = "INSERT INTO kategoriler (kategori_adi) VALUES (@kategori_adi);";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@kategori_adi", kategoriAdi.Trim());
        cmd.ExecuteNonQuery();
    }

    public void Update(int kategoriId, string kategoriAdi)
    {
        const string sql = "UPDATE kategoriler SET kategori_adi = @kategori_adi WHERE kategori_id = @kategori_id;";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@kategori_adi", kategoriAdi.Trim());
        cmd.Parameters.AddWithValue("@kategori_id", kategoriId);

        if (cmd.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Güncellenecek kategori bulunamadı.");
        }
    }

    public void Delete(int kategoriId)
    {
        const string checkSql = "SELECT COUNT(*) FROM urunler WHERE kategori_id = @kategori_id;";
        const string deleteSql = "DELETE FROM kategoriler WHERE kategori_id = @kategori_id;";

        using var connection = Database.CreateConnection();
        connection.Open();

        using (var checkCmd = new MySqlCommand(checkSql, connection))
        {
            checkCmd.Parameters.AddWithValue("@kategori_id", kategoriId);
            var productCount = Convert.ToInt32(checkCmd.ExecuteScalar());
            if (productCount > 0)
            {
                throw new InvalidOperationException("Bu kategoriye bağlı ürünler var. Önce ürünleri silin veya başka kategoriye taşıyın.");
            }
        }

        using var deleteCmd = new MySqlCommand(deleteSql, connection);
        deleteCmd.Parameters.AddWithValue("@kategori_id", kategoriId);

        if (deleteCmd.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Silinecek kategori bulunamadı.");
        }
    }
}

