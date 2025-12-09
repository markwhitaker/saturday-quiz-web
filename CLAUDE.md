# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

This is an ASP.NET Core web application targeting .NET 10.0 with JavaScript testing components. Common development commands:

### Building
- `dotnet build` - Build the solution
- `dotnet build -c Release` - Build in release configuration

### Testing
- `dotnet test SaturdayQuizWeb.UnitTests` - Run C# unit tests (NUnit)
- `dotnet test SaturdayQuizWeb.IntegrationTests` - Run C# integration tests
- `dotnet test SaturdayQuizWeb.IntegrationTests --filter Category!=LongRunning` - Run integration tests excluding long-running tests
- `dotnet test --filter "FullyQualifiedName~MethodName"` - Run specific test by name
- `bun test` (from SaturdayQuizWeb.JavaScriptTests/) - Run JavaScript tests using Bun

### Running the Application
- `dotnet run --project SaturdayQuizWeb` - Start the web application
- The app serves both a web interface and REST API with Swagger documentation

## Architecture Overview

This is a Saturday Quiz web application that fetches quiz data from The Guardian API and serves it via REST endpoints.

### Project Structure
- **SaturdayQuizWeb/** - Main web application project
- **SaturdayQuizWeb.UnitTests/** - C# unit tests using NUnit
- **SaturdayQuizWeb.IntegrationTests/** - Integration tests
- **SaturdayQuizWeb.JavaScriptTests/** - JavaScript tests using Bun

### Key Components

#### Main Web App (SaturdayQuizWeb/)
- **Program.cs** - Application entry point with minimal API endpoints
- **Clients/** - HTTP clients for external API integration (Guardian API)
  - Uses typed HTTP clients pattern (see Clients/HttpClients/README.md)
- **Services/** - Business logic and parsing services
- **Models/** - Domain models
- **Dtos/** - Data transfer objects for API responses
- **Extensions/** - Extension methods
- **Config/** - Configuration classes
- **Utils/** - Utility classes
- **Wrappers/** - Wrapper classes
- **wwwroot/** - Static web assets including JavaScript

#### Technology Stack
- **.NET 10.0** with nullable reference types enabled
- **ASP.NET Core** minimal APIs
- **Swagger/OpenAPI** for API documentation
- **Global usings** defined in GlobalUsings.cs
- **User secrets** for local development configuration
- **NUnit** and **NSubstitute** for C# testing
- **Bun** for JavaScript testing

#### External Dependencies
- **Guardian API** - Source for quiz data (requires API key via Guardian__ApiKey)
- **JetBrains.Annotations** - Code annotations
- **RegexToolbox** - Regular expression utilities
- **Mainwave.MimeTypes** - MIME type handling

### Development Notes

The application uses a clean architecture with separation between HTTP clients, services, and presentation layers. The Guardian API integration requires proper configuration of the API key through user secrets or environment variables.

JavaScript components use ES6 modules as specified in the package.json configurations.