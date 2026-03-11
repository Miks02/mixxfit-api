# MixxFit API

MixxFit API is a backend Web API written in C# targeting .NET 10.  
The project is organized as a single project inside a single solution and follows **Vertical Slice Architecture (VSA)** — each feature is fully self-contained with its own endpoint, handler, request/response models, validators and DTOs.

## Summary

Minimal Web API built around vertical slices. Each feature folder contains everything needed to fulfill a specific use case: the HTTP endpoint, the business logic handler, request/response types and validators. Cross-cutting concerns (auth, persistence, file storage, error handling) live in dedicated infrastructure layers shared across slices.

## Folder Structure (Vertical Slice)

```
├── Common/                        # Shared abstractions used across slices
│   ├── Extensions/                # ResultExtensions, UtilityExtensions
│   ├── Interfaces/                # ICookieProvider, ICurrentUserProvider, IEndpoint, IFileService, IHandler, ITokenService
│   └── Results/                   # Result<T>, Error, PagedResult
│
├── Domain/                        # Pure domain layer — no dependencies on infrastructure
│   ├── Entities/                  # CalorieEntry, ExerciseEntry, FitnessProfile, SetEntry, User, WeightEntry, Workout
│   ├── Enums/                     # AccountStatus, ActivityLevel, CardioType, ExerciseType, Gender
│   └── ErrorCatalog/              # AuthError, FileError, GeneralError, UserError
│
├── Features/                      # One sub-folder per use case (the "slices")
│   ├── Auth/
│   │   ├── ChangePassword/
│   │   ├── Login/
│   │   ├── Logout/
│   │   ├── Register/
│   │   └── RotateTokens/
│   ├── Dashboard/
│   │   └── GetDashboard/
│   ├── Nutrition/
│   │   ├── CalculateCalories/
│   │   └── SetDailyCalories/
│   ├── Profile/
│   │   └── UpdateFitnessProfile/
│   ├── Users/
│   │   ├── DeleteUser/
│   │   ├── GetMe/
│   │   └── UpdateUserFields/
│   ├── WeightEntries/
│   │   ├── DeleteWeight/
│   │   ├── GetWeightById/
│   │   ├── GetWeightChart/
│   │   ├── GetWeightLogs/
│   │   ├── GetWeightSummary/
│   │   ├── LogWeight/
│   │   └── Shared/
│   └── Workouts/
│       ├── CreateWorkout/
│       ├── DeleteWorkout/
│       ├── GetPagedWorkouts/
│       ├── GetWorkoutById/
│       ├── GetWorkoutChartData/
│       └── GetWorkoutsOverview/
│
├── Infrastructure/                # Cross-cutting concerns
|   ├── Configuration/             # CloudinaryOptions and other settings
│   ├── Exceptions/                # GlobalExceptionHandler
│   ├── Extensions/                # DependencyInjection, EndpointExtensions
│   ├── Filters/                   # ProblemDetailsFilter, ValidationFilter
│   ├── Persistence/               # AppDbContext, entity configurations, EF migrations
│   ├── Security/                  # CookieProvider, CurrentUserProvider, TokenService
│   └── Storage/                   # CloudinaryFileStorage, LocalFileStorage
│
├── Program.cs
└── Properties/
    └── launchSettings.json
```

Each slice (e.g. `Features/Auth/Login/`) typically contains:
- `*Endpoint.cs` — maps the HTTP route via `IEndpoint`, handles request/response binding.
- `*Handler.cs` — contains all business logic for that use case, implements `IHandler`.
- `*Request.cs` — incoming request model.
- `*Response.cs` — outgoing response DTO.
- `*Validator.cs` — FluentValidation validator for the request.

## Result Pattern / Error Handling

This project uses a result pattern instead of exception-driven control flow.  
Exceptions are reserved for truly exceptional scenarios (database errors, network failures, etc.).  
Business errors are expressed through the `Result` / `Result<T>` types and defined in the `ErrorCatalog` (`AuthError`, `UserError`, `FileError`, `GeneralError`).

Extension methods in `ResultExtensions.cs` (`HandleResult`, etc.) eliminate boilerplate at the endpoint level — no manual success/failure checks needed per endpoint.

## Authentication / Authorization

Custom JWT-based authentication with refresh token rotation:
- On login, the user receives a short-lived **access token** and a **refresh token** stored in an HTTP-only cookie.
- Tokens are rotated via the `RotateTokens` endpoint.
- Currently, each user holds a single refresh token, meaning a new login invalidates the previous session (no multi-device support yet).
- **Future plan:** extract refresh tokens into a separate entity to support concurrent sessions across multiple devices.

## Logging and API Responses

- **Logging:** `ILogger<T>` is used for structured logging inside of filters and some handlers.
- **Standardized errors:** `ProblemDetailsFilter` intercepts all error results and maps them to a `ProblemDetails` response with the appropriate HTTP status code.
  - Example: a handler returns `AuthError.InvalidCredentials` → filter maps it to `401 Unauthorized`.
- **Validation errors:** `ValidationFilter` runs FluentValidation before the handler is invoked and returns a `400 Bad Request` with field-level details on failure.

## Validation

- Request validation is handled by FluentValidation validators (`*Validator.cs`) co-located with each slice.
- More complex, business-rule-level validations are performed inside the handler itself.
- Handlers always return DTOs — domain entities are never exposed directly to clients.
