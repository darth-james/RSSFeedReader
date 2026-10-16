# Subscription API Contract

The API supports the MVP subscription list only. It does not fetch, parse, validate, or verify subscription URLs.

## `GET /api/subscriptions`

Returns the current list, including duplicate values, in insertion order.

- **Success**: `200 OK`
- **Content type**: `application/json`
- **Response body**:

```json
[
  { "url": "https://example.com/feed.xml" },
  { "url": "https://example.com/feed.xml" }
]
```

An empty list is `[]`.

## `POST /api/subscriptions`

Adds one subscription value as a new entry. Repeated values are not deduplicated.

- **Request content type**: `application/json`
- **Request body**:

```json
{ "url": "https://example.com/feed.xml" }
```

- **Success**: `200 OK`
- **Success response content type**: `application/json`
- **Success response body**:

```json
{ "url": "https://example.com/feed.xml" }
```

- **Invalid input**: `400 Bad Request` when the body is missing a value or its value is empty or whitespace-only. The response must give the caller a clear indication that a value is required. The list remains unchanged.

Other non-empty values are accepted unchanged. The API must not send requests to the submitted URL. Subscription values are data and must not be interpreted as markup by the UI.

## State and errors

- State is held in memory for the API process lifetime; after the process restarts, `GET` returns an empty list.
- Configure the API to allow requests only from the intended local frontend origin for development.
- No remove, refresh, feed-item, authentication, or persistence endpoint is part of this contract.
