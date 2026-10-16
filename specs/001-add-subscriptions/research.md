# Research: Add RSS/Atom Subscriptions

**Date**: 2026-10-15
**Scope**: MVP add-and-list behavior only; no feed fetching, parsing, or persistence.

## Decision: Keep subscription state in one process-lifetime API service

**Rationale**: The API is the approved owner of subscription operations, and the application is a single-user local proof of concept. A single in-memory store lets separate add and list requests observe the same values and naturally loses them when the API process restarts. Protect shared collection updates and reads against concurrent requests. Do not use cookie-backed ASP.NET Core Session or browser storage; neither is needed for this process-lifetime MVP behavior.

**Alternatives considered**:

- Browser-only state: rejected because the approved architecture assigns subscription operations and storage to the API.
- Database or browser persistence: rejected because persistence is explicitly deferred.
- Cookie-backed ASP.NET Core Session: rejected as unnecessary for this locally run WebAssembly client/API design.

## Decision: Use a small JSON API contract for add and list

**Rationale**: The UI and API are separate applications. A JSON request containing the entered value and a JSON list response keep that boundary explicit. Typed responses make status codes and response shapes clear and testable. Reject blank or whitespace-only values as required by the feature spec, but do not validate URL syntax or availability. Preserve insertion order and duplicates.

**Alternatives considered**:

- Direct shared UI/server state: rejected because it would bypass the required API boundary.
- Feed verification during add: rejected because it would make network calls and violate MVP scope.

## Decision: Use a dependency-injected API client in Blazor

**Rationale**: Microsoft guidance for Blazor WebAssembly supports using an injected `HttpClient` for API calls. Keep API communication out of the page's presentation logic and externalize the API base URL as required by the constitution and stakeholder guidance. Configure backend CORS for the actual frontend origin.

**Alternatives considered**:

- Hardcoded endpoint in page code: rejected because endpoint configuration must remain externalized.
- Manually constructing `HttpClient` in a component: rejected in favor of the normal dependency-injection pattern.

## Decision: Use focused unit and integration coverage

**Rationale**: Test collection behavior and blank-input rejection with fast unit tests. Use a small integration test to verify the real HTTP add-then-list behavior, and browser validation to confirm the form/list UX and runtime configuration. Keep test projects separated by purpose.

**Alternatives considered**:

- Integration tests for every input permutation: rejected as unnecessary overhead for this small feature.
- Browser-only verification: rejected because API/store edge cases should be independently testable.

## Technology baseline

Use .NET 10.0 as the implementation target for this new project. The inspected environment has .NET SDK 8.0.425 and 9.0.318 but not SDK 10; installing the target SDK is a prerequisite before project creation/build validation. The feature does not require any RSS/Atom parser or HTTP client that contacts feed URLs.

## References

- [ASP.NET Core app state overview](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-10.0) — distinguishes ASP.NET Core session/state approaches.
- [Blazor state management overview](https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management/?view=aspnetcore-10.0) — in-memory state container patterns and non-persistent state.
- [Call a web API from an ASP.NET Core Blazor app](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-10.0) — API calls from Blazor through dependency injection.
- [Create responses in Minimal API applications](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-10.0) — JSON and typed response patterns.
- [Test Minimal API apps](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/test-min-api?view=aspnetcore-10.0) — unit and integration test guidance.
