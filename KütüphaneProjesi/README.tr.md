# Kütüphane Projesi

C# WinForms, .NET 8 ve MySQL ile geliştirilmiş rol tabanlı masaüstü kütüphane yönetim uygulaması.

## Temel Özellikler

- Rol bazlı giriş (`Yonetici` / `Kullanici`)
- Kitap adına, yazara ve türe göre arama/filtreleme
- Kitap CRUD işlemleri (yönetici)
- Yazar ve tür yönetimi (yönetici)
- Kullanıcı listeleme, yetki güncelleme ve kullanıcı temizleme (yönetici)
- Stok güvenli ödünç işlemleri:
  - ödünç kaydı oluşturma
  - seçili ödünç kaydını güncelleme
  - seçili ödünç kaydını silme
- Mor temalı Türkçe arayüz

## Proje Yapısı

```text
KütüphaneProjesi/
|-- database/
|   `-- schema_seed_200.sql
|-- KütüphaneProjesi.sln
|-- KütüphaneProjesi/
|   |-- App.config
|   |-- Data/
|   |-- Models/
|   |-- Security/
|   |-- Services/
|   `-- UI/
`-- ProjeRaporu.md
```

## Veritabanı Kurulumu

1. MySQL servisini başlatın.
2. `database/schema_seed_200.sql` scriptini çalıştırın.
3. `KütüphaneProjesi/App.config` içindeki `MySqlConnection` değerini doğrulayın.

Seed standardı: her ana tablo için **200 kayıt** hazırlanmıştır.

## Çalıştırma

1. `KütüphaneProjesi.sln` dosyasını açın.
2. Visual Studio üzerinden build alıp çalıştırın.

## Demo Hesaplar

- `admin` / `Admin123!`
- `kullanici` / `Kullanici123!`

## Notlar

- Arayüz metinlerinde ve dokümanlarda Türkçe karakter kullanılır.
- GitHub taşınabilirliği için göreli dosya yolları kullanılır.
