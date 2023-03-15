# Study Tool (name WIP)

## Project Components

|          |               |
|----------| -------------:|
| Backend Language |  C# |
| Web Framework  |    ASP.NET   |
| Web Page Generation | Razor Pages (C#, HTML, CSS(bootstrap)) |
| Logging  |    SeriLog   |
| Database  |    SQLite   |

As it is common in ASP.NET projects;  
dependency injection is used to register & access services.

Directory structure & short describtion about the project files/folders:

```md
...
|-- BusinessLogic/ (application specific logic)
|   |-- HashManager.cs
|   `-- PresentationConversion.cs
|-- Data/ (access to data)
|   |-- AppValues.cs
|   |-- DataAccessor.cs
|   `-- DatabaseContext.cs    
|-- Database/ (the SQLite database files)
|-- Logs/ (logging files created at runtime)
|-- Models/ (object structures)
|   |-- Project.cs
|   |-- User.cs
|   `-- UserCredentials.cs
|-- Pages/ (the displayable webpages)
|   |-- Account/
|   |   |-- Login.cshtml
|   |   |   `-- Login.cshtml.cs
|   |   |-- Profile.cshtml
|   |   |   `-- Profile.cshtml.cs
|   |   `-- Register.cshtml
|   |       `-- Register.cshtml.cs
|   |-- Common/
|   |   |-- Error.cshtml
|   |   |   `-- Error.cshtml.cs
|   |   `-- Privacy.cshtml
|   |       `-- Privacy.cshtml.cs
|   |-- Partial/
|   |   |-- _Api.cshtml
|   |   |   `-- _Api.cshtml.cs
|   |   |-- _Notes.cshtml
|   |   |   `-- _Notes.cshtml.cs
|   |   `-- _Slides.cshtml
|   |       `-- _Slides.cshtml.cs
|   |-- Shared/
|   |   `-- _Layout.cshtml
|   |-- Test/
|   |   `-- DbTest.cshtml
|   |       `-- DbTest.cshtml.cs
|   |-- Workspace/
|   |   `-- WorkPage.cshtml
|   |       `-- WorkPage.cshtml.cs
|   |-- _ViewImports.cshtml
|   |-- _ViewStart.cshtml
|   |-- Index.cshtml
|   |   `-- Index.cshtml.cs
|   `-- MainPage.cshtml
|       `-- MainPage.cshtml.cs
|-- Uploads/ (User uploaded files, may want to remove this) 
|-- Appsettings.json (app settings)
|-- ReadMe.MD (general information about the project)
`-- Startup.cs
```
