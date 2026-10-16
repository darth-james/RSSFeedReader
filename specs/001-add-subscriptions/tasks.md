---

description: "Task list for implementing add and view RSS/Atom subscriptions"
---

# Tasks: Add RSS/Atom Subscriptions

**Input**: Design documents from `/specs/001-add-subscriptions/`

**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Include focused API/store unit tests and API HTTP integration tests, as required by the project constitution and design.

**Organization**: Tasks are grouped by user story. The specification defines one P1 story; completing it delivers the full MVP.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: The task can run in parallel with other marked tasks because it changes separate files and has no unfinished prerequisites.
- **[Story]**: The user story this task implements.
- Every task names the repository file path(s) it creates or changes.

## Phase 1: Setup

**Purpose**: Establish the .NET 10 solution and project skeleton. Install the .NET 10 SDK before running these commands; the current environment has only .NET 8 and 9.

- [X] T001 Install the .NET 10 SDK and add `global.json` at the repository root to select the .NET 10 SDK.
- [X] T002 Create `RSSFeedReader.sln` and the ASP.NET Core Web API project at `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.
- [X] T003 Create the standalone Blazor WebAssembly project at `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj` and add it to `RSSFeedReader.sln`.
- [X] T004 Create the xUnit API unit-test project at `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj`, reference the API project, and add it to `RSSFeedReader.sln`.
- [X] T005 Create the ASP.NET Core HTTP integration-test project at `backend/RSSFeedReader.Api.IntegrationTests/RSSFeedReader.Api.IntegrationTests.csproj`, add the `Microsoft.AspNetCore.Mvc.Testing` dependency, reference the API project, and add it to `RSSFeedReader.sln`.

---

## Phase 2: Foundational

**Purpose**: Establish local API hosting and cross-origin configuration required before the UI can use the API.

- [X] T006 Configure the API listening address and development launch profile in `backend/RSSFeedReader.Api/Properties/launchSettings.json`, and configure a development CORS policy for the actual Blazor frontend origin in `backend/RSSFeedReader.Api/Program.cs`.

**Checkpoint**: The API and UI projects build and can be launched locally with a coordinated API URL, frontend origin, and CORS policy.

---

## Phase 3: User Story 1 - Add and view feed subscriptions (Priority: P1) - MVP

**Goal**: Let the local user submit any non-empty subscription value and see all submitted entries, including duplicates, in insertion order for the API process lifetime.

**Independent Test**: Start with an empty list, submit a non-empty value, and verify it appears in the UI without a page reload. Add another value and repeat the first; confirm all three entries appear in order. Submit blank and whitespace-only input and confirm neither is added and the user receives a required-value message. Restart the API and confirm the list is empty. No request may be made to a submitted subscription URL.

### Tests for User Story 1

- [X] T007 [P] [US1] Add unit tests in `backend/RSSFeedReader.Api.Tests/SubscriptionStoreTests.cs` for an initially empty list, insertion order, preservation of repeated values, and rejection of empty or whitespace-only values without changing the list.
- [X] T008 [P] [US1] Add HTTP integration tests in `backend/RSSFeedReader.Api.IntegrationTests/SubscriptionsApiTests.cs` using `WebApplicationFactory` for `GET /api/subscriptions`, successful `POST` followed by `GET`, duplicate entries, and `400 Bad Request` for missing, empty, and whitespace-only values.

### Implementation for User Story 1

- [X] T009 [P] [US1] Create the subscription request and response JSON types in `backend/RSSFeedReader.Api/Models/SubscriptionRequest.cs` and `backend/RSSFeedReader.Api/Models/SubscriptionResponse.cs`, each exposing the submitted `url` value required by `contracts/subscriptions-api.md`.
- [X] T010 [US1] Implement `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs` as a process-lifetime, concurrency-safe in-memory ordered collection; reject only empty or whitespace-only values and append duplicates unchanged.
- [X] T011 [US1] Implement `backend/RSSFeedReader.Api/Endpoints/SubscriptionEndpoints.cs` and map it from `backend/RSSFeedReader.Api/Program.cs` to provide `GET /api/subscriptions` returning the ordered JSON list and `POST /api/subscriptions` returning the added value or a clear `400 Bad Request`; do not make requests to submitted URLs.
- [X] T012 [US1] Add the configured API base URL to `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`, create `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs` to call only the subscription list and add endpoints through dependency-injected `HttpClient`, and register the client in `frontend/RSSFeedReader.UI/Program.cs`.
- [X] T013 [US1] Implement the add form, subscription list, blank-input feedback, and text-only rendering in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor`; make it the sole `/` route by removing `frontend/RSSFeedReader.UI/Pages/Home.razor`, `frontend/RSSFeedReader.UI/Pages/Counter.razor`, and `frontend/RSSFeedReader.UI/Pages/Weather.razor`, and remove obsolete links from `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`.

