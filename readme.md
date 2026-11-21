# SalonBooking

SalonBooking is an ASP.NET Core Web API for booking appointments at a beauty salon.

## Getting started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) with Docker Compose

### Run

1. Start the PostgreSQL database:

   ```bash
   docker compose up -d
   ```

2. Run the API:

   ```bash
   dotnet run --project src/SalonBooking.Api
   ```

### Test

```bash
dotnet test
```

> Integration tests use Testcontainers, so Docker must be running.