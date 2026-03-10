using MiniE_TicaretUygulaması.Security;
using MySqlConnector;

namespace MiniE_TicaretUygulaması.Data;

public static class DatabaseInitializer
{
    private const int SeedTargetCount = 200;

    private static readonly string[] UserFirstNames =
    [
        "Ahmet", "Mehmet", "Ayşe", "Fatma", "Can", "Deniz", "Ece", "Zeynep",
        "Mert", "Selin", "Kerem", "Aslı", "Yusuf", "Elif", "Onur", "Derya",
        "Burak", "Ceren", "Emre", "Seda"
    ];

    private static readonly string[] UserLastNames =
    [
        "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Aydın", "Arslan", "Polat",
        "Özkan", "Kurt", "Acar", "Taş", "Akın", "Erdem", "Güneş", "Doğan",
        "Yıldız", "Koç", "Aksoy", "Eren"
    ];

    private static readonly string[] CategoryRoots =
    [
        "Elektronik", "Moda", "Kitap", "Ev", "Mutfak", "Kişisel Bakım", "Spor", "Oyuncak",
        "Ofis", "Bahçe", "Pet", "Kırtasiye", "Anne Bebek", "Müzik", "Yapı", "Hobi"
    ];

    private static readonly string[] CategoryThemes =
    [
        "Aksesuarları", "Koleksiyonu", "Dünyası", "Çözümleri", "Serisi", "Ürünleri", "Seçkisi",
        "Marketi", "Atölyesi", "Reyonu", "Merkezi", "Paketi", "Setleri"
    ];

    private static readonly string[] ProductBrands =
    [
        "Nova", "Atlas", "Luna", "Mira", "Zen", "Artemis", "Pera", "Nex", "Vega", "Aura",
        "Trendio", "Optima", "Mobi", "Form", "Pratik", "Linea"
    ];

    private static readonly string[] ProductTypes =
    [
        "Bluetooth Kulaklık", "Kablosuz Mouse", "Sırt Çantası", "Termos", "Masa Lambası", "Roman",
        "Defter Seti", "Yoga Matı", "Kamp Sandalyesi", "Su Sebili", "Tava Seti", "Akıllı Saat",
        "Tişört", "Sweatshirt", "Eğitici Oyun", "Powerbank"
    ];

    private static readonly string[] ProductSeries =
    [
        "Pro", "Plus", "Lite", "Max", "Prime", "Ultra", "X", "S", "One", "Go"
    ];

