using MiniE_TicaretUygulaması.Data;
using MiniE_TicaretUygulaması.Models;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Services;

public sealed class OrderService
{
    public static readonly IReadOnlyList<string> AllowedStatuses =
    [
        "Hazırlanıyor",
        "Kargoda",
        "Tamamlandı",
        "İptal"
    ];

    public int CreateOrder(int kullaniciId, IReadOnlyList<CartItem> cartItems)
    {
        if (cartItems.Count == 0)
        {
            throw new InvalidOperationException("Sepet boşken sipariş oluşturulamaz.");
        }

        using var connection = Database.CreateConnection();
        connection.Open();

        using var tx = connection.BeginTransaction();

        try
        {
            decimal total = 0m;

            foreach (var item in cartItems)
            {
                using var stockCmd = new MySqlCommand("SELECT stok FROM urunler WHERE urun_id = @urun_id FOR UPDATE;", connection, tx);
                stockCmd.Parameters.AddWithValue("@urun_id", item.UrunId);
                var stockObj = stockCmd.ExecuteScalar();
                if (stockObj is null)
                {
                    throw new InvalidOperationException($"Ürün bulunamadı (ID: {item.UrunId}).");
                }

                var stock = Convert.ToInt32(stockObj);
                if (stock < item.Adet)
                {
                    throw new InvalidOperationException($"'{item.UrunAdi}' ürününde yeterli stok yok. Mevcut: {stock}, İstenen: {item.Adet}");
                }

                total += item.AraToplam;
            }

            using var insertOrderCmd = new MySqlCommand(
                @"INSERT INTO siparisler (kullanici_id, tarih, toplam_tutar, durum)
                  VALUES (@kullanici_id, NOW(), @toplam_tutar, 'Hazırlanıyor');
                  SELECT LAST_INSERT_ID();", connection, tx);
            insertOrderCmd.Parameters.AddWithValue("@kullanici_id", kullaniciId);
            insertOrderCmd.Parameters.AddWithValue("@toplam_tutar", total);

            var orderIdObj = insertOrderCmd.ExecuteScalar();
            var orderId = Convert.ToInt32(orderIdObj);

            foreach (var item in cartItems)
            {
                using var insertItemCmd = new MySqlCommand(
                    @"INSERT INTO siparis_kalemleri (siparis_id, urun_id, adet, birim_fiyat, ara_toplam)
                      VALUES (@siparis_id, @urun_id, @adet, @birim_fiyat, @ara_toplam);", connection, tx);
                insertItemCmd.Parameters.AddWithValue("@siparis_id", orderId);
                insertItemCmd.Parameters.AddWithValue("@urun_id", item.UrunId);
                insertItemCmd.Parameters.AddWithValue("@adet", item.Adet);
                insertItemCmd.Parameters.AddWithValue("@birim_fiyat", item.BirimFiyat);
                insertItemCmd.Parameters.AddWithValue("@ara_toplam", item.AraToplam);
                insertItemCmd.ExecuteNonQuery();

                using var stockUpdateCmd = new MySqlCommand(
                    "UPDATE urunler SET stok = stok - @adet WHERE urun_id = @urun_id;",
                    connection,
                    tx);
                stockUpdateCmd.Parameters.AddWithValue("@adet", item.Adet);
                stockUpdateCmd.Parameters.AddWithValue("@urun_id", item.UrunId);
                stockUpdateCmd.ExecuteNonQuery();
            }

            tx.Commit();
            return orderId;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public IReadOnlyList<OrderSummary> GetOrders(int? kullaniciId = null)
    {
        var orders = new List<OrderSummary>();
        var sql = @"
            SELECT s.siparis_id, s.kullanici_id, k.isim AS kullanici_adi, s.tarih, s.toplam_tutar, s.durum
            FROM siparisler s
            INNER JOIN kullanicilar k ON s.kullanici_id = k.kullanici_id";

        if (kullaniciId.HasValue)
        {
            sql += " WHERE s.kullanici_id = @kullanici_id";
        }

        sql += " ORDER BY s.tarih DESC";

        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        if (kullaniciId.HasValue)
        {
            cmd.Parameters.AddWithValue("@kullanici_id", kullaniciId.Value);
        }

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            orders.Add(new OrderSummary
            {
                SiparisId = reader.GetInt32(reader.GetOrdinal("siparis_id")),
                KullaniciId = reader.GetInt32(reader.GetOrdinal("kullanici_id")),
                KullaniciAdi = reader.GetString(reader.GetOrdinal("kullanici_adi")),
                Tarih = reader.GetDateTime(reader.GetOrdinal("tarih")),
                ToplamTutar = reader.GetDecimal(reader.GetOrdinal("toplam_tutar")),
                Durum = reader.GetString(reader.GetOrdinal("durum"))
            });
        }

        return orders;
    }

    public void UpdateStatus(int siparisId, string durum)
    {
        if (!AllowedStatuses.Contains(durum))
        {
            throw new InvalidOperationException("Geçersiz sipariş durumu.");
        }

        const string sql = "UPDATE siparisler SET durum = @durum WHERE siparis_id = @siparis_id;";
        using var connection = Database.CreateConnection();
        connection.Open();

        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@durum", durum);
        cmd.Parameters.AddWithValue("@siparis_id", siparisId);

        if (cmd.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Güncellenecek sipariş bulunamadı.");
        }
    }
}

