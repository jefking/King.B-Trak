APLHA Release
===========

Synchronize Between SQL Server and Azure Table Storage;
+ Move data from SQL Server to Table Storage
+ Move data from Table Storage to SQL Server

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.BTrak)
```
PM> Install-Package King.BTrak
```
Currently:
Move Data from SQL Server to Table Storage
PM> Install-Package King.BTrak -Version 0.1.1
Move Data from Table Storage to SQL Server
PM> Install-Package King.BTrak-Version 0.2.0
These will be amalgamated for the BETA release soon!

## Getting Started
* Clone the repository
* Run the B-Trak.csproj file in Visual Studio
* Configure the command line arguments in DEBUG tab of Project Properties
  * Example SQL Server to Table Storage: <code>"Server=localhost;Database=KingBTrak;Trusted_Connection=True;" "UseDevelopmentStorage=true;"</code>
* Run that puppy (Hit F5)!