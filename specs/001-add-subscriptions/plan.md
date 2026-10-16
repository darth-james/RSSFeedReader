# Implementation Plan: Add RSS/Atom Subscriptions

**Branch**: `main` | **Date**: 2026-10-15 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/001-add-subscriptions/spec.md`; stakeholder context from `StakeholderDocuments/ProjectGoals.md` and `StakeholderDocuments/TechStack.md`.

## Summary

Deliver the MVP's single-user, locally run subscription workflow: accept each non-empty user-entered value, keep it in memory for the backend process lifetime, and display the complete list in the Blazor UI. Use the constitution-mandated ASP.NET Core Web API and Blazor WebAssembly separation. The UI communicates with the API; the API makes no requests to subscription URLs. Repeated values remain separate entries. Feed validation, fetching, parsing, persistence, removal, and refresh are excluded.

## Technical Context

**Language/Version**: C# on .NET 10.0; install the .NET 10 SDK before implementation. The current environment has .NET 8.0.425 and 9.0.318, but no application projects.

**Primary Dependencies**: ASP.NET Core Web API; Blazor WebAssembly; .NET built-in dependency injection and JSON support. Add no feed-fetching or parsing dependency for this MVP.

**Storage**: A process-lifetime in-memory subscription store in the API. No database, browser storage, or cross-restart persistence.

**Testing**: xUnit for focused API/store unit tests and a separate ASP.NET Core integration-test project using `WebApplicationFactory`; validate the user flow in the browser using the quickstart.

**Target Platform**: Local development and use on Windows, macOS, and Linux; browser UI served by Blazor WebAssembly and API served locally.

**Project Type**: Web application with separate backend and frontend projects.

**Performance Goals**: Every successful add is visible in the list within 2 seconds without a page reload; support at least 10 distinct subscriptions in a session.

**Constraints**: Single local user; accept non-empty values without URL/feed validation; reject empty or whitespace-only input; append duplicates as separate entries; safely display user values as text; do not contact subscription URLs. Coordinate API URL, listening port, and CORS origins.

**Scale/Scope**: One local application instance and one subscription list; one add-and-list UI flow and two API operations. No multi-user isolation or production-scale availability requirement.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle / gate | Plan alignment | Status |
|---|---|---|
| I. MVP Scope and Incremental Delivery | Only add and list session-only subscriptions; no fetching, parsing, persistence, removal, or background polling. | PASS |
| II. Security and Safe Data Handling | Treat entered values as untrusted text; API never requests those URLs. | PASS |
| III. Maintainable Architecture | Keep ASP.NET Core API, application state, and Blazor presentation separated; externalize the API base URL. | PASS |
| IV. Verified Quality | Add store/API tests and a real HTTP add/list integration test; verify the UI flow and affected builds. | PASS |
| V. Focused User Experience | Provide a direct URL entry form and list; communicate blank-input errors without implying feed validation or fetching. | PASS |
| Project constraints | Single-user local proof of concept on Windows, macOS, and Linux using the approved stack. | PASS |

No constitution deviations or complexity exceptions are required.

## Project Structure

### Documentation (this feature)

```text
specs/001-add-subscriptions/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── subscriptions-api.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
├── RSSFeedReader.Api/
│   ├── Program.cs
│   └── Properties/
│       └── launchSettings.json
├── RSSFeedReader.Api.Tests/
└── RSSFeedReader.Api.IntegrationTests/

frontend/
├── RSSFeedReader.UI/
│   ├── Pages/
│   ├── Layout/
│   └── wwwroot/
│       └── appsettings.json
```

The backend tests are split between fast unit tests and a small HTTP pipeline integration suite. The end-to-end browser acceptance flow validates the frontend interaction and is documented in `quickstart.md`. Create only the projects needed to deliver this feature. The landing page must be the sole page mapped to `/`; remove template demo pages and navigation entries that would conflict with the MVP.

**Structure Decision**: Use the separate `backend/` and `frontend/` layout and project naming described by `StakeholderDocuments/TechStack.md`. The repository currently has no solution or application source tree, so these paths describe the planned initial scaffold.

## Phase 0: Outline & Research

See [research.md](./research.md) for decisions and references. Technical choices are specified by the project constitution and stakeholder documents; research confirms the minimal in-memory store, API contract, Blazor API-client pattern, and focused test strategy. The implementation target is .NET 10.0; the required SDK is not currently installed in the environment.

## Phase 1: Design & Contracts

- [data-model.md](./data-model.md) defines the subscription value, ordering, duplicate behavior, and validation boundaries.
- [contracts/subscriptions-api.md](./contracts/subscriptions-api.md) specifies the add and list HTTP operations and error response.
- [quickstart.md](./quickstart.md) gives setup, run, and end-to-end validation steps.

The design keeps state in the API process and has the Blazor UI retrieve and update it through the API. API configuration, backend listening address, and frontend CORS origin must agree. The design does not introduce a second independent UI-only source of truth or any persistent storage.

### Post-Design Constitution Check

| Principle / gate | Design evidence | Status |
|---|---|---|
| I. MVP Scope and Incremental Delivery | API operations are add and list only; data is process-lifetime memory. | PASS |
| II. Security and Safe Data Handling | No feed URL is fetched; render values as text; reject blank input without URL-format validation. | PASS |
| III. Maintainable Architecture | API store/operations, API client, and UI remain separated; base URL is configuration. | PASS |
| IV. Verified Quality | Store/API unit tests, API integration coverage, browser validation, and project builds are planned. | PASS |
| V. Focused User Experience | One subscription entry flow and a visible list; blank input receives a clear message. | PASS |

No violations remain after design.
