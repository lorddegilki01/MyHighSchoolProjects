# My High School Projects (C#, .NET, MySQL)

A GitHub-ready portfolio of desktop applications built with C# WinForms, .NET 8 and MySQL.

Maintainer: **Efe** (`whoisefe`)

## Included Projects

| Project | Focus | Tech Stack | Database Script | Documentation |
|---|---|---|---|---|
| [Mini E-Ticaret Uygulaması](./MiniE-TicaretUygulaması/README.md) | Product listing, cart and order flow | C# WinForms, .NET 8, MySqlConnector | `MiniE-TicaretUygulaması/database/minieticaret_schema.sql` | EN + TR |
| [Müzik Kütüphane Uygulaması](./MüzikKütüphaneUygulaması/README.md) | Songs, playlists and role-based management | C# WinForms, .NET 8, MySqlConnector | `MüzikKütüphaneUygulaması/database/schema_seed_200.sql` | EN + TR |
| [Kütüphane Projesi](./KütüphaneProjesi/README.md) | Book and loan management | C# WinForms, .NET 8, MySql.Data | `KütüphaneProjesi/database/schema_seed_200.sql` | EN + TR |

## Repository Standards

- Turkish characters are used consistently in UI and documentation.
- Documentation uses only relative paths (no machine-specific absolute paths).
- Every project contains a ready-to-run MySQL schema + seed script.
- Seed policy: each main table is prepared with **200 rows**.

## Requirements

- Windows 10/11
- Visual Studio 2022 (with .NET desktop workload)
- .NET 8 SDK
- MySQL 8+

## Quick Start

1. Clone the repository.
2. Open one of the `.sln` files in Visual Studio.
3. Update the connection string in the project `App.config` if needed.
4. Run the project SQL script from the `database` folder.
5. Build and run.

## Demo Accounts

| Project | Admin / Manager | User |
|---|---|---|
| Mini E-Ticaret | `Sistem Yöneticisi` / `Admin123!` | `Demo Kullanıcı` / `Kullanici123!` |
| Müzik Kütüphane | `yonetici@muzik.local` / `Yonetici123!` | `kullanici@muzik.local` / `Kullanici123!` |
| Kütüphane Projesi | `admin` / `Admin123!` | `kullanici` / `Kullanici123!` |

## License

Licensed under the [MIT License](./LICENSE).
Turkish summary: [LICENSE_TR.md](./LICENSE_TR.md)

