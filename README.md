# Blue Archive Wiki Scraper

A C# console application that scrapes and structures character and game data from the Blue Archive Wiki.

## Features

- Crawls the Blue Archive Wiki to extract character data
- Parses HTML content using HtmlAgilityPack with XPath expressions
- Persists scraped data to a local SQLite database via Entity Framework
- Uses dependency injection for service management
- Outputs structured data from unstructured wiki markup

## Tech Stack

- **C#** - Programming language
- **.NET** - Runtime platform
- **Entity Framework Core** - ORM for database access
- **SQLite** - Local database for data persistence
- **HtmlAgilityPack** - HTML parsing library
- **XPath** - Query language for navigating HTML documents
- **Dependency Injection** - Built-in .NET DI container

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (or later)

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
dotnet run
```

On first run, Entity Framework will create the SQLite database and apply any pending migrations automatically.

## Project Architecture

```
Blue-Archive-Wiki-Web-Scrapper-DotNet/
├── Program.cs                  # Entry point and DI configuration
├── Services/
│   ├── ScraperService.cs       # HTTP requests and HTML fetching
│   ├── ParserService.cs        # HtmlAgilityPack + XPath extraction
│   └── DataService.cs          # Entity Framework database operations
├── Models/
│   └── Character.cs            # Data model for scraped characters
├── Data/
│   └── AppDbContext.cs         # EF Core DbContext with SQLite
└── appsettings.json            # Configuration
```

### How It Works

1. **ScraperService** uses `HttpClient` to fetch raw HTML from the Blue Archive Wiki
2. **ParserService** loads the HTML into HtmlAgilityPack and extracts structured data using XPath queries
3. **DataService** persists the extracted data to a SQLite database via Entity Framework Core
4. All services are registered through .NET's built-in dependency injection container

## Output

The scraper produces structured character data including names, roles, stats, and other metadata extracted from the wiki pages. Data is stored in a local `bluearchive.db` SQLite file.

## License

This project is for educational purposes only. I do not own any of the materials, including images, data, trademarks, or other related content used in this project. All rights belong to their respective owners.
