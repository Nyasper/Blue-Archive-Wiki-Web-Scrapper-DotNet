# Blue Archive Wiki Web Scrapper

## English
Console application created in C# with dotnet 9, 
I made it for extract images and character data from the page: [Blue Archive Wiki](https://bluearchive.wiki/wiki/Characters), 
which is a free wiki of the gacha video game 'Blue Archive'.  
The Files and database are saved in the folder called "BlueArchiveWS" located in the "Documents" folder.  
The files and data extracted with this program are used in this other front end [project](https://github.com/Nyasper/Proyecto-2-Blue-Archive-Database-react.git).
  
## Español
Programa de Consola hecho con C# en .net 9, 
lo hice para extraer datos e Imágenes de la página: [Blue Archive Wiki](https://bluearchive.wiki/wiki/Characters), 
la cuál es una wiki gratis del videojuego gacha Blue Archive. Los archivos y la base de datos se guardan en el directorio "BlueArchiveWS" dentro de "Documentos".
Los Archivos y la base de datos se guardan en la carpeta llamada "BlueArchiveWS" ubicada dentro de la carpeta "Documents".  
Los datos y archivos extraídos con este programa son utilizados in este otro [proyecto](https://github.com/Nyasper/Proyecto-2-Blue-Archive-Database-react.git) front end.

## How to run this program

1. [Install .NET 10](https://dotnet.microsoft.com/es-es/download) (if you don't have it)
2. Download this project, then in the project folder run the following commands:
3. Install Entity Framework tool: `dotnet tool install --global dotnet-ef`
4. Install Entity Framework.Sqlite: `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
5. Install Entity Framework.Desing: `dotnet add package Microsoft.EntityFrameworkCore.Design`
6. Generate Database Migration: `dotnet ef Migrations add InitialCreate`
7. Update the database with migration: `dotnet ef database update`
8.  build with: `dotnet build`
9. run with: `dotnet run`
