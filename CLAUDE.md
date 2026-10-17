# CLAUDE.md

Guidance for AI agents working in this repository (a fork of NimblePros/eShopOnWeb used to pilot an agent pipeline). Humans: this is also a fair summary for you.

## What this is

eShopOnWeb is a sample ASP.NET Core 10 monolith demonstrating a domain-centric (clean) architecture. Solution: `eShopOnWeb.slnx`. SDK pinned by `global.json` (10.0, `rollForward: latestFeature`).

| Project | Role |
|---|---|
| `src/ApplicationCore` | Domain: entities, aggregates, domain services, interfaces, specifications. No dependency on Infrastructure or Web. |
| `src/Infrastructure` | EF Core data access, identity, implementations of ApplicationCore interfaces. |
| `src/Web` | ASP.NET Core MVC + Razor Pages storefront. |
| `src/PublicApi` | HTTP API endpoints (used by BlazorAdmin). |
| `src/BlazorAdmin`, `src/BlazorShared` | Blazor WebAssembly admin UI and shared models. |
| `src/eShopWeb.AppHost`, `src/eShopWeb.AspireServiceDefaults` | .NET Aspire host for local orchestration; not needed for building or testing. |

## Build and test

```bash
dotnet build eShopOnWeb.slnx
dotnet test tests/UnitTests/UnitTests.csproj
dotnet test tests/IntegrationTests/IntegrationTests.csproj
dotnet test tests/FunctionalTests/FunctionalTests.csproj
dotnet test tests/PublicApiIntegrationTests/PublicApiIntegrationTests.csproj
```

All four run headless with the EF Core in-memory provider and `WebApplicationFactory`; no SQL Server or Docker is needed. The whole set takes about two minutes on one CPU.

`tests/EndToEndTests` (Playwright) needs a running app and browsers. It is **not** part of the standard test run; only run it when a task explicitly asks for end-to-end verification.

## Conventions

- **Dependencies point inward.** ApplicationCore must not reference Infrastructure, Web or PublicApi. New domain logic goes in ApplicationCore; persistence details go in Infrastructure.
- **Queries use specifications** (`Ardalis.Specification`) in `src/ApplicationCore/Specifications`, not ad-hoc LINQ in controllers or pages.
- **Guard clauses** (`Ardalis.GuardClauses`) validate arguments in domain code; follow the existing style in `src/ApplicationCore/Entities`.
- **Central package management:** versions live only in `Directory.Packages.props`; a `PackageReference` in a `.csproj` has no `Version`. Do not add packages unless the task needs them.
- **Tests:** xUnit v3 with NSubstitute. Put unit tests next to the existing ones for the same area (`tests/UnitTests/ApplicationCore/...`), named `MethodOrBehavior_Condition_Expectation` like the surrounding tests.
- Follow `.editorconfig`. Keep changes scoped to the task; no drive-by refactors or reformatting.

## Do not touch

- `.github/` (workflows, CODEOWNERS), `CLAUDE.md`, `.claude/`, `.mcp.json`: changes here are rejected automatically.
- `azure.yaml`, `infra/` and deployment files, unless the task is about deployment.
- The README table of contents block (a workflow regenerates it).
