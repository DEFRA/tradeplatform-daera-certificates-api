# Setup

To run this webapp, you will need a `src/Defra.Trade.API.Daera.Certificates/appsettings.Development.json` file. The file will need the following settings:

```jsonc 
{
  "ConnectionStrings": {
    "sql_db": "<secret>",
    "sql_db_ef": "<secret>"
  },
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "DaeraCertificatesSettings": {
    "BaseUrl": "<secret>",
    "DaeraCertificatesApiPathV1": "<secret>"
  },
  "ConfigurationServer": {
    "ConnectionString": "<secret>",
    "TenantId": "<secret>"
  },
  "SocSettings": {
    "EventHubName": "<secret>",
    "EventHubConnectionString": "<secret>"
  },
  "ProtectiveMonitoringSettings": {
    "Enabled": true,
    "Environment": "DEV"
  }
}
```

Secrets reference can be found here: https://dev.azure.com/defragovuk/DEFRA-TRADE-APIS/_wiki/wikis/DEFRA-TRADE-APIS.wiki/26086

## Database seeding

For a local working dataset:

1. In SSMS, export a `.bacpac` of deployed db `trade-daera-certificates`
2. In SSMS, run an 'import data-tier application' of the above `.bacpac` to your local server
