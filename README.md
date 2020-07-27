# TheIntegrator

This Integrator API provides 2 endpoints which will save and retrieve sales.

Requirements for this project : <br />
https://dotnet.microsoft.com/download/dotnet-core/3.1 <br />
https://docs.docker.com/desktop/

Docker image for ASP.NET Core 2.1/3.1 Runtime :
```
docker pull mcr.microsoft.com/dotnet/core/aspnet
```

Post endpoint :
```
/sales/v1/api/record
```

Get endpoint :
```
/sales/v1/api/report
```

How to test API :
```
https://localhost:32768/swagger/index.html
```

I used Swagger for API testing.