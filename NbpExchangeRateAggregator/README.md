# NBP Exchange Rate Aggregator (ASP.NET Core Web API)

**Status:** Planned

A RESTful API that integrates with the public National Bank of Poland (NBP) API to fetch, transform, and aggregate currency exchange data. The generated reports are initially saved to the local file system.

**Key Learning Objectives:**
- ASP.NET Core Web API fundamentals (Controllers, Routing, Dependency Injection)
- External API communication using `IHttpClientFactory`
- JSON Serialization/Deserialization (`System.Text.Json`)
- Data processing and transformation using LINQ
- File I/O operations (writing JSON/CSV files)

**Technical Specification:**
- **Endpoint:** `GET /api/currency/report?currencyCode={code}&days={days}`
  - Example: `/api/currency/report?currencyCode=EUR&days=30`
- **Application Flow:**
  1. API receives the request with parameters (currency code, number of days).
  2. Application makes an HTTP GET request to the NBP API.
  3. JSON response is deserialized into C# models.
  4. LINQ is used to calculate:
     - Average exchange rate for the given period.
     - Minimum and maximum exchange rates along with their specific dates.
  5. The aggregated report is serialized and saved to a local `Reports/` directory.
  6. The API returns the generated report payload to the client.
- **Next Phase:** Migrate from file-based storage to SQL Server using Entity Framework Core.
