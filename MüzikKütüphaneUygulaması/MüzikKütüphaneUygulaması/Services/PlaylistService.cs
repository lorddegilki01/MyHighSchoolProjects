using MüzikKütüphaneUygulaması.Data;
using MüzikKütüphaneUygulaması.Models;
using MySqlConnector;

namespace MüzikKütüphaneUygulaması.Services;

public sealed class PlaylistService
{
    public void SavePlaylist(int kullaniciId, string listeAdi, IReadOnlyList<PlaylistItem> songs)
    {
        if (songs.Count == 0)
        {
            throw new InvalidOperationException("Listeye en az bir şarkı eklenmelidir.");
        }

        using var connection = Database.CreateConnection();
        connection.Open();
        using var tx = connection.BeginTransaction();

        try
        {
            using var insertList = new MySqlCommand(
                @"INSERT INTO listeler (kullanici_id, liste_adi, olusturma_tarihi)
                  VALUES (@kullanici, @ad, NOW());
                  SELECT LAST_INSERT_ID();", connection, tx);

            insertList.Parameters.AddWithValue("@kullanici", kullaniciId);
            insertList.Parameters.AddWithValue("@ad", string.IsNullOrWhiteSpace(listeAdi) ? "Yeni Liste" : listeAdi.Trim());

            var listId = Convert.ToInt32(insertList.ExecuteScalar());

            var order = 1;
            foreach (var song in songs)
            {
                using var insertItem = new MySqlCommand(
                    @"INSERT INTO liste_kalemleri (liste_id, sarki_id, sira_no, eklenme_tarihi)
                      VALUES (@liste, @sarki, @sira, NOW());", connection, tx);

                insertItem.Parameters.AddWithValue("@liste", listId);
                insertItem.Parameters.AddWithValue("@sarki", song.SarkiId);
                insertItem.Parameters.AddWithValue("@sira", order++);
                insertItem.ExecuteNonQuery();
            }

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public IReadOnlyList<PlaylistSummary> GetPlaylists(int kullaniciId)
    {
        const string sql = @"
SELECT l.liste_id, l.liste_adi, l.olusturma_tarihi, COUNT(k.liste_kalem_id) AS sarki_sayisi
FROM listeler l
LEFT JOIN liste_kalemleri k ON k.liste_id = l.liste_id
WHERE l.kullanici_id = @kullanici
GROUP BY l.liste_id, l.liste_adi, l.olusturma_tarihi
ORDER BY l.olusturma_tarihi DESC;";

        var list = new List<PlaylistSummary>();

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@kullanici", kullaniciId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new PlaylistSummary
            {
                ListeId = reader.GetInt32("liste_id"),
                ListeAdi = reader.GetString("liste_adi"),
                SarkiSayisi = reader.GetInt32("sarki_sayisi"),
                OlusturmaTarihi = reader.GetDateTime("olusturma_tarihi")
            });
        }

        return list;
    }
}
