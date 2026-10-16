# Quickstart: Add RSS/Atom Subscriptions

This guide validates the MVP add-and-list flow from the browser. It assumes the initial solution has been scaffolded at the project paths in [plan.md](./plan.md).

## Prerequisites

- Install the .NET 10 SDK, the planned target. The currently inspected environment has .NET SDK 8 and 9 only.
- Have a modern browser available.
- Confirm the API and frontend local ports, the frontend API base URL, and the API CORS origin agree. Keep the API base URL in frontend configuration rather than hardcoding it in page logic.

## Run locally

From the repository root, in separate terminals:

```powershell
dotnet run --project backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj
```

```powershell
dotnet run --project frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj
```

Open the frontend URL reported by the second command.

## Validate in the browser

1. Confirm the subscription page loads and starts with an empty list.
2. Enter `https://example.com/feed.xml` and submit it. Confirm it appears in the list without a page reload.
3. Submit a different non-empty value and confirm both entries remain visible in insertion order.
4. Submit `https://example.com/feed.xml` again and confirm a second entry appears.
5. Submit an empty value and a whitespace-only value separately; confirm neither is added and the UI explains that a value is required.
6. Enter a non-empty arbitrary value that is not a working feed URL; confirm it is displayed and no request is sent to that value.
7. Restart both applications and confirm the list is empty.
8. Confirm the browser console reports no API connection/CORS errors and user-provided values are rendered as text.

## Automated checks

Run focused backend tests and integration tests:

```powershell
dotnet test backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj
dotnet test backend/RSSFeedReader.Api.IntegrationTests/RSSFeedReader.Api.IntegrationTests.csproj
```

Build the affected applications:

```powershell
dotnet build backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj
dotnet build frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj
```

Expected coverage includes empty-list retrieval, adding and listing values, repeated values, whitespace rejection without list mutation, and an HTTP POST-then-GET round trip. None of the tests should contact a subscription URL.
