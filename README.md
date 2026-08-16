# Blue Archive Wiki Scraper

A C# console application that scrapes and structures character and game data from the Blue Archive Wiki.

## Features

- Crawls the Blue Archive Wiki to extract character data.
- Parses HTML content using HtmlAgilityPack with XPath expressions.
- Persists scraped data to a local SQLite database via Dapper.
- Auto-initializes the database schema on application startup.
- Uses dependency injection for service management.
- Outputs structured data from unstructured wiki markup.

## Tech Stack

- **C#** - Programming language (.NET 10)
- **Dapper** - Lightweight Object Mapper for database access
- **SQLite** - Local database for data persistence via `Microsoft.Data.Sqlite`
- **HtmlAgilityPack** - HTML parsing library
- **XPath** - Query language for navigating HTML documents
- **Dependency Injection** - Built-in .NET DI container

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or later)


## Getting Started

```bash
# Clone the repository
git clone https://github.com/Nyasper/Blue-Archive-Wiki-Web-Scrapper-DotNet.git
cd Blue-Archive-Wiki-Web-Scrapper-DotNet

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project Main

# Run the application answering "yes" to all prompts automatically
dotnet run --project Main -- -y
dotnet run --project Main -- --yes
```

## Project Architecture

```
BlueArchiveWebScrapper/
├── Main/                       # Main application project
│   ├── Program.cs              # Entry point and DI configuration
│   ├── FileHandler/            # Utilities to download, generate JSON files, and verify local data
│   │   ├── Downloader/
│   │   ├── FileGenerator/
│   │   ├── Updater/
│   │   └── Verifier/
│   ├── Repository/             # Dapper database context and Repository implementations
│   │   ├── IRepository.cs
│   │   ├── Repository.cs
│   │   └── StudentContext.cs   # SQLite connection factory & schema initializer
│   └── Utils/
├── Scanner/                    # Parsing and Scraping logic project
│   ├── CharaDetails/           # Scanner and getters for individual student info
│   ├── CharaList/              # Scanner for listing pages
│   ├── Configuration/          # HTML agility configurations
│   └── Model/                  # Record models (Student, StudentDetailsItem, etc.)
└── Testing/                    # Unit and integration test suite
```

### How It Works

1. **Scanner** fetches raw HTML from the wiki and parses it into records (like `Student`) using HtmlAgilityPack and XPath queries.
2. **Repository** persists the data into a SQLite database using **Dapper** with parameterized queries and transaction control.
3. **Updater** coordinates comparing wiki data, local JSON data, and the database, offering options to sync and download media files (portraits, audio, etc.).
4. All services are registered through the standard .NET dependency injection container.

## Output Directory (BlueArchiveWS)

On execution, the application automatically initializes a data directory in your local `Documents` folder called `BlueArchiveWS`. This folder holds all the scraped database entries, backups, preview pages, and downloaded assets.

### Folder Structure & Contents

```text
MyDocuments/BlueArchiveWS/
├── data/
│   ├── BlueArchive.db         # The SQLite database containing all the scraped student records.
│   ├── data.json              # A JSON backup array containing all student records.
│   └── imagesPreview.html     # An interactive HTML dashboard to browse portraits and play voice clips locally.
└── media/
    └── [school_name]/         # Subfolders organized dynamically by each student's school (e.g., abydos, trinity).
        ├── [CharaName].png          # Character profile avatar image.
        ├── [CharaName]_full.png     # Full-body character portrait artwork.
        ├── [CharaName]_small.png    # Small character icon image.
        └── [CharaName].ogg          # Voice clip audio file.
```

- **SQLite Database (`BlueArchive.db`)**: Self-contained database storing characters' profiles, stats, and metadata (automatically initialized and updated via Dapper).
- **HTML Preview (`imagesPreview.html`)**: A local web interface built automatically that groups students by school, plays their audio clips, and lets you click to zoom in on their full-body portraits.
- **Media Files**: Organized cleanly in directories matching their school name, ensuring fast lookups and a structured asset pipeline.

## License

This project is for educational purposes only. I do not own any of the materials, including images, data, trademarks, or other related content used in this project. All rights belong to their respective owners.
