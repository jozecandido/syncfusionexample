Hi. Here is the sample with the same basic structure we're dealing with.

#Requirements
 - Dotnet 6
 - SQL Server

 #Setup

 after cloning the repository, please do these steps

  1- Create an database (read_db)
  2- Update the connection string in the file src\Services.Query\appsettings.json
  3- Run the dotnet run command within these folders:
   - src\Web\Server
   - src\Services.Query
  4- Access the url localhost:5100