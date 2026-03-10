# Kütüphane Projesi Raporu

## Proje Amacı

Bu proje kapsamında, veritabanı bağlantılı çalışan, kullanıcı ve yönetici rollerine sahip bir masaüstü kütüphane uygulaması geliştirilmiştir. Uygulama ile kitap yönetimi, ödünç işlemleri ve yönetim paneli süreçleri tek ekranda yönetilebilir.

## Kullanılan Teknolojiler

- C#
- .NET 8 (WinForms)
- MySQL
- `MySql.Data`

## Mimari ve Dosya Düzeni

- `Data`: bağlantı ve veritabanı başlatıcı
- `Models`: model/oturum sınıfları
- `Security`: şifre hash/doğrulama
- `Services`: kimlik doğrulama katmanı
- `UI`: giriş, ana panel ve kayıt ekranları

## Veritabanı Tasarımı

Temel tablolar:

- `kullanicilar`
- `yazarlar`
- `turler`
- `kitaplar`
- `odunc_islemleri`

İlişkiler foreign key yapısı ile tanımlanmıştır. Türkçe karakter desteği için `utf8mb4` ve `utf8mb4_turkish_ci` kullanılmıştır.

## Kimlik Doğrulama

- Giriş kullanıcı adı + şifre ile yapılır
- Şifreler hash olarak tutulur
- Rol bazlı yetki kontrolü uygulanır

## Fonksiyonel Özellikler

- Kitap arama ve listeleme
- Yazar/tür filtreleme
- Yönetici işlemleri (kullanıcı, kitap, yazar, tür yönetimi)
- Ödünç kaydı oluşturma ve kayıt listeleme
- Stok düşümü ve kontrolü

## Seed ve Veri Hazırlığı

`database/schema_seed_200.sql` ile tüm ana tablolar için 200’er kayıt oluşturulur.

Demo kullanıcılar:

- `admin / Admin123!`
- `kullanici / Kullanici123!`
