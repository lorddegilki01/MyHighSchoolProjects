# Lise Projelerim (C#, .NET, MySQL)

C# WinForms, .NET 8 ve MySQL ile geliştirilen masaüstü lise projelerimden oluşan, GitHub paylaşımına hazır proje portföyü.

Sorumlu geliştirici: **Efe** (`whoisefe`)

## İçerikteki Projeler

| Proje | Odak | Teknoloji | Veritabanı Scripti | Dokümantasyon |
|---|---|---|---|---|
| [Mini E-Ticaret Uygulaması](./MiniE-TicaretUygulaması/README.tr.md) | Ürün listeleme, sepet ve sipariş akışı | C# WinForms, .NET 8, MySqlConnector | `MiniE-TicaretUygulaması/database/minieticaret_schema.sql` | EN + TR |
| [Müzik Kütüphane Uygulaması](./MüzikKütüphaneUygulaması/README.tr.md) | Şarkı, çalma listesi ve rol tabanlı yönetim | C# WinForms, .NET 8, MySqlConnector | `MüzikKütüphaneUygulaması/database/schema_seed_200.sql` | EN + TR |
| [Kütüphane Projesi](./KütüphaneProjesi/README.tr.md) | Kitap ve ödünç yönetimi | C# WinForms, .NET 8, MySql.Data | `KütüphaneProjesi/database/schema_seed_200.sql` | EN + TR |

## Depo Standartları

- Arayüz ve dokümanlarda Türkçe karakter kullanımı korunur.
- Dokümantasyonda yalnızca göreli yollar kullanılır; makineye özel tam yol kullanılmaz.
- Her projede doğrudan çalıştırılabilir MySQL şema + seed scripti bulunur.
- Seed standardı: ana tabloların her birinde **200 kayıt** hazırlanır.

## Gereksinimler

- Windows 10/11
- Visual Studio 2022 (.NET masaüstü iş yükü)
- .NET 8 SDK
- MySQL 8+

## Hızlı Başlangıç

1. Depoyu klonlayın.
2. İlgili proje `.sln` dosyasını Visual Studio ile açın.
3. Gerekirse proje içindeki `App.config` bağlantı bilgisini güncelleyin.
4. `database` klasöründeki SQL scriptini çalıştırın.
5. Build alıp çalıştırın.

## Demo Hesaplar

| Proje | Yönetici | Kullanıcı |
|---|---|---|
| Mini E-Ticaret | `Sistem Yöneticisi` / `Admin123!` | `Demo Kullanıcı` / `Kullanici123!` |
| Müzik Kütüphane | `yonetici@muzik.local` / `Yonetici123!` | `kullanici@muzik.local` / `Kullanici123!` |
| Kütüphane Projesi | `admin` / `Admin123!` | `kullanici` / `Kullanici123!` |

## Lisans

Bu depo [MIT Lisansı](./LICENSE) ile lisanslanmıştır.
Türkçe bilgilendirme: [LICENSE_TR.md](./LICENSE_TR.md)

