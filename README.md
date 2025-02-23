# GIC Banking System

A banking system solution built with .NET Core implementing Clean Architecture principles, CQRS, and Mediator pattern.

## Architecture Overview

The solution follows Clean Architecture with the following structure:

- **GICBankingSystem.Core.API**: REST API layer handling HTTP requests
- **GICBankingSystem.Core.Application**: Application business logic and use cases
- **GICBankingSystem.Core.Domain**: Domain entities and business rules
- **GICBankingSystem.Core.Infrastructure**: Data access and external services
- **GICBankingSystem.Console**: Console application consuming the API
- **GICBankingSystem.Shared**: Shared components and utilities


![image](https://github.com/user-attachments/assets/6f029792-8327-40ad-b9ef-2bb30f6ae16d)


## Key Features

- Clean Architecture implementation
- CQRS (Command Query Responsibility Segregation) pattern
- Mediator pattern for handling commands and queries
- Dependency Injection for loose coupling
- MS SQL Server database
- Comprehensive unit testing with NUnit and Moq
- Unit of Work pattern for transaction management
- FluentValidation for robust input validation

## Technical Stack

- **.NET Core**
- **MS SQL Server**
- **NUnit** & **Moq** for testing
- **FluentValidation** for request validation
- **Unit of Work** pattern for data consistency
- **Docker** support
- **GitHub Actions** for CI/CD
- **SonarCloud** for code quality analysis

## Project Structure

```
├── ConsoleApp
├── Libraries
│   └── GICBankingSystem.Shared
├── Services
│   └── Core
│       ├── GICBankingSystem.Core.API
│       ├── GICBankingSystem.Core.Application
│       ├── GICBankingSystem.Core.Domain
│       └── GICBankingSystem.Core.Infrastructure
└── Test
    ├── GICBankingSystem.Core.API.Tests
    └── GICBankingSystem.Core.Application.Tests
```

## Getting Started

### Prerequisites
- .NET Core SDK
- MS SQL Server
- Visual Studio 2022 (recommended)

### Setup Instructions

1. Clone the repository

2. Configure Connection Strings:
   Update the connection string in below location:
   - `GICBankingSystem.Core.API/appsettings.json`
   

   Example connection string:
   ```
   Server=localhost;Database=GICBankingSystem;User Id=sa;Password=YourPassword;TrustServerCertificate=True
   ```

3. Set Up Database:
   - Open Package Manager Console in Visual Studio
   - Set Default Project to `GICBankingSystem.Core.Infrastructure`
   - Run the command:
     ```
     update-database
     ```
   This will create all necessary tables in your database.

4. Configure Startup Projects:
   - Right-click on the solution in Solution Explorer
   - Select "Set Startup Projects"
   - Choose "Multiple startup projects"
   - Set both `GICBankingSystem.Core.API` and `GICBankingSystem.Console` to "Start"

5. Run the application
   - Press F5 or click Start to run both projects simultaneously
  

     ![image](https://github.com/user-attachments/assets/7eb40950-565c-4f5e-acfe-b55eab63172a)

     ![image](https://github.com/user-attachments/assets/2fa45c1c-1c27-4af3-a44e-10f9d8b400c2)

     ![image](https://github.com/user-attachments/assets/1663aef7-0d72-4027-bcf5-acb3b1842e06)

**Interesting calculation have some minor issue in logic.
## Testing

The project includes comprehensive unit tests using NUnit framework with Moq for mocking dependencies.

## Code Quality

The project is integrated with SonarCloud for continuous code quality monitoring. You can view the detailed analysis at:
[https://sonarcloud.io/summary/overall?id=thilandakshina_GICBankingSystem](https://sonarcloud.io/summary/overall?id=thilandakshina_GICBankingConsole)

## CI/CD

GitHub Actions workflows are configured to:
- Build and test the application
- Run SonarCloud analysis
- Automated deployments (when configured)
