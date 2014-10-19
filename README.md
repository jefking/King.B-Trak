BETA Release
===========

Synchronize Between SQL Server and Azure Table Storage;
+ Move data from SQL Server to Table Storage
+ Move data from Table Storage to SQL Server

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.BTrak)
```
PM> Install-Package King.BTrak
```

### [Wiki](https://github.com/jefking/King.B-Trak/wiki)
View the wiki to learn how to use this.

## Getting Started
* Clone the repository
* Run the B-Trak.csproj file in Visual Studio
* Configure the command line arguments in DEBUG tab of Project Properties
  * Example SQL Server to Table Storage: <code>"Server=localhost;Database=KingBTrak;Trusted_Connection=True;" "UseDevelopmentStorage=true;"</code>
  * Example Table Storage to SQL Server: <code>"UseDevelopmentStorage=true;" "Server=localhost;Database=KingBTrak;Trusted_Connection=True;"</code>
* Run that puppy (Hit F5)!