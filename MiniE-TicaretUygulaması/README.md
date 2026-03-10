# Mini E-Ticaret Uygulaması

Mini E-Ticaret Uygulaması is a role-based desktop shopping system built with C# WinForms, .NET 8 and MySQL.

## School Requirement

This project is part of a high school assignment and the use of **C# + .NET + MySQL** is mandatory.

## Core Features

- Secure login with hashed passwords
- Role-based panel routing (`admin` / `kullanici`)
- Product listing, category filtering, search and sorting
- Cart operations: add, remove, quantity update
- Order creation and order history tracking
- Admin modules for product, category, user and order management

## Project Structure

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

## Database Setup

1. Start MySQL.
2. Run `database/minieticaret_schema.sql`.
3. Ensure the `MySqlConnection` string in `MiniE-TicaretUygulaması/App.config` is correct.

Seed policy: each main table is pre-filled with **200 rows**.

## Run

1. Open `MiniE-TicaretUygulaması.sln`.
2. Build and run in Visual Studio.

## Verified Demo Accounts

- Admin (Username): `Sistem Yöneticisi` / `Admin123!`
- User (Username): `Demo Kullanıcı` / `Kullanici123!`

## Notes

- UI texts use Turkish characters consistently.
- This repository avoids machine-specific absolute file paths.
