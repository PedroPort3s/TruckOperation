# Truck Operations

## Project Overview

Truck Operations is a comprehensive web application built with Blazor WebAssembly for the front end and ASP.NET Core for the API. It supports CRUD operations for managing trucks using Entity Framework with migrations. The project includes a test suite using xUnit and Entity Framework In-Memory to ensure expected functionality.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or later)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or another SQL Server version for Entity Framework)


## Project Structure

- `BlazorClient`: Blazor WebAssembly front end.
- `WebApi`: ASP.NET Core API with Entity Framework.
- `UnitTests`: xUnit test project with Entity Framework In-Memory.
- `Models`: Shared models project.

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/PedroPort3s/TruckOperation.git
cd TruckOperation
```


### **Set Up the Database**


### 2. Set Up the Database

1. Open the solution in Visual Studio.
2. Open the Package Manager Console (PMC) in Visual Studio.
3. Set the default project to `WebApi`.
4. Run the following command to apply the migrations and set up the database:

```bash
Update-Database
```


### **Running the Application**


### 3. Running the Application

To run both the front end and the back end in parallel in Visual Studio, follow these steps:

1. **Set Multiple Startup Projects**:
    - Right-click on the solution in Solution Explorer and select `Properties`.
    - Under `Startup Project`, select `Multiple startup projects`.
    - Set `WebApi` and `BlazorClient` to `Start`.

2. **Run the Application**:
    - Press `F5` or click the `Start` button in Visual Studio. This will launch both the API and the Blazor WebAssembly app.

### 4. Running the Tests

1. Open Test Explorer in Visual Studio.
2. Build the solution to discover all tests.
3. Run all tests to ensure everything is working as expected.

## Project Details

### Front End - Blazor WebAssembly

- Implements the UI for CRUD operations.
- Interacts with the API to fetch, create, update, and delete truck records.

### Back End - ASP.NET Core API

- Provides endpoints for CRUD operations.
- Uses Entity Framework for database interactions with migrations to manage schema changes.

### Testing - xUnit & Entity Framework In-Memory

- Includes unit tests for the API controllers.
- Utilizes Entity Framework In-Memory for testing purposes to simulate database operations.

### Shared Models

- Contains shared model classes used across different projects.