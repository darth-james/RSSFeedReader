<!--
Sync Impact Report
Version change: unversioned -> 1.0.0 (initial constitution)
Modified principles: unresolved template slots -> I. MVP Scope and Incremental Delivery;
	II. Security and Safe Data Handling; III. Maintainable Architecture;
	IV. Verified Quality; V. Focused User Experience
Added sections: Project Constraints; Development Workflow
Removed sections: none
Follow-up TODOs: Record the original ratification date when known.
-->
# RSS Feed Reader Constitution

## Core Principles

### I. MVP Scope and Incremental Delivery
The MVP MUST support adding a feed subscription by URL and displaying the subscription list.
It MUST NOT fetch or parse feeds, persist data, remove subscriptions, or add background polling
unless those capabilities are explicitly approved for a later phase. Each change MUST identify
the phase it serves and meet that phase's acceptance criteria. This keeps the proof of concept
small and prevents unapproved scope from obscuring its purpose.

### II. Security and Safe Data Handling
All externally supplied values, including subscription URLs and future feed content, MUST be
treated as untrusted when stored, logged, returned by the API, or rendered. The MVP MUST NOT
make network requests to subscription URLs. Any approved feed-fetching work MUST restrict
requests to supported HTTP(S) schemes, prevent access to loopback, private, and link-local
destinations, enforce timeouts and response-size limits, and avoid forwarding credentials.
Feed-derived HTML MUST NOT be rendered unless it is sanitized with a maintained allowlist-based
sanitizer. These controls limit injection and server-side request forgery risks without adding
network complexity to the MVP.

### III. Maintainable Architecture
The application MUST use the ASP.NET Core Web API for subscription operations and Blazor
WebAssembly for the user interface. API communication, application behavior, and presentation
MUST remain separated so each can be changed and tested independently. Configuration such as
the API base URL MUST be externalized rather than hardcoded in application logic. New
abstractions MUST have a concrete testability or change-isolation benefit; otherwise prefer the
simplest implementation that meets the current phase.

### IV. Verified Quality
Every change MUST build successfully for each affected project and include tests for new or
changed behavior. Subscription changes MUST verify that adding a URL updates the displayed list;
API changes MUST verify request and response behavior at the API boundary. Changes involving
network access, parsing, persistence, or rendering untrusted content MUST include focused tests
for their relevant failure and security cases. A feature is not complete until its documented
acceptance criteria pass.

### V. Focused User Experience
The MVP interface MUST provide a direct way to enter a feed URL, add it, and see the resulting
subscription list. It MUST communicate the current operation's result without implying that
feeds have been fetched or validated. UI complexity MUST be proportional to approved MVP
behavior; item views, refresh controls, organization, and other deferred features MUST NOT be
introduced before their phase is approved.

## Project Constraints
The application is a single-user, locally run proof of concept targeting Windows, macOS, and
Linux. Its approved stack is an ASP.NET Core Web API backend and a Blazor WebAssembly frontend.
The MVP stores subscriptions in memory and accepts URLs without feed validation, but MUST still
render user-provided values safely. Feed fetching and parsing belong to the Extended-MVP; manual
refresh is the only approved refresh behavior for that phase. Database persistence, automatic
polling, and rich HTML rendering remain deferred until separately approved.

## Development Workflow
Work MUST proceed phase by phase: establish the subscription API and UI behavior for the MVP
before implementing Extended-MVP feed operations. Before validating the local application,
developers MUST confirm that the frontend API base URL, backend listening port, and backend CORS
origins agree. Changes to routing MUST leave exactly one page mapped to the root route. Reviews
MUST check phase scope, security implications, maintainability, tests, and successful builds for
affected projects. Any exception to a principle MUST be documented with its rationale and
approved before implementation.

## Governance
This constitution governs implementation and review decisions for the RSS Feed Reader. Amend it
by proposing the exact text change, documenting its rationale and affected principles, and
reviewing compatibility with existing and planned phases before approval. Record every approved
amendment in this file.

Use semantic versioning for constitution changes: increment MAJOR for incompatible principle or
governance changes, MINOR for added principles or materially expanded requirements, and PATCH
for clarifications or non-semantic wording changes. Every amendment MUST update the last-amended
date and version. Compliance MUST be reviewed for each change before merge; any deviation MUST
have an explicit, approved rationale recorded with the change.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): original adoption date not recorded | **Last Amended**: 2026-10-15
