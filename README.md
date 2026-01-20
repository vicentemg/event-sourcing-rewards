# Event Sourcing Proof of Concept

## Overview
This project is a Proof of Concept (PoC) designed to demonstrate the implementation of the **Event Sourcing** pattern in a modern .NET environment. It illustrates how to manage application state through an immutable sequence of events rather than just storing the current state.

### Template Usage
This repository is configured as a **GitHub Template**, allowing you to quickly bootstrap new projects with this architecture. Simply click the **"Use this template"** button to generate a new repository pre-populated with this project's structure and code.

## Architecture and Design
The project follows **Clean Architecture** principles and **Domain-Driven Design (DDD)**, structuring the solution in concentric layers to ensure separation of concerns and maintainability.

### Layer Structure
The source code is located in the `src/` directory and is divided into the following projects:

- **`EventSourcing.Domain`**: 
  - The core of the application. Contains pure business logic, entities, and **Aggregates** (such as `Account` and `Party`).
  - Defines **Domain Events** that represent state changes.
  - Has no infrastructure dependencies.

- **`EventSourcing.Application`**:
  - Contains application logic and use cases (Features).
  - Implements the **CQRS** (Command Query Responsibility Segregation) pattern to separate write operations (Commands) from read operations (Queries).

- **`EventSourcing.Infrastructure`**:
  - Implements technical details and adapters.
  - Configures **Marten** as the primary library for Event Sourcing and document persistence. Marten uses PostgreSQL as the underlying database engine.

- **`EventSourcing.WebApi`**:
  - The presentation layer that exposes functionality via a REST API.
  - Configures endpoints and dependency injection.

### Key Technologies
- **.NET 9.0**: Development platform.
- **Marten**: Library that enables using PostgreSQL as an Event Store and document database.
- **PostgreSQL**: Relational database used for persistence infrastructure.

## Main Aggregates
The current domain models the following concepts:
- **Party**: Represents a customer or entity that can own accounts.
- **Account**: Represents an account (e.g., bank account) on which transactions are recorded.

## Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (to run PostgreSQL if a local instance is not available).

## How to Run

1. **Database Configuration**:
   Ensure you have a PostgreSQL instance running and accessible. Connection settings are typically found in `appsettings.json` or `appsettings.Development.json` in the `EventSourcing.WebApi` project.

2. **Run the API**:
   Navigate to the root directory and run:
   ```bash
   dotnet run --project src/EventSourcing.WebApi
   ```

## Development Environment (DevContainer)
This project is configured with a **DevContainer**, which allows for a quick and consistent setup of the development environment.

- **Fast Interaction**: By opening the folder in VS Code with the "Dev Containers" extension, all dependencies (including .NET SDK and PostgreSQL tools) will be automatically installed.
- **Reproducibility**: Ensures that all developers work in the exact same environment, avoiding "it works on my machine" issues.

## Usage Examples
The project includes a `requests-collection.http` file in the root. You can use this file with the VS Code "REST Client" extension (or similar) to send test requests to the API.
