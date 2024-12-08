# Blue Archive Wiki Web Scrapper

## Español

Programa de Consola hecho con C# en .net 8, lo creè para extraer datos e Imágenes de los personajes de Blue Archive (juego gacha de smarphone). Dichos datos y Archivos fueron extraìdos de [Blue Archive Wiki](https://bluearchive.wiki/wiki/Characters).
Los Archivos y la base de datos se guardan en la carpeta llamada "BlueArchiveWS" ubicada dentro de la carpeta "Documents".

## English

Console application created in C# with dotnet 8, I made it to extract images and character data from Blue Archive (gacha game for smartphones). The data mentioned above was extracted from: [Blue Archive Wiki](https://bluearchive.wiki/wiki/Characters). The Files and database are saved in the folder called "BlueArchiveWS" located inside the "Documents" folder.

## Requeriments

.net 9

## Comands to Setup

1. Install Entity Framework tool: `dotnet tool install --global dotnet-ef`
2. Install Entity Framework.Sqlite: `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
3. Install Entity Framework.Desing: `dotnet add package Microsoft.EntityFrameworkCore.Design`
4. Generate Database Migration: `dotnet ef Migrations add InitialCreate`
5. Update the database with migration: `dotnet ef database update`
6. run the program: `dotnet run`
