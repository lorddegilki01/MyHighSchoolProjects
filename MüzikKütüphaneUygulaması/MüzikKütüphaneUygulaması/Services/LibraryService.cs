using MüzikKütüphaneUygulaması.Data;
using MüzikKütüphaneUygulaması.Models;
using MySqlConnector;

namespace MüzikKütüphaneUygulaması.Services;

public sealed class LibraryService
{
    public IReadOnlyList<Genre> GetGenres()
    {
        const string sql = "SELECT tur_id, tur_adi FROM turler ORDER BY tur_adi;";
        var list = new List<Genre>();

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Genre
            {
                TurId = reader.GetInt32("tur_id"),
                TurAdi = reader.GetString("tur_adi")
            });
        }

        return list;
    }

    public IReadOnlyList<Artist> GetArtists()
    {
        const string sql = "SELECT sanatci_id, sanatci_adi FROM sanatcilar ORDER BY sanatci_adi;";
        var list = new List<Artist>();

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Artist
            {
                SanatciId = reader.GetInt32("sanatci_id"),
                SanatciAdi = reader.GetString("sanatci_adi")
            });
        }

        return list;
    }

    public IReadOnlyList<Song> GetSongs(string search, int? genreId, string sortKey)
    {
        var orderBy = sortKey switch
        {
            "AdAzalan" => "s.isim DESC",
            "YilYeni" => "s.yil DESC, s.isim ASC",
            "YilEski" => "s.yil ASC, s.isim ASC",
            _ => "s.isim ASC"
        };

        var sql = $@"
SELECT s.sarki_id, s.isim AS sarki_adi, s.sanatci_id, sa.sanatci_adi, s.tur_id, t.tur_adi,
       IFNULL(s.album,'') AS album, IFNULL(s.yil,0) AS yil, s.sure_saniye, s.eklenme_tarihi
FROM sarkilar s
JOIN sanatcilar sa ON sa.sanatci_id = s.sanatci_id
JOIN turler t ON t.tur_id = s.tur_id
WHERE (@ara = '' OR s.isim LIKE @araLike OR sa.sanatci_adi LIKE @araLike OR IFNULL(s.album,'') LIKE @araLike)
  AND (@turId IS NULL OR s.tur_id = @turId)
ORDER BY {orderBy};";

        var list = new List<Song>();

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@ara", search?.Trim() ?? string.Empty);
        cmd.Parameters.AddWithValue("@araLike", "%" + (search?.Trim() ?? string.Empty) + "%");
        cmd.Parameters.AddWithValue("@turId", genreId.HasValue ? genreId.Value : DBNull.Value);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Song
            {
                SarkiId = reader.GetInt32("sarki_id"),
                SarkiAdi = reader.GetString("sarki_adi"),
                SanatciId = reader.GetInt32("sanatci_id"),
                SanatciAdi = reader.GetString("sanatci_adi"),
                TurId = reader.GetInt32("tur_id"),
                TurAdi = reader.GetString("tur_adi"),
                Album = reader.GetString("album"),
                Yil = reader.GetInt32("yil"),
                SureSaniye = reader.GetInt32("sure_saniye"),
                EklenmeTarihi = reader.GetDateTime("eklenme_tarihi")
            });
        }

        return list;
    }

    public void AddSong(Song song)
    {
        const string sql = @"
INSERT INTO sarkilar (isim, sanatci_id, tur_id, album, yil, sure_saniye)
VALUES (@ad, @sanatci, @tur, @album, @yil, @sure);";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@ad", song.SarkiAdi.Trim());
        cmd.Parameters.AddWithValue("@sanatci", song.SanatciId);
        cmd.Parameters.AddWithValue("@tur", song.TurId);
        cmd.Parameters.AddWithValue("@album", string.IsNullOrWhiteSpace(song.Album) ? DBNull.Value : song.Album.Trim());
        cmd.Parameters.AddWithValue("@yil", song.Yil <= 0 ? DBNull.Value : song.Yil);
        cmd.Parameters.AddWithValue("@sure", song.SureSaniye <= 0 ? 180 : song.SureSaniye);
        cmd.ExecuteNonQuery();
    }

    public void UpdateSong(Song song)
    {
        const string sql = @"
UPDATE sarkilar
SET isim = @ad,
    sanatci_id = @sanatci,
    tur_id = @tur,
    album = @album,
    yil = @yil,
    sure_saniye = @sure
WHERE sarki_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", song.SarkiId);
        cmd.Parameters.AddWithValue("@ad", song.SarkiAdi.Trim());
        cmd.Parameters.AddWithValue("@sanatci", song.SanatciId);
        cmd.Parameters.AddWithValue("@tur", song.TurId);
        cmd.Parameters.AddWithValue("@album", string.IsNullOrWhiteSpace(song.Album) ? DBNull.Value : song.Album.Trim());
        cmd.Parameters.AddWithValue("@yil", song.Yil <= 0 ? DBNull.Value : song.Yil);
        cmd.Parameters.AddWithValue("@sure", song.SureSaniye <= 0 ? 180 : song.SureSaniye);
        cmd.ExecuteNonQuery();
    }

    public void DeleteSong(int songId)
    {
        const string sql = "DELETE FROM sarkilar WHERE sarki_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", songId);
        cmd.ExecuteNonQuery();
    }

    public void AddArtist(string name)
    {
        const string sql = "INSERT INTO sanatcilar (sanatci_adi) VALUES (@ad);";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@ad", name.Trim());
        cmd.ExecuteNonQuery();
    }

    public void UpdateArtist(int id, string name)
    {
        const string sql = "UPDATE sanatcilar SET sanatci_adi = @ad WHERE sanatci_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@ad", name.Trim());
        cmd.ExecuteNonQuery();
    }

    public void DeleteArtist(int id)
    {
        const string sql = "DELETE FROM sanatcilar WHERE sanatci_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public void AddGenre(string name)
    {
        const string sql = "INSERT INTO turler (tur_adi) VALUES (@ad);";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@ad", name.Trim());
        cmd.ExecuteNonQuery();
    }

    public void UpdateGenre(int id, string name)
    {
        const string sql = "UPDATE turler SET tur_adi = @ad WHERE tur_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@ad", name.Trim());
        cmd.ExecuteNonQuery();
    }

    public void DeleteGenre(int id)
    {
        const string sql = "DELETE FROM turler WHERE tur_id = @id;";

        using var connection = Database.CreateConnection();
        connection.Open();
        using var cmd = new MySqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}

