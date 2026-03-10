# Müzik Kütüphane Uygulaması

C# WinForms, .NET 8 ve MySQL ile geliştirilmiş rol tabanlı panellere sahip masaüstü müzik kütüphane uygulaması.

## Okul Zorunluluğu

Bu proje lise kapsamında hazırlanmıştır ve **C# + .NET + MySQL** kullanımı zorunludur.

## Temel Özellikler

- Hashlenmiş şifre ile güvenli giriş
- Rol bazlı kullanım:
  - `yonetici`: şarkı, sanatçı, tür ve kullanıcı yönetimi
  - `kullanici`: arama, listeleme ve çalma listesi akışı
- Şarkı adına, sanatçıya ve türe göre arama/filtreleme
- Çalma listesi oluşturma, kaydetme ve listeden çıkarma işlemleri
- Türkçe metinler ve profesyonel turkuaz tema

## Proje Yapısı

```text
MüzikKütüphaneUygulaması/
|-- database/
|   `-- schema_seed_200.sql
|-- MüzikKütüphaneUygulaması.sln
|-- MüzikKütüphaneUygulaması/
|   |-- App.config
|   |-- Data/
|   |-- Models/
|   |-- Security/
|   |-- Services/
|   `-- UI/
`-- README.md
```

## Veritabanı Kurulumu

1. MySQL servisini başlatın.
2. `database/schema_seed_200.sql` scriptini çalıştırın.
3. `MüzikKütüphaneUygulaması/App.config` içindeki `MySqlConnection` değerini kontrol edin.

Seed standardı: her ana tabloda **200 kayıt** bulunur.

## Çalıştırma

1. `MüzikKütüphaneUygulaması.sln` dosyasını açın.
2. Visual Studio üzerinden build alıp çalıştırın.

## Doğrulanmış Demo Hesaplar

- Yönetici (E-posta): `yonetici@muzik.local` / `Yonetici123!`
- Kullanıcı (E-posta): `kullanici@muzik.local` / `Kullanici123!`

## Notlar

- Arayüz ve dökümantasyonda Türkçe karakter kullanımı korunmuştur.
- Dokümantasyon GitHub için göreli yol standardıyla hazırlanmıştır.
