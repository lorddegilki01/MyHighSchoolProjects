using MüzikKütüphaneUygulaması.Security;
using MySqlConnector;

namespace MüzikKütüphaneUygulaması.Data;

public static class DatabaseInitializer
{
    private const int SeedTarget = 200;

    private static readonly string[] ArtistPrefixes =
    [
        "Anadolu", "Ritim", "Akustik", "Dijital", "Kuzey", "Güney", "Mavi", "Kırmızı", "Gece", "Sabah"
    ];

    private static readonly string[] GenrePrefixes =
    [
        "Pop", "Rock", "Rap", "Caz", "Klasik", "Folk", "Elektronik", "Ambient", "Metal", "Soul"
    ];

    public static void Initialize()
    {
        EnsureDatabaseExists();

        using var connection = Database.CreateConnection();
        connection.Open();

        CreateTables(connection);
        EnsureLegacySchema(connection);
        SeedBaseData(connection);
    }

    private static void EnsureDatabaseExists()
    {
        var builder = new MySqlConnectionStringBuilder(Database.ConnectionString);
        var databaseName = builder.Database;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("Connection string içinde Database alanı zorunludur.");
        }

        builder.Database = string.Empty;

        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        using var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;", connection);
        cmd.ExecuteNonQuery();
    }

    private static void CreateTables(MySqlConnection connection)
    {
        var scripts = new[]
        {
            @"CREATE TABLE IF NOT EXISTS kullanicilar (
                kullanici_id INT AUTO_INCREMENT PRIMARY KEY,
                isim VARCHAR(120) NOT NULL,
                email VARCHAR(200) NOT NULL UNIQUE,
                sifre VARCHAR(500) NOT NULL,
                rol ENUM('kullanici','yonetici') NOT NULL DEFAULT 'kullanici',
                kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            );",
            @"CREATE TABLE IF NOT EXISTS sanatcilar (
                sanatci_id INT AUTO_INCREMENT PRIMARY KEY,
                sanatci_adi VARCHAR(140) NOT NULL UNIQUE
            );",
            @"CREATE TABLE IF NOT EXISTS turler (
                tur_id INT AUTO_INCREMENT PRIMARY KEY,
                tur_adi VARCHAR(140) NOT NULL UNIQUE
            );",
            @"CREATE TABLE IF NOT EXISTS sarkilar (
                sarki_id INT AUTO_INCREMENT PRIMARY KEY,
                isim VARCHAR(200) NOT NULL,
                sanatci_id INT NOT NULL,
                tur_id INT NOT NULL,
                album VARCHAR(180),
                yil INT,
                sure_saniye INT NOT NULL DEFAULT 180,
                eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (sanatci_id) REFERENCES sanatcilar(sanatci_id) ON UPDATE CASCADE ON DELETE RESTRICT,
                FOREIGN KEY (tur_id) REFERENCES turler(tur_id) ON UPDATE CASCADE ON DELETE RESTRICT
            );",
            @"CREATE TABLE IF NOT EXISTS listeler (
                liste_id INT AUTO_INCREMENT PRIMARY KEY,
                kullanici_id INT NOT NULL,
                liste_adi VARCHAR(160) NOT NULL,
                olusturma_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id) ON UPDATE CASCADE ON DELETE CASCADE
            );",
            @"CREATE TABLE IF NOT EXISTS liste_kalemleri (
                liste_kalem_id INT AUTO_INCREMENT PRIMARY KEY,
                liste_id INT NOT NULL,
                sarki_id INT NOT NULL,
                sira_no INT NOT NULL,
                eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (liste_id) REFERENCES listeler(liste_id) ON UPDATE CASCADE ON DELETE CASCADE,
                FOREIGN KEY (sarki_id) REFERENCES sarkilar(sarki_id) ON UPDATE CASCADE ON DELETE RESTRICT
            );"
        };

        foreach (var script in scripts)
        {
            using var cmd = new MySqlCommand(script, connection);
            cmd.ExecuteNonQuery();
        }
    }


    private static void EnsureLegacySchema(MySqlConnection connection)
    {
        EnsureColumnExists(connection, "kullanicilar", "email", "VARCHAR(200) NULL");
        EnsureColumnExists(connection, "kullanicilar", "kayit_tarihi", "DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP");
        EnsureSongNameColumn(connection);
        EnsureSongDataColumns(connection);
        EnsureSongOwnerColumn(connection);

        EnsureValidEmails(connection, "kullanicilar", "kullanici_id", "email", "muzik.local");
        EnsureUniqueEmailIndex(connection, "kullanicilar", "kullanici_id", "email", "ux_kullanicilar_email", "muzik.local");
    }

    private static void EnsureColumnExists(MySqlConnection connection, string tableName, string columnName, string definition)
    {
        using var existsCmd = new MySqlCommand(
            @"SELECT COUNT(*)
              FROM INFORMATION_SCHEMA.COLUMNS
              WHERE TABLE_SCHEMA = DATABASE()
                AND TABLE_NAME = @table
                AND COLUMN_NAME = @column;",
            connection);

        existsCmd.Parameters.AddWithValue("@table", tableName);
        existsCmd.Parameters.AddWithValue("@column", columnName);

        var exists = Convert.ToInt32(existsCmd.ExecuteScalar()) > 0;
        if (exists)
        {
            return;
        }

        using var alter = new MySqlCommand($"ALTER TABLE `{tableName}` ADD COLUMN `{columnName}` {definition};", connection);
        alter.ExecuteNonQuery();
    }

    private static void EnsureSongNameColumn(MySqlConnection connection)
    {
        EnsureColumnExists(connection, "sarkilar", "isim", "VARCHAR(200) NOT NULL DEFAULT ''");

        if (!ColumnExists(connection, "sarkilar", "sarki_adi"))
        {
            return;
        }

        using var copy = new MySqlCommand(
            @"UPDATE sarkilar
              SET isim = sarki_adi
              WHERE (isim IS NULL OR isim = '')
                AND sarki_adi IS NOT NULL
                AND sarki_adi <> '';",
            connection);

        copy.ExecuteNonQuery();
    }

    private static void EnsureSongDataColumns(MySqlConnection connection)
    {
        EnsureColumnExists(connection, "sarkilar", "album", "VARCHAR(180) NULL");
        EnsureColumnExists(connection, "sarkilar", "yil", "INT NULL");
        EnsureColumnExists(connection, "sarkilar", "sure_saniye", "INT NOT NULL DEFAULT 180");
        EnsureColumnExists(connection, "sarkilar", "eklenme_tarihi", "DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP");

        if (ColumnExists(connection, "sarkilar", "sure"))
        {
            using var fromSure = new MySqlCommand(
                @"UPDATE sarkilar
                  SET sure_saniye = sure
                  WHERE sure_saniye <= 0
                    AND sure IS NOT NULL
                    AND sure > 0;",
                connection);

            fromSure.ExecuteNonQuery();
        }
    }
    private static void EnsureSongOwnerColumn(MySqlConnection connection)
    {
        if (!ColumnExists(connection, "sarkilar", "kullanici_id"))
        {
            return;
        }

        using var relax = new MySqlCommand(
            @"ALTER TABLE sarkilar
              MODIFY COLUMN kullanici_id INT NULL;",
            connection);

        relax.ExecuteNonQuery();
    }
    private static bool ColumnExists(MySqlConnection connection, string tableName, string columnName)
    {
        using var cmd = new MySqlCommand(
            @"SELECT COUNT(*)
              FROM INFORMATION_SCHEMA.COLUMNS
              WHERE TABLE_SCHEMA = DATABASE()
                AND TABLE_NAME = @table
                AND COLUMN_NAME = @column;",
            connection);

        cmd.Parameters.AddWithValue("@table", tableName);
        cmd.Parameters.AddWithValue("@column", columnName);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }
    private static void EnsureValidEmails(
        MySqlConnection connection,
        string tableName,
        string idColumn,
        string emailColumn,
        string domain)
    {
        using var fix = new MySqlCommand(
            $@"UPDATE `{tableName}`
               SET `{emailColumn}` = CONCAT('kullanici', `{idColumn}`, '@', @domain)
               WHERE `{emailColumn}` IS NULL
                  OR `{emailColumn}` = ''
                  OR `{emailColumn}` NOT LIKE '%@%';",
            connection);

        fix.Parameters.AddWithValue("@domain", domain);
        fix.ExecuteNonQuery();
    }

    private static void EnsureUniqueEmailIndex(
        MySqlConnection connection,
        string tableName,
        string idColumn,
        string emailColumn,
        string indexName,
        string domain)
    {
        using var existsCmd = new MySqlCommand(
            @"SELECT COUNT(*)
              FROM INFORMATION_SCHEMA.STATISTICS
              WHERE TABLE_SCHEMA = DATABASE()
                AND TABLE_NAME = @table
                AND COLUMN_NAME = @column
                AND NON_UNIQUE = 0;",
            connection);

        existsCmd.Parameters.AddWithValue("@table", tableName);
        existsCmd.Parameters.AddWithValue("@column", emailColumn);

        var hasUnique = Convert.ToInt32(existsCmd.ExecuteScalar()) > 0;
        if (hasUnique)
        {
            return;
        }

        using var dedupe = new MySqlCommand(
            $@"UPDATE `{tableName}` k1
               JOIN `{tableName}` k2
                 ON k1.`{emailColumn}` = k2.`{emailColumn}`
                AND k1.`{idColumn}` > k2.`{idColumn}`
               SET k1.`{emailColumn}` = CONCAT('kullanici', k1.`{idColumn}`, '@', @domain);",
            connection);

        dedupe.Parameters.AddWithValue("@domain", domain);
        dedupe.ExecuteNonQuery();

        using var create = new MySqlCommand(
            $"CREATE UNIQUE INDEX `{indexName}` ON `{tableName}` (`{emailColumn}`);",
            connection);

        create.ExecuteNonQuery();
    }
    private static void SeedBaseData(MySqlConnection connection)
    {
        EnsureAccount(connection, "Sistem Yöneticisi", "yonetici@muzik.local", "Yonetici123!", "yonetici");
        EnsureAccount(connection, "Demo Kullanıcı", "kullanici@muzik.local", "Kullanici123!", "kullanici");

        EnsureUsers(connection, SeedTarget);
        EnsureArtists(connection, SeedTarget);
        EnsureGenres(connection, SeedTarget);
        EnsureSongs(connection, SeedTarget);
    }

    private static void EnsureAccount(MySqlConnection connection, string isim, string email, string sifre, string rol)
    {
        using var cmd = new MySqlCommand(
            @"INSERT INTO kullanicilar (isim, email, sifre, rol)
              VALUES (@isim, @email, @sifre, @rol)
              ON DUPLICATE KEY UPDATE
                isim = VALUES(isim),
                sifre = VALUES(sifre),
                rol = VALUES(rol);", connection);

        cmd.Parameters.AddWithValue("@isim", isim);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@sifre", PasswordHasher.HashPassword(sifre));
        cmd.Parameters.AddWithValue("@rol", rol);
        cmd.ExecuteNonQuery();
    }

    private static void EnsureUsers(MySqlConnection connection, int target)
    {
        var count = GetCount(connection, "kullanicilar");
        var i = 1;

        while (count < target)
        {
            using var cmd = new MySqlCommand(
                @"INSERT IGNORE INTO kullanicilar (isim, email, sifre, rol, kayit_tarihi)
                  VALUES (@isim, @email, @sifre, @rol, @kayit);", connection);

            cmd.Parameters.AddWithValue("@isim", $"Müzik Kullanıcısı {i:000}");
            cmd.Parameters.AddWithValue("@email", $"kullanici{i:000}@muzik.local");
            cmd.Parameters.AddWithValue("@sifre", PasswordHasher.HashPassword("Kullanici123!"));
            cmd.Parameters.AddWithValue("@rol", i % 25 == 0 ? "yonetici" : "kullanici");
            cmd.Parameters.AddWithValue("@kayit", DateTime.Now.AddDays(-i));

            if (cmd.ExecuteNonQuery() > 0)
            {
                count++;
            }

            i++;
        }
    }

    private static void EnsureArtists(MySqlConnection connection, int target)
    {
        var count = GetCount(connection, "sanatcilar");
        var i = 1;

        while (count < target)
        {
            var prefix = ArtistPrefixes[(i - 1) % ArtistPrefixes.Length];
            using var cmd = new MySqlCommand("INSERT IGNORE INTO sanatcilar (sanatci_adi) VALUES (@ad);", connection);
            cmd.Parameters.AddWithValue("@ad", $"{prefix} Sanatçı {i:000}");

            if (cmd.ExecuteNonQuery() > 0)
            {
                count++;
            }

            i++;
        }
    }

    private static void EnsureGenres(MySqlConnection connection, int target)
    {
        var count = GetCount(connection, "turler");
        var i = 1;

        while (count < target)
        {
            var prefix = GenrePrefixes[(i - 1) % GenrePrefixes.Length];
            using var cmd = new MySqlCommand("INSERT IGNORE INTO turler (tur_adi) VALUES (@ad);", connection);
            cmd.Parameters.AddWithValue("@ad", $"{prefix} Tür {i:000}");

            if (cmd.ExecuteNonQuery() > 0)
            {
                count++;
            }

            i++;
        }
    }

    private static void EnsureSongs(MySqlConnection connection, int target)
    {
        var count = GetCount(connection, "sarkilar");
        if (count >= target)
        {
            return;
        }

        var artistIds = GetIds(connection, "sanatcilar", "sanatci_id");
        var genreIds = GetIds(connection, "turler", "tur_id");
        var hasSongOwnerColumn = ColumnExists(connection, "sarkilar", "kullanici_id");
        var userIds = hasSongOwnerColumn ? GetIds(connection, "kullanicilar", "kullanici_id") : new List<int>();

        if (hasSongOwnerColumn && userIds.Count == 0)
        {
            throw new InvalidOperationException("sarkilar.kullanici_id kolonu bulundu ancak kullanici kaydi yok.");
        }

        var i = count + 1;
        while (count < target)
        {
            var artistId = artistIds[(i - 1) % artistIds.Count];
            var genreId = genreIds[(i - 1) % genreIds.Count];

            var sql = hasSongOwnerColumn
                ? @"INSERT INTO sarkilar (isim, sanatci_id, tur_id, album, yil, sure_saniye, eklenme_tarihi, kullanici_id)
                    VALUES (@ad, @sanatci, @tur, @album, @yil, @sure, @eklenme, @kullanici_id);"
                : @"INSERT INTO sarkilar (isim, sanatci_id, tur_id, album, yil, sure_saniye, eklenme_tarihi)
                    VALUES (@ad, @sanatci, @tur, @album, @yil, @sure, @eklenme);";

            using var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@ad", $"Parça {i:000}");
            cmd.Parameters.AddWithValue("@sanatci", artistId);
            cmd.Parameters.AddWithValue("@tur", genreId);
            cmd.Parameters.AddWithValue("@album", $"Albüm {((i - 1) % 40) + 1:00}");
            cmd.Parameters.AddWithValue("@yil", 1990 + (i % 34));
            cmd.Parameters.AddWithValue("@sure", 120 + (i % 240));
            cmd.Parameters.AddWithValue("@eklenme", DateTime.Now.AddHours(-i));

            if (hasSongOwnerColumn)
            {
                var ownerId = userIds[(i - 1) % userIds.Count];
                cmd.Parameters.AddWithValue("@kullanici_id", ownerId);
            }

            if (cmd.ExecuteNonQuery() > 0)
            {
                count++;
            }

            i++;
        }
    }

    private static int GetCount(MySqlConnection connection, string table)
    {
        using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM {table};", connection);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    private static List<int> GetIds(MySqlConnection connection, string table, string idColumn)
    {
        var result = new List<int>();
        using var cmd = new MySqlCommand($"SELECT {idColumn} FROM {table} ORDER BY {idColumn};", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(reader.GetInt32(0));
        }

        return result;
    }
}









