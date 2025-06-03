# SA ID Importer

SAIdImporter is a Windows Forms (.NET) application designed to import South African ID records from an Excel file, validate them, extract key personal information such as Date of Birth and Gender, and store valid entries in a SQL Server database. The app includes filtering, sorting, and database viewing features.

---

## 🧩 Features

- ✅ Import RSA ID data from Excel (.xlsx) files
- ✅ Automatically calculate:
  - *Date of Birth*
  - *Gender*
  - *ID Validity* (using checksum logic)
- ✅ Skip:
  - Duplicate ID Numbers
- ✅ Store clean data to SQL Server database
- ✅ Filter by name, surname, and gender
- ✅ Sort by any column in the table
- ✅ View previously imported data from the database

---

## 🛠 Technologies Used

- [.NET WinForms (C#)](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/)
- [Microsoft SQL Server (LocalDB or full)](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [EPPlus](https://github.com/EPPlusSoftware/EPPlus) (for Excel file reading)

---

## 🧪 RSA ID Validation Logic

- The ID number must be *13 digits long* (padded if necessary).
- The first 6 digits represent the *date of birth* (YYMMDD).
  - Assumes age is between 16 and 84 years (to determine the correct century).
- Digits 7–10 determine *gender*:
  - Numbers >= 5000 = Male
  - Numbers < 5000 = Female
- The last digit is a *checksum* (Luhn algorithm or similar).
- An ID is only stored if it passes all checks.

---

## 🖥 UI Overview

| Control             | Description                                         |
|---------------------|-----------------------------------------------------|
| Import Excel      | Opens Excel file and imports records                |
| View from Database| Loads and displays all records from the SQL DB      |
| DataGridView      | Displays all imported and stored records            |
| TextBox           | Filter by first name or surname                     |
| ComboBox          | Filter by gender (All, Male, Female)          |

---

## 📦 Requirements

- Windows 10 or 11
- .NET SDK or Runtime (e.g., .NET 6/7)
- Microsoft SQL Server (or LocalDB)
- Visual Studio (Community Edition is fine)
- EPPlus NuGet package (>=5.8)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/mikeywestie/SAIdImporter.git
cd SAIdImporter