**Checkpoint**: User Story 1 is independently testable against the acceptance scenarios and [contracts/subscriptions-api.md](./contracts/subscriptions-api.md).

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify the complete MVP, configuration, and security/scope boundaries.

- [X] T014 Run `dotnet test backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` and `dotnet test backend/RSSFeedReader.Api.IntegrationTests/RSSFeedReader.Api.IntegrationTests.csproj`; fix failures in the corresponding test or implementation files.
- [X] T015 Build `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj` and `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`, then perform every browser scenario in `specs/001-add-subscriptions/quickstart.md` and record/fix issues in the affected project files.
- [X] T016 Verify exactly one page maps to `/`, user-provided values render as text, the browser makes no request to any submitted subscription URL, and no fetching, parsing, persistence, removal, or refresh behavior was introduced; check `frontend/RSSFeedReader.UI/Pages/`, `backend/RSSFeedReader.Api/`, and `frontend/RSSFeedReader.UI/`.

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: Requires the .NET 10 SDK; tasks run in order because the solution and project references are created incrementally.
- **Foundational (Phase 2)**: Depends on all projects in Setup; the local API origin and CORS configuration are prerequisites for browser/API integration.
- **User Story 1 (Phase 3)**: Depends on Phase 2. The API tests may be authored before implementation; integration tests and implementation require the API host project.
- **Polish (Phase 4)**: Depends on the completed User Story 1 implementation.

### User Story Dependencies

- **User Story 1 (P1)**: Depends on Setup and Foundational phases; no other user stories exist.

### Within User Story 1

- T007 and T008 can be authored in parallel and should fail until their behavior is implemented.
- T009 can be authored independently of T007-T008. T010 depends on the request/value rules in T009; T011 depends on T009 and T010.
- T012 depends on the API paths and JSON shapes in T011 and `contracts/subscriptions-api.md`.
- T013 depends on T012 and the API base URL/CORS configuration from T006.
- T014-T016 depend on all implementation tasks.

### Parallel Opportunities

- **Setup**: None of T001-T005 are marked parallel because project creation and solution/project references modify shared solution state.
- **Foundational**: T006 is a single coordinated API host/CORS configuration task.
- **User Story 1**: T007 and T008 may run in parallel (different test projects/files); T009 may also proceed independently of those tests (different files). Other story tasks are ordered by their file and runtime dependencies.
- **Polish**: Automated test runs and project builds can run concurrently after implementation, but the documented task sequence keeps failures easier to diagnose.

## Parallel Example: User Story 1

After Phase 2, author the two test suites and the API types in parallel:

```text
Task: T007 - Add store unit tests in backend/RSSFeedReader.Api.Tests/SubscriptionStoreTests.cs
Task: T008 - Add HTTP integration tests in backend/RSSFeedReader.Api.IntegrationTests/SubscriptionsApiTests.cs
Task: T009 - Add request/response types in backend/RSSFeedReader.Api/Models/
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Install .NET 10 and complete Setup and Foundational phases.
2. Add failing store and HTTP tests, then implement the in-memory API contract.
3. Add the configured Blazor API client and subscription page.
4. **Stop and validate** using T014-T016 and [quickstart.md](./quickstart.md).
5. Do not add feed fetching, feed items, persistence, removal, or background refresh.

### Incremental Delivery

The feature has one user story; complete and validate that story as a single MVP increment. Extended-MVP capabilities require a separately approved feature and are not additional task phases here.

## Notes

- Every executable task line uses `- [ ] T###`, includes `[US1]` for User Story 1 tasks only, marks `[P]` only where tasks can safely run in parallel, and names the relevant repository file path(s).
- The current environment lacks the .NET 10 SDK required by `global.json`; implementation/build tasks cannot be run until the SDK is installed.
- No GitHub extension hooks are registered for task generation.