    private static readonly string[] CreateTableScripts =
    [
        @"CREATE TABLE IF NOT EXISTS kullanicilar (
            kullanici_id INT AUTO_INCREMENT PRIMARY KEY,
            isim VARCHAR(120) NOT NULL,
            email VARCHAR(200) NOT NULL UNIQUE,
            sifre VARCHAR(500) NOT NULL,
            rol ENUM('kullanici','admin') NOT NULL DEFAULT 'kullanici',
            kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        );",
        @"CREATE TABLE IF NOT EXISTS kategoriler (
            kategori_id INT AUTO_INCREMENT PRIMARY KEY,
            kategori_adi VARCHAR(120) NOT NULL UNIQUE
        );",
        @"CREATE TABLE IF NOT EXISTS urunler (
            urun_id INT AUTO_INCREMENT PRIMARY KEY,
            urun_adi VARCHAR(180) NOT NULL,
            kategori_id INT NOT NULL,
            fiyat DECIMAL(12,2) NOT NULL,
            stok INT NOT NULL DEFAULT 0,
            eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            CONSTRAINT fk_urun_kategori FOREIGN KEY (kategori_id) REFERENCES kategoriler(kategori_id)
                ON UPDATE CASCADE
                ON DELETE RESTRICT
        );",
        @"CREATE TABLE IF NOT EXISTS siparisler (
            siparis_id INT AUTO_INCREMENT PRIMARY KEY,
            kullanici_id INT NOT NULL,
            tarih DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            toplam_tutar DECIMAL(12,2) NOT NULL,
            durum VARCHAR(60) NOT NULL,
            CONSTRAINT fk_siparis_kullanici FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id)
                ON UPDATE CASCADE
                ON DELETE CASCADE
        );",
        @"CREATE TABLE IF NOT EXISTS siparis_kalemleri (
            siparis_kalem_id INT AUTO_INCREMENT PRIMARY KEY,
            siparis_id INT NOT NULL,
            urun_id INT NOT NULL,
            adet INT NOT NULL,
            birim_fiyat DECIMAL(12,2) NOT NULL,
            ara_toplam DECIMAL(12,2) NOT NULL,
            CONSTRAINT fk_kalem_siparis FOREIGN KEY (siparis_id) REFERENCES siparisler(siparis_id)
                ON UPDATE CASCADE
                ON DELETE CASCADE,
            CONSTRAINT fk_kalem_urun FOREIGN KEY (urun_id) REFERENCES urunler(urun_id)
                ON UPDATE CASCADE
                ON DELETE RESTRICT
        );"
    ];

    private sealed class SeedProduct
    {
        public int UrunId { get; init; }
        public decimal Fiyat { get; init; }
    }

    public static void Initialize()
    {
        EnsureDatabaseExists();

        using var connection = Database.CreateConnection();
        connection.Open();

        foreach (var script in CreateTableScripts)
        {
            using var cmd = new MySqlCommand(script, connection);
            cmd.ExecuteNonQuery();
        }

        EnsureLegacySchema(connection);
        SeedDefaultData(connection);
    }

    private static void EnsureDatabaseExists()
    {
        var builder = new MySqlConnectionStringBuilder(Database.ConnectionString);
        if (string.IsNullOrWhiteSpace(builder.Database))
        {
            throw new InvalidOperationException("Connection string içinde Database alanı zorunludur.");
        }

        var databaseName = builder.Database;
        builder.Database = string.Empty;

        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();

        using var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;", connection);
        cmd.ExecuteNonQuery();
    }

    private static void SeedDefaultData(MySqlConnection connection)
    {
        EnsureBaseAccounts(connection);
        EnsureUsers(connection, SeedTargetCount);
        EnsureCategories(connection, SeedTargetCount);
        EnsureProducts(connection, SeedTargetCount);
        NormalizeLegacyNames(connection);
        EnsureOrdersAndItems(connection, SeedTargetCount, SeedTargetCount);
        NormalizeTurkishTextData(connection);
    }

    private static void EnsureBaseAccounts(MySqlConnection connection)
    {
        EnsureAccount(
            connection,
            isim: "Sistem Yöneticisi",
            email: "admin@minieticaret.local",
            password: "Admin123!",
            rol: "admin");

        EnsureAccount(
            connection,
            isim: "Demo Kullanıcı",
            email: "kullanici@minieticaret.local",
            password: "Kullanici123!",
            rol: "kullanici");
    }

    private static void EnsureAccount(
        MySqlConnection connection,
        string isim,
        string email,
        string password,
        string rol)
    {
        using var upsert = new MySqlCommand(
            @"INSERT INTO kullanicilar (isim, email, sifre, rol)
              VALUES (@isim, @email, @sifre, @rol)
              ON DUPLICATE KEY UPDATE
                isim = VALUES(isim),
                sifre = VALUES(sifre),
                rol = VALUES(rol);",
            connection);

        upsert.Parameters.AddWithValue("@isim", isim);
        upsert.Parameters.AddWithValue("@email", email);
        upsert.Parameters.AddWithValue("@sifre", PasswordHasher.HashPassword(password));
        upsert.Parameters.AddWithValue("@rol", rol);
        upsert.ExecuteNonQuery();
    }

    private static void EnsureUsers(MySqlConnection connection, int targetCount)
    {
        var existingCount = GetCount(connection, "kullanicilar");
        if (existingCount >= targetCount)
        {
            return;
        }

        var defaultUserHash = PasswordHasher.HashPassword("Kullanici123!");
        var index = 1;

        while (existingCount < targetCount)
        {
            var email = $"seed{index:000}@minieticaret.local";
            var role = index % 25 == 0 ? "admin" : "kullanici";
            var isim = BuildUserName(index);

            using var cmd = new MySqlCommand(
                @"INSERT IGNORE INTO kullanicilar (isim, email, sifre, rol, kayit_tarihi)
                  VALUES (@isim, @email, @sifre, @rol, @kayit_tarihi);",
                connection);

            cmd.Parameters.AddWithValue("@isim", isim);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@sifre", defaultUserHash);
            cmd.Parameters.AddWithValue("@rol", role);
            cmd.Parameters.AddWithValue("@kayit_tarihi", DateTime.Now.AddDays(-index));

            if (cmd.ExecuteNonQuery() > 0)
            {
                existingCount++;
            }

            index++;
        }
    }

    private static void EnsureCategories(MySqlConnection connection, int targetCount)
    {
        var baseCategories = new[] { "Elektronik", "Giyim", "Kitap", "Ev Yaşam" };

        foreach (var categoryName in baseCategories)
        {
            using var cmd = new MySqlCommand("INSERT IGNORE INTO kategoriler (kategori_adi) VALUES (@kategori_adi);", connection);
            cmd.Parameters.AddWithValue("@kategori_adi", categoryName);
            cmd.ExecuteNonQuery();
        }

        var existingCount = GetCount(connection, "kategoriler");
        var index = 1;

        while (existingCount < targetCount)
        {
            using var cmd = new MySqlCommand("INSERT IGNORE INTO kategoriler (kategori_adi) VALUES (@kategori_adi);", connection);
            cmd.Parameters.AddWithValue("@kategori_adi", BuildCategoryName(index));

            if (cmd.ExecuteNonQuery() > 0)
            {
                existingCount++;
            }

            index++;
        }
    }

    private static void EnsureProducts(MySqlConnection connection, int targetCount)
    {
        var categoryIds = GetIntList(connection, "SELECT kategori_id FROM kategoriler ORDER BY kategori_id;");
        if (categoryIds.Count == 0)
        {
            throw new InvalidOperationException("Seed işlemi için en az bir kategori bulunmalıdır.");
        }

        var existingCount = GetCount(connection, "urunler");
        if (existingCount >= targetCount)
        {
            return;
        }

        var random = new Random(20260310);
        var missing = targetCount - existingCount;

        for (var i = 1; i <= missing; i++)
        {
            var sequence = existingCount + i;
            var kategoriId = categoryIds[random.Next(categoryIds.Count)];
            var fiyat = Math.Round((decimal)(random.NextDouble() * 4500d + 50d), 2);
            var stok = random.Next(70, 350);
            var eklenme = DateTime.Now.AddDays(-random.Next(0, 730));

            using var cmd = new MySqlCommand(
                @"INSERT INTO urunler (urun_adi, kategori_id, fiyat, stok, eklenme_tarihi)
                  VALUES (@urun_adi, @kategori_id, @fiyat, @stok, @eklenme_tarihi);",
                connection);

            cmd.Parameters.AddWithValue("@urun_adi", BuildProductName(sequence));
            cmd.Parameters.AddWithValue("@kategori_id", kategoriId);
            cmd.Parameters.AddWithValue("@fiyat", fiyat);
            cmd.Parameters.AddWithValue("@stok", stok);
            cmd.Parameters.AddWithValue("@eklenme_tarihi", eklenme);
            cmd.ExecuteNonQuery();
        }
    }

    private static void NormalizeLegacyNames(MySqlConnection connection)
    {
        RenameLegacyUsers(connection);
        RenameLegacyCategories(connection);
        RenameLegacyProducts(connection);
    }

    private static void NormalizeTurkishTextData(MySqlConnection connection)
    {
        var replacements = new (string TableName, string ColumnName, string FromText, string ToText)[]
        {
            ("kullanicilar", "isim", "Sistem Yoneticisi", "Sistem Yöneticisi"),
            ("kullanicilar", "isim", "Demo Kullanici", "Demo Kullanıcı"),
            ("kullanicilar", "isim", "Aydin", "Aydın"),
            ("kullanicilar", "isim", "Tas", "Taş"),
            ("kullanicilar", "isim", "Dogan", "Doğan"),
            ("kategoriler", "kategori_adi", "Ev Yasam", "Ev Yaşam"),
            ("kategoriler", "kategori_adi", "Kisisel Bakim", "Kişisel Bakım"),
            ("kategoriler", "kategori_adi", "Bahce", "Bahçe"),
            ("kategoriler", "kategori_adi", "Kirtasiye", "Kırtasiye"),
            ("kategoriler", "kategori_adi", "Muzik", "Müzik"),
            ("kategoriler", "kategori_adi", "Yapi", "Yapı"),
            ("kategoriler", "kategori_adi", "Aksesuarlari", "Aksesuarları"),
            ("kategoriler", "kategori_adi", "Dunyasi", "Dünyası"),
            ("kategoriler", "kategori_adi", "Cozumleri", "Çözümleri"),
            ("kategoriler", "kategori_adi", "Urunleri", "Ürünleri"),
            ("kategoriler", "kategori_adi", "Seckisi", "Seçkisi"),
            ("kategoriler", "kategori_adi", "Atolyesi", "Atölyesi"),
            ("urunler", "urun_adi", "Kulaklik", "Kulaklık"),
            ("urunler", "urun_adi", "Sirt Cantasi", "Sırt Çantası"),
            ("urunler", "urun_adi", "Lambasi", "Lambası"),
            ("urunler", "urun_adi", "Mati", "Matı"),
            ("urunler", "urun_adi", "Akilli Saat", "Akıllı Saat"),
            ("urunler", "urun_adi", "Tisort", "Tişört"),
            ("urunler", "urun_adi", "Egitici Oyun", "Eğitici Oyun"),
            ("siparisler", "durum", "Hazirlaniyor", "Hazırlanıyor"),
            ("siparisler", "durum", "Tamamlandi", "Tamamlandı"),
            ("siparisler", "durum", "Iptal", "İptal")
        };

        foreach (var replacement in replacements)
        {
            ReplaceColumnValue(
                connection,
                replacement.TableName,
                replacement.ColumnName,
                replacement.FromText,
                replacement.ToText);
        }
    }

    private static void ReplaceColumnValue(
        MySqlConnection connection,
        string tableName,
        string columnName,
        string fromText,
        string toText)
    {
        using var cmd = new MySqlCommand(
            $"UPDATE {tableName} " +
            $"SET {columnName} = REPLACE({columnName}, @fromText, @toText) " +
            $"WHERE {columnName} LIKE CONCAT('%', @fromText, '%');",
            connection);

        cmd.Parameters.AddWithValue("@fromText", fromText);
        cmd.Parameters.AddWithValue("@toText", toText);
        cmd.ExecuteNonQuery();
    }

    private static void RenameLegacyUsers(MySqlConnection connection)
    {
        using var cmd = new MySqlCommand(
            @"SELECT kullanici_id
              FROM kullanicilar
              WHERE email LIKE 'seed%@minieticaret.local'
                 OR isim LIKE 'Seed Kullanici %';",
            connection);

        using var reader = cmd.ExecuteReader();
        var userIds = new List<int>();

        while (reader.Read())
        {
            userIds.Add(reader.GetInt32(0));
        }

        reader.Close();

        foreach (var userId in userIds)
        {
            using var update = new MySqlCommand(
                "UPDATE kullanicilar SET isim = @isim WHERE kullanici_id = @kullanici_id;",
                connection);

            update.Parameters.AddWithValue("@isim", BuildUserName(userId));
            update.Parameters.AddWithValue("@kullanici_id", userId);
            update.ExecuteNonQuery();
        }
    }

    private static void RenameLegacyCategories(MySqlConnection connection)
    {
        using var cmd = new MySqlCommand(
            @"SELECT kategori_id
              FROM kategoriler
              WHERE kategori_adi LIKE 'Kategori %';",
            connection);

        using var reader = cmd.ExecuteReader();
        var categoryIds = new List<int>();

        while (reader.Read())
        {
            categoryIds.Add(reader.GetInt32(0));
        }

        reader.Close();

        foreach (var categoryId in categoryIds)
        {
            using var update = new MySqlCommand(
                "UPDATE kategoriler SET kategori_adi = @kategori_adi WHERE kategori_id = @kategori_id;",
                connection);

            update.Parameters.AddWithValue("@kategori_adi", BuildCategoryName(categoryId));
            update.Parameters.AddWithValue("@kategori_id", categoryId);
            update.ExecuteNonQuery();
        }
    }

    private static void RenameLegacyProducts(MySqlConnection connection)
    {
        using var cmd = new MySqlCommand(
            @"SELECT urun_id
              FROM urunler
              WHERE urun_adi LIKE 'Urun %';",
            connection);

        using var reader = cmd.ExecuteReader();
        var productIds = new List<int>();

        while (reader.Read())
        {
            productIds.Add(reader.GetInt32(0));
        }

        reader.Close();

        foreach (var productId in productIds)
        {
            using var update = new MySqlCommand(
                "UPDATE urunler SET urun_adi = @urun_adi WHERE urun_id = @urun_id;",
                connection);

            update.Parameters.AddWithValue("@urun_adi", BuildProductName(productId));
            update.Parameters.AddWithValue("@urun_id", productId);
            update.ExecuteNonQuery();
        }
    }


    private static void EnsureLegacySchema(MySqlConnection connection)
    {
        EnsureColumnExists(connection, "kullanicilar", "email", "VARCHAR(200) NULL");
        EnsureColumnExists(connection, "kullanicilar", "kayit_tarihi", "DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP");

        EnsureValidEmails(connection, "kullanicilar", "kullanici_id", "email", "minieticaret.local");
        EnsureUniqueEmailIndex(connection, "kullanicilar", "kullanici_id", "email", "ux_kullanicilar_email", "minieticaret.local");
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
    private static string BuildUserName(int index)
    {
        var firstName = UserFirstNames[(index - 1) % UserFirstNames.Length];
        var lastNameIndex = ((index - 1) / UserFirstNames.Length) % UserLastNames.Length;
        var lastName = UserLastNames[lastNameIndex];
        return $"{firstName} {lastName}";
    }

    private static string BuildCategoryName(int index)
    {
        var root = CategoryRoots[(index - 1) % CategoryRoots.Length];
        var themeIndex = ((index - 1) / CategoryRoots.Length) % CategoryThemes.Length;
        var theme = CategoryThemes[themeIndex];
        return $"{root} {theme} {index:000}";
    }

    private static string BuildProductName(int index)
    {
        var brand = ProductBrands[(index - 1) % ProductBrands.Length];
        var typeIndex = ((index - 1) / ProductBrands.Length) % ProductTypes.Length;
        var seriesIndex = ((index - 1) / (ProductBrands.Length * ProductTypes.Length)) % ProductSeries.Length;
        var type = ProductTypes[typeIndex];
        var series = ProductSeries[seriesIndex];
        return $"{brand} {type} {series}-{index:000}";
    }

    private static void EnsureOrdersAndItems(MySqlConnection connection, int orderTarget, int itemTarget)
    {
        var userIds = GetIntList(connection, "SELECT kullanici_id FROM kullanicilar ORDER BY kullanici_id;");
        var products = GetSeedProducts(connection);

        if (userIds.Count == 0 || products.Count == 0)
        {
            throw new InvalidOperationException("Seed siparişleri için kullanıcı ve ürün kayıtları bulunmalıdır.");
        }

        var statuses = new[] { "Hazırlanıyor", "Kargoda", "Tamamlandı", "İptal" };
        var random = new Random(20260311);

        var existingOrderCount = GetCount(connection, "siparisler");
        var missingOrders = Math.Max(0, orderTarget - existingOrderCount);

        for (var i = 0; i < missingOrders; i++)
        {
            var selectedUserId = userIds[random.Next(userIds.Count)];
            var status = statuses[random.Next(statuses.Length)];
            var orderDate = DateTime.Now.AddDays(-random.Next(0, 365)).AddMinutes(random.Next(0, 1440));

            var lineCount = Math.Min(products.Count, random.Next(1, 4));
            var selectedProducts = PickDistinctProducts(products, lineCount, random);

            decimal total = 0m;
            var lines = new List<(int UrunId, int Adet, decimal BirimFiyat, decimal AraToplam)>();

            foreach (var product in selectedProducts)
            {
                var adet = random.Next(1, 4);
                var araToplam = product.Fiyat * adet;
                total += araToplam;
                lines.Add((product.UrunId, adet, product.Fiyat, araToplam));
            }

            using var insertOrder = new MySqlCommand(
                @"INSERT INTO siparisler (kullanici_id, tarih, toplam_tutar, durum)
                  VALUES (@kullanici_id, @tarih, @toplam_tutar, @durum);",
                connection);

            insertOrder.Parameters.AddWithValue("@kullanici_id", selectedUserId);
            insertOrder.Parameters.AddWithValue("@tarih", orderDate);
            insertOrder.Parameters.AddWithValue("@toplam_tutar", total);
            insertOrder.Parameters.AddWithValue("@durum", status);
            insertOrder.ExecuteNonQuery();

            var orderId = Convert.ToInt32(insertOrder.LastInsertedId);

            foreach (var line in lines)
            {
                using var insertLine = new MySqlCommand(
                    @"INSERT INTO siparis_kalemleri (siparis_id, urun_id, adet, birim_fiyat, ara_toplam)
                      VALUES (@siparis_id, @urun_id, @adet, @birim_fiyat, @ara_toplam);",
                    connection);

                insertLine.Parameters.AddWithValue("@siparis_id", orderId);
                insertLine.Parameters.AddWithValue("@urun_id", line.UrunId);
                insertLine.Parameters.AddWithValue("@adet", line.Adet);
                insertLine.Parameters.AddWithValue("@birim_fiyat", line.BirimFiyat);
                insertLine.Parameters.AddWithValue("@ara_toplam", line.AraToplam);
                insertLine.ExecuteNonQuery();
            }
        }

        var existingItemCount = GetCount(connection, "siparis_kalemleri");
        var missingItems = Math.Max(0, itemTarget - existingItemCount);

        if (missingItems == 0)
        {
            return;
        }

        var orderIds = GetIntList(connection, "SELECT siparis_id FROM siparisler ORDER BY siparis_id;");
        if (orderIds.Count == 0)
        {
            throw new InvalidOperationException("Sipariş kaydı bulunamadığı için sipariş kalemi seed edilemedi.");
        }

        for (var i = 0; i < missingItems; i++)
        {
            var orderId = orderIds[random.Next(orderIds.Count)];
            var product = products[random.Next(products.Count)];
            var adet = random.Next(1, 4);
            var araToplam = product.Fiyat * adet;

            using var insertLine = new MySqlCommand(
                @"INSERT INTO siparis_kalemleri (siparis_id, urun_id, adet, birim_fiyat, ara_toplam)
                  VALUES (@siparis_id, @urun_id, @adet, @birim_fiyat, @ara_toplam);",
                connection);

            insertLine.Parameters.AddWithValue("@siparis_id", orderId);
            insertLine.Parameters.AddWithValue("@urun_id", product.UrunId);
            insertLine.Parameters.AddWithValue("@adet", adet);
            insertLine.Parameters.AddWithValue("@birim_fiyat", product.Fiyat);
            insertLine.Parameters.AddWithValue("@ara_toplam", araToplam);
            insertLine.ExecuteNonQuery();

            using var updateOrderTotal = new MySqlCommand(
                "UPDATE siparisler SET toplam_tutar = toplam_tutar + @ara_toplam WHERE siparis_id = @siparis_id;",
                connection);

            updateOrderTotal.Parameters.AddWithValue("@ara_toplam", araToplam);
            updateOrderTotal.Parameters.AddWithValue("@siparis_id", orderId);
            updateOrderTotal.ExecuteNonQuery();
        }
    }

    private static List<SeedProduct> PickDistinctProducts(IReadOnlyList<SeedProduct> products, int count, Random random)
    {
        var selected = new List<SeedProduct>();
        var used = new HashSet<int>();

        while (selected.Count < count && used.Count < products.Count)
        {
            var product = products[random.Next(products.Count)];
            if (used.Add(product.UrunId))
            {
                selected.Add(product);
            }
        }

        return selected;
    }

    private static int GetCount(MySqlConnection connection, string tableName)
    {
        using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM {tableName};", connection);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    private static List<int> GetIntList(MySqlConnection connection, string query)
    {
        var values = new List<int>();

        using var cmd = new MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            values.Add(reader.GetInt32(0));
        }

        return values;
    }

    private static List<SeedProduct> GetSeedProducts(MySqlConnection connection)
    {
        var products = new List<SeedProduct>();

        using var cmd = new MySqlCommand("SELECT urun_id, fiyat FROM urunler ORDER BY urun_id;", connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new SeedProduct
            {
                UrunId = reader.GetInt32(reader.GetOrdinal("urun_id")),
                Fiyat = reader.GetDecimal(reader.GetOrdinal("fiyat"))
            });
        }

        return products;
    }
}



