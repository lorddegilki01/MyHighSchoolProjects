# Kütüphane Projesi

A role-based desktop library management application built with C# WinForms, .NET 8 and MySQL.

## School Requirement

This project is part of a high school assignment and the use of **C# + .NET + MySQL** is mandatory.

## Core Features

- Login with user roles (`Yonetici` / `Kullanici`)
- Book search and filtering by name, author and genre
- Book CRUD operations (admin)
- Author and genre management (admin)
- User listing, role update and user cleanup (admin)
- Loan operations with stock-safe flow:
  - create loan
  - update selected loan
  - delete selected loan
- Turkish UI with purple theme

## Project Structure

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

## Database Setup

1. Start MySQL.
2. Run `database/schema_seed_200.sql`.
3. Verify `MySqlConnection` in `KütüphaneProjesi/App.config`.

Seed policy: each main table contains **200 rows**.

## Run

1. Open `KütüphaneProjesi.sln`.
2. Build and run with Visual Studio.

## Verified Demo Accounts

- Admin (Username): `admin` / `Admin123!`
- User (Username): `kullanici` / `Kullanici123!`

## Notes

- Turkish characters are used in UI texts and docs.
- Repository docs use relative paths for GitHub portability.
