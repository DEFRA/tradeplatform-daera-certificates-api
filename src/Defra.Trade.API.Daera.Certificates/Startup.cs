// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Diagnostics.CodeAnalysis;
using Defra.Trade.API.Daera.Certificates.Infrastructure;
using Defra.Trade.API.Daera.Certificates.Logic.Extensions;
using Defra.Trade.Common.Api.Infrastructure;
using Defra.Trade.Common.ExternalApi.ApimIdentity;
using Defra.Trade.Common.ExternalApi.Auditing;
using Defra.Trade.Common.Security.Authentication.Infrastructure;
using Defra.Trade.Common.Sql.Infrastructure;

namespace Defra.Trade.API.Daera.Certificates;

/// <summary>
/// Startup class.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Startup"/> class.
/// </remarks>
/// <param name="configuration">Application Config.</param>
[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    /// <summary>
    /// Config services registrations.
    /// </summary>
    /// <param name="services">Application Service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTradeApi(Configuration);
        services.AddTradeExternalApimIdentity(Configuration);
        services.AddTradeExternalAuditing(Configuration);
        services.AddApimAuthentication(Configuration.GetSection(InternalApimSettings.SectionName));
        services.AddTradeSql(Configuration);
        services.AddServiceRegistrations(Configuration);
    }

    /// <summary>
    /// Method to configure application startup.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="env">Web environment</param>
    /// <param name="logger">Application logger</param>
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        logger.LogStartup(
            env.EnvironmentName,
            env.ApplicationName,
            env.ContentRootPath);

        app.UseTradeExternalAuditing();
        app.UseTradeApp(env);
    }
}