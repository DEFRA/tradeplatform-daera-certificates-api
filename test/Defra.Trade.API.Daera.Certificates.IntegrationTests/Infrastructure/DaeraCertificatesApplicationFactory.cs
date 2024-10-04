// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.CertificatesStore.V1.ApiClient.Api;
using Defra.Trade.API.Daera.Certificates.Database.Context;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;
using Defra.Trade.API.Daera.Certificates.IntegrationTests.Helpers;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Defra.Trade.Common.ExternalApi.Auditing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.Infrastructure;

public class DaeraCertificatesApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public Mock<IDateTimeProvider> DateTimeProvider { get; set; }
    public DaeraCertificateDbContext DaeraCertificateDbContext { get; set; }
    public Mock<IAuditRepository> AuditRepository { get; set; }
    public Mock<ICertificatesStoreRepository> CertificatesStoreRepository { get; set; }
    public Mock<IDocumentRetrievalApi> DocumentRetrievalApi { get; set; }

    public DaeraCertificatesApplicationFactory() : base()
    {
        ClientOptions.AllowAutoRedirect = false;
        DateTimeProvider = new Mock<IDateTimeProvider>();
        DaeraCertificateDbContext = GetDatabaseContext();
        AuditRepository = new Mock<IAuditRepository>();
        CertificatesStoreRepository = new Mock<ICertificatesStoreRepository>();
        DocumentRetrievalApi = new Mock<IDocumentRetrievalApi>();
    }

    private string ApiVersion { get; set; } = "1";

    private static DaeraCertificateDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<DaeraCertificateDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new DaeraCertificateDbContext(options);

        databaseContext.Database.EnsureCreated();

        return databaseContext;
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        client.BaseAddress = new Uri("https://localhost:5001");
        client.DefaultRequestHeaders.Add("x-api-version", ApiVersion);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void AddApimUserContextHeaders(HttpClient client, Guid? clientId, string clientIpAddress)
    {
        if (clientId.HasValue)
            client.DefaultRequestHeaders.Add("x-client-id", clientId.Value.ToString());

        if (clientIpAddress != null)
            client.DefaultRequestHeaders.Add("x-client-ipaddress", clientIpAddress);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment(Environments.Production);

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["https_port"] = "",
                    ["ConnectionStrings:sql_db"] = "Data Source=.;Initial Catalog=trade-daera-certificates;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True;",
                    ["OpenApi:UseXmlComments"] = "false",
                    ["CommonError:ExposeErrorDetail"] = "true",
                    ["Apim:External:BaseUrl"] = "https://integrationtest-gateway.trade.azure.defra.cloud",
                    ["DaeraCertificatesSettings:BaseUrl"] = "https://integrationtest-gateway.trade.azure.defra.cloud",
                    ["DaeraCertificatesSettings:DaeraCertificatesApiPathV1"] = "/daera-certificates/v1",
                    ["SocSettings:EventHubName"] = "insights-application-logs",
                    ["SocSettings:EventHubConnectionString"] = "Endpoint=sb://not.a.real.service.bus/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=not-a-real-key",
                    ["ProtectiveMonitoringSettings:Enabled"] = bool.FalseString,
                    ["ProtectiveMonitoringSettings:Environment"] = "DEV",
                    ["GcDocumentBlobStorageOptions:TenantId"] = "mocked",
                    ["GcDocumentBlobStorageOptions:ClientId"] = "mocked",
                    ["GcDocumentBlobStorageOptions:ClientSecret"] = "mocked",
                    ["GcDocumentBlobStorageOptions:StorageAccountName"] = "mocked",
                    ["GcDocumentBlobStorageOptions:ConnectionString"] = "mocked",
                });
        });

        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton(DateTimeProvider.Object));
            services.Replace(ServiceDescriptor.Singleton(DaeraCertificateDbContext));
            services.Replace(ServiceDescriptor.Singleton(AuditRepository.Object));
            services.Replace(ServiceDescriptor.Singleton(CertificatesStoreRepository.Object));
            services.AddTransient<IAzureBlobService<GcDocumentBlobStorageOptions>, FakeBlobService<GcDocumentBlobStorageOptions>>();
            services.Replace(ServiceDescriptor.Singleton(DocumentRetrievalApi.Object));
        });
    }
}
