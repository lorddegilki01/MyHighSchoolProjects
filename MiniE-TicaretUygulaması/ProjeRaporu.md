# Mini E-Ticaret Masaüstü Uygulaması Proje Raporu

## 1. Proje Amacı
Bu proje kapsamında C# ve .NET kullanılarak, MySQL veritabanı ile çalışan mini bir e-ticaret masaüstü uygulaması geliştirilmiştir.
Uygulama, kullanıcı ve admin rollerini destekler; ürün listeleme/arama, sepet işlemleri, sipariş oluşturma ve yönetim modüllerini kapsar.

## 2. Kullanılan Teknolojiler
- Dil: C#
- Platform: .NET 9 WinForms (masaüstü)
- Veritabanı: MySQL
- Veri erişim kütüphanesi: MySqlConnector
- Güvenlik: PBKDF2-SHA256 şifre hashleme

## 3. Veritabanı Tasarımı
Veritabanı ilişkisel yapıda tasarlanmıştır.

### 3.1 Tablolar
1. `kullanicilar`
- `kullanici_id` (PK)
- `isim`
- `email` (unique)
- `sifre` (hashlenmiş)
- `rol` (`kullanici` / `admin`)
- `kayit_tarihi`

2. `kategoriler`
- `kategori_id` (PK)
- `kategori_adi`

3. `urunler`
- `urun_id` (PK)
- `urun_adi`
- `kategori_id` (FK -> kategoriler)
- `fiyat`
- `stok`
- `eklenme_tarihi`

4. `siparisler`
- `siparis_id` (PK)
- `kullanici_id` (FK -> kullanicilar)
- `tarih`
- `toplam_tutar`
- `durum`

5. `siparis_kalemleri`
- `siparis_kalem_id` (PK)
- `siparis_id` (FK -> siparisler)
- `urun_id` (FK -> urunler)
- `adet`
- `birim_fiyat`
- `ara_toplam`

## 4. Sistem Özellikleri
### 4.1 Giriş Sistemi
- Rol bazlı giriş (`kullanici`/`admin`)
- Email + şifre ile kimlik doğrulama
- Hatalı girişte uyarı mesajı
- Başarılı girişte oturum oluşturma

### 4.2 Güvenlik
- Şifreler düz metin tutulmaz.
- PBKDF2 (SHA256) ile hashlenerek saklanır.

### 4.3 Kullanıcı Paneli
- Tüm ürünleri listeleme
- Kategoriye göre filtreleme
- Fiyata göre sıralama
- Ürün adına göre arama
- Sepete ürün ekleme/silme/adet güncelleme
- Sipariş oluşturma
- Sipariş geçmişini görüntüleme

### 4.4 Admin Paneli
- Ürün yönetimi: ekle, güncelle, sil, ara, listele
- Kategori yönetimi: ekle, güncelle, sil
- Kullanıcı yönetimi: listele, sil
- Sipariş yönetimi: listele, durum güncelle

## 5. Uygulama Akışı
1. Uygulama açılışında veritabanı ve tablolar kontrol edilir, yoksa otomatik oluşturulur.
2. Demo admin ve demo kullanıcı hesapları otomatik eklenir/güncellenir.
3. Tüm ana tablolarda (`kullanicilar`, `kategoriler`, `urunler`, `siparisler`, `siparis_kalemleri`) en az 200 kayıt olacak şekilde otomatik seed işlemi yapılır.
4. Giriş yapan kullanıcının rolüne göre uygun ekranlar açılır.

## 6. Varsayılan Demo Hesaplar
- Admin:
  - Email: `admin@minieticaret.local`
  - Şifre: `Admin123!`
- Kullanıcı:
  - Email: `kullanici@minieticaret.local`
  - Şifre: `Kullanici123!`

## 7. Kurulum ve Çalıştırma
1. `App.config` dosyasında MySQL bağlantı bilgisini güncelleyin.
2. MySQL servisinin çalıştığından emin olun.
3. Projeyi derleyin:
   - `dotnet build MiniE-TicaretUygulaması.sln`
4. Uygulamayı çalıştırın (Visual Studio veya `dotnet run`).

## 8. Veritabanı Dosyası
Veritabanı şema dosyası aşağıdadır:
- `database/minieticaret_schema.sql`

## 9. Sonuç
Proje kapsamında istenen rol bazlı e-ticaret fonksiyonları tamamlanmış,
MySQL bağlantılı, şifre güvenliği olan, kullanıcı ve admin modüllerine sahip
çalışan bir mini masaüstü uygulaması geliştirilmiştir.
