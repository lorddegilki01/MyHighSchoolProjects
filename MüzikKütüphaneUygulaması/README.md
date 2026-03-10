# Müzik Kütüphane Uygulaması

A desktop music library application with role-based panels, built with C# WinForms, .NET 8 and MySQL.

## Core Features

- Secure login with hashed passwords
- Role-based experience:
  - `yonetici`: song, artist, genre and user management
  - `kullanici`: search, list and playlist flow
- Song search/filter by name, artist and genre
- Playlist creation, save and remove operations
- Professional teal theme and Turkish UI texts

## Project Structure

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
`-- README.tr.md
```

## Database Setup

1. Start MySQL.
2. Run `database/schema_seed_200.sql`.
3. Check `MySqlConnection` in `MüzikKütüphaneUygulaması/App.config`.

Seed policy: each main table contains **200 rows**.

## Run

1. Open `MüzikKütüphaneUygulaması.sln`.
2. Build and run in Visual Studio.

## Demo Accounts

- Manager: `yonetici@muzik.local` / `Yonetici123!`
- User: `kullanici@muzik.local` / `Kullanici123!`

## Notes

- Turkish characters are preserved in UI and documentation.
- Documentation is prepared for GitHub with relative paths.
