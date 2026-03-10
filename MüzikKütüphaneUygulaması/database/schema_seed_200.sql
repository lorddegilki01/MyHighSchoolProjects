SET NAMES utf8mb4;

DROP DATABASE IF EXISTS muzik_kutuphanesi;
CREATE DATABASE muzik_kutuphanesi CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;
USE muzik_kutuphanesi;

CREATE TABLE kullanicilar (
    kullanici_id INT AUTO_INCREMENT PRIMARY KEY,
    isim VARCHAR(120) NOT NULL,
    email VARCHAR(200) NOT NULL UNIQUE,
    sifre VARCHAR(500) NOT NULL,
    rol ENUM('kullanici','yonetici') NOT NULL DEFAULT 'kullanici',
    kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE sanatcilar (
    sanatci_id INT AUTO_INCREMENT PRIMARY KEY,
    sanatci_adi VARCHAR(140) NOT NULL UNIQUE
);

CREATE TABLE turler (
    tur_id INT AUTO_INCREMENT PRIMARY KEY,
    tur_adi VARCHAR(140) NOT NULL UNIQUE
);

CREATE TABLE sarkilar (
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
);

CREATE TABLE listeler (
    liste_id INT AUTO_INCREMENT PRIMARY KEY,
    kullanici_id INT NOT NULL,
    liste_adi VARCHAR(160) NOT NULL,
    olusturma_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE liste_kalemleri (
    liste_kalem_id INT AUTO_INCREMENT PRIMARY KEY,
    liste_id INT NOT NULL,
    sarki_id INT NOT NULL,
    sira_no INT NOT NULL,
    eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (liste_id) REFERENCES listeler(liste_id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (sarki_id) REFERENCES sarkilar(sarki_id) ON UPDATE CASCADE ON DELETE RESTRICT
);

INSERT INTO kullanicilar (isim, email, sifre, rol, kayit_tarihi)
VALUES
('Sistem Yöneticisi', 'yonetici@muzik.local', '120000./FrXwtFLlpKdoBOw64ZqjA==.GKZQVFQcwitYR91Zud7TEdWWrR3BQqUR8kssev6clOg=', 'yonetici', NOW() - INTERVAL 250 DAY),
('Demo Kullanıcı', 'kullanici@muzik.local', '120000.607Ib2jxxRl9RIoIfA/v7Q==.bIZyLEa//rXhhkBuAzM+GX+YpOWVPwLqWAU0P/nvaek=', 'kullanici', NOW() - INTERVAL 240 DAY);

WITH RECURSIVE sayilar AS (
    SELECT 3 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO kullanicilar (isim, email, sifre, rol, kayit_tarihi)
SELECT
    CONCAT('Müzik Kullanıcısı ', LPAD(n, 3, '0')),
    CONCAT('kullanici', LPAD(n, 3, '0'), '@muzik.local'),
    '120000.607Ib2jxxRl9RIoIfA/v7Q==.bIZyLEa//rXhhkBuAzM+GX+YpOWVPwLqWAU0P/nvaek=',
    CASE WHEN MOD(n, 25) = 0 THEN 'yonetici' ELSE 'kullanici' END,
    NOW() - INTERVAL n DAY
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO sanatcilar (sanatci_adi)
SELECT CONCAT(
    ELT(MOD(n, 10) + 1, 'Anadolu', 'Ritim', 'Akustik', 'Dijital', 'Kuzey', 'Güney', 'Mavi', 'Kırmızı', 'Gece', 'Sabah'),
    ' Sanatçı ',
    LPAD(n, 3, '0')
)
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO turler (tur_adi)
SELECT CONCAT(
    ELT(MOD(n, 10) + 1, 'Pop', 'Rock', 'Rap', 'Caz', 'Klasik', 'Folk', 'Elektronik', 'Ambient', 'Metal', 'Soul'),
    ' Tür ',
    LPAD(n, 3, '0')
)
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO sarkilar (isim, sanatci_id, tur_id, album, yil, sure_saniye, eklenme_tarihi)
SELECT
    CONCAT('Parça ', LPAD(n, 3, '0')),
    n,
    n,
    CONCAT('Albüm ', LPAD(MOD(n - 1, 40) + 1, 2, '0')),
    1990 + MOD(n, 34),
    120 + MOD(n, 240),
    NOW() - INTERVAL n HOUR
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO listeler (kullanici_id, liste_adi, olusturma_tarihi)
SELECT
    n,
    CONCAT('Liste ', LPAD(n, 3, '0')),
    NOW() - INTERVAL MOD(n, 500) HOUR
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO liste_kalemleri (liste_id, sarki_id, sira_no, eklenme_tarihi)
SELECT
    n,
    MOD(n + 10, 200) + 1,
    MOD(n, 20) + 1,
    NOW() - INTERVAL MOD(n, 1000) MINUTE
FROM sayilar;

SELECT 'kullanicilar' AS tablo, COUNT(*) AS kayit_sayisi FROM kullanicilar
UNION ALL
SELECT 'sanatcilar', COUNT(*) FROM sanatcilar
UNION ALL
SELECT 'turler', COUNT(*) FROM turler
UNION ALL
SELECT 'sarkilar', COUNT(*) FROM sarkilar
UNION ALL
SELECT 'listeler', COUNT(*) FROM listeler
UNION ALL
SELECT 'liste_kalemleri', COUNT(*) FROM liste_kalemleri;

