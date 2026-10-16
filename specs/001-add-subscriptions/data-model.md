# Data Model: Add RSS/Atom Subscriptions

## Subscription

Represents one non-empty value submitted by the local user during the API process lifetime.

| Attribute | Meaning | Rule |
|---|---|---|
| URL | The exact user-entered value shown in the list | Must not be empty or whitespace-only; no URL syntax, scheme, reachability, or feed-format validation |
| Position | The value's insertion order in the current list | New entries appear after prior entries |

### Identity and duplicates

Subscriptions have no independent identifier in this MVP. Each successful submission creates a new list entry, including an exact repeated value. Position distinguishes entries within the current list; the URL value alone is not unique.

### Lifecycle

1. The user submits a value.
2. Empty or whitespace-only values are rejected and are not added.
3. Any other value is appended and available from the API list operation.
4. Entries exist only while the API process remains running and are lost when it stops.

## Explicitly excluded data

The MVP stores no feed title, feed content, fetch status, item, read state, persisted identifier, or user/account data.
