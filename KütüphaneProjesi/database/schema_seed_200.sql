SET NAMES utf8mb4;

DROP DATABASE IF EXISTS kutuphane;
CREATE DATABASE kutuphane CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;
USE kutuphane;

CREATE TABLE kullanicilar (
    id INT AUTO_INCREMENT PRIMARY KEY,
    isim VARCHAR(120) NOT NULL UNIQUE,
    sifre_hash VARCHAR(500) NOT NULL,
    rol ENUM('Kullanici','Yonetici') NOT NULL DEFAULT 'Kullanici',
    kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE yazarlar (
    id INT AUTO_INCREMENT PRIMARY KEY,
    yazar_adi VARCHAR(180) NOT NULL UNIQUE
);

CREATE TABLE turler (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tur_adi VARCHAR(180) NOT NULL UNIQUE
);

CREATE TABLE kitaplar (
    id INT AUTO_INCREMENT PRIMARY KEY,
    kitap_adi VARCHAR(220) NOT NULL,
    yazar_id INT NOT NULL,
    tur_id INT NOT NULL,
    yayinevi VARCHAR(180),
    basim_yili INT,
    stok INT NOT NULL DEFAULT 5,
    eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_kitap_yazar (kitap_adi, yazar_id),
    FOREIGN KEY (yazar_id) REFERENCES yazarlar(id) ON UPDATE CASCADE ON DELETE RESTRICT,
    FOREIGN KEY (tur_id) REFERENCES turler(id) ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE odunc_islemleri (
    id INT AUTO_INCREMENT PRIMARY KEY,
    kullanici_id INT NOT NULL,
    kitap_id INT NOT NULL,
    odunc_tarihi DATE NOT NULL,
    iade_tarihi DATE NULL,
    durum ENUM('Oduncte','IadeEdildi') NOT NULL DEFAULT 'Oduncte',
    FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (kitap_id) REFERENCES kitaplar(id) ON UPDATE CASCADE ON DELETE CASCADE
);

INSERT INTO kullanicilar (isim, sifre_hash, rol, kayit_tarihi)
VALUES
('admin', '120000.d4mx8N7gLnFUbFCp2iDFJQ==.zEGJftkq/Hig9oVgu4f2xkT47Mxa9y0kfg1Hm1Cqweo=', 'Yonetici', NOW() - INTERVAL 250 DAY),
('kullanici', '120000.gIskqurmcu2P5n7bXNURGQ==.Hqy9O5Two7CSqdwmOOP5Y6NnYSguoo0bUrxVDHHc35M=', 'Kullanici', NOW() - INTERVAL 200 DAY);

WITH RECURSIVE sayilar AS (
    SELECT 3 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO kullanicilar (isim, sifre_hash, rol, kayit_tarihi)
SELECT
    CONCAT('uye', LPAD(n, 3, '0')),
    '120000.gIskqurmcu2P5n7bXNURGQ==.Hqy9O5Two7CSqdwmOOP5Y6NnYSguoo0bUrxVDHHc35M=',
    CASE WHEN MOD(n, 25) = 0 THEN 'Yonetici' ELSE 'Kullanici' END,
    NOW() - INTERVAL n DAY
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO yazarlar (yazar_adi)
SELECT CONCAT('Yazar ', LPAD(n, 3, '0'))
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO turler (tur_adi)
SELECT CONCAT('Tür ', LPAD(n, 3, '0'))
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO kitaplar (kitap_adi, yazar_id, tur_id, yayinevi, basim_yili, stok, eklenme_tarihi)
SELECT
    CONCAT('Kitap ', LPAD(n, 3, '0')),
    n,
    n,
    CONCAT('Yayınevi ', LPAD(MOD(n - 1, 30) + 1, 2, '0')),
    1980 + MOD(n, 46),
    4 + MOD(n, 12),
    NOW() - INTERVAL n HOUR
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO odunc_islemleri (kullanici_id, kitap_id, odunc_tarihi, iade_tarihi, durum)
SELECT
    n,
    MOD(n + 15, 200) + 1,
    CURDATE() - INTERVAL MOD(n, 120) DAY,
    CASE WHEN MOD(n, 4) = 0 THEN CURDATE() - INTERVAL MOD(n, 100) DAY ELSE NULL END,
    CASE WHEN MOD(n, 4) = 0 THEN 'IadeEdildi' ELSE 'Oduncte' END
FROM sayilar;

SELECT 'kullanicilar' AS tablo, COUNT(*) AS kayit_sayisi FROM kullanicilar
UNION ALL
SELECT 'yazarlar', COUNT(*) FROM yazarlar
UNION ALL
SELECT 'turler', COUNT(*) FROM turler
UNION ALL
SELECT 'kitaplar', COUNT(*) FROM kitaplar
UNION ALL
SELECT 'odunc_islemleri', COUNT(*) FROM odunc_islemleri;
