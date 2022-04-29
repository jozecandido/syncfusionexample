Hi. Here is the sample with the same basic structure we're dealing with.

#Requirements
 - Dotnet 6
 - SQL Server

 #Setup

 after cloning the repository, please do these steps

  - Create an database (read_db)
  - Update the connection string in the file src\Services.Query\appsettings.json
  - Run the dotnet run command within these folders:
    - src\Web\Server
    - src\Services.Query
  - Access the url localhost:5100
