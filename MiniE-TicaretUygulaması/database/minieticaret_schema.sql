SET NAMES utf8mb4;

DROP DATABASE IF EXISTS minieticaret;
CREATE DATABASE minieticaret CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;
USE minieticaret;

CREATE TABLE kullanicilar (
    kullanici_id INT AUTO_INCREMENT PRIMARY KEY,
    isim VARCHAR(120) NOT NULL,
    email VARCHAR(200) NOT NULL UNIQUE,
    sifre VARCHAR(500) NOT NULL,
    rol ENUM('kullanici','admin') NOT NULL DEFAULT 'kullanici',
    kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE kategoriler (
    kategori_id INT AUTO_INCREMENT PRIMARY KEY,
    kategori_adi VARCHAR(120) NOT NULL UNIQUE
);

CREATE TABLE urunler (
    urun_id INT AUTO_INCREMENT PRIMARY KEY,
    urun_adi VARCHAR(180) NOT NULL,
    kategori_id INT NOT NULL,
    fiyat DECIMAL(12,2) NOT NULL,
    stok INT NOT NULL DEFAULT 0,
    eklenme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_urun_kategori FOREIGN KEY (kategori_id)
        REFERENCES kategoriler(kategori_id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE siparisler (
    siparis_id INT AUTO_INCREMENT PRIMARY KEY,
    kullanici_id INT NOT NULL,
    tarih DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    toplam_tutar DECIMAL(12,2) NOT NULL,
    durum VARCHAR(60) NOT NULL,
    CONSTRAINT fk_siparis_kullanici FOREIGN KEY (kullanici_id)
        REFERENCES kullanicilar(kullanici_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE siparis_kalemleri (
    siparis_kalem_id INT AUTO_INCREMENT PRIMARY KEY,
    siparis_id INT NOT NULL,
    urun_id INT NOT NULL,
    adet INT NOT NULL,
    birim_fiyat DECIMAL(12,2) NOT NULL,
    ara_toplam DECIMAL(12,2) NOT NULL,
    CONSTRAINT fk_kalem_siparis FOREIGN KEY (siparis_id)
        REFERENCES siparisler(siparis_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT fk_kalem_urun FOREIGN KEY (urun_id)
        REFERENCES urunler(urun_id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

INSERT INTO kullanicilar (isim, email, sifre, rol, kayit_tarihi)
VALUES
('Sistem Yöneticisi', 'admin@minieticaret.local', '120000.eZxSkZSEKmZ7J+zHuithsg==.2t38Raiku4zgqI8PdNV82sboyYX+RIByQCv3S05ix2Q=', 'admin', NOW() - INTERVAL 240 DAY),
('Demo Kullanıcı', 'kullanici@minieticaret.local', '120000.H5JDxBh9hESmyfaQEJmP9Q==.Tyy0qdKLYCsJMP2ieO0wEG6qNP0b7yFcApP83saF4MQ=', 'kullanici', NOW() - INTERVAL 230 DAY);

WITH RECURSIVE sayilar AS (
    SELECT 3 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO kullanicilar (isim, email, sifre, rol, kayit_tarihi)
SELECT
    CONCAT('Müşteri ', LPAD(n, 3, '0')),
    CONCAT('musteri', LPAD(n, 3, '0'), '@minieticaret.local'),
    '120000.H5JDxBh9hESmyfaQEJmP9Q==.Tyy0qdKLYCsJMP2ieO0wEG6qNP0b7yFcApP83saF4MQ=',
    CASE WHEN MOD(n, 25) = 0 THEN 'admin' ELSE 'kullanici' END,
    NOW() - INTERVAL n DAY
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO kategoriler (kategori_adi)
SELECT CONCAT(
    'Kategori ', LPAD(n, 3, '0'), ' - ',
    ELT(MOD(n, 12) + 1,
        'Elektronik', 'Moda', 'Kitap', 'Ev Yaşam', 'Spor', 'Hobi',
        'Mutfak', 'Kırtasiye', 'Bahçe', 'Kişisel Bakım', 'Oyuncak', 'Ofis')
)
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO urunler (urun_adi, kategori_id, fiyat, stok, eklenme_tarihi)
SELECT
    CONCAT(
        ELT(MOD(n, 10) + 1,
            'Akıllı', 'Pratik', 'Şık', 'Güçlü', 'Premium',
            'Ekonomik', 'Dayanıklı', 'Konfor', 'Hızlı', 'Mini'),
        ' ',
        ELT(MOD(n, 12) + 1,
            'Kulaklık', 'Sırt Çantası', 'Roman', 'Masa Lambası', 'Koşu Ayakkabısı',
            'Kahve Makinesi', 'Kalem Seti', 'Kamp Çadırı', 'Bakım Seti', 'Oyuncak Araba',
            'Dizüstü Standı', 'Duvar Saati'),
        ' ',
        ELT(MOD(n, 8) + 1, 'Pro', 'Plus', 'Lite', 'Max', 'Ultra', 'One', 'Go', 'Prime')
    ),
    n,
    ROUND(79 + (n * 5.35), 2),
    20 + MOD(n * 7, 180),
    NOW() - INTERVAL n HOUR
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO siparisler (kullanici_id, tarih, toplam_tutar, durum)
SELECT
    MOD(n - 1, 200) + 1,
    NOW() - INTERVAL MOD(n * 3, 1440) HOUR,
    0,
    ELT(MOD(n, 4) + 1, 'Hazırlanıyor', 'Kargoda', 'Teslim Edildi', 'İptal Edildi')
FROM sayilar;

WITH RECURSIVE sayilar AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM sayilar WHERE n < 200
)
INSERT INTO siparis_kalemleri (siparis_id, urun_id, adet, birim_fiyat, ara_toplam)
SELECT
    n,
    MOD(n - 1, 200) + 1,
    MOD(n - 1, 5) + 1,
    u.fiyat,
    ROUND(u.fiyat * (MOD(n - 1, 5) + 1), 2)
FROM sayilar s
JOIN urunler u ON u.urun_id = MOD(s.n - 1, 200) + 1;

UPDATE siparisler s
JOIN (
    SELECT siparis_id, ROUND(SUM(ara_toplam), 2) AS hesaplanan_toplam
    FROM siparis_kalemleri
    GROUP BY siparis_id
) k ON k.siparis_id = s.siparis_id
SET s.toplam_tutar = k.hesaplanan_toplam;

SELECT 'kullanicilar' AS tablo, COUNT(*) AS kayit_sayisi FROM kullanicilar
UNION ALL
SELECT 'kategoriler', COUNT(*) FROM kategoriler
UNION ALL
SELECT 'urunler', COUNT(*) FROM urunler
UNION ALL
SELECT 'siparisler', COUNT(*) FROM siparisler
UNION ALL
SELECT 'siparis_kalemleri', COUNT(*) FROM siparis_kalemleri;
