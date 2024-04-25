# Blue Archive Wiki Web Scrapper

## Español

Programa de Consola hecho con C# en .net 8, lo creè para extraer datos e Imágenes de los personajes de Blue Archive (juego gacha de smarphone). Dichos datos y Archivos fueron extraìdos de [Blue Archive Wiki](https://bluearchive.wiki/wiki/Characters).

## English

Console application created in C# with dotnet 8, I made it to extract images and character data from Blue Archive (gacha game for smartphones). The data mentioned above was extracted from: [Blue Archive Wiki](https://bluearchive.wiki/wiki/Characters).

## Requeriments

.net 8

## Comands to Setup

1. dotnet tool install --global dotnet-ef
2. dotnet add package Microsoft.EntityFrameworkCore.Sqlite
3. dotnet add package Microsoft.EntityFrameworkCore.Design
4. dotnet ef Migrations add InitialCreate
5. dotnet ef database update
6. dotnet run
