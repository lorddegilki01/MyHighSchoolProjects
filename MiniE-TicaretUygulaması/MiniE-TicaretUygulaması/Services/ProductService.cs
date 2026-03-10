using System.Text;
using MiniE_TicaretUygulaması.Data;
using MiniE_TicaretUygulaması.Models;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Services;

public sealed class ProductService
{
    public IReadOnlyList<Product> GetProducts(string search, int? kategoriId, string sortBy)
    {
        var sql = new StringBuilder(@"
            SELECT u.urun_id, u.urun_adi, u.kategori_id, k.kategori_adi, u.fiyat, u.stok, u.eklenme_tarihi
            FROM urunler u
            INNER JOIN kategoriler k ON u.kategori_id = k.kategori_id
            WHERE 1 = 1");

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand();
        cmd.Connection = connection;

        if (!string.IsNullOrWhiteSpace(search))
        {
            sql.Append(" AND u.urun_adi LIKE @search");
            cmd.Parameters.AddWithValue("@search", $"%{search.Trim()}%");
        }

        if (kategoriId.HasValue)
        {
            sql.Append(" AND u.kategori_id = @kategori_id");
            cmd.Parameters.AddWithValue("@kategori_id", kategoriId.Value);
        }

        sql.Append(sortBy switch
        {
            "FiyatArtan" => " ORDER BY u.fiyat ASC",
            "FiyatAzalan" => " ORDER BY u.fiyat DESC",
            "AdAzalan" => " ORDER BY u.urun_adi DESC",
            _ => " ORDER BY u.urun_adi ASC"
        });

        cmd.CommandText = sql.ToString();

        var products = new List<Product>();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                UrunId = reader.GetInt32(reader.GetOrdinal("urun_id")),
                UrunAdi = reader.GetString(reader.GetOrdinal("urun_adi")),
                KategoriId = reader.GetInt32(reader.GetOrdinal("kategori_id")),
                KategoriAdi = reader.GetString(reader.GetOrdinal("kategori_adi")),
                Fiyat = reader.GetDecimal(reader.GetOrdinal("fiyat")),
                Stok = reader.GetInt32(reader.GetOrdinal("stok")),
                EklenmeTarihi = reader.GetDateTime(reader.GetOrdinal("eklenme_tarihi"))
            });
        }

        return products;
    }

    public void Add(Product product)
    {
        const string sql = @"
            INSERT INTO urunler (urun_adi, kategori_id, fiyat, stok)
            VALUES (@urun_adi, @kategori_id, @fiyat, @stok);";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@urun_adi", product.UrunAdi.Trim());
        cmd.Parameters.AddWithValue("@kategori_id", product.KategoriId);
        cmd.Parameters.AddWithValue("@fiyat", product.Fiyat);
        cmd.Parameters.AddWithValue("@stok", product.Stok);
        cmd.ExecuteNonQuery();
    }

    public void Update(Product product)
    {
        const string sql = @"
            UPDATE urunler
            SET urun_adi = @urun_adi,
                kategori_id = @kategori_id,
                fiyat = @fiyat,
                stok = @stok
            WHERE urun_id = @urun_id;";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@urun_adi", product.UrunAdi.Trim());
        cmd.Parameters.AddWithValue("@kategori_id", product.KategoriId);
        cmd.Parameters.AddWithValue("@fiyat", product.Fiyat);
        cmd.Parameters.AddWithValue("@stok", product.Stok);
        cmd.Parameters.AddWithValue("@urun_id", product.UrunId);

        if (cmd.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Güncellenecek ürün bulunamadı.");
        }
    }

    public void Delete(int urunId)
    {
        const string sql = "DELETE FROM urunler WHERE urun_id = @urun_id;";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@urun_id", urunId);

        if (cmd.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Silinecek ürün bulunamadı.");
        }
    }
}

