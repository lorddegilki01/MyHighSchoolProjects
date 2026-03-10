# Mini E-Ticaret Uygulaması

Mini E-Ticaret Uygulaması, C# WinForms, .NET 8 ve MySQL ile geliştirilmiş rol tabanlı masaüstü alışveriş sistemidir.

## Okul Zorunluluğu

Bu proje lise kapsamında hazırlanmıştır ve **C# + .NET + MySQL** kullanımı zorunludur.

## Temel Özellikler

- Hashlenmiş şifrelerle güvenli giriş
- Rol bazlı panel yönlendirme (`admin` / `kullanici`)
- Ürün listeleme, kategori filtreleme, arama ve sıralama
- Sepet işlemleri: ekleme, çıkarma, adet güncelleme
- Sipariş oluşturma ve sipariş geçmişi takibi
- Yönetici modülleri: ürün, kategori, kullanıcı ve sipariş yönetimi

## Proje Yapısı

```text
MiniE-TicaretUygulaması/
|-- database/
|   `-- minieticaret_schema.sql
|-- MiniE-TicaretUygulaması.sln
|-- MiniE-TicaretUygulaması/
|   |-- App.config
|   |-- Data/
|   |-- Models/
|   |-- Services/
|   `-- UI/
`-- ProjeRaporu.md
```

## Veritabanı Kurulumu

1. MySQL servisini başlatın.
2. `database/minieticaret_schema.sql` scriptini çalıştırın.
3. `MiniE-TicaretUygulaması/App.config` içindeki `MySqlConnection` bilgisini kontrol edin.

Seed standardı: her ana tablo için **200 kayıt** hazırlanmıştır.

## Çalıştırma

1. `MiniE-TicaretUygulaması.sln` dosyasını açın.
2. Visual Studio üzerinden build alıp çalıştırın.

## Doğrulanmış Demo Hesaplar

- Yönetici (Kullanıcı adı): `Sistem Yöneticisi` / `Admin123!`
- Kullanıcı (Kullanıcı adı): `Demo Kullanıcı` / `Kullanici123!`

## Notlar

- Arayüz metinlerinde Türkçe karakter kullanımı korunmuştur.
- Dokümantasyonda makineye özel tam dosya yolları kullanılmaz.
