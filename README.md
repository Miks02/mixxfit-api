# MixxFit API

MixxFit API is a backend Web API written in C# targeting .NET 10.  
The project is organized as a single project inside a single solution and follows a horizontal folder structure (Controllers, Services, Validators, Models, DTOs, etc.).

## Summary
Minimal, service-oriented Web API that keeps controllers thin and places business logic in services. Validation is handled by a dedicated validators layer and DTOs are used for external communication.

## Folder structure (horizontal)
- Controllers — HTTP endpoints, routing and request/response mapping.
- Services — business logic and orchestration.
- Validators — input validation for DTOs using FluentValidation.
- Models — domain entities and internal types.
- DTOs — request/response objects exposed to clients.
- Configurations — DI and application setup helpers.
- Extensions - for extensions methods like ResultExtensions that can be seen in the repo, etc...
- Exceptions - for exception handlers
- Mappers - Custom model/dto mappers (i don't like mapping libraries so i make mappers manually)
- Data - Persistence layer, db context and configuration classes are here
- Filters - API Filters like ProblemDetailsFilter that transforms the request when an error happens and returns a ProblemDetails to the client

Keep controllers thin: map and validate input, call services, return appropriate HTTP responses.

## Result pattern / error handling
This project uses a result pattern for handling business logic as opposed to throwing exceptions everywhere,
exceptions are reserved only for "exceptional stuff" like database errors, network errors and others.
Business errors are handled using the Result and Result<T> classes along with the ErrorCatalogue class.
It is also worth mentioning that i implemented extension methods like: HandleResult, ToActionResult etc which can be seen in ResultExtensions.cs file . These extension methods drastically reduce code duplication especially in controllers where without these methods i would have to manually check if a result method returned a success or failure before proceeding.

## Authentication / Authorization
This project uses custom authentication based on JWT access tokens plus refresh tokens.
Currently auth is simplified, which means that each user has a refresh token and refresh token expiration date as a property. Which further means that whenever user's login from a different device, they would be logged out of their previous device since refresh tokens regenerate on every login. 
Future plan is to split refresh tokens as a separate entity from a user which would allow multi-device authentication at the same time

## Logging and Api responses
- This project uses ILogger<T> interface for structured logging inside every service class.
- Error return type is standardized across the API using the ProblemDetails filter class that provides a standardized error message. It works perfectly with the result pattern's error catalogue in a sense that a ProblemDetails filter has a method for mapping those errors to a corresponding status code.
- Error example: Service returns Error.Resource.NotFound, problem details filter maps the error and returns 404 Not Found to the user.

## Validation and DTOs
- Validation is done using FluentValidator classes that validate client requests. More comples business type validations are done in services.
- Services always return some sort of Data Transfer Object and never whole entities.
