# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Stack and prerequisites

- .NET SDK 10.0.x (target framework is net10.0). A `global.json` is present allowing roll-forward from 8.0 to the latest major.
- Bun (for JavaScript tests) in `SaturdayQuizWeb.JavaScriptTests/`.
- Guardian API key is required for the running app and some integration tests:
  - Environment variable: `Guardian__ApiKey`
  - Or user-secrets in the web project directory: `dotnet user-secrets set "Guardian:ApiKey" "{{your_key}}"`

## Common commands

All commands are intended to be run from the repo root unless noted.

- Build
  - Release: `dotnet build -c Release`
  - Debug: `dotnet build`
- Run the web app
  - `dotnet run --project SaturdayQuizWeb`
  - Swagger UI will be available at `/swagger` on the chosen port.
- Test (C#)
  - Unit tests: `dotnet test SaturdayQuizWeb.UnitTests`
  - Integration tests: `dotnet test SaturdayQuizWeb.IntegrationTests --filter Category!=LongRunning`
  - Run a single test (by name): `dotnet test --filter "FullyQualifiedName~Namespace.ClassName.MethodName"`
  - Run tests in a single class: `dotnet test --filter "FullyQualifiedName~Namespace.ClassName"`
- Test (JavaScript via Bun)
  - From `SaturdayQuizWeb.JavaScriptTests/`: `bun test`
  - Single file: `bun test test/Quiz.test.js`
  - By test name pattern: `bun test -t "pattern"`
- Docker
  - Build: `docker build -t saturday-quiz-web .`
  - Run: `docker run --rm -p 8080:8080 -e Guardian__ApiKey={{your_key}} saturday-quiz-web`

Notes on linting/formatting: no repository-specific linter configuration is present. Analyzer warnings surface on `dotnet build`. If you have `dotnet format` installed, you can run it locally for formatting.

## Architecture overview

High-level layout

- `SaturdayQuizWeb/` — ASP.NET Core minimal API that serves both the REST API and static frontend assets under `wwwroot/`.
- `SaturdayQuizWeb.UnitTests/` — NUnit unit tests.
- `SaturdayQuizWeb.IntegrationTests/` — NUnit integration tests (some tests are marked `LongRunning`).
- `SaturdayQuizWeb.JavaScriptTests/` — Bun-powered tests for the browser-side modules under `SaturdayQuizWeb/wwwroot/script/`.

Key flows and components (server)

- Endpoints are defined in `Program.cs` using minimal APIs:
  - `GET /api/quiz/` — latest quiz.
  - `GET /api/quiz/{date}` — quiz by date (`yyyy-MM-dd`).
  - `GET /api/quiz-metadata?count=N` — recent quiz metadata (default 10).
- Dependency injection is configured in `Program.cs`:
  - Typed HTTP clients in `Clients/HttpClients/` fetch from The Guardian website and API.
  - Services orchestrate fetching and parsing:
    - `QuizMetadataService` gets recent metadata from the Guardian API; if the newest item is older than a week, it augments via the Guardian RSS feed.
    - `QuizService` uses metadata to fetch the quiz HTML from the website client and parses it into domain models.
  - Swagger is enabled via `AddEndpointsApiExplorer` and `Swashbuckle`.
- Parsing pipeline (HTML → domain models) lives in `Services/` and `Services/Parsing/`:
  - `HtmlService` composes `ISectionExtractor`, `IHtmlStripper`, `ISectionSplitter`, and `IQuestionAssembler` to produce `QuestionModel` instances from the quiz HTML.
- Domain and API shapes
  - Domain models under `Models/` (`Quiz`, `QuizMetadata`, `QuestionModel`, etc.).
  - DTOs under `Dtos/` adapt domain models for the API responses.
- HTTP and caching behavior
  - Static assets served from `wwwroot/` with a custom UTF-8 content type provider.
  - `HttpResponseExtensions.AddCustomHeaders` sets `Cache-Control` and security headers; API endpoints specify cache durations (e.g., 0 or 365 days).
- Configuration
  - Strongly-typed `GuardianConfig` bound from config section `Guardian`; env var mapping uses the `Guardian__*` convention (e.g., `Guardian__ApiKey`).

Key flows and components (client assets)

- Browser-side modules live in `SaturdayQuizWeb/wwwroot/script/` and include quiz domain logic (e.g., `Quiz.js`, `Question.js`, repositories, caching, and time helpers). These are exercised by Bun tests in `SaturdayQuizWeb.JavaScriptTests/test/`.

## CI parity

GitHub Actions (`.github/workflows/build-and-test.yml`) runs the following, which you can mirror locally:

- `dotnet build -c Release`
- `dotnet test SaturdayQuizWeb.UnitTests -c Release`
- `dotnet test SaturdayQuizWeb.IntegrationTests -c Release --filter Category!=LongRunning`
- `bun test` (from `SaturdayQuizWeb.JavaScriptTests/`)

## Important links

- Deployed web app: see `README.md` for the current URL.
- Swagger UI of the deployed instance: see `README.md`.
